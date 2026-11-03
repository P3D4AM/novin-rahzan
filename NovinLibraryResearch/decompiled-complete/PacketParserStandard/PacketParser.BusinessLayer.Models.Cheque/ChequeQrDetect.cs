using Newtonsoft.Json;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Cheque;

public class ChequeQrDetect
{
	public string NationalCode { get; set; }

	public string ShabaCode { get; set; }

	public string BranchCode { get; set; }

	public string ChequeNumber { get; set; }

	public string SayadiNumber { get; set; }

	public EnBank BankType { get; set; }

	[JsonIgnore]
	public string BankTypeForDisplay => BankType.GetDisplay();
}
