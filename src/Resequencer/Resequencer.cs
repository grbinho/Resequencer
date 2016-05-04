using System;
using System.Collections.Generic;

namespace Resequencer
{
	/*
	 * Receives messages (objects with header property) and resequences them for subscribers.
	 * Inital version will only be able to call one internal handler, but eventually we want
	 * to handle subscribers and different message types.
	 * 
	 * To work, resequencer needs in memory buffer to store provided messages and to resequence them.
	 * Assumption is that messages do not come out of sequence that often, so we need to optimize
	 * for sequential intake of messages. 
	 * 
	 * Messages have group id's and messages from same group id, need to come in sequence.
	 *  
	 */

	public class Resequencer
	{
		private readonly IStateStore StateStore;
		private List<Func<Message<object>, bool>> Subscribers;

		public Resequencer(IStateStore stateStore)
		{
			StateStore = stateStore;
			Subscribers = new List<Func<Message<object>, bool>>();
		}

		/// <summary>
		/// Add message to resequencer. Message will be either processed, or buffered for future processing.
		/// It will be passed to processing depending on it's sequence numbers.
		/// </summary>
		/// <param name="message">Message to resequence.</param>
		public void AddMessage(Message<object> message)
		{
			if(IsNexInSequence(message))
			{
				Handle(message);
				//Save new current sequence to the store
				StateStore.UpdateCurrent(message.Header);
			}
			else
			{
				//Buffer the message. We leave it up to the store to handle ordering
				StateStore.BufferMessage(message);
			}

			//Find out if there are candidates for handling.
			//We should handle all buffered messages that are available.
			//Those are all messages that are buffered and their sequence number
			//is bigger that current sequence number stored.
			var bufferedMessages = StateStore.GetBufferedMessages();

			foreach(var bufferedMessage in bufferedMessages)
			{
				Handle(bufferedMessage);
				//Save new current sequence to the store
				StateStore.UpdateCurrent(message.Header);
			}
		}

		/// <summary>
		/// Add messages to resequencer. Messages will be either processed or buffered for future processing.
		/// </summary>
		/// <param name="messages">Messages to resequence.</param>
		public void AddMessages(IList<Message<object>> messages)
		{
			foreach(var message in messages)
			{
				AddMessage(message);
			}
		}


		/// <summary>
		/// Check if given message is next message in sequence. It uses <see cref="IStateStore"/> to do the check.
		/// </summary>
		/// <param name="message">Message to check</param>
		/// <returns>True if message is next in sequence, False otherwise</returns>
		private bool IsNexInSequence(Message<object> message)
		{
			//For groupId, get current sequence number from the store.
			var currentMessage = StateStore.GetCurrent(message.Header.GroupId);
						
			//If message sequence number is 1 and currentMessage == default(MessageHeader)
			//This is first message in sequence. Save header and process the message.
			if(default(MessageHeader) == currentMessage && message.Header.SequenceNumber == 1)
			{
				return true;
			}
			//If message sequence number - currentMessage.SequenceNumber == 1.
			//This message is next in sequence.
			if(message.Header.SequenceNumber - currentMessage.SequenceNumber == 1)
			{
				return true;
			}

			//Else, message is not in sequence.
			return false;
		}

		/// <summary>
		/// Call all subscribers with provided message.
		/// </summary>
		/// <param name="message">Message to handle</param>
		private void Handle(Message<object> message)
		{
			foreach(var subscriber in Subscribers)
			{
				var result = subscriber(message);

				//TODO: Decide how to handle false results (keep local queue for each subscriber?)
			}
		}

		//TODO: Throw events when message is received or call callbacks??

		/// <summary>
		/// Subscribe handlers for messages
		/// </summary>
		/// <param name="handler">Function that handles message processing.</param>
		public void Subscribe(Func<Message<object>, bool> handler)
		{
			Subscribers.Add(handler);
		}
	}
}
