﻿using System.Diagnostics;
using System.IO;
using System.Linq;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrometheusFileServiceDiscoveryApi.Services.Models;


namespace PromTargetApi
{
    public class Program
    {
        public IConfiguration Configuration { get; }

        public static void Main(string[] args)
        {
            bool isService = true;
            if (Debugger.IsAttached || args.Contains("--console"))
            {
                isService = false;
            }

            var pathToContentRoot = Directory.GetCurrentDirectory();
            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                pathToContentRoot = Path.GetDirectoryName(pathToExe);
            }

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddCommandLine(args)
                .Build();

            var host = BuildWebHost(args, configuration, pathToContentRoot);

            host.Run();

            //if (isService)
            //{
            //    host.RunAsService();
            //}
            //else
            //{
            //    host.Run();
            //}
        }

        public static IWebHost BuildWebHost(string[] args, IConfigurationRoot configuration,
            string pathToContentRoot) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseContentRoot(pathToContentRoot)
                .ConfigureServices(services =>
                {
                    services.AddAutofac();
                })
                .UseUrls(configuration["Host"])
                .Build();
    }
}