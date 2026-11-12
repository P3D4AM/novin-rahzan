using System;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class XmlExporter : FmtExporter
{
	private void WriteProps()
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlSyntaxEditInfo));
		try
		{
			xmlSerializer.Serialize(writer, ((SyntaxEdit)owner).XmlInfo);
		}
		catch (Exception exception)
		{
			writer.Flush();
			ErrorHandler.Error(exception);
		}
	}

	protected override void WriteContent()
	{
		WriteProps();
	}
}
