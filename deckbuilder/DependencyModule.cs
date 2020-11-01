namespace deckbuilder
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using deckbuilder.BusinessLogic;
    using deckbuilder.BusinessLogic.Interfaces;
    using deckbuilder.DataAccess;
    using deckbuilder.DataAccess.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO;

    public class DependencyModule: Module
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new DependencyModule());
            builder.RegisterInstance(Environment.GetEnvironmentVariables());
            return builder.Build();
        }

        private void ConfigureLogging(ContainerBuilder builder)
        {
            var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();

            IConfiguration configuration = configBuilder.Build();
            builder.RegisterInstance(configuration);

            var services = new ServiceCollection().AddLogging();

            builder.Populate(services);

            var loggerFactory = services.BuildServiceProvider().GetService<ILoggerFactory>();

            loggerFactory.AddAWSProvider(configuration.GetAWSLoggingConfigSection());
            loggerFactory.AddLambdaLogger(new LambdaLoggerOptions(configuration));
            builder.RegisterInstance(loggerFactory);
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            ConfigureLogging(builder);

            //Inject Libraries
            // example
            // builder.RegisterType<IMPLEMENTATION>().As<INTERFACE>().SingleInstance();
            builder.RegisterType<DeckService>().As<IDeckService>().SingleInstance();
        }
    }
}
