using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiLinkLabelUX : LinkLabel
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color ForeColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((Control)this).ForeColor;
			}
			return StiUX.Foreground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Control)this).ForeColor = value;
			}
		}
	}

	public StiLinkLabelUX()
	{
		if (StiUX.IsDark)
		{
			((LinkLabel)this).LinkColor = StiUX.Hyperlink;
			((LinkLabel)this).ActiveLinkColor = StiUX.HyperlinkHover;
		}
	}
}
