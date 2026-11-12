using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiListBoxUX : ListBox
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color ForeColor
	{
		get
		{
			return StiUX.InputForeground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((ListBox)this).ForeColor = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			return StiUX.InputBackground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((ListBox)this).BackColor = value;
			}
		}
	}

	public StiListBoxUX()
	{
		((Control)this).SetStyle((ControlStyles)512, false);
	}
}
