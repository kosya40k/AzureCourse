using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Text;

class Program
{
    private const string STORAGE_ACCOUNT_CONNECTION_STRING = ""; // Storage Account -> Security -> Access keys
    private const string FROM_QUEUE_NAME = "queue-1";
    private const string TO_QUEUE_NAME = "queue-2";

    static void Main()
    {
        try
        {
            var fromQueueClient = new QueueClient(STORAGE_ACCOUNT_CONNECTION_STRING, FROM_QUEUE_NAME);
            var toQueueClient = new QueueClient(STORAGE_ACCOUNT_CONNECTION_STRING, TO_QUEUE_NAME);

            while (true)
            {
                QueueMessage[] retrievedMessages = fromQueueClient.ReceiveMessages();

                if (retrievedMessages.Length > 0)
                {
                    foreach (var message in retrievedMessages) 
                    {
                        var messageId = message.MessageId;
                        var popReceipt = message.PopReceipt;
                        var messageText = DecodeBase64(message.MessageText);

                        Console.WriteLine($"Message: {messageText} (ID: {messageId}) - dequeued from {FROM_QUEUE_NAME}");

                        fromQueueClient.DeleteMessage(messageId, popReceipt);

                        Console.WriteLine($"Message: {messageText} (ID: {messageId}) - deleted from {FROM_QUEUE_NAME}");

                        toQueueClient.SendMessage(message.MessageText);

                        Console.WriteLine($"Message: {messageText} - queued to {TO_QUEUE_NAME}");
                    } 
                }
            }
        }
        catch (Exception ex) 
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static string DecodeBase64(string base64Encoded)
    {
        var bytes = Convert.FromBase64String(base64Encoded);
        return Encoding.UTF8.GetString(bytes);
    }
}