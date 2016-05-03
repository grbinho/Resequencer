using System.Collections.Generic;

namespace Resequencer
{
	public interface IStateStore
	{
		/// <summary>
		/// Returns current message header for the group. Message header has sequence number in it.
		/// </summary>
		/// <param name="groupId">Group for which to get current header</param>
		/// <returns><see cref="MessageHeader"/></returns>
		MessageHeader GetCurrent(string groupId);

		/// <summary>
		/// Update message header for the group. Since header contains groupId it will be enough.
		/// </summary>
		/// <param name="messageHeader">Message header to set as current.</param>
		void UpdateCurrent(MessageHeader messageHeader);

		/// <summary>
		/// Save this message to buffer
		/// </summary>
		/// <typeparam name="T">Type of the payload</typeparam>
		/// <param name="message">Message to save</param>
		void BufferMessage(Message<object> message);

		/// <summary>
		/// Get all messages that can be processed.
		/// </summary>
		/// <typeparam name="T">Type of the payload</typeparam>
		/// <returns>Returns all messages that can be processed. For every group it will return all messages that are in buffer and are in sequence as per current highest sequence number.</returns>
		IEnumerable<Message<object>> GetBufferedMessages();
	}
}
