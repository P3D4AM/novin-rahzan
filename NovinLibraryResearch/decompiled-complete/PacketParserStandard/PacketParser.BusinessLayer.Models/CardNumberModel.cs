using Newtonsoft.Json;

namespace PacketParser.BusinessLayer.Models;

public class CardNumberModel
{
	public string CardNumber { get; set; }

	[JsonIgnore]
	public string Card1
	{
		get
		{
			if (!string.IsNullOrEmpty(CardNumber) && CardNumber.Length >= 4)
			{
				return CardNumber.Substring(0, 4);
			}
			return "";
		}
	}

	[JsonIgnore]
	public string Card2
	{
		get
		{
			if (!string.IsNullOrEmpty(CardNumber) && CardNumber.Length >= 8)
			{
				return CardNumber.Substring(4, 4);
			}
			return "";
		}
	}

	[JsonIgnore]
	public string Card3
	{
		get
		{
			if (!string.IsNullOrEmpty(CardNumber) && CardNumber.Length >= 12)
			{
				return CardNumber.Substring(8, 4);
			}
			return "";
		}
	}

	[JsonIgnore]
	public string Card4
	{
		get
		{
			if (!string.IsNullOrEmpty(CardNumber) && CardNumber.Length >= 16)
			{
				return CardNumber.Substring(12, 4);
			}
			return "";
		}
	}
}
