using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Report;

public class StiTypeItem
{
	public StiType Type { get; private set; }

	public string TypeName => Type.GetDisplay().GetTranslate();

	public StiTypeItem(StiType _type)
	{
		Type = _type;
	}
}
