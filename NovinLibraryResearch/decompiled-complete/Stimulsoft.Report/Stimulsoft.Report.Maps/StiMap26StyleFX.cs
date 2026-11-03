using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Maps;

public class StiMap26StyleFX : StiMapStyleFX
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "Turquoise");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.Turquoise;

	public override StiMapStyleIdent StyleId => StiMapStyleIdent.Style26;

	public override string LocalizeName => StiLocalization.Get("Chart", "Style") + 26;

	public override Color IndividualColor
	{
		get
		{
			return StiColor.Get("#2ec6c8");
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
				StiColor.Get("#2ec6c8"),
				StiColor.Get("#b5a1dd"),
				StiColor.Get("#5ab0ee"),
				StiColor.Get("#f4984e"),
				StiColor.Get("#d77a80"),
				StiColor.Get("#d04456")
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

	public StiMap26StyleFX()
	{
		base.HeatmapWithGroup.Colors = new Color[2]
		{
			StiColor.Get("#2ec6c8"),
			StiColor.Get("#f4984e")
		};
		base.Heatmap.Color = StiColor.Get("#2ec6c8");
	}
}
