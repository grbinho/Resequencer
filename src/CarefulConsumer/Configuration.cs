using System.Configuration;

namespace CarefulConsumer
{
	public class Configuration
	{
		public static string StorageAccountConnectionString => ConfigurationManager.AppSettings["StorageAccountConnectionString"];
		public static string RedisConnectionString => ConfigurationManager.AppSettings["RedisConnectionString"];
		public static string InputQueueName => ConfigurationManager.AppSettings["InputQueueName"];
		public static bool RunForever => bool.Parse(ConfigurationManager.AppSettings["RunForever"]);
	}
}
