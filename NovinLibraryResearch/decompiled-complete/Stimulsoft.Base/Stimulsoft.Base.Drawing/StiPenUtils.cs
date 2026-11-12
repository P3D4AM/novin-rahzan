using System.Drawing.Drawing2D;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Base.Drawing;

public sealed class StiPenUtils
{
	public static DashStyle GetPenStyle(StiPenStyle penStyle)
	{
		return (DashStyle)(penStyle switch
		{
			StiPenStyle.Dash => 1, 
			StiPenStyle.DashDot => 3, 
			StiPenStyle.DashDotDot => 4, 
			StiPenStyle.Dot => 2, 
			StiPenStyle.Double => 0, 
			_ => 0, 
		});
	}

	public static LineCap GetLineCap(StiPenLineCap lineCap)
	{
		return (LineCap)(lineCap switch
		{
			StiPenLineCap.ArrowAnchor => 20, 
			StiPenLineCap.DiamondAnchor => 19, 
			StiPenLineCap.Flat => 0, 
			StiPenLineCap.NoAnchor => 16, 
			StiPenLineCap.Round => 2, 
			StiPenLineCap.RoundAnchor => 18, 
			StiPenLineCap.Square => 1, 
			StiPenLineCap.SquareAnchor => 17, 
			StiPenLineCap.Triangle => 3, 
			_ => 1, 
		});
	}
}
