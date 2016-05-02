using System.Configuration;

namespace IntermediateProcessor
{
	public static class Configuration
	{
		public static string StorageAccountConnectionString => ConfigurationManager.AppSettings["StorageAccountConnectionString"];
		public static string RedisConnectionString => ConfigurationManager.AppSettings["RedisConnectionString"];
		public static string InputQueueName => ConfigurationManager.AppSettings["InputQueueName"];
		public static string OutputQueueName => ConfigurationManager.AppSettings["OutputQueueName"];
	}
}
