using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Tax;

public class TaxResultForDisplay
{
	public EnTaxResult TaxResult { get; private set; }

	public string ResultForDisplay => TaxResult.GetDisplay().GetTranslate();

	public TaxResultForDisplay(EnTaxResult taxResult)
	{
		TaxResult = taxResult;
	}
}
