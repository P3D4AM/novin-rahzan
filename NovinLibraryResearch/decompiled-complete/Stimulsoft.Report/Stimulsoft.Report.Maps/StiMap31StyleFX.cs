using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Maps;

public class StiMap31StyleFX : StiMapStyleFX
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "DarkTurquoise");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.DarkTurquoise;

	public override StiMapStyleIdent StyleId => StiMapStyleIdent.Style31;

	public override string LocalizeName => StiLocalization.Get("Chart", "Style") + "31";

	public override Color BorderColor
	{
		get
		{
			return StiColor.Get("aa1c4458");
		}
		set
		{
		}
	}

	public override Color IndividualColor
	{
		get
		{
			return StiColor.Get("#5ea8bf");
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
				StiColor.Get("#fefef9"),
				StiColor.Get("#a8d7e2"),
				StiColor.Get("#5ea8bf"),
				StiColor.Get("#2b7f9e"),
				StiColor.Get("#1c4458")
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
			return StiColor.Get("#fefef9");
		}
		set
		{
		}
	}

	public override Color BackColor
	{
		get
		{
			return StiColor.Get("#235e6d");
		}
		set
		{
		}
	}

	public StiMap31StyleFX()
	{
		base.HeatmapWithGroup.Colors = new Color[2]
		{
			StiColor.Get("#a8d7e2"),
			StiColor.Get("#1c4458")
		};
		base.Heatmap.Color = StiColor.Get("#a8d7e2");
	}
}
