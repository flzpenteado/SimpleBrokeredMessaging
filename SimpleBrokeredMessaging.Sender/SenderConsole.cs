using Microsoft.Azure.ServiceBus;
using System;
using System.Text;

namespace SimpleBrokeredMessaging.Sender
{
    class SenderConsole
    {

        static string ConnectionString = "Endpoint=sb://hdn-origins.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=JjasGOb2B46he9eR9CtHLqsBNQexlJuaOYfqa9e6mIk=";
        static string QueuePath = "demo-queue";

        static void Main(string[] args)
        {
            var queueClient = new QueueClient(ConnectionString, QueuePath);

            for (var i = 0; i < 10; i++)
            {
                var content = $"Message: { i }";

                var message = new Message(Encoding.UTF8.GetBytes(content));

                queueClient.SendAsync(message).Wait();

                Console.WriteLine($"Sent: { content }");
            }

            queueClient.CloseAsync().Wait();
        }
    }
}
