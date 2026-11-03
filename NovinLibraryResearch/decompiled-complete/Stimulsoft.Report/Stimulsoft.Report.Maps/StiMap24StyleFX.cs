using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Maps;

public class StiMap24StyleFX : StiMapStyleFX
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "Orange");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.Orange;

	public override StiMapStyleIdent StyleId => StiMapStyleIdent.Style24;

	public override string LocalizeName => StiLocalization.Get("Chart", "Style") + 24;

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
			return StiColor.Get("#ed7d31");
		}
		set
		{
		}
	}

	public override Color[] Colors
	{
		get
		{
			return new Color[6]
			{
				StiColor.Get("#ed7d31"),
				StiColor.Get("#ffc000"),
				StiColor.Get("#70ad47"),
				StiColor.Get("#9e480e"),
				StiColor.Get("#997300"),
				StiColor.Get("#43682b")
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

	public StiMap24StyleFX()
	{
		base.HeatmapWithGroup.Colors = new Color[2]
		{
			StiColor.Get("#ed7d31"),
			StiColor.Get("#70ad47")
		};
		base.Heatmap.Color = StiColor.Get("#ed7d31");
	}
}
