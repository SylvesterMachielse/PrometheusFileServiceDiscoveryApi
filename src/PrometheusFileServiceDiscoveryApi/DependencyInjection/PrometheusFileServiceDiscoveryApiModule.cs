using System.Collections.Generic;
using System.IO.Abstractions;
using Autofac;
using PrometheusFileServiceDiscoveryApi.Services.FileOperations;
using PrometheusFileServiceDiscoveryApi.Services.Models;
using PrometheusFileServiceDiscoveryApi.Services.Settings;
using PrometheusFileServiceDiscoveryApi.Services.Targets;

namespace PrometheusFileServiceDiscoveryApi.DependencyInjection
{
    public class PrometheusFileServiceDiscoveryApiModule : Module
    {
        private readonly AppConfiguration _configuration;

        public PrometheusFileServiceDiscoveryApiModule(AppConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<SettingsProvider>().As<IProvideSettings>().SingleInstance();
            builder.Register(x => new SettingsProvider(_configuration)).As<IProvideSettings>().SingleInstance();
            builder.RegisterType<FileReader>().As<IReadFiles>();
            builder.RegisterType<FileWriter>().As<IWriteFiles>();
            builder.RegisterType<TargetsProvider>().As<IProvideTargets>().SingleInstance();
            builder.RegisterType<TargetPersister>().As<IPersistTargets>().SingleInstance();
            builder.RegisterType<TargetDeleter>().As<IDeleteTargets>().SingleInstance();
            builder.RegisterType<FileOperationAttempter>().As<IAttemptFileOperations>().SingleInstance();
            builder.Register(context => new FileSystem()).As<IFileSystem>();
        }
    }
}