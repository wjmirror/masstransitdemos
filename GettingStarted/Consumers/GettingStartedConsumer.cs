namespace Company.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using Microsoft.Extensions.Logging;

    public class GettingStartedConsumer :
        IConsumer<HelloMessage>
    {
        readonly ILogger<GettingStartedConsumer> _logger;

        public GettingStartedConsumer(ILogger<GettingStartedConsumer> logger)
        {
            _logger = logger;
        }
    
        public Task Consume(ConsumeContext<HelloMessage> context)
        {
            _logger.LogInformation($"Hello {context.Message.Name}");
            return Task.CompletedTask;
        }
    }
}