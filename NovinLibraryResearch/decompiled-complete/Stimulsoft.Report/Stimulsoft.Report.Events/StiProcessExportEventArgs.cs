using System.IO;
using Stimulsoft.Report.Export;

namespace Stimulsoft.Report.Events;

public class StiProcessExportEventArgs : StiExportEventArgs
{
	public virtual StiExportService ExportService { get; set; }

	public virtual Stream Stream { get; set; }

	public virtual StiExportSettings ExportSettings { get; set; }

	public virtual bool Processed { get; set; }

	public StiProcessExportEventArgs(StiExportFormat format, StiExportService exportService, Stream stream, StiExportSettings settings)
		: base(format)
	{
		ExportService = exportService;
		Stream = stream;
		ExportSettings = settings;
	}
}
