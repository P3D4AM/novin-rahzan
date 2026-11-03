using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class SourceForDisplay
{
	public ENSource Source { get; private set; }

	public string SourceString => Source.GetDisplay().GetTranslate();

	public SourceForDisplay(ENSource source)
	{
		Source = source;
	}
}
