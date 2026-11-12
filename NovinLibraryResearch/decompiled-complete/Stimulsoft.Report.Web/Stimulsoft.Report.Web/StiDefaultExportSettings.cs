using Stimulsoft.Report.Export;

namespace Stimulsoft.Report.Web;

public class StiDefaultExportSettings
{
	public StiPdfExportSettings ExportToPdf { get; } = new StiPdfExportSettings();


	public StiXpsExportSettings ExportToXps { get; } = new StiXpsExportSettings();


	public StiPpt2007ExportSettings ExportToPowerPoint { get; } = new StiPpt2007ExportSettings();


	public StiHtmlExportSettings ExportToHtml { get; } = new StiHtmlExportSettings();


	public StiTxtExportSettings ExportToText { get; } = new StiTxtExportSettings();


	public StiRtfExportSettings ExportToRtf { get; } = new StiRtfExportSettings();


	public StiWord2007ExportSettings ExportToWord2007 { get; } = new StiWord2007ExportSettings();


	public StiOdtExportSettings ExportToOpenDocumentWriter { get; } = new StiOdtExportSettings();


	public StiExcelExportSettings ExportToExcel { get; } = new StiExcelExportSettings();


	public StiOdsExportSettings ExportToOpenDocumentCalc { get; } = new StiOdsExportSettings();


	public StiDataExportSettings ExportToData { get; } = new StiDataExportSettings();


	public StiImageExportSettings ExportToImage { get; } = new StiImageExportSettings();

}
