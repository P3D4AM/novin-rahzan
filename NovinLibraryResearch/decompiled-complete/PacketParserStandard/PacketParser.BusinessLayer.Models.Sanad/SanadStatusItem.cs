using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Sanad;

public class SanadStatusItem
{
	public SanadSt SanadStatus { get; private set; }

	public string SanadStatusForDisplay => SanadStatus.GetDisplay().GetTranslate();

	public SanadStatusItem(SanadSt _doc)
	{
		SanadStatus = _doc;
	}
}
