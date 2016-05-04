using System.Collections.Generic;
using System.Linq;

namespace Resequencer
{
	public class InProcessStateStore: IStateStore
	{
		private Dictionary<string, SortedSet<Message<object>>> MessageBuffer = new Dictionary<string, SortedSet<Message<object>>>();
		private Dictionary<string, MessageHeader> CurrenSequenceCache = new Dictionary<string, MessageHeader>();

		public void BufferMessage(Message<object> message)
		{
			//If group already has messages in the list,
			//Add it to the list and sort.

			var groupId = message.Header.GroupId;

			if(!MessageBuffer.ContainsKey(groupId))
			{
				MessageBuffer[groupId] = new SortedSet<Message<object>>();
			}

			var groupMessages = MessageBuffer[groupId];
			groupMessages.Add(message);
		}

		public IEnumerable<Message<object>> GetBufferedMessages()
		{
			//Need to go through CurrentSequenceCache and check every group in MessageBuffer
			//If first message for the group in MessageBuffer has sequence number by one greater that current sequence
			//number for the same group in CurrentSequenceCache, return all messages from that buffer that are in sequence.

			var listOfMessages = new List<Message<object>>();

			foreach(var groupId in CurrenSequenceCache.Keys)
			{
				if(MessageBuffer.ContainsKey(groupId))
				{
					var groupCurrentSequence = CurrenSequenceCache[groupId];
					var groupMessageBuffer = MessageBuffer[groupId];

					var currentBufferedMessage = groupMessageBuffer.FirstOrDefault();

                    if(currentBufferedMessage == null)
                    {
                        MessageBuffer.Remove(groupId);
                        continue;
                    }

					var previousBufferedMessage = default(Message<object>);
					var shouldContinue = currentBufferedMessage.Header.SequenceNumber - groupCurrentSequence.SequenceNumber == 1;

					while (shouldContinue)
					{
						listOfMessages.Add(currentBufferedMessage);
						groupMessageBuffer.Remove(currentBufferedMessage);
						previousBufferedMessage = currentBufferedMessage;
						currentBufferedMessage = groupMessageBuffer.FirstOrDefault();

                        if (currentBufferedMessage == null)
                        {
                            MessageBuffer.Remove(groupId);
                            break;
                        }

                        shouldContinue = currentBufferedMessage.Header.SequenceNumber - previousBufferedMessage.Header.SequenceNumber == 1;
					}
				}
			}

			return listOfMessages;
		}

		public MessageHeader GetCurrent(string groupId)
		{
			if(!CurrenSequenceCache.ContainsKey(groupId))
			{
				return default(MessageHeader);
			}

			return CurrenSequenceCache[groupId];
		}

		public void UpdateCurrent(MessageHeader messageHeader)
		{
			CurrenSequenceCache[messageHeader.GroupId] = messageHeader;
		}
	}
}
