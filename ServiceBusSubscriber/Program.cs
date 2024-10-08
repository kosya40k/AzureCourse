#pragma warning disable IDE0063

using Azure.Messaging.ServiceBus;
using ServiceBusSubscriber;

class Program
{
    // Service Bus Namespace -> Topic -> Settings -> Shared Access Policies
    private const string CONNECTION_STRING = "";
    private const string TOPIC_NAME = "topic-1";
    private const string SUBSCRIPTION1_NAME = "subscription-1";
    private const string SUBSCRIPTION2_NAME = "subscription-2";

    static async Task Main()
    {
        try
        {
            await using (var client = new ServiceBusClient(CONNECTION_STRING))
            {
                var processor1 = client.CreateProcessor(TOPIC_NAME, SUBSCRIPTION1_NAME);
                var processor2 = client.CreateProcessor(TOPIC_NAME, SUBSCRIPTION2_NAME);

                var subscriber1 = new Subscriber(SUBSCRIPTION1_NAME);
                var subscriber2 = new Subscriber(SUBSCRIPTION2_NAME);

                subscriber1.Subscribe(processor1);
                subscriber2.Subscribe(processor2);

                await processor1.StartProcessingAsync();
                await processor2.StartProcessingAsync();

                Console.WriteLine("Press any key to stop processing...");
                Console.ReadKey();

                await processor1.StopProcessingAsync();
                await processor2.StopProcessingAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}