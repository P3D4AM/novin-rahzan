using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Sylk.png")]
public class StiSylkExportService : StiExportService
{
	private struct DataFont
	{
		public string Name;

		public bool Bold;

		public bool Italic;

		public bool Underlined;

		public int Height;

		public int Color;

		public DataFont(string Name, bool Bold, bool Italic, bool Underlined, int Height, int Color)
		{
			this.Name = Name;
			this.Bold = Bold;
			this.Italic = Italic;
			this.Underlined = Underlined;
			this.Height = Height;
			this.Color = Color;
		}
	}

	private StiReport report;

	private string fileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	private int colorTableSize = 64;

	private int[] colorTable = new int[64]
	{
		0, 16711422, 16711680, 65280, 255, 16776960, 16711935, 65535, 0, 16711422,
		16711680, 65280, 255, 16776960, 16711935, 65535, 8388608, 32768, 128, 8421376,
		8388736, 32896, 12632256, 8421504, 10066431, 10040166, 16777164, 13434879, 6684774, 16744576,
		26316, 13421823, 128, 16711935, 16776960, 65535, 8388736, 8388608, 32896, 255,
		52479, 13434879, 13434828, 16777113, 10079487, 16751052, 13408767, 16764057, 3368703, 3394764,
		10079232, 16763904, 16750848, 16737792, 6710937, 9868950, 13158, 3381606, 13056, 3355392,
		10040064, 10040166, 3355545, 3355443
	};

	private StreamWriter writer;

	private List<string> formatList;

	private List<DataFont> fontList;

	private Hashtable colorHash;

	private static double HiToTwips = 14.688;

	private const double TwipsToColinfo = 2.2453980000000002;

	private const double TwipsToSymbols = 0.008699796000000001;

	public override string DefaultExtension => "slk";

	public override StiExportFormat ExportFormat => StiExportFormat.Sylk;

	public override string GroupCategory => "Data";

	public override int Position => 40;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeSylkFile");

	public override bool MultipleFiles => false;

	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		ExportSylk(report, stream, settings as StiDataExportSettings);
		InvokeExporting(100, 100, 1, 1);
	}

	public override void Export(StiReport report, string fileName, bool sendEMail, StiGuiMode guiMode)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		IStiFormRunner exportFormRunner = StiGuiOptions.GetExportFormRunner("StiSylkExportSetupForm", guiMode, base.OwnerWindow);
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

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "SylkFiles");
	}

	private void Form_Complete(IStiFormRunner form, StiShowDialogCompleteEvetArgs e)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
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
				StiDataExportSettings stiDataExportSettings = new StiDataExportSettings();
				stiDataExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
				stiDataExportSettings.ExportDataOnly = (bool)form["ExportDataOnly"];
				stiDataExportSettings.Encoding = form["Encoding"] as Encoding;
				stiDataExportSettings.UseDefaultSystemEncoding = (bool)form["UseDefaultSystemEncoding"];
				StiDataExportSettings settings = stiDataExportSettings;
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

	private int GetFontNumber(DataFont dataIn)
	{
		if (fontList.Count > 4)
		{
			for (int i = 4; i < fontList.Count; i++)
			{
				if (object.Equals(fontList[i], dataIn))
				{
					return i;
				}
			}
		}
		fontList.Add(dataIn);
		return fontList.Count - 1;
	}

	private int GetFormatNumber(string dataIn)
	{
		if (formatList.Count > 0)
		{
			for (int i = 0; i < formatList.Count; i++)
			{
				if (object.Equals(formatList[i], dataIn))
				{
					return i;
				}
			}
		}
		formatList.Add(dataIn);
		return formatList.Count - 1;
	}

	private int GetColorIndex(Color color)
	{
		if (color == Color.Transparent)
		{
			return 0;
		}
		int num = color.ToArgb();
		if (colorHash.ContainsKey(num))
		{
			return (int)colorHash[num];
		}
		int num2 = 768;
		int num3 = 0;
		for (int i = 0; i < colorTableSize; i++)
		{
			int num4 = colorTable[i];
			int num5 = Math.Abs(color.R - ((num4 >> 16) & 0xFF)) + Math.Abs(color.G - ((num4 >> 8) & 0xFF)) + Math.Abs(color.B - (num4 & 0xFF));
			if (num5 < num2)
			{
				num3 = i;
				num2 = num5;
			}
		}
		num3++;
		colorHash[num] = num3;
		return num3;
	}

	public void ExportSylk(StiReport report, string fileName)
	{
		StiFileUtils.ProcessReadOnly(fileName);
		using FileStream fileStream = new FileStream(fileName, FileMode.Create);
		ExportSylk(report, fileStream);
		fileStream.Flush();
		fileStream.Close();
	}

	public void ExportSylk(StiReport report, Stream stream)
	{
		ExportSylk(report, stream, new StiDataExportSettings());
	}

	public void ExportSylk(StiReport report, Stream stream, StiPagesRange pageRange, bool exportDataOnly, bool useDefaultSystemEncoding, Encoding encoding)
	{
		StiDataExportSettings settings = new StiDataExportSettings
		{
			PageRange = pageRange,
			ExportDataOnly = exportDataOnly,
			UseDefaultSystemEncoding = useDefaultSystemEncoding,
			Encoding = encoding
		};
		ExportSylk(report, stream, settings);
	}

	public void ExportSylk(StiReport report, Stream stream, StiDataExportSettings settings)
	{
		//IL_0a7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a83: Invalid comparison between Unknown and I4
		//IL_0a93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a99: Invalid comparison between Unknown and I4
		//IL_0aa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaf: Invalid comparison between Unknown and I4
		StiLogService.Write(((object)this).GetType(), "Export report to Sylk format");
		if (!report.IsDocument && report.GetCurrentPage() is IStiDashboard)
		{
			StiDashboardExport.Export(report, stream, settings);
			return;
		}
		if (settings == null)
		{
			throw new ArgumentNullException("The 'settings' argument cannot be equal in null.");
		}
		StiPagesRange pageRange = settings.PageRange;
		bool exportDataOnly = settings.ExportDataOnly;
		bool useDefaultSystemEncoding = settings.UseDefaultSystemEncoding;
		Encoding encoding = settings.Encoding;
		fontList = new List<DataFont>();
		DataFont dataIn = new DataFont("Arial", Bold: false, Italic: false, Underlined: false, 200, 0);
		GetFontNumber(dataIn);
		GetFontNumber(dataIn);
		GetFontNumber(dataIn);
		GetFontNumber(dataIn);
		colorHash = new Hashtable();
		formatList = new List<string>();
		GetFormatNumber("General");
		if (useDefaultSystemEncoding)
		{
			encoding = Encoding.GetEncoding(CultureInfo.InstalledUICulture.TextInfo.ANSICodePage);
		}
		MemoryStream memoryStream = new MemoryStream();
		writer = new StreamWriter(memoryStream, encoding);
		base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
		StiPagesCollection selectedPages = pageRange.GetSelectedPages(report.RenderedPages);
		base.CurrentPassNumber = 0;
		base.MaximumPassNumber = 3;
		StiDataExportMode dataMode = (exportDataOnly ? StiDataExportMode.DataAndHeaders : StiDataExportMode.AllBands);
		StiMatrix stiMatrix = new StiMatrix(selectedPages, checkForExcel: true, this, null, dataMode);
		if (base.IsStopped)
		{
			return;
		}
		if (exportDataOnly)
		{
			stiMatrix.ScanComponentsPlacement(optimize: true, exportObjectFormatting: false);
			int num = 0;
			Hashtable hashtable = new Hashtable();
			string text = null;
			for (int i = 0; i < stiMatrix.CoordY.Count - 1; i++)
			{
				bool flag = false;
				if (stiMatrix.LinePlacement[i] == StiMatrix.StiTableLineInfo.HeaderD || stiMatrix.LinePlacement[i] == StiMatrix.StiTableLineInfo.HeaderAP)
				{
					string text2 = stiMatrix.ParentBandName[i];
					int num2 = text2.IndexOf('\u001f');
					if (num2 != -1)
					{
						string text3 = text2.Substring(0, num2);
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
				if (!(stiMatrix.LinePlacement[i] == StiMatrix.StiTableLineInfo.Data || flag))
				{
					continue;
				}
				for (int j = 0; j < stiMatrix.CoordX.Count - 1; j++)
				{
					stiMatrix.Cells[num, j] = stiMatrix.Cells[i, j];
					if (stiMatrix.Cells[num, j] != null)
					{
						stiMatrix.Cells[num, j].Top = num;
					}
					if (num == 0 || stiMatrix.BordersX[i, j] != (StiBorderSide)null)
					{
						stiMatrix.BordersX[num, j] = stiMatrix.BordersX[i, j];
					}
					stiMatrix.BordersX[num + 1, j] = stiMatrix.BordersX[i + 1, j];
					stiMatrix.BordersY[num, j] = stiMatrix.BordersY[i, j];
					stiMatrix.Bookmarks[num, j] = stiMatrix.Bookmarks[i, j];
				}
				stiMatrix.BordersY[num, stiMatrix.CoordX.Count - 1] = stiMatrix.BordersY[i, stiMatrix.CoordX.Count - 1];
				double num3 = (double)stiMatrix.CoordY.GetByIndex(i + 1) - (double)stiMatrix.CoordY.GetByIndex(i);
				stiMatrix.CoordY.SetByIndex(num + 1, (double)stiMatrix.CoordY.GetByIndex(num) + num3);
				num++;
			}
			int num4 = stiMatrix.CoordY.Count - 1 - num;
			if (num4 > 0)
			{
				for (int k = 0; k < num4; k++)
				{
					stiMatrix.CoordY.RemoveAt(num + 1);
				}
			}
		}
		base.CurrentPassNumber = 2;
		RichTextBox val = null;
		for (int l = 0; l < stiMatrix.CoordY.Count - 1; l++)
		{
			InvokeExporting(l, stiMatrix.CoordY.Count - 1, base.CurrentPassNumber, base.MaximumPassNumber);
			if (base.IsStopped)
			{
				return;
			}
			int num5 = 0;
			for (int m = 0; m < stiMatrix.CoordX.Count - 1; m++)
			{
				string text4 = null;
				string text5 = null;
				string text6 = null;
				string text7 = null;
				Font val2 = null;
				int color = 0;
				bool flag2 = false;
				bool flag3 = false;
				if (num5 > 0)
				{
					flag3 = true;
					num5--;
				}
				StiCell stiCell = stiMatrix.Cells[l, m];
				if (stiCell != null)
				{
					StiText stiText = stiCell.Component as StiText;
					StiRichText stiRichText = stiCell.Component as StiRichText;
					if (!string.IsNullOrEmpty(stiCell.Text))
					{
						text4 = stiCell.Text;
					}
					if (stiRichText != null && stiRichText.RtfText != string.Empty)
					{
						if (val == null)
						{
							val = (RichTextBox)(object)new StiRichTextBox(drawTransparent: false);
						}
						stiRichText.GetPreparedText(val);
						text4 = ((Control)val).Text;
					}
					if (text4 == null)
					{
						text4 = string.Empty;
					}
					StringBuilder stringBuilder = new StringBuilder();
					foreach (int num6 in text4)
					{
						if (num6 < 32)
						{
							string value = $"\u001b{(char)(32 + ((num6 >> 4) & 0xF))}{(char)(48 + (num6 & 0xF))}";
							stringBuilder.Append(value);
						}
						else
						{
							stringBuilder.Append((char)num6);
						}
					}
					text4 = StiExportUtils.TrimEndWhiteSpace(stringBuilder.ToString());
					val2 = stiCell.CellStyle.Font;
					color = GetColorIndex(stiCell.CellStyle.TextColor);
					if (stiCell.CellStyle.Color != Color.Transparent && stiCell.CellStyle.Color.R + stiCell.CellStyle.Color.G + stiCell.CellStyle.Color.B < 544)
					{
						flag3 = true;
						num5 = stiCell.Width;
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
					string text9 = string.Empty;
					int num7 = 2;
					int num8 = 0;
					string text10 = "$";
					bool flag9 = false;
					bool flag10 = false;
					bool flag11 = stiText?.HideZeros ?? false;
					string text11 = null;
					string text12 = null;
					int num9 = text8?.IndexOf("|") ?? (-1);
					if (num9 != -1)
					{
						text11 = StiExportUtils.GetPositivePattern(text8[num9 + 1] - 65);
						text12 = StiExportUtils.GetNegativePattern(text8[num9 + 2] - 65);
						text8 = text8.Substring(0, num9);
					}
					if (!string.IsNullOrEmpty(text8))
					{
						if (text8[0] == 'C')
						{
							flag4 = true;
						}
						if (text8[0] == 'N')
						{
							flag5 = true;
						}
						if (text8[0] == 'P')
						{
							flag6 = true;
						}
						if (text8[0] == 'D')
						{
							flag7 = true;
						}
						if (text8[0] == 'T')
						{
							flag8 = true;
						}
						if (text8.Length > 1 && (flag4 || flag5 || flag6))
						{
							int num10 = 1;
							if (char.IsDigit(text8[num10]))
							{
								StringBuilder stringBuilder2 = new StringBuilder();
								for (; num10 < text8.Length && char.IsDigit(text8[num10]); num10++)
								{
									stringBuilder2.Append(text8[num10]);
								}
								num7 = int.Parse(stringBuilder2.ToString());
							}
							if (num10 < text8.Length && text8[num10] == 'G')
							{
								num10++;
								num8 = 3;
							}
							if (num10 < text8.Length && text8[num10] == '(')
							{
								num10++;
								flag10 = true;
							}
							if (num10 < text8.Length && (text8[num10] == '+' || text8[num10] == '-'))
							{
								if (text8[num10] == '+')
								{
									flag9 = true;
								}
								num10++;
								if (num10 < text8.Length)
								{
									text10 = text8.Substring(num10);
								}
							}
						}
					}
					if (flag4 || flag5 || flag6)
					{
						if (num9 != -1)
						{
							StringBuilder stringBuilder3 = new StringBuilder();
							if (num8 > 1)
							{
								stringBuilder3.Append("#,");
								stringBuilder3.Append('#', num8 - 1);
							}
							stringBuilder3.Append('0');
							if (num7 > 0)
							{
								stringBuilder3.Append(".");
								stringBuilder3.Append('0', num7);
							}
							string newValue = stringBuilder3.ToString();
							string text13 = text11.Replace("n", newValue).Replace("$", "\"" + text10 + "\"");
							string text14 = text12.Replace("n", newValue).Replace("$", "\"" + text10 + "\"");
							text9 = text13 + ";" + text14 + (flag11 ? ";" : "");
						}
						else
						{
							StringBuilder stringBuilder4 = new StringBuilder();
							if (flag4 && flag9)
							{
								stringBuilder4.Append("\"");
								stringBuilder4.Append(text10);
								stringBuilder4.Append("\"");
							}
							if (num8 > 1)
							{
								stringBuilder4.Append("#,");
								stringBuilder4.Append('#', num8 - 1);
							}
							stringBuilder4.Append('0');
							if (num7 > 0)
							{
								stringBuilder4.Append(".");
								stringBuilder4.Append('0', num7);
							}
							if (flag4 && !flag9)
							{
								stringBuilder4.Append("\"");
								stringBuilder4.Append(text10);
								stringBuilder4.Append("\"");
							}
							if (flag6)
							{
								stringBuilder4.Append("%");
							}
							text9 = stringBuilder4.ToString();
							string text15 = (flag10 ? "(" : "-") + text9 + (flag10 ? ")" : "");
							if (stiText != null && stiText.HideZeros)
							{
								text9 = text9 + ";" + text15 + ";";
							}
							else if (flag10)
							{
								text9 = text9 + ";" + text15;
							}
						}
					}
					if (flag7)
					{
						text9 = "dd/mm/yyyy";
					}
					if (flag8)
					{
						text9 = "[$-F400]h:mm:ss\\ AM/PM";
					}
					if (text9 != string.Empty)
					{
						text7 = $"P{GetFormatNumber(text9)};";
					}
					string arg = "G0";
					if (flag5)
					{
						arg = $"F{num7}";
					}
					if (flag4)
					{
						arg = $"${num7}";
					}
					if (flag6)
					{
						arg = $"%{num7}";
					}
					string arg2 = "L";
					if ((int)stiCell.CellStyle.HorAlignment == 2)
					{
						arg2 = "R";
					}
					if ((int)stiCell.CellStyle.HorAlignment == 1)
					{
						arg2 = "C";
					}
					if ((int)stiCell.CellStyle.HorAlignment == 3)
					{
						arg2 = "X";
					}
					text6 = $"F{arg}{arg2};";
					if (stiText != null && stiText.ExcelDataValue != null && stiText.ExcelDataValue != "-")
					{
						double result = 0.0;
						string excelDataValue = stiText.ExcelDataValue;
						string numberDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
						string s = excelDataValue.Replace(".", ",").Replace(",", numberDecimalSeparator);
						flag2 = true;
						try
						{
							if (flag7 || flag8)
							{
								if (DateTime.TryParse(s, CultureInfo.InstalledUICulture, DateTimeStyles.None, out var result2))
								{
									result = ((!flag7) ? (result2.TimeOfDay.TotalSeconds / 86400.0) : ((double)(result2.Subtract(new DateTime(1900, 1, 1)).Days + 1 + 1)));
								}
								else
								{
									flag2 = false;
								}
							}
							else if (!double.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out result))
							{
								flag2 = false;
							}
						}
						catch (FormatException)
						{
							flag2 = false;
						}
						if (flag2)
						{
							text4 = result.ToString().Replace(",", ".");
						}
					}
				}
				bool flag12 = stiMatrix.BordersY[l, m] != (StiBorderSide)null;
				bool flag13 = stiMatrix.BordersY[l, m + 1] != (StiBorderSide)null;
				bool flag14 = stiMatrix.BordersX[l, m] != (StiBorderSide)null;
				bool flag15 = stiMatrix.BordersX[l + 1, m] != (StiBorderSide)null;
				if (flag12 || flag13 || flag14 || flag15 || val2 != null || flag3)
				{
					int num11 = 0;
					if (val2 != null)
					{
						num11 = GetFontNumber(new DataFont(val2.Name, val2.Bold, val2.Italic, val2.Underline, (int)(val2.SizeInPoints * 20f), color));
					}
					text5 = string.Format("S{0}{1}{2}{3}{4}{5}{6}{7};", (val2 != null && val2.Bold) ? "D" : "", (val2 != null && val2.Italic) ? "I" : "", flag12 ? "L" : "", flag13 ? "R" : "", flag14 ? "T" : "", flag15 ? "B" : "", flag3 ? "S" : "", (val2 != null) ? $"M{num11 + 1}" : "");
				}
				if (text6 == null && text5 == null && text4 == null && text7 == null)
				{
					continue;
				}
				writer.WriteLine("F;{0}{1}{2}Y{3};X{4}", text7, text6, text5, l + 1, m + 1);
				if (!string.IsNullOrEmpty(text4))
				{
					if (flag2)
					{
						writer.WriteLine("C;K{0}", text4);
					}
					else
					{
						writer.WriteLine("C;K\"{0}\"", text4);
					}
				}
			}
		}
		writer.Flush();
		writer = new StreamWriter(stream, encoding);
		writer.WriteLine("ID;PWXL;N;E");
		for (int num12 = 0; num12 < formatList.Count; num12++)
		{
			writer.WriteLine("P;P{0}", formatList[num12]);
		}
		for (int num13 = 0; num13 < fontList.Count; num13++)
		{
			DataFont dataFont = fontList[num13];
			StringBuilder stringBuilder5 = new StringBuilder("P;");
			stringBuilder5.Append(string.Format("{0}{1};M{2}", (num13 < 4) ? "F" : "E", dataFont.Name, dataFont.Height));
			if (dataFont.Bold || dataFont.Italic || dataFont.Underlined)
			{
				stringBuilder5.Append(";S");
				if (dataFont.Bold)
				{
					stringBuilder5.Append("B");
				}
				if (dataFont.Italic)
				{
					stringBuilder5.Append("I");
				}
				if (dataFont.Underlined)
				{
					stringBuilder5.Append("U");
				}
			}
			if (dataFont.Color != 0)
			{
				stringBuilder5.Append($";L{dataFont.Color}");
			}
			writer.WriteLine(stringBuilder5.ToString());
		}
		writer.WriteLine("F;P0;DG0G8;M255");
		writer.WriteLine("B;Y{0};X{1};D0 0 {2} {3}", stiMatrix.CoordY.Count - 1, stiMatrix.CoordX.Count - 1, stiMatrix.CoordY.Count - 2, stiMatrix.CoordX.Count - 2);
		for (int num14 = 1; num14 < stiMatrix.CoordX.Count; num14++)
		{
			double num15 = (double)stiMatrix.CoordX.GetByIndex(num14);
			double num16 = (double)stiMatrix.CoordX.GetByIndex(num14 - 1);
			int num17 = (int)Math.Round((num15 - num16) * HiToTwips * 0.008699796000000001);
			if (num17 == 0)
			{
				num17++;
			}
			writer.WriteLine("F;W{0} {0} {1}", num14, num17);
		}
		for (int num18 = 1; num18 < stiMatrix.CoordY.Count; num18++)
		{
			int num19 = (int)Math.Round(((double)stiMatrix.CoordY.GetByIndex(num18) - (double)stiMatrix.CoordY.GetByIndex(num18 - 1)) * HiToTwips);
			if (num19 == 0)
			{
				num19++;
			}
			writer.WriteLine("F;M{0};R{1}", num19, num18);
		}
		writer.Flush();
		memoryStream.WriteTo(stream);
		writer.WriteLine("E");
		writer.Flush();
		if (stiMatrix != null)
		{
			stiMatrix.Clear();
			stiMatrix = null;
		}
		formatList = null;
		fontList = null;
		colorHash = null;
	}
}
