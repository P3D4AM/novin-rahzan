using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Drawing.Painters.Table;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Viewer.Controls.Table;

public class StiTableViewImageCell : DataGridViewImageCell
{
	protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
	{
		return null;
	}

	protected override void Paint(Graphics g, Rectangle clipRect, Rectangle cellRect, int rowIndex, DataGridViewElementStates state, object value, object formattedValue, string errorText, DataGridViewCellStyle style, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts parts)
	{
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		StiTableViewControl stiTableViewControl = ((DataGridViewElement)((DataGridViewCell)this).OwningColumn).DataGridView as StiTableViewControl;
		StiTableElement stiTableElement = stiTableViewControl.GridColumnToTable[((DataGridViewCell)this).OwningColumn] as StiTableElement;
		StiTableColumn stiTableColumn = stiTableViewControl.GridColumnToTableColumn[((DataGridViewCell)this).OwningColumn] as StiTableColumn;
		string[] columnKeys = stiTableElement.Columns.Select((StiTableColumn c) => c.Key).ToArray();
		object[] rowValues = (from DataGridViewCell c in (IEnumerable)((DataGridViewElement)this).DataGridView.Rows[rowIndex].Cells
			select c.Value).ToArray();
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(stiTableElement);
		if ((rowIndex & 1) == 1)
		{
			style.BackColor = tableStyle.AlternatingCellBackColor;
			style.SelectionBackColor = tableStyle.SelectedCellBackColor;
		}
		else
		{
			style.BackColor = tableStyle.CellBackColor;
			style.SelectionBackColor = tableStyle.SelectedCellBackColor;
		}
		Color? color = StiTableElementConditionHelper.ProcessBackColor(style.BackColor, stiTableColumn.Key, stiTableElement, rowValues, columnKeys);
		if (color.HasValue)
		{
			style.BackColor = ((color.Value.A == 0) ? StiDashboardStyleHelper.GetBackColor(stiTableViewControl.Element) : color.GetValueOrDefault());
		}
		((DataGridViewImageCell)this).Paint(g, clipRect, cellRect, rowIndex, state, value, formattedValue, errorText, style, advancedBorderStyle, (DataGridViewPaintParts)101);
		RectangleD rect = new RectangleD(cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
		StiImageCellPainter.Draw(g, rect, value, stiTableColumn);
		((DataGridViewCell)this).PaintBorder(g, clipRect, cellRect, style, advancedBorderStyle);
	}
}
