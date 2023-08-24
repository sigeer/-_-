using AspNetCoreRateLimit;
using DDDApi.Models;
using DDDDomain.Users;
using DDDEF;
using DDDUtility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;
using Utility.Files;
using Utility.Notify;
using Utility.Notify.Email;
using Utility.Serilog.Extension;

namespace DDDApi
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDbContextCenter(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StorageDbContext>(option =>
            {
                option.UseMySql(configuration.GetConnectionString(AppSettingItems.ConnectStr), new MySqlServerVersion("8.0.27"));
            });
        }

        public static void AddDDDServices(this IServiceCollection services)
        {
            services.AddSingleton<IFileManager, DefaultLocalFileManager>(x => new DefaultLocalFileManager(AppSettingItems.GetUploadRootDir()));
        }

        public static void AddNotifyService(this IServiceCollection services, IConfiguration configuration)
        {
            var emailConfig = new EmailConfigs();
            configuration.GetSection("EmailConfigs").Bind(emailConfig);
            services.AddSingleton<EmailConfigs>(emailConfig);
            services.AddSingleton<EmailNotify>();

            services.AddSingleton<NotifyResolver>(options =>
            {
                Func<Type, INotifyBase> accessor = key =>
                {
                    if (key == typeof(EmailNotify))
                        return new EmailNotify(emailConfig);
                    throw new ArgumentNullException($"{key} is not supported!");
                };
                return new NotifyResolver(accessor);
            });

        }
        public static void AddJwtTokenCenter(this IServiceCollection services, IConfigurationSection authSection)
        {
            #region token Authentication
            services.AddAuthentication(s =>
            {
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30),
                    ValidateAudience = true,
                    ValidAudience = authSection["Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = authSection["Issuer"],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSection["IssuerSigningKey"]!))
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        //Token expired
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    },
                    OnMessageReceived = (context) =>
                    {
                        if (!context.HttpContext.Request.Path.HasValue)
                        {
                            return Task.CompletedTask;
                        }
                        //重点在于这里；判断是Signalr的路径
                        var accessToken = context.HttpContext.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrWhiteSpace(accessToken) && path.StartsWithSegments("/SignalRHub"))
                        {
                            context.Token = accessToken;
                            return Task.CompletedTask;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddScoped<IIdentityUserContainer, IdentityUserContainer>();
            #endregion
        }

        public static void UseSerilogCenter(this IHostBuilder host, IConfiguration configuration)
        {
            host.UseSerilog((context, config) =>
            {
                //config.ReadFrom.Configuration(configuration);
                config
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("Hangfire", LogEventLevel.Warning)
                    .Enrich.WithClientIp()
                    .Enrich.WithClientAgent()
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.Seq("http://localhost:5341/")
                .CustomWriteTo();
            });
        }

        public static void AddIpPolicyRateLimitSetup(this IServiceCollection services, IConfiguration Configuration)
        {
            ArgumentNullException.ThrowIfNull(services);
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            services.AddDistributedRateLimiting();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }
    }
}
