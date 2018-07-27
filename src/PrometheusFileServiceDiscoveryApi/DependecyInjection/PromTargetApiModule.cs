using System.IO.Abstractions;
using Autofac;
using PrometheusFileServiceDiscoveryApi.Services.FileOperations;
using PrometheusFileServiceDiscoveryApi.Services.Settings;
using PrometheusFileServiceDiscoveryApi.Services.Targets;

namespace PrometheusFileServiceDiscoveryApi.DependecyInjection
{
    public class PromTargetApiModule : Module
    {
        private readonly string _targetsFileLocation;

        public PromTargetApiModule(string targetsFileLocation)
        {
            _targetsFileLocation = targetsFileLocation;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => new SettingsProvider(_targetsFileLocation)).As<IProvideSettings>().SingleInstance();
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