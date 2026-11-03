using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.BarCodes;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Export;

public sealed class StiHtmlTableRender
{
	private StiHtmlExportService htmlExport;

	private StiHtmlExportSettings htmlExportSettings;

	private StiMatrix matrix;

	public StiMatrix Matrix
	{
		get
		{
			return matrix;
		}
		set
		{
			matrix = value;
		}
	}

	public void RenderStyle(StiCellStyle style)
	{
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		htmlExport.RenderBackColor(null, style.Color);
		htmlExport.RenderTextColor(null, style.TextColor);
		htmlExport.RenderFont(null, style.Font);
		htmlExport.RenderBorder(null, style.Border, "top");
		htmlExport.RenderBorder(null, style.BorderL, "left");
		htmlExport.RenderBorder(null, style.BorderR, "right");
		htmlExport.RenderBorder(null, style.BorderB, "bottom");
		htmlExport.RenderTextDirection(null, style.TextOptions);
		htmlExport.RenderTextHorAlignment(null, style.TextOptions, style.HorAlignment);
		htmlExport.RenderVertAlignment(null, style.VertAlignment, style.TextOptions, style.AllowHtmlTags);
		if (style.AbsolutePosition)
		{
			htmlExport.HtmlWriter.WriteStyleAttribute("position", "absolute");
		}
		htmlExport.HtmlWriter.Write("overflow:hidden;");
		htmlExport.HtmlWriter.Write($"line-height:{Math.Round(1.15 * style.LineSpacing * StiHtmlExportService.GetFontHeightCorrection(style.Font.Name), 2)}em;".Replace(",", "."));
		if (style.TextOptions != null && (int)style.TextOptions.Trimming != 0)
		{
			htmlExport.HtmlWriter.Write("text-overflow:ellipsis;");
		}
	}

	public void RenderStyleTable(StiHtmlTableCell cell, StiCellStyle style)
	{
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Invalid comparison between Unknown and I4
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		htmlExport.RenderBackColor(cell, style.Color);
		htmlExport.RenderTextColor(cell, style.TextColor);
		htmlExport.RenderFont(cell, style.Font);
		if (cell == null)
		{
			htmlExport.HtmlWriter.Write("border:0px;");
		}
		htmlExport.RenderBorder(cell, style.Border, "top");
		htmlExport.RenderBorder(cell, style.BorderL, "left");
		htmlExport.RenderBorder(cell, style.BorderR, "right");
		htmlExport.RenderBorder(cell, style.BorderB, "bottom");
		htmlExport.RenderTextDirection(cell, style.TextOptions);
		htmlExport.RenderTextHorAlignment(cell, style.TextOptions, style.HorAlignment);
		htmlExport.RenderVertAlignment(cell, style.VertAlignment, style.TextOptions, style.AllowHtmlTags);
		bool flag = style.WordWrap;
		if ((int)style.HorAlignment == 3)
		{
			flag = true;
		}
		if (cell == null)
		{
			if (style.AbsolutePosition)
			{
				htmlExport.HtmlWriter.WriteStyleAttribute("position", "absolute");
			}
			htmlExport.HtmlWriter.Write($"line-height:{Math.Round(1.15 * style.LineSpacing * StiHtmlExportService.GetFontHeightCorrection(style.Font.Name), 2)}em;".Replace(",", "."));
			if (StiOptions.Export.Html.UseStrictTableCellSize || StiOptions.Export.Html.UseStrictTableCellSizeV2)
			{
				if (flag)
				{
					htmlExport.HtmlWriter.Write("word-wrap:break-word;");
				}
				else
				{
					htmlExport.HtmlWriter.Write("white-space:nowrap;");
				}
				htmlExport.HtmlWriter.Write("overflow:hidden;");
				if (style.TextOptions != null && (int)style.TextOptions.Trimming != 0)
				{
					htmlExport.HtmlWriter.Write("text-overflow:ellipsis;");
				}
			}
			else if (!flag && StiOptions.Export.Html.UseWordWrapBreakWordMode)
			{
				htmlExport.HtmlWriter.Write("word-wrap:break-word;");
			}
			return;
		}
		if (style.AbsolutePosition)
		{
			cell.Style["position"] = "absolute";
		}
		cell.Style["line-height"] = $"{Math.Round(1.15 * style.LineSpacing * StiHtmlExportService.GetFontHeightCorrection(style.Font.Name), 2)}em".Replace(",", ".");
		if (StiOptions.Export.Html.UseStrictTableCellSize || StiOptions.Export.Html.UseStrictTableCellSizeV2)
		{
			if (flag)
			{
				cell.Style["word-wrap"] = "break-word";
			}
			else
			{
				cell.Style["white-space"] = "nowrap";
			}
			cell.Style["overflow"] = "hidden";
			if (style.TextOptions != null && (int)style.TextOptions.Trimming != 0)
			{
				cell.Style["text-overflow"] = "ellipsis";
			}
		}
		else if (!flag && StiOptions.Export.Html.UseWordWrapBreakWordMode)
		{
			cell.Style["word-wrap"] = "break-word";
		}
	}

	public void RenderStyles(bool useBookmarks, bool exportBookmarksOnly, Hashtable cssStyles)
	{
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		htmlExport.HtmlWriter.WriteLine("<style type=\"text/css\">");
		if (StiOptions.Export.Html.UseExtendedStyle)
		{
			htmlExport.HtmlWriter.WriteLine(".sBaseStyleFix { border: 0; }");
		}
		if (!exportBookmarksOnly && htmlExport.useStylesTable)
		{
			for (int i = 0; i < matrix.Styles.Count; i++)
			{
				StiCellStyle stiCellStyle = matrix.Styles[i] as StiCellStyle;
				htmlExport.HtmlWriter.Write(".s" + stiCellStyle.StyleName);
				htmlExport.HtmlWriter.Write("{");
				RenderStyle(stiCellStyle);
				htmlExport.HtmlWriter.WriteLine("}");
			}
		}
		if (cssStyles != null && cssStyles.Count > 0)
		{
			foreach (DictionaryEntry cssStyle in cssStyles)
			{
				htmlExport.HtmlWriter.WriteLine("." + (string)cssStyle.Key + " {" + (string)cssStyle.Value + ";overflow:hidden;}");
			}
		}
		if (useBookmarks)
		{
			if (StiOptions.Export.Html.UseExtendedStyle)
			{
				htmlExport.HtmlWriter.WriteLine(".dtree {border:0; font-family: Verdana, Geneva, Arial, Helvetica, sans-serif;font-size:11px;color:#666;white-space:nowrap;}");
				htmlExport.HtmlWriter.WriteLine(".dTreeNode {border:0;}");
				htmlExport.HtmlWriter.WriteLine(".dtreeStyleFix {border:0;}");
				htmlExport.HtmlWriter.WriteLine(".dtree img {border:0; vertical-align: middle;}");
				htmlExport.HtmlWriter.WriteLine(".dtree a {border:0;line-height:0; color:#333;text-decoration:none;}");
				htmlExport.HtmlWriter.WriteLine(".dtree a.node, .dtree a.nodeSel {border:0; white-space: nowrap;padding: 1px 2px 1px 2px;font-family: Verdana, Geneva, Arial, Helvetica, sans-serif;font-size:11px;color:#666;text-decoration: none;font-weight:normal;}");
				htmlExport.HtmlWriter.WriteLine(".dtree a.node:hover, .dtree a.nodeSel:hover {border:0; color: #333;text-decoration: underline;}");
				htmlExport.HtmlWriter.WriteLine(".dtree a.nodeSel {border:0; background-color: #c0d2ec;}");
				htmlExport.HtmlWriter.WriteLine(".dtree .clip {border:0; overflow: hidden;}");
				htmlExport.HtmlWriter.WriteLine(".dtreeframe {border:0; border-right:1px;border-right-style:solid;border-right-color:Gray;}");
			}
			else
			{
				htmlExport.HtmlWriter.WriteLine(".dtree {font-family: Verdana, Geneva, Arial, Helvetica, sans-serif;font-size:11px;color:#666;white-space:nowrap;}");
				htmlExport.HtmlWriter.WriteLine(".dtree img {border: 0px;vertical-align: middle;}");
				htmlExport.HtmlWriter.WriteLine(".dtree a {color: #333;text-decoration: none;}");
				htmlExport.HtmlWriter.WriteLine(".dtree a.node, .dtree a.nodeSel {white-space: nowrap;padding: 1px 2px 1px 2px;}");
				htmlExport.HtmlWriter.WriteLine(".dtree a.node:hover, .dtree a.nodeSel:hover {color: #333;text-decoration: underline;}");
				htmlExport.HtmlWriter.WriteLine(".dtree a.nodeSel {background-color: #c0d2ec;}");
				htmlExport.HtmlWriter.WriteLine(".dtree .clip {overflow: hidden;}");
				htmlExport.HtmlWriter.WriteLine(".dtreeframe {border-right:1px;border-right-style:solid;border-right-color:Gray;}");
			}
		}
		if (htmlExportSettings.AddPageBreaks)
		{
			StiPage stiPage = htmlExport.report.RenderedPages[0];
			htmlExport.report.RenderedPages.GetPage(stiPage);
			double num = Math.Round(StiUnit.Millimeters.ConvertFromHInches(htmlExport.report.Unit.ConvertToHInches(stiPage.PageWidth)), 2);
			double num2 = Math.Round(StiUnit.Millimeters.ConvertFromHInches(htmlExport.report.Unit.ConvertToHInches(stiPage.PageHeight)), 2);
			double num3 = Math.Round(StiUnit.Millimeters.ConvertFromHInches(htmlExport.report.Unit.ConvertToHInches(stiPage.Margins.Left)), 2);
			double num4 = Math.Round(StiUnit.Millimeters.ConvertFromHInches(htmlExport.report.Unit.ConvertToHInches(stiPage.Margins.Right)), 2);
			double num5 = Math.Round(StiUnit.Millimeters.ConvertFromHInches(htmlExport.report.Unit.ConvertToHInches(stiPage.Margins.Top)), 2);
			double num6 = Math.Round(StiUnit.Millimeters.ConvertFromHInches(htmlExport.report.Unit.ConvertToHInches(stiPage.Margins.Bottom)), 2);
			htmlExport.HtmlWriter.WriteLine(".pagemargins { padding:0; border:0; }");
			htmlExport.HtmlWriter.Write("@page { size: ");
			if ((int)stiPage.PaperSize != 0)
			{
				StiHtmlTextWriter htmlWriter = htmlExport.HtmlWriter;
				PaperKind paperSize = stiPage.PaperSize;
				htmlWriter.Write(((object)(PaperKind)(ref paperSize)).ToString());
				if (stiPage.Orientation == StiPageOrientation.Landscape)
				{
					htmlExport.HtmlWriter.Write(" landscape");
				}
			}
			else
			{
				htmlExport.HtmlWriter.Write($"{num}mm {num2}mm");
			}
			htmlExport.HtmlWriter.WriteLine("; margin: 0; }");
			htmlExport.HtmlWriter.Write($"@media print {{ html,body {{ width: {num - num3 - num4}mm; height: {num2 - num5 - num6}mm; }} ");
			htmlExport.HtmlWriter.WriteLine($".pagemargins {{ padding: {num5}mm {num4}mm {num6 / 2.0}mm {num3}mm; border:0; }} }}");
		}
		htmlExport.HtmlWriter.WriteLine("</style>");
	}

	public void RenderStylesTable(bool useBookmarks, bool exportBookmarksOnly)
	{
		RenderStylesTable(useBookmarks, exportBookmarksOnly, null);
	}

	public void RenderStylesTable(bool useBookmarks, bool exportBookmarksOnly, bool addStyleTag)
	{
		RenderStylesTable(useBookmarks, exportBookmarksOnly, addStyleTag, null);
	}

	public void RenderStylesTable(bool useBookmarks, bool exportBookmarksOnly, Hashtable cssStyles)
	{
		RenderStylesTable(useBookmarks, exportBookmarksOnly, addStyleTag: true, cssStyles);
	}

	public void RenderStylesTable(bool useBookmarks, bool exportBookmarksOnly, bool addStyleTag, Hashtable cssStyles)
	{
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		if (addStyleTag)
		{
			htmlExport.HtmlWriter.WriteLine("<style type=\"text/css\">");
		}
		if (StiOptions.Export.Html.UseExtendedStyle)
		{
			htmlExport.HtmlWriter.WriteLine(".sBaseStyleFix { border: 0; }");
		}
		if (!exportBookmarksOnly && htmlExport.useStylesTable)
		{
			for (int i = 0; i < matrix.Styles.Count; i++)
			{
				StiCellStyle stiCellStyle = matrix.Styles[i] as StiCellStyle;
				htmlExport.HtmlWriter.Write(".s" + stiCellStyle.StyleName);
				htmlExport.HtmlWriter.Write("{");
				RenderStyleTable(null, stiCellStyle);
				htmlExport.HtmlWriter.WriteLine("}");
			}
		}
		if (cssStyles != null && cssStyles.Count > 0)
		{
			foreach (DictionaryEntry cssStyle in cssStyles)
			{
				htmlExport.HtmlWriter.WriteLine("." + (string)cssStyle.Key + " {" + (string)cssStyle.Value + ";}");
			}
		}
		if (useBookmarks)
		{
			if (StiOptions.Export.Html.UseExtendedStyle)
			{
				htmlExport.HtmlWriter.WriteLine(".dtree {border:0; font-family: Verdana, Geneva, Arial, Helvetica, sans-serif;font-size:11px;color:#666;white-space:nowrap;}");
				htmlExport.HtmlWriter.WriteLine(".dTreeNode {border:0;}");
				htmlExport.HtmlWriter.WriteLine(".dtreeStyleFix {border:0;}");
				htmlExport.HtmlWriter.WriteLine(".dtree img {border:0; vertical-align: middle;}");
				htmlExport.HtmlWriter.WriteLine(".dtree a {border:0;line-height:0; color:#333;text-decoration:none;}");
				htmlExport.HtmlWriter.WriteLine(".dtree a.node, .dtree a.nodeSel {border:0; white-space: nowrap;padding: 1px 2px 1px 2px;font-family: Verdana, Geneva, Arial, Helvetica, sans-serif;font-size:11px;color:#666;text-decoration: none;font-weight:normal;}");
				htmlExport.HtmlWriter.WriteLine(".dtree a.node:hover, .dtree a.nodeSel:hover {border:0; color: #333;text-decoration: underline;}");
				htmlExport.HtmlWriter.WriteLine(".dtree a.nodeSel {border:0; background-color: #c0d2ec;}");
				htmlExport.HtmlWriter.WriteLine(".dtree .clip {border:0; overflow: hidden;}");
				htmlExport.HtmlWriter.WriteLine(".dtreeframe {border:0; border-right:1px;border-right-style:solid;border-right-color:Gray;}");
			}
			else
			{
				htmlExport.HtmlWriter.WriteLine(".dtree {font-family: Verdana, Geneva, Arial, Helvetica, sans-serif;font-size:11px;color:#666;white-space:nowrap;}");
				htmlExport.HtmlWriter.WriteLine(".dtree img {border: 0px;vertical-align: middle;}");
				htmlExport.HtmlWriter.WriteLine(".dtree a {color: #333;text-decoration: none;}");
				htmlExport.HtmlWriter.WriteLine(".dtree a.node, .dtree a.nodeSel {white-space: nowrap;padding: 1px 2px 1px 2px;}");
				htmlExport.HtmlWriter.WriteLine(".dtree a.node:hover, .dtree a.nodeSel:hover {color: #333;text-decoration: underline;}");
				htmlExport.HtmlWriter.WriteLine(".dtree a.nodeSel {background-color: #c0d2ec;}");
				htmlExport.HtmlWriter.WriteLine(".dtree .clip {overflow: hidden;}");
				htmlExport.HtmlWriter.WriteLine(".dtreeframe {border-right:1px;border-right-style:solid;border-right-color:Gray;}");
			}
		}
		if (htmlExportSettings.AddPageBreaks)
		{
			StiPage stiPage = htmlExport.report.RenderedPages[0];
			htmlExport.report.RenderedPages.GetPage(stiPage);
			double num = Math.Round(StiUnit.Millimeters.ConvertFromHInches(htmlExport.report.Unit.ConvertToHInches(stiPage.PageWidth)), 2);
			double num2 = Math.Round(StiUnit.Millimeters.ConvertFromHInches(htmlExport.report.Unit.ConvertToHInches(stiPage.PageHeight)), 2);
			double num3 = Math.Round(StiUnit.Millimeters.ConvertFromHInches(htmlExport.report.Unit.ConvertToHInches(stiPage.Margins.Left)), 2);
			double num4 = Math.Round(StiUnit.Millimeters.ConvertFromHInches(htmlExport.report.Unit.ConvertToHInches(stiPage.Margins.Right)), 2);
			double num5 = Math.Round(StiUnit.Millimeters.ConvertFromHInches(htmlExport.report.Unit.ConvertToHInches(stiPage.Margins.Top)), 2);
			double num6 = Math.Round(StiUnit.Millimeters.ConvertFromHInches(htmlExport.report.Unit.ConvertToHInches(stiPage.Margins.Bottom)), 2);
			htmlExport.HtmlWriter.WriteLine(".pagemargins { padding:0; border:0; }");
			htmlExport.HtmlWriter.Write("@page { size: ");
			if ((int)stiPage.PaperSize != 0)
			{
				StiHtmlTextWriter htmlWriter = htmlExport.HtmlWriter;
				PaperKind paperSize = stiPage.PaperSize;
				htmlWriter.Write(((object)(PaperKind)(ref paperSize)).ToString());
				if (stiPage.Orientation == StiPageOrientation.Landscape)
				{
					htmlExport.HtmlWriter.Write(" landscape");
				}
			}
			else
			{
				htmlExport.HtmlWriter.Write($"{num}mm {num2}mm");
			}
			htmlExport.HtmlWriter.WriteLine("; margin: 0; }");
			htmlExport.HtmlWriter.Write($"@media print {{ html,body {{ width: {num - num3 - num4}mm; height: {num2 - num5 - num6}mm; }} ");
			htmlExport.HtmlWriter.WriteLine($".pagemargins {{ padding: {num5}mm {num4}mm {num6 / 2.0}mm {num3}mm; border:0; }} }}");
		}
		if (addStyleTag)
		{
			htmlExport.HtmlWriter.WriteLine("</style>");
		}
	}

	private double GetWidth(IList listX, int columnIndex, double zoom)
	{
		return ((double)listX[columnIndex + 1] - (double)listX[columnIndex]) * zoom;
	}

	private double GetHeight(IList listY, int rowIndex, double zoom)
	{
		return ((double)listY[rowIndex + 1] - (double)listY[rowIndex]) * zoom;
	}

	public void RenderTable(bool renderStyles, string backGroundImageString, bool useBookmarks, bool exportBookmarksOnly, Hashtable cssStyles, bool watermarkShowBehind = false, StiBorder border = null)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Invalid comparison between Unknown and I4
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_28bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Invalid comparison between Unknown and I4
		//IL_0664: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Invalid comparison between Unknown and I4
		//IL_07ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f5: Invalid comparison between Unknown and I4
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Invalid comparison between Unknown and I4
		//IL_0850: Unknown result type (might be due to invalid IL or missing references)
		//IL_0856: Invalid comparison between Unknown and I4
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06aa: Invalid comparison between Unknown and I4
		//IL_1d17: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d38: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d46: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2401: Unknown result type (might be due to invalid IL or missing references)
		//IL_240a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1044: Unknown result type (might be due to invalid IL or missing references)
		//IL_106a: Unknown result type (might be due to invalid IL or missing references)
		//IL_108d: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_258e: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2609: Unknown result type (might be due to invalid IL or missing references)
		//IL_260f: Invalid comparison between Unknown and I4
		//IL_2641: Unknown result type (might be due to invalid IL or missing references)
		//IL_2658: Unknown result type (might be due to invalid IL or missing references)
		//IL_265e: Invalid comparison between Unknown and I4
		//IL_2670: Unknown result type (might be due to invalid IL or missing references)
		//IL_2676: Invalid comparison between Unknown and I4
		//IL_2681: Unknown result type (might be due to invalid IL or missing references)
		//IL_2687: Invalid comparison between Unknown and I4
		if (renderStyles)
		{
			RenderStylesTable(useBookmarks, exportBookmarksOnly, cssStyles);
		}
		StiHtmlTable stiHtmlTable = new StiHtmlTable();
		stiHtmlTable.Align = htmlExport.PageHorAlignment;
		stiHtmlTable.BackImageUrl = backGroundImageString;
		if (watermarkShowBehind)
		{
			stiHtmlTable.Position = "relative";
		}
		stiHtmlTable.Width = StiHtmlUnit.NewUnit(Math.Round(matrix.TotalWidth * htmlExport.zoom, 0), StiOptions.Export.Html.PrintLayoutOptimization);
		stiHtmlTable.Border = border;
		stiHtmlTable.BorderWidth = 0;
		stiHtmlTable.CellPadding = 0;
		stiHtmlTable.CellSpacing = 0;
		IList valueList = matrix.CoordX.GetValueList();
		IList valueList2 = matrix.CoordY.GetValueList();
		bool[,] array = new bool[matrix.CoordX.Count, matrix.CoordY.Count];
		Hashtable hashtable = new Hashtable();
		foreach (object style in matrix.Styles)
		{
			hashtable[style] = matrix.Styles.IndexOf(style);
		}
		htmlExport.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
		double num = Math.Max((float)matrix.CoordY.Count / 200f, 1f);
		int num2 = 0;
		for (int i = 0; i < matrix.CoordY.Count - 1; i++)
		{
			int num3 = (int)((double)i / num);
			if (num3 > num2)
			{
				num2 = num3;
				htmlExport.InvokeExporting(i, matrix.CoordY.Count, 2, 3);
			}
			if (htmlExport.IsStopped)
			{
				return;
			}
			double height = GetHeight(valueList2, i, htmlExport.zoom);
			StiHtmlTableRow stiHtmlTableRow = new StiHtmlTableRow();
			stiHtmlTableRow.Height = StiHtmlUnit.NewUnit(height, StiOptions.Export.Html.PrintLayoutOptimization);
			stiHtmlTable.Rows.Add(stiHtmlTableRow);
			if (htmlExportSettings.AddPageBreaks && matrix.HorizontalPageBreaks.Contains(i))
			{
				stiHtmlTableRow.Style["page-break-before"] = "always";
			}
			Color[] array2 = new Color[matrix.CoordX.Count - 1];
			double num4 = 0.0;
			for (int j = 0; j < matrix.CoordX.Count - 1; j++)
			{
				StiBorderSide val = matrix.BordersX[i, j];
				if (val != (StiBorderSide)null && (int)val.Style != 6)
				{
					num4 = Math.Max(num4, val.GetSize());
				}
			}
			List<StiMatrix.RoundedRectangleInfo> value = null;
			matrix.RoundedRectanglesList.TryGetValue(i, out value);
			for (int k = 0; k < matrix.CoordX.Count - 1; k++)
			{
				if (array[k, i])
				{
					continue;
				}
				StiHtmlTableCell stiHtmlTableCell = new StiHtmlTableCell();
				stiHtmlTableRow.Cells.Add(stiHtmlTableCell);
				double num5 = GetWidth(valueList, k, htmlExport.zoom);
				double num6 = height;
				StiCell stiCell = matrix.Cells[i, k];
				if (stiCell != null)
				{
					string text = ((stiCell.Text != null) ? stiCell.Text : string.Empty);
					IStiTextOptions stiTextOptions = stiCell.Component as IStiTextOptions;
					if (stiTextOptions != null && StiOptions.Export.Html.ConvertDigitsToArabic && stiTextOptions.TextOptions.RightToLeft)
					{
						text = StiExportUtils.ConvertDigitsToArabic(text, StiOptions.Export.Html.ArabicDigitsType);
					}
					StiText stiText = stiCell.Component as StiText;
					bool flag = true;
					if (stiCell.Component != null)
					{
						if (stiText != null && stiText.CheckAllowHtmlTags())
						{
							text = StiHtmlExportService.ConvertTextWithHtmlTagsToHtmlText(stiText, text, htmlExport.zoom);
							flag = false;
						}
						if (StiOptions.Export.Html.ForceWysiwygWordwrap && stiText != null && !stiText.CheckAllowHtmlTags() && stiText.TextQuality == StiTextQuality.Wysiwyg && stiTextOptions != null && stiTextOptions.TextOptions.WordWrap)
						{
							List<string> textLines = StiTextRenderer.GetTextLines(htmlExport.report.ReportMeasureGraphics, ref text, stiText.Font, htmlExport.report.Unit.ConvertToHInches(stiCell.Component.ComponentToPage(stiCell.Component.ClientRectangle)), 1.0, true, stiTextOptions.TextOptions.RightToLeft, 1.0, (double)stiTextOptions.TextOptions.Angle, stiTextOptions.TextOptions.Trimming, stiText.CheckAllowHtmlTags(), stiTextOptions.TextOptions, (double?)1.0);
							StringBuilder stringBuilder = new StringBuilder();
							for (int l = 0; l < textLines.Count; l++)
							{
								string value2 = textLines[l];
								stringBuilder.Append(value2);
								if (l < textLines.Count - 1)
								{
									stringBuilder.Append("\n");
								}
							}
							text = stringBuilder.ToString();
						}
					}
					if (StiOptions.Export.Html.ReplaceSpecialCharacters && flag)
					{
						text = text.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("<", "&lt;")
							.Replace(">", "&gt;")
							.Replace("\u00a0", "&nbsp;");
					}
					int num7 = stiCell.Width + 1;
					int num8 = stiCell.Height + 1;
					for (int m = k; m < k + num7; m++)
					{
						for (int n = i; n < i + num8; n++)
						{
							array[m, n] = true;
						}
					}
					if (num7 > 1)
					{
						stiHtmlTableCell.ColumnSpan = num7;
					}
					if (num8 > 1)
					{
						stiHtmlTableCell.RowSpan = num8;
					}
					StiCellStyle stiCellStyle = matrix.CellStyles[i, k];
					if (stiCellStyle == null)
					{
						stiCellStyle = stiCell.CellStyle;
					}
					if (stiCellStyle != null)
					{
						object obj = hashtable[stiCellStyle];
						if (obj != null && htmlExport.useStylesTable)
						{
							stiHtmlTableCell.CssClass = "s" + stiCellStyle.StyleName;
						}
						if (StiOptions.Export.Html.UseStrictTableCellSize)
						{
							if ((int)stiCellStyle.VertAlignment != 0)
							{
								stiHtmlTableCell.Style["div:vertical-align"] = (((int)stiCellStyle.VertAlignment == 1) ? "middle" : "bottom");
							}
							bool flag2 = stiCellStyle.TextOptions != null && stiCellStyle.TextOptions.RightToLeft;
							string text2 = null;
							if ((int)stiCellStyle.HorAlignment == 0)
							{
								text2 = ((!flag2) ? null : "right");
							}
							if ((int)stiCellStyle.HorAlignment == 2)
							{
								text2 = (flag2 ? null : "right");
							}
							if ((int)stiCellStyle.HorAlignment == 1)
							{
								text2 = "center";
							}
							if ((int)stiCellStyle.HorAlignment == 3)
							{
								text2 = "justify";
							}
							if (text2 != null)
							{
								stiHtmlTableCell.Style["div:text-align"] = text2;
							}
							for (int num9 = 0; num9 < num7; num9++)
							{
								array2[k + num9] = stiCellStyle.Color;
							}
						}
					}
					if (stiCell.Component != null)
					{
						string text3 = stiCell.Component.TagValue as string;
						if (!string.IsNullOrEmpty(text3))
						{
							string[] array3 = matrix.SplitTagWithCache(text3);
							for (int num10 = 0; num10 < array3.Length; num10++)
							{
								if (array3[num10].ToLower().StartsWith("css", StringComparison.InvariantCulture))
								{
									string[] stringsFromTag = StiMatrix.GetStringsFromTag(array3[num10], 3);
									if (stringsFromTag.Length > 1 && htmlExport.useStylesTable)
									{
										stiHtmlTableCell.CssClass = stringsFromTag[0].Trim();
										break;
									}
								}
							}
						}
					}
					if (!htmlExport.useStylesTable)
					{
						RenderStyleTable(stiHtmlTableCell, stiCellStyle);
					}
					else if (StiOptions.Export.Html.UseExtendedStyle)
					{
						htmlExport.RenderTextColor(stiHtmlTableCell, stiCellStyle.TextColor, forceAnyColor: true);
						htmlExport.RenderFont(stiHtmlTableCell, stiCellStyle.Font);
					}
					if (StiOptions.Export.Html.PreserveWhiteSpaces && !string.IsNullOrWhiteSpace(text) && text.Contains("  ") && (stiCellStyle == null || (int)stiCellStyle.HorAlignment != 3))
					{
						stiHtmlTableCell.Style["white-space"] = "pre-wrap";
						text = htmlExport.PrepareTextForHtml(text, processWhiteSpaces: false);
					}
					else
					{
						text = htmlExport.PrepareTextForHtml(text);
					}
					if (StiOptions.Export.Html.PrintLayoutOptimization && stiCellStyle != null && stiCellStyle.Border != (StiBorderSide)null && (int)stiCellStyle.Border.Style != 6)
					{
						num6 -= stiCellStyle.Border.Size;
						num5 -= stiCellStyle.Border.Size;
						if (num6 < 0.0)
						{
							num6 = 0.0;
						}
						if (num5 < 0.0)
						{
							num5 = 0.0;
						}
					}
					if (htmlExport.exportQuality == StiHtmlExportQuality.High)
					{
						if (StiOptions.Export.Html.ForceIE6Compatibility)
						{
							if (num7 == 1)
							{
								stiHtmlTableCell.Width = StiHtmlUnit.NewUnit(num5, StiOptions.Export.Html.PrintLayoutOptimization);
							}
							if (num8 == 1)
							{
								stiHtmlTableCell.Height = StiHtmlUnit.NewUnit(num6, StiOptions.Export.Html.PrintLayoutOptimization);
							}
						}
						else
						{
							if (num7 > 1)
							{
								for (int num11 = 1; num11 < num7; num11++)
								{
									num5 += GetWidth(valueList, k + num11, htmlExport.zoom);
								}
							}
							if (num8 > 1)
							{
								for (int num12 = 1; num12 < num8; num12++)
								{
									num6 += GetHeight(valueList2, i + num12, htmlExport.zoom);
								}
							}
							stiHtmlTableCell.Width = StiHtmlUnit.NewUnit(num5, StiOptions.Export.Html.PrintLayoutOptimization);
							stiHtmlTableCell.Height = StiHtmlUnit.NewUnit(num6, StiOptions.Export.Html.PrintLayoutOptimization);
							if (StiOptions.Export.Html.UseStrictTableCellSizeV2 && text.Length > 3)
							{
								stiHtmlTableCell.Style["max-width"] = stiHtmlTableCell.Width.ToString();
								stiHtmlTableCell.Style["max-height"] = stiHtmlTableCell.Height.ToString();
							}
						}
					}
					if (htmlExport.RenderWebInteractions && stiCell.Component != null && (matrix.IsComponentHasInteraction(stiCell.Component) || matrix.Interactions[i, k, 1] > 0))
					{
						StiComponent stiComponent = stiCell.Component;
						if (!matrix.IsComponentHasInteraction(stiComponent))
						{
							StiPage stiPage = htmlExport.report.RenderedPages[matrix.Interactions[i, k, 0] - 1];
							htmlExport.report.RenderedPages.GetPage(stiPage);
							stiComponent = stiPage.Components[matrix.Interactions[i, k, 1] - 1];
						}
						stiHtmlTableCell.Interaction = stiComponent.Name;
						if (stiComponent.Interaction.SortingEnabled)
						{
							string sortDataBandName = stiComponent.Interaction.GetSortDataBandName();
							if (stiComponent.Report.GetComponentByName(sortDataBandName) is StiDataBand stiDataBand)
							{
								stiHtmlTableCell.DataBandSort = sortDataBandName + ";" + string.Join(";", stiDataBand.Sort);
								int num13 = 0;
								string empty = string.Empty;
								while (num13 < stiDataBand.Sort.Length)
								{
									string text4 = string.Empty;
									empty = stiDataBand.Sort[num13++];
									for (; num13 < stiDataBand.Sort.Length && stiDataBand.Sort[num13] != "ASC" && stiDataBand.Sort[num13] != "DESC"; num13++)
									{
										text4 = ((text4.Length != 0) ? (text4 + "." + stiDataBand.Sort[num13]) : stiDataBand.Sort[num13]);
									}
									if (text4 == stiComponent.Interaction.GetSortColumnsString())
									{
										stiHtmlTableCell.SortDirection = empty.ToLower();
									}
								}
							}
						}
						if (stiComponent.Interaction.DrillDownEnabled && (!string.IsNullOrEmpty(stiComponent.Interaction.DrillDownPageGuid) || !string.IsNullOrEmpty(stiComponent.Interaction.DrillDownReport)))
						{
							if (!string.IsNullOrEmpty(stiComponent.Interaction.DrillDownPageGuid))
							{
								stiHtmlTableCell.PageGuid = stiComponent.Interaction.DrillDownPageGuid;
							}
							if (!string.IsNullOrEmpty(stiComponent.Interaction.DrillDownReport))
							{
								stiHtmlTableCell.ReportFile = stiComponent.Interaction.DrillDownReport;
							}
							stiHtmlTableCell.PageIndex = stiComponent.Page.Report.RenderedPages.IndexOf(stiComponent.Page).ToString();
							stiHtmlTableCell.ComponentIndex = stiComponent.Page.Components.IndexOf(stiComponent).ToString();
							stiHtmlTableCell.DrillDownMode = stiComponent.Interaction.DrillDownMode.ToString();
						}
						if (stiComponent.Interaction is StiBandInteraction { CollapsingEnabled: not false } && stiComponent is StiContainer)
						{
							StiContainer stiContainer = stiComponent as StiContainer;
							stiHtmlTableCell.Collapsed = StiDataBandV2Builder.IsCollapsed(stiContainer, isRendering: false).ToString().ToLower();
							stiHtmlTableCell.ComponentIndex = stiContainer.CollapsingIndex.ToString();
						}
					}
					string text5 = null;
					string toolTip = null;
					string value3 = null;
					string value4 = null;
					bool flag3 = false;
					bool flag4 = false;
					bool flag5 = false;
					bool flag6 = false;
					IStiExportImage stiExportImage = stiCell.ExportImage;
					if (stiCell.Component != null)
					{
						text5 = stiCell.Component.HyperlinkValue as string;
						toolTip = stiCell.Component.ToolTipValue as string;
						value3 = stiCell.Component.BookmarkValue as string;
						if (!string.IsNullOrEmpty(text5))
						{
							text5 = text5.Trim();
							if (text5.StartsWith("javascript:"))
							{
								text5 = null;
							}
						}
						if (!string.IsNullOrWhiteSpace(text5) && text5.StartsWith("##"))
						{
							if (text5.Length > 2 && text5[2] == '#')
							{
								text5 = text5.Substring(2);
								string text6 = (string)matrix.pointerToBookmark[text5.Substring(1)];
								if (text6 != null)
								{
									text5 = "#" + text6;
								}
							}
							else
							{
								text5 = text5.Substring(1);
							}
						}
						if (string.IsNullOrEmpty(value3))
						{
							for (int num14 = 0; num14 <= stiCell.Height; num14++)
							{
								bool flag7 = false;
								for (int num15 = 0; num15 <= stiCell.Width; num15++)
								{
									string text7 = matrix.Bookmarks[i + num14, k + num15];
									if (!string.IsNullOrEmpty(text7))
									{
										value3 = text7;
										flag7 = true;
										break;
									}
								}
								if (flag7)
								{
									break;
								}
							}
						}
						if (!string.IsNullOrWhiteSpace(stiCell.Component.Guid) && htmlExport.hashBookmarkGuid.ContainsKey(stiCell.Component.Guid))
						{
							value4 = stiCell.Component.Guid;
						}
						if (StiOptions.Export.Html.AllowStrippedImages && stiExportImage == null && stiCell.Component is IStiExportImage stiExportImage2 && stiExportImage2 is IStiExportImageExtended && ((IStiExportImageExtended)stiExportImage2).IsExportAsImage(StiExportFormat.HtmlTable) && (stiTextOptions == null || stiTextOptions.TextOptions.Angle == 0f))
						{
							stiExportImage = stiExportImage2;
						}
						if (stiCell.Component is StiChart && htmlExport.chartType != StiHtmlChartType.Image)
						{
							flag3 = true;
						}
						if (stiCell.Component is StiSparkline && htmlExport.chartType != StiHtmlChartType.Image)
						{
							flag4 = true;
						}
						if (stiCell.Component is StiGauge && htmlExport.chartType != StiHtmlChartType.Image)
						{
							flag5 = true;
						}
						if (stiCell.Component is StiMap stiMap && htmlExport.chartType != StiHtmlChartType.Image && stiMap.MapMode != StiMapMode.Online)
						{
							flag6 = true;
						}
						if (stiCell.Component is IStiEditable { Editable: not false })
						{
							StringBuilder stringBuilder2 = new StringBuilder();
							int num16 = stiCell.Component.Page.Components.IndexOf(stiCell.Component);
							stringBuilder2.AppendFormat("{0};", num16);
							if (stiCell.Component is StiCheckBox stiCheckBox)
							{
								Color color = Color.Transparent;
								if (stiCheckBox.TextBrush is StiSolidBrush)
								{
									color = ((StiSolidBrush)stiCheckBox.TextBrush).Color;
								}
								else if (stiCheckBox.TextBrush is StiGradientBrush)
								{
									color = ((StiGradientBrush)stiCheckBox.TextBrush).StartColor;
								}
								else if (stiCheckBox.TextBrush is StiGlareBrush)
								{
									color = ((StiGlareBrush)stiCheckBox.TextBrush).StartColor;
								}
								else if (stiCheckBox.TextBrush is StiGlassBrush)
								{
									color = ((StiGlassBrush)stiCheckBox.TextBrush).Color;
								}
								else if (stiCheckBox.TextBrush is StiHatchBrush)
								{
									color = ((StiHatchBrush)stiCheckBox.TextBrush).ForeColor;
								}
								stringBuilder2.AppendFormat("CheckBox;{0};{1};{2};#{3:X2}{4:X2}{5:X2};{6};#{7:X2}{8:X2}{9:X2}", stiCheckBox.CheckedValue, stiCheckBox.CheckStyleForFalse.ToString(), stiCheckBox.CheckStyleForTrue.ToString(), stiCheckBox.ContourColor.R, stiCheckBox.ContourColor.G, stiCheckBox.ContourColor.B, stiCheckBox.Size, color.R, color.G, color.B);
							}
							if (stiText != null)
							{
								stringBuilder2.AppendFormat("Text");
								if (stiText.CheckAllowHtmlTags())
								{
									stringBuilder2.AppendFormat(";html");
								}
							}
							if (stiCell.Component is StiRichText)
							{
								stringBuilder2.AppendFormat("RichText");
							}
							stiHtmlTableCell.Editable = stringBuilder2.ToString();
						}
						if (stiExportImage == null)
						{
							StiGradientBrush val2 = null;
							StiGlareBrush val3 = null;
							StiGlassBrush val4 = null;
							if (stiText != null)
							{
								if (stiText.Brush is StiGradientBrush)
								{
									StiBrush brush = stiText.Brush;
									val2 = (StiGradientBrush)(object)((brush is StiGradientBrush) ? brush : null);
								}
								if (stiText.Brush is StiGlareBrush)
								{
									StiBrush brush2 = stiText.Brush;
									val3 = (StiGlareBrush)(object)((brush2 is StiGlareBrush) ? brush2 : null);
								}
								if (stiText.Brush is StiGlassBrush)
								{
									StiBrush brush3 = stiText.Brush;
									val4 = (StiGlassBrush)(object)((brush3 is StiGlassBrush) ? brush3 : null);
								}
							}
							if (stiCell.Component is StiContainer stiContainer2)
							{
								if (stiContainer2.Brush is StiGradientBrush)
								{
									StiBrush brush4 = stiContainer2.Brush;
									val2 = (StiGradientBrush)(object)((brush4 is StiGradientBrush) ? brush4 : null);
								}
								if (stiContainer2.Brush is StiGlareBrush)
								{
									StiBrush brush5 = stiContainer2.Brush;
									val3 = (StiGlareBrush)(object)((brush5 is StiGlareBrush) ? brush5 : null);
								}
								if (stiContainer2.Brush is StiGlassBrush)
								{
									StiBrush brush6 = stiContainer2.Brush;
									val4 = (StiGlassBrush)(object)((brush6 is StiGlassBrush) ? brush6 : null);
								}
							}
							if (val2 != null)
							{
								string text8 = $"{Math.Round(val2.Angle) + 90.0}deg";
								stiHtmlTableCell.Style["background"] = "linear-gradient(" + text8 + ", " + htmlExport.FormatColor(val2.StartColor) + ", " + htmlExport.FormatColor(val2.EndColor) + ")";
							}
							if (val3 != null)
							{
								string text9 = $"{Math.Round(val3.Angle) + 90.0}deg";
								stiHtmlTableCell.Style["background"] = $"linear-gradient({text9}, {htmlExport.FormatColor(val3.StartColor)}, {htmlExport.FormatColor(val3.EndColor)}  {Math.Round(val3.Focus * 100f)}%, {htmlExport.FormatColor(val3.StartColor)})";
							}
							if (val4 != null)
							{
								stiHtmlTableCell.Style["background"] = "linear-gradient(" + htmlExport.FormatColor(val4.GetTopColor()) + ", " + htmlExport.FormatColor(val4.GetTopColor()) + " 49%, " + htmlExport.FormatColor(val4.GetBottomColor()) + " 50%, " + htmlExport.FormatColor(val4.GetBottomColor()) + ");";
							}
						}
					}
					bool flag8 = true;
					if (!string.IsNullOrWhiteSpace(value3) || !string.IsNullOrEmpty(value4))
					{
						StiHtmlHyperlink stiHtmlHyperlink = new StiHtmlHyperlink();
						if (!string.IsNullOrWhiteSpace(value3))
						{
							stiHtmlHyperlink.Attributes["name"] = value3;
						}
						if (!string.IsNullOrEmpty(value4))
						{
							stiHtmlHyperlink.Attributes["guid"] = value4;
						}
						stiHtmlHyperlink.ToolTip = toolTip;
						stiHtmlHyperlink.NavigateUrl = text5;
						stiHtmlHyperlink.OpenLinksTarget = htmlExport.openLinksTarget;
						stiHtmlTableCell.ToolTip = toolTip;
						if (flag3)
						{
							stiHtmlTableCell.Id = htmlExport.GetGuid(stiCell.Component);
							stiHtmlTableCell.Text = htmlExport.PrepareChartData(null, stiCell.Component as StiChart, num5, num6);
						}
						else if (flag5)
						{
							stiHtmlTableCell.Id = htmlExport.GetGuid(stiCell.Component);
							stiHtmlTableCell.Text = htmlExport.PrepareGaugeData(null, stiCell.Component as StiGauge, num5, num6);
						}
						else if (flag6)
						{
							stiHtmlTableCell.Id = htmlExport.GetGuid(stiCell.Component);
							stiHtmlTableCell.Text = htmlExport.PrepareMapData(null, stiCell.Component as StiMap, num5, num6);
						}
						else if (flag4)
						{
							stiHtmlTableCell.Id = htmlExport.GetGuid(stiCell.Component);
							stiHtmlTableCell.Text = htmlExport.PrepareSparklineData(null, stiCell.Component as StiSparkline, num5, num6);
						}
						else if (stiExportImage != null)
						{
							float zoom = (float)htmlExport.zoom;
							float num17 = htmlExport.imageResolution;
							StiImage stiImage = stiExportImage as StiImage;
							if (StiOptions.Export.Html.UseImageResolution && stiImage != null && stiImage.ExistImageToDraw())
							{
								Image val5 = stiImage.TakeGdiImageToDraw();
								try
								{
									if (val5 != null)
									{
										float horizontalResolution = val5.HorizontalResolution;
										if (horizontalResolution >= 50f && horizontalResolution <= 1250f)
										{
											num17 = horizontalResolution;
										}
									}
								}
								finally
								{
									((IDisposable)val5)?.Dispose();
								}
							}
							if (num17 != 100f)
							{
								zoom *= num17 / 100f;
							}
							Image val6 = null;
							htmlExport.SetCurrentCulture();
							val6 = ((!(stiExportImage is IStiExportImageExtended stiExportImageExtended) || !stiExportImageExtended.IsExportAsImage(StiExportFormat.HtmlTable)) ? stiExportImage.GetImage(ref zoom) : stiExportImageExtended.GetImage(ref zoom, StiExportFormat.HtmlTable));
							htmlExport.RestoreCulture();
							if (val6 != null)
							{
								Image realImageData = matrix.GetRealImageData(stiCell, val6);
								if (realImageData != null)
								{
									val6 = realImageData;
								}
								if (htmlExport.HtmlImageHost != null)
								{
									stiHtmlHyperlink.ImageUrl = htmlExport.HtmlImageHost.GetImageString((Bitmap)(object)((val6 is Bitmap) ? val6 : null));
								}
								stiHtmlHyperlink.Width = StiHtmlUnit.NewUnit((double)((float)val6.Width / zoom) * htmlExport.zoom, StiOptions.Export.Html.PrintLayoutOptimization);
								stiHtmlHyperlink.Height = StiHtmlUnit.NewUnit((double)((float)val6.Height / zoom) * htmlExport.zoom, StiOptions.Export.Html.PrintLayoutOptimization);
								val6.Dispose();
								flag8 = false;
							}
						}
						if (flag8 && !flag3)
						{
							stiHtmlHyperlink.Text = text;
							if (stiCell.Component != null && stiCell.Component is IStiTextBrush)
							{
								IStiTextBrush stiTextBrush = stiCell.Component as IStiTextBrush;
								Color color2 = StiBrush.ToColor(stiTextBrush.TextBrush);
								stiHtmlHyperlink.Style["color"] = htmlExport.FormatColor(color2);
							}
							if (stiCell.Component != null && stiCell.Component is IStiFont)
							{
								IStiFont stiFont = stiCell.Component as IStiFont;
								if (stiFont.Font.Underline)
								{
									stiHtmlHyperlink.Style["text-decoration"] = "underline";
								}
								else
								{
									stiHtmlHyperlink.Style["text-decoration"] = "none";
								}
							}
						}
						stiHtmlTableCell.Controls.Add(stiHtmlHyperlink);
					}
					else if (flag3)
					{
						if (!string.IsNullOrEmpty(text5))
						{
							StiHtmlHyperlink stiHtmlHyperlink2 = new StiHtmlHyperlink();
							stiHtmlHyperlink2.ToolTip = toolTip;
							stiHtmlHyperlink2.NavigateUrl = text5;
							stiHtmlHyperlink2.OpenLinksTarget = htmlExport.openLinksTarget;
							stiHtmlHyperlink2.Style["display"] = "block";
							stiHtmlTableCell.Controls.Add(stiHtmlHyperlink2);
						}
						else
						{
							stiHtmlTableCell.ToolTip = toolTip;
						}
						stiHtmlTableCell.Id = htmlExport.GetGuid(stiCell.Component);
						stiHtmlTableCell.Text = htmlExport.PrepareChartData(null, stiCell.Component as StiChart, num5, num6);
					}
					else if (flag4)
					{
						if (!string.IsNullOrEmpty(text5))
						{
							StiHtmlHyperlink stiHtmlHyperlink3 = new StiHtmlHyperlink();
							stiHtmlHyperlink3.ToolTip = toolTip;
							stiHtmlHyperlink3.NavigateUrl = text5;
							stiHtmlHyperlink3.OpenLinksTarget = htmlExport.openLinksTarget;
							stiHtmlHyperlink3.Style["display"] = "block";
							stiHtmlTableCell.Controls.Add(stiHtmlHyperlink3);
						}
						else
						{
							stiHtmlTableCell.ToolTip = toolTip;
						}
						stiHtmlTableCell.Id = htmlExport.GetGuid(stiCell.Component);
						stiHtmlTableCell.Text = htmlExport.PrepareSparklineData(null, stiCell.Component as StiSparkline, num5, num6);
					}
					else if (flag5)
					{
						if (!string.IsNullOrEmpty(text5))
						{
							StiHtmlHyperlink stiHtmlHyperlink4 = new StiHtmlHyperlink();
							stiHtmlHyperlink4.ToolTip = toolTip;
							stiHtmlHyperlink4.NavigateUrl = text5;
							stiHtmlHyperlink4.OpenLinksTarget = htmlExport.openLinksTarget;
							stiHtmlHyperlink4.Style["display"] = "block";
							stiHtmlTableCell.Controls.Add(stiHtmlHyperlink4);
						}
						else
						{
							stiHtmlTableCell.ToolTip = toolTip;
						}
						stiHtmlTableCell.Id = htmlExport.GetGuid(stiCell.Component);
						stiHtmlTableCell.Text = htmlExport.PrepareGaugeData(null, stiCell.Component as StiGauge, num5, num6);
					}
					else if (flag6)
					{
						stiHtmlTableCell.Id = htmlExport.GetGuid(stiCell.Component);
						stiHtmlTableCell.Text = htmlExport.PrepareMapData(null, stiCell.Component as StiMap, num5, num6);
					}
					else if (stiExportImage == null)
					{
						if (text5 != null && text5.Length > 0)
						{
							StiHtmlHyperlink stiHtmlHyperlink5 = new StiHtmlHyperlink();
							stiHtmlHyperlink5.Text = text;
							stiHtmlHyperlink5.ToolTip = toolTip;
							stiHtmlHyperlink5.NavigateUrl = text5;
							stiHtmlHyperlink5.OpenLinksTarget = htmlExport.openLinksTarget;
							stiHtmlHyperlink5.Style["display"] = "block";
							if (stiCell.Component != null && stiCell.Component is IStiTextBrush)
							{
								IStiTextBrush stiTextBrush2 = stiCell.Component as IStiTextBrush;
								Color color3 = StiBrush.ToColor(stiTextBrush2.TextBrush);
								stiHtmlHyperlink5.Style["color"] = htmlExport.FormatColor(color3);
							}
							if (stiCell.Component != null && stiCell.Component is IStiFont)
							{
								IStiFont stiFont2 = stiCell.Component as IStiFont;
								if (stiFont2.Font.Underline)
								{
									stiHtmlHyperlink5.Style["text-decoration"] = "underline";
								}
								else
								{
									stiHtmlHyperlink5.Style["text-decoration"] = "none";
								}
							}
							stiHtmlTableCell.Controls.Add(stiHtmlHyperlink5);
						}
						else
						{
							if (stiText != null && stiText.TextOptions != null && stiText.Angle != 0f)
							{
								string text10 = StiSvgHelper.SaveComponentToString(stiText);
								string text11 = $"<svg width=\"{Math.Ceiling(num5)}\" height=\"{Math.Ceiling(num6)}\"><g transform=\"scale({htmlExport.zoom})\">{text10}</g></svg>";
								stiHtmlTableCell.Text = text11;
								stiHtmlTableCell.Style["line-height"] = "0";
							}
							else
							{
								stiHtmlTableCell.Text = text;
							}
							stiHtmlTableCell.ToolTip = toolTip;
						}
					}
					else
					{
						float zoom2 = (float)htmlExport.zoom;
						if (StiSvgHelper.CheckShape(stiCell.Component))
						{
							StiShape stiShape = stiCell.Component as StiShape;
							RectangleD val7 = stiShape.ComponentToPage(stiShape.ClientRectangle);
							RectangleD rect = ((RectangleD)(ref val7)).Normalize();
							val7 = stiShape.Report.Unit.ConvertToHInches(rect);
							rect = ((RectangleD)(ref val7)).Multiply((double)zoom2);
							((RectangleD)(ref rect)).X = 0.0;
							((RectangleD)(ref rect)).Y = 0.0;
							int num18 = (int)(((RectangleD)(ref rect)).Width + 0.5) + 1 + (int)(stiShape.Size * zoom2);
							int num19 = (int)(((RectangleD)(ref rect)).Height + 0.5) + 1 + (int)(stiShape.Size * zoom2);
							int num20 = (int)Math.Round(stiShape.Size * zoom2 / 2f);
							StiHtmlImage stiHtmlImage = new StiHtmlImage();
							stiHtmlImage.Zoom = zoom2;
							stiHtmlImage.ToolTip = toolTip;
							stiHtmlImage.Margin = StiHtmlUnit.NewUnit(-num20);
							stiHtmlImage.Width = StiHtmlUnit.NewUnit((float)num18 + stiShape.Size * zoom2, usePoints: false);
							stiHtmlImage.Height = StiHtmlUnit.NewUnit((float)num19 + stiShape.Size * zoom2, usePoints: false);
							stiHtmlImage.ImageUrl = StiSvgHelper.RenderShapeAsBase64(stiShape);
							stiHtmlTableCell.Style.Add("overflow", "visible");
							stiHtmlTableCell.Controls.Add(stiHtmlImage);
						}
						else
						{
							float num21 = htmlExport.imageResolution;
							StiImage stiImage2 = stiExportImage as StiImage;
							if (StiOptions.Export.Html.UseImageResolution && stiImage2 != null && stiImage2.ExistImageToDraw())
							{
								Image val8 = stiImage2.TakeGdiImageToDraw();
								try
								{
									if (val8 != null)
									{
										float horizontalResolution2 = val8.HorizontalResolution;
										if (horizontalResolution2 >= 50f && horizontalResolution2 <= 1250f)
										{
											num21 = horizontalResolution2;
										}
									}
								}
								finally
								{
									((IDisposable)val8)?.Dispose();
								}
							}
							if (num21 != 100f)
							{
								zoom2 *= num21 / 100f;
							}
							Image val9 = null;
							htmlExport.SetCurrentCulture();
							val9 = ((!(stiExportImage is IStiExportImageExtended stiExportImageExtended2) || !stiExportImageExtended2.IsExportAsImage(StiExportFormat.HtmlTable)) ? stiExportImage.GetImage(ref zoom2) : stiExportImageExtended2.GetImage(ref zoom2, StiExportFormat.HtmlTable));
							htmlExport.RestoreCulture();
							if (val9 != null)
							{
								Image val10 = null;
								if (val10 != null)
								{
									val9 = val10;
								}
								if (text5 != null && text5.Length > 0)
								{
									StiHtmlHyperlink stiHtmlHyperlink6 = new StiHtmlHyperlink();
									stiHtmlHyperlink6.NavigateUrl = text5;
									stiHtmlHyperlink6.ToolTip = toolTip;
									if (htmlExport.HtmlImageHost != null)
									{
										stiHtmlHyperlink6.ImageUrl = htmlExport.HtmlImageHost.GetImageString((Bitmap)(object)((val9 is Bitmap) ? val9 : null));
									}
									stiHtmlHyperlink6.Width = StiHtmlUnit.NewUnit((double)((float)val9.Width / zoom2) * htmlExport.zoom, StiOptions.Export.Html.PrintLayoutOptimization);
									stiHtmlHyperlink6.Height = StiHtmlUnit.NewUnit((double)((float)val9.Height / zoom2) * htmlExport.zoom, StiOptions.Export.Html.PrintLayoutOptimization);
									stiHtmlHyperlink6.OpenLinksTarget = htmlExport.openLinksTarget;
									stiHtmlHyperlink6.ImageComp = stiCell.Component as StiImage;
									stiHtmlHyperlink6.Zoom = zoom2;
									stiHtmlTableCell.Controls.Add(stiHtmlHyperlink6);
								}
								else
								{
									string text12 = null;
									if (stiCell.Component != null && stiCell.Component is StiImage)
									{
										StiImage stiImage3 = stiCell.Component as StiImage;
										if (stiImage3.Stretch && !stiImage3.AspectRatio && stiImage3.ImageRotation == StiImageRotation.None)
										{
											string text13 = stiImage3.ImageURLValue as string;
											if (!StiHyperlinkProcessor.IsResourceHyperlink(text13))
											{
												text12 = text13;
											}
										}
									}
									StiHtmlImage stiHtmlImage2 = new StiHtmlImage();
									stiHtmlImage2.ImageComp = stiImage2;
									stiHtmlImage2.Zoom = zoom2;
									if (text12 != null && text12.Length != 0)
									{
										stiHtmlImage2.ImageUrl = text12;
									}
									else if (htmlExport.HtmlImageHost != null)
									{
										stiHtmlImage2.ImageUrl = htmlExport.HtmlImageHost.GetImageString((Bitmap)(object)((val9 is Bitmap) ? val9 : null));
									}
									stiHtmlImage2.ToolTip = toolTip;
									if (!(stiCell.Component is StiShape stiShape2))
									{
										stiHtmlImage2.Width = StiHtmlUnit.NewUnit((double)((float)val9.Width / zoom2) * htmlExport.zoom, usePoints: false);
										stiHtmlImage2.Height = StiHtmlUnit.NewUnit((double)((float)val9.Height / zoom2) * htmlExport.zoom, usePoints: false);
										stiHtmlImage2.UseImageSize = StiOptions.Engine.BarcodeImproveQualityHtmlExport && stiExportImage is StiBarCode;
									}
									else
									{
										int num22 = (int)Math.Round(stiShape2.Size * zoom2 / 2f);
										stiHtmlImage2.Margin = StiHtmlUnit.NewUnit(-num22);
										stiHtmlImage2.Width = StiHtmlUnit.NewUnit((double)(((float)val9.Width + stiShape2.Size * zoom2) / zoom2) * htmlExport.zoom, usePoints: false);
										stiHtmlImage2.Height = StiHtmlUnit.NewUnit((double)(((float)val9.Height + stiShape2.Size * zoom2) / zoom2) * htmlExport.zoom, usePoints: false);
										stiHtmlTableCell.Style.Add("overflow", "visible");
									}
									stiHtmlTableCell.Controls.Add(stiHtmlImage2);
								}
								val9.Dispose();
								flag8 = false;
							}
						}
					}
					if (stiText != null && !stiText.Margins.IsEmpty && flag8)
					{
						stiHtmlTableCell.Style["padding"] = $"{StiHtmlUnit.NewUnit(Math.Truncate(stiText.Margins.Top * htmlExport.zoom))} {StiHtmlUnit.NewUnit(Math.Truncate(stiText.Margins.Right * htmlExport.zoom))} {StiHtmlUnit.NewUnit(Math.Truncate(stiText.Margins.Bottom * htmlExport.zoom))} {StiHtmlUnit.NewUnit(Math.Truncate(stiText.Margins.Left * htmlExport.zoom))}";
						if (stiHtmlTableCell.Width != null && stiText.Margins.Left + stiText.Margins.Right > 0.0)
						{
							stiHtmlTableCell.Width.Value -= Math.Truncate(stiText.Margins.Left * htmlExport.zoom) + Math.Truncate(stiText.Margins.Right * htmlExport.zoom);
						}
						if (stiHtmlTableCell.Height != null && stiText.Margins.Top + stiText.Margins.Bottom > 0.0)
						{
							stiHtmlTableCell.Height.Value -= Math.Truncate(stiText.Margins.Top * htmlExport.zoom) + Math.Truncate(stiText.Margins.Bottom * htmlExport.zoom);
						}
					}
					if (stiTextOptions != null && stiTextOptions.TextOptions.WordWrap && stiCellStyle != null && ((int)stiCellStyle.VertAlignment != 0 || (int)stiCellStyle.HorAlignment != 0))
					{
						stiHtmlTableCell.Style["div:wordwrap"] = null;
					}
					if (value == null)
					{
						continue;
					}
					foreach (StiMatrix.RoundedRectangleInfo item in value)
					{
						if (item.Rect.Left != k)
						{
							continue;
						}
						double num23 = num5;
						double num24 = num6;
						bool flag9 = item.Rect.Width == stiCell.Width + 1 && item.Rect.Height == stiCell.Height + 1;
						if (!flag9)
						{
							if (item.Rect.Width > 1)
							{
								for (int num25 = 1; num25 < item.Rect.Width; num25++)
								{
									num23 += GetWidth(valueList, k + num25, htmlExport.zoom);
								}
							}
							if (item.Rect.Height > 1)
							{
								for (int num26 = 1; num26 < item.Rect.Height; num26++)
								{
									num24 += GetHeight(valueList2, i + num26, htmlExport.zoom);
								}
							}
						}
						int num27 = (int)Math.Round(Math.Min((num24 < num23) ? num24 : num23, 100.0) * (double)item.Primitive.Round * 1.04);
						StiHtmlRoundedRectangle stiHtmlRoundedRectangle = new StiHtmlRoundedRectangle();
						stiHtmlRoundedRectangle.Width = num23;
						stiHtmlRoundedRectangle.Height = num24;
						stiHtmlRoundedRectangle.FitToCell = flag9;
						stiHtmlRoundedRectangle.Style = htmlExport.GetBorderStyle(item.Primitive.Style);
						stiHtmlRoundedRectangle.Size = Math.Round(item.Primitive.Size, 2);
						stiHtmlRoundedRectangle.Color = htmlExport.FormatColor(item.Primitive.Color);
						stiHtmlRoundedRectangle.Round = num27.ToString();
						stiHtmlTableCell.Controls.Add(stiHtmlRoundedRectangle);
						if ((int)stiCellStyle.VertAlignment != 0)
						{
							stiHtmlTableCell.Style["div:vertical-align"] = (((int)stiCellStyle.VertAlignment == 1) ? "middle" : "bottom");
						}
						bool flag10 = stiCellStyle.TextOptions != null && stiCellStyle.TextOptions.RightToLeft;
						string text14 = null;
						if ((int)stiCellStyle.HorAlignment == 0)
						{
							text14 = ((!flag10) ? null : "right");
						}
						if ((int)stiCellStyle.HorAlignment == 2)
						{
							text14 = (flag10 ? null : "right");
						}
						if ((int)stiCellStyle.HorAlignment == 1)
						{
							text14 = "center";
						}
						if ((int)stiCellStyle.HorAlignment == 3)
						{
							text14 = "justify";
						}
						if (text14 != null)
						{
							stiHtmlTableCell.Style["div:text-align"] = text14;
						}
					}
					continue;
				}
				if (StiOptions.Export.Html.PrintLayoutOptimization && num4 > 0.0)
				{
					num6 -= num4;
					if (num6 < 0.0)
					{
						num6 = 0.0;
					}
				}
				if (htmlExport.exportQuality == StiHtmlExportQuality.High)
				{
					stiHtmlTableCell.Width = StiHtmlUnit.NewUnit(num5, StiOptions.Export.Html.PrintLayoutOptimization);
					stiHtmlTableCell.Height = StiHtmlUnit.NewUnit(num6, StiOptions.Export.Html.PrintLayoutOptimization);
				}
				StiCellStyle stiCellStyle2 = matrix.CellStyles[i, k];
				if (stiCellStyle2 != null)
				{
					object obj2 = hashtable[stiCellStyle2];
					if (obj2 != null && htmlExport.useStylesTable)
					{
						stiHtmlTableCell.CssClass = "s" + stiCellStyle2.StyleName;
					}
				}
				if (string.IsNullOrEmpty(stiHtmlTableCell.CssClass))
				{
					stiHtmlTableCell.Style["border"] = "0px";
				}
				if (value == null)
				{
					continue;
				}
				foreach (StiMatrix.RoundedRectangleInfo item2 in value)
				{
					if (item2.Rect.Left != k)
					{
						continue;
					}
					if (item2.Rect.Width > 1)
					{
						for (int num28 = 1; num28 < item2.Rect.Width; num28++)
						{
							num5 += GetWidth(valueList, k + num28, htmlExport.zoom);
						}
					}
					if (item2.Rect.Height > 1)
					{
						for (int num29 = 1; num29 < item2.Rect.Height; num29++)
						{
							num6 += GetHeight(valueList2, i + num29, htmlExport.zoom);
						}
					}
					int num30 = (int)Math.Round(Math.Min((num6 < num5) ? num6 : num5, 100.0) * (double)item2.Primitive.Round * 1.04);
					StiHtmlRoundedRectangle stiHtmlRoundedRectangle2 = new StiHtmlRoundedRectangle();
					stiHtmlRoundedRectangle2.Width = num5;
					stiHtmlRoundedRectangle2.Height = num6;
					stiHtmlRoundedRectangle2.Style = htmlExport.GetBorderStyle(item2.Primitive.Style);
					stiHtmlRoundedRectangle2.Size = Math.Round(item2.Primitive.Size, 2);
					stiHtmlRoundedRectangle2.Color = htmlExport.FormatColor(item2.Primitive.Color);
					stiHtmlRoundedRectangle2.Round = num30.ToString();
					stiHtmlTableCell.Controls.Add(stiHtmlRoundedRectangle2);
				}
			}
			if (!StiOptions.Export.Html.UseStrictTableCellSize || array2[0].A == 0)
			{
				continue;
			}
			bool flag11 = true;
			for (int num31 = 0; num31 < array2.Length - 1; num31++)
			{
				if (!array2[num31].Equals((object?)array2[num31 + 1]))
				{
					flag11 = false;
					break;
				}
			}
			if (flag11)
			{
				stiHtmlTableRow.Style["background-color"] = htmlExport.FormatColor(array2[0]);
			}
		}
		stiHtmlTable.RenderControl(htmlExport.HtmlWriter, htmlExportSettings.AddPageBreaks);
	}

	internal StiHtmlTableRender(StiHtmlExportService htmlExport, StiHtmlExportSettings htmlExportSettings, StiPagesCollection pages)
	{
		matrix = new StiMatrix(pages, htmlExport, htmlExport.Styles);
		this.htmlExport = htmlExport;
		this.htmlExportSettings = htmlExportSettings;
	}
}
