using System;
using System.IO;
using Microsoft.AspNetCore.Builder;

namespace CoreTemplate.Middlewares
{
    /// <summary>
    /// Swagger 中间件
    /// </summary>
    public static class SwaggerMiddleware
    {
        public static void UseSwaggerMiddleware(this IApplicationBuilder app, Func<Stream> streamHtml)
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
