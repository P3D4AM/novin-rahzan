using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Base.Zip;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Export.Services.Helpers;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Html.png")]
public class StiHtmlExportService : StiExportService
{
	private struct SizeStrings
	{
		public string Width;

		public string Height;

		public double DWidth;

		public double DHeight;
	}

	private class StiBookmarkTreeNode
	{
		public int Parent;

		public string Title;

		public string Url;

		public string FullUrl;

		public bool Used;
	}

	private StiHtmlExportSettings exportSettings;

	private StiReport reportTmp;

	private string documentFileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	public StiHtmlTableRender TableRender;

	public StiHtmlTextWriter HtmlWriter;

	internal StiZipWriter20 zip;

	internal StiReport report;

	internal string fileName = string.Empty;

	private double startPage;

	internal int imageNumber = 1;

	internal double zoom = 0.75;

	internal ImageFormat imageFormat;

	private NumberFormatInfo numberFormat = new NumberFormatInfo();

	internal StiHtmlExportQuality exportQuality;

	internal bool useStylesTable = true;

	internal bool isFileStreamMode = true;

	internal float imageQuality = 0.75f;

	internal float imageResolution = 96f;

	internal bool compressToArchive;

	internal bool useEmbeddedImages;

	internal string openLinksTarget;

	internal StiHtmlChartType chartType = StiHtmlChartType.Image;

	internal StiHtmlExportMode exportMode = StiHtmlExportMode.Table;

	private SortedList coordX;

	private SortedList coordY;

	private string strSpanDiv = "span";

	private Hashtable hyperlinksToTag;

	private Hashtable pointerToBookmark;

	private Hashtable pointerToTag;

	internal Hashtable chartData = new Hashtable();

	private CultureInfo currentCulture;

	private CultureInfo storedCulture;

	internal Hashtable hashBookmarkGuid;

	private const double hiToPt = 0.716;

	private const double pxToPt = 0.75;

	private int defaultCoordinatesPrecision = StiOptions.Export.Html.DefaultCoordinatesPrecision;

	private static Hashtable fontsToCorrectHeight = new Hashtable
	{
		{ "微软雅黑", 1.09 },
		{ "Agency FB", 0.98 },
		{ "Algerian", 1.08 },
		{ "Arial Black", 1.17 },
		{ "Arial Narrow", 0.96 },
		{ "Arial Rounded MT Bold", 0.97 },
		{ "Arial", 0.96 },
		{ "Arial Unicode MS", 1.09 },
		{ "Baskerville Old Face", 0.96 },
		{ "Bauhaus 93", 1.21 },
		{ "Bell MT", 0.94 },
		{ "Berlin Sans FB Demi", 0.94 },
		{ "Berlin Sans FB", 0.94 },
		{ "Bernard MT Condensed", 0.99 },
		{ "Bodoni MT Black", 0.97 },
		{ "Bodoni MT Condensed", 0.98 },
		{ "Bodoni MT Poster Compressed", 0.96 },
		{ "Bodoni MT", 1.01 },
		{ "Calibri Light", 1.01 },
		{ "Calibri", 1.01 },
		{ "Californian FB", 0.94 },
		{ "Cambria Math", 0.98 },
		{ "Cambria", 0.98 },
		{ "Candara", 1.01 },
		{ "Courier New", 0.96 },
		{ "Franklin Gothic Book", 0.94 },
		{ "Franklin Gothic Demi Cond", 0.93 },
		{ "Franklin Gothic Demi", 0.93 },
		{ "Franklin Gothic Heavy", 0.93 },
		{ "Franklin Gothic Medium Cond", 0.93 },
		{ "Franklin Gothic Medium", 0.93 },
		{ "Futura CondensedLight", 0.98 },
		{ "Futura Lt BT", 1.0 },
		{ "Futura Md BT", 1.0 },
		{ "Garamond", 0.94 },
		{ "Georgia", 0.94 },
		{ "Gill Sans MT Condensed", 1.0 },
		{ "Gill Sans MT Ext Condensed Bold", 0.99 },
		{ "Gill Sans MT", 0.96 },
		{ "Gill Sans Ultra Bold Condensed", 1.03 },
		{ "Gill Sans Ultra Bold", 1.03 },
		{ "HelveticaNeueCyr", 0.94 },
		{ "Lucida Bright", 0.98 },
		{ "Lucida Calligraphy", 1.16 },
		{ "Lucida Console", 0.84 },
		{ "Lucida Fax", 0.98 },
		{ "Lucida Handwriting", 1.15 },
		{ "Lucida Sans Typewriter", 0.98 },
		{ "Lucida Sans Unicode", 1.28 },
		{ "Lucida Sans", 0.98 },
		{ "Microsoft JhengHei Light", 1.11 },
		{ "Microsoft JhengHei UI Light", 1.04 },
		{ "Microsoft JhengHei UI", 1.04 },
		{ "Microsoft JhengHei", 1.12 },
		{ "Microsoft New Tai Lue", 1.1 },
		{ "Microsoft PhagsPa", 1.06 },
		{ "Microsoft Sans Serif", 0.94 },
		{ "Microsoft Tai Le", 1.07 },
		{ "Microsoft Uighur", 0.92 },
		{ "Microsoft YaHei", 1.09 },
		{ "Microsoft YaHei Light", 1.12 },
		{ "Microsoft YaHei UI Light", 1.1 },
		{ "Microsoft YaHei UI", 1.04 },
		{ "Microsoft Yi Baiti", 0.86 },
		{ "MS Gothic", 0.84 },
		{ "MS Outlook", 0.86 },
		{ "MS PGothic", 0.84 },
		{ "MS Reference Specialty", 1.0 },
		{ "MS UI Gothic", 0.84 },
		{ "Rockwell Condensed", 0.98 },
		{ "Rockwell Extra Bold", 0.97 },
		{ "Rockwell", 0.97 },
		{ "Segoe MDL2 Assets", 0.82 },
		{ "Segoe Print", 1.47 },
		{ "Segoe Script", 1.31 },
		{ "Segoe UI", 1.09 },
		{ "Segoe UI Black", 1.12 },
		{ "Segoe UI Emoji", 1.1 },
		{ "Segoe UI Historic", 1.1 },
		{ "Segoe UI Light", 1.1 },
		{ "Segoe UI Semibold", 1.1 },
		{ "Segoe UI Semilight", 1.1 },
		{ "Segoe UI Symbol", 1.1 },
		{ "Times New Roman", 0.95 },
		{ "Yu Gothic Light", 1.32 },
		{ "Yu Gothic Medium", 1.32 },
		{ "Yu Gothic UI Light", 1.09 },
		{ "Yu Gothic UI Semibold", 1.09 },
		{ "Yu Gothic UI Semilight", 1.09 },
		{ "Yu Gothic UI", 1.09 },
		{ "Yu Gothic", 1.32 }
	};

	public override string DefaultExtension
	{
		get
		{
			if (compressToArchive)
			{
				return "zip";
			}
			if (exportSettings is StiMhtExportSettings)
			{
				return "mht";
			}
			if (exportSettings != null && exportSettings.HtmlType == StiHtmlType.Mht)
			{
				return "mht";
			}
			return "html";
		}
	}

	public override StiExportFormat ExportFormat
	{
		get
		{
			if (exportSettings is StiHtml5ExportSettings)
			{
				return StiExportFormat.Html5;
			}
			if (exportSettings is StiMhtExportSettings)
			{
				return StiExportFormat.Mht;
			}
			if (exportSettings != null)
			{
				switch (exportSettings.HtmlType)
				{
				case StiHtmlType.Html5:
					return StiExportFormat.Html5;
				case StiHtmlType.Mht:
					return StiExportFormat.Mht;
				}
			}
			return StiExportFormat.Html;
		}
	}

	public override string GroupCategory => "Web";

	public override int Position => 10;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeHtmlFile");

	public override bool MultipleFiles => false;

	public bool ClearOnFinish { get; set; } = true;


	public bool RenderStyles { get; set; } = true;


	public ArrayList Styles { get; set; }

	public bool RenderWebViewer { get; set; }

	public bool RenderWebInteractions { get; set; }

	public StiHtmlImageHost HtmlImageHost { get; set; }

	public double TotalPageWidth { get; set; }

	public double TotalPageHeight { get; set; }

	public bool RenderAsDocument { get; set; } = true;


	internal bool RemoveEmptySpaceAtBottom { get; private set; } = StiOptions.Export.Html.RemoveEmptySpaceAtBottom;


	internal StiHorAlignment PageHorAlignment { get; private set; } = (StiHorAlignment)2;


	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		exportSettings = settings as StiHtmlExportSettings;
		StiHtmlExportSettings stiHtmlExportSettings = settings as StiHtmlExportSettings;
		StiHtmlType stiHtmlType = stiHtmlExportSettings?.HtmlType ?? StiHtmlType.Html;
		if (stiHtmlType == StiHtmlType.Html5 || stiHtmlExportSettings is StiHtml5ExportSettings)
		{
			StiHtml5ExportService stiHtml5ExportService = new StiHtml5ExportService();
			stiHtml5ExportService.ExportHtml(report, stream, stiHtmlExportSettings as StiHtml5ExportSettings);
		}
		else if (stiHtmlType == StiHtmlType.Mht || stiHtmlExportSettings is StiMhtExportSettings)
		{
			StiMhtExportService stiMhtExportService = new StiMhtExportService();
			stiMhtExportService.ExportMht(report, stream, stiHtmlExportSettings as StiMhtExportSettings);
		}
		else
		{
			ExportHtml(report, stream, stiHtmlExportSettings);
		}
		InvokeExporting(100, 100, 1, 1);
	}

	public override void Export(StiReport report, string fileName, bool sendEMail, StiGuiMode guiMode)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		IStiFormRunner exportFormRunner = StiGuiOptions.GetExportFormRunner("StiHtmlSetupForm", guiMode, base.OwnerWindow);
		try
		{
			exportFormRunner["CurrentPage"] = report.CurrentPrintPage;
			exportFormRunner["OpenAfterExportEnabled"] = !sendEMail;
			reportTmp = report;
			documentFileName = fileName;
			this.sendEMail = sendEMail;
			this.guiMode = guiMode;
			exportFormRunner.Complete += new StiShowDialogCompleteEventHandler(Form_Complete);
			exportFormRunner.ShowDialog();
		}
		finally
		{
			((IDisposable)exportFormRunner)?.Dispose();
		}
	}

	public override string GetFilter()
	{
		if (compressToArchive)
		{
			return StiLocalization.Get("FileFilters", "ZipArchives");
		}
		return StiLocalization.Get("FileFilters", "HtmlFiles");
	}

	private void Form_Complete(IStiFormRunner form, StiShowDialogCompleteEvetArgs e)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		if (e.DialogResult)
		{
			StiExportFormat stiExportFormat = (StiExportFormat)form["ExportFormat"];
			if (stiExportFormat == StiExportFormat.Html5)
			{
				StiHtml5ExportService stiHtml5ExportService = new StiHtml5ExportService();
				stiHtml5ExportService.compressToArchive = (bool)form["CompressToArchive"];
				if (string.IsNullOrEmpty(documentFileName))
				{
					documentFileName = stiHtml5ExportService.GetFileName(reportTmp, sendEMail);
				}
				if (documentFileName != null)
				{
					StiFileUtils.ProcessReadOnly(documentFileName);
					try
					{
						using FileStream stream = new FileStream(documentFileName, FileMode.Create);
						stiHtml5ExportService.StartProgress(guiMode);
						StiHtml5ExportSettings stiHtml5ExportSettings = new StiHtml5ExportSettings();
						stiHtml5ExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
						stiHtml5ExportSettings.ImageFormat = (ImageFormat)form["ImageFormat"];
						stiHtml5ExportSettings.ImageResolution = (float)form["Resolution"];
						stiHtml5ExportSettings.ImageQuality = (float)form["ImageQuality"];
						stiHtml5ExportSettings.ContinuousPages = (bool)form["ContinuousPages"];
						stiHtml5ExportSettings.CompressToArchive = (bool)form["CompressToArchive"];
						stiHtml5ExportService.StartExport(settings: exportSettings = stiHtml5ExportSettings, report: reportTmp, stream: stream, sendEMail: sendEMail, openAfterExport: (bool)form["OpenAfterExport"], fileName: documentFileName, guiMode: guiMode);
					}
					catch (Exception ex)
					{
						throw ex;
					}
				}
			}
			if (stiExportFormat == StiExportFormat.Mht)
			{
				StiMhtExportService stiMhtExportService = new StiMhtExportService();
				stiMhtExportService.compressToArchive = (bool)form["CompressToArchive"];
				if (string.IsNullOrEmpty(documentFileName))
				{
					documentFileName = stiMhtExportService.GetFileName(reportTmp, sendEMail);
				}
				if (documentFileName != null)
				{
					StiFileUtils.ProcessReadOnly(documentFileName);
					try
					{
						using FileStream stream2 = new FileStream(documentFileName, FileMode.Create);
						stiMhtExportService.StartProgress(guiMode);
						StiMhtExportSettings stiMhtExportSettings = new StiMhtExportSettings();
						stiMhtExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
						stiMhtExportSettings.Zoom = (float)form["Zoom"];
						stiMhtExportSettings.ImageFormat = (ImageFormat)form["ImageFormat"];
						stiMhtExportSettings.ExportMode = (StiHtmlExportMode)form["ExportMode"];
						stiMhtExportSettings.ExportQuality = StiHtmlExportQuality.High;
						stiMhtExportSettings.AddPageBreaks = (bool)form["AddPageBreaks"];
						stiMhtExportSettings.CompressToArchive = (bool)form["CompressToArchive"];
						stiMhtExportSettings.Encoding = Encoding.UTF8;
						StiMhtExportSettings settings2 = stiMhtExportSettings;
						stiMhtExportService.fileName = documentFileName;
						exportSettings = settings2;
						stiMhtExportService.StartExport(reportTmp, stream2, settings2, sendEMail, (bool)form["OpenAfterExport"], documentFileName, guiMode);
					}
					catch (Exception ex2)
					{
						throw ex2;
					}
				}
			}
			if (stiExportFormat != StiExportFormat.Html)
			{
				return;
			}
			compressToArchive = (bool)form["CompressToArchive"];
			if (string.IsNullOrEmpty(documentFileName))
			{
				documentFileName = GetFileName(reportTmp, sendEMail);
			}
			if (documentFileName == null)
			{
				return;
			}
			StiFileUtils.ProcessReadOnly(documentFileName);
			try
			{
				using FileStream stream3 = new FileStream(documentFileName, FileMode.Create);
				StartProgress(guiMode);
				StiHtmlExportSettings stiHtmlExportSettings = new StiHtmlExportSettings();
				stiHtmlExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
				stiHtmlExportSettings.Zoom = (float)form["Zoom"];
				stiHtmlExportSettings.ImageFormat = (ImageFormat)form["ImageFormat"];
				stiHtmlExportSettings.ExportMode = (StiHtmlExportMode)form["ExportMode"];
				stiHtmlExportSettings.ExportQuality = StiHtmlExportQuality.High;
				stiHtmlExportSettings.AddPageBreaks = (bool)form["AddPageBreaks"];
				stiHtmlExportSettings.CompressToArchive = (bool)form["CompressToArchive"];
				stiHtmlExportSettings.UseEmbeddedImages = (bool)form["UseEmbeddedImages"];
				StartExport(settings: exportSettings = stiHtmlExportSettings, report: reportTmp, stream: stream3, sendEMail: sendEMail, openAfterExport: (bool)form["OpenAfterExport"], fileName: documentFileName, guiMode: guiMode);
				return;
			}
			catch (Exception ex3)
			{
				throw ex3;
			}
		}
		base.IsStopped = true;
	}

	public void AddCoord(RectangleD rect)
	{
		AddCoord(((RectangleD)(ref rect)).Left, ((RectangleD)(ref rect)).Top);
		AddCoord(((RectangleD)(ref rect)).Right, ((RectangleD)(ref rect)).Bottom);
	}

	private void AddCoord(double x, double y)
	{
		coordX[x] = x;
		coordY[y] = y;
	}

	private void FormatCoords(StiReport report)
	{
		SortedList sortedList = new SortedList();
		foreach (double key in coordX.Keys)
		{
			sortedList[key] = (double)Math.Round((decimal)(report.Unit.ConvertToHInches((double)coordX[key]) * zoom * 0.716), 2);
		}
		coordX = sortedList;
		SortedList sortedList2 = new SortedList();
		foreach (double key2 in coordY.Keys)
		{
			sortedList2[key2] = (double)Math.Round((decimal)(report.Unit.ConvertToHInches((double)coordY[key2]) * zoom * 0.716), 2);
		}
		coordY = sortedList2;
	}

	private string FormatCoord(double value)
	{
		return Math.Round((decimal)value, defaultCoordinatesPrecision).ToString(numberFormat) + "pt";
	}

	internal string FormatColor(Color color)
	{
		if (color.A < byte.MaxValue && color.A > 0)
		{
			return $"rgba({color.R},{color.G},{color.B},{Math.Round((float)(int)color.A / 255f, 3)})";
		}
		return ColorTranslator.ToHtml(color);
	}

	internal static double GetFontHeightCorrection(string font)
	{
		object obj = fontsToCorrectHeight[font];
		if (obj != null)
		{
			return (double)obj;
		}
		return 1.0;
	}

	internal string GetBorderStyle(StiPenStyle style)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected I4, but got Unknown
		switch (style - 1)
		{
		case 3:
			return " dotted";
		case 0:
		case 1:
		case 2:
			return " dashed";
		case 4:
			return " double";
		default:
			return " solid";
		}
	}

	internal void SetCurrentCulture()
	{
		storedCulture = Thread.CurrentThread.CurrentCulture;
		if (StiOptions.Export.Html.ForcedCultureForCharts != null)
		{
			Thread.CurrentThread.CurrentCulture = StiOptions.Export.Html.ForcedCultureForCharts;
		}
		else
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	internal void RestoreCulture()
	{
		Thread.CurrentThread.CurrentCulture = storedCulture;
	}

	internal void RenderFont(StiHtmlTableCell cell, Font font)
	{
		string text = "";
		if (font.Bold)
		{
			text += "bold ";
		}
		if (font.Italic)
		{
			text += "italic ";
		}
		text += Math.Round((double)font.SizeInPoints * zoom, 0);
		string text2 = font.FontFamily.GetName(0);
		if (text2.Contains(" "))
		{
			text2 = "'" + text2 + "'";
		}
		text = text + "pt " + text2;
		string value = ((!font.Underline) ? (font.Strikeout ? "line-through" : null) : (font.Strikeout ? "underline line-through" : "underline"));
		if (cell == null)
		{
			HtmlWriter.WriteStyleAttribute("Font", text);
			if (font.Underline || font.Strikeout)
			{
				HtmlWriter.WriteStyleAttribute("text-decoration", value);
			}
		}
		else
		{
			cell.Style["Font"] = text;
			if (font.Underline || font.Strikeout)
			{
				cell.Style["text-decoration"] = value;
			}
		}
	}

	internal void RenderTextHorAlignment(StiHtmlTableCell cell, StiTextOptions textOptions, StiTextHorAlignment textHorAlignment)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Invalid comparison between Unknown and I4
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Invalid comparison between Unknown and I4
		bool flag = textOptions != null && textOptions.RightToLeft;
		string text = string.Empty;
		if ((int)textHorAlignment == 0)
		{
			text = ((!flag) ? "left" : "right");
		}
		if ((int)textHorAlignment == 2)
		{
			text = (flag ? "left" : "right");
		}
		if ((int)textHorAlignment == 1)
		{
			text = "center";
		}
		if ((int)textHorAlignment == 3)
		{
			text = "justify";
		}
		if (!(text != string.Empty))
		{
			return;
		}
		if (cell == null)
		{
			HtmlWriter.WriteStyleAttribute("text-align", text);
			if (exportMode != StiHtmlExportMode.Table)
			{
				if (text == "left")
				{
					HtmlWriter.WriteStyleAttribute("justify-content", "flex-start");
				}
				if (text == "right")
				{
					HtmlWriter.WriteStyleAttribute("justify-content", "flex-end");
				}
				if (text == "center")
				{
					HtmlWriter.WriteStyleAttribute("justify-content", "center");
				}
				if (text == "justify")
				{
					HtmlWriter.WriteStyleAttribute("justify-content", "space-between");
				}
			}
		}
		else
		{
			cell.Style["text-align"] = text;
		}
	}

	internal void RenderVertAlignment(StiHtmlTableCell cell, StiVertAlignment textVertAlignment, StiTextOptions textOptions = null, bool allowHtmlTags = false)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Invalid comparison between Unknown and I4
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Invalid comparison between Unknown and I4
		bool flag = textOptions != null && (int)textOptions.Trimming > 0;
		string value = null;
		if ((int)textVertAlignment == 0)
		{
			value = "top";
		}
		if ((int)textVertAlignment == 1)
		{
			value = "middle";
		}
		if ((int)textVertAlignment == 2)
		{
			value = "bottom";
		}
		if (string.IsNullOrEmpty(value))
		{
			return;
		}
		if (cell == null)
		{
			if (exportMode == StiHtmlExportMode.Table || allowHtmlTags)
			{
				HtmlWriter.WriteStyleAttribute("vertical-align", value);
				return;
			}
			if ((int)textVertAlignment == 0)
			{
				value = "flex-start";
			}
			if ((int)textVertAlignment == 1)
			{
				value = "center";
			}
			if ((int)textVertAlignment == 2)
			{
				value = "flex-end";
			}
			if (flag)
			{
				HtmlWriter.WriteStyleAttribute("display", "block");
				HtmlWriter.WriteStyleAttribute("white-space", "nowrap");
			}
			else
			{
				HtmlWriter.WriteStyleAttribute("display", "flex");
			}
			HtmlWriter.WriteStyleAttribute("align-items", value);
		}
		else
		{
			cell.Style["vertical-align"] = value;
		}
	}

	internal void RenderTextDirection(StiHtmlTableCell cell, StiTextOptions textOptions)
	{
		if (textOptions != null && textOptions.RightToLeft)
		{
			if (cell == null)
			{
				HtmlWriter.WriteStyleAttribute("direction", "rtl");
			}
			else
			{
				cell.Style["direction"] = "rtl";
			}
		}
	}

	internal void RenderBackColor(StiHtmlTableCell cell, Color color)
	{
		if (color.A > 0)
		{
			if (cell == null)
			{
				HtmlWriter.WriteStyleAttribute("background-color", FormatColor(color));
			}
			else
			{
				cell.Style["background-color"] = FormatColor(color);
			}
		}
	}

	internal void RenderTextColor(StiHtmlTableCell cell, Color color, bool forceAnyColor = false)
	{
		if (color != Color.Black || forceAnyColor)
		{
			if (cell == null)
			{
				HtmlWriter.WriteStyleAttribute("color", FormatColor(color));
			}
			else
			{
				cell.Style["color"] = FormatColor(color);
			}
		}
	}

	private void RenderBorder(StiComponent comp)
	{
		if (comp is IStiBorder && !(comp is IStiIgnoreBorderWhenExport))
		{
			IStiBorder stiBorder = comp as IStiBorder;
			RenderBorder(stiBorder.Border);
		}
	}

	internal void RenderBorder(StiBorder border)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		if (border == null)
		{
			return;
		}
		StiBorderSide val = null;
		StiBorderSide border2 = null;
		StiBorderSide border3 = null;
		StiBorderSide border4 = null;
		StiAdvancedBorder val2 = (StiAdvancedBorder)(object)((border is StiAdvancedBorder) ? border : null);
		if (val2 != (StiAdvancedBorder)null)
		{
			val = val2.LeftSide;
			border2 = val2.RightSide;
			border3 = val2.TopSide;
			border4 = val2.BottomSide;
		}
		else
		{
			val = new StiBorderSide(border.Color, border.Size, border.Style);
			if (border.IsRightBorderSidePresent)
			{
				border2 = val;
			}
			if (border.IsTopBorderSidePresent)
			{
				border3 = val;
			}
			if (border.IsBottomBorderSidePresent)
			{
				border4 = val;
			}
			if (!border.IsLeftBorderSidePresent)
			{
				val = null;
			}
		}
		RenderBorder(null, val, "left");
		RenderBorder(null, border2, "right");
		RenderBorder(null, border3, "top");
		RenderBorder(null, border4, "bottom");
	}

	internal void RenderBorder(StiHtmlTableCell cell, StiBorderSide border, string side)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Invalid comparison between Unknown and I4
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Invalid comparison between Unknown and I4
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Invalid comparison between Unknown and I4
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Invalid comparison between Unknown and I4
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Invalid comparison between Unknown and I4
		if (border != (StiBorderSide)null && (int)border.Style != 6)
		{
			string value = FormatColor(border.Color);
			string borderStyle = GetBorderStyle(border.Style);
			double num = border.Size;
			if (num > 0.0 && num < 1.0)
			{
				num = 1.0;
			}
			if (num < 1.0 && ((int)border.Style == 1 || (int)border.Style == 2 || (int)border.Style == 3 || (int)border.Style == 4))
			{
				num = 1.0;
			}
			if ((int)border.Style == 5)
			{
				num = 2.5;
			}
			string value2 = Math.Round(num, MidpointRounding.AwayFromZero) + "px";
			if (cell == null)
			{
				HtmlWriter.WriteStyleAttribute($"border-{side}-color", value);
				HtmlWriter.WriteStyleAttribute($"border-{side}-style", borderStyle);
				HtmlWriter.WriteStyleAttribute($"border-{side}-width", value2);
			}
			else
			{
				cell.Style[$"border-{side}-color"] = value;
				cell.Style[$"border-{side}-style"] = borderStyle;
				cell.Style[$"border-{side}-width"] = value2;
			}
		}
		else if (StiOptions.Export.Html.UseStrictTableCellSize && cell == null)
		{
			HtmlWriter.WriteStyleAttribute($"border-{side}-color", "transparent");
			HtmlWriter.WriteStyleAttribute($"border-{side}-style", "solid");
			HtmlWriter.WriteStyleAttribute($"border-{side}-width", "1px");
		}
	}

	private string RenderPosition(StiComponent comp, bool returnSize = false)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Invalid comparison between Unknown and I4
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		RectangleD val = comp.ComponentToPage(comp.ClientRectangle);
		double num = (double)coordX[((RectangleD)(ref val)).Left];
		double num2 = (double)coordY[((RectangleD)(ref val)).Top] + startPage * zoom * 0.716;
		double num3 = (double)coordX[((RectangleD)(ref val)).Right] - (double)coordX[((RectangleD)(ref val)).Left];
		double num4 = (double)coordY[((RectangleD)(ref val)).Bottom] - (double)coordY[((RectangleD)(ref val)).Top];
		if (comp is IStiBorder { Border: not null } stiBorder)
		{
			StiBorder border = stiBorder.Border;
			StiAdvancedBorder val2 = (StiAdvancedBorder)(object)((border is StiAdvancedBorder) ? border : null);
			if (val2 != (StiAdvancedBorder)null)
			{
				if (((StiBorder)val2).IsLeftBorderSidePresent)
				{
					double num5 = (double)val2.LeftSide.GetSizeOffset() * 0.75;
					num -= num5;
					num3 -= num5;
				}
				if (((StiBorder)val2).IsRightBorderSidePresent)
				{
					num3 -= (double)val2.RightSide.GetSizeOffset() * 0.75;
				}
				if (((StiBorder)val2).IsTopBorderSidePresent)
				{
					double num6 = (double)val2.TopSide.GetSizeOffset() * 0.75;
					num2 -= num6;
					num4 -= num6;
				}
				if (((StiBorder)val2).IsBottomBorderSidePresent)
				{
					num4 -= (double)val2.BottomSide.GetSizeOffset() * 0.75;
				}
			}
			else if ((int)stiBorder.Border.Style != 6 && stiBorder.Border.Size > 0.0 && (int)stiBorder.Border.Side != 0)
			{
				double num7 = (double)stiBorder.Border.GetSizeOffset() * 0.75;
				if (stiBorder.Border.IsLeftBorderSidePresent)
				{
					num -= num7;
					num3 -= num7;
				}
				if (stiBorder.Border.IsRightBorderSidePresent)
				{
					num3 -= num7;
				}
				if (stiBorder.Border.IsTopBorderSidePresent)
				{
					num2 -= num7;
					num4 -= num7;
				}
				if (stiBorder.Border.IsBottomBorderSidePresent)
				{
					num4 -= num7;
				}
			}
		}
		StiText stiText = comp as StiText;
		if (stiText != null && !stiText.Margins.IsEmpty)
		{
			num3 -= (Math.Truncate(stiText.Margins.Left * zoom) + Math.Truncate(stiText.Margins.Right * zoom)) * 0.75;
			num4 -= (Math.Truncate(stiText.Margins.Top * zoom) + Math.Truncate(stiText.Margins.Bottom * zoom)) * 0.75;
		}
		if (num3 < 0.0)
		{
			num3 = 0.0;
		}
		if (num4 < 0.0)
		{
			num4 = 0.0;
		}
		string value = FormatCoord(num);
		string value2 = FormatCoord(num2);
		string text = FormatCoord(num3);
		string text2 = FormatCoord(num4);
		HtmlWriter.WriteStyleAttribute("left", value);
		HtmlWriter.WriteStyleAttribute("top", value2);
		HtmlWriter.WriteStyleAttribute("width", text);
		HtmlWriter.WriteStyleAttribute("height", text2);
		if (stiText != null && !stiText.Margins.IsEmpty)
		{
			HtmlWriter.WriteStyleAttribute("padding", $"{StiHtmlUnit.NewUnit(Math.Truncate(stiText.Margins.Top * zoom)).ToString()} {StiHtmlUnit.NewUnit(Math.Truncate(stiText.Margins.Right * zoom)).ToString()} {StiHtmlUnit.NewUnit(Math.Truncate(stiText.Margins.Bottom * zoom)).ToString()} {StiHtmlUnit.NewUnit(Math.Truncate(stiText.Margins.Left * zoom)).ToString()}");
		}
		if (comp is StiShape)
		{
			HtmlWriter.WriteStyleAttribute("overflow", "visible");
		}
		if (returnSize)
		{
			return text + ";" + text2;
		}
		return null;
	}

	private SizeStrings GetSizeStrings(StiComponent comp)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Invalid comparison between Unknown and I4
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		RectangleD val = comp.ComponentToPage(comp.ClientRectangle);
		double num = (double)coordX[((RectangleD)(ref val)).Right] - (double)coordX[((RectangleD)(ref val)).Left];
		double num2 = (double)coordY[((RectangleD)(ref val)).Bottom] - (double)coordY[((RectangleD)(ref val)).Top];
		if (comp is IStiBorder { Border: not null } stiBorder)
		{
			StiBorder border = stiBorder.Border;
			StiAdvancedBorder val2 = (StiAdvancedBorder)(object)((border is StiAdvancedBorder) ? border : null);
			if (val2 != (StiAdvancedBorder)null)
			{
				if (((StiBorder)val2).IsLeftBorderSidePresent)
				{
					num -= (double)val2.LeftSide.GetSizeOffset() * 0.75;
				}
				if (((StiBorder)val2).IsRightBorderSidePresent)
				{
					num -= (double)val2.RightSide.GetSizeOffset() * 0.75;
				}
				if (((StiBorder)val2).IsTopBorderSidePresent)
				{
					num2 -= (double)val2.TopSide.GetSizeOffset() * 0.75;
				}
				if (((StiBorder)val2).IsBottomBorderSidePresent)
				{
					num2 -= (double)val2.BottomSide.GetSizeOffset() * 0.75;
				}
			}
			else if ((int)stiBorder.Border.Style != 6 && stiBorder.Border.Size > 0.0 && (int)stiBorder.Border.Side != 0)
			{
				double num3 = (double)stiBorder.Border.GetSizeOffset() * 0.75;
				if (stiBorder.Border.IsLeftBorderSidePresent)
				{
					num -= num3;
				}
				if (stiBorder.Border.IsRightBorderSidePresent)
				{
					num -= num3;
				}
				if (stiBorder.Border.IsTopBorderSidePresent)
				{
					num2 -= num3;
				}
				if (stiBorder.Border.IsBottomBorderSidePresent)
				{
					num2 -= num3;
				}
			}
		}
		if (comp is StiText stiText && !stiText.Margins.IsEmpty)
		{
			num -= (Math.Truncate(stiText.Margins.Left * zoom) + Math.Truncate(stiText.Margins.Right * zoom)) * 0.75;
			num2 -= (Math.Truncate(stiText.Margins.Top * zoom) + Math.Truncate(stiText.Margins.Bottom * zoom)) * 0.75;
		}
		if (num < 0.0)
		{
			num = 0.0;
		}
		if (num2 < 0.0)
		{
			num2 = 0.0;
		}
		SizeStrings result = default(SizeStrings);
		result.Width = FormatCoord(num);
		result.Height = FormatCoord(num2);
		result.DWidth = num;
		result.DHeight = num2;
		return result;
	}

	private void RenderImage(StiComponent comp)
	{
		string text = null;
		if (comp is StiImage)
		{
			text = ((StiImage)comp).ImageURLValue as string;
		}
		if (!string.IsNullOrEmpty(text) && !text.StartsWith("http") && !text.StartsWith("ftp"))
		{
			text = null;
		}
		if (StiSvgHelper.CheckShape(comp))
		{
			RenderShape(comp, (float)zoom);
			return;
		}
		SetCurrentCulture();
		RenderImage(comp, text);
		RestoreCulture();
	}

	private void RenderImage(StiComponent component, string imageURL)
	{
		if (!(component is IStiExportImage stiExportImage))
		{
			return;
		}
		IStiExportImageExtended stiExportImageExtended = stiExportImage as IStiExportImageExtended;
		float num = (float)zoom;
		float num2 = imageResolution;
		StiImage stiImage = stiExportImage as StiImage;
		if (StiOptions.Export.Html.UseImageResolution && stiImage != null && stiImage.ExistImageToDraw())
		{
			Image val = stiImage.TryTakeGdiImageToDraw();
			try
			{
				float horizontalResolution = val.HorizontalResolution;
				if (horizontalResolution >= 50f && horizontalResolution <= 1250f)
				{
					num2 = horizontalResolution;
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (num2 != 100f)
		{
			num *= num2 / 100f;
		}
		Image val2;
		if (stiExportImageExtended != null)
		{
			if (!stiExportImageExtended.IsExportAsImage(StiExportFormat.Html))
			{
				return;
			}
			val2 = ((imageFormat == ImageFormat.Png) ? stiExportImageExtended.GetImage(ref num, StiExportFormat.ImagePng) : ((!(strSpanDiv == "span")) ? stiExportImageExtended.GetImage(ref num, StiExportFormat.HtmlDiv) : stiExportImageExtended.GetImage(ref num, StiExportFormat.HtmlSpan)));
		}
		else
		{
			val2 = stiExportImage.GetImage(ref num);
		}
		RestoreCulture();
		StiShape stiShape = stiExportImage as StiShape;
		RenderImage(val2, component, imageURL, num, (stiShape != null) ? (-(int)Math.Round(stiShape.Size * num / 2f)) : 0);
		if (val2 != null)
		{
			val2.Dispose();
		}
	}

	private void RenderImage(Image image, StiComponent component, string imageURL, float zoom, int margin)
	{
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Invalid comparison between Unknown and I4
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Invalid comparison between Unknown and I4
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Invalid comparison between Unknown and I4
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Invalid comparison between Unknown and I4
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Invalid comparison between Unknown and I4
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Invalid comparison between Unknown and I4
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Invalid comparison between Unknown and I4
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Invalid comparison between Unknown and I4
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Invalid comparison between Unknown and I4
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Invalid comparison between Unknown and I4
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Invalid comparison between Unknown and I4
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Invalid comparison between Unknown and I4
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		if (image == null)
		{
			return;
		}
		string text = imageURL;
		if (string.IsNullOrEmpty(imageURL))
		{
			if (HtmlImageHost != null)
			{
				text = HtmlImageHost.GetImageString((Bitmap)(object)((image is Bitmap) ? image : null));
			}
			if (text == null)
			{
				text = string.Empty;
			}
		}
		double num = (StiOptions.Export.Html.PrintLayoutOptimization ? 0.96 : 1.0);
		if (component is StiImage stiImage && !string.IsNullOrWhiteSpace(imageURL) && (stiImage.AspectRatio || stiImage.ImageRotation != 0))
		{
			HtmlWriter.Write("<div style=\"");
			SizeStrings sizeStrings = GetSizeStrings(stiImage);
			HtmlWriter.WriteStyleAttribute("width", sizeStrings.Width);
			HtmlWriter.WriteStyleAttribute("height", sizeStrings.Height);
			StiHorAlignment val = stiImage.HorAlignment;
			StiVertAlignment val2 = stiImage.VertAlignment;
			switch (stiImage.ImageRotation)
			{
			case StiImageRotation.Rotate90CW:
				val2 = (((int)stiImage.HorAlignment == 1) ? ((StiVertAlignment)2) : (((int)stiImage.HorAlignment != 3) ? ((StiVertAlignment)1) : ((StiVertAlignment)0)));
				val = (((int)stiImage.VertAlignment != 0) ? (((int)stiImage.VertAlignment != 2) ? ((StiHorAlignment)2) : ((StiHorAlignment)3)) : ((StiHorAlignment)1));
				break;
			case StiImageRotation.Rotate90CCW:
				val2 = (((int)stiImage.HorAlignment == 1) ? ((StiVertAlignment)0) : (((int)stiImage.HorAlignment != 3) ? ((StiVertAlignment)1) : ((StiVertAlignment)2)));
				val = (((int)stiImage.VertAlignment != 0) ? (((int)stiImage.VertAlignment != 2) ? ((StiHorAlignment)2) : ((StiHorAlignment)1)) : ((StiHorAlignment)3));
				break;
			case StiImageRotation.Rotate180:
				if ((int)stiImage.HorAlignment == 1)
				{
					val = (StiHorAlignment)3;
				}
				else if ((int)stiImage.HorAlignment == 3)
				{
					val = (StiHorAlignment)1;
				}
				if ((int)stiImage.VertAlignment == 0)
				{
					val2 = (StiVertAlignment)2;
				}
				else if ((int)stiImage.VertAlignment == 2)
				{
					val2 = (StiVertAlignment)0;
				}
				break;
			case StiImageRotation.FlipHorizontal:
				if ((int)stiImage.HorAlignment == 1)
				{
					val = (StiHorAlignment)3;
				}
				else if ((int)stiImage.HorAlignment == 3)
				{
					val = (StiHorAlignment)1;
				}
				break;
			case StiImageRotation.FlipVertical:
				if ((int)stiImage.VertAlignment == 0)
				{
					val2 = (StiVertAlignment)2;
				}
				else if ((int)stiImage.VertAlignment == 2)
				{
					val2 = (StiVertAlignment)0;
				}
				break;
			}
			if (stiImage.ImageRotation == StiImageRotation.Rotate90CCW || stiImage.ImageRotation == StiImageRotation.Rotate90CW)
			{
				HtmlWriter.Write("\"><div style=\"");
				HtmlWriter.WriteStyleAttribute("width", sizeStrings.Height);
				HtmlWriter.WriteStyleAttribute("height", sizeStrings.Width);
				double num2 = (sizeStrings.DWidth - sizeStrings.DHeight) / 2.0;
				HtmlWriter.WriteStyleAttribute("position", "relative");
				HtmlWriter.WriteStyleAttribute("left", StiHtmlUnit.NewUnit(num2).ToString());
				HtmlWriter.WriteStyleAttribute("top", StiHtmlUnit.NewUnit(0.0 - num2).ToString());
				HtmlWriter.WriteStyleAttribute("transform", "rotate(" + ((stiImage.ImageRotation == StiImageRotation.Rotate90CCW) ? "-" : "") + "90deg)");
			}
			HtmlWriter.WriteStyleAttribute("background-repeat", "no-repeat");
			HtmlWriter.WriteStyleAttribute("background-position", ((object)(StiHorAlignment)(ref val)).ToString().ToLower() + " " + ((object)(StiVertAlignment)(ref val2)).ToString().ToLower());
			HtmlWriter.WriteStyleAttribute("background-image", "url(" + StiHtmlTable.StringToUrl(imageURL) + ")");
			double num3 = sizeStrings.DWidth * stiImage.MultipleFactor * (double)zoom;
			double num4 = sizeStrings.DHeight * stiImage.MultipleFactor * (double)zoom;
			HtmlWriter.WriteStyleAttribute("background-size", (!stiImage.Stretch) ? $"{num3}px {num4}px" : (stiImage.AspectRatio ? "contain" : "100% 100%"));
			if (stiImage.ImageRotation == StiImageRotation.FlipHorizontal)
			{
				HtmlWriter.WriteStyleAttribute("transform", "scaleX(-1)");
			}
			else if (stiImage.ImageRotation == StiImageRotation.FlipVertical)
			{
				HtmlWriter.WriteStyleAttribute("transform", "scaleY(-1)");
			}
			else if (stiImage.ImageRotation == StiImageRotation.Rotate180)
			{
				HtmlWriter.WriteStyleAttribute("transform", "scale(-1)");
			}
			if (stiImage.ImageRotation == StiImageRotation.Rotate90CCW || stiImage.ImageRotation == StiImageRotation.Rotate90CW)
			{
				HtmlWriter.Write("\"></div>");
			}
			else
			{
				HtmlWriter.Write("\">");
			}
			HtmlWriter.Write("</div>");
		}
		else
		{
			HtmlWriter.WriteBeginTag("img ");
			HtmlWriter.Write("style=\"width:" + (int)((double)((float)image.Width / zoom) * this.zoom * num) + "px;");
			if (margin != 0)
			{
				HtmlWriter.Write("margin:" + margin + "px;");
			}
			HtmlWriter.Write("height:" + (int)((double)((float)image.Height / zoom) * this.zoom * num) + "px;border:0px\"");
			HtmlWriter.WriteAttribute("src", text);
			HtmlWriter.Write(">");
			HtmlWriter.WriteEndTag("img");
		}
	}

	private void RenderShape(StiComponent component, float zoom)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		if (component is StiShape stiShape)
		{
			double num = (StiOptions.Export.Html.PrintLayoutOptimization ? 0.96 : 1.0);
			RectangleD val = component.ComponentToPage(component.ClientRectangle);
			RectangleD rect = ((RectangleD)(ref val)).Normalize();
			val = stiShape.Report.Unit.ConvertToHInches(rect);
			rect = ((RectangleD)(ref val)).Multiply((double)zoom);
			((RectangleD)(ref rect)).X = 0.0;
			((RectangleD)(ref rect)).Y = 0.0;
			int num2 = (int)(((RectangleD)(ref rect)).Width + 0.5) + 1 + (int)(stiShape.Size * zoom);
			int num3 = (int)(((RectangleD)(ref rect)).Height + 0.5) + 1 + (int)(stiShape.Size * zoom);
			int num4 = (int)Math.Round(stiShape.Size * zoom / 2f);
			HtmlWriter.WriteBeginTag("img ");
			HtmlWriter.Write("style=\"width:" + (int)((double)num2 * num) + "px;");
			HtmlWriter.Write("height:" + (int)((double)num3 * num) + "px;border:0px\"");
			if (num4 != 0)
			{
				HtmlWriter.Write("margin:" + num4 + "px;");
			}
			HtmlWriter.WriteAttribute("src", StiSvgHelper.RenderShapeAsBase64(component));
			HtmlWriter.Write(">");
			HtmlWriter.WriteEndTag("img");
		}
	}

	private bool RenderHyperlink(StiComponent comp)
	{
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		string text = comp.HyperlinkValue as string;
		if (!string.IsNullOrEmpty(text))
		{
			text = text.Trim();
			if (text.StartsWith("javascript:"))
			{
				text = null;
			}
		}
		if (!string.IsNullOrWhiteSpace(text) && text.StartsWith("##"))
		{
			if (text.Length > 2 && text[2] == '#')
			{
				text = text.Substring(2);
				string text2 = (string)pointerToBookmark[text.Substring(1)];
				if (text2 != null)
				{
					text = "#" + text2;
				}
			}
			else
			{
				text = text.Substring(1);
			}
		}
		string text3 = comp.BookmarkValue as string;
		string text4 = comp.TagValue as string;
		if (text3 == null)
		{
			if (!string.IsNullOrEmpty(text4) && hyperlinksToTag.ContainsKey(text4))
			{
				text3 = text4;
			}
			else if (!string.IsNullOrWhiteSpace((string)comp.PointerValue) && !string.IsNullOrEmpty(comp.Guid))
			{
				text3 = $"{comp.PointerValue}#GUID#{comp.Guid}";
			}
		}
		string value = null;
		if (!string.IsNullOrWhiteSpace(comp.Guid) && hashBookmarkGuid.ContainsKey(comp.Guid))
		{
			value = comp.Guid;
		}
		if (!string.IsNullOrWhiteSpace(text))
		{
			RectangleD val = comp.ComponentToPage(comp.ClientRectangle);
			string text5 = FormatCoord((double)coordY[((RectangleD)(ref val)).Bottom] - (double)coordY[((RectangleD)(ref val)).Top]);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("display:block;height:" + text5 + ";text-decoration:none;");
			if (comp is IStiTextBrush)
			{
				IStiTextBrush stiTextBrush = comp as IStiTextBrush;
				Color color = StiBrush.ToColor(stiTextBrush.TextBrush);
				stringBuilder.Append("color:" + FormatColor(color) + ";");
			}
			if (comp is IStiFont)
			{
				IStiFont stiFont = comp as IStiFont;
				if (stiFont.Font.Underline)
				{
					stringBuilder.Append("text-decoration:underline;");
				}
				else
				{
					stringBuilder.Append("text-decoration:none;");
				}
			}
			HtmlWriter.WriteBeginTag("a");
			if (!string.IsNullOrWhiteSpace(openLinksTarget))
			{
				HtmlWriter.WriteAttribute("target", openLinksTarget);
			}
			if (!string.IsNullOrWhiteSpace(text3))
			{
				HtmlWriter.WriteAttribute("name", text3.Replace("'", ""));
			}
			if (!string.IsNullOrWhiteSpace(value))
			{
				HtmlWriter.WriteAttribute("guid", value);
			}
			HtmlWriter.WriteAttribute("style", stringBuilder.ToString());
			HtmlWriter.WriteAttribute("href", text);
			HtmlWriter.Write(">");
			return true;
		}
		if (!string.IsNullOrWhiteSpace(text3) || !string.IsNullOrWhiteSpace(value))
		{
			HtmlWriter.WriteBeginTag("a");
			if (!string.IsNullOrWhiteSpace(openLinksTarget))
			{
				HtmlWriter.WriteAttribute("target", openLinksTarget);
			}
			if (!string.IsNullOrWhiteSpace(text3))
			{
				HtmlWriter.WriteAttribute("name", text3.Replace("'", ""));
			}
			if (!string.IsNullOrWhiteSpace(value))
			{
				HtmlWriter.WriteAttribute("guid", value);
			}
			if (StiOptions.Export.Html.UseExtendedStyle)
			{
				HtmlWriter.WriteAttribute("class", "sBaseStyleFix");
			}
			HtmlWriter.Write(">");
			return true;
		}
		return false;
	}

	private void RenderPage(StiPagesCollection pages, bool useBookmarksTree, int bookmarkWidth, StiPage page = null, bool writePageBreak = false)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Invalid comparison between Unknown and I4
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Invalid comparison between Unknown and I4
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Invalid comparison between Unknown and I4
		if (exportSettings.AddPageBreaks)
		{
			HtmlWriter.WriteBeginTag("div");
			HtmlWriter.WriteAttribute("class", "pagemargins");
			if (writePageBreak)
			{
				HtmlWriter.Write(" style=\"");
				HtmlWriter.WriteStyleAttribute("page-break-before", "always");
				HtmlWriter.Write("\"");
			}
			HtmlWriter.WriteLine(">");
		}
		if ((int)PageHorAlignment != 1 && !exportSettings.AddPageBreaks)
		{
			HtmlWriter.WriteBeginTag(strSpanDiv + " style=\"");
			HtmlWriter.WriteStyleAttribute("text-align", ((int)PageHorAlignment == 2) ? "center" : "right");
			if (useBookmarksTree)
			{
				HtmlWriter.WriteStyleAttribute("margin-left", $"{bookmarkWidth + 4}px");
			}
			HtmlWriter.Write("\">");
			HtmlWriter.Indent++;
			HtmlWriter.WriteLine();
		}
		HtmlWriter.WriteBeginTag(strSpanDiv + " class=\"StiPageContainer\" style=\"");
		if (RenderAsDocument)
		{
			if ((int)PageHorAlignment != 1)
			{
				HtmlWriter.WriteStyleAttribute("display", "inline-block");
			}
			else if (useBookmarksTree)
			{
				HtmlWriter.WriteStyleAttribute("left", $"{bookmarkWidth + 4}px");
			}
			if (page != null)
			{
				double num = report.Unit.ConvertToHInches(page.PageWidth - page.Margins.Left - page.Margins.Right);
				double num2 = report.Unit.ConvertToHInches(page.PageHeight - page.Margins.Top - page.Margins.Bottom);
				HtmlWriter.WriteStyleAttribute("width", FormatCoord(num * zoom * 0.716));
				HtmlWriter.WriteStyleAttribute("height", FormatCoord(num2 * zoom * 0.716));
			}
			else
			{
				HtmlWriter.WriteStyleAttribute("width", FormatCoord(TotalPageWidth * zoom * 0.716));
				HtmlWriter.WriteStyleAttribute("height", FormatCoord(TotalPageHeight * zoom * 0.716));
			}
			HtmlWriter.WriteStyleAttribute("position", "relative");
			HtmlWriter.WriteStyleAttribute("white-space", "normal");
			Color color = Color.Transparent;
			if (pages != null && pages.Count > 0)
			{
				if (pages[0].Brush != null)
				{
					color = StiBrush.ToColor(pages[0].Brush);
				}
				if (color.A == 0 && !(pages[0].Brush is StiEmptyBrush))
				{
					color = Color.White;
				}
				if (pages[0].Border != null)
				{
					RenderBorder((StiComponent)pages[0]);
				}
			}
			HtmlWriter.WriteStyleAttribute("background-color", FormatColor(color));
			string backgroundImagePath = GetBackgroundImagePath(pages, zoom, exportSettings.UseWatermarkMargins);
			if (!string.IsNullOrWhiteSpace(backgroundImagePath))
			{
				HtmlWriter.WriteStyleAttribute("background-image", $"url('{backgroundImagePath}')");
			}
		}
		HtmlWriter.Write("\">");
		HtmlWriter.Indent++;
		HtmlWriter.WriteLine();
	}

	private void RenderEndPage()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		HtmlWriter.Indent--;
		HtmlWriter.WriteEndTag(strSpanDiv);
		HtmlWriter.WriteLine();
		if ((int)PageHorAlignment != 1 && !exportSettings.AddPageBreaks)
		{
			HtmlWriter.Indent--;
			HtmlWriter.WriteEndTag(strSpanDiv);
			HtmlWriter.WriteLine();
		}
		if (exportSettings.AddPageBreaks)
		{
			HtmlWriter.WriteLine();
			HtmlWriter.WriteFullEndTag("div");
		}
	}

	private void RenderStartDoc(StiHtmlTableRender render, bool asTable, bool useBookmarks, bool exportBookmarksOnly, bool useStimulsoftFont, Hashtable cssStyles, StiPagesCollection pages, Encoding encoding)
	{
		HtmlWriter.WriteLine("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\">");
		HtmlWriter.Write("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
		HtmlWriter.WriteLine();
		HtmlWriter.Indent++;
		HtmlWriter.WriteFullBeginTag("head");
		HtmlWriter.WriteLine();
		HtmlWriter.Indent++;
		if (HtmlImageHost is StiMhtImageHost)
		{
			HtmlWriter.WriteBeginTag("meta");
			HtmlWriter.WriteAttribute("http-equiv", "X-UA-Compatible");
			HtmlWriter.WriteAttribute("content", "IE=11.0000");
			HtmlWriter.WriteEndTag("meta");
		}
		HtmlWriter.WriteFullBeginTag("title");
		HtmlWriter.Write(report.ReportAlias);
		HtmlWriter.WriteEndTag("title");
		HtmlWriter.WriteLine();
		HtmlWriter.WriteBeginTag("meta");
		HtmlWriter.WriteAttribute("http-equiv", "Content-Type");
		HtmlWriter.WriteAttribute("content", $"text/html; charset={encoding.WebName}");
		HtmlWriter.WriteEndTag("meta");
		HtmlWriter.WriteLine();
		if (useStimulsoftFont)
		{
			using Stream stream = typeof(StiFontIconsHelper).Assembly.GetManifestResourceStream("Stimulsoft.Base.FontIcons.Stimulsoft.ttf");
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, array.Length);
			string value = Convert.ToBase64String(array);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<style>@font-face {font-family: 'Stimulsoft';src: url(data:font/ttf;base64,");
			stringBuilder.Append(value);
			stringBuilder.Append(") format('truetype');font-weight: normal;font-style: normal;}</style>");
			HtmlWriter.Write(stringBuilder.ToString());
		}
		if (render != null)
		{
			if (asTable)
			{
				render.RenderStylesTable(useBookmarks, exportBookmarksOnly, cssStyles);
			}
			else
			{
				render.RenderStyles(useBookmarks, exportBookmarksOnly, cssStyles);
			}
		}
		if (useBookmarks)
		{
			RenderBookmarkScript();
		}
		HtmlWriter.Indent--;
		HtmlWriter.WriteLine();
		HtmlWriter.WriteEndTag("head");
		HtmlWriter.WriteLine();
		HtmlWriter.WriteBeginTag("body");
		if (pages != null && pages.Count > 0 && pages[0].Brush != null)
		{
			Color color = StiBrush.ToColor(pages[0].Brush);
			if (color.A > 0)
			{
				HtmlWriter.WriteAttribute("bgcolor", FormatColor(color));
			}
		}
		if (StiOptions.Export.Html.UseExtendedStyle)
		{
			HtmlWriter.WriteAttribute("class", "sBaseStyleFix");
		}
		if (StiOptions.Export.Html.PrintLayoutOptimization)
		{
			HtmlWriter.WriteAttribute("style", "margin:0;");
		}
		HtmlWriter.Write(">");
		HtmlWriter.Indent++;
		HtmlWriter.WriteLine();
	}

	private void FillBitmapBackground(ref Bitmap bmp, Color fillColor)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		Color color = Color.LightGray;
		if (((Image)bmp).Height > 0 && ((Image)bmp).Width > 0)
		{
			color = bmp.GetPixel(0, ((Image)bmp).Size.Height - 1);
		}
		if (color.A != byte.MaxValue)
		{
			Size size = ((Image)bmp).Size;
			PixelFormat pixelFormat = ((Image)bmp).PixelFormat;
			Bitmap val = new Bitmap(size.Width, size.Height, (PixelFormat)2498570);
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			val2.Clear(fillColor);
			Rectangle rectangle = new Rectangle(0, 0, size.Width, size.Height);
			ImageAttributes val3 = new ImageAttributes();
			val3.SetColorKey(color, color);
			val2.DrawImage((Image)(object)bmp, rectangle, 0, 0, size.Width, size.Height, (GraphicsUnit)2, val3, (DrawImageAbort)null, IntPtr.Zero);
			val3.Dispose();
			val2.Dispose();
			((Image)bmp).Dispose();
			bmp = val;
		}
	}

	private void RenderBookmarkScript()
	{
		HtmlWriter.WriteBeginTag("script");
		HtmlWriter.WriteAttribute("type", "text/javascript");
		HtmlWriter.WriteLine(">");
		byte[] file = GetFile("Stimulsoft.Report", "Stimulsoft.Report.Export.Dtree.DtreeScript.js");
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < file.Length; i++)
		{
			stringBuilder.Append((char)file[i]);
		}
		string[,] array = new string[12, 2]
		{
			{ "img/base.gif", "DtreeBase.png" },
			{ "img/page.gif", "DtreePage.png" },
			{ "img/folder.gif", "DtreeFolder.png" },
			{ "img/folderopen.gif", "DtreeFolderopen.png" },
			{ "img/empty.gif", "DtreeEmpty.png" },
			{ "img/line.gif", "DtreeLine.png" },
			{ "img/join.gif", "DtreeJoin.png" },
			{ "img/joinbottom.gif", "DtreeJoinbottom.png" },
			{ "img/plus.gif", "DtreePlus.png" },
			{ "img/plusbottom.gif", "DtreePlusbottom.png" },
			{ "img/minus.gif", "DtreeMinus.png" },
			{ "img/minusbottom.gif", "DtreeMinusbottom.png" }
		};
		HtmlImageHost.ForcePng = true;
		for (int j = 0; j < array.GetLength(0); j++)
		{
			Bitmap bmp = GetImage("Stimulsoft.Report", "Stimulsoft.Report.Export.Dtree." + array[j, 1], makeTransparent: false);
			FillBitmapBackground(ref bmp, Color.FromArgb(240, 240, 240));
			string newValue = HtmlImageHost.GetImageString(bmp).Replace("\\", "/");
			stringBuilder.Replace(array[j, 0], newValue);
		}
		HtmlImageHost.ForcePng = false;
		string[] array2 = stringBuilder.ToString().Split(new char[1] { '\r' });
		for (int k = 0; k < array2.Length; k++)
		{
			HtmlWriter.WriteLine(array2[k]);
		}
		HtmlWriter.WriteEndTag("script");
		HtmlWriter.WriteLine();
	}

	private void RenderChartScripts(bool writeScriptTag = true)
	{
		if (chartData.Count == 0)
		{
			return;
		}
		if (writeScriptTag)
		{
			HtmlWriter.WriteBeginTag("script");
			HtmlWriter.WriteAttribute("type", "text/javascript");
			HtmlWriter.WriteLine(">");
		}
		string text = Guid.NewGuid().ToString().Replace("-", "");
		string[] array = new string[1] { "stianimation.js" };
		for (int i = 0; i < array.Length; i++)
		{
			byte[] file = GetFile(((object)this).GetType().Assembly, "Stimulsoft.Report.Export.Services.Htmls.ChartScripts." + array[i]);
			string @string = Encoding.ASCII.GetString(file);
			HtmlWriter.WriteLine(@string.Replace("animateSti", "animateSti" + text) + ";");
		}
		HtmlWriter.WriteLine("setTimeout(function() {");
		foreach (object key in chartData.Keys)
		{
			HtmlWriter.WriteLine($"animateSti{text}(\"{chartData[key]}\");");
		}
		HtmlWriter.WriteLine("}, 300);");
		if (writeScriptTag)
		{
			HtmlWriter.WriteEndTag("script");
			HtmlWriter.WriteLine();
		}
	}

	internal string GetGuid(StiComponent comp)
	{
		if (!chartData.ContainsKey(comp))
		{
			chartData.Add(comp, Guid.NewGuid().ToString().Replace("-", ""));
		}
		return chartData[comp] as string;
	}

	private void RenderEndDoc()
	{
		HtmlWriter.Indent--;
		HtmlWriter.WriteLine();
		HtmlWriter.WriteEndTag("body");
		HtmlWriter.Indent--;
		HtmlWriter.WriteLine();
		HtmlWriter.WriteEndTag("html");
	}

	private void RenderBookmarkTree(StiBookmark root, int bookmarkWidth, Hashtable bookmarksPageIndex)
	{
		List<StiBookmarkTreeNode> list = new List<StiBookmarkTreeNode>();
		AddBookmarkNode(root, -1, list);
		HtmlWriter.WriteStyleAttribute("width", $"{bookmarkWidth}px");
		HtmlWriter.WriteStyleAttribute("background-color", "#f0f0f0");
		HtmlWriter.Write("\">");
		HtmlWriter.Indent++;
		HtmlWriter.WriteLine();
		HtmlWriter.Indent++;
		HtmlWriter.WriteFullBeginTag("frame");
		HtmlWriter.WriteLine();
		HtmlWriter.WriteBeginTag("script");
		HtmlWriter.WriteAttribute("type", "text/javascript");
		HtmlWriter.WriteLine(">");
		HtmlWriter.Indent++;
		HtmlWriter.WriteLine("<!--");
		HtmlWriter.WriteLine("bmrk = new dTree('bmrk');");
		for (int i = 0; i < list.Count; i++)
		{
			StiBookmarkTreeNode stiBookmarkTreeNode = list[i];
			string text = stiBookmarkTreeNode.Url;
			string text2;
			if (bookmarksPageIndex.ContainsKey(stiBookmarkTreeNode.Title))
			{
				text2 = $"Page {(int)bookmarksPageIndex[stiBookmarkTreeNode.Title] + 1}";
			}
			else if (!string.IsNullOrWhiteSpace(stiBookmarkTreeNode.FullUrl) && bookmarksPageIndex.ContainsKey(stiBookmarkTreeNode.FullUrl))
			{
				text2 = $"Page {(int)bookmarksPageIndex[stiBookmarkTreeNode.FullUrl] + 1}";
				text = "#" + stiBookmarkTreeNode.FullUrl.Replace("\\", "\\\\");
			}
			else
			{
				text2 = "Page 0";
			}
			HtmlWriter.WriteLine($"bmrk.add({i}, {stiBookmarkTreeNode.Parent}, '{SecurityElement.Escape(stiBookmarkTreeNode.Title)}', '{text}', '{text2}');");
		}
		HtmlWriter.WriteLine("document.write(bmrk);");
		HtmlWriter.WriteLine("//-->");
		HtmlWriter.Indent--;
		HtmlWriter.WriteEndTag("script");
		HtmlWriter.Indent--;
		HtmlWriter.WriteLine();
		HtmlWriter.Indent--;
	}

	private void AddBookmarkNode(StiBookmark bkm, int parentNode, List<StiBookmarkTreeNode> bookmarksTree, string path = null)
	{
		StiBookmarkTreeNode stiBookmarkTreeNode = new StiBookmarkTreeNode();
		stiBookmarkTreeNode.Parent = parentNode;
		string text = (stiBookmarkTreeNode.Title = bkm.Text.Replace("'", "\\'").Replace("\r", "").Replace("\n", ""));
		stiBookmarkTreeNode.Url = "#" + text;
		if (!string.IsNullOrWhiteSpace(path))
		{
			path = path + "\\" + text;
		}
		stiBookmarkTreeNode.FullUrl = path;
		stiBookmarkTreeNode.Used = true;
		bookmarksTree.Add(stiBookmarkTreeNode);
		int parentNode2 = bookmarksTree.Count - 1;
		if (bkm.Bookmarks.Count != 0)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				path = "%";
			}
			for (int i = 0; i < bkm.Bookmarks.Count; i++)
			{
				AddBookmarkNode(bkm.Bookmarks[i], parentNode2, bookmarksTree, path);
			}
		}
	}

	internal string PrepareTextForHtml(string text, bool processWhiteSpaces = true)
	{
		if (text == null)
		{
			return null;
		}
		text = text.Replace("\r", "");
		StringBuilder stringBuilder = new StringBuilder();
		if (processWhiteSpaces)
		{
			string[] array = text.Split(new char[1] { '\n' });
			for (int i = 0; i < array.Length; i++)
			{
				string text2 = array[i];
				int j;
				for (j = 0; j < text2.Length && text2[j] == ' '; j++)
				{
				}
				if (j > 0)
				{
					for (int k = 0; k < j; k++)
					{
						stringBuilder.Append("&nbsp;");
					}
					stringBuilder.Append(text2.Substring(j));
				}
				else
				{
					stringBuilder.Append(text2);
				}
				if (i < array.Length - 1)
				{
					stringBuilder.Append("<br>");
				}
			}
		}
		else
		{
			stringBuilder.Append(text.Replace("\n", "<br>"));
		}
		return stringBuilder.ToString();
	}

	internal static string ConvertTextWithHtmlTagsToHtmlText(StiText stiText, string text, double zoom)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0707: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0732: Unknown result type (might be due to invalid IL or missing references)
		//IL_0711: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Unknown result type (might be due to invalid IL or missing references)
		//IL_073c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0781: Unknown result type (might be due to invalid IL or missing references)
		//IL_0788: Unknown result type (might be due to invalid IL or missing references)
		//IL_0767: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0792: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07de: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0545: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		StiHtmlTagsState val = default(StiHtmlTagsState);
		((StiHtmlTagsState)(ref val))._002Ector(stiText.Font.Bold, stiText.Font.Italic, stiText.Font.Underline, stiText.Font.Strikeout, stiText.Font.SizeInPoints, stiText.Font.Name, StiBrush.ToColor(stiText.TextBrush), StiBrush.ToColor(stiText.Brush), false, false, 0.0, 0.0, (double)stiText.LineSpacing, stiText.HorAlignment);
		StiHtmlState val2 = default(StiHtmlState);
		((StiHtmlState)(ref val2))._002Ector(val, 0);
		List<StiHtmlState> list = StiTextRenderer.ParseHtmlToStates(text, val2, false);
		StringBuilder stringBuilder = new StringBuilder();
		StiTextHorAlignment textAlign = (StiTextHorAlignment)0;
		double lineHeight = stiText.LineSpacing;
		bool flag = true;
		bool flag2 = false;
		StringBuilder stringBuilder2 = new StringBuilder();
		StiHtmlTagsState val3 = val;
		for (int i = 0; i < list.Count; i++)
		{
			StiHtmlState val4 = list[i];
			StiHtmlTagsState tS = val4.TS;
			if (tS.Bold != val3.Bold && !tS.Bold)
			{
				stringBuilder2.Append("</b>");
			}
			if (tS.Italic != val3.Italic && !tS.Italic)
			{
				stringBuilder2.Append("</i>");
			}
			if (tS.Underline != val3.Underline && !tS.Underline)
			{
				stringBuilder2.Append("</u>");
			}
			if (tS.Strikeout != val3.Strikeout && !tS.Strikeout)
			{
				stringBuilder2.Append("</s>");
			}
			if (tS.Superscript != val3.Superscript && !tS.Superscript)
			{
				stringBuilder2.Append("</sup>");
			}
			if (tS.Subscript != val3.Subscript && !tS.Subscript)
			{
				stringBuilder2.Append("</sub>");
			}
			if (tS.Bold != val3.Bold && tS.Bold)
			{
				stringBuilder2.Append("<b>");
			}
			if (tS.Italic != val3.Italic && tS.Italic)
			{
				stringBuilder2.Append("<i>");
			}
			if (tS.Underline != val3.Underline && tS.Underline)
			{
				stringBuilder2.Append("<u>");
			}
			if (tS.Strikeout != val3.Strikeout && tS.Strikeout)
			{
				stringBuilder2.Append("<s>");
			}
			if (tS.Superscript != val3.Superscript && tS.Superscript)
			{
				stringBuilder2.Append("<sup>");
			}
			if (tS.Subscript != val3.Subscript && tS.Subscript)
			{
				stringBuilder2.Append("<sub>");
			}
			if (tS.FontColor != val3.FontColor || tS.BackColor != val3.BackColor || tS.FontName != val3.FontName || tS.FontSize != val3.FontSize || tS.LetterSpacing != val3.LetterSpacing || tS.WordSpacing != val3.WordSpacing || tS.HtmlStyle != val3.HtmlStyle)
			{
				string text2 = "";
				if (tS.HtmlStyle != val.HtmlStyle)
				{
					text2 = text2 + tS.HtmlStyle + ";";
				}
				if (tS.FontColor != val.FontColor && !text2.Contains("color:") && tS.FontColor.A > 0)
				{
					text2 += $"color:#{tS.FontColor.ToArgb() & 0xFFFFFF:X6};";
				}
				if (tS.BackColor != val.BackColor && !text2.Contains("background-color:") && tS.BackColor.A > 0)
				{
					text2 += $"background-color:#{tS.BackColor.ToArgb() & 0xFFFFFF:X6};";
				}
				if (tS.FontName != val.FontName && !text2.Contains("font-family:"))
				{
					text2 += $"font-family:{tS.FontName};";
				}
				if (tS.FontSize != val.FontSize && !text2.Contains("font-size:"))
				{
					double num = (double)tS.FontSize * zoom;
					text2 += $"font-size:{num}pt;".Replace(",", ".");
					text2 += $"line-height:{Math.Round(tS.LineHeight * 1.12, 2)}em;".Replace(",", ".");
				}
				if (tS.LetterSpacing != val.LetterSpacing && !text2.Contains("letter-spacing:"))
				{
					text2 += $"letter-spacing:{tS.LetterSpacing}em;".Replace(",", ".");
				}
				if (tS.WordSpacing != val.WordSpacing && !text2.Contains("word-spacing:"))
				{
					text2 += $"word-spacing:{tS.WordSpacing}em;".Replace(",", ".");
				}
				if (flag2)
				{
					stringBuilder2.Append("</font>");
				}
				flag2 = false;
				if (text2.Length > 0)
				{
					flag2 = true;
					stringBuilder2.Append($"<font style=\"{text2}\">");
				}
			}
			textAlign = tS.TextAlign;
			double lineHeight2 = tS.LineHeight;
			if (val4.Text.ToString() == "\n")
			{
				if (flag)
				{
					stringBuilder2.Append("&nbsp;");
				}
				stringBuilder.Append(GetParagraphString(stringBuilder2, textAlign, lineHeight2, (double)stiText.LineSpacing * GetFontHeightCorrection(tS.FontName), stiText.RightToLeft));
				stringBuilder2 = new StringBuilder();
				flag = true;
				lineHeight = lineHeight2;
			}
			else
			{
				string text3 = tS.Href;
				if (!string.IsNullOrEmpty(text3))
				{
					text3 = text3.Trim();
					if (text3.StartsWith("javascript:"))
					{
						text3 = null;
					}
				}
				if (!string.IsNullOrEmpty(text3))
				{
					stringBuilder2.AppendFormat("<a style=\"text-decoration:none;\" href=\"{0}\">", text3);
				}
				stringBuilder2.Append((object?)val4.Text);
				if (!string.IsNullOrEmpty(text3))
				{
					stringBuilder2.Append("</a>");
				}
				if (val4.Text.ToString().Trim().Length > 0)
				{
					flag = false;
				}
			}
			val3 = tS;
		}
		if (stringBuilder2.Length > 0)
		{
			stringBuilder.Append(GetParagraphString(stringBuilder2, textAlign, lineHeight, (double)stiText.LineSpacing * GetFontHeightCorrection(val3.FontName), stiText.RightToLeft));
		}
		if (flag2)
		{
			stringBuilder.Append("</font>");
		}
		if (val3.Bold != val.Bold)
		{
			stringBuilder.Append(val.Bold ? "<b>" : "</b>");
		}
		if (val3.Italic != val.Italic)
		{
			stringBuilder.Append(val.Italic ? "<i>" : "</i>");
		}
		if (val3.Underline != val.Underline)
		{
			stringBuilder.Append(val.Underline ? "<u>" : "</u>");
		}
		if (val3.Strikeout != val.Strikeout)
		{
			stringBuilder.Append(val.Strikeout ? "<s>" : "</s>");
		}
		if (val3.Superscript != val.Superscript)
		{
			stringBuilder.Append(val.Superscript ? "<sup>" : "</sup>");
		}
		if (val3.Subscript != val.Subscript)
		{
			stringBuilder.Append(val.Subscript ? "<sub>" : "</sub>");
		}
		return stringBuilder.ToString();
	}

	private static string GetParagraphString(StringBuilder text, StiTextHorAlignment textAlign, double lineHeight, double baseLineSpacing, bool rtl)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Invalid comparison between Unknown and I4
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Invalid comparison between Unknown and I4
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<p ");
		stringBuilder.Append("style=\"margin:0px;");
		string arg = (rtl ? "right" : "left");
		if ((int)textAlign == 1)
		{
			arg = "center";
		}
		if ((int)textAlign == 2)
		{
			arg = (rtl ? "left" : "right");
		}
		if ((int)textAlign == 3)
		{
			arg = "justify";
		}
		stringBuilder.Append($"text-align:{arg};");
		if (lineHeight != baseLineSpacing)
		{
			stringBuilder.Append($"line-height:{Math.Round(lineHeight * 1.12, 2)}em;".Replace(",", "."));
		}
		stringBuilder.Append("\">");
		stringBuilder.Append((object?)text);
		stringBuilder.Append("</p>");
		return stringBuilder.ToString();
	}

	private string GetBackgroundImagePath(StiPagesCollection pages, double zoomBase, bool useMargins)
	{
		string text = string.Empty;
		if (pages.Count > 0)
		{
			StiPage stiPage = pages[0];
			StiWatermark watermark = stiPage.Watermark;
			if (watermark != null && watermark.Enabled && (watermark.ExistImage() || !string.IsNullOrWhiteSpace(watermark.ImageHyperlink) || !string.IsNullOrEmpty(watermark.Text)))
			{
				IStiPagePainter stiPagePainter = StiPainter.GetPainter(typeof(StiPage), (StiGuiMode)0) as IStiPagePainter;
				double num = (StiOptions.Export.Html.PrintLayoutOptimization ? (zoomBase * 0.96) : zoomBase);
				Bitmap watermarkImage = stiPagePainter.GetWatermarkImage(stiPage, num, useMargins);
				if (HtmlImageHost != null)
				{
					text = HtmlImageHost.GetImageString(watermarkImage).Replace('\\', '/');
				}
				if (text == null)
				{
					text = string.Empty;
				}
				if (watermarkImage != null)
				{
					((Image)watermarkImage).Dispose();
				}
			}
		}
		return text.Replace('\\', '/');
	}

	private static string ImageBytesToBase64String(byte[] image, double pageWidth, double pageHeight)
	{
		string text = "data:image/png;base64,";
		if (Stimulsoft.Report.Helpers.StiImageHelper.IsMetafile(image))
		{
			image = StiMetafileConverter.MetafileToPngBytes(image, (int)pageWidth, (int)pageHeight);
		}
		else if (Stimulsoft.Report.Helpers.StiImageHelper.IsBmp(image))
		{
			text = "data:image/bmp;base64,";
		}
		else if (Stimulsoft.Report.Helpers.StiImageHelper.IsJpeg(image))
		{
			text = "data:image/jpeg;base64,";
		}
		else if (Stimulsoft.Report.Helpers.StiImageHelper.IsGif(image))
		{
			text = "data:image/gif;base64,";
		}
		else if (StiSvgHelper.IsSvg(image))
		{
			text = "data:image/svg+xml;base64,";
		}
		return text + Convert.ToBase64String(image);
	}

	private void RenderWatermarkImage(StiPage page, double topPos = 0.0)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Invalid comparison between Unknown and I4
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Invalid comparison between Unknown and I4
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Expected I4, but got Unknown
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Invalid comparison between Unknown and I4
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Invalid comparison between Unknown and I4
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Invalid comparison between Unknown and I4
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Invalid comparison between Unknown and I4
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Invalid comparison between Unknown and I4
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Invalid comparison between Unknown and I4
		if (page == null || page.Watermark == null || !page.Watermark.Enabled || (!page.Watermark.ExistImage() && string.IsNullOrWhiteSpace(page.Watermark.ImageHyperlink)))
		{
			return;
		}
		RectangleD val = page.Unit.ConvertToHInches(page.ClientRectangle);
		double num = ((RectangleD)(ref val)).Width * zoom;
		double num2 = ((RectangleD)(ref val)).Height * zoom * (StiOptions.Export.Html.PrintLayoutOptimization ? 0.956 : 1.0);
		int num3 = ((!page.Watermark.ShowImageBehind) ? 1 : 0);
		int num4 = 0;
		string text = string.Empty;
		if (page.Watermark.TakeImage() != null)
		{
			if (page.Watermark.TakeGdiImage() != null)
			{
				Image val2 = page.Watermark.TakeGdiImage();
				try
				{
					text = ImageBytesToBase64String(page.Watermark.TakeImage(), num, num2);
					num4 = val2.Width;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
		else if (!string.IsNullOrWhiteSpace(page.Watermark.ImageHyperlink))
		{
			if (StiHyperlinkProcessor.IsResourceHyperlink(page.Watermark.ImageHyperlink))
			{
				StiResource stiResource = page.Report.Dictionary.Resources[StiHyperlinkProcessor.GetResourceNameFromHyperlink(page.Watermark.ImageHyperlink)];
				if (stiResource != null)
				{
					Image val3 = StiImageConverter.BytesToImage(stiResource.Content, 200, 200, true, false);
					try
					{
						text = ImageBytesToBase64String(stiResource.Content, num, num2);
						num4 = val3.Width;
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
			}
			else if (StiHyperlinkProcessor.IsVariableHyperlink(page.Watermark.ImageHyperlink))
			{
				StiVariable stiVariable = page.Report.Dictionary.Variables[StiHyperlinkProcessor.GetVariableNameFromHyperlink(page.Watermark.ImageHyperlink)];
				if (stiVariable != null && stiVariable.ValueObject != null)
				{
					object valueObject = stiVariable.ValueObject;
					Image val4 = (Image)((valueObject is Image) ? valueObject : null);
					text = ImageBytesToBase64String(Stimulsoft.Report.Helpers.StiImageHelper.GetImageBytesFromObject(val4), num, num2);
					num4 = val4.Width;
				}
			}
		}
		double num5 = (double)num4 * page.Watermark.ImageMultipleFactor * zoom;
		string text2 = "";
		string arg = "center";
		string arg2 = "center";
		ContentAlignment imageAlignment = page.Watermark.ImageAlignment;
		if ((int)imageAlignment <= 32)
		{
			switch (imageAlignment - 1)
			{
			default:
				if ((int)imageAlignment != 16)
				{
					if ((int)imageAlignment == 32)
					{
						arg = "center";
						arg2 = "center";
					}
				}
				else
				{
					arg = "left";
					arg2 = "center";
				}
				break;
			case 0:
				arg = "left";
				arg2 = "top";
				break;
			case 1:
				arg = "center";
				arg2 = "top";
				break;
			case 3:
				arg = "right";
				arg2 = "top";
				break;
			case 2:
				break;
			}
		}
		else if ((int)imageAlignment <= 256)
		{
			if ((int)imageAlignment != 64)
			{
				if ((int)imageAlignment == 256)
				{
					arg = "left";
					arg2 = "bottom";
				}
			}
			else
			{
				arg = "right";
				arg2 = "center";
			}
		}
		else if ((int)imageAlignment != 512)
		{
			if ((int)imageAlignment == 1024)
			{
				arg = "right";
				arg2 = "bottom";
			}
		}
		else
		{
			arg = "center";
			arg2 = "bottom";
		}
		text2 += $"background-position-x: {arg};";
		text2 += $"background-position-y: {arg2};";
		if (page.Watermark.ImageStretch)
		{
			text2 += "background-size: 100% 100%;";
			text2 += "background-repeat: no-repeat;";
		}
		else
		{
			text2 += $"background-size: {((num5 != 0.0) ? Math.Round(num5 / num * 100.0) : 100.0)}%;";
			text2 += string.Format("background-repeat: {0};", page.Watermark.ImageTiling ? "repeat" : "no-repeat");
		}
		if (page.Watermark.ImageTransparency != 0)
		{
			double num6 = Math.Round((double)(255 - page.Watermark.ImageTransparency) / 255.0, 4);
			text2 += $" filter: alpha(Opacity={num6 * 10.0}); opacity: {num6}; -moz-opacity: {num6}; -khtml-opacity: {num6};";
		}
		HtmlWriter.WriteLine(string.Format("<div class='stiWatermarkImage' style=\"position: absolute; pointer-events: none; width: {0}px; height: {1}px; left: 0px; top: {5}px; right: 0px; bottom: 0px; z-index: {2}; background-image: url({3}); {4};\"></div>", num, num2, num3, text, text2, topPos));
	}

	private void RenderWatermarkText(StiPage page, double topPos = 0.0)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		if (page != null && !string.IsNullOrEmpty(page.Watermark.Text) && page.Watermark.Enabled)
		{
			RectangleD val = page.Unit.ConvertToHInches(page.DisplayRectangle);
			if (StiOptions.Export.Html.PrintLayoutOptimization)
			{
				((RectangleD)(ref val)).Height = ((RectangleD)(ref val)).Height * 0.956;
			}
			double num = Math.Round((double)page.Watermark.Font.Size * zoom);
			string text = $" font-size: {num}pt; font-family: {page.Watermark.Font.Name};";
			if (page.Watermark.Font.Bold)
			{
				text += "font-weight:bold;";
			}
			if (page.Watermark.Font.Italic)
			{
				text += "font-style:italic;";
			}
			if (page.Watermark.Font.Underline)
			{
				text = text + "text-decoration:underline" + (page.Watermark.Font.Strikeout ? " " : ";");
			}
			if (page.Watermark.Font.Strikeout)
			{
				text += (page.Watermark.Font.Underline ? "line-through;" : "text-decoration:line-through;");
			}
			Color color = StiBrush.ToColor(page.Watermark.TextBrush);
			string text2 = $"rgb({color.R},{color.G},{color.B})";
			double num2 = (double)(int)color.A / 255.0;
			int num3 = ((!page.Watermark.ShowBehind) ? 1 : 0);
			HtmlWriter.WriteLine(string.Format("<div class='stiWatermarkText' style=\"{0} position: absolute; pointer-events: none; filter: alpha(Opacity={2}); opacity: {1}; -moz-opacity: {1}; -khtml-opacity: {1}; color: {5}; min-width: 100%; text-align: center; z-index: {8}; margin-left: {3}; margin-top: {4}; transform: translate(-50%,-50%) rotate(-{6}deg); top:{9}px;\">{7}</div>", text, num2, num2 * 10.0, StiHtmlUnit.NewUnit(Math.Round((0.0 - page.Unit.ConvertToHInches(page.Margins.Left) + ((RectangleD)(ref val)).Width / 2.0) * zoom), StiOptions.Export.Html.PrintLayoutOptimization), StiHtmlUnit.NewUnit(Math.Round((0.0 - page.Unit.ConvertToHInches(page.Margins.Top) + ((RectangleD)(ref val)).Height / 2.0) * zoom), StiOptions.Export.Html.PrintLayoutOptimization), text2, page.Watermark.Angle, page.Watermark.Text, num3, topPos));
		}
	}

	public static Bitmap GetImage(string assemblyName, string imageName, bool makeTransparent)
	{
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		Assembly[] array = assemblies;
		foreach (Assembly assembly in array)
		{
			if (StiOptions.Engine.FullTrust)
			{
				string name = assembly.GetName().Name;
				if (name == assemblyName)
				{
					return StiImageUtils.GetImage(assembly, imageName, makeTransparent);
				}
			}
			else
			{
				string fullName = assembly.FullName;
				if (fullName.StartsWith(assemblyName + ", "))
				{
					return StiImageUtils.GetImage(assembly, imageName, makeTransparent);
				}
			}
		}
		throw new Exception($"Can't find assembly '{assemblyName}'");
	}

	public static byte[] GetFile(string assemblyName, string fileName)
	{
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		Assembly[] array = assemblies;
		foreach (Assembly assembly in array)
		{
			if (StiOptions.Engine.FullTrust)
			{
				string name = assembly.GetName().Name;
				if (name == assemblyName)
				{
					return GetFile(assembly, fileName);
				}
			}
			else
			{
				string fullName = assembly.FullName;
				if (fullName.StartsWith(assemblyName + ", "))
				{
					return GetFile(assembly, fileName);
				}
			}
		}
		throw new Exception($"Can't find assembly '{assemblyName}'");
	}

	public static byte[] GetFile(Assembly imageAssembly, string fileName)
	{
		Stream manifestResourceStream = imageAssembly.GetManifestResourceStream(fileName);
		if (manifestResourceStream != null)
		{
			byte[] array = new byte[manifestResourceStream.Length];
			manifestResourceStream.Read(array, 0, array.Length);
			return array;
		}
		throw new Exception($"Can't find file '{fileName}' in resources");
	}

	private void AssembleGuidUsedInBookmark(StiBookmark node, Hashtable hash)
	{
		if (!string.IsNullOrWhiteSpace(node.ComponentGuid))
		{
			hash[node.ComponentGuid] = node.Text;
		}
		if (node.Bookmarks == null || node.Bookmarks.Count <= 0)
		{
			return;
		}
		foreach (StiBookmark bookmark in node.Bookmarks)
		{
			AssembleGuidUsedInBookmark(bookmark, hash);
		}
	}

	private void ProcessGradientBrushes(StiComponent comp)
	{
		StiGradientBrush val = null;
		StiGlareBrush val2 = null;
		StiGlassBrush val3 = null;
		if (comp is StiText stiText)
		{
			if (stiText.Brush is StiGradientBrush)
			{
				StiBrush brush = stiText.Brush;
				val = (StiGradientBrush)(object)((brush is StiGradientBrush) ? brush : null);
			}
			if (stiText.Brush is StiGlareBrush)
			{
				StiBrush brush2 = stiText.Brush;
				val2 = (StiGlareBrush)(object)((brush2 is StiGlareBrush) ? brush2 : null);
			}
			if (stiText.Brush is StiGlassBrush)
			{
				StiBrush brush3 = stiText.Brush;
				val3 = (StiGlassBrush)(object)((brush3 is StiGlassBrush) ? brush3 : null);
			}
		}
		if (comp is StiContainer stiContainer)
		{
			if (stiContainer.Brush is StiGradientBrush)
			{
				StiBrush brush4 = stiContainer.Brush;
				val = (StiGradientBrush)(object)((brush4 is StiGradientBrush) ? brush4 : null);
			}
			if (stiContainer.Brush is StiGlareBrush)
			{
				StiBrush brush5 = stiContainer.Brush;
				val2 = (StiGlareBrush)(object)((brush5 is StiGlareBrush) ? brush5 : null);
			}
			if (stiContainer.Brush is StiGlassBrush)
			{
				StiBrush brush6 = stiContainer.Brush;
				val3 = (StiGlassBrush)(object)((brush6 is StiGlassBrush) ? brush6 : null);
			}
		}
		if (val != null)
		{
			string text = $"{Math.Round(val.Angle) + 90.0}deg";
			HtmlWriter.Write("background: linear-gradient(" + text + ", " + FormatColor(val.StartColor) + ", " + FormatColor(val.EndColor) + ");");
		}
		if (val2 != null)
		{
			string text2 = $"{Math.Round(val2.Angle) + 90.0}deg";
			HtmlWriter.Write($"background: linear-gradient({text2}, {FormatColor(val2.StartColor)}, {FormatColor(val2.EndColor)} {Math.Round(val2.Focus * 100f)}%, {FormatColor(val2.StartColor)});");
		}
		if (val3 != null)
		{
			HtmlWriter.Write("background: linear-gradient(" + FormatColor(val3.GetTopColor()) + ", " + FormatColor(val3.GetTopColor()) + " 49%, " + FormatColor(val3.GetBottomColor()) + " 50%, " + FormatColor(val3.GetBottomColor()) + ");");
		}
	}

	internal static void PrepareSvgData(StiHtmlTextWriter sWriter, double width, double height)
	{
		sWriter.WriteBeginTag("svg");
		sWriter.WriteAttribute("version", "1.1");
		sWriter.WriteAttribute("baseProfile", "full");
		sWriter.WriteAttribute("xmlns", "http://www.w3.org/2000/svg");
		sWriter.WriteAttribute("xmlns:xlink", "http://www.w3.org/1999/xlink");
		sWriter.WriteAttribute("xmlns:ev", "http://www.w3.org/2001/xml-events");
		sWriter.WriteAttribute("height", width.ToString().Replace(",", "."));
		sWriter.WriteAttribute("width", height.ToString().Replace(",", "."));
		sWriter.Write(">");
	}

	internal void PrepareSvg(StiHtmlTextWriter sWriter, double width, double height)
	{
		PrepareSvgData(sWriter, width, height);
	}

	internal string PrepareChartData(StiHtmlTextWriter writer, StiChart chart, double width, double height)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				StiGdiContextPainter contextPainter = new StiGdiContextPainter(val2);
				StiContext stiContext = new StiContext(contextPainter, isGdi: true, isWpf: false, isPrinting: false, (float)zoom);
				StringBuilder stringBuilder = new StringBuilder();
				StiHtmlTextWriter stiHtmlTextWriter = null;
				if (writer == null)
				{
					StringWriter baseStream = new StringWriter(stringBuilder);
					stiHtmlTextWriter = new StiHtmlTextWriter(baseStream);
				}
				else
				{
					stiHtmlTextWriter = writer;
				}
				float num = ((exportMode == StiHtmlExportMode.Table) ? 1f : 0.96f);
				PrepareSvg(stiHtmlTextWriter, Math.Round(height * (double)num, 2), Math.Round(width * (double)num, 2));
				StiSvgData svgData = default(StiSvgData);
				svgData.X = 0.0;
				svgData.Y = 0.0;
				svgData.Width = (float)(width * (double)num);
				svgData.Height = (float)(height * (double)num);
				svgData.Component = chart;
				MemoryStream memoryStream = new MemoryStream();
				XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
				StiChartSvgHelper.WriteChart(xmlTextWriter, svgData, (float)zoom, chartType == StiHtmlChartType.AnimatedVector);
				xmlTextWriter.Flush();
				stiHtmlTextWriter.Write(Encoding.UTF8.GetString(memoryStream.ToArray()));
				stiHtmlTextWriter.WriteEndTag("svg");
				GetGuid(chart);
				if (writer == null)
				{
					return stringBuilder.ToString();
				}
				return null;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	internal static string GetSparklineData(StiHtmlTextWriter writer, StiSparkline sparkline, double width, double height, StiHtmlExportMode exportMode = StiHtmlExportMode.Div)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				StiGdiContextPainter contextPainter = new StiGdiContextPainter(val2);
				StiContext stiContext = new StiContext(contextPainter, isGdi: true, isWpf: false, isPrinting: false, 1f);
				StringBuilder stringBuilder = new StringBuilder();
				StiHtmlTextWriter stiHtmlTextWriter = null;
				if (writer == null)
				{
					StringWriter baseStream = new StringWriter(stringBuilder);
					stiHtmlTextWriter = new StiHtmlTextWriter(baseStream);
				}
				else
				{
					stiHtmlTextWriter = writer;
				}
				float num = ((exportMode == StiHtmlExportMode.Table) ? 1f : 0.96f);
				PrepareSvgData(stiHtmlTextWriter, Math.Round(height * (double)num, 2), Math.Round(width * (double)num, 2));
				StiSvgData svgData = default(StiSvgData);
				svgData.X = 0.0;
				svgData.Y = 0.0;
				svgData.Width = (float)(width * (double)num);
				svgData.Height = (float)(height * (double)num);
				svgData.Component = sparkline;
				MemoryStream memoryStream = new MemoryStream();
				XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
				StiSparklineSvgHelper.WriteSparkline(xmlTextWriter, svgData);
				xmlTextWriter.Flush();
				stiHtmlTextWriter.Write(Encoding.UTF8.GetString(memoryStream.ToArray()));
				stiHtmlTextWriter.WriteEndTag("svg");
				if (writer == null)
				{
					return stringBuilder.ToString();
				}
				return null;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	internal string PrepareSparklineData(StiHtmlTextWriter writer, StiSparkline sparkline, double width, double height)
	{
		return GetSparklineData(writer, sparkline, width, height, exportMode);
	}

	internal string PrepareGaugeData(StiHtmlTextWriter writer, StiGauge gauge, double width, double height)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				StiGdiContextPainter contextPainter = new StiGdiContextPainter(val2);
				StiContext stiContext = new StiContext(contextPainter, isGdi: true, isWpf: false, isPrinting: false, (float)zoom);
				StringBuilder stringBuilder = new StringBuilder();
				StiHtmlTextWriter stiHtmlTextWriter = null;
				if (writer == null)
				{
					StringWriter baseStream = new StringWriter(stringBuilder);
					stiHtmlTextWriter = new StiHtmlTextWriter(baseStream);
				}
				else
				{
					stiHtmlTextWriter = writer;
				}
				float num = ((exportMode == StiHtmlExportMode.Table) ? 1f : 0.96f);
				PrepareSvg(stiHtmlTextWriter, Math.Round(height * (double)num, 2), Math.Round(width * (double)num, 2));
				StiSvgData svgData = default(StiSvgData);
				svgData.X = 0.0;
				svgData.Y = 0.0;
				svgData.Width = (float)(width * (double)num);
				svgData.Height = (float)(height * (double)num);
				svgData.Component = gauge;
				MemoryStream memoryStream = new MemoryStream();
				XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
				StiGaugeSvgHelper.WriteGauge(xmlTextWriter, svgData, (float)zoom, chartType == StiHtmlChartType.AnimatedVector);
				xmlTextWriter.Flush();
				stiHtmlTextWriter.Write(Encoding.UTF8.GetString(memoryStream.ToArray()));
				stiHtmlTextWriter.WriteEndTag("svg");
				GetGuid(gauge);
				if (writer == null)
				{
					return stringBuilder.ToString();
				}
				return null;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	internal string PrepareMapData(StiHtmlTextWriter writer, StiMap map, double width, double height)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StiHtmlTextWriter stiHtmlTextWriter;
		if (writer == null)
		{
			StringWriter baseStream = new StringWriter(stringBuilder);
			stiHtmlTextWriter = new StiHtmlTextWriter(baseStream);
		}
		else
		{
			stiHtmlTextWriter = writer;
		}
		float num = ((exportMode == StiHtmlExportMode.Table) ? 1f : 0.96f);
		PrepareSvg(stiHtmlTextWriter, Math.Round(height * (double)num, 2), Math.Round(width * (double)num, 2));
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		StiMapSvgHelper.DrawMap(xmlTextWriter, map, 0.0, 0.0, width * (double)num, height * (double)num, chartType == StiHtmlChartType.AnimatedVector);
		xmlTextWriter.Flush();
		stiHtmlTextWriter.Write(Encoding.UTF8.GetString(memoryStream.ToArray()));
		stiHtmlTextWriter.WriteEndTag("svg");
		GetGuid(map);
		if (writer == null)
		{
			return stringBuilder.ToString();
		}
		return null;
	}

	public string GetChartScript()
	{
		StiHtmlTextWriter htmlWriter = HtmlWriter;
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder);
		HtmlWriter = new StiHtmlTextWriter(stringWriter);
		RenderChartScripts(writeScriptTag: false);
		HtmlWriter.Flush();
		HtmlWriter = htmlWriter;
		stringWriter.Flush();
		stringWriter.Close();
		return stringBuilder.ToString();
	}

	public void Clear()
	{
		if (TableRender != null && TableRender.Matrix != null)
		{
			TableRender.Matrix.Clear();
			TableRender.Matrix = null;
		}
		TableRender = null;
		coordX = null;
		coordY = null;
		Styles = null;
		chartData.Clear();
	}

	private bool IsComponentHasInteraction(StiComponent comp)
	{
		if (RenderWebInteractions && comp.Interaction != null)
		{
			if (comp.Interaction.SortingEnabled && !string.IsNullOrWhiteSpace(comp.Interaction.SortingColumn))
			{
				return true;
			}
			if (comp.Interaction.DrillDownEnabled && (!string.IsNullOrEmpty(comp.Interaction.DrillDownPageGuid) || !string.IsNullOrEmpty(comp.Interaction.DrillDownReport)))
			{
				return true;
			}
			if (comp.Interaction is StiBandInteraction && ((StiBandInteraction)comp.Interaction).CollapsingEnabled)
			{
				return true;
			}
		}
		return false;
	}

	public void ExportHtml(StiReport report, string fileName)
	{
		StiFileUtils.ProcessReadOnly(fileName);
		using FileStream fileStream = new FileStream(fileName, FileMode.Create);
		ExportHtml(report, fileStream);
		fileStream.Flush();
		fileStream.Close();
	}

	public void ExportHtml(StiReport report, Stream stream)
	{
		ExportHtml(report, stream, 1.0, ImageFormat.Gif);
	}

	public void ExportHtml(StiReport report, Stream stream, double zoom, ImageFormat imageFormat)
	{
		ExportHtml(report, stream, zoom, imageFormat, StiPagesRange.All);
	}

	public void ExportHtml(StiReport report, Stream stream, StiPagesRange pageRange)
	{
		ExportHtml(report, stream, 1.0, ImageFormat.Gif, pageRange, StiHtmlExportMode.Table, StiHtmlExportQuality.High);
	}

	public void ExportHtml(StiReport report, Stream stream, double zoom, ImageFormat imageFormat, StiPagesRange pageRange)
	{
		ExportHtml(report, stream, zoom, imageFormat, pageRange, StiHtmlExportMode.Table, StiHtmlExportQuality.High);
	}

	public void ExportHtml(StiReport report, Stream stream, double zoom, ImageFormat imageFormat, StiPagesRange pageRange, StiHtmlExportMode exportMode, StiHtmlExportQuality exportQuality)
	{
		ExportHtml(report, stream, zoom, imageFormat, pageRange, exportMode, exportQuality, Encoding.UTF8);
	}

	public void ExportHtml(StiReport report, Stream stream, double zoom, ImageFormat imageFormat, StiPagesRange pageRange, StiHtmlExportMode exportMode, StiHtmlExportQuality exportQuality, Encoding encoding)
	{
		if (stream is FileStream fileStream)
		{
			fileName = fileStream.Name;
		}
		StreamWriter streamWriter = new StreamWriter(stream, encoding);
		HtmlWriter = new StiHtmlTextWriter(streamWriter);
		StiPagesCollection selectedPages = pageRange.GetSelectedPages(report.RenderedPages);
		ExportHtml(report, HtmlWriter, zoom, imageFormat, selectedPages, exportMode, exportQuality);
		streamWriter.Flush();
	}

	public void ExportHtml(StiReport report, StiHtmlTextWriter writer, double zoom, ImageFormat imageFormat, StiPagesRange pageRange)
	{
		StiPagesCollection selectedPages = pageRange.GetSelectedPages(report.RenderedPages);
		StiHtmlExportSettings settings = new StiHtmlExportSettings
		{
			PageRange = pageRange,
			Zoom = zoom,
			ImageFormat = imageFormat
		};
		ExportHtml(report, writer, settings, selectedPages);
	}

	public void ExportHtml(StiReport report, StiHtmlTextWriter writer, double zoom, ImageFormat imageFormat, int pageIndex)
	{
		StiPagesCollection stiPagesCollection = new StiPagesCollection(report, report.RenderedPages);
		stiPagesCollection.CacheMode = report.RenderedPages.CacheMode;
		stiPagesCollection.AddV2Internal(report.RenderedPages[pageIndex]);
		StiHtmlExportSettings settings = new StiHtmlExportSettings
		{
			Zoom = zoom,
			ImageFormat = imageFormat
		};
		ExportHtml(report, writer, settings, stiPagesCollection);
	}

	public void ExportHtml(StiReport report, StiHtmlTextWriter writer, double zoom, ImageFormat imageFormat, int pageIndex, StiHtmlExportMode exportMode, StiHtmlExportQuality exportQuality)
	{
		ExportHtml(report, writer, zoom, imageFormat, pageIndex, exportMode, exportQuality, StiHtmlExportBookmarksMode.All, 150);
	}

	public void ExportHtml(StiReport report, StiHtmlTextWriter writer, double zoom, ImageFormat imageFormat, int pageIndex, StiHtmlExportMode exportMode, StiHtmlExportQuality exportQuality, StiHtmlExportBookmarksMode exportBookmarksMode, int bookmarksTreeWidth)
	{
		StiPagesCollection stiPagesCollection = new StiPagesCollection(report, report.RenderedPages)
		{
			CacheMode = report.RenderedPages.CacheMode
		};
		stiPagesCollection.AddV2Internal(report.RenderedPages[pageIndex]);
		StiHtmlExportSettings settings = new StiHtmlExportSettings
		{
			Zoom = zoom,
			ImageFormat = imageFormat,
			ExportMode = exportMode,
			ExportQuality = exportQuality,
			ExportBookmarksMode = exportBookmarksMode,
			BookmarksTreeWidth = bookmarksTreeWidth
		};
		ExportHtml(report, writer, settings, stiPagesCollection);
	}

	public void ExportHtml(StiReport report, StiHtmlTextWriter writer, double zoom, ImageFormat imageFormat, StiPagesCollection pages)
	{
		StiHtmlExportSettings settings = new StiHtmlExportSettings
		{
			Zoom = zoom,
			ImageFormat = imageFormat
		};
		ExportHtml(report, writer, settings, pages);
	}

	public void ExportHtml(StiReport report, StiHtmlTextWriter writer, double zoom, ImageFormat imageFormat, StiPagesCollection pages, StiHtmlExportMode exportMode, StiHtmlExportQuality exportQuality)
	{
		StiHtmlExportSettings settings = new StiHtmlExportSettings
		{
			Zoom = zoom,
			ImageFormat = imageFormat,
			ExportMode = exportMode,
			ExportQuality = exportQuality
		};
		ExportHtml(report, writer, settings, pages);
	}

	public void ExportHtml(StiReport report, Stream stream, StiHtmlExportSettings settings)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		if (!report.IsDocument && report.GetCurrentPage() is IStiDashboard)
		{
			StiDashboardExport.Export(report, stream, settings);
			return;
		}
		FileStream fileStream = stream as FileStream;
		fileName = string.Empty;
		if (fileStream != null)
		{
			fileName = fileStream.Name;
		}
		MemoryStream memoryStream = null;
		if (settings.CompressToArchive)
		{
			zip = new StiZipWriter20();
			zip.Begin(stream, true);
			memoryStream = new MemoryStream();
			stream = memoryStream;
			try
			{
				if (!string.IsNullOrEmpty(fileName))
				{
					fileName = Path.GetFileNameWithoutExtension(fileName);
				}
			}
			catch
			{
			}
			if (string.IsNullOrEmpty(fileName))
			{
				fileName = report.ReportName;
			}
			if (string.IsNullOrEmpty(fileName))
			{
				fileName = "report";
			}
		}
		StreamWriter streamWriter = new StreamWriter(stream, settings.Encoding);
		HtmlWriter = new StiHtmlTextWriter(streamWriter);
		StiPagesCollection selectedPages = settings.PageRange.GetSelectedPages(report.RenderedPages);
		ExportHtml(report, HtmlWriter, settings, selectedPages);
		streamWriter.Flush();
		if (settings.CompressToArchive)
		{
			zip.AddFile(fileName + ".html", memoryStream);
			zip.End();
			zip = null;
		}
	}

	public void ExportHtml(StiReport report, StiHtmlTextWriter writer, StiHtmlExportSettings settings, StiPagesCollection pages)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_2659: Unknown result type (might be due to invalid IL or missing references)
		//IL_265e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2663: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dce: Unknown result type (might be due to invalid IL or missing references)
		//IL_282e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2833: Unknown result type (might be due to invalid IL or missing references)
		//IL_2838: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b11: Invalid comparison between Unknown and I4
		//IL_2c99: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ca3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1245: Unknown result type (might be due to invalid IL or missing references)
		//IL_124a: Unknown result type (might be due to invalid IL or missing references)
		//IL_124f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1259: Unknown result type (might be due to invalid IL or missing references)
		//IL_125e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e88: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_11bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1455: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b5: Expected O, but got Unknown
		//IL_0951: Unknown result type (might be due to invalid IL or missing references)
		//IL_187b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0972: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Expected O, but got Unknown
		//IL_0993: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_190a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1efa: Invalid comparison between Unknown and I4
		//IL_23a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_23f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_23f8: Invalid comparison between Unknown and I4
		//IL_20b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2130: Unknown result type (might be due to invalid IL or missing references)
		//IL_244b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2290: Unknown result type (might be due to invalid IL or missing references)
		//IL_2295: Unknown result type (might be due to invalid IL or missing references)
		//IL_229a: Unknown result type (might be due to invalid IL or missing references)
		//IL_22d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2179: Unknown result type (might be due to invalid IL or missing references)
		//IL_2187: Unknown result type (might be due to invalid IL or missing references)
		//IL_24a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b4: Unknown result type (might be due to invalid IL or missing references)
		StiLogService.Write(((object)this).GetType(), "Export report to Html format");
		exportSettings = settings;
		if (settings == null)
		{
			throw new ArgumentNullException("The 'settings' argument cannot be equal in null.");
		}
		StiPagesRange pageRange = settings.PageRange;
		zoom = settings.Zoom;
		imageFormat = settings.ImageFormat;
		exportQuality = settings.ExportQuality;
		exportMode = settings.ExportMode;
		bool flag = settings.ExportBookmarksMode != StiHtmlExportBookmarksMode.ReportOnly;
		int bookmarksTreeWidth = settings.BookmarksTreeWidth;
		bool flag2 = settings.ExportBookmarksMode == StiHtmlExportBookmarksMode.BookmarksOnly;
		useStylesTable = settings.UseStylesTable;
		imageResolution = settings.ImageResolution;
		imageQuality = settings.ImageQuality;
		RemoveEmptySpaceAtBottom = settings.RemoveEmptySpaceAtBottom;
		PageHorAlignment = settings.PageHorAlignment;
		compressToArchive = settings.CompressToArchive;
		useEmbeddedImages = settings.UseEmbeddedImages;
		openLinksTarget = settings.OpenLinksTarget;
		chartType = settings.ChartType;
		bool flag3 = true;
		flag &= report.Bookmark != null && report.Bookmark.Bookmarks.Count != 0;
		if (exportMode == StiHtmlExportMode.Span)
		{
			strSpanDiv = "span";
		}
		else if (exportMode == StiHtmlExportMode.Div)
		{
			strSpanDiv = "div";
		}
		isFileStreamMode = !string.IsNullOrEmpty(fileName);
		if (useEmbeddedImages)
		{
			isFileStreamMode = false;
		}
		currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			if (HtmlImageHost == null)
			{
				HtmlImageHost = new StiHtmlImageHost(this);
			}
			HtmlImageHost.ImageCache = new StiImageCache(StiOptions.Export.Html.AllowImageComparer, imageFormat, imageQuality);
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			HtmlWriter = writer;
			this.report = report;
			hashBookmarkGuid = new Hashtable();
			AssembleGuidUsedInBookmark(report.Bookmark, hashBookmarkGuid);
			TotalPageWidth = 0.0;
			TotalPageHeight = 0.0;
			startPage = 0.0;
			imageNumber = 1;
			Hashtable hashtable = new Hashtable();
			if (flag)
			{
				int num = 0;
				foreach (StiPage page in pages)
				{
					pages.GetPage(page);
					StiComponentsCollection components = page.GetComponents();
					components.Add(page);
					foreach (StiComponent item in components)
					{
						if (!item.Enabled)
						{
							continue;
						}
						string text = item.BookmarkValue as string;
						if (!string.IsNullOrWhiteSpace(text))
						{
							text = text.Replace("'", "");
							if (text.Length > 0 && !hashtable.ContainsKey(text))
							{
								hashtable.Add(text, num);
							}
						}
					}
					num++;
				}
			}
			bool useStimulsoftFont = StiFontIconsExHelper.NeedToUseStimulsoftFont(report);
			RectangleD val2;
			if (exportMode == StiHtmlExportMode.Span || exportMode == StiHtmlExportMode.Div)
			{
				StiHtmlTableRender stiHtmlTableRender = new StiHtmlTableRender(this, settings, new StiPagesCollection(report, pages));
				if (base.IsStopped)
				{
					return;
				}
				coordX = new SortedList();
				coordY = new SortedList();
				hyperlinksToTag = new Hashtable();
				pointerToBookmark = new Hashtable();
				pointerToTag = new Hashtable();
				foreach (StiPage page2 in pages)
				{
					TotalPageWidth = Math.Max(TotalPageWidth, page2.Unit.ConvertToHInches(page2.Width));
					TotalPageHeight += page2.Unit.ConvertToHInches(page2.Height);
				}
				foreach (StiPage page3 in pages)
				{
					pages.GetPage(page3);
					InvokeExporting(page3, pages, 0, 2);
					StiComponentsCollection components2 = page3.GetComponents();
					RectangleD val = page3.Unit.ConvertToHInches(page3.ClientRectangle);
					if (StiOptions.Export.Html.ExportComponentsFromPageMargins)
					{
						double num2 = page3.Unit.ConvertToHInches(page3.Margins.Left);
						double num3 = page3.Unit.ConvertToHInches(page3.Margins.Right);
						double num4 = page3.Unit.ConvertToHInches(page3.Margins.Top);
						double num5 = page3.Unit.ConvertToHInches(page3.Margins.Bottom);
						((RectangleD)(ref val))._002Ector(0.0 - num2, 0.0 - num4, ((RectangleD)(ref val)).Width + num2 + num3, ((RectangleD)(ref val)).Height + num4 + num5);
					}
					foreach (StiComponent item2 in components2)
					{
						RectangleD rect = page3.Unit.ConvertToHInches(item2.DisplayRectangle);
						bool flag4 = true;
						if (item2 is StiPointPrimitive)
						{
							flag4 = false;
						}
						if (((RectangleD)(ref rect)).Right < ((RectangleD)(ref val)).Left || ((RectangleD)(ref rect)).Left > ((RectangleD)(ref val)).Right || ((RectangleD)(ref rect)).Bottom < ((RectangleD)(ref val)).Top || ((RectangleD)(ref rect)).Top > ((RectangleD)(ref val)).Bottom)
						{
							flag4 = false;
						}
						if (!(item2.Enabled && flag4))
						{
							continue;
						}
						AddCoord(item2.DisplayRectangle);
						if (item2 is StiText { ExceedMargins: not StiExceedMargins.None } stiText && !(stiText.Brush is StiEmptyBrush))
						{
							rect = item2.DisplayRectangle;
							if ((stiText.ExceedMargins & StiExceedMargins.Left) > StiExceedMargins.None)
							{
								((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width + (((RectangleD)(ref rect)).Left + page3.Margins.Left);
								((RectangleD)(ref rect)).X = 0.0 - page3.Margins.Left;
							}
							if ((stiText.ExceedMargins & StiExceedMargins.Top) > StiExceedMargins.None)
							{
								((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height + (((RectangleD)(ref rect)).Top + page3.Margins.Top);
								((RectangleD)(ref rect)).Y = 0.0 - page3.Margins.Top;
							}
							if ((stiText.ExceedMargins & StiExceedMargins.Right) > StiExceedMargins.None)
							{
								((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width + ((page3.Width + page3.Margins.Right) * 1.006 - ((RectangleD)(ref rect)).Right);
							}
							if ((stiText.ExceedMargins & StiExceedMargins.Bottom) > StiExceedMargins.None)
							{
								((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height + ((page3.Height + page3.Margins.Bottom) * 1.006 - ((RectangleD)(ref rect)).Bottom);
							}
							AddCoord(rect);
						}
						if (item2.HyperlinkValue != null)
						{
							string text2 = item2.HyperlinkValue as string;
							if (!string.IsNullOrEmpty(text2) && text2.Length > 2 && text2.StartsWith("##") && text2[2] != '#')
							{
								hyperlinksToTag[text2.Substring(2)] = null;
							}
						}
						if (item2.PointerValue != null && !string.IsNullOrWhiteSpace((string)item2.PointerValue) && !string.IsNullOrEmpty(item2.Guid))
						{
							string key = $"{item2.PointerValue}#GUID#{item2.Guid}";
							string text3 = item2.BookmarkValue as string;
							if (!string.IsNullOrWhiteSpace(text3))
							{
								pointerToBookmark[key] = text3;
							}
							string value = item2.TagValue as string;
							if (text3 == null && !string.IsNullOrEmpty(value))
							{
								pointerToTag[key] = value;
							}
						}
					}
				}
				foreach (DictionaryEntry item3 in pointerToTag)
				{
					if (hyperlinksToTag.Contains(item3.Value))
					{
						pointerToBookmark[item3.Key] = item3.Value;
					}
				}
				pointerToTag.Clear();
				FormatCoords(report);
				Hashtable hashtable2 = new Hashtable();
				Hashtable hashtable3 = new Hashtable();
				foreach (StiPage page4 in pages)
				{
					pages.GetPage(page4);
					StiComponentsCollection components3 = page4.GetComponents();
					foreach (StiComponent item4 in components3)
					{
						StiComponent stiComponent4 = item4;
						if (!stiComponent4.Enabled)
						{
							continue;
						}
						if (flag3 && stiComponent4 is StiText { Brush: not null } stiText2 && !(stiText2.Brush is StiSolidBrush) && !(stiText2.Brush is StiEmptyBrush))
						{
							stiComponent4 = (StiComponent)((StiService)item4).Clone();
							Color color = Color.Transparent;
							if (stiText2.Brush is StiGradientBrush)
							{
								color = ((StiGradientBrush)stiText2.Brush).StartColor;
							}
							if (stiText2.Brush is StiGlareBrush)
							{
								color = ((StiGlareBrush)stiText2.Brush).StartColor;
							}
							if (stiText2.Brush is StiGlassBrush)
							{
								color = ((StiGlassBrush)stiText2.Brush).Color;
							}
							(stiComponent4 as StiText).Brush = (StiBrush)new StiSolidBrush(color);
						}
						StiCellStyle styleFromComponent = stiHtmlTableRender.Matrix.GetStyleFromComponent(stiComponent4, -1, -1);
						styleFromComponent.AbsolutePosition = true;
						if (!pages.CacheMode)
						{
							hashtable2[item4] = styleFromComponent;
						}
						string text4 = stiComponent4.TagValue as string;
						if (string.IsNullOrEmpty(text4))
						{
							continue;
						}
						string[] array = StiMatrix.SplitTag(text4);
						for (int i = 0; i < array.Length; i++)
						{
							if (array[i].ToLower().StartsWith("css", StringComparison.InvariantCulture))
							{
								string[] stringsFromTag = StiMatrix.GetStringsFromTag(array[i], 3);
								if (stringsFromTag.Length > 1)
								{
									string text5 = stringsFromTag[0].Trim();
									hashtable3[text5] = stringsFromTag[1].Trim() + ";position:absolute;";
									hashtable2[item4] = text5;
									break;
								}
							}
						}
					}
				}
				stiHtmlTableRender.Matrix.CheckStylesNames();
				if (RenderAsDocument)
				{
					RenderStartDoc(stiHtmlTableRender, asTable: false, flag, flag2, useStimulsoftFont, hashtable3, pages, settings.Encoding);
				}
				if (!RenderAsDocument && flag)
				{
					RenderBookmarkScript();
				}
				double num6 = 0.0;
				for (int j = 0; j < pages.Count; j++)
				{
					StiPage stiPage5 = pages[j];
					RenderWatermarkImage(stiPage5, num6);
					RenderWatermarkText(stiPage5, num6);
					double num7 = num6;
					val2 = stiPage5.Unit.ConvertToHInches(stiPage5.ClientRectangle);
					num6 = num7 + ((RectangleD)(ref val2)).Height * (StiOptions.Export.Html.PrintLayoutOptimization ? 0.956 : 1.0);
				}
				if (flag)
				{
					HtmlWriter.WriteBeginTag(strSpanDiv + " class=\"dtreeframe\" style=\"");
					HtmlWriter.WriteStyleAttribute("position", "absolute");
					if (!flag2)
					{
						HtmlWriter.WriteStyleAttribute("height", FormatCoord(TotalPageHeight * zoom * 0.716));
					}
					RenderBookmarkTree(report.Bookmark, bookmarksTreeWidth, hashtable);
					HtmlWriter.WriteEndTag(strSpanDiv);
					HtmlWriter.WriteLine();
				}
				if (!flag2 && !exportSettings.AddPageBreaks)
				{
					RenderPage(pages, flag, bookmarksTreeWidth);
				}
				if (!RenderAsDocument)
				{
					stiHtmlTableRender.RenderStyles(flag, flag2, hashtable3);
				}
				base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
				if (!flag2)
				{
					for (int k = 0; k < pages.Count; k++)
					{
						StiPage stiPage6 = pages[k];
						pages.GetPage(stiPage6);
						InvokeExporting(stiPage6, pages, 1, 2);
						if (base.IsStopped)
						{
							return;
						}
						if (exportSettings.AddPageBreaks)
						{
							RenderPage(pages, flag, bookmarksTreeWidth, stiPage6, k > 0);
						}
						StiComponentsCollection components4 = stiPage6.GetComponents();
						RectangleD val3 = stiPage6.Unit.ConvertToHInches(stiPage6.ClientRectangle);
						if (StiOptions.Export.Html.ExportComponentsFromPageMargins)
						{
							double num8 = stiPage6.Unit.ConvertToHInches(stiPage6.Margins.Left);
							double num9 = stiPage6.Unit.ConvertToHInches(stiPage6.Margins.Right);
							double num10 = stiPage6.Unit.ConvertToHInches(stiPage6.Margins.Top);
							double num11 = stiPage6.Unit.ConvertToHInches(stiPage6.Margins.Bottom);
							((RectangleD)(ref val3))._002Ector(0.0 - num8, 0.0 - num10, ((RectangleD)(ref val3)).Width + num8 + num9, ((RectangleD)(ref val3)).Height + num10 + num11);
						}
						foreach (StiComponent item5 in components4)
						{
							StiText stiText3 = item5 as StiText;
							if (!item5.Enabled || stiText3 == null || stiText3.ExceedMargins == StiExceedMargins.None || stiText3.Brush == null || stiText3.Brush is StiEmptyBrush)
							{
								continue;
							}
							if (stiText3.Brush is StiSolidBrush && ((StiSolidBrush)/*isinst with value type is only supported in some contexts*/).Color.Equals((object?)Color.Transparent))
							{
								return;
							}
							RectangleD clientRectangle = stiPage6.Unit.ConvertToHInches(item5.DisplayRectangle);
							bool flag5 = true;
							if (((RectangleD)(ref clientRectangle)).Right < ((RectangleD)(ref val3)).Left || ((RectangleD)(ref clientRectangle)).Left > ((RectangleD)(ref val3)).Right || ((RectangleD)(ref clientRectangle)).Bottom < ((RectangleD)(ref val3)).Top || ((RectangleD)(ref clientRectangle)).Top > ((RectangleD)(ref val3)).Bottom)
							{
								flag5 = false;
							}
							if (((RectangleD)(ref clientRectangle)).Width == 0.0 || ((RectangleD)(ref clientRectangle)).Height == 0.0)
							{
								flag5 = false;
							}
							if (!flag5)
							{
								continue;
							}
							clientRectangle = item5.DisplayRectangle;
							if ((stiText3.ExceedMargins & StiExceedMargins.Left) > StiExceedMargins.None)
							{
								((RectangleD)(ref clientRectangle)).Width = ((RectangleD)(ref clientRectangle)).Width + (((RectangleD)(ref clientRectangle)).Left + stiPage6.Margins.Left);
								((RectangleD)(ref clientRectangle)).X = 0.0 - stiPage6.Margins.Left;
							}
							if ((stiText3.ExceedMargins & StiExceedMargins.Top) > StiExceedMargins.None)
							{
								((RectangleD)(ref clientRectangle)).Height = ((RectangleD)(ref clientRectangle)).Height + (((RectangleD)(ref clientRectangle)).Top + stiPage6.Margins.Top);
								((RectangleD)(ref clientRectangle)).Y = 0.0 - stiPage6.Margins.Top;
							}
							if ((stiText3.ExceedMargins & StiExceedMargins.Right) > StiExceedMargins.None)
							{
								((RectangleD)(ref clientRectangle)).Width = ((RectangleD)(ref clientRectangle)).Width + ((stiPage6.Width + stiPage6.Margins.Right) * 1.006 - ((RectangleD)(ref clientRectangle)).Right);
							}
							if ((stiText3.ExceedMargins & StiExceedMargins.Bottom) > StiExceedMargins.None)
							{
								((RectangleD)(ref clientRectangle)).Height = ((RectangleD)(ref clientRectangle)).Height + ((stiPage6.Height + stiPage6.Margins.Bottom) * 1.006 - ((RectangleD)(ref clientRectangle)).Bottom);
							}
							writer.WriteBeginTag(strSpanDiv);
							writer.Write(" style=\"");
							double value2 = (double)coordX[((RectangleD)(ref clientRectangle)).Left];
							double value3 = (double)coordY[((RectangleD)(ref clientRectangle)).Top] + startPage * zoom * 0.716;
							double num12 = (double)coordX[((RectangleD)(ref clientRectangle)).Right] - (double)coordX[((RectangleD)(ref clientRectangle)).Left];
							double num13 = (double)coordY[((RectangleD)(ref clientRectangle)).Bottom] - (double)coordY[((RectangleD)(ref clientRectangle)).Top];
							if (num12 < 0.0)
							{
								num12 = 0.0;
							}
							if (num13 < 0.0)
							{
								num13 = 0.0;
							}
							HtmlWriter.WriteStyleAttribute("left", FormatCoord(value2));
							HtmlWriter.WriteStyleAttribute("top", FormatCoord(value3));
							HtmlWriter.WriteStyleAttribute("width", FormatCoord(num12));
							HtmlWriter.WriteStyleAttribute("height", FormatCoord(num13));
							HtmlWriter.WriteStyleAttribute("border", "0");
							HtmlWriter.WriteStyleAttribute("position", "absolute");
							if (stiText3.Brush is StiSolidBrush || stiText3.Brush is StiGradientBrush || stiText3.Brush is StiGlareBrush || stiText3.Brush is StiGlassBrush)
							{
								if (stiText3.Brush is StiSolidBrush)
								{
									StiBrush brush = stiText3.Brush;
									RenderBackColor(null, ((StiSolidBrush)((brush is StiSolidBrush) ? brush : null)).Color);
								}
								else
								{
									ProcessGradientBrushes(stiText3);
								}
								writer.Write("\">");
							}
							else
							{
								writer.Write("\">");
								StiText stiText4 = new StiText();
								stiText4.Page = stiPage6;
								stiText4.ClientRectangle = clientRectangle;
								stiText4.Brush = stiText3.Brush;
								RenderImage(stiText4);
							}
							writer.WriteEndTag(strSpanDiv);
							writer.WriteLine("");
						}
						List<StiComponent> list = new List<StiComponent>();
						int num14 = 0;
						foreach (StiComponent item6 in components4)
						{
							StiComponent stiComponent7 = item6;
							RectangleD val4 = stiPage6.Unit.ConvertToHInches(stiComponent7.DisplayRectangle);
							RectangleD val5 = ((RectangleD)(ref val4)).Multiply(zoom);
							bool flag6 = true;
							if (stiComponent7 is StiPointPrimitive)
							{
								flag6 = false;
							}
							if (((RectangleD)(ref val4)).Right < ((RectangleD)(ref val3)).Left || ((RectangleD)(ref val4)).Left > ((RectangleD)(ref val3)).Right || ((RectangleD)(ref val4)).Bottom < ((RectangleD)(ref val3)).Top || ((RectangleD)(ref val4)).Top > ((RectangleD)(ref val3)).Bottom)
							{
								flag6 = false;
							}
							if (((RectangleD)(ref val4)).Width == 0.0 || ((RectangleD)(ref val4)).Height == 0.0)
							{
								flag6 = false;
							}
							if (!(stiComponent7.Enabled && flag6))
							{
								continue;
							}
							string value4 = null;
							StiCellStyle stiCellStyle = hashtable2[item6] as StiCellStyle;
							if (stiCellStyle == null && pages.CacheMode)
							{
								stiCellStyle = stiHtmlTableRender.Matrix.GetStyleFromComponent(item6, -1, -1);
								stiCellStyle.AbsolutePosition = true;
							}
							int num15 = stiHtmlTableRender.Matrix.Styles.IndexOf(stiCellStyle);
							if (num15 != -1 && useStylesTable)
							{
								value4 = "s" + stiCellStyle.StyleName;
							}
							string text6 = hashtable2[item6] as string;
							if (!string.IsNullOrEmpty(text6) && useStylesTable)
							{
								value4 = text6;
							}
							StiText stiText5 = stiComponent7 as StiText;
							if (flag3 && stiText5 != null && stiText5.Brush != null && !(stiText5.Brush is StiSolidBrush) && !(stiText5.Brush is StiEmptyBrush) && !(stiText5.Brush is StiGradientBrush) && !(stiText5.Brush is StiGlareBrush) && !(stiText5.Brush is StiGlassBrush))
							{
								writer.WriteBeginTag(strSpanDiv);
								if (!string.IsNullOrEmpty(value4))
								{
									writer.WriteAttribute("class", value4);
								}
								writer.Write(" style=\"");
								writer.Write("");
								RenderPosition(stiComponent7);
								writer.Write("position:absolute;\">");
								StiText stiText6 = new StiText();
								stiText6.Page = stiPage6;
								stiText6.ClientRectangle = stiComponent7.ClientRectangle;
								stiText6.Brush = (stiComponent7 as StiText).Brush;
								RenderImage(stiText6);
								writer.WriteEndTag(strSpanDiv);
								writer.WriteLine("");
								stiComponent7 = (StiComponent)((StiService)item6).Clone();
								(stiComponent7 as StiText).Brush = (StiBrush)new StiSolidBrush(Color.Transparent);
							}
							bool flag7 = stiComponent7.IsExportAsImage(StiExportFormat.Html);
							bool flag8 = false;
							writer.WriteBeginTag(strSpanDiv);
							if (!string.IsNullOrEmpty(value4))
							{
								writer.WriteAttribute("class", value4);
							}
							if (exportMode == StiHtmlExportMode.Div && stiComponent7.ToolTipValue != null)
							{
								writer.WriteAttribute("title", stiComponent7.ToolTipValue.ToString());
							}
							if (RenderWebInteractions)
							{
								StiComponent stiComponent8 = stiComponent7;
								StiInteraction stiInteraction = null;
								if (item6.Report != null && IsComponentHasInteraction(item6))
								{
									stiInteraction = item6.Interaction;
									if (item6 is StiContainer)
									{
										list.Add(item6);
									}
								}
								if (stiInteraction == null)
								{
									double num16 = item6.Left + item6.Width / 2.0;
									double num17 = item6.Top + item6.Height / 2.0;
									foreach (StiComponent item7 in list)
									{
										if (num16 > item7.Left && num16 < item7.Right && num17 > item7.Top && num17 < item7.Bottom)
										{
											stiInteraction = item7.Interaction;
											stiComponent8 = item7;
										}
									}
								}
								if (stiInteraction != null)
								{
									writer.Write($" interaction=\"{stiComponent8.Name}\"");
									if (stiInteraction.SortingEnabled)
									{
										string sortDataBandName = stiInteraction.GetSortDataBandName();
										if (stiComponent8.Report.GetComponentByName(sortDataBandName) is StiDataBand stiDataBand)
										{
											writer.Write(string.Format(" databandsort=\"{0};{1}\"", sortDataBandName, string.Join(";", stiDataBand.Sort)));
											for (int l = 0; l < stiDataBand.Sort.Length; l += 2)
											{
												if (stiDataBand.Sort[l + 1] == stiInteraction.GetSortColumnsString())
												{
													writer.Write($" sort=\"{stiDataBand.Sort[l].ToLower()}\"");
												}
											}
										}
									}
									if (stiInteraction.DrillDownEnabled && (!string.IsNullOrEmpty(stiInteraction.DrillDownPageGuid) || !string.IsNullOrEmpty(stiInteraction.DrillDownReport)))
									{
										if (!string.IsNullOrEmpty(stiInteraction.DrillDownPageGuid))
										{
											writer.Write($" pageguid=\"{stiInteraction.DrillDownPageGuid}\"");
										}
										if (!string.IsNullOrEmpty(stiInteraction.DrillDownReport))
										{
											writer.Write($" reportfile=\"{stiInteraction.DrillDownReport}\"");
										}
										writer.Write($" pageindex=\"{stiComponent8.Page.Report.RenderedPages.IndexOf(stiComponent8.Page).ToString()}\"");
										writer.Write($" compindex=\"{stiComponent8.Page.Components.IndexOf(stiComponent8).ToString()}\"");
									}
									if (stiInteraction is StiBandInteraction { CollapsingEnabled: not false } && stiComponent8 is StiContainer)
									{
										StiContainer stiContainer = stiComponent8 as StiContainer;
										writer.Write($" collapsed=\"{StiDataBandV2Builder.IsCollapsed(stiContainer, isRendering: false).ToString().ToLower()}\"");
										writer.Write($" compindex=\"{stiContainer.CollapsingIndex.ToString()}\"");
									}
								}
								if (item6.Page != null && item6 is IStiEditable && ((IStiEditable)item6).Editable)
								{
									StringBuilder stringBuilder = new StringBuilder();
									int num18 = item6.Page.Components.IndexOf(item6);
									stringBuilder.AppendFormat("{0};", num18);
									if (item6 is StiCheckBox stiCheckBox)
									{
										Color color2 = Color.Transparent;
										if (stiCheckBox.TextBrush is StiSolidBrush)
										{
											color2 = ((StiSolidBrush)stiCheckBox.TextBrush).Color;
										}
										else if (stiCheckBox.TextBrush is StiGradientBrush)
										{
											color2 = ((StiGradientBrush)stiCheckBox.TextBrush).StartColor;
										}
										else if (stiCheckBox.TextBrush is StiGlareBrush)
										{
											color2 = ((StiGlareBrush)stiCheckBox.TextBrush).StartColor;
										}
										else if (stiCheckBox.TextBrush is StiGlassBrush)
										{
											color2 = ((StiGlassBrush)stiCheckBox.TextBrush).Color;
										}
										else if (stiCheckBox.TextBrush is StiHatchBrush)
										{
											color2 = ((StiHatchBrush)stiCheckBox.TextBrush).ForeColor;
										}
										stringBuilder.AppendFormat("CheckBox;{0};{1};{2};#{3:X2}{4:X2}{5:X2};{6};#{7:X2}{8:X2}{9:X2}", stiCheckBox.CheckedValue, stiCheckBox.CheckStyleForFalse.ToString(), stiCheckBox.CheckStyleForTrue.ToString(), stiCheckBox.ContourColor.R, stiCheckBox.ContourColor.G, stiCheckBox.ContourColor.B, stiCheckBox.Size, color2.R, color2.G, color2.B);
									}
									if (item6 is StiText stiText7)
									{
										stringBuilder.AppendFormat("Text");
										if (stiText7.CheckAllowHtmlTags())
										{
											stringBuilder.AppendFormat(";html");
										}
									}
									if (item6 is StiRichText)
									{
										stringBuilder.AppendFormat("RichText");
									}
									writer.Write($" editable=\"{stringBuilder.ToString()}\"");
								}
							}
							writer.Write(" style=\"");
							if (!flag7)
							{
								ProcessGradientBrushes(stiComponent7);
							}
							string text7 = RenderPosition(stiComponent7, returnSize: true);
							if (stiComponent7 is IStiBorder { Border: not null } stiBorder && stiBorder.Border.DropShadow)
							{
								Color color3 = StiBrush.ToColor(stiBorder.Border.ShadowBrush);
								if (!color3.Equals((object?)Color.Transparent))
								{
									string st = string.Format("box-shadow: {0}px {0}px 1px 0 rgba({1},{2},{3},{4});", (int)stiBorder.Border.ShadowSize, color3.R, color3.G, color3.B, Math.Round((float)(int)color3.A / 255f, 3));
									writer.Write(st);
								}
							}
							if (!useStylesTable)
							{
								if (num15 != -1)
								{
									stiHtmlTableRender.RenderStyle(stiCellStyle);
								}
								if (!string.IsNullOrEmpty(text6))
								{
									writer.WriteLine(hashtable3[text6]?.ToString() + ";overflow:hidden;");
								}
							}
							StiChart stiChart = stiComponent7 as StiChart;
							StiGauge stiGauge = stiComponent7 as StiGauge;
							StiMap stiMap = stiComponent7 as StiMap;
							StiSparkline stiSparkline = stiComponent7 as StiSparkline;
							if (stiChart != null && chartType != StiHtmlChartType.Image)
							{
								writer.Write("\" ");
								writer.WriteAttribute("id", GetGuid(stiChart));
								writer.Write(">");
								flag8 = RenderHyperlink(stiComponent7);
								PrepareChartData(writer, stiChart, ((RectangleD)(ref val5)).Width, ((RectangleD)(ref val5)).Height);
								if (flag8)
								{
									writer.WriteEndTag("a");
								}
							}
							else if (stiGauge != null)
							{
								writer.Write("\" ");
								writer.WriteAttribute("id", GetGuid(stiGauge));
								writer.Write(">");
								flag8 = RenderHyperlink(stiComponent7);
								PrepareGaugeData(writer, stiGauge, ((RectangleD)(ref val5)).Width, ((RectangleD)(ref val5)).Height);
								if (flag8)
								{
									writer.WriteEndTag("a");
								}
							}
							else if (stiMap != null && stiMap.MapMode == StiMapMode.Choropleth)
							{
								writer.Write("background-color:transparent;");
								writer.Write("\" ");
								writer.WriteAttribute("id", GetGuid(stiMap));
								writer.WriteAttribute("isRegionMap", "true");
								writer.Write(">");
								flag8 = RenderHyperlink(stiComponent7);
								PrepareMapData(writer, stiMap, ((RectangleD)(ref val5)).Width, ((RectangleD)(ref val5)).Height);
								if (flag8)
								{
									writer.WriteEndTag("a");
								}
							}
							else if (stiSparkline != null && chartType != StiHtmlChartType.Image)
							{
								writer.Write("\" ");
								writer.WriteAttribute("id", GetGuid(stiSparkline));
								writer.Write(">");
								flag8 = RenderHyperlink(stiComponent7);
								PrepareSparklineData(writer, stiSparkline, ((RectangleD)(ref val5)).Width, ((RectangleD)(ref val5)).Height);
								if (flag8)
								{
									writer.WriteEndTag("a");
								}
							}
							else if (flag7)
							{
								writer.Write("\">");
								flag8 = RenderHyperlink(stiComponent7);
								RenderImage(stiComponent7);
								if (flag8)
								{
									writer.WriteEndTag("a");
								}
							}
							else if (stiText5 != null && ((stiText5.TextOptions != null && stiText5.Angle != 0f) || stiText5.Indicator != null))
							{
								string text8 = StiSvgHelper.SaveComponentToString(stiComponent7);
								double num19 = (StiOptions.Export.Html.PrintLayoutOptimization ? 0.96 : 1.0);
								double num20 = (StiOptions.Export.Html.PrintLayoutOptimization ? 0.956 : 1.0);
								string st2 = $"<svg width=\"{Math.Ceiling(((RectangleD)(ref val5)).Width * num19)}\" height=\"{Math.Ceiling(((RectangleD)(ref val5)).Height * num20)}\"><g transform=\"scale({zoom})\">{text8}</g></svg>";
								writer.Write("\">");
								writer.Write(st2);
							}
							else
							{
								IStiTextOptions stiTextOptions = stiComponent7 as IStiTextOptions;
								if (stiTextOptions != null)
								{
									RenderTextDirection(null, stiTextOptions.TextOptions);
								}
								bool flag9 = stiComponent7 is IStiText && !(stiComponent7 is StiRichText);
								string text9 = null;
								bool flag10 = true;
								bool flag11 = stiCellStyle != null && ((stiCellStyle.TextOptions != null && stiCellStyle.TextOptions.WordWrap) || (int)stiCellStyle.HorAlignment == 3);
								if (flag9)
								{
									text9 = ((IStiText)stiComponent7).Text;
									if (StiOptions.Export.Html.PreserveWhiteSpaces && !string.IsNullOrWhiteSpace(text9) && text9.Contains("  ") && flag11)
									{
										writer.Write("white-space:pre-wrap;");
										flag10 = false;
									}
									if (flag10 && !flag11)
									{
										writer.Write("white-space:nowrap");
									}
								}
								writer.Write("\">");
								if (flag11 && StiOptions.Export.Html.UseWordWrapBreakWordMode)
								{
									writer.WriteBeginTag(strSpanDiv);
									writer.Write(string.Format(" style=\"word-wrap:break-word;width:{0};\">", text7.Substring(0, text7.IndexOf(";"))));
								}
								flag8 = RenderHyperlink(stiComponent7);
								bool flag12 = true;
								if (stiTextOptions != null && !stiTextOptions.TextOptions.WordWrap)
								{
									if (flag9)
									{
										if (text9 != null)
										{
											if (stiText5 != null && stiText5.TextQuality == StiTextQuality.Wysiwyg && !string.IsNullOrEmpty(text9) && text9.EndsWith(StiTextRenderer.StiForceWidthAlignTag))
											{
												text9 = text9.Substring(0, text9.Length - StiTextRenderer.StiForceWidthAlignTag.Length);
											}
											if (stiText5 != null && stiText5.CheckAllowHtmlTags())
											{
												text9 = ConvertTextWithHtmlTagsToHtmlText(stiText5, text9, zoom);
											}
											else if (StiOptions.Export.Html.ReplaceSpecialCharacters)
											{
												text9 = text9.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("<", "&lt;")
													.Replace(">", "&gt;")
													.Replace("\u00a0", "&nbsp;");
											}
											if (StiOptions.Export.Html.ConvertDigitsToArabic && stiTextOptions.TextOptions.RightToLeft)
											{
												text9 = StiExportUtils.ConvertDigitsToArabic(text9, StiOptions.Export.Html.ArabicDigitsType);
											}
											if ((int)stiText5.VertAlignment == 0)
											{
												writer.Write(PrepareTextForHtml(text9, flag10));
											}
											else
											{
												SizeStrings sizeStrings = GetSizeStrings(stiText5);
												writer.WriteBeginTag(strSpanDiv + " style=\"");
												writer.WriteStyleAttribute("display", "table-cell");
												writer.WriteStyleAttribute("max-width", sizeStrings.Width);
												writer.WriteStyleAttribute("height", sizeStrings.Height);
												if (stiTextOptions != null && (int)stiTextOptions.TextOptions.Trimming != 0)
												{
													writer.WriteStyleAttribute("text-overflow", "ellipsis");
													writer.WriteStyleAttribute("overflow", "hidden");
													writer.WriteStyleAttribute("white-space", "nowrap");
												}
												RenderTextHorAlignment(null, stiTextOptions?.TextOptions, stiText5.HorAlignment);
												RenderVertAlignment(null, stiText5.VertAlignment);
												writer.Write("\">");
												writer.Write(PrepareTextForHtml(text9, flag10));
												writer.WriteEndTag(strSpanDiv);
											}
										}
										flag12 = false;
									}
								}
								else if (flag9)
								{
									if (text9 != null)
									{
										if (stiText5 != null && stiText5.TextQuality == StiTextQuality.Wysiwyg && !string.IsNullOrEmpty(text9) && text9.EndsWith(StiTextRenderer.StiForceWidthAlignTag))
										{
											text9 = text9.Substring(0, text9.Length - StiTextRenderer.StiForceWidthAlignTag.Length);
										}
										if (stiText5 != null && stiText5.CheckAllowHtmlTags())
										{
											text9 = ConvertTextWithHtmlTagsToHtmlText(stiText5, text9, zoom);
										}
										else
										{
											if (stiText5 != null && StiOptions.Export.Html.ForceWysiwygWordwrap && !stiText5.CheckAllowHtmlTags() && stiText5.TextQuality == StiTextQuality.Wysiwyg && stiTextOptions != null && stiTextOptions.TextOptions.WordWrap)
											{
												List<string> textLines = StiTextRenderer.GetTextLines(report.ReportMeasureGraphics, ref text9, stiText5.Font, stiPage6.Unit.ConvertToHInches(stiComponent7.ComponentToPage(stiComponent7.ClientRectangle)), 1.0, true, stiTextOptions.TextOptions.RightToLeft, 1.0, (double)stiTextOptions.TextOptions.Angle, stiTextOptions.TextOptions.Trimming, stiText5.CheckAllowHtmlTags(), stiTextOptions.TextOptions, (double?)1.0);
												string value5 = "\n";
												StringBuilder stringBuilder2 = new StringBuilder();
												for (int m = 0; m < textLines.Count; m++)
												{
													string value6 = textLines[m];
													stringBuilder2.Append(value6);
													if (m < textLines.Count - 1)
													{
														stringBuilder2.Append(value5);
													}
												}
												text9 = stringBuilder2.ToString();
											}
											if (StiOptions.Export.Html.ReplaceSpecialCharacters)
											{
												text9 = text9.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("<", "&lt;")
													.Replace(">", "&gt;")
													.Replace("\u00a0", "&nbsp;");
											}
										}
										if ((int)stiText5.VertAlignment == 0)
										{
											writer.Write(PrepareTextForHtml(text9, flag10));
										}
										else
										{
											SizeStrings sizeStrings2 = GetSizeStrings(stiText5);
											writer.WriteBeginTag(strSpanDiv + " style=\"");
											writer.WriteStyleAttribute("overflow", "hidden");
											if ((int)stiText5.VertAlignment == 1)
											{
												writer.WriteStyleAttribute("align-items", "center");
											}
											else
											{
												writer.WriteStyleAttribute("align-items", "baseline");
											}
											writer.WriteStyleAttribute("max-width", sizeStrings2.Width);
											writer.WriteStyleAttribute("height", sizeStrings2.Height);
											if (stiTextOptions != null && (int)stiTextOptions.TextOptions.Trimming != 0)
											{
												writer.WriteStyleAttribute("display", "block");
												writer.WriteStyleAttribute("text-overflow", "ellipsis");
												writer.WriteStyleAttribute("white-space", "nowrap");
											}
											else
											{
												writer.WriteStyleAttribute("display", "flex");
											}
											RenderTextHorAlignment(null, stiTextOptions?.TextOptions, stiText5.HorAlignment);
											RenderVertAlignment(null, stiText5.VertAlignment, stiTextOptions?.TextOptions, stiText5.AllowHtmlTags);
											writer.Write("\">");
											writer.Write(PrepareTextForHtml(text9, flag10));
											writer.WriteEndTag(strSpanDiv);
										}
									}
									flag12 = false;
								}
								if (flag8)
								{
									if (flag12)
									{
										writer.Write("<br>");
									}
									writer.WriteEndTag("a");
								}
								if (flag11 && StiOptions.Export.Html.UseWordWrapBreakWordMode)
								{
									writer.WriteEndTag(strSpanDiv);
								}
							}
							writer.WriteEndTag(strSpanDiv);
							writer.WriteLine("");
							num14++;
						}
						if (exportSettings.AddPageBreaks)
						{
							StiLicenseKey licenseKey = StiLicenseKeyValidator.GetLicenseKey();
							bool flag13 = (!StiLicenseKeyValidator.IsValidInReportsDesignerOrOnPlatform((StiProductIdent)2, licenseKey) || !StiDesignerAppStatus.IsRunning) && !(RenderWebViewer ? StiLicenseKeyValidator.IsValidOnWebFramework(licenseKey) : StiLicenseKeyValidator.IsValidOnNetFramework(licenseKey));
							if (!typeof(StiLicense).AssemblyQualifiedName.Contains("PublicKeyToken=ebe6666cba19647a"))
							{
								flag13 = true;
							}
							if (!flag13)
							{
								try
								{
									using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(512);
									using SHA1CryptoServiceProvider halg = new SHA1CryptoServiceProvider();
									rSACryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>2GCA4rfVor/2q7m+NVisL3ZFVvnklg14zeP0F+yNpBdoSm9iuOUBLodsOH6I3Xb0j5/OznYKFpii35RcjiPvww7Uthz1he2G8PGCvFMKuA4Wzgp5Kdv0uR7zSQ8J9lA/rZ7u8wh+h72umBku1pLFL2nvJIT56u4Qe7+OJFLrqWk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
									flag13 = !rSACryptoServiceProvider.VerifyData(licenseKey.GetCheckBytes(), halg, licenseKey.GetSignatureBytes());
								}
								catch (Exception)
								{
									flag13 = true;
								}
							}
							if (stiPage6 != null && flag13)
							{
								RectangleD val6 = stiPage6.Unit.ConvertToHInches(stiPage6.ClientRectangle);
								int num21 = (int)(100.0 * zoom);
								writer.WriteLine(string.Format("<div style=\"position: absolute; pointer-events: none; filter: alpha(Opacity=30); opacity: 0.3; -moz-opacity: 0.3; -khtml-opacity: 0.3; font-size: {1}px; font-weight: bold; width: {2}; margin-top: {3}; text-align: center; font-family: Arial; color: black; z-index: 9999; -ms-transform: rotate(-45deg); -webkit-transform: rotate(-45deg); transform: rotate(-45deg);\">{4}</div>", strSpanDiv, num21, StiHtmlUnit.NewUnit(((RectangleD)(ref val6)).Width * zoom, StiOptions.Export.Html.PrintLayoutOptimization), StiHtmlUnit.NewUnit(((RectangleD)(ref val6)).Height * zoom / 2.0 - (double)num21, StiOptions.Export.Html.PrintLayoutOptimization), "Trial"));
							}
							RenderEndPage();
						}
						else
						{
							startPage += (int)stiPage6.Unit.ConvertToHInches(stiPage6.Height);
						}
						writer.WriteLine("<!-- end page -->");
					}
				}
				StiLicenseKey licenseKey2 = StiLicenseKeyValidator.GetLicenseKey();
				bool flag14 = (!StiLicenseKeyValidator.IsValidInReportsDesignerOrOnPlatform((StiProductIdent)2, licenseKey2) || !StiDesignerAppStatus.IsRunning) && !(RenderWebViewer ? StiLicenseKeyValidator.IsValidOnWebFramework(licenseKey2) : StiLicenseKeyValidator.IsValidOnNetFramework(licenseKey2));
				if (!typeof(StiLicense).AssemblyQualifiedName.Contains("PublicKeyToken=ebe6666cba19647a"))
				{
					flag14 = true;
				}
				if (!flag14)
				{
					try
					{
						using RSACryptoServiceProvider rSACryptoServiceProvider2 = new RSACryptoServiceProvider(512);
						using SHA1CryptoServiceProvider halg2 = new SHA1CryptoServiceProvider();
						rSACryptoServiceProvider2.FromXmlString("<RSAKeyValue><Modulus>2GCA4rfVor/2q7m+NVisL3ZFVvnklg14zeP0F+yNpBdoSm9iuOUBLodsOH6I3Xb0j5/OznYKFpii35RcjiPvww7Uthz1he2G8PGCvFMKuA4Wzgp5Kdv0uR7zSQ8J9lA/rZ7u8wh+h72umBku1pLFL2nvJIT56u4Qe7+OJFLrqWk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
						flag14 = !rSACryptoServiceProvider2.VerifyData(licenseKey2.GetCheckBytes(), halg2, licenseKey2.GetSignatureBytes());
					}
					catch (Exception)
					{
						flag14 = true;
					}
				}
				if (pages.Count > 0 && flag14 && !exportSettings.AddPageBreaks)
				{
					RectangleD val7 = pages[0].Unit.ConvertToHInches(pages[0].ClientRectangle);
					int num22 = (int)(100.0 * zoom);
					writer.WriteLine(string.Format("<div style=\"position: absolute; pointer-events: none; filter: alpha(Opacity=30); opacity: 0.3; -moz-opacity: 0.3; -khtml-opacity: 0.3; font-size: {1}px; font-weight: bold; width: {2}; margin-top: {3}; text-align: center; font-family: Arial; color: black; z-index: 9999; -ms-transform: rotate(-45deg); -webkit-transform: rotate(-45deg); transform: rotate(-45deg);\">{4}</div>", strSpanDiv, num22, StiHtmlUnit.NewUnit(((RectangleD)(ref val7)).Width * zoom, StiOptions.Export.Html.PrintLayoutOptimization), StiHtmlUnit.NewUnit(((RectangleD)(ref val7)).Height * zoom / 2.0 - (double)num22, StiOptions.Export.Html.PrintLayoutOptimization), "Trial"));
				}
				if (!exportSettings.AddPageBreaks)
				{
					RenderEndPage();
				}
				if (RenderAsDocument)
				{
					RenderChartScripts();
				}
				if (RenderAsDocument)
				{
					RenderEndDoc();
				}
			}
			if (exportMode == StiHtmlExportMode.Table)
			{
				base.CurrentPassNumber = 0;
				base.MaximumPassNumber = 3;
				TableRender = new StiHtmlTableRender(this, settings, pages);
				if (base.IsStopped)
				{
					return;
				}
				Hashtable hashtable4 = new Hashtable();
				bool[,] array2 = new bool[TableRender.Matrix.CoordY.Count, TableRender.Matrix.CoordX.Count];
				for (int n = 1; n < TableRender.Matrix.CoordY.Count; n++)
				{
					for (int num23 = 1; num23 < TableRender.Matrix.CoordX.Count; num23++)
					{
						if (array2[n - 1, num23 - 1])
						{
							continue;
						}
						StiCell stiCell = TableRender.Matrix.Cells[n - 1, num23 - 1];
						if (stiCell == null)
						{
							continue;
						}
						for (int num24 = 0; num24 <= stiCell.Height; num24++)
						{
							for (int num25 = 0; num25 <= stiCell.Width; num25++)
							{
								array2[n - 1 + num24, num23 - 1 + num25] = true;
							}
						}
						if (stiCell.Component == null)
						{
							continue;
						}
						string text10 = stiCell.Component.TagValue as string;
						if (string.IsNullOrEmpty(text10))
						{
							continue;
						}
						string[] array3 = StiMatrix.SplitTag(text10);
						for (int num26 = 0; num26 < array3.Length; num26++)
						{
							if (array3[num26].ToLower().StartsWith("css", StringComparison.InvariantCulture))
							{
								string[] stringsFromTag2 = StiMatrix.GetStringsFromTag(array3[num26], 3);
								if (stringsFromTag2.Length > 1)
								{
									string key2 = stringsFromTag2[0].Trim();
									hashtable4[key2] = stringsFromTag2[1].Trim();
									break;
								}
							}
						}
					}
				}
				if (RenderAsDocument)
				{
					RenderStartDoc(TableRender, asTable: true, flag, flag2, useStimulsoftFont, hashtable4, pages, settings.Encoding);
				}
				if (!RenderAsDocument && flag)
				{
					RenderBookmarkScript();
				}
				if (flag)
				{
					HtmlWriter.WriteBeginTag("table");
					if ((int)PageHorAlignment != 1)
					{
						HtmlWriter.WriteAttribute("width", "100%");
					}
					if (StiOptions.Export.Html.UseExtendedStyle)
					{
						HtmlWriter.WriteAttribute("class", "sBaseStyleFix");
					}
					HtmlWriter.Write(">");
					if (StiOptions.Export.Html.UseExtendedStyle)
					{
						writer.WriteBeginTag("tbody");
						writer.WriteAttribute("class", "sBaseStyleFix");
						writer.WriteLine(">");
					}
					HtmlWriter.WriteBeginTag("tr");
					if (StiOptions.Export.Html.UseExtendedStyle)
					{
						HtmlWriter.WriteAttribute("class", "sBaseStyleFix");
					}
					HtmlWriter.Write(">");
					HtmlWriter.WriteBeginTag("td class=\"dtreeframe\" style=\"");
					HtmlWriter.WriteStyleAttribute("vertical-align", "top");
					RenderBookmarkTree(report.Bookmark, bookmarksTreeWidth, hashtable);
					HtmlWriter.WriteEndTag("td");
					HtmlWriter.WriteLine();
					HtmlWriter.WriteBeginTag("td");
					if (StiOptions.Export.Html.UseExtendedStyle)
					{
						HtmlWriter.WriteAttribute("class", "sBaseStyleFix");
					}
					HtmlWriter.WriteLine(">");
					HtmlWriter.Indent++;
				}
				double num27 = 0.0;
				for (int num28 = 0; num28 < pages.Count; num28++)
				{
					StiPage stiPage7 = pages[num28];
					RenderWatermarkImage(stiPage7, num27);
					RenderWatermarkText(stiPage7, num27);
					double num29 = num27;
					val2 = stiPage7.Unit.ConvertToHInches(stiPage7.ClientRectangle);
					num27 = num29 + ((RectangleD)(ref val2)).Height * (StiOptions.Export.Html.PrintLayoutOptimization ? 0.956 : 1.0);
				}
				StiLicenseKey licenseKey3 = StiLicenseKeyValidator.GetLicenseKey();
				bool flag15 = (!StiLicenseKeyValidator.IsValidInReportsDesignerOrOnPlatform((StiProductIdent)2, licenseKey3) || !StiDesignerAppStatus.IsRunning) && !(RenderWebViewer ? StiLicenseKeyValidator.IsValidOnWebFramework(licenseKey3) : StiLicenseKeyValidator.IsValidOnNetFramework(licenseKey3));
				if (!typeof(StiLicense).AssemblyQualifiedName.Contains("PublicKeyToken=ebe6666cba19647a"))
				{
					flag15 = true;
				}
				if (!flag15)
				{
					try
					{
						using RSACryptoServiceProvider rSACryptoServiceProvider3 = new RSACryptoServiceProvider(512);
						using SHA1CryptoServiceProvider halg3 = new SHA1CryptoServiceProvider();
						rSACryptoServiceProvider3.FromXmlString("<RSAKeyValue><Modulus>2GCA4rfVor/2q7m+NVisL3ZFVvnklg14zeP0F+yNpBdoSm9iuOUBLodsOH6I3Xb0j5/OznYKFpii35RcjiPvww7Uthz1he2G8PGCvFMKuA4Wzgp5Kdv0uR7zSQ8J9lA/rZ7u8wh+h72umBku1pLFL2nvJIT56u4Qe7+OJFLrqWk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
						flag15 = !rSACryptoServiceProvider3.VerifyData(licenseKey3.GetCheckBytes(), halg3, licenseKey3.GetSignatureBytes());
					}
					catch (Exception)
					{
						flag15 = true;
					}
				}
				if (pages.Count > 0 && flag15)
				{
					RectangleD val8 = pages[0].Unit.ConvertToHInches(pages[0].ClientRectangle);
					int num30 = (int)(100.0 * zoom);
					writer.WriteLine(string.Format("<div style=\"position: absolute; pointer-events: none; filter: alpha(Opacity=30); opacity: 0.3; -moz-opacity: 0.3; -khtml-opacity: 0.3; font-size: {1}px; font-weight: bold; width: {2}; margin-top: {3}; text-align: center; font-family: Arial; color: black; z-index: 9999; -ms-transform: rotate(-45deg); -webkit-transform: rotate(-45deg); transform: rotate(-45deg);\">{4}</div>", strSpanDiv, num30, StiHtmlUnit.NewUnit(((RectangleD)(ref val8)).Width * zoom, StiOptions.Export.Html.PrintLayoutOptimization), StiHtmlUnit.NewUnit(((RectangleD)(ref val8)).Height * zoom / 2.0 - (double)num30, StiOptions.Export.Html.PrintLayoutOptimization), "Trial"));
				}
				if (!flag2)
				{
					StiPage stiPage8 = ((pages.Count > 0) ? pages[0] : null);
					bool watermarkShowBehind = stiPage8 != null && (((stiPage8.Watermark.Image != null || !string.IsNullOrEmpty(stiPage8.Watermark.ImageHyperlink)) && stiPage8.Watermark.ShowImageBehind) || (!string.IsNullOrEmpty(stiPage8.Watermark.Text) && stiPage8.Watermark.ShowBehind));
					TableRender.RenderTable(!RenderAsDocument && RenderStyles, null, flag, flag2, hashtable4, watermarkShowBehind, stiPage8?.Border);
				}
				if (base.IsStopped)
				{
					return;
				}
				if (flag)
				{
					HtmlWriter.Indent--;
					HtmlWriter.WriteEndTag("td");
					HtmlWriter.WriteEndTag("tr");
					HtmlWriter.WriteEndTag("tbody");
					HtmlWriter.WriteEndTag("table");
					HtmlWriter.WriteLine();
				}
				if (RenderAsDocument)
				{
					RenderChartScripts();
				}
				if (RenderAsDocument)
				{
					RenderEndDoc();
				}
			}
			writer.Flush();
		}
		finally
		{
			StiExportUtils.EnableFontSmoothing(report);
			Thread.CurrentThread.CurrentCulture = currentCulture;
			if (!HtmlImageHost.IsMhtExport)
			{
				HtmlImageHost.ImageCache.Clear();
			}
			report = null;
			if (ClearOnFinish)
			{
				Clear();
			}
		}
	}

	public StiHtmlExportService()
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		numberFormat.NumberDecimalSeparator = ".";
	}
}
