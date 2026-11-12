using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Report.Design.DataTransformation.Controls;

[ToolboxItem(false)]
public class StiDataTransformColumnTextBox : TextBox
{
	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		if ((int)keyData == 27)
		{
			((StiDataTransformColumnListBox)(object)((Control)this).Parent).EndEdit();
			((Control)this).Parent.Controls.Remove((Control)(object)this);
			return true;
		}
		if ((int)keyData == 13)
		{
			((StiDataTransformColumnListBox)(object)((Control)this).Parent).OnAfterLabelEdit(((Control)this).Text);
			((StiDataTransformColumnListBox)(object)((Control)this).Parent).EndEdit();
			((Control)this).Parent.Controls.Remove((Control)(object)this);
			return true;
		}
		return false;
	}

	public StiDataTransformColumnTextBox()
	{
		((TextBox)this).AcceptsReturn = true;
		((TextBoxBase)this).AcceptsTab = true;
		((TextBoxBase)this).BorderStyle = (BorderStyle)0;
	}
}
