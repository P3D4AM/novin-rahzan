using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.Export.Tools;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Excel.png")]
public class StiExcelExportService : StiExportService
{
	private struct DataXF
	{
		public int FontIndex;

		public int FormatIndex;

		public int XFType;

		public int ParentStyleXF;

		public int HorAlign;

		public int VertAlign;

		public int TextWrapped;

		public int TextRotationAngle;

		public int TextDirection;

		public int UsedAttrib;

		public int LineStyleLeft;

		public int LineStyleRight;

		public int LineStyleTop;

		public int LineStyleBottom;

		public int ColorIndexLeft;

		public int ColorIndexRight;

		public int ColorIndexTop;

		public int ColorIndexBottom;

		public int FillPatern;

		public int ColorIndexPattern;

		public int ColorIndexBackground;

		public DataXF(int FontIndex, int FormatIndex, int XFType, int ParentStyleXF, int HorAlign, int VertAlign, int TextWrapped, int TextRotationAngle, int TextDirection, int UsedAttrib, int LineStyleLeft, int LineStyleRight, int LineStyleTop, int LineStyleBottom, int ColorIndexLeft, int ColorIndexRight, int ColorIndexTop, int ColorIndexBottom, int FillPatern, int ColorIndexPattern, int ColorIndexBackground)
		{
			this.FontIndex = FontIndex;
			this.FormatIndex = FormatIndex;
			this.XFType = XFType;
			this.ParentStyleXF = ParentStyleXF;
			this.HorAlign = HorAlign;
			this.VertAlign = VertAlign;
			this.TextWrapped = TextWrapped;
			this.TextRotationAngle = TextRotationAngle;
			this.TextDirection = TextDirection;
			this.UsedAttrib = UsedAttrib;
			this.LineStyleLeft = LineStyleLeft;
			this.LineStyleRight = LineStyleRight;
			this.LineStyleTop = LineStyleTop;
			this.LineStyleBottom = LineStyleBottom;
			this.ColorIndexLeft = ColorIndexLeft;
			this.ColorIndexRight = ColorIndexRight;
			this.ColorIndexTop = ColorIndexTop;
			this.ColorIndexBottom = ColorIndexBottom;
			this.FillPatern = FillPatern;
			this.ColorIndexPattern = ColorIndexPattern;
			this.ColorIndexBackground = ColorIndexBackground;
		}
	}

	private struct DataFont
	{
		public string Name;

		public bool Bold;

		public bool Italic;

		public bool Underlined;

		public bool Strikeout;

		public int Height;

		public int Color;

		public int Charset;

		public DataFont(string Name, bool Bold, bool Italic, bool Underlined, bool Strikeout, int Height, int Color, int Charset)
		{
			this.Name = Name;
			this.Bold = Bold;
			this.Italic = Italic;
			this.Underlined = Underlined;
			this.Strikeout = Strikeout;
			this.Height = Height;
			this.Color = Color;
			this.Charset = Charset;
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

	private class Escher
	{
		public struct BiffImageData
		{
			public ushort FirstRowIndex;

			public ushort FirstRowOffset;

			public ushort FirstColumnIndex;

			public ushort FirstColumnOffset;

			public ushort LastRowIndex;

			public ushort LastRowOffset;

			public ushort LastColumnIndex;

			public ushort LastColumnOffset;

			public byte[] ImageData;

			public BiffImageData(ushort FirstRowIndex, ushort FirstRowOffset, ushort FirstColumnIndex, ushort FirstColumnOffset, ushort LastRowIndex, ushort LastRowOffset, ushort LastColumnIndex, ushort LastColumnOffset, byte[] ImageData)
			{
				this.FirstRowIndex = FirstRowIndex;
				this.FirstRowOffset = FirstRowOffset;
				this.FirstColumnIndex = FirstColumnIndex;
				this.FirstColumnOffset = FirstColumnOffset;
				this.LastRowIndex = LastRowIndex;
				this.LastRowOffset = LastRowOffset;
				this.LastColumnIndex = LastColumnIndex;
				this.LastColumnOffset = LastColumnOffset;
				this.ImageData = ImageData;
			}
		}

		public MemoryStream mem;

		public int[] MemBookmarks;

		private int[] containerPos;

		private int containerNum;

		private byte[] buf;

		public Escher()
		{
			mem = new MemoryStream();
			containerPos = new int[10];
			containerNum = 0;
			buf = new byte[64];
		}

		public void WriteToStream(Stream stream)
		{
			mem.WriteTo(stream);
			mem.Close();
			stream.Flush();
		}

		private void dwByte(int position, byte dataValue)
		{
			buf[position] = dataValue;
		}

		private void dwWord(int position, ushort dataValue)
		{
			buf[position] = (byte)(dataValue & 0xFFu);
			buf[position + 1] = (byte)((uint)(dataValue >> 8) & 0xFFu);
		}

		private void dwDWord(int position, uint dataValue)
		{
			buf[position] = (byte)(dataValue & 0xFFu);
			buf[position + 1] = (byte)((dataValue >> 8) & 0xFFu);
			buf[position + 2] = (byte)((dataValue >> 16) & 0xFFu);
			buf[position + 3] = (byte)((dataValue >> 24) & 0xFFu);
		}

		private void dwByte(int position, byte dv1, byte dv2)
		{
			dwByte(position, dv1);
			dwByte(position + 1, dv2);
		}

		private void dwByte(int position, byte dv1, byte dv2, byte dv3)
		{
			dwByte(position, dv1, dv2);
			dwByte(position + 2, dv3);
		}

		private void dwByte(int position, byte dv1, byte dv2, byte dv3, byte dv4)
		{
			dwByte(position, dv1, dv2, dv3);
			dwByte(position + 3, dv4);
		}

		private void dwFill(int position, byte dataValue, int count)
		{
			for (int i = 0; i < count; i++)
			{
				buf[position + i] = dataValue;
			}
		}

		private void WriteRecordHeader(ushort recordId, byte ver, ushort inst)
		{
			ushort dataValue = (ushort)((uint)(inst << 4) | (ver & 0xFu));
			dwWord(0, dataValue);
			dwWord(2, recordId);
			dwDWord(4, 0u);
			mem.Write(buf, 0, 8);
			containerPos[containerNum] = (int)mem.Position;
			containerNum++;
		}

		private void CloseRecord()
		{
			long position = mem.Position;
			containerNum--;
			long num = containerPos[containerNum];
			mem.Seek(num - 4, SeekOrigin.Begin);
			dwDWord(0, (uint)(position - num));
			mem.Write(buf, 0, 4);
			mem.Seek(position, SeekOrigin.Begin);
		}

		public void WriteDGG(ArrayList imageList)
		{
			mem = new MemoryStream();
			containerNum = 0;
			uint count = (uint)imageList.Count;
			WriteRecordHeader(61440, 15, 0);
			WriteRecordHeader(61446, 0, 0);
			dwDWord(0, 1024 + count + 1024);
			dwDWord(4, 2u);
			dwDWord(8, count + 1);
			dwDWord(12, 1u);
			dwDWord(16, 1u);
			dwDWord(20, count + 1);
			mem.Write(buf, 0, 24);
			CloseRecord();
			WriteRecordHeader(61441, 15, (ushort)count);
			for (int i = 0; i < count; i++)
			{
				byte[] imageData = ((BiffImageData)imageList[i]).ImageData;
				uint adler32Checksum = StiExportUtils.GetAdler32Checksum(imageData);
				WriteRecordHeader(61447, 2, 5);
				dwByte(0, 5);
				dwByte(1, 5);
				dwFill(2, 0, 16);
				dwDWord(2, adler32Checksum);
				dwDWord(6, adler32Checksum);
				dwDWord(10, adler32Checksum);
				dwDWord(14, adler32Checksum);
				dwWord(18, 255);
				dwDWord(20, (uint)(imageData.Length + 25));
				dwDWord(24, 1u);
				dwDWord(28, 0u);
				dwByte(32, 0);
				dwByte(33, 0);
				dwByte(34, 0);
				dwByte(35, 0);
				mem.Write(buf, 0, 36);
				WriteRecordHeader(61469, 0, 1130);
				dwFill(0, 0, 16);
				dwDWord(0, adler32Checksum);
				dwDWord(4, adler32Checksum);
				dwDWord(8, adler32Checksum);
				dwDWord(12, adler32Checksum);
				dwByte(16, byte.MaxValue);
				mem.Write(buf, 0, 17);
				mem.Write(imageData, 0, imageData.Length);
				CloseRecord();
				CloseRecord();
			}
			CloseRecord();
			WriteRecordHeader(61451, 3, 3);
			dwWord(0, 191);
			dwDWord(2, 524296u);
			dwWord(6, 385);
			dwDWord(8, 134217793u);
			dwWord(12, 448);
			dwDWord(14, 134217792u);
			mem.Write(buf, 0, 18);
			CloseRecord();
			WriteRecordHeader(61726, 0, 4);
			dwDWord(0, 134217741u);
			dwDWord(4, 134217740u);
			dwDWord(8, 134217751u);
			dwDWord(12, 268435703u);
			mem.Write(buf, 0, 16);
			CloseRecord();
			CloseRecord();
		}

		public void WriteDG(ArrayList imageList, int startImageIndex)
		{
			mem = new MemoryStream();
			containerNum = 0;
			uint num = (uint)(imageList.Count - startImageIndex);
			MemBookmarks = new int[num + 1];
			WriteRecordHeader(61442, 15, 0);
			WriteRecordHeader(61448, 0, 1);
			dwDWord(0, num + 1);
			dwDWord(4, (ushort)(1024 + num));
			mem.Write(buf, 0, 8);
			CloseRecord();
			WriteRecordHeader(61443, 15, 0);
			WriteRecordHeader(61444, 15, 0);
			WriteRecordHeader(61449, 1, 0);
			dwFill(0, 0, 16);
			mem.Write(buf, 0, 16);
			CloseRecord();
			WriteRecordHeader(61450, 2, 0);
			dwDWord(0, 1024u);
			dwDWord(4, 5u);
			mem.Write(buf, 0, 8);
			CloseRecord();
			CloseRecord();
			for (int i = 0; i < num; i++)
			{
				MemBookmarks[i] = (int)mem.Position;
				WriteRecordHeader(61444, 15, 0);
				WriteRecordHeader(61450, 2, 75);
				dwDWord(0, (ushort)(1024 + startImageIndex + i + 1));
				dwDWord(4, 2560u);
				mem.Write(buf, 0, 8);
				CloseRecord();
				WriteRecordHeader(61451, 3, 2);
				dwWord(0, 16644);
				dwDWord(2, (uint)(startImageIndex + i + 1));
				string text = $"graph{startImageIndex + i + 1:X4}" + "\0";
				dwWord(6, 49413);
				dwDWord(8, (uint)(text.Length * 2));
				for (int j = 0; j < text.Length; j++)
				{
					dwWord(12 + j * 2, text[j]);
				}
				mem.Write(buf, 0, 12 + text.Length * 2);
				CloseRecord();
				BiffImageData biffImageData = (BiffImageData)imageList[startImageIndex + i];
				WriteRecordHeader(61456, 0, 0);
				dwWord(0, 2);
				dwWord(2, biffImageData.FirstColumnIndex);
				dwWord(4, biffImageData.FirstColumnOffset);
				dwWord(6, biffImageData.FirstRowIndex);
				dwWord(8, biffImageData.FirstRowOffset);
				dwWord(10, biffImageData.LastColumnIndex);
				dwWord(12, biffImageData.LastColumnOffset);
				dwWord(14, biffImageData.LastRowIndex);
				dwWord(16, biffImageData.LastRowOffset);
				mem.Write(buf, 0, 18);
				CloseRecord();
				WriteRecordHeader(61457, 0, 0);
				CloseRecord();
				CloseRecord();
			}
			CloseRecord();
			CloseRecord();
			MemBookmarks[0] = 0;
			MemBookmarks[num] = (int)mem.Position;
		}
	}

	private StiExportSettings exportSettings;

	private StiReport report;

	private string fileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	private StiMatrix matrix;

	private ArrayList xfList;

	private ArrayList colorList;

	private ArrayList fontList;

	private ArrayList sstList;

	private Hashtable sstHash;

	private ArrayList formatList;

	private int sstCounter;

	private byte[] bfHeader;

	private byte[] bfData;

	private MemoryStream memst;

	private MemoryStream memStMain;

	private ArrayList imageList;

	private Escher escher;

	private const int maxRecordLength = 8224;

	private bool useOnePageHeaderAndFooter;

	private StiDataExportMode dataExportMode = StiDataExportMode.DataAndHeaders;

	private bool exportObjectFormatting = true;

	private bool exportEachPageToSheet;

	private ArrayList pagesList;

	private ArrayList bookmarksList;

	private ArrayList hlinksList;

	private ArrayList boundsheetsOffsetsList;

	private ArrayList boundsheetsNamesList;

	private float imageResolution = 1f;

	private float imageQuality = 0.75f;

	private ImageCodecInfo imageCodec;

	private bool exportHorizontalPageBreaks;

	private int minRowIndex;

	private int maxRowIndex;

	private int maximumSheetHeight = 65534;

	private CultureInfo reportCulture;

	private static byte percentScaleForFontHeightCorrection = 100;

	private const double TwipsToColinfo = 2.2795300000000003;

	public override string DefaultExtension
	{
		get
		{
			if (exportSettings is StiExcel2007ExportSettings)
			{
				return "xlsx";
			}
			if (exportSettings is StiExcelExportSettings && (exportSettings as StiExcelExportSettings).ExcelType == StiExcelType.Excel2007)
			{
				return "xlsx";
			}
			return "xls";
		}
	}

	public override StiExportFormat ExportFormat
	{
		get
		{
			if (exportSettings is StiExcel2007ExportSettings)
			{
				return StiExportFormat.Excel2007;
			}
			if (exportSettings is StiExcelXmlExportSettings)
			{
				return StiExportFormat.ExcelXml;
			}
			if (exportSettings is StiExcelExportSettings)
			{
				switch ((exportSettings as StiExcelExportSettings).ExcelType)
				{
				case StiExcelType.Excel2007:
					return StiExportFormat.Excel2007;
				case StiExcelType.ExcelXml:
					return StiExportFormat.ExcelXml;
				}
			}
			return StiExportFormat.Excel;
		}
	}

	public override string GroupCategory => "Excel";

	public override int Position => 30;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeExcelFile");

	public override bool MultipleFiles => false;

	internal StiMatrix Matrix => matrix;

	public static byte PercentScaleForFontHeightCorrection
	{
		get
		{
			return percentScaleForFontHeightCorrection;
		}
		set
		{
			if (value >= 10)
			{
				percentScaleForFontHeightCorrection = value;
			}
		}
	}

	private static double HiToTwips => 1454.4 / (double)(int)PercentScaleForFontHeightCorrection;

	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		exportSettings = settings;
		StiExcelExportSettings stiExcelExportSettings = settings as StiExcelExportSettings;
		StiExcelType stiExcelType = stiExcelExportSettings?.ExcelType ?? StiExcelType.ExcelBinary;
		if (stiExcelType == StiExcelType.ExcelXml || stiExcelExportSettings is StiExcelXmlExportSettings)
		{
			StiExcelXmlExportService stiExcelXmlExportService = new StiExcelXmlExportService();
			stiExcelXmlExportService.ExportExcel(report, stream, stiExcelExportSettings as StiExcelXmlExportSettings);
		}
		else if (stiExcelType == StiExcelType.Excel2007 || stiExcelExportSettings is StiExcel2007ExportSettings)
		{
			StiExcel2007ExportService stiExcel2007ExportService = new StiExcel2007ExportService();
			stiExcel2007ExportService.ExportExcel(report, stream, stiExcelExportSettings as StiExcel2007ExportSettings);
		}
		else if (stiExcelType == StiExcelType.ExcelBinary)
		{
			StiExcelExportService stiExcelExportService = new StiExcelExportService();
			stiExcelExportService.ExportExcel(report, stream, stiExcelExportSettings);
		}
		else
		{
			StiExcel2007ExportService stiExcel2007ExportService2 = new StiExcel2007ExportService();
			stiExcel2007ExportService2.ExportExcel(report, stream, stiExcelExportSettings);
		}
		InvokeExporting(100, 100, 1, 1);
	}

	public override void Export(StiReport report, string fileName, bool sendEMail, StiGuiMode guiMode)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		IStiFormRunner exportFormRunner = StiGuiOptions.GetExportFormRunner("StiExcelSetupForm", guiMode, base.OwnerWindow);
		try
		{
			exportFormRunner["CurrentPage"] = report.CurrentPrintPage;
			exportFormRunner["OpenAfterExportEnabled"] = !sendEMail;
			if (!string.IsNullOrWhiteSpace(fileName))
			{
				exportFormRunner["ExportFormat"] = StiExportFormat.Excel;
			}
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
		return StiLocalization.Get("FileFilters", "ExcelFiles");
	}

	private void Form_Complete(IStiFormRunner form, StiShowDialogCompleteEvetArgs e)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		if (e.DialogResult)
		{
			switch ((StiExportFormat)form["ExportFormat"])
			{
			case StiExportFormat.Excel2007:
			{
				StiExcel2007ExportService stiExcel2007ExportService = new StiExcel2007ExportService();
				if (string.IsNullOrEmpty(fileName))
				{
					fileName = stiExcel2007ExportService.GetFileName(report, sendEMail);
				}
				if (fileName == null)
				{
					return;
				}
				StiFileUtils.ProcessReadOnly(fileName);
				try
				{
					using FileStream stream2 = new FileStream(fileName, FileMode.Create);
					stiExcel2007ExportService.StartProgress(guiMode);
					StiExcel2007ExportSettings stiExcel2007ExportSettings = new StiExcel2007ExportSettings();
					stiExcel2007ExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
					stiExcel2007ExportSettings.UseOnePageHeaderAndFooter = (bool)form["UseOnePageHeaderAndFooter"];
					stiExcel2007ExportSettings.DataExportMode = (StiDataExportMode)form["DataExportMode"];
					stiExcel2007ExportSettings.ExportObjectFormatting = (bool)form["ExportObjectFormatting"];
					stiExcel2007ExportSettings.ExportEachPageToSheet = (bool)form["ExportEachPageToSheet"];
					stiExcel2007ExportSettings.ExportPageBreaks = (bool)form["ExportPageBreaks"];
					stiExcel2007ExportSettings.ImageResolution = (float)form["Resolution"];
					stiExcel2007ExportSettings.ImageQuality = (float)form["ImageQuality"];
					stiExcel2007ExportSettings.RestrictEditing = (StiExcel2007RestrictEditing)form["RestrictEditing"];
					stiExcel2007ExportService.StartExport(settings: exportSettings = stiExcel2007ExportSettings, report: report, stream: stream2, sendEMail: sendEMail, openAfterExport: (bool)form["OpenAfterExport"], fileName: fileName, guiMode: guiMode);
					return;
				}
				catch (Exception ex2)
				{
					throw ex2;
				}
			}
			case StiExportFormat.ExcelXml:
			{
				StiExcelXmlExportService stiExcelXmlExportService = new StiExcelXmlExportService();
				if (string.IsNullOrEmpty(fileName))
				{
					fileName = stiExcelXmlExportService.GetFileName(report, sendEMail);
				}
				if (fileName == null)
				{
					return;
				}
				StiFileUtils.ProcessReadOnly(fileName);
				try
				{
					using FileStream stream = new FileStream(fileName, FileMode.Create);
					stiExcelXmlExportService.StartProgress(guiMode);
					StiExcelXmlExportSettings stiExcelXmlExportSettings = new StiExcelXmlExportSettings();
					stiExcelXmlExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
					stiExcelXmlExportService.StartExport(settings: exportSettings = stiExcelXmlExportSettings, report: report, stream: stream, sendEMail: sendEMail, openAfterExport: (bool)form["OpenAfterExport"], fileName: fileName, guiMode: guiMode);
					return;
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			}
			exportSettings = null;
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
				using FileStream stream3 = new FileStream(fileName, FileMode.Create);
				StartProgress(guiMode);
				StiExcelExportSettings stiExcelExportSettings = new StiExcelExportSettings();
				stiExcelExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
				stiExcelExportSettings.UseOnePageHeaderAndFooter = (bool)form["UseOnePageHeaderAndFooter"];
				stiExcelExportSettings.DataExportMode = (StiDataExportMode)form["DataExportMode"];
				stiExcelExportSettings.ExportObjectFormatting = (bool)form["ExportObjectFormatting"];
				stiExcelExportSettings.ExportEachPageToSheet = (bool)form["ExportEachPageToSheet"];
				stiExcelExportSettings.ExportPageBreaks = (bool)form["ExportPageBreaks"];
				stiExcelExportSettings.ImageResolution = (float)form["Resolution"];
				stiExcelExportSettings.ImageQuality = (float)form["ImageQuality"];
				StartExport(settings: exportSettings = stiExcelExportSettings, report: report, stream: stream3, sendEMail: sendEMail, openAfterExport: (bool)form["OpenAfterExport"], fileName: fileName, guiMode: guiMode);
				return;
			}
			catch (Exception ex3)
			{
				throw ex3;
			}
		}
		base.IsStopped = true;
	}

	private int GetLineStyle(StiBorderSide border)
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
				return 5;
			}
			if (num > 1.5)
			{
				return 2;
			}
			return 1;
		case 4:
			if (num > 1.5)
			{
				return 4;
			}
			return 7;
		case 1:
			if (num > 1.5)
			{
				return 8;
			}
			return 3;
		case 2:
			if (num > 1.5)
			{
				return 10;
			}
			return 9;
		case 3:
			if (num > 1.5)
			{
				return 12;
			}
			return 11;
		case 5:
			return 6;
		default:
			return 0;
		}
	}

	private string GetRefString(int column, int row)
	{
		int num = column / 26;
		int num2 = column % 26;
		StringBuilder stringBuilder = new StringBuilder();
		if (num > 0)
		{
			stringBuilder.Append((char)(65 + num - 1));
		}
		stringBuilder.Append((char)(65 + num2));
		stringBuilder.Append((row + 1).ToString());
		return stringBuilder.ToString();
	}

	private string GetRefAbsoluteString(int column, int row)
	{
		int num = column / 26;
		int num2 = column % 26;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("$");
		if (num > 0)
		{
			stringBuilder.Append((char)(65 + num - 1));
		}
		stringBuilder.Append((char)(65 + num2));
		stringBuilder.Append("$");
		stringBuilder.Append((row + 1).ToString());
		return stringBuilder.ToString();
	}

	private int GetXFNumber(DataXF dataIn)
	{
		if (xfList.Count > 16)
		{
			for (int i = 0; i < xfList.Count; i++)
			{
				if (object.Equals((DataXF)xfList[i], dataIn))
				{
					return i;
				}
			}
		}
		xfList.Add(dataIn);
		return xfList.Count - 1;
	}

	private int GetColorNumber(Color incomingColor)
	{
		if (colorList.Count > 0)
		{
			for (int i = 0; i < colorList.Count; i++)
			{
				if ((Color)colorList[i] == incomingColor)
				{
					return i + 8;
				}
			}
		}
		colorList.Add(incomingColor);
		int num = colorList.Count - 1;
		return num + 8;
	}

	private int GetFontNumber(DataFont dataIn)
	{
		if (fontList.Count > 4)
		{
			for (int i = 4; i < fontList.Count; i++)
			{
				if (object.Equals((DataFont)fontList[i], dataIn))
				{
					return i + 1;
				}
			}
		}
		fontList.Add(dataIn);
		int num = fontList.Count - 1;
		if (num >= 4)
		{
			num++;
		}
		return num;
	}

	private int GetSSTNumber(string dataIn)
	{
		sstCounter++;
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

	private void AddRec(int com, int len)
	{
		bfHeader[0] = (byte)((uint)com & 0xFFu);
		bfHeader[1] = (byte)((uint)(com >> 8) & 0xFFu);
		bfHeader[2] = (byte)((uint)len & 0xFFu);
		bfHeader[3] = (byte)((uint)(len >> 8) & 0xFFu);
		memst.Write(bfHeader, 0, 4);
		memst.Write(bfData, 0, len);
	}

	private void dwByte(int position, byte dataValue)
	{
		bfData[position] = dataValue;
	}

	private void dwWord(int position, short dataValue)
	{
		bfData[position] = (byte)((uint)dataValue & 0xFFu);
		bfData[position + 1] = (byte)((uint)(dataValue >> 8) & 0xFFu);
	}

	private void dwWord(int position, ushort dataValue)
	{
		bfData[position] = (byte)(dataValue & 0xFFu);
		bfData[position + 1] = (byte)((uint)(dataValue >> 8) & 0xFFu);
	}

	private void dwDWord(int position, int dataValue)
	{
		bfData[position] = (byte)((uint)dataValue & 0xFFu);
		bfData[position + 1] = (byte)((uint)(dataValue >> 8) & 0xFFu);
		bfData[position + 2] = (byte)((uint)(dataValue >> 16) & 0xFFu);
		bfData[position + 3] = (byte)((uint)(dataValue >> 24) & 0xFFu);
	}

	private void dwByte(int position, byte dv1, byte dv2)
	{
		dwByte(position, dv1);
		dwByte(position + 1, dv2);
	}

	private void dwByte(int position, byte dv1, byte dv2, byte dv3)
	{
		dwByte(position, dv1, dv2);
		dwByte(position + 2, dv3);
	}

	private void dwByte(int position, byte dv1, byte dv2, byte dv3, byte dv4)
	{
		dwByte(position, dv1, dv2, dv3);
		dwByte(position + 3, dv4);
	}

	private void dwByte(int position, byte dv1, byte dv2, byte dv3, byte dv4, byte dv5)
	{
		dwByte(position, dv1, dv2, dv3, dv4);
		dwByte(position + 4, dv5);
	}

	private void dwByte(int position, byte dv1, byte dv2, byte dv3, byte dv4, byte dv5, byte dv6)
	{
		dwByte(position, dv1, dv2, dv3, dv4, dv5);
		dwByte(position + 5, dv6);
	}

	private void dwByte(int position, byte dv1, byte dv2, byte dv3, byte dv4, byte dv5, byte dv6, byte dv7)
	{
		dwByte(position, dv1, dv2, dv3, dv4, dv5, dv6);
		dwByte(position + 6, dv7);
	}

	private void dwFill(int position, byte dataValue, int count)
	{
		for (int i = 0; i < count; i++)
		{
			bfData[position + i] = dataValue;
		}
	}

	private bool stringMayBePacked(string data)
	{
		bool result = true;
		for (int i = 0; i < data.Length; i++)
		{
			if (data[i] >= '\u0080')
			{
				result = false;
			}
		}
		return result;
	}

	private void dwString(int position, string dataValue, int bytesLen, out int offset, bool pack)
	{
		short num = (short)dataValue.Length;
		bool flag = stringMayBePacked(dataValue);
		if (!pack)
		{
			flag = false;
		}
		if (bytesLen != 0)
		{
			dwWord(position, num);
			position += 2;
			if (bytesLen == 1)
			{
				position--;
			}
			dwByte(position, (!flag) ? ((byte)1) : ((byte)0));
			position++;
		}
		for (int i = 0; i < num; i++)
		{
			if (flag)
			{
				dwByte(position, (byte)dataValue[i]);
				position++;
			}
			else
			{
				dwWord(position, (short)dataValue[i]);
				position += 2;
			}
		}
		offset = position;
	}

	private void dwDouble(int position, double dataValue)
	{
		byte[] bytes = BitConverter.GetBytes(dataValue);
		bytes.CopyTo(bfData, position);
	}

	private int Convert(double x)
	{
		return (int)(x * HiToTwips);
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

	private void PrepareData()
	{
		xfList = new ArrayList();
		colorList = new ArrayList();
		fontList = new ArrayList();
		sstList = new ArrayList();
		sstHash = new Hashtable();
		formatList = new ArrayList();
		imageList = new ArrayList();
		sstCounter = 0;
		pagesList = new ArrayList();
		bookmarksList = new ArrayList();
		hlinksList = new ArrayList();
		boundsheetsOffsetsList = new ArrayList();
		boundsheetsNamesList = new ArrayList();
		imageCodec = StiImageCodecInfo.GetImageCodec("image/jpeg");
		escher = new Escher();
		DataXF dataIn = new DataXF(0, 0, 1, 4095, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(1, 0, 1, 4095, 0, 2, 0, 0, 0, 61, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(1, 0, 1, 4095, 0, 2, 0, 0, 0, 61, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(2, 0, 1, 4095, 0, 2, 0, 0, 0, 61, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(2, 0, 1, 4095, 0, 2, 0, 0, 0, 61, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(0, 0, 1, 4095, 0, 2, 0, 0, 0, 61, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(0, 0, 1, 4095, 0, 2, 0, 0, 0, 61, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(0, 0, 1, 4095, 0, 2, 0, 0, 0, 61, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(0, 0, 1, 4095, 0, 2, 0, 0, 0, 61, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(0, 0, 1, 4095, 0, 2, 0, 0, 0, 61, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(0, 0, 1, 4095, 0, 2, 0, 0, 0, 61, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(0, 0, 1, 4095, 0, 2, 0, 0, 0, 61, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(0, 0, 1, 4095, 0, 2, 0, 0, 0, 61, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(0, 0, 1, 4095, 0, 2, 0, 0, 0, 61, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(0, 0, 1, 4095, 0, 2, 0, 0, 0, 61, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(1, 44, 1, 4095, 0, 2, 0, 0, 0, 62, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(1, 42, 1, 4095, 0, 2, 0, 0, 0, 62, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(1, 9, 1, 4095, 0, 2, 0, 0, 0, 62, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(1, 43, 1, 4095, 0, 2, 0, 0, 0, 62, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		dataIn = new DataXF(1, 41, 1, 4095, 0, 2, 0, 0, 0, 62, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 65);
		GetXFNumber(dataIn);
		DataFont dataIn2 = new DataFont("Arial", Bold: false, Italic: false, Underlined: false, Strikeout: false, 200, 32767, 1);
		GetFontNumber(dataIn2);
		GetFontNumber(dataIn2);
		GetFontNumber(dataIn2);
		GetFontNumber(dataIn2);
	}

	private Size MakeWorksheetStream(StiPagesCollection pages, int sheetIndex)
	{
		//IL_0c27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0660: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1459: Unknown result type (might be due to invalid IL or missing references)
		//IL_1460: Expected O, but got Unknown
		//IL_147a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1480: Expected O, but got Unknown
		//IL_1b53: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b58: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b77: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b73: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b95: Expected I4, but got Unknown
		//IL_1bc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bde: Expected I4, but got Unknown
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		ArrayList arrayList3 = new ArrayList();
		int count = imageList.Count;
		StiPage stiPage = pages[0];
		if (matrix == null)
		{
			StiDataExportMode stiDataExportMode = dataExportMode;
			if (StiOptions.Export.Excel.AllowExportFootersInDataOnlyMode)
			{
				stiDataExportMode |= StiDataExportMode.Footers;
			}
			matrix = new StiMatrix(pages, StiOptions.Export.Excel.DivideBigCells, this, null, stiDataExportMode);
			if (base.IsStopped)
			{
				return new Size(0, 0);
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
				for (int j = 0; j < Matrix.CoordY.Count - 1; j++)
				{
					if (Matrix.LinePlacement[j] == StiMatrix.StiTableLineInfo.Trash)
					{
						continue;
					}
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
									double num2 = (double)matrix.CoordY.GetByIndex(j) - (double)matrix.CoordY.GetByIndex(num);
									((RectangleD)(ref val)).Y = ((RectangleD)(ref val)).Y - num2;
									matrix.imagesBaseRect[component] = val;
								}
							}
						}
						matrix.BordersX[num, k] = matrix.BordersX[j, k];
						matrix.BordersY[num, k] = matrix.BordersY[j, k];
						matrix.Bookmarks[num, k] = matrix.Bookmarks[j, k];
					}
					matrix.BordersY[num, Matrix.CoordX.Count - 1] = matrix.BordersY[j, Matrix.CoordX.Count - 1];
					double num3 = (double)matrix.CoordY.GetByIndex(j + 1) - (double)matrix.CoordY.GetByIndex(j);
					matrix.CoordY.SetByIndex(num + 1, (double)matrix.CoordY.GetByIndex(num) + num3);
					num++;
				}
				for (int l = 0; l < Matrix.CoordX.Count - 1; l++)
				{
					matrix.BordersX[num, l] = matrix.BordersX[Matrix.CoordY.Count - 1, l];
				}
				int num4 = Matrix.CoordY.Count - 1 - num;
				if (num4 > 0)
				{
					for (int m = 0; m < num4; m++)
					{
						matrix.CoordY.RemoveAt(num + 1);
					}
				}
				matrix.AllowModification(flag: false);
			}
			if (dataExportMode != StiDataExportMode.AllBands)
			{
				matrix.ScanComponentsPlacement(optimize: true, exportObjectFormatting);
				matrix.AllowModification(flag: true);
				int num5 = 0;
				Hashtable hashtable = new Hashtable();
				string text = null;
				for (int n = 0; n < Matrix.CoordY.Count - 1; n++)
				{
					bool flag = false;
					if (Matrix.LinePlacement[n] == StiMatrix.StiTableLineInfo.HeaderD || Matrix.LinePlacement[n] == StiMatrix.StiTableLineInfo.HeaderAP)
					{
						string text2 = Matrix.ParentBandName[n];
						int num6 = text2.IndexOf('\u001f');
						if (num6 != -1)
						{
							string text3 = text2.Substring(0, num6);
							if (text3 != text)
							{
								text = text3;
								hashtable.Clear();
							}
						}
						if (!hashtable.ContainsKey(text2))
						{
							flag = true;
							hashtable.Add(text2, text2);
						}
					}
					if (!(Matrix.LinePlacement[n] == StiMatrix.StiTableLineInfo.Data || Matrix.LinePlacement[n] == StiMatrix.StiTableLineInfo.FooterD || flag))
					{
						continue;
					}
					for (int num7 = 0; num7 < Matrix.CoordX.Count - 1; num7++)
					{
						matrix.Cells[num5, num7] = matrix.Cells[n, num7];
						StiCell stiCell2 = matrix.Cells[num5, num7];
						if (stiCell2 != null)
						{
							stiCell2.Top = num5;
							if (stiCell2.ExportImage != null)
							{
								StiComponent component2 = stiCell2.Component;
								object obj2 = matrix.imagesBaseRect[component2];
								if (obj2 != null)
								{
									RectangleD val2 = (RectangleD)obj2;
									double num8 = (double)matrix.CoordY.GetByIndex(n) - (double)matrix.CoordY.GetByIndex(num5);
									((RectangleD)(ref val2)).Y = ((RectangleD)(ref val2)).Y - num8;
									matrix.imagesBaseRect[component2] = val2;
								}
							}
						}
						if (num5 == 0 || matrix.BordersX[n, num7] != (StiBorderSide)null)
						{
							matrix.BordersX[num5, num7] = matrix.BordersX[n, num7];
						}
						matrix.BordersX[num5 + 1, num7] = matrix.BordersX[n + 1, num7];
						matrix.BordersY[num5, num7] = matrix.BordersY[n, num7];
						matrix.Bookmarks[num5, num7] = matrix.Bookmarks[n, num7];
					}
					matrix.BordersY[num5, Matrix.CoordX.Count - 1] = matrix.BordersY[n, Matrix.CoordX.Count - 1];
					double num9 = (double)matrix.CoordY.GetByIndex(n + 1) - (double)matrix.CoordY.GetByIndex(n);
					matrix.CoordY.SetByIndex(num5 + 1, (double)matrix.CoordY.GetByIndex(num5) + num9);
					num5++;
				}
				int num10 = Matrix.CoordY.Count - 1 - num5;
				if (num10 > 0)
				{
					for (int num11 = 0; num11 < num10; num11++)
					{
						matrix.CoordY.RemoveAt(num5 + 1);
					}
				}
				matrix.AllowModification(flag: false);
			}
			minRowIndex = 1;
		}
		maxRowIndex = Matrix.CoordY.Count;
		if (maxRowIndex - minRowIndex > maximumSheetHeight)
		{
			maxRowIndex = minRowIndex + maximumSheetHeight;
		}
		dwFill(0, 0, 16);
		dwWord(0, 1536);
		dwWord(2, 16);
		dwWord(4, 3910);
		dwWord(6, 1996);
		dwDWord(8, 32969);
		dwDWord(12, 6);
		AddRec(2057, 16);
		dwWord(0, 1);
		AddRec(13, 2);
		dwWord(0, 100);
		AddRec(12, 2);
		dwWord(0, 1);
		AddRec(15, 2);
		dwWord(0, 0);
		AddRec(17, 2);
		dwDouble(0, 0.001);
		AddRec(16, 8);
		dwWord(0, 0);
		AddRec(95, 2);
		dwWord(0, 0);
		AddRec(42, 2);
		dwWord(0, 0);
		AddRec(43, 2);
		dwWord(0, 1);
		AddRec(130, 2);
		dwWord(0, 0);
		dwWord(2, 0);
		dwWord(4, 0);
		dwWord(6, 0);
		AddRec(128, 8);
		dwWord(0, 0);
		dwWord(2, 255);
		AddRec(549, 4);
		dwWord(0, 1217);
		AddRec(129, 2);
		int num12 = Matrix.HorizontalPageBreaks.Count;
		if (exportHorizontalPageBreaks && num12 > 0)
		{
			if (num12 > 320)
			{
				num12 = 320;
			}
			short dataValue = 0;
			short dataValue2 = (short)(Matrix.CoordX.Count - 2);
			dwWord(0, (short)num12);
			for (short num13 = 0; num13 < num12; num13++)
			{
				dwWord(2 + num13 * 6, (short)Matrix.HorizontalPageBreaks[num13]);
				dwWord(2 + num13 * 6 + 2, dataValue);
				dwWord(2 + num13 * 6 + 4, dataValue2);
			}
			AddRec(27, num12 * 6 + 2);
		}
		AddRec(20, 0);
		AddRec(21, 0);
		dwWord(0, 0);
		AddRec(131, 2);
		dwWord(0, 0);
		AddRec(132, 2);
		dwDouble(0, stiPage.Unit.ConvertToHInches(stiPage.Margins.Top) / 100.0);
		AddRec(40, 8);
		dwDouble(0, stiPage.Unit.ConvertToHInches(stiPage.Margins.Bottom) / 100.0);
		AddRec(41, 8);
		dwDouble(0, stiPage.Unit.ConvertToHInches(stiPage.Margins.Left) / 100.0);
		AddRec(38, 8);
		dwDouble(0, stiPage.Unit.ConvertToHInches(stiPage.Margins.Right) / 100.0);
		AddRec(39, 8);
		dwWord(0, (short)stiPage.PaperSize);
		dwWord(2, PercentScaleForFontHeightCorrection);
		dwWord(4, 1);
		dwWord(6, 1);
		dwWord(8, 1);
		int num14 = 0 | ((stiPage.Orientation == StiPageOrientation.Portrait) ? 2 : 0);
		dwWord(10, (short)num14);
		dwWord(12, 0);
		dwWord(14, 0);
		dwDouble(16, 0.0);
		dwDouble(24, 0.0);
		dwWord(32, 1);
		AddRec(161, 34);
		dwWord(0, 8);
		AddRec(85, 2);
		if (exportObjectFormatting)
		{
			double num15 = (double)Matrix.CoordX.GetByIndex(0);
			int num16 = 0;
			for (int num17 = 1; num17 < Matrix.CoordX.Count; num17++)
			{
				double num18 = (double)Matrix.CoordX.GetByIndex(num17);
				double a = (num18 - num15) * HiToTwips * 2.2795300000000003;
				short num19 = (short)(Math.Round(a) - (double)num16);
				num16 += num19;
				dwWord(0, (short)(num17 - 1));
				dwWord(2, (short)(num17 - 1));
				dwWord(4, num19);
				dwWord(6, 15);
				dwWord(8, 0);
				dwWord(10, 0);
				AddRec(125, 12);
			}
		}
		dwFill(0, 0, 14);
		dwDWord(0, 0);
		dwDWord(4, maxRowIndex - minRowIndex);
		dwWord(8, 0);
		dwWord(10, (short)Matrix.CoordX.Count);
		AddRec(512, 14);
		RichTextBox val3 = null;
		bool[,] array = new bool[maxRowIndex, Matrix.CoordX.Count];
		int[,] array2 = new int[maxRowIndex + 1, Matrix.CoordX.Count];
		base.CurrentPassNumber = 2;
		double num20 = Math.Max((float)Matrix.CoordY.Count / 200f, 1f);
		int num21 = 0;
		for (int num22 = minRowIndex; num22 < maxRowIndex; num22++)
		{
			int num23 = (int)((double)num22 / num20);
			if (num23 > num21)
			{
				num21 = num23;
				InvokeExporting(num22, Matrix.CoordY.Count, base.CurrentPassNumber, base.MaximumPassNumber);
			}
			if ((num22 - minRowIndex) % 32 == 0)
			{
				for (int num24 = 0; num24 < 32; num24++)
				{
					int num25 = num22 - 1 + num24;
					if (num25 < maxRowIndex - 1)
					{
						double x = (double)Matrix.CoordY.GetByIndex(num25 + 1) - (double)Matrix.CoordY.GetByIndex(num25);
						dwFill(0, 0, 16);
						dwWord(0, (short)(num25 - (minRowIndex - 1)));
						dwWord(2, 0);
						dwWord(4, (short)Matrix.CoordX.Count);
						if (exportObjectFormatting)
						{
							dwWord(6, (short)Convert(x));
							dwDWord(12, 983360);
						}
						else
						{
							dwDWord(6, 255);
							dwDWord(12, 983296);
						}
						AddRec(520, 16);
					}
				}
			}
			for (int num26 = 1; num26 < Matrix.CoordX.Count; num26++)
			{
				StiCell stiCell3 = Matrix.Cells[num22 - 1, num26 - 1];
				DataXF dataIn2;
				if (!array[num22 - 1, num26 - 1] && stiCell3 != null)
				{
					array[num22 - 1, num26 - 1] = true;
					StiRichText stiRichText = stiCell3.Component as StiRichText;
					StiText stiText = stiCell3.Component as StiText;
					string text4 = stiCell3.Text;
					if (stiRichText != null && stiRichText.RtfText != string.Empty)
					{
						if (val3 == null)
						{
							val3 = (RichTextBox)(object)new StiRichTextBox(drawTransparent: false);
						}
						stiRichText.GetPreparedText(val3);
						text4 = ((Control)val3).Text;
					}
					StiCheckBox stiCheckBox = stiCell3.Component as StiCheckBox;
					bool flag2 = false;
					if (stiCheckBox != null && stiCheckBox.ExcelDataValue != null && stiCheckBox.ExcelDataValue.Length > 0)
					{
						flag2 = true;
						text4 = stiCheckBox.ExcelDataValue;
					}
					if (stiCell3.Component.HyperlinkValue != null)
					{
						string text5 = stiCell3.Component.HyperlinkValue.ToString().Trim();
						if (text5.Length > 0 && !text5.StartsWith("javascript:"))
						{
							string text6 = text4;
							if (text6 == null || text6.Length == 0)
							{
								text6 = text5;
							}
							if (text5.StartsWith("#", StringComparison.InvariantCulture))
							{
								text5 = text5.Substring(1);
								CellRangeAddress range = new CellRangeAddress(num22 - minRowIndex, num22 - minRowIndex + stiCell3.Height, num26 - 1, num26 - 1 + stiCell3.Width);
								HlinkData hlinkData = new HlinkData(range, text6, text5);
								arrayList3.Add(hlinkData);
							}
							else
							{
								dwWord(0, (short)(num22 - minRowIndex));
								dwWord(2, (short)(num22 - minRowIndex + stiCell3.Height));
								dwWord(4, (short)(num26 - 1));
								dwWord(6, (short)(num26 - 1 + stiCell3.Width));
								dwByte(8, 208, 201, 234, 121);
								dwByte(12, 249, 186, 206, 17);
								dwByte(16, 140, 130, 0, 170);
								dwByte(20, 0, 75, 169, 11);
								dwDWord(24, 2);
								dwDWord(28, 23);
								int num27 = 32;
								dwDWord(num27, text6.Length + 1);
								num27 += 4;
								for (int num28 = 0; num28 < text6.Length; num28++)
								{
									dwWord(num27, text6[num28]);
									num27 += 2;
								}
								dwWord(num27, 0);
								num27 += 2;
								dwByte(num27, 224, 201, 234, 121);
								dwByte(num27 + 4, 249, 186, 206, 17);
								dwByte(num27 + 8, 140, 130, 0, 170);
								dwByte(num27 + 12, 0, 75, 169, 11);
								num27 += 16;
								dwDWord(num27, (text5.Length + 1) * 2);
								num27 += 4;
								for (int num29 = 0; num29 < text5.Length; num29++)
								{
									dwWord(num27, text5[num29]);
									num27 += 2;
								}
								dwWord(num27, 0);
								num27 += 2;
								byte[] array3 = new byte[num27];
								Array.Copy(bfData, 0, array3, 0, num27);
								arrayList2.Add(array3);
							}
						}
					}
					if (stiCell3.Component is IStiExportImage stiExportImage && imageList.Count < 1024)
					{
						IStiExportImageExtended stiExportImageExtended = stiExportImage as IStiExportImageExtended;
						float zoom = imageResolution;
						StiImage stiImage = stiExportImage as StiImage;
						if (StiOptions.Export.Excel.UseImageResolution && stiImage != null && stiImage.ExistImageToDraw())
						{
							Image val4 = stiImage.TakeGdiImageToDraw();
							try
							{
								if (val4 != null)
								{
									float horizontalResolution = val4.HorizontalResolution;
									if (horizontalResolution >= 50f && horizontalResolution <= 600f)
									{
										zoom *= horizontalResolution / 100f;
									}
								}
							}
							finally
							{
								((IDisposable)val4)?.Dispose();
							}
						}
						Image val5 = null;
						if (stiCell3.Component.IsExportAsImage(StiExportFormat.Excel))
						{
							val5 = ((stiExportImageExtended == null || !stiExportImageExtended.IsExportAsImage(StiExportFormat.Excel)) ? stiExportImage.GetImage(ref zoom) : stiExportImageExtended.GetImage(ref zoom, StiExportFormat.Excel));
						}
						if (val5 != null)
						{
							Image realImageData = Matrix.GetRealImageData(stiCell3, val5);
							if (realImageData != null)
							{
								val5 = realImageData;
							}
							MemoryStream memoryStream = new MemoryStream();
							if (imageCodec == null)
							{
								val5.Save((Stream)memoryStream, ImageFormat.Jpeg);
							}
							else
							{
								EncoderParameters val6 = new EncoderParameters(1);
								val6.Param[0] = new EncoderParameter(Encoder.Quality, (long)(imageQuality * 100f));
								val5.Save((Stream)memoryStream, imageCodec, val6);
							}
							byte[] imageData = memoryStream.ToArray();
							Escher.BiffImageData biffImageData = new Escher.BiffImageData((ushort)(num22 - minRowIndex), 0, (ushort)(num26 - 1), 0, (ushort)(num22 - (minRowIndex - 1) + stiCell3.Height), 0, (ushort)(num26 + stiCell3.Width), 0, imageData);
							imageList.Add(biffImageData);
							val5.Dispose();
						}
					}
					bool flag3 = false;
					if (stiCell3.Component != null && stiCell3.Component.TagValue != null)
					{
						string text7 = stiCell3.Component.TagValue.ToString().ToLower();
						if (text7.IndexOf("exceltext", StringComparison.InvariantCulture) != -1)
						{
							flag3 = true;
						}
					}
					string text8 = string.Empty;
					if (stiText != null)
					{
						text8 = stiText.Format;
					}
					bool flag4 = false;
					bool flag5 = false;
					bool flag6 = false;
					bool flag7 = false;
					bool flag8 = false;
					bool flag9 = false;
					bool flag10 = false;
					string text9 = string.Empty;
					int num30 = 2;
					int num31 = 0;
					string text10 = "$";
					bool flag11 = false;
					bool flag12 = false;
					bool flag13 = stiText?.HideZeros ?? false;
					string text11 = null;
					string text12 = null;
					int num32 = text8?.IndexOf("|") ?? (-1);
					if (num32 != -1)
					{
						text11 = StiExportUtils.GetPositivePattern(text8[num32 + 1] - 65);
						text12 = StiExportUtils.GetNegativePattern(text8[num32 + 2] - 65);
						text8 = text8.Substring(0, num32);
					}
					if (text8 != null && text8.Length > 0)
					{
						if (text8[0] == 'C')
						{
							flag4 = true;
						}
						else if (text8[0] == 'N')
						{
							flag5 = true;
						}
						else if (text8[0] == 'P')
						{
							flag6 = true;
						}
						else if (text8[0] == 'D')
						{
							flag7 = true;
						}
						else if (text8[0] == 'T')
						{
							flag8 = true;
						}
						else if (text8[0] == 'U')
						{
							flag9 = true;
						}
						if (text8.Length == 1)
						{
							flag10 = true;
						}
						else if (flag4 || flag5 || flag6)
						{
							int num33 = 1;
							if (char.IsDigit(text8[num33]))
							{
								StringBuilder stringBuilder = new StringBuilder();
								for (; num33 < text8.Length && char.IsDigit(text8[num33]); num33++)
								{
									stringBuilder.Append(text8[num33]);
								}
								num30 = int.Parse(stringBuilder.ToString());
							}
							if (num33 < text8.Length && text8[num33] == 'G')
							{
								num33++;
								num31 = 3;
							}
							if (num33 < text8.Length && text8[num33] == '(')
							{
								num33++;
								flag12 = true;
							}
							if (num33 < text8.Length && (text8[num33] == '.' || text8[num33] == ','))
							{
								num33++;
							}
							if (num33 < text8.Length && (text8[num33] == '+' || text8[num33] == '-'))
							{
								if (text8[num33] == '+')
								{
									flag11 = true;
								}
								num33++;
								if (num33 < text8.Length)
								{
									text10 = text8.Substring(num33);
								}
							}
						}
					}
					if (!(flag4 || flag5 || flag6))
					{
						flag10 = true;
					}
					if (flag13 && text8 != null && text8.Length > 0)
					{
						flag10 = false;
					}
					if (flag3)
					{
						flag10 = true;
					}
					if (!flag10)
					{
						if (num32 != -1)
						{
							StringBuilder stringBuilder2 = new StringBuilder();
							if (num31 > 1)
							{
								stringBuilder2.Append("#,");
								stringBuilder2.Append('#', num31 - 1);
							}
							stringBuilder2.Append('0');
							if (num30 > 0)
							{
								stringBuilder2.Append(".");
								stringBuilder2.Append('0', num30);
							}
							string newValue = stringBuilder2.ToString();
							string text13 = text11.Replace("n", newValue).Replace("$", "\"" + text10 + "\"");
							string text14 = text12.Replace("n", newValue).Replace("$", "\"" + text10 + "\"");
							text9 = text13 + ";" + text14 + (flag13 ? ";" : "");
						}
						else
						{
							StringBuilder stringBuilder3 = new StringBuilder();
							if (flag4 && flag11)
							{
								stringBuilder3.Append("\"");
								stringBuilder3.Append(text10);
								stringBuilder3.Append("\"");
							}
							if (num31 > 1)
							{
								stringBuilder3.Append("#,");
								stringBuilder3.Append('#', num31 - 1);
							}
							stringBuilder3.Append('0');
							if (num30 > 0)
							{
								stringBuilder3.Append(".");
								stringBuilder3.Append('0', num30);
							}
							if (flag4 && !flag11)
							{
								stringBuilder3.Append("\"");
								stringBuilder3.Append(text10);
								stringBuilder3.Append("\"");
							}
							if (flag6)
							{
								stringBuilder3.Append("%");
							}
							text9 = stringBuilder3.ToString();
							string text15 = (flag12 ? "(" : "-") + text9 + (flag12 ? ")" : "");
							if (stiText != null && stiText.HideZeros)
							{
								text9 = text9 + ";" + text15 + ";";
							}
							else if (flag12)
							{
								text9 = text9 + ";" + text15;
							}
						}
					}
					int formatIndex = 0;
					if (flag10)
					{
						if (flag5)
						{
							formatIndex = 4;
						}
						if (flag4)
						{
							formatIndex = 7;
						}
						if (flag6)
						{
							formatIndex = 10;
						}
						if (flag7)
						{
							formatIndex = 14;
						}
						if (flag8)
						{
							formatIndex = 21;
						}
						if (flag3)
						{
							formatIndex = 49;
						}
					}
					else
					{
						formatIndex = GetFormatNumber(text9);
					}
					StiCellStyle cellStyle = stiCell3.CellStyle;
					DataFont dataIn = new DataFont(cellStyle.Font.Name, cellStyle.Font.Bold, cellStyle.Font.Italic, cellStyle.Font.Underline, cellStyle.Font.Strikeout, (int)(cellStyle.Font.SizeInPoints * 20f), GetColorNumber(cellStyle.TextColor), cellStyle.Font.GdiCharSet);
					if (flag2)
					{
						dataIn = new DataFont(StiOptions.Export.CheckBoxReplacementForExcelValue.Font.Name, StiOptions.Export.CheckBoxReplacementForExcelValue.Font.Bold, StiOptions.Export.CheckBoxReplacementForExcelValue.Font.Italic, StiOptions.Export.CheckBoxReplacementForExcelValue.Font.Underline, StiOptions.Export.CheckBoxReplacementForExcelValue.Font.Strikeout, (int)(StiOptions.Export.CheckBoxReplacementForExcelValue.Font.SizeInPoints * 20f), GetColorNumber(cellStyle.TextColor), StiOptions.Export.CheckBoxReplacementForExcelValue.Font.GdiCharSet);
					}
					bool flag14 = false;
					if (cellStyle.TextOptions != null)
					{
						flag14 = cellStyle.TextOptions.RightToLeft;
					}
					int num34 = 0;
					if (cellStyle.TextOptions != null)
					{
						int num35 = (int)(cellStyle.TextOptions.Angle % 360f);
						if (num35 < 0)
						{
							num35 = 360 + num35;
						}
						if (num35 >= 0 && num35 <= 90)
						{
							num34 = num35;
						}
						if (num35 >= 270 && num35 < 360)
						{
							num34 = 360 - num35 + 90;
						}
					}
					StiTextHorAlignment horAlign = cellStyle.HorAlignment;
					StiVertAlignment vertAlign = cellStyle.VertAlignment;
					StiExcel2007ExportService.ConvertRotatedAlignment(num34, ref horAlign, ref vertAlign);
					StiTextHorAlignment val7 = (flag2 ? StiOptions.Export.CheckBoxReplacementForExcelValue.HorAlignment : horAlign);
					int horAlign2 = (int)val7 switch
					{
						0 => (!flag14) ? 1 : 3, 
						2 => flag14 ? 1 : 3, 
						1 => 2, 
						3 => 7, 
						_ => 0, 
					};
					StiVertAlignment val8 = (flag2 ? StiOptions.Export.CheckBoxReplacementForExcelValue.VertAlignment : vertAlign);
					int vertAlign2 = (int)val8 switch
					{
						0 => 0, 
						1 => 1, 
						2 => 2, 
						_ => 0, 
					};
					int textWrapped = 0;
					bool flag15 = false;
					if (cellStyle.TextOptions != null)
					{
						flag15 = cellStyle.TextOptions.WordWrap;
					}
					if (text4 != null && text4.Length > 0 && (text4.IndexOf("\r", StringComparison.InvariantCulture) != -1 || text4.IndexOf("\n", StringComparison.InvariantCulture) != -1))
					{
						flag15 = true;
					}
					if (flag15)
					{
						textWrapped = 1;
					}
					bool flag16 = true;
					bool flag17 = true;
					for (int num36 = 0; num36 < stiCell3.Height + 1; num36++)
					{
						if (matrix.BordersY[stiCell3.Top + num36, stiCell3.Left] == (StiBorderSide)null)
						{
							flag16 = false;
						}
						if (matrix.BordersY[stiCell3.Top + num36, stiCell3.Left + stiCell3.Width + 1] == (StiBorderSide)null)
						{
							flag17 = false;
						}
					}
					bool flag18 = true;
					bool flag19 = true;
					for (int num37 = 0; num37 < stiCell3.Width + 1; num37++)
					{
						if (matrix.BordersX[stiCell3.Top, stiCell3.Left + num37] == (StiBorderSide)null)
						{
							flag18 = false;
						}
						if (matrix.BordersX[stiCell3.Top + stiCell3.Height + 1, stiCell3.Left + num37] == (StiBorderSide)null)
						{
							flag19 = false;
						}
					}
					dataIn2 = new DataXF(GetFontNumber(dataIn), formatIndex, 0, 0, horAlign2, vertAlign2, textWrapped, num34, (!flag14) ? 1 : 2, 63, GetLineStyle(flag16 ? matrix.BordersY[num22 - 1, num26 - 1] : null), GetLineStyle(flag17 ? matrix.BordersY[num22 - 1, num26 - 1 + stiCell3.Width + 1] : null), GetLineStyle(flag18 ? matrix.BordersX[num22 - 1, num26 - 1] : null), GetLineStyle(flag19 ? matrix.BordersX[num22 - 1 + stiCell3.Height + 1, num26 - 1] : null), flag16 ? GetColorNumber(matrix.BordersY[num22 - 1, num26 - 1].Color) : 0, flag17 ? GetColorNumber(matrix.BordersY[num22 - 1, num26 - 1 + stiCell3.Width + 1].Color) : 0, flag18 ? GetColorNumber(matrix.BordersX[num22 - 1, num26 - 1].Color) : 0, flag19 ? GetColorNumber(matrix.BordersX[num22 - 1 + stiCell3.Height + 1, num26 - 1].Color) : 0, (cellStyle.Color.A != 0 && cellStyle.Color != Color.White) ? 1 : 0, GetColorNumber(cellStyle.Color), GetColorNumber(cellStyle.Color));
					int num38 = GetXFNumber(dataIn2);
					if (!exportObjectFormatting)
					{
						num38 = 15;
					}
					if (exportObjectFormatting)
					{
						for (int num39 = 0; num39 <= stiCell3.Width; num39++)
						{
							for (int num40 = 0; num40 <= stiCell3.Height; num40++)
							{
								array[num22 - 1 + num40, num26 - 1 + num39] = true;
								array2[num22 + num40, num26 + num39] = num38;
							}
						}
						array2[num22, num26] = 0;
						if (stiCell3.Width > 0 || stiCell3.Height > 0)
						{
							CellRangeAddress cellRangeAddress = new CellRangeAddress(num22 - minRowIndex, num22 + stiCell3.Height - minRowIndex, num26 - 1, num26 + stiCell3.Width - 1);
							arrayList.Add(cellRangeAddress);
						}
					}
					else
					{
						array[num22 - 1, num26 - 1] = true;
						array2[num22, num26] = 0;
					}
					string text16 = null;
					if (stiText != null)
					{
						text16 = stiText.ExcelDataValue;
						if (flag5 && text16 == "" && text4 == " ")
						{
							text4 = "";
						}
					}
					if (!stiCell3.Component.IsExportAsImage(StiExportFormat.Excel) && (!string.IsNullOrEmpty(text4) || (!string.IsNullOrEmpty(text16) && text16 != "-")))
					{
						bool flag20 = false;
						double result = 0.0;
						if (!string.IsNullOrEmpty(text16) && text16 != "-")
						{
							string text17 = text16;
							string numberDecimalSeparator = reportCulture.NumberFormat.NumberDecimalSeparator;
							string text18 = text17.Replace(".", ",").Replace(",", numberDecimalSeparator);
							if (!string.IsNullOrEmpty(text18))
							{
								flag20 = true;
								try
								{
									if (flag7 || flag8)
									{
										if (StiOptions.Export.Excel.AllowExportDateTime)
										{
											DateTime dateTime = DateTime.Parse(text18, reportCulture);
											double num41 = dateTime.Subtract(new DateTime(1900, 1, 1)).Days + 1 + 1;
											double num42 = dateTime.TimeOfDay.TotalSeconds / 86400.0;
											result = num41 + num42;
										}
										else
										{
											flag20 = false;
										}
									}
									else if (!double.TryParse(text18, NumberStyles.Any, reportCulture, out result))
									{
										flag20 = false;
									}
								}
								catch (FormatException)
								{
									flag20 = false;
								}
							}
							if (flag9)
							{
								flag20 = false;
							}
							if (!flag20 && text4 == null)
							{
								text4 = text17;
							}
						}
						if (flag20)
						{
							dwWord(0, (short)(num22 - minRowIndex));
							dwWord(2, (short)(num26 - 1));
							dwWord(4, (short)num38);
							dwDouble(6, result);
							AddRec(515, 14);
							continue;
						}
						string text19 = text4.Replace("\r", "").Replace('\t', ' ');
						if (StiOptions.Export.Excel.TrimTrailingSpaces)
						{
							text19 = StiExportUtils.TrimEndWhiteSpace(text19);
						}
						int sSTNumber = GetSSTNumber(text19);
						dwFill(0, 0, 10);
						dwWord(0, (short)(num22 - minRowIndex));
						dwWord(2, (short)(num26 - 1));
						dwWord(4, (short)num38);
						dwDWord(6, sSTNumber);
						AddRec(253, 10);
					}
					else
					{
						dwFill(0, 0, 6);
						dwWord(0, (short)(num22 - minRowIndex));
						dwWord(2, (short)(num26 - 1));
						dwWord(4, (short)num38);
						AddRec(513, 6);
					}
					continue;
				}
				if (array2[num22, num26] != 0)
				{
					int num43 = array2[num22, num26];
					dwFill(0, 0, 6);
					dwWord(0, (short)(num22 - minRowIndex));
					dwWord(2, (short)(num26 - 1));
					dwWord(4, (short)num43);
					AddRec(513, 6);
					continue;
				}
				bool flag21 = true;
				bool flag22 = true;
				bool flag23 = true;
				bool flag24 = true;
				if (matrix.BordersY[num22 - 1, num26 - 1] == (StiBorderSide)null)
				{
					flag21 = false;
				}
				if (matrix.BordersY[num22 - 1, num26] == (StiBorderSide)null)
				{
					flag22 = false;
				}
				if (matrix.BordersX[num22 - 1, num26 - 1] == (StiBorderSide)null)
				{
					flag23 = false;
				}
				if (matrix.BordersX[num22, num26 - 1] == (StiBorderSide)null)
				{
					flag24 = false;
				}
				if (flag21 || flag22 || flag23 || flag24)
				{
					dataIn2 = new DataXF(0, 0, 0, 0, 0, 0, 0, 0, 0, 63, GetLineStyle(flag21 ? matrix.BordersY[num22 - 1, num26 - 1] : null), GetLineStyle(flag22 ? matrix.BordersY[num22 - 1, num26] : null), GetLineStyle(flag23 ? matrix.BordersX[num22 - 1, num26 - 1] : null), GetLineStyle(flag24 ? matrix.BordersX[num22, num26 - 1] : null), flag21 ? GetColorNumber(matrix.BordersY[num22 - 1, num26 - 1].Color) : 0, flag22 ? GetColorNumber(matrix.BordersY[num22 - 1, num26].Color) : 0, flag23 ? GetColorNumber(matrix.BordersX[num22 - 1, num26 - 1].Color) : 0, flag24 ? GetColorNumber(matrix.BordersX[num22, num26 - 1].Color) : 0, 0, 0, 0);
					int num44 = GetXFNumber(dataIn2);
					if (!exportObjectFormatting)
					{
						num44 = 15;
					}
					dwFill(0, 0, 6);
					dwWord(0, (short)(num22 - minRowIndex));
					dwWord(2, (short)(num26 - 1));
					dwWord(4, (short)num44);
					AddRec(513, 6);
				}
			}
		}
		((Component)(object)val3)?.Dispose();
		if (arrayList.Count > 0 && (dataExportMode == StiDataExportMode.AllBands || exportObjectFormatting))
		{
			int num45 = 0;
			for (int num46 = 0; num46 < arrayList.Count; num46++)
			{
				if (num45 >= 1027)
				{
					dwWord(0, (short)num45);
					AddRec(229, num45 * 8 + 2);
					num45 = 0;
				}
				CellRangeAddress cellRangeAddress = (CellRangeAddress)arrayList[num46];
				dwWord(2 + num45 * 8, (short)cellRangeAddress.FirstRow);
				dwWord(2 + num45 * 8 + 2, (short)cellRangeAddress.LastRow);
				dwWord(2 + num45 * 8 + 4, (short)cellRangeAddress.FirstColumn);
				dwWord(2 + num45 * 8 + 6, (short)cellRangeAddress.LastColumn);
				num45++;
			}
			dwWord(0, (short)num45);
			AddRec(229, num45 * 8 + 2);
		}
		escher.WriteDG(imageList, count);
		escher.mem.Seek(0L, SeekOrigin.Begin);
		for (int num47 = count; num47 < imageList.Count; num47++)
		{
			int num48 = escher.MemBookmarks[num47 + 1 - count] - escher.MemBookmarks[num47 - count];
			escher.mem.Read(bfData, 0, num48);
			AddRec(236, num48);
			dwWord(0, 21);
			dwWord(2, 18);
			dwWord(4, 8);
			dwWord(6, (short)(num47 + 1));
			dwWord(8, 24593);
			dwDWord(10, 0);
			dwDWord(14, 0);
			dwDWord(18, 0);
			dwWord(22, 7);
			dwWord(24, 2);
			dwWord(26, -1);
			dwWord(28, 8);
			dwWord(30, 2);
			dwWord(32, 0);
			dwWord(34, 0);
			dwWord(36, 0);
			AddRec(93, 38);
		}
		escher.mem.Close();
		ushort num49 = 1204;
		if (sheetIndex == 0)
		{
			num49 = (ushort)(num49 | 0x200u);
		}
		if (StiOptions.Export.Excel.ColumnsRightToLeft)
		{
			num49 = (ushort)(num49 | 0x40u);
		}
		if (StiOptions.Export.Excel.ShowGridLines)
		{
			num49 = (ushort)(num49 | 2u);
		}
		dwFill(0, 0, 18);
		dwWord(0, num49);
		dwWord(2, 0);
		dwWord(4, 0);
		dwWord(6, 64);
		dwWord(8, 0);
		dwWord(10, 0);
		dwWord(12, 0);
		AddRec(574, 18);
		dwByte(0, 3);
		dwWord(1, 0);
		dwWord(3, 0);
		dwWord(5, 0);
		dwWord(7, 1);
		dwWord(9, 0);
		dwWord(11, 0);
		dwByte(13, 0);
		dwByte(14, 0);
		AddRec(29, 15);
		if (arrayList2.Count > 0)
		{
			for (int num50 = 0; num50 < arrayList2.Count; num50++)
			{
				byte[] array4 = (byte[])arrayList2[num50];
				Array.Copy(array4, 0, bfData, 0, array4.Length);
				AddRec(440, array4.Length);
			}
		}
		if (maxRowIndex < Matrix.CoordY.Count)
		{
			Hashtable bookmarksTable = matrix.Bookmarks.BookmarksTable;
			Hashtable hashtable2 = new Hashtable();
			if (bookmarksTable != null)
			{
				foreach (DictionaryEntry item in bookmarksTable)
				{
					Size size = (Size)item.Value;
					if (size.Height >= minRowIndex && size.Height < maxRowIndex)
					{
						hashtable2[item.Key] = new Size(size.Width, size.Height - minRowIndex);
					}
				}
			}
			bookmarksList.Add(hashtable2);
		}
		else
		{
			bookmarksList.Add(matrix.Bookmarks.BookmarksTable);
		}
		hlinksList.Add(arrayList3);
		Size result2 = new Size(Matrix.CoordX.Count - 2, maxRowIndex - minRowIndex - 1);
		if (Matrix.CoordY.Count > maxRowIndex)
		{
			minRowIndex = maxRowIndex;
		}
		else
		{
			matrix = null;
		}
		return result2;
	}

	private void MakeMainStream(StiReport report, StiPagesCollection allPages)
	{
		ArrayList arrayList = new ArrayList();
		base.CurrentPassNumber = 0;
		base.MaximumPassNumber = 3;
		for (int i = 0; i < allPages.Count; i++)
		{
			StiPagesCollection stiPagesCollection = new StiPagesCollection(report, allPages);
			stiPagesCollection.AddV2Internal(allPages.GetPageWithoutCache(i));
			if (!exportEachPageToSheet)
			{
				string excelSheetValue = allPages.GetPageWithoutCache(i).ExcelSheetValue;
				while (i < allPages.Count - 1 && CompareExcellSheetNames(allPages.GetPageWithoutCache(i + 1).ExcelSheetValue, excelSheetValue))
				{
					i++;
					stiPagesCollection.AddV2Internal(allPages.GetPageWithoutCache(i));
				}
			}
			stiPagesCollection.CacheMode = report.RenderedPages.CacheMode;
			string text = stiPagesCollection[0].ExcelSheetValue;
			if (text == null || text == string.Empty)
			{
				text = $"Page {boundsheetsNamesList.Count + 1}";
			}
			string text2 = "";
			int num = 1;
			do
			{
				memst = new StiCachedStream();
				arrayList.Add(MakeWorksheetStream(stiPagesCollection, pagesList.Count));
				if (base.IsStopped)
				{
					return;
				}
				pagesList.Add(memst);
				if (matrix != null || text2.Length > 0)
				{
					text2 = $" part{num++}";
				}
				boundsheetsNamesList.Add(text + text2);
			}
			while (matrix != null);
		}
		Hashtable hashtable = new Hashtable();
		for (int j = 0; j < boundsheetsNamesList.Count; j++)
		{
			string text3 = (string)boundsheetsNamesList[j];
			text3 = text3.Replace("*", "_").Replace("\\", "_").Replace("/", "_")
				.Replace("[", "_")
				.Replace("]", "_")
				.Replace(":", "_")
				.Replace("?", "_");
			if (hashtable.Contains(text3))
			{
				int num2 = 1;
				while (hashtable.Contains(text3 + "-" + num2))
				{
					num2++;
				}
				text3 = text3 + "-" + num2;
			}
			hashtable.Add(text3, text3);
			boundsheetsNamesList[j] = text3;
		}
		memst = new StiCachedStream();
		dwFill(0, 0, 16);
		dwWord(0, 1536);
		dwWord(2, 5);
		dwWord(4, 6301);
		dwWord(6, 1997);
		dwDWord(8, 32969);
		dwDWord(12, 518);
		AddRec(2057, 16);
		dwFill(0, 32, 128);
		dwString(0, new string(' ', 34) + StiExportUtils.GetReportVersion(report), 2, out var offset, pack: true);
		AddRec(92, 112);
		dwWord(0, 1200);
		AddRec(66, 2);
		dwWord(0, 0);
		AddRec(353, 2);
		dwWord(0, 0);
		AddRec(25, 2);
		dwWord(0, 0);
		AddRec(18, 2);
		dwWord(0, 0);
		AddRec(19, 2);
		dwWord(0, 480);
		dwWord(2, 30);
		dwWord(4, 14235);
		dwWord(6, 8700);
		dwWord(8, 56);
		dwWord(10, 0);
		dwWord(12, 0);
		dwWord(14, 1);
		dwWord(16, 600);
		AddRec(61, 18);
		dwWord(0, 0);
		AddRec(64, 2);
		dwWord(0, 0);
		AddRec(141, 2);
		dwWord(0, 0);
		AddRec(34, 2);
		dwWord(0, 1);
		AddRec(14, 2);
		dwWord(0, 0);
		AddRec(218, 2);
		for (int k = 0; k < fontList.Count; k++)
		{
			DataFont dataFont = (DataFont)fontList[k];
			dwFill(0, 0, 14);
			dwWord(0, (short)dataFont.Height);
			int num3 = 0;
			if (dataFont.Italic)
			{
				num3 |= 2;
			}
			if (dataFont.Strikeout)
			{
				num3 |= 8;
			}
			dwWord(2, (short)num3);
			dwWord(4, (short)dataFont.Color);
			if (dataFont.Bold)
			{
				dwWord(6, 700);
			}
			else
			{
				dwWord(6, 400);
			}
			dwWord(8, 0);
			if (dataFont.Underlined)
			{
				dwByte(10, 1);
			}
			else
			{
				dwByte(10, 0);
			}
			dwByte(11, 0);
			dwByte(12, (byte)dataFont.Charset);
			dwString(14, dataFont.Name, 1, out offset, pack: false);
			AddRec(49, offset);
		}
		if (formatList.Count > 0)
		{
			for (int l = 0; l < formatList.Count; l++)
			{
				dwWord(0, (short)(l + 164));
				dwString(2, (string)formatList[l], 2, out offset, pack: false);
				AddRec(1054, offset);
			}
		}
		for (int m = 0; m < xfList.Count; m++)
		{
			DataXF dataXF = (DataXF)xfList[m];
			dwFill(0, 0, 20);
			dwWord(0, (short)dataXF.FontIndex);
			dwWord(2, (short)dataXF.FormatIndex);
			int num4 = ((dataXF.XFType << 2) & 4) | ((dataXF.ParentStyleXF << 4) & 0xFFF0);
			dwWord(4, (ushort)((uint)num4 | 1u));
			num4 = dataXF.HorAlign | ((dataXF.TextWrapped << 3) & 8) | ((dataXF.VertAlign << 4) & 0x70);
			dwByte(6, (byte)num4);
			dwByte(7, (byte)dataXF.TextRotationAngle);
			dwByte(8, (byte)(dataXF.TextDirection << 6));
			dwByte(9, (byte)(dataXF.UsedAttrib << 2));
			num4 = (dataXF.LineStyleLeft & 0xF) | ((dataXF.LineStyleRight << 4) & 0xF0) | ((dataXF.LineStyleTop << 8) & 0xF00) | ((dataXF.LineStyleBottom << 12) & 0xF000) | ((dataXF.ColorIndexLeft << 16) & 0x7F0000) | ((dataXF.ColorIndexRight << 23) & 0x3F800000);
			dwDWord(10, num4);
			num4 = (dataXF.ColorIndexTop & 0x7F) | ((dataXF.ColorIndexBottom << 7) & 0x3F80) | (int)((dataXF.FillPatern << 26) & 0xFC000000u);
			dwDWord(14, num4);
			num4 = (dataXF.ColorIndexPattern & 0x7F) | ((dataXF.ColorIndexBackground << 7) & 0x3F80);
			dwWord(18, (short)num4);
			AddRec(224, 20);
		}
		dwDWord(0, 32784);
		dwByte(2, 4);
		dwByte(3, byte.MaxValue);
		AddRec(659, 4);
		dwDWord(0, 32785);
		dwByte(2, 7);
		dwByte(3, byte.MaxValue);
		AddRec(659, 4);
		dwDWord(0, 32768);
		dwByte(2, 0);
		dwByte(3, byte.MaxValue);
		AddRec(659, 4);
		dwDWord(0, 32786);
		dwByte(2, 5);
		dwByte(3, byte.MaxValue);
		AddRec(659, 4);
		dwDWord(0, 32787);
		dwByte(2, 3);
		dwByte(3, byte.MaxValue);
		AddRec(659, 4);
		dwDWord(0, 32788);
		dwByte(2, 6);
		dwByte(3, byte.MaxValue);
		AddRec(659, 4);
		if (colorList.Count > 0)
		{
			int num5 = colorList.Count;
			if (num5 > 56)
			{
				num5 = 56;
			}
			dwWord(0, (short)num5);
			for (int n = 0; n < num5; n++)
			{
				Color color = (Color)colorList[n];
				dwByte(2 + n * 4, color.R);
				dwByte(2 + n * 4 + 1, color.G);
				dwByte(2 + n * 4 + 2, color.B);
				dwByte(2 + n * 4 + 3, 0);
			}
			AddRec(146, 2 + num5 * 4);
		}
		dwWord(0, 0);
		AddRec(352, 2);
		for (int num6 = 0; num6 < pagesList.Count; num6++)
		{
			string dataValue = (string)boundsheetsNamesList[num6];
			int num7 = (int)memst.Position + 4;
			boundsheetsOffsetsList.Add(num7);
			dwDWord(0, 0);
			dwByte(4, 0);
			dwByte(5, 0);
			dwString(6, dataValue, 1, out offset, pack: false);
			AddRec(133, offset);
		}
		dwWord(0, (short)pagesList.Count);
		dwWord(2, 1025);
		AddRec(430, 4);
		dwWord(0, (short)pagesList.Count);
		for (int num8 = 0; num8 < pagesList.Count; num8++)
		{
			dwWord(2 + num8 * 6, 0);
			dwWord(4 + num8 * 6, (short)num8);
			dwWord(6 + num8 * 6, (short)num8);
		}
		AddRec(23, 2 + pagesList.Count * 6);
		for (int num9 = 0; num9 < pagesList.Count; num9++)
		{
			Size size = (Size)arrayList[num9];
			if (size.Height > 65535)
			{
				size.Height = 65535;
			}
			if (size.Width > 255)
			{
				size.Width = 255;
			}
			dwWord(0, 32);
			dwByte(2, 0);
			dwByte(3, 1);
			dwWord(4, 11);
			dwWord(6, 0);
			dwWord(8, (short)(num9 + 1));
			dwByte(10, 0);
			dwByte(11, 0);
			dwByte(12, 0);
			dwByte(13, 0);
			dwByte(14, 0);
			dwByte(15, 6);
			dwByte(16, 59);
			dwWord(17, (short)num9);
			dwWord(19, 0);
			dwWord(21, (short)size.Height);
			dwWord(23, 0);
			dwWord(25, (short)size.Width);
			AddRec(24, 27);
		}
		escher.WriteDGG(imageList);
		escher.mem.Seek(0L, SeekOrigin.Begin);
		int num10 = 0;
		short com = 235;
		for (; num10 + 8224 < escher.mem.Length; num10 += 8224)
		{
			escher.mem.Read(bfData, 0, 8224);
			AddRec(com, 8224);
			com = 60;
		}
		escher.mem.Read(bfData, 0, (int)(escher.mem.Length - num10));
		AddRec(com, (int)(escher.mem.Length - num10));
		escher.mem.Close();
		dwDWord(0, sstCounter);
		dwDWord(4, sstList.Count);
		int num11 = sstList.Count / 1020 + 1;
		if (num11 < 8)
		{
			num11 = 8;
		}
		int num12 = 0;
		int num13 = sstList.Count / num11;
		if (sstList.Count % num11 > 0)
		{
			num13++;
		}
		int[,] array = new int[num13, 2];
		int com2 = 252;
		int num14 = 8;
		if (sstList.Count > 0)
		{
			for (int num15 = 0; num15 < sstList.Count; num15++)
			{
				if (num15 % num11 == 0)
				{
					array[num12, 0] = (int)memst.Position + num14 + 4;
					array[num12, 1] = num14 + 4;
					num12++;
				}
				string dataValue2 = (string)sstList[num15];
				dwString(num14, dataValue2, 2, out offset, pack: true);
				byte b = bfData[num14 + 2];
				num14 = offset;
				int num16 = 8192 + (num14 & 1);
				while (num14 > num16)
				{
					AddRec(com2, num16);
					bfData[0] = b;
					Array.Copy(bfData, num16, bfData, 1, num14);
					num14 -= num16 - 1;
					num16 = 8192 + (num14 & 1);
					com2 = 60;
				}
				if (num14 > num16 - 10)
				{
					AddRec(com2, num14);
					com2 = 60;
					num14 = 0;
				}
			}
		}
		if (num14 != 0)
		{
			AddRec(com2, num14);
		}
		dwWord(0, (short)num11);
		num14 = 2;
		if (sstList.Count > 0)
		{
			for (int num17 = 0; num17 < num13; num17++)
			{
				dwDWord(num14, array[num17, 0]);
				dwDWord(num14 + 4, array[num17, 1]);
				dwWord(num14 + 6, 0);
				num14 += 8;
			}
		}
		AddRec(255, num14);
		AddRec(10, 0);
		for (int num18 = 0; num18 < pagesList.Count; num18++)
		{
			int num19 = (int)memst.Position;
			memst.Seek((int)boundsheetsOffsetsList[num18], SeekOrigin.Begin);
			dwDWord(0, num19);
			memst.Write(bfData, 0, 4);
			memst.Seek(num19, SeekOrigin.Begin);
			MemoryStream memoryStream = (MemoryStream)pagesList[num18];
			memoryStream.WriteTo(memst);
			memoryStream.Close();
			ArrayList arrayList2 = (ArrayList)hlinksList[num18];
			for (int num20 = 0; num20 < arrayList2.Count; num20++)
			{
				HlinkData hlinkData = (HlinkData)arrayList2[num20];
				string text4 = hlinkData.Bookmark;
				bool flag = false;
				for (int num21 = 0; num21 < bookmarksList.Count; num21++)
				{
					Hashtable hashtable2 = (Hashtable)bookmarksList[num21];
					if (hashtable2 != null)
					{
						object obj = hashtable2[text4];
						if (obj != null)
						{
							Size size2 = (Size)obj;
							text4 = $"'{(string)boundsheetsNamesList[num21]}'!{GetRefString(size2.Width, size2.Height)}";
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					dwWord(0, (short)hlinkData.Range.FirstRow);
					dwWord(2, (short)hlinkData.Range.LastRow);
					dwWord(4, (short)hlinkData.Range.FirstColumn);
					dwWord(6, (short)hlinkData.Range.LastColumn);
					dwByte(8, 208, 201, 234, 121);
					dwByte(12, 249, 186, 206, 17);
					dwByte(16, 140, 130, 0, 170);
					dwByte(20, 0, 75, 169, 11);
					dwDWord(24, 2);
					dwDWord(28, 28);
					int num22 = 32;
					dwDWord(num22, hlinkData.Description.Length + 1);
					num22 += 4;
					for (int num23 = 0; num23 < hlinkData.Description.Length; num23++)
					{
						dwWord(num22, hlinkData.Description[num23]);
						num22 += 2;
					}
					dwWord(num22, 0);
					num22 += 2;
					dwDWord(num22, text4.Length + 1);
					num22 += 4;
					for (int num24 = 0; num24 < text4.Length; num24++)
					{
						dwWord(num22, text4[num24]);
						num22 += 2;
					}
					dwWord(num22, 0);
					num22 += 2;
					AddRec(440, num22);
				}
			}
			AddRec(10, 0);
		}
	}

	private void WriteToOLEContainer()
	{
		int num = 1;
		int num2 = (int)(memst.Length / 512);
		if (memst.Length % 512 != 0L)
		{
			num2++;
		}
		if (num2 < 8)
		{
			num2 = 8;
		}
		int num3 = 0;
		int num4 = 0;
		int num5;
		do
		{
			num5 = num3;
			if (num3 >= 109)
			{
				num4 = (num3 - 109) / 127;
				if ((num3 - 109) % 127 != 0)
				{
					num4++;
				}
			}
			else
			{
				num4 = 0;
			}
			num3 = (num2 + num + num5 + num4) / 128;
			if ((num2 + num + num5 + num4) % 128 != 0)
			{
				num3++;
			}
		}
		while (num3 != num5);
		memStMain = new StiCachedStream();
		dwFill(0, 0, 512);
		dwByte(0, 208, 207, 17, 224);
		dwByte(4, 161, 177, 26, 225);
		dwWord(24, 62);
		dwWord(26, 3);
		dwWord(28, -2);
		dwWord(30, 9);
		dwWord(32, 6);
		dwDWord(44, num3);
		dwDWord(48, num4 + num3 + num2);
		dwDWord(56, 4096);
		dwDWord(60, -2);
		dwDWord(64, 0);
		dwDWord(68, (num4 == 0) ? (-2) : 0);
		dwDWord(72, num4);
		dwFill(76, byte.MaxValue, 436 + num4 * 512);
		int num6 = 0;
		int num7 = 0;
		while (num6 < num3)
		{
			if (num7 % 128 == 108 && num7 > 108)
			{
				dwDWord(76 + num7 * 4, num7 / 128);
			}
			else
			{
				dwDWord(76 + num7 * 4, num4 + num6);
				num6++;
			}
			num7++;
		}
		if (num4 > 0)
		{
			dwDWord(512 + num4 * 512 - 2, -2);
		}
		memStMain.Write(bfData, 0, 512 + num4 * 512);
		if (bfData.Length < (num3 + 1) * 512)
		{
			bfData = new byte[(num3 + 1) * 512];
		}
		dwFill(0, byte.MaxValue, num3 * 512);
		for (int i = 0; i < num4; i++)
		{
			dwDWord(i * 4, -4);
		}
		for (int j = 0; j < num3; j++)
		{
			dwDWord((num4 + j) * 4, -3);
		}
		for (int k = 0; k < num2; k++)
		{
			dwDWord((num4 + num3 + k) * 4, num4 + num3 + k + 1);
		}
		dwDWord((num4 + num3 + num2 - 1) * 4, -2);
		dwDWord((num4 + num3 + num2) * 4, -2);
		memStMain.Write(bfData, 0, num3 * 512);
		int count = num2 * 512 - (int)memst.Length;
		memst.WriteTo(memStMain);
		dwFill(0, 0, count);
		memStMain.Write(bfData, 0, count);
		dwFill(0, 0, 512);
		dwString(0, "Root Entry", 0, out var offset, pack: false);
		dwWord(64, 22);
		dwByte(66, 5);
		dwByte(67, 1);
		dwDWord(68, -1);
		dwDWord(72, -1);
		dwDWord(76, 1);
		dwDWord(116, -2);
		dwDWord(120, 0);
		dwString(128, "Workbook", 0, out offset, pack: false);
		dwWord(192, 18);
		dwByte(194, 2);
		dwByte(195, 1);
		dwDWord(196, -1);
		dwDWord(200, -1);
		dwDWord(204, -1);
		dwDWord(244, num4 + num3);
		dwDWord(248, num2 * 512);
		memStMain.Write(bfData, 0, 512);
		memst.Close();
	}

	public void ExportExcel(StiReport report, string fileName)
	{
		StiFileUtils.ProcessReadOnly(fileName);
		using FileStream fileStream = new FileStream(fileName, FileMode.Create);
		ExportExcel(report, fileStream);
		fileStream.Flush();
		fileStream.Close();
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

	public void ExportExcel(StiReport report, Stream stream, StiPagesRange pageRange, bool useOnePageHeaderAndFooter)
	{
		StiExcelExportSettings settings = new StiExcelExportSettings
		{
			UseOnePageHeaderAndFooter = useOnePageHeaderAndFooter,
			PageRange = pageRange
		};
		ExportExcel(report, stream, settings);
	}

	public void ExportExcel(StiReport report, Stream stream, StiPagesRange pageRange, bool useOnePageHeaderAndFooter, bool exportDataOnly, bool exportObjectFormatting)
	{
		StiExcelExportSettings settings = new StiExcelExportSettings
		{
			UseOnePageHeaderAndFooter = useOnePageHeaderAndFooter,
			DataExportMode = (exportDataOnly ? StiDataExportMode.DataAndHeaders : StiDataExportMode.AllBands),
			ExportObjectFormatting = exportObjectFormatting,
			PageRange = pageRange
		};
		ExportExcel(report, stream, settings);
	}

	public void ExportExcel(StiReport report, Stream stream, StiPagesRange pageRange, bool useOnePageHeaderAndFooter, bool exportDataOnly, bool exportObjectFormatting, bool exportEachPageToSheet)
	{
		StiExcelExportSettings settings = new StiExcelExportSettings
		{
			UseOnePageHeaderAndFooter = useOnePageHeaderAndFooter,
			DataExportMode = (exportDataOnly ? StiDataExportMode.DataAndHeaders : StiDataExportMode.AllBands),
			ExportObjectFormatting = exportObjectFormatting,
			ExportEachPageToSheet = exportEachPageToSheet,
			PageRange = pageRange
		};
		ExportExcel(report, stream, settings);
	}

	public void ExportExcel(StiReport report, Stream stream, StiExcelExportSettings settings)
	{
		StiLogService.Write(((object)this).GetType(), "Export report to Excel format");
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
		bfHeader = new byte[4];
		bfData = new byte[262144];
		if (imageResolution < 10f)
		{
			imageResolution = 10f;
		}
		imageResolution /= 100f;
		maximumSheetHeight = StiOptions.Export.Excel.MaximumSheetHeight;
		if (maximumSheetHeight < 1)
		{
			maximumSheetHeight = 1;
		}
		if (maximumSheetHeight > 65535)
		{
			maximumSheetHeight = 65535;
		}
		if (dataExportMode != StiDataExportMode.AllBands)
		{
			useOnePageHeaderAndFooter = false;
		}
		else
		{
			exportObjectFormatting = true;
		}
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
			if (!base.IsStopped)
			{
				base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
				PrepareData();
				MakeMainStream(report, selectedPages);
				WriteToOLEContainer();
				memStMain.WriteTo(stream);
				memStMain.Close();
			}
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
			xfList = null;
			colorList = null;
			fontList = null;
			sstList = null;
			sstHash = null;
			formatList = null;
			memst = null;
			memStMain = null;
			imageList = null;
			escher = null;
			pagesList = null;
			bookmarksList = null;
			hlinksList = null;
			boundsheetsOffsetsList = null;
			boundsheetsNamesList = null;
			imageCodec = null;
			if (report.RenderedPages.CacheMode)
			{
				StiMatrix.GCCollect();
			}
		}
	}
}
