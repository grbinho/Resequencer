namespace IntermediateProcessor
{
	public class MessageHeader
	{
		public string GroupId { get; set; }
		public int SequenceNumber { get; set; }
		public bool End { get; set; } = false;
	}
}
