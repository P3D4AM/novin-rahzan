using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Rtf.png")]
public class StiRtfExportService : StiExportService
{
	private struct StiRtfData
	{
		public int X;

		public int Y;

		public int Width;

		public int Height;

		public StiComponent Component;
	}

	private struct StiRtfStyleInfo
	{
		public string Name;

		public StiTextHorAlignment Alignment;

		public bool RightToLeft;

		public int FontNumber;

		public int FontSize;

		public bool Bold;

		public bool Italic;

		public bool Underline;

		public int TextColor;
	}

	private StiReport report;

	private string fileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	private StreamWriter sw;

	private ArrayList colorList;

	private ArrayList fontList;

	private ArrayList styleList;

	private int[] unicodeMapArray;

	private byte[] codePageToFont;

	private byte charsetCount;

	private int[] fontToCodePages;

	private int baseFontNumber;

	private Stream sw2;

	private bool usePageHeadersAndFooters;

	private float imageResolution = 0.96f;

	private float imageQuality = 0.75f;

	private StiExportFormat imageFormat = StiExportFormat.Rtf;

	private ImageCodecInfo imageCodec;

	private bool useStyles;

	private Hashtable bookmarkList;

	private Hashtable usedBookmarks;

	private bool removeEmptySpaceAtBottom = StiOptions.Export.RichText.RemoveEmptySpaceAtBottom;

	private const double HiToTwips = 14.4;

	private const int frameCorrectValue = 38;

	private int pageHeight;

	private int pageWidth;

	private StiMatrix matrix;

	public override string DefaultExtension => "rtf";

	public override StiExportFormat ExportFormat => StiExportFormat.Rtf;

	public override string GroupCategory => "Word";

	public override int Position => 21;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeRtfFile");

	public override bool MultipleFiles => false;

	internal bool RemoveEmptySpaceAtBottom => removeEmptySpaceAtBottom;

	internal StiMatrix Matrix => matrix;

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "RtfFiles");
	}

	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		ExportRtf(report, stream, settings as StiRtfExportSettings);
		InvokeExporting(100, 100, 1, 1);
	}

	public override void Export(StiReport report, string fileName, bool sendEMail, StiGuiMode guiMode)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		IStiFormRunner exportFormRunner = StiGuiOptions.GetExportFormRunner("StiRtfExportSetupForm", guiMode, base.OwnerWindow);
		try
		{
			exportFormRunner["CurrentPage"] = report.CurrentPrintPage;
			exportFormRunner["OpenAfterExportEnabled"] = !sendEMail;
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

	private void Form_Complete(IStiFormRunner form, StiShowDialogCompleteEvetArgs e)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
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
				StiRtfExportSettings stiRtfExportSettings = new StiRtfExportSettings();
				stiRtfExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
				stiRtfExportSettings.ExportMode = (StiRtfExportMode)form["ExportMode"];
				stiRtfExportSettings.UsePageHeadersAndFooters = (bool)form["UsePageHeadersAndFooters"];
				stiRtfExportSettings.ImageQuality = (float)form["ImageQuality"];
				stiRtfExportSettings.ImageResolution = (float)form["Resolution"];
				stiRtfExportSettings.RemoveEmptySpaceAtBottom = (bool)form["RemoveEmptySpaceAtBottom"];
				StiRtfExportSettings settings = stiRtfExportSettings;
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

	private int GetColorNumberInt(ArrayList tmpColorList, Color incomingColor)
	{
		if (tmpColorList.Count > 0)
		{
			for (int i = 0; i < tmpColorList.Count; i++)
			{
				if ((Color)tmpColorList[i] == incomingColor)
				{
					return i;
				}
			}
		}
		tmpColorList.Add(incomingColor);
		return tmpColorList.Count - 1;
	}

	private string GetColorNumber(ArrayList tmpColorList, Color incomingColor)
	{
		return GetColorNumberInt(tmpColorList, incomingColor).ToString();
	}

	private int GetFontNumber(ArrayList tmpFontList, Font incomingFont)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		if (tmpFontList.Count > 0)
		{
			for (int i = 0; i < tmpFontList.Count; i++)
			{
				Font val = (Font)tmpFontList[i];
				if (val.Name == incomingFont.Name)
				{
					return i * ((charsetCount == 0) ? 1 : charsetCount);
				}
			}
		}
		tmpFontList.Add(incomingFont);
		int num = tmpFontList.Count - 1;
		return num * ((charsetCount == 0) ? 1 : charsetCount);
	}

	private int GetFontNumber(ArrayList tmpFontList, Font incomingFont, int charset)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		if (tmpFontList.Count > 0)
		{
			for (int i = 0; i < tmpFontList.Count; i++)
			{
				Font val = (Font)tmpFontList[i];
				if (val.Name == incomingFont.Name)
				{
					return i * ((charsetCount == 0) ? 1 : charsetCount) + GetCharsetIndex(charset);
				}
			}
		}
		tmpFontList.Add(incomingFont);
		int num = tmpFontList.Count - 1;
		return num * ((charsetCount == 0) ? 1 : charsetCount) + GetCharsetIndex(charset);
	}

	private int GetCharsetIndex(int charset)
	{
		int num = 0;
		for (int i = 0; i < 9; i++)
		{
			if (StiEncode.codePagesTable[i, 2] == charset)
			{
				num = i + 1;
				break;
			}
		}
		if (charsetCount == 0)
		{
			codePageToFont[num] = 1;
			return 0;
		}
		return codePageToFont[num];
	}

	private int GetFontNumber(ArrayList tmpFontList, string fontName)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		if (tmpFontList.Count > 0)
		{
			for (int i = 0; i < tmpFontList.Count; i++)
			{
				Font val = (Font)tmpFontList[i];
				if (val.Name == fontName)
				{
					return i * ((charsetCount == 0) ? 1 : charsetCount);
				}
			}
		}
		tmpFontList.Add((object?)new Font(StiFontCollection.GetFontFamily(fontName, false), 8f));
		int num = tmpFontList.Count - 1;
		return num * ((charsetCount == 0) ? 1 : charsetCount);
	}

	private int GetStyleNumber(ArrayList tmpStyleList, StiRtfStyleInfo styleInfo)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (tmpStyleList.Count > 0)
		{
			for (int i = 0; i < tmpStyleList.Count; i++)
			{
				StiRtfStyleInfo stiRtfStyleInfo = (StiRtfStyleInfo)tmpStyleList[i];
				if (stiRtfStyleInfo.Alignment == styleInfo.Alignment && stiRtfStyleInfo.Name == styleInfo.Name && stiRtfStyleInfo.FontNumber == styleInfo.FontNumber && stiRtfStyleInfo.FontSize == styleInfo.FontSize && stiRtfStyleInfo.Bold == styleInfo.Bold && stiRtfStyleInfo.Italic == styleInfo.Italic && stiRtfStyleInfo.Underline == styleInfo.Underline && stiRtfStyleInfo.TextColor == styleInfo.TextColor && stiRtfStyleInfo.RightToLeft == styleInfo.RightToLeft)
				{
					return i + 1;
				}
			}
		}
		tmpStyleList.Add(styleInfo);
		int num = tmpStyleList.Count - 1;
		return num + 1;
	}

	private string GetLineStyle(StiBorderSide border, ArrayList colorList)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected I4, but got Unknown
		StringBuilder stringBuilder = new StringBuilder();
		if (border != (StiBorderSide)null && (int)border.Style != 6)
		{
			StiPenStyle style = border.Style;
			switch ((int)style)
			{
			case 0:
				stringBuilder.Append("\\brdrs");
				break;
			case 4:
				stringBuilder.Append("\\brdrdot");
				break;
			case 1:
				stringBuilder.Append("\\brdrdash");
				break;
			case 2:
				stringBuilder.Append("\\brdrdashd");
				break;
			case 3:
				stringBuilder.Append("\\brdrdashdd");
				break;
			case 5:
				stringBuilder.Append("\\brdrdb");
				break;
			}
			stringBuilder.Append("\\brdrw");
			stringBuilder.Append((int)(border.Size * 15.0));
			stringBuilder.Append($"\\brdrcf{GetColorNumber(colorList, border.Color)}");
		}
		return stringBuilder.ToString();
	}

	private string ConvertStringToBookmark(string inputString)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (char c in inputString)
		{
			if (char.IsLetterOrDigit(c))
			{
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString();
	}

	private bool CompareExcellSheetNames(string name1, string name2)
	{
		string text = name1;
		if (string.IsNullOrEmpty(text))
		{
			text = string.Empty;
		}
		string text2 = name2;
		if (string.IsNullOrEmpty(text2))
		{
			text2 = string.Empty;
		}
		return text == text2;
	}

	private string ConvertTextWithHtmlTagsToRtfText(StiText stiText, string text)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Invalid comparison between Unknown and I4
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Invalid comparison between Unknown and I4
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Invalid comparison between Unknown and I4
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		StiHtmlTagsState val = default(StiHtmlTagsState);
		((StiHtmlTagsState)(ref val))._002Ector(stiText.Font.Bold, stiText.Font.Italic, stiText.Font.Underline, stiText.Font.Strikeout, stiText.Font.SizeInPoints, stiText.Font.Name, StiBrush.ToColor(stiText.TextBrush), StiBrush.ToColor(stiText.Brush), false, false, 0.0, 0.0, (double)stiText.LineSpacing, stiText.HorAlignment);
		StiHtmlState val2 = default(StiHtmlState);
		((StiHtmlState)(ref val2))._002Ector(val, 0);
		List<StiHtmlState> list = StiTextRenderer.ParseHtmlToStates(text, val2, false);
		baseFontNumber = GetFontNumber(fontList, stiText.Font);
		StringBuilder stringBuilder = new StringBuilder(GetLineHeightInTwips(stiText.LineSpacing));
		StiHtmlTagsState val3 = val;
		for (int i = 0; i < list.Count; i++)
		{
			StiHtmlState val4 = list[i];
			StiHtmlTagsState tS = val4.TS;
			StringBuilder stringBuilder2 = new StringBuilder();
			string text2 = null;
			if (tS.Bold != val3.Bold)
			{
				stringBuilder2.Append(tS.Bold ? "\\b" : "\\b0");
			}
			if (tS.Italic != val3.Italic)
			{
				stringBuilder2.Append(tS.Italic ? "\\i" : "\\i0");
			}
			if (tS.Underline != val3.Underline)
			{
				stringBuilder2.Append(tS.Underline ? "\\ul" : "\\ul0");
			}
			if (tS.Strikeout != val3.Strikeout)
			{
				stringBuilder2.Append(tS.Strikeout ? "\\strike" : "\\strike0");
			}
			if (tS.Superscript != val3.Superscript)
			{
				stringBuilder2.Append(tS.Superscript ? "\\super" : "\\nosupersub");
			}
			if (tS.Subscript != val3.Subscript)
			{
				stringBuilder2.Append(tS.Subscript ? "\\sub" : "\\nosupersub");
			}
			if (tS.FontColor != val3.FontColor)
			{
				stringBuilder2.Append($"\\cf{GetColorNumberInt(colorList, tS.FontColor)}");
			}
			if (tS.BackColor != val3.BackColor)
			{
				stringBuilder2.Append($"\\chshdng0\\chcbpat{GetColorNumber(colorList, tS.BackColor)}");
			}
			if (tS.FontSize != val3.FontSize)
			{
				stringBuilder2.Append($"\\fs{(int)(tS.FontSize * 2f)}");
			}
			if (tS.LetterSpacing != val3.LetterSpacing)
			{
				stringBuilder2.Append($"\\expnd{(int)(tS.LetterSpacing * (double)tS.FontSize * 3.8 + (double)StiOptions.Export.RichText.SpaceBetweenCharacters)}");
			}
			if (tS.LineHeight != val3.LineHeight)
			{
				stringBuilder2.Append(GetLineHeightInTwips(tS.LineHeight));
			}
			if (tS.TextAlign != val3.TextAlign)
			{
				string text3 = "\\ql";
				if ((int)tS.TextAlign == 1)
				{
					text3 = "\\qc";
				}
				if ((int)tS.TextAlign == 2)
				{
					text3 = "\\qr";
				}
				if ((int)tS.TextAlign == 3)
				{
					text3 = "\\qj";
				}
				if (val4.Text.ToString() == "\n")
				{
					text2 = text3;
				}
				else
				{
					stringBuilder2.Append(text3);
				}
			}
			if (tS.FontName != val3.FontName)
			{
				baseFontNumber = GetFontNumber(fontList, tS.FontName);
				stringBuilder2.Append($"{'\u0010'}{(char)(256 + baseFontNumber)}");
			}
			if (stringBuilder2.Length > 0)
			{
				stringBuilder.Append((object?)stringBuilder2.Replace("\\", "\u0017"));
				stringBuilder.Append(" ");
			}
			if (val4.Text.ToString() == "\n")
			{
				stringBuilder.Append("\n");
				if (text2 != null)
				{
					stringBuilder.Append(text2.Replace("\\", "\u0017"));
				}
			}
			else
			{
				if (stringBuilder[stringBuilder.Length - 1] != ' ' && stringBuilder[stringBuilder.Length - 1] != '\n')
				{
					stringBuilder.Append(" ");
				}
				stringBuilder.Append((object?)StiTextRenderer.PrepareStateText(val4.Text));
			}
			val3 = tS;
		}
		return stringBuilder.ToString();
	}

	private static string GetLineHeightInTwips(double lineHeightScale)
	{
		double lineSpacing = StiOptions.Export.RichText.LineSpacing;
		return string.Format("{0}sl{1}{0}slmult1", "\u0017", Math.Round(240.0 * lineSpacing * lineHeightScale));
	}

	internal static string GetRtfFileFromHtmlTags(StiText stiText, string text)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		StiText stiText2 = stiText.Clone(cloneProperties: true) as StiText;
		stiText2.Border = new StiBorder();
		stiText2.Left = 0.0;
		stiText2.Top = 0.0;
		stiText2.Text = new StiExpression(text);
		stiText2.TagValue = "rtfparagraph";
		MemoryStream memoryStream = new MemoryStream();
		StiReport stiReport = new StiReport();
		StiPage stiPage = new StiPage(stiReport);
		stiPage.Components.Add(stiText2);
		stiReport.RenderedPages.Clear();
		stiReport.RenderedPages.AddV2Internal(stiPage);
		StiRtfExportService stiRtfExportService = new StiRtfExportService();
		stiRtfExportService.ExportRtf(stiReport, memoryStream, StiRtfExportMode.Table);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		StreamReader streamReader = new StreamReader(memoryStream, Encoding.GetEncoding(1252));
		return streamReader.ReadToEnd();
	}

	private string ReplacePardInRtf(string st)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		int num2 = 0;
		bool flag = false;
		int num3 = 0;
		while (num3 < st.Length)
		{
			char c = st[num3++];
			stringBuilder.Append(c);
			switch (c)
			{
			case '{':
				num++;
				break;
			case '}':
				num--;
				if (num < num2)
				{
					flag = false;
				}
				break;
			case '\\':
				if (num3 < st.Length && (st[num3] == '{' || st[num3] == '}'))
				{
					num3++;
					break;
				}
				if (num3 + 2 < st.Length && st[num3] == 's' && st[num3 + 1] == 'h' && st[num3 + 2] == 'p' && st[num3 - 2] == '{')
				{
					flag = true;
					num2 = num;
				}
				if (!flag && num3 + 3 < st.Length && st[num3] == 'p' && st[num3 + 1] == 'a' && st[num3 + 2] == 'r' && st[num3 + 3] == 'd')
				{
					num3 += 4;
					stringBuilder.Append("pard\\intbl");
				}
				break;
			}
		}
		return stringBuilder.ToString();
	}

	private string GetImageString(Image image, float zoom, int absw, int absh)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		MemoryStream memoryStream = new MemoryStream();
		string text = "jpeg";
		if (imageFormat == StiExportFormat.ImagePng)
		{
			text = "png";
			image.Save((Stream)memoryStream, ImageFormat.Png);
		}
		else if (imageCodec == null)
		{
			image.Save((Stream)memoryStream, ImageFormat.Jpeg);
		}
		else
		{
			EncoderParameters val = new EncoderParameters(1);
			val.Param[0] = new EncoderParameter(Encoder.Quality, (long)(imageQuality * 100f));
			image.Save((Stream)memoryStream, imageCodec, val);
		}
		byte[] array = memoryStream.ToArray();
		memoryStream.Close();
		StringBuilder stringBuilder = new StringBuilder(array.Length * 2 + 200);
		float num = zoom / imageResolution;
		stringBuilder.Append("{" + $"\\pict\\picscalex{100}\\picscaley{100}\\picwgoal{absw}\\pichgoal{absh}\\{text}blip ");
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		stringBuilder.Append("}");
		return stringBuilder.ToString();
	}

	private void DrawLine(int tX1, int tY1, int tX2, int tY2, Color tColor, string stBorderWidth)
	{
		if (tColor.A != 0)
		{
			sw.Write("{\\shp{\\*");
			sw.Write("\\shpinst\\shpleft{0}\\shptop{1}\\shpright{2}\\shpbottom{3}", (tX1 < tX2) ? tX1 : tX2, (tY1 < tY2) ? tY1 : tY2, (tX1 > tX2) ? tX1 : tX2, (tY1 > tY2) ? tY1 : tY2);
			sw.Write("\\shpwr3");
			sw.Write("{\\sp{\\sn shapeType}{\\sv 20}}");
			sw.Write("{\\sp{\\sn fFlipH}{\\sv " + ((tX1 < tX2) ? "0" : "1") + "}}");
			sw.Write("{\\sp{\\sn fFlipV}{\\sv " + ((tY1 < tY2) ? "0" : "1") + "}}");
			sw.Write("{\\sp{\\sn fFilled}{\\sv 0}}");
			sw.Write("{\\sp{\\sn lineColor}{\\sv " + $"{tColor.B * 65536 + tColor.G * 256 + tColor.R}" + "}}");
			sw.Write("{\\sp{\\sn lineWidth}{\\sv " + stBorderWidth + "}}");
			sw.Write("{\\sp{\\sn fLine}{\\sv 1}}");
			sw.WriteLine("}}");
		}
	}

	private void FillRect(StiRtfData pp, Color tColor)
	{
		if (tColor.A != 0)
		{
			sw.Write("{\\shp{\\*");
			sw.Write("\\shpinst\\shpleft{0}\\shptop{1}\\shpright{2}\\shpbottom{3}", pp.X, pp.Y, pp.X + pp.Width, pp.Y + pp.Height);
			sw.Write("\\shpwr3");
			sw.Write("{\\sp{\\sn shapeType}{\\sv 1}}");
			sw.Write("{\\sp{\\sn fFlipH}{\\sv 0}}");
			sw.Write("{\\sp{\\sn fFlipV}{\\sv 0}}");
			sw.Write("{\\sp{\\sn fillColor}{\\sv " + $"{tColor.B * 65536 + tColor.G * 256 + tColor.R}" + "}}");
			sw.Write("{\\sp{\\sn fFilled}{\\sv 1}}");
			sw.Write("{\\sp{\\sn fLine}{\\sv 0}}");
			sw.WriteLine("}}");
		}
	}

	private bool CheckShape1(StiShape shape)
	{
		if (shape.ShapeType is StiVerticalLineShapeType || shape.ShapeType is StiHorizontalLineShapeType || shape.ShapeType is StiTopAndBottomLineShapeType || shape.ShapeType is StiLeftAndRightLineShapeType || shape.ShapeType is StiRectangleShapeType)
		{
			return true;
		}
		return false;
	}

	private void RenderShape1(StiRtfData pp, int correctX, int correctW)
	{
		StiComponent component = pp.Component;
		if (!(component is StiShape stiShape))
		{
			return;
		}
		if (CheckShape1(stiShape))
		{
			IStiBrush stiBrush = component as IStiBrush;
			string value = string.Empty;
			Color incomingColor = Color.Transparent;
			if (stiBrush != null)
			{
				incomingColor = StiBrush.ToColor(stiBrush.Brush);
			}
			if (incomingColor.A != 0)
			{
				string colorNumber = GetColorNumber(colorList, incomingColor);
				value = $"\\cbpat{colorNumber}";
			}
			string text = string.Empty;
			Color borderColor = stiShape.BorderColor;
			if (borderColor.A != 0)
			{
				string colorNumber2 = GetColorNumber(colorList, borderColor);
				text = $"\\brdrcf{colorNumber2}";
			}
			int num = (int)(stiShape.Size * 14f);
			string text2 = $"\\brdrs\\brdrw{num}";
			sw.Write("\\fs1");
			if (stiShape.ShapeType is StiVerticalLineShapeType)
			{
				if (incomingColor.A != 0)
				{
					sw.Write(value);
				}
				if (borderColor.A != 0)
				{
					sw.WriteLine(" \\par}");
					sw.Write("{");
					sw.Write("\\nowrap\\posx{0}\\posy{1}\\absw{2}\\absh{3}", pp.X + correctX, pp.Y, (pp.Width - 76 - num) / 2, pp.Height);
					sw.Write("\\brdrr" + text2 + text + "\\fs1");
				}
			}
			if (stiShape.ShapeType is StiHorizontalLineShapeType)
			{
				if (incomingColor.A != 0)
				{
					sw.Write(value);
				}
				if (borderColor.A != 0)
				{
					sw.WriteLine(" \\par}");
					sw.Write("{");
					sw.Write("\\nowrap\\posx{0}\\posy{1}\\absw{2}\\absh{3}", pp.X + correctX, pp.Y, pp.Width - correctW, (pp.Height + num) / 2);
					sw.Write("\\brdrb" + text2 + text + "\\fs1");
				}
			}
			if (stiShape.ShapeType is StiTopAndBottomLineShapeType)
			{
				if (incomingColor.A != 0)
				{
					sw.Write(value);
				}
				sw.Write("\\brdrt" + text2 + text);
				sw.Write("\\brdrb" + text2 + text);
				sw.WriteLine("\\fs1");
			}
			if (stiShape.ShapeType is StiLeftAndRightLineShapeType)
			{
				if (incomingColor.A != 0)
				{
					sw.Write(value);
				}
				sw.Write("\\brdrl" + text2 + text);
				sw.Write("\\brdrr" + text2 + text);
				sw.WriteLine("\\fs1");
			}
			if (stiShape.ShapeType is StiRectangleShapeType)
			{
				if (incomingColor.A != 0)
				{
					sw.Write(value);
				}
				sw.Write("\\box" + text2 + text);
				sw.WriteLine("\\fs1");
			}
		}
		else
		{
			RenderImage12(component, pp.Width - correctW, pp.Height);
		}
	}

	private bool CheckShape2(StiShape shape)
	{
		if (shape.ShapeType is StiVerticalLineShapeType || shape.ShapeType is StiHorizontalLineShapeType || shape.ShapeType is StiTopAndBottomLineShapeType || shape.ShapeType is StiLeftAndRightLineShapeType || shape.ShapeType is StiRectangleShapeType || shape.ShapeType is StiDiagonalDownLineShapeType || shape.ShapeType is StiDiagonalUpLineShapeType || shape.ShapeType is StiOvalShapeType)
		{
			return true;
		}
		return false;
	}

	private void RenderShape2(StiRtfData pp)
	{
		StiComponent component = pp.Component;
		if (!(component is StiShape stiShape))
		{
			return;
		}
		if (CheckShape2(stiShape))
		{
			IStiBrush stiBrush = component as IStiBrush;
			Color tColor = Color.Transparent;
			if (stiBrush != null)
			{
				tColor = StiBrush.ToColor(stiBrush.Brush);
			}
			Color borderColor = stiShape.BorderColor;
			int num = (int)(stiShape.Size * 8400f);
			string text = $"{num}";
			if (stiShape.ShapeType is StiVerticalLineShapeType)
			{
				FillRect(pp, tColor);
				DrawLine(pp.X + pp.Width / 2, pp.Y, pp.X + pp.Width / 2, pp.Y + pp.Height, borderColor, text);
			}
			if (stiShape.ShapeType is StiHorizontalLineShapeType)
			{
				FillRect(pp, tColor);
				DrawLine(pp.X, pp.Y + pp.Height / 2, pp.X + pp.Width, pp.Y + pp.Height / 2, borderColor, text);
			}
			if (stiShape.ShapeType is StiTopAndBottomLineShapeType)
			{
				FillRect(pp, tColor);
				DrawLine(pp.X, pp.Y, pp.X + pp.Width, pp.Y, borderColor, text);
				DrawLine(pp.X, pp.Y + pp.Height, pp.X + pp.Width, pp.Y + pp.Height, borderColor, text);
			}
			if (stiShape.ShapeType is StiLeftAndRightLineShapeType)
			{
				FillRect(pp, tColor);
				DrawLine(pp.X, pp.Y, pp.X, pp.Y + pp.Height, borderColor, text);
				DrawLine(pp.X + pp.Width, pp.Y, pp.X + pp.Width, pp.Y + pp.Height, borderColor, text);
			}
			if (stiShape.ShapeType is StiRectangleShapeType)
			{
				sw.Write("{\\shp{\\*");
				sw.Write("\\shpinst\\shpleft{0}\\shptop{1}\\shpright{2}\\shpbottom{3}", pp.X, pp.Y, pp.X + pp.Width, pp.Y + pp.Height);
				sw.Write("\\shpwr3");
				sw.Write("{\\sp{\\sn shapeType}{\\sv 1}}");
				sw.Write("{\\sp{\\sn fFlipH}{\\sv 0}}");
				sw.Write("{\\sp{\\sn fFlipV}{\\sv 0}}");
				if (tColor.A != 0)
				{
					sw.Write("{\\sp{\\sn fillColor}{\\sv " + $"{tColor.B * 65536 + tColor.G * 256 + tColor.R}" + "}}");
					sw.Write("{\\sp{\\sn fFilled}{\\sv 1}}");
				}
				else
				{
					sw.Write("{\\sp{\\sn fFilled}{\\sv 0}}");
				}
				if (borderColor.A != 0)
				{
					sw.Write("{\\sp{\\sn lineColor}{\\sv " + $"{borderColor.B * 65536 + borderColor.G * 256 + borderColor.R}" + "}}");
					sw.Write("{\\sp{\\sn lineWidth}{\\sv " + text + "}}");
					sw.Write("{\\sp{\\sn fLine}{\\sv 1}}");
				}
				else
				{
					sw.Write("{\\sp{\\sn fLine}{\\sv 0}}");
				}
				sw.WriteLine("}}");
			}
			if (stiShape.ShapeType is StiOvalShapeType)
			{
				sw.Write("{\\shp{\\*");
				sw.Write("\\shpinst\\shpleft{0}\\shptop{1}\\shpright{2}\\shpbottom{3}", pp.X, pp.Y, pp.X + pp.Width, pp.Y + pp.Height);
				sw.Write("\\shpwr3");
				sw.Write("{\\sp{\\sn shapeType}{\\sv 3}}");
				sw.Write("{\\sp{\\sn fFlipH}{\\sv 0}}");
				sw.Write("{\\sp{\\sn fFlipV}{\\sv 0}}");
				if (tColor.A != 0)
				{
					sw.Write("{\\sp{\\sn fillColor}{\\sv " + $"{tColor.B * 65536 + tColor.G * 256 + tColor.R}" + "}}");
					sw.Write("{\\sp{\\sn fFilled}{\\sv 1}}");
				}
				else
				{
					sw.Write("{\\sp{\\sn fFilled}{\\sv 0}}");
				}
				if (borderColor.A != 0)
				{
					sw.Write("{\\sp{\\sn lineColor}{\\sv " + $"{borderColor.B * 65536 + borderColor.G * 256 + borderColor.R}" + "}}");
					sw.Write("{\\sp{\\sn lineWidth}{\\sv " + text + "}}");
					sw.Write("{\\sp{\\sn fLine}{\\sv 1}}");
				}
				else
				{
					sw.Write("{\\sp{\\sn fLine}{\\sv 0}}");
				}
				sw.WriteLine("}}");
			}
			if (stiShape.ShapeType is StiDiagonalDownLineShapeType)
			{
				FillRect(pp, tColor);
				DrawLine(pp.X, pp.Y, pp.X + pp.Width, pp.Y + pp.Height, borderColor, text);
			}
			if (stiShape.ShapeType is StiDiagonalUpLineShapeType)
			{
				FillRect(pp, tColor);
				DrawLine(pp.X, pp.Y + pp.Height, pp.X + pp.Width, pp.Y, borderColor, text);
			}
		}
		else
		{
			RenderImage12(component, pp.Width, pp.Height);
		}
	}

	private StringBuilder CheckArabic(StringBuilder outputString)
	{
		if (StiOptions.Export.RichText.ConvertDigitsToArabic)
		{
			return StiExportUtils.ConvertDigitsToArabic(outputString, StiOptions.Export.RichText.ArabicDigitsType);
		}
		return outputString;
	}

	private StringBuilder UnicodeToRtfString(StringBuilder stInput, bool useRightToLeft)
	{
		if (useRightToLeft)
		{
			stInput = CheckArabic(stInput);
		}
		stInput.Replace("\\", "\\\\");
		stInput.Replace("{", "\\{");
		stInput.Replace("}", "\\}");
		if (stInput.Length > 1 && stInput[0] == '\t')
		{
			stInput.Insert(0, "\t\t");
		}
		stInput.Replace("\n\t", "\n\t\t\t");
		stInput.Replace("\n", "\\par ");
		stInput.Replace("\u0017", "\\");
		stInput.Replace("\u0018", "{\\field{\\*\\fldinst {PAGE}}}");
		stInput.Replace("\u0019", "{\\field{\\*\\fldinst {NUMPAGES}}}");
		stInput.Replace("\u001a", "{\\field{\\*\\fldinst {");
		stInput.Replace("\u001b", " }}{\\fldrslt {");
		stInput.Replace("\u001c", "{\\*\\bkmkstart ");
		stInput.Replace("\u001d", "}{\\*\\bkmkend ");
		stInput.Replace("\u001e", "}");
		string text = stInput.ToString();
		string[] array = null;
		array = ((text.IndexOf('\u0010') != -1) ? text.Split(new char[1] { '\u0010' }) : new string[1] { text });
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			string text2 = array[i];
			if (i != 0)
			{
				baseFontNumber = text2[0] - 256;
				if (baseFontNumber < 0)
				{
					baseFontNumber = 0;
				}
				text2 = text2.Substring(2);
			}
			int j = 0;
			while (j < text2.Length)
			{
				int num = j;
				StringBuilder stringBuilder2 = new StringBuilder();
				int num2 = 1;
				for (; j < text2.Length && StiEncode.unicodeToCodePageArray[(uint)text2[j]] == 0; j++)
				{
					stringBuilder2.Append(text2[j]);
				}
				if (j < text2.Length)
				{
					num2 = StiEncode.unicodeToCodePageArray[(uint)text2[j]];
					stringBuilder2.Append(text2[j]);
					for (j++; j < text2.Length && (StiEncode.unicodeToCodePageArray[(uint)text2[j]] == 0 || StiEncode.unicodeToCodePageArray[(uint)text2[j]] == num2); j++)
					{
						stringBuilder2.Append(text2[j]);
					}
				}
				StringBuilder stringBuilder3 = StiEncode.Encode(stringBuilder2, num2);
				stringBuilder.Append("\\f" + (baseFontNumber + codePageToFont[num2]) + " ");
				for (int k = 0; k < stringBuilder3.Length; k++)
				{
					int num3 = text2[num + k];
					if (stringBuilder3[k] == '?' && StiEncode.unicodeToCodePageArray[num3] < 2)
					{
						stringBuilder.Append("\\u" + num3 + "?");
					}
					else
					{
						stringBuilder.Append(stringBuilder3[k]);
					}
				}
			}
		}
		return stringBuilder;
	}

	private string deleteToken(string inputString, string token)
	{
		int num;
		do
		{
			num = inputString.IndexOf(token, StringComparison.InvariantCulture);
			if (num > -1)
			{
				int i;
				for (i = num + token.Length; i < inputString.Length && char.IsDigit(inputString[i]); i++)
				{
				}
				inputString = inputString.Remove(num, i - num);
			}
		}
		while (num != -1);
		return inputString;
	}

	private string GetRtfString(StiComponent component)
	{
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Expected O, but got Unknown
		string text = string.Empty;
		if (component is StiRichText stiRichText && stiRichText.RtfText != string.Empty)
		{
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			text = stiRichText.RtfText;
			if (text.TrimStart(new char[0]).ToLowerInvariant().IndexOf("rtf") == -1)
			{
				return text;
			}
			int num = text.IndexOf("\\ansicpg");
			int num2 = text.IndexOf("\\'");
			if (num != -1 && num2 != -1 && char.IsDigit(text, num + 8))
			{
				num += 8;
				int i;
				for (i = 0; num + i < text.Length && char.IsDigit(text, num + i); i++)
				{
				}
				int codepage = int.Parse(text.Substring(num, i));
				Encoding encoding = null;
				try
				{
					encoding = Encoding.GetEncoding(codepage);
				}
				catch
				{
				}
				if (encoding != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					num = 0;
					while (num < text.Length)
					{
						if (num < text.Length - 4 && text[num] == '\\' && text[num + 1] == '\'')
						{
							List<byte> list = new List<byte>();
							for (; num < text.Length - 4 && text[num] == '\\' && text[num + 1] == '\''; num += 4)
							{
								int num3 = ParseHexTwoCharToInt(text[num + 2], text[num + 3]);
								if (num3 < 256)
								{
									list.Add((byte)num3);
								}
								else
								{
									list.Add(63);
								}
							}
							if (list.Count > 0)
							{
								stringBuilder.Append(encoding.GetString(list.ToArray()));
							}
						}
						else
						{
							stringBuilder.Append(text[num]);
							if (text[num] == '\\' && num < text.Length - 2 && text[num + 1] == '\\')
							{
								stringBuilder.Append('\\');
								num++;
							}
							num++;
						}
					}
					text = stringBuilder.ToString();
				}
			}
			num = text.IndexOf("{\\fonttbl", StringComparison.InvariantCulture);
			if (num != -1)
			{
				num += 9;
				while (num < text.Length && text[num] != '}')
				{
					for (; num < text.Length && text[num] != '{'; num++)
					{
					}
					int num4 = 0;
					int num5 = num;
					do
					{
						if (text[num5] == '{')
						{
							num4++;
						}
						if (text[num5] == '}')
						{
							num4--;
						}
						num5++;
					}
					while (num4 > 0);
					arrayList.Add(text.Substring(num, num5 - num));
					for (num = num5; num < text.Length && text[num] != '{' && text[num] != '}'; num++)
					{
					}
				}
				text = text.Remove(0, num + 1);
			}
			for (int j = 0; j < arrayList.Count; j++)
			{
				string text2 = (string)arrayList[j];
				int num6;
				do
				{
					num6 = text2.IndexOf("{\\*", StringComparison.InvariantCulture);
					if (num6 == -1)
					{
						continue;
					}
					int num7 = 0;
					int num8 = num6;
					do
					{
						if (text2[num8] == '{')
						{
							num7++;
						}
						if (text2[num8] == '}')
						{
							num7--;
						}
						num8++;
					}
					while (num7 > 0);
					text2 = text2.Remove(num6, num8 - num6);
					if (text2[num6 - 1] != ' ' && text2[num6] != ' ')
					{
						text2 = text2.Insert(num6, " ");
					}
				}
				while (num6 != -1);
				num6 = text2.IndexOf(" ", StringComparison.InvariantCulture);
				int num9 = text2.Length - num6 - 2;
				if (text2[num6 + 1 + num9 - 1] == ';')
				{
					num9--;
				}
				string text3 = text2.Substring(num6 + 1, num9);
				Font incomingFont = new Font(StiFontCollection.GetFontFamily(text3, false), 10f);
				int result = 0;
				num6 = text2.IndexOf("\\fcharset", StringComparison.InvariantCulture);
				if (num6 > 0)
				{
					num6 += 9;
					StringBuilder stringBuilder2 = new StringBuilder();
					for (; char.IsDigit(text2[num6]); num6++)
					{
						stringBuilder2.Append(text2[num6]);
					}
					if (!int.TryParse(stringBuilder2.ToString(), out result))
					{
						result = 0;
					}
				}
				string value = GetFontNumber(fontList, incomingFont, result).ToString();
				arrayList2.Add(value);
				StringBuilder stringBuilder3 = new StringBuilder();
				num6 = text2.IndexOf("\\f", StringComparison.InvariantCulture);
				for (num6 += 2; char.IsDigit(text2[num6]); num6++)
				{
					stringBuilder3.Append(text2[num6]);
				}
				arrayList[j] = stringBuilder3.ToString();
			}
			num = text.IndexOf("{\\colortbl", StringComparison.InvariantCulture);
			if (num != -1)
			{
				num += 10;
				while (num < text.Length && text[num] != '}')
				{
					for (; num < text.Length && text[num] != ';' && text[num] != '\\'; num++)
					{
					}
					int k;
					for (k = num; k < text.Length && text[k] != ';' && text[k] != '}'; k++)
					{
					}
					arrayList3.Add(text.Substring(num, k + 1 - num));
					if (text[k] == ';')
					{
						k++;
					}
					for (num = k; num < text.Length && text[num] != ';' && text[num] != '\\' && text[num] != '}'; num++)
					{
					}
				}
				text = text.Remove(0, num + 1);
			}
			for (int l = 0; l < arrayList3.Count; l++)
			{
				string text4 = (string)arrayList3[l];
				if (text4 != ";")
				{
					int m = text4.IndexOf("\\red", StringComparison.InvariantCulture) + 4;
					StringBuilder stringBuilder4 = new StringBuilder();
					for (; char.IsDigit(text4[m]); m++)
					{
						stringBuilder4.Append(text4[m]);
					}
					string s = stringBuilder4.ToString();
					m = text4.IndexOf("\\green", StringComparison.InvariantCulture) + 6;
					stringBuilder4 = new StringBuilder();
					for (; char.IsDigit(text4[m]); m++)
					{
						stringBuilder4.Append(text4[m]);
					}
					string s2 = stringBuilder4.ToString();
					m = text4.IndexOf("\\blue", StringComparison.InvariantCulture) + 5;
					stringBuilder4 = new StringBuilder();
					for (; char.IsDigit(text4[m]); m++)
					{
						stringBuilder4.Append(text4[m]);
					}
					string s3 = stringBuilder4.ToString();
					Color incomingColor = Color.FromArgb(int.Parse(s), int.Parse(s2), int.Parse(s3));
					arrayList3[l] = GetColorNumber(colorList, incomingColor);
				}
				else
				{
					arrayList3[l] = GetColorNumber(colorList, Color.Transparent);
				}
			}
			num = text.IndexOf("{\\stylesheet", StringComparison.InvariantCulture);
			if (num != -1)
			{
				int num10 = 0;
				int num11 = num;
				do
				{
					if (text[num11] == '{')
					{
						num10++;
					}
					if (text[num11] == '}')
					{
						num10--;
					}
					num11++;
				}
				while (num10 > 0);
				text = text.Remove(0, num11);
			}
			num = text.IndexOf("{\\*\\generator", StringComparison.InvariantCulture);
			if (num != -1)
			{
				int num12 = 0;
				int num13 = num;
				do
				{
					if (text[num13] == '{')
					{
						num12++;
					}
					if (text[num13] == '}')
					{
						num12--;
					}
					num13++;
				}
				while (num12 > 0);
				text = text.Remove(0, num13);
			}
			int num14 = 0;
			do
			{
				num14 = text.IndexOf("\\f", num14, StringComparison.InvariantCulture);
				if (num14 == -1)
				{
					continue;
				}
				if (char.IsDigit(text[num14 + 2]))
				{
					num14 += 2;
					int n = num14;
					StringBuilder stringBuilder5 = new StringBuilder();
					for (; char.IsDigit(text[n]); n++)
					{
						stringBuilder5.Append(text[n]);
					}
					string text5 = stringBuilder5.ToString();
					for (int num15 = 0; num15 < arrayList.Count; num15++)
					{
						if ((string)arrayList[num15] == text5)
						{
							text = text.Substring(0, num14) + (string)arrayList2[num15] + text.Substring(n);
						}
					}
					num14 = n;
				}
				else
				{
					num14 += 2;
				}
			}
			while (num14 != -1);
			if (arrayList3.Count > 0)
			{
				num14 = 0;
				int num16 = 3;
				do
				{
					num14 = text.IndexOf("\\cf", num14, StringComparison.InvariantCulture);
					if (num14 == -1)
					{
						continue;
					}
					if (char.IsDigit(text[num14 + num16]))
					{
						num14 += num16;
						int num17 = num14;
						StringBuilder stringBuilder5 = new StringBuilder();
						for (; char.IsDigit(text[num17]); num17++)
						{
							stringBuilder5.Append(text[num17]);
						}
						int num18 = int.Parse(stringBuilder5.ToString());
						if (num18 < arrayList3.Count)
						{
							text = text.Substring(0, num14) + (string)arrayList3[num18] + text.Substring(num17);
						}
						num14 = num17;
					}
					else
					{
						num14 += num16;
					}
				}
				while (num14 != -1);
				num14 = 0;
				num16 = 6;
				do
				{
					num14 = text.IndexOf("\\cbpat", num14, StringComparison.InvariantCulture);
					if (num14 == -1)
					{
						continue;
					}
					if (char.IsDigit(text[num14 + num16]))
					{
						num14 += num16;
						int num19 = num14;
						StringBuilder stringBuilder5 = new StringBuilder();
						for (; char.IsDigit(text[num19]); num19++)
						{
							stringBuilder5.Append(text[num19]);
						}
						int num20 = int.Parse(stringBuilder5.ToString());
						if (num20 < arrayList3.Count)
						{
							text = text.Substring(0, num14) + (string)arrayList3[num20] + text.Substring(num19);
						}
						num14 = num19;
					}
					else
					{
						num14 += num16;
					}
				}
				while (num14 != -1);
				num14 = 0;
				num16 = 10;
				do
				{
					num14 = text.IndexOf("\\highlight", num14, StringComparison.InvariantCulture);
					if (num14 == -1)
					{
						continue;
					}
					if (char.IsDigit(text[num14 + num16]))
					{
						num14 += num16;
						int num21 = num14;
						StringBuilder stringBuilder5 = new StringBuilder();
						for (; char.IsDigit(text[num21]); num21++)
						{
							stringBuilder5.Append(text[num21]);
						}
						int num22 = int.Parse(stringBuilder5.ToString());
						if (num22 < arrayList3.Count)
						{
							text = text.Substring(0, num14) + (string)arrayList3[num22] + text.Substring(num21);
						}
						num14 = num21;
					}
					else
					{
						num14 += num16;
					}
				}
				while (num14 != -1);
			}
			num = text.Length - 1;
			while (text[num] != '}')
			{
				num--;
			}
			num--;
			while (text[num] == '\r' || text[num] == '\n' || text[num] == ' ')
			{
				num--;
			}
			text = text.Substring(0, num + 1);
			if (text.EndsWith("\\par", StringComparison.InvariantCulture))
			{
				text = text.Substring(0, text.Length - "\\par".Length);
			}
			if (text.EndsWith("\\par }", StringComparison.InvariantCulture))
			{
				text = text.Substring(0, text.Length - "\\par }".Length) + "}";
			}
			text = deleteToken(text, "\\viewkind");
			text = deleteToken(text, "\\uc");
			if (string.IsNullOrWhiteSpace(text))
			{
				return "";
			}
			while (text[0] == '\r' || text[0] == '\n')
			{
				text = text.Remove(0, 1);
			}
			if (text.Substring(0, 5) == "\\pard")
			{
				text = text.Remove(0, 5);
			}
			text = deleteToken(text, "\\formprot");
			text = deleteToken(text, "\\pagebb");
			StringBuilder stringBuilder6 = new StringBuilder();
			for (int num23 = 0; num23 < text.Length; num23++)
			{
				int num24 = text[num23];
				if (num24 > 255)
				{
					stringBuilder6.AppendFormat("\\u{0}{1}", num24, "?");
				}
				else
				{
					stringBuilder6.Append(text[num23]);
				}
			}
			text = stringBuilder6.ToString();
		}
		int num25 = 0;
		int num26 = 0;
		while (num26 < text.Length)
		{
			switch (text[num26++])
			{
			case '{':
				num25++;
				break;
			case '}':
				num25--;
				break;
			case '\\':
				if (num26 < text.Length && (text[num26] == '{' || text[num26] == '}'))
				{
					num26++;
				}
				break;
			}
		}
		if (num25 < 0)
		{
			text = new string('{', -num25) + text;
		}
		if (num25 > 0)
		{
			text += new string('}', num25);
		}
		return text;
	}

	private static int ParseHexTwoCharToInt(char c1, char c2)
	{
		return ParseHexCharToInt(c1) * 16 + ParseHexCharToInt(c2);
	}

	private static int ParseHexCharToInt(char ch)
	{
		if (ch >= '0' && ch <= '9')
		{
			return ch - 48;
		}
		switch (ch)
		{
		case 'A':
		case 'a':
			return 10;
		case 'B':
		case 'b':
			return 11;
		case 'C':
		case 'c':
			return 12;
		case 'D':
		case 'd':
			return 13;
		case 'E':
		case 'e':
			return 14;
		case 'F':
		case 'f':
			return 15;
		default:
			return 256;
		}
	}

	private string MakeHorAlignString(StiTextHorAlignment alignment, bool rightToLeft)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and I4
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Invalid comparison between Unknown and I4
		int num = 0;
		string result = "\\ql" + ((num != 0) ? $"\\ri{num}" : "");
		if (((int)alignment == 0 && rightToLeft) || ((int)alignment == 2 && !rightToLeft))
		{
			result = "\\qr" + ((num != 0) ? $"\\li{num}" : "");
		}
		if ((int)alignment == 1)
		{
			result = "\\qc" + ((num != 0) ? $"\\ri{num}" : "");
		}
		if ((int)alignment == 3)
		{
			result = "\\qj";
		}
		return result;
	}

	private int GetRtfStyleFromComponent(StiComponent component)
	{
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		if (component != null)
		{
			IStiFont stiFont = component as IStiFont;
			IStiTextBrush stiTextBrush = component as IStiTextBrush;
			IStiTextHorAlignment stiTextHorAlignment = component as IStiTextHorAlignment;
			IStiTextOptions stiTextOptions = component as IStiTextOptions;
			StiRtfStyleInfo styleInfo = default(StiRtfStyleInfo);
			styleInfo.Name = component.Name;
			if (component.ComponentStyle != null && component.ComponentStyle.Length > 0)
			{
				styleInfo.Name = component.ComponentStyle;
			}
			if (stiFont != null)
			{
				styleInfo.FontNumber = GetFontNumber(fontList, stiFont.Font);
				styleInfo.FontSize = (int)Math.Round(stiFont.Font.SizeInPoints * 2f, 0);
				styleInfo.Bold = stiFont.Font.Bold;
				styleInfo.Italic = stiFont.Font.Italic;
				styleInfo.Underline = stiFont.Font.Underline;
			}
			if (stiTextBrush != null)
			{
				styleInfo.TextColor = GetColorNumberInt(colorList, StiBrush.ToColor(stiTextBrush.TextBrush));
			}
			if (stiTextHorAlignment != null)
			{
				styleInfo.Alignment = stiTextHorAlignment.HorAlignment;
			}
			if (stiTextOptions != null)
			{
				styleInfo.RightToLeft = stiTextOptions.TextOptions.RightToLeft;
			}
			return GetStyleNumber(styleList, styleInfo);
		}
		return 0;
	}

	private void RenderStartDoc()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		sw.Write("{\\rtf1");
		sw.WriteLine("\\ansi\\ansicpg{0}", 1252);
		sw.WriteLine("{\\fonttbl");
		for (int i = 0; i < fontList.Count; i++)
		{
			Font val = (Font)fontList[i];
			for (int j = 0; j < charsetCount; j++)
			{
				int num = StiEncode.codePagesTable[fontToCodePages[j], 2];
				sw.Write("{");
				sw.Write("\\f{0}\\fcharset{1} {2};", i * charsetCount + j, (num != 1) ? num : 0, val.Name);
				sw.WriteLine("}");
			}
		}
		sw.WriteLine("}");
		sw.WriteLine("{\\colortbl");
		for (int k = 0; k < colorList.Count; k++)
		{
			Color color = (Color)colorList[k];
			if (color == Color.Transparent)
			{
				sw.WriteLine(";");
			}
			else
			{
				sw.WriteLine("\\red{0}\\green{1}\\blue{2};", color.R, color.G, color.B);
			}
		}
		sw.WriteLine("}");
		if (useStyles)
		{
			sw.WriteLine("{\\stylesheet");
			for (int l = 0; l < styleList.Count; l++)
			{
				sw.Write("{");
				StiRtfStyleInfo stiRtfStyleInfo = (StiRtfStyleInfo)styleList[l];
				sw.Write("\\s{0}\\sbasedon0\\snext{0} ", l + 1);
				if (stiRtfStyleInfo.FontNumber != -1)
				{
					sw.Write("\\f{0}", stiRtfStyleInfo.FontNumber);
					sw.Write("\\fs{0}", stiRtfStyleInfo.FontSize);
					if (stiRtfStyleInfo.Bold)
					{
						sw.Write("\\b");
					}
					if (stiRtfStyleInfo.Italic)
					{
						sw.Write("\\i");
					}
					if (stiRtfStyleInfo.Underline)
					{
						sw.Write("\\ul");
					}
				}
				if (stiRtfStyleInfo.TextColor != -1)
				{
					sw.Write("\\cf{0}", stiRtfStyleInfo.TextColor);
				}
				sw.Write(MakeHorAlignString(stiRtfStyleInfo.Alignment, stiRtfStyleInfo.RightToLeft));
				if (stiRtfStyleInfo.RightToLeft)
				{
					sw.Write("\\rtlpar");
				}
				else
				{
					sw.Write("\\ltrpar");
				}
				sw.Write(" {0};", stiRtfStyleInfo.Name);
				sw.WriteLine("}");
			}
			sw.WriteLine("}");
		}
		sw.WriteLine("{\\info{\\doccomm " + StiExportUtils.GetReportVersion(report) + "}}");
		sw.Write("\\viewkind1");
		sw.Write("\\deftab{0}", (int)Math.Round(288.0));
		sw.Write("\\nouicompat");
	}

	private void RenderEndDoc()
	{
		sw.WriteLine("");
		sw.WriteLine("}");
	}

	private void RenderPageHeader(StiPage page)
	{
		bool useTemplatePageSize = StiOptions.Export.RichText.UseTemplatePageSize;
		pageHeight = (int)Math.Round(14.4 * page.Unit.ConvertToHInches(page.PageHeight * (double)(useTemplatePageSize ? 1 : page.SegmentPerHeight)));
		pageWidth = (int)Math.Round(14.4 * page.Unit.ConvertToHInches(page.PageWidth * (double)(useTemplatePageSize ? 1 : page.SegmentPerWidth)));
		int num = (int)Math.Round(14.4 * page.Unit.ConvertToHInches(page.Margins.Left));
		int num2 = (int)Math.Round(14.4 * page.Unit.ConvertToHInches(page.Margins.Right));
		int num3 = (int)Math.Round(14.4 * page.Unit.ConvertToHInches(page.Margins.Top));
		int num4 = (int)Math.Round(14.4 * page.Unit.ConvertToHInches(page.Margins.Bottom)) - StiOptions.Export.RichText.BottomMarginCorrection;
		if (num4 < 0)
		{
			num4 = 0;
		}
		if (pageWidth > 31500)
		{
			pageWidth = 31500;
		}
		if (pageHeight > 31500)
		{
			pageHeight = 31500;
		}
		sw.Write("\\sectd");
		if (page.Orientation == StiPageOrientation.Landscape)
		{
			sw.Write("\\lndscpsxn");
		}
		sw.Write("\\paperw{0}\\paperh{1}", pageWidth, pageHeight);
		sw.WriteLine("\\margl{0}\\margr{1}\\margt{2}\\margb{3}\\headery{2}\\footery{3}", num, num2, num3, num4);
	}

	private void RenderPageFooter()
	{
		if (StiOptions.Export.RichText.UseNewPageCommandInsteadOfNewSection)
		{
			sw.WriteLine("\\page\\par");
		}
		else
		{
			sw.WriteLine("\\sect");
		}
	}

	private void RenderTextAngle1(StiComponent component)
	{
		if (component is IStiTextOptions stiTextOptions)
		{
			float angle = stiTextOptions.TextOptions.Angle;
			int num = 0;
			if (angle > 45f && angle < 135f)
			{
				num = 1;
			}
			if (angle > 225f && angle < 315f)
			{
				num = 2;
			}
			switch (num)
			{
			case 1:
				sw.Write("\\frmtxbtlr");
				break;
			case 2:
				sw.Write("\\frmtxtbrl");
				break;
			default:
				sw.Write("\\frmtxlrtb");
				break;
			}
		}
	}

	private void RenderTextAngle2(StiComponent component)
	{
		if (component is IStiTextOptions stiTextOptions)
		{
			float angle = stiTextOptions.TextOptions.Angle;
			int num = 0;
			if (angle > 45f && angle < 135f)
			{
				num = 1;
			}
			if (angle > 225f && angle < 315f)
			{
				num = 2;
			}
			switch (num)
			{
			case 1:
				sw.Write("\\cltxbtlr");
				break;
			case 2:
				sw.Write("\\cltxtbrl");
				break;
			default:
				sw.Write("\\cltxlrtb");
				break;
			}
		}
	}

	private void RenderHorAlign12(StiComponent component)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		IStiTextHorAlignment stiTextHorAlignment = component as IStiTextHorAlignment;
		bool rightToLeft = component is IStiTextOptions stiTextOptions && stiTextOptions.TextOptions.RightToLeft;
		if (stiTextHorAlignment != null)
		{
			string arg = MakeHorAlignString(stiTextHorAlignment.HorAlignment, rightToLeft);
			sw.Write("{0}", arg);
		}
	}

	private void RenderVerAlign2(StiComponent component)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		if (component is IStiVertAlignment stiVertAlignment)
		{
			if ((int)stiVertAlignment.VertAlignment == 0)
			{
				sw.Write("\\clvertalt");
			}
			if ((int)stiVertAlignment.VertAlignment == 1)
			{
				sw.Write("\\clvertalc");
			}
			if ((int)stiVertAlignment.VertAlignment == 2)
			{
				sw.Write("\\clvertalb");
			}
		}
	}

	private void RenderBorder1(StiComponent component)
	{
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Invalid comparison between Unknown and I4
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Invalid comparison between Unknown and I4
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Invalid comparison between Unknown and I4
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Invalid comparison between Unknown and I4
		if (!(component is IStiBorder stiBorder) || component is IStiIgnoreBorderWhenExport)
		{
			return;
		}
		if (stiBorder.Border is StiAdvancedBorder)
		{
			StiBorder border = stiBorder.Border;
			StiAdvancedBorder val = (StiAdvancedBorder)(object)((border is StiAdvancedBorder) ? border : null);
			if (((StiBorder)val).IsLeftBorderSidePresent && val.LeftSide.Color.A != 0)
			{
				sw.Write("\\brdrl" + GetLineStyle(val.LeftSide, colorList));
			}
			if (((StiBorder)val).IsRightBorderSidePresent && val.RightSide.Color.A != 0)
			{
				sw.Write("\\brdrr" + GetLineStyle(val.RightSide, colorList));
			}
			if (((StiBorder)val).IsTopBorderSidePresent && val.TopSide.Color.A != 0)
			{
				sw.Write("\\brdrt" + GetLineStyle(val.TopSide, colorList));
			}
			if (((StiBorder)val).IsBottomBorderSidePresent && val.BottomSide.Color.A != 0)
			{
				sw.Write("\\brdrb" + GetLineStyle(val.BottomSide, colorList));
			}
		}
		else if (stiBorder.Border.Color.A != 0)
		{
			string lineStyle = GetLineStyle(new StiBorderSide(stiBorder.Border.Color, stiBorder.Border.Size, stiBorder.Border.Style), colorList);
			if ((stiBorder.Border.Side & 2) > 0)
			{
				sw.Write("\\brdrl" + lineStyle);
			}
			if ((stiBorder.Border.Side & 4) > 0)
			{
				sw.Write("\\brdrr" + lineStyle);
			}
			if ((stiBorder.Border.Side & 1) > 0)
			{
				sw.Write("\\brdrt" + lineStyle);
			}
			if ((stiBorder.Border.Side & 8) > 0)
			{
				sw.Write("\\brdrb" + lineStyle);
			}
		}
	}

	private void RenderBorder2(StiComponent component)
	{
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Invalid comparison between Unknown and I4
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Invalid comparison between Unknown and I4
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Invalid comparison between Unknown and I4
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Invalid comparison between Unknown and I4
		if (!(component is IStiBorder stiBorder) || component is IStiIgnoreBorderWhenExport)
		{
			return;
		}
		if (stiBorder.Border is StiAdvancedBorder)
		{
			StiBorder border = stiBorder.Border;
			StiAdvancedBorder val = (StiAdvancedBorder)(object)((border is StiAdvancedBorder) ? border : null);
			if (((StiBorder)val).IsLeftBorderSidePresent && val.LeftSide.Color.A != 0)
			{
				sw.Write("\\clbrdrl" + GetLineStyle(val.LeftSide, colorList));
			}
			if (((StiBorder)val).IsRightBorderSidePresent && val.RightSide.Color.A != 0)
			{
				sw.Write("\\clbrdrr" + GetLineStyle(val.RightSide, colorList));
			}
			if (((StiBorder)val).IsTopBorderSidePresent && val.TopSide.Color.A != 0)
			{
				sw.Write("\\clbrdrt" + GetLineStyle(val.TopSide, colorList));
			}
			if (((StiBorder)val).IsBottomBorderSidePresent && val.BottomSide.Color.A != 0)
			{
				sw.Write("\\clbrdrb" + GetLineStyle(val.BottomSide, colorList));
			}
		}
		else if (stiBorder.Border.Color.A != 0)
		{
			string lineStyle = GetLineStyle(new StiBorderSide(stiBorder.Border.Color, stiBorder.Border.Size, stiBorder.Border.Style), colorList);
			if ((stiBorder.Border.Side & 2) > 0)
			{
				sw.Write("\\clbrdrl" + lineStyle);
			}
			if ((stiBorder.Border.Side & 4) > 0)
			{
				sw.Write("\\clbrdrr" + lineStyle);
			}
			if ((stiBorder.Border.Side & 1) > 0)
			{
				sw.Write("\\clbrdrt" + lineStyle);
			}
			if ((stiBorder.Border.Side & 8) > 0)
			{
				sw.Write("\\clbrdrb" + lineStyle);
			}
		}
	}

	private string RenderBorder2Table(int rowIndex, int columnIndex, int height, int width, bool returnValue)
	{
		bool flag = true;
		bool flag2 = true;
		for (int i = 0; i < height + 1; i++)
		{
			if (matrix.BordersY[rowIndex - 1 + i, columnIndex - 1] == (StiBorderSide)null)
			{
				flag = false;
			}
			if (matrix.BordersY[rowIndex - 1 + i, columnIndex - 1 + width + 1] == (StiBorderSide)null)
			{
				flag2 = false;
			}
		}
		bool flag3 = true;
		bool flag4 = true;
		for (int j = 0; j < width + 1; j++)
		{
			if (matrix.BordersX[rowIndex - 1, columnIndex - 1 + j] == (StiBorderSide)null)
			{
				flag3 = false;
			}
			if (matrix.BordersX[rowIndex - 1 + height + 1, columnIndex - 1 + j] == (StiBorderSide)null)
			{
				flag4 = false;
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (flag)
		{
			stringBuilder.Append("\\clbrdrl" + GetLineStyle(matrix.BordersY[rowIndex - 1, columnIndex - 1], colorList));
		}
		if (flag2)
		{
			stringBuilder.Append("\\clbrdrr" + GetLineStyle(matrix.BordersY[rowIndex - 1, columnIndex - 1 + width + 1], colorList));
		}
		if (flag3)
		{
			stringBuilder.Append("\\clbrdrt" + GetLineStyle(matrix.BordersX[rowIndex - 1, columnIndex - 1], colorList));
		}
		if (flag4)
		{
			stringBuilder.Append("\\clbrdrb" + GetLineStyle(matrix.BordersX[rowIndex - 1 + height + 1, columnIndex - 1], colorList));
		}
		if (returnValue)
		{
			return stringBuilder.ToString();
		}
		if (stringBuilder.Length > 0)
		{
			sw.Write(stringBuilder.ToString());
		}
		return null;
	}

	private StiBorderSides RenderBorder2TableGetValues(int rowIndex, int columnIndex, ref string styles)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		StiBorderSides val = (StiBorderSides)0;
		StringBuilder stringBuilder = new StringBuilder();
		if (matrix.BordersY[rowIndex - 1, columnIndex - 1] != (StiBorderSide)null)
		{
			val = (StiBorderSides)(val | 2);
		}
		if (matrix.BordersY[rowIndex - 1, columnIndex - 1 + 1] != (StiBorderSide)null)
		{
			val = (StiBorderSides)(val | 4);
		}
		if (matrix.BordersX[rowIndex - 1, columnIndex - 1] != (StiBorderSide)null)
		{
			stringBuilder.Append("\\clbrdrt" + GetLineStyle(matrix.BordersX[rowIndex - 1, columnIndex - 1], colorList));
			val = (StiBorderSides)(val | 1);
		}
		if (matrix.BordersX[rowIndex - 1 + 1, columnIndex - 1] != (StiBorderSide)null)
		{
			stringBuilder.Append("\\clbrdrb" + GetLineStyle(matrix.BordersX[rowIndex - 1 + 1, columnIndex - 1], colorList));
			val = (StiBorderSides)(val | 8);
		}
		styles = stringBuilder.ToString();
		return val;
	}

	private void RenderBrush1(StiComponent component)
	{
		if (component is IStiBrush stiBrush && !component.IsExportAsImage(StiExportFormat.Rtf))
		{
			Color incomingColor = StiBrush.ToColor(stiBrush.Brush);
			if (incomingColor.A != 0)
			{
				string colorNumber = GetColorNumber(colorList, incomingColor);
				sw.Write("\\cbpat{0}", colorNumber);
			}
		}
	}

	private void RenderBrush2(StiComponent component)
	{
		if (component is IStiBrush stiBrush && !component.IsExportAsImage(StiExportFormat.Rtf))
		{
			Color color = StiBrush.ToColor(stiBrush.Brush);
			sw.Write("{\\sp{\\sn fillColor}{\\sv ");
			sw.Write("{0}", color.B * 65536 + color.G * 256 + color.R);
			sw.Write("}}");
			if (color.A != 0)
			{
				sw.Write("{\\sp{\\sn fFilled}{\\sv 1}} ");
			}
			else
			{
				sw.Write("{\\sp{\\sn fFilled}{\\sv 0}} ");
			}
		}
	}

	private void RenderTextBrush12(StiComponent component)
	{
		if (component is IStiTextBrush stiTextBrush)
		{
			string colorNumber = GetColorNumber(colorList, StiBrush.ToColor(stiTextBrush.TextBrush));
			sw.Write("\\cf{0}", colorNumber);
		}
	}

	private void RenderTextFont12(StiComponent component)
	{
		if (component is IStiFont stiFont)
		{
			baseFontNumber = GetFontNumber(fontList, stiFont.Font);
			sw.Write("\\f{0}", baseFontNumber);
			sw.Write("\\fs{0}", Math.Round(stiFont.Font.SizeInPoints * 2f));
			if (stiFont.Font.Bold)
			{
				sw.Write("\\b");
			}
			if (stiFont.Font.Italic)
			{
				sw.Write("\\i");
			}
			if (stiFont.Font.Underline)
			{
				sw.Write("\\ul");
			}
			sw.Write($"\\sl{Math.Round(240.0 * StiOptions.Export.RichText.LineSpacing * (double)((component is StiText) ? (component as StiText).LineSpacing : 1f))}\\slmult1");
			if (StiOptions.Export.RichText.SpaceBetweenCharacters != 0)
			{
				sw.Write($"\\expndtw{StiOptions.Export.RichText.SpaceBetweenCharacters}");
			}
		}
	}

	private void RenderText12(StiComponent component)
	{
		IStiText stiText = component as IStiText;
		IStiTextOptions stiTextOptions = component as IStiTextOptions;
		if (stiText != null && !component.IsExportAsImage(StiExportFormat.Rtf))
		{
			string text = stiText.Text;
			if (component is StiText stiText2 && stiText2.CheckAllowHtmlTags())
			{
				text = ConvertTextWithHtmlTagsToRtfText(stiText2, text);
			}
			if (component is StiText && (component as StiText).TextQuality == StiTextQuality.Wysiwyg && !string.IsNullOrEmpty(text) && text.EndsWith(StiTextRenderer.StiForceWidthAlignTag))
			{
				text = text.Substring(0, text.Length - StiTextRenderer.StiForceWidthAlignTag.Length);
			}
			StringBuilder stInput = new StringBuilder(text);
			bool useRightToLeft = stiTextOptions != null && stiTextOptions.TextOptions != null && stiTextOptions.TextOptions.RightToLeft;
			StringBuilder stringBuilder = UnicodeToRtfString(stInput, useRightToLeft);
			byte[] array = new byte[stringBuilder.Length];
			for (int i = 0; i < stringBuilder.Length; i++)
			{
				array[i] = (byte)stringBuilder[i];
			}
			sw.Flush();
			sw2.Write(array, 0, array.Length);
		}
	}

	private void RenderImage12(StiComponent component, int absw, int absh)
	{
		if (component is IStiExportImage stiExportImage)
		{
			float zoom = imageResolution;
			IStiExportImageExtended stiExportImageExtended = stiExportImage as IStiExportImageExtended;
			Image val = null;
			if (component.IsExportAsImage(StiExportFormat.Rtf))
			{
				val = ((stiExportImageExtended == null || !stiExportImageExtended.IsExportAsImage(StiExportFormat.Rtf)) ? stiExportImage.GetImage(ref zoom) : stiExportImageExtended.GetImage(ref zoom, imageFormat));
			}
			if (val != null)
			{
				sw.Write(GetImageString(val, zoom, absw, absh));
				val.Dispose();
			}
		}
	}

	private void RenderRtf12(StiComponent component)
	{
		if (component is StiRichText)
		{
			string value = GetRtfString(component).Replace("\\pard", "");
			sw.Write(value);
		}
	}

	private void RenderImage3(StiComponent component, StringBuilder dataStr, int absw, int absh)
	{
		if (component is IStiExportImageExtended stiExportImageExtended)
		{
			float zoom = imageResolution;
			IStiExportImageExtended stiExportImageExtended2 = stiExportImageExtended;
			Image val = null;
			if (component.IsExportAsImage(StiExportFormat.Rtf))
			{
				val = ((stiExportImageExtended2 == null || !stiExportImageExtended2.IsExportAsImage(StiExportFormat.Rtf)) ? stiExportImageExtended.GetImage(ref zoom) : stiExportImageExtended.GetImage(ref zoom, imageFormat));
			}
			if (val != null)
			{
				dataStr.Append(GetImageString(val, zoom, absw, absh));
				val.Dispose();
			}
		}
	}

	private void RenderRtf3(StiComponent component, StringBuilder dataStr)
	{
		if (component is StiRichText)
		{
			string value = GetRtfString(component).Replace("\\pard", "\\par");
			dataStr.Append(value);
		}
	}

	private void RenderText3(StiComponent component, StringBuilder dataStr)
	{
		IStiText stiText = component as IStiText;
		IStiTextOptions stiTextOptions = component as IStiTextOptions;
		if (stiText != null && !component.IsExportAsImage(StiExportFormat.Rtf))
		{
			string text = stiText.Text;
			if (component is StiText && (component as StiText).TextQuality == StiTextQuality.Wysiwyg && !string.IsNullOrEmpty(text) && text.EndsWith(StiTextRenderer.StiForceWidthAlignTag))
			{
				text = text.Substring(0, text.Length - StiTextRenderer.StiForceWidthAlignTag.Length);
			}
			StringBuilder stInput = new StringBuilder(text);
			bool useRightToLeft = stiTextOptions != null && stiTextOptions.TextOptions != null && stiTextOptions.TextOptions.RightToLeft;
			StringBuilder value = UnicodeToRtfString(stInput, useRightToLeft);
			dataStr.Append((object?)value);
		}
	}

	private void RenderTextFont3(StiComponent component, StringBuilder dataStr)
	{
		if (component is IStiFont stiFont)
		{
			dataStr.Append("\\f");
			baseFontNumber = GetFontNumber(fontList, stiFont.Font);
			dataStr.Append(baseFontNumber.ToString());
			dataStr.Append("\\fs");
			dataStr.Append(Math.Round(stiFont.Font.SizeInPoints * 2f));
			if (stiFont.Font.Bold)
			{
				dataStr.Append("\\b");
			}
			if (stiFont.Font.Italic)
			{
				dataStr.Append("\\i");
			}
			if (stiFont.Font.Underline)
			{
				dataStr.Append("\\ul");
			}
			dataStr.Append($"\\sl{Math.Round(240.0 * StiOptions.Export.RichText.LineSpacing * (double)((component is StiText) ? (component as StiText).LineSpacing : 1f))}\\slmult1");
			if (StiOptions.Export.RichText.SpaceBetweenCharacters != 0)
			{
				dataStr.Append($"\\expndtw{StiOptions.Export.RichText.SpaceBetweenCharacters}");
			}
		}
	}

	private void RenderTextBrush3(StiComponent component, StringBuilder dataStr)
	{
		if (component is IStiTextBrush stiTextBrush)
		{
			string colorNumber = GetColorNumber(colorList, StiBrush.ToColor(stiTextBrush.TextBrush));
			dataStr.Append("\\cf");
			dataStr.Append(colorNumber);
		}
	}

	private void RenderBrush3(StiComponent component, StringBuilder dataStr)
	{
		if (component is IStiBrush stiBrush && !component.IsExportAsImage(StiExportFormat.Rtf))
		{
			string colorNumber = GetColorNumber(colorList, StiBrush.ToColor(stiBrush.Brush));
			dataStr.Append("\\cbpat");
			dataStr.Append(colorNumber);
		}
	}

	private void RenderBorder3(StiComponent component, StringBuilder dataStr)
	{
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Invalid comparison between Unknown and I4
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Invalid comparison between Unknown and I4
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Invalid comparison between Unknown and I4
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Invalid comparison between Unknown and I4
		if (!(component is IStiBorder stiBorder) || component is IStiIgnoreBorderWhenExport)
		{
			return;
		}
		if (stiBorder.Border is StiAdvancedBorder)
		{
			StiBorder border = stiBorder.Border;
			StiAdvancedBorder val = (StiAdvancedBorder)(object)((border is StiAdvancedBorder) ? border : null);
			if (((StiBorder)val).IsLeftBorderSidePresent && val.LeftSide.Color.A != 0)
			{
				dataStr.Append("\\brdrl" + GetLineStyle(val.LeftSide, colorList));
			}
			if (((StiBorder)val).IsRightBorderSidePresent && val.RightSide.Color.A != 0)
			{
				dataStr.Append("\\brdrr" + GetLineStyle(val.RightSide, colorList));
			}
			if (((StiBorder)val).IsTopBorderSidePresent && val.TopSide.Color.A != 0)
			{
				dataStr.Append("\\brdrt" + GetLineStyle(val.TopSide, colorList));
			}
			if (((StiBorder)val).IsBottomBorderSidePresent && val.BottomSide.Color.A != 0)
			{
				dataStr.Append("\\brdrb" + GetLineStyle(val.BottomSide, colorList));
			}
		}
		else if (stiBorder.Border.Color.A != 0)
		{
			string lineStyle = GetLineStyle(new StiBorderSide(stiBorder.Border.Color, stiBorder.Border.Size, stiBorder.Border.Style), colorList);
			if ((stiBorder.Border.Side & 2) > 0)
			{
				dataStr.Append("\\brdrl" + lineStyle);
			}
			if ((stiBorder.Border.Side & 4) > 0)
			{
				dataStr.Append("\\brdrr" + lineStyle);
			}
			if ((stiBorder.Border.Side & 1) > 0)
			{
				dataStr.Append("\\brdrt" + lineStyle);
			}
			if ((stiBorder.Border.Side & 8) > 0)
			{
				dataStr.Append("\\brdrb" + lineStyle);
			}
		}
	}

	private void RenderStyle12(StiComponent component)
	{
		StiText stiText = component as StiText;
		if (useStyles && stiText != null)
		{
			int rtfStyleFromComponent = GetRtfStyleFromComponent(component);
			sw.Write("\\s{0}", rtfStyleFromComponent);
		}
	}

	private void writeFromMatrix(int beginLine, int endLine, bool outHeadersAndFooters)
	{
		//IL_0f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4f: Invalid comparison between Unknown and I4
		//IL_0f51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af8: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f73: Invalid comparison between Unknown and I4
		//IL_1b68: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b22: Invalid comparison between Unknown and I4
		//IL_1b29: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b2f: Invalid comparison between Unknown and I4
		//IL_1b25: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b36: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b3c: Invalid comparison between Unknown and I4
		//IL_1b32: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b3f: Unknown result type (might be due to invalid IL or missing references)
		int count = Matrix.CoordX.Count;
		int[] array = new int[Matrix.CoordX.Count];
		for (int i = 1; i < Matrix.CoordX.Count; i++)
		{
			double num = (double)Matrix.CoordX.GetByIndex(i);
			array[i] = (int)Math.Round(num * 14.4);
		}
		int[,] array2 = new int[Matrix.CoordY.Count, Matrix.CoordX.Count];
		int[,] array3 = new int[Matrix.CoordY.Count, Matrix.CoordX.Count];
		string text = string.Empty;
		int num2 = 0;
		base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
		double num3 = Math.Max((float)Matrix.CoordY.Count / 200f, 1f);
		int num4 = 0;
		for (int j = 1; j < Matrix.CoordY.Count; j++)
		{
			int num5 = (int)((double)j / num3);
			if (num5 > num4)
			{
				num4 = num5;
				InvokeExporting(j, matrix.CoordY.Count, base.CurrentPassNumber, base.MaximumPassNumber);
			}
			int num6 = (int)Math.Round(((double)Matrix.CoordY.GetByIndex(j) - (double)Matrix.CoordY.GetByIndex(j - 1)) * 14.4);
			bool flag = j - 1 >= beginLine && j - 1 <= endLine && num2 == 0;
			if (num2 > 0)
			{
				num2--;
			}
			if (!outHeadersAndFooters && (matrix.LinePlacement[j - 1] == StiMatrix.StiTableLineInfo.PageHeader || matrix.LinePlacement[j - 1] == StiMatrix.StiTableLineInfo.PageFooter || matrix.LinePlacement[j - 1] == StiMatrix.StiTableLineInfo.Trash))
			{
				flag = false;
			}
			double num7 = 100005.0;
			ArrayList arrayList = new ArrayList();
			bool flag2 = false;
			bool flag3 = StiOptions.Export.RichText.UseCanBreakProperty;
			if (flag)
			{
				for (int k = 1; k < count; k++)
				{
					StiCell stiCell = Matrix.Cells[j - 1, k - 1];
					if (array2[j, k] < 0 || stiCell == null || stiCell.Component == null)
					{
						continue;
					}
					if (stiCell.Component.TagValue != null)
					{
						string text2 = stiCell.Component.TagValue.ToString().ToLowerInvariant();
						if (text2.IndexOf("rtfparagraph", StringComparison.InvariantCulture) != -1)
						{
							arrayList.Add(stiCell);
							flag = false;
						}
						if (text2.IndexOf("rtfnewpage", StringComparison.InvariantCulture) != -1)
						{
							flag2 = true;
						}
					}
					if (stiCell.Component is IStiBreakable { CanBreak: false })
					{
						flag3 = false;
					}
					StiMargins stiMargins = null;
					if (stiCell.Component is StiText)
					{
						stiMargins = (stiCell.Component as StiText).Margins;
					}
					if (stiCell.Component is StiRichText)
					{
						stiMargins = (stiCell.Component as StiRichText).Margins;
					}
					if (stiMargins != null)
					{
						if (stiMargins.Top < num7)
						{
							num7 = stiMargins.Top;
						}
						if (stiCell.Height > 0)
						{
							num7 = 0.0;
						}
					}
				}
			}
			if (flag2)
			{
				sw.WriteLine("}\\fs2\\page\\par{");
			}
			if (!usePageHeadersAndFooters)
			{
				flag3 = false;
			}
			num7 *= 14.4;
			int num8 = (int)Math.Round(num7);
			if (flag)
			{
				if (usePageHeadersAndFooters && matrix.LinePlacement[j - 1] == StiMatrix.StiTableLineInfo.HeaderAP)
				{
					string text3 = matrix.ParentBandName[j - 1];
					int num9 = text3.IndexOf('\u001f');
					if (num9 != -1)
					{
						text3 = text3.Substring(0, num9);
					}
					if (text3 != text)
					{
						text = text3;
						sw.WriteLine("}\\fs2\\par{");
					}
				}
				bool flag4 = (usePageHeadersAndFooters && matrix.LinePlacement[j - 1] != 0) || !StiOptions.Export.RichText.LineHeightExactly;
				sw.Write("\\trowd");
				if (!usePageHeadersAndFooters || StiOptions.Export.RichText.ForceLineHeight)
				{
					sw.Write("\\trrh{0}{1}", flag4 ? "" : "-", num6);
				}
				if (!flag3)
				{
					sw.Write("\\trkeep");
				}
				sw.Write("\\ltrrow");
				if (usePageHeadersAndFooters && matrix.LinePlacement[j - 1] == StiMatrix.StiTableLineInfo.HeaderAP)
				{
					sw.Write("\\trhdr");
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num10 = 0;
			for (int l = 1; l < count; l++)
			{
				StiCell stiCell2 = Matrix.Cells[j - 1, l - 1];
				if (array2[j, l] >= 0)
				{
					num10++;
					stringBuilder.Append("{");
					if (stiCell2 != null)
					{
						StiCellStyle cellStyle = stiCell2.CellStyle;
						bool flag5 = false;
						if (cellStyle.TextOptions != null)
						{
							flag5 = cellStyle.TextOptions.RightToLeft;
						}
						Font font = cellStyle.Font;
						baseFontNumber = GetFontNumber(fontList, font);
						if (useStyles)
						{
							int rtfStyleFromComponent = GetRtfStyleFromComponent(stiCell2.Component);
							stringBuilder.Append($"\\s{rtfStyleFromComponent}");
						}
						if (flag5)
						{
							stringBuilder.Append("\\rtlpar");
						}
						else
						{
							stringBuilder.Append("\\ltrpar");
						}
						stringBuilder.Append(MakeHorAlignString(cellStyle.HorAlignment, flag5));
						string colorNumber = GetColorNumber(colorList, cellStyle.TextColor);
						stringBuilder.Append($"\\cf{colorNumber}");
						stringBuilder.Append($"\\f{baseFontNumber}");
						stringBuilder.Append($"\\fs{Math.Round(font.SizeInPoints * 2f)}");
						if (font.Bold)
						{
							stringBuilder.Append("\\b");
						}
						if (font.Italic)
						{
							stringBuilder.Append("\\i");
						}
						if (font.Underline)
						{
							stringBuilder.Append("\\ul");
						}
						stringBuilder.Append($"\\sl{Math.Round(240.0 * StiOptions.Export.RichText.LineSpacing * (double)((stiCell2.Component is StiText) ? (stiCell2.Component as StiText).LineSpacing : 1f))}\\slmult1");
						if (StiOptions.Export.RichText.SpaceBetweenCharacters != 0)
						{
							stringBuilder.Append($"\\expndtw{StiOptions.Export.RichText.SpaceBetweenCharacters}");
						}
						string text4 = Matrix.Bookmarks[j - 1, l - 1];
						if (text4 != null && flag && !usedBookmarks.ContainsKey(text4))
						{
							usedBookmarks[text4] = text4;
							string text5 = ConvertStringToBookmark(text4);
							stringBuilder.Append((object?)UnicodeToRtfString(new StringBuilder("\u001c" + text5 + "\u001d" + text5 + "\u001e"), useRightToLeft: false));
						}
						if (!(stiCell2.Component is StiRichText))
						{
							string text6 = stiCell2.Text;
							if (text6 != null && text6.Length > 0 && !stiCell2.Component.IsExportAsImage(StiExportFormat.Rtf))
							{
								StiText stiText = stiCell2.Component as StiText;
								StringBuilder stringBuilder2 = new StringBuilder(text6);
								if (stiText != null && stiText.CheckAllowHtmlTags())
								{
									stringBuilder2 = new StringBuilder(ConvertTextWithHtmlTagsToRtfText(stiText, text6));
								}
								if (usePageHeadersAndFooters && stiCell2.Component.TagValue is string { Length: >0 } text7)
								{
									if (StiOptions.Export.RichText.UsePageRefField && bookmarkList.ContainsKey(text7))
									{
										stringBuilder2 = new StringBuilder("\u001aPAGEREF " + ConvertStringToBookmark(text7) + "\u001b" + stiCell2.Text + "\u001e\u001e\u001e");
									}
									else if (text7.ToLowerInvariant().IndexOf("rtfnewpage", StringComparison.InvariantCulture) == -1 && (text7.IndexOf("#PageNumber#", StringComparison.InvariantCulture) != -1 || text7.IndexOf("#TotalPageCount#", StringComparison.InvariantCulture) != -1 || text7.IndexOf("#PageRef", StringComparison.InvariantCulture) != -1))
									{
										stringBuilder2 = new StringBuilder(text7);
										stringBuilder2.Replace("#PageNumber#", "\u0018");
										stringBuilder2.Replace("#TotalPageCount#", "\u0019");
										int startIndex = 0;
										while ((startIndex = text7.IndexOf("#PageRef", startIndex, StringComparison.InvariantCulture)) != -1)
										{
											int m;
											for (m = startIndex + 8; m < text7.Length && text7[m] != '#'; m++)
											{
											}
											string text8 = ConvertStringToBookmark(text7.Substring(startIndex + 8, m - (startIndex + 8)));
											stringBuilder2.Replace(text7.Substring(startIndex, m + 1 - startIndex), "\u001aPAGEREF " + text7 + "\u001b" + stiCell2.Text + "\u001e\u001e\u001e");
											startIndex += 8;
										}
									}
								}
								if (stiCell2.Component.HyperlinkValue != null)
								{
									string text9 = stiCell2.Component.HyperlinkValue.ToString().Trim();
									if (text9.Length > 0 && !text9.StartsWith("javascript:"))
									{
										string text10 = stringBuilder2.ToString();
										if (text10.Length == 0)
										{
											text10 = text9;
										}
										if (text9.StartsWith("#", StringComparison.InvariantCulture))
										{
											text9 = text9.Substring(1);
											stringBuilder2 = new StringBuilder("\u001aHYPERLINK \\l \"" + ConvertStringToBookmark(text9) + "\"\u001b" + text10 + "\u001e\u001e\u001e");
										}
										else
										{
											stringBuilder2 = new StringBuilder("\u001aHYPERLINK \"" + text9 + "\"\u001b" + text10 + "\u001e\u001e\u001e");
										}
									}
								}
								StringBuilder arg = UnicodeToRtfString(stringBuilder2, flag5);
								stringBuilder.Append($" {arg}");
							}
						}
						if (stiCell2.Component is StiRichText)
						{
							if (stiCell2.Component is StiRichText)
							{
								StringBuilder stringBuilder3 = new StringBuilder(ReplacePardInRtf(GetRtfString(stiCell2.Component)));
								if (stiCell2.Component.HyperlinkValue != null)
								{
									string text11 = stiCell2.Component.HyperlinkValue.ToString().Trim();
									if (text11.Length > 0 && !text11.StartsWith("javascript:"))
									{
										string text12 = stringBuilder3.ToString();
										if (text12.Length == 0)
										{
											text12 = text11;
										}
										if (text11.StartsWith("#", StringComparison.InvariantCulture))
										{
											text11 = text11.Substring(1);
											stringBuilder3 = new StringBuilder("\u001aHYPERLINK \\\\l \"" + ConvertStringToBookmark(text11) + "\"\u001b" + text12 + "\u001e\u001e\u001e");
										}
										else
										{
											stringBuilder3 = new StringBuilder("\u001aHYPERLINK \"" + text11 + "\"\u001b" + text12 + "\u001e\u001e\u001e");
										}
									}
								}
								stringBuilder3.Replace("\u0017", "\\");
								stringBuilder3.Replace("\u0018", "{\\field{\\*\\fldinst {PAGE}}}");
								stringBuilder3.Replace("\u0019", "{\\field{\\*\\fldinst {NUMPAGES}}}");
								stringBuilder3.Replace("\u001a", "{\\field{\\*\\fldinst {");
								stringBuilder3.Replace("\u001b", " }}{\\fldrslt {");
								stringBuilder3.Replace("\u001c", "{\\*\\bkmkstart ");
								stringBuilder3.Replace("\u001d", "}{\\*\\bkmkend ");
								stringBuilder3.Replace("\u001e", "}");
								stringBuilder.Append((object?)stringBuilder3);
							}
						}
						else if (stiCell2.Component is IStiExportImage stiExportImage)
						{
							float zoom = imageResolution;
							IStiExportImageExtended stiExportImageExtended = stiExportImage as IStiExportImageExtended;
							Image val = null;
							if (stiCell2.Component.IsExportAsImage(StiExportFormat.Rtf))
							{
								val = ((stiExportImageExtended == null || !stiExportImageExtended.IsExportAsImage(StiExportFormat.Rtf)) ? stiExportImage.GetImage(ref zoom) : stiExportImageExtended.GetImage(ref zoom, imageFormat));
							}
							if (val != null)
							{
								Image realImageData = Matrix.GetRealImageData(stiCell2, val);
								if (realImageData != null)
								{
									val = realImageData;
								}
								string text13 = GetImageString(val, zoom, (int)Math.Round(((double)Matrix.CoordX.GetByIndex(l + stiCell2.Width) - (double)Matrix.CoordX.GetByIndex(l - 1)) * 14.4), (int)Math.Round(((double)Matrix.CoordY.GetByIndex(j + stiCell2.Height) - (double)Matrix.CoordY.GetByIndex(j - 1)) * 14.4));
								val.Dispose();
								if (stiCell2.Component.HyperlinkValue != null)
								{
									string text14 = stiCell2.Component.HyperlinkValue.ToString().Trim();
									if (text14.Length > 0 && !text14.StartsWith("javascript:"))
									{
										string text15 = text13;
										if (text14.StartsWith("#", StringComparison.InvariantCulture))
										{
											text14 = text14.Substring(1);
											text13 = "{\\field{\\*\\fldinst {HYPERLINK \\\\l \"" + ConvertStringToBookmark(text14) + "\" }}{\\fldrslt {" + text15 + "}}}";
										}
										else
										{
											text13 = "{\\field{\\*\\fldinst {HYPERLINK \"" + text14 + "\" }}{\\fldrslt {" + text15 + "}}}";
										}
									}
								}
								stringBuilder.Append(text13);
								stringBuilder.Append("\\f0\\fs2");
							}
						}
						if (array3[j, l] != 2)
						{
							for (int n = 0; n <= stiCell2.Height; n++)
							{
								for (int num11 = 0; num11 <= stiCell2.Width; num11++)
								{
									array2[j + n, l + num11] = -1;
								}
								array2[j + n, l] = stiCell2.Width;
							}
							if (stiCell2.Height > 0)
							{
								array3[j, l] = 1;
								for (int num12 = 1; num12 <= stiCell2.Height; num12++)
								{
									array3[j + num12, l] = 2;
								}
							}
						}
					}
					else if (array3[j, l] == 0 && StiOptions.Export.RichText.ForceEmptyCellsOptimization)
					{
						string styles = null;
						StiBorderSides val2 = RenderBorder2TableGetValues(j, l, ref styles);
						if ((val2 & 4) == 0)
						{
							int num13 = 0;
							while (l + num13 + 1 < count && Matrix.Cells[j - 1, l + num13 + 1 - 1] == null && array2[j, l + num13 + 1] == 0 && array3[j, l + num13 + 1] == 0)
							{
								string styles2 = null;
								StiBorderSides val3 = RenderBorder2TableGetValues(j, l + num13 + 1, ref styles2);
								if ((val3 & 2) > 0 || (val3 & 9) != (val2 & 9) || styles != styles2)
								{
									break;
								}
								num13++;
								if ((val3 & 4) > 0)
								{
									break;
								}
							}
							if (num13 > 0)
							{
								for (int num14 = 1; num14 <= num13; num14++)
								{
									array2[j, l + num14] = -1;
								}
								array2[j, l] = num13;
							}
						}
					}
					stringBuilder.Append("\\cell}");
				}
				if (num10 >= 64)
				{
					break;
				}
			}
			if (flag)
			{
				num10 = 0;
				for (int num15 = 1; num15 < count; num15++)
				{
					if (array3[j, num15] == 1)
					{
						sw.Write("\\clvmgf");
					}
					if (array3[j, num15] == 2)
					{
						sw.Write("\\clvmrg");
					}
					if (array2[j, num15] >= 0)
					{
						num10++;
						if (array3[j, num15] == 2)
						{
							int num16 = j;
							while (array3[num16, num15] == 2)
							{
								num16--;
							}
							StiCell stiCell3 = Matrix.Cells[num16 - 1, num15 - 1];
							RenderBorder2Table(num16, num15, stiCell3.Height, stiCell3.Width, returnValue: false);
						}
						else
						{
							StiCell stiCell4 = Matrix.Cells[j - 1, num15 - 1];
							if (stiCell4 != null)
							{
								StiCellStyle cellStyle2 = stiCell4.CellStyle;
								RenderVerAlign2(stiCell4.Component);
								RenderBorder2Table(j, num15, stiCell4.Height, stiCell4.Width, returnValue: false);
								RenderTextAngle2(stiCell4.Component);
								if (!stiCell4.Component.IsExportAsImage(StiExportFormat.Rtf))
								{
									Color color = cellStyle2.Color;
									if (color.A != 0)
									{
										string colorNumber2 = GetColorNumber(colorList, color);
										sw.Write("\\clcbpat{0}", colorNumber2);
									}
									StiText stiText2 = stiCell4.Component as StiText;
									StiRichText stiRichText2 = stiCell4.Component as StiRichText;
									if (stiText2 != null || stiRichText2 != null)
									{
										int rightMarginCorrection = StiOptions.Export.RichText.RightMarginCorrection;
										int num17 = 0;
										int num18 = 0;
										int num19 = 0;
										if (stiText2 != null)
										{
											num17 = (int)Math.Round(stiText2.Margins.Left * 14.4);
											num18 = (int)Math.Round((stiText2.Margins.Right - (double)rightMarginCorrection) * 14.4);
											num19 = (int)Math.Round(stiText2.Margins.Top * 14.4);
										}
										if (stiRichText2 != null)
										{
											num17 = (int)Math.Round(stiRichText2.Margins.Left * 14.4);
											num18 = (int)Math.Round((stiRichText2.Margins.Right - (double)rightMarginCorrection) * 14.4);
											num19 = (int)Math.Round(stiRichText2.Margins.Top * 14.4);
										}
										if (num19 > num8)
										{
											num19 = num8;
										}
										if (num17 > 0)
										{
											sw.Write("\\clpadt{0}\\clpadft3", num17);
										}
										if (num18 > 0)
										{
											sw.Write("\\clpadr{0}\\clpadfr3", num18);
										}
										if (num19 > 0)
										{
											sw.Write("\\clpadl{0}\\clpadfl3", num19);
										}
									}
								}
							}
							else
							{
								RenderBorder2Table(j, num15, 0, array2[j, num15], returnValue: false);
							}
						}
					}
					if (array2[j, num15] >= 0)
					{
						sw.Write("\\cellx{0}", array[num15 + array2[j, num15]]);
					}
					if (num10 >= 64)
					{
						break;
					}
				}
				sw.Write("\\pard\\intbl ");
				byte[] array4 = new byte[stringBuilder.Length];
				for (int num20 = 0; num20 < stringBuilder.Length; num20++)
				{
					array4[num20] = (byte)stringBuilder[num20];
				}
				sw.Flush();
				sw2.Write(array4, 0, array4.Length);
				sw.WriteLine("\\row");
			}
			foreach (StiCell item in arrayList)
			{
				sw.Write("\\pard{");
				stringBuilder = new StringBuilder();
				num2 = item.Height;
				string text16 = item.Text;
				if (!(item.Component is StiRichText) && text16 != null && text16.Length > 0 && !item.Component.IsExportAsImage(StiExportFormat.Rtf))
				{
					StiCellStyle cellStyle3 = item.CellStyle;
					bool flag6 = false;
					if (cellStyle3.TextOptions != null)
					{
						flag6 = cellStyle3.TextOptions.RightToLeft;
					}
					Font font2 = cellStyle3.Font;
					baseFontNumber = GetFontNumber(fontList, font2);
					if (useStyles)
					{
						int rtfStyleFromComponent2 = GetRtfStyleFromComponent(item.Component);
						stringBuilder.Append($"\\s{rtfStyleFromComponent2}");
					}
					if (flag6)
					{
						stringBuilder.Append("\\rtlpar");
					}
					else
					{
						stringBuilder.Append("\\ltrpar");
					}
					stringBuilder.Append(MakeHorAlignString(cellStyle3.HorAlignment, flag6));
					string colorNumber3 = GetColorNumber(colorList, cellStyle3.TextColor);
					stringBuilder.Append($"\\cf{colorNumber3}");
					stringBuilder.Append($"\\f{baseFontNumber}");
					stringBuilder.Append($"\\fs{Math.Round(font2.SizeInPoints * 2f)}");
					if (font2.Bold)
					{
						stringBuilder.Append("\\b");
					}
					if (font2.Italic)
					{
						stringBuilder.Append("\\i");
					}
					if (font2.Underline)
					{
						stringBuilder.Append("\\ul");
					}
					stringBuilder.Append($"\\sl{Math.Round(240.0 * StiOptions.Export.RichText.LineSpacing * (double)((item.Component is StiText) ? (item.Component as StiText).LineSpacing : 1f))}\\slmult1");
					if (StiOptions.Export.RichText.SpaceBetweenCharacters != 0)
					{
						stringBuilder.Append($"\\expndtw{StiOptions.Export.RichText.SpaceBetweenCharacters}");
					}
					StringBuilder stringBuilder4 = new StringBuilder(text16);
					if (item.Component is StiText stiText3 && stiText3.CheckAllowHtmlTags())
					{
						stringBuilder4 = new StringBuilder(ConvertTextWithHtmlTagsToRtfText(stiText3, text16));
					}
					if (item.Component.HyperlinkValue != null)
					{
						string text17 = item.Component.HyperlinkValue.ToString().Trim();
						if (text17.Length > 0 && !text17.StartsWith("javascript:"))
						{
							string text18 = stringBuilder4.ToString();
							if (text18.Length == 0)
							{
								text18 = text17;
							}
							if (text17.StartsWith("#", StringComparison.InvariantCulture))
							{
								text17 = text17.Substring(1);
								stringBuilder4 = new StringBuilder("\u001aHYPERLINK \\\\l \"" + ConvertStringToBookmark(text17) + "\"\u001b" + text18 + "\u001e\u001e\u001e");
							}
							else
							{
								stringBuilder4 = new StringBuilder("\u001aHYPERLINK \"" + text17 + "\"\u001b" + text18 + "\u001e\u001e\u001e");
							}
						}
					}
					StringBuilder arg2 = UnicodeToRtfString(stringBuilder4, flag6);
					stringBuilder.Append($" {arg2}");
				}
				if (item.Component is StiRichText)
				{
					if (item.Component is StiRichText)
					{
						StringBuilder stringBuilder5 = new StringBuilder(GetRtfString(item.Component));
						if (item.Component.HyperlinkValue != null)
						{
							string text19 = item.Component.HyperlinkValue.ToString().Trim();
							if (text19.Length > 0 && !text19.StartsWith("javascript:"))
							{
								string text20 = stringBuilder5.ToString();
								if (text20.Length == 0)
								{
									text20 = text19;
								}
								if (text19.StartsWith("#", StringComparison.InvariantCulture))
								{
									text19 = text19.Substring(1);
									stringBuilder5 = new StringBuilder("\u001aHYPERLINK \\\\l \"" + ConvertStringToBookmark(text19) + "\"\u001b" + text20 + "\u001e\u001e\u001e");
								}
								else
								{
									stringBuilder5 = new StringBuilder("\u001aHYPERLINK \"" + text19 + "\"\u001b" + text20 + "\u001e\u001e\u001e");
								}
							}
						}
						stringBuilder5.Replace("\u0017", "\\");
						stringBuilder5.Replace("\u0018", "{\\field{\\*\\fldinst {PAGE}}}");
						stringBuilder5.Replace("\u0019", "{\\field{\\*\\fldinst {NUMPAGES}}}");
						stringBuilder5.Replace("\u001a", "{\\field{\\*\\fldinst {");
						stringBuilder5.Replace("\u001b", " }}{\\fldrslt {");
						stringBuilder5.Replace("\u001c", "{\\*\\bkmkstart ");
						stringBuilder5.Replace("\u001d", "}{\\*\\bkmkend ");
						stringBuilder5.Replace("\u001e", "}");
						stringBuilder.Append((object?)stringBuilder5);
					}
				}
				else
				{
					if (item.Component is IStiExportImage stiExportImage2)
					{
						float zoom2 = imageResolution;
						IStiExportImageExtended stiExportImageExtended2 = stiExportImage2 as IStiExportImageExtended;
						Image val4 = null;
						if (item.Component.IsExportAsImage(StiExportFormat.Rtf))
						{
							val4 = ((stiExportImageExtended2 == null || !stiExportImageExtended2.IsExportAsImage(StiExportFormat.Rtf)) ? stiExportImage2.GetImage(ref zoom2) : stiExportImageExtended2.GetImage(ref zoom2, imageFormat));
						}
						if (val4 != null)
						{
							Image realImageData2 = Matrix.GetRealImageData(item, val4);
							if (realImageData2 != null)
							{
								val4 = realImageData2;
							}
							string text21 = GetImageString(val4, zoom2, (int)Math.Round(((double)Matrix.CoordX.GetByIndex(item.Left + 1 + item.Width) - (double)Matrix.CoordX.GetByIndex(item.Left)) * 14.4), (int)Math.Round(((double)Matrix.CoordY.GetByIndex(item.Top + 1 + item.Height) - (double)Matrix.CoordY.GetByIndex(item.Top)) * 14.4));
							val4.Dispose();
							if (item.Component.HyperlinkValue != null)
							{
								string text22 = item.Component.HyperlinkValue.ToString().Trim();
								if (text22.Length > 0 && !text22.StartsWith("javascript:"))
								{
									string text23 = text21;
									if (text22.StartsWith("#", StringComparison.InvariantCulture))
									{
										text22 = text22.Substring(1);
										text21 = "{\\field{\\*\\fldinst {HYPERLINK \\\\l \"" + ConvertStringToBookmark(text22) + "\" }}{\\fldrslt {" + text23 + "}}}";
									}
									else
									{
										text21 = "{\\field{\\*\\fldinst {HYPERLINK \"" + text22 + "\" }}{\\fldrslt {" + text23 + "}}}";
									}
								}
							}
							StiTextHorAlignment alignment = item.CellStyle.HorAlignment;
							if (!(item.Component is IStiTextHorAlignment) && item.Component is IStiHorAlignment stiHorAlignment)
							{
								if ((int)stiHorAlignment.HorAlignment == 1)
								{
									alignment = (StiTextHorAlignment)0;
								}
								if ((int)stiHorAlignment.HorAlignment == 2)
								{
									alignment = (StiTextHorAlignment)1;
								}
								if ((int)stiHorAlignment.HorAlignment == 3)
								{
									alignment = (StiTextHorAlignment)2;
								}
							}
							bool rightToLeft = false;
							if (item.CellStyle.TextOptions != null)
							{
								rightToLeft = item.CellStyle.TextOptions.RightToLeft;
							}
							stringBuilder.Append(MakeHorAlignString(alignment, rightToLeft));
							stringBuilder.Append(text21);
						}
					}
					stringBuilder.Append("\\f0\\fs2");
				}
				byte[] array5 = new byte[stringBuilder.Length];
				for (int num21 = 0; num21 < stringBuilder.Length; num21++)
				{
					array5[num21] = (byte)stringBuilder[num21];
				}
				sw.Flush();
				sw2.Write(array5, 0, array5.Length);
				if (!stringBuilder.ToString().Trim().EndsWith("\\par"))
				{
					sw.Write("\\par");
				}
				sw.WriteLine("}");
			}
		}
	}

	public void ExportRtf(StiReport report, string fileName)
	{
		StiFileUtils.ProcessReadOnly(fileName);
		FileStream fileStream = new FileStream(fileName, FileMode.Create);
		ExportRtf(report, fileStream);
		fileStream.Flush();
		fileStream.Close();
	}

	public void ExportRtf(StiReport report, Stream stream)
	{
		StiRtfExportSettings settings = new StiRtfExportSettings();
		ExportRtf(report, stream, settings);
	}

	public void ExportRtf(StiReport report, Stream stream, StiRtfExportMode exportMode)
	{
		StiRtfExportSettings stiRtfExportSettings = new StiRtfExportSettings();
		stiRtfExportSettings.ExportMode = exportMode;
		ExportRtf(report, stream, stiRtfExportSettings);
	}

	public void ExportRtf(StiReport report, Stream stream, StiRtfExportMode exportMode, StiPagesRange pageRange)
	{
		StiRtfExportSettings stiRtfExportSettings = new StiRtfExportSettings();
		stiRtfExportSettings.PageRange = pageRange;
		stiRtfExportSettings.ExportMode = exportMode;
		ExportRtf(report, stream, stiRtfExportSettings);
	}

	public void ExportRtf(StiReport report, Stream stream, StiRtfExportMode exportMode, StiPagesRange pageRange, bool usePageHeadersAndFooters)
	{
		StiRtfExportSettings stiRtfExportSettings = new StiRtfExportSettings();
		stiRtfExportSettings.PageRange = pageRange;
		stiRtfExportSettings.ExportMode = exportMode;
		stiRtfExportSettings.UsePageHeadersAndFooters = usePageHeadersAndFooters;
		ExportRtf(report, stream, stiRtfExportSettings);
	}

	public void ExportRtf(StiReport report, Stream stream, int codePage, StiRtfExportMode exportMode, StiPagesRange pageRange)
	{
		StiRtfExportSettings stiRtfExportSettings = new StiRtfExportSettings();
		stiRtfExportSettings.PageRange = pageRange;
		stiRtfExportSettings.CodePage = codePage;
		stiRtfExportSettings.ExportMode = exportMode;
		ExportRtf(report, stream, stiRtfExportSettings);
	}

	public void ExportRtf(StiReport report, Stream stream, int codePage, StiRtfExportMode exportMode, StiPagesRange pageRange, bool usePageHeadersAndFooters)
	{
		StiRtfExportSettings stiRtfExportSettings = new StiRtfExportSettings();
		stiRtfExportSettings.PageRange = pageRange;
		stiRtfExportSettings.CodePage = codePage;
		stiRtfExportSettings.ExportMode = exportMode;
		stiRtfExportSettings.UsePageHeadersAndFooters = usePageHeadersAndFooters;
		ExportRtf(report, stream, stiRtfExportSettings);
	}

	public void ExportRtf(StiReport report, Stream stream, StiRtfExportSettings settings)
	{
		try
		{
			ExportRtf1(report, stream, settings);
		}
		finally
		{
			StiExportUtils.EnableFontSmoothing(report);
		}
	}

	private void ExportRtf1(StiReport report, Stream stream, StiRtfExportSettings settings)
	{
		//IL_152e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1534: Unknown result type (might be due to invalid IL or missing references)
		//IL_1536: Invalid comparison between Unknown and I4
		//IL_156b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1571: Unknown result type (might be due to invalid IL or missing references)
		//IL_1573: Invalid comparison between Unknown and I4
		//IL_0fb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc0: Invalid comparison between Unknown and I4
		//IL_0fd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdf: Invalid comparison between Unknown and I4
		//IL_11ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c2: Invalid comparison between Unknown and I4
		StiLogService.Write(((object)this).GetType(), "Export report to Rtf format");
		if (settings == null)
		{
			throw new ArgumentNullException("The 'settings' argument cannot be equal in null.");
		}
		int codePage = settings.CodePage;
		StiRtfExportMode exportMode = settings.ExportMode;
		StiPagesRange pageRange = settings.PageRange;
		usePageHeadersAndFooters = settings.UsePageHeadersAndFooters;
		removeEmptySpaceAtBottom = settings.RemoveEmptySpaceAtBottom;
		imageResolution = settings.ImageResolution;
		imageQuality = settings.ImageQuality;
		bool storeImagesAsPng = settings.StoreImagesAsPng;
		unicodeMapArray = new int[65536];
		codePageToFont = new byte[10];
		fontToCodePages = new int[10];
		charsetCount = 0;
		useStyles = true;
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
		imageResolution /= 100f;
		imageFormat = (storeImagesAsPng ? StiExportFormat.ImagePng : StiExportFormat.Rtf);
		double num = 1.0;
		double num2 = 1.0;
		colorList = new ArrayList();
		fontList = new ArrayList();
		styleList = new ArrayList();
		sw = new StreamWriter(stream, Encoding.GetEncoding(1252));
		sw2 = stream;
		for (int i = 0; i < 65536; i++)
		{
			unicodeMapArray[i] = 0;
		}
		imageCodec = StiImageCodecInfo.GetImageCodec("image/jpeg");
		bookmarkList = new Hashtable();
		usedBookmarks = new Hashtable();
		base.CurrentPassNumber = 0;
		base.MaximumPassNumber = ((exportMode == StiRtfExportMode.Table) ? 4 : 2) + (StiOptions.Export.RichText.DivideSegmentPages ? 1 : 0);
		StiPagesCollection stiPagesCollection = pageRange.GetSelectedPages(report.RenderedPages);
		if (StiOptions.Export.RichText.DivideSegmentPages)
		{
			stiPagesCollection = StiSegmentPagesDivider.Divide(stiPagesCollection, this);
			base.CurrentPassNumber++;
		}
		base.StatusString = StiLocalization.Get("Report", "PreparingReport");
		foreach (StiPage item in stiPagesCollection)
		{
			stiPagesCollection.GetPage(item);
			InvokeExporting(item, stiPagesCollection, base.CurrentPassNumber, base.MaximumPassNumber);
			foreach (StiComponent component5 in item.Components)
			{
				if (!component5.Enabled)
				{
					continue;
				}
				if (component5 is IStiTextBrush stiTextBrush)
				{
					GetColorNumberInt(colorList, StiBrush.ToColor(stiTextBrush.TextBrush));
				}
				if (component5 is IStiBrush stiBrush)
				{
					GetColorNumber(colorList, StiBrush.ToColor(stiBrush.Brush));
				}
				if (component5 is IStiBorder stiBorder && !(component5 is IStiIgnoreBorderWhenExport))
				{
					StiBorder border = stiBorder.Border;
					StiAdvancedBorder val = (StiAdvancedBorder)(object)((border is StiAdvancedBorder) ? border : null);
					if (val != (StiAdvancedBorder)null)
					{
						GetColorNumber(colorList, val.LeftSide.Color);
						GetColorNumber(colorList, val.RightSide.Color);
						GetColorNumber(colorList, val.TopSide.Color);
						GetColorNumber(colorList, val.BottomSide.Color);
					}
					else
					{
						GetColorNumber(colorList, stiBorder.Border.Color);
					}
				}
				if (component5 is StiShape stiShape)
				{
					GetColorNumber(colorList, stiShape.BorderColor);
				}
				if (component5 is IStiFont stiFont)
				{
					GetFontNumber(fontList, stiFont.Font);
				}
				if (component5 is StiRichText stiRichText)
				{
					GetRtfString(component5);
					GetColorNumber(colorList, stiRichText.BackColor);
				}
				if (component5 is StiText stiText)
				{
					StringBuilder outputString = new StringBuilder(stiText.Text);
					if (stiText.CheckAllowHtmlTags())
					{
						outputString = new StringBuilder(ConvertTextWithHtmlTagsToRtfText(stiText, stiText.Text));
					}
					StringBuilder stringBuilder = CheckArabic(outputString);
					for (int j = 0; j < stringBuilder.Length; j++)
					{
						unicodeMapArray[(uint)stringBuilder[j]] = 1;
					}
					GetRtfStyleFromComponent(component5);
				}
				if (StiOptions.Export.RichText.UsePageRefField && component5.BookmarkValue != null)
				{
					string text = component5.BookmarkValue.ToString();
					if (text != null && text.Length > 0 && !bookmarkList.ContainsKey(text))
					{
						bookmarkList.Add(text, text);
					}
				}
			}
		}
		if (exportMode == StiRtfExportMode.Table)
		{
			GetFontNumber(fontList, "Arial");
		}
		for (int k = 0; k < 65536; k++)
		{
			if (unicodeMapArray[k] != 0)
			{
				codePageToFont[StiEncode.unicodeToCodePageArray[k]] = 1;
			}
		}
		codePageToFont[1] = 1;
		for (int l = 1; l < 10; l++)
		{
			if (codePageToFont[l] != 0)
			{
				codePageToFont[l] = charsetCount;
				fontToCodePages[charsetCount] = l - 1;
				charsetCount++;
			}
		}
		codePageToFont[0] = codePageToFont[1];
		for (int m = 0; m < styleList.Count; m++)
		{
			StiRtfStyleInfo stiRtfStyleInfo = (StiRtfStyleInfo)styleList[m];
			stiRtfStyleInfo.FontNumber *= charsetCount;
			styleList[m] = stiRtfStyleInfo;
		}
		RenderStartDoc();
		base.CurrentPassNumber++;
		if (exportMode == StiRtfExportMode.Table)
		{
			sw.WriteLine("\\nolead");
			sw.WriteLine("\\afs16");
			StiPagesCollection stiPagesCollection2 = stiPagesCollection;
			stiPagesCollection = null;
			bool flag = false;
			for (int n = 0; n < stiPagesCollection2.Count; n++)
			{
				stiPagesCollection = new StiPagesCollection(report, report.RenderedPages);
				stiPagesCollection.CacheMode = report.RenderedPages.CacheMode;
				stiPagesCollection.AddV2Internal(stiPagesCollection2.GetPageWithoutCache(n));
				string excelSheetValue = stiPagesCollection2.GetPageWithoutCache(n).ExcelSheetValue;
				while (n < stiPagesCollection2.Count - 1 && CompareExcellSheetNames(stiPagesCollection2.GetPageWithoutCache(n + 1).ExcelSheetValue, excelSheetValue) && stiPagesCollection2.GetPageWithoutCache(n + 1).Orientation == stiPagesCollection2.GetPageWithoutCache(n).Orientation)
				{
					n++;
					stiPagesCollection.AddV2Internal(stiPagesCollection2.GetPageWithoutCache(n));
				}
				StiPage stiPage2 = stiPagesCollection[0];
				if (stiPage2 != null)
				{
					stiPagesCollection.GetPage(stiPage2);
					if (!StiOptions.Export.RichText.UseNewPageCommandInsteadOfNewSection || n == 0)
					{
						RenderPageHeader(stiPage2);
					}
					bool checkForExcel = StiOptions.Export.RichText.DivideBigCells && !StiOptions.Export.RichText.UseCanBreakProperty;
					if (usePageHeadersAndFooters)
					{
						StiPagesCollection stiPagesCollection3 = new StiPagesCollection(report, report.RenderedPages);
						stiPagesCollection3.CacheMode = report.RenderedPages.CacheMode;
						stiPagesCollection3.AddV2Internal(stiPagesCollection[0]);
						matrix = new StiMatrix(stiPagesCollection3, checkForExcel, this);
						if (base.IsStopped)
						{
							return;
						}
						matrix.ScanComponentsPlacement(optimize: false, exportObjectFormatting: true);
						bool flag2 = false;
						int num3 = matrix.CoordY.Count - 1 - 1;
						while (matrix.LinePlacement[num3] != StiMatrix.StiTableLineInfo.PageHeader && num3 > 0)
						{
							num3--;
						}
						int num4;
						if (matrix.LinePlacement[num3] == StiMatrix.StiTableLineInfo.PageHeader)
						{
							for (num4 = 0; matrix.LinePlacement[num4] != StiMatrix.StiTableLineInfo.PageHeader; num4++)
							{
							}
							sw.WriteLine("{");
							sw.WriteLine("{\\header ");
							writeFromMatrix(num4, num3, outHeadersAndFooters: true);
							sw.WriteLine("}");
							flag = true;
						}
						else
						{
							bool flag3 = false;
							if (stiPagesCollection.Count > 1 && stiPagesCollection[1] != null)
							{
								StiMatrix stiMatrix = matrix;
								stiPagesCollection3 = new StiPagesCollection(report, report.RenderedPages);
								stiPagesCollection3.CacheMode = report.RenderedPages.CacheMode;
								stiPagesCollection3.AddV2Internal(stiPagesCollection[1]);
								matrix = new StiMatrix(stiPagesCollection3, checkForExcel, this);
								if (base.IsStopped)
								{
									return;
								}
								matrix.ScanComponentsPlacement(optimize: false, exportObjectFormatting: true);
								num3 = matrix.CoordY.Count - 1 - 1;
								while (matrix.LinePlacement[num3] != StiMatrix.StiTableLineInfo.PageHeader && num3 > 0)
								{
									num3--;
								}
								if (matrix.LinePlacement[num3] == StiMatrix.StiTableLineInfo.PageHeader)
								{
									for (num4 = 0; matrix.LinePlacement[num4] != StiMatrix.StiTableLineInfo.PageHeader; num4++)
									{
									}
									StiPrintOnType stiPrintOnType = StiPrintOnType.AllPages;
									bool flag4 = false;
									for (int num5 = num4; num5 < num3; num5++)
									{
										for (int num6 = 0; num6 < Matrix.CoordX.Count - 1; num6++)
										{
											StiCell stiCell = Matrix.Cells[num5, num6];
											if (stiCell != null && stiCell.Component != null)
											{
												stiPrintOnType = stiCell.Component.PrintOn;
												flag4 = true;
												break;
											}
										}
										if (flag4)
										{
											break;
										}
									}
									if (flag4 && (stiPrintOnType == StiPrintOnType.ExceptFirstAndLastPage || stiPrintOnType == StiPrintOnType.ExceptFirstPage))
									{
										sw.WriteLine("\\titlepg ");
										flag2 = true;
									}
									sw.WriteLine("{");
									sw.WriteLine("{\\header ");
									writeFromMatrix(num4, num3, outHeadersAndFooters: true);
									sw.WriteLine("}");
									flag = true;
									flag3 = true;
								}
								else
								{
									sw.WriteLine("{");
								}
								matrix = stiMatrix;
							}
							else
							{
								sw.WriteLine("{");
							}
							if (!flag3 && flag)
							{
								sw.WriteLine("{\\header \\fs2 }");
							}
						}
						for (num4 = 0; matrix.LinePlacement[num4] != StiMatrix.StiTableLineInfo.PageFooter && num4 < matrix.CoordY.Count - 1 - 1; num4++)
						{
						}
						if (matrix.LinePlacement[num4] == StiMatrix.StiTableLineInfo.PageFooter)
						{
							num3 = matrix.CoordY.Count - 1 - 1;
							while (matrix.LinePlacement[num3] != StiMatrix.StiTableLineInfo.PageFooter)
							{
								num3--;
							}
							if (flag2)
							{
								sw.WriteLine("{\\footerf ");
								writeFromMatrix(num4, num3, outHeadersAndFooters: true);
								sw.WriteLine("}");
							}
							sw.WriteLine("{\\footer ");
							writeFromMatrix(num4, num3, outHeadersAndFooters: true);
							sw.WriteLine("}");
							flag = true;
						}
						else if (flag)
						{
							sw.WriteLine("{\\footer \\fs2 }");
						}
						matrix = new StiMatrix(stiPagesCollection, checkForExcel, this);
						matrix.ScanComponentsPlacement(optimize: true, exportObjectFormatting: true);
						num3 = matrix.CoordY.Count - 1 - 1;
						base.CurrentPassNumber = (StiOptions.Export.RichText.DivideSegmentPages ? 4 : 3);
						writeFromMatrix(0, num3, outHeadersAndFooters: false);
					}
					else
					{
						matrix = new StiMatrix(stiPagesCollection, checkForExcel, this);
						if (base.IsStopped)
						{
							return;
						}
						sw.WriteLine("{");
						base.CurrentPassNumber = (StiOptions.Export.RichText.DivideSegmentPages ? 4 : 3);
						writeFromMatrix(0, matrix.CoordY.Count - 1 - 1, outHeadersAndFooters: true);
					}
					sw.WriteLine("\\pard\\fs2 \\par");
					sw.WriteLine("}");
				}
				if (n < stiPagesCollection2.Count - 1)
				{
					RenderPageFooter();
				}
			}
		}
		else
		{
			base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
			sw.WriteLine("\\nolead");
			foreach (StiPage item2 in stiPagesCollection)
			{
				stiPagesCollection.GetPage(item2);
				InvokeExporting(item2, stiPagesCollection, base.CurrentPassNumber, base.MaximumPassNumber);
				if (base.IsStopped)
				{
					return;
				}
				RenderPageHeader(item2);
				sw.WriteLine("{");
				int num7 = (int)Math.Round((double)pageHeight / (12.0 / num2)) + 2;
				SortedList[] array = new SortedList[num7];
				for (int num8 = 0; num8 < num7; num8++)
				{
					array[num8] = new SortedList();
				}
				Hashtable hashtable = new Hashtable();
				foreach (StiComponent component6 in item2.Components)
				{
					if (!component6.Enabled)
					{
						continue;
					}
					double num9 = item2.Unit.ConvertToHInches(component6.Left);
					double num10 = item2.Unit.ConvertToHInches(component6.Top);
					double num11 = item2.Unit.ConvertToHInches(component6.Right);
					double num12 = item2.Unit.ConvertToHInches(component6.Bottom);
					StiRtfData stiRtfData = default(StiRtfData);
					stiRtfData.X = (int)Math.Round(num9 * 14.4 * num);
					stiRtfData.Y = (int)Math.Round(num10 * 14.4 * num2);
					stiRtfData.Width = (int)Math.Round(num11 * 14.4 * num) - stiRtfData.X;
					stiRtfData.Height = (int)Math.Round(num12 * 14.4 * num2) - stiRtfData.Y;
					stiRtfData.Component = component6;
					stiRtfData.X++;
					stiRtfData.Y++;
					string text2;
					while (true)
					{
						text2 = $"{stiRtfData.X}:{stiRtfData.Y}:{stiRtfData.Width}:{stiRtfData.Height}";
						if (!hashtable.Contains(text2))
						{
							break;
						}
						stiRtfData.Width++;
					}
					hashtable[text2] = text2;
					int num13 = stiRtfData.X;
					int num14 = (int)Math.Round(num10 / (12.0 / num2));
					if (num14 < 0)
					{
						num14 = 0;
					}
					for (; array[num14].IndexOfKey(num13) != -1; num13++)
					{
					}
					array[num14].Add(num13, stiRtfData);
				}
				switch (exportMode)
				{
				case StiRtfExportMode.Frame:
				{
					sw.WriteLine("\\nowidctlpar\\phmrg\\pvmrg\\nowrap");
					for (int num18 = 0; num18 < array.GetLength(0); num18++)
					{
						if (array[num18].Count <= 0)
						{
							continue;
						}
						sw.Write("\\pard");
						for (int num19 = 0; num19 < array[num18].Count; num19++)
						{
							StiRtfData pp = (StiRtfData)array[num18].GetByIndex(num19);
							StiComponent component2 = pp.Component;
							if (component2.Width == 0.0 || component2.Height == 0.0 || component2 is StiLinePrimitive)
							{
								continue;
							}
							int num20 = 0;
							int num21 = 0;
							if (component2 is IStiBorder stiBorder2 && !(component2 is IStiIgnoreBorderWhenExport))
							{
								if ((stiBorder2.Border.Side & 2) > 0)
								{
									num20 += 38;
									num21 += 38;
								}
								if ((stiBorder2.Border.Side & 4) > 0)
								{
									num21 += 38;
								}
							}
							sw.Write("{");
							sw.Write("\\nowrap\\posx{0}\\posy{1}\\absw{2}\\absh{3}", pp.X + num20, pp.Y, pp.Width - num21, pp.Height);
							RenderTextAngle1(component2);
							RenderBorder1(component2);
							RenderBrush1(component2);
							RenderStyle12(component2);
							RenderHorAlign12(component2);
							RenderTextBrush12(component2);
							RenderTextFont12(component2);
							if (component2 is StiText)
							{
								RenderText12(component2);
							}
							if (component2 is StiRichText)
							{
								RenderRtf12(component2);
							}
							else if (component2 is StiShape)
							{
								RenderShape1(pp, num20, num21);
							}
							else
							{
								RenderImage12(component2, pp.Width - num21, pp.Height);
							}
							sw.WriteLine(" \\par}");
						}
					}
					for (int num22 = 0; num22 < array.GetLength(0); num22++)
					{
						if (array[num22].Count <= 0)
						{
							continue;
						}
						sw.Write("\\pard");
						for (int num23 = 0; num23 < array[num22].Count; num23++)
						{
							StiRtfData stiRtfData3 = (StiRtfData)array[num22].GetByIndex(num23);
							StiComponent component3 = stiRtfData3.Component;
							if (component3.Width != 0.0 && component3.Height != 0.0 && component3 is StiLinePrimitive)
							{
								int num24 = 0;
								if (component3 is IStiBorder stiBorder3 && !(component3 is IStiIgnoreBorderWhenExport) && (stiBorder3.Border.Side & 2) > 0)
								{
									num24 += 38;
								}
								sw.Write("{");
								sw.Write("\\nowrap\\posx{0}\\posy{1}\\absw{2}\\absh{3}", stiRtfData3.X + num24, stiRtfData3.Y, stiRtfData3.Width, stiRtfData3.Height);
								RenderBorder1(component3);
								sw.WriteLine("\\fs0\\par}");
							}
						}
					}
					break;
				}
				case StiRtfExportMode.WinWord:
				{
					for (int num25 = 0; num25 < array.GetLength(0); num25++)
					{
						if (array[num25].Count <= 0)
						{
							continue;
						}
						for (int num26 = 0; num26 < array[num25].Count; num26++)
						{
							StiRtfData pp2 = (StiRtfData)array[num25].GetByIndex(num26);
							StiComponent component4 = pp2.Component;
							StiShape stiShape2 = component4 as StiShape;
							if (component4 is StiShape && stiShape2 != null && CheckShape2(stiShape2))
							{
								RenderShape2(pp2);
								continue;
							}
							sw.Write("{\\shp{\\*");
							sw.Write("\\shpinst\\shpleft{0}\\shptop{1}\\shpright{2}\\shpbottom{3}", pp2.X, pp2.Y, pp2.X + pp2.Width, pp2.Y + pp2.Height);
							sw.Write("\\shpwr3");
							sw.Write("{\\sp{\\sn dxTextLeft}{\\sv 0}}");
							sw.Write("{\\sp{\\sn dyTextTop}{\\sv 0}}");
							sw.Write("{\\sp{\\sn dxTextRight}{\\sv 0}}");
							sw.Write("{\\sp{\\sn dyTextBottom}{\\sv 0}}");
							RenderBrush2(component4);
							sw.Write("{\\sp{\\sn fLine}{\\sv 0}}");
							sw.Write("{\\shptxt\\trowd\\trqc");
							sw.Write("\\trrh-{0}", pp2.Height);
							RenderVerAlign2(component4);
							RenderBorder2(component4);
							RenderTextAngle2(component4);
							sw.Write("\\cellx{0}\\intbl", pp2.Width);
							RenderHorAlign12(component4);
							RenderTextBrush12(component4);
							RenderTextFont12(component4);
							if (!(component4 is StiRichText))
							{
								RenderText12(component4);
							}
							if (component4 is StiRichText)
							{
								RenderRtf12(component4);
							}
							else
							{
								RenderImage12(component4, pp2.Width, pp2.Height);
							}
							sw.WriteLine("\\cell\\row\\pard\\par}}}");
						}
					}
					break;
				}
				case StiRtfExportMode.TabbedText:
				{
					for (int num15 = 0; num15 < array.GetLength(0); num15++)
					{
						if (array[num15].Count > 0)
						{
							sw.Write("\\pard{");
						}
						if (array[num15].Count > 0)
						{
							StringBuilder stringBuilder2 = new StringBuilder();
							StringBuilder stringBuilder3 = new StringBuilder();
							for (int num16 = 0; num16 < array[num15].Count; num16++)
							{
								StiRtfData stiRtfData2 = (StiRtfData)array[num15].GetByIndex(num16);
								StiComponent component = stiRtfData2.Component;
								int value = stiRtfData2.X;
								if (component is IStiTextHorAlignment stiTextHorAlignment)
								{
									if ((stiTextHorAlignment.HorAlignment & 1) > 0)
									{
										value = stiRtfData2.X + (int)Math.Round((double)stiRtfData2.Width / 2.0) - 1;
										stringBuilder2.Append("\\tqc");
									}
									if ((stiTextHorAlignment.HorAlignment & 2) > 0)
									{
										value = stiRtfData2.X + stiRtfData2.Width - 28;
										stringBuilder2.Append("\\tqr");
									}
								}
								stringBuilder2.Append("\\tx");
								stringBuilder2.Append(value);
								stringBuilder3.Append("\\tab");
								RenderBorder3(component, stringBuilder3);
								RenderBrush3(component, stringBuilder3);
								RenderTextFont3(component, stringBuilder3);
								RenderTextBrush3(component, stringBuilder3);
								if (!(component is StiRichText))
								{
									RenderText3(component, stringBuilder3);
								}
								if (component is StiRichText)
								{
									RenderRtf3(component, stringBuilder3);
								}
								else
								{
									RenderImage3(component, stringBuilder3, stiRtfData2.Width, stiRtfData2.Height);
								}
							}
							sw.Write((object?)stringBuilder2);
							byte[] array2 = new byte[stringBuilder3.Length];
							for (int num17 = 0; num17 < stringBuilder3.Length; num17++)
							{
								array2[num17] = (byte)stringBuilder3[num17];
							}
							sw.Flush();
							sw2.Write(array2, 0, array2.Length);
						}
						if (array[num15].Count > 0)
						{
							sw.WriteLine("\\par}");
						}
					}
					break;
				}
				}
				sw.WriteLine("}");
				if (item2 != stiPagesCollection[stiPagesCollection.Count - 1])
				{
					RenderPageFooter();
				}
			}
		}
		RenderEndDoc();
		sw.Flush();
		if (matrix != null)
		{
			matrix.Clear();
			matrix = null;
		}
		colorList = null;
		fontList = null;
		styleList = null;
		unicodeMapArray = null;
		codePageToFont = null;
		fontToCodePages = null;
		imageCodec = null;
		bookmarkList = null;
		usedBookmarks = null;
		if (report.RenderedPages.CacheMode)
		{
			StiMatrix.GCCollect();
		}
	}
}
