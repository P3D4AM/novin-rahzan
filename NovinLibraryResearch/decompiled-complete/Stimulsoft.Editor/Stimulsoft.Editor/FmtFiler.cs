using System.IO;

namespace Stimulsoft.Editor;

public class FmtFiler
{
	private ISyntaxEdit owner;

	private ISyntaxStrings lines;

	private ExportFormat format;

	public FmtFiler(ISyntaxEdit owner, ISyntaxStrings lines, ExportFormat format)
	{
		this.owner = owner;
		this.lines = lines;
		this.format = format;
	}

	public bool Write(TextWriter writer)
	{
		FmtExporter fmtExporter = null;
		switch (format)
		{
		case ExportFormat.Text:
			foreach (StrItem line in lines)
			{
				writer.WriteLine(line.String);
			}
			break;
		case ExportFormat.Rtf:
			fmtExporter = new RtfExporter();
			break;
		case ExportFormat.Html:
			fmtExporter = new HtmlExporter();
			break;
		case ExportFormat.Xml:
			fmtExporter = new XmlExporter();
			break;
		}
		if (fmtExporter != null && owner != null)
		{
			fmtExporter.Write(owner, lines, writer);
		}
		return fmtExporter != null;
	}

	public bool Read(TextReader reader)
	{
		FmtImporter fmtImporter = null;
		switch (format)
		{
		case ExportFormat.Text:
			lines.LoadStream(reader);
			break;
		case ExportFormat.Rtf:
			fmtImporter = new RtfImporter();
			break;
		case ExportFormat.Html:
			fmtImporter = new HtmlImporter();
			break;
		case ExportFormat.Xml:
			fmtImporter = new XmlImporter();
			break;
		}
		if (fmtImporter != null && owner != null)
		{
			fmtImporter.Read(owner, reader, null);
		}
		return fmtImporter != null;
	}
}
