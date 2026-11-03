using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Maps;

public class StiMap35StyleFX : StiMapStyleFX
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "Sienna");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.Sienna;

	public override StiMapStyleIdent StyleId => StiMapStyleIdent.Style35;

	public override string LocalizeName => Loc.Get("Chart", "Style") + "35";

	public override Color DefaultColor
	{
		get
		{
			return StiColor.Get("#c2b39c");
		}
		set
		{
		}
	}

	public override Color BorderColor
	{
		get
		{
			return StiColor.Get("#442513");
		}
		set
		{
		}
	}

	public override Color IndividualColor
	{
		get
		{
			return StiColor.Get("#c2b39c");
		}
		set
		{
		}
	}

	public override Color[] Colors
	{
		get
		{
			return new Color[10]
			{
				StiColor.Get("#794d26"),
				StiColor.Get("#c7986a"),
				StiColor.Get("#c4b49a"),
				StiColor.Get("#894d29"),
				StiColor.Get("#422515"),
				StiColor.Get("#564438"),
				StiColor.Get("#876c57"),
				StiColor.Get("#d8814b"),
				StiColor.Get("#532525"),
				StiColor.Get("#59413f")
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
			return StiColor.Get("#fefefe");
		}
		set
		{
		}
	}

	public StiMap35StyleFX()
	{
		base.HeatmapWithGroup.Colors = new Color[2]
		{
			StiColor.Get("#c7986a"),
			StiColor.Get("#260d09")
		};
		base.Heatmap.Color = StiColor.Get("#c7986a");
	}
}
