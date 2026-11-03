using Newtonsoft.Json;
using PacketParser.Entities;

namespace PacketParser.PosToPc;

public class PosToPcResponseData
{
	public string AcquirerDiscountAmount { get; set; }

	public string TransactionAmou { get; set; }

	public ResultCode ResultCode { get; set; }

	public string ResultDescription { get; set; }

	[JsonProperty("refrenceID")]
	public string RefrenceId { get; set; }

	public string RetrievalReferencedNumber { get; set; }

	public string DateOfTransaction { get; set; }

	public string TimeOfTransaction { get; set; }

	public string MaskedCardNumber { get; set; }

	public string BIN { get; set; }

	public string TerminalID { get; set; }

	public string AcquirerMessage { get; set; }
}
