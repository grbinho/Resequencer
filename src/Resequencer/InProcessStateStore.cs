using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resequencer
{
	public class InProcessStateStore : IStateStore
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
