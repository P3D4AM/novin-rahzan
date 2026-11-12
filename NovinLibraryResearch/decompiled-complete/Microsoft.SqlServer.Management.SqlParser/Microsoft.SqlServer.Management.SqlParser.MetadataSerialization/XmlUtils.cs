using System.Xml;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal static class XmlUtils
{
	public static void WriteRootElementStart(XmlWriter xmlWriter, XmlQualifiedName qName, XmlNamespace[] namespaces)
	{
		string prefix = null;
		XmlNamespace[] array = namespaces;
		foreach (XmlNamespace xmlNamespace in array)
		{
			if ((object)xmlNamespace.Urn == qName.Namespace)
			{
				prefix = xmlNamespace.Prefix;
			}
		}
		xmlWriter.WriteStartElement(prefix, qName.Name, qName.Namespace);
		array = namespaces;
		foreach (XmlNamespace xmlNamespace2 in array)
		{
			xmlWriter.WriteAttributeString("xmlns", xmlNamespace2.Prefix, null, xmlNamespace2.Urn);
		}
	}

	public static void WriteStartElement(XmlWriter xmlWriter, XmlQualifiedName qName)
	{
		xmlWriter.WriteStartElement(qName.Name, qName.Namespace);
	}

	public static void WriteEndElement(XmlWriter xmlWriter)
	{
		xmlWriter.WriteEndElement();
	}

	public static void WriteAttribute(XmlWriter xmlWriter, XmlQualifiedName qName, string value)
	{
		xmlWriter.WriteAttributeString(qName.Name, qName.Namespace, value);
	}
}
