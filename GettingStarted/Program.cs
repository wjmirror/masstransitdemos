using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Company.Consumers;
using Contracts;

namespace GettingStarted
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(bus =>
                    {
                        bus.SetKebabCaseEndpointNameFormatter();

                        //// By default, sagas are in-memory, but should be changed to a durable
                        //// saga repository.
                        //x.SetInMemorySagaRepositoryProvider();

                        //var entryAssembly = Assembly.GetEntryAssembly();

                        //x.AddConsumers(entryAssembly);
                        //x.AddSagaStateMachines(entryAssembly);
                        //x.AddSagas(entryAssembly);
                        //x.AddActivities(entryAssembly);

                        bus.AddConsumer<GettingStartedConsumer>();

                        bus.UsingRabbitMq((context, config) =>
                        {
                            config.Host("localhost", "/", rabbit =>
                            {
                                rabbit.Username("guest");
                                rabbit.Password("guest");
                            });
                            config.Message<HelloMessage>(cfg =>
                            {
                                cfg.SetEntityName("weberp.demo");
                            });
                            config.ReceiveEndpoint("weberp.demo.test", cfgEndpoint =>
                            {
                                cfgEndpoint.ConfigureConsumeTopology = false;
                                cfgEndpoint.Bind<HelloMessage>();
                                cfgEndpoint.ConfigureConsumer<GettingStartedConsumer>(context);
                            });

                        });
                    });
                    services.AddHostedService<Worker>();
                });
    }
}
