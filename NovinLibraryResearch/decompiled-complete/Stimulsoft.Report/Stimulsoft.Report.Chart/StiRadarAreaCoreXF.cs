using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public abstract class StiRadarAreaCoreXF : StiAreaCoreXF
{
	internal int ValuesCount;

	internal List<PointF> Points = new List<PointF>();

	internal List<object> Arguments = new List<object>();

	internal PointF CenterPoint = PointF.Empty;

	public override void ApplyStyle(IStiChartStyle style)
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		base.ApplyStyle(style);
		IStiRadarArea stiRadarArea = base.Area as IStiRadarArea;
		if (stiRadarArea.AllowApplyStyle)
		{
			if (stiRadarArea.InterlacingHor != null)
			{
				stiRadarArea.InterlacingHor.Core.ApplyStyle(style);
			}
			if (stiRadarArea.InterlacingVert != null)
			{
				stiRadarArea.InterlacingVert.Core.ApplyStyle(style);
			}
			if (stiRadarArea.GridLinesHor != null)
			{
				stiRadarArea.GridLinesHor.Core.ApplyStyle(style);
			}
			if (stiRadarArea.GridLinesVert != null)
			{
				stiRadarArea.GridLinesVert.Core.ApplyStyle(style);
			}
			if (stiRadarArea.XAxis != null)
			{
				stiRadarArea.XAxis.Core.ApplyStyle(style);
			}
			if (stiRadarArea.YAxis != null)
			{
				stiRadarArea.YAxis.Core.ApplyStyle(style);
			}
			stiRadarArea.Brush = (StiBrush)new StiEmptyBrush();
		}
	}

	public override StiCellGeom Render(StiContext context, RectangleF areaRect)
	{
		IStiRadarArea stiRadarArea = base.Area as IStiRadarArea;
		RectangleF rect = areaRect;
		rect = CenterArea(rect);
		PrepareInfo(rect);
		rect = MeasureLabels(context, rect);
		rect = CenterArea(rect);
		rect = new RectangleF(areaRect.X + (areaRect.Width - rect.Width) / 2f, areaRect.Y + (areaRect.Height - rect.Height) / 2f, rect.Width, rect.Height);
		PrepareInfo(rect);
		StiRadarAreaGeom stiRadarAreaGeom = new StiRadarAreaGeom(base.Area, rect, ValuesCount);
		if (stiRadarArea.YAxis.YCore.Render(context, rect) is StiRadarAxisGeom item)
		{
			stiRadarAreaGeom.CreateChildGeoms();
			stiRadarAreaGeom.ChildGeoms.Add(item);
		}
		if (base.Area.Chart.Series.Count > 0)
		{
			RenderArguments(context, stiRadarAreaGeom, base.Area.Chart.Series[0]);
		}
		List<IStiSeries> series = GetSeries();
		RenderSeries(context, rect, stiRadarAreaGeom, series);
		return stiRadarAreaGeom;
	}

	private static RectangleF CenterArea(RectangleF rect)
	{
		if (rect.Width > rect.Height)
		{
			rect.Width = rect.Height;
		}
		else
		{
			rect.Height = rect.Width;
		}
		return rect;
	}

	public RectangleF MeasureLabels(StiContext context, RectangleF rect)
	{
		if (ValuesCount == 0)
		{
			return rect;
		}
		IStiRadarArea stiRadarArea = base.Area as IStiRadarArea;
		if (stiRadarArea.XAxis != null && stiRadarArea.XAxis.Visible && stiRadarArea.YAxis.Info.StripPositions != null)
		{
			float num = stiRadarArea.YAxis.Info.StripPositions[stiRadarArea.YAxis.Info.StripPositions.Length - 1] + 4f * context.Options.Zoom;
			int num2 = 0;
			float num3 = 360f / (float)ValuesCount;
			float num4 = 0f;
			RectangleF a = rect;
			foreach (PointF point2 in Points)
			{
				object value = ((num2 < Arguments.Count) ? Arguments[num2] : null);
				PointF point = new PointF(CenterPoint.X + num * point2.X, CenterPoint.Y + num * point2.Y);
				string labelText = ((StiXRadarAxisCoreXF)stiRadarArea.XAxis.Core).GetLabelText(value);
				RectangleF labelRect = stiRadarArea.XAxis.XCore.GetLabelRect(context, point, labelText, num4);
				if (!labelRect.IsEmpty)
				{
					a = RectangleF.Union(a, labelRect);
				}
				num4 += num3;
				num2++;
			}
			float num5 = rect.Left - a.Left;
			float num6 = a.Right - rect.Right;
			float num7 = rect.Top - a.Top;
			float num8 = a.Bottom - rect.Bottom;
			if (num5 > 0f)
			{
				rect.Width -= num5;
			}
			if (num6 > 0f)
			{
				rect.Width -= num6;
			}
			if (num7 > 0f)
			{
				rect.Height -= num7;
			}
			if (num8 > 0f)
			{
				rect.Height -= num8;
			}
		}
		return rect;
	}

	public void RenderArguments(StiContext context, StiRadarAreaGeom geom, IStiSeries series)
	{
		if (ValuesCount == 0)
		{
			return;
		}
		IStiRadarArea stiRadarArea = base.Area as IStiRadarArea;
		if (stiRadarArea.XAxis != null && stiRadarArea.XAxis.Visible && stiRadarArea.YAxis.Info.StripPositions != null)
		{
			float num = stiRadarArea.YAxis.Info.StripPositions[stiRadarArea.YAxis.Info.StripPositions.Length - 1] + 4f * context.Options.Zoom;
			int num2 = 0;
			float num3 = 360f / (float)ValuesCount;
			float num4 = 0f;
			foreach (PointF point2 in Points)
			{
				object argument = ((num2 < Arguments.Count) ? Arguments[num2] : null);
				PointF point = new PointF(CenterPoint.X + num * point2.X, CenterPoint.Y + num * point2.Y);
				point.X -= geom.ClientRectangle.X;
				point.Y -= geom.ClientRectangle.Y;
				StiXRadarAxisLabelGeom stiXRadarAxisLabelGeom = stiRadarArea.XAxis.XCore.RenderLabel(context, series, point, argument, num4, num2, ValuesCount);
				if (stiXRadarAxisLabelGeom != null)
				{
					geom.CreateChildGeoms();
					geom.ChildGeoms.Add(stiXRadarAxisLabelGeom);
				}
				num4 += num3;
				num2++;
			}
		}
		if (!stiRadarArea.XAxis.Labels.RotationLabels && stiRadarArea.XAxis.Labels.PreventIntersection)
		{
			CheckIntersectionLabels(geom);
		}
	}

	protected virtual void CheckIntersectionLabels(StiAreaGeom geom)
	{
		List<StiCellGeom> childGeoms = geom.ChildGeoms;
		PointF pointF = new PointF(geom.ClientRectangle.Width / 2f, geom.ClientRectangle.Height / 2f);
		List<StiXRadarAxisLabelGeom> list = new List<StiXRadarAxisLabelGeom>();
		List<StiXRadarAxisLabelGeom> list2 = new List<StiXRadarAxisLabelGeom>();
		foreach (StiCellGeom item in childGeoms)
		{
			if (item is StiXRadarAxisLabelGeom stiXRadarAxisLabelGeom)
			{
				if ((float)stiXRadarAxisLabelGeom.GetDrawRectangle().Y < pointF.Y)
				{
					list.Add(stiXRadarAxisLabelGeom);
				}
				else
				{
					list2.Add(stiXRadarAxisLabelGeom);
				}
			}
		}
		CheckTopIntersectionLabels(list.OrderBy((StiXRadarAxisLabelGeom x) => x.GetDrawRectangle().Y).ToList());
		CheckBottomIntersectionLabels(list2.OrderBy((StiXRadarAxisLabelGeom x) => x.GetDrawRectangle().Y).ToList());
	}

	private void CheckTopIntersectionLabels(List<StiXRadarAxisLabelGeom> labelGeoms)
	{
		int count = labelGeoms.Count;
		bool flag = true;
		int num = 0;
		while (flag && num < 20)
		{
			num++;
			for (int i = 0; i < count; i++)
			{
				for (int j = 0; j < count; j++)
				{
					if (j == i)
					{
						continue;
					}
					Rectangle drawRectangle = labelGeoms[i].GetDrawRectangle();
					Rectangle drawRectangle2 = labelGeoms[j].GetDrawRectangle();
					if (drawRectangle.IntersectsWith(drawRectangle2) && drawRectangle.IntersectsWith(drawRectangle2))
					{
						if (drawRectangle.Y < drawRectangle2.Y)
						{
							labelGeoms[i].Point = new PointF(labelGeoms[i].Point.X, labelGeoms[i].Point.Y - 1f);
						}
						else
						{
							labelGeoms[j].Point = new PointF(labelGeoms[j].Point.X, labelGeoms[j].Point.Y - 1f);
						}
					}
				}
			}
		}
	}

	private void CheckBottomIntersectionLabels(List<StiXRadarAxisLabelGeom> labelGeoms)
	{
		int count = labelGeoms.Count;
		bool flag = true;
		int num = 0;
		while (flag && num < 20)
		{
			num++;
			for (int i = 0; i < count; i++)
			{
				for (int j = 0; j < count; j++)
				{
					if (j == i)
					{
						continue;
					}
					Rectangle drawRectangle = labelGeoms[i].GetDrawRectangle();
					Rectangle drawRectangle2 = labelGeoms[j].GetDrawRectangle();
					if (drawRectangle.IntersectsWith(drawRectangle2) && drawRectangle.IntersectsWith(drawRectangle2))
					{
						if (drawRectangle.Y > drawRectangle2.Y)
						{
							labelGeoms[i].Point = new PointF(labelGeoms[i].Point.X, labelGeoms[i].Point.Y + 1f);
						}
						else
						{
							labelGeoms[j].Point = new PointF(labelGeoms[j].Point.X, labelGeoms[j].Point.Y + 1f);
						}
					}
				}
			}
		}
	}

	public void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, List<IStiSeries> seriesCollection)
	{
		List<List<IStiSeries>> list = new List<List<IStiSeries>>();
		Hashtable hashtable = new Hashtable();
		foreach (IStiSeries item in seriesCollection)
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
		foreach (List<IStiSeries> item2 in list)
		{
			IStiSeries[] array = item2.ToArray();
			array[0].Core.RenderSeries(context, rect, geom, array);
		}
	}

	protected override void PrepareInfo(RectangleF rect)
	{
		IStiRadarArea stiRadarArea = base.Area as IStiRadarArea;
		ValuesCount = 0;
		List<IStiSeries> series = GetSeries();
		if (series.Count > 0)
		{
			for (int i = 0; i < series.Count; i++)
			{
				double?[] values = series[i].Values;
				if (values != null)
				{
					ValuesCount = Math.Max(values.Length, ValuesCount);
				}
			}
		}
		Arguments = new List<object>();
		if (series.Count > 0)
		{
			IStiSeries stiSeries = series[0];
			foreach (IStiSeries item3 in series)
			{
				if (item3.Arguments.Length == ValuesCount)
				{
					stiSeries = item3;
					break;
				}
			}
			for (int j = 0; j < ValuesCount; j++)
			{
				if (j < stiSeries.Arguments.Length)
				{
					string item = ((stiSeries.Arguments[j] == null) ? string.Empty : stiSeries.Arguments[j].ToString());
					Arguments.Add(item);
				}
			}
		}
		else
		{
			Arguments.Add("1");
			Arguments.Add("2");
			Arguments.Add("3");
			Arguments.Add("4");
			Arguments.Add("5");
		}
		CenterPoint = new PointF(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f);
		decimal num = ((ValuesCount <= 0) ? 72m : (360m / (decimal)ValuesCount));
		Points = new List<PointF>();
		decimal num2 = default(decimal);
		while (num2 < 360m)
		{
			float num3 = (float)((double)((float)num2 - 90f) * Math.PI / 180.0);
			PointF item2 = new PointF((float)Math.Cos(num3), (float)Math.Sin(num3));
			Points.Add(item2);
			num2 += num;
			if (Points.Count == ValuesCount)
			{
				break;
			}
		}
		double num4 = 0.0;
		double num5 = 0.0;
		if (stiRadarArea.YAxis.Range.Auto)
		{
			bool flag = true;
			for (int k = 0; k < series.Count; k++)
			{
				double?[] values2 = series[k].Values;
				if (values2 == null)
				{
					continue;
				}
				double?[] array = values2;
				for (int l = 0; l < array.Length; l++)
				{
					double? num6 = array[l];
					if (num6.HasValue)
					{
						if (flag)
						{
							num4 = num6.Value;
							num5 = num6.Value;
							flag = false;
						}
						else
						{
							num4 = Math.Min(num4, num6.Value);
							num5 = Math.Max(num5, num6.Value);
						}
					}
				}
			}
			if (series.Count == 0)
			{
				num5 = 1.0;
			}
			if (Math.Abs(num4) > num5)
			{
				num5 = Math.Abs(num4);
			}
			if (num5 == 0.0 && num4 == 0.0)
			{
				num5 = 1.0;
			}
			num4 = 0.0;
		}
		else
		{
			num4 = stiRadarArea.YAxis.Range.Minimum;
			num5 = stiRadarArea.YAxis.Range.Maximum;
		}
		CreateStripLinesAxis(stiRadarArea.YAxis, (float)num4, (float)num5);
		if (stiRadarArea.YAxis.Info.StripLines != null && stiRadarArea.YAxis.Info.StripLines.Count > 0)
		{
			stiRadarArea.YAxis.Info.Minimum = stiRadarArea.YAxis.Info.StripLines[stiRadarArea.YAxis.Info.StripLines.Count - 1].Value;
			stiRadarArea.YAxis.Info.Maximum = stiRadarArea.YAxis.Info.StripLines[0].Value;
		}
		stiRadarArea.YAxis.Info.Dpi = (double)rect.Height * 0.5 / stiRadarArea.YAxis.Info.Range;
		CalculateStep(stiRadarArea.YAxis, 0f, rect.Height / 2f);
		CalculatePositions(stiRadarArea.YAxis, ref stiRadarArea.YAxis.Info.LabelsCollection, (!(stiRadarArea.YAxis.Labels.Step > 0f)) ? 1 : ((int)stiRadarArea.YAxis.Labels.Step));
		CalculatePositions(stiRadarArea.YAxis, ref stiRadarArea.YAxis.Info.TicksCollection, (stiRadarArea.YAxis.Ticks.Step <= 0) ? 1 : stiRadarArea.YAxis.Ticks.Step, calculationForTicks: true);
	}

	protected virtual void CreateStripLinesAxis(IStiYRadarAxis axis, float minimum, float maximum)
	{
		IStiRadarArea stiRadarArea = base.Area as IStiRadarArea;
		double num = stiRadarArea.YAxis.Labels.Step;
		if (num == 0.0)
		{
			num = StiStripLineCalculatorXF.GetInterval(minimum, maximum, 6);
		}
		stiRadarArea.YAxis.Info.StripLines = StiStripLineCalculatorXF.GetStripLines(minimum, maximum, num, asDateTimeValue: false);
		foreach (StiStripLineXF stripLine in stiRadarArea.YAxis.Info.StripLines)
		{
			stripLine.ValueObject = $"{stripLine.ValueObject}";
		}
	}

	private void CalculateStep(IStiYRadarAxis axis, float topPosition, float bottomPosition)
	{
		if (axis.Info.StripLines.Count >= 2)
		{
			axis.Info.Step = Math.Abs((float)((axis.Info.StripLines[1].Value - axis.Info.StripLines[0].Value) * axis.Info.Dpi));
			axis.YCore.CalculateStripPositions(topPosition, bottomPosition);
		}
		else
		{
			axis.Info.Step = 1f;
		}
	}

	internal void CalculatePositions(IStiYRadarAxis axis, ref List<StiStripPositionXF> collection, int step, bool calculationForTicks = false)
	{
		collection = new List<StiStripPositionXF>();
		int num = 0;
		if (axis.Info.StripPositions == null)
		{
			return;
		}
		int num2 = axis.Info.StripPositions.Length;
		for (int i = 0; i < num2; i++)
		{
			if (num == 0)
			{
				StiStripPositionXF stiStripPositionXF = new StiStripPositionXF();
				int num3 = i;
				if (axis.Info.StripLines.Count > 0 && axis.Info.StripPositions.Length != 0)
				{
					stiStripPositionXF.StripLine = axis.Info.StripLines[num3];
					stiStripPositionXF.Position = axis.Info.StripPositions[num3];
				}
				collection.Add(stiStripPositionXF);
			}
			num++;
			if (num == step || !calculationForTicks)
			{
				num = 0;
			}
		}
	}

	public StiRadarAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
