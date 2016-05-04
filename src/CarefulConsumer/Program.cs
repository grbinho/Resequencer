using System.Linq;
using static System.Console;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using System.Threading;
using Resequencer;

namespace CarefulConsumer
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
			 * Receive messages from input queue
			 * 
			 * 
			 */

			WriteLine($"Storage connection string: {Configuration.StorageAccountConnectionString}");
			WriteLine($"Redis connection string: {Configuration.RedisConnectionString}");
			WriteLine($"Input queue name: {Configuration.InputQueueName}");


			int messageBatchSize = 1;
			int dequeueWaitLength = 100;

			//Get storage account
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Configuration.StorageAccountConnectionString);
			//Create queue client
			CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
			//Create queue if it does not exist (rules of azure queues, say that it needs to be lower
			CloudQueue inputQueue = queueClient.GetQueueReference(Configuration.InputQueueName.ToLowerInvariant());

            var resequencer = new Resequencer.Resequencer(new InProcessStateStore());
            resequencer.Subscribe((message) =>
            {
                WriteLine(message.Header.ToString());
                return true;
            });

			do
			{
				//Dequeue message
				var inputMessages = inputQueue.GetMessages(messageBatchSize);

				if(inputMessages == null || !inputMessages.Any())
				{
					//WriteLine($"No messages in the queue, waiting for: {dequeueWaitLength}ms");
					//If there are no messages, wait a bit
					Thread.Sleep(dequeueWaitLength);
					dequeueWaitLength += 100;
					if (dequeueWaitLength > 30000)
					{
						dequeueWaitLength = 30000;
					}

					continue;
				}

				foreach(var queueMessage in inputMessages)
				{
					var message = JsonConvert.DeserializeObject<Message<object>>(queueMessage.AsString);                    
                    resequencer.AddMessage(message);
					inputQueue.DeleteMessage(queueMessage);					
				}				

				dequeueWaitLength = 0;
			} while (Configuration.RunForever);

		}
	}
}
