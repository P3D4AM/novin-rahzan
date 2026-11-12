using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Export;
using Stimulsoft.Report.Export;

namespace Stimulsoft.Report.Web;

internal class StiExportsHelper
{
	public static string GetReportFileContentType(StiRequestParams requestParams)
	{
		switch (requestParams.ExportFormat)
		{
		case StiExportFormat.Document:
			switch (GetExportDocumentType(requestParams))
			{
			case StiDocumentType.Mdc:
				return "text/xml";
			case StiDocumentType.Mdz:
				return "application/zip";
			case StiDocumentType.Mdx:
				return "application/octet-stream";
			}
			break;
		case StiExportFormat.Pdf:
			return "application/pdf";
		case StiExportFormat.Xps:
			return "application/vnd.ms-xpsdocument";
		case StiExportFormat.Ppt2007:
			return "application/vnd.ms-powerpoint";
		case StiExportFormat.Text:
			return "text/plain";
		case StiExportFormat.Rtf:
			return "application/rtf";
		case StiExportFormat.Word2007:
			return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
		case StiExportFormat.Odt:
			return "application/vnd.oasis.opendocument.text";
		case StiExportFormat.Ods:
			return "application/vnd.oasis.opendocument.spreadsheet";
		case StiExportFormat.Html:
		case StiExportFormat.Html5:
			return "text/html";
		case StiExportFormat.Mht:
			return "message/rfc822";
		case StiExportFormat.Excel:
		case StiExportFormat.ExcelXml:
			return "application/vnd.ms-excel";
		case StiExportFormat.Excel2007:
			return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
		case StiExportFormat.Csv:
			return "text/csv";
		case StiExportFormat.Dbf:
			return "application/dbf";
		case StiExportFormat.Xml:
			return "application/xml";
		case StiExportFormat.Json:
			return "application/json";
		case StiExportFormat.Dif:
			return "video/x-dv";
		case StiExportFormat.Sylk:
			return "application/excel";
		case StiExportFormat.ImageBmp:
			return "image/bmp";
		case StiExportFormat.ImageGif:
			return "image/gif";
		case StiExportFormat.ImageJpeg:
			return "image/jpeg";
		case StiExportFormat.ImagePcx:
			return "image/x-pcx";
		case StiExportFormat.ImagePng:
			return "image/png";
		case StiExportFormat.ImageTiff:
			return "image/tiff";
		case StiExportFormat.ImageEmf:
			return "image/x-emf";
		case StiExportFormat.ImageSvg:
		case StiExportFormat.ImageSvgz:
			return "image/svg+xml";
		}
		if (requestParams.ExportFormat == StiExportFormat.Image && requestParams.ExportSettings != null && requestParams.ExportSettings.Contains("ImageType"))
		{
			switch (requestParams.ExportSettings["ImageType"].ToString())
			{
			case "Bmp":
				return "image/bmp";
			case "Gif":
				return "image/gif";
			case "Jpeg":
				return "image/jpeg";
			case "Pcx":
				return "image/x-pcx";
			case "Png":
				return "image/png";
			case "Tiff":
				return "image/tiff";
			case "Emf":
				return "image/x-emf";
			case "Svg":
			case "Svgz":
				return "image/svg+xml";
			}
		}
		return "text/plain";
	}

	public static string GetReportFileName(StiRequestParams requestParams, StiReport report)
	{
		string text = ((requestParams.Viewer.ReportType != StiReportType.Dashboard) ? StiReportHelper.GetReportFileName(report) : ((!string.IsNullOrEmpty(requestParams.Viewer.ElementName)) ? requestParams.Viewer.ElementName : (string.IsNullOrWhiteSpace(report.Pages[requestParams.Viewer.PageNumber].Alias) ? report.Pages[requestParams.Viewer.PageNumber].Name : report.Pages[requestParams.Viewer.PageNumber].Alias)));
		text += ".";
		switch (requestParams.ExportFormat)
		{
		case StiExportFormat.Document:
			return text + GetExportDocumentType(requestParams).ToString().ToLower();
		case StiExportFormat.Excel:
			return text + "xls";
		case StiExportFormat.Excel2007:
			return text + "xlsx";
		case StiExportFormat.ExcelXml:
			return text + "xml.xls";
		case StiExportFormat.Ppt2007:
			return text + "pptx";
		case StiExportFormat.Text:
			return text + "txt";
		case StiExportFormat.Word2007:
			return text + "docx";
		case StiExportFormat.Html5:
			return text + "html";
		default:
		{
			string text2 = requestParams.ExportFormat.ToString().ToLower().Replace("image", "");
			if (requestParams.ExportSettings != null && requestParams.ExportSettings.Contains("ImageType"))
			{
				if (string.IsNullOrEmpty(text2))
				{
					text2 = requestParams.ExportSettings["ImageType"].ToString().ToLower();
				}
				if (Convert.ToBoolean(requestParams.ExportSettings["CompressToArchive"]))
				{
					text2 = "zip";
				}
			}
			if (text2 == "data" && requestParams.ExportSettings != null && requestParams.ExportSettings.Contains("DataType"))
			{
				text2 = requestParams.ExportSettings["DataType"].ToString().ToLower();
			}
			return text + text2;
		}
		}
	}

	private static StiDocumentType GetExportDocumentType(StiRequestParams requestParams)
	{
		return ((requestParams.ExportSettings["Format"] != null) ? requestParams.ExportSettings["Format"].ToString().ToLower() : null) switch
		{
			"mdc" => StiDocumentType.Mdc, 
			"mdz" => StiDocumentType.Mdz, 
			"mdx" => StiDocumentType.Mdx, 
			_ => StiDocumentType.Mdc, 
		};
	}

	public static StiExportSettings GetExportSettings(StiRequestParams requestParams)
	{
		if (requestParams.Action == StiAction.PrintReport)
		{
			if (requestParams.Viewer.PrintAction == StiPrintAction.PrintPdf)
			{
				return new StiPdfExportSettings();
			}
			return new StiHtmlExportSettings();
		}
		switch (requestParams.ExportFormat)
		{
		case StiExportFormat.Pdf:
			return GetPdfExportSettings(requestParams.ExportSettings);
		case StiExportFormat.Xps:
			return GetXpsExportSettings(requestParams.ExportSettings);
		case StiExportFormat.Ppt2007:
			return GetPpt2007ExportSettings(requestParams.ExportSettings);
		case StiExportFormat.Text:
			return GetTextExportSettings(requestParams.ExportSettings);
		case StiExportFormat.Rtf:
			return GetRtfExportSettings(requestParams.ExportSettings);
		case StiExportFormat.Word2007:
			return GetWord2007ExportSettings(requestParams.ExportSettings);
		case StiExportFormat.Odt:
			return GetOdtExportSettings(requestParams.ExportSettings);
		case StiExportFormat.Ods:
			return GetOdsExportSettings(requestParams.ExportSettings);
		case StiExportFormat.Mht:
		case StiExportFormat.Html:
		case StiExportFormat.Html5:
			return GetHtmlExportSettings(requestParams.ExportSettings);
		case StiExportFormat.Excel:
		case StiExportFormat.ExcelXml:
		case StiExportFormat.Excel2007:
			return GetExcelExportSettings(requestParams.ExportSettings);
		case StiExportFormat.Xml:
		case StiExportFormat.Csv:
		case StiExportFormat.Dif:
		case StiExportFormat.Sylk:
		case StiExportFormat.Dbf:
		case StiExportFormat.Json:
			return GetDataExportSettings(requestParams.ExportSettings);
		case StiExportFormat.ImageGif:
		case StiExportFormat.ImageBmp:
		case StiExportFormat.ImagePng:
		case StiExportFormat.ImageTiff:
		case StiExportFormat.ImageJpeg:
		case StiExportFormat.ImagePcx:
		case StiExportFormat.ImageEmf:
		case StiExportFormat.ImageSvg:
		case StiExportFormat.ImageSvgz:
			return GetImageExportSettings(requestParams.ExportSettings);
		default:
			return null;
		}
	}

	public static IStiDashboardExportSettings GetDashboardExportSettings(StiRequestParams requestParams)
	{
		return requestParams.ExportFormat switch
		{
			StiExportFormat.Pdf => GetPdfDashboardExportSettings(requestParams.ExportSettings), 
			StiExportFormat.Excel2007 => GetExcelDashboardExportSettings(requestParams.ExportSettings), 
			StiExportFormat.Data => GetDataDashboardExportSettings(requestParams.ExportSettings), 
			StiExportFormat.Image => GetImageDashboardExportSettings(requestParams.ExportSettings), 
			StiExportFormat.Html => GetHtmlDashboardExportSettings(requestParams.ExportSettings), 
			_ => null, 
		};
	}

	private static Hashtable GetExportSettingsValues(object settings)
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		Hashtable hashtable = new Hashtable();
		Type type = settings.GetType();
		PropertyInfo[] properties = type.GetProperties();
		PropertyInfo[] array = properties;
		foreach (PropertyInfo propertyInfo in array)
		{
			if (propertyInfo.Name != "CompanyString" && propertyInfo.Name != "LastModifiedString")
			{
				object obj = propertyInfo.GetValue(settings, null);
				if (obj is StiPagesRange)
				{
					obj = ((((StiPagesRange)obj).RangeType == StiRangeType.CurrentPage) ? ((StiPagesRange)obj).CurrentPage.ToString() : ((((StiPagesRange)obj).RangeType == StiRangeType.Pages) ? ((StiPagesRange)obj).PageRanges : ((StiPagesRange)obj).RangeType.ToString()));
				}
				if (obj is Encoding)
				{
					obj = ((Encoding)obj).CodePage.ToString();
				}
				if (obj is ImageFormat)
				{
					obj = ((object)(ImageFormat)obj).ToString();
				}
				hashtable[propertyInfo.Name] = obj;
			}
		}
		return hashtable;
	}

	public static Hashtable GetDefaultExportSettings(StiDefaultExportSettings exportSettings)
	{
		StiExportSettings[] array = new StiExportSettings[12]
		{
			exportSettings.ExportToPdf, exportSettings.ExportToXps, exportSettings.ExportToPowerPoint, exportSettings.ExportToHtml, exportSettings.ExportToText, exportSettings.ExportToRtf, exportSettings.ExportToWord2007, exportSettings.ExportToOpenDocumentWriter, exportSettings.ExportToExcel, exportSettings.ExportToOpenDocumentCalc,
			exportSettings.ExportToData, exportSettings.ExportToImage
		};
		Hashtable hashtable = new Hashtable();
		StiExportSettings[] array2 = array;
		foreach (StiExportSettings stiExportSettings in array2)
		{
			hashtable[stiExportSettings.GetType().Name] = GetExportSettingsValues(stiExportSettings);
		}
		if (StiDashboardExportAssembly.IsAssemblyLoaded)
		{
			List<StiExportFormat> list = new List<StiExportFormat>
			{
				StiExportFormat.Pdf,
				StiExportFormat.Excel2007,
				StiExportFormat.Data,
				StiExportFormat.Image,
				StiExportFormat.Html,
				StiExportFormat.Html5
			};
			foreach (StiExportFormat item in list)
			{
				object settings = StiInvokeMethodsHelper.InvokeStaticMethod("Stimulsoft.Dashboard.Export", "Helpers.StiExportSettingsHelper", "GetDashboardExportSettings", new object[1] { item }, new Type[1] { typeof(StiExportFormat) });
				hashtable["Dashboard" + item] = GetExportSettingsValues(settings);
			}
		}
		return hashtable;
	}

	private static Encoding GetEncoding(string value)
	{
		if (char.IsDigit(value[0]))
		{
			return Encoding.GetEncoding(Convert.ToInt32(value));
		}
		return Encoding.GetEncoding(value);
	}

	private static IStiPdfDashboardExportSettings GetPdfDashboardExportSettings(Hashtable jsSettings)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		IStiPdfDashboardExportSettings stiPdfDashboardExportSettings = StiInvokeMethodsHelper.InvokeStaticMethod("Stimulsoft.Dashboard.Export", "Helpers.StiExportSettingsHelper", "GetDashboardExportSettings", new object[1] { StiExportFormat.Pdf }, new Type[1] { typeof(StiExportFormat) }) as IStiPdfDashboardExportSettings;
		foreach (string key in jsSettings.Keys)
		{
			string text2 = ((jsSettings[key] != null) ? jsSettings[key].ToString() : null);
			switch (key)
			{
			case "PaperSize":
				stiPdfDashboardExportSettings.PaperSize = (PaperKind)Enum.Parse(typeof(PaperKind), text2);
				break;
			case "Orientation":
				stiPdfDashboardExportSettings.Orientation = (StiPageOrientation)Enum.Parse(typeof(StiPageOrientation), text2);
				break;
			case "ImageQuality":
			{
				int result = 200;
				if (int.TryParse(text2, out result))
				{
					stiPdfDashboardExportSettings.ImageQuality = result;
				}
				break;
			}
			}
		}
		return stiPdfDashboardExportSettings;
	}

	private static IStiDataDashboardExportSettings GetDataDashboardExportSettings(Hashtable jsSettings)
	{
		IStiDataDashboardExportSettings stiDataDashboardExportSettings = StiInvokeMethodsHelper.InvokeStaticMethod("Stimulsoft.Dashboard.Export", "Helpers.StiExportSettingsHelper", "GetDashboardExportSettings", new object[1] { StiExportFormat.Data }, new Type[1] { typeof(StiExportFormat) }) as IStiDataDashboardExportSettings;
		foreach (string key in jsSettings.Keys)
		{
			string value = ((jsSettings[key] != null) ? jsSettings[key].ToString() : null);
			if (key == "DataType")
			{
				stiDataDashboardExportSettings.DataType = (StiDataType)Enum.Parse(typeof(StiDataType), value);
			}
		}
		return stiDataDashboardExportSettings;
	}

	private static IStiExcelDashboardExportSettings GetExcelDashboardExportSettings(Hashtable jsSettings)
	{
		IStiExcelDashboardExportSettings stiExcelDashboardExportSettings = StiInvokeMethodsHelper.InvokeStaticMethod("Stimulsoft.Dashboard.Export", "Helpers.StiExportSettingsHelper", "GetDashboardExportSettings", new object[1] { StiExportFormat.Excel2007 }, new Type[1] { typeof(StiExportFormat) }) as IStiExcelDashboardExportSettings;
		foreach (string key in jsSettings.Keys)
		{
			string text2 = ((jsSettings[key] != null) ? jsSettings[key].ToString() : null);
			if (!(key == "ImageQuality"))
			{
				if (key == "ExportDataOnly")
				{
					stiExcelDashboardExportSettings.ExportDataOnly = Convert.ToBoolean(text2);
				}
				continue;
			}
			int result = 200;
			if (int.TryParse(text2, out result))
			{
				stiExcelDashboardExportSettings.ImageQuality = result;
			}
		}
		return stiExcelDashboardExportSettings;
	}

	private static IStiImageDashboardExportSettings GetImageDashboardExportSettings(Hashtable jsSettings)
	{
		IStiImageDashboardExportSettings stiImageDashboardExportSettings = StiInvokeMethodsHelper.InvokeStaticMethod("Stimulsoft.Dashboard.Export", "Helpers.StiExportSettingsHelper", "GetDashboardExportSettings", new object[1] { StiExportFormat.Image }, new Type[1] { typeof(StiExportFormat) }) as IStiImageDashboardExportSettings;
		foreach (string key in jsSettings.Keys)
		{
			string text2 = ((jsSettings[key] != null) ? jsSettings[key].ToString() : null);
			if (!(key == "ImageType"))
			{
				if (key == "Scale")
				{
					int result = 100;
					if (int.TryParse(text2, out result))
					{
						stiImageDashboardExportSettings.Scale = result;
					}
				}
			}
			else
			{
				stiImageDashboardExportSettings.ImageType = (StiImageType)Enum.Parse(typeof(StiImageType), text2);
			}
		}
		return stiImageDashboardExportSettings;
	}

	private static IStiHtmlDashboardExportSettings GetHtmlDashboardExportSettings(Hashtable jsSettings)
	{
		IStiHtmlDashboardExportSettings stiHtmlDashboardExportSettings = StiInvokeMethodsHelper.InvokeStaticMethod("Stimulsoft.Dashboard.Export", "Helpers.StiExportSettingsHelper", "GetDashboardExportSettings", new object[1] { StiExportFormat.Html }, new Type[1] { typeof(StiExportFormat) }) as IStiHtmlDashboardExportSettings;
		foreach (string key in jsSettings.Keys)
		{
			string text2 = ((jsSettings[key] != null) ? jsSettings[key].ToString() : null);
			switch (key)
			{
			case "ImageQuality":
			{
				int result2 = 200;
				if (int.TryParse(text2, out result2))
				{
					stiHtmlDashboardExportSettings.ImageQuality = result2;
				}
				break;
			}
			case "Scale":
			{
				int result = 100;
				if (int.TryParse(text2, out result))
				{
					stiHtmlDashboardExportSettings.Scale = result;
				}
				break;
			}
			case "EnableAnimation":
				stiHtmlDashboardExportSettings.EnableAnimation = Convert.ToBoolean(text2);
				break;
			}
		}
		return stiHtmlDashboardExportSettings;
	}

	private static StiPdfExportSettings GetPdfExportSettings(Hashtable jsSettings)
	{
		StiPdfExportSettings stiPdfExportSettings = new StiPdfExportSettings();
		foreach (string key in jsSettings.Keys)
		{
			string text2 = ((jsSettings[key] != null) ? jsSettings[key].ToString() : null);
			switch (key)
			{
			case "PageRange":
			case "PagesRange":
				stiPdfExportSettings.PageRange = new StiPagesRange(text2);
				break;
			case "ImageQuality":
				stiPdfExportSettings.ImageQuality = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			case "ImageResolution":
				stiPdfExportSettings.ImageResolution = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			case "EmbeddedFonts":
				stiPdfExportSettings.EmbeddedFonts = Convert.ToBoolean(text2);
				break;
			case "StandardPdfFonts":
				stiPdfExportSettings.StandardPdfFonts = Convert.ToBoolean(text2);
				break;
			case "Compressed":
				stiPdfExportSettings.Compressed = Convert.ToBoolean(text2);
				break;
			case "ExportRtfTextAsImage":
				stiPdfExportSettings.ExportRtfTextAsImage = Convert.ToBoolean(text2);
				break;
			case "PasswordInputUser":
				stiPdfExportSettings.PasswordInputUser = text2;
				break;
			case "PasswordInputOwner":
				stiPdfExportSettings.PasswordInputOwner = text2;
				break;
			case "UserAccessPrivileges":
				stiPdfExportSettings.UserAccessPrivileges = (StiUserAccessPrivileges)Enum.Parse(typeof(StiUserAccessPrivileges), text2);
				break;
			case "KeyLength":
				stiPdfExportSettings.KeyLength = (StiPdfEncryptionKeyLength)Enum.Parse(typeof(StiPdfEncryptionKeyLength), text2);
				break;
			case "UseUnicode":
				stiPdfExportSettings.UseUnicode = Convert.ToBoolean(text2);
				break;
			case "UseDigitalSignature":
				stiPdfExportSettings.UseDigitalSignature = Convert.ToBoolean(text2);
				break;
			case "GetCertificateFromCryptoUI":
				stiPdfExportSettings.GetCertificateFromCryptoUI = Convert.ToBoolean(text2);
				break;
			case "SubjectNameString":
				stiPdfExportSettings.SubjectNameString = text2;
				break;
			case "UseLocalMachineCertificates":
				stiPdfExportSettings.UseLocalMachineCertificates = Convert.ToBoolean(text2);
				break;
			case "CreatorString":
				stiPdfExportSettings.CreatorString = text2;
				break;
			case "KeywordsString":
				stiPdfExportSettings.KeywordsString = text2;
				break;
			case "ImageCompressionMethod":
				stiPdfExportSettings.ImageCompressionMethod = (StiPdfImageCompressionMethod)Enum.Parse(typeof(StiPdfImageCompressionMethod), text2);
				break;
			case "ImageFormat":
				stiPdfExportSettings.ImageFormat = (StiImageFormat)Enum.Parse(typeof(StiImageFormat), text2);
				break;
			case "DitheringType":
				stiPdfExportSettings.DitheringType = (StiMonochromeDitheringType)Enum.Parse(typeof(StiMonochromeDitheringType), text2);
				break;
			case "PdfACompliance":
				stiPdfExportSettings.PdfComplianceMode = (Convert.ToBoolean(text2) ? StiPdfComplianceMode.A1 : StiPdfComplianceMode.None);
				break;
			case "AutoPrintMode":
				stiPdfExportSettings.AutoPrintMode = (StiPdfAutoPrintMode)Enum.Parse(typeof(StiPdfAutoPrintMode), text2);
				break;
			case "AllowEditable":
				stiPdfExportSettings.AllowEditable = (StiPdfAllowEditable)Enum.Parse(typeof(StiPdfAllowEditable), text2);
				break;
			case "CertificateThumbprint":
				stiPdfExportSettings.CertificateThumbprint = text2;
				break;
			case "ImageResolutionMode":
				stiPdfExportSettings.ImageResolutionMode = (StiImageResolutionMode)Enum.Parse(typeof(StiImageResolutionMode), text2);
				break;
			}
		}
		return stiPdfExportSettings;
	}

	private static StiXpsExportSettings GetXpsExportSettings(Hashtable jsSettings)
	{
		StiXpsExportSettings stiXpsExportSettings = new StiXpsExportSettings();
		foreach (string key in jsSettings.Keys)
		{
			string text2 = ((jsSettings[key] != null) ? jsSettings[key].ToString() : null);
			switch (key)
			{
			case "PageRange":
			case "PagesRange":
				stiXpsExportSettings.PageRange = new StiPagesRange(text2);
				break;
			case "ImageResolution":
				stiXpsExportSettings.ImageResolution = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			case "ImageQuality":
				stiXpsExportSettings.ImageQuality = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			case "ExportRtfTextAsImage":
				stiXpsExportSettings.ExportRtfTextAsImage = Convert.ToBoolean(text2);
				break;
			}
		}
		return stiXpsExportSettings;
	}

	private static StiPpt2007ExportSettings GetPpt2007ExportSettings(Hashtable jsSettings)
	{
		StiPpt2007ExportSettings stiPpt2007ExportSettings = new StiPpt2007ExportSettings();
		foreach (string key in jsSettings.Keys)
		{
			string text2 = ((jsSettings[key] != null) ? jsSettings[key].ToString() : null);
			switch (key)
			{
			case "PageRange":
			case "PagesRange":
				stiPpt2007ExportSettings.PageRange = new StiPagesRange(text2);
				break;
			case "ImageResolution":
				stiPpt2007ExportSettings.ImageResolution = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			case "ImageQuality":
				stiPpt2007ExportSettings.ImageQuality = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			}
		}
		return stiPpt2007ExportSettings;
	}

	private static StiHtmlExportSettings GetHtmlExportSettings(Hashtable jsSettings)
	{
		StiHtmlExportSettings stiHtmlExportSettings = new StiHtmlExportSettings();
		foreach (string key in jsSettings.Keys)
		{
			string text2 = ((jsSettings[key] != null) ? jsSettings[key].ToString() : null);
			switch (key)
			{
			case "PageRange":
			case "PagesRange":
				stiHtmlExportSettings.PageRange = new StiPagesRange(text2);
				break;
			case "HtmlType":
				stiHtmlExportSettings.HtmlType = (StiHtmlType)Enum.Parse(typeof(StiHtmlType), text2);
				break;
			case "ImageQuality":
				stiHtmlExportSettings.ImageQuality = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			case "ImageResolution":
				stiHtmlExportSettings.ImageResolution = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			case "Encoding":
				stiHtmlExportSettings.Encoding = GetEncoding(text2);
				break;
			case "Zoom":
				stiHtmlExportSettings.Zoom = StiObjectConverter.ConvertToDouble(text2);
				break;
			case "ExportMode":
				stiHtmlExportSettings.ExportMode = (StiHtmlExportMode)Enum.Parse(typeof(StiHtmlExportMode), text2);
				break;
			case "ExportQuality":
				stiHtmlExportSettings.ExportQuality = (StiHtmlExportQuality)Enum.Parse(typeof(StiHtmlExportQuality), text2);
				break;
			case "AddPageBreaks":
				stiHtmlExportSettings.AddPageBreaks = Convert.ToBoolean(text2);
				break;
			case "BookmarksTreeWidth":
				stiHtmlExportSettings.BookmarksTreeWidth = Convert.ToInt32(text2);
				break;
			case "ExportBookmarksMode":
				stiHtmlExportSettings.ExportBookmarksMode = (StiHtmlExportBookmarksMode)Enum.Parse(typeof(StiHtmlExportBookmarksMode), text2);
				break;
			case "UseStylesTable":
				stiHtmlExportSettings.UseStylesTable = Convert.ToBoolean(text2);
				break;
			case "RemoveEmptySpaceAtBottom":
				stiHtmlExportSettings.RemoveEmptySpaceAtBottom = Convert.ToBoolean(text2);
				break;
			case "PageHorAlignment":
				stiHtmlExportSettings.PageHorAlignment = (StiHorAlignment)Enum.Parse(typeof(StiHorAlignment), text2);
				break;
			case "CompressToArchive":
				stiHtmlExportSettings.CompressToArchive = Convert.ToBoolean(text2);
				break;
			case "UseEmbeddedImages":
				stiHtmlExportSettings.UseEmbeddedImages = Convert.ToBoolean(text2);
				break;
			case "ContinuousPages":
				stiHtmlExportSettings.ContinuousPages = Convert.ToBoolean(text2);
				break;
			case "ImageFormat":
				switch (text2)
				{
				case "Bmp":
					stiHtmlExportSettings.ImageFormat = ImageFormat.Bmp;
					break;
				case "Emf":
					stiHtmlExportSettings.ImageFormat = ImageFormat.Emf;
					break;
				case "Gif":
					stiHtmlExportSettings.ImageFormat = ImageFormat.Gif;
					break;
				case "Jpeg":
					stiHtmlExportSettings.ImageFormat = ImageFormat.Jpeg;
					break;
				case "Png":
					stiHtmlExportSettings.ImageFormat = ImageFormat.Png;
					break;
				case "Tiff":
					stiHtmlExportSettings.ImageFormat = ImageFormat.Tiff;
					break;
				}
				break;
			}
		}
		return stiHtmlExportSettings;
	}

	private static StiTxtExportSettings GetTextExportSettings(Hashtable jsSettings)
	{
		StiTxtExportSettings stiTxtExportSettings = new StiTxtExportSettings();
		foreach (string key in jsSettings.Keys)
		{
			string text2 = ((jsSettings[key] != null) ? jsSettings[key].ToString() : null);
			switch (key)
			{
			case "PageRange":
			case "PagesRange":
				stiTxtExportSettings.PageRange = new StiPagesRange(text2);
				break;
			case "Encoding":
				stiTxtExportSettings.Encoding = GetEncoding(text2);
				break;
			case "DrawBorder":
				stiTxtExportSettings.DrawBorder = Convert.ToBoolean(text2);
				break;
			case "BorderType":
				stiTxtExportSettings.BorderType = (StiTxtBorderType)Enum.Parse(typeof(StiTxtBorderType), text2);
				break;
			case "KillSpaceLines":
				stiTxtExportSettings.KillSpaceLines = Convert.ToBoolean(text2);
				break;
			case "KillSpaceGraphLines":
				stiTxtExportSettings.KillSpaceGraphLines = Convert.ToBoolean(text2);
				break;
			case "PutFeedPageCode":
				stiTxtExportSettings.PutFeedPageCode = Convert.ToBoolean(text2);
				break;
			case "CutLongLines":
				stiTxtExportSettings.CutLongLines = Convert.ToBoolean(text2);
				break;
			case "ZoomX":
				stiTxtExportSettings.ZoomX = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			case "ZoomY":
				stiTxtExportSettings.ZoomY = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			case "UseEscapeCodes":
				stiTxtExportSettings.UseEscapeCodes = Convert.ToBoolean(text2);
				break;
			case "EscapeCodesCollectionName":
				stiTxtExportSettings.EscapeCodesCollectionName = text2;
				break;
			}
		}
		return stiTxtExportSettings;
	}

	private static StiRtfExportSettings GetRtfExportSettings(Hashtable jsSettings)
	{
		StiRtfExportSettings stiRtfExportSettings = new StiRtfExportSettings();
		foreach (string key in jsSettings.Keys)
		{
			string text2 = ((jsSettings[key] != null) ? jsSettings[key].ToString() : null);
			switch (key)
			{
			case "PageRange":
			case "PagesRange":
				stiRtfExportSettings.PageRange = new StiPagesRange(text2);
				break;
			case "CodePage":
				stiRtfExportSettings.CodePage = Convert.ToInt32(text2);
				break;
			case "ExportMode":
				stiRtfExportSettings.ExportMode = (StiRtfExportMode)Enum.Parse(typeof(StiRtfExportMode), text2);
				break;
			case "UsePageHeadersAndFooters":
				stiRtfExportSettings.UsePageHeadersAndFooters = Convert.ToBoolean(text2);
				break;
			case "ImageResolution":
				stiRtfExportSettings.ImageResolution = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			case "ImageQuality":
				stiRtfExportSettings.ImageQuality = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			case "RemoveEmptySpaceAtBottom":
				stiRtfExportSettings.RemoveEmptySpaceAtBottom = Convert.ToBoolean(text2);
				break;
			case "StoreImagesAsPng":
				stiRtfExportSettings.StoreImagesAsPng = Convert.ToBoolean(text2);
				break;
			}
		}
		return stiRtfExportSettings;
	}

	private static StiWord2007ExportSettings GetWord2007ExportSettings(Hashtable jsSettings)
	{
		StiWord2007ExportSettings stiWord2007ExportSettings = new StiWord2007ExportSettings();
		foreach (string key in jsSettings.Keys)
		{
			string text2 = ((jsSettings[key] != null) ? jsSettings[key].ToString() : null);
			switch (key)
			{
			case "PageRange":
			case "PagesRange":
				stiWord2007ExportSettings.PageRange = new StiPagesRange(text2);
				break;
			case "UsePageHeadersAndFooters":
				stiWord2007ExportSettings.UsePageHeadersAndFooters = Convert.ToBoolean(text2);
				break;
			case "ImageQuality":
				stiWord2007ExportSettings.ImageQuality = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			case "ImageResolution":
				stiWord2007ExportSettings.ImageResolution = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			case "RemoveEmptySpaceAtBottom":
				stiWord2007ExportSettings.RemoveEmptySpaceAtBottom = Convert.ToBoolean(text2);
				break;
			case "CompanyString":
				stiWord2007ExportSettings.CompanyString = text2;
				break;
			case "LastModifiedString":
				stiWord2007ExportSettings.LastModifiedString = text2;
				break;
			case "RestrictEditing":
				stiWord2007ExportSettings.RestrictEditing = (StiWord2007RestrictEditing)Enum.Parse(typeof(StiWord2007RestrictEditing), text2);
				break;
			}
		}
		return stiWord2007ExportSettings;
	}

	private static StiOdtExportSettings GetOdtExportSettings(Hashtable jsSettings)
	{
		StiOdtExportSettings stiOdtExportSettings = new StiOdtExportSettings();
		foreach (string key in jsSettings.Keys)
		{
			string text2 = ((jsSettings[key] != null) ? jsSettings[key].ToString() : null);
			switch (key)
			{
			case "PageRange":
			case "PagesRange":
				stiOdtExportSettings.PageRange = new StiPagesRange(text2);
				break;
			case "UsePageHeadersAndFooters":
				stiOdtExportSettings.UsePageHeadersAndFooters = Convert.ToBoolean(text2);
				break;
			case "ImageResolution":
				stiOdtExportSettings.ImageResolution = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			case "ImageQuality":
				stiOdtExportSettings.ImageQuality = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			case "RemoveEmptySpaceAtBottom":
				stiOdtExportSettings.RemoveEmptySpaceAtBottom = Convert.ToBoolean(text2);
				break;
			}
		}
		return stiOdtExportSettings;
	}

	private static StiExcelExportSettings GetExcelExportSettings(Hashtable jsSettings)
	{
		StiExcelExportSettings stiExcelExportSettings = new StiExcelExportSettings();
		foreach (string key in jsSettings.Keys)
		{
			string text2 = ((jsSettings[key] != null) ? jsSettings[key].ToString() : null);
			switch (key)
			{
			case "PageRange":
			case "PagesRange":
				stiExcelExportSettings.PageRange = new StiPagesRange(text2);
				break;
			case "ExcelType":
				stiExcelExportSettings.ExcelType = (StiExcelType)Enum.Parse(typeof(StiExcelType), text2);
				break;
			case "UseOnePageHeaderAndFooter":
				stiExcelExportSettings.UseOnePageHeaderAndFooter = Convert.ToBoolean(text2);
				break;
			case "DataExportMode":
				stiExcelExportSettings.DataExportMode = (StiDataExportMode)Enum.Parse(typeof(StiDataExportMode), text2);
				break;
			case "ExportPageBreaks":
				stiExcelExportSettings.ExportPageBreaks = Convert.ToBoolean(text2);
				break;
			case "ExportObjectFormatting":
				stiExcelExportSettings.ExportObjectFormatting = Convert.ToBoolean(text2);
				break;
			case "ExportEachPageToSheet":
				stiExcelExportSettings.ExportEachPageToSheet = Convert.ToBoolean(text2);
				break;
			case "ImageQuality":
				stiExcelExportSettings.ImageQuality = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			case "ImageResolution":
				stiExcelExportSettings.ImageResolution = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			case "CompanyString":
				stiExcelExportSettings.CompanyString = text2;
				break;
			case "LastModifiedString":
				stiExcelExportSettings.LastModifiedString = text2;
				break;
			}
		}
		return stiExcelExportSettings;
	}

	private static StiOdsExportSettings GetOdsExportSettings(Hashtable jsSettings)
	{
		StiOdsExportSettings stiOdsExportSettings = new StiOdsExportSettings();
		foreach (string key in jsSettings.Keys)
		{
			string text2 = ((jsSettings[key] != null) ? jsSettings[key].ToString() : null);
			switch (key)
			{
			case "PageRange":
			case "PagesRange":
				stiOdsExportSettings.PageRange = new StiPagesRange(text2);
				break;
			case "ImageResolution":
				stiOdsExportSettings.ImageResolution = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			case "ImageQuality":
				stiOdsExportSettings.ImageQuality = (float)StiObjectConverter.ConvertToDouble(text2);
				break;
			}
		}
		return stiOdsExportSettings;
	}

	private static StiDataExportSettings GetDataExportSettings(Hashtable jsSettings)
	{
		StiDataExportSettings stiDataExportSettings = new StiDataExportSettings();
		foreach (string key in jsSettings.Keys)
		{
			string text2 = ((jsSettings[key] != null) ? jsSettings[key].ToString() : null);
			switch (key)
			{
			case "PageRange":
			case "PagesRange":
				stiDataExportSettings.PageRange = new StiPagesRange(text2);
				break;
			case "DataType":
				stiDataExportSettings.DataType = (StiDataType)Enum.Parse(typeof(StiDataType), text2);
				break;
			case "DataExportMode":
				stiDataExportSettings.DataExportMode = (StiDataExportMode)Enum.Parse(typeof(StiDataExportMode), text2);
				break;
			case "Encoding":
				stiDataExportSettings.Encoding = GetEncoding(text2);
				break;
			case "ExportDataOnly":
				stiDataExportSettings.ExportDataOnly = Convert.ToBoolean(text2);
				break;
			case "CodePage":
				stiDataExportSettings.CodePage = (StiDbfCodePages)Enum.Parse(typeof(StiDbfCodePages), text2);
				break;
			case "Separator":
				stiDataExportSettings.Separator = text2;
				break;
			case "SkipColumnHeaders":
				stiDataExportSettings.SkipColumnHeaders = Convert.ToBoolean(text2);
				break;
			case "UseDefaultSystemEncoding":
				stiDataExportSettings.UseDefaultSystemEncoding = Convert.ToBoolean(text2);
				break;
			}
		}
		return stiDataExportSettings;
	}

	private static StiImageExportSettings GetImageExportSettings(Hashtable jsSettings)
	{
		StiImageExportSettings stiImageExportSettings = new StiImageExportSettings();
		foreach (string key in jsSettings.Keys)
		{
			string text2 = ((jsSettings[key] != null) ? jsSettings[key].ToString() : null);
			switch (key)
			{
			case "PageRange":
			case "PagesRange":
				stiImageExportSettings.PageRange = new StiPagesRange(text2);
				break;
			case "ImageType":
				stiImageExportSettings.ImageType = (StiImageType)Enum.Parse(typeof(StiImageType), text2);
				break;
			case "ImageZoom":
				stiImageExportSettings.ImageZoom = StiObjectConverter.ConvertToDouble(text2);
				break;
			case "ImageResolution":
				stiImageExportSettings.ImageResolution = Convert.ToInt32(text2);
				break;
			case "CutEdges":
				stiImageExportSettings.CutEdges = Convert.ToBoolean(text2);
				break;
			case "ImageFormat":
				stiImageExportSettings.ImageFormat = (StiImageFormat)Enum.Parse(typeof(StiImageFormat), text2);
				break;
			case "MultipleFiles":
				stiImageExportSettings.MultipleFiles = Convert.ToBoolean(text2);
				break;
			case "DitheringType":
				stiImageExportSettings.DitheringType = (StiMonochromeDitheringType)Enum.Parse(typeof(StiMonochromeDitheringType), text2);
				break;
			case "TiffCompressionScheme":
				stiImageExportSettings.TiffCompressionScheme = (StiTiffCompressionScheme)Enum.Parse(typeof(StiTiffCompressionScheme), text2);
				break;
			case "CompressToArchive":
				stiImageExportSettings.CompressToArchive = Convert.ToBoolean(text2);
				break;
			}
		}
		return stiImageExportSettings;
	}

	public static Stream ExportDashboard(StiRequestParams requestParams, StiReport report, IStiDashboardExportSettings exportSettings)
	{
		StiPage stiPage = report.Pages.ToList().FirstOrDefault((StiPage p) => !(p is IStiDashboard)) ?? report.Pages[0];
		report.CurrentPage = requestParams.Viewer.PageNumber;
		IStiDashboard stiDashboard = (IStiDashboard)report.Pages[requestParams.Viewer.PageNumber];
		if (!((StiPage)stiDashboard).Enabled)
		{
			stiDashboard = report.Pages.ToList().FirstOrDefault((StiPage p) => p.Enabled && p.IsDashboard) as IStiDashboard;
		}
		IStiElement stiElement = stiDashboard.GetElements(nested: false).FirstOrDefault((IStiElement e) => e.Name == requestParams.Viewer.ElementName);
		if (exportSettings is IStiImageDashboardExportSettings)
		{
			((IStiImageDashboardExportSettings)exportSettings).Width = requestParams.Viewer.DashboardWidth;
			((IStiImageDashboardExportSettings)exportSettings).Height = requestParams.Viewer.DashboardHeight;
		}
		report.InvokeExporting(requestParams.ExportFormat);
		if (requestParams.ExportFormat == StiExportFormat.Document)
		{
			StiReport stiReport = stiDashboard.Report.Clone() as StiReport;
			using MemoryStream memoryStream = new MemoryStream();
			if (!string.IsNullOrEmpty(report.ReportGuid))
			{
				stiReport.ReportGuid = report.ReportGuid;
			}
			stiReport.SaveSnapshot(memoryStream);
			report.InvokeExported(requestParams.ExportFormat);
			return memoryStream;
		}
		object[] array = new object[2];
		IStiElement stiElement3;
		if (stiElement == null)
		{
			IStiElement stiElement2 = stiDashboard;
			stiElement3 = stiElement2;
		}
		else
		{
			stiElement3 = stiElement;
		}
		array[0] = stiElement3;
		array[1] = exportSettings;
		object obj = StiInvokeMethodsHelper.InvokeStaticMethod("Stimulsoft.Dashboard.Export", "StiDashboardExportTools", "ExportToStream", array, new Type[2]
		{
			typeof(IStiElement),
			StiInvokeMethodsHelper.GetType("Stimulsoft.Dashboard.Export", "Settings.StiDashboardExportSettings")
		});
		report.InvokeExported(requestParams.ExportFormat);
		return (Stream)obj;
	}

	public static Stream ExportReport(StiRequestParams requestParams, StiReport report, StiExportSettings settings)
	{
		StiPage page = report.Pages.ToList().FirstOrDefault((StiPage p) => !(p is IStiDashboard)) ?? report.Pages[0];
		report.CurrentPage = report.Pages.IndexOf(page);
		StiEditableFieldsHelper.ApplyEditableFieldsToReport(report, requestParams.Interaction.Editable);
		StiExportService stiExportService = null;
		Stream stream = new MemoryStream();
		switch (requestParams.ExportFormat)
		{
		case StiExportFormat.Document:
			switch (GetExportDocumentType(requestParams))
			{
			case StiDocumentType.Mdc:
				report.SaveDocument(stream);
				break;
			case StiDocumentType.Mdz:
				report.SavePackedDocument(stream);
				break;
			case StiDocumentType.Mdx:
			{
				string key = ((requestParams.ExportSettings["SaveReportPassword"] != null) ? ((string)requestParams.ExportSettings["SaveReportPassword"]) : string.Empty);
				report.SaveEncryptedDocument(stream, key);
				break;
			}
			}
			break;
		case StiExportFormat.Pdf:
			stiExportService = new StiPdfExportService();
			break;
		case StiExportFormat.Xps:
			stiExportService = new StiXpsExportService();
			break;
		case StiExportFormat.Ppt2007:
			stiExportService = new StiPpt2007ExportService();
			break;
		case StiExportFormat.Html:
			stiExportService = new StiHtmlExportService();
			break;
		case StiExportFormat.Html5:
			stiExportService = new StiHtml5ExportService();
			break;
		case StiExportFormat.Mht:
			stiExportService = new StiMhtExportService();
			break;
		case StiExportFormat.Text:
			stiExportService = new StiTxtExportService();
			break;
		case StiExportFormat.Rtf:
			stiExportService = new StiRtfExportService();
			break;
		case StiExportFormat.Word2007:
			stiExportService = new StiWord2007ExportService();
			break;
		case StiExportFormat.Odt:
			stiExportService = new StiOdtExportService();
			break;
		case StiExportFormat.Excel:
			stiExportService = new StiExcelExportService();
			break;
		case StiExportFormat.ExcelXml:
			stiExportService = new StiExcelXmlExportService();
			break;
		case StiExportFormat.Excel2007:
			stiExportService = new StiExcel2007ExportService();
			break;
		case StiExportFormat.Ods:
			stiExportService = new StiOdsExportService();
			break;
		case StiExportFormat.Xml:
		case StiExportFormat.Csv:
		case StiExportFormat.Dif:
		case StiExportFormat.Sylk:
		case StiExportFormat.Dbf:
		case StiExportFormat.Json:
			stiExportService = new StiDataExportService();
			break;
		case StiExportFormat.ImageGif:
		case StiExportFormat.ImageBmp:
		case StiExportFormat.ImagePng:
		case StiExportFormat.ImageTiff:
		case StiExportFormat.ImageJpeg:
		case StiExportFormat.ImagePcx:
		case StiExportFormat.ImageEmf:
		case StiExportFormat.ImageSvg:
		case StiExportFormat.ImageSvgz:
			stiExportService = new StiImageExportService();
			break;
		}
		if (stiExportService != null)
		{
			if (report.CookieContainer == null || report.CookieContainer.Count == 0 || requestParams.Server.AllowAutoUpdateCookies)
			{
				report.CookieContainer = StiReportHelper.GetCookieContainer(requestParams.HttpContext.Request);
			}
			report.InvokeExporting(requestParams.ExportFormat);
			stiExportService.ExportTo(report, stream, settings);
			report.InvokeExported(requestParams.ExportFormat);
		}
		return stream;
	}

	public static Stream EmailReport(StiRequestParams requestParams, StiReport report, StiExportSettings settings, StiEmailOptions options)
	{
		string reportFileName = GetReportFileName(requestParams, report);
		string s = string.Empty;
		Stream stream = ExportReport(requestParams, report, settings);
		stream.Seek(0L, SeekOrigin.Begin);
		try
		{
			Attachment item = new Attachment(stream, reportFileName);
			MailMessage mailMessage = new MailMessage(options.AddressFrom, options.AddressTo);
			mailMessage.Attachments.Add(item);
			mailMessage.Subject = options.Subject;
			if (!string.IsNullOrEmpty(options.ReplyTo))
			{
				string[] array = options.ReplyTo.Split(new string[2] { ",", ";" }, StringSplitOptions.None);
				string[] array2 = array;
				foreach (string text in array2)
				{
					mailMessage.ReplyToList.Add(text.Trim());
				}
			}
			foreach (object item2 in options.CC)
			{
				if (item2 is MailAddress)
				{
					mailMessage.CC.Add((MailAddress)item2);
				}
				else
				{
					mailMessage.CC.Add((string)item2);
				}
			}
			foreach (object item3 in options.BCC)
			{
				if (item3 is MailAddress)
				{
					mailMessage.Bcc.Add((MailAddress)item3);
				}
				else
				{
					mailMessage.Bcc.Add((string)item3);
				}
			}
			if (!string.IsNullOrEmpty(options.Body))
			{
				mailMessage.Body = options.Body;
			}
			else
			{
				mailMessage.Body = "This Email contains the '" + reportFileName + "' report file.";
			}
			SmtpClient smtpClient = new SmtpClient(options.Host, options.Port);
			smtpClient.EnableSsl = options.EnableSsl;
			smtpClient.UseDefaultCredentials = false;
			smtpClient.Credentials = new NetworkCredential(options.UserName, options.Password);
			s = "0";
			smtpClient.Send(mailMessage);
		}
		catch (Exception ex)
		{
			s = $"<?xml version=\"1.0\" encoding=\"UTF-8\"?><SendEmail><ErrorCode>An error occurred while sending Email.</ErrorCode><ErrorDescription>{((ex.InnerException != null) ? ex.InnerException.Message : ex.Message)}</ErrorDescription></SendEmail>";
		}
		finally
		{
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			stream = new MemoryStream(bytes);
		}
		return stream;
	}

	public static StiWebActionResult PrintReportResult(StiRequestParams requestParams, StiReport report, StiExportSettings settings)
	{
		if (report == null)
		{
			return StiWebActionResult.EmptyReportResult();
		}
		if (!report.IsRendered)
		{
			try
			{
				report.Render(showProgress: false);
			}
			catch (Exception ex)
			{
				string text = ex.Message;
				if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
				{
					text = text + " " + ex.InnerException.Message;
				}
				return new StiWebActionResult("ServerError:" + text);
			}
		}
		report.InvokePrinting();
		StiEditableFieldsHelper.ApplyEditableFieldsToReport(report, requestParams.Interaction.Editable);
		MemoryStream memoryStream = new MemoryStream();
		if (requestParams.Viewer.PrintAction == StiPrintAction.PrintPdf)
		{
			StiPdfExportSettings stiPdfExportSettings = (StiPdfExportSettings)settings;
			stiPdfExportSettings.AutoPrintMode = StiPdfAutoPrintMode.Dialog;
			StiPdfExportService stiPdfExportService = new StiPdfExportService();
			report.IsPrinting = true;
			stiPdfExportService.ExportPdf(report, memoryStream, stiPdfExportSettings);
			report.IsPrinting = false;
			report.InvokePrinted();
			return new StiWebActionResult(memoryStream, "application/pdf");
		}
		StiHtmlExportSettings stiHtmlExportSettings = (StiHtmlExportSettings)settings;
		stiHtmlExportSettings.AddPageBreaks = true;
		stiHtmlExportSettings.UseWatermarkMargins = false;
		stiHtmlExportSettings.ExportQuality = StiHtmlExportQuality.High;
		stiHtmlExportSettings.ChartType = ((requestParams.Viewer.ChartRenderType == StiChartRenderType.Image) ? StiHtmlChartType.Image : StiHtmlChartType.Vector);
		if (requestParams.Viewer.BookmarksPrint)
		{
			stiHtmlExportSettings.ExportBookmarksMode = StiHtmlExportBookmarksMode.All;
		}
		else
		{
			stiHtmlExportSettings.ExportBookmarksMode = StiHtmlExportBookmarksMode.ReportOnly;
		}
		switch (requestParams.Viewer.ReportDisplayMode)
		{
		case StiReportDisplayMode.Table:
			stiHtmlExportSettings.ExportMode = StiHtmlExportMode.Table;
			break;
		case StiReportDisplayMode.Div:
			stiHtmlExportSettings.ExportMode = StiHtmlExportMode.Div;
			break;
		case StiReportDisplayMode.Span:
			stiHtmlExportSettings.ExportMode = StiHtmlExportMode.Span;
			break;
		}
		switch (requestParams.Viewer.ImagesQuality)
		{
		case StiImagesQuality.Low:
			stiHtmlExportSettings.ImageResolution = 50f;
			break;
		case StiImagesQuality.Normal:
			stiHtmlExportSettings.ImageResolution = 100f;
			break;
		case StiImagesQuality.High:
			stiHtmlExportSettings.ImageResolution = 200f;
			break;
		}
		StiHtmlExportService stiHtmlExportService = new StiHtmlExportService();
		report.IsPrinting = true;
		stiHtmlExportService.ExportHtml(report, memoryStream, stiHtmlExportSettings);
		report.IsPrinting = false;
		string @string = Encoding.UTF8.GetString(memoryStream.ToArray());
		memoryStream.Close();
		@string = @string.Insert(@string.IndexOf("></meta>") + 8, "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"></meta>");
		report.InvokePrinted();
		return new StiWebActionResult(@string, "text/html");
	}

	public static StiWebActionResult ExportDashboardResult(StiRequestParams requestParams, StiReport report, IStiDashboardExportSettings exportSettings)
	{
		if (report == null)
		{
			return StiWebActionResult.EmptyReportResult();
		}
		Stream data = ExportDashboard(requestParams, report, exportSettings);
		if (requestParams.ExportFormat == StiExportFormat.Document)
		{
			return new StiWebActionResult(data, "text/xml", StiReportHelper.GetReportFileName(report) + ".mrt");
		}
		string reportFileContentType = GetReportFileContentType(requestParams);
		string reportFileName = GetReportFileName(requestParams, report);
		bool flag = requestParams.ExportSettings.Contains("OpenAfterExport") && (bool)requestParams.ExportSettings["OpenAfterExport"];
		return new StiWebActionResult(data, reportFileContentType, reportFileName, !flag);
	}

	public static StiWebActionResult ExportReportResult(StiRequestParams requestParams, StiReport report, StiExportSettings settings)
	{
		if (report == null)
		{
			return StiWebActionResult.EmptyReportResult();
		}
		if (requestParams.Viewer.ReportType == StiReportType.Report && !report.IsRendered)
		{
			try
			{
				report.Render(showProgress: false);
			}
			catch (Exception ex)
			{
				string text = ex.Message;
				if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
				{
					text = text + " " + ex.InnerException.Message;
				}
				return new StiWebActionResult("ServerError:" + text);
			}
		}
		Stream data = ExportReport(requestParams, report, settings);
		string reportFileContentType = GetReportFileContentType(requestParams);
		string reportFileName = GetReportFileName(requestParams, report);
		bool flag = requestParams.ExportSettings.Contains("OpenAfterExport") && (bool)requestParams.ExportSettings["OpenAfterExport"];
		return new StiWebActionResult(data, reportFileContentType, reportFileName, !flag);
	}

	public static StiWebActionResult EmailReportResult(StiRequestParams requestParams, StiReport report, StiExportSettings settings, StiEmailOptions options)
	{
		if (options == null)
		{
			return StiWebActionResult.ErrorResult(requestParams, "An error occurred while sending Email: StiEmailOptions object cannot be null.");
		}
		if (report == null)
		{
			return StiWebActionResult.EmptyReportResult();
		}
		if (!report.IsRendered)
		{
			try
			{
				report.Render(showProgress: false);
			}
			catch (Exception ex)
			{
				string text = ex.Message;
				if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
				{
					text = text + " " + ex.InnerException.Message;
				}
				return StiWebActionResult.ErrorResult(requestParams, text);
			}
		}
		Stream data = EmailReport(requestParams, report, settings, options);
		return new StiWebActionResult(data, "text/plain");
	}
}
