using ApiPermissionControl.AspnetCore;
using AspNetCoreRateLimit;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using DDDApi;
using DDDApi.Interceptions;
using DDDApi.Middlewares;
using DDDApi.Models;
using DDDApplication.Contract.Ueditor;
using DDDDomain.Shared.Notify;
using DDDUtility;
using DDDUtility.Autofac;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog;
using Serilog.Events;
using System.Reflection;
using Utility.CaptchaValidation;
using Utility.DistributedCache;
using Utility.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

var domainAssembly = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "DDDDomain.dll"));
var serviceAssembly = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "DDDApplication.dll"));

// Add services to the container.
AppSettings.FileUrl = builder.Configuration.GetValue<string>(AppSettingsItem.FileUrl)!;
UEditorConfig.Register(Path.Combine(Environment.CurrentDirectory, "appsettings.json"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddJwtTokenCenter(builder.Configuration.GetSection("AuthSettings"));
builder.Services.AddDbContextCenter(builder.Configuration);
builder.Services.AddDDDServices();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddDistributedCacheService();
builder.Services.AddNotifyService(builder.Configuration);
builder.Services.AddDefaultCaptchaValidationService<NotifyBodyAdapter>(builder.Configuration.GetSection("AuthSettings"));
builder.Services.AddIpPolicyRateLimitSetup(builder.Configuration);
builder.Services.AddHangfire(config =>
{
    config.UseRedisStorage(builder.Configuration.GetValue<string>(AppSettingsItem.RedisHost));
});
builder.Services.AddHangfireServer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("global", v =>
    {
        var allowedHost = builder.Configuration.GetValue<string>("AllowedHosts");
        if (string.IsNullOrEmpty(allowedHost) || allowedHost == "*")
            v.SetIsOriginAllowed(_ => true);
        else
            v.WithOrigins(allowedHost.Split(","));

        v.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .WithExposedHeaders("Content-Disposition");
    });
});

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(serviceAssembly);

builder.Host.UseSerilogCenter(builder.Configuration);
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterType<EventInterception>();
    builder.RegisterType<TimeWatcherInterception>();

    var allServices = builder.RegisterAssemblyTypes(serviceAssembly);

    allServices.AsImplementedInterfaces()
                  .InstancePerLifetimeScope()
                  .PropertiesAutowired(new AutowiredPropertySelector())
                  .EnableInterfaceInterceptors()
                  .InterceptedBy(new Type[] { typeof(EventInterception), typeof(TimeWatcherInterception) });

    builder.RegisterAssemblyTypes(domainAssembly)
                .PropertiesAutowired(new AutowiredPropertySelector());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(options =>
{
    // Customize the message template
    options.MessageTemplate = "{RequestMethod,8:u} {RequestPath} from {Referer}, responded {StatusCode} in {Elapsed} ms, [UserId: {UserId,8} | ClientIP: {ClientIp,18} | ClientAgent: {ClientAgent}]";

    // Emit debug-level events instead of the defaults
    options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Information;

    // Attach additional properties to the request completion event
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("UserId", httpContext.Request.HttpContext.User.Identity?.GetUserId());
        diagnosticContext.Set("FullPath", httpContext.Request.GetEncodedUrl().ToString());
        diagnosticContext.Set("Referer", httpContext.Request.Headers["Referer"]);
        diagnosticContext.Set("ClientAgent", httpContext.Request.Headers.UserAgent);
    };
});
app.UseCors("global");
app.UseRouting();
app.UseIpRateLimiting();

app.UseMiddleware(typeof(GlobalErrorCatcher));
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseApiPermissionService<PermissionBase>();


app.MapControllers();
app.MapHangfireDashboard();
//app.MapHub<SignalRHub>("/SignalRHub");

app.Run();
