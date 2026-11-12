using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxItem(false)]
[ComVisible(false)]
public class DataGridViewMaskedTextBoxAdvEditingControl : MaskedTextBoxAdv, IDataGridViewEditingControl
{
	private DataGridView _DataGridView;

	private int _RowIndex;

	private bool _ValueChanged;

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
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = (string)value;
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

	protected override void OnTextChanged(EventArgs e)
	{
		_ValueChanged = true;
		_DataGridView.NotifyCurrentCellDirty(true);
		((Control)this).OnTextChanged(e);
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
		return !dataGridViewWantsInputKey;
	}

	public void PrepareEditingControlForEdit(bool selectAll)
	{
	}
}
