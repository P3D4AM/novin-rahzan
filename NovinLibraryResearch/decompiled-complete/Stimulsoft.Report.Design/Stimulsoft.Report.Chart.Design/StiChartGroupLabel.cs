using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart.Design;

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
			return Color.DimGray;
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
