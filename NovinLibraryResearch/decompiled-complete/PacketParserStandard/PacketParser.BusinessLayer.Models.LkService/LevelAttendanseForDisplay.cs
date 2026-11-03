using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.LkService;

public class LevelAttendanseForDisplay
{
	public TypeHozor Type { get; set; }

	public string TypeForDisplay => Type.GetDisplay().GetTranslate();

	public LevelAttendanseForDisplay(TypeHozor type)
	{
		Type = type;
	}
}
