using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls;

[ComVisible(false)]
[ToolboxItem(false)]
public class DataGridViewIpAddressInputEditingControl : IpAddressInput, IDataGridViewEditingControl
{
	private DataGridView __2B1C;

	private int __2B03;

	private bool __2B1D;

	private bool __2B2E;

	internal bool _2B2D
	{
		get
		{
			return __2B2E;
		}
		set
		{
			__2B2E = value;
		}
	}

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

	public Cursor EditingPanelCursor => ((Control)this).Cursor;

	public bool RepositionEditingControlOnValueChange => false;

	protected override void OnTextChanged(EventArgs e)
	{
		__2B1D = true;
		__2B1C.NotifyCurrentCellDirty(true);
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
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		if ((keyData & 0x27) == 39)
		{
			return true;
		}
		if ((keyData & 0x25) == 37)
		{
			return true;
		}
		if ((keyData & 0x1B) == 27)
		{
			__2B2E = true;
		}
		return !dataGridViewWantsInputKey;
	}

	public void PrepareEditingControlForEdit(bool selectAll)
	{
	}
}
