using System.Configuration;

namespace IntermediateProcessor
{
	public static class Configuration
	{
		public static string StorageAccountConnectionString => ConfigurationManager.AppSettings["StorageAccountConnectionString"];
		public static string RedisConnectionString => ConfigurationManager.AppSettings["RedisConnectionString"];
		public static string InputQueueName => ConfigurationManager.AppSettings["InputQueueName"];
		public static string OutputQueueName => ConfigurationManager.AppSettings["OutputQueueName"];
		public static int NumberOfProcessors => int.Parse(ConfigurationManager.AppSettings["NumberOfProcessors"]);
        public static int PercentageOfDelay => int.Parse(ConfigurationManager.AppSettings["PercentageOfDelay"]);

    }
}
