using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Maps;

public class StiMap30StyleFX : StiMapStyleFX
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "DarkGray");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.DarkGray;

	public override StiMapStyleIdent StyleId => StiMapStyleIdent.Style30;

	public override string LocalizeName => StiLocalization.Get("Chart", "Style") + "30";

	public override Color BorderColor
	{
		get
		{
			return StiColor.Get("aadb7b46");
		}
		set
		{
		}
	}

	public override Color IndividualColor
	{
		get
		{
			return StiColor.Get("#4a4c55");
		}
		set
		{
		}
	}

	public override Color[] Colors
	{
		get
		{
			return new Color[4]
			{
				StiColor.Get("#db7b46"),
				StiColor.Get("#d3d3d5"),
				StiColor.Get("#6b6e75"),
				StiColor.Get("#4a4c55")
			};
		}
		set
		{
		}
	}

	public override Color DefaultColor
	{
		get
		{
			return StiColor.Get("#d0d0d0");
		}
		set
		{
		}
	}

	public override Color BackColor
	{
		get
		{
			return StiColor.Get("#595b65");
		}
		set
		{
		}
	}

	public StiMap30StyleFX()
	{
		base.HeatmapWithGroup.Colors = new Color[2]
		{
			StiColor.Get("#d3d3d5"),
			StiColor.Get("#4a4c55")
		};
		base.Heatmap.Color = StiColor.Get("#d3d3d5");
	}
}
