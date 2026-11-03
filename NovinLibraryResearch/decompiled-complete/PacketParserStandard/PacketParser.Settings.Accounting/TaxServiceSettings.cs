using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class TaxServiceSettings : Serializable<TaxServiceSettings>
{
	private List<string> _taxUrlList = new List<string>();

	[DefaultValue(false)]
	public bool TaxServiceEnable { get; set; }

	[DefaultValue(EnTaxBillType.Disable)]
	public EnTaxBillType TaxBillType { get; set; }

	public string URL { get; set; } = "";


	public string ClientId { get; set; } = "";


	[DefaultValue(EnTaxTokenType.SoftwareToken)]
	public EnTaxTokenType TokenType { get; set; } = EnTaxTokenType.SoftwareToken;


	public string PublicKey { get; set; } = "";


	public string PrivateKey { get; set; } = "";


	public List<string> DefaultUrls
	{
		get
		{
			List<string> taxUrlList = _taxUrlList;
			if (taxUrlList == null || !taxUrlList.Any())
			{
				_taxUrlList = new List<string> { "https://tp.tax.gov.ir/req/api/", "https://sandboxrc.tax.gov.ir/req/api/" };
			}
			return _taxUrlList;
		}
		set
		{
			_taxUrlList = value;
		}
	}

	public string SellerBranchNumber { get; set; }
}
