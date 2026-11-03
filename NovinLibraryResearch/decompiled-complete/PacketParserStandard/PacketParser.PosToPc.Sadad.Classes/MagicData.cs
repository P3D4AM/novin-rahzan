namespace PacketParser.PosToPc.Sadad.Classes;

public class MagicData
{
	public string Amount { get; set; }

	public string BillNo { get; set; }

	public string MerchantId { get; set; }

	public string RequestId { get; set; }

	public string TerminalId { get; set; }

	public MagicData()
	{
		RequestId = string.Empty;
		TerminalId = string.Empty;
		MerchantId = string.Empty;
		BillNo = string.Empty;
		Amount = string.Empty;
	}
}
