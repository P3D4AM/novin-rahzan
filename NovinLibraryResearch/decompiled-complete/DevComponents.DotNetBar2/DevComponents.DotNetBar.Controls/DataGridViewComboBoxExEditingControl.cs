using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[ComVisible(false)]
[ToolboxItem(false)]
public class DataGridViewComboBoxExEditingControl : ComboBoxEx, IDataGridViewEditingControl
{
	private DataGridView __2B1C;

	private int __2B03;

	private bool __2B1D;

	public DataGridView EditingControlDataGridView
	{
		get
		{
			return __2B1C;
		}
		set
		{
			__2B1C = value;
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
			return __2B03;
		}
		set
		{
			__2B03 = value;
		}
	}

	public bool EditingControlValueChanged
	{
		get
		{
			return __2B1D;
		}
		set
		{
			__2B1D = value;
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
		_2B1E();
		base.OnTextChanged(e);
	}

	protected override void OnSelectedIndexChanged(EventArgs e)
	{
		base.OnSelectedIndexChanged(e);
		if (((ListControl)this).SelectedIndex != -1)
		{
			_2B1E();
		}
	}

	private void _2B1E()
	{
		if (__2B1C.EditingControl == this)
		{
			__2B1D = true;
			__2B1C.NotifyCurrentCellDirty(true);
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Invalid comparison between Unknown and I4
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		if (!((ComboBox)this).DroppedDown)
		{
			if ((keyData & 0x27) == 39)
			{
				return true;
			}
			if ((keyData & 0x25) == 37)
			{
				return true;
			}
		}
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
