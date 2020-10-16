using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleBrokeredMessaging.ChatConsole
{
    class ChatApplication
    {

        static string ConnectionString = "Endpoint=sb://hdn-origins.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=JjasGOb2B46he9eR9CtHLqsBNQexlJuaOYfqa9e6mIk=";
        static string TopicPath = "chat-topic";
        static void Main(string[] args)
        {
            Console.WriteLine("Enter name:");
            var userName = Console.ReadLine();

            var manager = new ManagementClient(ConnectionString);

            if (!manager.TopicExistsAsync(TopicPath).Result)
            {
                manager.CreateTopicAsync(TopicPath).Wait();
            }

            var description = new SubscriptionDescription(TopicPath, userName)
            {
                AutoDeleteOnIdle = TimeSpan.FromMinutes(5)
            };

            manager.CreateSubscriptionAsync(description).Wait();

            var topicClient = new TopicClient(ConnectionString, TopicPath);

            var subscriptionClient = new SubscriptionClient(ConnectionString, TopicPath, userName);

            subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, ExceptionReceivedHandler);

            var helloMessage = new Message(Encoding.UTF8.GetBytes("Has entered the room"));
            helloMessage.Label = userName;
            topicClient.SendAsync(helloMessage).Wait();

            while (true)
            {
                string text = Console.ReadLine();

                if (text.Equals("\\exit"))
                {
                    break;
                }

                var chatMessage = new Message(Encoding.UTF8.GetBytes(text));
                chatMessage.Label = userName;
                topicClient.SendAsync(chatMessage).Wait();
            }

            var goodbyeMessage = new Message(Encoding.UTF8.GetBytes("Has left the room"));
            goodbyeMessage.Label = userName;
            topicClient.SendAsync(goodbyeMessage).Wait();


            topicClient.CloseAsync().Wait();
            subscriptionClient.CloseAsync().Wait();
        }

        private async static Task ProcessMessagesAsync(Message message, CancellationToken cancellationToken)
        {
            var text = Encoding.UTF8.GetString(message.Body);
            Console.WriteLine($"{ message.Label } > { text }");
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            return Task.CompletedTask;
        }


    }
}
