using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls;

public class StiExcelImageCell : DataGridViewImageCell
{
	protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (value == null || value == DBNull.Value)
		{
			if (((Enum)paintParts).HasFlag((Enum)(object)(DataGridViewPaintParts)8))
			{
				paintParts = (DataGridViewPaintParts)(paintParts - 8);
			}
			if (((Enum)paintParts).HasFlag((Enum)(object)(DataGridViewPaintParts)4))
			{
				paintParts = (DataGridViewPaintParts)(paintParts - 4);
			}
		}
		((DataGridViewImageCell)this).Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
	}

	public StiExcelImageCell()
	{
		((DataGridViewCell)this).ReadOnly = false;
	}
}
