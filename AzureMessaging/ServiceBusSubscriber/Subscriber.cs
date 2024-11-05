using Azure.Messaging.ServiceBus;

namespace ServiceBusSubscriber
{
    public class Subscriber(string subscription)
    {
        private readonly string _subscription = subscription;

        private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
        {
            var message = args.Message;

            Console.WriteLine($"Message: {message.Body} - received by {_subscription}");

            await args.CompleteMessageAsync(args.Message);
        }

        private async Task ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            var error = args.Exception;

            Console.WriteLine($"Error: {error.Message} - received by {_subscription}");

            await Task.CompletedTask;
        }

        public void Subscribe(ServiceBusProcessor processor)
        {
            processor.ProcessMessageAsync += ProcessMessageAsync;
            processor.ProcessErrorAsync += ProcessErrorAsync;
        }
    }
}
