using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Settings;

public class TaxBillTypeForDisplay
{
	public EnTaxBillType Type { get; private set; }

	public string TypeForDisplay => Type.GetDisplay().GetTranslate();

	public TaxBillTypeForDisplay(EnTaxBillType _type)
	{
		Type = _type;
	}
}
