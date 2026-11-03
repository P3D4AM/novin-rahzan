using System;
using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Controls;

namespace Stimulsoft.Report.Viewer;

[ToolboxItem(false)]
internal class ZoomComboBox : ComboBoxEx
{
	public override bool PreProcessMessage(ref Message m)
	{
		if (((Message)(ref m)).Msg == 256 && ((Message)(ref m)).WParam == (IntPtr)13)
		{
			return false;
		}
		return ((Control)this).PreProcessMessage(ref m);
	}
}
