using System;
using System.Collections.Generic;

namespace Resequencer
{
    public class RedisStateStore : IStateStore
    {
        public void BufferMessage(Message<object> message)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Message<object>> GetBufferedMessages()
        {
            throw new NotImplementedException();
        }

        public MessageHeader GetCurrent(string groupId)
        {
            throw new NotImplementedException();
        }

        public void UpdateCurrent(MessageHeader messageHeader)
        {
            throw new NotImplementedException();
        }
    }
}
