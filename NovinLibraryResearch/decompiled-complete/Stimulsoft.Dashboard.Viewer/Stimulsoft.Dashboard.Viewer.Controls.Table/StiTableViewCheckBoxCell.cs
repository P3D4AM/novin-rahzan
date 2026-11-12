using System.Collections;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Drawing.Painters;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Viewer.Controls.Table;

public class StiTableViewCheckBoxCell : DataGridViewCheckBoxCell
{
	protected override void Paint(Graphics g, Rectangle clipRect, Rectangle cellRect, int rowIndex, DataGridViewElementStates state, object value, object formattedValue, string errorText, DataGridViewCellStyle style, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts parts)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Invalid comparison between Unknown and I4
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		StiTableViewControl stiTableViewControl = ((DataGridViewElement)((DataGridViewCell)this).OwningColumn).DataGridView as StiTableViewControl;
		StiTableElement stiTableElement = stiTableViewControl.GridColumnToTable[((DataGridViewCell)this).OwningColumn] as StiTableElement;
		StiTableColumn stiTableColumn = stiTableViewControl.GridColumnToTableColumn[((DataGridViewCell)this).OwningColumn] as StiTableColumn;
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(stiTableElement);
		bool isSelected = (state & 0x20) > 0;
		string[] columnKeys = stiTableElement.Columns.Select((StiTableColumn c) => c.Key).ToArray();
		object[] rowValues = (from DataGridViewCell c in (IEnumerable)((DataGridViewElement)this).DataGridView.Rows[rowIndex].Cells
			select c.Value).ToArray();
		object value2 = ((DataGridViewCell)this).GetValue(rowIndex);
		Color? color = StiTableElementConditionHelper.ProcessBackColor(style.BackColor, stiTableColumn.Key, stiTableElement, rowValues, columnKeys);
		if (color.HasValue)
		{
			style.BackColor = ((color.Value.A == 0) ? StiDashboardStyleHelper.GetBackColor(stiTableViewControl.Element) : color.GetValueOrDefault());
		}
		((DataGridViewCheckBoxCell)this).Paint(g, clipRect, cellRect, rowIndex, state, value, formattedValue, errorText, style, advancedBorderStyle, (DataGridViewPaintParts)101);
		RectangleD rect = new RectangleD(cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
		rect.Width--;
		rect.Height--;
		StiTableElementGdiPainter.PaintCell(g, rect, stiTableElement, columnKeys, rowValues, stiTableColumn, value2, 1.0, tableStyle, 0m, 100m, rowIndex + 1, isSelected, drawBorderSides: false);
		((DataGridViewCell)this).PaintBorder(g, clipRect, cellRect, style, advancedBorderStyle);
	}
}
