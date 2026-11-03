using System;
using System.Collections;
using System.Collections.Generic;
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
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.Export.Tools;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Word.png")]
public class StiWord2007ExportService : StiExportService
{
	private struct StiWord2007StyleInfo
	{
		public string Name;

		public StiTextHorAlignment Alignment;

		public bool RightToLeft;

		public string FontName;

		public int FontSize;

		public bool Bold;

		public bool Italic;

		public bool Underline;

		public Color TextColor;
	}

	private StiReport report;

	private string fileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	private StiMatrix matrix;

	private Hashtable fontList;

	private ArrayList styleList;

	private StiImageCache imageCache;

	private Hashtable bookmarkList;

	private Hashtable hyperlinkList;

	private ArrayList embedsList;

	private int xmlIndentation = 1;

	private float imageQuality = 0.75f;

	private float imageResolution = 96f;

	private string lineSpace;

	private string lineSpace2;

	private bool usePageHeadersAndFooters;

	private StiWord2007RestrictEditing restrictEditing = StiWord2007RestrictEditing.No;

	private ArrayList headersData;

	private ArrayList headersRels;

	private ArrayList footersData;

	private ArrayList footersRels;

	private string docCompanyString;

	private string docLastModifiedString;

	private Hashtable fontsToCorrectHeight = new Hashtable
	{
		{ "Arial Unicode MS", null },
		{ "Cascadia Code ExtraLight", null },
		{ "Cascadia Code SemiBold", null },
		{ "Cascadia Code SemiLight", null },
		{ "Cascadia Mono ExtraLight", null },
		{ "Cascadia Mono Light", null },
		{ "Cascadia Mono SemiBold", null },
		{ "Cascadia Mono SemiLight", null },
		{ "Malgun Gothic", null },
		{ "Malgun Gothic Semilight", null },
		{ "Microsoft JhengHei", null },
		{ "Microsoft JhengHei Light", null },
		{ "Microsoft JhengHei UI", null },
		{ "Microsoft JhengHei UI Light", null },
		{ "Microsoft YaHei", null },
		{ "Microsoft YaHei Light", null },
		{ "Microsoft YaHei UI", null },
		{ "Microsoft YaHei UI Light", null },
		{ "MingLiU_HKSCS-ExtB", null },
		{ "MingLiU-ExtB", null },
		{ "MS Gothic", null },
		{ "MS PGothic", null },
		{ "MS UI Gothic", null },
		{ "NSimSun", null },
		{ "PMingLiU-ExtB", null },
		{ "Segoe MDL2 Assets", null },
		{ "SimSun", null },
		{ "SimSun-ExtB", null },
		{ "Yu Gothic", null },
		{ "Yu Gothic Light", null },
		{ "Yu Gothic Medium", null },
		{ "Yu Gothic UI", null },
		{ "Yu Gothic UI Light", null },
		{ "Yu Gothic UI Semibold", null },
		{ "Yu Gothic UI Semilight", null },
		{ "微软雅黑", null }
	};

	private string wrongUrlSymbols = " \"#'*,;<>[^`{|}";

	public override string DefaultExtension => "docx";

	public override StiExportFormat ExportFormat => StiExportFormat.Word2007;

	public override string GroupCategory => "Word";

	public override int Position => 22;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeWord2007File");

	public override bool MultipleFiles => false;

	internal StiMatrix Matrix => matrix;

	internal bool RemoveEmptySpaceAtBottom { get; private set; } = StiOptions.Export.Word.RemoveEmptySpaceAtBottom;


	private static double HiToTwips => 14.328;

	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		ExportWord(report, stream, settings as StiWord2007ExportSettings);
		InvokeExporting(100, 100, 1, 1);
	}

	public override void Export(StiReport report, string fileName, bool sendEMail, StiGuiMode guiMode)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		IStiFormRunner exportFormRunner = StiGuiOptions.GetExportFormRunner("StiWord2007ExportSetupForm", guiMode, base.OwnerWindow);
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
		return StiLocalization.Get("FileFilters", "Word2007Files");
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
				StiWord2007ExportSettings stiWord2007ExportSettings = new StiWord2007ExportSettings();
				stiWord2007ExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
				stiWord2007ExportSettings.UsePageHeadersAndFooters = (bool)form["UsePageHeadersAndFooters"];
				stiWord2007ExportSettings.ImageQuality = (float)form["ImageQuality"];
				stiWord2007ExportSettings.ImageResolution = (float)form["Resolution"];
				stiWord2007ExportSettings.RemoveEmptySpaceAtBottom = (bool)form["RemoveEmptySpaceAtBottom"];
				stiWord2007ExportSettings.RestrictEditing = (StiWord2007RestrictEditing)form["RestrictEditing"];
				StiWord2007ExportSettings settings = stiWord2007ExportSettings;
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

	private static string GetImageExt(ImageFormat format)
	{
		string result = "jpeg";
		if (format == ImageFormat.Emf)
		{
			result = "emf";
		}
		if (format == ImageFormat.Png)
		{
			result = "png";
		}
		return result;
	}

	private string GetLineStyle(StiPenStyle penStyle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected I4, but got Unknown
		return (int)penStyle switch
		{
			0 => "single", 
			4 => "dotted", 
			1 => "dashSmallGap", 
			2 => "dotDash", 
			3 => "dotDotDash", 
			5 => "double", 
			_ => string.Empty, 
		};
	}

	private string GetColorString(Color color)
	{
		if (color.A == 0)
		{
			return "auto";
		}
		if (color.A < 32)
		{
			return "FFFFFF";
		}
		return color.ToArgb().ToString("X8").Substring(2);
	}

	private int GetStyleNumber(ArrayList tmpStyleList, StiWord2007StyleInfo styleInfo)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (tmpStyleList.Count > 0)
		{
			for (int i = 0; i < tmpStyleList.Count; i++)
			{
				StiWord2007StyleInfo stiWord2007StyleInfo = (StiWord2007StyleInfo)tmpStyleList[i];
				if (stiWord2007StyleInfo.Alignment == styleInfo.Alignment && stiWord2007StyleInfo.Name == styleInfo.Name && stiWord2007StyleInfo.FontName == styleInfo.FontName && stiWord2007StyleInfo.FontSize == styleInfo.FontSize && stiWord2007StyleInfo.Bold == styleInfo.Bold && stiWord2007StyleInfo.Italic == styleInfo.Italic && stiWord2007StyleInfo.Underline == styleInfo.Underline && stiWord2007StyleInfo.TextColor == styleInfo.TextColor && stiWord2007StyleInfo.RightToLeft == styleInfo.RightToLeft)
				{
					return i;
				}
			}
		}
		tmpStyleList.Add(styleInfo);
		return tmpStyleList.Count - 1;
	}

	private int GetStyleFromComponent(StiComponent component)
	{
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		if (component != null && !string.IsNullOrEmpty(component.ComponentStyle))
		{
			IStiFont stiFont = component as IStiFont;
			IStiTextBrush stiTextBrush = component as IStiTextBrush;
			IStiTextHorAlignment stiTextHorAlignment = component as IStiTextHorAlignment;
			IStiTextOptions stiTextOptions = component as IStiTextOptions;
			StiWord2007StyleInfo styleInfo = default(StiWord2007StyleInfo);
			styleInfo.Name = component.ComponentStyle;
			if (stiFont != null)
			{
				styleInfo.FontName = stiFont.Font.Name;
				styleInfo.FontSize = (int)Math.Round(stiFont.Font.SizeInPoints * 2f, 0);
				styleInfo.Bold = stiFont.Font.Bold;
				styleInfo.Italic = stiFont.Font.Italic;
				styleInfo.Underline = stiFont.Font.Underline;
			}
			if (stiTextBrush != null)
			{
				styleInfo.TextColor = StiBrush.ToColor(stiTextBrush.TextBrush);
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
		return -1;
	}

	private string StringToUrl(string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (input.StartsWith("file:"))
		{
			foreach (char c in input)
			{
				if (c < ' ' || c == '"')
				{
					stringBuilder.Append($"%{c:x2}");
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
		}
		else
		{
			foreach (char c2 in input)
			{
				if (c2 < ' ' || wrongUrlSymbols.IndexOf(c2) != -1)
				{
					stringBuilder.Append('_');
				}
				else
				{
					stringBuilder.Append(c2);
				}
			}
		}
		return stringBuilder.ToString();
	}

	private int GetHyperlinkRefId(string url)
	{
		if (url.StartsWith("www."))
		{
			url = "http:\\" + url;
		}
		int num;
		if (hyperlinkList.ContainsKey(url))
		{
			num = (int)hyperlinkList[url];
		}
		else
		{
			num = hyperlinkList.Count;
			hyperlinkList.Add(url, num);
		}
		return num;
	}

	private int Convert(double x)
	{
		return (int)Math.Round((decimal)(x * HiToTwips));
	}

	private int ConvertHiToTwips(double x)
	{
		return (int)Math.Round((decimal)(x * 14.4));
	}

	private int ConvertTwipsToEmu(double x)
	{
		return (int)Math.Round((decimal)(x / HiToTwips / 100.0 * 914400.0));
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
		if (stringBuilder.Length > 0 && char.IsDigit(stringBuilder[0]))
		{
			stringBuilder.Insert(0, 'b');
		}
		return stringBuilder.ToString();
	}

	private void WriteFromMatrix(XmlTextWriter writer, int startLine, int endLine, bool outHeadersAndFooters)
	{
		//IL_0afd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2d: Invalid comparison between Unknown and I4
		//IL_0b2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b38: Unknown result type (might be due to invalid IL or missing references)
		//IL_095b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0960: Unknown result type (might be due to invalid IL or missing references)
		//IL_0962: Unknown result type (might be due to invalid IL or missing references)
		//IL_0965: Invalid comparison between Unknown and I4
		//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b51: Invalid comparison between Unknown and I4
		//IL_0967: Unknown result type (might be due to invalid IL or missing references)
		//IL_096a: Invalid comparison between Unknown and I4
		int count = Matrix.CoordX.Count;
		int[] array = new int[Matrix.CoordX.Count];
		for (int i = 0; i < Matrix.CoordX.Count; i++)
		{
			double x = (double)Matrix.CoordX.GetByIndex(i);
			array[i] = Convert(x);
		}
		int[] array2 = new int[Matrix.CoordY.Count];
		for (int j = 0; j < Matrix.CoordY.Count; j++)
		{
			double x2 = (double)Matrix.CoordY.GetByIndex(j);
			array2[j] = Convert(x2);
		}
		bool flag = false;
		int num = 0;
		bool[,] array3 = new bool[Matrix.CoordY.Count, Matrix.CoordX.Count];
		bool[,] array4 = new bool[Matrix.CoordY.Count, Matrix.CoordX.Count];
		string text = string.Empty;
		base.CurrentPassNumber = 2 + (StiOptions.Export.Word.DivideSegmentPages ? 1 : 0);
		double num2 = Math.Max((float)Matrix.CoordY.Count / 200f, 1f);
		int num3 = 0;
		for (int k = 1; k < Matrix.CoordY.Count; k++)
		{
			int num4 = (int)((double)k / num2);
			if (num4 > num3)
			{
				num3 = num4;
				InvokeExporting(k, matrix.CoordY.Count, base.CurrentPassNumber, base.MaximumPassNumber);
			}
			bool flag2 = k - 1 >= startLine && k - 1 <= endLine;
			if (!outHeadersAndFooters && (matrix.LinePlacement[k - 1] == StiMatrix.StiTableLineInfo.PageHeader || matrix.LinePlacement[k - 1] == StiMatrix.StiTableLineInfo.PageFooter || matrix.LinePlacement[k - 1] == StiMatrix.StiTableLineInfo.Trash))
			{
				flag2 = false;
			}
			if (num > 0)
			{
				num--;
				flag2 = false;
			}
			double num5 = 100005.0;
			ArrayList arrayList = new ArrayList();
			bool flag3 = false;
			if (flag2)
			{
				for (int l = 1; l < count; l++)
				{
					StiCell stiCell = Matrix.Cells[k - 1, l - 1];
					if (array3[k, l] || stiCell == null || stiCell.Component == null)
					{
						continue;
					}
					if (stiCell.Component.TagValue != null)
					{
						string text2 = stiCell.Component.TagValue.ToString().ToLower();
						if (text2.IndexOf("rtfparagraph", StringComparison.InvariantCulture) != -1)
						{
							arrayList.Add(stiCell);
							flag2 = false;
						}
						if (text2.IndexOf("rtfnewpage", StringComparison.InvariantCulture) != -1)
						{
							flag3 = true;
						}
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
						if (stiMargins.Top < num5)
						{
							num5 = stiMargins.Top;
						}
						if (stiCell.Height > 0)
						{
							num5 = 0.0;
						}
					}
				}
			}
			num5 *= HiToTwips;
			int num6 = (int)Math.Round(num5);
			if (flag3)
			{
				if (flag)
				{
					writer.WriteFullEndElement();
					flag = false;
				}
				writer.WriteStartElement("w:p");
				writer.WriteStartElement("w:r");
				writer.WriteStartElement("w:br");
				writer.WriteAttributeString("w:type", "page");
				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
			if (flag2)
			{
				if (usePageHeadersAndFooters && matrix.LinePlacement[k - 1] == StiMatrix.StiTableLineInfo.HeaderAP)
				{
					string text3 = matrix.ParentBandName[k - 1];
					int num7 = text3.IndexOf('\u001f');
					if (num7 != -1)
					{
						text3 = text3.Substring(0, num7);
					}
					if (text3 != text)
					{
						text = text3;
						if (flag)
						{
							writer.WriteFullEndElement();
						}
						writer.WriteStartElement("w:p");
						writer.WriteEndElement();
						writer.WriteStartElement("w:tbl");
						writeTableInfo(writer, array, count);
						flag = true;
					}
				}
				if (!flag)
				{
					writer.WriteStartElement("w:tbl");
					writeTableInfo(writer, array, count);
					flag = true;
				}
				writer.WriteStartElement("w:tr");
				int num8 = array2[k] - array2[k - 1];
				writer.WriteStartElement("w:trPr");
				if (!usePageHeadersAndFooters || StiOptions.Export.Word.ForceLineHeight)
				{
					writer.WriteStartElement("w:trHeight");
					if ((!usePageHeadersAndFooters && StiOptions.Export.Word.LineHeightExactly) || (usePageHeadersAndFooters && StiOptions.Export.Word.LineHeightExactlyForPHFMode))
					{
						writer.WriteAttributeString("w:hRule", "exact");
					}
					writer.WriteAttributeString("w:val", $"{num8}");
					writer.WriteEndElement();
				}
				if (usePageHeadersAndFooters && matrix.LinePlacement[k - 1] == StiMatrix.StiTableLineInfo.HeaderAP)
				{
					writer.WriteStartElement("w:tblHeader");
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			int num9 = 0;
			for (int m = 1; m < count; m++)
			{
				StiCell stiCell2 = Matrix.Cells[k - 1, m - 1];
				if (!array3[k, m] || array4[k, m])
				{
					num9++;
					bool flag4 = true;
					if (array4[k, m])
					{
						int num10 = k;
						while (array4[num10, m])
						{
							num10--;
						}
						stiCell2 = Matrix.Cells[num10 - 1, m - 1];
						flag4 = false;
					}
					if (stiCell2 != null && flag4)
					{
						for (int n = 0; n <= stiCell2.Height; n++)
						{
							for (int num11 = 0; num11 <= stiCell2.Width; num11++)
							{
								array3[k + n, m + num11] = true;
							}
						}
						if (stiCell2.Height > 0)
						{
							for (int num12 = 1; num12 <= stiCell2.Height; num12++)
							{
								array4[k + num12, m] = true;
							}
						}
					}
					if (flag2)
					{
						writer.WriteStartElement("w:tc");
						if (stiCell2 != null)
						{
							writer.WriteStartElement("w:tcPr");
							int num13 = 0;
							for (int num14 = 0; num14 < stiCell2.Width + 1; num14++)
							{
								num13 += array[m + num14] - array[m - 1 + num14];
							}
							writer.WriteStartElement("w:tcW");
							writer.WriteAttributeString("w:w", $"{num13}");
							writer.WriteAttributeString("w:type", "dxa");
							writer.WriteEndElement();
							if (stiCell2.Width > 0)
							{
								writer.WriteStartElement("w:gridSpan");
								writer.WriteAttributeString("w:val", $"{stiCell2.Width + 1}");
								writer.WriteEndElement();
							}
							if (stiCell2.Height > 0)
							{
								if (!array4[k, m])
								{
									writer.WriteStartElement("w:vMerge");
									writer.WriteAttributeString("w:val", "restart");
									writer.WriteEndElement();
								}
								else
								{
									writer.WriteStartElement("w:vMerge");
									writer.WriteEndElement();
								}
							}
							if (stiCell2.CellStyle.TextOptions != null)
							{
								float angle = stiCell2.CellStyle.TextOptions.Angle;
								string text4 = string.Empty;
								if (angle > 45f && angle < 135f)
								{
									text4 = "btLr";
								}
								if (angle > 225f && angle < 315f)
								{
									text4 = "tbRl";
								}
								if (text4 != string.Empty)
								{
									writer.WriteStartElement("w:textDirection");
									writer.WriteAttributeString("w:val", text4);
									writer.WriteEndElement();
								}
							}
							bool flag5 = stiCell2.Component is StiText && (stiCell2.Component as StiText).CheckAllowHtmlTags() && !StiOptions.Export.Word.RenderHtmlTagsAsImage;
							if (flag4 && stiCell2.Component is StiText && (!stiCell2.Component.IsExportAsImage(StiExportFormat.Word2007) || flag5))
							{
								StiText stiText = stiCell2.Component as StiText;
								int num15 = Convert(stiText.Margins.Left);
								int num16 = Convert(stiText.Margins.Right - (double)StiOptions.Export.Word.RightMarginCorrection);
								int num17 = Convert(stiText.Margins.Top);
								if (num17 > num6)
								{
									num17 = num6;
								}
								if (num15 > 0 || num16 > 0 || num17 > 0)
								{
									writer.WriteStartElement("w:tcMar");
									if (num17 > 0)
									{
										writer.WriteStartElement("w:top");
										writer.WriteAttributeString("w:w", $"{num17}");
										writer.WriteAttributeString("w:type", "dxa");
										writer.WriteEndElement();
									}
									if (num15 > 0)
									{
										writer.WriteStartElement("w:left");
										writer.WriteAttributeString("w:w", $"{num15}");
										writer.WriteAttributeString("w:type", "dxa");
										writer.WriteEndElement();
									}
									if (num16 > 0)
									{
										writer.WriteStartElement("w:right");
										writer.WriteAttributeString("w:w", $"{num16}");
										writer.WriteAttributeString("w:type", "dxa");
										writer.WriteEndElement();
									}
									writer.WriteEndElement();
								}
							}
							string text5 = string.Empty;
							StiVertAlignment vertAlignment = stiCell2.CellStyle.VertAlignment;
							if ((int)vertAlignment != 1)
							{
								if ((int)vertAlignment == 2)
								{
									text5 = "bottom";
								}
							}
							else
							{
								text5 = "center";
							}
							if (text5 != string.Empty)
							{
								writer.WriteStartElement("w:vAlign");
								writer.WriteAttributeString("w:val", text5);
								writer.WriteEndElement();
							}
							WriteBorders(writer, stiCell2.Top, stiCell2.Left, stiCell2.Height + 1, stiCell2.Width + 1);
							if (stiCell2.Component != null && !stiCell2.Component.IsExportAsImage(StiExportFormat.Word2007))
							{
								writer.WriteStartElement("w:shd");
								writer.WriteAttributeString("w:val", "clear");
								writer.WriteAttributeString("w:color", "auto");
								writer.WriteAttributeString("w:fill", GetColorString(stiCell2.CellStyle.Color));
								writer.WriteEndElement();
							}
							writer.WriteEndElement();
							bool flag6 = restrictEditing == StiWord2007RestrictEditing.ExceptEditableFields && stiCell2.Component != null && stiCell2.Component is StiText && (stiCell2.Component as StiText).Editable;
							if (flag6)
							{
								writer.WriteStartElement("w:permStart");
								writer.WriteAttributeString("w:id", "0");
								writer.WriteEndElement();
							}
							bool needEmptyParagraph = true;
							if (flag4)
							{
								WriteCellContent(writer, stiCell2, ref needEmptyParagraph, k, m, array, array2);
							}
							if (needEmptyParagraph)
							{
								writer.WriteStartElement("w:p");
								writer.WriteFullEndElement();
							}
							if (flag6)
							{
								writer.WriteStartElement("w:permEnd");
								writer.WriteAttributeString("w:id", "0");
								writer.WriteEndElement();
							}
						}
						else
						{
							string styles = null;
							StiBorderSides val = RenderBorder2TableGetValues(k, m, ref styles);
							int num18 = 0;
							if ((val & 4) == 0)
							{
								while (m + num18 + 1 < count && Matrix.Cells[k - 1, m + num18 + 1 - 1] == null && !array3[k, m + num18 + 1] && !array4[k, m + num18 + 1])
								{
									string styles2 = null;
									StiBorderSides val2 = RenderBorder2TableGetValues(k, m + num18 + 1, ref styles2);
									if ((val2 & 2) > 0 || (val2 & 9) != (val & 9) || styles != styles2)
									{
										break;
									}
									num18++;
									if ((val2 & 4) > 0)
									{
										break;
									}
								}
								if (num18 > 0)
								{
									for (int num19 = 1; num19 <= num18; num19++)
									{
										array3[k, m + num19] = true;
									}
								}
							}
							writer.WriteStartElement("w:tcPr");
							int num20 = array[m + num18] - array[m - 1];
							writer.WriteStartElement("w:tcW");
							writer.WriteAttributeString("w:w", $"{num20}");
							writer.WriteAttributeString("w:type", "dxa");
							writer.WriteEndElement();
							if (num18 > 0)
							{
								writer.WriteStartElement("w:gridSpan");
								writer.WriteAttributeString("w:val", $"{num18 + 1}");
								writer.WriteEndElement();
							}
							WriteBorders(writer, k - 1, m - 1, 1, num18 + 1);
							writer.WriteEndElement();
							writer.WriteStartElement("w:p");
							writer.WriteFullEndElement();
						}
						writer.WriteFullEndElement();
					}
				}
				if (num9 >= 64)
				{
					break;
				}
			}
			if (flag2)
			{
				writer.WriteFullEndElement();
			}
			foreach (StiCell item in arrayList)
			{
				if (flag)
				{
					writer.WriteEndElement();
				}
				flag = false;
				num = item.Height;
				bool needEmptyParagraph2 = false;
				WriteCellContent(writer, item, ref needEmptyParagraph2, item.Top + 1, item.Left + 1, array, array2);
			}
		}
		if (flag)
		{
			writer.WriteEndElement();
		}
	}

	private void WriteCellContent(XmlTextWriter writer, StiCell cell, ref bool needEmptyParagraph, int indexRow, int indexColumn, int[] wordCoordX, int[] wordCoordY)
	{
		//IL_0cfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cff: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7a: Invalid comparison between Unknown and I4
		//IL_0d43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d49: Invalid comparison between Unknown and I4
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Invalid comparison between Unknown and I4
		//IL_0d87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8a: Invalid comparison between Unknown and I4
		//IL_0d50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d56: Invalid comparison between Unknown and I4
		//IL_0d4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Invalid comparison between Unknown and I4
		//IL_0d93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d96: Invalid comparison between Unknown and I4
		//IL_0d5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d63: Invalid comparison between Unknown and I4
		//IL_0d59: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Invalid comparison between Unknown and I4
		//IL_0d66: Unknown result type (might be due to invalid IL or missing references)
		string value = null;
		int num = -1;
		string text = Matrix.Bookmarks[indexRow - 1, indexColumn - 1];
		if (text != null && !bookmarkList.ContainsKey(text))
		{
			num = bookmarkList.Count;
			value = ConvertStringToBookmark(text);
			bookmarkList[text] = num;
		}
		string text2 = null;
		int num2 = -1;
		if (cell.Component != null && cell.Component.HyperlinkValue != null)
		{
			string text3 = cell.Component.HyperlinkValue.ToString().Trim();
			if (text3.Length > 0 && !text3.StartsWith("javascript:"))
			{
				if (text3.StartsWith("#", StringComparison.InvariantCulture))
				{
					text2 = ConvertStringToBookmark(text3.Substring(1));
				}
				else
				{
					text2 = StringToUrl(text3);
					num2 = GetHyperlinkRefId(text2);
				}
			}
		}
		if (cell.Component is StiText && !cell.Component.IsExportAsImage(StiExportFormat.Word2007) && !(cell.Component as StiText).CheckAllowHtmlTags())
		{
			StringBuilder stringBuilder = new StringBuilder(cell.Text);
			if (usePageHeadersAndFooters)
			{
				string text4 = cell.Component.TagValue as string;
				if (!string.IsNullOrEmpty(text4) && text4.ToLowerInvariant().IndexOf("rtfnewpage", StringComparison.InvariantCulture) == -1 && (text4.IndexOf("#PageNumber#", StringComparison.InvariantCulture) != -1 || text4.IndexOf("#TotalPageCount#", StringComparison.InvariantCulture) != -1 || text4.IndexOf("#PageRef", StringComparison.InvariantCulture) != -1))
				{
					stringBuilder = new StringBuilder(text4);
				}
			}
			ArrayList arrayList = new ArrayList();
			string text5 = string.Empty;
			string text6 = stringBuilder.ToString();
			for (int i = 0; i < text6.Length; i++)
			{
				char c = text6[i];
				if (char.IsControl(c))
				{
					switch (c)
					{
					case '\n':
						arrayList.Add(text5);
						text5 = string.Empty;
						continue;
					case '\t':
						break;
					default:
						continue;
					}
				}
				text5 += c;
			}
			if (text5 != string.Empty)
			{
				arrayList.Add(text5);
			}
			if (arrayList.Count == 0)
			{
				arrayList.Add(text5);
			}
			if (arrayList.Count > 0)
			{
				needEmptyParagraph = false;
			}
			int styleFromComponent = GetStyleFromComponent(cell.Component);
			for (int j = 0; j < arrayList.Count; j++)
			{
				string text7 = (string)arrayList[j];
				writer.WriteStartElement("w:p");
				writer.WriteStartElement("w:pPr");
				float lineSpacing = (cell.Component as StiText).LineSpacing;
				if (lineSpace != null || lineSpacing != 1f)
				{
					string value2 = lineSpace;
					if (lineSpacing != 1f)
					{
						value2 = ((int)Math.Round(StiOptions.Export.Word.LineSpacing * 240.0 * (double)lineSpacing)).ToString();
					}
					if (cell.Component is IStiFont { Font: not null } stiFont)
					{
						string name = stiFont.Font.Name;
						if (fontsToCorrectHeight.ContainsKey(name))
						{
							value2 = lineSpace2;
							if (lineSpacing != 1f)
							{
								value2 = ((int)Math.Round(StiOptions.Export.Word.LineSpacing * 240.0 * 0.772 * (double)lineSpacing)).ToString();
							}
						}
					}
					writer.WriteStartElement("w:spacing");
					writer.WriteAttributeString("w:line", value2);
					writer.WriteEndElement();
				}
				if (styleFromComponent != -1)
				{
					writer.WriteStartElement("w:pStyle");
					writer.WriteAttributeString("w:val", $"Style{styleFromComponent}");
					writer.WriteEndElement();
				}
				StiTextHorAlignment horAlignment = cell.CellStyle.HorAlignment;
				bool flag = false;
				if (cell.CellStyle.TextOptions != null)
				{
					flag = cell.CellStyle.TextOptions.RightToLeft;
				}
				string text8 = string.Empty;
				if (((int)horAlignment == 0 && flag) || ((int)horAlignment == 2 && !flag))
				{
					text8 = "right";
				}
				if ((int)horAlignment == 1)
				{
					text8 = "center";
				}
				if ((int)horAlignment == 3)
				{
					text8 = "both";
				}
				if (text8 != string.Empty)
				{
					writer.WriteStartElement("w:jc");
					writer.WriteAttributeString("w:val", text8);
					writer.WriteEndElement();
				}
				if (styleFromComponent == -1)
				{
					writeRunProperties(writer, cell);
				}
				writer.WriteEndElement();
				if (text2 != null)
				{
					writer.WriteStartElement("w:hyperlink");
					if (num2 == -1)
					{
						writer.WriteAttributeString("w:anchor", text2);
					}
					else
					{
						writer.WriteAttributeString("r:id", $"hId{num2}");
					}
				}
				if (num != -1)
				{
					writer.WriteStartElement("w:bookmarkStart");
					writer.WriteAttributeString("w:id", num.ToString(CultureInfo.InvariantCulture));
					writer.WriteAttributeString("w:name", value);
					writer.WriteEndElement();
				}
				if (text7 != string.Empty)
				{
					if (usePageHeadersAndFooters)
					{
						text7 = text7.Replace("#PageNumber#", "\u0001").Replace("#TotalPageCount#", "\u0002");
						ArrayList arrayList2 = new ArrayList();
						StringBuilder stringBuilder2 = new StringBuilder();
						string text9 = text7;
						for (int k = 0; k < text9.Length; k++)
						{
							char c2 = text9[k];
							if (c2 == '\u0001' || c2 == '\u0002')
							{
								if (stringBuilder2.Length > 0)
								{
									arrayList2.Add(stringBuilder2.ToString());
								}
								arrayList2.Add(c2.ToString(CultureInfo.InvariantCulture));
								stringBuilder2 = new StringBuilder();
							}
							else
							{
								stringBuilder2.Append(c2);
							}
						}
						if (stringBuilder2.Length > 0)
						{
							arrayList2.Add(stringBuilder2.ToString());
						}
						if (arrayList2.Count == 0)
						{
							arrayList2.Add(text7);
						}
						for (int l = 0; l < arrayList2.Count; l++)
						{
							string text10 = (string)arrayList2[l];
							if (text10.Length > 0 && (text10[0] == '\u0001' || text10[0] == '\u0002'))
							{
								writer.WriteStartElement("w:r");
								if (styleFromComponent == -1)
								{
									writeRunProperties(writer, cell);
								}
								writer.WriteStartElement("w:fldChar");
								writer.WriteAttributeString("w:fldCharType", "begin");
								writer.WriteEndElement();
								writer.WriteEndElement();
								writer.WriteStartElement("w:r");
								writeRunProperties(writer, cell);
								writer.WriteStartElement("w:instrText");
								switch (text10[0])
								{
								case '\u0001':
									writer.WriteString("PAGE");
									break;
								case '\u0002':
									writer.WriteString("NUMPAGES");
									break;
								}
								writer.WriteEndElement();
								writer.WriteEndElement();
								writer.WriteStartElement("w:r");
								writeRunProperties(writer, cell);
								writer.WriteStartElement("w:fldChar");
								writer.WriteAttributeString("w:fldCharType", "end");
								writer.WriteEndElement();
								writer.WriteEndElement();
							}
							else
							{
								writer.WriteStartElement("w:r");
								if (styleFromComponent == -1)
								{
									writeRunProperties(writer, cell);
								}
								writer.WriteStartElement("w:t");
								writer.WriteAttributeString("xml:space", "preserve");
								writer.WriteString(text10);
								writer.WriteEndElement();
								writer.WriteEndElement();
							}
						}
					}
					else
					{
						writer.WriteStartElement("w:r");
						if (styleFromComponent == -1)
						{
							writeRunProperties(writer, cell);
						}
						writer.WriteStartElement("w:t");
						writer.WriteAttributeString("xml:space", "preserve");
						writer.WriteString(text7);
						writer.WriteEndElement();
						writer.WriteEndElement();
					}
				}
				if (num != -1)
				{
					writer.WriteStartElement("w:bookmarkEnd");
					writer.WriteAttributeString("w:id", num.ToString(CultureInfo.InvariantCulture));
					writer.WriteEndElement();
				}
				if (text2 != null)
				{
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
		}
		else if (cell.Component is StiRichText && !StiOptions.Export.Word.RenderRichTextAsImage)
		{
			StiRichText stiRichText = cell.Component as StiRichText;
			string text11 = stiRichText.RtfText;
			int num3 = text11.TrimStart(new char[0]).ToLowerInvariant().IndexOf("rtf");
			if (num3 == -1 || num3 > 30)
			{
				StiRichTextBox richTextBox = new StiRichTextBox(drawTransparent: false);
				text11 = stiRichText.GetPreparedText((RichTextBox)(object)richTextBox);
			}
			if (StiOptions.Export.Word.AllowCorrectFontSize11Problem && !string.IsNullOrWhiteSpace(text11))
			{
				int num4 = text11.IndexOf("{\\stylesheet", StringComparison.InvariantCulture);
				int num5 = 0;
				if (num4 != -1)
				{
					int num6 = 0;
					num5 = num4;
					try
					{
						do
						{
							if (text11[num5] == '{')
							{
								num6++;
							}
							if (text11[num5] == '}')
							{
								num6--;
							}
							num5++;
						}
						while (num6 > 0);
					}
					catch
					{
					}
				}
				StringBuilder stringBuilder3 = new StringBuilder(text11);
				int num7 = -1;
				while ((num7 = text11.IndexOf("\\fs22", num7 + 1)) != -1)
				{
					if (num7 > num4 && num7 < num5)
					{
						stringBuilder3[num7 + 4] = '3';
					}
					else
					{
						stringBuilder3[num7 + 4] = '1';
					}
				}
				bool flag2 = false;
				int num8 = -1;
				while ((num8 = text11.IndexOf("\\sl", num8 + 1)) != -1)
				{
					int m;
					for (m = num8 + 3; char.IsDigit(text11, m); m++)
					{
					}
					int num9 = m - num8;
					if (num9 != 0 && int.TryParse(text11.Substring(num8, m - num8), out var result))
					{
						string text12 = (int)((double)result * StiOptions.Export.Word.RichTextLineSpacingCorrection) + "   ";
						for (int n = 0; n < num9; n++)
						{
							stringBuilder3[num8 + 3 + n] = text12[n];
						}
						flag2 = true;
					}
				}
				if (!flag2)
				{
					string text13 = $"\\sl{((int)(240.0 * StiOptions.Export.Word.RichTextLineSpacingCorrection)).ToString()}\\slmult1";
					string text14 = $"\\expndtw{StiOptions.Export.RichText.SpaceBetweenCharacters}";
					int num10 = text11.Length - 1;
					while ((num10 = text11.LastIndexOf("\\pard\\", num10)) != -1)
					{
						stringBuilder3.Insert(num10 + 5, text13 + text14);
					}
				}
				text11 = stringBuilder3.ToString();
				if (num4 != -1)
				{
					int num11 = text11.IndexOf(" Normal;}", num4);
					if (num11 != -1 && num11 < num5)
					{
						stringBuilder3.Insert(num11 + 7, "1");
						text11 = stringBuilder3.ToString();
					}
				}
			}
			embedsList.Add(text11);
			if (text2 != null)
			{
				writer.WriteStartElement("w:hyperlink");
				if (num2 == -1)
				{
					writer.WriteAttributeString("w:anchor", text2);
				}
				else
				{
					writer.WriteAttributeString("r:id", $"hId{num2}");
				}
			}
			writer.WriteStartElement("w:altChunk");
			writer.WriteAttributeString("r:id", $"reId{embedsList.Count}");
			writer.WriteEndElement();
			if (text2 != null)
			{
				writer.WriteEndElement();
			}
		}
		else if (cell.Component is StiText && (cell.Component as StiText).CheckAllowHtmlTags() && !StiOptions.Export.Word.RenderHtmlTagsAsImage)
		{
			if (string.IsNullOrWhiteSpace(cell.Text))
			{
				return;
			}
			if (StiOptions.Export.Word.RenderHtmlTagsAsEmbeddedRichText)
			{
				string text15 = StiRtfExportService.GetRtfFileFromHtmlTags(cell.Component as StiText, cell.Text);
				if (StiOptions.Export.Word.AllowCorrectFontSize11Problem)
				{
					text15 = text15.Replace("\\fs22", "\\fs21");
				}
				embedsList.Add(text15);
				writer.WriteStartElement("w:altChunk");
				writer.WriteAttributeString("r:id", $"reId{embedsList.Count}");
				writer.WriteEndElement();
			}
			else
			{
				writeHtmlTags(writer, cell);
			}
		}
		else
		{
			if (cell.Component != null && cell.Component.IsExportAsImage(StiExportFormat.Word2007))
			{
				if (!(cell.Component is IStiExportImage stiExportImage))
				{
					return;
				}
				float zoom = imageResolution;
				Image val = ((stiExportImage is IStiExportImageExtended stiExportImageExtended && stiExportImageExtended.IsExportAsImage(StiExportFormat.Word2007)) ? stiExportImageExtended.GetImage(ref zoom, StiExportFormat.Word2007) : stiExportImage.GetImage(ref zoom));
				try
				{
					if (val == null)
					{
						return;
					}
					Image realImageData = Matrix.GetRealImageData(cell, val);
					needEmptyParagraph = false;
					int num12 = 0;
					num12 = ((realImageData == null) ? imageCache.AddImageInt(val) : imageCache.AddImageInt(realImageData));
					int num13 = -1;
					int num14 = -1;
					if (cell.Component is StiRichText && StiOptions.Engine.FullTrust)
					{
						StiRichText stiRichText2 = cell.Component as StiRichText;
						stiRichText2.RenderMetafile();
						num13 = Convert((double)stiRichText2.Image.PhysicalDimension.Width / 25.4);
						num14 = Convert((double)stiRichText2.Image.PhysicalDimension.Height / 25.4);
						imageCache.ImagePackedStore[num12] = ConvertRichTextToImageInFullTrust(stiRichText2);
						imageCache.ImageFormatStore[num12] = ImageFormat.Emf;
					}
					string imageExt = GetImageExt(imageCache.ImageFormatStore[num12]);
					writer.WriteStartElement("w:p");
					writer.WriteStartElement("w:pPr");
					StiTextHorAlignment val2 = cell.CellStyle.HorAlignment;
					bool flag3 = false;
					if (cell.CellStyle.TextOptions != null)
					{
						flag3 = cell.CellStyle.TextOptions.RightToLeft;
					}
					if (!(cell.Component is IStiTextHorAlignment) && cell.Component is IStiHorAlignment stiHorAlignment)
					{
						if ((int)stiHorAlignment.HorAlignment == 1)
						{
							val2 = (StiTextHorAlignment)0;
						}
						if ((int)stiHorAlignment.HorAlignment == 2)
						{
							val2 = (StiTextHorAlignment)1;
						}
						if ((int)stiHorAlignment.HorAlignment == 3)
						{
							val2 = (StiTextHorAlignment)2;
						}
					}
					string text16 = string.Empty;
					if (((int)val2 == 0 && flag3) || ((int)val2 == 2 && !flag3))
					{
						text16 = "right";
					}
					if ((int)val2 == 1)
					{
						text16 = "center";
					}
					if ((int)val2 == 3)
					{
						text16 = "both";
					}
					if (text16 != string.Empty)
					{
						writer.WriteStartElement("w:jc");
						writer.WriteAttributeString("w:val", text16);
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
					if (num != -1)
					{
						writer.WriteStartElement("w:bookmarkStart");
						writer.WriteAttributeString("w:id", num.ToString(CultureInfo.InvariantCulture));
						writer.WriteAttributeString("w:name", value);
						writer.WriteEndElement();
					}
					writer.WriteStartElement("w:r");
					writer.WriteStartElement("w:rPr");
					writer.WriteElementString("w:noProof", "");
					writer.WriteEndElement();
					writer.WriteStartElement("w:drawing");
					writer.WriteStartElement("wp:inline");
					writer.WriteAttributeString("distT", "0");
					writer.WriteAttributeString("distB", "0");
					writer.WriteAttributeString("distL", "0");
					writer.WriteAttributeString("distR", "0");
					int num15 = wordCoordY[indexRow - 1 + cell.Height + 1] - wordCoordY[indexRow - 1];
					int num16 = wordCoordX[indexColumn - 1 + cell.Width + 1] - wordCoordX[indexColumn - 1];
					if (num13 != -1)
					{
						num16 = num13;
						num15 = num14;
					}
					writer.WriteStartElement("wp:extent");
					writer.WriteAttributeString("cx", $"{ConvertTwipsToEmu(num16)}");
					writer.WriteAttributeString("cy", $"{ConvertTwipsToEmu(num15)}");
					writer.WriteEndElement();
					writer.WriteStartElement("wp:effectExtent");
					writer.WriteAttributeString("l", "0");
					writer.WriteAttributeString("t", "0");
					writer.WriteAttributeString("r", "0");
					writer.WriteAttributeString("b", "0");
					writer.WriteEndElement();
					writer.WriteStartElement("wp:docPr");
					writer.WriteAttributeString("id", $"{num12 + 2}");
					writer.WriteAttributeString("name", $"Picture {num12 + 1}");
					writer.WriteAttributeString("descr", $"Image{num12 + 1:D5}.{imageExt}");
					if (text2 != null)
					{
						if (num2 == -1)
						{
							text2 = "#" + text2;
							num2 = GetHyperlinkRefId(text2);
						}
						writer.WriteStartElement("a:hlinkClick");
						writer.WriteAttributeString("xmlns:a", "http://schemas.openxmlformats.org/drawingml/2006/main");
						writer.WriteAttributeString("r:id", $"hId{num2}");
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
					writer.WriteStartElement("wp:cNvGraphicFramePr");
					writer.WriteStartElement("a:graphicFrameLocks");
					writer.WriteAttributeString("xmlns:a", "http://schemas.openxmlformats.org/drawingml/2006/main");
					writer.WriteAttributeString("noChangeAspect", "1");
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteStartElement("a:graphic");
					writer.WriteAttributeString("xmlns:a", "http://schemas.openxmlformats.org/drawingml/2006/main");
					writer.WriteStartElement("a:graphicData");
					writer.WriteAttributeString("uri", "http://schemas.openxmlformats.org/drawingml/2006/picture");
					writer.WriteStartElement("pic:pic");
					writer.WriteAttributeString("xmlns:pic", "http://schemas.openxmlformats.org/drawingml/2006/picture");
					writer.WriteStartElement("pic:nvPicPr");
					writer.WriteStartElement("pic:cNvPr");
					writer.WriteAttributeString("id", "0");
					writer.WriteAttributeString("name", $"Image{num12 + 1:D5}.{imageExt}");
					writer.WriteEndElement();
					writer.WriteStartElement("pic:cNvPicPr");
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteStartElement("pic:blipFill");
					writer.WriteStartElement("a:blip");
					writer.WriteAttributeString("r:embed", $"rId{5 + num12}");
					writer.WriteAttributeString("cstate", "print");
					writer.WriteEndElement();
					writer.WriteStartElement("a:stretch");
					writer.WriteStartElement("a:fillRect");
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteStartElement("pic:spPr");
					writer.WriteStartElement("a:xfrm");
					writer.WriteStartElement("a:off");
					writer.WriteAttributeString("x", "0");
					writer.WriteAttributeString("y", "0");
					writer.WriteEndElement();
					writer.WriteStartElement("a:ext");
					writer.WriteAttributeString("cx", $"{ConvertTwipsToEmu(num16)}");
					writer.WriteAttributeString("cy", $"{ConvertTwipsToEmu(num15)}");
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteStartElement("a:prstGeom");
					writer.WriteAttributeString("prst", "rect");
					writer.WriteStartElement("a:avLst");
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteEndElement();
					if (num != -1)
					{
						writer.WriteStartElement("w:bookmarkEnd");
						writer.WriteAttributeString("w:id", num.ToString(CultureInfo.InvariantCulture));
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
					return;
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			if (num != -1)
			{
				writer.WriteStartElement("w:bookmarkStart");
				writer.WriteAttributeString("w:id", num.ToString(CultureInfo.InvariantCulture));
				writer.WriteAttributeString("w:name", value);
				writer.WriteEndElement();
				writer.WriteStartElement("w:bookmarkEnd");
				writer.WriteAttributeString("w:id", num.ToString(CultureInfo.InvariantCulture));
				writer.WriteEndElement();
			}
		}
	}

	private byte[] ConvertRichTextToImageInFullTrust(StiRichText rich)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		MemoryStream memoryStream = new MemoryStream();
		Image val = null;
		Bitmap val2 = new Bitmap(1, 1);
		try
		{
			Graphics val3 = Graphics.FromImage((Image)(object)val2);
			try
			{
				IntPtr hdc = val3.GetHdc();
				val = (Image)new Metafile((Stream)memoryStream, hdc);
				val3.ReleaseHdc(hdc);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		Graphics val4 = Graphics.FromImage(val);
		try
		{
			val4.DrawImage(rich.Image, 0, 0);
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
		memoryStream.Flush();
		return memoryStream.ToArray();
	}

	private void writeTableInfo(XmlTextWriter writer, int[] wordCoordX, int maxCoordX)
	{
		writer.WriteStartElement("w:tblPr");
		writer.WriteStartElement("w:tblStyle");
		writer.WriteAttributeString("w:val", "a1");
		writer.WriteEndElement();
		writer.WriteStartElement("w:tblW");
		writer.WriteAttributeString("w:w", "0");
		writer.WriteAttributeString("w:type", "dxa");
		writer.WriteEndElement();
		writer.WriteStartElement("w:tblLayout");
		writer.WriteAttributeString("w:type", "fixed");
		writer.WriteEndElement();
		writer.WriteStartElement("w:tblCellMar");
		writer.WriteStartElement("w:top");
		writer.WriteAttributeString("w:w", "0");
		writer.WriteAttributeString("w:type", "dxa");
		writer.WriteEndElement();
		writer.WriteStartElement("w:left");
		writer.WriteAttributeString("w:w", "0");
		writer.WriteAttributeString("w:type", "dxa");
		writer.WriteEndElement();
		writer.WriteStartElement("w:bottom");
		writer.WriteAttributeString("w:w", "0");
		writer.WriteAttributeString("w:type", "dxa");
		writer.WriteEndElement();
		writer.WriteStartElement("w:right");
		writer.WriteAttributeString("w:w", "0");
		writer.WriteAttributeString("w:type", "dxa");
		writer.WriteEndElement();
		writer.WriteEndElement();
		writer.WriteStartElement("w:tblLook");
		writer.WriteAttributeString("w:val", "04A0");
		writer.WriteEndElement();
		writer.WriteEndElement();
		writer.WriteStartElement("w:tblGrid");
		for (int i = 1; i < maxCoordX; i++)
		{
			double num = wordCoordX[i] - wordCoordX[i - 1];
			writer.WriteStartElement("w:gridCol");
			writer.WriteAttributeString("w:w", $"{num}");
			writer.WriteEndElement();
		}
		writer.WriteFullEndElement();
	}

	private void writeHtmlTags(XmlTextWriter writer, StiCell cell)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		StiText stiText = cell.Component as StiText;
		string text = cell.Text;
		StiHtmlTagsState val = default(StiHtmlTagsState);
		((StiHtmlTagsState)(ref val))._002Ector(stiText.Font.Bold, stiText.Font.Italic, stiText.Font.Underline, stiText.Font.Strikeout, stiText.Font.SizeInPoints, stiText.Font.Name, StiBrush.ToColor(stiText.TextBrush), StiBrush.ToColor(stiText.Brush), false, false, 0.0, 0.0, (double)stiText.LineSpacing, stiText.HorAlignment);
		StiHtmlState val2 = default(StiHtmlState);
		((StiHtmlState)(ref val2))._002Ector(val, 0);
		List<StiHtmlState> list = StiTextRenderer.ParseHtmlToStates(text, val2, false);
		int styleFromComponent = GetStyleFromComponent(cell.Component);
		bool flag = StiBidirectionalConvert.StringContainArabicOrHebrew(text);
		bool isRtl = cell.CellStyle.TextOptions != null && cell.CellStyle.TextOptions.RightToLeft;
		writeParagraphBegin(writer, cell, styleFromComponent, list, 0, isRtl);
		for (int i = 0; i < list.Count; i++)
		{
			StiHtmlState val3 = list[i];
			StiHtmlTagsState tS = val3.TS;
			if (val3.Text.ToString() == "\n" && i < list.Count - 1)
			{
				writer.WriteEndElement();
				writeParagraphBegin(writer, cell, styleFromComponent, list, i + 1, isRtl);
				continue;
			}
			if (!string.IsNullOrWhiteSpace(tS.Href))
			{
				int hyperlinkRefId = GetHyperlinkRefId(StringToUrl(tS.Href));
				writer.WriteStartElement("w:hyperlink");
				writer.WriteAttributeString("r:id", $"hId{hyperlinkRefId}");
			}
			writer.WriteStartElement("w:r");
			writer.WriteStartElement("w:rPr");
			if (tS.Bold)
			{
				writer.WriteStartElement("w:b");
				writer.WriteEndElement();
			}
			if (tS.Italic)
			{
				writer.WriteStartElement("w:i");
				writer.WriteEndElement();
			}
			if (tS.Underline)
			{
				writer.WriteStartElement("w:u");
				writer.WriteAttributeString("w:val", "single");
				writer.WriteEndElement();
			}
			if (tS.Strikeout)
			{
				writer.WriteStartElement("w:strike");
				writer.WriteEndElement();
			}
			if (tS.Superscript)
			{
				writer.WriteStartElement("w:vertAlign");
				writer.WriteAttributeString("w:val", "superscript");
				writer.WriteEndElement();
			}
			if (tS.Subscript)
			{
				writer.WriteStartElement("w:vertAlign");
				writer.WriteAttributeString("w:val", "subscript");
				writer.WriteEndElement();
			}
			writer.WriteStartElement("w:color");
			writer.WriteAttributeString("w:val", GetColorString(tS.FontColor));
			writer.WriteEndElement();
			if (tS.BackColor.ToArgb() != val.BackColor.ToArgb())
			{
				writer.WriteStartElement("w:shd");
				writer.WriteAttributeString("w:val", "clear");
				writer.WriteAttributeString("w:color", "auto");
				writer.WriteAttributeString("w:fill", GetColorString(tS.BackColor));
				writer.WriteEndElement();
			}
			writer.WriteStartElement("w:sz");
			writer.WriteAttributeString("w:val", $"{tS.FontSize * 2f}");
			writer.WriteEndElement();
			writer.WriteStartElement("w:szCs");
			writer.WriteAttributeString("w:val", $"{tS.FontSize * 2f}");
			writer.WriteEndElement();
			writer.WriteStartElement("w:rFonts");
			string fontName = tS.FontName;
			fontList[fontName] = fontName;
			writer.WriteAttributeString("w:ascii", fontName);
			writer.WriteAttributeString("w:hAnsi", fontName);
			writer.WriteAttributeString("w:eastAsia", fontName);
			writer.WriteAttributeString("w:cs", fontName);
			writer.WriteEndElement();
			double num = tS.LetterSpacing * (double)tS.FontSize * 18.8 + (double)StiOptions.Export.Word.SpaceBetweenCharacters;
			if (num != 0.0)
			{
				writer.WriteStartElement("w:spacing");
				writer.WriteAttributeString("w:val", $"{num}");
				writer.WriteEndElement();
			}
			if (flag)
			{
				writer.WriteStartElement("w:rtl");
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
			string text2 = StiTextRenderer.PrepareStateText(val3.Text).ToString();
			StringBuilder stringBuilder = new StringBuilder();
			string text3 = text2;
			foreach (char c in text3)
			{
				if (!char.IsControl(c) || c == '\t')
				{
					stringBuilder.Append(c);
				}
			}
			writer.WriteStartElement("w:t");
			writer.WriteAttributeString("xml:space", "preserve");
			writer.WriteString(stringBuilder.ToString());
			writer.WriteEndElement();
			writer.WriteEndElement();
			if (!string.IsNullOrWhiteSpace(tS.Href))
			{
				writer.WriteEndElement();
			}
		}
		writer.WriteEndElement();
	}

	private void writeParagraphBegin(XmlTextWriter writer, StiCell cell, int styleIndex, List<StiHtmlState> states, int stateIndex, bool isRtl)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Invalid comparison between Unknown and I4
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Invalid comparison between Unknown and I4
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Invalid comparison between Unknown and I4
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Invalid comparison between Unknown and I4
		writer.WriteStartElement("w:p");
		writer.WriteStartElement("w:pPr");
		if (styleIndex != -1)
		{
			writer.WriteStartElement("w:pStyle");
			writer.WriteAttributeString("w:val", $"Style{styleIndex}");
			writer.WriteEndElement();
		}
		else
		{
			writeRunProperties(writer, cell);
		}
		StiHtmlState val = states[stateIndex];
		if (val.Text.ToString() != "\n")
		{
			for (int i = stateIndex + 1; i < states.Count && !(states[i].Text.ToString() == "\n") && (!(states[i].Text.ToString() == " ") || i != states.Count - 1 || states[i].TS.LineHeight == states[i - 1].TS.LineHeight); i++)
			{
				val = states[i];
			}
		}
		double num = StiOptions.Export.Word.LineSpacing * val.TS.LineHeight;
		if (val.TS.LineHeight < 1.0)
		{
			num *= 1.0 - (1.0 - val.TS.LineHeight) * 0.2;
		}
		if (fontsToCorrectHeight.ContainsKey(val.TS.FontName))
		{
			num *= 0.772;
		}
		if (num != 1.0)
		{
			writer.WriteStartElement("w:spacing");
			writer.WriteAttributeString("w:line", ((int)(num * 240.0)).ToString());
			writer.WriteEndElement();
		}
		string text = null;
		if ((int)val.TS.TextAlign == 1)
		{
			text = "center";
		}
		if (((int)val.TS.TextAlign == 2 && !isRtl) || ((int)val.TS.TextAlign == 0 && isRtl))
		{
			text = "right";
		}
		if ((int)val.TS.TextAlign == 3)
		{
			text = "both";
		}
		if (text != null)
		{
			writer.WriteStartElement("w:jc");
			writer.WriteAttributeString("w:val", text);
			writer.WriteEndElement();
		}
		writer.WriteEndElement();
	}

	private void writeRunProperties(XmlTextWriter writer, StiCell cell)
	{
		writer.WriteStartElement("w:rPr");
		writer.WriteStartElement("w:rFonts");
		string name = cell.CellStyle.Font.Name;
		fontList[name] = name;
		writer.WriteAttributeString("w:ascii", name);
		writer.WriteAttributeString("w:hAnsi", name);
		writer.WriteAttributeString("w:eastAsia", name);
		writer.WriteAttributeString("w:cs", name);
		writer.WriteEndElement();
		if (cell.CellStyle.Font.Bold)
		{
			writer.WriteStartElement("w:b");
			writer.WriteEndElement();
		}
		if (cell.CellStyle.Font.Italic)
		{
			writer.WriteStartElement("w:i");
			writer.WriteEndElement();
		}
		if (cell.CellStyle.Font.Underline)
		{
			writer.WriteStartElement("w:u");
			writer.WriteAttributeString("w:val", "single");
			writer.WriteEndElement();
		}
		writer.WriteStartElement("w:color");
		writer.WriteAttributeString("w:val", GetColorString(cell.CellStyle.TextColor));
		writer.WriteEndElement();
		writer.WriteStartElement("w:sz");
		writer.WriteAttributeString("w:val", $"{cell.CellStyle.Font.SizeInPoints * 2f}");
		writer.WriteEndElement();
		if (StiOptions.Export.Word.SpaceBetweenCharacters != 0)
		{
			writer.WriteStartElement("w:spacing");
			writer.WriteAttributeString("w:val", $"{StiOptions.Export.Word.SpaceBetweenCharacters}");
			writer.WriteEndElement();
		}
		if (StiBidirectionalConvert.StringContainArabicOrHebrew(cell.Text))
		{
			if (cell.CellStyle.Font.Bold)
			{
				writer.WriteStartElement("w:bCs");
				writer.WriteEndElement();
			}
			if (cell.CellStyle.Font.Italic)
			{
				writer.WriteStartElement("w:iCs");
				writer.WriteEndElement();
			}
			writer.WriteStartElement("w:szCs");
			writer.WriteAttributeString("w:val", $"{cell.CellStyle.Font.SizeInPoints * 2f}");
			writer.WriteEndElement();
			writer.WriteStartElement("w:rtl");
			writer.WriteEndElement();
		}
		writer.WriteEndElement();
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
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
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
			stringBuilder.Append("\\t" + GetLineStyle2TableGetValues(matrix.BordersX[rowIndex - 1, columnIndex - 1]));
			val = (StiBorderSides)(val | 1);
		}
		if (matrix.BordersX[rowIndex - 1 + 1, columnIndex - 1] != (StiBorderSide)null)
		{
			stringBuilder.Append("\\b" + GetLineStyle2TableGetValues(matrix.BordersX[rowIndex - 1 + 1, columnIndex - 1]));
			val = (StiBorderSides)(val | 8);
		}
		styles = stringBuilder.ToString();
		return val;
	}

	private string GetLineStyle2TableGetValues(StiBorderSide border)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(GetLineStyle(border.Style));
		stringBuilder.Append((int)(border.Size * 15.0));
		stringBuilder.Append($"c{GetColorString(border.Color)}");
		return stringBuilder.ToString();
	}

	private MemoryStream WriteDocument(StiPagesCollection pages, StiReport report, bool hasDividedPages)
	{
		StiCachedStream stiCachedStream = new StiCachedStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(stiCachedStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("w:document");
		xmlTextWriter.WriteAttributeString("xmlns:ve", "http://schemas.openxmlformats.org/markup-compatibility/2006");
		xmlTextWriter.WriteAttributeString("xmlns:o", "urn:schemas-microsoft-com:office:office");
		xmlTextWriter.WriteAttributeString("xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
		xmlTextWriter.WriteAttributeString("xmlns:m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
		xmlTextWriter.WriteAttributeString("xmlns:v", "urn:schemas-microsoft-com:vml");
		xmlTextWriter.WriteAttributeString("xmlns:wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing");
		xmlTextWriter.WriteAttributeString("xmlns:w10", "urn:schemas-microsoft-com:office:word");
		xmlTextWriter.WriteAttributeString("xmlns:w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
		xmlTextWriter.WriteAttributeString("xmlns:wne", "http://schemas.microsoft.com/office/word/2006/wordml");
		Color pagesBackColor = GetPagesBackColor(pages);
		if (!pagesBackColor.Equals((object?)Color.White) && pagesBackColor.A != 0)
		{
			xmlTextWriter.WriteStartElement("w:background");
			xmlTextWriter.WriteAttributeString("w:color", GetColorString(pagesBackColor));
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteStartElement("w:body");
		StiPagesCollection stiPagesCollection = pages;
		pages = null;
		int num = 0;
		int num2 = 0;
		while (num < stiPagesCollection.Count)
		{
			pages = new StiPagesCollection(report, stiPagesCollection);
			pages.CacheMode = report.RenderedPages.CacheMode;
			pages.AddV2Internal(stiPagesCollection.GetPageWithoutCache(num));
			while (num < stiPagesCollection.Count - 1 && ComparePages(stiPagesCollection.GetPageWithoutCache(num + 1), stiPagesCollection.GetPageWithoutCache(num)))
			{
				num++;
				pages.AddV2Internal(stiPagesCollection.GetPageWithoutCache(num));
			}
			StiPage stiPage = pages[0];
			if (stiPage != null)
			{
				pages.GetPage(stiPage);
				if (usePageHeadersAndFooters)
				{
					StiPagesCollection stiPagesCollection2 = new StiPagesCollection(report, report.RenderedPages);
					stiPagesCollection2.CacheMode = report.RenderedPages.CacheMode;
					stiPagesCollection2.AddV2Internal(pages[0]);
					matrix = new StiMatrix(stiPagesCollection2, StiOptions.Export.Word.DivideBigCells, this);
					if (base.IsStopped)
					{
						return null;
					}
					matrix.ScanComponentsPlacement(optimize: false);
					int i = 0;
					int num3 = 0;
					bool needHeader = false;
					num3 = matrix.CoordY.Count - 1 - 1;
					while (num3 > 0 && matrix.LinePlacement[num3] != StiMatrix.StiTableLineInfo.PageHeader)
					{
						num3--;
					}
					if (num3 >= 0 && matrix.LinePlacement[num3] == StiMatrix.StiTableLineInfo.PageHeader)
					{
						for (i = 0; matrix.LinePlacement[i] != StiMatrix.StiTableLineInfo.PageHeader; i++)
						{
						}
						needHeader = true;
					}
					headersData.Add(WriteHeader(i, num3, needHeader));
					if (imageCache.ImagePackedStore.Count > 0)
					{
						headersRels.Add(WriteHeaderFooterRels());
					}
					else
					{
						headersRels.Add(null);
					}
					bool needFooter = false;
					for (i = 0; matrix.LinePlacement[i] != StiMatrix.StiTableLineInfo.PageFooter && i < matrix.CoordY.Count - 1 - 1; i++)
					{
					}
					if (matrix.LinePlacement[i] == StiMatrix.StiTableLineInfo.PageFooter)
					{
						num3 = matrix.CoordY.Count - 1 - 1;
						while (matrix.LinePlacement[num3] != StiMatrix.StiTableLineInfo.PageFooter)
						{
							num3--;
						}
						needFooter = true;
					}
					footersData.Add(WriteFooter(i, num3, needFooter));
					if (imageCache.ImagePackedStore.Count > 0)
					{
						footersRels.Add(WriteHeaderFooterRels());
					}
					else
					{
						footersRels.Add(null);
					}
					matrix = new StiMatrix(pages, StiOptions.Export.Word.DivideBigCells, this);
					matrix.ScanComponentsPlacement(optimize: true);
					num3 = matrix.CoordY.Count - 1 - 1;
					WriteFromMatrix(xmlTextWriter, 0, num3, outHeadersAndFooters: false);
				}
				else
				{
					matrix = new StiMatrix(pages, StiOptions.Export.Word.DivideBigCells, this, null, StiDataExportMode.AllBands, hasDividedPages);
					if (base.IsStopped)
					{
						return null;
					}
					WriteFromMatrix(xmlTextWriter, 0, matrix.CoordY.Count - 1 - 1, outHeadersAndFooters: true);
				}
				if (num < stiPagesCollection.Count - 1)
				{
					xmlTextWriter.WriteStartElement("w:p");
					xmlTextWriter.WriteStartElement("w:pPr");
					WritePageInfo(xmlTextWriter, pages[0], num2);
					xmlTextWriter.WriteEndElement();
					xmlTextWriter.WriteEndElement();
				}
			}
			num++;
			num2++;
		}
		if (pages.Count > 0)
		{
			WritePageInfo(xmlTextWriter, pages[0], num2 - 1);
		}
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return stiCachedStream;
	}

	private Color GetPagesBackColor(StiPagesCollection pages)
	{
		Color result = Color.Transparent;
		if (pages.Count > 0)
		{
			StiPage pageWithoutCache = pages.GetPageWithoutCache(0);
			result = StiBrush.ToColor(pageWithoutCache.Brush);
			for (int i = 1; i < pages.Count; i++)
			{
				StiPage pageWithoutCache2 = pages.GetPageWithoutCache(i);
				Color color = StiBrush.ToColor(pageWithoutCache2.Brush);
				if (!result.Equals((object?)color))
				{
					result = Color.Transparent;
					break;
				}
			}
		}
		return result;
	}

	private bool ComparePages(StiPage page1, StiPage page2)
	{
		string text = page1.ExcelSheetValue;
		if (string.IsNullOrEmpty(text))
		{
			text = string.Empty;
		}
		string text2 = page2.ExcelSheetValue;
		if (string.IsNullOrEmpty(text2))
		{
			text2 = string.Empty;
		}
		if (text != text2)
		{
			return false;
		}
		if (page1.Orientation != page2.Orientation)
		{
			return false;
		}
		if (!page1.Margins.Equals(page2.Margins))
		{
			return false;
		}
		if (page1.UnlimitedHeight != page2.UnlimitedHeight)
		{
			return false;
		}
		return true;
	}

	private void WritePageInfo(XmlTextWriter writer, StiPage page, int partIndex)
	{
		int num = ConvertHiToTwips(Math.Ceiling(page.Unit.ConvertToHInches(page.PageHeight * (double)(usePageHeadersAndFooters ? 1 : page.SegmentPerHeight))));
		int num2 = ConvertHiToTwips(page.Unit.ConvertToHInches(page.PageWidth * (double)(usePageHeadersAndFooters ? 1 : page.SegmentPerWidth)));
		int num3 = ConvertHiToTwips(page.Unit.ConvertToHInches(page.Margins.Left));
		int num4 = ConvertHiToTwips(page.Unit.ConvertToHInches(page.Margins.Right));
		int num5 = ConvertHiToTwips(page.Unit.ConvertToHInches(page.Margins.Top));
		int num6 = ConvertHiToTwips(page.Unit.ConvertToHInches(page.Margins.Bottom)) - StiOptions.Export.Word.BottomMarginCorrection;
		if (num6 < 0)
		{
			num6 = 0;
		}
		if (num2 > 31500)
		{
			num2 = 31500;
		}
		if (num > 31500)
		{
			num = 31500;
		}
		writer.WriteStartElement("w:sectPr");
		if (usePageHeadersAndFooters)
		{
			writer.WriteStartElement("w:headerReference");
			writer.WriteAttributeString("w:type", "default");
			writer.WriteAttributeString("r:id", $"rIdh{partIndex + 1}");
			writer.WriteEndElement();
			writer.WriteStartElement("w:footerReference");
			writer.WriteAttributeString("w:type", "default");
			writer.WriteAttributeString("r:id", $"rIdf{partIndex + 1}");
			writer.WriteEndElement();
		}
		else
		{
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
			if (flag)
			{
				writer.WriteStartElement("w:headerReference");
				writer.WriteAttributeString("w:type", "default");
				writer.WriteAttributeString("r:id", "rIdh1");
				writer.WriteEndElement();
			}
		}
		writer.WriteStartElement("w:pgSz");
		writer.WriteAttributeString("w:w", $"{num2}");
		writer.WriteAttributeString("w:h", $"{num}");
		if (page.Orientation == StiPageOrientation.Landscape)
		{
			writer.WriteAttributeString("w:orient", "landscape");
		}
		writer.WriteEndElement();
		writer.WriteStartElement("w:pgMar");
		writer.WriteAttributeString("w:top", $"{num5}");
		writer.WriteAttributeString("w:right", $"{num4}");
		writer.WriteAttributeString("w:bottom", $"{num6}");
		writer.WriteAttributeString("w:left", $"{num3}");
		writer.WriteAttributeString("w:header", $"{num5}");
		writer.WriteAttributeString("w:footer", $"{num6}");
		writer.WriteAttributeString("w:gutter", "0");
		writer.WriteEndElement();
		writer.WriteEndElement();
	}

	private void WriteBorders(XmlTextWriter writer, int indexRow, int indexColumn, int height, int width)
	{
		bool flag = true;
		bool flag2 = true;
		for (int i = 0; i < height; i++)
		{
			if (matrix.BordersY[indexRow + i, indexColumn] == (StiBorderSide)null)
			{
				flag = false;
			}
			if (matrix.BordersY[indexRow + i, indexColumn + width] == (StiBorderSide)null)
			{
				flag2 = false;
			}
		}
		bool flag3 = true;
		bool flag4 = true;
		for (int j = 0; j < width; j++)
		{
			if (matrix.BordersX[indexRow, indexColumn + j] == (StiBorderSide)null)
			{
				flag3 = false;
			}
			if (matrix.BordersX[indexRow + height, indexColumn + j] == (StiBorderSide)null)
			{
				flag4 = false;
			}
		}
		if (flag3 || flag || flag4 || flag2)
		{
			writer.WriteStartElement("w:tcBorders");
			if (flag3)
			{
				WriteBorderData(writer, "w:top", Matrix.BordersX[indexRow, indexColumn]);
			}
			if (flag)
			{
				WriteBorderData(writer, "w:left", Matrix.BordersY[indexRow, indexColumn]);
			}
			if (flag4)
			{
				WriteBorderData(writer, "w:bottom", Matrix.BordersX[indexRow + height, indexColumn]);
			}
			if (flag2)
			{
				WriteBorderData(writer, "w:right", Matrix.BordersY[indexRow, indexColumn + width]);
			}
			writer.WriteEndElement();
		}
	}

	private void WriteBorderData(XmlTextWriter writer, string side, StiBorderSide border)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (border != (StiBorderSide)null)
		{
			string lineStyle = GetLineStyle(border.Style);
			if (lineStyle != string.Empty)
			{
				writer.WriteStartElement(side);
				writer.WriteAttributeString("w:val", lineStyle);
				writer.WriteAttributeString("w:sz", $"{(int)(border.Size * 5.0)}");
				writer.WriteAttributeString("w:space", "0");
				writer.WriteAttributeString("w:color", GetColorString(border.Color));
				writer.WriteEndElement();
			}
		}
	}

	private MemoryStream WriteFootNotes()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("w:footnotes");
		xmlTextWriter.WriteAttributeString("xmlns:ve", "http://schemas.openxmlformats.org/markup-compatibility/2006");
		xmlTextWriter.WriteAttributeString("xmlns:o", "urn:schemas-microsoft-com:office:office");
		xmlTextWriter.WriteAttributeString("xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
		xmlTextWriter.WriteAttributeString("xmlns:m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
		xmlTextWriter.WriteAttributeString("xmlns:v", "urn:schemas-microsoft-com:vml");
		xmlTextWriter.WriteAttributeString("xmlns:wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing");
		xmlTextWriter.WriteAttributeString("xmlns:w10", "urn:schemas-microsoft-com:office:word");
		xmlTextWriter.WriteAttributeString("xmlns:w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
		xmlTextWriter.WriteAttributeString("xmlns:wne", "http://schemas.microsoft.com/office/word/2006/wordml");
		xmlTextWriter.WriteStartElement("w:footnote");
		xmlTextWriter.WriteAttributeString("w:type", "separator");
		xmlTextWriter.WriteAttributeString("w:id", "0");
		xmlTextWriter.WriteStartElement("w:p");
		xmlTextWriter.WriteStartElement("w:r");
		xmlTextWriter.WriteStartElement("w:separator");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteStartElement("w:footnote");
		xmlTextWriter.WriteAttributeString("w:type", "continuationSeparator");
		xmlTextWriter.WriteAttributeString("w:id", "1");
		xmlTextWriter.WriteStartElement("w:p");
		xmlTextWriter.WriteStartElement("w:r");
		xmlTextWriter.WriteStartElement("w:continuationSeparator");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteEndNotes()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("w:endnotes");
		xmlTextWriter.WriteAttributeString("xmlns:ve", "http://schemas.openxmlformats.org/markup-compatibility/2006");
		xmlTextWriter.WriteAttributeString("xmlns:o", "urn:schemas-microsoft-com:office:office");
		xmlTextWriter.WriteAttributeString("xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
		xmlTextWriter.WriteAttributeString("xmlns:m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
		xmlTextWriter.WriteAttributeString("xmlns:v", "urn:schemas-microsoft-com:vml");
		xmlTextWriter.WriteAttributeString("xmlns:wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing");
		xmlTextWriter.WriteAttributeString("xmlns:w10", "urn:schemas-microsoft-com:office:word");
		xmlTextWriter.WriteAttributeString("xmlns:w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
		xmlTextWriter.WriteAttributeString("xmlns:wne", "http://schemas.microsoft.com/office/word/2006/wordml");
		xmlTextWriter.WriteStartElement("w:endnote");
		xmlTextWriter.WriteAttributeString("w:type", "separator");
		xmlTextWriter.WriteAttributeString("w:id", "0");
		xmlTextWriter.WriteStartElement("w:p");
		xmlTextWriter.WriteStartElement("w:r");
		xmlTextWriter.WriteStartElement("w:separator");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteStartElement("w:endnote");
		xmlTextWriter.WriteAttributeString("w:type", "continuationSeparator");
		xmlTextWriter.WriteAttributeString("w:id", "1");
		xmlTextWriter.WriteStartElement("w:p");
		xmlTextWriter.WriteStartElement("w:r");
		xmlTextWriter.WriteStartElement("w:continuationSeparator");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteHeader(int startLine, int endLine, bool needHeader)
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("w:hdr");
		xmlTextWriter.WriteAttributeString("xmlns:ve", "http://schemas.openxmlformats.org/markup-compatibility/2006");
		xmlTextWriter.WriteAttributeString("xmlns:o", "urn:schemas-microsoft-com:office:office");
		xmlTextWriter.WriteAttributeString("xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
		xmlTextWriter.WriteAttributeString("xmlns:m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
		xmlTextWriter.WriteAttributeString("xmlns:v", "urn:schemas-microsoft-com:vml");
		xmlTextWriter.WriteAttributeString("xmlns:wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing");
		xmlTextWriter.WriteAttributeString("xmlns:w10", "urn:schemas-microsoft-com:office:word");
		xmlTextWriter.WriteAttributeString("xmlns:w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
		xmlTextWriter.WriteAttributeString("xmlns:wne", "http://schemas.microsoft.com/office/word/2006/wordml");
		if (needHeader)
		{
			WriteFromMatrix(xmlTextWriter, startLine, endLine, outHeadersAndFooters: true);
		}
		else
		{
			xmlTextWriter.WriteStartElement("w:p");
			xmlTextWriter.WriteStartElement("w:r");
			xmlTextWriter.WriteStartElement("w:cr");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteFooter(int startLine, int endLine, bool needFooter)
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("w:ftr");
		xmlTextWriter.WriteAttributeString("xmlns:ve", "http://schemas.openxmlformats.org/markup-compatibility/2006");
		xmlTextWriter.WriteAttributeString("xmlns:o", "urn:schemas-microsoft-com:office:office");
		xmlTextWriter.WriteAttributeString("xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
		xmlTextWriter.WriteAttributeString("xmlns:m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
		xmlTextWriter.WriteAttributeString("xmlns:v", "urn:schemas-microsoft-com:vml");
		xmlTextWriter.WriteAttributeString("xmlns:wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing");
		xmlTextWriter.WriteAttributeString("xmlns:w10", "urn:schemas-microsoft-com:office:word");
		xmlTextWriter.WriteAttributeString("xmlns:w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
		xmlTextWriter.WriteAttributeString("xmlns:wne", "http://schemas.microsoft.com/office/word/2006/wordml");
		if (needFooter)
		{
			WriteFromMatrix(xmlTextWriter, startLine, endLine, outHeadersAndFooters: true);
		}
		else
		{
			xmlTextWriter.WriteStartElement("w:p");
			xmlTextWriter.WriteStartElement("w:r");
			xmlTextWriter.WriteStartElement("w:cr");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
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
		xmlTextWriter.WriteAttributeString("Extension", "emf");
		xmlTextWriter.WriteAttributeString("ContentType", "image/x-emf");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Default");
		xmlTextWriter.WriteAttributeString("Extension", "rtf");
		xmlTextWriter.WriteAttributeString("ContentType", "application/rtf");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Default");
		xmlTextWriter.WriteAttributeString("Extension", "png");
		xmlTextWriter.WriteAttributeString("ContentType", "image/png");
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
		xmlTextWriter.WriteAttributeString("PartName", "/word/document.xml");
		xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Override");
		xmlTextWriter.WriteAttributeString("PartName", "/word/styles.xml");
		xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.wordprocessingml.styles+xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Override");
		xmlTextWriter.WriteAttributeString("PartName", "/word/settings.xml");
		xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.wordprocessingml.settings+xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Override");
		xmlTextWriter.WriteAttributeString("PartName", "/word/webSettings.xml");
		xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.wordprocessingml.webSettings+xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Override");
		xmlTextWriter.WriteAttributeString("PartName", "/word/fontTable.xml");
		xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.wordprocessingml.fontTable+xml");
		xmlTextWriter.WriteEndElement();
		if (usePageHeadersAndFooters)
		{
			xmlTextWriter.WriteStartElement("Override");
			xmlTextWriter.WriteAttributeString("PartName", "/word/footnotes.xml");
			xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.wordprocessingml.footnotes+xml");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("Override");
			xmlTextWriter.WriteAttributeString("PartName", "/word/endnotes.xml");
			xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.wordprocessingml.endnotes+xml");
			xmlTextWriter.WriteEndElement();
			for (int i = 0; i < headersData.Count; i++)
			{
				xmlTextWriter.WriteStartElement("Override");
				xmlTextWriter.WriteAttributeString("PartName", $"/word/footer{i + 1}.xml");
				xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.wordprocessingml.footer+xml");
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("Override");
				xmlTextWriter.WriteAttributeString("PartName", $"/word/header{i + 1}.xml");
				xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.wordprocessingml.header+xml");
				xmlTextWriter.WriteEndElement();
			}
		}
		else
		{
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
			if (flag)
			{
				xmlTextWriter.WriteStartElement("Override");
				xmlTextWriter.WriteAttributeString("PartName", "/word/footnotes.xml");
				xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.wordprocessingml.footnotes+xml");
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("Override");
				xmlTextWriter.WriteAttributeString("PartName", "/word/endnotes.xml");
				xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.wordprocessingml.endnotes+xml");
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("Override");
				xmlTextWriter.WriteAttributeString("PartName", "/word/headerAdditional.xml");
				xmlTextWriter.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.wordprocessingml.header+xml");
				xmlTextWriter.WriteEndElement();
			}
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
		xmlTextWriter.WriteAttributeString("Target", "word/document.xml");
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
		xmlTextWriter.WriteElementString("Application", "Microsoft Office Word");
		xmlTextWriter.WriteElementString("Company", (docCompanyString == null) ? "" : docCompanyString);
		xmlTextWriter.WriteElementString("Template", "Normal.dotm");
		xmlTextWriter.WriteElementString("TotalTime", "0");
		xmlTextWriter.WriteElementString("Pages", "1");
		xmlTextWriter.WriteElementString("Words", "1");
		xmlTextWriter.WriteElementString("Characters", "1");
		xmlTextWriter.WriteElementString("DocSecurity", (restrictEditing == StiWord2007RestrictEditing.No) ? "0" : "8");
		xmlTextWriter.WriteElementString("Lines", "1");
		xmlTextWriter.WriteElementString("Paragraphs", "1");
		xmlTextWriter.WriteElementString("ScaleCrop", "false");
		xmlTextWriter.WriteElementString("LinksUpToDate", "false");
		xmlTextWriter.WriteElementString("CharactersWithSpaces", "1");
		xmlTextWriter.WriteElementString("SharedDoc", "false");
		xmlTextWriter.WriteElementString("HyperlinksChanged", "false");
		xmlTextWriter.WriteElementString("AppVersion", "12.0000");
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
		xmlTextWriter.WriteElementString("dc:title", (report.ReportName == null) ? "" : report.ReportName);
		xmlTextWriter.WriteElementString("dc:subject", (report.ReportAlias == null) ? "" : report.ReportAlias);
		xmlTextWriter.WriteElementString("dc:creator", (report.ReportAuthor == null) ? "" : report.ReportAuthor);
		xmlTextWriter.WriteElementString("cp:keywords", "");
		xmlTextWriter.WriteElementString("dc:description", (report.ReportDescription == null) ? "" : report.ReportDescription);
		xmlTextWriter.WriteElementString("cp:lastModifiedBy", (docLastModifiedString == null) ? "" : docLastModifiedString);
		xmlTextWriter.WriteElementString("cp:revision", "1");
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

	private MemoryStream WriteSettings(StiPagesCollection pages)
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("w:settings");
		xmlTextWriter.WriteAttributeString("xmlns:o", "urn:schemas-microsoft-com:office:office");
		xmlTextWriter.WriteAttributeString("xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
		xmlTextWriter.WriteAttributeString("xmlns:m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
		xmlTextWriter.WriteAttributeString("xmlns:v", "urn:schemas-microsoft-com:vml");
		xmlTextWriter.WriteAttributeString("xmlns:w10", "urn:schemas-microsoft-com:office:word");
		xmlTextWriter.WriteAttributeString("xmlns:w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
		xmlTextWriter.WriteAttributeString("xmlns:sl", "http://schemas.openxmlformats.org/schemaLibrary/2006/main");
		if (restrictEditing != StiWord2007RestrictEditing.No)
		{
			xmlTextWriter.WriteStartElement("w:documentProtection");
			xmlTextWriter.WriteAttributeString("w:edit", "readOnly");
			xmlTextWriter.WriteAttributeString("w:enforcement", "1");
			xmlTextWriter.WriteAttributeString("w:cryptProviderType", "rsaFull");
			xmlTextWriter.WriteAttributeString("w:cryptAlgorithmClass", "hash");
			xmlTextWriter.WriteAttributeString("w:cryptAlgorithmType", "typeAny");
			xmlTextWriter.WriteAttributeString("w:cryptAlgorithmSid", "4");
			xmlTextWriter.WriteAttributeString("w:cryptSpinCount", "50000");
			xmlTextWriter.WriteAttributeString("w:hash", "aw5VYrxGrQVOl7/SJDI9GvbcwaE=");
			xmlTextWriter.WriteAttributeString("w:salt", "974T1u8C/8p0OAB+3ev3nQ==");
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteStartElement("w:zoom");
		xmlTextWriter.WriteAttributeString("w:percent", "100");
		xmlTextWriter.WriteEndElement();
		Color pagesBackColor = GetPagesBackColor(pages);
		if (!pagesBackColor.Equals((object?)Color.White) && pagesBackColor.A != 0)
		{
			xmlTextWriter.WriteStartElement("w:displayBackgroundShape");
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteStartElement("w:proofState");
		xmlTextWriter.WriteAttributeString("w:spelling", "clean");
		xmlTextWriter.WriteAttributeString("w:grammar", "clean");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:defaultTabStop");
		xmlTextWriter.WriteAttributeString("w:val", "708");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:characterSpacingControl");
		xmlTextWriter.WriteAttributeString("w:val", "doNotCompress");
		xmlTextWriter.WriteEndElement();
		if (usePageHeadersAndFooters)
		{
			xmlTextWriter.WriteStartElement("w:footnotePr");
			xmlTextWriter.WriteStartElement("w:footnote");
			xmlTextWriter.WriteAttributeString("w:id", "0");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("w:footnote");
			xmlTextWriter.WriteAttributeString("w:id", "1");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("w:endnotePr");
			xmlTextWriter.WriteStartElement("w:endnote");
			xmlTextWriter.WriteAttributeString("w:id", "0");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("w:endnote");
			xmlTextWriter.WriteAttributeString("w:id", "1");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteStartElement("w:compat");
		if (StiOptions.Export.Word.CompatibilityModeValue > 12)
		{
			xmlTextWriter.WriteStartElement("w:compatSetting");
			xmlTextWriter.WriteAttributeString("w:name", "compatibilityMode");
			xmlTextWriter.WriteAttributeString("w:uri", "http://schemas.microsoft.com/office/word");
			xmlTextWriter.WriteAttributeString("w:val", StiOptions.Export.Word.CompatibilityModeValue.ToString());
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("m:mathPr");
		xmlTextWriter.WriteStartElement("m:mathFont");
		xmlTextWriter.WriteAttributeString("m:val", "Cambria Math");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("m:brkBin");
		xmlTextWriter.WriteAttributeString("m:val", "before");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("m:brkBinSub");
		xmlTextWriter.WriteAttributeString("m:val", "--");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("m:smallFrac");
		xmlTextWriter.WriteAttributeString("m:val", "off");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("m:dispDef");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("m:lMargin");
		xmlTextWriter.WriteAttributeString("m:val", "0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("m:rMargin");
		xmlTextWriter.WriteAttributeString("m:val", "0");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("m:defJc");
		xmlTextWriter.WriteAttributeString("m:val", "centerGroup");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("m:wrapIndent");
		xmlTextWriter.WriteAttributeString("m:val", "1440");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("m:intLim");
		xmlTextWriter.WriteAttributeString("m:val", "subSup");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("m:naryLim");
		xmlTextWriter.WriteAttributeString("m:val", "undOvr");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:themeFontLang");
		xmlTextWriter.WriteAttributeString("w:val", "en-EN");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:clrSchemeMapping");
		xmlTextWriter.WriteAttributeString("w:bg1", "light1");
		xmlTextWriter.WriteAttributeString("w:t1", "dark1");
		xmlTextWriter.WriteAttributeString("w:bg2", "light2");
		xmlTextWriter.WriteAttributeString("w:t2", "dark2");
		xmlTextWriter.WriteAttributeString("w:accent1", "accent1");
		xmlTextWriter.WriteAttributeString("w:accent2", "accent2");
		xmlTextWriter.WriteAttributeString("w:accent3", "accent3");
		xmlTextWriter.WriteAttributeString("w:accent4", "accent4");
		xmlTextWriter.WriteAttributeString("w:accent5", "accent5");
		xmlTextWriter.WriteAttributeString("w:accent6", "accent6");
		xmlTextWriter.WriteAttributeString("w:hyperlink", "hyperlink");
		xmlTextWriter.WriteAttributeString("w:followedHyperlink", "followedHyperlink");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:shapeDefaults");
		xmlTextWriter.WriteStartElement("o:shapedefaults");
		xmlTextWriter.WriteAttributeString("v:ext", "edit");
		xmlTextWriter.WriteAttributeString("spidmax", "2050");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("o:shapelayout");
		xmlTextWriter.WriteAttributeString("v:ext", "edit");
		xmlTextWriter.WriteStartElement("o:idmap");
		xmlTextWriter.WriteAttributeString("v:ext", "edit");
		xmlTextWriter.WriteAttributeString("data", "1");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:decimalSymbol");
		xmlTextWriter.WriteAttributeString("w:val", ",");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:listSeparator");
		xmlTextWriter.WriteAttributeString("w:val", ";");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteWebSettings()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("w:webSettings");
		xmlTextWriter.WriteAttributeString("xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
		xmlTextWriter.WriteAttributeString("xmlns:w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
		xmlTextWriter.WriteStartElement("w:optimizeForBrowser");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteFontTable()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("w:fonts");
		xmlTextWriter.WriteAttributeString("xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
		xmlTextWriter.WriteAttributeString("xmlns:w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
		fontList["Calibri"] = "Calibri";
		fontList["Cambria"] = "Cambria";
		foreach (string value in fontList.Values)
		{
			xmlTextWriter.WriteStartElement("w:font");
			xmlTextWriter.WriteAttributeString("w:name", value);
			xmlTextWriter.WriteFullEndElement();
		}
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteDocumentRels()
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
		xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles");
		xmlTextWriter.WriteAttributeString("Target", "styles.xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Relationship");
		xmlTextWriter.WriteAttributeString("Id", "rId2");
		xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/settings");
		xmlTextWriter.WriteAttributeString("Target", "settings.xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Relationship");
		xmlTextWriter.WriteAttributeString("Id", "rId3");
		xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/webSettings");
		xmlTextWriter.WriteAttributeString("Target", "webSettings.xml");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("Relationship");
		xmlTextWriter.WriteAttributeString("Id", "rId4");
		xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/fontTable");
		xmlTextWriter.WriteAttributeString("Target", "fontTable.xml");
		xmlTextWriter.WriteEndElement();
		for (int i = 0; i < imageCache.ImagePackedStore.Count; i++)
		{
			xmlTextWriter.WriteStartElement("Relationship");
			xmlTextWriter.WriteAttributeString("Id", $"rId{5 + i}");
			xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");
			xmlTextWriter.WriteAttributeString("Target", $"media/image{i + 1:D5}.{GetImageExt(imageCache.ImageFormatStore[i])}");
			xmlTextWriter.WriteEndElement();
		}
		if (usePageHeadersAndFooters)
		{
			xmlTextWriter.WriteStartElement("Relationship");
			xmlTextWriter.WriteAttributeString("Id", $"rId{5 + imageCache.ImagePackedStore.Count}");
			xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/footnotes");
			xmlTextWriter.WriteAttributeString("Target", "footnotes.xml");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("Relationship");
			xmlTextWriter.WriteAttributeString("Id", $"rId{5 + imageCache.ImagePackedStore.Count + 1}");
			xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/endnotes");
			xmlTextWriter.WriteAttributeString("Target", "endnotes.xml");
			xmlTextWriter.WriteEndElement();
			for (int j = 0; j < headersData.Count; j++)
			{
				xmlTextWriter.WriteStartElement("Relationship");
				xmlTextWriter.WriteAttributeString("Id", $"rIdh{j + 1}");
				xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/header");
				xmlTextWriter.WriteAttributeString("Target", $"header{j + 1}.xml");
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("Relationship");
				xmlTextWriter.WriteAttributeString("Id", $"rIdf{j + 1}");
				xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/footer");
				xmlTextWriter.WriteAttributeString("Target", $"footer{j + 1}.xml");
				xmlTextWriter.WriteEndElement();
			}
		}
		else
		{
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
			if (flag)
			{
				xmlTextWriter.WriteStartElement("Relationship");
				xmlTextWriter.WriteAttributeString("Id", $"rId{5 + imageCache.ImagePackedStore.Count}");
				xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/footnotes");
				xmlTextWriter.WriteAttributeString("Target", "footnotes.xml");
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("Relationship");
				xmlTextWriter.WriteAttributeString("Id", $"rId{5 + imageCache.ImagePackedStore.Count + 1}");
				xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/endnotes");
				xmlTextWriter.WriteAttributeString("Target", "endnotes.xml");
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("Relationship");
				xmlTextWriter.WriteAttributeString("Id", "rIdh1");
				xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/header");
				xmlTextWriter.WriteAttributeString("Target", "headerAdditional.xml");
				xmlTextWriter.WriteEndElement();
			}
		}
		if (hyperlinkList.Count > 0)
		{
			foreach (DictionaryEntry hyperlink in hyperlinkList)
			{
				xmlTextWriter.WriteStartElement("Relationship");
				xmlTextWriter.WriteAttributeString("Id", $"hId{(int)hyperlink.Value}");
				xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink");
				xmlTextWriter.WriteAttributeString("Target", (string)hyperlink.Key);
				xmlTextWriter.WriteAttributeString("TargetMode", "External");
				xmlTextWriter.WriteEndElement();
			}
		}
		if (embedsList.Count > 0)
		{
			for (int k = 0; k < embedsList.Count; k++)
			{
				xmlTextWriter.WriteStartElement("Relationship");
				xmlTextWriter.WriteAttributeString("Id", $"reId{1 + k}");
				xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/aFChunk");
				xmlTextWriter.WriteAttributeString("Target", $"embeddings/richtext{k + 1:D5}.rtf");
				xmlTextWriter.WriteAttributeString("TargetMode", "Internal");
				xmlTextWriter.WriteEndElement();
			}
		}
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteHeaderFooterRels()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("Relationships");
		xmlTextWriter.WriteAttributeString("xmlns", "http://schemas.openxmlformats.org/package/2006/relationships");
		for (int i = 0; i < imageCache.ImagePackedStore.Count; i++)
		{
			xmlTextWriter.WriteStartElement("Relationship");
			xmlTextWriter.WriteAttributeString("Id", $"rId{5 + i}");
			xmlTextWriter.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");
			xmlTextWriter.WriteAttributeString("Target", $"media/image{i + 1:D5}.jpeg");
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteStyles()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		xmlTextWriter.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("w:styles");
		xmlTextWriter.WriteAttributeString("xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
		xmlTextWriter.WriteAttributeString("xmlns:w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
		xmlTextWriter.WriteStartElement("w:style");
		xmlTextWriter.WriteAttributeString("w:type", "paragraph");
		xmlTextWriter.WriteAttributeString("w:default", "1");
		xmlTextWriter.WriteAttributeString("w:styleId", "a");
		xmlTextWriter.WriteStartElement("w:name");
		xmlTextWriter.WriteAttributeString("w:val", "Normal");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:qFormat");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:pPr");
		xmlTextWriter.WriteStartElement("w:spacing");
		xmlTextWriter.WriteAttributeString("w:after", "0");
		xmlTextWriter.WriteAttributeString("w:line", "240");
		xmlTextWriter.WriteAttributeString("w:lineRule", "auto");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:rPr");
		xmlTextWriter.WriteStartElement("w:sz");
		xmlTextWriter.WriteAttributeString("w:val", $"{(int)Math.Round(StiOptions.Export.Word.NormalStyleDefaultFontSize * 2f)}");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:style");
		xmlTextWriter.WriteAttributeString("w:type", "character");
		xmlTextWriter.WriteAttributeString("w:default", "1");
		xmlTextWriter.WriteAttributeString("w:styleId", "a0");
		xmlTextWriter.WriteStartElement("w:name");
		xmlTextWriter.WriteAttributeString("w:val", "Default Paragraph Font");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:uiPriority");
		xmlTextWriter.WriteAttributeString("w:val", "1");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:semiHidden");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:unhideWhenUsed");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:style");
		xmlTextWriter.WriteAttributeString("w:type", "table");
		xmlTextWriter.WriteAttributeString("w:default", "1");
		xmlTextWriter.WriteAttributeString("w:styleId", "a1");
		xmlTextWriter.WriteStartElement("w:name");
		xmlTextWriter.WriteAttributeString("w:val", "Normal Table");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:uiPriority");
		xmlTextWriter.WriteAttributeString("w:val", "99");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:semiHidden");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:unhideWhenUsed");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:qFormat");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:tblPr");
		xmlTextWriter.WriteStartElement("w:tblInd");
		xmlTextWriter.WriteAttributeString("w:w", "0");
		xmlTextWriter.WriteAttributeString("w:type", "dxa");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:tblCellMar");
		xmlTextWriter.WriteStartElement("w:top");
		xmlTextWriter.WriteAttributeString("w:w", "0");
		xmlTextWriter.WriteAttributeString("w:type", "dxa");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:left");
		xmlTextWriter.WriteAttributeString("w:w", "0");
		xmlTextWriter.WriteAttributeString("w:type", "dxa");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:bottom");
		xmlTextWriter.WriteAttributeString("w:w", "0");
		xmlTextWriter.WriteAttributeString("w:type", "dxa");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:right");
		xmlTextWriter.WriteAttributeString("w:w", "0");
		xmlTextWriter.WriteAttributeString("w:type", "dxa");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:style");
		xmlTextWriter.WriteAttributeString("w:type", "numbering");
		xmlTextWriter.WriteAttributeString("w:default", "1");
		xmlTextWriter.WriteAttributeString("w:styleId", "a2");
		xmlTextWriter.WriteStartElement("w:name");
		xmlTextWriter.WriteAttributeString("w:val", "No List");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:uiPriority");
		xmlTextWriter.WriteAttributeString("w:val", "99");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:semiHidden");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("w:unhideWhenUsed");
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		for (int i = 0; i < styleList.Count; i++)
		{
			StiWord2007StyleInfo stiWord2007StyleInfo = (StiWord2007StyleInfo)styleList[i];
			xmlTextWriter.WriteStartElement("w:style");
			xmlTextWriter.WriteAttributeString("w:type", "paragraph");
			xmlTextWriter.WriteAttributeString("w:customStyle", "1");
			xmlTextWriter.WriteAttributeString("w:styleId", $"Style{i}");
			xmlTextWriter.WriteStartElement("w:name");
			xmlTextWriter.WriteAttributeString("w:val", stiWord2007StyleInfo.Name);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("w:basedOn");
			xmlTextWriter.WriteAttributeString("w:val", "a");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("w:rPr");
			xmlTextWriter.WriteStartElement("w:rFonts");
			xmlTextWriter.WriteAttributeString("w:ascii", stiWord2007StyleInfo.FontName);
			xmlTextWriter.WriteAttributeString("w:hAnsi", stiWord2007StyleInfo.FontName);
			xmlTextWriter.WriteAttributeString("w:cs", stiWord2007StyleInfo.FontName);
			xmlTextWriter.WriteEndElement();
			if (stiWord2007StyleInfo.Bold)
			{
				xmlTextWriter.WriteStartElement("w:b");
				xmlTextWriter.WriteEndElement();
			}
			if (stiWord2007StyleInfo.Italic)
			{
				xmlTextWriter.WriteStartElement("w:i");
				xmlTextWriter.WriteEndElement();
			}
			if (stiWord2007StyleInfo.Underline)
			{
				xmlTextWriter.WriteStartElement("w:u");
				xmlTextWriter.WriteAttributeString("w:val", "single");
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteStartElement("w:color");
			xmlTextWriter.WriteAttributeString("w:val", GetColorString(stiWord2007StyleInfo.TextColor));
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("w:sz");
			xmlTextWriter.WriteAttributeString("w:val", $"{stiWord2007StyleInfo.FontSize}");
			xmlTextWriter.WriteEndElement();
			if (StiOptions.Export.Word.SpaceBetweenCharacters != 0)
			{
				xmlTextWriter.WriteStartElement("w:spacing");
				xmlTextWriter.WriteAttributeString("w:val", $"{StiOptions.Export.Word.SpaceBetweenCharacters}");
				xmlTextWriter.WriteEndElement();
			}
			if (stiWord2007StyleInfo.RightToLeft)
			{
				if (stiWord2007StyleInfo.Bold)
				{
					xmlTextWriter.WriteStartElement("w:bCs");
					xmlTextWriter.WriteEndElement();
				}
				if (stiWord2007StyleInfo.Italic)
				{
					xmlTextWriter.WriteStartElement("w:iCs");
					xmlTextWriter.WriteEndElement();
				}
				xmlTextWriter.WriteStartElement("w:szCs");
				xmlTextWriter.WriteAttributeString("w:val", $"{stiWord2007StyleInfo.FontSize}");
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("w:rtl");
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteImage(int number)
	{
		MemoryStream memoryStream = new MemoryStream();
		byte[] array = (byte[])imageCache.ImagePackedStore[number];
		memoryStream.Write(array, 0, array.Length);
		return memoryStream;
	}

	private MemoryStream WriteEmbed(int number)
	{
		MemoryStream memoryStream = new MemoryStream();
		StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.GetEncoding(1252));
		string text = (string)embedsList[number];
		string text2 = StiExportUtils.CorrectEncoding(text);
		if (text2 != null)
		{
			text = text2;
		}
		streamWriter.Write(text);
		streamWriter.Flush();
		return memoryStream;
	}

	private MemoryStream WriteAdditionalData(string st, bool base64)
	{
		MemoryStream memoryStream = new MemoryStream();
		byte[] array = (base64 ? System.Convert.FromBase64String(st) : Encoding.ASCII.GetBytes(st));
		memoryStream.Write(array, 0, array.Length);
		return memoryStream;
	}

	public void ExportWord(StiReport report, string fileName)
	{
		FileStream fileStream = null;
		try
		{
			StiFileUtils.ProcessReadOnly(fileName);
			fileStream = new FileStream(fileName, FileMode.Create);
			ExportWord(report, fileStream);
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

	public void ExportWord(StiReport report, Stream stream)
	{
		ExportWord(report, stream, new StiWord2007ExportSettings());
	}

	public void ExportWord(StiReport report, Stream stream, StiPagesRange pageRange)
	{
		StiWord2007ExportSettings settings = new StiWord2007ExportSettings
		{
			PageRange = pageRange
		};
		ExportWord(report, stream, settings);
	}

	public void ExportWord(StiReport report, Stream stream, StiWord2007ExportSettings settings)
	{
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		StiLogService.Write(((object)this).GetType(), "Export report to Word 2007 format");
		string reportVersion = StiExportUtils.GetReportVersion();
		string reportVersion2 = StiExportUtils.GetReportVersion(report);
		if (settings == null)
		{
			throw new ArgumentNullException("The 'settings' argument cannot be equal in null.");
		}
		StiPagesRange pageRange = settings.PageRange;
		usePageHeadersAndFooters = settings.UsePageHeadersAndFooters;
		RemoveEmptySpaceAtBottom = settings.RemoveEmptySpaceAtBottom;
		imageResolution = settings.ImageResolution;
		imageQuality = settings.ImageQuality;
		docCompanyString = ((settings.CompanyString == reportVersion) ? reportVersion2 : settings.CompanyString);
		docLastModifiedString = ((settings.LastModifiedString == reportVersion) ? reportVersion2 : settings.LastModifiedString);
		restrictEditing = settings.RestrictEditing;
		xmlIndentation = -1;
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
		if (StiOptions.Export.Word.LineSpacing != 1.0)
		{
			lineSpace = ((int)Math.Round(StiOptions.Export.Word.LineSpacing * 240.0)).ToString();
			lineSpace2 = ((int)Math.Round(StiOptions.Export.Word.LineSpacing * 0.772 * 240.0)).ToString();
		}
		if (StiOptions.Export.Word.RestrictEditing > restrictEditing)
		{
			restrictEditing = StiOptions.Export.Word.RestrictEditing;
		}
		this.report = report;
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			styleList = new ArrayList();
			fontList = new Hashtable();
			imageCache = new StiImageCache(StiOptions.Export.Word.AllowImageComparer, ImageFormat.Jpeg, imageQuality);
			bookmarkList = new Hashtable();
			hyperlinkList = new Hashtable();
			embedsList = new ArrayList();
			base.CurrentPassNumber = 0;
			base.MaximumPassNumber = 3 + (StiOptions.Export.Word.DivideSegmentPages ? 1 : 0);
			bool hasDividedPages = false;
			StiPagesCollection stiPagesCollection = pageRange.GetSelectedPages(report.RenderedPages);
			if (StiOptions.Export.Word.DivideSegmentPages)
			{
				bool flag = true;
				if (usePageHeadersAndFooters && stiPagesCollection.Count > 0 && stiPagesCollection[0].UnlimitedHeight && stiPagesCollection[0].SegmentPerWidth == 1)
				{
					flag = false;
				}
				if (flag)
				{
					StiPagesCollection stiPagesCollection2 = StiSegmentPagesDivider.Divide(stiPagesCollection, this);
					if (stiPagesCollection != stiPagesCollection2)
					{
						hasDividedPages = true;
					}
					stiPagesCollection = stiPagesCollection2;
					base.CurrentPassNumber++;
				}
			}
			if (base.IsStopped)
			{
				return;
			}
			base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
			StiZipWriter20 val = new StiZipWriter20();
			val.Begin(stream, true);
			if (usePageHeadersAndFooters)
			{
				headersData = new ArrayList();
				headersRels = new ArrayList();
				footersData = new ArrayList();
				footersRels = new ArrayList();
			}
			val.AddFile("word/document.xml", WriteDocument(stiPagesCollection, report, hasDividedPages), true);
			if (base.IsStopped)
			{
				return;
			}
			if (usePageHeadersAndFooters)
			{
				for (int i = 0; i < headersData.Count; i++)
				{
					val.AddFile($"word/header{i + 1}.xml", (MemoryStream)headersData[i]);
					if (headersRels[i] != null)
					{
						val.AddFile($"word/_rels/header{i + 1}.xml.rels", (MemoryStream)headersRels[i]);
					}
					val.AddFile($"word/footer{i + 1}.xml", (MemoryStream)footersData[i]);
					if (footersRels[i] != null)
					{
						val.AddFile($"word/_rels/footer{i + 1}.xml.rels", (MemoryStream)footersRels[i]);
					}
				}
			}
			if (usePageHeadersAndFooters)
			{
				val.AddFile("word/footnotes.xml", WriteFootNotes());
				val.AddFile("word/endnotes.xml", WriteEndNotes());
			}
			val.AddFile("[Content_Types].xml", WriteContentTypes());
			val.AddFile("_rels/.rels", WriteMainRels());
			val.AddFile("docProps/app.xml", WriteDocPropsApp());
			val.AddFile("docProps/core.xml", WriteDocPropsCore());
			val.AddFile("word/settings.xml", WriteSettings(stiPagesCollection));
			val.AddFile("word/webSettings.xml", WriteWebSettings());
			val.AddFile("word/fontTable.xml", WriteFontTable());
			val.AddFile("word/_rels/document.xml.rels", WriteDocumentRels());
			val.AddFile("word/styles.xml", WriteStyles());
			if (imageCache.ImagePackedStore.Count > 0)
			{
				for (int j = 0; j < imageCache.ImagePackedStore.Count; j++)
				{
					val.AddFile($"word/media/image{j + 1:D5}.{GetImageExt(imageCache.ImageFormatStore[j])}", WriteImage(j));
				}
			}
			if (embedsList.Count > 0)
			{
				for (int k = 0; k < embedsList.Count; k++)
				{
					val.AddFile($"word/embeddings/richtext{k + 1:D5}.rtf", WriteEmbed(k));
				}
			}
			StiLicenseKey licenseKey = StiLicenseKeyValidator.GetLicenseKey();
			bool flag2 = !StiLicenseKeyValidator.IsValidOnNetFramework(licenseKey);
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
			if (!usePageHeadersAndFooters && flag2)
			{
				val.AddFile("word/footnotes.xml", WriteFootNotes());
				val.AddFile("word/endnotes.xml", WriteEndNotes());
				val.AddFile("word/media/imageAdditional.png", WriteAdditionalData(StiExportUtils.AdditionalData, base64: true));
				val.AddFile("word/headerAdditional.xml", WriteAdditionalData("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n<w:hdr xmlns:ve=\"http://schemas.openxmlformats.org/markup-compatibility/2006\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\" xmlns:m=\"http://schemas.openxmlformats.org/officeDocument/2006/math\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:wp=\"http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing\" xmlns:w10=\"urn:schemas-microsoft-com:office:word\" xmlns:w=\"http://schemas.openxmlformats.org/wordprocessingml/2006/main\" xmlns:wne=\"http://schemas.microsoft.com/office/word/2006/wordml\"><w:p><w:pPr><w:pStyle w:val=\"a3\"/></w:pPr><w:r><w:rPr><w:noProof/><w:lang w:eastAsia=\"ru-RU\"/></w:rPr><w:pict><v:shapetype id=\"_x0000_t75\" coordsize=\"21600,21600\" o:spt=\"75\" o:preferrelative=\"t\" path=\"m@4@5l@4@11@9@11@9@5xe\" filled=\"f\" stroked=\"f\"> <v:stroke joinstyle=\"miter\"/> <v:formulas><v:f eqn=\"if lineDrawn pixelLineWidth 0\"/><v:f eqn=\"sum @0 1 0\"/><v:f eqn=\"sum 0 0 @1\"/><v:f eqn=\"prod @2 1 2\"/><v:f eqn=\"prod @3 21600 pixelWidth\"/><v:f eqn=\"prod @3 21600 pixelHeight\"/><v:f eqn=\"sum @0 0 1\"/><v:f eqn=\"prod @6 1 2\"/><v:f eqn=\"prod @7 21600 pixelWidth\"/><v:f eqn=\"sum @8 21600 0\"/><v:f eqn=\"prod @7 21600 pixelHeight\"/><v:f eqn=\"sum @10 21600 0\"/></v:formulas><v:path o:extrusionok=\"f\" gradientshapeok=\"t\" o:connecttype=\"rect\"/><o:lock v:ext=\"edit\" aspectratio=\"t\"/></v:shapetype><v:shape id=\"WordPictureWatermark11777093\" o:spid=\"_x0000_s2050\" type=\"#_x0000_t75\" style=\"position:absolute;margin-left:0;margin-top:0;width:467.45pt;height:381pt;z-index:-251657216;mso-position-horizontal:center;mso-position-horizontal-relative:margin;mso-position-vertical:center;mso-position-vertical-relative:margin\" o:allowincell=\"f\"><v:imagedata r:id=\"rId1\" o:title=\"additional\"/></v:shape></w:pict></w:r></w:p></w:hdr>", base64: false));
				val.AddFile("word/_rels/headerAdditional.xml.rels", WriteAdditionalData("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\"><Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/image\" Target=\"media/imageAdditional.png\" /></Relationships>", base64: false));
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
			fontList.Clear();
			fontList = null;
			imageCache.Clear();
			imageCache = null;
			styleList.Clear();
			styleList = null;
			bookmarkList.Clear();
			bookmarkList = null;
			hyperlinkList.Clear();
			hyperlinkList = null;
			embedsList.Clear();
			embedsList = null;
			if (usePageHeadersAndFooters)
			{
				headersData.Clear();
				headersData = null;
				headersRels.Clear();
				headersRels = null;
				footersData.Clear();
				footersData = null;
				footersRels.Clear();
				footersRels = null;
			}
			if (report.RenderedPages.CacheMode)
			{
				StiMatrix.GCCollect();
			}
		}
	}
}
