using System;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Export;
using Stimulsoft.Report.Export;

namespace Stimulsoft.Report.Web;

public sealed class StiReportResponse
{
	public static void ResponseAsDocument(StiReport report, string documentType, string password)
	{
		ResponseAsDocument(null, report, documentType, password);
	}

	public static void ResponseAsDocument(StiReport report, StiDocumentType documentType, string password)
	{
		ResponseAsDocument(null, report, documentType, password);
	}

	public static void ResponseAsDocument(Page page, StiReport report, string documentType, string password)
	{
		switch (documentType)
		{
		case "SaveReportMdc":
		case "mdc":
			ResponseAsDocument(page, report, StiDocumentType.Mdc, password);
			break;
		case "SaveReportMdz":
		case "mdz":
			ResponseAsDocument(page, report, StiDocumentType.Mdz, password);
			break;
		case "SaveReportMdx":
		case "mdx":
			ResponseAsDocument(page, report, StiDocumentType.Mdx, password);
			break;
		}
	}

	public static void ResponseAsDocument(Page page, StiReport report, StiDocumentType documentType, string password)
	{
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		string text = ".mdc";
		string contentType = "text/xml";
		switch (documentType)
		{
		case StiDocumentType.Mdc:
			report.SaveDocument(stream);
			break;
		case StiDocumentType.Mdz:
			report.SavePackedDocument(stream);
			text = ".mdz";
			contentType = "application/zip";
			break;
		case StiDocumentType.Mdx:
			report.SaveEncryptedDocument(stream, password);
			text = ".mdz";
			contentType = "application/octet-stream";
			break;
		}
		ResponseStream(stream, contentType, browserCache: false, StiReportHelper.GetReportFileName(report) + text);
	}

	public static void ResponseAsPdf(StiReport report)
	{
		ResponseAsPdf(report, StiPagesRange.All);
	}

	public static void ResponseAsPdf(StiReport report, bool saveFileDialog)
	{
		ResponseAsPdf(report, StiPagesRange.All, 1f, 100f, embeddedFonts: true, standardPdfFonts: false, saveFileDialog);
	}

	public static void ResponseAsPdf(StiReport report, StiPagesRange pageRange)
	{
		ResponseAsPdf(report, pageRange, 1f, 100f, embeddedFonts: true, standardPdfFonts: false);
	}

	public static void ResponseAsPdf(StiReport report, StiPagesRange pageRange, bool saveFileDialog)
	{
		ResponseAsPdf(report, pageRange, 1f, 100f, embeddedFonts: true, standardPdfFonts: false, saveFileDialog);
	}

	public static void ResponseAsPdf(StiReport report, StiPagesRange pageRange, float imageQuality, float imageResolution, bool embeddedFonts, bool standardPdfFonts)
	{
		ResponseAsPdf(report, pageRange, imageQuality, imageResolution, embeddedFonts, standardPdfFonts, saveFileDialog: true);
	}

	public static void ResponseAsPdf(StiReport report, StiPagesRange pageRange, float imageQuality, float imageResolution, bool embeddedFonts, bool standardPdfFonts, bool saveFileDialog)
	{
		ResponseAsPdf(report, pageRange, imageQuality, imageResolution, embeddedFonts, standardPdfFonts, exportRtfTextAsImage: false, "", "", StiUserAccessPrivileges.All, StiPdfEncryptionKeyLength.Bit40, useUnicode: true, saveFileDialog);
	}

	public static void ResponseAsPdf(StiReport report, StiPagesRange pageRange, float imageQuality, float imageResolution, bool embeddedFonts, bool standardPdfFonts, bool exportRtfTextAsImage, string passwordUser, string passwordOwner, StiUserAccessPrivileges userAccessPrivileges, StiPdfEncryptionKeyLength keyLength, bool useUnicode, bool saveFileDialog)
	{
		StiPdfExportSettings stiPdfExportSettings = new StiPdfExportSettings();
		stiPdfExportSettings.PageRange = pageRange;
		stiPdfExportSettings.ImageQuality = imageQuality;
		stiPdfExportSettings.ImageResolution = imageResolution;
		stiPdfExportSettings.EmbeddedFonts = embeddedFonts;
		stiPdfExportSettings.StandardPdfFonts = standardPdfFonts;
		stiPdfExportSettings.ExportRtfTextAsImage = exportRtfTextAsImage;
		stiPdfExportSettings.PasswordInputUser = passwordUser;
		stiPdfExportSettings.PasswordInputOwner = passwordOwner;
		stiPdfExportSettings.UserAccessPrivileges = userAccessPrivileges;
		stiPdfExportSettings.KeyLength = keyLength;
		stiPdfExportSettings.UseUnicode = useUnicode;
		ResponseAsPdf(report, stiPdfExportSettings, saveFileDialog);
	}

	public static void ResponseAsPdf(StiReport report, StiPdfExportSettings settings)
	{
		ResponseAsPdf(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsPdf(StiReport report, StiPdfExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiPdfExportService stiPdfExportService = new StiPdfExportService();
		stiPdfExportService.ExportPdf(report, stream, settings);
		ResponseStream(stream, "application/pdf", browserCache: false, StiReportHelper.GetReportFileName(report) + ".pdf", saveFileDialog);
	}

	public static void ResponseAsPdf(StiReport report, IStiPdfDashboardExportSettings settings)
	{
		ResponseAsPdf(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsPdf(StiReport report, IStiPdfDashboardExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		MemoryStream stream = new MemoryStream();
		StiDashboardExport.Export(report, stream, settings);
		ResponseStream(stream, "application/pdf", browserCache: false, StiReportHelper.GetReportFileName(report) + ".pdf", saveFileDialog);
	}

	public static void ResponseAsXps(StiReport report)
	{
		ResponseAsXps(report, StiPagesRange.All);
	}

	public static void ResponseAsXps(StiReport report, StiPagesRange pageRange)
	{
		StiXpsExportSettings stiXpsExportSettings = new StiXpsExportSettings();
		stiXpsExportSettings.PageRange = pageRange;
		ResponseAsXps(report, stiXpsExportSettings);
	}

	public static void ResponseAsXps(StiReport report, StiXpsExportSettings settings)
	{
		ResponseAsXps(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsXps(StiReport report, StiXpsExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiXpsExportService stiXpsExportService = new StiXpsExportService();
		stiXpsExportService.ExportXps(report, stream, settings);
		ResponseStream(stream, "application/vnd.ms-xpsdocument", browserCache: false, StiReportHelper.GetReportFileName(report) + ".xps", saveFileDialog);
	}

	public static void ResponseAsPpt(StiReport report)
	{
		ResponseAsPpt(report, StiPagesRange.All);
	}

	public static void ResponseAsPpt(StiReport report, StiPagesRange pageRange)
	{
		StiPpt2007ExportSettings stiPpt2007ExportSettings = new StiPpt2007ExportSettings();
		stiPpt2007ExportSettings.PageRange = pageRange;
		ResponseAsPpt(report, stiPpt2007ExportSettings, saveFileDialog: true);
	}

	public static void ResponseAsPpt(StiReport report, StiPpt2007ExportSettings settings)
	{
		ResponseAsPpt(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsPpt(StiReport report, StiPpt2007ExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiPpt2007ExportService stiPpt2007ExportService = new StiPpt2007ExportService();
		stiPpt2007ExportService.ExportPowerPoint(report, stream, settings);
		ResponseStream(stream, "application/vnd.ms-powerpoint", browserCache: false, StiReportHelper.GetReportFileName(report) + ".pptx", saveFileDialog);
	}

	public static void ResponseAsHtml(StiReport report)
	{
		ResponseAsHtml(report, StiPagesRange.All);
	}

	public static void ResponseAsHtml(StiReport report, StiPagesRange pageRange)
	{
		ResponseAsHtml(report, pageRange, ImageFormat.Png);
	}

	public static void ResponseAsHtml(StiReport report, StiPagesRange pageRange, ImageFormat imageFormat)
	{
		ResponseAsHtml(report, pageRange, imageFormat, null);
	}

	public static void ResponseAsHtml(StiReport report, StiPagesRange pageRange, ImageFormat imageFormat, StiHtmlImageHost htmlImageHost)
	{
		ResponseAsHtml(report, pageRange, imageFormat, htmlImageHost, saveFileDialog: true);
	}

	public static void ResponseAsHtml(StiReport report, bool saveFileDialog)
	{
		ResponseAsHtml(report, StiPagesRange.All, ImageFormat.Png, null, saveFileDialog);
	}

	public static void ResponseAsHtml(StiReport report, StiHtmlImageHost htmlImageHost, bool saveFileDialog)
	{
		ResponseAsHtml(report, StiPagesRange.All, ImageFormat.Png, htmlImageHost, saveFileDialog);
	}

	public static void ResponseAsHtml(StiReport report, StiPagesRange pageRange, ImageFormat imageFormat, StiHtmlImageHost htmlImageHost, bool saveFileDialog)
	{
		ResponseAsHtml(report, pageRange, imageFormat, htmlImageHost, StiHtmlExportMode.Table, StiHtmlExportQuality.High, Encoding.UTF8, saveFileDialog);
	}

	public static void ResponseAsHtml(StiReport report, StiPagesRange pageRange, ImageFormat imageFormat, StiHtmlImageHost htmlImageHost, StiHtmlExportMode exportMode, StiHtmlExportQuality exportQuality, Encoding encoding, bool saveFileDialog)
	{
		StiHtmlExportSettings stiHtmlExportSettings = new StiHtmlExportSettings();
		stiHtmlExportSettings.PageRange = pageRange;
		stiHtmlExportSettings.ImageFormat = imageFormat;
		stiHtmlExportSettings.ExportMode = exportMode;
		stiHtmlExportSettings.ExportQuality = exportQuality;
		stiHtmlExportSettings.Encoding = encoding;
		ResponseAsHtml(report, stiHtmlExportSettings, htmlImageHost, saveFileDialog);
	}

	public static void ResponseAsHtml(StiReport report, StiHtmlExportSettings settings)
	{
		ResponseAsHtml(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsHtml(StiReport report, StiHtmlExportSettings settings, bool saveFileDialog)
	{
		ResponseAsHtml(report, settings, null, saveFileDialog);
	}

	public static void ResponseAsHtml(StiReport report, StiHtmlExportSettings settings, StiHtmlImageHost htmlImageHost, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiHtmlExportService stiHtmlExportService = new StiHtmlExportService();
		stiHtmlExportService.HtmlImageHost = htmlImageHost;
		stiHtmlExportService.ExportHtml(report, stream, settings);
		ResponseStream(stream, "text/html", browserCache: false, StiReportHelper.GetReportFileName(report) + ".html", saveFileDialog);
	}

	public static void ResponseAsHtml(StiReport report, IStiHtmlDashboardExportSettings settings)
	{
		ResponseAsHtml(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsHtml(StiReport report, IStiHtmlDashboardExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		MemoryStream stream = new MemoryStream();
		StiDashboardExport.Export(report, stream, settings);
		ResponseStream(stream, "text/html", browserCache: false, StiReportHelper.GetReportFileName(report) + ".html", saveFileDialog);
	}

	public static void ResponseAsHtml5(StiReport report)
	{
		ResponseAsHtml5(report, StiPagesRange.All);
	}

	public static void ResponseAsHtml5(StiReport report, StiPagesRange pageRange)
	{
		ResponseAsHtml5(report, pageRange, ImageFormat.Png);
	}

	public static void ResponseAsHtml5(StiReport report, bool saveFileDialog)
	{
		ResponseAsHtml5(report, StiPagesRange.All, ImageFormat.Png, saveFileDialog);
	}

	public static void ResponseAsHtml5(StiReport report, StiPagesRange pageRange, ImageFormat imageFormat)
	{
		ResponseAsHtml5(report, pageRange, imageFormat, saveFileDialog: true);
	}

	public static void ResponseAsHtml5(StiReport report, StiPagesRange pageRange, ImageFormat imageFormat, bool saveFileDialog)
	{
		ResponseAsHtml5(report, pageRange, imageFormat, StiHtmlExportMode.Table, StiHtmlExportQuality.High, Encoding.UTF8, saveFileDialog);
	}

	public static void ResponseAsHtml5(StiReport report, StiPagesRange pageRange, ImageFormat imageFormat, StiHtmlExportMode exportMode, StiHtmlExportQuality exportQuality, Encoding encoding, bool saveFileDialog)
	{
		StiHtmlExportSettings stiHtmlExportSettings = new StiHtmlExportSettings(StiHtmlType.Html5);
		stiHtmlExportSettings.PageRange = pageRange;
		stiHtmlExportSettings.ImageFormat = imageFormat;
		stiHtmlExportSettings.ExportMode = exportMode;
		stiHtmlExportSettings.ExportQuality = exportQuality;
		stiHtmlExportSettings.Encoding = encoding;
		ResponseAsHtml5(report, stiHtmlExportSettings, saveFileDialog);
	}

	public static void ResponseAsHtml5(StiReport report, StiHtmlExportSettings settings)
	{
		ResponseAsHtml5(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsHtml5(StiReport report, StiHtmlExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiHtml5ExportService stiHtml5ExportService = new StiHtml5ExportService();
		stiHtml5ExportService.ExportHtml(report, stream, settings);
		ResponseStream(stream, "text/html", browserCache: false, StiReportHelper.GetReportFileName(report) + ".html", saveFileDialog);
	}

	public static void ResponseAsMht(StiReport report, bool saveFileDialog)
	{
		ResponseAsMht(report, StiPagesRange.All, ImageFormat.Png, StiHtmlExportMode.Table, Encoding.UTF8, saveFileDialog);
	}

	public static void ResponseAsMht(StiReport report, StiPagesRange pageRange, ImageFormat imageFormat, StiHtmlExportMode exportMode, Encoding encoding, bool saveFileDialog)
	{
		StiMhtExportSettings stiMhtExportSettings = new StiMhtExportSettings();
		stiMhtExportSettings.PageRange = pageRange;
		stiMhtExportSettings.ImageFormat = imageFormat;
		stiMhtExportSettings.ExportMode = exportMode;
		stiMhtExportSettings.Encoding = encoding;
		ResponseAsMht(report, stiMhtExportSettings, saveFileDialog);
	}

	public static void ResponseAsMht(StiReport report, StiHtmlExportSettings settings)
	{
		ResponseAsMht(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsMht(StiReport report, StiHtmlExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiMhtExportService stiMhtExportService = new StiMhtExportService();
		stiMhtExportService.ExportMht(report, stream, settings);
		ResponseStream(stream, "message/rfc822", browserCache: false, StiReportHelper.GetReportFileName(report) + ".mht", saveFileDialog);
	}

	public static void ResponseAsText(StiReport report)
	{
		ResponseAsText(report, StiPagesRange.All);
	}

	public static void ResponseAsText(StiReport report, StiPagesRange pageRange)
	{
		StiTxtExportSettings stiTxtExportSettings = new StiTxtExportSettings();
		stiTxtExportSettings.PageRange = pageRange;
		ResponseAsText(report, stiTxtExportSettings);
	}

	public static void ResponseAsText(StiReport report, StiTxtExportSettings settings)
	{
		ResponseAsText(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsText(StiReport report, StiTxtExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiTxtExportService stiTxtExportService = new StiTxtExportService();
		stiTxtExportService.ExportTxt(report, stream, settings);
		ResponseStream(stream, "text/plain", browserCache: false, StiReportHelper.GetReportFileName(report) + ".txt", saveFileDialog);
	}

	public static void ResponseAsRtf(StiReport report)
	{
		ResponseAsRtf(report, StiPagesRange.All);
	}

	public static void ResponseAsRtf(StiReport report, StiPagesRange pageRange)
	{
		ResponseAsRtf(report, pageRange, StiRtfExportMode.Table, saveFileDialog: true);
	}

	public static void ResponseAsRtf(StiReport report, StiPagesRange pageRange, StiRtfExportMode exportMode)
	{
		ResponseAsRtf(report, pageRange, exportMode, saveFileDialog: true);
	}

	public static void ResponseAsRtf(StiReport report, StiPagesRange pageRange, StiRtfExportMode exportMode, bool saveFileDialog)
	{
		StiRtfExportSettings stiRtfExportSettings = new StiRtfExportSettings();
		stiRtfExportSettings.PageRange = pageRange;
		stiRtfExportSettings.ExportMode = exportMode;
		ResponseAsRtf(report, stiRtfExportSettings, saveFileDialog);
	}

	public static void ResponseAsRtf(StiReport report, StiRtfExportSettings settings)
	{
		ResponseAsRtf(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsRtf(StiReport report, StiRtfExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiRtfExportService stiRtfExportService = new StiRtfExportService();
		stiRtfExportService.ExportRtf(report, stream, settings);
		ResponseStream(stream, "application/rtf", browserCache: false, StiReportHelper.GetReportFileName(report) + ".rtf", saveFileDialog);
	}

	public static void ResponseAsWord2007(StiReport report)
	{
		ResponseAsWord2007(report, StiPagesRange.All);
	}

	public static void ResponseAsWord2007(StiReport report, StiPagesRange pageRange)
	{
		StiWord2007ExportSettings stiWord2007ExportSettings = new StiWord2007ExportSettings();
		stiWord2007ExportSettings.PageRange = pageRange;
		ResponseAsWord2007(report, stiWord2007ExportSettings);
	}

	public static void ResponseAsWord2007(StiReport report, StiWord2007ExportSettings settings)
	{
		ResponseAsWord2007(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsWord2007(StiReport report, StiWord2007ExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiWord2007ExportService stiWord2007ExportService = new StiWord2007ExportService();
		stiWord2007ExportService.ExportWord(report, stream, settings);
		ResponseStream(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", browserCache: false, StiReportHelper.GetReportFileName(report) + ".docx", saveFileDialog);
	}

	public static void ResponseAsOdt(StiReport report)
	{
		ResponseAsOdt(report, StiPagesRange.All);
	}

	public static void ResponseAsOdt(StiReport report, StiPagesRange pageRange)
	{
		StiOdtExportSettings stiOdtExportSettings = new StiOdtExportSettings();
		stiOdtExportSettings.PageRange = pageRange;
		ResponseAsOdt(report, stiOdtExportSettings);
	}

	public static void ResponseAsOdt(StiReport report, StiOdtExportSettings settings)
	{
		ResponseAsOdt(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsOdt(StiReport report, StiOdtExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiOdtExportService stiOdtExportService = new StiOdtExportService();
		stiOdtExportService.ExportOdt(report, stream, settings);
		ResponseStream(stream, "application/vnd.oasis.opendocument.text", browserCache: false, StiReportHelper.GetReportFileName(report) + ".odt", saveFileDialog);
	}

	public static void ResponseAsXls(StiReport report)
	{
		ResponseAsXls(report, StiPagesRange.All);
	}

	public static void ResponseAsXls(StiReport report, StiPagesRange pageRange)
	{
		StiExcelExportSettings stiExcelExportSettings = new StiExcelExportSettings();
		stiExcelExportSettings.PageRange = pageRange;
		ResponseAsXls(report, stiExcelExportSettings);
	}

	public static void ResponseAsXls(StiReport report, StiExcelExportSettings settings)
	{
		ResponseAsXls(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsXls(StiReport report, StiExcelExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiExcelExportService stiExcelExportService = new StiExcelExportService();
		stiExcelExportService.ExportExcel(report, stream, settings);
		ResponseStream(stream, "application/vnd.ms-excel", browserCache: false, StiReportHelper.GetReportFileName(report) + ".xls", saveFileDialog);
	}

	public static void ResponseAsXlsXml(StiReport report)
	{
		ResponseAsXlsXml(report, StiPagesRange.All);
	}

	public static void ResponseAsXlsXml(StiReport report, StiPagesRange pageRange)
	{
		StiExcelXmlExportSettings stiExcelXmlExportSettings = new StiExcelXmlExportSettings();
		stiExcelXmlExportSettings.PageRange = pageRange;
		ResponseAsXlsXml(report, stiExcelXmlExportSettings);
	}

	public static void ResponseAsXlsXml(StiReport report, StiExcelExportSettings settings)
	{
		ResponseAsXlsXml(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsXlsXml(StiReport report, StiExcelExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiExcelXmlExportService stiExcelXmlExportService = new StiExcelXmlExportService();
		stiExcelXmlExportService.ExportExcel(report, stream, settings);
		ResponseStream(stream, "application/vnd.ms-excel", browserCache: false, StiReportHelper.GetReportFileName(report) + ".xls", saveFileDialog);
	}

	public static void ResponseAsExcel2007(StiReport report)
	{
		ResponseAsExcel2007(report, StiPagesRange.All);
	}

	public static void ResponseAsExcel2007(StiReport report, StiPagesRange pageRange)
	{
		StiExcel2007ExportSettings stiExcel2007ExportSettings = new StiExcel2007ExportSettings();
		stiExcel2007ExportSettings.PageRange = pageRange;
		ResponseAsExcel2007(report, stiExcel2007ExportSettings);
	}

	public static void ResponseAsExcel2007(StiReport report, StiExcelExportSettings settings)
	{
		ResponseAsExcel2007(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsExcel2007(StiReport report, StiExcelExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiExcel2007ExportService stiExcel2007ExportService = new StiExcel2007ExportService();
		stiExcel2007ExportService.ExportExcel(report, stream, settings);
		ResponseStream(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", browserCache: false, StiReportHelper.GetReportFileName(report) + ".xlsx", saveFileDialog);
	}

	public static void ResponseAsExcel2007(StiReport report, IStiExcelDashboardExportSettings settings)
	{
		ResponseAsExcel2007(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsExcel2007(StiReport report, IStiExcelDashboardExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		MemoryStream stream = new MemoryStream();
		StiDashboardExport.Export(report, stream, settings);
		ResponseStream(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", browserCache: false, StiReportHelper.GetReportFileName(report) + ".xlsx", saveFileDialog);
	}

	public static void ResponseAsOds(StiReport report)
	{
		ResponseAsOds(report, StiPagesRange.All);
	}

	public static void ResponseAsOds(StiReport report, StiPagesRange pageRange)
	{
		StiOdsExportSettings stiOdsExportSettings = new StiOdsExportSettings();
		stiOdsExportSettings.PageRange = pageRange;
		ResponseAsOds(report, stiOdsExportSettings);
	}

	public static void ResponseAsOds(StiReport report, StiOdsExportSettings settings)
	{
		ResponseAsOds(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsOds(StiReport report, StiOdsExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiOdsExportService stiOdsExportService = new StiOdsExportService();
		stiOdsExportService.ExportOds(report, stream, settings);
		ResponseStream(stream, "application/vnd.oasis.opendocument.spreadsheet", browserCache: false, StiReportHelper.GetReportFileName(report) + ".ods", saveFileDialog);
	}

	public static void ResponseAsCsv(StiReport report)
	{
		ResponseAsCsv(report, StiPagesRange.All);
	}

	public static void ResponseAsCsv(StiReport report, StiPagesRange pageRange)
	{
		ResponseAsCsv(report, StiPagesRange.All, ";", Encoding.UTF8);
	}

	public static void ResponseAsCsv(StiReport report, StiPagesRange pageRange, string separator, Encoding encoding)
	{
		StiCsvExportSettings stiCsvExportSettings = new StiCsvExportSettings();
		stiCsvExportSettings.Encoding = encoding;
		stiCsvExportSettings.PageRange = pageRange;
		stiCsvExportSettings.Separator = separator;
		ResponseAsCsv(report, stiCsvExportSettings);
	}

	public static void ResponseAsCsv(StiReport report, StiDataExportSettings settings)
	{
		ResponseAsCsv(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsCsv(StiReport report, StiDataExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiCsvExportService stiCsvExportService = new StiCsvExportService();
		stiCsvExportService.ExportCsv(report, stream, settings);
		ResponseStream(stream, "text/csv", browserCache: false, StiReportHelper.GetReportFileName(report) + ".csv", saveFileDialog);
	}

	public static void ResponseAsCsv(StiReport report, IStiDataDashboardExportSettings settings)
	{
		ResponseAsCsv(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsCsv(StiReport report, IStiDataDashboardExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		settings.DataType = StiDataType.Csv;
		MemoryStream stream = new MemoryStream();
		StiDashboardExport.Export(report, stream, settings);
		ResponseStream(stream, "text/csv", browserCache: false, StiReportHelper.GetReportFileName(report) + ".csv", saveFileDialog);
	}

	public static void ResponseAsXml(StiReport report)
	{
		ResponseAsXml(report, saveFileDialog: true);
	}

	public static void ResponseAsXml(StiReport report, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiXmlExportService stiXmlExportService = new StiXmlExportService();
		stiXmlExportService.ExportXml(report, stream);
		ResponseStream(stream, "application/xml", browserCache: false, StiReportHelper.GetReportFileName(report) + ".xml", saveFileDialog);
	}

	public static void ResponseAsXml(StiReport report, IStiDataDashboardExportSettings settings)
	{
		ResponseAsXml(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsXml(StiReport report, IStiDataDashboardExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		settings.DataType = StiDataType.Xml;
		MemoryStream stream = new MemoryStream();
		StiDashboardExport.Export(report, stream, settings);
		ResponseStream(stream, "application/xml", browserCache: false, StiReportHelper.GetReportFileName(report) + ".xml", saveFileDialog);
	}

	public static void ResponseAsDbf(StiReport report, StiDbfCodePages codePage, bool saveFileDialog)
	{
		ResponseAsDbf(report, StiPagesRange.All, codePage, saveFileDialog);
	}

	public static void ResponseAsDbf(StiReport report, StiPagesRange pageRange, StiDbfCodePages codePage, bool saveFileDialog)
	{
		StiDbfExportSettings stiDbfExportSettings = new StiDbfExportSettings();
		stiDbfExportSettings.CodePage = codePage;
		stiDbfExportSettings.PageRange = pageRange;
		ResponseAsDbf(report, stiDbfExportSettings, saveFileDialog);
	}

	public static void ResponseAsDbf(StiReport report, StiDataExportSettings settings)
	{
		ResponseAsDbf(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsDbf(StiReport report, StiDataExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiDbfExportService stiDbfExportService = new StiDbfExportService();
		stiDbfExportService.ExportDbf(report, stream, settings);
		ResponseStream(stream, "application/dbf", browserCache: false, StiReportHelper.GetReportFileName(report) + ".dbf", saveFileDialog);
	}

	public static void ResponseAsDbf(StiReport report, IStiDataDashboardExportSettings settings)
	{
		ResponseAsDbf(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsDbf(StiReport report, IStiDataDashboardExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		settings.DataType = StiDataType.Dbf;
		MemoryStream stream = new MemoryStream();
		StiDashboardExport.Export(report, stream, settings);
		ResponseStream(stream, "application/dbf", browserCache: false, StiReportHelper.GetReportFileName(report) + ".dbf", saveFileDialog);
	}

	public static void ResponseAsDif(StiReport report)
	{
		ResponseAsDif(report, StiPagesRange.All);
	}

	public static void ResponseAsDif(StiReport report, StiPagesRange pageRange)
	{
		StiDifExportSettings stiDifExportSettings = new StiDifExportSettings();
		stiDifExportSettings.PageRange = pageRange;
		ResponseAsDif(report, stiDifExportSettings, saveFileDialog: true);
	}

	public static void ResponseAsDif(StiReport report, StiDataExportSettings settings)
	{
		ResponseAsDif(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsDif(StiReport report, StiDataExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiDifExportService stiDifExportService = new StiDifExportService();
		stiDifExportService.ExportDif(report, stream, settings);
		ResponseStream(stream, "video/x-dv", browserCache: false, StiReportHelper.GetReportFileName(report) + ".dif", saveFileDialog);
	}

	public static void ResponseAsDif(StiReport report, IStiDataDashboardExportSettings settings)
	{
		ResponseAsDif(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsDif(StiReport report, IStiDataDashboardExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		settings.DataType = StiDataType.Dif;
		MemoryStream stream = new MemoryStream();
		StiDashboardExport.Export(report, stream, settings);
		ResponseStream(stream, "video/x-dv", browserCache: false, StiReportHelper.GetReportFileName(report) + ".dif", saveFileDialog);
	}

	public static void ResponseAsSylk(StiReport report)
	{
		ResponseAsSylk(report, StiPagesRange.All);
	}

	public static void ResponseAsSylk(StiReport report, StiPagesRange pageRange)
	{
		StiSylkExportSettings stiSylkExportSettings = new StiSylkExportSettings();
		stiSylkExportSettings.PageRange = pageRange;
		ResponseAsSylk(report, stiSylkExportSettings, saveFileDialog: true);
	}

	public static void ResponseAsSylk(StiReport report, StiDataExportSettings settings)
	{
		ResponseAsSylk(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsSylk(StiReport report, StiDataExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiSylkExportService stiSylkExportService = new StiSylkExportService();
		stiSylkExportService.ExportSylk(report, stream, settings);
		ResponseStream(stream, "application/excel", browserCache: false, StiReportHelper.GetReportFileName(report) + ".sylk", saveFileDialog);
	}

	public static void ResponseAsSylk(StiReport report, IStiDataDashboardExportSettings settings)
	{
		ResponseAsSylk(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsSylk(StiReport report, IStiDataDashboardExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		settings.DataType = StiDataType.Sylk;
		MemoryStream stream = new MemoryStream();
		StiDashboardExport.Export(report, stream, settings);
		ResponseStream(stream, "application/excel", browserCache: false, StiReportHelper.GetReportFileName(report) + ".sylk", saveFileDialog);
	}

	public static void ResponseAsBmp(StiReport report)
	{
		ResponseAsBmp(report, new StiImageExportSettings(StiImageType.Bmp));
	}

	public static void ResponseAsBmp(StiReport report, StiImageExportSettings settings)
	{
		ResponseAsBmp(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsBmp(StiReport report, StiImageExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiImageExportService stiImageExportService = new StiImageExportService();
		stiImageExportService.ExportImage(report, stream, settings);
		ResponseStream(stream, "image/bmp", browserCache: false, StiReportHelper.GetReportFileName(report) + ".bmp", saveFileDialog);
	}

	public static void ResponseAsBmp(StiReport report, IStiImageDashboardExportSettings settings)
	{
		ResponseAsBmp(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsBmp(StiReport report, IStiImageDashboardExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		settings.ImageType = StiImageType.Bmp;
		MemoryStream stream = new MemoryStream();
		StiDashboardExport.Export(report, stream, settings);
		ResponseStream(stream, "image/bmp", browserCache: false, StiReportHelper.GetReportFileName(report) + ".bmp", saveFileDialog);
	}

	public static void ResponseAsGif(StiReport report)
	{
		ResponseAsGif(report, new StiImageExportSettings(StiImageType.Gif));
	}

	public static void ResponseAsGif(StiReport report, StiImageExportSettings settings)
	{
		ResponseAsGif(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsGif(StiReport report, StiImageExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiImageExportService stiImageExportService = new StiImageExportService();
		stiImageExportService.ExportImage(report, stream, settings);
		ResponseStream(stream, "image/gif", browserCache: false, StiReportHelper.GetReportFileName(report) + ".gif", saveFileDialog);
	}

	public static void ResponseAsGif(StiReport report, IStiImageDashboardExportSettings settings)
	{
		ResponseAsGif(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsGif(StiReport report, IStiImageDashboardExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		settings.ImageType = StiImageType.Gif;
		MemoryStream stream = new MemoryStream();
		StiDashboardExport.Export(report, stream, settings);
		ResponseStream(stream, "image/gif", browserCache: false, StiReportHelper.GetReportFileName(report) + ".gif", saveFileDialog);
	}

	public static void ResponseAsJpeg(StiReport report)
	{
		ResponseAsJpeg(report, new StiImageExportSettings(StiImageType.Jpeg));
	}

	public static void ResponseAsJpeg(StiReport report, StiImageExportSettings settings)
	{
		ResponseAsJpeg(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsJpeg(StiReport report, StiImageExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiImageExportService stiImageExportService = new StiImageExportService();
		stiImageExportService.ExportImage(report, stream, settings);
		ResponseStream(stream, "image/jpeg", browserCache: false, StiReportHelper.GetReportFileName(report) + ".jpg", saveFileDialog);
	}

	public static void ResponseAsJpeg(StiReport report, IStiImageDashboardExportSettings settings)
	{
		ResponseAsJpeg(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsJpeg(StiReport report, IStiImageDashboardExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		settings.ImageType = StiImageType.Jpeg;
		MemoryStream stream = new MemoryStream();
		StiDashboardExport.Export(report, stream, settings);
		ResponseStream(stream, "image/jpeg", browserCache: false, StiReportHelper.GetReportFileName(report) + ".jpg", saveFileDialog);
	}

	public static void ResponseAsPng(StiReport report)
	{
		ResponseAsPng(report, new StiImageExportSettings(StiImageType.Png));
	}

	public static void ResponseAsPng(StiReport report, StiImageExportSettings settings)
	{
		ResponseAsPng(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsPng(StiReport report, StiImageExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiImageExportService stiImageExportService = new StiImageExportService();
		stiImageExportService.ExportImage(report, stream, settings);
		ResponseStream(stream, "image/png", browserCache: false, StiReportHelper.GetReportFileName(report) + ".png", saveFileDialog);
	}

	public static void ResponseAsPng(StiReport report, IStiImageDashboardExportSettings settings)
	{
		ResponseAsPng(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsPng(StiReport report, IStiImageDashboardExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		settings.ImageType = StiImageType.Png;
		MemoryStream stream = new MemoryStream();
		StiDashboardExport.Export(report, stream, settings);
		ResponseStream(stream, "image/png", browserCache: false, StiReportHelper.GetReportFileName(report) + ".png", saveFileDialog);
	}

	public static void ResponseAsPcx(StiReport report)
	{
		ResponseAsPcx(report, new StiImageExportSettings(StiImageType.Pcx));
	}

	public static void ResponseAsPcx(StiReport report, StiImageExportSettings settings)
	{
		ResponseAsPcx(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsPcx(StiReport report, StiImageExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiImageExportService stiImageExportService = new StiImageExportService();
		stiImageExportService.ExportImage(report, stream, settings);
		ResponseStream(stream, "image/x-pcx", browserCache: false, StiReportHelper.GetReportFileName(report) + ".pcx", saveFileDialog);
	}

	public static void ResponseAsPcx(StiReport report, IStiImageDashboardExportSettings settings)
	{
		ResponseAsPcx(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsPcx(StiReport report, IStiImageDashboardExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		settings.ImageType = StiImageType.Pcx;
		MemoryStream stream = new MemoryStream();
		StiDashboardExport.Export(report, stream, settings);
		ResponseStream(stream, "image/x-pcx", browserCache: false, StiReportHelper.GetReportFileName(report) + ".pcx", saveFileDialog);
	}

	public static void ResponseAsTiff(StiReport report)
	{
		ResponseAsTiff(report, new StiImageExportSettings(StiImageType.Tiff));
	}

	public static void ResponseAsTiff(StiReport report, StiImageExportSettings settings)
	{
		ResponseAsTiff(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsTiff(StiReport report, StiImageExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiImageExportService stiImageExportService = new StiImageExportService();
		stiImageExportService.ExportImage(report, stream, settings);
		ResponseStream(stream, "image/tiff", browserCache: false, StiReportHelper.GetReportFileName(report) + ".tiff", saveFileDialog);
	}

	public static void ResponseAsTiff(StiReport report, IStiImageDashboardExportSettings settings)
	{
		ResponseAsTiff(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsTiff(StiReport report, IStiImageDashboardExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		settings.ImageType = StiImageType.Tiff;
		MemoryStream stream = new MemoryStream();
		StiDashboardExport.Export(report, stream, settings);
		ResponseStream(stream, "image/tiff", browserCache: false, StiReportHelper.GetReportFileName(report) + ".tiff", saveFileDialog);
	}

	public static void ResponseAsMetafile(StiReport report)
	{
		ResponseAsMetafile(report, new StiImageExportSettings(StiImageType.Emf));
	}

	public static void ResponseAsMetafile(StiReport report, StiImageExportSettings settings)
	{
		ResponseAsMetafile(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsMetafile(StiReport report, StiImageExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiImageExportService stiImageExportService = new StiImageExportService();
		stiImageExportService.ExportImage(report, stream, settings);
		ResponseStream(stream, "image/x-emf", browserCache: false, StiReportHelper.GetReportFileName(report) + ".emf", saveFileDialog);
	}

	public static void ResponseAsMetafile(StiReport report, IStiImageDashboardExportSettings settings)
	{
		ResponseAsMetafile(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsMetafile(StiReport report, IStiImageDashboardExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		settings.ImageType = StiImageType.Emf;
		MemoryStream stream = new MemoryStream();
		StiDashboardExport.Export(report, stream, settings);
		ResponseStream(stream, "image/x-emf", browserCache: false, StiReportHelper.GetReportFileName(report) + ".emf", saveFileDialog);
	}

	public static void ResponseAsSvg(StiReport report)
	{
		ResponseAsSvg(report, StiPagesRange.All);
	}

	public static void ResponseAsSvg(StiReport report, StiPagesRange pageRange)
	{
		StiImageExportSettings stiImageExportSettings = new StiImageExportSettings(StiImageType.Svg);
		stiImageExportSettings.PageRange = pageRange;
		ResponseAsSvg(report, stiImageExportSettings, saveFileDialog: true);
	}

	public static void ResponseAsSvg(StiReport report, StiImageExportSettings settings)
	{
		ResponseAsSvg(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsSvg(StiReport report, StiImageExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiImageExportService stiImageExportService = new StiImageExportService();
		stiImageExportService.ExportImage(report, stream, settings);
		ResponseStream(stream, "image/svg+xml", browserCache: false, StiReportHelper.GetReportFileName(report) + ".svg", saveFileDialog);
	}

	public static void ResponseAsSvg(StiReport report, IStiImageDashboardExportSettings settings)
	{
		ResponseAsSvg(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsSvg(StiReport report, IStiImageDashboardExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		settings.ImageType = StiImageType.Emf;
		MemoryStream stream = new MemoryStream();
		StiDashboardExport.Export(report, stream, settings);
		ResponseStream(stream, "image/svg+xml", browserCache: false, StiReportHelper.GetReportFileName(report) + ".svg", saveFileDialog);
	}

	public static void ResponseAsSvgz(StiReport report)
	{
		ResponseAsSvgz(report, StiPagesRange.All);
	}

	public static void ResponseAsSvgz(StiReport report, StiPagesRange pageRange)
	{
		StiImageExportSettings stiImageExportSettings = new StiImageExportSettings(StiImageType.Svgz);
		stiImageExportSettings.PageRange = pageRange;
		ResponseAsSvgz(report, stiImageExportSettings, saveFileDialog: true);
	}

	public static void ResponseAsSvgz(StiReport report, StiImageExportSettings settings)
	{
		ResponseAsSvgz(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsSvgz(StiReport report, StiImageExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		MemoryStream stream = new MemoryStream();
		StiImageExportService stiImageExportService = new StiImageExportService();
		stiImageExportService.ExportImage(report, stream, settings);
		ResponseStream(stream, "image/svg+xml", browserCache: false, StiReportHelper.GetReportFileName(report) + ".svgz", saveFileDialog);
	}

	public static void ResponseAsSvgz(StiReport report, IStiImageDashboardExportSettings settings)
	{
		ResponseAsSvgz(report, settings, saveFileDialog: true);
	}

	public static void ResponseAsSvgz(StiReport report, IStiImageDashboardExportSettings settings, bool saveFileDialog)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		settings.ImageType = StiImageType.Svgz;
		MemoryStream stream = new MemoryStream();
		StiDashboardExport.Export(report, stream, settings);
		ResponseStream(stream, "image/svg+xml", browserCache: false, StiReportHelper.GetReportFileName(report) + ".svgz", saveFileDialog);
	}

	public static void PrintAsPdf(StiReport report)
	{
		PrintAsPdf(report, StiPagesRange.All);
	}

	public static void PrintAsPdf(StiReport report, StiPagesRange pageRange)
	{
		StiPdfExportSettings stiPdfExportSettings = new StiPdfExportSettings();
		stiPdfExportSettings.PageRange = pageRange;
		PrintAsPdf(report, stiPdfExportSettings);
	}

	public static void PrintAsPdf(StiReport report, StiPdfExportSettings settings)
	{
		settings.AutoPrintMode = StiPdfAutoPrintMode.Dialog;
		ResponseAsPdf(report, settings, saveFileDialog: false);
	}

	public static void PrintAsHtml(StiReport report)
	{
		PrintAsHtml(report, StiPagesRange.All);
	}

	public static void PrintAsHtml(StiReport report, StiPagesRange pageRange)
	{
		StiHtmlExportSettings stiHtmlExportSettings = new StiHtmlExportSettings();
		stiHtmlExportSettings.PageRange = pageRange;
		PrintAsHtml(report, stiHtmlExportSettings);
	}

	public static void PrintAsHtml(StiReport report, StiHtmlExportSettings settings)
	{
		if (report.CookieContainer == null || report.CookieContainer.Count == 0)
		{
			report.CookieContainer = StiReportHelper.GetCookieContainer();
		}
		if (!report.IsRendered)
		{
			report.Render(showProgress: false);
		}
		settings.ChartType = StiHtmlChartType.Image;
		MemoryStream memoryStream = new MemoryStream();
		StiHtmlExportService stiHtmlExportService = new StiHtmlExportService();
		stiHtmlExportService.ExportHtml(report, memoryStream, settings);
		memoryStream.Position = 0L;
		using (StreamReader streamReader = new StreamReader(memoryStream, Encoding.UTF8))
		{
			string text = streamReader.ReadToEnd();
			text = text.Replace("<body", "<body onload='window.print()'");
			memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(text));
		}
		ResponseStream(memoryStream, "text/html", browserCache: false);
	}

	public static void ResponseString(string data, string contentType, bool browserCache, string fileName = null, bool showSaveFileDialog = true)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(data);
		ResponseBuffer(bytes, contentType, browserCache, fileName, showSaveFileDialog);
	}

	public static void ResponseStream(Stream stream, string contentType, bool browserCache, string fileName = null, bool showSaveFileDialog = true)
	{
		ResponseBuffer(((MemoryStream)stream).ToArray(), contentType, browserCache, fileName, showSaveFileDialog);
	}

	public static void ResponseBuffer(byte[] buffer, string contentType, bool browserCache, string fileName = null, bool showSaveFileDialog = true)
	{
		HttpResponse val = null;
		try
		{
			val = HttpContext.Current.Response;
			val.StatusCode = 200;
		}
		catch
		{
			return;
		}
		if (StiOptions.Web.ClearResponseHeaders)
		{
			val.ClearHeaders();
		}
		val.Buffer = true;
		val.ClearContent();
		if (browserCache)
		{
			val.Cache.SetExpires(DateTime.Now.AddYears(1));
			val.Cache.SetCacheability((HttpCacheability)4);
		}
		else if (!string.IsNullOrEmpty(fileName) && StiOptions.Web.ResponseCacheTimeout > 0)
		{
			val.Cache.SetExpires(DateTime.Now.AddSeconds(StiOptions.Web.ResponseCacheTimeout));
			val.Cache.SetCacheability((HttpCacheability)4);
		}
		else
		{
			val.Cache.SetExpires(DateTime.MinValue);
			val.Cache.SetCacheability((HttpCacheability)1);
		}
		val.ContentType = contentType;
		val.ContentEncoding = Encoding.UTF8;
		val.AppendHeader("Content-Length", buffer.Length.ToString());
		if (!string.IsNullOrEmpty(fileName))
		{
			string arg = (showSaveFileDialog ? "attachment" : "inline");
			string text = string.Format("{0};filename=\"{1}\";filename*=UTF-8''{2}", arg, fileName, HttpUtility.UrlEncode(fileName).Replace("+", "%20"));
			val.AppendHeader("Content-Disposition", text);
		}
		val.BinaryWrite(buffer);
		if (StiOptions.Web.AllowGCCollect)
		{
			GC.Collect();
			GC.Collect();
		}
		try
		{
			if (StiOptions.Web.AllowUseResponseFlush)
			{
				val.Flush();
			}
			if (StiOptions.Web.AllowUseResponseEnd)
			{
				val.End();
				return;
			}
			HttpContext.Current.Response.SuppressContent = true;
			HttpContext.Current.ApplicationInstance.CompleteRequest();
		}
		catch (ThreadAbortException)
		{
		}
		catch
		{
		}
	}
}
