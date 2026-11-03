using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Xml;

namespace DevComponents.DotNetBar;

public class ItemSerializationContext
{
	public XmlElement ItemXmlElement;

	public bool HasSerializeItemHandlers;

	public bool HasDeserializeItemHandlers;

	public ICustomSerialization Serializer;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public IDesignerHost _DesignerHost;

	internal Hashtable ᵠ;

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
				baseItem = BarFunctions._0953(xmlItem, _DesignerHost, text);
			}
			catch
			{
			}
			if (baseItem == null)
			{
				baseItem = BarFunctions._0953(xmlItem, _DesignerHost, "");
			}
			if (text != "")
			{
				baseItem.GlobalName = text;
			}
			return baseItem;
		}
		return BarFunctions._0953(xmlItem);
	}
}
