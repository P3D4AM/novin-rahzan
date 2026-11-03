using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class DocumentItem
{
	public Document Document { get; private set; }

	public string DocumentStr { get; private set; }

	public DocumentItem(Document _doc)
	{
		Document = _doc;
		if (_doc == Document.None)
		{
			DocumentStr = "انتخاب همه".GetTranslate();
		}
		else
		{
			DocumentStr = Document.GetDisplay().GetTranslate();
		}
	}
}
