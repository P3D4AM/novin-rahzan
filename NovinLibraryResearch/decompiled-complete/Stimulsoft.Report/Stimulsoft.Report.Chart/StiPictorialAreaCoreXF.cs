using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiPictorialAreaCoreXF : StiAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Pictorial");

	public override int Position => 131;

	public override StiCellGeom Render(StiContext context, RectangleF rect)
	{
		StiPictorialAreaGeom stiPictorialAreaGeom = new StiPictorialAreaGeom(base.Area, rect);
		List<IStiSeries> series = GetSeries();
		context.PushTranslateTransform(rect.X, rect.Y);
		if (series.Count > 0)
		{
			series[0].Core.RenderSeries(context, new RectangleF(0f, 0f, rect.Width, rect.Height), stiPictorialAreaGeom, series.ToArray());
		}
		context.PopTransform();
		return stiPictorialAreaGeom;
	}

	public void RenderSeries(StiContext context, RectangleF rectSeries, StiAreaGeom geom, List<IStiSeries> seriesCollection)
	{
	}

	protected override void PrepareInfo(RectangleF rect)
	{
	}

	public StiPictorialAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
