using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Drawing.Helpers;

namespace Stimulsoft.Dashboard.Viewer.Controls;

[ToolboxItem(false)]
public class StiAllCheckBox : StiTreeStateCheckBox
{
	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		StiDrawing.DrawLine(e.Graphics, base.Colors.SeparatorColor, 0.0, ((Control)this).Height - 2, ((Control)this).Width, ((Control)this).Height - 2);
	}

	public StiAllCheckBox()
	{
		((Control)this).Text = StiLocHelper.LocAll;
		((Control)this).Height = StiScale.YYI(StiElementConsts.ListBox.ItemHeight);
	}
}
