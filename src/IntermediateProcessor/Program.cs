using System.Threading;
using System.Threading.Tasks;
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
			WriteLine($"Number of processors: {Configuration.NumberOfProcessors}");
            WriteLine($"Percentage of delay: {Configuration.PercentageOfDelay}");

			var cancellationToken = new CancellationToken();
			Task[] processors = new Task[Configuration.NumberOfProcessors];

			for(int i = 0; i < Configuration.NumberOfProcessors; i++)
			{
				var task = new Task(() => {					
					var processor = new Processor(Configuration.StorageAccountConnectionString, Configuration.InputQueueName, Configuration.OutputQueueName);
					processor.Process(cancellationToken);
					}, cancellationToken);
			
				processors[i] = task;
				task.Start();
			}

			Task.WaitAll(processors);		
		}
	}
}
