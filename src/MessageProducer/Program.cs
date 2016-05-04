using System;
using System.Threading;
using static System.Console;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace MessageProducer
{
	class Program
	{
		private static readonly int MaxMessages = 20;
		private static readonly int MaxDelay = 5000;
		private static Random RandomSequenceGenerator = new Random();
		private static Random RandomDelayGenerator = new Random();

		static void Main(string[] args)
		{
			/*
			 * Generate groups of messages in sequence. Sequence lenght will be random number in a range from 1 to 20.
			 * Messages are grouped with a groupId. Last message will have End flag set to true.
			 * 
			 * There will be random delay between each groups.
			 */		

			//Current configuration
			WriteLine($"Storage connection string: {Configuration.StorageAccountConnectionString}");
			WriteLine($"Redis connection string: {Configuration.RedisConnectionString}");
			WriteLine($"Run forever: {Configuration.RunForever}");
			WriteLine($"Destination queue name: {Configuration.DestinationQueueName}");


			//Get storage account
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Configuration.StorageAccountConnectionString);
			//Create queue client
			CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
			//Create queue if it does not exist (rules of azure queues, say that it needs to be lower
			CloudQueue destinationQueue = queueClient.GetQueueReference(Configuration.DestinationQueueName.ToLowerInvariant());
			destinationQueue.CreateIfNotExists();

			do
			{
				//Generate sequences of messages with random range and random delay
				var sequenceLength = RandomSequenceGenerator.Next(1, MaxMessages);
				var groupId = Guid.NewGuid().ToString();

				for(var i = 1; i<=sequenceLength; i++) { 	
					var header = new MessageHeader
					{
						GroupId = groupId,
						SequenceNumber = i,
						End = i == sequenceLength
					};

					var message = new Message<object>(header, new
					{
						Test = "Test parameter for test payload!"
					});

					//Send message
					CloudQueueMessage queueMessage = new CloudQueueMessage(JsonConvert.SerializeObject(message));
					destinationQueue.AddMessage(queueMessage);

					WriteLine($"GroupId: {header.GroupId}, Seq: {header.SequenceNumber}, End: {header.End}");

                }				

				Thread.Sleep(RandomDelayGenerator.Next(500, MaxDelay));

			} while (Configuration.RunForever);
		}
	}
}
