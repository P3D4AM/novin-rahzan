using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ItemSerializationContext
{
	public XmlElement ItemXmlElement;

	public bool HasSerializeItemHandlers;

	public bool HasDeserializeItemHandlers;

	public ICustomSerialization Serializer;

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal IDesignerHost _DesignerHost;

	internal Hashtable DockControls;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public BaseItem CreateItemFromXml(XmlElement xmlItem)
	{
		if (_DesignerHost != null)
		{
			BaseItem baseItem = null;
			string text = "";
			if (xmlItem.HasAttribute("name"))
			{
				text = xmlItem.GetAttribute("name");
			}
			try
			{
				baseItem = BarFunctions.CreateItemFromXml(xmlItem, _DesignerHost, text);
			}
			catch
			{
			}
			if (baseItem == null)
			{
				baseItem = BarFunctions.CreateItemFromXml(xmlItem, _DesignerHost, "");
			}
			if (text != "")
			{
				baseItem.GlobalName = text;
			}
			return baseItem;
		}
		return BarFunctions.CreateItemFromXml(xmlItem);
	}
}
