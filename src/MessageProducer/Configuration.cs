using System.Configuration;

namespace MessageProducer
{
	public static class Configuration
	{
		public static string StorageAccountConnectionString => ConfigurationManager.AppSettings["StorageAccountConnectionString"];
		public static string RedisConnectionString => ConfigurationManager.AppSettings["RedisConnectionString"];
		public static bool RunForever => bool.Parse(ConfigurationManager.AppSettings["RunForever"]);
		public static string DestinationQueueName => ConfigurationManager.AppSettings["DestinationQueueName"];
	}
}
