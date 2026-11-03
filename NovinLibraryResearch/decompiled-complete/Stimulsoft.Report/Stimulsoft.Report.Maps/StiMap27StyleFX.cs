using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Maps;

public class StiMap27StyleFX : StiMapStyleFX
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "SlateGray");

	public override StiMapStyleIdent StyleId => StiMapStyleIdent.Style27;

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.SlateGray;

	public override string LocalizeName => StiLocalization.Get("Chart", "Style") + 27;

	public override Color IndividualColor
	{
		get
		{
			return StiColor.Get("#0bac45");
		}
		set
		{
		}
	}

	public override Color DefaultColor
	{
		get
		{
			return StiColor.Get("#ffffff");
		}
		set
		{
		}
	}

	public override Color BorderColor
	{
		get
		{
			return StiColor.Get("#bb33475b");
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
				StiColor.Get("#0bac45"),
				StiColor.Get("#585257"),
				StiColor.Get("#ec334d"),
				StiColor.Get("#a1ae94"),
				StiColor.Get("#ed7d31"),
				StiColor.Get("#5ab0ee")
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
			return StiColor.Get("#33475B");
		}
		set
		{
		}
	}

	public StiMap27StyleFX()
	{
		base.HeatmapWithGroup.Colors = new Color[2]
		{
			StiColor.Get("#0bac45"),
			StiColor.Get("#ec334d")
		};
		base.Heatmap.Color = StiColor.Get("#0bac45");
	}
}
