using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiLegendGeom : StiCellGeom
{
	private IStiLegend legend;

	private List<StiLegendItemCoreXF> seriesItems;

	private StiLegendTitleGeom legendTitleGeom;

	public IStiLegend Legend => legend;

	public List<StiLegendItemCoreXF> SeriesItems => seriesItems;

	public StiLegendTitleGeom LegendTitleGeom
	{
		get
		{
			return legendTitleGeom;
		}
		set
		{
			legendTitleGeom = value;
		}
	}

	public override void Dispose()
	{
		base.Dispose();
		seriesItems.Clear();
		seriesItems = null;
	}

	public override void Draw(StiContext context)
	{
		if (!legend.Visible || seriesItems.Count == 0)
		{
			return;
		}
		RectangleF rect = base.ClientRectangle;
		if (LegendTitleGeom != null)
		{
			rect.Y += LegendTitleGeom.ClientRectangle.Height;
			rect.Height -= LegendTitleGeom.ClientRectangle.Height;
		}
		if (!(rect.Width > 0f) || !(rect.Height > 0f))
		{
			return;
		}
		if (legend.ShowShadow)
		{
			if (((StiChart)Legend.Chart).IsAnimation)
			{
				StiOpacityAnimation animation = new StiOpacityAnimation(TimeSpan.Zero, TimeSpan.Zero);
				context.DrawShadowRect(rect, 6, animation);
			}
			else
			{
				context.DrawCachedShadow(rect, (StiShadowSides)31, context.Options.IsPrinting);
			}
		}
		context.FillRectangle(legend.Brush, rect.X, rect.Y, rect.Width, rect.Height);
		StiPenGeom pen = new StiPenGeom(legend.BorderColor);
		context.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
	}

	public StiLegendGeom(IStiLegend legend, RectangleF clientRectangle, List<StiLegendItemCoreXF> seriesItems)
		: base(clientRectangle)
	{
		this.legend = legend;
		this.seriesItems = seriesItems;
	}
}
