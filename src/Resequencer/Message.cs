using System;

namespace Resequencer
{
	public class Message<T> : IComparable<Message<T>>
		where T : class
	{
		public Message(MessageHeader header, T payload)
		{
			Header = header;
			Payload = payload;
		}

		public MessageHeader Header { get; set; }
		public T Payload { get; set; }

		public int CompareTo(Message<T> other)
		{
			//<0 -> Current instance precedes other
			//=0 -> Order is the same
			//>0 -> Current instance is afer other

			if (other.Header.GroupId != Header.GroupId)
				return 0;

			if (Header.SequenceNumber > other.Header.SequenceNumber)
				return 1;

			if (Header.SequenceNumber < other.Header.SequenceNumber)
				return -1;

			if (Header.SequenceNumber == other.Header.SequenceNumber)
				return 0;

			return 0;
		}	

		public Message<object> ToMessageObject()
		{
			return new Message<object>(Header, Payload);
		}
 	}
}
