using Stimulsoft.Data.Exceptions;
using Stimulsoft.Report;

namespace Stimulsoft.Dashboard.Export.Helpers;

public static class StiExportFormatHelper
{
	public static StiExportFormat Convert(StiDashboardExportFormat format)
	{
		return format switch
		{
			StiDashboardExportFormat.Pdf => StiExportFormat.Pdf, 
			StiDashboardExportFormat.Excel => StiExportFormat.Excel2007, 
			StiDashboardExportFormat.Html => StiExportFormat.Html, 
			StiDashboardExportFormat.Data => StiExportFormat.Data, 
			StiDashboardExportFormat.Image => StiExportFormat.Image, 
			StiDashboardExportFormat.Document => StiExportFormat.Document, 
			_ => throw new StiTypeNotRecognizedException(format), 
		};
	}
}
