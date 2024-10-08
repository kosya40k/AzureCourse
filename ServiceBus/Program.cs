#pragma warning disable IDE0063

using Azure.Messaging.ServiceBus;

class Program
{
    // Service Bus Namespace -> Queue -> Settings -> Shared Access Policies
    private const string FROM_QUEUE_CONNECTION_STRING = ""; 
    private const string TO_QUEUE_CONNECTION_STRING = "";

    private const string FROM_QUEUE_NAME = "queue-1";
    private const string TO_QUEUE_NAME = "queue-2";

    private static async Task Main()
    {
        try
        {
            while (true) 
            {
                var message = await ReceiveMessageAsync();

                if (message != null)
                {
                    Console.WriteLine($"Message: {message.Body} - received from {FROM_QUEUE_NAME}");

                    var sendMessage = new ServiceBusMessage(message);
                    await SendMessageAsync(sendMessage);

                    Console.WriteLine($"Message: {message.Body} - sent to {TO_QUEUE_NAME}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static async Task<ServiceBusReceivedMessage> ReceiveMessageAsync()
    {
        await using (var client = new ServiceBusClient(FROM_QUEUE_CONNECTION_STRING))
        {
            var receiverOptions = new ServiceBusReceiverOptions()
            {
                ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete
            };
            var receiver = client.CreateReceiver(FROM_QUEUE_NAME, receiverOptions);

            return await receiver.ReceiveMessageAsync();
        }
    }

    private static async Task SendMessageAsync(ServiceBusMessage message)
    {
        await using (var client = new ServiceBusClient(TO_QUEUE_CONNECTION_STRING))
        {
            var sender = client.CreateSender(TO_QUEUE_NAME);

            await sender.SendMessageAsync(message);
            // Then check message in Service Bus Explorer
        }
    }
}