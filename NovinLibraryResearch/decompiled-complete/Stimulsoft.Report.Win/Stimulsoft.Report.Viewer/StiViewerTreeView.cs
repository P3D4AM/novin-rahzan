using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Report.Viewer;

[ToolboxItem(false)]
public class StiViewerTreeView : TreeView
{
	private const int WS_EX_LAYOUTRTL = 4194304;

	private bool mirrored;

	public bool Mirrored
	{
		get
		{
			return mirrored;
		}
		set
		{
			if (mirrored != value)
			{
				mirrored = value;
				((Control)this).OnRightToLeftChanged(EventArgs.Empty);
			}
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((TreeView)this).CreateParams;
			if (Mirrored)
			{
				createParams.ExStyle |= 0x400000;
			}
			return createParams;
		}
	}
}
