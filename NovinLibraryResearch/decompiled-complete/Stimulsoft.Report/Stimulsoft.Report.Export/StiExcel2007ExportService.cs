using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Base.Zip;
using Stimulsoft.Report.BarCodes;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Export.Tools;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Excel.png")]
public class StiExcel2007ExportService : StiExportService
{
	private struct DataFont
	{
		public string Name;

		public bool Bold;

		public bool Italic;

		public bool Underlined;

		public bool Strikeout;

		public int Height;

		public Color Color;

		public int Charset;

		public int Family;

		public DataFont(string name, bool bold, bool italic, bool underlined, bool strikeout, int height, Color color, int charset, int family)
		{
			Name = name;
			Bold = bold;
			Italic = italic;
			Underlined = underlined;
			Strikeout = strikeout;
			Height = height;
			Color = color;
			Charset = charset;
			Family = family;
		}
	}

	private struct DataFill
	{
		public string Type;

		public Color FgColor;

		public Color BgColor;

		public DataFill(string type, Color fgColor, Color bgColor)
		{
			Type = type;
			FgColor = fgColor;
			BgColor = bgColor;
		}
	}

	private struct DataBorder
	{
		public StiBorderSide BorderLeft;

		public StiBorderSide BorderRight;

		public StiBorderSide BorderTop;

		public StiBorderSide BorderBottom;

		public bool EqualDataBorder(DataBorder db)
		{
			if (object.Equals(BorderLeft, db.BorderLeft) && object.Equals(BorderRight, db.BorderRight) && object.Equals(BorderTop, db.BorderTop) && object.Equals(BorderBottom, db.BorderBottom))
			{
				return true;
			}
			return false;
		}

		public DataBorder(StiBorderSide borderLeft, StiBorderSide borderRight, StiBorderSide borderTop, StiBorderSide borderBottom)
		{
			BorderLeft = borderLeft;
			BorderRight = borderRight;
			BorderTop = borderTop;
			BorderBottom = borderBottom;
		}
	}

	private struct DataXF
	{
		public int FormatIndex;

		public int FontIndex;

		public int FillIndex;

		public int BorderIndex;

		public int XFId;

		public StiTextHorAlignment HorAlign;

		public StiVertAlignment VertAlign;

		public int TextRotationAngle;

		public bool TextWrapped;

		public bool RightToLeft;

		public bool Editable;

		public bool EqualDataXF(DataXF xf)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			if (FormatIndex != xf.FormatIndex || FontIndex != xf.FontIndex || FillIndex != xf.FillIndex || BorderIndex != xf.BorderIndex || XFId != xf.XFId || HorAlign != xf.HorAlign || VertAlign != xf.VertAlign || TextRotationAngle != xf.TextRotationAngle || TextWrapped != xf.TextWrapped || RightToLeft != xf.RightToLeft || Editable != xf.Editable)
			{
				return false;
			}
			return true;
		}

		public DataXF(int FormatIndex, int FontIndex, int FillIndex, int BorderIndex, int XFId, StiTextHorAlignment HorAlign, StiVertAlignment VertAlign, int TextRotationAngle, bool TextWrapped, bool RightToLeft, bool Editable)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			this.FormatIndex = FormatIndex;
			this.FontIndex = FontIndex;
			this.FillIndex = FillIndex;
			this.BorderIndex = BorderIndex;
			this.XFId = XFId;
			this.HorAlign = HorAlign;
			this.VertAlign = VertAlign;
			this.TextRotationAngle = TextRotationAngle;
			this.TextWrapped = TextWrapped;
			this.RightToLeft = RightToLeft;
			this.Editable = Editable;
		}
	}

	public struct ExcelImageData
	{
		public int FirstRowIndex;

		public int FirstRowOffset;

		public int FirstColumnIndex;

		public int FirstColumnOffset;

		public int LastRowIndex;

		public int LastRowOffset;

		public int LastColumnIndex;

		public int LastColumnOffset;

		public int ImageIndex;

		public string Hyperlink;

		public StiBorder Border;

		public ExcelImageData(int FirstRowIndex, int FirstRowOffset, int FirstColumnIndex, int FirstColumnOffset, int LastRowIndex, int LastRowOffset, int LastColumnIndex, int LastColumnOffset, int ImageIndex, string Hyperlink, StiBorder border)
		{
			this.FirstRowIndex = FirstRowIndex;
			this.FirstRowOffset = FirstRowOffset;
			this.FirstColumnIndex = FirstColumnIndex;
			this.FirstColumnOffset = FirstColumnOffset;
			this.LastRowIndex = LastRowIndex;
			this.LastRowOffset = LastRowOffset;
			this.LastColumnIndex = LastColumnIndex;
			this.LastColumnOffset = LastColumnOffset;
			this.ImageIndex = ImageIndex;
			this.Hyperlink = Hyperlink;
			Border = border;
		}
	}

	private struct CellRangeAddress
	{
		public int FirstRow;

		public int LastRow;

		public int FirstColumn;

		public int LastColumn;

		public CellRangeAddress(int FirstRow, int LastRow, int FirstColumn, int LastColumn)
		{
			this.FirstRow = FirstRow;
			this.LastRow = LastRow;
			this.FirstColumn = FirstColumn;
			this.LastColumn = LastColumn;
		}
	}

	private struct HlinkData
	{
		public CellRangeAddress Range;

		public string Description;

		public string Bookmark;

		public HlinkData(CellRangeAddress Range, string Description, string Bookmark)
		{
			this.Range = Range;
			this.Description = Description;
			this.Bookmark = Bookmark;
		}
	}

	internal struct BarCodeLineData
	{
		public int Row;

		public int Column;

		public PointD Position;

		public SizeD Size;

		public Color Color;
	}

	private StiReport report;

	private string fileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	private StiMatrix matrix;

	private ArrayList fontList;

	private ArrayList fillList;

	private ArrayList borderList;

	private ArrayList xfList;

	private ArrayList sstList;

	private Hashtable sstHash;

	private Hashtable sstHashIsTags;

	private ArrayList imageList;

	private ArrayList formatList;

	private int sstCount;

	private ArrayList sheetNameList;

	private int imageListOffset;

	private ArrayList printAreasList;

	private ArrayList matrixList;

	private ArrayList firstPageIndexList;

	private ArrayList hyperlinkList;

	private ArrayList minRowList;

	private ArrayList maxRowList;

	private List<BarCodeLineData> barCodeLinesList;

	private bool useOnePageHeaderAndFooter;

	private StiDataExportMode dataExportMode = StiDataExportMode.AllBands;

	private bool exportObjectFormatting = true;

	private bool exportEachPageToSheet;

	private bool exportHorizontalPageBreaks;

	private float imageResolution = 1f;

	private float imageQuality = 0.75f;

	private ImageFormat imageFormat = ImageFormat.Jpeg;

	private StiImageCache imageCache;

	private StiExcel2007RestrictEditing restrictEditing = StiExcel2007RestrictEditing.No;

	private CultureInfo reportCulture;

	private string docCompanyString;

	private string docLastModifiedString;

	private int xmlIndentation = 1;

	private readonly string wrongUrlSymbols = " \"#'*,;<>[^`{|}";

	private const double TwipsToColinfo = 0.17846160000000003;

	public override string DefaultExtension => "xlsx";

	public override StiExportFormat ExportFormat => StiExportFormat.Excel2007;

	public override string GroupCategory => "Excel";

	public override int Position => 32;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeExcel2007File");

	public override bool MultipleFiles => false;

	internal StiMatrix Matrix => matrix;

	public static int MaximumSheetHeight { get; set; } = 1048574;


	public static bool AllowNativeImageBorders { get; set; } = false;


	private static double HiToTwips => 0.7272;

	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		ExportExcel(report, stream, settings as StiExcelExportSettings);
		InvokeExporting(100, 100, 1, 1);
	}

	public override void Export(StiReport report, string fileName, bool sendEMail, StiGuiMode guiMode)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		IStiFormRunner exportFormRunner = StiGuiOptions.GetExportFormRunner("StiExcelSetupForm", guiMode, base.OwnerWindow);
		try
		{
			exportFormRunner["CurrentPage"] = report.CurrentPrintPage;
			exportFormRunner["OpenAfterExportEnabled"] = !sendEMail;
			exportFormRunner["ExportFormat"] = StiExportFormat.Excel2007;
			this.report = report;
			this.fileName = fileName;
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
		return StiLocalization.Get("FileFilters", "Excel2007Files");
	}

	private void Form_Complete(IStiFormRunner form, StiShowDialogCompleteEvetArgs e)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		if (e.DialogResult)
		{
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
				StiExcelExportSettings stiExcelExportSettings = new StiExcelExportSettings();
				stiExcelExportSettings.ExcelType = StiExcelType.Excel2007;
				stiExcelExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
				stiExcelExportSettings.UseOnePageHeaderAndFooter = (bool)form["UseOnePageHeaderAndFooter"];
				stiExcelExportSettings.DataExportMode = (StiDataExportMode)form["DataExportMode"];
				stiExcelExportSettings.ExportObjectFormatting = (bool)form["ExportObjectFormatting"];
				stiExcelExportSettings.ExportEachPageToSheet = (bool)form["ExportEachPageToSheet"];
				stiExcelExportSettings.ExportPageBreaks = (bool)form["ExportPageBreaks"];
				stiExcelExportSettings.ImageResolution = (float)form["Resolution"];
				stiExcelExportSettings.ImageQuality = (float)form["ImageQuality"];
				stiExcelExportSettings.RestrictEditing = (StiExcel2007RestrictEditing)form["RestrictEditing"];
				StiExcelExportSettings settings = stiExcelExportSettings;
				StartExport(report, stream, settings, sendEMail, (bool)form["OpenAfterExport"], fileName, guiMode);
				return;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		base.IsStopped = true;
	}

	private string GetLineStyle(StiBorderSide border)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected I4, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		StiPenStyle val = (StiPenStyle)6;
		double num = 0.0;
		if (border != (StiBorderSide)null)
		{
			val = border.Style;
			num = border.Size;
		}
		switch ((int)val)
		{
		case 0:
			if (num > 3.0)
			{
				return "thick";
			}
			if (num > 1.5)
			{
				return "medium";
			}
			return "thin";
		case 4:
			if (num > 1.5)
			{
				return "dotted";
			}
			return "hair";
		case 1:
			if (num > 1.5)
			{
				return "mediumDashed";
			}
			return "dashed";
		case 2:
			if (num > 1.5)
			{
				return "mediumDashDot";
			}
			return "dashDot";
		case 3:
			if (num > 1.5)
			{
				return "mediumDashDotDot";
			}
			return "dashDotDot";
		case 5:
			return "double";
		default:
			return null;
		}
	}

	private string GetRefString(int column, int row)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(GetRefColumnName(column));
		stringBuilder.Append((row + 1).ToString());
		return stringBuilder.ToString();
	}

	private string GetRefAbsoluteString(int column, int row)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("$");
		stringBuilder.Append(GetRefColumnName(column));
		stringBuilder.Append("$");
		stringBuilder.Append((row + 1).ToString());
		return stringBuilder.ToString();
	}

	private string GetRefColumnName(int column)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		int num2 = 0;
		if (column >= 702)
		{
			int num3 = column - 26;
			int num4 = num3 / 676;
			stringBuilder.Append((char)(65 + num4 - 1));
			num3 -= num4 * 26 * 26;
			num = num3 / 26;
			num2 = num3 % 26;
			stringBuilder.Append((char)(65 + num));
		}
		else
		{
			num = column / 26;
			num2 = column % 26;
			if (num > 0)
			{
				stringBuilder.Append((char)(65 + num - 1));
			}
		}
		stringBuilder.Append((char)(65 + num2));
		return stringBuilder.ToString();
	}

	private string FloatToString(double number)
	{
		return number.ToString().Replace(",", ".");
	}

	private string StringToUrl(string input)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (char c in input)
		{
			if (c != '\'')
			{
				if (c < ' ' || wrongUrlSymbols.IndexOf(c) != -1)
				{
					stringBuilder.Append($"%{(int)c:x2}");
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
		}
		return stringBuilder.ToString();
	}

	private int GetFontNumber(DataFont dataIn)
	{
		if (fontList.Count > 0)
		{
			for (int i = 0; i < fontList.Count; i++)
			{
				if (object.Equals((DataFont)fontList[i], dataIn))
				{
					return i;
				}
			}
		}
		fontList.Add(dataIn);
		return fontList.Count - 1;
	}

	private int GetFillNumber(DataFill dataIn)
	{
		if (fillList.Count > 0)
		{
			for (int i = 0; i < fillList.Count; i++)
			{
				if (object.Equals((DataFill)fillList[i], dataIn))
				{
					return i;
				}
			}
		}
		fillList.Add(dataIn);
		return fillList.Count - 1;
	}

	private int GetBorderNumber(DataBorder dataIn)
	{
		if (borderList.Count > 0)
		{
			for (int i = 0; i < borderList.Count; i++)
			{
				if (((DataBorder)borderList[i]).EqualDataBorder(dataIn))
				{
					return i;
				}
			}
		}
		borderList.Add(dataIn);
		return borderList.Count - 1;
	}

	private int GetXFNumber(DataXF dataIn)
	{
		if (xfList.Count > 0)
		{
			for (int i = 0; i < xfList.Count; i++)
			{
				if (((DataXF)xfList[i]).EqualDataXF(dataIn))
				{
					return i;
				}
			}
		}
		xfList.Add(dataIn);
		return xfList.Count - 1;
	}

	private int GetSSTNumber(string dataIn)
	{
		sstCount++;
		if (sstList.Count > 0 && sstHash.ContainsKey(dataIn))
		{
			return (int)sstHash[dataIn];
		}
		sstList.Add(dataIn);
		int num = sstList.Count - 1;
		sstHash.Add(dataIn, num);
		return num;
	}

	private int GetFormatNumber(string dataIn)
	{
		if (formatList.Count > 0)
		{
			for (int i = 0; i < formatList.Count; i++)
			{
				if (object.Equals((string)formatList[i], dataIn))
				{
					return i + 164;
				}
			}
		}
		formatList.Add(dataIn);
		int num = formatList.Count - 1;
		return num + 164;
	}

	internal static void ConvertRotatedAlignment(float angle, ref StiTextHorAlignment horAlign, ref StiVertAlignment vertAlign)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Invalid comparison between Unknown and I4
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Invalid comparison between Unknown and I4
		if (angle == 0f)
		{
			return;
		}
		StiTextHorAlignment val = horAlign;
		StiVertAlignment val2 = vertAlign;
		horAlign = (StiTextHorAlignment)1;
		vertAlign = (StiVertAlignment)1;
		if (angle == 90f)
		{
			if ((int)val != 0)
			{
				if ((int)val == 2)
				{
					vertAlign = (StiVertAlignment)0;
				}
			}
			else
			{
				vertAlign = (StiVertAlignment)2;
			}
			if ((int)val2 != 0)
			{
				if ((int)val2 == 2)
				{
					horAlign = (StiTextHorAlignment)2;
				}
			}
			else
			{
				horAlign = (StiTextHorAlignment)0;
			}
		}
		if (angle != 180f)
		{
			return;
		}
		if ((int)val != 0)
		{
			if ((int)val == 2)
			{
				vertAlign = (StiVertAlignment)2;
			}
		}
		else
		{
			vertAlign = (StiVertAlignment)0;
		}
		if ((int)val2 != 0)
		{
			if ((int)val2 == 2)
			{
				horAlign = (StiTextHorAlignment)0;
			}
		}
		else
		{
			horAlign = (StiTextHorAlignment)2;
		}
	}

	private string GetImageFormatExtension(ImageFormat currImageFormat)
	{
		if (currImageFormat == ImageFormat.Png)
		{
			return "png";
		}
		return "jpeg";
	}

	private double Convert(double x)
	{
		return x * HiToTwips;
	}

	private double ConvertToEMU(double x)
	{
		return Math.Round(x * 914400.0 / 100.0 * 0.93385);
	}

	private bool CompareExcellSheetNames(string name1, string name2)
	{
		string text = name1;
		if (text == null || text.Length == 0)
		{
			text = string.Empty;
		}
		string text2 = name2;
		if (text2 == null || text2.Length == 0)
		{
			text2 = string.Empty;
		}
		return text == text2;
	}

	private string RemoveControlCharacters(string input)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (char c in input)
		{
			if (!char.IsControl(c) || c == '\t' || c == '\n')
			{
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString();
	}

	private void PrepareData()
	{
		fontList = new ArrayList();
		fillList = new ArrayList();
		borderList = new ArrayList();
		xfList = new ArrayList();
		sstList = new ArrayList();
		sstHash = new Hashtable();
		sstHashIsTags = new Hashtable();
		imageList = new ArrayList();
		formatList = new ArrayList();
		sheetNameList = new ArrayList();
		sstCount = 0;
		printAreasList = new ArrayList();
		matrixList = new ArrayList();
		firstPageIndexList = new ArrayList();
		minRowList = new ArrayList();
		maxRowList = new ArrayList();
		imageCache = new StiImageCache(StiOptions.Export.Excel.AllowImageComparer, imageFormat, imageQuality);
		DataFont dataIn = new DataFont("Arial", bold: false, italic: false, underlined: false, strikeout: false, 10, Color.Black, 1, 0);
		GetFontNumber(dataIn);
		DataFill dataIn2 = new DataFill("none", Color.Transparent, Color.Black);
		GetFillNumber(dataIn2);
		dataIn2 = new DataFill("gray125", Color.Transparent, Color.Black);
		GetFillNumber(dataIn2);
		DataBorder dataIn3 = new DataBorder(null, null, null, null);
		GetBorderNumber(dataIn3);
		DataXF dataIn4 = new DataXF(0, 0, 0, 0, 0, (StiTextHorAlignment)0, (StiVertAlignment)2, 0, TextWrapped: false, RightToLeft: false, Editable: false);
		GetXFNumber(dataIn4);
	}

	private MemoryStream WriteContentTypes()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("Types");
		xmlTextWriter.WriteAttributeString("xmlns", "http://schemas.openxmlformats.org/package/2006/content-types");
		xmlTextWriter.WriteStartElement("Default");
		xmlTextWriter.WriteAttributeString("Extension", "rels");
		xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-package.relationships+xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Default");
		xmlTextWriter.WriteAttributeString("Extension", "xml");
		xmlTextWriter.WriteAttributeString("ContentType", "application/xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Default");
		xmlTextWriter.WriteAttributeString("Extension", "jpeg");
		xmlTextWriter.WriteAttributeString("ContentType", "image/jpeg");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Default");
		xmlTextWriter.WriteAttributeString("Extension", "png");
		xmlTextWriter.WriteAttributeString("ContentType", "image/png");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Default");
		xmlTextWriter.WriteAttributeString("Extension", "vml");
		xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.vmlDrawing");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Override");
		xmlTextWriter.WriteAttributeString("PartName", "/docProps/app.xml");
		xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.extended-properties+xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Override");
		xmlTextWriter.WriteAttributeString("PartName", "/docProps/core.xml");
		xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-package.core-properties+xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Override");
		xmlTextWriter.WriteAttributeString("PartName", "/xl/workbook.xml");
		xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml");
		xmlTextWriter.WriteEndElement();
		for (int i = 0; i < sheetNameList.Count; i++)
		{
			xmlTextWriter.WriteStartElement("Override");
			xmlTextWriter.WriteAttributeString("PartName", $"/xl/worksheets/sheet{i + 1}.xml");
			xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml");
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteStartElement("Override");
		xmlTextWriter.WriteAttributeString("PartName", "/xl/styles.xml");
		xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml");
		xmlTextWriter.WriteEndElement();
		for (int j = 0; j < sheetNameList.Count; j++)
		{
			xmlTextWriter.WriteStartElement("Override");
			xmlTextWriter.WriteAttributeString("PartName", $"/xl/drawings/drawing{j + 1}.xml");
			xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.drawing+xml");
			xmlTextWriter.WriteEndElement();
		}
		if (sstList.Count > 0)
		{
			xmlTextWriter.WriteStartElement("Override");
			xmlTextWriter.WriteAttributeString("PartName", "/xl/sharedStrings.xml");
			xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.sharedStrings+xml");
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteMainRels()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("Relationships");
		xmlTextWriter.WriteAttributeString("xmlns", "http://schemas.openxmlformats.org/package/2006/relationships");
		xmlTextWriter.WriteStartElement("Relationship");
		xmlTextWriter.WriteAttributeString("Id", "rId1");
		xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument");
		xmlTextWriter.WriteAttributeString("Target", "xl/workbook.xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Relationship");
		xmlTextWriter.WriteAttributeString("Id", "rId2");
		xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties");
		xmlTextWriter.WriteAttributeString("Target", "docProps/core.xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Relationship");
		xmlTextWriter.WriteAttributeString("Id", "rId3");
		xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties");
		xmlTextWriter.WriteAttributeString("Target", "docProps/app.xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteDocPropsApp()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("Properties");
		xmlTextWriter.WriteAttributeString("xmlns", "http://schemas.openxmlformats.org/officeDocument/2006/extended-properties");
		xmlTextWriter.WriteAttributeString("xmlns:vt", "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes");
		xmlTextWriter.WriteElementString("Application", "Microsoft Excel");
		xmlTextWriter.WriteElementString("DocSecurity", "0");
		xmlTextWriter.WriteElementString("ScaleCrop", "false");
		xmlTextWriter.WriteElementString("Company", docCompanyString ?? "");
		xmlTextWriter.WriteElementString("LinksUpToDate", "false");
		xmlTextWriter.WriteElementString("SharedDoc", "false");
		xmlTextWriter.WriteElementString("HyperlinksChanged", "false");
		xmlTextWriter.WriteElementString("AppVersion", "12.0000");
		xmlTextWriter.WriteStartElement("HeadingPairs");
		xmlTextWriter.WriteStartElement("vt:vector");
		xmlTextWriter.WriteAttributeString("size", "4");
		xmlTextWriter.WriteAttributeString("baseType", "variant");
		xmlTextWriter.WriteStartElement("vt:variant");
		xmlTextWriter.WriteElementString("vt:lpstr", "Worksheets");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("vt:variant");
		xmlTextWriter.WriteElementString("vt:i4", $"{sheetNameList.Count}");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("vt:variant");
		xmlTextWriter.WriteElementString("vt:lpstr", "Named Ranges");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("vt:variant");
		xmlTextWriter.WriteElementString("vt:i4", $"{sheetNameList.Count}");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("TitlesOfParts");
		xmlTextWriter.WriteStartElement("vt:vector");
		xmlTextWriter.WriteAttributeString("size", $"{sheetNameList.Count * 2}");
		xmlTextWriter.WriteAttributeString("baseType", "lpstr");
		for (int i = 0; i < sheetNameList.Count; i++)
		{
			xmlTextWriter.WriteElementString("vt:lpstr", (string)sheetNameList[i]);
		}
		for (int j = 0; j < sheetNameList.Count; j++)
		{
			xmlTextWriter.WriteElementString("vt:lpstr", $"'{(string)sheetNameList[j]}'!Print_Area");
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteDocPropsCore()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("cp:coreProperties");
		xmlTextWriter.WriteAttributeString("xmlns:cp", "http://schemas.openxmlformats.org/package/2006/metadata/core-properties");
		xmlTextWriter.WriteAttributeString("xmlns:dc", "http://purl.org/dc/elements/1.1/");
		xmlTextWriter.WriteAttributeString("xmlns:dcterms", "http://purl.org/dc/terms/");
		xmlTextWriter.WriteAttributeString("xmlns:dcmitype", "http://purl.org/dc/dcmitype/");
		xmlTextWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
		string text = string.Format("{0}", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));
		xmlTextWriter.WriteElementString("dc:title", report.ReportName ?? "");
		xmlTextWriter.WriteElementString("dc:subject", report.ReportAlias ?? "");
		xmlTextWriter.WriteElementString("dc:creator", report.ReportAuthor ?? "");
		xmlTextWriter.WriteElementString("dc:description", report.ReportDescription ?? "");
		xmlTextWriter.WriteElementString("cp:lastModifiedBy", docLastModifiedString ?? "");
		xmlTextWriter.WriteStartElement("dcterms:created");
		xmlTextWriter.WriteAttributeString("xsi:type", "dcterms:W3CDTF");
		xmlTextWriter.WriteString(text);
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("dcterms:modified");
		xmlTextWriter.WriteAttributeString("xsi:type", "dcterms:W3CDTF");
		xmlTextWriter.WriteString(text);
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteWorkbookRels()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("Relationships");
		xmlTextWriter.WriteAttributeString("xmlns", "http://schemas.openxmlformats.org/package/2006/relationships");
		for (int i = 0; i < sheetNameList.Count; i++)
		{
			xmlTextWriter.WriteStartElement("Relationship");
			xmlTextWriter.WriteAttributeString("Id", $"rId{i + 1}");
			xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet");
			xmlTextWriter.WriteAttributeString("Target", $"worksheets/sheet{i + 1}.xml");
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteStartElement("Relationship");
		xmlTextWriter.WriteAttributeString("Id", $"rId{sheetNameList.Count + 2}");
		xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles");
		xmlTextWriter.WriteAttributeString("Target", "styles.xml");
		xmlTextWriter.WriteEndElement();
		if (sstCount > 0)
		{
			xmlTextWriter.WriteStartElement("Relationship");
			xmlTextWriter.WriteAttributeString("Id", $"rId{sheetNameList.Count + 3}");
			xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings");
			xmlTextWriter.WriteAttributeString("Target", "sharedStrings.xml");
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteWorkbook()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("workbook");
		xmlTextWriter.WriteAttributeString("xmlns", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
		xmlTextWriter.WriteAttributeString("xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
		xmlTextWriter.WriteStartElement("fileVersion");
		xmlTextWriter.WriteAttributeString("appName", "xl");
		xmlTextWriter.WriteAttributeString("lastEdited", "4");
		xmlTextWriter.WriteAttributeString("lowestEdited", "4");
		xmlTextWriter.WriteAttributeString("rupBuild", "4505");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("workbookPr");
		xmlTextWriter.WriteAttributeString("defaultThemeVersion", "124226");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("bookViews");
		xmlTextWriter.WriteStartElement("workbookView");
		xmlTextWriter.WriteAttributeString("xWindow", "120");
		xmlTextWriter.WriteAttributeString("yWindow", "15");
		xmlTextWriter.WriteAttributeString("windowWidth", "18975");
		xmlTextWriter.WriteAttributeString("windowHeight", "11955");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("sheets");
		for (int i = 0; i < sheetNameList.Count; i++)
		{
			xmlTextWriter.WriteStartElement("sheet");
			xmlTextWriter.WriteAttributeString("name", (string)sheetNameList[i]);
			xmlTextWriter.WriteAttributeString("sheetId", $"{i + 1}");
			xmlTextWriter.WriteAttributeString("r:id", $"rId{i + 1}");
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("definedNames");
		for (int j = 0; j < sheetNameList.Count; j++)
		{
			Size size = (Size)printAreasList[j];
			if (size.Height > 1048575)
			{
				size.Height = 1048575;
			}
			if (size.Width > 16383)
			{
				size.Width = 16383;
			}
			string refAbsoluteString = GetRefAbsoluteString(size.Width, size.Height);
			xmlTextWriter.WriteStartElement("definedName");
			xmlTextWriter.WriteAttributeString("name", "_xlnm.Print_Area");
			xmlTextWriter.WriteAttributeString("localSheetId", $"{j}");
			xmlTextWriter.WriteString($"'{(string)sheetNameList[j]}'!$A$1:{refAbsoluteString}");
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("calcPr");
		xmlTextWriter.WriteAttributeString("calcId", "124519");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteSheetRels(int indexSheet)
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("Relationships");
		xmlTextWriter.WriteAttributeString("xmlns", "http://schemas.openxmlformats.org/package/2006/relationships");
		StiLicenseKey licenseKey = StiLicenseKeyValidator.GetLicenseKey();
		bool flag = (!StiLicenseKeyValidator.IsValidInReportsDesignerOrOnPlatform((StiProductIdent)2, licenseKey) || !StiDesignerAppStatus.IsRunning) && !StiLicenseKeyValidator.IsValidOnNetFramework(licenseKey);
		if (!typeof(StiLicense).AssemblyQualifiedName.Contains("PublicKeyToken=ebe6666cba19647a"))
		{
			flag = true;
		}
		if (!flag)
		{
			try
			{
				using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(512);
				using SHA1CryptoServiceProvider halg = new SHA1CryptoServiceProvider();
				rSACryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>2GCA4rfVor/2q7m+NVisL3ZFVvnklg14zeP0F+yNpBdoSm9iuOUBLodsOH6I3Xb0j5/OznYKFpii35RcjiPvww7Uthz1he2G8PGCvFMKuA4Wzgp5Kdv0uR7zSQ8J9lA/rZ7u8wh+h72umBku1pLFL2nvJIT56u4Qe7+OJFLrqWk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
				flag = !rSACryptoServiceProvider.VerifyData(licenseKey.GetCheckBytes(), halg, licenseKey.GetSignatureBytes());
			}
			catch (Exception)
			{
				flag = true;
			}
		}
		if (imageList.Count - imageListOffset > 0 || barCodeLinesList.Count > 0 || flag)
		{
			xmlTextWriter.WriteStartElement("Relationship");
			xmlTextWriter.WriteAttributeString("Id", $"rId{1}");
			xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/drawing");
			xmlTextWriter.WriteAttributeString("Target", $"../drawings/drawing{indexSheet + 1}.xml");
			xmlTextWriter.WriteEndElement();
		}
		if (hyperlinkList.Count > 0)
		{
			for (int i = 0; i < hyperlinkList.Count; i++)
			{
				xmlTextWriter.WriteStartElement("Relationship");
				xmlTextWriter.WriteAttributeString("Id", $"rId{2 + i}");
				xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink");
				xmlTextWriter.WriteAttributeString("Target", (string)hyperlinkList[i]);
				xmlTextWriter.WriteAttributeString("TargetMode", "External");
				xmlTextWriter.WriteEndElement();
			}
		}
		if (flag)
		{
			xmlTextWriter.WriteStartElement("Relationship");
			xmlTextWriter.WriteAttributeString("Id", "dId1");
			xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/vmlDrawing");
			xmlTextWriter.WriteAttributeString("Target", "../drawings/vmlDrawingAdditional.vml");
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteSheet(int indexSheet, StiPage page)
	{
		//IL_2844: Unknown result type (might be due to invalid IL or missing references)
		//IL_284e: Expected I4, but got Unknown
		//IL_0b5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b65: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_21ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bed: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c23: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bf9: Invalid comparison between Unknown and I4
		//IL_1bf2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c35: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c2e: Unknown result type (might be due to invalid IL or missing references)
		MemoryStream memoryStream = new StiCachedStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("worksheet");
		xmlTextWriter.WriteAttributeString("xmlns", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
		xmlTextWriter.WriteAttributeString("xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
		if (StiOptions.Export.Excel.NumberOfPagesInWideToFit > 0 || StiOptions.Export.Excel.NumberOfPagesInHeightToFit > 0)
		{
			xmlTextWriter.WriteStartElement("sheetPr");
			xmlTextWriter.WriteStartElement("pageSetUpPr");
			xmlTextWriter.WriteAttributeString("fitToPage", "1");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		matrix = (StiMatrix)matrixList[indexSheet];
		int num = (int)minRowList[indexSheet];
		int num2 = (int)maxRowList[indexSheet];
		Size size = new Size(Matrix.CoordX.Count - 2, num2 - num - 1);
		if (size.Width < 0)
		{
			size.Width = 0;
		}
		if (size.Height < 0)
		{
			size.Height = 0;
		}
		printAreasList.Add(size);
		ArrayList arrayList = new ArrayList();
		xmlTextWriter.WriteStartElement("dimension");
		xmlTextWriter.WriteAttributeString("ref", $"{GetRefString(0, 0)}:{GetRefString(size.Width, size.Height)}");
		xmlTextWriter.WriteEndElement();
		int num3 = 0;
		int num4 = 0;
		if (StiOptions.Export.Excel.AllowFreezePanes)
		{
			for (int i = num; i < num2; i++)
			{
				for (int j = 0; j < Matrix.CoordX.Count - 1; j++)
				{
					StiCell stiCell = Matrix.Cells[i, j];
					if (stiCell != null && stiCell.Component != null && (stiCell.Component.Locked || (stiCell.Component.TagValue != null && stiCell.Component.TagValue.ToString().Contains("excelfreezepanes"))))
					{
						string componentPlacement = stiCell.Component.ComponentPlacement;
						if (componentPlacement != null && componentPlacement.Length > 0 && (componentPlacement.StartsWith("rt") || componentPlacement.StartsWith("ph") || componentPlacement.StartsWith("h")))
						{
							num3 = stiCell.Left;
							num4 = stiCell.Top + stiCell.Height + 1;
							break;
						}
					}
				}
			}
		}
		xmlTextWriter.WriteStartElement("sheetViews");
		xmlTextWriter.WriteStartElement("sheetView");
		if (StiOptions.Export.Excel.ColumnsRightToLeft)
		{
			xmlTextWriter.WriteAttributeString("rightToLeft", "1");
		}
		if (!StiOptions.Export.Excel.ShowGridLines)
		{
			xmlTextWriter.WriteAttributeString("showGridLines", "0");
		}
		if (StiOptions.Export.Excel.SheetViewMode == StiExcelSheetViewMode.PageBreakPreview)
		{
			xmlTextWriter.WriteAttributeString("view", "pageBreakPreview");
			xmlTextWriter.WriteAttributeString("zoomScale", "60");
			xmlTextWriter.WriteAttributeString("zoomScaleNormal", "100");
		}
		if (StiOptions.Export.Excel.SheetViewMode == StiExcelSheetViewMode.PageLayout)
		{
			xmlTextWriter.WriteAttributeString("view", "pageLayout");
			xmlTextWriter.WriteAttributeString("zoomScaleNormal", "100");
		}
		if (indexSheet == 0)
		{
			xmlTextWriter.WriteAttributeString("tabSelected", "1");
		}
		xmlTextWriter.WriteAttributeString("workbookViewId", "0");
		if (num3 == 0 && num4 == 0)
		{
			xmlTextWriter.WriteStartElement("selection");
			xmlTextWriter.WriteAttributeString("activeCell", "A1");
			xmlTextWriter.WriteAttributeString("sqref", "A1");
			xmlTextWriter.WriteEndElement();
		}
		else if (num3 == 0)
		{
			xmlTextWriter.WriteStartElement("pane");
			xmlTextWriter.WriteAttributeString("ySplit", num4.ToString());
			xmlTextWriter.WriteAttributeString("topLeftCell", GetRefString(num3, num4));
			xmlTextWriter.WriteAttributeString("activePane", "bottomLeft");
			xmlTextWriter.WriteAttributeString("state", "frozen");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("selection");
			xmlTextWriter.WriteAttributeString("pane", "bottomLeft");
			xmlTextWriter.WriteAttributeString("activeCell", GetRefString(0, num4));
			xmlTextWriter.WriteAttributeString("sqref", GetRefString(0, num4));
			xmlTextWriter.WriteEndElement();
		}
		else
		{
			xmlTextWriter.WriteStartElement("pane");
			xmlTextWriter.WriteAttributeString("xSplit", num3.ToString());
			xmlTextWriter.WriteAttributeString("ySplit", num4.ToString());
			xmlTextWriter.WriteAttributeString("topLeftCell", GetRefString(num3, num4));
			xmlTextWriter.WriteAttributeString("activePane", "bottomRight");
			xmlTextWriter.WriteAttributeString("state", "frozen");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("selection");
			xmlTextWriter.WriteAttributeString("pane", "topRight");
			xmlTextWriter.WriteAttributeString("activeCell", GetRefString(num3, 0));
			xmlTextWriter.WriteAttributeString("sqref", GetRefString(num3, 0));
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("selection");
			xmlTextWriter.WriteAttributeString("pane", "bottomLeft");
			xmlTextWriter.WriteAttributeString("activeCell", GetRefString(0, num4));
			xmlTextWriter.WriteAttributeString("sqref", GetRefString(0, num4));
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("selection");
			xmlTextWriter.WriteAttributeString("pane", "bottomRight");
			xmlTextWriter.WriteAttributeString("activeCell", GetRefString(num3, num4));
			xmlTextWriter.WriteAttributeString("sqref", GetRefString(num3, num4));
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("sheetFormatPr");
		xmlTextWriter.WriteAttributeString("defaultRowHeight", $"{15}");
		xmlTextWriter.WriteEndElement();
		int num5 = 0;
		Color fgColor = StiBrush.ToColor(page.Brush);
		if (!fgColor.Equals((object?)Color.White) && fgColor.A != 0)
		{
			DataFill dataIn = new DataFill("solid", fgColor, Color.Transparent);
			int fillNumber = GetFillNumber(dataIn);
			DataXF dataIn2 = new DataXF(0, 0, fillNumber, 0, 0, (StiTextHorAlignment)0, (StiVertAlignment)2, 0, TextWrapped: false, RightToLeft: false, Editable: false);
			num5 = GetXFNumber(dataIn2);
		}
		if (exportObjectFormatting && Matrix.CoordX.Count > 1)
		{
			xmlTextWriter.WriteStartElement("cols");
			for (int k = 0; k < Matrix.CoordX.Count - 1; k++)
			{
				double num6 = (double)Matrix.CoordX.GetByIndex(k + 1);
				double num7 = (double)Matrix.CoordX.GetByIndex(k);
				double num8 = Convert(num6 - num7) * 0.17846160000000003;
				xmlTextWriter.WriteStartElement("col");
				xmlTextWriter.WriteAttributeString("min", $"{k + 1}");
				xmlTextWriter.WriteAttributeString("max", $"{k + 1}");
				xmlTextWriter.WriteAttributeString("width", $"{num8}");
				xmlTextWriter.WriteAttributeString("customWidth", "1");
				if (num5 > 0)
				{
					xmlTextWriter.WriteAttributeString("style", $"{num5}");
				}
				xmlTextWriter.WriteEndElement();
			}
			if (num5 > 0)
			{
				xmlTextWriter.WriteStartElement("col");
				xmlTextWriter.WriteAttributeString("min", $"{Matrix.CoordX.Count}");
				xmlTextWriter.WriteAttributeString("max", $"{16384}");
				xmlTextWriter.WriteAttributeString("width", "8.7265625");
				xmlTextWriter.WriteAttributeString("style", $"{num5}");
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
		}
		RichTextBox val = null;
		bool[,] array = new bool[num2 + 1, Matrix.CoordX.Count];
		int[,] array2 = new int[num2 + 1, Matrix.CoordX.Count];
		ArrayList arrayList2 = new ArrayList();
		ArrayList arrayList3 = new ArrayList();
		double num9 = Math.Max((float)Matrix.CoordY.Count / 200f, 1f);
		int num10 = 0;
		xmlTextWriter.WriteStartElement("sheetData");
		for (int l = num; l < num2; l++)
		{
			int num11 = (int)((double)l / num9);
			if (num11 > num10)
			{
				num10 = num11;
				InvokeExporting(l, matrix.CoordY.Count, base.CurrentPassNumber, base.MaximumPassNumber);
			}
			double x = (double)Matrix.CoordY.GetByIndex(l + 1) - (double)Matrix.CoordY.GetByIndex(l);
			xmlTextWriter.WriteStartElement("row");
			xmlTextWriter.WriteAttributeString("r", $"{l + 1 - num}");
			xmlTextWriter.WriteAttributeString("spans", $"{1}:{Matrix.CoordX.Count - 1}");
			if (exportObjectFormatting)
			{
				xmlTextWriter.WriteAttributeString("ht", $"{Convert(x)}");
				xmlTextWriter.WriteAttributeString("customHeight", "1");
			}
			for (int m = 0; m < Matrix.CoordX.Count - 1; m++)
			{
				StiCell stiCell2 = Matrix.Cells[l, m];
				if (!array[l, m] && stiCell2 != null)
				{
					array[l, m] = true;
					StiRichText stiRichText = stiCell2.Component as StiRichText;
					StiText stiText = stiCell2.Component as StiText;
					StiBarCode stiBarCode = stiCell2.Component as StiBarCode;
					string text = stiCell2.Text;
					if (stiRichText != null && stiRichText.RtfText != string.Empty)
					{
						if (val == null)
						{
							val = (RichTextBox)(object)new StiRichTextBox(drawTransparent: false);
						}
						stiRichText.GetPreparedText(val);
						text = ((Control)val).Text;
					}
					StiCheckBox stiCheckBox = stiCell2.Component as StiCheckBox;
					bool flag = false;
					bool flag2 = false;
					if (stiCheckBox != null && stiCheckBox.ExcelDataValue != null && stiCheckBox.ExcelDataValue.Length > 0)
					{
						flag = true;
						text = stiCheckBox.ExcelDataValue;
					}
					string text2 = null;
					if (stiCell2.Component != null && stiCell2.Component.HyperlinkValue != null)
					{
						text2 = stiCell2.Component.HyperlinkValue.ToString().Trim();
						if (text2.Length > 0 && !text2.StartsWith("javascript:"))
						{
							string text3 = text;
							if (text3 == null || text3.Length == 0)
							{
								text3 = text2;
							}
							CellRangeAddress range = new CellRangeAddress(l - num, l - num + stiCell2.Height, m, m + stiCell2.Width);
							HlinkData hlinkData = new HlinkData(range, text3, text2);
							arrayList3.Add(hlinkData);
						}
					}
					bool flag3 = false;
					if (false)
					{
						StiExcel2007GeomWriter geomWriter = new StiExcel2007GeomWriter(barCodeLinesList, l, m);
						StiBarCodeExportPainter context = new StiBarCodeExportPainter(geomWriter);
						if (!string.IsNullOrEmpty(stiBarCode.CodeValue) && stiBarCode.Page != null)
						{
							RectangleD val2 = report.Unit.ConvertToHInches(stiBarCode.ClientRectangle);
							RectangleF rect = ((RectangleD)(ref val2)).ToRectangleF();
							stiBarCode.BarCodeType.Draw(context, stiBarCode, rect, 1f);
						}
						flag3 = true;
					}
					bool flag4 = false;
					IStiExportImage stiExportImage = stiCell2.Component as IStiExportImage;
					if (!flag3 && !flag2 && stiExportImage != null)
					{
						IStiExportImageExtended stiExportImageExtended = stiExportImage as IStiExportImageExtended;
						float zoom = imageResolution;
						if (StiOptions.Export.Excel.UseImageResolution && stiExportImage is StiImage && (stiExportImage as StiImage).ExistImageToDraw())
						{
							Image val3 = (stiExportImage as StiImage).TryTakeGdiImageToDraw();
							try
							{
								float horizontalResolution = val3.HorizontalResolution;
								if (horizontalResolution >= 50f && horizontalResolution <= 600f)
								{
									zoom *= horizontalResolution / 100f;
								}
							}
							finally
							{
								((IDisposable)val3)?.Dispose();
							}
						}
						Image val4 = null;
						object tagValue = stiCell2.Component.TagValue;
						if (stiCell2.Component.IsExportAsImage(StiExportFormat.Excel2007))
						{
							if (tagValue == null)
							{
								stiCell2.Component.TagValue = StiViewGdiPainter.StiGetRawFormatTag;
							}
							val4 = ((stiExportImageExtended == null || !stiExportImageExtended.IsExportAsImage(StiExportFormat.Excel2007)) ? stiExportImage.GetImage(ref zoom) : stiExportImageExtended.GetImage(ref zoom, (imageFormat == null || imageFormat == ImageFormat.Png) ? StiExportFormat.ImagePng : StiExportFormat.Excel));
							if (tagValue == null)
							{
								tagValue = stiCell2.Component.TagValue;
								stiCell2.Component.TagValue = null;
							}
						}
						if (val4 != null)
						{
							Image realImageData = Matrix.GetRealImageData(stiCell2, val4);
							if (realImageData != null)
							{
								val4 = realImageData;
							}
							ImageFormat val5 = imageFormat;
							if (imageFormat == null)
							{
								val5 = ((!((object)ImageFormat.MemoryBmp).Equals((object?)val4.RawFormat) && !((object)ImageFormat.Png).Equals((object?)val4.RawFormat) && !((object)ImageFormat.Gif).Equals((object?)val4.RawFormat) && !((object)ImageFormat.Bmp).Equals((object?)val4.RawFormat)) ? ImageFormat.Jpeg : ImageFormat.Png);
								if (stiCell2.Component is StiImage && ((object)ImageFormat.MemoryBmp).Equals((object?)val4.RawFormat))
								{
									val5 = ((!(tagValue is ImageFormat)) ? ImageFormat.Jpeg : (((object)ImageFormat.Png).Equals(tagValue) ? ImageFormat.Png : ImageFormat.Jpeg));
								}
							}
							int imageIndex = imageCache.AddImageInt(val4, val5);
							ExcelImageData excelImageData = new ExcelImageData(l - num, 0, m, 0, l - num + 1 + stiCell2.Height, 0, m + 1 + stiCell2.Width, 0, imageIndex, text2, (!AllowNativeImageBorders) ? null : (stiCell2.Component as IStiBorder)?.Border);
							imageList.Add(excelImageData);
							val4.Dispose();
							flag4 = true;
							flag3 = true;
						}
					}
					bool flag5 = false;
					if (stiCell2.Component != null && stiCell2.Component.TagValue != null)
					{
						string text4 = stiCell2.Component.TagValue.ToString().ToLower();
						if (text4.IndexOf("exceltext", StringComparison.InvariantCulture) != -1)
						{
							flag5 = true;
							arrayList.Add(GetRefString(m, l));
						}
					}
					string text5 = string.Empty;
					if (stiText != null)
					{
						text5 = stiText.Format;
					}
					bool flag6 = false;
					bool flag7 = false;
					bool flag8 = false;
					bool flag9 = false;
					bool flag10 = false;
					bool flag11 = false;
					bool flag12 = false;
					string text6 = string.Empty;
					int num12 = 2;
					int num13 = 0;
					string text7 = reportCulture.NumberFormat.CurrencySymbol;
					bool flag13 = reportCulture.NumberFormat.CurrencyPositivePattern == 0 || reportCulture.NumberFormat.CurrencyPositivePattern == 2;
					bool flag14 = false;
					bool flag15 = stiText?.HideZeros ?? false;
					string text8 = null;
					string text9 = null;
					int num14 = text5?.IndexOf("|") ?? (-1);
					if (num14 != -1)
					{
						text8 = StiExportUtils.GetPositivePattern(text5[num14 + 1] - 65);
						text9 = StiExportUtils.GetNegativePattern(text5[num14 + 2] - 65);
						text5 = text5.Substring(0, num14);
					}
					if (text5 != null && text5.Length > 0)
					{
						if (text5[0] == 'C')
						{
							flag6 = true;
						}
						else if (text5[0] == 'N')
						{
							flag7 = true;
						}
						else if (text5[0] == 'P')
						{
							flag8 = true;
						}
						else if (text5[0] == 'D')
						{
							flag9 = true;
						}
						else if (text5[0] == 'T')
						{
							flag10 = true;
						}
						else if (text5[0] == 'U')
						{
							flag11 = true;
						}
						if (text5.Length == 1)
						{
							flag12 = true;
						}
						else if (flag6 || flag7 || flag8)
						{
							int n = 1;
							if (char.IsDigit(text5[n]))
							{
								StringBuilder stringBuilder = new StringBuilder();
								for (; n < text5.Length && char.IsDigit(text5[n]); n++)
								{
									stringBuilder.Append(text5[n]);
								}
								num12 = int.Parse(stringBuilder.ToString());
							}
							if (n < text5.Length && text5[n] == 'G')
							{
								n++;
								num13 = 3;
							}
							if (n < text5.Length && text5[n] == '(')
							{
								n++;
								flag14 = true;
							}
							if (n < text5.Length && (text5[n] == '.' || text5[n] == ','))
							{
								n++;
							}
							if (n < text5.Length && (text5[n] == '+' || text5[n] == '-'))
							{
								if (text5[n] == '+')
								{
									flag13 = true;
								}
								n++;
								text7 = ((n >= text5.Length) ? string.Empty : text5.Substring(n));
							}
						}
					}
					if (!(flag6 || flag7 || flag8 || flag9 || flag10))
					{
						flag12 = true;
					}
					if (flag6 && flag12)
					{
						flag12 = false;
					}
					if (flag15 && !string.IsNullOrEmpty(text5))
					{
						if (flag12 && flag7)
						{
							num13 = 3;
						}
						flag12 = false;
					}
					if (flag5)
					{
						flag12 = true;
					}
					if (flag9 || flag10)
					{
						string text10 = text5.Substring(1);
						switch (text10)
						{
						case "d":
							text6 = reportCulture.DateTimeFormat.ShortDatePattern;
							break;
						case "D":
							text6 = reportCulture.DateTimeFormat.LongDatePattern;
							break;
						case "f":
							text6 = reportCulture.DateTimeFormat.LongDatePattern + " " + reportCulture.DateTimeFormat.ShortTimePattern;
							break;
						case "F":
							text6 = reportCulture.DateTimeFormat.FullDateTimePattern;
							break;
						case "g":
							text6 = reportCulture.DateTimeFormat.ShortDatePattern + " " + reportCulture.DateTimeFormat.ShortTimePattern;
							break;
						case "G":
							text6 = reportCulture.DateTimeFormat.ShortDatePattern + " " + reportCulture.DateTimeFormat.LongTimePattern;
							break;
						case "m":
						case "M":
							text6 = reportCulture.DateTimeFormat.MonthDayPattern;
							break;
						case "s":
							text6 = reportCulture.DateTimeFormat.SortableDateTimePattern;
							break;
						case "t":
							text6 = reportCulture.DateTimeFormat.ShortTimePattern;
							break;
						case "T":
							text6 = reportCulture.DateTimeFormat.LongTimePattern;
							break;
						case "u":
							text6 = reportCulture.DateTimeFormat.UniversalSortableDateTimePattern;
							break;
						case "y":
						case "Y":
							text6 = reportCulture.DateTimeFormat.YearMonthPattern;
							break;
						default:
							text6 = text10;
							break;
						}
						text6.Replace(" ", "\\ ");
						int num15 = text6.IndexOf('\'');
						while (num15 != -1)
						{
							int num16 = text6.IndexOf('\'', num15 + 1);
							if (num16 == -1)
							{
								break;
							}
							text6 = text6.Substring(0, num15) + text6.Substring(num15 + 1, num16 - num15 - 1) + text6.Substring(num16 + 1);
							num15 = text6.IndexOf('\'', num16 - 2);
						}
					}
					else if (!flag12)
					{
						if (num14 != -1)
						{
							StringBuilder stringBuilder2 = new StringBuilder();
							if (num13 > 1)
							{
								stringBuilder2.Append("#,");
								stringBuilder2.Append('#', num13 - 1);
							}
							stringBuilder2.Append('0');
							if (num12 > 0)
							{
								stringBuilder2.Append(".");
								stringBuilder2.Append('0', num12);
							}
							string newValue = stringBuilder2.ToString();
							string text11 = text8.Replace("n", newValue).Replace("$", "\"" + text7 + "\"");
							string text12 = text9.Replace("n", newValue).Replace("$", "\"" + text7 + "\"");
							text6 = text11 + ";" + text12 + (flag15 ? ";" : "");
						}
						else
						{
							StringBuilder stringBuilder3 = new StringBuilder();
							if (flag6 && flag13)
							{
								stringBuilder3.Append("\"");
								stringBuilder3.Append(text7);
								stringBuilder3.Append("\"");
							}
							if (num13 > 1)
							{
								stringBuilder3.Append("#,");
								stringBuilder3.Append('#', num13 - 1);
							}
							stringBuilder3.Append('0');
							if (num12 > 0)
							{
								stringBuilder3.Append(".");
								stringBuilder3.Append('0', num12);
							}
							if (flag6 && !flag13)
							{
								stringBuilder3.Append("\"");
								stringBuilder3.Append(text7);
								stringBuilder3.Append("\"");
							}
							if (flag8)
							{
								stringBuilder3.Append("%");
							}
							text6 = stringBuilder3.ToString();
							string text13 = (flag14 ? "(" : "-") + text6 + (flag14 ? ")" : "");
							if (flag15)
							{
								text6 = text6 + ";" + text13 + ";";
							}
							else if (flag14)
							{
								text6 = text6 + ";" + text13;
							}
						}
					}
					int formatIndex = 0;
					if (flag12)
					{
						if (flag7)
						{
							formatIndex = 4;
						}
						if (flag8)
						{
							formatIndex = 10;
						}
						if (flag9)
						{
							formatIndex = 14;
						}
						if (flag10)
						{
							formatIndex = 21;
						}
						if (flag5)
						{
							formatIndex = 49;
						}
					}
					else
					{
						formatIndex = GetFormatNumber(text6);
					}
					int num17 = 0;
					if (exportObjectFormatting)
					{
						StiCellStyle cellStyle = stiCell2.CellStyle;
						DataFont dataIn3 = new DataFont(cellStyle.Font.Name, cellStyle.Font.Bold, cellStyle.Font.Italic, cellStyle.Font.Underline, cellStyle.Font.Strikeout, (int)cellStyle.Font.SizeInPoints, cellStyle.TextColor, cellStyle.Font.GdiCharSet, 0);
						if (flag)
						{
							dataIn3 = new DataFont(StiOptions.Export.CheckBoxReplacementForExcelValue.Font.Name, StiOptions.Export.CheckBoxReplacementForExcelValue.Font.Bold, StiOptions.Export.CheckBoxReplacementForExcelValue.Font.Italic, StiOptions.Export.CheckBoxReplacementForExcelValue.Font.Underline, StiOptions.Export.CheckBoxReplacementForExcelValue.Font.Strikeout, (int)StiOptions.Export.CheckBoxReplacementForExcelValue.Font.SizeInPoints, cellStyle.TextColor, StiOptions.Export.CheckBoxReplacementForExcelValue.Font.GdiCharSet, 0);
						}
						DataFill dataIn4 = new DataFill("solid", cellStyle.Color, cellStyle.Color);
						if (flag4)
						{
							Color color = StiBrush.ToColor(stiCell2.Component?.Page?.Brush);
							if (color.Equals((object?)Color.Empty) || color.Equals((object?)Color.White))
							{
								color = Color.Transparent;
							}
							dataIn4 = new DataFill("solid", color, color);
						}
						bool flag16 = true;
						bool flag17 = true;
						for (int num18 = 0; num18 < stiCell2.Height + 1; num18++)
						{
							if (matrix.BordersY[stiCell2.Top + num18, stiCell2.Left] == (StiBorderSide)null)
							{
								flag16 = false;
							}
							if (matrix.BordersY[stiCell2.Top + num18, stiCell2.Left + stiCell2.Width + 1] == (StiBorderSide)null)
							{
								flag17 = false;
							}
						}
						bool flag18 = true;
						bool flag19 = true;
						for (int num19 = 0; num19 < stiCell2.Width + 1; num19++)
						{
							if (matrix.BordersX[stiCell2.Top, stiCell2.Left + num19] == (StiBorderSide)null)
							{
								flag18 = false;
							}
							if (matrix.BordersX[stiCell2.Top + stiCell2.Height + 1, stiCell2.Left + num19] == (StiBorderSide)null)
							{
								flag19 = false;
							}
						}
						DataBorder dataIn5 = new DataBorder(flag16 ? matrix.BordersY[stiCell2.Top, stiCell2.Left] : null, flag17 ? matrix.BordersY[stiCell2.Top, stiCell2.Left + stiCell2.Width + 1] : null, flag18 ? matrix.BordersX[stiCell2.Top, stiCell2.Left] : null, flag19 ? matrix.BordersX[stiCell2.Top + stiCell2.Height + 1, stiCell2.Left] : null);
						bool flag20 = false;
						if (cellStyle.TextOptions != null)
						{
							flag20 = cellStyle.TextOptions.RightToLeft;
						}
						bool textWrapped = false;
						if (cellStyle.TextOptions != null)
						{
							textWrapped = cellStyle.TextOptions.WordWrap;
						}
						if (!string.IsNullOrEmpty(text))
						{
							if (text.IndexOf("\r", StringComparison.InvariantCulture) != -1 || text.IndexOf("\n", StringComparison.InvariantCulture) != -1)
							{
								textWrapped = true;
							}
							if (stiText != null && stiText.CheckAllowHtmlTags() && (text.IndexOf("<br", StringComparison.InvariantCulture) != -1 || text.IndexOf("<ul", StringComparison.InvariantCulture) != -1 || text.IndexOf("<ol", StringComparison.InvariantCulture) != -1 || text.IndexOf("<p", StringComparison.InvariantCulture) != -1))
							{
								textWrapped = true;
							}
						}
						int textRotationAngle = 0;
						if (cellStyle.TextOptions != null)
						{
							int num20 = (int)(cellStyle.TextOptions.Angle % 360f);
							if (num20 < 0)
							{
								num20 = 360 + num20;
							}
							if (num20 >= 0 && num20 <= 90)
							{
								textRotationAngle = num20;
							}
							if (num20 >= 270 && num20 < 360)
							{
								textRotationAngle = 360 - num20 + 90;
							}
						}
						bool editable = restrictEditing == StiExcel2007RestrictEditing.ExceptEditableFields && stiCell2.Component != null && stiCell2.Component is StiText && (stiCell2.Component as StiText).Editable;
						StiTextHorAlignment val6 = cellStyle.HorAlignment;
						if (flag20)
						{
							if ((int)val6 == 0)
							{
								val6 = (StiTextHorAlignment)2;
							}
							else if ((int)val6 == 2)
							{
								val6 = (StiTextHorAlignment)0;
							}
						}
						DataXF dataIn6 = new DataXF(formatIndex, GetFontNumber(dataIn3), GetFillNumber(dataIn4), GetBorderNumber(dataIn5), 0, flag ? StiOptions.Export.CheckBoxReplacementForExcelValue.HorAlignment : val6, flag ? StiOptions.Export.CheckBoxReplacementForExcelValue.VertAlignment : cellStyle.VertAlignment, textRotationAngle, textWrapped, flag20, editable);
						num17 = GetXFNumber(dataIn6);
					}
					xmlTextWriter.WriteStartElement("c");
					xmlTextWriter.WriteAttributeString("r", GetRefString(m, l - num));
					xmlTextWriter.WriteAttributeString("s", num17.ToString());
					if (exportObjectFormatting)
					{
						for (int num21 = 0; num21 <= stiCell2.Width; num21++)
						{
							for (int num22 = 0; num22 <= stiCell2.Height; num22++)
							{
								array[l + num22, m + num21] = true;
								array2[l + num22, m + num21] = num17;
							}
						}
						if (stiCell2.Width > 0 || stiCell2.Height > 0)
						{
							CellRangeAddress cellRangeAddress = new CellRangeAddress(l - num, l - num + stiCell2.Height, m, m + stiCell2.Width);
							arrayList2.Add(cellRangeAddress);
						}
					}
					else
					{
						array[l, m] = true;
						array2[l, m] = num17;
					}
					string text14 = null;
					if (stiText != null)
					{
						text14 = stiText.ExcelDataValue;
						if (flag7 && text14 == "" && text == " ")
						{
							text = "";
						}
					}
					if (!flag3 && (!string.IsNullOrEmpty(text) || !string.IsNullOrEmpty(text14)))
					{
						bool flag21 = false;
						bool flag22 = false;
						double result = 0.0;
						if (!string.IsNullOrEmpty(text14) && text14 != "-")
						{
							string text15 = text14;
							string text16 = text15;
							int num23 = text16.IndexOf(".");
							int num24 = text16.IndexOf(",");
							if (num23 != -1 && num24 != -1)
							{
								text16 = text16.Replace((num23 > num24) ? "," : ".", "");
							}
							string numberDecimalSeparator = reportCulture.NumberFormat.NumberDecimalSeparator;
							text16 = text16.Replace(".", ",").Replace(",", numberDecimalSeparator);
							if (!string.IsNullOrEmpty(text16))
							{
								flag22 = true;
								try
								{
									if (flag9 || flag10)
									{
										flag22 = false;
										if (StiOptions.Export.Excel.AllowExportDateTime)
										{
											switch (text6)
											{
											default:
											{
												if (DateTime.TryParse(text15, reportCulture, DateTimeStyles.None, out var result2))
												{
													result = result2.Subtract(new DateTime(1900, 1, 1, 0, 0, 0)).TotalDays + 1.0;
													if (result > 59.0)
													{
														result += 1.0;
													}
													flag22 = true;
												}
												break;
											}
											case "QI":
												break;
											case "YQ":
												break;
											case "YQI":
												break;
											}
										}
									}
									else if (!double.TryParse(text16, NumberStyles.Any, reportCulture, out result))
									{
										flag22 = false;
									}
								}
								catch (FormatException)
								{
									flag22 = false;
								}
							}
							if (flag11)
							{
								flag22 = false;
							}
							if (text15 != text && text15.StartsWith("="))
							{
								flag21 = true;
							}
							if (!flag22 && text == null)
							{
								text = text15;
							}
						}
						if (flag21)
						{
							string text17 = text14.Replace("\r", "").Replace('\t', ' ').Substring(1);
							if (StiOptions.Export.Excel.TrimTrailingSpaces)
							{
								text17 = StiExportUtils.TrimEndWhiteSpace(text17);
							}
							xmlTextWriter.WriteElementString("f", text17);
						}
						else if (flag22)
						{
							xmlTextWriter.WriteElementString("v", result.ToString().Replace(',', '.'));
						}
						else
						{
							string text18 = text.Replace("\r", "").Replace('\t', ' ');
							if (StiOptions.Export.Excel.TrimTrailingSpaces)
							{
								text18 = StiExportUtils.TrimEndWhiteSpace(text18);
							}
							text18 = RemoveControlCharacters(text18);
							int num25 = 0;
							if (stiText != null && stiText.CheckAllowHtmlTags())
							{
								text18 = ConvertAllowHtmlTagsToExcelString(stiText, text18);
								num25 = GetSSTNumber(text18);
								sstHashIsTags[num25] = null;
							}
							else
							{
								num25 = GetSSTNumber(text18);
							}
							xmlTextWriter.WriteAttributeString("t", "s");
							xmlTextWriter.WriteElementString("v", num25.ToString());
						}
					}
					xmlTextWriter.WriteEndElement();
					continue;
				}
				int num26 = num5;
				if (array[l, m])
				{
					num26 = array2[l, m];
				}
				bool flag23 = matrix.BordersY[l, m] != (StiBorderSide)null;
				bool flag24 = matrix.BordersY[l, m + 1] != (StiBorderSide)null;
				bool flag25 = matrix.BordersX[l, m] != (StiBorderSide)null;
				bool flag26 = matrix.BordersX[l + 1, m] != (StiBorderSide)null;
				if (flag23 || flag24 || flag25 || flag26 || num26 != 0)
				{
					DataBorder dataIn7 = new DataBorder(flag23 ? matrix.BordersY[l, m] : null, flag24 ? matrix.BordersY[l, m + 1] : null, flag25 ? matrix.BordersX[l, m] : null, flag26 ? matrix.BordersX[l + 1, m] : null);
					DataXF dataXF = (DataXF)xfList[num26];
					DataXF dataIn8 = new DataXF(dataXF.FormatIndex, dataXF.FontIndex, dataXF.FillIndex, GetBorderNumber(dataIn7), dataXF.XFId, dataXF.HorAlign, dataXF.VertAlign, dataXF.TextRotationAngle, dataXF.TextWrapped, dataXF.RightToLeft, dataXF.Editable);
					int num27 = GetXFNumber(dataIn8);
					if (!exportObjectFormatting)
					{
						num27 = 0;
					}
					xmlTextWriter.WriteStartElement("c");
					xmlTextWriter.WriteAttributeString("r", GetRefString(m, l - num));
					xmlTextWriter.WriteAttributeString("s", num27.ToString());
					xmlTextWriter.WriteEndElement();
				}
			}
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		((Component)(object)val)?.Dispose();
		if (restrictEditing != StiExcel2007RestrictEditing.No)
		{
			xmlTextWriter.WriteStartElement("sheetProtection");
			xmlTextWriter.WriteAttributeString("password", "F05F");
			xmlTextWriter.WriteAttributeString("sheet", "1");
			xmlTextWriter.WriteAttributeString("objects", "1");
			xmlTextWriter.WriteAttributeString("scenarios", "1");
			xmlTextWriter.WriteEndElement();
		}
		if (arrayList2.Count > 0 && (dataExportMode == StiDataExportMode.AllBands || exportObjectFormatting))
		{
			xmlTextWriter.WriteStartElement("mergeCells");
			xmlTextWriter.WriteAttributeString("count", $"{arrayList2.Count}");
			for (int num28 = 0; num28 < arrayList2.Count; num28++)
			{
				CellRangeAddress cellRangeAddress2 = (CellRangeAddress)arrayList2[num28];
				xmlTextWriter.WriteStartElement("mergeCell");
				xmlTextWriter.WriteAttributeString("ref", $"{GetRefString(cellRangeAddress2.FirstColumn, cellRangeAddress2.FirstRow)}:{GetRefString(cellRangeAddress2.LastColumn, cellRangeAddress2.LastRow)}");
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
		}
		if (arrayList3.Count > 0)
		{
			bool flag27 = false;
			for (int num29 = 0; num29 < arrayList3.Count; num29++)
			{
				string bookmark = ((HlinkData)arrayList3[num29]).Bookmark;
				if (bookmark.StartsWith("#", StringComparison.InvariantCulture))
				{
					bookmark = bookmark.Substring(1);
					for (int num30 = 0; num30 < matrixList.Count; num30++)
					{
						Hashtable bookmarksTable = ((StiMatrix)matrixList[num30]).Bookmarks.BookmarksTable;
						if (bookmarksTable == null)
						{
							continue;
						}
						object obj = bookmarksTable[bookmark];
						if (obj != null)
						{
							int num31 = (int)minRowList[num30];
							int num32 = (int)maxRowList[num30];
							Size size2 = (Size)obj;
							if (size2.Height >= num31 && size2.Height < num32)
							{
								flag27 = true;
								break;
							}
						}
					}
				}
				else
				{
					flag27 = true;
				}
				if (flag27)
				{
					break;
				}
			}
			if (flag27)
			{
				xmlTextWriter.WriteStartElement("hyperlinks");
				for (int num33 = 0; num33 < arrayList3.Count; num33++)
				{
					HlinkData hlinkData2 = (HlinkData)arrayList3[num33];
					string text19 = hlinkData2.Bookmark;
					if (text19.StartsWith("#", StringComparison.InvariantCulture))
					{
						text19 = text19.Substring(1);
						bool flag28 = false;
						for (int num34 = 0; num34 < matrixList.Count; num34++)
						{
							Hashtable bookmarksTable2 = ((StiMatrix)matrixList[num34]).Bookmarks.BookmarksTable;
							if (bookmarksTable2 == null)
							{
								continue;
							}
							object obj2 = bookmarksTable2[text19];
							if (obj2 != null)
							{
								int num35 = (int)minRowList[num34];
								int num36 = (int)maxRowList[num34];
								Size size3 = (Size)obj2;
								if (size3.Height >= num35 && size3.Height < num36)
								{
									text19 = $"'{(string)sheetNameList[num34]}'!{GetRefString(size3.Width, size3.Height - num35)}";
									flag28 = true;
									break;
								}
							}
						}
						if (flag28)
						{
							xmlTextWriter.WriteStartElement("hyperlink");
							xmlTextWriter.WriteAttributeString("ref", GetRefString(hlinkData2.Range.FirstColumn, hlinkData2.Range.FirstRow));
							xmlTextWriter.WriteAttributeString("location", text19);
							xmlTextWriter.WriteAttributeString("display", hlinkData2.Description);
							xmlTextWriter.WriteEndElement();
						}
					}
					else
					{
						string text20 = text19;
						if (text19.StartsWith("file:\\\\\\", StringComparison.InvariantCulture))
						{
							text20 = text19.Substring(8);
							text19 = "file:///" + text20;
						}
						if (text19.StartsWith("http:", StringComparison.InvariantCulture))
						{
							text19 = text19.Replace(" ", "");
						}
						xmlTextWriter.WriteStartElement("hyperlink");
						xmlTextWriter.WriteAttributeString("ref", GetRefString(hlinkData2.Range.FirstColumn, hlinkData2.Range.FirstRow));
						xmlTextWriter.WriteAttributeString("r:id", $"rId{2 + hyperlinkList.Count}");
						xmlTextWriter.WriteAttributeString("display", text20);
						xmlTextWriter.WriteEndElement();
						hyperlinkList.Add(StringToUrl(text19));
					}
				}
				xmlTextWriter.WriteEndElement();
			}
		}
		xmlTextWriter.WriteStartElement("pageMargins");
		xmlTextWriter.WriteAttributeString("left", FloatToString(page.Unit.ConvertToHInches(page.Margins.Left) / 100.0));
		xmlTextWriter.WriteAttributeString("right", FloatToString(page.Unit.ConvertToHInches(page.Margins.Right) / 100.0));
		xmlTextWriter.WriteAttributeString("top", FloatToString(page.Unit.ConvertToHInches(page.Margins.Top) / 100.0));
		xmlTextWriter.WriteAttributeString("bottom", FloatToString(page.Unit.ConvertToHInches(page.Margins.Bottom) / 100.0));
		xmlTextWriter.WriteAttributeString("header", FloatToString(0.0));
		xmlTextWriter.WriteAttributeString("footer", FloatToString(0.0));
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("pageSetup");
		xmlTextWriter.WriteAttributeString("paperSize", $"{(int)page.PaperSize}");
		xmlTextWriter.WriteAttributeString("orientation", (page.Orientation == StiPageOrientation.Portrait) ? "portrait" : "landscape");
		if (StiOptions.Export.Excel.NumberOfPagesInWideToFit > 0 || StiOptions.Export.Excel.NumberOfPagesInHeightToFit > 0)
		{
			if (StiOptions.Export.Excel.NumberOfPagesInWideToFit != 1)
			{
				xmlTextWriter.WriteAttributeString("fitToWidth", StiOptions.Export.Excel.NumberOfPagesInWideToFit.ToString());
			}
			if (StiOptions.Export.Excel.NumberOfPagesInHeightToFit != 1)
			{
				xmlTextWriter.WriteAttributeString("fitToHeight", StiOptions.Export.Excel.NumberOfPagesInHeightToFit.ToString());
			}
		}
		xmlTextWriter.WriteEndElement();
		StiLicenseKey licenseKey = StiLicenseKeyValidator.GetLicenseKey();
		bool flag29 = !StiLicenseKeyValidator.IsValidOnNetFramework(licenseKey);
		if (!typeof(StiLicense).AssemblyQualifiedName.Contains("PublicKeyToken=ebe6666cba19647a"))
		{
			flag29 = true;
		}
		if (!flag29)
		{
			try
			{
				using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(512);
				using SHA1CryptoServiceProvider halg = new SHA1CryptoServiceProvider();
				rSACryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>2GCA4rfVor/2q7m+NVisL3ZFVvnklg14zeP0F+yNpBdoSm9iuOUBLodsOH6I3Xb0j5/OznYKFpii35RcjiPvww7Uthz1he2G8PGCvFMKuA4Wzgp5Kdv0uR7zSQ8J9lA/rZ7u8wh+h72umBku1pLFL2nvJIT56u4Qe7+OJFLrqWk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
				flag29 = !rSACryptoServiceProvider.VerifyData(licenseKey.GetCheckBytes(), halg, licenseKey.GetSignatureBytes());
			}
			catch (Exception)
			{
				flag29 = true;
			}
		}
		if (flag29)
		{
			bool flag30 = page.Unit.ConvertToHInches(page.PageHeight) > 400.0;
			xmlTextWriter.WriteStartElement("headerFooter");
			xmlTextWriter.WriteStartElement("oddHeader");
			xmlTextWriter.WriteRaw("&amp;C\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n" + (flag30 ? "\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n" : "") + "&amp;G");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		int num37 = Math.Min(1024, Matrix.HorizontalPageBreaks.Count);
		if (exportHorizontalPageBreaks && num37 > 0)
		{
			int num38 = Math.Max(0, Matrix.CoordX.Count - 1 - 1);
			xmlTextWriter.WriteStartElement("rowBreaks");
			xmlTextWriter.WriteAttributeString("count", num37.ToString());
			xmlTextWriter.WriteAttributeString("manualBreakCount", num37.ToString());
			for (int num39 = 0; num39 < num37; num39++)
			{
				xmlTextWriter.WriteStartElement("brk");
				xmlTextWriter.WriteAttributeString("id", $"{Matrix.HorizontalPageBreaks[num39]}");
				xmlTextWriter.WriteAttributeString("max", $"{num38}");
				xmlTextWriter.WriteAttributeString("man", "1");
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
		}
		if (imageList.Count > imageListOffset || barCodeLinesList.Count > 0 || flag29)
		{
			xmlTextWriter.WriteStartElement("drawing");
			xmlTextWriter.WriteAttributeString("r:id", $"rId{1}");
			xmlTextWriter.WriteEndElement();
		}
		if (arrayList.Count > 0)
		{
			xmlTextWriter.WriteStartElement("ignoredErrors");
			foreach (string item in arrayList)
			{
				xmlTextWriter.WriteStartElement("ignoredError");
				xmlTextWriter.WriteAttributeString("sqref", item);
				xmlTextWriter.WriteAttributeString("numberStoredAsText", "1");
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
		}
		if (flag29)
		{
			xmlTextWriter.WriteStartElement("legacyDrawingHF");
			xmlTextWriter.WriteAttributeString("r:id", "dId1");
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private void PrepareMatrix(StiPagesCollection pages)
	{
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06de: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		StiDataExportMode stiDataExportMode = dataExportMode;
		if (StiOptions.Export.Excel.AllowExportFootersInDataOnlyMode)
		{
			stiDataExportMode |= StiDataExportMode.Footers;
		}
		matrix = new StiMatrix(pages, StiOptions.Export.Excel.DivideBigCells, this, null, stiDataExportMode);
		if (base.IsStopped)
		{
			return;
		}
		if (useOnePageHeaderAndFooter)
		{
			matrix.ScanComponentsPlacement(optimize: true, exportObjectFormatting);
			int i;
			for (i = 0; matrix.LinePlacement[i] != StiMatrix.StiTableLineInfo.PageHeader && i < matrix.CoordY.Count - 1; i++)
			{
			}
			if (matrix.LinePlacement[i] == StiMatrix.StiTableLineInfo.PageHeader)
			{
				for (; matrix.LinePlacement[i] == StiMatrix.StiTableLineInfo.PageHeader && i < matrix.CoordY.Count - 1; i++)
				{
				}
				for (; i < matrix.CoordY.Count - 1; i++)
				{
					if (matrix.LinePlacement[i] == StiMatrix.StiTableLineInfo.PageHeader)
					{
						matrix.LinePlacement[i] = StiMatrix.StiTableLineInfo.Trash;
					}
				}
			}
			i = matrix.CoordY.Count - 1;
			while (matrix.LinePlacement[i] != StiMatrix.StiTableLineInfo.PageFooter && i > 0)
			{
				i--;
			}
			if (matrix.LinePlacement[i] == StiMatrix.StiTableLineInfo.PageFooter)
			{
				while (matrix.LinePlacement[i] == StiMatrix.StiTableLineInfo.PageFooter && i > 0)
				{
					i--;
				}
				while (i > 0)
				{
					if (matrix.LinePlacement[i] == StiMatrix.StiTableLineInfo.PageFooter)
					{
						matrix.LinePlacement[i] = StiMatrix.StiTableLineInfo.Trash;
					}
					i--;
				}
			}
			matrix.AllowModification(flag: true);
			int num = 0;
			int num2 = 0;
			for (int j = 0; j < Matrix.CoordY.Count - 1; j++)
			{
				if (Matrix.LinePlacement[j] != StiMatrix.StiTableLineInfo.Trash)
				{
					for (int k = 0; k < Matrix.CoordX.Count - 1; k++)
					{
						matrix.Cells[num, k] = matrix.Cells[j, k];
						StiCell stiCell = matrix.Cells[num, k];
						if (stiCell != null)
						{
							stiCell.Top = num;
							if (stiCell.ExportImage != null)
							{
								StiComponent component = stiCell.Component;
								object obj = matrix.imagesBaseRect[component];
								if (obj != null)
								{
									RectangleD val = (RectangleD)obj;
									double num3 = (double)matrix.CoordY.GetByIndex(j) - (double)matrix.CoordY.GetByIndex(num);
									((RectangleD)(ref val)).Y = ((RectangleD)(ref val)).Y - num3;
									matrix.imagesBaseRect[component] = val;
								}
							}
						}
						matrix.BordersX[num, k] = matrix.BordersX[j, k];
						matrix.BordersY[num, k] = matrix.BordersY[j, k];
						matrix.Bookmarks[num, k] = matrix.Bookmarks[j, k];
					}
					matrix.BordersY[num, Matrix.CoordX.Count - 1] = matrix.BordersY[j, Matrix.CoordX.Count - 1];
					double num4 = (double)matrix.CoordY.GetByIndex(j + 1) - (double)matrix.CoordY.GetByIndex(j);
					matrix.CoordY.SetByIndex(num + 1, (double)matrix.CoordY.GetByIndex(num) + num4);
					num++;
				}
				else
				{
					num2++;
				}
				if (!exportHorizontalPageBreaks)
				{
					continue;
				}
				for (int l = 0; l < matrix.HorizontalPageBreaks.Count; l++)
				{
					if (matrix.HorizontalPageBreaks[l] == j)
					{
						matrix.HorizontalPageBreaks[l] = matrix.HorizontalPageBreaks[l] - (num2 - 1);
					}
				}
			}
			for (int m = 0; m < Matrix.CoordX.Count - 1; m++)
			{
				matrix.BordersX[num, m] = matrix.BordersX[Matrix.CoordY.Count - 1, m];
			}
			int num5 = Matrix.CoordY.Count - 1 - num;
			if (num5 > 0)
			{
				for (int n = 0; n < num5; n++)
				{
					matrix.CoordY.RemoveAt(num + 1);
				}
			}
			matrix.AllowModification(flag: false);
		}
		if (dataExportMode == StiDataExportMode.AllBands)
		{
			return;
		}
		matrix.ScanComponentsPlacement(optimize: true, exportObjectFormatting);
		matrix.AllowModification(flag: true);
		int num6 = 0;
		Hashtable hashtable = new Hashtable();
		string text = null;
		bool flag = false;
		for (int num7 = 0; num7 < Matrix.CoordY.Count - 1; num7++)
		{
			bool flag2 = false;
			if (Matrix.LinePlacement[num7] == StiMatrix.StiTableLineInfo.HeaderD || Matrix.LinePlacement[num7] == StiMatrix.StiTableLineInfo.HeaderAP)
			{
				string text2 = Matrix.ParentBandName[num7];
				if (flag && GetParentBandName(num7) == GetParentBandName(num7 - 1))
				{
					flag2 = true;
				}
				else
				{
					int num8 = text2.IndexOf('\u001f');
					if (num8 != -1)
					{
						string text3 = text2.Substring(0, num8);
						if (text3 != text)
						{
							text = text3;
							hashtable.Clear();
						}
					}
					if (!hashtable.ContainsKey(text2))
					{
						flag2 = true;
						hashtable.Add(text2, text2);
					}
				}
			}
			flag = flag2;
			if (!(Matrix.LinePlacement[num7] == StiMatrix.StiTableLineInfo.Data || Matrix.LinePlacement[num7] == StiMatrix.StiTableLineInfo.FooterD || flag2))
			{
				continue;
			}
			for (int num9 = 0; num9 < Matrix.CoordX.Count - 1; num9++)
			{
				matrix.Cells[num6, num9] = matrix.Cells[num7, num9];
				StiCell stiCell2 = matrix.Cells[num6, num9];
				if (stiCell2 != null)
				{
					stiCell2.Top = num6;
					if (stiCell2.ExportImage != null)
					{
						StiComponent component2 = stiCell2.Component;
						object obj2 = matrix.imagesBaseRect[component2];
						if (obj2 != null)
						{
							RectangleD val2 = (RectangleD)obj2;
							double num10 = (double)matrix.CoordY.GetByIndex(num7) - (double)matrix.CoordY.GetByIndex(num6);
							((RectangleD)(ref val2)).Y = ((RectangleD)(ref val2)).Y - num10;
							matrix.imagesBaseRect[component2] = val2;
						}
					}
				}
				if (num6 == 0 || matrix.BordersX[num7, num9] != (StiBorderSide)null)
				{
					matrix.BordersX[num6, num9] = matrix.BordersX[num7, num9];
				}
				matrix.BordersX[num6 + 1, num9] = matrix.BordersX[num7 + 1, num9];
				matrix.BordersY[num6, num9] = matrix.BordersY[num7, num9];
				matrix.Bookmarks[num6, num9] = matrix.Bookmarks[num7, num9];
			}
			matrix.BordersY[num6, Matrix.CoordX.Count - 1] = matrix.BordersY[num7, Matrix.CoordX.Count - 1];
			double num11 = (double)matrix.CoordY.GetByIndex(num7 + 1) - (double)matrix.CoordY.GetByIndex(num7);
			matrix.CoordY.SetByIndex(num6 + 1, (double)matrix.CoordY.GetByIndex(num6) + num11);
			num6++;
		}
		int num12 = Matrix.CoordY.Count - 1 - num6;
		if (num12 > 0)
		{
			for (int num13 = 0; num13 < num12; num13++)
			{
				matrix.CoordY.RemoveAt(num6 + 1);
			}
		}
		matrix.AllowModification(flag: false);
	}

	private string GetParentBandName(int rowIndex)
	{
		string text = Matrix.ParentBandName[rowIndex];
		int num = text.IndexOf('\u001f');
		if (num == -1)
		{
			return text;
		}
		return text.Substring(0, num);
	}

	private string ConvertAllowHtmlTagsToExcelString(StiText textBox, string input)
	{
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder stringBuilder = new StringBuilder(input.Length * 2);
		StringWriter w = new StringWriter(stringBuilder);
		XmlTextWriter xmlTextWriter = new XmlTextWriter(w);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		StiHtmlTagsState val = default(StiHtmlTagsState);
		((StiHtmlTagsState)(ref val))._002Ector(textBox.Font.Bold, textBox.Font.Italic, textBox.Font.Underline, textBox.Font.Strikeout, textBox.Font.SizeInPoints, textBox.Font.Name, StiBrush.ToColor(textBox.TextBrush), StiBrush.ToColor(textBox.Brush), false, false, 0.0, 0.0, 1.0, textBox.HorAlignment);
		StiHtmlState val2 = default(StiHtmlState);
		((StiHtmlState)(ref val2))._002Ector(val, 0);
		List<StiHtmlState> list = StiTextRenderer.ParseHtmlToStates(input, val2, false);
		foreach (StiHtmlState item in list)
		{
			xmlTextWriter.WriteStartElement("r");
			xmlTextWriter.WriteStartElement("rPr");
			if (item.TS.Bold)
			{
				xmlTextWriter.WriteElementString("b", null);
			}
			if (item.TS.Italic)
			{
				xmlTextWriter.WriteElementString("i", null);
			}
			if (item.TS.Underline)
			{
				xmlTextWriter.WriteElementString("u", null);
			}
			if (item.TS.Strikeout)
			{
				xmlTextWriter.WriteElementString("strike", null);
			}
			if (item.TS.Superscript)
			{
				xmlTextWriter.WriteStartElement("vertAlign");
				xmlTextWriter.WriteAttributeString("val", "superscript");
				xmlTextWriter.WriteEndElement();
			}
			if (item.TS.Subscript)
			{
				xmlTextWriter.WriteStartElement("vertAlign");
				xmlTextWriter.WriteAttributeString("val", "subscript");
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteStartElement("sz");
			float fontSize = item.TS.FontSize;
			xmlTextWriter.WriteAttributeString("val", fontSize.ToString());
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("color");
			Color fontColor = item.TS.FontColor;
			xmlTextWriter.WriteAttributeString("rgb", $"{fontColor.ToArgb():X8}");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("rFont");
			xmlTextWriter.WriteAttributeString("val", item.TS.FontName);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			string text = StiTextRenderer.PrepareStateText(item.Text).ToString();
			if (string.IsNullOrEmpty(text))
			{
				text = " ";
			}
			ConvertTextToExcelString(xmlTextWriter, text);
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.Flush();
		xmlTextWriter.Close();
		return stringBuilder.ToString();
	}

	private void ConvertTextToExcelString(XmlTextWriter writer, string input)
	{
		writer.WriteStartElement("t");
		if (input.Length > 0 && (char.IsWhiteSpace(input, input.Length - 1) || char.IsWhiteSpace(input, 0) || input.IndexOf('\n') != -1))
		{
			writer.WriteAttributeString("xml:space", "preserve");
		}
		writer.WriteString(input);
		writer.WriteFullEndElement();
	}

	private MemoryStream WriteDrawingRels(int indexSheet)
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("Relationships");
		xmlTextWriter.WriteAttributeString("xmlns", "http://schemas.openxmlformats.org/package/2006/relationships");
		for (int i = imageListOffset; i < imageList.Count; i++)
		{
			ExcelImageData excelImageData = (ExcelImageData)imageList[i];
			xmlTextWriter.WriteStartElement("Relationship");
			xmlTextWriter.WriteAttributeString("Id", $"rId{i - imageListOffset + 1}");
			xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");
			xmlTextWriter.WriteAttributeString("Target", $"../media/image{excelImageData.ImageIndex + 1:D5}.{GetImageFormatExtension(imageCache.ImageFormatStore[excelImageData.ImageIndex])}");
			xmlTextWriter.WriteEndElement();
			if (!string.IsNullOrWhiteSpace(excelImageData.Hyperlink))
			{
				xmlTextWriter.WriteStartElement("Relationship");
				xmlTextWriter.WriteAttributeString("Id", $"hId{i - imageListOffset + 1}");
				xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink");
				xmlTextWriter.WriteAttributeString("Target", excelImageData.Hyperlink);
				xmlTextWriter.WriteAttributeString("TargetMode", "External");
				xmlTextWriter.WriteEndElement();
			}
		}
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteDrawing(int indexSheet, StiPage page)
	{
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Invalid comparison between Unknown and I4
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("xdr:wsDr");
		xmlTextWriter.WriteAttributeString("xmlns:xdr", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
		xmlTextWriter.WriteAttributeString("xmlns:a", "http://schemas.openxmlformats.org/drawingml/2006/main");
		for (int i = imageListOffset; i < imageList.Count; i++)
		{
			ExcelImageData excelImageData = (ExcelImageData)imageList[i];
			xmlTextWriter.WriteStartElement("xdr:twoCellAnchor");
			if (!StiOptions.Export.Excel.ImageMoveAndSizeWithCells)
			{
				xmlTextWriter.WriteAttributeString("editAs", "oneCell");
			}
			xmlTextWriter.WriteStartElement("xdr:from");
			xmlTextWriter.WriteElementString("xdr:col", $"{excelImageData.FirstColumnIndex}");
			xmlTextWriter.WriteElementString("xdr:colOff", $"{excelImageData.FirstColumnOffset}");
			xmlTextWriter.WriteElementString("xdr:row", $"{excelImageData.FirstRowIndex}");
			xmlTextWriter.WriteElementString("xdr:rowOff", $"{excelImageData.FirstRowOffset}");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("xdr:to");
			xmlTextWriter.WriteElementString("xdr:col", $"{excelImageData.LastColumnIndex}");
			xmlTextWriter.WriteElementString("xdr:colOff", $"{excelImageData.LastColumnOffset}");
			xmlTextWriter.WriteElementString("xdr:row", $"{excelImageData.LastRowIndex}");
			xmlTextWriter.WriteElementString("xdr:rowOff", $"{excelImageData.LastRowOffset}");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("xdr:pic");
			xmlTextWriter.WriteStartElement("xdr:nvPicPr");
			xmlTextWriter.WriteStartElement("xdr:cNvPr");
			xmlTextWriter.WriteAttributeString("id", $"{i - imageListOffset + 2}");
			xmlTextWriter.WriteAttributeString("name", $"Picture {i - imageListOffset + 1}");
			xmlTextWriter.WriteAttributeString("descr", $"image{i + 1:D5}");
			if (!string.IsNullOrWhiteSpace(excelImageData.Hyperlink))
			{
				xmlTextWriter.WriteStartElement("a:hlinkClick");
				xmlTextWriter.WriteAttributeString("xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
				xmlTextWriter.WriteAttributeString("r:id", $"hId{i - imageListOffset + 1}");
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("xdr:cNvPicPr");
			xmlTextWriter.WriteStartElement("a:picLocks");
			xmlTextWriter.WriteAttributeString("noChangeAspect", "1");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("xdr:blipFill");
			xmlTextWriter.WriteStartElement("a:blip");
			xmlTextWriter.WriteAttributeString("xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
			xmlTextWriter.WriteAttributeString("r:embed", $"rId{i - imageListOffset + 1}");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("a:stretch");
			xmlTextWriter.WriteStartElement("a:fillRect");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("xdr:spPr");
			xmlTextWriter.WriteStartElement("a:xfrm");
			xmlTextWriter.WriteStartElement("a:off");
			xmlTextWriter.WriteAttributeString("x", "1");
			xmlTextWriter.WriteAttributeString("y", "1");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("a:ext");
			xmlTextWriter.WriteAttributeString("cx", "2");
			xmlTextWriter.WriteAttributeString("cy", "2");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("a:prstGeom");
			xmlTextWriter.WriteAttributeString("prst", "rect");
			xmlTextWriter.WriteStartElement("a:avLst");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			if (AllowNativeImageBorders && excelImageData.Border != null && !(excelImageData.Border is StiAdvancedBorder) && excelImageData.Border.IsAllBorderSidesPresent && !excelImageData.Border.Color.Equals((object?)Color.Transparent) && (int)excelImageData.Border.Style != 6)
			{
				xmlTextWriter.WriteStartElement("a:ln");
				xmlTextWriter.WriteAttributeString("w", $"{ConvertToEMU(excelImageData.Border.GetSize())}");
				xmlTextWriter.WriteStartElement("a:solidFill");
				xmlTextWriter.WriteStartElement("a:srgbClr");
				xmlTextWriter.WriteAttributeString("val", $"{excelImageData.Border.Color.ToArgb():X8}".Substring(2));
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				if ((int)excelImageData.Border.Style != 0)
				{
					string lineStyle = GetLineStyle(new StiBorderSide(excelImageData.Border.Color, excelImageData.Border.Size, excelImageData.Border.Style));
					xmlTextWriter.WriteStartElement("a:prstDash");
					xmlTextWriter.WriteAttributeString("val", lineStyle);
					xmlTextWriter.WriteEndElement();
				}
				xmlTextWriter.WriteStartElement("a:round");
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("a:headEnd");
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("a:tailEnd");
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("xdr:clientData");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		for (int j = 0; j < barCodeLinesList.Count; j++)
		{
			BarCodeLineData line = barCodeLinesList[j];
			BarCodeLineData realRowCol = GetRealRowCol(line, start: true);
			BarCodeLineData realRowCol2 = GetRealRowCol(line, start: false);
			xmlTextWriter.WriteStartElement("xdr:twoCellAnchor");
			xmlTextWriter.WriteStartElement("xdr:from");
			xmlTextWriter.WriteElementString("xdr:col", $"{realRowCol.Column}");
			xmlTextWriter.WriteElementString("xdr:colOff", $"{ConvertToEMU(((PointD)(ref realRowCol.Position)).X)}");
			xmlTextWriter.WriteElementString("xdr:row", $"{realRowCol.Row}");
			xmlTextWriter.WriteElementString("xdr:rowOff", $"{ConvertToEMU(((PointD)(ref realRowCol.Position)).Y)}");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("xdr:to");
			xmlTextWriter.WriteElementString("xdr:col", $"{realRowCol2.Column}");
			xmlTextWriter.WriteElementString("xdr:colOff", $"{ConvertToEMU(((PointD)(ref realRowCol2.Position)).X)}");
			xmlTextWriter.WriteElementString("xdr:row", $"{realRowCol2.Row}");
			xmlTextWriter.WriteElementString("xdr:rowOff", $"{ConvertToEMU(((PointD)(ref realRowCol2.Position)).Y)}");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("xdr:sp");
			xmlTextWriter.WriteAttributeString("macro", "");
			xmlTextWriter.WriteAttributeString("textlink", "");
			xmlTextWriter.WriteStartElement("xdr:nvSpPr");
			xmlTextWriter.WriteStartElement("xdr:cNvPr");
			xmlTextWriter.WriteAttributeString("id", $"{imageList.Count + j + 2}");
			xmlTextWriter.WriteAttributeString("name", $"Line {imageList.Count + j + 2}");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("xdr:cNvSpPr");
			xmlTextWriter.WriteStartElement("a:picLocks");
			xmlTextWriter.WriteAttributeString("noChangeShapeType", "1");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("xdr:spPr");
			xmlTextWriter.WriteAttributeString("bwMode", "auto");
			xmlTextWriter.WriteStartElement("a:xfrm");
			xmlTextWriter.WriteStartElement("a:off");
			xmlTextWriter.WriteAttributeString("x", $"{ConvertToEMU(((PointD)(ref line.Position)).X + ((SizeD)(ref line.Size)).Width / 2.0)}");
			xmlTextWriter.WriteAttributeString("y", $"{ConvertToEMU(((PointD)(ref line.Position)).Y)}");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("a:ext");
			xmlTextWriter.WriteAttributeString("cx", $"{ConvertToEMU(((PointD)(ref line.Position)).X + ((SizeD)(ref line.Size)).Width / 2.0)}");
			xmlTextWriter.WriteAttributeString("cy", $"{ConvertToEMU(((PointD)(ref line.Position)).Y + ((SizeD)(ref line.Size)).Height)}");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("a:prstGeom");
			xmlTextWriter.WriteAttributeString("prst", "line");
			xmlTextWriter.WriteStartElement("a:avLst");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("a:noFill");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("a:ln");
			xmlTextWriter.WriteAttributeString("w", $"{ConvertToEMU(((SizeD)(ref line.Size)).Width)}");
			xmlTextWriter.WriteStartElement("a:solidFill");
			xmlTextWriter.WriteStartElement("a:srgbClr");
			xmlTextWriter.WriteAttributeString("val", $"{line.Color.ToArgb():X8}".Substring(2));
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("a:round");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("a:headEnd");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("a:tailEnd");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("xdr:clientData");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		if (page != null)
		{
			double x = page.Unit.ConvertToHInches(page.PageWidth - page.Margins.Left - page.Margins.Right);
			double num = page.Unit.ConvertToHInches(page.PageHeight - page.Margins.Top - page.Margins.Bottom);
			double num2 = num * 0.45;
			int num3 = (int)minRowList[indexSheet];
			int num4 = num3;
			double num5 = (double)matrix.CoordY.GetByIndex(num3);
			double num6 = num5;
			for (int k = num3 + 1; k < matrix.CoordY.Count - 1; k++)
			{
				double num7 = (double)matrix.CoordY.GetByIndex(k);
				if (num7 - num5 > num2)
				{
					break;
				}
				num4 = k;
				num6 = num7;
			}
			double x2 = num2 - (num6 - num5);
			string data = "<xdr:oneCellAnchor><xdr:from><xdr:col>0</xdr:col><xdr:colOff>0</xdr:colOff><xdr:row>" + num4 + "</xdr:row><xdr:rowOff>" + (int)ConvertToEMU(x2) + "</xdr:rowOff></xdr:from><xdr:ext cx=\"" + ConvertToEMU(x) + "\" cy=\"" + (int)ConvertToEMU(num * 0.1) + "\"/><xdr:sp macro=\"\" textlink=\"\"><xdr:nvSpPr><xdr:cNvPr id=\"" + (imageList.Count + barCodeLinesList.Count + indexSheet + 2) + "\" name=\"TextBox 1\"/><xdr:cNvSpPr txBox=\"1\"/></xdr:nvSpPr><xdr:spPr><a:xfrm rot=\"-2700000\"><a:off x=\"0\" y=\"0\"/><a:ext cx=\"" + ConvertToEMU(x) + "\" cy=\"" + ConvertToEMU(num * 0.1) + "\"/></a:xfrm><a:prstGeom prst=\"rect\"><a:avLst/></a:prstGeom><a:noFill/><a:ln w=\"9525\" cmpd=\"sng\"><a:noFill/></a:ln></xdr:spPr><xdr:style><a:lnRef idx=\"0\"><a:scrgbClr r=\"0\" g=\"0\" b=\"0\"/></a:lnRef><a:fillRef idx=\"0\"><a:scrgbClr r=\"0\" g=\"0\" b=\"0\"/></a:fillRef><a:effectRef idx=\"0\"><a:scrgbClr r=\"0\" g=\"0\" b=\"0\"/></a:effectRef><a:fontRef idx=\"minor\"><a:schemeClr val=\"dk1\"/></a:fontRef></xdr:style><xdr:txBody><a:bodyPr vertOverflow=\"overflow\" horzOverflow=\"overflow\" wrap=\"square\" rtlCol=\"0\" anchor=\"ctr\"/><a:lstStyle/><a:p><a:pPr algn=\"ctr\"/><a:r><a:rPr lang=\"en-US\" sz=\"9600\" b=\"1\"><a:solidFill><a:schemeClr val=\"dk1\"><a:alpha val=\"18000\"/></a:schemeClr></a:solidFill><a:latin typeface=\"Arial\" panose=\"020B0604020202020204\" pitchFamily=\"34\" charset=\"0\"/><a:cs typeface=\"Arial\" panose=\"020B0604020202020204\" pitchFamily=\"34\" charset=\"0\"/></a:rPr><a:t>Trial</a:t></a:r></a:p></xdr:txBody></xdr:sp><xdr:clientData/></xdr:oneCellAnchor>";
			xmlTextWriter.WriteRaw(data);
		}
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private BarCodeLineData GetRealRowCol(BarCodeLineData line, bool start)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		BarCodeLineData barCodeLineData = default(BarCodeLineData);
		barCodeLineData.Row = line.Row;
		barCodeLineData.Column = line.Column;
		barCodeLineData.Position = line.Position;
		BarCodeLineData result = barCodeLineData;
		ref PointD position = ref result.Position;
		((PointD)(ref position)).X = ((PointD)(ref position)).X + ((SizeD)(ref line.Size)).Width / 2.0;
		if (!start)
		{
			ref PointD position2 = ref result.Position;
			((PointD)(ref position2)).Y = ((PointD)(ref position2)).Y + ((SizeD)(ref line.Size)).Height;
		}
		ref PointD position3 = ref result.Position;
		((PointD)(ref position3)).X = ((PointD)(ref position3)).X - (double)Matrix.CoordX.GetByIndex(result.Column);
		ref PointD position4 = ref result.Position;
		((PointD)(ref position4)).Y = ((PointD)(ref position4)).Y - (double)Matrix.CoordY.GetByIndex(result.Row);
		bool flag = true;
		while (flag)
		{
			double num = (double)Matrix.CoordX.GetByIndex(result.Column + 1) - (double)Matrix.CoordX.GetByIndex(result.Column);
			if (((PointD)(ref result.Position)).X > num)
			{
				result.Column++;
				ref PointD position5 = ref result.Position;
				((PointD)(ref position5)).X = ((PointD)(ref position5)).X - num;
			}
			else
			{
				flag = false;
			}
		}
		flag = true;
		while (flag)
		{
			double num2 = (double)Matrix.CoordY.GetByIndex(result.Row + 1) - (double)Matrix.CoordY.GetByIndex(result.Row);
			if (((PointD)(ref result.Position)).Y > num2)
			{
				result.Row++;
				ref PointD position6 = ref result.Position;
				((PointD)(ref position6)).Y = ((PointD)(ref position6)).Y - num2;
			}
			else
			{
				flag = false;
			}
		}
		return result;
	}

	private MemoryStream WriteStyles()
	{
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0722: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_0737: Expected I4, but got Unknown
		//IL_075f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0762: Invalid comparison between Unknown and I4
		//IL_076b: Unknown result type (might be due to invalid IL or missing references)
		//IL_076f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0772: Invalid comparison between Unknown and I4
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("styleSheet");
		xmlTextWriter.WriteAttributeString("xmlns", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
		if (formatList.Count > 0)
		{
			xmlTextWriter.WriteStartElement("numFmts");
			xmlTextWriter.WriteAttributeString("count", $"{formatList.Count}");
			for (int i = 0; i < formatList.Count; i++)
			{
				xmlTextWriter.WriteStartElement("numFmt");
				xmlTextWriter.WriteAttributeString("numFmtId", $"{164 + i}");
				xmlTextWriter.WriteAttributeString("formatCode", (string)formatList[i]);
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteStartElement("fonts");
		xmlTextWriter.WriteAttributeString("count", $"{fontList.Count}");
		for (int j = 0; j < fontList.Count; j++)
		{
			DataFont dataFont = (DataFont)fontList[j];
			xmlTextWriter.WriteStartElement("font");
			if (dataFont.Bold)
			{
				xmlTextWriter.WriteElementString("b", "");
			}
			if (dataFont.Italic)
			{
				xmlTextWriter.WriteElementString("i", "");
			}
			if (dataFont.Underlined)
			{
				xmlTextWriter.WriteElementString("u", "");
			}
			if (dataFont.Strikeout)
			{
				xmlTextWriter.WriteElementString("strike", "");
			}
			xmlTextWriter.WriteStartElement("sz");
			xmlTextWriter.WriteAttributeString("val", $"{dataFont.Height}");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("color");
			xmlTextWriter.WriteAttributeString("rgb", $"{dataFont.Color.ToArgb():X8}");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("name");
			xmlTextWriter.WriteAttributeString("val", dataFont.Name);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("charset");
			xmlTextWriter.WriteAttributeString("val", $"{dataFont.Charset}");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("fills");
		xmlTextWriter.WriteAttributeString("count", $"{fillList.Count}");
		for (int k = 0; k < fillList.Count; k++)
		{
			DataFill dataFill = (DataFill)fillList[k];
			xmlTextWriter.WriteStartElement("fill");
			xmlTextWriter.WriteStartElement("patternFill");
			xmlTextWriter.WriteAttributeString("patternType", dataFill.Type);
			if (dataFill.Type == "solid")
			{
				xmlTextWriter.WriteStartElement("fgColor");
				xmlTextWriter.WriteAttributeString("rgb", dataFill.FgColor.ToArgb().ToString("X8"));
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("bgColor");
				xmlTextWriter.WriteAttributeString("indexed", $"{64}");
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("borders");
		xmlTextWriter.WriteAttributeString("count", $"{borderList.Count}");
		for (int l = 0; l < borderList.Count; l++)
		{
			DataBorder dataBorder = (DataBorder)borderList[l];
			xmlTextWriter.WriteStartElement("border");
			WriteBorderData(xmlTextWriter, "left", dataBorder.BorderLeft);
			WriteBorderData(xmlTextWriter, "right", dataBorder.BorderRight);
			WriteBorderData(xmlTextWriter, "top", dataBorder.BorderTop);
			WriteBorderData(xmlTextWriter, "bottom", dataBorder.BorderBottom);
			WriteBorderData(xmlTextWriter, "diagonal", null);
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("cellStyleXfs");
		xmlTextWriter.WriteAttributeString("count", $"{1}");
		xmlTextWriter.WriteStartElement("xf");
		xmlTextWriter.WriteAttributeString("numFmtId", $"{0}");
		xmlTextWriter.WriteAttributeString("fontId", $"{0}");
		xmlTextWriter.WriteAttributeString("fillId", $"{0}");
		xmlTextWriter.WriteAttributeString("borderId", $"{0}");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("cellXfs");
		xmlTextWriter.WriteAttributeString("count", $"{xfList.Count}");
		for (int m = 0; m < xfList.Count; m++)
		{
			DataXF dataXF = (DataXF)xfList[m];
			DataFill dataFill2 = (DataFill)fillList[dataXF.FillIndex];
			int num = dataXF.FillIndex;
			if (dataFill2.FgColor.A == 0)
			{
				num = 0;
			}
			xmlTextWriter.WriteStartElement("xf");
			xmlTextWriter.WriteAttributeString("numFmtId", $"{dataXF.FormatIndex}");
			xmlTextWriter.WriteAttributeString("fontId", $"{dataXF.FontIndex}");
			xmlTextWriter.WriteAttributeString("fillId", $"{num}");
			xmlTextWriter.WriteAttributeString("borderId", $"{dataXF.BorderIndex}");
			xmlTextWriter.WriteAttributeString("xfId", $"{dataXF.XFId}");
			if (dataXF.FormatIndex != 0)
			{
				xmlTextWriter.WriteAttributeString("applyNumberFormat", "1");
			}
			if (dataXF.FontIndex != 0)
			{
				xmlTextWriter.WriteAttributeString("applyFont", "1");
			}
			if (dataXF.FillIndex != 0)
			{
				xmlTextWriter.WriteAttributeString("applyFill", "1");
			}
			if (dataXF.BorderIndex != 0)
			{
				xmlTextWriter.WriteAttributeString("applyBorder", "1");
			}
			xmlTextWriter.WriteAttributeString("applyAlignment", "1");
			if (dataXF.Editable)
			{
				xmlTextWriter.WriteAttributeString("applyProtection", "1");
			}
			StiTextHorAlignment horAlign = dataXF.HorAlign;
			StiVertAlignment vertAlign = dataXF.VertAlign;
			float num2 = dataXF.TextRotationAngle;
			ConvertRotatedAlignment(num2, ref horAlign, ref vertAlign);
			xmlTextWriter.WriteStartElement("alignment");
			string value = "left";
			switch (horAlign - 1)
			{
			case 1:
				value = "right";
				break;
			case 0:
				value = "center";
				break;
			case 2:
				value = "justify";
				break;
			}
			xmlTextWriter.WriteAttributeString("horizontal", value);
			if ((int)vertAlign != 2)
			{
				string value2 = string.Empty;
				if ((int)vertAlign != 0)
				{
					if ((int)vertAlign == 1)
					{
						value2 = "center";
					}
				}
				else
				{
					value2 = "top";
				}
				xmlTextWriter.WriteAttributeString("vertical", value2);
			}
			if (num2 != 0f)
			{
				xmlTextWriter.WriteAttributeString("textRotation", num2.ToString());
			}
			if (dataXF.TextWrapped)
			{
				xmlTextWriter.WriteAttributeString("wrapText", "1");
			}
			if (dataXF.RightToLeft)
			{
				xmlTextWriter.WriteAttributeString("readingOrder", "2");
			}
			xmlTextWriter.WriteEndElement();
			if (dataXF.Editable)
			{
				xmlTextWriter.WriteStartElement("protection");
				xmlTextWriter.WriteAttributeString("locked", "0");
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("cellStyles");
		xmlTextWriter.WriteAttributeString("count", $"{1}");
		xmlTextWriter.WriteStartElement("cellStyle");
		xmlTextWriter.WriteAttributeString("name", "Normal");
		xmlTextWriter.WriteAttributeString("xfId", $"{0}");
		xmlTextWriter.WriteAttributeString("builtinId", $"{0}");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("dxfs");
		xmlTextWriter.WriteAttributeString("count", $"{0}");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("tableStyles");
		xmlTextWriter.WriteAttributeString("count", $"{0}");
		xmlTextWriter.WriteAttributeString("defaultTableStyle", "TableStyleMedium9");
		xmlTextWriter.WriteAttributeString("defaultPivotStyle", "PivotStyleLight16");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private void WriteBorderData(XmlTextWriter writer, string side, StiBorderSide border)
	{
		writer.WriteStartElement(side);
		if (border != (StiBorderSide)null)
		{
			string lineStyle = GetLineStyle(border);
			Color color = border.Color;
			if (lineStyle != string.Empty)
			{
				writer.WriteAttributeString("style", lineStyle);
				writer.WriteStartElement("color");
				writer.WriteAttributeString("rgb", color.ToArgb().ToString("X8"));
				writer.WriteEndElement();
			}
		}
		writer.WriteEndElement();
	}

	private MemoryStream WriteSST()
	{
		MemoryStream memoryStream = new StiCachedStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("sst");
		xmlTextWriter.WriteAttributeString("xmlns", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
		xmlTextWriter.WriteAttributeString("count", $"{sstCount}");
		xmlTextWriter.WriteAttributeString("uniqueCount", $"{sstList.Count}");
		for (int i = 0; i < sstList.Count; i++)
		{
			xmlTextWriter.WriteStartElement("si");
			string text = (string)sstList[i];
			StringBuilder stringBuilder = new StringBuilder();
			string text2 = text;
			foreach (char c in text2)
			{
				if (c >= ' ' || c == '\n' || c == '\t')
				{
					stringBuilder.Append(c);
				}
			}
			text = stringBuilder.ToString();
			if (sstHashIsTags.ContainsKey(i))
			{
				xmlTextWriter.WriteRaw(text);
			}
			else
			{
				ConvertTextToExcelString(xmlTextWriter, text);
			}
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteAdditionalData(string st, bool base64)
	{
		MemoryStream memoryStream = new MemoryStream();
		byte[] array = null;
		array = ((!base64) ? Encoding.ASCII.GetBytes(st) : System.Convert.FromBase64String(st));
		memoryStream.Write(array, 0, array.Length);
		return memoryStream;
	}

	private MemoryStream WriteImage(int number)
	{
		MemoryStream memoryStream = new MemoryStream();
		byte[] array = (byte[])imageCache.ImagePackedStore[number];
		memoryStream.Write(array, 0, array.Length);
		return memoryStream;
	}

	public void ExportExcel(StiReport report, string fileName)
	{
		FileStream fileStream = null;
		try
		{
			StiFileUtils.ProcessReadOnly(fileName);
			fileStream = new FileStream(fileName, FileMode.Create);
			ExportExcel(report, fileStream);
		}
		finally
		{
			if (fileStream != null)
			{
				fileStream.Flush();
				fileStream.Close();
			}
		}
	}

	public void ExportExcel(StiReport report, Stream stream)
	{
		ExportExcel(report, stream, new StiExcelExportSettings());
	}

	public void ExportExcel(StiReport report, Stream stream, StiPagesRange pageRange)
	{
		StiExcelExportSettings settings = new StiExcelExportSettings
		{
			PageRange = pageRange
		};
		ExportExcel(report, stream, settings);
	}

	public void ExportExcel(StiReport report, Stream stream, StiExcelExportSettings settings)
	{
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		StiLogService.Write(((object)this).GetType(), "Export report to Excel 2007 format");
		if (!report.IsDocument && report.GetCurrentPage() is IStiDashboard)
		{
			StiDashboardExport.Export(report, stream, settings);
			return;
		}
		string reportVersion = StiExportUtils.GetReportVersion();
		string reportVersion2 = StiExportUtils.GetReportVersion(report);
		if (settings == null)
		{
			throw new ArgumentNullException("The 'settings' argument cannot be equal in null.");
		}
		StiPagesRange pageRange = settings.PageRange;
		useOnePageHeaderAndFooter = settings.UseOnePageHeaderAndFooter;
		dataExportMode = settings.DataExportMode;
		exportObjectFormatting = settings.ExportObjectFormatting;
		exportEachPageToSheet = settings.ExportEachPageToSheet;
		exportHorizontalPageBreaks = settings.ExportPageBreaks;
		imageResolution = settings.ImageResolution;
		imageQuality = settings.ImageQuality;
		imageFormat = settings.ImageFormat;
		docCompanyString = ((settings.CompanyString == reportVersion) ? reportVersion2 : settings.CompanyString);
		docLastModifiedString = ((settings.LastModifiedString == reportVersion) ? reportVersion2 : settings.LastModifiedString);
		restrictEditing = settings.RestrictEditing;
		xmlIndentation = -1;
		if (imageResolution < 10f)
		{
			imageResolution = 10f;
		}
		imageResolution /= 100f;
		if (imageFormat != null && imageFormat != ImageFormat.Png)
		{
			imageFormat = ImageFormat.Jpeg;
		}
		if (dataExportMode != StiDataExportMode.AllBands)
		{
			useOnePageHeaderAndFooter = false;
		}
		else
		{
			exportObjectFormatting = true;
		}
		if (StiOptions.Export.Excel.RestrictEditing > restrictEditing)
		{
			restrictEditing = StiOptions.Export.Excel.RestrictEditing;
		}
		this.report = report;
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			reportCulture = currentCulture;
			if (!string.IsNullOrEmpty(report.Culture))
			{
				try
				{
					reportCulture = new CultureInfo(report.Culture);
				}
				catch
				{
				}
			}
			StiPagesCollection selectedPages = pageRange.GetSelectedPages(report.RenderedPages);
			if (base.IsStopped)
			{
				return;
			}
			base.CurrentPassNumber = 0;
			base.MaximumPassNumber = 3;
			base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
			PrepareData();
			for (int i = 0; i < selectedPages.Count; i++)
			{
				StiPagesCollection stiPagesCollection = new StiPagesCollection(report, report.RenderedPages)
				{
					CacheMode = report.RenderedPages.CacheMode
				};
				stiPagesCollection.AddV2Internal(selectedPages.GetPageWithoutCache(i));
				int num = i;
				if (!exportEachPageToSheet)
				{
					string excelSheetValue = selectedPages.GetPageWithoutCache(i).ExcelSheetValue;
					while (i < selectedPages.Count - 1 && CompareExcellSheetNames(selectedPages.GetPageWithoutCache(i + 1).ExcelSheetValue, excelSheetValue))
					{
						i++;
						stiPagesCollection.AddV2Internal(selectedPages.GetPageWithoutCache(i));
					}
				}
				string text = stiPagesCollection[0].ExcelSheetValue;
				if (text == null || text == string.Empty)
				{
					text = $"Page {sheetNameList.Count + 1}";
				}
				text = text.Replace("'", "");
				string text2 = string.Empty;
				int num2 = 1;
				PrepareMatrix(stiPagesCollection);
				int num3 = 0;
				do
				{
					firstPageIndexList.Add(num);
					matrixList.Add(matrix);
					int num4 = matrix.CoordY.Count - 1;
					if (num4 - num3 > MaximumSheetHeight)
					{
						num4 = num3 + MaximumSheetHeight;
					}
					else
					{
						matrix = null;
					}
					minRowList.Add(num3);
					maxRowList.Add(num4);
					num3 = num4;
					if (matrix != null || text2.Length > 0)
					{
						text2 = $" part{num2++}";
						if (text.Length > 24)
						{
							text = text.Substring(0, 24);
						}
					}
					else if (text.Length > 30)
					{
						text = text.Substring(0, 30);
					}
					sheetNameList.Add(text + text2);
					if (base.IsStopped)
					{
						return;
					}
				}
				while (matrix != null);
			}
			Hashtable hashtable = new Hashtable();
			for (int j = 0; j < sheetNameList.Count; j++)
			{
				string text3 = (string)sheetNameList[j];
				text3 = text3.Replace("*", "_").Replace("\\", "_").Replace("/", "_")
					.Replace("[", "_")
					.Replace("]", "_")
					.Replace(":", "_")
					.Replace("?", "_");
				if (hashtable.Contains(text3))
				{
					int num5 = 1;
					while (hashtable.Contains(text3 + "-" + num5))
					{
						num5++;
					}
					text3 = text3 + "-" + num5;
				}
				hashtable.Add(text3, text3);
				sheetNameList[j] = text3;
			}
			StiZipWriter20 val = new StiZipWriter20();
			val.Begin(stream, true);
			base.CurrentPassNumber = 2;
			StiLicenseKey licenseKey = StiLicenseKeyValidator.GetLicenseKey();
			bool flag = !StiLicenseKeyValidator.IsValidOnNetFramework(licenseKey);
			if (!typeof(StiLicense).AssemblyQualifiedName.Contains("PublicKeyToken=ebe6666cba19647a"))
			{
				flag = true;
			}
			if (!flag)
			{
				try
				{
					using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(512);
					using SHA1CryptoServiceProvider halg = new SHA1CryptoServiceProvider();
					rSACryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>2GCA4rfVor/2q7m+NVisL3ZFVvnklg14zeP0F+yNpBdoSm9iuOUBLodsOH6I3Xb0j5/OznYKFpii35RcjiPvww7Uthz1he2G8PGCvFMKuA4Wzgp5Kdv0uR7zSQ8J9lA/rZ7u8wh+h72umBku1pLFL2nvJIT56u4Qe7+OJFLrqWk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
					flag = !rSACryptoServiceProvider.VerifyData(licenseKey.GetCheckBytes(), halg, licenseKey.GetSignatureBytes());
				}
				catch (Exception)
				{
					flag = true;
				}
			}
			imageListOffset = 0;
			for (int k = 0; k < matrixList.Count; k++)
			{
				barCodeLinesList = new List<BarCodeLineData>();
				hyperlinkList = new ArrayList();
				val.AddFile($"xl/worksheets/sheet{k + 1}.xml", WriteSheet(k, selectedPages[(int)firstPageIndexList[k]]));
				if (hyperlinkList.Count > 0 || imageList.Count - imageListOffset > 0 || barCodeLinesList.Count > 0 || flag)
				{
					val.AddFile($"xl/worksheets/_rels/sheet{k + 1}.xml.rels", WriteSheetRels(k));
				}
				if (imageList.Count - imageListOffset > 0 || barCodeLinesList.Count > 0 || flag)
				{
					val.AddFile($"xl/drawings/_rels/drawing{k + 1}.xml.rels", WriteDrawingRels(k));
					val.AddFile($"xl/drawings/drawing{k + 1}.xml", WriteDrawing(k, flag ? selectedPages[(int)firstPageIndexList[k]] : null));
				}
				imageListOffset = imageList.Count;
				barCodeLinesList.Clear();
				barCodeLinesList = null;
				if (base.IsStopped)
				{
					return;
				}
			}
			val.AddFile("[Content_Types].xml", WriteContentTypes());
			val.AddFile("_rels/.rels", WriteMainRels());
			val.AddFile("docProps/app.xml", WriteDocPropsApp());
			val.AddFile("docProps/core.xml", WriteDocPropsCore());
			val.AddFile("xl/_rels/workbook.xml.rels", WriteWorkbookRels());
			val.AddFile("xl/workbook.xml", WriteWorkbook());
			val.AddFile("xl/styles.xml", WriteStyles());
			if (sstList.Count > 0)
			{
				val.AddFile("xl/sharedStrings.xml", WriteSST());
			}
			if (imageCache.ImagePackedStore.Count > 0)
			{
				for (int l = 0; l < imageCache.ImagePackedStore.Count; l++)
				{
					val.AddFile($"xl/media/image{l + 1:D5}.{GetImageFormatExtension(imageCache.ImageFormatStore[l])}", WriteImage(l));
				}
			}
			if (flag)
			{
				val.AddFile("xl/media/imageAdditional.png", WriteAdditionalData(StiExportUtils.AdditionalData, base64: true));
				val.AddFile("xl/drawings/_rels/vmlDrawingAdditional.vml.rels", WriteAdditionalData("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\"><Relationship Id=\"dId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/image\" Target=\"../media/imageAdditional.png\"/></Relationships>", base64: false));
				val.AddFile("xl/drawings/vmlDrawingAdditional.vml", WriteAdditionalData("<xml xmlns:v=\"urn:schemas-microsoft-com:vml\"\r\n xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n xmlns:x=\"urn:schemas-microsoft-com:office:excel\">\r\n <o:shapelayout v:ext=\"edit\">\r\n  <o:idmap v:ext=\"edit\" data=\"1\"/>\r\n </o:shapelayout><v:shapetype id=\"_x0000_t75\" coordsize=\"21600,21600\" o:spt=\"75\"\r\n  o:preferrelative=\"t\" path=\"m@4@5l@4@11@9@11@9@5xe\" filled=\"f\" stroked=\"f\">\r\n  <v:stroke joinstyle=\"miter\"/>\r\n  <v:formulas>\r\n   <v:f eqn=\"if lineDrawn pixelLineWidth 0\"/>\r\n   <v:f eqn=\"sum @0 1 0\"/>\r\n   <v:f eqn=\"sum 0 0 @1\"/>\r\n   <v:f eqn=\"prod @2 1 2\"/>\r\n   <v:f eqn=\"prod @3 21600 pixelWidth\"/>\r\n   <v:f eqn=\"prod @3 21600 pixelHeight\"/>\r\n   <v:f eqn=\"sum @0 0 1\"/>\r\n   <v:f eqn=\"prod @6 1 2\"/>\r\n   <v:f eqn=\"prod @7 21600 pixelWidth\"/>\r\n   <v:f eqn=\"sum @8 21600 0\"/>\r\n   <v:f eqn=\"prod @7 21600 pixelHeight\"/>\r\n   <v:f eqn=\"sum @10 21600 0\"/>\r\n  </v:formulas>\r\n  <v:path o:extrusionok=\"f\" gradientshapeok=\"t\" o:connecttype=\"rect\"/>\r\n  <o:lock v:ext=\"edit\" aspectratio=\"t\"/>\r\n </v:shapetype><v:shape id=\"CH\" o:spid=\"_x0000_s1027\" type=\"#_x0000_t75\"\r\n  style='position:absolute;margin-left:0;margin-top:0;width:365pt;height:298pt;\r\n  z-index:1'>\r\n  <v:imagedata o:relid=\"dId1\" o:title=\"additional\"/>\r\n  <o:lock v:ext=\"edit\" rotation=\"t\"/>\r\n </v:shape></xml>", base64: false));
			}
			val.End();
		}
		finally
		{
			StiExportUtils.EnableFontSmoothing(report);
			Thread.CurrentThread.CurrentCulture = currentCulture;
			if (matrix != null)
			{
				matrix.Clear();
				matrix = null;
			}
			fontList = null;
			fillList = null;
			borderList = null;
			xfList = null;
			sstList.Clear();
			sstList = null;
			sstHash.Clear();
			sstHash = null;
			sstHashIsTags.Clear();
			sstHashIsTags = null;
			formatList = null;
			sheetNameList = null;
			imageList.Clear();
			imageList = null;
			imageCache.Clear();
			printAreasList = null;
			matrixList = null;
			firstPageIndexList = null;
			hyperlinkList = null;
			minRowList = null;
			maxRowList = null;
			if (report.RenderedPages.CacheMode)
			{
				StiMatrix.GCCollect();
			}
		}
	}
}
