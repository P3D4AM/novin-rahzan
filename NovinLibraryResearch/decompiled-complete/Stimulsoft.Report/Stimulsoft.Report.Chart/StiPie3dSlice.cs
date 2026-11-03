using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Chart.Geoms.Series.Pie;

namespace Stimulsoft.Report.Chart;

public class StiPie3dSlice
{
	private RectangleF rectPieSlilce;

	private float pieHeight;

	private StiBrush seriesBrush;

	private Color borderColor;

	private IStiPie3dSeries series;

	public int ColorCount { get; private set; }

	public float StartAngle { get; private set; }

	public float SweepAngle { get; private set; }

	public float EndAngle => (StartAngle + SweepAngle) % 360f;

	public double Value { get; private set; }

	public string ArgumentText { get; private set; }

	public string Tag { get; private set; }

	public int Index { get; private set; }

	public int ColorIndex { get; private set; }

	public PointF TextPosition { get; private set; }

	public List<StiSeriesElementGeom> ChildGeoms { get; private set; }

	public StiSeriesInteractionData Interaction { get; private set; }

	internal void DrawLabels(StiAreaGeom areaGeom, StiContext context)
	{
		IStiPieSeriesLabels pieSeriesLabels = ((StiPie3dSeriesCoreXF)series.Core).GetPieSeriesLabels();
		if (pieSeriesLabels != null && !(pieSeriesLabels is StiNoneLabels) && pieSeriesLabels.Visible && pieSeriesLabels.Core is StiCenterPie3dLabelsCoreXF stiCenterPie3dLabelsCoreXF)
		{
			StiSeriesLabelsGeom item = stiCenterPie3dLabelsCoreXF.RenderLabel(series, context, this);
			areaGeom.CreateChildGeoms();
			areaGeom.ChildGeoms.Add(item);
		}
	}

	internal void DrawTopPieSliceGeom(StiAreaGeom areaGeom)
	{
		StiPie3dMainSideSeriesElementGeom item = new StiPie3dMainSideSeriesElementGeom(this, areaGeom, Value, ColorIndex, ColorCount, series, rectPieSlilce, borderColor, seriesBrush, StartAngle, SweepAngle);
		ChildGeoms.Add(item);
		areaGeom.CreateChildGeoms();
		areaGeom.ChildGeoms.Add(item);
	}

	internal void DrawBottomPieSliceGeom(StiAreaGeom areaGeom)
	{
		StiPie3dMainSideSeriesElementGeom item = new StiPie3dMainSideSeriesElementGeom(clientRectangle: new RectangleF(rectPieSlilce.X, rectPieSlilce.Y + pieHeight, rectPieSlilce.Width, rectPieSlilce.Height), pie3DSlice: this, areaGeom: areaGeom, value: Value, index: ColorIndex, count: ColorCount, series: series, borderColor: borderColor, brush: seriesBrush, startAngle: StartAngle, sweepAngle: SweepAngle);
		areaGeom.CreateChildGeoms();
		areaGeom.ChildGeoms.Add(item);
	}

	internal void DrawSides(StiAreaGeom areaGeom)
	{
		StiPie3dSidesSeriesElementGeom stiPie3dSidesSeriesElementGeom = new StiPie3dSidesSeriesElementGeom(this, areaGeom, Value, ColorIndex, ColorCount, series, rectPieSlilce, borderColor, seriesBrush, StartAngle, SweepAngle, pieHeight);
		ChildGeoms.Add(stiPie3dSidesSeriesElementGeom);
		TextPosition = stiPie3dSidesSeriesElementGeom.GetTextPosition();
		areaGeom.CreateChildGeoms();
		areaGeom.ChildGeoms.Add(stiPie3dSidesSeriesElementGeom);
	}

	public StiPie3dSlice(IStiArea area, double? value, string argumentText, string tag, int index, IStiPie3dSeries series, RectangleF rectPieSlilce, float pieHeight, float startAngle, float sweepAngle, StiBrush seriesBrush, Color borderColor, int colorIndex, int colorCount)
	{
		Value = value.GetValueOrDefault();
		ArgumentText = argumentText;
		Tag = tag;
		Index = index;
		ColorIndex = colorIndex;
		ColorCount = colorCount;
		this.series = series;
		this.rectPieSlilce = rectPieSlilce;
		this.pieHeight = pieHeight;
		StartAngle = startAngle;
		SweepAngle = sweepAngle;
		this.seriesBrush = seriesBrush;
		this.borderColor = borderColor;
		ChildGeoms = new List<StiSeriesElementGeom>();
		if (series.Core.Interaction != null)
		{
			Interaction = new StiSeriesInteractionData(area, series, ColorIndex);
		}
	}
}
