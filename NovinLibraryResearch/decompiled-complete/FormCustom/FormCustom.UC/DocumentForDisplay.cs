using PacketParser;
using PacketParser.Entities;

namespace FormCustom.UC;

public class DocumentForDisplay
{
	public Document FactorType { get; private set; }

	public string Name => FactorType.GetDisplay().GetTranslate();

	public DocumentForDisplay(Document _doc)
	{
		FactorType = _doc;
	}
}
