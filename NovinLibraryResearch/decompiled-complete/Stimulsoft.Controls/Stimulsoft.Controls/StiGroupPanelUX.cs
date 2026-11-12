using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiGroupPanelUX : StiGroupLine
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			return StiUX.Background;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Control)this).BackColor = value;
			}
		}
	}
}
