using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiSampleLabelUX : Label
{
	protected override void OnMouseEnter(EventArgs e)
	{
		((Label)this).OnMouseEnter(e);
		((Control)this).ForeColor = (StiUX.IsDark ? StiUX.HyperlinkHover : Color.Blue);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Label)this).OnMouseLeave(e);
		((Control)this).ForeColor = (StiUX.IsDark ? StiUX.Hyperlink : Color.DimGray);
	}
}
