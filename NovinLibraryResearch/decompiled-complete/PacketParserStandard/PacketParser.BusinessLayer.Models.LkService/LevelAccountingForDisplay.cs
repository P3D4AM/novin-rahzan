using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.LkService;

public class LevelAccountingForDisplay
{
	public ENCode Level { get; set; }

	public string LevelForDisplay => Level.GetDisplay().GetTranslate();

	public LevelAccountingForDisplay(ENCode type)
	{
		Level = type;
	}
}
