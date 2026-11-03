using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class SeasonForDisplay
{
	public EnSeason Season { get; private set; }

	public string SeasonName => Season.GetDisplay().GetTranslate();

	public SeasonForDisplay(EnSeason _season)
	{
		Season = _season;
	}
}
