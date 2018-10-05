using System.Collections.Generic;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PrometheusFileServiceDiscoveryApi.DependencyInjection;
using PrometheusFileServiceDiscoveryApi.Services.Models;

namespace PromTargetApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<AppConfiguration>(Configuration);
            services.AddMvc();
            services.AddAutofac();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //var fileConfigurations =   Configuration.GetValue<List<FileConfiguration>>("fileconfigurations");
          
            

            //var fileConfigurations = Configuration.GetValue<List<FileConfiguration>>("FileConfigurations");

            //var blarf = (List<FileConfiguration>) fileConfigurations;

            var appConfiguration = Configuration.Get<AppConfiguration>();
            builder.RegisterModule(new PrometheusFileServiceDiscoveryApiModule(appConfiguration));
                //builder.RegisterModule(new PrometheusFileServiceDiscoveryApiModule());
        }
    }
}