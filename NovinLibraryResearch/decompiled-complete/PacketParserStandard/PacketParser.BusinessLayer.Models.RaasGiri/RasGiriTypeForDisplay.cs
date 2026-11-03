using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.RaasGiri;

public class RasGiriTypeForDisplay
{
	public EnRasGiriType Type { get; private set; }

	public string TypeForDisplay => Type.GetDisplay().GetTranslate();

	public RasGiriTypeForDisplay(EnRasGiriType _type)
	{
		Type = _type;
	}
}
