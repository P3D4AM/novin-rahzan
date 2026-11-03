using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiPie3dSeriesCoreXF : StiPieSeriesCoreXF
{
	private StiPie3dSlice[] mPieSlices;

	private float pieHeight;

	public override string LocalizedName => "3D " + StiLocalization.Get("Chart", "Pie");

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] seriesArray)
	{
		InitializePieSlices(geom.Area, rect, seriesArray, context.Options.Zoom);
		DrawBottoms(geom);
		if (pieHeight > 0f)
		{
			DrawSliceSides(geom);
		}
		DrawTops(geom);
		DrawLabels(geom, context);
	}

	private void DrawLabels(StiAreaGeom areaGeom, StiContext context)
	{
		if (mPieSlices != null)
		{
			for (int i = 0; i < mPieSlices.Length; i++)
			{
				StiPie3dSlice stiPie3dSlice = mPieSlices[i];
				stiPie3dSlice.DrawLabels(areaGeom, context);
			}
		}
	}

	private void DrawTops(StiAreaGeom areaGeom)
	{
		if (mPieSlices != null)
		{
			for (int i = 0; i < mPieSlices.Length; i++)
			{
				StiPie3dSlice stiPie3dSlice = mPieSlices[i];
				stiPie3dSlice.DrawTopPieSliceGeom(areaGeom);
			}
		}
	}

	private void DrawSliceSides(StiAreaGeom areaGeom)
	{
		if (mPieSlices == null)
		{
			return;
		}
		List<StiPie3dSlice> list = mPieSlices.ToList();
		if (list.Count == 0)
		{
			return;
		}
		StiPie3dSlice stiPie3dSlice = null;
		stiPie3dSlice = list[0];
		stiPie3dSlice.DrawSides(areaGeom);
		int num = 1;
		int num2 = list.Count - 1;
		while (num < num2)
		{
			StiPie3dSlice stiPie3dSlice2 = list[num2];
			float num3 = stiPie3dSlice2.StartAngle - 90f;
			if (num3 > 180f || num3 < 0f)
			{
				num3 = 0f;
			}
			StiPie3dSlice stiPie3dSlice3 = list[num];
			float num4 = (450f - stiPie3dSlice3.EndAngle) % 360f;
			if (num4 > 180f || num4 < 0f)
			{
				num4 = 0f;
			}
			if (num4 >= num3)
			{
				stiPie3dSlice3.DrawSides(areaGeom);
				num++;
			}
			else if (num4 < num3)
			{
				stiPie3dSlice2.DrawSides(areaGeom);
				num2--;
			}
		}
		stiPie3dSlice = list[num2];
		stiPie3dSlice.DrawSides(areaGeom);
	}

	private void DrawBottoms(StiAreaGeom areaGeom)
	{
		if (mPieSlices != null)
		{
			for (int i = 0; i < mPieSlices.Length; i++)
			{
				StiPie3dSlice stiPie3dSlice = mPieSlices[i];
				stiPie3dSlice.DrawBottomPieSliceGeom(areaGeom);
			}
		}
	}

	protected virtual void InitializePieSlices(IStiArea area, RectangleF mainRect, IStiSeries[] seriesArray, float zoom)
	{
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Expected O, but got Unknown
		float num = 1.61f;
		RectangleF empty = RectangleF.Empty;
		if (mainRect.Width / mainRect.Height > num)
		{
			float num2 = mainRect.Height * num;
			empty = new RectangleF((mainRect.Width - num2) / 2f, 0f, num2, mainRect.Height);
		}
		else
		{
			float num3 = mainRect.Width / num;
			empty = new RectangleF(0f, (mainRect.Height - num3) / 2f, mainRect.Width, num3);
		}
		empty = new RectangleF(empty.X + empty.Width * 0.025f, empty.Y + empty.Height * 0.025f, empty.Width * 0.95f, empty.Height * 0.95f);
		List<StiPie3dSlice> list = new List<StiPie3dSlice>();
		float gradPerValue = GetGradPerValue(seriesArray);
		IStiPie3dSeries stiPie3dSeries = seriesArray[0] as IStiPie3dSeries;
		float num4 = stiPie3dSeries.StartAngle % 360f;
		int colorCount = GetColorCount(seriesArray);
		pieHeight = stiPie3dSeries.Options3D.Height * zoom;
		float largestDisplacement = GetLargestDisplacement(zoom);
		SizeF topEllipseSize = GetTopEllipseSize(empty, zoom);
		int num5 = 0;
		int num6 = -1;
		for (int i = 0; i < seriesArray.Length; i++)
		{
			IStiPie3dSeries stiPie3dSeries2 = (IStiPie3dSeries)seriesArray[i];
			for (int j = 0; j < stiPie3dSeries2.Values.Length; j++)
			{
				double valueOrDefault = stiPie3dSeries2.Values[j].GetValueOrDefault();
				if (valueOrDefault != 0.0)
				{
					float num7 = (float)((double)gradPerValue * Math.Abs(valueOrDefault));
					StiBrush val = stiPie3dSeries2.Brush;
					if (stiPie3dSeries2.AllowApplyBrush)
					{
						val = stiPie3dSeries2.Core.GetSeriesBrush(num5, colorCount);
						val = stiPie3dSeries2.ProcessSeriesBrushes(j, val);
					}
					if (stiPie3dSeries2.Options3D.Opacity >= 0f && stiPie3dSeries2.Options3D.Opacity < 1f)
					{
						Color color = StiBrush.ToColor(val);
						color = Color.FromArgb((int)(255f * stiPie3dSeries2.Options3D.Opacity), color.R, color.G, color.B);
						val = (StiBrush)new StiSolidBrush(color);
					}
					Color borderColor = stiPie3dSeries2.BorderColor;
					if (stiPie3dSeries2.AllowApplyBorderColor)
					{
						borderColor = (Color)stiPie3dSeries2.Core.GetSeriesBorderColor(num5, colorCount);
					}
					float num8 = GetLargestDisplacement(zoom);
					float num9 = GetLargestDisplacement(zoom);
					if (num8 > 0f)
					{
						SizeF sliceDisplacement = GetSliceDisplacement(num4 + num7 / 2f, num8, num9);
						num8 = sliceDisplacement.Width;
						num9 = sliceDisplacement.Height;
					}
					StiPie3dSlice item = new StiPie3dSlice(rectPieSlilce: new RectangleF(empty.X + 2f * largestDisplacement + num8, empty.Y + 2f * largestDisplacement + num9, topEllipseSize.Width, topEllipseSize.Height), area: area, value: valueOrDefault, argumentText: GetArgumentText(stiPie3dSeries2, j), tag: stiPie3dSeries2.Core.GetTag(j), index: j, series: stiPie3dSeries2, pieHeight: pieHeight, startAngle: num4 % 360f, sweepAngle: num7, seriesBrush: val, borderColor: borderColor, colorIndex: num5, colorCount: colorCount);
					if (num6 > -1 || (num4 <= 270f && num4 + num7 > 270f) || (num4 >= 270f && num4 + num7 > 630f))
					{
						num6++;
						list.Insert(num6, item);
					}
					else
					{
						list.Add(item);
					}
					num4 += num7;
				}
				num5++;
				if (num4 > 360f)
				{
					num4 -= 360f;
				}
			}
		}
		mPieSlices = list.ToArray();
	}

	private SizeF GetTopEllipseSize(RectangleF boundingRect, float zoom)
	{
		float largestDisplacement = GetLargestDisplacement(zoom);
		float num = boundingRect.Width - 4f * largestDisplacement;
		float num2 = boundingRect.Height - 4f * largestDisplacement - pieHeight;
		if (num < 1f)
		{
			num = 1f;
		}
		if (num2 < 1f)
		{
			num2 = 1f;
		}
		return new SizeF(num, num2);
	}

	private SizeF GetLargestDisplacementEllipseSize(float zoom)
	{
		float largestDisplacement = GetLargestDisplacement(zoom);
		return new SizeF(largestDisplacement, largestDisplacement);
	}

	protected SizeF GetSliceDisplacement(float angle, float xDisplacement, float yDisplacement)
	{
		if (xDisplacement == 0f)
		{
			return SizeF.Empty;
		}
		float width = (float)((double)xDisplacement * Math.Cos((double)angle * Math.PI / 180.0));
		float height = (float)((double)yDisplacement * Math.Sin((double)angle * Math.PI / 180.0));
		return new SizeF(width, height);
	}

	private float GetLargestDisplacement(float zoom)
	{
		return ((IStiPie3dSeries)base.Series).Options3D.Distance * zoom;
	}

	public StiPie3dSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
