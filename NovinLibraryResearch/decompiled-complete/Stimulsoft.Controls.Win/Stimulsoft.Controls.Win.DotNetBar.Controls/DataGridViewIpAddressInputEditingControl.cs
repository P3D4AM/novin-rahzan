using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.Editors;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxItem(false)]
[ComVisible(false)]
public class DataGridViewIpAddressInputEditingControl : IpAddressInput, IDataGridViewEditingControl
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
			return base.Value ?? "";
		}
		set
		{
			base.Value = (string)value;
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
