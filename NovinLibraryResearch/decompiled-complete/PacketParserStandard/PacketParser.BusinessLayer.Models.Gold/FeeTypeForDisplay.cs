using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Gold;

public class FeeTypeForDisplay
{
	public EnFeeType Type { get; private set; }

	public string TypeForDisplay => Type.GetDisplay().GetTranslate();

	public FeeTypeForDisplay(EnFeeType type)
	{
		Type = type;
	}
}
