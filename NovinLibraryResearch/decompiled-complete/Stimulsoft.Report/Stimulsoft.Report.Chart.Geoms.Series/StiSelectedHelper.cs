using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart.Geoms.Series;

internal static class StiSelectedHelper
{
	public static StiBrush GetSelectedBrush(StiBrush regularBrush)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		if (regularBrush is StiSolidBrush)
		{
			Color color = ((StiSolidBrush)regularBrush).Color;
			return (StiBrush)new StiHatchBrush((HatchStyle)23, StiColorUtils.Light(color, (byte)50), StiColorUtils.Dark(color, (byte)50));
		}
		return (StiBrush)new StiHatchBrush((HatchStyle)23, Color.Transparent, Color.FromArgb(100, Color.White));
	}
}
