using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Controls;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.ExcelXml.png")]
public class StiExcelXmlExportService : StiExportService
{
	private StiReport report;

	private string fileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	private XmlTextWriter writer;

	private string title;

	private StiMatrix matrix;

	public override string DefaultExtension => "xls";

	public override StiExportFormat ExportFormat => StiExportFormat.ExcelXml;

	public override string GroupCategory => "Excel";

	public override int Position => 31;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeExcelXmlFile");

	public override bool MultipleFiles => false;

	internal StiMatrix Matrix => matrix;

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
			exportFormRunner["ExportFormat"] = StiExportFormat.ExcelXml;
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
		return StiLocalization.Get("FileFilters", "ExcelXmlFiles");
	}

	private void Form_Complete(IStiFormRunner form, StiShowDialogCompleteEvetArgs e)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
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
			title = Path.GetFileNameWithoutExtension(fileName);
			StiFileUtils.ProcessReadOnly(fileName);
			try
			{
				using FileStream stream = new FileStream(fileName, FileMode.Create);
				StartProgress(guiMode);
				StiExcelExportSettings stiExcelExportSettings = new StiExcelExportSettings();
				stiExcelExportSettings.ExcelType = StiExcelType.ExcelXml;
				stiExcelExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
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

	private string ConvertColor(Color color)
	{
		return "#" + (color.ToArgb() & 0xFFFFFF).ToString("X").PadLeft(6, '0');
	}

	private string Convert(double x)
	{
		x = x / 300.0 * 72.0 * 3.200000047683716 * 0.96;
		return Math.Round((decimal)x, 3).ToString(CultureInfo.InvariantCulture);
	}

	private string ConvertColumn(double x)
	{
		x = x / 300.0 * 72.0 * 3.200000047683716 * 0.864 * 1.015;
		return Math.Round((decimal)x, 3).ToString(CultureInfo.InvariantCulture);
	}

	private string ConvertPageMargins(double x)
	{
		x /= 100.0;
		return Math.Round((decimal)x, 3).ToString(CultureInfo.InvariantCulture);
	}

	private string GetLineStyle(StiPenStyle penStyle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected I4, but got Unknown
		return (int)penStyle switch
		{
			0 => "Continuous", 
			4 => "Dot", 
			1 => "Dash", 
			2 => "DashDot", 
			3 => "DashDotDot", 
			5 => "Double", 
			_ => "None", 
		};
	}

	private void RenderBeginDoc()
	{
		writer.WriteStartElement("Workbook");
		writer.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:spreadsheet");
		writer.WriteAttributeString("xmlns", "o", null, "urn:schemas-microsoft-com:office:office");
		writer.WriteAttributeString("xmlns", "x", null, "urn:schemas-microsoft-com:office:excel");
		writer.WriteAttributeString("xmlns", "ss", null, "urn:schemas-microsoft-com:office:spreadsheet");
		writer.WriteAttributeString("xmlns", "html", null, "http://www.w3.org/TR/REC-html40");
		writer.WriteStartElement("DocumentProperties");
		writer.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:office");
		writer.WriteEndElement();
		writer.WriteStartElement("ExcelWorkbook");
		writer.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:excel");
		writer.WriteElementString("WindowTopX", "0");
		writer.WriteElementString("WindowTopY", "0");
		writer.WriteElementString("ProtectStructure", "False");
		writer.WriteElementString("ProtectWindows", "False");
		writer.WriteEndElement();
	}

	private void RenderStyles()
	{
		writer.WriteStartElement("Styles");
		for (int i = 0; i < Matrix.Styles.Count; i++)
		{
			StiCellStyle style = Matrix.Styles[i] as StiCellStyle;
			writer.WriteStartElement("Style");
			writer.WriteAttributeString("ss", "ID", null, "s" + i);
			RenderAlignment(style);
			RenderFont(style);
			RenderBorders(style);
			RenderInterior(style);
			RenderNumberFormat(style);
			writer.WriteEndElement();
		}
		writer.WriteEndElement();
	}

	private void RenderNumberFormat(StiCellStyle style)
	{
		writer.WriteStartElement("NumberFormat");
		if (style.Format != null && style.Format != string.Empty)
		{
			writer.WriteAttributeString("ss", "Format", null, style.Format);
		}
		writer.WriteEndElement();
	}

	private void RenderAlignment(StiCellStyle style)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		writer.WriteStartElement("Alignment");
		if (style.TextOptions != null && style.TextOptions.RightToLeft)
		{
			writer.WriteAttributeString("ss", "ReadingOrder", null, "RightToLeft");
		}
		if ((int)style.HorAlignment == 3)
		{
			writer.WriteAttributeString("ss", "Horizontal", null, "Justify");
		}
		else
		{
			writer.WriteAttributeString("ss", "Horizontal", null, ((object)(StiTextHorAlignment)(ref style.HorAlignment)).ToString());
		}
		writer.WriteAttributeString("ss", "Vertical", null, ((object)(StiVertAlignment)(ref style.VertAlignment)).ToString());
		if (style.TextOptions != null)
		{
			if (style.TextOptions.WordWrap || style.WordWrap)
			{
				writer.WriteAttributeString("ss", "WrapText", null, "1");
			}
			writer.WriteAttributeString("ss", "Rotate", null, style.TextOptions.Angle.ToString());
		}
		writer.WriteEndElement();
	}

	private void RenderFont(StiCellStyle style)
	{
		writer.WriteStartElement("Font");
		writer.WriteAttributeString("ss", "FontName", null, style.Font.Name);
		writer.WriteAttributeString("x", "CharSet", null, style.Font.GdiCharSet.ToString());
		writer.WriteAttributeString("ss", "Size", null, style.Font.SizeInPoints.ToString(CultureInfo.InvariantCulture));
		if (style.Font.Bold)
		{
			writer.WriteAttributeString("ss", "Bold", null, "1");
		}
		if (style.Font.Italic)
		{
			writer.WriteAttributeString("ss", "Italic", null, "1");
		}
		if (style.Font.Strikeout)
		{
			writer.WriteAttributeString("ss", "StrikeThrough", null, "1");
		}
		if (style.Font.Underline)
		{
			writer.WriteAttributeString("ss", "Underline", null, "Single");
		}
		writer.WriteAttributeString("ss", "Color", null, ConvertColor(style.TextColor));
		writer.WriteEndElement();
	}

	private void RenderBorder(StiBorderSide border, string position)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		if (border != (StiBorderSide)null && (int)border.Style != 6)
		{
			writer.WriteStartElement("Border");
			writer.WriteAttributeString("ss", "Position", null, position);
			writer.WriteAttributeString("ss", "LineStyle", null, GetLineStyle(border.Style));
			double num = border.Size;
			if (num > 3.0)
			{
				num = 3.0;
			}
			writer.WriteAttributeString("ss", "Weight", null, num.ToString());
			writer.WriteAttributeString("ss", "Color", null, ConvertColor(border.Color));
			writer.WriteEndElement();
		}
	}

	private void RenderBorders(StiCellStyle style)
	{
		if (style.Border != (StiBorderSide)null || style.BorderL != (StiBorderSide)null || style.BorderR != (StiBorderSide)null || style.BorderB != (StiBorderSide)null)
		{
			writer.WriteStartElement("Borders");
			RenderBorder(style.BorderL, "Left");
			RenderBorder(style.BorderR, "Right");
			RenderBorder(style.Border, "Top");
			RenderBorder(style.BorderB, "Bottom");
			writer.WriteEndElement();
		}
	}

	private void RenderInterior(StiCellStyle style)
	{
		writer.WriteStartElement("Interior");
		if (style.Color.A != 0 && style.Color != Color.White)
		{
			writer.WriteAttributeString("ss", "Color", null, ConvertColor(style.Color));
			writer.WriteAttributeString("ss", "Pattern", null, "Solid");
		}
		writer.WriteEndElement();
	}

	private void RenderWorksheetOptions(StiPage page)
	{
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Expected I4, but got Unknown
		int num = 0;
		int num2 = 0;
		if (StiOptions.Export.Excel.AllowFreezePanes)
		{
			for (int i = 0; i < Matrix.CoordY.Count - 1; i++)
			{
				for (int j = 0; j < Matrix.CoordX.Count - 1; j++)
				{
					StiCell stiCell = Matrix.Cells[i, j];
					if (stiCell != null && stiCell.Component != null && (stiCell.Component.Locked || (stiCell.Component.TagValue != null && stiCell.Component.TagValue.ToString().Contains("excelfreezepanes"))))
					{
						string componentPlacement = stiCell.Component.ComponentPlacement;
						if (componentPlacement != null && componentPlacement.Length > 0 && (componentPlacement.StartsWith("rt") || componentPlacement.StartsWith("ph") || componentPlacement.StartsWith("h")))
						{
							num = stiCell.Left;
							num2 = stiCell.Top + stiCell.Height + 1;
							break;
						}
					}
				}
			}
		}
		writer.WriteStartElement("WorksheetOptions");
		writer.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:excel");
		writer.WriteStartElement("PageSetup");
		if (page.Orientation == StiPageOrientation.Landscape)
		{
			writer.WriteStartElement("Layout");
			writer.WriteAttributeString("x", "Orientation", null, "Landscape");
			writer.WriteEndElement();
		}
		writer.WriteStartElement("PageMargins");
		writer.WriteAttributeString("x", "Bottom", null, ConvertPageMargins(page.Unit.ConvertToHInches(page.Margins.Bottom)));
		writer.WriteAttributeString("x", "Left", null, ConvertPageMargins(page.Unit.ConvertToHInches(page.Margins.Left)));
		writer.WriteAttributeString("x", "Right", null, ConvertPageMargins(page.Unit.ConvertToHInches(page.Margins.Right)));
		writer.WriteAttributeString("x", "Top", null, ConvertPageMargins(page.Unit.ConvertToHInches(page.Margins.Top)));
		writer.WriteEndElement();
		writer.WriteEndElement();
		writer.WriteStartElement("DoNotDisplayGridlines");
		writer.WriteEndElement();
		if (num != 0 || num2 != 0)
		{
			if (num == 0)
			{
				writer.WriteElementString("SplitHorizontal", num2.ToString());
				writer.WriteElementString("TopRowBottomPane", num2.ToString());
				writer.WriteStartElement("FreezePanes");
				writer.WriteEndElement();
				writer.WriteStartElement("FrozenNoSplit");
				writer.WriteEndElement();
				writer.WriteElementString("ActivePane", "2");
			}
			else if (num2 == 0)
			{
				writer.WriteElementString("SplitVertical", num.ToString());
				writer.WriteElementString("LeftColumnRightPane", num.ToString());
				writer.WriteStartElement("FreezePanes");
				writer.WriteEndElement();
				writer.WriteStartElement("FrozenNoSplit");
				writer.WriteEndElement();
				writer.WriteElementString("ActivePane", "1");
			}
			else
			{
				writer.WriteElementString("SplitHorizontal", num2.ToString());
				writer.WriteElementString("TopRowBottomPane", num2.ToString());
				writer.WriteElementString("SplitVertical", num.ToString());
				writer.WriteElementString("LeftColumnRightPane", num.ToString());
				writer.WriteStartElement("FreezePanes");
				writer.WriteEndElement();
				writer.WriteStartElement("FrozenNoSplit");
				writer.WriteEndElement();
				writer.WriteElementString("ActivePane", "0");
			}
		}
		writer.WriteStartElement("ProtectObjects");
		writer.WriteString("False");
		writer.WriteEndElement();
		writer.WriteStartElement("ProtectScenarios");
		writer.WriteString("False");
		writer.WriteEndElement();
		if ((int)page.PaperSize != 0)
		{
			writer.WriteStartElement("Print");
			writer.WriteElementString("ValidPrinterInfo", "");
			writer.WriteElementString("PaperSizeIndex", $"{(int)page.PaperSize}");
			writer.WriteElementString("HorizontalResolution", "600");
			writer.WriteElementString("VerticalResolution", "600");
			writer.WriteEndElement();
		}
		writer.WriteEndElement();
	}

	private void RenderColumns()
	{
		for (int i = 1; i < Matrix.CoordX.Count; i++)
		{
			writer.WriteStartElement("Column");
			writer.WriteAttributeString("ss", "AutoFitWidth", null, "0");
			double num = (double)Matrix.CoordX.GetByIndex(i);
			double num2 = (double)Matrix.CoordX.GetByIndex(i - 1);
			writer.WriteAttributeString("ss", "Width", null, ConvertColumn(num - num2));
			writer.WriteEndElement();
		}
	}

	private void RenderRows()
	{
		RichTextBox val = null;
		bool[,] array = new bool[Matrix.CoordY.Count, Matrix.CoordX.Count];
		float num = Math.Max((float)Matrix.CoordY.Count / 200f, 1f);
		int num2 = 0;
		for (int i = 1; i < Matrix.CoordY.Count; i++)
		{
			int num3 = (int)((float)i / num);
			if (num3 > num2)
			{
				num2 = num3;
				InvokeExporting(i, matrix.CoordY.Count, base.CurrentPassNumber, base.MaximumPassNumber);
			}
			if (base.IsStopped)
			{
				return;
			}
			writer.WriteStartElement("Row");
			writer.WriteAttributeString("ss", "AutoFitHeight", null, "0");
			double x = (double)Matrix.CoordY.GetByIndex(i) - (double)Matrix.CoordY.GetByIndex(i - 1);
			writer.WriteAttributeString("ss", "Height", null, Convert(x));
			for (int j = 1; j < Matrix.CoordX.Count; j++)
			{
				StiCell stiCell = Matrix.Cells[i - 1, j - 1];
				if (stiCell != null && !array[i, j])
				{
					array[i, j] = true;
					StiRichText stiRichText = stiCell.Component as StiRichText;
					string text = stiCell.Text;
					if (stiRichText != null && stiRichText.RtfText != string.Empty)
					{
						if (val == null)
						{
							val = (RichTextBox)(object)new StiRichTextBox(drawTransparent: false);
						}
						stiRichText.GetPreparedText(val);
						text = ((Control)val).Text;
					}
					if (stiCell.Component is StiCheckBox stiCheckBox && !string.IsNullOrEmpty(stiCheckBox.ExcelDataValue))
					{
						text = stiCheckBox.ExcelDataValue;
					}
					writer.WriteStartElement("Cell");
					writer.WriteAttributeString("ss", "Index", null, j.ToString());
					StiCellStyle value = matrix.CellStyles[i - 1, j - 1];
					int num4 = Matrix.Styles.IndexOf(value);
					writer.WriteAttributeString("ss", "StyleID", null, "s" + num4);
					if (stiCell.Width > 0)
					{
						writer.WriteAttributeString("ss", "MergeAcross", null, stiCell.Width.ToString());
					}
					if (stiCell.Height > 0)
					{
						writer.WriteAttributeString("ss", "MergeDown", null, stiCell.Height.ToString());
					}
					for (int k = 0; k <= stiCell.Width; k++)
					{
						for (int l = 0; l <= stiCell.Height; l++)
						{
							array[i + l, j + k] = true;
						}
					}
					StiText stiText = stiCell.Component as StiText;
					if (!string.IsNullOrEmpty(text) || (stiText != null && stiText.ExcelDataValue != null && stiText.ExcelDataValue != "-"))
					{
						writer.WriteStartElement("Data");
						string value2 = "String";
						if (stiText != null && stiText.ExcelDataValue != null)
						{
							string excelDataValue = stiText.ExcelDataValue;
							string numberDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
							excelDataValue = excelDataValue.Replace(".", ",").Replace(",", numberDecimalSeparator);
							bool flag = false;
							bool flag2 = false;
							bool flag3 = false;
							bool flag4 = false;
							string text2 = string.Empty;
							if (stiText != null)
							{
								text2 = stiText.Format;
							}
							if (text2 != null && text2.Length > 0)
							{
								if (text2[0] == 'C')
								{
									flag = true;
								}
								if (text2[0] == 'P')
								{
									flag2 = true;
								}
								if (text2[0] == 'D')
								{
									flag3 = true;
								}
								if (text2[0] == 'T')
								{
									flag4 = true;
								}
							}
							bool flag5 = false;
							double result;
							if (flag || flag2 || flag3 || flag4)
							{
								flag5 = true;
							}
							else if (!double.TryParse(excelDataValue, NumberStyles.Any, NumberFormatInfo.CurrentInfo, out result))
							{
								flag5 = true;
							}
							if (!flag5)
							{
								value2 = "Number";
								text = excelDataValue;
							}
						}
						writer.WriteAttributeString("ss", "Type", null, value2);
						string[] array2 = text.Replace("\n", "").Replace('\t', ' ').Split(new char[1] { '\r' });
						for (int m = 0; m < array2.Length; m++)
						{
							writer.WriteString(array2[m].TrimEnd(new char[1] { ' ' }));
							if (m < array2.Length - 1)
							{
								writer.WriteRaw("&#10;");
							}
						}
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
				}
				if (stiCell != null || array[i, j])
				{
					continue;
				}
				StiCellStyle stiCellStyle = matrix.CellStyles[i - 1, j - 1];
				if (stiCellStyle != null)
				{
					int num5 = Matrix.Styles.IndexOf(stiCellStyle);
					writer.WriteStartElement("Cell");
					if (j > 1)
					{
						writer.WriteAttributeString("ss", "Index", null, j.ToString());
					}
					writer.WriteAttributeString("ss", "StyleID", null, "s" + num5);
					writer.WriteEndElement();
				}
			}
			writer.WriteEndElement();
		}
		((Component)(object)val)?.Dispose();
	}

	private void RenderCells(StiPage page)
	{
		writer.WriteStartElement("Worksheet");
		writer.WriteAttributeString("ss", "Name", null, title);
		writer.WriteStartElement("Table");
		writer.WriteAttributeString("ss", "ExpandedColumnCount", null, (Matrix.CoordX.Count - 1).ToString());
		writer.WriteAttributeString("ss", "ExpandedRowCount", null, (Matrix.CoordY.Count - 1).ToString());
		writer.WriteAttributeString("x", "FullColumns", null, "1");
		writer.WriteAttributeString("x", "FullRows", null, "1");
		RenderColumns();
		RenderRows();
		writer.WriteEndElement();
		RenderWorksheetOptions(page);
		writer.WriteEndElement();
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
		ExportExcel(report, stream, new StiExcelXmlExportSettings());
	}

	public void ExportExcel(StiReport report, Stream stream, StiPagesRange pageRange)
	{
		StiExcelXmlExportSettings settings = new StiExcelXmlExportSettings
		{
			PageRange = pageRange
		};
		ExportExcel(report, stream, settings);
	}

	public void ExportExcel(StiReport report, Stream stream, StiExcelExportSettings settings)
	{
		StiLogService.Write(((object)this).GetType(), "Export report to Excel Xml format");
		if (settings == null)
		{
			throw new ArgumentNullException("The 'settings' argument cannot be equal in null.");
		}
		StiPagesRange pageRange = settings.PageRange;
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			title = ((report.ReportAlias == null || report.ReportAlias.Trim().Length == 0) ? report.ReportName : report.ReportAlias);
			StiPagesCollection selectedPages = pageRange.GetSelectedPages(report.RenderedPages);
			base.CurrentPassNumber = 0;
			base.MaximumPassNumber = 3;
			matrix = new StiMatrix(selectedPages, StiOptions.Export.Excel.DivideBigCells, this);
			if (!base.IsStopped)
			{
				base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
				base.CurrentPassNumber = 2;
				writer = new XmlTextWriter(stream, Encoding.UTF8)
				{
					Indentation = 2,
					Formatting = Formatting.Indented
				};
				writer.WriteStartDocument();
				RenderBeginDoc();
				RenderStyles();
				selectedPages.GetPage(selectedPages[0]);
				RenderCells(selectedPages[0]);
				if (!base.IsStopped)
				{
					writer.WriteFullEndElement();
					writer.WriteEndDocument();
					writer.Flush();
				}
			}
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
			if (matrix != null)
			{
				matrix.Clear();
				matrix = null;
			}
		}
	}
}
