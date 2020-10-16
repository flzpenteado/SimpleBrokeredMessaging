using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleBrokeredMessaging.Receiver
{
    class ReceiverConsole
    {
        static string ConnectionString = "Endpoint=sb://hdn-origins.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=JjasGOb2B46he9eR9CtHLqsBNQexlJuaOYfqa9e6mIk=";
        static string QueuePath = "demo-queue";

        static void Main(string[] args)
        { 
        var queueClient = new QueueClient(ConnectionString, QueuePath);

            queueClient.RegisterMessageHandler(ProcessMessagesAsync, HandleExceptionsAsync);

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();


        }
        private async static Task ProcessMessagesAsync(Message message, CancellationToken cancellationToken)
        {
            var content = Encoding.UTF8.GetString(message.Body);

            Console.WriteLine($"Received: ${content}"); ;

        }

        private static Task HandleExceptionsAsync(ExceptionReceivedEventArgs arg)
        {
            throw new NotImplementedException();
        }


    }
}
