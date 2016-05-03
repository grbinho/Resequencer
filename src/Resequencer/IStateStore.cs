using System.Collections.Generic;

namespace Resequencer
{
	public interface IStateStore
	{
		MessageHeader GetCurrent(string groupId);

		void UpdateCurrent(MessageHeader message);

		void BufferMessage<T>(Message<T> message) where T: class;

		IEnumerable<Message<T>> GetBufferedMessages<T>() where T : class;
	}
}
