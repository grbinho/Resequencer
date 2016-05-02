using System.Threading;
using static System.Console;

namespace IntermediateProcessor
{
	class Program
	{			
		static void Main(string[] args)
		{
			/*
			 * This porcessor will run multiple workers that take messages from the input queue.
			 * To some percentage of input messages, we will add random delay before sending them to the destination queue.		
			 */

			WriteLine($"Storage connection string: {Configuration.StorageAccountConnectionString}");
			WriteLine($"Redis connection string: {Configuration.RedisConnectionString}");
			WriteLine($"Input queue name: {Configuration.InputQueueName}");
			WriteLine($"Output queue name: {Configuration.OutputQueueName}");


			var processor = new Processor(Configuration.StorageAccountConnectionString, Configuration.InputQueueName, Configuration.OutputQueueName);
			var cancellationToken = new CancellationToken();

			processor.Process(cancellationToken);
		}
	}
}
