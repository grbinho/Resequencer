using System;
using System.Collections.Generic;

namespace Resequencer
{
	public class RedisStateStore : IStateStore
	{
		public void BufferMessage<T>(Message<T> message) where T : class
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Message<T>> GetBufferedMessages<T>() where T : class
		{
			throw new NotImplementedException();
		}

		public MessageHeader GetCurrent(string groupId)
		{
			throw new NotImplementedException();
		}

		public void UpdateCurrent(MessageHeader message)
		{
			throw new NotImplementedException();
		}
	}
}
