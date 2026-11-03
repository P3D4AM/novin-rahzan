using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Maps;

public class StiMap28StyleFX : StiMapStyleFX
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "DarkBlue");

	public override StiMapStyleIdent StyleId => StiMapStyleIdent.Style28;

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.DarkBlue;

	public override string LocalizeName => StiLocalization.Get("Chart", "Style") + 28;

	public override Color IndividualColor
	{
		get
		{
			return StiColor.Get("#165d9e");
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
				StiColor.Get("#165d9e"),
				StiColor.Get("#577eb6"),
				StiColor.Get("#569436"),
				StiColor.Get("#225056"),
				StiColor.Get("#d4dae0")
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
			return StiColor.Get("#ffffff");
		}
		set
		{
		}
	}

	public override Color BackColor
	{
		get
		{
			return StiColor.Get("#0a325a");
		}
		set
		{
		}
	}

	public override Color BorderColor
	{
		get
		{
			return StiColor.Get("#99bbbbbb");
		}
		set
		{
		}
	}

	public StiMap28StyleFX()
	{
		base.HeatmapWithGroup.Colors = new Color[2]
		{
			StiColor.Get("#165d9e"),
			StiColor.Get("#569436")
		};
		base.Heatmap.Color = StiColor.Get("#165d9e");
	}
}
