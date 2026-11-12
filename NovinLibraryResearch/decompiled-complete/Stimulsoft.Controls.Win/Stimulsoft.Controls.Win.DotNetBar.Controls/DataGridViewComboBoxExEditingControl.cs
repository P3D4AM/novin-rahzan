using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxItem(false)]
[ComVisible(false)]
public class DataGridViewComboBoxExEditingControl : ComboBoxEx, IDataGridViewEditingControl
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
			return GetEditingControlFormattedValue((DataGridViewDataErrorContexts)1);
		}
		set
		{
			if (value is string text)
			{
				((Control)this).Text = text;
				if (string.Compare(text, ((Control)this).Text, ignoreCase: true, CultureInfo.CurrentCulture) != 0)
				{
					((ListControl)this).SelectedIndex = -1;
				}
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

	public Cursor EditingPanelCursor => Cursors.Default;

	public bool RepositionEditingControlOnValueChange => false;

	public DataGridViewComboBoxExEditingControl()
	{
		((Control)this).TabStop = false;
	}

	protected override void OnTextChanged(EventArgs e)
	{
		NotifyDataGridViewOfValueChange();
		base.OnTextChanged(e);
	}

	protected override void OnSelectedIndexChanged(EventArgs e)
	{
		base.OnSelectedIndexChanged(e);
		if (((ListControl)this).SelectedIndex != -1)
		{
			NotifyDataGridViewOfValueChange();
		}
	}

	private void NotifyDataGridViewOfValueChange()
	{
		if (_DataGridView.EditingControl == this)
		{
			_ValueChanged = true;
			_DataGridView.NotifyCurrentCellDirty(true);
		}
	}

	public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
	{
		((Control)this).Font = dataGridViewCellStyle.Font;
		((Control)this).ForeColor = dataGridViewCellStyle.ForeColor;
		((Control)this).BackColor = dataGridViewCellStyle.BackColor;
	}

	public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
	{
		return ((Control)this).Text;
	}

	public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		if ((keyData & 0xFFFF) != 40 && (keyData & 0xFFFF) != 38 && (!((ComboBox)this).DroppedDown || (keyData & 0xFFFF) != 27) && (keyData & 0xFFFF) != 13)
		{
			return !dataGridViewWantsInputKey;
		}
		return true;
	}

	public void PrepareEditingControlForEdit(bool selectAll)
	{
		if (selectAll)
		{
			((ComboBox)this).SelectAll();
		}
	}
}
