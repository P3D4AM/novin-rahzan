namespace PacketParser.PosToPc.Sadad.Classes;

internal class TLV
{
	public string Data
	{
		get
		{
			if (RawData == null)
			{
				return "";
			}
			return Length.ToString(LengthFormat) + RawData;
		}
	}

	public int Length
	{
		get
		{
			if (RawData == null)
			{
				return 0;
			}
			return RawData.Length;
		}
	}

	public string LengthFormat { get; private set; } = "D4";


	public string RawData { get; private set; }

	public TLV(string rawData, string lengthFormat = "D4")
	{
		RawData = rawData;
		LengthFormat = lengthFormat;
	}

	public string ToBcdString()
	{
		int length = RawData.Length;
		length = length;
		return length.ToString(LengthFormat) + RawData;
	}

	public override string ToString()
	{
		if (RawData == null)
		{
			return "";
		}
		return Length.ToString(LengthFormat) + RawData;
	}
}
