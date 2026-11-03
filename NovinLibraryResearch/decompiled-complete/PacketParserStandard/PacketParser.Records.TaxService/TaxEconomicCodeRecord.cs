using System.ComponentModel;
using Newtonsoft.Json;

namespace PacketParser.Records.TaxService;

public record TaxEconomicCodeRecord
{
	[JsonIgnore]
	public static string KeyName => "TaxEconomicCodeResult";

	public string TaxClientId { get; private set; }

	[DefaultValue("")]
	public string NameTrade { get; set; }

	[DefaultValue("")]
	public string TaxpayerStatus { get; set; }

	[DefaultValue("")]
	public string TaxpayerType { get; set; }

	[DefaultValue("")]
	public string PostalcodeTaxpayer { get; set; }

	[DefaultValue("")]
	public string AddressTaxpayer { get; set; }

	[DefaultValue("")]
	public string EconomicCode { get; set; }

	public TaxEconomicCodeRecord(string taxClientId)
	{
		TaxClientId = taxClientId;
	}
}
