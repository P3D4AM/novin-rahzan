using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.Editors;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxItem(false)]
[ComVisible(false)]
public class DataGridViewDoubleInputEditingControl : DoubleInput, IDataGridViewEditingControl
{
	private DataGridView _DataGridView;

	private int _RowIndex;

	private bool _ValueChanged;

	private bool _EditCancelled;

	internal bool EditCancelled
	{
		get
		{
			return _EditCancelled;
		}
		set
		{
			_EditCancelled = value;
		}
	}

	public DataGridView EditingControlDataGridView
	{
		get
		{
			return _DataGridView;
		}
		set
		{
			_DataGridView = value;
		}
	}

	public object EditingControlFormattedValue
	{
		get
		{
			return base.Value.ToString();
		}
		set
		{
			if (value is string s)
			{
				base.Value = int.Parse(s);
			}
		}
	}

	public int EditingControlRowIndex
	{
		get
		{
			return _RowIndex;
		}
		set
		{
			_RowIndex = value;
		}
	}

	public bool EditingControlValueChanged
	{
		get
		{
			return _ValueChanged;
		}
		set
		{
			_ValueChanged = value;
		}
	}

	public Cursor EditingPanelCursor => ((Control)this).Cursor;

	public bool RepositionEditingControlOnValueChange => false;

	protected override void OnValueChanged(EventArgs e)
	{
		_ValueChanged = true;
		_DataGridView.NotifyCurrentCellDirty(true);
		base.OnValueChanged(e);
	}

	public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
	{
		((Control)this).Font = dataGridViewCellStyle.Font;
		((Control)this).ForeColor = dataGridViewCellStyle.ForeColor;
		((Control)this).BackColor = dataGridViewCellStyle.BackColor;
	}

	public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
	{
		return EditingControlFormattedValue;
	}

	public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Invalid comparison between Unknown and I4
		if ((keyData & 0x1B) == 27)
		{
			_EditCancelled = true;
		}
		return !dataGridViewWantsInputKey;
	}

	public void PrepareEditingControlForEdit(bool selectAll)
	{
	}
}
