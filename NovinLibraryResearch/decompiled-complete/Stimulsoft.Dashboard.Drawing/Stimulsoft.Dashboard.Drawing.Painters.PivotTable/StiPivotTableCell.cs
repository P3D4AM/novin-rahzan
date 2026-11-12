#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Dashboard.Drawing.Painters.PivotTable;

public class StiPivotTableCell
{
	public static float ARROW_MARGIN = 4f * (float)StiScale.Factor;

	public static int ARROW_MARGIN_INT = (int)Math.Round(4f * (float)StiScale.Factor);

	public static float ARROW_SIZE = 6f * (float)StiScale.Factor;

	private StiPivotTableContainer container;

	private bool isSelected;

	public double Height { get; set; }

	public double Width { get; set; }

	public int ColSpan { get; set; }

	public int RowSpan { get; set; }

	public int Row { get; set; }

	public int Column { get; set; }

	public int Level { get; set; }

	public StiPivotTableCell Parent { get; set; }

	public List<StiPivotTableCell> DependedCells { get; set; }

	public StiPivotTableCell TotalCell { get; set; }

	public bool Opened { get; set; }

	public bool IsTotal { get; set; }

	public bool IsColumn { get; set; }

	public bool IsHeader { get; set; }

	public StiCrossField Field { get; set; }

	public Size MinimalTextSize { get; set; }

	public string ParentGuid { get; set; }

	public string Text { get; set; }

	public bool MouseEntered { get; set; }

	public bool IsSelected
	{
		get
		{
			return isSelected;
		}
		set
		{
			if (container.SelectedCell != null && container.SelectedCell.IsSelected && container.SelectedCell != this)
			{
				container.SelectedCell.IsSelected = false;
			}
			container.SelectedCell = (value ? this : null);
			isSelected = value;
			Rectangle cellRect = GetCellRect();
			cellRect.Inflate(2, 2);
			Control control = container.Control;
			if (control != null)
			{
				control.Invalidate(cellRect);
			}
		}
	}

	internal void Paint(PaintEventArgs e, Rectangle cellBounds, bool isDesigner, StiPivotElementStyle style)
	{
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Expected O, but got Unknown
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Expected O, but got Unknown
		try
		{
			if (Parent != null)
			{
				Parent.Paint(e, cellBounds, isDesigner, style);
			}
			else
			{
				if (Field == null)
				{
					return;
				}
				RectangleD rectangleD = GetCellRectD().Multiply(1.0 / StiScale.Factor);
				double num = (container.PivotTable.Page.Report.IsDesigning ? container.PivotTable.Page.Zoom : 1.0);
				Field.Page.Report.Info.Zoom = num;
				RectangleD directDisplayRectangle = ((Field.Page.Unit != StiUnit.HundredthsOfInch) ? container.Report.Unit.ConvertFromHInches(rectangleD.Divide(num)) : rectangleD.Divide(num));
				if (Field.MaxSize.Width < directDisplayRectangle.Width)
				{
					Field.MaxSize = new SizeD(directDisplayRectangle.Width, Field.MaxSize.Height);
				}
				Field.SetDirectDisplayRectangle(directDisplayRectangle);
				if (!IsHeader)
				{
					Color foregroundColor = GetForegroundColor(style, container.Rows[Row].IsEven);
					foregroundColor = CheckNegativeColor(foregroundColor);
					Field.Brush = ((Field.ConditionBrush != null && this != container.SelectedCell) ? Field.ConditionBrush : new StiSolidBrush(GetBackgroundColor(style, container.Rows[Row].IsEven)));
					Field.TextBrush = Field.ConditionTextBrush ?? new StiSolidBrush(foregroundColor);
				}
				else
				{
					Color color = (MouseEntered ? style.HotColumnHeaderBackColor : style.ColumnHeaderBackColor);
					Color color2 = style.ColumnHeaderForeColor;
					if (Column < container.HeaderWidth && Row >= container.HeaderHeight)
					{
						color = (MouseEntered ? style.HotRowHeaderBackColor : style.RowHeaderBackColor);
						color2 = style.RowHeaderForeColor;
					}
					Field.Brush = (MouseEntered ? new StiSolidBrush(color) : (Field.ConditionBrush ?? new StiSolidBrush(color)));
					Field.TextBrush = Field.ConditionTextBrush ?? new StiSolidBrush(color2);
				}
				Field.SetTextInternal(Text);
				container.Painter.Paint(Field, new StiPaintEventArgs(e.Graphics, ToRectangleD(e.ClipRectangle)));
				if (DependedCells == null)
				{
					return;
				}
				GraphicsPath val = new GraphicsPath();
				try
				{
					RectangleD rectangleD2 = rectangleD.Multiply(StiScale.Factor);
					float num2 = (float)rectangleD2.Height / (float)num / 2f;
					if (Opened)
					{
						val.AddLine(ARROW_MARGIN + ARROW_SIZE, num2 - 4f * (float)StiScale.Factor, ARROW_MARGIN + ARROW_SIZE, num2 + 3f * (float)StiScale.Factor);
						val.AddLine(ARROW_MARGIN + ARROW_SIZE, num2 + 3f * (float)StiScale.Factor, ARROW_MARGIN, num2 + 3f * (float)StiScale.Factor);
						val.CloseFigure();
					}
					else
					{
						val.AddLine(ARROW_MARGIN + 1f, num2 - 4f * (float)StiScale.Factor, ARROW_MARGIN + ARROW_SIZE, num2);
						val.AddLine(ARROW_MARGIN + ARROW_SIZE, num2, ARROW_MARGIN + 1f, num2 + 4f * (float)StiScale.Factor);
						val.CloseFigure();
					}
					Graphics graphics = e.Graphics;
					GraphicsState val2 = graphics.Save();
					try
					{
						graphics.SetClip(rectangleD2.ToRectangle());
						graphics.TranslateTransform((float)rectangleD2.X, (float)rectangleD2.Y);
						graphics.ScaleTransform((float)num, (float)num);
						graphics.SmoothingMode = (SmoothingMode)2;
						SolidBrush val3 = new SolidBrush(style.ColumnHeaderForeColor);
						try
						{
							graphics.FillPath((Brush)(object)val3, val);
							return;
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
					}
					finally
					{
						graphics.Restore(val2);
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
		}
		catch (Exception ex)
		{
			Trace.WriteLine(ex.ToString());
		}
	}

	private Color CheckNegativeColor(Color color)
	{
		if (StiNegativeColorChecker.IsNegativeInRed(Field.TextFormat))
		{
			decimal? num = StiValueHelper.TryToNullableDecimal(Field.OriginalValue);
			if (num.HasValue && !(num.Value >= 0m))
			{
				return StiOptions.Engine.NegativeColor;
			}
			return color;
		}
		return color;
	}

	internal Color GetBackgroundColor(StiPivotElementStyle style, bool isEven)
	{
		if (this != container.SelectedCell)
		{
			if (!isEven)
			{
				return style.CellBackColor;
			}
			return style.AlternatingCellBackColor;
		}
		return style.SelectedCellBackColor;
	}

	internal Color GetForegroundColor(StiPivotElementStyle style, bool isEven)
	{
		if (this != container.SelectedCell)
		{
			if (!isEven)
			{
				return style.CellForeColor;
			}
			return style.AlternatingCellForeColor;
		}
		return style.SelectedCellForeColor;
	}

	internal void ChangeTotal(bool open, bool updateSize = true)
	{
		container.SelectedCell = null;
		Opened = open;
		foreach (StiPivotTableCell dependedCell in DependedCells)
		{
			if (IsColumn)
			{
				container.Columns[dependedCell.Column].Visible = Opened;
			}
			else
			{
				container.Rows[dependedCell.Row].Visible = Opened;
			}
		}
		if (IsColumn)
		{
			RowSpan = (Opened ? 1 : (container.MaxColLevel - Level + 1));
		}
		else
		{
			ColSpan = (Opened ? 1 : (container.MaxRowLevel - Level + 1));
		}
		if (TotalCell != null)
		{
			TotalCell.Parent = (Opened ? null : this);
		}
		if (open)
		{
			foreach (StiPivotTableCell dependedCell2 in DependedCells)
			{
				if (dependedCell2.DependedCells != null)
				{
					dependedCell2.ChangeTotal(dependedCell2.Opened, updateSize: false);
				}
			}
		}
		if (updateSize)
		{
			container.UpdateSize();
		}
	}

	internal Rectangle GetCellRect(double inflate = 0.0)
	{
		Rectangle result = GetCellRectD().ToRectangle();
		result.Inflate(1, 1);
		return result;
	}

	internal RectangleD GetCellRectD()
	{
		double num = (container.PivotTable.Page.Report.IsDesigning ? container.PivotTable.Page.Zoom : 1.0);
		RectangleD result = new RectangleD(container.Columns[Column].Left * num, container.Rows[Row].Top * num, container.Columns[Column].Visible ? (container.Columns[Column].Width * num) : 0.0, container.Rows[Row].Visible ? (Height * num) : 0.0);
		for (int i = 1; i < ColSpan; i++)
		{
			if (container.Columns[Column + i].Visible)
			{
				result.Width += container.Columns[Column + i].Width * num;
			}
		}
		for (int j = 1; j < RowSpan; j++)
		{
			if (container.Rows[Row + j].Visible)
			{
				result.Height += container.Rows[Row + j].Height * num;
			}
		}
		return result;
	}

	private RectangleD ToRectangleD(Rectangle rect)
	{
		return new RectangleD(rect.X, rect.Y, rect.Width, rect.Height);
	}

	internal void OnMouseMove(MouseEventArgs e)
	{
		Parent?.OnMouseMove(e);
	}

	internal void OnMouseClick(MouseEventArgs e)
	{
		if (Parent != null)
		{
			Parent.OnMouseClick(e);
			return;
		}
		if (DependedCells != null && TotalCell != null)
		{
			ChangeTotal(!Opened);
		}
		if (!IsHeader)
		{
			IsSelected = true;
		}
	}

	internal void OnMouseEnter(EventArgs e)
	{
		if (Parent != null)
		{
			Parent.OnMouseEnter(e);
			return;
		}
		MouseEntered = true;
		if (DependedCells != null && container.Control != null)
		{
			container.Control.Cursor = Cursors.Hand;
		}
		if (Row < container.HeaderHeight || Column < container.HeaderWidth)
		{
			Control control = container.Control;
			if (control != null)
			{
				control.Invalidate(GetCellRect(4.0));
			}
		}
	}

	internal void OnMouseLeave(EventArgs e)
	{
		if (Parent != null)
		{
			Parent.OnMouseLeave(e);
			return;
		}
		MouseEntered = false;
		if (container.Control != null && (Row < container.HeaderHeight || Column < container.HeaderWidth))
		{
			container.Control.Invalidate(GetCellRect(4.0));
			container.Control.Cursor = Cursors.Default;
		}
	}

	public StiPivotTableCell(StiPivotTableContainer container)
	{
		this.container = container;
		Opened = true;
	}
}
