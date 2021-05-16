using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoreTemplate.Application.Middlewares
{
    /// <summary>
    /// Swagger 中间件
    /// </summary>
    public static class SwaggerMilddleware
    {
        public static void UseSwaggerMilddleware(this IApplicationBuilder app, Func<Stream> streamHtml)
        {
            app.UseSwagger();
            try
            {
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreTemplate API v1");
                    var basePath = AppContext.BaseDirectory;
                    //var index = new FileStream(Path.Combine(basePath + @"/wwwroot/swagger/ui/index.html"), FileMode.Open);
                    //自定义UI
                    options.IndexStream = streamHtml;
                });
            }
            catch (Exception e)
            {
                Console.WriteLine("swagger报错"+e);
            }
        }
    }
}
