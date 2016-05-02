using System;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage;
using System.Threading;

namespace IntermediateProcessor
{
	public class Processor
	{
		private readonly string StorageConnectionString;
		private readonly string InputQueueName;
		private readonly string OutputQueueName;
		private readonly CloudQueue InputQueue;
		private readonly CloudQueue OutputQueue;
		private readonly CloudQueueClient QueueClient;

		private readonly double PercentageOfDelay = 10;
		private readonly Random RandomGenerator = new Random();
		private readonly Random RandomDelayGenerator = new Random();
		private int DequeueWaitLenght = 0;	
		private readonly QueueRequestOptions RequestOptions;

		public Processor(string storageConnectionString, string inputQueueName, string outputQueueName)
		{
			if (string.IsNullOrWhiteSpace(storageConnectionString))
			{
				throw new ArgumentNullException(nameof(storageConnectionString));
			}

			if (string.IsNullOrWhiteSpace(inputQueueName))
			{
				throw new ArgumentNullException(nameof(inputQueueName));
			}

			if(string.IsNullOrWhiteSpace(outputQueueName))
			{
				throw new ArgumentNullException(nameof(outputQueueName));
			}

			StorageConnectionString = storageConnectionString;
			InputQueueName = inputQueueName.ToLowerInvariant();
			OutputQueueName = outputQueueName.ToLowerInvariant();

			RequestOptions = new QueueRequestOptions
			{
				RetryPolicy = new ExponentialRetry()
			};

			//Get storage account
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
			//Create queue client
			QueueClient = storageAccount.CreateCloudQueueClient();
			QueueClient.DefaultRequestOptions = RequestOptions;
			//Create queue if it does not exist
			InputQueue = QueueClient.GetQueueReference(InputQueueName);
			OutputQueue = QueueClient.GetQueueReference(OutputQueueName);
		}

		private bool ShouldDelay()
		{
			return RandomGenerator.Next(0, 100) <= PercentageOfDelay;
		}

		public void Process(CancellationToken cancellationToken)
		{
			OutputQueue.CreateIfNotExists();

			while (!cancellationToken.IsCancellationRequested)
			{
				Console.WriteLine("$Processing message.");

				var inputQueueMessage = InputQueue.GetMessage();

				if(inputQueueMessage == null)
				{
					Console.WriteLine($"Empty queue. Waiting {DequeueWaitLenght}ms.");
					Thread.Sleep(DequeueWaitLenght);
					DequeueWaitLenght += 100;
					if (DequeueWaitLenght > 30000)
					{
						DequeueWaitLenght = 30000;
					}

					continue;
				}

				//Check if we need to delay
				if (ShouldDelay())				
					Thread.Sleep(RandomDelayGenerator.Next(10, 100));

				//Put message to output queue
				OutputQueue.AddMessage(inputQueueMessage);

				//Delete the message
				InputQueue.DeleteMessage(inputQueueMessage);
				
				Console.WriteLine($"Message detected, restarting processing.");
				DequeueWaitLenght = 0;				
			}
		}
	}
}
