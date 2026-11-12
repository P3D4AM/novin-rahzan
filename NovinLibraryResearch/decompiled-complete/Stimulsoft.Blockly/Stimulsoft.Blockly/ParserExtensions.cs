using System.Xml;

namespace Stimulsoft.Blockly;

internal static class ParserExtensions
{
	public static XmlNode GetChild(this XmlNode node, string name)
	{
		foreach (XmlNode childNode in node.ChildNodes)
		{
			if (childNode.LocalName == name)
			{
				return childNode;
			}
		}
		return null;
	}

	public static string GetAttribute(this XmlNode node, string name)
	{
		foreach (XmlAttribute attribute in node.Attributes)
		{
			if (attribute.Name == name)
			{
				return attribute.Value;
			}
		}
		return null;
	}
}
