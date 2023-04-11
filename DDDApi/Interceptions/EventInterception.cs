using Castle.DynamicProxy;
using DDDApplication.Contract.Variables;
using System.Diagnostics;
using System.Reflection;
using Utility.Extensions;

namespace DDDApi.Interceptions
{
    public class TimeWatcherInterception : IInterceptor
    {
        readonly ILogger<TimeWatcherInterception> _logger;

        public TimeWatcherInterception(ILogger<TimeWatcherInterception> logger)
        {
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            invocation.Proceed();//直接执行被拦截方法
            sw.Stop();
            if (sw.Elapsed >= TimeSpan.FromSeconds(2))
            {
                _logger.LogWarning($"method {invocation.Method.Name} has taked {sw.Elapsed.TotalSeconds}s ");
            }
        }
    }

    public class EventInterception : IInterceptor
    {
        readonly ILogger<EventInterception> _logger;
        readonly IHttpContextAccessor _httpContextAccessor;
        int _userId;
        public EventInterception(ILogger<EventInterception> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?.Identity?.GetUserId() ?? 0;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();//直接执行被拦截方法
            var eventInfo = invocation.Method.GetCustomAttribute<NoticeEventAttribute>();
            if (eventInfo != null)
            {
                _logger.LogInformation(eventInfo.Content.Replace("{UserId}", _userId.ToString()));
            }
        }
    }
}
