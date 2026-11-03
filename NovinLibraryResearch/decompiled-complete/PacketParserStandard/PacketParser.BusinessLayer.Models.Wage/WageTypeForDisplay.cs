using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Wage;

public class WageTypeForDisplay
{
	public EnWageType Type { get; private set; }

	public string TypeName => Type.GetDisplay().GetTranslate();

	public WageTypeForDisplay(EnWageType _type)
	{
		Type = _type;
	}
}
