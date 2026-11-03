using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Aghsat;

public class AveargeTypeForDisplay
{
	public ENMoadel Type { get; set; }

	public string TypeForDisplay => Type.GetDisplay().GetTranslate();

	public AveargeTypeForDisplay(ENMoadel type)
	{
		Type = type;
	}
}
