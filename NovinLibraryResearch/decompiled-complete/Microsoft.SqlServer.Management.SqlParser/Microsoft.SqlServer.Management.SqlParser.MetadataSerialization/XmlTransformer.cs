using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal static class XmlTransformer
{
	internal static void Transform(MemoryStream xmlInputStream, Stream xslInputStream)
	{
		using Stream input = CloneStream(xmlInputStream);
		using XmlReader input2 = XmlReader.Create(input);
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.Indent = true;
		xmlWriterSettings.NewLineChars = Environment.NewLine;
		xmlWriterSettings.Encoding = new UTF8Encoding();
		xmlInputStream.SetLength(0L);
		using XmlWriter results = XmlWriter.Create(xmlInputStream, xmlWriterSettings);
		using XmlReader stylesheet = XmlReader.Create(xslInputStream);
		XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();
		xslCompiledTransform.Load(stylesheet);
		xslCompiledTransform.Transform(input2, results);
	}

	private static Stream CloneStream(MemoryStream inputStream)
	{
		MemoryStream memoryStream = new MemoryStream();
		inputStream.Seek(0L, SeekOrigin.Begin);
		inputStream.WriteTo(memoryStream);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		return memoryStream;
	}
}
