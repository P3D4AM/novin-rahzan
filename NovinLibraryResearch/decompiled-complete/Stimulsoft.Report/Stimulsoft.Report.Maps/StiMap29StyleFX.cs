using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Maps;

public class StiMap29StyleFX : StiMapStyleFX
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "Blue");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.Blue;

	public override StiMapStyleIdent StyleId => StiMapStyleIdent.Style29;

	public override string LocalizeName => StiLocalization.Get("Chart", "Style") + 29;

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

	public override Color BorderColor
	{
		get
		{
			return StiColor.Get("#b4b4b5");
		}
		set
		{
		}
	}

	public override Color IndividualColor
	{
		get
		{
			return StiColor.Get("#3498db");
		}
		set
		{
		}
	}

	public override Color[] Colors
	{
		get
		{
			return new Color[5]
			{
				StiColor.Get("#3498db"),
				StiColor.Get("#ef717a"),
				StiColor.Get("#6dcbb3"),
				StiColor.Get("#f28161"),
				StiColor.Get("#fccd1b")
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
			return StiColor.Get("#ffffff");
		}
		set
		{
		}
	}

	public StiMap29StyleFX()
	{
		base.HeatmapWithGroup.Colors = new Color[2]
		{
			StiColor.Get("#3498db"),
			StiColor.Get("#ef717a")
		};
		base.Heatmap.Color = StiColor.Get("#3498db");
	}
}
