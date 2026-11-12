using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Dashboard;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiDataGridView : DataGridView
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DataGridViewCellStyle ColumnHeadersDefaultCellStyle
	{
		get
		{
			return ((DataGridView)this).ColumnHeadersDefaultCellStyle;
		}
		set
		{
			((DataGridView)this).ColumnHeadersDefaultCellStyle = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DataGridViewCellStyle DefaultCellStyle
	{
		get
		{
			return ((DataGridView)this).DefaultCellStyle;
		}
		set
		{
			((DataGridView)this).DefaultCellStyle = value;
		}
	}

	private DataGridViewCellStyle CreateHeaderStyle()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		return new DataGridViewCellStyle
		{
			Alignment = (DataGridViewContentAlignment)16,
			BackColor = StiElementConsts.Table.Header.BackgroundColor,
			Font = StiElementConsts.Table.Font.GetGdiFont(),
			ForeColor = StiElementConsts.Table.Font.Color,
			Padding = new Padding(0, 2, 0, 2),
			SelectionBackColor = SystemColors.Highlight,
			SelectionForeColor = SystemColors.HighlightText,
			WrapMode = (DataGridViewTriState)1
		};
	}

	private DataGridViewCellStyle CreateCellStyle()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		return new DataGridViewCellStyle
		{
			Alignment = (DataGridViewContentAlignment)16,
			BackColor = Color.White,
			Font = StiElementConsts.Table.Font.GetGdiFont(),
			ForeColor = StiElementConsts.Table.Font.Color,
			SelectionBackColor = Color.FromArgb(224, 236, 255),
			SelectionForeColor = StiElementConsts.Table.Font.Color,
			WrapMode = (DataGridViewTriState)2
		};
	}

	protected virtual DataGridViewCellStyle CreateAlternatingCellStyle()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		return new DataGridViewCellStyle
		{
			BackColor = Color.FromArgb(245, 245, 245)
		};
	}

	public void InitGridProperties()
	{
		((DataGridView)this).AllowUserToAddRows = false;
		((DataGridView)this).AllowUserToDeleteRows = false;
		((DataGridView)this).AllowUserToOrderColumns = false;
		((DataGridView)this).AllowUserToResizeColumns = true;
		((DataGridView)this).AllowUserToResizeRows = false;
		((DataGridView)this).ColumnHeadersBorderStyle = (DataGridViewHeaderBorderStyle)1;
		((DataGridView)this).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)1;
		((DataGridView)this).RowHeadersBorderStyle = (DataGridViewHeaderBorderStyle)1;
		((DataGridView)this).RowHeadersWidthSizeMode = (DataGridViewRowHeadersWidthSizeMode)0;
		((DataGridView)this).EditMode = (DataGridViewEditMode)4;
		((DataGridView)this).MultiSelect = false;
		((DataGridView)this).RowHeadersVisible = false;
		((DataGridView)this).ShowEditingIcon = false;
		((DataGridView)this).CellBorderStyle = (DataGridViewCellBorderStyle)4;
		ColumnHeadersDefaultCellStyle = CreateHeaderStyle();
		DefaultCellStyle = CreateCellStyle();
		((DataGridView)this).AlternatingRowsDefaultCellStyle = CreateAlternatingCellStyle();
		((DataGridView)this).BackgroundColor = Color.White;
		((DataGridView)this).BorderStyle = (BorderStyle)0;
	}

	public void RestorePosition(StiDataGridViewPosition position)
	{
		if (position == null)
		{
			return;
		}
		if (position.RowIndex >= 0 && position.RowIndex < ((DataGridView)this).Rows.Count)
		{
			DataGridViewRow val = ((DataGridView)this).Rows[position.RowIndex];
			if (position.ColumnIndex >= 0 && position.ColumnIndex < ((BaseCollection)val.Cells).Count)
			{
				((DataGridView)this).ClearSelection();
				val.Cells[position.ColumnIndex].Selected = true;
			}
		}
		if (position.HorizontalScroll != -1 && position.HorizontalScroll < ((BaseCollection)((DataGridView)this).Columns).Count)
		{
			((DataGridView)this).FirstDisplayedScrollingColumnIndex = position.HorizontalScroll;
		}
		if (position.VerticalScroll != -1 && position.VerticalScroll < ((DataGridView)this).Rows.Count)
		{
			((DataGridView)this).FirstDisplayedScrollingRowIndex = position.VerticalScroll;
		}
		if (position.ColumnWidths == null)
		{
			return;
		}
		foreach (KeyValuePair<string, int> columnWidth in position.ColumnWidths)
		{
			DataGridViewColumn val2 = ((DataGridView)this).Columns[columnWidth.Key];
			if (val2 != null)
			{
				val2.Width = columnWidth.Value;
			}
		}
	}

	public StiDataGridViewPosition SavePosition()
	{
		return new StiDataGridViewPosition
		{
			HorizontalScroll = ((DataGridView)this).FirstDisplayedScrollingColumnIndex,
			VerticalScroll = ((DataGridView)this).FirstDisplayedScrollingRowIndex,
			RowIndex = ((((DataGridView)this).CurrentCell != null) ? ((DataGridView)this).CurrentCell.RowIndex : (-1)),
			ColumnIndex = ((((DataGridView)this).CurrentCell != null) ? ((DataGridView)this).CurrentCell.ColumnIndex : (-1)),
			ColumnWidths = ((IEnumerable)((DataGridView)this).Columns).Cast<DataGridViewColumn>().ToDictionary((DataGridViewColumn c) => c.Name, (DataGridViewColumn c) => c.Width)
		};
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).SetStyle((ControlStyles)131072, false);
		((DataGridView)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).SetStyle((ControlStyles)131072, true);
		((DataGridView)this).OnMouseUp(e);
	}

	public StiDataGridView()
	{
		((Control)this).SetStyle((ControlStyles)131072, true);
	}
}
