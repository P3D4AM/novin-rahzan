using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Base.Zip;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Xps.png")]
public class StiXpsExportService : StiExportService
{
	private struct StiXpsData
	{
		public double X;

		public double Y;

		public double Width;

		public double Height;

		public StiComponent Component;

		public double Right => X + Width;

		public double Bottom => Y + Height;
	}

	private const double hiToDpi = 0.9599999785423279;

	private StiReport report;

	private string fileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	private StiImageCache imageCache;

	private Hashtable pageImages;

	private Hashtable pageFonts;

	private Hashtable pageBookmarks;

	private int indexPage;

	private int xmlIndentation = 1;

	private PdfFonts pdfFont;

	private int precision_digits = 3;

	private bool reduceFontSize = true;

	private bool useImageComparer = true;

	private float imageQuality = 0.75f;

	private float imageResolution = 96f;

	private double fontCorrectValue = 1.279999666213996;

	private bool exportRtfTextAsImage = true;

	private Graphics graphicsForTextRenderer;

	private Image imageForGraphicsForTextRenderer;

	public override string DefaultExtension => "xps";

	public override StiExportFormat ExportFormat => StiExportFormat.Xps;

	public override string GroupCategory => "Document";

	public override int Position => 1;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeXpsFile");

	public override bool MultipleFiles => false;

	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		ExportXps(report, stream, settings as StiXpsExportSettings);
		InvokeExporting(100, 100, 1, 1);
	}

	public override void Export(StiReport report, string fileName, bool sendEMail, StiGuiMode guiMode)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		IStiFormRunner exportFormRunner = StiGuiOptions.GetExportFormRunner("StiXpsExportSetupForm", guiMode, base.OwnerWindow);
		try
		{
			exportFormRunner["CurrentPage"] = report.CurrentPrintPage;
			exportFormRunner["OpenAfterExportEnabled"] = !sendEMail;
			this.report = report;
			this.fileName = fileName;
			this.sendEMail = sendEMail;
			this.guiMode = guiMode;
			exportFormRunner.Complete += new StiShowDialogCompleteEventHandler(FormComplete);
			exportFormRunner.ShowDialog();
		}
		finally
		{
			((IDisposable)exportFormRunner)?.Dispose();
		}
	}

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "XpsFiles");
	}

	private void FormComplete(IStiFormRunner form, StiShowDialogCompleteEvetArgs e)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		if (!e.DialogResult)
		{
			base.IsStopped = true;
			return;
		}
		if (string.IsNullOrEmpty(fileName))
		{
			fileName = GetFileName(report, sendEMail);
		}
		if (fileName == null)
		{
			return;
		}
		StiFileUtils.ProcessReadOnly(fileName);
		try
		{
			using FileStream stream = new FileStream(fileName, FileMode.Create);
			StartProgress(guiMode);
			StiXpsExportSettings stiXpsExportSettings = new StiXpsExportSettings();
			stiXpsExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
			stiXpsExportSettings.ImageQuality = (float)form["ImageQuality"];
			stiXpsExportSettings.ImageResolution = (float)form["Resolution"];
			stiXpsExportSettings.ExportRtfTextAsImage = (bool)form["ExportRtfTextAsImage"];
			StiXpsExportSettings settings = stiXpsExportSettings;
			StartExport(report, stream, settings, sendEMail, (bool)form["OpenAfterExport"], fileName, guiMode);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private bool IsWordWrapSymbol(StringBuilder sb, int index)
	{
		char sym = ' ';
		if (index < sb.Length - 1)
		{
			sym = sb[index + 1];
		}
		return IsWordWrapSymbol(sb[index], sym);
	}

	private bool IsWordWrapSymbol(char sym1, char sym2)
	{
		UnicodeCategory unicodeCategory = char.GetUnicodeCategory(sym1);
		UnicodeCategory unicodeCategory2 = char.GetUnicodeCategory(sym2);
		if (unicodeCategory != UnicodeCategory.OpenPunctuation && (unicodeCategory != UnicodeCategory.ClosePunctuation || unicodeCategory2 == UnicodeCategory.OtherPunctuation) && unicodeCategory != UnicodeCategory.DashPunctuation && unicodeCategory != UnicodeCategory.InitialQuotePunctuation && unicodeCategory != UnicodeCategory.FinalQuotePunctuation)
		{
			return unicodeCategory == UnicodeCategory.SpaceSeparator;
		}
		return true;
	}

	private string FloatToString(double number)
	{
		int decimals = precision_digits;
		if (Math.Abs(number) < 1.0)
		{
			decimals = precision_digits + 2;
		}
		return Math.Round((decimal)number, decimals).ToString("G");
	}

	private void PrepareData()
	{
		pdfFont = new PdfFonts
		{
			standardPdfFonts = false,
			MaxSymbols = 32767,
			fontList = new ArrayList()
		};
		imageCache = new StiImageCache(useImageComparer, ImageFormat.Jpeg, imageQuality);
	}

	private float GetTabsSize(IStiTextOptions textOp, double sizeInPt, double currentPosition)
	{
		if (textOp == null || textOp.TextOptions == null)
		{
			return 0f;
		}
		double num = currentPosition;
		float num2 = 750f / (float)sizeInPt;
		float num3 = num2 * textOp.TextOptions.DistanceBetweenTabs;
		float num4 = num2 * textOp.TextOptions.FirstTabOffset + num3;
		if (currentPosition < (double)num4)
		{
			num = num4;
		}
		else if (textOp.TextOptions.DistanceBetweenTabs > 0f)
		{
			int num5 = (int)((currentPosition - (double)num4) / (double)num3);
			num5++;
			num = num4 + (float)num5 * num3;
		}
		return (float)(num - currentPosition);
	}

	internal static string ConvertToEscapeSequence(string value)
	{
		string text = "\r\n\b\f";
		string text2 = "rnbf";
		if (value == null)
		{
			return "";
		}
		string text3 = string.Empty;
		for (int i = 0; i < value.Length; i++)
		{
			bool flag = false;
			for (int j = 0; j < text.Length; j++)
			{
				if (value[i] == text[j])
				{
					text3 = text3 + "\\" + text2[j];
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				text3 += value[i];
			}
		}
		return text3;
	}

	private string GetLineStyleDash(StiPenStyle penStyle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected I4, but got Unknown
		return (penStyle - 1) switch
		{
			3 => "1.1 1.1", 
			0 => "3 1", 
			1 => "3 1 1 1", 
			2 => "3 1 1 1 1 1", 
			_ => string.Empty, 
		};
	}

	private void CheckGraphicsForTextRenderer()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		if (graphicsForTextRenderer == null)
		{
			if (StiOptions.Engine.FullTrust && StiOptions.Export.Pdf.AllowImportSystemLibraries)
			{
				graphicsForTextRenderer = Graphics.FromHwnd(IntPtr.Zero);
				return;
			}
			imageForGraphicsForTextRenderer = (Image)new Bitmap(1, 1);
			graphicsForTextRenderer = Graphics.FromImage(imageForGraphicsForTextRenderer);
		}
	}

	private MemoryStream WriteContentTypes()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8)
		{
			Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0),
			Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None)
		};
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("Types");
		xmlTextWriter.WriteAttributeString("xmlns", "http://schemas.openxmlformats.org/package/2006/content-types");
		xmlTextWriter.WriteStartElement("Default");
		xmlTextWriter.WriteAttributeString("Extension", "jpg");
		xmlTextWriter.WriteAttributeString("ContentType", "image/jpeg");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Default");
		xmlTextWriter.WriteAttributeString("Extension", "png");
		xmlTextWriter.WriteAttributeString("ContentType", "image/png");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Default");
		xmlTextWriter.WriteAttributeString("Extension", "rels");
		xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-package.relationships+xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Default");
		xmlTextWriter.WriteAttributeString("Extension", "xml");
		xmlTextWriter.WriteAttributeString("ContentType", "application/xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Default");
		xmlTextWriter.WriteAttributeString("Extension", "fdseq");
		xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.ms-package.xps-fixeddocumentsequence+xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Default");
		xmlTextWriter.WriteAttributeString("Extension", "fpage");
		xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.ms-package.xps-fixedpage+xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Default");
		xmlTextWriter.WriteAttributeString("Extension", "ttf");
		xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.ms-opentype");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Default");
		xmlTextWriter.WriteAttributeString("Extension", "fdoc");
		xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.ms-package.xps-fixeddocument+xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Override");
		xmlTextWriter.WriteAttributeString("PartName", "/docProps/core.xml");
		xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-package.core-properties+xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteMainRels()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8)
		{
			Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0),
			Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None)
		};
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("Relationships");
		xmlTextWriter.WriteAttributeString("xmlns", "http://schemas.openxmlformats.org/package/2006/relationships");
		xmlTextWriter.WriteStartElement("Relationship");
		xmlTextWriter.WriteAttributeString("Id", "rId1");
		xmlTextWriter.WriteAttributeString("Type", "http://schemas.microsoft.com/xps/2005/06/fixedrepresentation");
		xmlTextWriter.WriteAttributeString("Target", "FixedDocSeq.fdseq");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Relationship");
		xmlTextWriter.WriteAttributeString("Id", "rId2");
		xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties");
		xmlTextWriter.WriteAttributeString("Target", "docProps/core.xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteDocPropsCore(StiReport report)
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8)
		{
			Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0),
			Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None)
		};
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("cp:coreProperties");
		xmlTextWriter.WriteAttributeString("xmlns:cp", "http://schemas.openxmlformats.org/package/2006/metadata/core-properties");
		xmlTextWriter.WriteAttributeString("xmlns:dc", "http://purl.org/dc/elements/1.1/");
		xmlTextWriter.WriteAttributeString("xmlns:dcterms", "http://purl.org/dc/terms/");
		xmlTextWriter.WriteAttributeString("xmlns:dcmitype", "http://purl.org/dc/dcmitype/");
		xmlTextWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
		string text = $"{DateTime.Now:yyyy-MM-ddTHH:mm:ssZ}";
		xmlTextWriter.WriteStartElement("dcterms:created");
		xmlTextWriter.WriteAttributeString("xsi:type", "dcterms:W3CDTF");
		xmlTextWriter.WriteString(text);
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("dcterms:modified");
		xmlTextWriter.WriteAttributeString("xsi:type", "dcterms:W3CDTF");
		xmlTextWriter.WriteString(text);
		xmlTextWriter.WriteEndElement();
		if (!string.IsNullOrEmpty(report.ReportName))
		{
			xmlTextWriter.WriteElementString("dc:title", report.ReportName);
		}
		if (!string.IsNullOrEmpty(report.ReportAlias))
		{
			xmlTextWriter.WriteElementString("dc:subject", report.ReportAlias);
		}
		if (!string.IsNullOrEmpty(report.ReportDescription))
		{
			xmlTextWriter.WriteElementString("dc:description", report.ReportDescription);
		}
		xmlTextWriter.WriteElementString("dc:creator", StiExportUtils.GetReportVersion(report));
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteFixedDocSeq()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.Unicode)
		{
			Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0),
			Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None)
		};
		xmlTextWriter.WriteStartElement("FixedDocumentSequence");
		xmlTextWriter.WriteAttributeString("xmlns", "http://schemas.microsoft.com/xps/2005/06");
		xmlTextWriter.WriteStartElement("DocumentReference");
		xmlTextWriter.WriteAttributeString("Source", "/Documents/1/FixedDoc.fdoc");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteFixedDoc(StiPagesCollection pages)
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.Unicode)
		{
			Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0),
			Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None)
		};
		xmlTextWriter.WriteStartElement("FixedDocument");
		xmlTextWriter.WriteAttributeString("xmlns", "http://schemas.microsoft.com/xps/2005/06");
		for (int i = 0; i < pages.Count; i++)
		{
			xmlTextWriter.WriteStartElement("PageContent");
			xmlTextWriter.WriteAttributeString("Source", $"Pages/{i + 1}.fpage");
			List<string> list = new List<string>();
			foreach (DictionaryEntry pageBookmark in pageBookmarks)
			{
				if ((int)pageBookmark.Value == i)
				{
					list.Add((string)pageBookmark.Key);
				}
			}
			if (list.Count > 0)
			{
				xmlTextWriter.WriteStartElement("PageContent.LinkTargets");
				foreach (string item in list)
				{
					xmlTextWriter.WriteStartElement("LinkTarget");
					xmlTextWriter.WriteAttributeString("Name", item);
					xmlTextWriter.WriteEndElement();
				}
				xmlTextWriter.WriteFullEndElement();
			}
			xmlTextWriter.WriteFullEndElement();
		}
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WritePage(StiPage page, StiReport report)
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.Unicode)
		{
			Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0),
			Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None)
		};
		double num = 0.9599999785423279 * report.Unit.ConvertToHInches(page.PageHeight * (double)page.SegmentPerHeight);
		double num2 = 0.9599999785423279 * report.Unit.ConvertToHInches(page.PageWidth * (double)page.SegmentPerWidth);
		double num3 = 0.9599999785423279 * report.Unit.ConvertToHInches(page.Margins.Left);
		double num4 = 0.9599999785423279 * report.Unit.ConvertToHInches(page.Margins.Right);
		double num5 = 0.9599999785423279 * report.Unit.ConvertToHInches(page.Margins.Top);
		double num6 = 0.9599999785423279 * report.Unit.ConvertToHInches(page.Margins.Bottom);
		xmlTextWriter.WriteStartElement("FixedPage");
		xmlTextWriter.WriteAttributeString("xmlns", "http://schemas.microsoft.com/xps/2005/06");
		xmlTextWriter.WriteAttributeString("Width", FloatToString(num2) ?? "");
		xmlTextWriter.WriteAttributeString("Height", FloatToString(num) ?? "");
		xmlTextWriter.WriteAttributeString("xml:lang", "en-EN");
		xmlTextWriter.WriteStartElement("Canvas");
		WriteHyperlinksData(xmlTextWriter, page, check: false);
		if (page.Brush != null)
		{
			StiXpsData stiXpsData = default(StiXpsData);
			stiXpsData.X = 0.0;
			stiXpsData.Y = 0.0;
			stiXpsData.Width = num2;
			stiXpsData.Height = num;
			stiXpsData.Component = new StiContainer();
			StiXpsData data = stiXpsData;
			(data.Component as StiContainer).Brush = page.Brush;
			(data.Component as StiContainer).Border = null;
			WriteBorderFill(xmlTextWriter, data);
		}
		WriteWatermark(xmlTextWriter, page.Watermark, behind: true, num2, num);
		foreach (StiComponent component in page.Components)
		{
			StiText stiText = component as StiText;
			if (component.Enabled && component.Width > 0.0 && component.Height > 0.0 && stiText != null && stiText.ExceedMargins != 0 && !(stiText.Brush is StiEmptyBrush))
			{
				double num7 = 0.9599999785423279 * report.Unit.ConvertToHInches(component.Left);
				double num8 = 0.9599999785423279 * report.Unit.ConvertToHInches(component.Top);
				double num9 = 0.9599999785423279 * report.Unit.ConvertToHInches(component.Right);
				double num10 = 0.9599999785423279 * report.Unit.ConvertToHInches(component.Bottom);
				StiXpsData stiXpsData = default(StiXpsData);
				stiXpsData.X = num7 + num3;
				stiXpsData.Y = num8 + num5;
				stiXpsData.Width = num9 - num7;
				stiXpsData.Height = num10 - num8;
				stiXpsData.Component = component;
				StiXpsData data2 = stiXpsData;
				if ((stiText.ExceedMargins & StiExceedMargins.Left) > StiExceedMargins.None)
				{
					data2.Width = data2.Right;
					data2.X = 0.0;
				}
				if ((stiText.ExceedMargins & StiExceedMargins.Right) > StiExceedMargins.None)
				{
					data2.Width = num2 - data2.X;
				}
				if ((stiText.ExceedMargins & StiExceedMargins.Top) > StiExceedMargins.None)
				{
					data2.Height = data2.Bottom;
					data2.Y = 0.0;
				}
				if ((stiText.ExceedMargins & StiExceedMargins.Bottom) > StiExceedMargins.None)
				{
					data2.Height = num - data2.Y;
				}
				WriteBorderFill(xmlTextWriter, data2);
			}
		}
		foreach (StiComponent component2 in page.Components)
		{
			bool flag = component2.Width > 0.0 && component2.Height > 0.0;
			if (component2.Enabled && flag)
			{
				double num11 = 0.9599999785423279 * report.Unit.ConvertToHInches(component2.Left);
				double num12 = 0.9599999785423279 * report.Unit.ConvertToHInches(component2.Top);
				double num13 = 0.9599999785423279 * report.Unit.ConvertToHInches(component2.Right);
				double num14 = 0.9599999785423279 * report.Unit.ConvertToHInches(component2.Bottom);
				StiXpsData stiXpsData = default(StiXpsData);
				stiXpsData.X = num11;
				stiXpsData.Y = num12;
				stiXpsData.Width = num13 - num11;
				stiXpsData.Height = num14 - num12;
				StiXpsData stiXpsData2 = stiXpsData;
				stiXpsData2.Y += num5;
				stiXpsData2.X += num3;
				stiXpsData2.Component = component2;
				if (!(component2 is StiShape))
				{
					WriteBorderFill(xmlTextWriter, stiXpsData2);
				}
				if (component2 is StiText && !component2.IsExportAsImage(StiExportFormat.Xps))
				{
					WriteText(xmlTextWriter, stiXpsData2);
				}
				if (component2.IsExportAsImage(StiExportFormat.Xps))
				{
					WriteImage(xmlTextWriter, stiXpsData2);
				}
				WriteBorderStroke(xmlTextWriter, stiXpsData2);
			}
		}
		if (page.Border != null)
		{
			StiXpsData stiXpsData = default(StiXpsData);
			stiXpsData.X = num3;
			stiXpsData.Y = num5;
			stiXpsData.Width = num2 - num3 - num4;
			stiXpsData.Height = num - num5 - num6;
			stiXpsData.Component = new StiContainer();
			StiXpsData data3 = stiXpsData;
			(data3.Component as StiContainer).Border = page.Border;
			WriteBorderStroke(xmlTextWriter, data3);
		}
		WriteWatermark(xmlTextWriter, page.Watermark, behind: false, num2, num);
		StiLicenseKey licenseKey = StiLicenseKeyValidator.GetLicenseKey();
		bool flag2 = (!StiLicenseKeyValidator.IsValidInReportsDesignerOrOnPlatform((StiProductIdent)2, licenseKey) || !StiDesignerAppStatus.IsRunning) && !StiLicenseKeyValidator.IsValidOnNetFramework(licenseKey);
		if (!typeof(StiLicense).AssemblyQualifiedName.Contains("PublicKeyToken=ebe6666cba19647a"))
		{
			flag2 = true;
		}
		if (!flag2)
		{
			try
			{
				using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(512);
				using SHA1CryptoServiceProvider halg = new SHA1CryptoServiceProvider();
				rSACryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>2GCA4rfVor/2q7m+NVisL3ZFVvnklg14zeP0F+yNpBdoSm9iuOUBLodsOH6I3Xb0j5/OznYKFpii35RcjiPvww7Uthz1he2G8PGCvFMKuA4Wzgp5Kdv0uR7zSQ8J9lA/rZ7u8wh+h72umBku1pLFL2nvJIT56u4Qe7+OJFLrqWk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
				flag2 = !rSACryptoServiceProvider.VerifyData(licenseKey.GetCheckBytes(), halg, licenseKey.GetSignatureBytes());
			}
			catch (Exception)
			{
				flag2 = true;
			}
		}
		if (flag2)
		{
			double num15 = num2 / 794.0 * 1.75;
			double num16 = num / 1123.0 * 1.75;
			if (num15 > num16)
			{
				num15 = num16;
			}
			else
			{
				num16 = num15;
			}
			xmlTextWriter.WriteStartElement("Canvas");
			xmlTextWriter.WriteAttributeString("RenderTransform", $"{FloatToString(num15)},0,0,{FloatToString(num16)},{FloatToString(num2 / 2.0)},{FloatToString(num / 2.0)}");
			xmlTextWriter.WriteStartElement("Canvas");
			xmlTextWriter.WriteAttributeString("RenderTransform", "0.707,-0.707,0.707,0.707,0,0");
			xmlTextWriter.WriteStartElement("Path");
			xmlTextWriter.WriteAttributeString("Stroke", $"#{1080321124:X8}");
			xmlTextWriter.WriteAttributeString("StrokeThickness", FloatToString(15.0) ?? "");
			xmlTextWriter.WriteAttributeString("StrokeStartLineCap", "Round");
			xmlTextWriter.WriteAttributeString("StrokeEndLineCap", "Round");
			xmlTextWriter.WriteAttributeString("StrokeLineJoin", "Round");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("M 40,0 L 40,100 M 0,100 L 80,100 ");
			stringBuilder.Append("M 100,0 L 100,70 M 100,45 L 120,65 L 130,72 L 140,68 ");
			stringBuilder.Append("M 170,0 L 170,70 M 169,100 L 171,100 ");
			stringBuilder.Append("M 215,60 L 222,69 L 232,71 L 255,70 L 265,60 L 265,5 L 270,0 M 265,44 L 220,31 L 212,20 L 212,10 L 225,0 L 235,0 L 250,5 L 265,18 ");
			stringBuilder.Append("M 310,0 L 310,100 ");
			xmlTextWriter.WriteAttributeString("Data", stringBuilder.ToString());
			xmlTextWriter.WriteAttributeString("RenderTransform", "1,0,0,-1,-155,50");
			xmlTextWriter.WriteFullEndElement();
			xmlTextWriter.WriteFullEndElement();
			xmlTextWriter.WriteFullEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private void WriteBorderFill(XmlTextWriter writer, StiXpsData data)
	{
		Color color = Color.Transparent;
		IStiBrush stiBrush = data.Component as IStiBrush;
		if (stiBrush != null && stiBrush.Brush != null)
		{
			color = StiBrush.ToColor(stiBrush.Brush);
		}
		if (data.Component is StiRichText stiRichText)
		{
			color = stiRichText.BackColor;
		}
		if (color.A != 0)
		{
			writer.WriteStartElement("Path");
			writer.WriteAttributeString("Data", string.Format("M {0},{1} L {0},{3} L {2},{3} L {2},{1} Z ", FloatToString(data.X), FloatToString(data.Y), FloatToString(data.X + data.Width), FloatToString(data.Y + data.Height)));
			if (!(data.Component is StiText) && !(data.Component is StiImage))
			{
				WriteHyperlinksData(writer, data.Component, check: false);
			}
			writer.WriteStartElement("Path.Fill");
			if (stiBrush != null && stiBrush.Brush != null && stiBrush.Brush is StiGradientBrush)
			{
				StiBrush brush = stiBrush.Brush;
				StiGradientBrush val = (StiGradientBrush)(object)((brush is StiGradientBrush) ? brush : null);
				double num = 1.0;
				double num2 = 1.0;
				double num3 = val.Angle;
				if (num3 < 0.0)
				{
					num3 += 360.0;
				}
				if (num3 >= 270.0 && num3 < 360.0)
				{
					num3 = 360.0 - num3;
					num2 = -1.0;
				}
				if (num3 >= 180.0 && num3 < 270.0)
				{
					num3 -= 180.0;
					num2 = -1.0;
					num = -1.0;
				}
				if (num3 >= 90.0 && num3 < 180.0)
				{
					num3 = 180.0 - num3;
					num = -1.0;
				}
				num3 = num3 * Math.PI / 180.0;
				double num4 = data.X + data.Width / 2.0;
				double num5 = data.Y + data.Height / 2.0;
				double num6 = Math.Sqrt(data.Width * data.Width + data.Height * data.Height) / 2.0;
				double num7 = Math.Atan2(data.Height, data.Width);
				double a = Math.PI / 2.0 - num3 + num7;
				double num8 = num6 * Math.Sin(a);
				double num9 = num8 * Math.Cos(num3) * num;
				double num10 = num8 * Math.Sin(num3) * num2;
				double number = num4 - num9;
				double number2 = num4 + num9;
				double number3 = num5 - num10;
				double number4 = num5 + num10;
				writer.WriteStartElement("LinearGradientBrush");
				writer.WriteAttributeString("MappingMode", "Absolute");
				writer.WriteAttributeString("StartPoint", FloatToString(number) + "," + FloatToString(number3));
				writer.WriteAttributeString("EndPoint", FloatToString(number2) + "," + FloatToString(number4));
				writer.WriteStartElement("LinearGradientBrush.GradientStops");
				writer.WriteStartElement("GradientStop");
				writer.WriteAttributeString("Color", $"#{val.StartColor.ToArgb():X8}");
				writer.WriteAttributeString("Offset", "0");
				writer.WriteEndElement();
				writer.WriteStartElement("GradientStop");
				writer.WriteAttributeString("Color", $"#{val.EndColor.ToArgb():X8}");
				writer.WriteAttributeString("Offset", "1");
				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
			else
			{
				writer.WriteStartElement("SolidColorBrush");
				writer.WriteAttributeString("Color", $"#{color.ToArgb():X8}");
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
			writer.WriteFullEndElement();
		}
		else if (WriteHyperlinksData(writer, data.Component, check: true) && !(data.Component is StiText) && !(data.Component is StiImage))
		{
			writer.WriteStartElement("Path");
			writer.WriteAttributeString("Data", string.Format("M {0},{1} L {0},{3} L {2},{3} L {2},{1} Z ", FloatToString(data.X), FloatToString(data.Y), FloatToString(data.X + data.Width), FloatToString(data.Y + data.Height)));
			WriteHyperlinksData(writer, data.Component, check: false);
			writer.WriteEndElement();
		}
	}

	private void WriteBorderStroke(XmlTextWriter writer, StiXpsData data)
	{
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Invalid comparison between Unknown and I4
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Invalid comparison between Unknown and I4
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Invalid comparison between Unknown and I4
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Invalid comparison between Unknown and I4
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Invalid comparison between Unknown and I4
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Invalid comparison between Unknown and I4
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Invalid comparison between Unknown and I4
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Invalid comparison between Unknown and I4
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Invalid comparison between Unknown and I4
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0772: Unknown result type (might be due to invalid IL or missing references)
		//IL_0783: Expected O, but got Unknown
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Invalid comparison between Unknown and I4
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_061d: Invalid comparison between Unknown and I4
		if (!(data.Component is IStiBorder stiBorder))
		{
			return;
		}
		if (stiBorder.Border.DropShadow && stiBorder.Border.ShadowBrush != null)
		{
			Color color = StiBrush.ToColor(stiBorder.Border.ShadowBrush);
			if (color.A != 0)
			{
				double shadowSize = stiBorder.Border.ShadowSize;
				writer.WriteStartElement("Path");
				writer.WriteAttributeString("Data", string.Format("M {0},{4} L {1},{4} L {1},{3} L {2},{3} L {2},{5} L {0},{5} Z ", FloatToString(data.X + shadowSize), FloatToString(data.X + data.Width), FloatToString(data.X + data.Width + shadowSize), FloatToString(data.Y + shadowSize), FloatToString(data.Y + data.Height), FloatToString(data.Y + data.Height + shadowSize)));
				writer.WriteStartElement("Path.Fill");
				writer.WriteStartElement("SolidColorBrush");
				writer.WriteAttributeString("Color", $"#{color.ToArgb():X8}");
				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.WriteFullEndElement();
			}
		}
		StiBorder border = stiBorder.Border;
		StiAdvancedBorder val = (StiAdvancedBorder)(object)((border is StiAdvancedBorder) ? border : null);
		bool flag = val != (StiAdvancedBorder)null;
		StiRectanglePrimitive stiRectanglePrimitive = new StiRectanglePrimitive();
		if (data.Component is StiRectanglePrimitive)
		{
			stiRectanglePrimitive = data.Component as StiRectanglePrimitive;
		}
		bool flag2 = stiBorder.Border.IsLeftBorderSidePresent && stiRectanglePrimitive.LeftSide;
		bool flag3 = stiBorder.Border.IsRightBorderSidePresent && stiRectanglePrimitive.RightSide;
		bool flag4 = stiBorder.Border.IsTopBorderSidePresent && stiRectanglePrimitive.TopSide;
		bool flag5 = stiBorder.Border.IsBottomBorderSidePresent && stiRectanglePrimitive.BottomSide;
		bool flag6 = (int)stiBorder.Border.Side != 0 && (int)stiBorder.Border.Style != 6;
		if (flag)
		{
			flag6 = (int)((StiBorder)val).Side > 0;
		}
		StiBorderSide val2 = new StiBorderSide(stiBorder.Border.Color, stiBorder.Border.Size, stiBorder.Border.Style);
		double num = 0.0;
		if ((int)val2.Style == 5)
		{
			num = 0.8999999761581421;
		}
		if (!flag6)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (flag2)
		{
			if (flag)
			{
				val2 = val.LeftSide;
				num = (((int)val2.Style == 5) ? 0.9f : 0f);
			}
			string text = $"M {FloatToString(data.X - num)},{FloatToString(data.Y - num)} L {FloatToString(data.X - num)},{FloatToString(data.Y + num + data.Height)} ";
			if ((int)val2.Style == 5)
			{
				text += $"M {FloatToString(data.X + num)},{FloatToString(data.Y + (flag4 ? num : (0.0 - num)))} L {FloatToString(data.X + num)},{FloatToString(data.Y + (flag5 ? (0.0 - num) : num) + data.Height)} ";
			}
			if (flag)
			{
				WriteBorderSide(writer, val2, text);
			}
			else
			{
				stringBuilder.Append(text);
			}
		}
		if (flag3)
		{
			if (flag)
			{
				val2 = val.RightSide;
				num = (((int)val2.Style == 5) ? 0.9f : 0f);
			}
			string text2 = $"M {FloatToString(data.X + num + data.Width)},{FloatToString(data.Y - num)} L {FloatToString(data.X + num + data.Width)},{FloatToString(data.Y + num + data.Height)} ";
			if ((int)val2.Style == 5)
			{
				text2 += $"M {FloatToString(data.X - num + data.Width)},{FloatToString(data.Y + (flag4 ? num : (0.0 - num)))} L {FloatToString(data.X - num + data.Width)},{FloatToString(data.Y + (flag5 ? (0.0 - num) : num) + data.Height)} ";
			}
			if (flag)
			{
				WriteBorderSide(writer, val2, text2);
			}
			else
			{
				stringBuilder.Append(text2);
			}
		}
		if (flag4)
		{
			if (flag)
			{
				val2 = val.TopSide;
				num = (((int)val2.Style == 5) ? 0.9f : 0f);
			}
			string text3 = $"M {FloatToString(data.X - num)},{FloatToString(data.Y - num)} L {FloatToString(data.X + num + data.Width)},{FloatToString(data.Y - num)} ";
			if ((int)val2.Style == 5)
			{
				text3 += $"M {FloatToString(data.X + (flag2 ? num : (0.0 - num)))},{FloatToString(data.Y + num)} L {FloatToString(data.X + (flag3 ? (0.0 - num) : num) + data.Width)},{FloatToString(data.Y + num)} ";
			}
			if (flag)
			{
				WriteBorderSide(writer, val2, text3);
			}
			else
			{
				stringBuilder.Append(text3);
			}
		}
		if (flag5)
		{
			if (flag)
			{
				val2 = val.BottomSide;
				num = (((int)val2.Style == 5) ? 0.9f : 0f);
			}
			string text4 = $"M {FloatToString(data.X - num)},{FloatToString(data.Y + num + data.Height)} L {FloatToString(data.X + num + data.Width)},{FloatToString(data.Y + num + data.Height)} ";
			if ((int)val2.Style == 5)
			{
				text4 += $"M {FloatToString(data.X + (flag2 ? num : (0.0 - num)))},{FloatToString(data.Y - num + data.Height)} L {FloatToString(data.X + (flag3 ? (0.0 - num) : num) + data.Width)},{FloatToString(data.Y - num + data.Height)} ";
			}
			if (flag)
			{
				WriteBorderSide(writer, val2, text4);
			}
			else
			{
				stringBuilder.Append(text4);
			}
		}
		if (!flag)
		{
			WriteBorderSide(writer, new StiBorderSide(stiBorder.Border.Color, stiBorder.Border.Size, stiBorder.Border.Style), stringBuilder.ToString());
		}
	}

	private void WriteBorderSide(XmlTextWriter writer, StiBorderSide border, string path)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		double num = border.Size;
		if ((int)border.Style == 5)
		{
			num = 1.0;
		}
		double num2 = num * 0.9;
		Color color = border.Color;
		string lineStyleDash = GetLineStyleDash(border.Style);
		writer.WriteStartElement("Path");
		writer.WriteAttributeString("Stroke", $"#{color.ToArgb():X8}");
		writer.WriteAttributeString("StrokeThickness", $"{num2}");
		writer.WriteAttributeString("StrokeStartLineCap", "Square");
		writer.WriteAttributeString("StrokeEndLineCap", "Square");
		if (lineStyleDash != string.Empty)
		{
			writer.WriteAttributeString("StrokeDashArray", lineStyleDash);
			writer.WriteAttributeString("StrokeDashCap", "Flat");
		}
		writer.WriteAttributeString("Data", path);
		writer.WriteFullEndElement();
	}

	private bool WriteHyperlinksData(XmlTextWriter writer, StiComponent component, bool check)
	{
		bool result = false;
		if (component.BookmarkValue != null && (string)component.BookmarkValue != "")
		{
			string text = StiNameValidator.CorrectName(component.BookmarkValue.ToString(), report);
			if (!check && !pageBookmarks.ContainsKey(text))
			{
				writer.WriteAttributeString("Name", text);
				pageBookmarks[text] = indexPage;
			}
			result = true;
		}
		if (component.HyperlinkValue != null && component.HyperlinkValue.ToString().Trim().Length > 0 && !component.HyperlinkValue.ToString().Trim().StartsWith("javascript:"))
		{
			string text2 = component.HyperlinkValue.ToString().Trim();
			if (!check)
			{
				if (text2.StartsWith("#"))
				{
					text2 = StiNameValidator.CorrectName(text2.Substring(1), report);
					writer.WriteAttributeString("FixedPage.NavigateUri", "../FixedDoc.fdoc#" + text2);
				}
				else
				{
					writer.WriteAttributeString("FixedPage.NavigateUri", text2);
				}
			}
			result = true;
		}
		return result;
	}

	private void WriteText(XmlTextWriter writer, StiXpsData pp)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Invalid comparison between Unknown and I4
		//IL_106d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1073: Invalid comparison between Unknown and I4
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_1157: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10de: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e1: Invalid comparison between Unknown and I4
		//IL_10fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_10fd: Invalid comparison between Unknown and I4
		//IL_10ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0915: Unknown result type (might be due to invalid IL or missing references)
		//IL_091a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0943: Unknown result type (might be due to invalid IL or missing references)
		//IL_094e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0951: Invalid comparison between Unknown and I4
		//IL_096a: Unknown result type (might be due to invalid IL or missing references)
		//IL_096d: Invalid comparison between Unknown and I4
		//IL_0941: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afc: Invalid comparison between Unknown and I4
		//IL_0acb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b15: Invalid comparison between Unknown and I4
		//IL_0b21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b24: Invalid comparison between Unknown and I4
		//IL_0ae8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af4: Invalid comparison between Unknown and I4
		//IL_0aed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af7: Unknown result type (might be due to invalid IL or missing references)
		IStiText stiText = pp.Component as IStiText;
		StiText stiText2 = pp.Component as StiText;
		IStiFont stiFont = pp.Component as IStiFont;
		IStiTextBrush stiTextBrush = pp.Component as IStiTextBrush;
		IStiTextOptions stiTextOptions = pp.Component as IStiTextOptions;
		IStiTextHorAlignment stiTextHorAlignment = pp.Component as IStiTextHorAlignment;
		IStiVertAlignment stiVertAlignment = pp.Component as IStiVertAlignment;
		bool flag = stiTextOptions != null && stiTextOptions.TextOptions.WordWrap;
		bool flag2 = stiTextHorAlignment != null && (int)stiTextHorAlignment.HorAlignment == 3;
		bool rightToLeft = stiTextOptions != null && stiTextOptions.TextOptions != null && stiTextOptions.TextOptions.RightToLeft;
		int num = 0;
		float num2 = 0f;
		int num3 = 0;
		double number = 0.0;
		if (stiFont != null)
		{
			num3 = pdfFont.GetFontNumber(stiFont.Font);
			number = (double)stiFont.Font.SizeInPoints * fontCorrectValue;
			pageFonts[num3] = num3;
		}
		if (stiText == null)
		{
			return;
		}
		Color color = Color.Transparent;
		if (stiTextBrush != null)
		{
			color = StiBrush.ToColor(stiTextBrush.TextBrush);
		}
		double num4 = 1.0;
		if (stiFont != null)
		{
			num4 = stiFont.Font.SizeInPoints;
		}
		double graphicsScale = StiDpiHelper.GraphicsScale;
		double num5 = num4 * fontCorrectValue * (double)pdfFont.tmASC / 1000.0 * graphicsScale;
		double num6 = num4 * fontCorrectValue * (double)pdfFont.tmDESC / 1000.0 * -1.0 * graphicsScale;
		double num7 = num4 * fontCorrectValue * (double)pdfFont.tmExternal / 1000.0 * graphicsScale;
		double num8 = num4 * fontCorrectValue * (double)pdfFont.UnderscoreSize / 1000.0 * graphicsScale;
		double num9 = num4 * fontCorrectValue * (double)pdfFont.UnderscorePosition / 1000.0 * graphicsScale;
		double num10 = num4 * fontCorrectValue * (double)pdfFont.StrikeoutSize / 1000.0 * graphicsScale;
		double num11 = num4 * fontCorrectValue * (double)pdfFont.StrikeoutPosition / 1000.0 * graphicsScale;
		double num12 = (num5 - num6 + num7) * (double)stiText2.LineSpacing;
		double num13 = num5;
		double num14 = num6;
		double left = stiText2.Margins.Left;
		double right = stiText2.Margins.Right;
		double top = stiText2.Margins.Top;
		double bottom = stiText2.Margins.Bottom;
		double num15 = pp.X + left;
		double num16 = pp.Y + bottom;
		double num17 = pp.Width - left - right;
		double num18 = pp.Height - top - bottom;
		float num19 = 2f;
		float num20 = 1f;
		num15 += (double)num19;
		num17 -= (double)(num19 * 2f);
		num16 += (double)num20;
		num18 -= (double)(num20 * 2f);
		StringBuilder stringBuilder = new StringBuilder(stiText.Text);
		stringBuilder.Replace("\r", "");
		if (num17 > 0.0 && stringBuilder.Length > 0)
		{
			writer.WriteStartElement("Canvas");
			writer.WriteAttributeString("Clip", string.Format("M {0},{1} L {0},{3} L {2},{3} L {2},{1} Z ", FloatToString(pp.X), FloatToString(pp.Y), FloatToString(pp.X + pp.Width), FloatToString(pp.Y + pp.Height)));
			WriteHyperlinksData(writer, pp.Component, check: false);
			List<string> list = null;
			if (flag2)
			{
				RectangleD val = stiText2.Page.Unit.ConvertToHInches(pp.Component.ComponentToPage(pp.Component.ClientRectangle));
				val = stiText2.ConvertTextMargins(val, convert: false);
				val = stiText2.ConvertTextBorders(val, convert: false);
				((RectangleD)(ref val)).Width = ((RectangleD)(ref val)).Width / StiDpiHelper.DeviceCapsScale;
				((RectangleD)(ref val)).Height = ((RectangleD)(ref val)).Height / StiDpiHelper.DeviceCapsScale;
				CheckGraphicsForTextRenderer();
				list = StiTextDrawing.SplitTextWordwrapWidth(stringBuilder.ToString(), graphicsForTextRenderer, stiText2.Font, val);
				flag = false;
			}
			else
			{
				list = StiExportUtils.SplitString(stringBuilder.ToString(), removeControl: true);
			}
			num = list.Count;
			double num21 = num15;
			double num22 = num16;
			double num23 = num17;
			double num24 = num18;
			if (stiTextOptions != null)
			{
				num2 = stiTextOptions.TextOptions.Angle;
				if (num2 != 0f)
				{
					if ((num2 > 45f && num2 < 135f) || (num2 > 225f && num2 < 315f))
					{
						double num25 = num17;
						num17 = num18;
						num18 = num25;
					}
					num15 = (0.0 - num17) / 2.0;
					num16 = (0.0 - num18) / 2.0;
				}
			}
			writer.WriteStartElement("Canvas");
			if (num2 != 0f)
			{
				double num26 = (double)(0f - num2) * Math.PI / 180.0;
				writer.WriteAttributeString("RenderTransform", $"{FloatToString(Math.Cos(num26))},{FloatToString(Math.Sin(num26))},{FloatToString(0.0 - Math.Sin(num26))},{FloatToString(Math.Cos(num26))},{FloatToString(num21 + num23 / 2.0)},{FloatToString(num22 + num24 / 2.0)}");
			}
			if (flag)
			{
				for (int i = 0; i < list.Count; i++)
				{
					string text = list[i];
					StringBuilder stringBuilder2 = new StringBuilder();
					int j = 0;
					while (j < text.Length)
					{
						if (char.IsWhiteSpace(text[j]))
						{
							stringBuilder2.Append(text[j]);
							j++;
							continue;
						}
						StringBuilder stringBuilder3 = new StringBuilder();
						for (; j < text.Length && !char.IsWhiteSpace(text[j]); j++)
						{
							stringBuilder3.Append(text[j]);
						}
						StringBuilder stringBuilder4 = StiBidirectionalConvert2.ConvertStringBuilder(stringBuilder3, rightToLeft: false);
						stringBuilder2.Append((object?)stringBuilder4);
						int num27 = stringBuilder3.Length - stringBuilder4.Length;
						if (num27 > 0)
						{
							stringBuilder2.Append('\0', num27);
						}
					}
					int[] array = new int[text.Length];
					for (int k = 0; k < text.Length; k++)
					{
						array[k] = pdfFont.UnicodeMap[(uint)stringBuilder2[k]];
					}
					int[] array2 = new int[text.Length];
					int[] array3 = new int[text.Length];
					int num28 = 0;
					float num29 = 0f;
					for (int l = 0; l < array.Length; l++)
					{
						if (array[l] >= 32)
						{
							num29 += (float)pdfFont.Widths[array[l] - 32];
						}
						if (array[l] == 9)
						{
							num29 += GetTabsSize(stiTextOptions, num4, num29);
						}
						array2[l] = (int)num29;
						if (IsWordWrapSymbol(stringBuilder2, l) && l > 0)
						{
							num28++;
						}
						array3[l] = num28;
					}
					double num30 = (double)num29 * (num4 * fontCorrectValue) / 1000.0;
					if (!(num30 > num17))
					{
						continue;
					}
					int m = text.Length - 1;
					int num31 = (int)(num17 * 1000.0 / (num4 * fontCorrectValue));
					while (array2[m] > num31 && m > 0)
					{
						m--;
					}
					int num32 = m;
					if (array3[m] > 0)
					{
						if (array3[m] != array3[m + 1])
						{
							num32 = m++;
							for (; char.IsWhiteSpace(stringBuilder2[m]); m++)
							{
							}
						}
						else
						{
							while (!IsWordWrapSymbol(stringBuilder2, m))
							{
								m--;
							}
							num32 = m - 1;
							while (char.IsWhiteSpace(stringBuilder2[num32]) && num32 > 0)
							{
								num32--;
							}
							for (; char.IsWhiteSpace(stringBuilder2[m]); m++)
							{
							}
						}
					}
					else
					{
						m++;
					}
					if (flag2)
					{
						list[i] = text.Substring(0, num32 + 1) + "\a";
					}
					else
					{
						list[i] = text.Substring(0, num32 + 1);
					}
					list.Insert(i + 1, text.Substring(m, text.Length - m).TrimStart(new char[1] { ' ' }));
				}
			}
			for (int n = 0; n < list.Count; n++)
			{
				string value = list[n];
				StringBuilder stringBuilder5 = StiBidirectionalConvert2.ConvertStringBuilder(new StringBuilder(value), rightToLeft);
				list[n] = stringBuilder5.ToString();
			}
			if (stiVertAlignment != null)
			{
				double num33 = num12;
				double num34 = (double)list.Count * num33;
				StiVertAlignment val2 = stiVertAlignment.VertAlignment;
				if (num2 != 0f && num2 != 90f && num2 != 180f && num2 != 270f)
				{
					val2 = (StiVertAlignment)1;
				}
				if ((int)val2 == 0)
				{
					num16 += num13;
				}
				if ((int)val2 == 1)
				{
					num16 += (num18 - num34) / 2.0 + num13;
				}
				if ((int)val2 == 2)
				{
					num16 += num18 - num34 + num13;
				}
			}
			double[,] array4 = new double[list.Count, 3];
			for (int num35 = 0; num35 < list.Count; num35++)
			{
				string text2 = list[num35];
				if (text2.Length <= 0)
				{
					continue;
				}
				double[] array5 = new double[text2.Length];
				double num36 = 0.0;
				for (int num37 = 0; num37 < text2.Length; num37++)
				{
					int num38 = text2[num37];
					if (num38 >= 32)
					{
						num36 += (double)pdfFont.Widths[pdfFont.UnicodeMap[num38] - 32];
					}
					if (num38 == 9)
					{
						float tabsSize = GetTabsSize(stiTextOptions, num4, (float)num36);
						num36 += (double)tabsSize;
						array5[num37] = tabsSize / 10f;
					}
				}
				num36 = num36 * (num4 * fontCorrectValue) / 1000.0;
				text2 = ConvertToEscapeSequence(text2);
				if (text2[0] == '{')
				{
					text2 = "{}" + text2;
				}
				double num39 = num15;
				double num40 = num16 + num12 * (double)num35;
				if (stiTextHorAlignment != null)
				{
					StiTextHorAlignment val3 = stiTextHorAlignment.HorAlignment;
					if (flag2)
					{
						if (text2.Length > 0 && text2[text2.Length - 1] == '\a')
						{
							text2 = text2.Substring(0, text2.Length - 1);
						}
						else
						{
							val3 = (StiTextHorAlignment)0;
						}
					}
					if (stiTextOptions != null && stiTextOptions.TextOptions != null && stiTextOptions.TextOptions.RightToLeft)
					{
						if ((int)val3 == 0)
						{
							val3 = (StiTextHorAlignment)2;
						}
						else if ((int)val3 == 2)
						{
							val3 = (StiTextHorAlignment)0;
						}
					}
					if ((int)val3 == 1)
					{
						num39 += (num17 - num36) / 2.0;
					}
					if ((int)val3 == 2)
					{
						num39 += num17 - num36;
					}
					if ((int)val3 == 3)
					{
						int num41 = 0;
						for (int num42 = 0; num42 < text2.Length; num42++)
						{
							if (text2[num42] == ' ')
							{
								num41++;
							}
						}
						if (num41 > 0 && (num35 != list.Count - 1 || flag2))
						{
							double num43 = (num17 - num36) / (double)num41 * 100.0 / (num4 * fontCorrectValue);
							if (num43 > 0.0)
							{
								num43 += (double)(pdfFont.Widths[0] / 10);
								for (int num44 = 0; num44 < text2.Length; num44++)
								{
									if (text2[num44] == ' ')
									{
										array5[num44] = num43;
									}
								}
							}
						}
					}
				}
				array4[num35, 0] = num39;
				array4[num35, 1] = num40;
				array4[num35, 2] = num36;
				PdfFonts.pfontInfo pfontInfo = (PdfFonts.pfontInfo)pdfFont.fontList[num3];
				string text3 = pfontInfo.Name;
				if (StiOptions.Export.UseAlternativeFontNames && StiOptions.Export.AlternativeFontNames.ContainsKey(text3))
				{
					text3 = (string)StiOptions.Export.AlternativeFontNames[text3];
				}
				StringBuilder stringBuilder6 = new StringBuilder(text3);
				stringBuilder6.Replace(" ", "_");
				if (pfontInfo.Bold || pfontInfo.Italic)
				{
					if (pfontInfo.Bold)
					{
						stringBuilder6.Append("_Bold");
					}
					if (pfontInfo.Italic)
					{
						stringBuilder6.Append("_Italic");
					}
				}
				bool flag3 = false;
				StringBuilder stringBuilder7 = new StringBuilder();
				for (int num45 = 0; num45 < array5.Length - 1; num45++)
				{
					if (array5[num45] != 0.0)
					{
						stringBuilder7.Append(",");
						stringBuilder7.Append(FloatToString(array5[num45]));
						flag3 = true;
					}
					if (num45 != array5.Length - 1 - 1)
					{
						stringBuilder7.Append(";");
					}
				}
				writer.WriteStartElement("Glyphs");
				writer.WriteAttributeString("BidiLevel", "0");
				writer.WriteAttributeString("Fill", $"#{color.ToArgb():X8}");
				writer.WriteAttributeString("FontUri", $"/Resources/{stringBuilder6.ToString()}.ttf");
				writer.WriteAttributeString("FontRenderingEmSize", FloatToString(number));
				writer.WriteAttributeString("StyleSimulations", "None");
				writer.WriteAttributeString("OriginX", FloatToString(num39));
				writer.WriteAttributeString("OriginY", FloatToString(num40));
				writer.WriteAttributeString("UnicodeString", text2);
				writer.WriteAttributeString("Indices", flag3 ? stringBuilder7.ToString() : string.Empty);
				writer.WriteFullEndElement();
			}
			if (stiFont != null && stiFont.Font.Underline)
			{
				double num46 = num4 * 0.09;
				double num47 = (0.0 - num4) * 0.115;
				if (pdfFont.UnderscoreSize != 0)
				{
					num46 = num8;
					num47 = num9;
				}
				if (num46 < 0.10000000149011612)
				{
					num46 = 0.10000000149011612;
				}
				for (int num48 = 0; num48 < list.Count; num48++)
				{
					if (array4[num48, 2] != 0.0)
					{
						writer.WriteStartElement("Path");
						writer.WriteAttributeString("Stroke", $"#{color.ToArgb():X8}");
						writer.WriteAttributeString("StrokeThickness", $"{FloatToString(num46)}");
						writer.WriteAttributeString("Data", string.Format("M {0},{2} L {1},{2}", FloatToString(array4[num48, 0]), FloatToString(array4[num48, 0] + array4[num48, 2]), FloatToString(array4[num48, 1] - num47)));
						writer.WriteFullEndElement();
					}
				}
			}
			if (stiFont != null && stiFont.Font.Strikeout)
			{
				double num49 = num4 * 0.09;
				double num50 = num4 * 0.4;
				if (pdfFont.StrikeoutSize != 0)
				{
					num49 = num10;
					num50 = num11;
				}
				if (num49 < 0.10000000149011612)
				{
					num49 = 0.10000000149011612;
				}
				for (int num51 = 0; num51 < list.Count; num51++)
				{
					if (array4[num51, 2] != 0.0)
					{
						writer.WriteStartElement("Path");
						writer.WriteAttributeString("Stroke", $"#{color.ToArgb():X8}");
						writer.WriteAttributeString("StrokeThickness", $"{FloatToString(num49)}");
						writer.WriteAttributeString("Data", string.Format("M {0},{2} L {1},{2}", FloatToString(array4[num51, 0]), FloatToString(array4[num51, 0] + array4[num51, 2]), FloatToString(array4[num51, 1] - num50)));
						writer.WriteFullEndElement();
					}
				}
			}
			writer.WriteFullEndElement();
			writer.WriteFullEndElement();
		}
		if ((int)stiText.LinesOfUnderline == 6)
		{
			return;
		}
		double num52 = pp.Y + bottom + (double)num20;
		bool flag4 = true;
		bool flag5 = true;
		if (stiVertAlignment != null)
		{
			if (num == 0)
			{
				num = 1;
			}
			double num53 = num12 * (double)num;
			StiVertAlignment val4 = stiVertAlignment.VertAlignment;
			if (num2 != 0f && num2 != 90f && num2 != 180f && num2 != 270f)
			{
				val4 = (StiVertAlignment)1;
			}
			if ((int)val4 == 0)
			{
				num52 += num13;
				flag4 = false;
			}
			if ((int)val4 == 1)
			{
				num52 += (num18 - num53) / 2.0 + num13;
			}
			if ((int)val4 == 2)
			{
				num52 += num18 - num12 + num13;
				flag5 = false;
			}
		}
		num52 += 0.0 - num14;
		if (!(pp.Component is IStiBorder stiBorder))
		{
			return;
		}
		double size = stiBorder.Border.Size;
		double num54 = size * 0.9;
		Color color2 = stiBorder.Border.Color;
		string lineStyleDash = GetLineStyleDash(stiText.LinesOfUnderline);
		StringBuilder stringBuilder8 = new StringBuilder();
		double num55 = num52;
		if (flag4)
		{
			while (num55 - num12 > pp.Y + top)
			{
				num55 -= num12;
			}
		}
		for (; num55 < pp.Y + pp.Height - bottom - (flag5 ? 0.0 : num12); num55 += num12)
		{
			stringBuilder8.Append($"M {FloatToString(pp.X)},{FloatToString(num55)} L {FloatToString(pp.X + pp.Width)},{FloatToString(num55)} ");
		}
		writer.WriteStartElement("Path");
		writer.WriteAttributeString("Stroke", $"#{color2.ToArgb():X8}");
		writer.WriteAttributeString("StrokeThickness", $"{num54}");
		writer.WriteAttributeString("StrokeStartLineCap", "Square");
		writer.WriteAttributeString("StrokeEndLineCap", "Square");
		if (lineStyleDash != string.Empty)
		{
			writer.WriteAttributeString("StrokeDashArray", lineStyleDash);
			writer.WriteAttributeString("StrokeDashCap", "Flat");
		}
		writer.WriteAttributeString("Data", stringBuilder8.ToString());
		writer.WriteFullEndElement();
	}

	private void WriteImage(XmlTextWriter writer, StiXpsData data)
	{
		if (!(data.Component is IStiExportImageExtended stiExportImageExtended) || !data.Component.IsExportAsImage(StiExportFormat.Xps))
		{
			return;
		}
		float zoom = imageResolution / 100f;
		Image image = stiExportImageExtended.GetImage(ref zoom, StiExportFormat.Xps);
		try
		{
			if (image == null)
			{
				return;
			}
			if (image is Bitmap)
			{
				((Bitmap)((image is Bitmap) ? image : null)).SetResolution(96f, 96f);
			}
			int num = imageCache.AddImageInt(image);
			int num2 = -1;
			if (StiOptions.Export.Xps.AllowImageTransparency && image is Bitmap)
			{
				Bitmap val = (Bitmap)(object)((image is Bitmap) ? image : null);
				Rectangle rectangle = new Rectangle(0, 0, ((Image)val).Width, ((Image)val).Height);
				BitmapData val2 = val.LockBits(rectangle, (ImageLockMode)1, (PixelFormat)2498570);
				int num3 = Math.Abs(val2.Stride);
				int num4 = num3 * val2.Height;
				byte[] array = new byte[num4];
				Marshal.Copy(val2.Scan0, array, 0, num4);
				val.UnlockBits(val2);
				bool flag = false;
				for (int i = 0; i < ((Image)val).Height; i++)
				{
					int num5 = i * num3;
					for (int num6 = ((Image)val).Width; num6 > 0; num6--)
					{
						array[num5++] = 0;
						array[num5++] = 0;
						array[num5++] = 0;
						if (array[num5++] != byte.MaxValue)
						{
							flag = true;
						}
					}
				}
				if (flag)
				{
					val2 = val.LockBits(rectangle, (ImageLockMode)2, (PixelFormat)2498570);
					Marshal.Copy(array, 0, val2.Scan0, num4);
					val.UnlockBits(val2);
					num2 = imageCache.AddImageInt(image, ImageFormat.Png);
				}
			}
			pageImages[num] = num;
			if (num2 != -1)
			{
				pageImages[num2] = num2;
			}
			writer.WriteStartElement("Path");
			WriteHyperlinksData(writer, data.Component, check: false);
			writer.WriteAttributeString("Data", string.Format("M {0},{1} L {0},{3} L {2},{3} L {2},{1} Z ", FloatToString(data.X), FloatToString(data.Y), FloatToString(data.X + data.Width), FloatToString(data.Y + data.Height)));
			if (num2 != -1)
			{
				writer.WriteStartElement("Path.OpacityMask");
				writer.WriteStartElement("ImageBrush");
				writer.WriteAttributeString("ImageSource", $"/Resources/Images/image_{num2}.png");
				writer.WriteAttributeString("Viewbox", $"{0},{0},{image.Width},{image.Height}");
				writer.WriteAttributeString("TileMode", "None");
				writer.WriteAttributeString("ViewboxUnits", "Absolute");
				writer.WriteAttributeString("ViewportUnits", "Absolute");
				writer.WriteAttributeString("Viewport", string.Format("0,0,{0},{0}", 1));
				writer.WriteStartElement("ImageBrush.Transform");
				writer.WriteStartElement("MatrixTransform");
				writer.WriteAttributeString("Matrix", $"{FloatToString(data.Width)},{0},{0},{FloatToString(data.Height)},{FloatToString(data.X)},{FloatToString(data.Y)}");
				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
			writer.WriteStartElement("Path.Fill");
			writer.WriteStartElement("ImageBrush");
			writer.WriteAttributeString("ImageSource", $"/Resources/Images/image_{num}.jpg");
			writer.WriteAttributeString("Viewbox", $"{0},{0},{image.Width},{image.Height}");
			writer.WriteAttributeString("TileMode", "None");
			writer.WriteAttributeString("ViewboxUnits", "Absolute");
			writer.WriteAttributeString("ViewportUnits", "Absolute");
			writer.WriteAttributeString("Viewport", string.Format("0,0,{0},{0}", 1));
			writer.WriteStartElement("ImageBrush.Transform");
			writer.WriteStartElement("MatrixTransform");
			writer.WriteAttributeString("Matrix", $"{FloatToString(data.Width)},{0},{0},{FloatToString(data.Height)},{FloatToString(data.X)},{FloatToString(data.Y)}");
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteFullEndElement();
		}
		finally
		{
			((IDisposable)image)?.Dispose();
		}
	}

	private void WriteWatermark(XmlTextWriter writer, StiWatermark watermark, bool behind, double pageWidth, double pageHeight)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		if (watermark != null && !string.IsNullOrEmpty(watermark.Text) && watermark.ShowBehind == behind)
		{
			StiXpsData stiXpsData = default(StiXpsData);
			stiXpsData.X = 0.0;
			stiXpsData.Y = 0.0;
			stiXpsData.Width = pageWidth;
			stiXpsData.Height = pageHeight;
			StiXpsData pp = stiXpsData;
			pp.Component = new StiText(new RectangleD(pp.X, pp.Y, pp.Width, pp.Height))
			{
				Text = watermark.Text,
				TextBrush = watermark.TextBrush,
				Font = watermark.Font,
				TextOptions = new StiTextOptions
				{
					Angle = watermark.Angle
				},
				HorAlignment = (StiTextHorAlignment)1,
				VertAlignment = (StiVertAlignment)1
			};
			WriteText(writer, pp);
		}
	}

	private MemoryStream WritePageRels()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8)
		{
			Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0),
			Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None)
		};
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("Relationships");
		xmlTextWriter.WriteAttributeString("xmlns", "http://schemas.openxmlformats.org/package/2006/relationships");
		int[] array = new int[pageFonts.Count];
		pageFonts.Values.CopyTo(array, 0);
		for (int i = 0; i < array.Length; i++)
		{
			PdfFonts.pfontInfo pfontInfo = (PdfFonts.pfontInfo)pdfFont.fontList[array[i]];
			string text = pfontInfo.Name;
			if (StiOptions.Export.UseAlternativeFontNames && StiOptions.Export.AlternativeFontNames.ContainsKey(text))
			{
				text = (string)StiOptions.Export.AlternativeFontNames[text];
			}
			StringBuilder stringBuilder = new StringBuilder(text);
			stringBuilder.Replace(" ", "_");
			if (pfontInfo.Bold || pfontInfo.Italic)
			{
				if (pfontInfo.Bold)
				{
					stringBuilder.Append("_Bold");
				}
				if (pfontInfo.Italic)
				{
					stringBuilder.Append("_Italic");
				}
			}
			xmlTextWriter.WriteStartElement("Relationship");
			xmlTextWriter.WriteAttributeString("Id", $"rId{i + 1}");
			xmlTextWriter.WriteAttributeString("Type", "http://schemas.microsoft.com/xps/2005/06/required-resource");
			xmlTextWriter.WriteAttributeString("Target", $"../../../Resources/{stringBuilder}.ttf");
			xmlTextWriter.WriteEndElement();
		}
		int[] array2 = new int[pageImages.Count];
		pageImages.Values.CopyTo(array2, 0);
		for (int j = 0; j < array2.Length; j++)
		{
			xmlTextWriter.WriteStartElement("Relationship");
			xmlTextWriter.WriteAttributeString("Id", $"rId{pageFonts.Count + j + 1}");
			xmlTextWriter.WriteAttributeString("Type", "http://schemas.microsoft.com/xps/2005/06/required-resource");
			string arg = "jpg";
			if (imageCache.ImageFormatStore[array2[j]] == ImageFormat.Png)
			{
				arg = "png";
			}
			xmlTextWriter.WriteAttributeString("Target", $"../../../Resources/Images/image_{array2[j]}.{arg}");
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteResourceImage(int index)
	{
		MemoryStream memoryStream = new MemoryStream();
		byte[] array = (byte[])imageCache.ImagePackedStore[index];
		memoryStream.Write(array, 0, array.Length);
		return memoryStream;
	}

	private MemoryStream WriteRecourceFont(int index)
	{
		MemoryStream memoryStream = new MemoryStream();
		pdfFont.CurrentFont = index;
		PdfFonts.pfontInfo pfontInfo = (PdfFonts.pfontInfo)pdfFont.fontList[index];
		pdfFont.GetFontDataBuf(pfontInfo.Font, out var buff, useFontInfoStore: true);
		if (buff.Length != 0 && reduceFontSize)
		{
			pdfFont.OpenTypeHelper.ReduceFontSize(ref buff, pfontInfo.Name, remakeGlyphTable: true, pdfFont.GlyphList, pdfFont.GlyphRtfList);
		}
		memoryStream.Write(buff, 0, buff.Length);
		return memoryStream;
	}

	public void ExportXps(StiReport report, string fileName)
	{
		FileStream fileStream = null;
		try
		{
			StiFileUtils.ProcessReadOnly(fileName);
			fileStream = new FileStream(fileName, FileMode.Create);
			ExportXps(report, fileStream);
		}
		finally
		{
			fileStream.Flush();
			fileStream.Close();
		}
	}

	public void ExportXps(StiReport report, Stream stream)
	{
		ExportXps(report, stream, new StiXpsExportSettings());
	}

	public void ExportXps(StiReport report, Stream stream, StiPagesRange pageRange)
	{
		ExportXps(report, stream, new StiXpsExportSettings
		{
			PageRange = pageRange
		});
	}

	public void ExportXps(StiReport report, Stream stream, StiXpsExportSettings settings)
	{
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Expected O, but got Unknown
		StiLogService.Write(((object)this).GetType(), "Export report to Microsoft XML Paper Specification format");
		if (settings == null)
		{
			throw new ArgumentNullException("settings");
		}
		StiPagesRange pageRange = settings.PageRange;
		imageResolution = settings.ImageResolution;
		imageQuality = settings.ImageQuality;
		exportRtfTextAsImage = settings.ExportRtfTextAsImage;
		if (imageQuality < 0f)
		{
			imageQuality = 0f;
		}
		if (imageQuality > 1f)
		{
			imageQuality = 1f;
		}
		if (imageResolution < 10f)
		{
			imageResolution = 10f;
		}
		xmlIndentation = 1;
		useImageComparer = StiOptions.Export.Xps.AllowImageComparer;
		reduceFontSize = StiOptions.Export.Xps.ReduceFontFileSize;
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
			StiPagesCollection selectedPages = pageRange.GetSelectedPages(report.RenderedPages);
			if (base.IsStopped)
			{
				return;
			}
			base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
			PrepareData();
			foreach (StiPage item in selectedPages)
			{
				selectedPages.GetPage(item);
				if (selectedPages.CacheMode)
				{
					InvokeExporting(item, selectedPages, 0, 2);
				}
				if (item.Watermark != null && !string.IsNullOrEmpty(item.Watermark.Text))
				{
					if (item.Watermark.Font != null)
					{
						pdfFont.GetFontNumber(item.Watermark.Font);
					}
					StringBuilder sb = new StringBuilder(StiBidirectionalConvert2.ConvertString(item.Watermark.Text, rightToLeft: false));
					pdfFont.StoreUnicodeSymbolsInMap(sb);
				}
				foreach (StiComponent component in item.Components)
				{
					if (component.Enabled)
					{
						if (component is IStiFont stiFont)
						{
							pdfFont.GetFontNumber(stiFont.Font);
						}
						IStiTextOptions stiTextOptions = component as IStiTextOptions;
						if (component is StiText && !component.IsExportAsImage(StiExportFormat.Xps))
						{
							IStiText stiText = component as IStiText;
							bool rightToLeft = stiTextOptions.TextOptions != null && stiTextOptions.TextOptions.RightToLeft;
							StringBuilder sb2 = new StringBuilder(StiBidirectionalConvert2.ConvertString(stiText.Text, rightToLeft));
							pdfFont.StoreUnicodeSymbolsInMap(sb2);
						}
					}
				}
			}
			pdfFont.InitFontsData(isWpf: false);
			StiZipWriter20 val = new StiZipWriter20();
			val.Begin(stream, true);
			val.AddFile("[Content_Types].xml", WriteContentTypes());
			val.AddFile("_rels/.rels", WriteMainRels());
			val.AddFile("docProps/core.xml", WriteDocPropsCore(report));
			val.AddFile("FixedDocSeq.fdseq", WriteFixedDocSeq());
			pageFonts = new Hashtable();
			pageImages = new Hashtable();
			pageBookmarks = new Hashtable();
			indexPage = 0;
			foreach (StiPage item2 in selectedPages)
			{
				selectedPages.GetPage(item2);
				InvokeExporting(item2, selectedPages, 1, 2);
				if (base.IsStopped)
				{
					return;
				}
				val.AddFile($"Documents/1/Pages/{indexPage + 1}.fpage", WritePage(item2, report));
				if (pageFonts.Count > 0 || pageImages.Count > 0)
				{
					val.AddFile($"Documents/1/Pages/_rels/{indexPage + 1}.fpage.rels", WritePageRels());
				}
				pageFonts.Clear();
				pageImages.Clear();
				if (base.IsStopped)
				{
					return;
				}
				indexPage++;
			}
			val.AddFile("Documents/1/FixedDoc.fdoc", WriteFixedDoc(selectedPages));
			if (pdfFont.fontList.Count > 0)
			{
				for (int i = 0; i < pdfFont.fontList.Count; i++)
				{
					PdfFonts.pfontInfo pfontInfo = (PdfFonts.pfontInfo)pdfFont.fontList[i];
					string text = pfontInfo.Name;
					if (StiOptions.Export.UseAlternativeFontNames && StiOptions.Export.AlternativeFontNames.ContainsKey(text))
					{
						text = (string)StiOptions.Export.AlternativeFontNames[text];
					}
					StringBuilder stringBuilder = new StringBuilder(text);
					stringBuilder.Replace(" ", "_");
					if (pfontInfo.Bold || pfontInfo.Italic)
					{
						if (pfontInfo.Bold)
						{
							stringBuilder.Append("_Bold");
						}
						if (pfontInfo.Italic)
						{
							stringBuilder.Append("_Italic");
						}
					}
					val.AddFile($"Resources/{stringBuilder}.ttf", WriteRecourceFont(i));
				}
			}
			if (imageCache.ImagePackedStore.Count > 0)
			{
				for (int j = 0; j < imageCache.ImagePackedStore.Count; j++)
				{
					string arg = "jpg";
					if (imageCache.ImageFormatStore[j] == ImageFormat.Png)
					{
						arg = "png";
					}
					val.AddFile($"Resources/Images/image_{j}.{arg}", WriteResourceImage(j));
				}
			}
			val.End();
		}
		finally
		{
			StiExportUtils.EnableFontSmoothing(report);
			Thread.CurrentThread.CurrentCulture = currentCulture;
			pdfFont.Clear();
			pdfFont = null;
			pageFonts = null;
			pageImages = null;
			imageCache.Clear();
			pageBookmarks.Clear();
			pageBookmarks = null;
		}
	}
}
