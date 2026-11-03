namespace PacketParser.PosToPc.Sadad.Classes;

internal class ByteArrayMessageBuilder : MessageBuilder
{
	public byte[] Data { get; set; }

	public override int Length => Data.Length;

	public ByteArrayMessageBuilder()
	{
	}

	public ByteArrayMessageBuilder(string name, int order, byte[] data, int maxLength)
	{
		base.Name = name;
		base.Order = order;
		Data = data;
		base.MaxLength = maxLength;
	}

	public override string ToString()
	{
		return $"{base.Order} {base.Name} {base.MaxLength} {Data}";
	}
}
