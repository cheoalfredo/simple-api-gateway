using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using MyApiGw;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Mime;
using System.Net;
using System.Collections.Generic;
using MyApiGw.Models;
using MyApiGw.Middleware;
using Microsoft.AspNetCore.Server.Kestrel.Https;

public class Program
{


    public static void Main(string[] args)
    {

        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        Host.CreateDefaultBuilder(args)           
        .ConfigureServices((hostBuilder, services) =>
        {
            services.AddHttpClient();
            var JsonConfig = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appgwcfg.json", false)
                       .AddJsonFile("routes.json", true).Build();

            services.AddSingleton<IConfiguration>(icfg => JsonConfig);
        })        
        .ConfigureWebHostDefaults(webBuilder =>
        {

            webBuilder.ConfigureKestrel(opts =>
            {
                opts.ConfigureHttpsDefaults(opt =>
                {
                    opt.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                    opt.CheckCertificateRevocation = false;
                    opt.AllowAnyClientCertificate();
                    /*opt.ClientCertificateValidation = (a, b, c) =>
                    {
                        return true;
                    };*/
                });
            });
            webBuilder.Configure(app =>
            {

                var iConfig = app.ApplicationServices.GetService<IConfiguration>();
                app.UseRouting();
                app
                .UseMiddleware<MutualTLSMiddleware>()
                .UseMiddleware<BWListMiddleware>()
                .UseMiddleware<ApiGatewayMiddleware>();
                app.UseEndpoints(route =>
                {
                    route.MapGet("/", context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        return context.Response.WriteAsync(string.Format(iConfig.GetValue<string>("404Body"), iConfig.GetValue<string>("404Logo").Replace(@"\", "")));
                    });
                });
            });
        })
        .Build().Run();
    }
}