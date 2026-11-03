using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Export;

public static class StiImageHelper
{
	public static string GetPathImageOffice2013FromBitmapService(StiService service, double xFactor = 1.0)
	{
		string text = "";
		if (xFactor == 1.5)
		{
			text = "_x1_5";
		}
		if (xFactor == 2.0)
		{
			text = "_x2";
		}
		if (xFactor == 3.0)
		{
			text = "_x3";
		}
		if (xFactor == 4.0)
		{
			text = "_x4";
		}
		if (service is StiPdfExportService)
		{
			return "Stimulsoft.Report.Images.Office2013.Exports.FilePDF" + text + ".png";
		}
		if (service is StiXpsExportService)
		{
			return "Stimulsoft.Report.Images.Office2013.Exports.FileXPS" + text + ".png";
		}
		if (service is StiPpt2007ExportService)
		{
			return "Stimulsoft.Report.Images.Office2013.Exports.FilePowerPoint" + text + ".png";
		}
		if (service is StiHtmlExportService || service is StiHtml5ExportService || service is StiMhtExportService)
		{
			return "Stimulsoft.Report.Images.Office2013.Exports.FileHTML" + text + ".png";
		}
		if (service is StiTxtExportService)
		{
			return "Stimulsoft.Report.Images.Office2013.Exports.FileTXT" + text + ".png";
		}
		if (service is StiRtfExportService)
		{
			return "Stimulsoft.Report.Images.Office2013.Exports.FileRTF" + text + ".png";
		}
		if (service is StiWord2007ExportService)
		{
			return "Stimulsoft.Report.Images.Office2013.Exports.FileWord" + text + ".png";
		}
		if (service is StiOdtExportService)
		{
			return "Stimulsoft.Report.Images.Office2013.Exports.FileODW" + text + ".png";
		}
		if (service is StiExcelExportService || service is StiExcel2007ExportService || service is StiExcelXmlExportService)
		{
			return "Stimulsoft.Report.Images.Office2013.Exports.FileExcel" + text + ".png";
		}
		if (service is StiOdsExportService)
		{
			return "Stimulsoft.Report.Images.Office2013.Exports.FileODC" + text + ".png";
		}
		if (service is StiImageExportService)
		{
			return "Stimulsoft.Report.Images.Office2013.Exports.FileImage" + text + ".png";
		}
		if (service is StiDataExportService || service is StiCsvExportService || service is StiDbfExportService || service is StiDifExportService || service is StiSylkExportService || service is StiXmlExportService)
		{
			return "Stimulsoft.Report.Images.Office2013.Exports.FileData" + text + ".png";
		}
		return null;
	}
}
