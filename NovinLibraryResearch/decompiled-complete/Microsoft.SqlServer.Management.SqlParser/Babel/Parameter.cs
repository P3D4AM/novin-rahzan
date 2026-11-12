using System.Xml;

namespace Babel;

public class Parameter
{
	private string name;

	private string description;

	private string display;

	public string Name => name;

	public string Description => description;

	public string Display => display;

	public Parameter(string name, string description, string display)
	{
		this.name = name;
		this.description = description;
		this.display = display;
	}

	internal void WriteXml(XmlWriter xmlWriter)
	{
		xmlWriter.WriteStartElement("Parameter");
		xmlWriter.WriteElementString("Name", Name);
		xmlWriter.WriteElementString("Description", Description);
		xmlWriter.WriteElementString("Display", Display);
		xmlWriter.WriteEndElement();
	}
}
