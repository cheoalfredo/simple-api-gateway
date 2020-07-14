using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace K8SCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .UseSerilog((hostBuilderContext, loggerConfig) =>
                 {
                     loggerConfig.MinimumLevel.Information()
                         .ReadFrom.Configuration(hostBuilderContext.Configuration)
                         .WriteTo.Console();
                         /*.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(hostBuilderContext.Configuration.GetValue<string>("ElasticServer")))
                         {
                             AutoRegisterTemplate = true,
                             AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6
                         })*/
                        // .WriteTo.File($"Api-{DateTime.Now.Millisecond}.log", rollingInterval: RollingInterval.Day);
                 })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
