using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiTreeViewClicked : TreeView
{
	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 515)
		{
			((Message)(ref m)).Result = IntPtr.Zero;
		}
		else
		{
			((TreeView)this).WndProc(ref m);
		}
	}
}
