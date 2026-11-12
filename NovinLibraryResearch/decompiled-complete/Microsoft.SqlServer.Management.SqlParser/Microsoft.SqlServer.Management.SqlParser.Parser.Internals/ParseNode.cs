using System.Globalization;
using System.IO;
using System.Xml;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal struct ParseNode
{
	public object value;

	public string Xml
	{
		get
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture);
			XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
			{
				OmitXmlDeclaration = true,
				Indent = true
			});
			WriteXml(xmlWriter);
			xmlWriter.Flush();
			string result = stringWriter.ToString();
			stringWriter.Dispose();
			return result;
		}
	}

	public void WriteXml(XmlWriter writer)
	{
		writer.WriteStartElement(GetType().Name);
		if (value != null)
		{
			writer.WriteString(value.ToString());
		}
		writer.WriteEndElement();
	}

	public ParseNode(SqlCodeObject value)
	{
		this.value = value;
	}
}
