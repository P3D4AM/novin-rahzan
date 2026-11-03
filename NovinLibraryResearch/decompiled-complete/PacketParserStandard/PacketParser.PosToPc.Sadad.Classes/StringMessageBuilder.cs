using System.Text;

namespace PacketParser.PosToPc.Sadad.Classes;

internal class StringMessageBuilder : MessageBuilder
{
	public string Data { get; set; }

	public Encoding Encoding { get; set; } = Encoding.ASCII;


	public bool IsPadRight { get; set; } = true;


	public override int Length
	{
		get
		{
			if (Data.Length >= base.MaxLength)
			{
				return Data.Length;
			}
			if (!IsPadRight)
			{
				return base.MaxLength;
			}
			return Data.Length;
		}
	}

	public char PadChar { get; set; } = ' ';


	public StringMessageBuilder()
	{
	}

	public StringMessageBuilder(string name, int order, string data, int length, Encoding encoding, bool isPadRight = true, char padChar = ' ')
	{
		base.Name = name;
		base.Order = order;
		Data = data;
		base.MaxLength = length;
		Encoding = encoding;
		IsPadRight = isPadRight;
		PadChar = padChar;
	}

	public override string ToString()
	{
		return $"{base.Order} {base.Name} {base.MaxLength} {Encoding} {IsPadRight} {PadChar} {Data}";
	}
}
