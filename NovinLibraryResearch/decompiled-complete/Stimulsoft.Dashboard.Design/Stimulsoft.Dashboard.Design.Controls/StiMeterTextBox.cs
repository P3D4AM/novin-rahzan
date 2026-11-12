using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Dashboard.Design.Controls;

[ToolboxItem(false)]
public class StiMeterTextBox : TextBox
{
	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		if ((int)keyData == 27)
		{
			((StiMeterListBox)(object)((Control)this).Parent).EndEdit();
			((Control)this).Parent.Controls.Remove((Control)(object)this);
			return true;
		}
		if ((int)keyData == 13)
		{
			((StiMeterListBox)(object)((Control)this).Parent).OnAfterLabelEdit(((Control)this).Text);
			((StiMeterListBox)(object)((Control)this).Parent).EndEdit();
			((Control)this).Parent.Controls.Remove((Control)(object)this);
			return true;
		}
		return false;
	}

	public StiMeterTextBox()
	{
		((TextBox)this).AcceptsReturn = true;
		((TextBoxBase)this).AcceptsTab = true;
		((TextBoxBase)this).BorderStyle = (BorderStyle)0;
	}
}
