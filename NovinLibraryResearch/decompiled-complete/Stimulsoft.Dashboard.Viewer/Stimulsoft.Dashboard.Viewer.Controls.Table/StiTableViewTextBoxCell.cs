using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Drawing.Painters;
using Stimulsoft.Data.Functions;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Viewer.Controls.Table;

public class StiTableViewTextBoxCell : DataGridViewTextBoxCell
{
	protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
	{
		if (((DataGridViewElement)this).DataGridView == null)
		{
			return new Size(-1, -1);
		}
		Size result = ((DataGridViewTextBoxCell)this).GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);
		if (constraintSize.Width == 0)
		{
			StiTableViewControl stiTableViewControl = ((DataGridViewElement)((DataGridViewCell)this).OwningColumn).DataGridView as StiTableViewControl;
			StiTableElement table = stiTableViewControl.GridColumnToTable[((DataGridViewCell)this).OwningColumn] as StiTableElement;
			StiTableColumn column = stiTableViewControl.GridColumnToTableColumn[((DataGridViewCell)this).OwningColumn] as StiTableColumn;
			object value = ((DataGridViewCell)this).GetValue(rowIndex);
			result = new Size((int)GetSize(graphics, table, column, value), result.Height);
		}
		return result;
	}

	private double GetSize(Graphics g, StiTableElement table, StiTableColumn column, object value)
	{
		if (column is StiDataBarsColumn)
		{
			return StiTableElementGdiPainter.MeasureDataBarsCell(g, table, column, value, 1f).Width;
		}
		if (column is StiIndicatorColumn)
		{
			return StiTableElementGdiPainter.MeasureIndicatorCell(g, table, column, value, 1f).Width;
		}
		if (column is StiBubbleColumn)
		{
			return StiTableElementGdiPainter.MeasureBubbleCell(g, table, column, value, 1f).Width;
		}
		if (column is StiSparklinesColumn)
		{
			return StiTableElementGdiPainter.MeasureSparklinesCell(table, 0.0, 1f).Width;
		}
		if (StiImageHelper.IsImage(value))
		{
			return 0.0;
		}
		return StiTableElementGdiPainter.MeasureCommonCell(g, table, column, value, 1f).Width;
	}

	protected override void Paint(Graphics g, Rectangle clipRect, Rectangle cellRect, int rowIndex, DataGridViewElementStates state, object value, object formattedValue, string errorText, DataGridViewCellStyle style, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts parts)
	{
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Invalid comparison between Unknown and I4
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		StiTableViewControl stiTableViewControl = ((DataGridViewElement)((DataGridViewCell)this).OwningColumn).DataGridView as StiTableViewControl;
		StiTableElement table = stiTableViewControl.GridColumnToTable[((DataGridViewCell)this).OwningColumn] as StiTableElement;
		StiTableColumn stiTableColumn = stiTableViewControl.GridColumnToTableColumn[((DataGridViewCell)this).OwningColumn] as StiTableColumn;
		string[] columnKeys = (from c in table.Columns
			where StiTableColumnVisibilityHelper.GetVisible(c, table.Report)
			select c.Key).ToArray();
		object[] rowValues = (from DataGridViewCell c in (IEnumerable)((DataGridViewElement)this).DataGridView.Rows[rowIndex].Cells
			select c.Value).ToArray();
		object obj = ((DataGridViewCell)this).GetValue(rowIndex);
		Color? color = StiTableElementConditionHelper.ProcessBackColor(style.BackColor, stiTableColumn.Key, table, rowValues, columnKeys);
		if (color.HasValue)
		{
			style.BackColor = ((color.Value.A == 0) ? StiDashboardStyleHelper.GetBackColor(stiTableViewControl.Element) : color.GetValueOrDefault());
		}
		((DataGridViewTextBoxCell)this).Paint(g, clipRect, cellRect, rowIndex, state, value, formattedValue, errorText, style, advancedBorderStyle, (DataGridViewPaintParts)101);
		decimal? num = (decimal?)stiTableViewControl.GridColumnToMin[((DataGridViewCell)this).OwningColumn];
		decimal? num2 = (decimal?)stiTableViewControl.GridColumnToMax[((DataGridViewCell)this).OwningColumn];
		RectangleD rect = new RectangleD(cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(table);
		bool isSelected = (state & 0x20) > 0;
		if (((DataGridViewCell)this).OwningColumn.ValueType.IsEnum && obj != null && obj != DBNull.Value)
		{
			obj = Enum.ToObject(((DataGridViewCell)this).OwningColumn.ValueType, obj);
		}
		if (obj is StiSystemVariableObject)
		{
			obj = (((StiSystemVariableObject)obj != 0) ? Funcs.GetSystemVariable((StiSystemVariableObject)obj, rowIndex + 1) : stiTableColumn.TextFormat.Format(rowIndex + 1));
		}
		StiTableElementGdiPainter.PaintCell(g, rect, table, columnKeys, rowValues, stiTableColumn, obj, 1.0, tableStyle, num.GetValueOrDefault(0m), num2.GetValueOrDefault(100m), rowIndex + 1, isSelected, drawBorderSides: false);
		DrawOverridedSelection(g, state, stiTableColumn, tableStyle, rect);
		((DataGridViewCell)this).PaintBorder(g, clipRect, cellRect, style, advancedBorderStyle);
	}

	private static void DrawOverridedSelection(Graphics g, DataGridViewElementStates state, StiTableColumn column, StiTableElementStyle tableStyle, RectangleD rect)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if (column is StiColorScaleColumn && (state & 0x20) > 0)
		{
			Color selectedCellBackColor = tableStyle.SelectedCellBackColor;
			selectedCellBackColor = Color.FromArgb(150, selectedCellBackColor.R, selectedCellBackColor.G, selectedCellBackColor.B);
			StiDrawing.FillRectangle(g, selectedCellBackColor, rect);
		}
	}
}
