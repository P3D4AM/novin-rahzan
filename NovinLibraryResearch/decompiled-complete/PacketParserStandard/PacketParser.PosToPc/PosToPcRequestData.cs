using System;
using Newtonsoft.Json;

namespace PacketParser.PosToPc;

public class PosToPcRequestData
{
	[JsonProperty("versionName")]
	public string VersionName { get; set; } = "1000";


	[JsonProperty("sessionId")]
	public string SessionId { get; set; } = Guid.NewGuid().ToString();


	[JsonProperty("applicationId")]
	public int ApplicationId { get; set; } = 3;


	[JsonProperty("totalAmount")]
	public string TotalAmount { get; set; }

	[JsonProperty("payerId")]
	public string PayerId { get; set; }

	[JsonProperty("accountId")]
	public string AccountId { get; set; } = "13246578";


	[JsonProperty("transactionType")]
	public string TransactionType { get; set; } = "PURCHASE";


	[JsonProperty("paymentDetail")]
	public string PaymentDetail { get; set; }

	[JsonProperty("merchantMessage")]
	public string MerchantMessage { get; set; }

	[JsonProperty("merchantAdditionalData")]
	public string MerchantAdditionalData { get; set; }

	[JsonProperty("printPaymentDetails")]
	public bool PrintPaymentDetails { get; set; }
}
