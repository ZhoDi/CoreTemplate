using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using CoreTemplate.Domain.Shared.Helper;
using Microsoft.AspNetCore.Builder;

namespace CoreTemplate.Middlewares
{
    /// <summary>
    /// IpLimitMiddleware
    /// </summary>
    public static class IpLimitMiddleware
    {
        public static void UseIpLimitMiddleware(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            try
            {
                if (Appsettings.App("Middleware", "IpRateLimit", "Enabled") == "true")
                {
                    app.UseIpRateLimiting();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ip限流错误:" + e.Message);
            }
        }
    }
}
