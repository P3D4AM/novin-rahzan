using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
internal class StiButtonText : StiButtonEdit
{
	public override bool ReadOnly
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public StiButtonText()
	{
		((TextBoxBase)base.TextBox).ReadOnly = true;
		((Control)base.TextBox).BackColor = SystemColors.Window;
		((Control)this).BackColor = SystemColors.Window;
	}
}
