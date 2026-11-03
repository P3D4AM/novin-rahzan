using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public abstract class StiAxisAreaCoreXF : StiAreaCoreXF
{
	internal int ValuesCount;

	private double scrollRangeX;

	private double scrollRangeY;

	private double scrollViewX;

	private double scrollViewY;

	private double scrollDpiX = 1.0;

	private double scrollDpiY = 1.0;

	public double ScrollDistanceX => ScrollDpiX * ScrollValueX;

	public double ScrollDistanceY => ScrollDpiY * ScrollValueY;

	public double ScrollRangeX => scrollRangeX;

	public double ScrollRangeY => scrollRangeY;

	public double ScrollViewX => scrollViewX;

	public double ScrollViewY => scrollViewY;

	public bool BlockScrollValueX { get; set; }

	public bool BlockScrollValueY { get; set; }

	public double ScrollValueX { get; set; }

	public double ScrollValueY { get; set; }

	public double ScrollDpiX => scrollDpiX;

	public double ScrollDpiY => scrollDpiY;

	public double ScrollDragStartValue { get; set; }

	public override void ApplyStyle(IStiChartStyle style)
	{
		base.ApplyStyle(style);
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		if (stiAxisArea.AllowApplyStyle)
		{
			if (stiAxisArea.InterlacingHor != null)
			{
				stiAxisArea.InterlacingHor.Core.ApplyStyle(style);
			}
			if (stiAxisArea.InterlacingVert != null)
			{
				stiAxisArea.InterlacingVert.Core.ApplyStyle(style);
			}
			if (stiAxisArea.GridLinesHor != null)
			{
				stiAxisArea.GridLinesHor.Core.ApplyStyle(style);
			}
			if (stiAxisArea.GridLinesVert != null)
			{
				stiAxisArea.GridLinesVert.Core.ApplyStyle(style);
			}
			if (stiAxisArea.XAxis != null)
			{
				stiAxisArea.XAxis.Core.ApplyStyle(style);
			}
			if (stiAxisArea.XTopAxis != null)
			{
				stiAxisArea.XTopAxis.Core.ApplyStyle(style);
			}
			if (stiAxisArea.YAxis != null)
			{
				stiAxisArea.YAxis.Core.ApplyStyle(style);
			}
			if (stiAxisArea.YRightAxis != null)
			{
				stiAxisArea.YRightAxis.Core.ApplyStyle(style);
			}
		}
	}

	public override StiCellGeom Render(StiContext context, RectangleF rect)
	{
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		stiAxisArea.YAxis.Info.Minimum = 0.0;
		stiAxisArea.YAxis.Info.Maximum = 0.0;
		stiAxisArea.YRightAxis.Info.Minimum = 0.0;
		stiAxisArea.YRightAxis.Info.Maximum = 0.0;
		stiAxisArea.XAxis.Info.Minimum = 0.0;
		stiAxisArea.XAxis.Info.Maximum = 0.0;
		stiAxisArea.XTopAxis.Info.Minimum = 0.0;
		stiAxisArea.XTopAxis.Info.Maximum = 0.0;
		PrepareInfo(rect);
		StiYAxisViewGeom stiYAxisViewGeom = null;
		StiYAxisViewGeom stiYAxisViewGeom2 = null;
		StiYAxisViewGeom stiYAxisViewGeom3 = null;
		StiXAxisViewGeom stiXAxisViewGeom = null;
		StiXAxisViewGeom stiXAxisViewGeom2 = null;
		StiXAxisViewGeom stiXAxisViewGeom3 = null;
		StiYAxisGeom stiYAxisGeom = null;
		StiYAxisGeom stiYAxisGeom2 = null;
		StiYAxisGeom stiYAxisGeom3 = null;
		StiXAxisGeom stiXAxisGeom = null;
		StiXAxisGeom stiXAxisGeom2 = null;
		StiXAxisGeom stiXAxisGeom3 = null;
		RectangleF empty = RectangleF.Empty;
		RectangleF empty2 = RectangleF.Empty;
		RectangleF empty3 = RectangleF.Empty;
		RectangleF empty4 = RectangleF.Empty;
		RectangleF empty5 = RectangleF.Empty;
		RectangleF empty6 = RectangleF.Empty;
		RectangleF empty7 = RectangleF.Empty;
		List<IStiSeries> series = stiAxisArea.Core.GetSeries();
		if (series.Count > 0)
		{
			float value = 0f;
			float value2 = 0f;
			float value3 = 0f;
			float value4 = 0f;
			empty = (stiAxisArea.ReverseHor ? ((StiYRightAxisCoreXF)stiAxisArea.YRightAxis.Core).GetAxisRect(context, rect, includeAxisArrow: true, includeLabelsHeight: true, isDrawing: false, includeScrollBar: true) : ((StiYAxisCoreXF)stiAxisArea.YAxis.Core).GetAxisRect(context, rect, includeAxisArrow: true, includeLabelsHeight: true, isDrawing: false, includeScrollBar: true));
			if (base.Area.Chart.Table.Core.ShowTable())
			{
				float widthCellLegend = base.Area.Chart.Table.Core.GetWidthCellLegend(context);
				if (widthCellLegend > empty.Width)
				{
					empty.Width = widthCellLegend;
					empty.X = 0f - widthCellLegend;
					if (empty.Height == 0f)
					{
						empty.Height = 1f;
					}
				}
			}
			if (!empty.IsEmpty)
			{
				empty.X += rect.X;
				empty.Y += rect.Y;
			}
			if (!empty.IsEmpty)
			{
				value3 = empty.Width;
				if (empty.Top < rect.Top)
				{
					value = Math.Abs(rect.Top - empty.Top);
				}
				if (rect.Bottom < empty.Bottom)
				{
					value2 = Math.Abs(empty.Bottom - rect.Bottom);
				}
			}
			empty2 = ((StiYAxisCoreXF)stiAxisArea.YAxis.Core).GetCenterAxisRect(context, rect, includeAxisArrow: true, includeLabelsHeight: true, isDrawing: false);
			if (!empty2.IsEmpty)
			{
				empty2.X += rect.X;
				empty2.Y += rect.Y;
			}
			empty3 = (stiAxisArea.ReverseHor ? ((StiYAxisCoreXF)stiAxisArea.YAxis.Core).GetAxisRect(context, rect, includeAxisArrow: true, includeLabelsHeight: true, isDrawing: false, includeScrollBar: true) : ((StiYRightAxisCoreXF)stiAxisArea.YRightAxis.Core).GetAxisRect(context, rect, includeAxisArrow: true, includeLabelsHeight: true, isDrawing: false, includeScrollBar: true));
			if (!empty3.IsEmpty)
			{
				empty3.X += rect.X;
				empty3.Y += rect.Y;
			}
			if (!empty3.IsEmpty)
			{
				value4 = empty3.Width;
				if (empty3.Top < rect.Top)
				{
					value = Math.Max(value, Math.Abs(rect.Top - empty3.Top));
				}
				if (rect.Bottom < empty3.Bottom)
				{
					value2 = Math.Max(value2, Math.Abs(empty3.Bottom - rect.Bottom));
				}
			}
			empty4 = ((StiXTopAxisCoreXF)stiAxisArea.XTopAxis.Core).GetAxisRect(context, rect, includeAxisArrow: true, includeLabelsWidth: true, isDrawing: false, includeScrollBar: true);
			if (!empty4.IsEmpty)
			{
				empty4.X += rect.X;
				empty4.Y += rect.Y;
			}
			if (!empty4.IsEmpty)
			{
				value = Math.Max(empty4.Height, value);
				if (empty4.X < rect.X)
				{
					value3 = Math.Max(value3, Math.Abs(rect.Left - empty4.Left));
				}
				if (rect.Right < empty4.Right)
				{
					value4 = Math.Max(value4, Math.Abs(empty4.Right - rect.Right));
				}
			}
			empty5 = ((StiXAxisCoreXF)stiAxisArea.XAxis.Core).GetCenterAxisRect(context, rect, includeAxisArrow: true, includeLabelsHeight: true, isDrawing: false);
			if (!empty5.IsEmpty)
			{
				empty5.X += rect.X;
				empty5.Y += rect.Y;
			}
			if (stiAxisArea.ReverseHor)
			{
				Swap(ref value3, ref value4);
			}
			RectangleF rect2 = new RectangleF(rect.X + value3, rect.Y, rect.Width - value3 - value4, rect.Height);
			stiAxisArea.XAxis.Info.Dpi = (double)rect2.Width / stiAxisArea.XAxis.Info.Range;
			CalculateStepX(stiAxisArea.XAxis, rect2.Left, rect2.Right);
			CalculatePositions(stiAxisArea.XAxis, ref stiAxisArea.XAxis.Info.LabelsCollection, (!(stiAxisArea.XAxis.Labels.Step > 0f)) ? 1 : ((int)stiAxisArea.XAxis.Labels.Step));
			empty6 = ((StiXAxisCoreXF)stiAxisArea.XAxis.Core).GetAxisRect(context, rect2, includeAxisArrow: true, includeLabelsWidth: true, isDrawing: false, includeScrollBar: true);
			if (!empty6.IsEmpty && !base.Area.Chart.Table.Core.ShowTable())
			{
				empty6.X += rect.X;
				empty6.Y += rect.Y;
			}
			if (!empty6.IsEmpty && (!base.Area.Chart.Table.Core.ShowTable() || base.Area.Chart.Table.Chart.Area is IStiClusteredBarArea))
			{
				value2 = Math.Max(empty6.Height, value2);
				if (empty6.X < rect.X)
				{
					value3 = Math.Max(value3, Math.Abs(rect.Left - empty6.Left));
				}
				if (rect.Right < empty6.Right)
				{
					value4 = Math.Max(value4, Math.Abs(empty6.Right - rect.Right));
				}
			}
			if (stiAxisArea.ReverseHor)
			{
				Swap(ref value3, ref value4);
			}
			if (stiAxisArea.ReverseVert)
			{
				Swap(ref value, ref value2);
			}
			rect.X += value3;
			rect.Width -= value3 + value4;
			rect.Y += value;
			rect.Height -= value + value2;
			PrepareInfo(rect);
			stiYAxisViewGeom = stiAxisArea.YAxis.Core.RenderView(context, rect) as StiYAxisViewGeom;
			stiYAxisViewGeom2 = ((StiYAxisCoreXF)stiAxisArea.YAxis.Core).RenderCenterView(context, rect) as StiYAxisViewGeom;
			stiYAxisViewGeom3 = stiAxisArea.YRightAxis.Core.RenderView(context, rect) as StiYAxisViewGeom;
			stiXAxisViewGeom = stiAxisArea.XTopAxis.Core.RenderView(context, rect) as StiXAxisViewGeom;
			stiXAxisViewGeom2 = ((StiXAxisCoreXF)stiAxisArea.XAxis.Core).RenderCenterView(context, rect) as StiXAxisViewGeom;
			stiXAxisViewGeom3 = stiAxisArea.XAxis.Core.RenderView(context, rect) as StiXAxisViewGeom;
		}
		empty7 = rect;
		double num = CalculateScrollValuesX(rect, stiAxisArea);
		double num2 = CalculateScrollValuesY(rect, stiAxisArea);
		rect.X = 0f - (float)ScrollDistanceX;
		rect.Y = 0f - (float)ScrollDistanceY;
		rect.Width *= (float)num;
		rect.Height *= (float)num2;
		PrepareInfo(rect);
		if (series.Count > 0)
		{
			stiYAxisGeom = stiAxisArea.YAxis.Core.Render(context, rect) as StiYAxisGeom;
			stiYAxisGeom2 = ((StiYAxisCoreXF)stiAxisArea.YAxis.Core).RenderCenter(context, rect) as StiYAxisGeom;
			stiYAxisGeom3 = stiAxisArea.YRightAxis.Core.Render(context, rect) as StiYAxisGeom;
			stiXAxisGeom = stiAxisArea.XTopAxis.Core.Render(context, rect) as StiXAxisGeom;
			stiXAxisGeom2 = ((StiXAxisCoreXF)stiAxisArea.XAxis.Core).RenderCenter(context, rect) as StiXAxisGeom;
			stiXAxisGeom3 = stiAxisArea.XAxis.Core.Render(context, rect) as StiXAxisGeom;
			if (stiYAxisGeom != null)
			{
				stiYAxisGeom.View = stiYAxisViewGeom;
			}
			if (stiYAxisGeom2 != null)
			{
				stiYAxisGeom2.View = stiYAxisViewGeom2;
			}
			if (stiYAxisGeom3 != null)
			{
				stiYAxisGeom3.View = stiYAxisViewGeom3;
			}
			if (stiXAxisGeom != null)
			{
				stiXAxisGeom.View = stiXAxisViewGeom;
			}
			if (stiXAxisGeom2 != null)
			{
				stiXAxisGeom2.View = stiXAxisViewGeom2;
			}
			if (stiXAxisGeom3 != null)
			{
				stiXAxisGeom3.View = stiXAxisViewGeom3;
			}
		}
		if (stiXAxisViewGeom3 != null)
		{
			((StiXAxisCoreXF)stiAxisArea.XAxis.Core).RenderScrollBar(context, stiXAxisViewGeom3.ClientRectangle, stiXAxisViewGeom3);
		}
		if (stiXAxisViewGeom != null)
		{
			((StiXAxisCoreXF)stiAxisArea.XTopAxis.Core).RenderScrollBar(context, stiXAxisViewGeom.ClientRectangle, stiXAxisViewGeom);
		}
		if (stiYAxisViewGeom != null)
		{
			((StiYAxisCoreXF)stiAxisArea.YAxis.Core).RenderScrollBar(context, stiYAxisViewGeom.ClientRectangle, stiYAxisViewGeom);
		}
		if (stiYAxisViewGeom3 != null)
		{
			((StiYAxisCoreXF)stiAxisArea.YRightAxis.Core).RenderScrollBar(context, stiYAxisViewGeom3.ClientRectangle, stiYAxisViewGeom3);
		}
		StiAxisAreaViewGeom stiAxisAreaViewGeom = new StiAxisAreaViewGeom(stiAxisArea, empty7);
		StiAxisAreaGeom stiAxisAreaGeom = new StiAxisAreaGeom(stiAxisAreaViewGeom, stiAxisArea, rect);
		stiAxisAreaViewGeom.CreateChildGeoms();
		stiAxisAreaViewGeom.ChildGeoms.Add(stiAxisAreaGeom);
		foreach (IStiStrips strip in base.Area.Chart.Strips)
		{
			if (strip.ShowBehind)
			{
				strip.Core.Render(context, stiAxisAreaGeom, rect);
			}
		}
		foreach (IStiConstantLines constantLine in base.Area.Chart.ConstantLines)
		{
			if (constantLine.ShowBehind)
			{
				constantLine.Core.Render(context, stiAxisAreaGeom, rect);
			}
		}
		RenderSeries(context, rect, stiAxisAreaGeom, series);
		foreach (IStiStrips strip2 in base.Area.Chart.Strips)
		{
			if (!strip2.ShowBehind)
			{
				strip2.Core.Render(context, stiAxisAreaGeom, rect);
			}
		}
		foreach (IStiConstantLines constantLine2 in base.Area.Chart.ConstantLines)
		{
			if (!constantLine2.ShowBehind)
			{
				constantLine2.Core.Render(context, stiAxisAreaGeom, rect);
			}
		}
		if (stiYAxisGeom != null)
		{
			stiAxisAreaViewGeom.CreateChildGeoms();
			stiYAxisViewGeom.CreateChildGeoms();
			stiAxisAreaViewGeom.ChildGeoms.Add(stiYAxisViewGeom);
			stiYAxisViewGeom.ChildGeoms.Add(stiYAxisGeom);
		}
		if (stiYAxisGeom2 != null)
		{
			stiAxisAreaViewGeom.CreateChildGeoms();
			stiYAxisViewGeom2.CreateChildGeoms();
			stiAxisAreaViewGeom.ChildGeoms.Add(stiYAxisViewGeom2);
			stiYAxisViewGeom2.ChildGeoms.Add(stiYAxisGeom2);
		}
		if (stiYAxisGeom3 != null)
		{
			stiAxisAreaViewGeom.CreateChildGeoms();
			stiYAxisViewGeom3.CreateChildGeoms();
			stiAxisAreaViewGeom.ChildGeoms.Add(stiYAxisViewGeom3);
			stiYAxisViewGeom3.ChildGeoms.Add(stiYAxisGeom3);
		}
		if (stiXAxisGeom != null)
		{
			stiAxisAreaViewGeom.CreateChildGeoms();
			stiXAxisViewGeom.CreateChildGeoms();
			stiAxisAreaViewGeom.ChildGeoms.Add(stiXAxisViewGeom);
			stiXAxisViewGeom.ChildGeoms.Add(stiXAxisGeom);
		}
		if (stiXAxisGeom2 != null)
		{
			stiAxisAreaViewGeom.CreateChildGeoms();
			stiXAxisViewGeom2.CreateChildGeoms();
			stiAxisAreaViewGeom.ChildGeoms.Add(stiXAxisViewGeom2);
			stiXAxisViewGeom2.ChildGeoms.Add(stiXAxisGeom2);
		}
		if (stiXAxisGeom3 != null && (!base.Area.Chart.Table.Core.ShowTable() || base.Area.Chart.Table.Chart.Area is IStiClusteredBarArea))
		{
			stiAxisAreaViewGeom.CreateChildGeoms();
			stiXAxisViewGeom3.CreateChildGeoms();
			stiAxisAreaViewGeom.ChildGeoms.Add(stiXAxisViewGeom3);
			stiXAxisViewGeom3.ChildGeoms.Add(stiXAxisGeom3);
		}
		return stiAxisAreaViewGeom;
	}

	private double CalculateScrollValuesX(RectangleF rect, IStiAxisArea axisArea)
	{
		double num = 1.0;
		double minimum;
		double maximum;
		if (!IsAutoRangeXAxis(axisArea.XAxis))
		{
			minimum = axisArea.XAxis.Range.Minimum;
			maximum = axisArea.XAxis.Range.Maximum;
			if (minimum > maximum)
			{
				minimum = axisArea.XAxis.Info.Minimum;
				maximum = axisArea.XAxis.Info.Maximum;
			}
		}
		else
		{
			minimum = axisArea.XAxis.Info.Minimum;
			maximum = axisArea.XAxis.Info.Maximum;
		}
		scrollRangeX = axisArea.XAxis.Info.Maximum - axisArea.XAxis.Info.Minimum;
		scrollViewX = maximum - minimum;
		if (!axisArea.XAxis.Core.GetStartFromZero() || axisArea.XAxis.LogarithmicScale)
		{
			BlockScrollValueX = true;
		}
		else
		{
			BlockScrollValueX = false;
		}
		if (!BlockScrollValueX && !axisArea.XAxis.Interaction.ShowScrollBar)
		{
			double num2 = minimum + Math.Abs(axisArea.XAxis.Info.Minimum);
			if (axisArea.XAxis.Info.Minimum > 0.0 && minimum > 0.0)
			{
				num2 = Math.Abs(minimum - axisArea.XAxis.Info.Minimum);
			}
			if (!axisArea.ReverseHor)
			{
				ScrollValueX = num2;
			}
			else
			{
				ScrollValueX = scrollRangeX - num2 - scrollViewX;
			}
		}
		num = ScrollRangeX / ScrollViewX;
		scrollDpiX = (double)rect.Width * num / ScrollRangeX;
		return num;
	}

	private double CalculateScrollValuesY(RectangleF rect, IStiAxisArea axisArea)
	{
		double num = 1.0;
		double minimum;
		double maximum;
		if (!IsAutoRangeYAxis(axisArea.YAxis))
		{
			minimum = axisArea.YAxis.Range.Minimum;
			maximum = axisArea.YAxis.Range.Maximum;
			if (minimum > maximum)
			{
				minimum = axisArea.YAxis.Info.Minimum;
				maximum = axisArea.YAxis.Info.Maximum;
			}
		}
		else
		{
			minimum = axisArea.YAxis.Info.Minimum;
			maximum = axisArea.YAxis.Info.Maximum;
		}
		scrollRangeY = axisArea.YAxis.Info.Maximum - axisArea.YAxis.Info.Minimum;
		scrollViewY = maximum - minimum;
		if (!axisArea.YAxis.Core.GetStartFromZero() || axisArea.YAxis.LogarithmicScale)
		{
			BlockScrollValueY = true;
		}
		else
		{
			BlockScrollValueY = false;
		}
		if (!BlockScrollValueY)
		{
			double num2 = minimum + Math.Abs(axisArea.YAxis.Info.Minimum);
			if (axisArea.ReverseVert)
			{
				ScrollValueY = num2;
			}
			else
			{
				ScrollValueY = scrollRangeY - num2 - scrollViewY;
			}
		}
		else
		{
			ScrollValueY = 0.0;
		}
		num = ScrollRangeY / ScrollViewY;
		scrollDpiY = (double)rect.Height * num / ScrollRangeY;
		return num;
	}

	protected override void PrepareInfo(RectangleF rect)
	{
		List<IStiSeries> series = GetSeries();
		if (series.Count <= 0)
		{
			return;
		}
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		ValuesCount = 0;
		IStiAxis stiAxis;
		if (!(this is StiClusteredBarAreaCoreXF))
		{
			IStiAxis xAxis = stiAxisArea.XAxis;
			stiAxis = xAxis;
		}
		else
		{
			IStiAxis xAxis = stiAxisArea.YAxis;
			stiAxis = xAxis;
		}
		IStiAxis stiAxis2 = stiAxis;
		IStiAxis stiAxis3;
		if (!(this is StiClusteredBarAreaCoreXF))
		{
			IStiAxis xAxis = stiAxisArea.XTopAxis;
			stiAxis3 = xAxis;
		}
		else
		{
			IStiAxis xAxis = stiAxisArea.YRightAxis;
			stiAxis3 = xAxis;
		}
		IStiAxis stiAxis4 = stiAxis3;
		IStiAxis stiAxis5;
		if (!(this is StiClusteredBarAreaCoreXF))
		{
			IStiAxis xAxis = stiAxisArea.YAxis;
			stiAxis5 = xAxis;
		}
		else
		{
			IStiAxis xAxis = stiAxisArea.XAxis;
			stiAxis5 = xAxis;
		}
		IStiAxis stiAxis6 = stiAxis5;
		IStiAxis stiAxis7;
		if (!(this is StiClusteredBarAreaCoreXF))
		{
			IStiAxis xAxis = stiAxisArea.YRightAxis;
			stiAxis7 = xAxis;
		}
		else
		{
			IStiAxis xAxis = stiAxisArea.XTopAxis;
			stiAxis7 = xAxis;
		}
		IStiAxis stiAxis8 = stiAxis7;
		stiAxis2.Info.StripLines = new StiStripLinesXF();
		stiAxis6.Info.StripLines = new StiStripLinesXF();
		stiAxis8.Info.StripLines = new StiStripLinesXF();
		bool isDateTimeValues = false;
		for (int i = 0; i < series.Count; i++)
		{
			double?[] array = series[i].Values;
			if (series[i] is StiCandlestickSeries)
			{
				array = ((StiCandlestickSeries)series[i]).ValuesOpen;
			}
			if (array != null)
			{
				ValuesCount = Math.Max(array.Length, ValuesCount);
			}
			if (series[i].Core.IsDateTimeValues)
			{
				isDateTimeValues = true;
			}
		}
		if (this is StiBoxAndWhiskerAreaCoreXF)
		{
			ValuesCount = series.Count;
		}
		PrepareRange(stiAxis2, stiAxis4, stiAxis6, stiAxis8);
		if (!stiAxis6.Range.Auto && stiAxis6.Range.Maximum > stiAxis6.Info.Maximum)
		{
			stiAxis6.Info.Maximum = stiAxis6.Range.Maximum;
		}
		if (!stiAxis6.Range.Auto && stiAxis6.Range.Minimum < stiAxis6.Info.Minimum)
		{
			stiAxis6.Info.Minimum = stiAxis6.Range.Minimum;
		}
		if (!stiAxis8.Range.Auto && (stiAxis8.Range.Maximum > stiAxis8.Info.Maximum || stiAxisArea is StiParetoArea))
		{
			stiAxis8.Info.Maximum = stiAxis8.Range.Maximum;
		}
		if (!stiAxis8.Range.Auto && stiAxis8.Range.Minimum < stiAxis8.Info.Minimum)
		{
			stiAxis8.Info.Minimum = stiAxis8.Range.Minimum;
		}
		bool flag = base.Area.Chart.Series.Count > 0 && base.Area.Chart.Series[0] is StiScatterSeries;
		if (!(stiAxis6.LogarithmicScale && flag))
		{
			CheckStartFromZeroYAxis(stiAxis6);
			CheckStartFromZeroYAxis(stiAxis8);
			CalculateMinimumAndMaximumYAxis(stiAxis6);
			if (stiAxis8.Range.Auto)
			{
				CalculateMinimumAndMaximumYAxis(stiAxis8);
			}
		}
		CalculateMinimumAndMaximumXAxis(stiAxis2);
		CreateStripLinesXAxis(stiAxis2);
		CreateStripLinesYAxis(stiAxis6, isDateTimeValues);
		CreateStripLinesYAxis(stiAxis8, isDateTimeValues);
		CheckStripLinesAndMaximumMinimumXAxis(stiAxis2);
		CheckStripLinesAndMaximumMinimumYAxis(stiAxis6);
		CheckStripLinesAndMaximumMinimumYAxis(stiAxis8);
		if (stiAxisArea is IStiClusteredBarArea)
		{
			RotateStripLines(stiAxis2);
			RotateStripLines(stiAxis6);
			RotateStripLines(stiAxis8);
		}
		stiAxis4.Info = stiAxis2.Info.Clone() as StiAxisInfoXF;
		CheckShowEdgeValues(stiAxis2);
		CheckShowEdgeValues(stiAxis4);
		if (stiAxisArea.ReverseHor)
		{
			RotateStripLines(stiAxisArea.XAxis);
			RotateStripLines(stiAxisArea.XTopAxis);
		}
		if (stiAxisArea.ReverseVert)
		{
			RotateStripLines(stiAxisArea.YAxis);
			RotateStripLines(stiAxisArea.YRightAxis);
		}
		stiAxisArea.XAxis.Info.Dpi = (double)rect.Width / stiAxisArea.XAxis.Info.Range;
		stiAxisArea.XTopAxis.Info.Dpi = (double)rect.Width / stiAxisArea.XTopAxis.Info.Range;
		stiAxisArea.YAxis.Info.Dpi = (double)rect.Height / stiAxisArea.YAxis.Info.Range;
		stiAxisArea.YRightAxis.Info.Dpi = (double)rect.Height / stiAxisArea.YRightAxis.Info.Range;
		CalculateStepX(stiAxisArea.XAxis, rect.Left, rect.Right);
		CalculateStepX(stiAxisArea.XTopAxis, rect.Left, rect.Right);
		CalculateStepY(stiAxisArea.YAxis, rect.Top, rect.Bottom);
		CalculateStepY(stiAxisArea.YRightAxis, rect.Top, rect.Bottom);
		CalculatePositions(stiAxisArea.XAxis, ref stiAxisArea.XAxis.Info.LabelsCollection, (!(stiAxisArea.XAxis.Labels.Step > 0f)) ? 1 : ((int)stiAxisArea.XAxis.Labels.Step));
		CalculatePositions(stiAxisArea.XTopAxis, ref stiAxisArea.XTopAxis.Info.LabelsCollection, (!(stiAxisArea.XTopAxis.Labels.Step > 0f)) ? 1 : ((int)stiAxisArea.XTopAxis.Labels.Step));
		CalculatePositions(stiAxisArea.YAxis, ref stiAxisArea.YAxis.Info.LabelsCollection, (!(stiAxisArea.YAxis.Labels.Step > 1f)) ? 1 : ((int)stiAxisArea.YAxis.Labels.Step));
		CalculatePositions(stiAxisArea.YRightAxis, ref stiAxisArea.YRightAxis.Info.LabelsCollection, (!(stiAxisArea.YRightAxis.Labels.Step > 0f)) ? 1 : ((int)stiAxisArea.YRightAxis.Labels.Step));
		CalculatePositions(stiAxisArea.XAxis, ref stiAxisArea.XAxis.Info.TicksCollection, (stiAxisArea.XAxis.Ticks.Step <= 0) ? 1 : stiAxisArea.XAxis.Ticks.Step, calculationForTicks: true);
		CalculatePositions(stiAxisArea.XTopAxis, ref stiAxisArea.XTopAxis.Info.TicksCollection, (stiAxisArea.XTopAxis.Ticks.Step <= 0) ? 1 : stiAxisArea.XTopAxis.Ticks.Step, calculationForTicks: true);
		CalculatePositions(stiAxisArea.YAxis, ref stiAxisArea.YAxis.Info.TicksCollection, (stiAxisArea.YAxis.Ticks.Step <= 0) ? 1 : stiAxisArea.YAxis.Ticks.Step, calculationForTicks: true);
		CalculatePositions(stiAxisArea.YRightAxis, ref stiAxisArea.YRightAxis.Info.TicksCollection, (stiAxisArea.YRightAxis.Ticks.Step <= 0) ? 1 : stiAxisArea.YRightAxis.Ticks.Step, calculationForTicks: true);
	}

	private void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, List<IStiSeries> seriesCollection)
	{
		List<List<IStiSeries>> list = new List<List<IStiSeries>>();
		Hashtable hashtable = new Hashtable();
		foreach (IStiSeries item in seriesCollection)
		{
			if (IsAcceptableSeries(item.GetType()))
			{
				List<IStiSeries> list2 = hashtable[item.GetType()] as List<IStiSeries>;
				if (list2 == null)
				{
					list2 = new List<IStiSeries>();
					list.Add(list2);
					hashtable.Add(item.GetType(), list2);
				}
				list2.Add(item);
			}
		}
		foreach (List<IStiSeries> item2 in list)
		{
			IStiSeries[] array = item2.ToArray();
			if (array[0] is StiStackedBarSeries || array[0] is StiStackedColumnSeries || array[0] is StiStackedBaseLineSeries)
			{
				List<IStiSeries> list3 = new List<IStiSeries>();
				List<IStiSeries> list4 = new List<IStiSeries>();
				IStiSeries[] array2 = array;
				foreach (IStiSeries stiSeries in array2)
				{
					if (stiSeries.YAxis == StiSeriesYAxis.LeftYAxis)
					{
						list3.Add(stiSeries);
					}
					else
					{
						list4.Add(stiSeries);
					}
				}
				if (list3.Count > 0)
				{
					list3.ToArray()[0].Core.RenderSeries(context, rect, geom, list3.ToArray());
				}
				if (list4.Count > 0)
				{
					list4.ToArray()[0].Core.RenderSeries(context, rect, geom, list4.ToArray());
				}
			}
			else
			{
				array[0].Core.RenderSeries(context, rect, geom, array);
			}
		}
	}

	internal bool IsAutoRangeXAxis(IStiAxis axis)
	{
		if (!axis.Range.Auto && !(axis.Range.Minimum >= axis.Range.Maximum))
		{
			return axis.LogarithmicScale;
		}
		return true;
	}

	internal bool IsAutoRangeYAxis(IStiAxis axis)
	{
		if (!axis.Range.Auto && axis.Range.Maximum != axis.Range.Minimum && !base.Area.IsDefaultSeriesTypeFullStackedColumnSeries)
		{
			return axis.LogarithmicScale;
		}
		return true;
	}

	internal void CalculateMinimumAndMaximumXAxis(IStiAxis axis)
	{
		if (!axis.Range.Auto)
		{
			axis.Info.Maximum = axis.Range.Maximum;
			axis.Info.Minimum = axis.Range.Minimum;
		}
	}

	internal void CalculateMinimumAndMaximumYAxis(ref double min, ref double max)
	{
		double val = 0.1;
		if (GetSeries().Count > 0 && GetSeries()[0].SeriesLabels is StiOutsideEndAxisLabels)
		{
			val = 0.15;
		}
		double num = Math.Min(val, Math.Abs(4.0 / ((max + min) / 2.0)));
		max *= 1.0 + num * (double)Math.Sign(max);
		min *= 1.0 - num * (double)Math.Sign(min);
		if (min == max)
		{
			if (max == 0.0)
			{
				max = 100.0;
			}
			else
			{
				min -= min * 0.1;
				max += max * 0.1;
			}
		}
		double interval = StiStripLineCalculatorXF.GetInterval(min, max, 6);
		StiStripLinesXF stripLines = StiStripLineCalculatorXF.GetStripLines(min, max, interval, asDateTimeValue: false);
		if (stripLines.Count > 0)
		{
			max = stripLines[0].Value;
			min = stripLines[stripLines.Count - 1].Value;
		}
		else
		{
			max = 100.0;
			min = 0.0;
		}
	}

	internal void CalculateMinimumAndMaximumYAxis(IStiAxis axis)
	{
		if (GetSeries().Count > 0 && GetSeries()[0] is StiGanttSeries)
		{
			if (!axis.Range.Auto)
			{
				axis.Info.Maximum = axis.Range.Maximum;
				axis.Info.Minimum = axis.Range.Minimum;
			}
			return;
		}
		if (!axis.StartFromZero && axis.Range.Auto && axis is IStiYAxis)
		{
			double num = (axis.Info.Maximum - axis.Info.Minimum) * 0.05;
			axis.Info.Minimum -= num;
			axis.Info.Maximum += num;
			if (axis.Info.Minimum < 0.0 && axis.Info.Minimum + num >= 0.0)
			{
				axis.Info.Minimum = 0.0;
			}
		}
		else
		{
			double val = 0.1;
			if (GetSeries().Count > 0 && GetSeries()[0].SeriesLabels is StiOutsideEndAxisLabels)
			{
				val = 0.15;
			}
			double num2 = Math.Min(val, Math.Abs(4.0 / ((axis.Info.Maximum + axis.Info.Minimum) / 2.0)));
			axis.Info.Maximum *= 1.0 + num2 * (double)Math.Sign(axis.Info.Maximum);
			axis.Info.Minimum *= 1.0 - num2 * (double)Math.Sign(axis.Info.Minimum);
		}
		if (axis.Info.Minimum == axis.Info.Maximum)
		{
			if (axis.Info.Maximum == 0.0)
			{
				axis.Info.Maximum = 100.0;
				return;
			}
			axis.Info.Minimum -= axis.Info.Minimum * 0.1;
			axis.Info.Maximum += axis.Info.Maximum * 0.1;
		}
	}

	internal string GetArgumentLabel(StiStripLineXF line, IStiSeries series)
	{
		if (line == null)
		{
			return string.Empty;
		}
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		if (stiAxisArea.Core.SeriesOrientation == StiChartSeriesOrientation.Vertical)
		{
			return ((StiXAxisCoreXF)stiAxisArea.XAxis.Core).GetLabelText(line, series);
		}
		return ((StiYAxisCoreXF)stiAxisArea.YAxis.Core).GetLabelText(line);
	}

	public void SwitchOff()
	{
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		stiAxisArea.GridLinesHor.Visible = false;
		stiAxisArea.GridLinesVert.Visible = false;
		stiAxisArea.InterlacingHor.Visible = false;
		stiAxisArea.InterlacingVert.Visible = false;
		stiAxisArea.XAxis.Visible = false;
		stiAxisArea.YAxis.Visible = false;
		stiAxisArea.XTopAxis.Visible = false;
		stiAxisArea.YRightAxis.Visible = false;
		stiAxisArea.YAxis.Range.Auto = true;
		stiAxisArea.XAxis.Range.Auto = true;
	}

	private void Swap(ref float value1, ref float value2)
	{
		float num = value1;
		value1 = value2;
		value2 = num;
	}

	private void Swap(ref double value1, ref double value2)
	{
		double num = value1;
		value1 = value2;
		value2 = num;
	}

	protected virtual void PrepareRange(IStiAxis specXAxis, IStiAxis specXTopAxis, IStiAxis specYAxis, IStiAxis specYRightAxis)
	{
	}

	protected virtual void CreateStripLinesXAxis(IStiAxis axis)
	{
		bool startFromZero = axis.Core.GetStartFromZero();
		int num = ((!startFromZero) ? 1 : 0);
		int num2 = (startFromZero ? (ValuesCount + 1) : ValuesCount);
		for (int i = num; i <= num2; i++)
		{
			axis.Info.StripLines.Add(i, i);
		}
		List<IStiSeries> series = GetSeries();
		foreach (IStiSeries item in series)
		{
			for (int j = 0; j < ValuesCount; j++)
			{
				object obj = null;
				if (item.Arguments != null && j < item.Arguments.Length)
				{
					obj = item.Arguments[j];
					if (obj == null)
					{
						continue;
					}
				}
				if (obj != null)
				{
					int num3 = (startFromZero ? (1 + j) : j);
					if (num3 < axis.Info.StripLines.Count)
					{
						axis.Info.StripLines[num3].ValueObject = obj;
					}
				}
			}
		}
	}

	protected virtual void CheckShowEdgeValues(IStiAxis axis)
	{
		if (axis.Info.StripLines.Count > 0)
		{
			IStiClusteredBarArea stiClusteredBarArea = axis.Area as IStiClusteredBarArea;
			if (!((axis is IStiXTopAxis) ? axis.Area.XTopAxis.ShowEdgeValues : axis.Area.XAxis.ShowEdgeValues) || stiClusteredBarArea != null)
			{
				axis.Info.StripLines[0].ValueObject = null;
				axis.Info.StripLines[axis.Info.StripLines.Count - 1].ValueObject = null;
			}
		}
	}

	protected virtual void CreateStripLinesYAxis(IStiAxis axis, bool isDateTimeValues)
	{
		if (base.Area.IsDefaultSeriesTypeFullStackedColumnSeries || base.Area.IsDefaultSeriesTypeFullStackedBarSeries)
		{
			bool flag = false;
			bool flag2 = false;
			List<IStiSeries> series = GetSeries();
			foreach (IStiSeries item in series)
			{
				double?[] values = item.Values;
				foreach (double? num in values)
				{
					if (num > 0.0)
					{
						flag = true;
					}
					if (num < 0.0)
					{
						flag2 = true;
					}
				}
			}
			double num2 = (flag2 ? (-100) : 0);
			double num3 = (flag ? 100 : 0);
			if (num2 == 0.0 && num3 == 0.0)
			{
				num3 = 100.0;
			}
			double num4 = axis.Labels.Step;
			if (num4 == 0.0)
			{
				num4 = StiStripLineCalculatorXF.GetInterval(num2, num3, 6);
			}
			axis.Info.StripLines = StiStripLineCalculatorXF.GetStripLines(num2, num3, num4, asDateTimeValue: false);
			{
				foreach (StiStripLineXF stripLine in axis.Info.StripLines)
				{
					stripLine.ValueObject = $"{stripLine.ValueObject}%";
				}
				return;
			}
		}
		double num5 = axis.Labels.Step;
		if (num5 > 0.0 && axis.Info.Range > 0.0 && axis.Info.Range / num5 > 500.0)
		{
			num5 = 0.0;
		}
		if (num5 == 0.0)
		{
			num5 = StiStripLineCalculatorXF.GetInterval(axis.Info.Minimum, axis.Info.Maximum, 6);
			if (((StiChart)base.Area.Chart).IsDashboard)
			{
				axis.Labels.CalculatedStep = (float)num5;
			}
		}
		axis.Info.StripLines = StiStripLineCalculatorXF.GetStripLines(axis.Info.Minimum, axis.Info.Maximum, num5, isDateTimeValues);
	}

	protected virtual void CheckStripLinesAndMaximumMinimumXAxis(IStiAxis axis)
	{
		if (axis.Info.StripLines.Count > 0)
		{
			axis.Info.Minimum = axis.Info.StripLines[0].Value;
			axis.Info.Maximum = axis.Info.StripLines[axis.Info.StripLines.Count - 1].Value;
		}
		else
		{
			axis.Info.Minimum = 0.0;
			axis.Info.Maximum = 1.0;
		}
	}

	protected virtual void CheckStripLinesAndMaximumMinimumYAxis(IStiAxis axis)
	{
		if (axis.Info.StripLines.Count > 0)
		{
			axis.Info.Maximum = axis.Info.StripLines[0].Value;
			axis.Info.Minimum = axis.Info.StripLines[axis.Info.StripLines.Count - 1].Value;
		}
		else
		{
			axis.Info.Maximum = 100.0;
			axis.Info.Minimum = 0.0;
		}
	}

	private void CalculateStepX(IStiAxis axis, float topPosition, float bottomPosition)
	{
		if (axis.Info.StripLines.Count >= 2)
		{
			axis.Info.Step = Math.Abs((float)((axis.Info.StripLines[0].Value - axis.Info.StripLines[1].Value) * axis.Info.Dpi));
			axis.Core.CalculateStripPositions(topPosition, bottomPosition);
		}
		else
		{
			axis.Info.Step = 1f;
			axis.Info.StripPositions = new float[0];
		}
	}

	private void CalculateStepY(IStiAxis axis, float topPosition, float bottomPosition)
	{
		if (axis.Info.StripLines.Count >= 2)
		{
			axis.Info.Step = Math.Abs((float)((axis.Info.StripLines[1].Value - axis.Info.StripLines[0].Value) * axis.Info.Dpi));
			axis.Core.CalculateStripPositions(topPosition, bottomPosition);
		}
		else
		{
			axis.Info.Step = 1f;
			axis.Info.StripPositions = new float[0];
		}
	}

	private void CheckStartFromZeroYAxis(IStiAxis axis)
	{
		if (axis.Core.GetStartFromZero())
		{
			if (axis.Info.Maximum < 0.0)
			{
				axis.Info.Maximum = 0.0;
			}
			if (axis.Info.Minimum > 0.0)
			{
				axis.Info.Minimum = 0.0;
			}
		}
	}

	internal void CalculatePositions(IStiAxis axis, ref List<StiStripPositionXF> collection, int step, bool calculationForTicks = false)
	{
		collection = new List<StiStripPositionXF>();
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		bool flag = false;
		if (base.Area is IStiAxisArea && axis is IStiYAxis)
		{
			flag = stiAxisArea.ReverseVert;
		}
		else if (base.Area is IStiAxisArea && axis is IStiXAxis)
		{
			flag = stiAxisArea.ReverseHor;
		}
		int num = 0;
		int num2 = axis.Info.StripPositions.Length;
		for (int i = 0; i < num2; i++)
		{
			if (num == 0)
			{
				StiStripPositionXF stiStripPositionXF = new StiStripPositionXF();
				int num3 = (flag ? (num2 - i - 1) : i);
				stiStripPositionXF.StripLine = axis.Info.StripLines[num3];
				stiStripPositionXF.Position = axis.Info.StripPositions[num3];
				if (flag)
				{
					collection.Insert(0, stiStripPositionXF);
				}
				else
				{
					collection.Add(stiStripPositionXF);
				}
			}
			if ((base.Area.Core is StiScatterAreaCoreXF && axis is IStiXAxis && !axis.LogarithmicScale) || (!(base.Area.Core is StiClusteredBarAreaCoreXF) && !(base.Area.Core is StiScatterAreaCoreXF) && axis is IStiYAxis && !calculationForTicks) || (base.Area.Core is StiClusteredBarAreaCoreXF && axis is IStiXAxis && !calculationForTicks))
			{
				continue;
			}
			if (i == num2 - 1 && num != 0 && axis is IStiXAxis { ShowEdgeValues: not false })
			{
				StiStripPositionXF stiStripPositionXF2 = new StiStripPositionXF();
				int num4 = ((!flag) ? (num2 - 1) : 0);
				stiStripPositionXF2.StripLine = axis.Info.StripLines[num4];
				stiStripPositionXF2.Position = axis.Info.StripPositions[num4];
				if (flag)
				{
					collection.Insert(0, stiStripPositionXF2);
				}
				else
				{
					collection.Add(stiStripPositionXF2);
				}
			}
			num++;
			if (num == step)
			{
				num = 0;
			}
		}
	}

	private double CalculateDivider(IStiAxis axis)
	{
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		int num = 0;
		if (axis.Info.StripLines != null)
		{
			foreach (StiStripLineXF stripLine in axis.Info.StripLines)
			{
				if (stripLine.Value == 0.0)
				{
					return axis.Info.StripPositions[num];
				}
				num++;
			}
		}
		if (axis is IStiYAxis)
		{
			if (stiAxisArea.ReverseVert)
			{
				return (float)((0.0 - axis.Info.Minimum) * axis.Info.Dpi);
			}
			return (axis.Info.StripPositions == null || axis.Info.StripPositions.Length == 0) ? ((float)(axis.Info.Minimum * axis.Info.Dpi)) : ((float)((double)axis.Info.StripPositions[axis.Info.StripPositions.Length - 1] + axis.Info.Minimum * axis.Info.Dpi));
		}
		if (axis is IStiXAxis)
		{
			if (stiAxisArea.ReverseHor)
			{
				return (axis.Info.StripPositions == null || axis.Info.StripPositions.Length == 0) ? ((float)(axis.Info.Minimum * axis.Info.Dpi)) : ((float)((double)axis.Info.StripPositions[axis.Info.StripPositions.Length - 1] + axis.Info.Minimum * axis.Info.Dpi));
			}
			return (0.0 - axis.Info.Minimum) * axis.Info.Dpi;
		}
		return 0.0;
	}

	private static void RotateStripLines(IStiAxis axis)
	{
		StiStripLinesXF stiStripLinesXF = new StiStripLinesXF();
		foreach (StiStripLineXF stripLine in axis.Info.StripLines)
		{
			stiStripLinesXF.Insert(0, stripLine);
		}
		axis.Info.StripLines = stiStripLinesXF;
	}

	public float GetDividerX()
	{
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		if (this is StiClusteredBarAreaCoreXF || this is StiScatterAreaCoreXF)
		{
			return (float)CalculateDivider(stiAxisArea.XAxis);
		}
		return 0f;
	}

	public double GetDividerXD()
	{
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		if (this is StiClusteredBarAreaCoreXF || this is StiScatterAreaCoreXF)
		{
			return CalculateDivider(stiAxisArea.XAxis);
		}
		return 0.0;
	}

	public float GetDividerTopX()
	{
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		if (this is StiClusteredBarAreaCoreXF)
		{
			return (float)CalculateDivider(stiAxisArea.XTopAxis);
		}
		return 0f;
	}

	public float GetDividerY()
	{
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		if (this is StiClusteredBarAreaCoreXF && !(this is StiGanttAreaCoreXF))
		{
			return 0f;
		}
		return (float)CalculateDivider(stiAxisArea.YAxis);
	}

	public float GetDividerRightY()
	{
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		if (this is StiClusteredBarAreaCoreXF && !(this is StiGanttAreaCoreXF))
		{
			return 0f;
		}
		return (float)CalculateDivider(stiAxisArea.YRightAxis);
	}

	public StiAxisAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
