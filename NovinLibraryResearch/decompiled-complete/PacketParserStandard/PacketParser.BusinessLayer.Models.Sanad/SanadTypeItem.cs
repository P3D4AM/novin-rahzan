using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Sanad;

public class SanadTypeItem
{
	public SanadType SanadType { get; private set; }

	public string SanadTypeForDisplay => SanadType.GetDisplay().GetTranslate();

	public SanadTypeItem(SanadType _doc)
	{
		SanadType = _doc;
	}
}
