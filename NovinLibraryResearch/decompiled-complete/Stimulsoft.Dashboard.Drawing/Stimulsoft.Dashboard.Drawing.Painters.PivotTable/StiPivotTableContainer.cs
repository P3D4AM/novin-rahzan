using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.PivotTable;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.CrossTab.Core;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Drawing.Painters.PivotTable;

public class StiPivotTableContainer : IStiPivotGridContainer
{
	internal StiPivotTableCell MouseEnterCell { get; set; }

	internal List<StiPivotTableRow> Rows { get; set; }

	internal List<StiPivotTableColumn> Columns { get; set; }

	internal int MaxRowLevel { get; set; }

	internal int MaxColLevel { get; set; }

	internal StiReport Report { get; set; }

	internal StiPivotTableCell SelectedCell { get; set; }

	internal StiPivotTableElement PivotTable { get; set; }

	internal StiPainter Painter { get; set; }

	public Control Control { get; set; }

	public double Width { get; set; }

	public double Height { get; set; }

	public int HeaderHeight { get; set; }

	public int HeaderWidth { get; set; }

	public bool OnlySummaries { get; set; }

	public event EventHandler AfterSizeUpdate;

	public void Clear()
	{
		Rows = new List<StiPivotTableRow>();
		Columns = new List<StiPivotTableColumn>();
		SelectedCell = null;
		UpdateSize();
	}

	public void MakeGrid(StiCross cross, StiPivotTableElement pivot)
	{
		Rows = new List<StiPivotTableRow>();
		Columns = new List<StiPivotTableColumn>();
		SelectedCell = null;
		PivotTable = pivot;
		OnlySummaries = pivot.Rows.Count == 0 && pivot.Columns.Count == 0 && pivot.Summaries.Count > 0;
		StiPivotElementStyle pivotTableStyle = StiDashboardStyleHelper.GetPivotTableStyle(pivot);
		StiCell[][] cells = cross.Cells;
		Report = cross.Report;
		Report.Info.Zoom = 1.0;
		decimal[] widths = cross.Widths;
		foreach (decimal num in widths)
		{
			Columns.Add(new StiPivotTableColumn(Report.Unit.ConvertToHInches((double)num * StiScale.Factor)));
		}
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		Hashtable hashtable3 = new Hashtable();
		StiPage stiPage = null;
		if (cells.Length != 0)
		{
			for (int j = 0; j < cells[0].Length; j++)
			{
				Rows.Add(new StiPivotTableRow(this, Report.Unit.ConvertToHInches((double)cross.Heights[j] * StiScale.Factor)));
			}
			for (int k = 0; k < cells.Length; k++)
			{
				for (int l = 0; l < cells[k].Length; l++)
				{
					StiPivotTableCell stiPivotTableCell = new StiPivotTableCell(this);
					StiCell stiCell = cells[k][l];
					stiPivotTableCell.RowSpan = stiCell.Height;
					stiPivotTableCell.ColSpan = stiCell.Width;
					stiPivotTableCell.Width = Columns[k].Width;
					stiPivotTableCell.Height = Rows[l].Height;
					stiPivotTableCell.Row = l;
					stiPivotTableCell.Column = k;
					stiPivotTableCell.Level = stiCell.Level;
					stiPivotTableCell.IsHeader = !stiCell.IsCrossSummary && !(stiCell.Field is StiCrossSummary) && ((!(stiCell.Field is StiCrossRowTotal) && !(stiCell.Field is StiCrossColumnTotal)) || stiCell.Guid != null);
					if (!stiPivotTableCell.IsHeader && !OnlySummaries)
					{
						stiCell.Field.Border = new StiBorder(StiBorderSides.All, pivotTableStyle.LineColor, 1.0, StiPenStyle.Solid);
					}
					if (stiCell.Field != null)
					{
						stiPivotTableCell.IsTotal = stiCell.Field is StiCrossTotal && stiCell.ParentCell == stiCell;
						stiPivotTableCell.Field = ((!StiOptions.Engine.CrossTab.MemoryOptimization) ? ((StiCrossField)stiCell.Field.Clone(cloneProperties: false)) : (stiPivotTableCell.IsHeader ? ((StiCrossField)stiCell.Field.Clone(cloneProperties: false)) : stiCell.Field));
						stiPivotTableCell.Field.Parent = null;
						if (!StiOptions.Engine.CrossTab.MemoryOptimization)
						{
							stiPivotTableCell.Field.SetTextInternal(stiCell.Text);
						}
						stiPivotTableCell.Text = stiCell.Text;
						if (stiPage == null && stiPivotTableCell.Field.Page != null)
						{
							stiPage = stiPivotTableCell.Field.Page;
						}
						if (stiPivotTableCell.Field.Page == null)
						{
							stiPivotTableCell.Field.Page = stiPage;
						}
					}
					if (stiCell.ParentCell == stiCell)
					{
						hashtable.Add(stiCell, stiPivotTableCell);
						if (!string.IsNullOrEmpty(stiCell.Guid))
						{
							hashtable2.Add(stiCell.Guid, stiPivotTableCell);
							stiPivotTableCell.IsColumn = stiCell.Field is StiCrossColumn;
							stiPivotTableCell.MinimalTextSize = GetCellSize(stiCell);
						}
					}
					else if (stiCell.ParentCell != null)
					{
						stiPivotTableCell.Parent = hashtable[stiCell.ParentCell] as StiPivotTableCell;
					}
					if (!string.IsNullOrEmpty(stiCell.ParentGuid) && (!(stiCell.Field is StiCrossTotal) || stiCell.ParentCell == stiCell))
					{
						if (hashtable3[stiCell.ParentGuid] == null)
						{
							hashtable3.Add(stiCell.ParentGuid, new List<StiPivotTableCell>());
						}
						(hashtable3[stiCell.ParentGuid] as List<StiPivotTableCell>).Add(stiPivotTableCell);
					}
					stiPivotTableCell.ParentGuid = stiCell.ParentGuid;
					Rows[l].Cells.Add(stiPivotTableCell);
				}
			}
		}
		foreach (string key3 in hashtable2.Keys)
		{
			StiPivotTableCell stiPivotTableCell2 = hashtable2[key3] as StiPivotTableCell;
			if (hashtable3[key3] != null)
			{
				stiPivotTableCell2.DependedCells = new List<StiPivotTableCell>();
				foreach (StiPivotTableCell item in hashtable3[key3] as List<StiPivotTableCell>)
				{
					if (item.IsTotal)
					{
						stiPivotTableCell2.TotalCell = item;
					}
					else
					{
						stiPivotTableCell2.DependedCells.Add(item);
					}
					if (stiPivotTableCell2.IsColumn)
					{
						MaxColLevel = Math.Max(MaxColLevel, item.Level);
					}
					else
					{
						MaxRowLevel = Math.Max(MaxRowLevel, item.Level);
					}
				}
			}
			else
			{
				stiPivotTableCell2.Field.Margins.Left = 0.0;
			}
		}
		foreach (string key4 in hashtable2.Keys)
		{
			StiPivotTableCell stiPivotTableCell3 = hashtable2[key4] as StiPivotTableCell;
			if (hashtable3[key4] != null && stiPivotTableCell3.TotalCell != null)
			{
				stiPivotTableCell3.ChangeTotal(open: false, updateSize: false);
			}
		}
		HeaderHeight = pivot.Columns.Count + ((pivot.Rows.Count > 0) ? 1 : 0) + ((pivot.Summaries.Count > 1 && pivot.SummaryDirection == StiSummaryDirection.LeftToRight) ? 1 : 0);
		HeaderWidth = ((pivot.Rows.Count == 0) ? 1 : pivot.Rows.Count) + ((pivot.Summaries.Count > 1 && pivot.SummaryDirection == StiSummaryDirection.UpToDown) ? 1 : 0);
		if (!OnlySummaries)
		{
			for (int m = HeaderHeight; m < Rows.Count; m++)
			{
				for (int n = HeaderWidth; n < Rows[m].Cells.Count; n++)
				{
					Rows[m].Cells[n].Field.Border.Side = ((n != Rows[m].Cells.Count - 1) ? (StiBorderSides.Left | StiBorderSides.Right) : StiBorderSides.Left);
					if (m == Rows.Count - pivot.Summaries.Count || m == HeaderHeight)
					{
						if (StiOptions.Engine.CrossTab.MemoryOptimization)
						{
							Rows[m].Cells[n].Field = Rows[m].Cells[n].Field.Clone() as StiCrossField;
						}
						Rows[m].Cells[n].Field.Border.Side |= StiBorderSides.Top;
					}
				}
			}
			if (cross.SumFields.Count > 1)
			{
				if (pivot.SummaryDirection == StiSummaryDirection.UpToDown)
				{
					for (int num2 = 0; num2 < Rows.Count; num2++)
					{
						StiPivotTableCell stiPivotTableCell4 = Rows[num2].Cells[HeaderWidth - 1];
						if (stiPivotTableCell4.Field != null)
						{
							if (StiOptions.Engine.CrossTab.MemoryOptimization)
							{
								stiPivotTableCell4.Field = stiPivotTableCell4.Field.Clone() as StiCrossField;
							}
							stiPivotTableCell4.Field.Border = new StiBorder(StiBorderSides.All, pivotTableStyle.LineColor, 1.0, StiPenStyle.Solid);
						}
					}
				}
				else if (HeaderHeight > 0)
				{
					for (int num3 = HeaderWidth; num3 < Rows[0].Cells.Count; num3++)
					{
						StiPivotTableCell stiPivotTableCell5 = Rows[HeaderHeight - 1].Cells[num3];
						if (stiPivotTableCell5.Field != null)
						{
							if (StiOptions.Engine.CrossTab.MemoryOptimization)
							{
								stiPivotTableCell5.Field = stiPivotTableCell5.Field.Clone() as StiCrossField;
							}
							stiPivotTableCell5.Field.Border = new StiBorder(StiBorderSides.All, pivotTableStyle.LineColor, 1.0, StiPenStyle.Solid);
						}
					}
				}
			}
		}
		UpdateSize();
	}

	public void UpdateSize()
	{
		int x;
		for (x = 0; x < Columns.Count; x++)
		{
			Columns[x].Width = Rows.Max((StiPivotTableRow r) => r.Cells[x].Width);
		}
		UpdateWidthAndHeight();
		for (int i = 0; i < Columns.Count; i++)
		{
			foreach (StiPivotTableRow row in Rows)
			{
				StiPivotTableCell stiPivotTableCell = row.Cells[i];
				if (stiPivotTableCell.DependedCells != null)
				{
					Rectangle cellRect = stiPivotTableCell.GetCellRect();
					if (stiPivotTableCell.IsColumn && cellRect.Width < stiPivotTableCell.MinimalTextSize.Width + StiPivotTableCell.ARROW_MARGIN_INT && stiPivotTableCell.TotalCell != null)
					{
						Columns[stiPivotTableCell.TotalCell.Column].Width += stiPivotTableCell.MinimalTextSize.Width + StiPivotTableCell.ARROW_MARGIN_INT - cellRect.Width;
					}
					else if (!stiPivotTableCell.IsColumn && cellRect.Width < stiPivotTableCell.MinimalTextSize.Width + StiPivotTableCell.ARROW_MARGIN_INT)
					{
						Columns[stiPivotTableCell.Column].Width += stiPivotTableCell.MinimalTextSize.Width + StiPivotTableCell.ARROW_MARGIN_INT - cellRect.Width;
					}
				}
			}
		}
		UpdateWidthAndHeight();
		this.AfterSizeUpdate?.Invoke(this, EventArgs.Empty);
	}

	private void UpdateWidthAndHeight()
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		double num = 0.0;
		foreach (StiPivotTableColumn column in Columns)
		{
			column.Left = num;
			if (column.Visible)
			{
				num += column.Width;
			}
		}
		Control control = Control;
		Padding padding;
		if (((control != null) ? control.Parent : null) != null)
		{
			Control control2 = Control;
			double num2 = num;
			padding = Control.Parent.Padding;
			control2.Width = (int)Math.Round(num2 + (double)((Padding)(ref padding)).Right);
		}
		Width = num;
		num = 0.0;
		bool flag = false;
		foreach (StiPivotTableRow row in Rows)
		{
			row.Top = num;
			if (row.Visible)
			{
				num += row.Height;
				row.IsEven = flag;
				flag = !flag;
			}
		}
		Control control3 = Control;
		if (((control3 != null) ? control3.Parent : null) != null)
		{
			Control control4 = Control;
			double num3 = num;
			padding = Control.Parent.Padding;
			control4.Height = (int)Math.Round(num3 + (double)((Padding)(ref padding)).Bottom);
		}
		Height = num;
	}

	private Size GetCellSize(StiCell cell)
	{
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Invalid comparison between Unknown and I4
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Invalid comparison between Unknown and I4
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Invalid comparison between Unknown and I4
		double num = 10000.0;
		cell.Field.Margins.Left = 9.0;
		if (cell.Field.MaxSize.Width != 0.0)
		{
			num = GetFieldWidth(cell.Field, cell.Field.Report.Unit.ConvertToHInches(cell.Field.MaxSize.Width));
		}
		if (cell.Field.MaxSize.Height != 0.0 && (cell.Field.Angle == 90f || cell.Field.Angle == 270f))
		{
			num = GetFieldHeight(cell.Field, cell.Field.Report.Unit.ConvertToHInches(cell.Field.MaxSize.Height));
		}
		StiExpression text = cell.Field.Text;
		cell.Field.Text = cell.Text;
		SizeD sizeD;
		if (cell.Field.Report == null || !cell.Field.Report.IsWpf)
		{
			sizeD = ((cell.Field.TextQuality == StiTextQuality.Wysiwyg || (StiOptions.Engine.FullTrust && cell.Field.AllowHtmlTags)) ? StiWysiwygTextRender.MeasureString(num, cell.Field.Font, cell.Field) : ((cell.Field.TextQuality != StiTextQuality.Typographic) ? StiStandardTextRenderer.MeasureString(num, cell.Field.Font, cell.Field) : StiTypographicTextRender.MeasureString(num, cell.Field.Font, cell.Field)));
		}
		else
		{
			sizeD = StiWpfTextRender.MeasureString(0.0, cell.Field);
			if (StiOptions.Engine.FullTrust && cell.Field.AllowHtmlTags)
			{
				sizeD.Height *= StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorWpfWithHtmlInCrossTab;
			}
		}
		cell.Field.Text = text;
		sizeD.Width += cell.Field.Margins.Left + cell.Field.Margins.Right;
		sizeD.Height += cell.Field.Margins.Top + cell.Field.Margins.Bottom;
		StiIconSetIndicator stiIconSetIndicator = cell.Field.Indicator as StiIconSetIndicator;
		if (stiIconSetIndicator?.CustomIcon != null && stiIconSetIndicator != null && stiIconSetIndicator.CustomIconSize.HasValue)
		{
			sizeD.Height = Math.Max(sizeD.Height, stiIconSetIndicator.CustomIconSize.Value.Height);
			if ((int)stiIconSetIndicator.Alignment != 512 && (int)stiIconSetIndicator.Alignment != 32 && (int)stiIconSetIndicator.Alignment != 2)
			{
				sizeD.Width += stiIconSetIndicator.CustomIconSize.Value.Width;
			}
		}
		sizeD.Width = Report.Unit.ConvertFromHInches(sizeD.Width);
		sizeD.Height = Report.Unit.ConvertFromHInches(sizeD.Height);
		if (cell.Field.MaxSize.Width != 0.0 && cell.Field.MaxSize.Width < sizeD.Width)
		{
			sizeD.Width = cell.Field.MaxSize.Width;
		}
		if (cell.Field.MinSize.Width != 0.0 && cell.Field.MinSize.Width > sizeD.Width)
		{
			sizeD.Width = cell.Field.MinSize.Width;
		}
		SizeD sizeD2 = Report.Unit.ConvertToHInches(sizeD.Multiply(StiScale.Factor));
		return new Size((int)Math.Round(sizeD2.Width), (int)Math.Round(sizeD2.Height));
	}

	private double GetFieldWidth(StiCrossField field, double value)
	{
		RectangleD rect = new RectangleD(0.0, 0.0, value, field.Height);
		rect = field.ConvertTextMargins(rect, convert: false);
		return field.ConvertTextBorders(rect, convert: false).Width;
	}

	private double GetFieldHeight(StiCrossField field, double value)
	{
		RectangleD rect = new RectangleD(0.0, 0.0, field.Height, value);
		rect = field.ConvertTextMargins(rect, convert: false);
		return field.ConvertTextBorders(rect, convert: false).Height;
	}

	public StiPivotTableCell GetParent(StiPivotTableCell cell)
	{
		if (cell != null && cell.Parent != null)
		{
			return GetParent(cell.Parent);
		}
		return cell;
	}

	public StiPivotTableCell FindSelectableCell(StiPivotTableCell cell, Keys key)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Invalid comparison between Unknown and I4
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Invalid comparison between Unknown and I4
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Invalid comparison between Unknown and I4
		if ((int)key == 39)
		{
			int i;
			for (i = cell.Column + 1; i < Columns.Count && !Columns[i].Visible; i++)
			{
			}
			if (i >= Columns.Count || !Columns[i].Visible)
			{
				return cell;
			}
			return Rows[cell.Row].Cells[i];
		}
		if ((int)key == 37)
		{
			int num = cell.Column - 1;
			while (num > 0 && !Columns[num].Visible)
			{
				num--;
			}
			if (num < 0 || !Columns[num].Visible || Rows[cell.Row].Cells[num].IsHeader)
			{
				return cell;
			}
			return Rows[cell.Row].Cells[num];
		}
		if ((int)key == 38)
		{
			int num2 = cell.Row - 1;
			while (num2 > 0 && !Rows[num2].Visible)
			{
				num2--;
			}
			if (num2 < 0 || !Rows[num2].Visible || Rows[num2].Cells[cell.Column].IsHeader)
			{
				return cell;
			}
			return Rows[num2].Cells[cell.Column];
		}
		if ((int)key == 40)
		{
			int j;
			for (j = cell.Row + 1; j < Rows.Count && !Rows[j].Visible; j++)
			{
			}
			if (j >= Rows.Count || !Rows[j].Visible || Rows[j].Cells[cell.Column].IsHeader)
			{
				return cell;
			}
			return Rows[j].Cells[cell.Column];
		}
		return cell;
	}

	public void GridPaint(PaintEventArgs e, StiPivotElementStyle style, bool isDesigner = false, Size? transformation = null)
	{
		if (Rows == null)
		{
			return;
		}
		Hashtable hashtable = new Hashtable();
		StiPivotTableRow[] array = Rows.ToArray();
		foreach (StiPivotTableRow stiPivotTableRow in array)
		{
			Rectangle rectangle = Rectangle.Empty;
			StiPivotTableCell[] array2 = stiPivotTableRow.Cells.ToArray();
			foreach (StiPivotTableCell cell in array2)
			{
				StiPivotTableCell parent = GetParent(cell);
				if (hashtable.Contains(parent))
				{
					continue;
				}
				StiMeter stiMeter = (isDesigner ? GetMeter(parent) : null);
				bool mouseEntered = parent.MouseEntered;
				rectangle = parent.GetCellRect();
				if (rectangle.Width > 0 && rectangle.Height > 0 && e.ClipRectangle.IntersectsWith(rectangle))
				{
					Rectangle rectangle2 = rectangle;
					if (transformation.HasValue)
					{
						rectangle2.X += transformation.Value.Width;
						rectangle2.Y += transformation.Value.Height;
					}
					if (stiMeter != null)
					{
						PointD? pointClick = StiPivotTableElementClickHelper.GetPointClick(PivotTable);
						if (pointClick.HasValue && rectangle2.Contains(pointClick.Value.ToPoint()))
						{
							StiPivotTableElementSelection.ResetSelection(PivotTable);
							StiPivotTableElementSelection.Select(PivotTable, stiMeter);
							StiPivotTableElementClickHelper.InvokeTableHeaderClick(PivotTable, stiMeter?.Key);
						}
					}
					if (stiMeter != null)
					{
						PointD? mouseOverPoint = StiPivotTableElementMouseOverHelper.GetMouseOverPoint(PivotTable);
						parent.MouseEntered = mouseOverPoint.HasValue && rectangle2.Contains(mouseOverPoint.Value.ToPoint());
					}
					parent.Paint(e, rectangle, isDesigner, style);
					parent.MouseEntered = mouseEntered;
					hashtable[parent] = parent;
				}
				if (rectangle.Left > e.ClipRectangle.Right)
				{
					break;
				}
			}
			if (rectangle.Top > e.ClipRectangle.Bottom)
			{
				break;
			}
		}
	}

	private StiMeter GetMeter(StiPivotTableCell parent)
	{
		StiCrossHeader stiCrossHeader = parent?.Field as StiCrossHeader;
		if (stiCrossHeader == null)
		{
			stiCrossHeader = parent?.Field?.TagValue as StiCrossHeader;
		}
		string key = stiCrossHeader?.MeterKey;
		if (key == null)
		{
			return null;
		}
		StiMeter stiMeter = PivotTable.Columns.FirstOrDefault((StiPivotColumn c) => c.Key == key);
		if (stiMeter == null)
		{
			stiMeter = PivotTable.Rows.FirstOrDefault((StiPivotRow c) => c.Key == key);
		}
		return stiMeter;
	}

	public void OnMouseMove(MouseEventArgs e)
	{
		int i;
		for (i = 0; i < Columns.Count && (Columns[i].Left + Columns[i].Width < (double)e.X || !Columns[i].Visible); i++)
		{
		}
		int j;
		for (j = 0; j < Rows.Count && (Rows[j].Top + Rows[j].Height < (double)e.Y || !Rows[j].Visible); j++)
		{
		}
		if (j < Rows.Count && i < Columns.Count && Columns[i].Left + Columns[i].Width >= (double)e.X && Rows[j].Top + Rows[j].Height >= (double)e.Y && Rows[j].Visible && Columns[i].Visible)
		{
			StiPivotTableCell parent = GetParent(Rows[j].Cells[i]);
			if (MouseEnterCell == null)
			{
				MouseEnterCell = parent;
				MouseEnterCell.OnMouseEnter((EventArgs)(object)e);
			}
			else if (MouseEnterCell != parent)
			{
				MouseEnterCell.OnMouseLeave((EventArgs)(object)e);
				MouseEnterCell = parent;
				MouseEnterCell.OnMouseEnter((EventArgs)(object)e);
			}
			parent.OnMouseMove(e);
		}
		else if (MouseEnterCell != null)
		{
			MouseEnterCell.OnMouseLeave((EventArgs)(object)e);
			MouseEnterCell = null;
		}
	}

	internal string GetTitle()
	{
		return Rows[0]?.Cells[0]?.Field?.Text;
	}

	public StiPivotTableContainer(Control control, StiPivotTableElement element)
	{
		Control = control;
		PivotTable = element;
		Painter = StiPainter.GetPainter(typeof(StiText), StiGuiMode.Gdi);
	}
}
