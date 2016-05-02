namespace MessageProducer
{
	public class Message<T> where T: class
	{
		public Message(MessageHeader header, T payload)
		{
			Header = header;
			Payload = payload;
		}

		public MessageHeader Header { get; set; }
		public T Payload { get; set; }
	}
}
