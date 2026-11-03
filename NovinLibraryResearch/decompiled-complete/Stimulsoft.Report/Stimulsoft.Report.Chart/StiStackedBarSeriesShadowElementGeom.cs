using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiStackedBarSeriesShadowElementGeom : StiCellGeom
{
	private IStiSeries series;

	private bool isLeftShadow;

	private bool isRightShadow;

	public override bool Invisible => true;

	public IStiSeries Series => series;

	public bool IsLeftShadow => isLeftShadow;

	public bool IsRightShadow => isRightShadow;

	public override void Draw(StiContext context)
	{
		RectangleF rect = base.ClientRectangle;
		if (Series is IStiFullStackedBarSeries)
		{
			context.DrawCachedShadow(rect, (StiShadowSides)8, context.Options.IsPrinting);
			return;
		}
		if (IsLeftShadow)
		{
			context.DrawCachedShadow(rect, (StiShadowSides)24, context.Options.IsPrinting);
		}
		if (IsRightShadow)
		{
			context.DrawCachedShadow(rect, (StiShadowSides)15, context.Options.IsPrinting);
		}
	}

	public StiStackedBarSeriesShadowElementGeom(IStiSeries series, RectangleF clientRectangle, bool isLeftShadow, bool isRightShadow)
		: base(clientRectangle)
	{
		this.series = series;
		this.isLeftShadow = isLeftShadow;
		this.isRightShadow = isRightShadow;
	}
}
