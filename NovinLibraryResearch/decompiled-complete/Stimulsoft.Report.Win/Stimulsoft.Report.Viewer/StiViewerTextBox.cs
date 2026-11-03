using System;
using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Controls;

namespace Stimulsoft.Report.Viewer;

[ToolboxItem(false)]
public class StiViewerTextBox : TextBoxX
{
	public override bool PreProcessMessage(ref Message m)
	{
		if (((Message)(ref m)).Msg == 256)
		{
			if (((Message)(ref m)).WParam == (IntPtr)9)
			{
				return false;
			}
			if (((Message)(ref m)).WParam == (IntPtr)13)
			{
				return false;
			}
		}
		return ((Control)this).PreProcessMessage(ref m);
	}
}
