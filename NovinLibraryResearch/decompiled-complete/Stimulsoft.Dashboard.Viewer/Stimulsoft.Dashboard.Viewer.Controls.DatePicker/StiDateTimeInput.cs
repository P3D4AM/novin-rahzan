using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Dashboard.Viewer.Controls.DatePicker;

[ToolboxItem(false)]
public class StiDateTimeInput : MaskedTextBox
{
	public event EventHandler PrevPart;

	public event EventHandler NextPart;

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Invalid comparison between Unknown and I4
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Invalid comparison between Unknown and I4
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 8)
		{
			if (((TextBoxBase)this).SelectionStart > 0)
			{
				int selectionStart = ((TextBoxBase)this).SelectionStart;
				((TextBoxBase)this).SelectionStart = selectionStart - 1;
			}
			return true;
		}
		if ((int)keyData == 39 && this.NextPart != null && ((Control)this).Text.Length == ((TextBoxBase)this).SelectionStart)
		{
			this.NextPart(this, EventArgs.Empty);
			return true;
		}
		if ((int)keyData == 37 && this.PrevPart != null && ((TextBoxBase)this).SelectionStart == 0)
		{
			this.PrevPart(this, EventArgs.Empty);
			return true;
		}
		if ((int)keyData == 32 || (int)keyData == 46)
		{
			return true;
		}
		return ((MaskedTextBox)this).ProcessCmdKey(ref msg, keyData);
	}

	public StiDateTimeInput()
	{
		((TextBoxBase)this).BorderStyle = (BorderStyle)0;
		((MaskedTextBox)this).InsertKeyMode = (InsertKeyMode)2;
		((MaskedTextBox)this).ValidatingType = typeof(DateTime);
		((MaskedTextBox)this).Mask = "00/00/0000";
	}
}
