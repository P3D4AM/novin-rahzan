using PacketParser;
using PacketParser.Entities;

namespace FormCustom.UC;

public class ChartTypeForDisplay
{
	public ChartType Type { get; private set; }

	public string Name => Type.GetDisplay().GetTranslate();

	public ChartTypeForDisplay(ChartType _doc)
	{
		Type = _doc;
	}
}
