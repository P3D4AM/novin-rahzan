using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Maps;

public class StiMap25StyleFX : StiMapStyleFX
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "Green");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.Green;

	public override StiMapStyleIdent StyleId => StiMapStyleIdent.Style25;

	public override string LocalizeName => StiLocalization.Get("Chart", "Style") + 25;

	public override Color IndividualColor
	{
		get
		{
			return StiColor.Get("#70ad47");
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

	public override Color[] Colors
	{
		get
		{
			return new Color[6]
			{
				StiColor.Get("#70ad47"),
				StiColor.Get("#4472c4"),
				StiColor.Get("#ffc000"),
				StiColor.Get("#43682b"),
				StiColor.Get("#fd6a37"),
				StiColor.Get("#997300")
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

	public StiMap25StyleFX()
	{
		base.HeatmapWithGroup.Colors = new Color[2]
		{
			StiColor.Get("#70ad47"),
			StiColor.Get("#ffc000")
		};
		base.Heatmap.Color = StiColor.Get("#70ad47");
	}
}
