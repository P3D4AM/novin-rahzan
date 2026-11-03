using System.Collections.Generic;
using System.Text;
using PacketParser.Entities;

namespace PacketParser.PosToPc.Sadad.Classes;

public class SaleIdProvider
{
	internal int Count
	{
		get
		{
			if (SaleIds == null)
			{
				return 0;
			}
			return SaleIds.Count;
		}
	}

	internal EnSadadFormatType FormatType { get; set; }

	public List<SaleIdData> SaleIds { get; set; } = new List<SaleIdData>();


	public SaleIdProvider()
	{
		FormatType = EnSadadFormatType.Ascii;
	}

	public SaleIdProvider(List<SaleIdData> saleIds)
	{
		SaleIds = saleIds;
	}

	public byte[] ToBcd()
	{
		return Utility.ConvertToBinaryCodedDecimal(isLittleEndian: false, ToBcdString());
	}

	public string ToBcdString()
	{
		StringBuilder stringBuilder = new StringBuilder(100000);
		SaleIds.ForEach(delegate(SaleIdData s)
		{
			stringBuilder.Append(s.ToBcdString());
		});
		object arg = (int)FormatType;
		return string.Format("{0}{1}{2}", arg, Count.ToString("D2"), stringBuilder.ToString());
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder(100000);
		SaleIds.ForEach(delegate(SaleIdData s)
		{
			stringBuilder.Append(s.ToString());
		});
		return Count.ToString("D2") + stringBuilder.ToString();
	}
}
