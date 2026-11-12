using System.IO;
using Stimulsoft.Report.Export;

namespace Stimulsoft.Report.Web;

public class StiExportReportResponseEventArgs : StiExportReportEventArgs
{
	public Stream Stream { get; set; }

	public string FileName { get; set; }

	public string ContentType { get; set; }

	public StiExportReportResponseEventArgs(StiRequestParams requestParams, StiReport report, StiExportSettings settings, Stream stream, string fileName, string contentType)
		: base(requestParams, report, settings)
	{
		Stream = stream;
		FileName = fileName;
		ContentType = contentType;
	}
}
