using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Controls;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Txt.png")]
public class StiTxtExportService : StiExportService
{
	private struct StiTxtStyleInfo
	{
		public bool Bold;

		public bool Italic;

		public bool Underline;

		public string Codes;
	}

	private const string borderCodes = "\u0006\t\f\n\u0005\u0003\r\v\u000e\a\u000f\u0001\u0002\u0004\b";

	private const int firstEscapeCodeIndex = 63742;

	private const string ltrMark = "\u200e";

	private StiReport report;

	private string fileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	private bool[] needVerticalBorders;

	private bool[] needHorizontalBorders;

	private bool useFullTextBoxWidth;

	private bool useFullVerticalBorder = true;

	private bool useFullHorizontalBorder = true;

	private bool useEscapeCodes;

	private List<StiTxtStyleInfo> styleList;

	private List<string> escapeCodesList;

	public override string DefaultExtension => "txt";

	public override StiExportFormat ExportFormat => StiExportFormat.Text;

	public override string GroupCategory => "Word";

	public override int Position => 20;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeTxtFile");

	public override bool MultipleFiles => false;

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "TxtFiles");
	}

	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		ExportTxt(report, stream, settings as StiTxtExportSettings);
		InvokeExporting(100, 100, 1, 1);
	}

	public override void Export(StiReport report, string fileName, bool sendEMail, StiGuiMode guiMode)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		IStiFormRunner exportFormRunner = StiGuiOptions.GetExportFormRunner("StiTxtExportSetupForm", guiMode, base.OwnerWindow);
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

	private void FormComplete(IStiFormRunner form, StiShowDialogCompleteEvetArgs e)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
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
			StiTxtExportSettings stiTxtExportSettings = new StiTxtExportSettings();
			stiTxtExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
			stiTxtExportSettings.Encoding = form["Encoding"] as Encoding;
			stiTxtExportSettings.DrawBorder = (bool)form["DrawBorder"];
			stiTxtExportSettings.BorderType = (StiTxtBorderType)form["BorderType"];
			stiTxtExportSettings.KillSpaceLines = (bool)form["KillSpaceLines"];
			stiTxtExportSettings.PutFeedPageCode = (bool)form["PutFeedPageCode"];
			stiTxtExportSettings.CutLongLines = (bool)form["CutLongLines"];
			stiTxtExportSettings.ZoomX = (float)form["ZoomX"];
			stiTxtExportSettings.ZoomY = (float)form["ZoomY"];
			StiTxtExportSettings settings = stiTxtExportSettings;
			StartExport(report, stream, settings, sendEMail, (bool)form["OpenAfterExport"], fileName, guiMode);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private char GetBorderChar(int code, StiTxtBorderType type)
	{
		if (useFullVerticalBorder)
		{
			return type switch
			{
				StiTxtBorderType.UnicodeSingle => "─│┌┐└┘├┤┬┴┼ ─  "[code], 
				StiTxtBorderType.UnicodeDouble => "═║╔╗╚╝╠╣╦╩╬ ═  "[code], 
				_ => "-|+++++++++ -  "[code], 
			};
		}
		return type switch
		{
			StiTxtBorderType.UnicodeSingle => "─│┌┐└┘├┤┬┴┼    "[code], 
			StiTxtBorderType.UnicodeDouble => "═║╔╗╚╝╠╣╦╩╬    "[code], 
			_ => "-|+++++++++    "[code], 
		};
	}

	private void LineFill(StringBuilder sb, int code, int startPosition, int count)
	{
		char c = "\u0006\t\f\n\u0005\u0003\r\v\u000e\a\u000f\u0001\u0002\u0004\b"[code - 1];
		for (int i = 0; i < count; i++)
		{
			if ("\u0006\t\f\n\u0005\u0003\r\v\u000e\a\u000f\u0001\u0002\u0004\b".IndexOf(sb[startPosition + i]) == -1)
			{
				if (sb[startPosition + i] == ' ')
				{
					sb[startPosition + i] = c;
				}
			}
			else
			{
				sb[startPosition + i] |= c;
			}
		}
	}

	private void LineFillChar(StringBuilder sb, int code, int startPosition, int count)
	{
		for (int i = 0; i < count; i++)
		{
			sb[startPosition + i] = (char)code;
		}
	}

	private void CheckWordWrap(StiCell cell, ref string[] arraySt, int cellWidth, bool cutLongLines)
	{
		if (useEscapeCodes)
		{
			for (int i = 0; i < arraySt.Length; i++)
			{
				string text = arraySt[i];
				StringBuilder stringBuilder = new StringBuilder(text);
				if (StiBidirectionalConvert.StringContainArabicOrHebrew(text))
				{
					int num = text.Length;
					while (char.IsWhiteSpace(text, num - 1))
					{
						num--;
					}
					stringBuilder.Insert(num, "<#ltrMark>");
				}
				for (int j = 0; j < stringBuilder.Length; j++)
				{
					if (stringBuilder[j] != '<' || j >= stringBuilder.Length + 1 || stringBuilder[j + 1] != '#')
					{
						continue;
					}
					int k = j;
					StringBuilder stringBuilder2 = new StringBuilder();
					while (stringBuilder[k] == '<' && k < stringBuilder.Length + 1 && stringBuilder[k + 1] == '#')
					{
						for (; k < stringBuilder.Length && stringBuilder[k] != '>'; k++)
						{
							stringBuilder2.Append(stringBuilder[k]);
						}
						stringBuilder2.Append('>');
						k++;
						if (k > stringBuilder.Length - 1)
						{
							break;
						}
					}
					if (k > stringBuilder.Length - 1)
					{
						if (j > 0)
						{
							j--;
							stringBuilder2.Insert(0, stringBuilder[j]);
						}
						else
						{
							stringBuilder2.Append('\uffff');
						}
					}
					else
					{
						stringBuilder2.Append(stringBuilder[k]);
						k++;
					}
					int escapeNumber = GetEscapeNumber(escapeCodesList, stringBuilder2.ToString());
					stringBuilder.Remove(j, k - j);
					stringBuilder.Insert(j, (char)(63742 - escapeNumber));
				}
				if (stringBuilder.Length != arraySt[i].Length)
				{
					arraySt[i] = stringBuilder.ToString();
				}
			}
		}
		else
		{
			for (int l = 0; l < arraySt.Length; l++)
			{
				if (StiBidirectionalConvert.StringContainArabicOrHebrew(arraySt[l]))
				{
					string text2 = arraySt[l];
					int num2 = text2.Length - 1;
					while (char.IsWhiteSpace(text2, num2))
					{
						num2--;
					}
					string escapeCodes = text2[num2] + "\u200e";
					int escapeNumber2 = GetEscapeNumber(escapeCodesList, escapeCodes);
					if (num2 == text2.Length - 1)
					{
						arraySt[l] = text2.Substring(0, num2) + (char)(63742 - escapeNumber2);
					}
					else
					{
						arraySt[l] = text2.Substring(0, num2) + (char)(63742 - escapeNumber2) + text2.Substring(num2 + 1);
					}
				}
			}
		}
		int num3 = cellWidth;
		if (useFullVerticalBorder)
		{
			if (needVerticalBorders[cell.Left + cell.Width])
			{
				num3--;
			}
		}
		else if (!useFullTextBoxWidth && num3 > 1)
		{
			num3--;
		}
		bool flag = false;
		if (cell.Component is IStiTextOptions stiTextOptions)
		{
			flag = stiTextOptions.TextOptions.WordWrap;
		}
		if (flag)
		{
			ArrayList arrayList = new ArrayList(arraySt);
			for (int m = 0; m < arrayList.Count; m++)
			{
				string text3 = (string)arrayList[m];
				if (text3.Length <= num3 - 1 || num3 <= 1)
				{
					continue;
				}
				int[] array = new int[text3.Length];
				int num4 = 0;
				int n;
				for (n = 0; n < text3.Length && text3[n] == ' '; n++)
				{
					array[n] = num4;
				}
				for (int num5 = n; num5 < text3.Length; num5++)
				{
					if (text3[num5] == ' ')
					{
						num4++;
					}
					array[num5] = num4;
				}
				int num6 = num3 - 1 - 1;
				int num7 = num6;
				if (array[num6] > 0)
				{
					if (array[num6] != array[num6 + 1])
					{
						num7 = num6++;
						for (; num6 < text3.Length && text3[num6] == ' '; num6++)
						{
						}
					}
					else
					{
						while (text3[num6] != ' ')
						{
							num6--;
						}
						num7 = num6++;
						while (text3[num7] == ' ')
						{
							num7--;
						}
					}
				}
				else
				{
					num6++;
				}
				arrayList[m] = text3.Substring(0, num7 + 1);
				arrayList.Insert(m + 1, text3.Substring(num6, text3.Length - num6));
			}
			if (arrayList.Count > arraySt.Length)
			{
				arraySt = new string[arrayList.Count];
				for (int num8 = 0; num8 < arrayList.Count; num8++)
				{
					arraySt[num8] = (string)arrayList[num8];
				}
			}
		}
		if (!cutLongLines)
		{
			return;
		}
		for (int num9 = 0; num9 < arraySt.Length; num9++)
		{
			if (arraySt[num9].Length > num3)
			{
				arraySt[num9] = arraySt[num9].Substring(0, num3);
			}
		}
	}

	private void CheckGrow(List<StringBuilder> lines, List<StringBuilder> styles, List<bool> pageBreaks, int currentY)
	{
		while (lines.Count <= currentY)
		{
			lines.Add(new StringBuilder());
			styles.Add(new StringBuilder());
			pageBreaks.Add(item: false);
		}
	}

	private int GetStyleNumber(List<StiTxtStyleInfo> tmpStyleList, StiTxtStyleInfo styleInfo)
	{
		if (tmpStyleList.Count > 0)
		{
			for (int i = 0; i < tmpStyleList.Count; i++)
			{
				StiTxtStyleInfo stiTxtStyleInfo = tmpStyleList[i];
				if (stiTxtStyleInfo.Bold == styleInfo.Bold && stiTxtStyleInfo.Italic == styleInfo.Italic && stiTxtStyleInfo.Underline == styleInfo.Underline && stiTxtStyleInfo.Codes == styleInfo.Codes)
				{
					return i;
				}
			}
		}
		tmpStyleList.Add(styleInfo);
		return tmpStyleList.Count - 1;
	}

	private int GetEscapeNumber(List<string> tmpStyleList, string escapeCodes)
	{
		if (tmpStyleList.Count > 0)
		{
			for (int i = 0; i < tmpStyleList.Count; i++)
			{
				if (tmpStyleList[i] == escapeCodes)
				{
					return i + 1;
				}
			}
		}
		tmpStyleList.Add(escapeCodes);
		return tmpStyleList.Count;
	}

	private StringBuilder GetEscapeNames(int oldStyleNumber, int newStyleNumber)
	{
		StiTxtStyleInfo stiTxtStyleInfo = styleList[oldStyleNumber];
		StiTxtStyleInfo stiTxtStyleInfo2 = styleList[newStyleNumber];
		StringBuilder stringBuilder = new StringBuilder();
		if (!stiTxtStyleInfo.Bold && stiTxtStyleInfo2.Bold)
		{
			stringBuilder.Append("<#b>");
		}
		if (stiTxtStyleInfo.Bold && !stiTxtStyleInfo2.Bold)
		{
			stringBuilder.Append("<#/b>");
		}
		if (!stiTxtStyleInfo.Italic && stiTxtStyleInfo2.Italic)
		{
			stringBuilder.Append("<#i>");
		}
		if (stiTxtStyleInfo.Italic && !stiTxtStyleInfo2.Italic)
		{
			stringBuilder.Append("<#/i>");
		}
		if (!stiTxtStyleInfo.Underline && stiTxtStyleInfo2.Underline)
		{
			stringBuilder.Append("<#u>");
		}
		if (stiTxtStyleInfo.Underline && !stiTxtStyleInfo2.Underline)
		{
			stringBuilder.Append("<#/u>");
		}
		return stringBuilder;
	}

	public void ExportTxt(StiReport report, string fileName)
	{
		StiFileUtils.ProcessReadOnly(fileName);
		FileStream fileStream = new FileStream(fileName, FileMode.Create);
		ExportTxt(report, fileStream);
		fileStream.Flush();
		fileStream.Close();
	}

	public void ExportTxt(StiReport report, Stream stream)
	{
		StiTxtExportSettings settings = new StiTxtExportSettings();
		ExportTxt(report, stream, settings);
	}

	public void ExportTxt(StiReport report, Stream stream, Encoding encoding, StiPagesRange pageRange)
	{
		ExportTxt(report, stream, new StiTxtExportSettings
		{
			PageRange = pageRange,
			Encoding = encoding
		});
	}

	public void ExportTxt(StiReport report, Stream stream, Encoding encoding, bool drawBorder, StiTxtBorderType borderType, bool killSpaceLines, bool killSpaceGraphLines, bool putFeedPageCode, bool cutLongLines, float zoomX, float zoomY, StiPagesRange pageRange)
	{
		ExportTxt(report, stream, new StiTxtExportSettings
		{
			PageRange = pageRange,
			Encoding = encoding,
			DrawBorder = drawBorder,
			BorderType = borderType,
			KillSpaceLines = killSpaceLines,
			KillSpaceGraphLines = killSpaceGraphLines,
			PutFeedPageCode = putFeedPageCode,
			CutLongLines = cutLongLines,
			ZoomX = zoomX,
			ZoomY = zoomY
		});
	}

	public void ExportTxt(StiReport report, Stream stream, Encoding encoding, bool drawBorder, StiTxtBorderType borderType, bool killSpaceLines, bool killSpaceGraphLines, bool putFeedPageCode, bool cutLongLines, float zoomX, float zoomY, StiPagesRange pageRange, bool useEscapeCodes, string escapeCodesCollectionName)
	{
		ExportTxt(report, stream, new StiTxtExportSettings
		{
			PageRange = pageRange,
			Encoding = encoding,
			DrawBorder = drawBorder,
			BorderType = borderType,
			KillSpaceLines = killSpaceLines,
			KillSpaceGraphLines = killSpaceGraphLines,
			PutFeedPageCode = putFeedPageCode,
			CutLongLines = cutLongLines,
			ZoomX = zoomX,
			ZoomY = zoomY,
			UseEscapeCodes = useEscapeCodes,
			EscapeCodesCollectionName = escapeCodesCollectionName
		});
	}

	public void ExportTxt(StiReport report, Stream stream, StiTxtExportSettings settings)
	{
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b17: Expected O, but got Unknown
		//IL_0c01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf1: Invalid comparison between Unknown and I4
		//IL_0d00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d08: Invalid comparison between Unknown and I4
		//IL_0d79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7f: Invalid comparison between Unknown and I4
		//IL_0d94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9a: Invalid comparison between Unknown and I4
		StiLogService.Write(((object)this).GetType(), "Export report to Text format");
		this.report = report;
		if (settings == null)
		{
			throw new ArgumentNullException("settings");
		}
		StiPagesRange pageRange = settings.PageRange;
		Encoding encoding = settings.Encoding;
		bool drawBorder = settings.DrawBorder;
		StiTxtBorderType borderType = settings.BorderType;
		bool killSpaceLines = settings.KillSpaceLines;
		bool flag = settings.KillSpaceGraphLines;
		bool putFeedPageCode = settings.PutFeedPageCode;
		bool cutLongLines = settings.CutLongLines;
		float zoomX = settings.ZoomX;
		float zoomY = settings.ZoomY;
		useEscapeCodes = settings.UseEscapeCodes;
		string escapeCodesCollectionName = settings.EscapeCodesCollectionName;
		RichTextBox val = null;
		useFullTextBoxWidth = StiOptions.Export.Text.UseFullTextBoxWidth;
		useFullVerticalBorder = StiOptions.Export.Text.UseFullVerticalBorder;
		useFullHorizontalBorder = StiOptions.Export.Text.UseFullHorizontalBorder;
		if (!killSpaceLines)
		{
			flag = false;
		}
		if (!drawBorder)
		{
			flag = false;
		}
		if (useFullVerticalBorder)
		{
			useFullTextBoxWidth = false;
		}
		MemoryStream memoryStream = new MemoryStream();
		StreamWriter streamWriter = ((encoding != null) ? new StreamWriter(memoryStream, encoding) : new StreamWriter(memoryStream));
		base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
		StiPagesCollection selectedPages = pageRange.GetSelectedPages(report.RenderedPages);
		int num = 0;
		styleList = new List<StiTxtStyleInfo>();
		GetStyleNumber(styleList, default(StiTxtStyleInfo));
		escapeCodesList = new List<string>();
		base.CurrentPassNumber = 0;
		base.MaximumPassNumber = 3;
		StiMatrix stiMatrix = new StiMatrix(selectedPages, checkForExcel: false, this);
		if (base.IsStopped)
		{
			return;
		}
		needVerticalBorders = new bool[stiMatrix.CoordX.Count];
		for (int i = 1; i < stiMatrix.CoordX.Count; i++)
		{
			bool flag2 = false;
			for (int j = 1; j < stiMatrix.CoordY.Count; j++)
			{
				if (stiMatrix.BordersY[j - 1, i] != (StiBorderSide)null)
				{
					flag2 = true;
					break;
				}
			}
			needVerticalBorders[i - 1] = flag2;
		}
		needHorizontalBorders = new bool[stiMatrix.CoordY.Count];
		for (int k = 1; k < stiMatrix.CoordY.Count; k++)
		{
			bool flag3 = false;
			for (int l = 1; l < stiMatrix.CoordX.Count; l++)
			{
				if (stiMatrix.BordersX[k, l - 1] != (StiBorderSide)null)
				{
					flag3 = true;
					break;
				}
			}
			needHorizontalBorders[k - 1] = flag3;
		}
		int[] array = new int[stiMatrix.CoordX.Count];
		for (int m = 1; m < stiMatrix.CoordX.Count; m++)
		{
			double num2 = (double)stiMatrix.CoordX.GetByIndex(m);
			double num3 = (double)stiMatrix.CoordX.GetByIndex(m - 1);
			array[m - 1] = (int)Math.Round((num2 - num3) / (9.7 / (double)zoomX));
			if (useFullVerticalBorder && needVerticalBorders[m - 1])
			{
				array[m - 1]++;
			}
		}
		bool[,] array2 = new bool[stiMatrix.CoordY.Count, stiMatrix.CoordX.Count];
		int[,] array3 = new int[stiMatrix.CoordY.Count, stiMatrix.CoordX.Count];
		int[,] array4 = new int[stiMatrix.CoordY.Count, stiMatrix.CoordX.Count];
		StiHtmlState val2 = default(StiHtmlState);
		for (int n = 1; n < stiMatrix.CoordY.Count; n++)
		{
			for (int num4 = 1; num4 < stiMatrix.CoordX.Count; num4++)
			{
				if (array2[n - 1, num4 - 1])
				{
					continue;
				}
				StiCell stiCell = stiMatrix.Cells[n - 1, num4 - 1];
				array3[n - 1, num4 - 1] = array[num4 - 1];
				if (stiCell == null)
				{
					continue;
				}
				if (stiCell.Width > 0)
				{
					for (int num5 = 0; num5 < stiCell.Width; num5++)
					{
						array3[n - 1, num4 - 1] += array[num4 - 1 + 1 + num5];
					}
				}
				if (stiCell.Component != null)
				{
					if (stiCell.Component != null)
					{
						string text = stiCell.Component.TagValue as string;
						if (!string.IsNullOrEmpty(text))
						{
							text = text.Trim();
							int result = 0;
							if (int.TryParse(text, out result) && result > 0)
							{
								array3[n - 1, num4 - 1] = result;
								if (useFullVerticalBorder && needVerticalBorders[num4 - 1 + stiCell.Width])
								{
									array3[n - 1, num4 - 1]++;
								}
							}
						}
					}
					string text2 = stiCell.Text;
					if (stiCell.Component is StiRichText stiRichText && stiRichText.RtfText != string.Empty)
					{
						if (val == null)
						{
							val = (RichTextBox)(object)new StiRichTextBox(drawTransparent: false);
						}
						stiRichText.GetPreparedText(val);
						text2 = ((Control)val).Text;
					}
					if (stiCell.Component is StiCheckBox { CheckedValue: not null } stiCheckBox)
					{
						bool flag4 = false;
						bool flag5 = false;
						if (stiCheckBox.CheckedValue is bool)
						{
							if ((bool)stiCheckBox.CheckedValue)
							{
								flag4 = true;
							}
							else
							{
								flag5 = true;
							}
						}
						if (stiCheckBox.CheckedValue is string)
						{
							if ((string)stiCheckBox.CheckedValue == "True")
							{
								flag4 = true;
							}
							else
							{
								flag5 = true;
							}
						}
						if (flag4)
						{
							text2 = StiOptions.Export.Text.CheckBoxTextForTrue;
						}
						if (flag5)
						{
							text2 = StiOptions.Export.Text.CheckBoxTextForFalse;
						}
					}
					if (!stiCell.Component.IsExportAsImage(StiExportFormat.Text) && !string.IsNullOrEmpty(text2))
					{
						if (stiCell.Component is StiText && (stiCell.Component as StiText).CheckAllowHtmlTags())
						{
							((StiHtmlState)(ref val2))._002Ector(string.Empty);
							List<StiHtmlState> list = StiTextRenderer.ParseHtmlToStates(text2, val2, false);
							StringBuilder stringBuilder = new StringBuilder();
							foreach (StiHtmlState item in list)
							{
								stringBuilder.Append((object?)StiTextRenderer.PrepareStateText(item.Text));
							}
							text2 = stringBuilder.ToString();
						}
						List<string> list2 = StiExportUtils.SplitString(text2.Replace("\r", ""), removeControl: false);
						string[] arraySt = new string[list2.Count];
						for (int num6 = 0; num6 < list2.Count; num6++)
						{
							arraySt[num6] = list2[num6];
						}
						CheckWordWrap(stiCell, ref arraySt, array3[n - 1, num4 - 1], cutLongLines);
						int num7 = arraySt.Length;
						array4[n - 1, num4 - 1] = num7;
					}
				}
				for (int num8 = 0; num8 <= stiCell.Height; num8++)
				{
					for (int num9 = 0; num9 <= stiCell.Width; num9++)
					{
						array2[n - 1 + num8, num4 - 1 + num9] = true;
					}
					array3[n - 1 + num8, num4 - 1] = array3[n - 1, num4 - 1];
				}
			}
		}
		int[] array5 = new int[stiMatrix.CoordY.Count];
		num = 1;
		for (int num10 = 1; num10 < stiMatrix.CoordY.Count; num10++)
		{
			double num11 = (double)stiMatrix.CoordY.GetByIndex(num10);
			double num12 = (double)stiMatrix.CoordY.GetByIndex(num10 - 1);
			int num13 = (int)Math.Round((num11 - num12) / (18.0 / (double)zoomY));
			if (num13 < 1)
			{
				num13 = 1;
			}
			for (int num14 = 1; num14 < stiMatrix.CoordX.Count; num14++)
			{
				StiCell stiCell2 = stiMatrix.Cells[num10 - 1, num14 - 1];
				if (stiCell2 != null && stiCell2.Height < 1 && array4[num10 - 1, num14 - 1] > num13)
				{
					num13 = array4[num10 - 1, num14 - 1];
				}
			}
			if (useFullHorizontalBorder && needHorizontalBorders[num10 - 1])
			{
				num13++;
			}
			array5[num10 - 1] = num13;
			num += num13;
		}
		num++;
		List<StringBuilder> list3 = new List<StringBuilder>();
		List<StringBuilder> list4 = new List<StringBuilder>();
		List<bool> list5 = new List<bool>();
		for (int num15 = 0; num15 < num; num15++)
		{
			list3.Add(new StringBuilder());
			list4.Add(new StringBuilder());
			list5.Add(item: false);
		}
		array2 = new bool[stiMatrix.CoordY.Count, stiMatrix.CoordX.Count];
		int num16 = (drawBorder ? 1 : 0);
		StiText stiText = new StiText
		{
			VertAlignment = (StiVertAlignment)1,
			HorAlignment = (StiTextHorAlignment)1
		};
		base.CurrentPassNumber = 2;
		double num17 = Math.Max((float)stiMatrix.CoordY.Count / 200f, 1f);
		int num18 = 0;
		StiHtmlState val3 = default(StiHtmlState);
		for (int num19 = 1; num19 < stiMatrix.CoordY.Count; num19++)
		{
			int num20 = (int)((double)num19 / num17);
			if (num20 > num18)
			{
				num18 = num20;
				InvokeExporting(num19, stiMatrix.CoordY.Count, base.CurrentPassNumber, base.MaximumPassNumber);
			}
			for (int num21 = 0; num21 < stiMatrix.HorizontalPageBreaks.Count; num21++)
			{
				if (stiMatrix.HorizontalPageBreaks[num21] == num19 - 1)
				{
					list5[num16 - ((drawBorder && num19 > 1 && needHorizontalBorders[num19 - 1 - 1]) ? 1 : 0)] = true;
				}
			}
			int num22 = (drawBorder ? 1 : 0);
			for (int num23 = 1; num23 < stiMatrix.CoordX.Count; num23++)
			{
				StiCell stiCell3 = stiMatrix.Cells[num19 - 1, num23 - 1];
				int num24 = array3[num19 - 1, num23 - 1];
				int num25 = array5[num19 - 1];
				if (!array2[num19 - 1, num23 - 1])
				{
					int num26 = 0;
					int num27 = 0;
					int code = 0;
					if (stiCell3 != null)
					{
						num26 = stiCell3.Width;
						num27 = stiCell3.Height;
						for (int num28 = 0; num28 <= stiCell3.Width; num28++)
						{
							for (int num29 = 0; num29 <= stiCell3.Height; num29++)
							{
								array2[num19 - 1 + num29, num23 - 1 + num28] = true;
							}
						}
						if (stiCell3.Height > 0)
						{
							for (int num30 = 0; num30 < stiCell3.Height; num30++)
							{
								num25 += array5[num19 - 1 + 1 + num30];
							}
						}
						if (useFullHorizontalBorder && needHorizontalBorders[num19 - 1 + num27])
						{
							num25--;
						}
						if (stiCell3.Component != null)
						{
							if (stiCell3.Component is StiText stiText2)
							{
								StiTxtStyleInfo stiTxtStyleInfo = default(StiTxtStyleInfo);
								stiTxtStyleInfo.Bold = stiText2.Font.Bold;
								stiTxtStyleInfo.Italic = stiText2.Font.Italic;
								stiTxtStyleInfo.Underline = stiText2.Font.Underline;
								stiTxtStyleInfo.Codes = null;
								StiTxtStyleInfo styleInfo = stiTxtStyleInfo;
								code = GetStyleNumber(styleList, styleInfo);
							}
							string text3 = stiCell3.Text;
							if (stiCell3.Component is StiRichText stiRichText2 && stiRichText2.RtfText != string.Empty)
							{
								if (val == null)
								{
									val = new RichTextBox();
								}
								stiRichText2.GetPreparedText(val);
								text3 = ((Control)val).Text;
							}
							StiCheckBox stiCheckBox2 = stiCell3.Component as StiCheckBox;
							if (stiCheckBox2 != null && stiCheckBox2.CheckedValue != null)
							{
								bool flag6 = false;
								bool flag7 = false;
								if (stiCheckBox2.CheckedValue is bool)
								{
									if ((bool)stiCheckBox2.CheckedValue)
									{
										flag6 = true;
									}
									else
									{
										flag7 = true;
									}
								}
								if (stiCheckBox2.CheckedValue is string)
								{
									if ((string)stiCheckBox2.CheckedValue == "True")
									{
										flag6 = true;
									}
									else
									{
										flag7 = true;
									}
								}
								if (flag6)
								{
									text3 = StiOptions.Export.Text.CheckBoxTextForTrue;
								}
								if (flag7)
								{
									text3 = StiOptions.Export.Text.CheckBoxTextForFalse;
								}
							}
							if (!stiCell3.Component.IsExportAsImage(StiExportFormat.Text) && !string.IsNullOrEmpty(text3))
							{
								if (stiCell3.Component is StiText && (stiCell3.Component as StiText).CheckAllowHtmlTags())
								{
									((StiHtmlState)(ref val3))._002Ector(string.Empty);
									List<StiHtmlState> list6 = StiTextRenderer.ParseHtmlToStates(text3, val3, false);
									StringBuilder stringBuilder2 = new StringBuilder();
									foreach (StiHtmlState item2 in list6)
									{
										stringBuilder2.Append((object?)StiTextRenderer.PrepareStateText(item2.Text));
									}
									text3 = stringBuilder2.ToString();
								}
								List<string> list7 = StiExportUtils.SplitString(text3.Replace("\r", ""), removeControl: false);
								string[] arraySt2 = new string[list7.Count];
								for (int num31 = 0; num31 < list7.Count; num31++)
								{
									arraySt2[num31] = list7[num31];
								}
								CheckWordWrap(stiCell3, ref arraySt2, array3[num19 - 1, num23 - 1], cutLongLines);
								int num32 = 0;
								IStiVertAlignment stiVertAlignment = stiCell3.Component as IStiVertAlignment;
								if (stiCheckBox2 != null)
								{
									stiVertAlignment = stiText;
								}
								if (stiVertAlignment != null && arraySt2.Length < num25)
								{
									if ((stiVertAlignment.VertAlignment & 1) > 0)
									{
										num32 = (num25 - arraySt2.Length) / 2;
									}
									if ((stiVertAlignment.VertAlignment & 2) > 0)
									{
										num32 = num25 - arraySt2.Length;
									}
								}
								for (int num33 = 0; num33 < arraySt2.Length; num33++)
								{
									int num34 = 0;
									int num35 = num24;
									if (useFullVerticalBorder)
									{
										if (needVerticalBorders[stiCell3.Left + stiCell3.Width])
										{
											num35--;
										}
									}
									else if (!useFullTextBoxWidth && num35 > 1)
									{
										num35--;
									}
									IStiTextHorAlignment stiTextHorAlignment = stiCell3.Component as IStiTextHorAlignment;
									if (stiCheckBox2 != null)
									{
										stiTextHorAlignment = stiText;
									}
									if (stiTextHorAlignment != null)
									{
										if ((int)stiTextHorAlignment.HorAlignment == 1)
										{
											num34 = (num35 - arraySt2[num33].Length) / 2;
										}
										if ((int)stiTextHorAlignment.HorAlignment == 2)
										{
											num34 = num35 - arraySt2[num33].Length;
										}
									}
									int num36 = num22 + num34;
									int num37 = num16 + num32 + num33;
									if (num36 < 0)
									{
										num36 = 0;
									}
									int num38 = num36 + arraySt2[num33].Length;
									CheckGrow(list3, list4, list5, num37);
									if (list3[num37].Length < num38)
									{
										list3[num37].Append(' ', num38 - list3[num37].Length);
									}
									for (int num39 = 0; num39 < arraySt2[num33].Length; num39++)
									{
										int num40 = arraySt2[num33][num39];
										if (num40 < 32)
										{
											num40 += 65280;
										}
										list3[num37][num36 + num39] = (char)num40;
									}
								}
							}
						}
					}
					else if (useFullHorizontalBorder && needHorizontalBorders[num19 - 1])
					{
						num25--;
					}
					if (!useFullHorizontalBorder)
					{
						num25--;
					}
					for (int num41 = 0; num41 < num25; num41++)
					{
						if (list4[num16 + num41].Length < num22 + num24)
						{
							list4[num16 + num41].Append('\0', num22 + num24 - list4[num16 + num41].Length);
						}
						int num42 = num24;
						if (useFullVerticalBorder)
						{
							if (stiCell3 != null && needVerticalBorders[stiCell3.Left + stiCell3.Width])
							{
								num42--;
							}
						}
						else if (!useFullTextBoxWidth && num42 > 1)
						{
							num42--;
						}
						LineFillChar(list4[num16 + num41], code, num22, num42);
					}
					if (drawBorder)
					{
						int num43 = num22 + num24 - 1;
						for (int num44 = -1; num44 <= num25; num44++)
						{
							CheckGrow(list3, list4, list5, num16 + num44);
							if (list3[num16 + num44].Length < num43 + 1)
							{
								list3[num16 + num44].Append(' ', num43 + 1 - list3[num16 + num44].Length);
							}
						}
						bool flag8 = true;
						bool flag9 = true;
						for (int num45 = 0; num45 < num27 + 1; num45++)
						{
							if (stiMatrix.BordersY[num19 - 1 + num45, num23 - 1] == (StiBorderSide)null)
							{
								flag8 = false;
							}
							if (stiMatrix.BordersY[num19 - 1 + num45, num23 - 1 + num26 + 1] == (StiBorderSide)null)
							{
								flag9 = false;
							}
						}
						bool flag10 = true;
						bool flag11 = true;
						for (int num46 = 0; num46 < num26 + 1; num46++)
						{
							if (stiMatrix.BordersX[num19 - 1, num23 - 1 + num46] == (StiBorderSide)null)
							{
								flag10 = false;
							}
							if (stiMatrix.BordersX[num19 - 1 + num27 + 1, num23 - 1 + num46] == (StiBorderSide)null)
							{
								flag11 = false;
							}
						}
						if (flag10)
						{
							LineFill(list3[num16 - 1], 14, num22 - 1, 1);
							LineFill(list3[num16 - 1], 1, num22, num24 - 1);
							LineFill(list3[num16 - 1], 13, num22 + num24 - 1, 1);
						}
						if (flag8)
						{
							LineFill(list3[num16 - 1], 15, num22 - 1, 1);
							for (int num47 = 0; num47 < num25; num47++)
							{
								LineFill(list3[num16 + num47], 2, num22 - 1, 1);
							}
							LineFill(list3[num16 + num25], 12, num22 - 1, 1);
						}
						if (flag9)
						{
							LineFill(list3[num16 - 1], 15, num22 + num24 - 1, 1);
							for (int num48 = 0; num48 < num25; num48++)
							{
								LineFill(list3[num16 + num48], 2, num22 + num24 - 1, 1);
							}
							LineFill(list3[num16 + num25], 12, num22 + num24 - 1, 1);
						}
						if (flag11)
						{
							LineFill(list3[num16 + num25], 14, num22 - 1, 1);
							LineFill(list3[num16 + num25], 1, num22, num24 - 1);
							LineFill(list3[num16 + num25], 13, num22 + num24 - 1, 1);
						}
					}
				}
				num22 += num24;
			}
			num16 += array5[num19 - 1];
		}
		if (drawBorder)
		{
			bool flag12 = true;
			for (int num49 = 0; num49 < list3.Count; num49++)
			{
				if (list3[num49].Length > 0 && list3[num49][0] != ' ')
				{
					flag12 = false;
					break;
				}
			}
			if (flag12)
			{
				for (int num50 = 0; num50 < list3.Count; num50++)
				{
					if (list3[num50].Length > 0)
					{
						list3[num50].Remove(0, 1);
					}
					if (list4[num50].Length > 0)
					{
						list4[num50].Remove(0, 1);
					}
				}
			}
		}
		StiLicenseKey licenseKey = StiLicenseKeyValidator.GetLicenseKey();
		bool flag13 = (!StiLicenseKeyValidator.IsValidInReportsDesignerOrOnPlatform((StiProductIdent)2, licenseKey) || !StiDesignerAppStatus.IsRunning) && !StiLicenseKeyValidator.IsValidOnNetFramework(licenseKey);
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
		if (flag13)
		{
			double num51 = ((double)stiMatrix.CoordX.GetByIndex(stiMatrix.CoordX.Count - 1) - (double)stiMatrix.CoordX.GetByIndex(0)) / (9.7 / (double)zoomX);
			int count = Math.Max(1, (int)(num51 / 2.0 - 10.0));
			streamWriter.WriteLine(new string(' ', count) + "---=== TRIAL ===---\r\n");
		}
		int num52 = list3.Count - 1;
		while (num52 > 0 && list3[num52].Length == 0)
		{
			num52--;
		}
		int num53 = 0;
		for (int num54 = 0; num54 < list3.Count; num54++)
		{
			if (list3[num54].Length > 0)
			{
				for (int num55 = 0; num55 < "\u0006\t\f\n\u0005\u0003\r\v\u000e\a\u000f\u0001\u0002\u0004\b".Length; num55++)
				{
					list3[num54].Replace("\u0006\t\f\n\u0005\u0003\r\v\u000e\a\u000f\u0001\u0002\u0004\b"[num55], GetBorderChar(num55, borderType));
				}
				if (StiOptions.Export.Text.TrimTrailingSpaces)
				{
					int num56 = list3[num54].Length - 1;
					while (num56 >= 0 && list3[num54][num56] == ' ')
					{
						list3[num54].Length--;
						num56--;
					}
				}
				if (flag)
				{
					string text4 = GetBorderChar(1, borderType) + " ";
					bool flag14 = true;
					for (int num57 = 0; num57 < list3[num54].Length; num57++)
					{
						if (text4.IndexOf(list3[num54][num57]) == -1)
						{
							flag14 = false;
						}
					}
					if (flag14)
					{
						list3[num54].Length = 0;
					}
				}
				if (putFeedPageCode && list5[num54])
				{
					if (useEscapeCodes)
					{
						streamWriter.Write((object?)GetEscapeNames(num53, 0));
					}
					num53 = 0;
					streamWriter.Write("\f");
				}
				if (useEscapeCodes)
				{
					StringBuilder stringBuilder3 = new StringBuilder();
					for (int num58 = 0; num58 < list3[num54].Length; num58++)
					{
						int num59 = 0;
						if (num58 < list4[num54].Length)
						{
							num59 = list4[num54][num58];
						}
						if (num59 != num53)
						{
							stringBuilder3.Append((object?)GetEscapeNames(num53, num59));
							num53 = num59;
						}
						char c = list3[num54][num58];
						if (c > 63742 - escapeCodesList.Count - 2 && c <= '\uf8fe')
						{
							int num60 = 63742 - c;
							string text5 = escapeCodesList[num60 - 1];
							if (text5[text5.Length - 1] == '\uffff')
							{
								text5 = text5.Substring(0, text5.Length - 1);
							}
							stringBuilder3.Append(text5);
						}
						else
						{
							stringBuilder3.Append(c);
						}
					}
					if (stringBuilder3.Length > list3[num54].Length)
					{
						list3[num54] = stringBuilder3;
					}
				}
				else
				{
					StringBuilder stringBuilder4 = new StringBuilder();
					for (int num61 = 0; num61 < list3[num54].Length; num61++)
					{
						char c2 = list3[num54][num61];
						if (c2 > 63742 - escapeCodesList.Count - 2 && c2 <= '\uf8fe')
						{
							int num62 = 63742 - c2;
							string value = escapeCodesList[num62 - 1];
							stringBuilder4.Append(value);
						}
						else
						{
							stringBuilder4.Append(c2);
						}
					}
					if (stringBuilder4.Length > list3[num54].Length)
					{
						list3[num54] = stringBuilder4;
					}
				}
			}
			if ((killSpaceLines || num54 > num52) && list3[num54].Length <= 0)
			{
				continue;
			}
			for (int num63 = 0; num63 < list3[num54].Length; num63++)
			{
				if (list3[num54][num63] >= '\uff00')
				{
					list3[num54][num63] = (char)(list3[num54][num63] - 65280);
				}
			}
			if (StiOptions.Export.Text.RemoveLastNewLineMarker && num54 == num52)
			{
				streamWriter.Write((object?)list3[num54]);
			}
			else
			{
				streamWriter.WriteLine((object?)list3[num54]);
			}
		}
		StringBuilder stringBuilder5 = new StringBuilder();
		if (useEscapeCodes)
		{
			stringBuilder5.Append((object?)GetEscapeNames(num53, 0));
		}
		if (putFeedPageCode)
		{
			stringBuilder5.Append('\f');
		}
		if (stringBuilder5.Length > 0)
		{
			streamWriter.Write((object?)stringBuilder5);
		}
		streamWriter.Flush();
		if (useEscapeCodes)
		{
			memoryStream.Flush();
			memoryStream.Seek(0L, SeekOrigin.Begin);
			byte[] array6 = memoryStream.ToArray();
			StringBuilder stringBuilder6 = new StringBuilder();
			byte[] array7 = array6;
			foreach (byte value2 in array7)
			{
				stringBuilder6.Append((char)value2);
			}
			StiEscapeCodesCollection.GetEscapeCodesCollection(escapeCodesCollectionName)["ltrMark"] = "\u200e";
			string text6 = StiEscapeCodesCollection.ConvertEscapeCodes(stringBuilder6.ToString(), escapeCodesCollectionName);
			memoryStream = new MemoryStream();
			string text7 = text6;
			foreach (char c3 in text7)
			{
				memoryStream.WriteByte((byte)c3);
			}
		}
		memoryStream.WriteTo(stream);
		memoryStream.Close();
		((Component)(object)val)?.Dispose();
		needVerticalBorders = null;
		needHorizontalBorders = null;
		styleList = null;
		escapeCodesList = null;
	}
}
