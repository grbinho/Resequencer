using Newtonsoft.Json;
using System;

namespace Resequencer
{
	public struct MessageHeader : IEquatable<MessageHeader>
	{
		public MessageHeader(string groupId, int sequenceNumber)
			: this(groupId, sequenceNumber, false)
		{
		}

		[JsonConstructor]
		public MessageHeader(string groupId, int sequenceNumber, bool end)
		{
			GroupId = groupId;
			SequenceNumber = sequenceNumber;
			End = end;
		}

		public string GroupId { get; set; }
		public int SequenceNumber { get; set; }
		public bool End { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			return Equals((MessageHeader)obj);
		}

		public bool Equals(MessageHeader other)
		{
			return
				GroupId == other.GroupId &&
				SequenceNumber == other.SequenceNumber &&
				End == other.End;
		}

		public static bool operator ==(MessageHeader left, MessageHeader right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(MessageHeader left, MessageHeader right)
		{
			return !left.Equals(right);
		}

		public override int GetHashCode()
		{
			return GroupId.GetHashCode() ^ SequenceNumber.GetHashCode() ^ End.GetHashCode();
		}

		public override string ToString()
		{
			return $"Group id: {GroupId}, Seq: {SequenceNumber}, End message: {End}";
		}
	}
