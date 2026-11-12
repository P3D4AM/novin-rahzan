using System;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class XmlImporter : FmtImporter
{
	protected override void ReadContent()
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlSyntaxEditInfo));
		try
		{
			((SyntaxEdit)owner).XmlInfo = xmlSerializer.Deserialize(reader);
		}
		catch (Exception exception)
		{
			ErrorHandler.Error(exception);
		}
	}
}
