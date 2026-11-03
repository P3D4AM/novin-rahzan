using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF18 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "18";

	public override Color[] StyleColors { get; } = new Color[6]
	{
		Color.White,
		Color.Black,
		Color.Silver,
		Color.Red,
		Color.Green,
		Color.Blue
	};


	public override Color BasicStyleColor => Color.White;

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle18;

	public override StiBrush GetColumnBrush(Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		HatchStyle val = (HatchStyle)4;
		if (color == Color.White)
		{
			val = (HatchStyle)3;
		}
		else if (color == Color.Black)
		{
			val = (HatchStyle)43;
		}
		else if (color == Color.Silver)
		{
			val = (HatchStyle)0;
		}
		else if (color == Color.Red)
		{
			val = (HatchStyle)9;
		}
		else if (color == Color.Green)
		{
			val = (HatchStyle)51;
		}
		else if (color == Color.Blue)
		{
			val = (HatchStyle)2;
		}
		return (StiBrush)new StiHatchBrush(val, Color.Black, Color.White);
	}
}
