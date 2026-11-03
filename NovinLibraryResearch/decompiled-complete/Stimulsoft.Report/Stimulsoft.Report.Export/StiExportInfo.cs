using System.IO;

namespace Stimulsoft.Report.Export;

internal class StiExportInfo
{
	public StiReport Report { get; set; }

	public StiExportSettings Settings { get; set; }

	public Stream Stream { get; set; }

	public bool SendEMail { get; set; }

	public bool OpenAfterExport { get; set; }

	public string FileName { get; set; }

	public StiExportInfo(StiReport report, StiExportSettings settings, Stream stream, bool sendEMail, bool openAfterExport, string fileName)
	{
		Report = report;
		Settings = settings;
		Stream = stream;
		SendEMail = sendEMail;
		OpenAfterExport = openAfterExport;
		FileName = fileName;
	}
}
