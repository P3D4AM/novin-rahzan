using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Maps;

public class StiMap32StyleFX : StiMapStyleFX
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "Silver");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.Silver;

	public override StiMapStyleIdent StyleId => StiMapStyleIdent.Style32;

	public override string LocalizeName => StiLocalization.Get("Chart", "Style") + "32";

	public override Color BorderColor
	{
		get
		{
			return StiColor.Get("bb8698a2");
		}
		set
		{
		}
	}

	public override Color IndividualColor
	{
		get
		{
			return StiColor.Get("3a5263");
		}
		set
		{
		}
	}

	public override Color[] Colors
	{
		get
		{
			return new Color[3]
			{
				StiColor.Get("3a5263"),
				StiColor.Get("90a1ab"),
				StiColor.Get("c9d5dc")
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
			return StiColor.Get("#6d7e8b");
		}
		set
		{
		}
	}

	public StiMap32StyleFX()
	{
		base.HeatmapWithGroup.Colors = new Color[2]
		{
			StiColor.Get("#3a5263"),
			StiColor.Get("#c9d5dc")
		};
		base.Heatmap.Color = StiColor.Get("#3a5263");
	}
}
