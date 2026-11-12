using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Export.Tools;

namespace Stimulsoft.Report.Web;

internal class StiCollectionsHelper
{
	private static ConcurrentDictionary<string, Hashtable> LocalizationItems = new ConcurrentDictionary<string, Hashtable>();

	private static Hashtable Item(string value, string key)
	{
		Hashtable hashtable = new Hashtable();
		hashtable["key"] = key;
		hashtable["value"] = value;
		return hashtable;
	}

	public static ArrayList GetEncodingDataItems()
	{
		ArrayList items = new ArrayList();
		ArrayList arrayList = new ArrayList
		{
			Encoding.Default.CodePage,
			Encoding.ASCII.CodePage,
			Encoding.BigEndianUnicode.CodePage,
			Encoding.Unicode.CodePage,
			Encoding.UTF7.CodePage,
			Encoding.UTF8.CodePage
		};
		for (int i = 1250; i < 1257; i++)
		{
			try
			{
				arrayList.Add(Encoding.GetEncoding(i).CodePage);
			}
			catch
			{
			}
		}
		arrayList.Cast<int>().Distinct().ToList()
			.ForEach(delegate(int e)
			{
				items.Add(Item(Encoding.GetEncoding(e).EncodingName, Encoding.GetEncoding(e).CodePage.ToString()));
			});
		return items;
	}

	public static ArrayList GetPdfSecurityCertificatesItems()
	{
		ArrayList items = new ArrayList();
		List<StiPdfSecurity.StiCertificateInfo> certificatesList = StiPdfSecurity.GetCertificatesList(useLocalMachineCertificates: false);
		certificatesList.ForEach(delegate(StiPdfSecurity.StiCertificateInfo c)
		{
			items.Add(new Hashtable
			{
				["name"] = c.Name,
				["issuer"] = c.Issuer,
				["from"] = c.From.ToString("MM'/'dd'/'yyyy"),
				["to"] = c.To.ToString("MM'/'dd'/'yyyy"),
				["thumbprint"] = c.Thumbprint
			});
		});
		return items;
	}

	public static void LoadLocalizationFile(HttpContext httpContext, string directoryPath, string filePath)
	{
		if (string.IsNullOrEmpty(filePath))
		{
			return;
		}
		try
		{
			if (filePath.ToLower() == "default")
			{
				StiLocalization.LoadDefaultLocalization();
				return;
			}
			if (!filePath.ToLower().EndsWith(".xml"))
			{
				filePath += ".xml";
			}
			if (string.IsNullOrEmpty(directoryPath))
			{
				directoryPath = "Localization";
			}
			string text = filePath;
			if (!File.Exists(text))
			{
				text = httpContext.Server.MapPath(filePath);
			}
			if (!File.Exists(text))
			{
				text = httpContext.Server.MapPath(Path.Combine(directoryPath, filePath));
			}
			if (!File.Exists(text))
			{
				text = httpContext.Server.MapPath("/" + filePath);
			}
			if (!File.Exists(text))
			{
				text = httpContext.Server.MapPath("/" + Path.Combine(directoryPath, filePath));
			}
			if (File.Exists(text))
			{
				StiOptions.Localization.Load(text);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
	}

	public static ArrayList GetLocalizationsList(HttpContext httpContext, string directoryPath)
	{
		ArrayList arrayList = new ArrayList();
		if (string.IsNullOrEmpty(directoryPath))
		{
			return arrayList;
		}
		string path = directoryPath;
		if (!Directory.Exists(path))
		{
			path = httpContext.Server.MapPath(directoryPath);
		}
		if (!Directory.Exists(path))
		{
			path = httpContext.Server.MapPath("/" + directoryPath);
		}
		if (Directory.Exists(path))
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			FileInfo[] files = directoryInfo.GetFiles();
			FileInfo[] array = files;
			foreach (FileInfo fileInfo in array)
			{
				if (!fileInfo.FullName.EndsWith(".xml") || fileInfo.FullName.EndsWith(".ext.xml"))
				{
					continue;
				}
				try
				{
					StiLocalization.GetParam(fileInfo.FullName, out var language, out var description, out var cultureName);
					if (!string.IsNullOrEmpty(language))
					{
						Hashtable hashtable = new Hashtable();
						hashtable["FileName"] = fileInfo.Name;
						hashtable["Language"] = language;
						hashtable["Description"] = description;
						hashtable["CultureName"] = cultureName;
						arrayList.Add(hashtable);
					}
				}
				catch
				{
				}
			}
		}
		return arrayList;
	}

	public static Hashtable GetLocalizationItems(StiRequestParams requestParams)
	{
		string localization = requestParams.Localization;
		string key = StiMD5Helper.ComputeHash(localization);
		LocalizationItems.TryGetValue(key, out var value);
		if (value != null)
		{
			return value;
		}
		LoadLocalizationFile(requestParams.HttpContext, string.Empty, localization);
		value = new Hashtable();
		value["Viewer"] = StiLocalization.Get("FormViewer", "title");
		value["UpgradeNow"] = StiLocalization.Get("Buttons", "UpgradeNow");
		value["EditorToolTip"] = StiLocalization.Get("FormViewer", "Editor");
		value["TellMeMore"] = StiLocalization.Get("HelpDesigner", "TellMeMore");
		value["Print"] = StiLocalization.Get("A_WebViewer", "PrintReport");
		value["PrintToolTip"] = StiLocalization.Get("HelpViewer", "Print");
		value["Save"] = StiLocalization.Get("A_WebViewer", "SaveReport");
		value["SaveToolTip"] = StiLocalization.Get("HelpViewer", "Save");
		value["Open"] = StiLocalization.Get("Buttons", "Open");
		value["OpenToolTip"] = StiLocalization.Get("HelpViewer", "Open");
		value["SendEmail"] = StiLocalization.Get("FormViewer", "SendEMail").Replace("...", string.Empty);
		value["SendEmailToolTip"] = StiLocalization.Get("HelpViewer", "SendEMail");
		value["BookmarksToolTip"] = StiLocalization.Get("HelpViewer", "Bookmarks");
		value["ParametersToolTip"] = StiLocalization.Get("HelpViewer", "Parameters");
		value["FindToolTip"] = StiLocalization.Get("HelpViewer", "Find");
		value["FirstPageToolTip"] = StiLocalization.Get("HelpViewer", "PageFirst");
		value["PrevPageToolTip"] = StiLocalization.Get("HelpViewer", "PagePrevious");
		value["NextPageToolTip"] = StiLocalization.Get("HelpViewer", "PageNext");
		value["LastPageToolTip"] = StiLocalization.Get("HelpViewer", "PageLast");
		value["FullScreenToolTip"] = StiLocalization.Get("HelpViewer", "FullScreen");
		value["ZoomToolTip"] = StiLocalization.Get("FormViewer", "Zoom");
		value["Loading"] = StiLocalization.Get("A_WebViewer", "Loading").Replace("...", "");
		value["Bookmarks"] = StiLocalization.Get("FormViewer", "Bookmarks");
		value["Parameters"] = StiLocalization.Get("FormViewer", "Parameters");
		value["Time"] = StiLocalization.Get("FormFormatEditor", "Time");
		value["Version"] = StiLocalization.Get("PropertyMain", "Version");
		value["Maximum"] = StiLocalization.Get("PropertyMain", "Maximum");
		value["Copy"] = StiLocalization.Get("Report", "CopyOf");
		value["FindWhat"] = StiLocalization.Get("FormViewerFind", "FindWhat");
		value["FindPrevious"] = StiLocalization.Get("FormViewerFind", "FindPrevious");
		value["FindNext"] = StiLocalization.Get("FormViewerFind", "FindNext");
		value["MatchCase"] = StiLocalization.Get("Editor", "MatchCase");
		value["MatchWholeWord"] = StiLocalization.Get("Editor", "MatchWholeWord");
		value["TypeToSearch"] = StiLocalization.Get("Editor", "TypeToSearch");
		value["EmailOptions"] = StiLocalization.Get("A_WebViewer", "EmailOptions");
		value["Email"] = StiLocalization.Get("A_WebViewer", "Email").Replace(":", "");
		value["Subject"] = StiLocalization.Get("A_WebViewer", "Subject").Replace(":", "");
		value["Message"] = StiLocalization.Get("A_WebViewer", "Message").Replace(":", "");
		value["Attachment"] = StiLocalization.Get("A_WebViewer", "Attachment").Replace(":", "");
		value["SinglePage"] = StiLocalization.Get("FormViewer", "PageViewModeSinglePage");
		value["Continuous"] = StiLocalization.Get("FormViewer", "PageViewModeContinuous");
		value["MultiplePages"] = StiLocalization.Get("FormViewer", "PageViewModeMultiplePages");
		value["ViewModeToolTip"] = StiLocalization.Get("FormViewer", "ViewMode");
		value["Design"] = StiLocalization.Get("Buttons", "Design");
		value["Page"] = StiLocalization.Get("A_WebViewer", "Page");
		value["PageOf"] = StiLocalization.Get("A_WebViewer", "PageOf");
		value["PrintPdf"] = StiLocalization.Get("A_WebViewer", "PrintToPdf");
		value["PrintWithPreview"] = StiLocalization.Get("A_WebViewer", "PrintWithPreview");
		value["PrintWithoutPreview"] = StiLocalization.Get("A_WebViewer", "PrintWithoutPreview");
		value["ZoomOnePage"] = StiLocalization.Get("Zoom", "PageHeight");
		value["ZoomPageWidth"] = StiLocalization.Get("FormViewer", "ZoomPageWidth");
		value["RemoveAll"] = StiLocalization.Get("Buttons", "RemoveAll");
		value["NewItem"] = StiLocalization.Get("FormDictionaryDesigner", "NewItem");
		value["Close"] = StiLocalization.Get("Buttons", "Close");
		value["Reset"] = StiLocalization.Get("Gui", "cust_pm_reset");
		value["Submit"] = StiLocalization.Get("Buttons", "Submit");
		value["RangeFrom"] = StiLocalization.Get("PropertyMain", "RangeFrom");
		value["RangeTo"] = StiLocalization.Get("PropertyMain", "RangeTo");
		value["ExportFormTitle"] = StiLocalization.Get("Export", "title");
		value["ButtonOk"] = StiLocalization.Get("Gui", "barname_ok");
		value["ButtonCancel"] = StiLocalization.Get("Gui", "barname_cancel");
		value["PagesRange"] = StiLocalization.Get("Report", "RangePage");
		value["PagesRangeAll"] = StiLocalization.Get("Report", "RangeAll");
		value["PagesRangeCurrentPage"] = StiLocalization.Get("Report", "RangeCurrentPage");
		value["PagesRangePages"] = StiLocalization.Get("Report", "RangePages").Replace(":", "");
		value["PagesRangeAllTooltip"] = StiLocalization.Get("HelpViewer", "PageAll");
		value["PagesRangeCurrentPageTooltip"] = StiLocalization.Get("HelpViewer", "CurrentPage");
		value["PagesRangePagesTooltip"] = StiLocalization.Get("HelpViewer", "RangePages");
		value["SettingsGroup"] = StiLocalization.Get("Export", "Settings");
		value["Type"] = StiLocalization.Get("PropertyMain", "Type").Replace(":", "");
		value["TypeTooltip"] = StiLocalization.Get("HelpViewer", "TypeExport");
		value["ZoomHtml"] = StiLocalization.Get("Export", "Scale").Replace(":", "");
		value["ZoomHtmlTooltip"] = StiLocalization.Get("HelpViewer", "ScaleHtml");
		value["ImageFormatForHtml"] = StiLocalization.Get("Export", "ImageFormat").Replace(":", "");
		value["ImageFormatForHtmlTooltip"] = StiLocalization.Get("HelpViewer", "ImageFormat");
		value["SavingReport"] = StiLocalization.Get("DesignerFx", "SavingReport");
		value["EmailSuccessfullySent"] = StiLocalization.Get("DesignerFx", "EmailSuccessfullySent");
		value["SaveReportMdc"] = StiLocalization.Get("FormViewer", "DocumentFile").Replace("...", "") + " (.mdc)";
		value["SaveReportMdz"] = StiLocalization.Get("FormViewer", "CompressedDocumentFile") + " (.mdz)";
		value["SaveReportMdx"] = StiLocalization.Get("FormViewer", "EncryptedDocumentFile") + " (.mdx)";
		value["PasswordEnter"] = StiLocalization.Get("Password", "lbPasswordLoad");
		value["PasswordSaveReport"] = StiLocalization.Get("Report", "LabelPassword");
		value["PasswordSaveReportTooltip"] = StiLocalization.Get("HelpViewer", "UserPassword");
		value["ExportMode"] = StiLocalization.Get("Export", "ExportMode").Replace(":", "");
		value["ExportModeTooltip"] = StiLocalization.Get("HelpViewer", "ExportMode");
		value["CompressToArchive"] = StiLocalization.Get("Export", "CompressToArchive");
		value["CompressToArchiveTooltip"] = StiLocalization.Get("HelpViewer", "CompressToArchive");
		value["EmbeddedImageData"] = StiLocalization.Get("Export", "EmbeddedImageData");
		value["EmbeddedImageDataTooltip"] = StiLocalization.Get("HelpViewer", "EmbeddedImageData");
		value["AddPageBreaks"] = StiLocalization.Get("Export", "AddPageBreaks");
		value["AddPageBreaksTooltip"] = StiLocalization.Get("HelpViewer", "AddPageBreaks");
		value["ImageResolution"] = StiLocalization.Get("Export", "ImageResolution").Replace(":", "");
		value["ImageResolutionTooltip"] = StiLocalization.Get("HelpViewer", "ImageResolution");
		value["ImageCompressionMethod"] = StiLocalization.Get("Export", "ImageCompressionMethod").Replace(":", "");
		value["ImageCompressionMethodTooltip"] = StiLocalization.Get("HelpViewer", "ImageCompressionMethod");
		value["ImageQuality"] = StiLocalization.Get("Export", "ImageQuality").Replace(":", "");
		value["ImageQualityTooltip"] = StiLocalization.Get("HelpViewer", "ImageQuality");
		value["ContinuousPages"] = StiLocalization.Get("Export", "ContinuousPages");
		value["ContinuousPagesTooltip"] = StiLocalization.Get("HelpViewer", "ContinuousPages");
		value["StandardPDFFonts"] = StiLocalization.Get("Export", "StandardPDFFonts");
		value["StandardPDFFontsTooltip"] = StiLocalization.Get("HelpViewer", "StandardPdfFonts");
		value["EmbeddedFonts"] = StiLocalization.Get("Export", "EmbeddedFonts");
		value["EmbeddedFontsTooltip"] = StiLocalization.Get("HelpViewer", "EmbeddedFonts");
		value["UseUnicode"] = StiLocalization.Get("Export", "UseUnicode");
		value["UseUnicodeTooltip"] = StiLocalization.Get("HelpViewer", "UseUnicode");
		value["Compressed"] = StiLocalization.Get("Export", "Compressed");
		value["CompressedTooltip"] = StiLocalization.Get("HelpViewer", "Compressed");
		value["ExportRtfTextAsImage"] = StiLocalization.Get("Export", "ExportRtfTextAsImage");
		value["ExportRtfTextAsImageTooltip"] = StiLocalization.Get("HelpViewer", "ExportRtfTextAsImage");
		value["PdfACompliance"] = StiLocalization.Get("Export", "PdfACompliance");
		value["PdfAComplianceTooltip"] = StiLocalization.Get("HelpViewer", "PdfACompliance");
		value["KillSpaceLines"] = StiLocalization.Get("Export", "TxtKillSpaceLines");
		value["KillSpaceLinesTooltip"] = StiLocalization.Get("HelpViewer", "KillSpaceLines");
		value["PutFeedPageCode"] = StiLocalization.Get("Export", "TxtPutFeedPageCode");
		value["PutFeedPageCodeTooltip"] = StiLocalization.Get("HelpViewer", "PutFeedPageCode");
		value["DrawBorder"] = StiLocalization.Get("Export", "TxtDrawBorder");
		value["DrawBorderTooltip"] = StiLocalization.Get("HelpViewer", "DrawBorder");
		value["CutLongLines"] = StiLocalization.Get("Export", "TxtCutLongLines");
		value["CutLongLinesTooltip"] = StiLocalization.Get("HelpViewer", "CutLongLines");
		value["BorderType"] = StiLocalization.Get("Export", "TxtBorderType").Replace(":", "");
		value["BorderTypeTooltip"] = StiLocalization.Get("HelpViewer", "BorderType");
		value["BorderTypeSimple"] = StiLocalization.Get("Export", "TxtBorderTypeSimple");
		value["BorderTypeSingle"] = StiLocalization.Get("Export", "TxtBorderTypeSingle");
		value["BorderTypeDouble"] = StiLocalization.Get("Export", "TxtBorderTypeDouble");
		value["ZoomXY"] = StiLocalization.Get("Export", "Zoom").Replace(":", "");
		value["ZoomXYTooltip"] = StiLocalization.Get("HelpViewer", "ZoomTxt");
		value["EncodingData"] = StiLocalization.Get("Export", "Encoding").Replace(":", "");
		value["EncodingDataTooltip"] = StiLocalization.Get("HelpViewer", "EncodingData");
		value["ImageFormat"] = StiLocalization.Get("Export", "ImageType");
		value["ImageFormatTooltip"] = StiLocalization.Get("HelpViewer", "ImageType");
		value["ImageFormatColor"] = StiLocalization.Get("PropertyMain", "Color");
		value["ImageFormatGrayscale"] = StiLocalization.Get("Export", "ImageGrayscale");
		value["ImageFormatMonochrome"] = StiLocalization.Get("Export", "ImageMonochrome").Replace(":", "");
		value["MonochromeDitheringType"] = StiLocalization.Get("Export", "MonochromeDitheringType").Replace(":", "");
		value["MonochromeDitheringTypeTooltip"] = StiLocalization.Get("HelpViewer", "DitheringType");
		value["TiffCompressionScheme"] = StiLocalization.Get("Export", "TiffCompressionScheme").Replace(":", "");
		value["TiffCompressionSchemeTooltip"] = StiLocalization.Get("HelpViewer", "TiffCompressionScheme");
		value["CutEdges"] = StiLocalization.Get("Export", "ImageCutEdges");
		value["CutEdgesTooltip"] = StiLocalization.Get("HelpViewer", "CutEdges");
		value["MultipleFiles"] = StiLocalization.Get("Export", "MultipleFiles");
		value["MultipleFilesTooltip"] = StiLocalization.Get("HelpViewer", "MultipleFiles");
		value["ExportDataOnly"] = StiLocalization.Get("Export", "ExportDataOnly");
		value["ExportDataOnlyTooltip"] = StiLocalization.Get("HelpViewer", "ExportDataOnly");
		value["UseDefaultSystemEncoding"] = StiLocalization.Get("Export", "UseDefaultSystemEncoding");
		value["UseDefaultSystemEncodingTooltip"] = StiLocalization.Get("HelpViewer", "UseDefaultSystemEncoding");
		value["EncodingDifFile"] = StiLocalization.Get("Export", "Encoding").Replace(":", "");
		value["EncodingDifFileTooltip"] = StiLocalization.Get("HelpViewer", "EncodingData");
		value["ExportModeRtf"] = StiLocalization.Get("Export", "ExportMode").Replace(":", "");
		value["ExportModeRtfTooltip"] = StiLocalization.Get("HelpViewer", "ExportModeRtf");
		value["ExportModeRtfTable"] = StiLocalization.Get("Export", "ExportModeTable");
		value["ExportModeRtfFrame"] = StiLocalization.Get("Export", "ExportModeFrame");
		value["UsePageHeadersFooters"] = StiLocalization.Get("Export", "UsePageHeadersAndFooters");
		value["UsePageHeadersFootersTooltip"] = StiLocalization.Get("HelpViewer", "UsePageHeadersAndFooters");
		value["RemoveEmptySpace"] = StiLocalization.Get("Export", "RemoveEmptySpaceAtBottom");
		value["RemoveEmptySpaceTooltip"] = StiLocalization.Get("HelpViewer", "RemoveEmptySpaceAtBottom");
		value["Separator"] = StiLocalization.Get("Export", "Separator").Replace(":", "");
		value["SeparatorTooltip"] = StiLocalization.Get("HelpViewer", "Separator");
		value["SkipColumnHeaders"] = StiLocalization.Get("Export", "SkipColumnHeaders");
		value["SkipColumnHeadersTooltip"] = StiLocalization.Get("HelpViewer", "SkipColumnHeaders");
		value["ExportObjectFormatting"] = StiLocalization.Get("Export", "ExportObjectFormatting");
		value["ExportObjectFormattingTooltip"] = StiLocalization.Get("HelpViewer", "ExportObjectFormatting");
		value["UseOnePageHeaderFooter"] = StiLocalization.Get("Export", "UseOnePageHeaderAndFooter");
		value["UseOnePageHeaderFooterTooltip"] = StiLocalization.Get("HelpViewer", "UseOnePageHeaderAndFooter");
		value["ExportEachPageToSheet"] = StiLocalization.Get("Export", "ExportEachPageToSheet");
		value["ExportEachPageToSheetTooltip"] = StiLocalization.Get("HelpViewer", "ExportEachPageToSheet");
		value["ExportPageBreaks"] = StiLocalization.Get("Export", "ExportPageBreaks");
		value["ExportPageBreaksTooltip"] = StiLocalization.Get("HelpViewer", "ExportPageBreaks");
		value["EncodingDbfFile"] = StiLocalization.Get("Export", "Encoding").Replace(":", "");
		value["EncodingDbfFileTooltip"] = StiLocalization.Get("HelpViewer", "EncodingData");
		value["DocumentSecurityButton"] = StiLocalization.Get("Export", "DocumentSecurity");
		value["DigitalSignatureButton"] = StiLocalization.Get("Export", "DigitalSignature");
		value["OpenAfterExport"] = StiLocalization.Get("Export", "OpenAfterExport");
		value["OpenAfterExportTooltip"] = StiLocalization.Get("HelpViewer", "OpenAfterExport");
		value["AllowEditable"] = StiLocalization.Get("Export", "AllowEditable").Replace(":", "");
		value["AllowEditableTooltip"] = StiLocalization.Get("HelpViewer", "AllowEditable");
		value["NameYes"] = StiLocalization.Get("FormFormatEditor", "nameYes");
		value["NameNo"] = StiLocalization.Get("FormFormatEditor", "nameNo");
		value["UserPassword"] = StiLocalization.Get("Export", "labelUserPassword").Replace(":", "");
		value["UserPasswordTooltip"] = StiLocalization.Get("HelpViewer", "UserPassword");
		value["OwnerPassword"] = StiLocalization.Get("Export", "labelOwnerPassword").Replace(":", "");
		value["OwnerPasswordTooltip"] = StiLocalization.Get("HelpViewer", "OwnerPassword");
		value["BandsFilter"] = StiLocalization.Get("Export", "BandsFilter").Replace(":", "");
		value["BandsFilterTooltip"] = StiLocalization.Get("HelpViewer", "ExportMode");
		value["BandsFilterAllBands"] = StiLocalization.Get("Export", "AllBands");
		value["BandsFilterDataOnly"] = StiLocalization.Get("Export", "DataOnly");
		value["BandsFilterDataAndHeaders"] = StiLocalization.Get("Export", "DataAndHeaders");
		value["BandsFilterDataAndHeadersFooters"] = StiLocalization.Get("Export", "DataAndHeadersFooters");
		value["Null"] = StiLocalization.Get("Report", "Null");
		value["ViewData"] = StiLocalization.Get("FormTitles", "ViewDataForm");
		value["SelectColumns"] = StiLocalization.Get("Wizards", "SelectColumns");
		value["ImageResolutionModeExactly"] = StiLocalization.Get("Export", "Exactly");
		value["ImageResolutionModeNoMoreThan"] = StiLocalization.Get("Export", "NoMoreThan");
		value["ImageResolutionModeAuto"] = StiLocalization.Get("Export", "Auto");
		value["ImageResolutionMode"] = StiLocalization.Get("Export", "ImageResolutionMode").Replace(":", "");
		value["AllowPrintDocument"] = StiLocalization.Get("Export", "AllowPrintDocument");
		value["AllowPrintDocumentTooltip"] = StiLocalization.Get("HelpViewer", "AllowPrintDocument");
		value["AllowModifyContents"] = StiLocalization.Get("Export", "AllowModifyContents");
		value["AllowModifyContentsTooltip"] = StiLocalization.Get("HelpViewer", "AllowModifyContents");
		value["AllowCopyTextAndGraphics"] = StiLocalization.Get("Export", "AllowCopyTextAndGraphics");
		value["AllowCopyTextAndGraphicsTooltip"] = StiLocalization.Get("HelpViewer", "AllowCopyTextAndGraphics");
		value["AllowAddOrModifyTextAnnotations"] = StiLocalization.Get("Export", "AllowAddOrModifyTextAnnotations");
		value["AllowAddOrModifyTextAnnotationsTooltip"] = StiLocalization.Get("HelpViewer", "AllowAddOrModifyTextAnnotations");
		value["EncryptionKeyLength"] = StiLocalization.Get("Export", "labelEncryptionKeyLength");
		value["EncryptionKeyLengthTooltip"] = StiLocalization.Get("HelpViewer", "EncryptionKeyLength");
		value["EnableAnimation"] = StiLocalization.Get("Export", "EnableAnimation");
		value["UseDigitalSignature"] = StiLocalization.Get("Export", "UseDigitalSignature");
		value["UseDigitalSignatureTooltip"] = StiLocalization.Get("HelpViewer", "DigitalSignature");
		value["GetCertificateFromCryptoUI"] = StiLocalization.Get("Export", "GetCertificateFromCryptoUI");
		value["GetCertificateFromCryptoUITooltip"] = StiLocalization.Get("HelpViewer", "GetCertificateFromCryptoUI");
		value["SubjectNameString"] = StiLocalization.Get("Export", "labelSubjectNameString").Replace(":", "");
		value["SubjectNameStringTooltip"] = StiLocalization.Get("HelpViewer", "SubjectNameString");
		value["MonthJanuary"] = StiLocalization.Get("A_WebViewer", "MonthJanuary");
		value["MonthFebruary"] = StiLocalization.Get("A_WebViewer", "MonthFebruary");
		value["MonthMarch"] = StiLocalization.Get("A_WebViewer", "MonthMarch");
		value["MonthApril"] = StiLocalization.Get("A_WebViewer", "MonthApril");
		value["MonthMay"] = StiLocalization.Get("A_WebViewer", "MonthMay");
		value["MonthJune"] = StiLocalization.Get("A_WebViewer", "MonthJune");
		value["MonthJuly"] = StiLocalization.Get("A_WebViewer", "MonthJuly");
		value["MonthAugust"] = StiLocalization.Get("A_WebViewer", "MonthAugust");
		value["MonthSeptember"] = StiLocalization.Get("A_WebViewer", "MonthSeptember");
		value["MonthOctober"] = StiLocalization.Get("A_WebViewer", "MonthOctober");
		value["MonthNovember"] = StiLocalization.Get("A_WebViewer", "MonthNovember");
		value["MonthDecember"] = StiLocalization.Get("A_WebViewer", "MonthDecember");
		value["DayMonday"] = StiLocalization.Get("A_WebViewer", "DayMonday");
		value["DayTuesday"] = StiLocalization.Get("A_WebViewer", "DayTuesday");
		value["DayWednesday"] = StiLocalization.Get("A_WebViewer", "DayWednesday");
		value["DayThursday"] = StiLocalization.Get("A_WebViewer", "DayThursday");
		value["DayFriday"] = StiLocalization.Get("A_WebViewer", "DayFriday");
		value["DaySaturday"] = StiLocalization.Get("A_WebViewer", "DaySaturday");
		value["DaySunday"] = StiLocalization.Get("A_WebViewer", "DaySunday");
		value["FormViewerTitle"] = StiLocalization.Get("FormViewer", "title");
		value["Error"] = StiLocalization.Get("Errors", "Error");
		value["SelectAll"] = StiLocalization.Get("MainMenu", "menuEditSelectAll").Replace("&", "");
		value["CurrentMonth"] = StiLocalization.Get("DatePickerRanges", "CurrentMonth");
		value["CurrentQuarter"] = StiLocalization.Get("DatePickerRanges", "CurrentQuarter");
		value["CurrentWeek"] = StiLocalization.Get("DatePickerRanges", "CurrentWeek");
		value["CurrentYear"] = StiLocalization.Get("DatePickerRanges", "CurrentYear");
		value["NextMonth"] = StiLocalization.Get("DatePickerRanges", "NextMonth");
		value["NextQuarter"] = StiLocalization.Get("DatePickerRanges", "NextQuarter");
		value["NextWeek"] = StiLocalization.Get("DatePickerRanges", "NextWeek");
		value["NextYear"] = StiLocalization.Get("DatePickerRanges", "NextYear");
		value["PreviousMonth"] = StiLocalization.Get("DatePickerRanges", "PreviousMonth");
		value["PreviousQuarter"] = StiLocalization.Get("DatePickerRanges", "PreviousQuarter");
		value["PreviousWeek"] = StiLocalization.Get("DatePickerRanges", "PreviousWeek");
		value["PreviousYear"] = StiLocalization.Get("DatePickerRanges", "PreviousYear");
		value["FirstQuarter"] = StiLocalization.Get("DatePickerRanges", "FirstQuarter");
		value["SecondQuarter"] = StiLocalization.Get("DatePickerRanges", "SecondQuarter");
		value["ThirdQuarter"] = StiLocalization.Get("DatePickerRanges", "ThirdQuarter");
		value["FourthQuarter"] = StiLocalization.Get("DatePickerRanges", "FourthQuarter");
		value["MonthToDate"] = StiLocalization.Get("DatePickerRanges", "MonthToDate");
		value["QuarterToDate"] = StiLocalization.Get("DatePickerRanges", "QuarterToDate");
		value["WeekToDate"] = StiLocalization.Get("DatePickerRanges", "WeekToDate");
		value["YearToDate"] = StiLocalization.Get("DatePickerRanges", "YearToDate");
		value["Today"] = StiLocalization.Get("DatePickerRanges", "Today");
		value["Tomorrow"] = StiLocalization.Get("DatePickerRanges", "Tomorrow");
		value["Yesterday"] = StiLocalization.Get("DatePickerRanges", "Yesterday");
		value["Last7Days"] = StiLocalization.Get("DatePickerRanges", "Last7Days");
		value["Last14Days"] = StiLocalization.Get("DatePickerRanges", "Last14Days");
		value["Last30Days"] = StiLocalization.Get("DatePickerRanges", "Last30Days");
		value["Resources"] = StiLocalization.Get("PropertyMain", "Resources");
		value["ResourcesToolTip"] = StiLocalization.Get("PropertyMain", "Resources");
		value["SaveFile"] = StiLocalization.Get("Cloud", "SaveFile");
		value["ButtonView"] = StiLocalization.Get("Cloud", "ButtonView");
		value["QuotaMaximumReportPagesCountExceeded"] = StiLocalization.Get("Notices", "QuotaMaximumReportPagesCountExceeded");
		value["QuotaMaximumDataRowsCountExceeded"] = StiLocalization.Get("Notices", "QuotaMaximumDataRowsCountExceeded");
		value["QuotaMaximumResourceSizeExceeded"] = StiLocalization.Get("Notices", "QuotaMaximumResourceSizeExceeded");
		value["QuotaMaximumResourcesCountExceeded"] = StiLocalization.Get("Notices", "QuotaMaximumResourcesCountExceeded");
		value["QuotaMaximumDataRowsCountExceeded"] = StiLocalization.Get("Notices", "QuotaMaximumDataRowsCountExceeded");
		value["QuotaMaximumRefreshCountExceeded"] = StiLocalization.Get("Notices", "QuotaMaximumRefreshCountExceeded");
		value["New"] = StiLocalization.Get("MainMenu", "menuFileNew").Replace("&", "");
		value["Edit"] = StiLocalization.Get("MainMenu", "menuEditEdit");
		value["DashboardSortSmallestToLargest"] = StiLocalization.Get("Dashboard", "SortSmallestToLargest");
		value["DashboardSortLargestToSmallest"] = StiLocalization.Get("Dashboard", "SortLargestToSmallest");
		value["DashboardSortAZ"] = StiLocalization.Get("Dashboard", "SortAZ");
		value["DashboardSortZA"] = StiLocalization.Get("Dashboard", "SortZA");
		value["DashboardSortOldestToNewest"] = StiLocalization.Get("Dashboard", "SortOldestToNewest");
		value["DashboardSortNewestToOldest"] = StiLocalization.Get("Dashboard", "SortNewestToOldest");
		value["FormBandNoSort"] = StiLocalization.Get("FormBand", "NoSort");
		value["DashboardNulls"] = StiLocalization.Get("Dashboard", "Nulls");
		value["DashboardBlanks"] = StiLocalization.Get("Dashboard", "Blanks");
		value["DashboardNumberFilters"] = StiLocalization.Get("Dashboard", "NumberFilters");
		value["DashboardDateFilters"] = StiLocalization.Get("Dashboard", "DateFilters");
		value["DashboardBooleanFilters"] = StiLocalization.Get("Dashboard", "BooleanFilters");
		value["DashboardStringFilters"] = StiLocalization.Get("Dashboard", "StringFilters");
		value["DashboardCustomFilter"] = StiLocalization.Get("Dashboard", "CustomFilter").Replace("&", "").Replace("...", "");
		value["DashboardNSelected"] = StiLocalization.Get("Dashboard", "NSelected");
		value["DashboardAllValue"] = "(" + StiLocalization.Get("Report", "RangeAll").Replace("&", "") + ")";
		value["DrillDown"] = StiLocalization.Get("Dashboard", "DrillDown");
		value["DrillUp"] = StiLocalization.Get("Dashboard", "DrillUp");
		value["DrillDownSelected"] = StiLocalization.Get("Dashboard", "DrillDownSelected");
		value["ReportSnapshot"] = StiLocalization.Get("Dashboard", "ReportSnapshot");
		value["Dashboard"] = StiLocalization.Get("Components", "StiDashboard");
		value["Report"] = StiLocalization.Get("Components", "StiReport");
		value["Sort"] = StiLocalization.Get("PropertyMain", "Sort");
		value["SortBy"] = StiLocalization.Get("PropertyMain", "SortBy");
		value["SortAsc"] = StiLocalization.Get("PropertyEnum", "StiSortDirectionAsc");
		value["SortDesc"] = StiLocalization.Get("PropertyEnum", "StiSortDirectionDesc");
		value["SortNone"] = StiLocalization.Get("PropertyEnum", "StiSortDirectionNone");
		value["Variation"] = StiLocalization.Get("PropertyEnum", "StiTargetModeVariation");
		value["DataNotFound"] = StiLocalization.Get("Errors", "DataNotFound");
		value["NoResult"] = StiLocalization.Get("Dashboard", "NoResult");
		value["Filter"] = StiLocalization.Get("PropertyMain", "Filter");
		value["Filters"] = StiLocalization.Get("PropertyMain", "Filters");
		value["AddFilter"] = StiLocalization.Get("FormBand", "AddFilter").Replace("&", "");
		value["RemoveFilter"] = StiLocalization.Get("FormBand", "RemoveFilter").Replace("&", "");
		value["FilterOn"] = StiLocalization.Get("PropertyMain", "FilterOn");
		value["NameTrue"] = StiLocalization.Get("FormFormatEditor", "nameTrue");
		value["NameFalse"] = StiLocalization.Get("FormFormatEditor", "nameFalse");
		value["FilterModeAnd"] = StiLocalization.Get("PropertyEnum", "StiFilterModeAnd");
		value["Expression"] = StiLocalization.Get("PropertyMain", "Expression");
		value["All"] = StiLocalization.Get("Report", "RangeAll");
		value["FilterMode"] = StiLocalization.Get("PropertyMain", "FilterMode");
		value["ConditionEqualTo"] = StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo");
		value["ConditionNotEqualTo"] = StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo");
		value["ConditionContaining"] = StiLocalization.Get("PropertyEnum", "StiFilterConditionContaining");
		value["ConditionNotContaining"] = StiLocalization.Get("PropertyEnum", "StiFilterConditionNotContaining");
		value["ConditionBeginningWith"] = StiLocalization.Get("PropertyEnum", "StiFilterConditionBeginningWith");
		value["ConditionEndingWith"] = StiLocalization.Get("PropertyEnum", "StiFilterConditionEndingWith");
		value["ConditionIsNull"] = StiLocalization.Get("PropertyEnum", "StiFilterConditionIsNull");
		value["ConditionIsNotNull"] = StiLocalization.Get("PropertyEnum", "StiFilterConditionIsNotNull");
		value["ConditionIsBlank"] = StiLocalization.Get("PropertyEnum", "StiFilterConditionIsBlank");
		value["ConditionIsNotBlank"] = StiLocalization.Get("PropertyEnum", "StiFilterConditionIsNotBlank");
		value["ConditionBetween"] = StiLocalization.Get("PropertyEnum", "StiFilterConditionBetween");
		value["ConditionNotBetween"] = StiLocalization.Get("PropertyEnum", "StiFilterConditionNotBetween");
		value["ConditionGreaterThan"] = StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThan");
		value["ConditionGreaterThanOrEqualTo"] = StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo");
		value["ConditionLessThan"] = StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThan");
		value["ConditionLessThanOrEqualTo"] = StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo");
		value["Refresh"] = StiLocalization.Get("PropertyMain", "Refresh");
		value["FullScreen"] = StiLocalization.Get("FormViewer", "FullScreen");
		value["Image"] = StiLocalization.Get("PropertyMain", "Image");
		value["Data"] = StiLocalization.Get("PropertyMain", "Data");
		value["Text"] = StiLocalization.Get("PropertyMain", "Text");
		value["RichText"] = StiLocalization.Get("Components", "StiRichText");
		value["PaperSize"] = StiLocalization.Get("PropertyMain", "PaperSize");
		value["Orientation"] = StiLocalization.Get("PropertyMain", "Orientation");
		value["Landscape"] = StiLocalization.Get("PropertyEnum", "StiPageOrientationLandscape");
		value["Portrait"] = StiLocalization.Get("PropertyEnum", "StiPageOrientationPortrait");
		value["Scale"] = StiLocalization.Get("PropertyMain", "Scale").Replace(":", "");
		value["DataType"] = StiLocalization.Get("PropertyMain", "DataType");
		value["AuthorizationWindowTitleLogin"] = StiLocalization.Get("Cloud", "WindowTitleLogin");
		value["AuthorizationWindowTitleSignUp"] = StiLocalization.Get("Cloud", "WindowTitleSignUp");
		value["AuthorizationWindowTitleForgotPassword"] = StiLocalization.Get("Cloud", "WindowTitleForgotPassword");
		value["AuthorizationTextUserName"] = StiLocalization.Get("Cloud", "TextUserName");
		value["AuthorizationTextPassword"] = StiLocalization.Get("Password", "StiSavePasswordForm");
		value["AuthorizationCheckBoxRememberMe"] = StiLocalization.Get("Cloud", "CheckBoxRememberMe");
		value["AuthorizationButtonLogin"] = StiLocalization.Get("Cloud", "Login");
		value["AuthorizationButtonSignUp"] = StiLocalization.Get("Cloud", "ButtonSignUp");
		value["NoticesYourTrialHasExpired"] = StiLocalization.Get("Notices", "YourTrialHasExpired");
		value["NoticesYouUsingTrialVersion"] = StiLocalization.Get("Notices", "YouUsingTrialVersion");
		key = StiMD5Helper.ComputeHash(localization);
		LocalizationItems.GetOrAdd(key, value);
		return value;
	}

	public static ArrayList GetDateRangesItems()
	{
		ArrayList arrayList = new ArrayList();
		foreach (StiDateRangeKind requestFromUserDateRange in StiOptions.Viewer.RequestFromUserDateRanges)
		{
			arrayList.Add(requestFromUserDateRange.ToString());
		}
		return arrayList;
	}

	public static ArrayList GetPaperSizes()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		ArrayList arrayList = new ArrayList();
		PaperSizeCollection val = StiOptions.Print.CustomPaperSizes;
		try
		{
			if (val == null)
			{
				PrinterSettings val2 = new PrinterSettings();
				val = val2.PaperSizes;
			}
			foreach (PaperSize item in val)
			{
				PaperSize val3 = item;
				if ((int)val3.Kind != 0)
				{
					arrayList.Add(val3.PaperName);
				}
			}
		}
		catch
		{
		}
		if (arrayList.Count == 0)
		{
			return new ArrayList(new string[10] { "Letter", "Tabloid", "Legal", "Statement", "Executive", "A3", "A4", "A5", "B4", "B5" });
		}
		return arrayList;
	}

	private void InitializePaperSizes()
	{
	}
}
