using PacketParser.Entities;

namespace PacketParser.PosToPc.Sadad.Classes;

public class SaleIdData
{
	public decimal Amount { get; set; }

	public string IBAN { get; set; }

	public int Index { get; set; }

	public EnSadadRowType RowType { get; set; }

	public string SaleId { get; set; }

	internal TLV SaleIdTlv { get; set; }

	public SaleIdData(int index, string iban, string saleId, decimal amount, EnSadadRowType rowType = EnSadadRowType.Private)
	{
		RowType = rowType;
		Index = index;
		IBAN = iban;
		SaleIdTlv = new TLV(saleId, "D2");
		SaleId = saleId;
		Amount = amount;
	}

	public string ToBcdString()
	{
		if (!string.IsNullOrEmpty(IBAN))
		{
			string text = RowType.ToString("D2");
			string iBAN = IBAN;
			string text2 = SaleIdTlv.ToBcdString();
			return text + iBAN + text2 + ((long)Amount).ToString("D12");
		}
		string text3 = RowType.ToString("D2");
		string text4 = Index.ToString("D4");
		string text5 = SaleIdTlv.ToBcdString();
		return text3 + text4 + text5 + ((long)Amount).ToString("D12");
	}

	public override string ToString()
	{
		if (!string.IsNullOrEmpty(IBAN))
		{
			string text = RowType.ToString("D2");
			string iBAN = IBAN;
			string data = SaleIdTlv.Data;
			return text + iBAN + data + ((long)Amount).ToString("D12");
		}
		string text2 = RowType.ToString("D2");
		string text3 = Index.ToString("D4");
		string data2 = SaleIdTlv.Data;
		return text2 + text3 + data2 + ((long)Amount).ToString("D12");
	}
}
