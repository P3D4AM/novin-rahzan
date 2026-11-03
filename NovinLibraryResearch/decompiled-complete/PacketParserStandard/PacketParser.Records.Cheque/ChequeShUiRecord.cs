using System.Collections.Generic;

namespace PacketParser.Records.Cheque;

public record ChequeShUiRecord
{
	public List<string> UsedNumberList { get; set; }

	public decimal? Price { get; set; }

	public string PoshtNomre { get; set; }

	public string Serial { get; set; }

	public string BankName { get; set; }

	public string AccountNumber { get; set; }

	public string SayadiName { get; set; }

	public string SayadiIdentity { get; set; }

	public string SayadiNationalCode { get; set; }
}
