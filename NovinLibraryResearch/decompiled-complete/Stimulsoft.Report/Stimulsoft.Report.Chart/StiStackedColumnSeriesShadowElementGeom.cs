using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiStackedColumnSeriesShadowElementGeom : StiCellGeom
{
	private IStiSeries series;

	private bool isTopShadow;

	private bool isBottomShadow;

	public override bool Invisible => true;

	public IStiSeries Series => series;

	public bool IsTopShadow => isTopShadow;

	public bool IsBottomShadow => isBottomShadow;

	public override void Draw(StiContext context)
	{
		RectangleF rect = base.ClientRectangle;
		if (Series is IStiFullStackedColumnSeries)
		{
			context.DrawCachedShadow(rect, (StiShadowSides)2, context.Options.IsPrinting);
			return;
		}
		if (IsTopShadow)
		{
			context.DrawCachedShadow(rect, (StiShadowSides)3, context.Options.IsPrinting);
		}
		if (IsBottomShadow)
		{
			context.DrawCachedShadow(rect, (StiShadowSides)30, context.Options.IsPrinting);
		}
	}

	public StiStackedColumnSeriesShadowElementGeom(IStiSeries series, RectangleF clientRectangle, bool isTopShadow, bool isBottomShadow)
		: base(clientRectangle)
	{
		this.series = series;
		this.isTopShadow = isTopShadow;
		this.isBottomShadow = isBottomShadow;
	}
}
