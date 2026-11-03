using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Maps;

public class StiMap21StyleFX : StiMapStyleFX
{
	public override StiMapStyleIdent StyleId => StiMapStyleIdent.Style21;

	public override string LocalizeName => Loc.Get("Chart", "Style") + "21";

	public override Color IndividualColor
	{
		get
		{
			return StiColor.Get("#239fd9");
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
				StiColor.Get("#239fd9"),
				StiColor.Get("#b2b2b2"),
				StiColor.Get("#55d1ff"),
				StiColor.Get("#e4e4e4"),
				StiColor.Get("#55d1ff"),
				StiColor.Get("#e4e4e4")
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
			return StiColor.Get("#666666");
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

	public StiMap21StyleFX()
	{
		base.HeatmapWithGroup.Colors = new Color[2]
		{
			StiColor.Get("#239fd9"),
			StiColor.Get("#b2b2b2")
		};
		base.Heatmap.Color = StiColor.Get("#239fd9");
	}
}
