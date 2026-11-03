using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Maps;

public class StiMap34StyleFX : StiMapStyleFX
{
	public override bool AllowDashboard => true;

	public override string DashboardName => Loc.Get("PropertyColor", "DarkGreen");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.DarkGreen;

	public override StiMapStyleIdent StyleId => StiMapStyleIdent.Style34;

	public override string LocalizeName => Loc.Get("Chart", "Style") + "34";

	public override Color DefaultColor
	{
		get
		{
			return StiColor.Get("e3c08e");
		}
		set
		{
		}
	}

	public override Color BorderColor
	{
		get
		{
			return Color.DarkGray;
		}
		set
		{
		}
	}

	public override Color IndividualColor
	{
		get
		{
			return StiColor.Get("e3c08e");
		}
		set
		{
		}
	}

	public override Color[] Colors
	{
		get
		{
			return new Color[7]
			{
				StiColor.Get("144b45"),
				StiColor.Get("ecb92f"),
				StiColor.Get("d66153"),
				StiColor.Get("25775b"),
				StiColor.Get("319491"),
				StiColor.Get("f7f7f7"),
				StiColor.Get("dd7c21")
			};
		}
		set
		{
		}
	}

	public override Color BackColor
	{
		get
		{
			return StiColor.Get("3f745e");
		}
		set
		{
		}
	}

	public StiMap34StyleFX()
	{
		base.HeatmapWithGroup.Colors = new Color[2]
		{
			StiColor.Get("#539790"),
			StiColor.Get("#264945")
		};
		base.Heatmap.Color = StiColor.Get("#539790");
	}
}
