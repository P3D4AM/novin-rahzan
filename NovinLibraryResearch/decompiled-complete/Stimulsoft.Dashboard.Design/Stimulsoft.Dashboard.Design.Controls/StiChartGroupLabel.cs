using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Dashboard.Design.Controls;

[ToolboxItem(false)]
public class StiChartGroupLabel : Label
{
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	protected override Size DefaultSize => new Size(((Label)this).DefaultSize.Width, 32);

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override Color ForeColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return Color.DimGray;
			}
			return StiUX.Foreground;
		}
		set
		{
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override Color BackColor
	{
		get
		{
			return Color.Transparent;
		}
		set
		{
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override ContentAlignment TextAlign
	{
		get
		{
			return (ContentAlignment)16;
		}
		set
		{
		}
	}

	public StiChartGroupLabel()
	{
		((Control)this).Height = StiScale.YYI(32.0);
	}
}
