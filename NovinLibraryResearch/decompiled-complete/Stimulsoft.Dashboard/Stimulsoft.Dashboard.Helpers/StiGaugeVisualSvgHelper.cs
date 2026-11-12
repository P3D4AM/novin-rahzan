using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components.Gauge;
using Stimulsoft.Dashboard.Render;
using Stimulsoft.Dashboard.Visuals.Gauge;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Dashboard.Visuals;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Gauge;

namespace Stimulsoft.Dashboard.Helpers;

public class StiGaugeVisualSvgHelper : IStiGaugeVisualSvgHelper
{
	public void WriteGauge(XmlTextWriter writer, StiSvgData svgData, bool needAnimation, ref bool needToScroll, ref int contentHeight, bool isStyleSample = false)
	{
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		RectangleF rect = new RectangleF((float)svgData.X, (float)svgData.Y, (float)svgData.Width, (float)svgData.Height);
		StiGaugeElement stiGaugeElement = svgData.Component as StiGaugeElement;
		StiDataTable dataTable = GetDataTable(stiGaugeElement);
		if (dataTable == null)
		{
			return;
		}
		SizeD size = new SizeD(svgData.Width, svgData.Height);
		IEnumerable<StiGaugeIteration> enumerable = new StiGaugeElementBuilder().Render(stiGaugeElement, size, dataTable);
		List<RectangleD> locationRectangles = StiElementLocationHelper.GetLocationRectangles(RectangleD.CreateFromRectangle(rect), enumerable.Count(), stiGaugeElement);
		if (enumerable.Count() > 1)
		{
			enumerable = new StiGaugeElementBuilder().Render(stiGaugeElement, new SizeD(locationRectangles[0].Width, locationRectangles[0].Height), dataTable);
		}
		if (enumerable == null || !enumerable.Any())
		{
			return;
		}
		if (needToScroll)
		{
			needToScroll = StiElementLocationHelper.GetSumRectangle(locationRectangles).ToRectangleF().Height > rect.Height + rect.Top + 5f;
			if (needToScroll)
			{
				rect.Width -= 15f;
				locationRectangles = StiElementLocationHelper.GetLocationRectangles(RectangleD.CreateFromRectangle(rect), enumerable.Count(), stiGaugeElement);
				enumerable = new StiGaugeElementBuilder().Render(stiGaugeElement, new SizeD(locationRectangles[0].Width, locationRectangles[0].Height), dataTable);
				RectangleF rectangleF = StiElementLocationHelper.GetSumRectangle(locationRectangles).ToRectangleF();
				rect = rectangleF;
				contentHeight = Convert.ToInt32(rect.Height);
			}
		}
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				double? titleHeight = GetTitleHeight(val2, enumerable, RectangleD.CreateFromRectangle(rect), stiGaugeElement);
				double valueOrDefault = titleHeight.GetValueOrDefault(0.0);
				RectangleD rectangleD = locationRectangles[0];
				double titleMinFontSize = GetTitleMinFontSize(val2, enumerable, stiGaugeElement.Font, rectangleD.Width, rectangleD.Height);
				int num = 0;
				foreach (StiGaugeIteration item in enumerable)
				{
					RectangleF rectangleF2 = locationRectangles[num].ToRectangleF();
					if (titleHeight.HasValue)
					{
						RectangleD rect2 = new RectangleD(rectangleF2.X, rectangleF2.Y, rectangleF2.Width, valueOrDefault);
						PaintTitle(stiGaugeElement, val2, writer, rect2, item.Series, titleMinFontSize);
					}
					StiSvgData stiSvgData = default(StiSvgData);
					stiSvgData.X = rectangleF2.X / 2f;
					stiSvgData.Y = ((double)rectangleF2.Y + valueOrDefault) / 2.0;
					stiSvgData.Width = rectangleF2.Width;
					stiSvgData.Height = (double)rectangleF2.Height - valueOrDefault;
					stiSvgData.Component = item.Gauge;
					StiSvgData svgData2 = stiSvgData;
					if (IsEmptyData(stiGaugeElement, dataTable) && !isStyleSample)
					{
						StiGauge emptyDataGauge = GetEmptyDataGauge(stiGaugeElement);
						if (emptyDataGauge != null)
						{
							svgData2.Component = emptyDataGauge;
						}
					}
					StiGaugeSvgHelper.WriteGauge(writer, svgData2, needAnimation, isStyleSample);
					stiGaugeElement.PreviousAnimations = ((StiGauge)svgData2.Component).PreviousAnimations;
					num++;
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private bool IsEmptyData(IStiElement element, StiDataTable dataTable)
	{
		if (dataTable != null && dataTable.IsEmpty)
		{
			return element.IsQuerable;
		}
		return false;
	}

	private static StiDataTable GetDataTable(IStiElement element)
	{
		StiDataTable stiDataTable = (element as IStiManuallyEnteredData)?.GetManuallyEnteredDataTable();
		if (stiDataTable != null)
		{
			return stiDataTable;
		}
		return StiElementDataCache.TryToGetOrCreate(element);
	}

	public void WriteGauge(XmlTextWriter writer, StiSvgData svgData, bool needAnimation, bool isStyleSample = false)
	{
		bool needToScroll = false;
		int contentHeight = 0;
		WriteGauge(writer, svgData, needAnimation, ref needToScroll, ref contentHeight, isStyleSample);
	}

	private void PaintTitle(StiGaugeElement element, Graphics g, XmlTextWriter writer, RectangleD rect, string titleText, double size)
	{
		if (string.IsNullOrEmpty(titleText))
		{
			return;
		}
		Font val = StiFontUtils.ChangeFontSize(element.Font, (float)size);
		try
		{
			SizeD sizeD = StiTitleMeasureHelper.MeasureTitle(g, rect, element.Font, titleText);
			RectangleD rectangleD = new RectangleD(rect.X, rect.Y, rect.Width, sizeD.Height);
			if (!string.IsNullOrWhiteSpace(titleText))
			{
				Color foreColor = StiDashboardExpressionHelper.GetForeColor(element, element.ForeColor);
				Color color = ((foreColor != Color.Transparent) ? foreColor : StiDashboardStyleHelper.GetForeColor(element));
				SizeF sizeF = g.MeasureString(titleText, val);
				StiGaugeSvgHelper.WriteText(pointF: new PointF((float)(rectangleD.X + rect.Width / 2.0 - 0.39 * (double)sizeF.Width), (float)(rectangleD.Y + (double)(sizeF.Height / 2f))), writer: writer, text: titleText, font: val, foreground: new StiSolidBrush(color), size: (float)size);
			}
			rect.Y += rectangleD.Height;
			rect.Height -= rectangleD.Height;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	internal static double GetTitleMinFontSize(Graphics g, IEnumerable<StiGaugeIteration> iterations, Font font, double width, double height)
	{
		float? num = null;
		RectangleD rect = new RectangleD(0.0, 0.0, width, height);
		foreach (StiGaugeIteration iteration in iterations)
		{
			float num2 = StiAutoSizeTextHelper.MeasureFontSize(g, iteration.Series, rect, font);
			num = ((!num.HasValue) ? num2 : Math.Min(num.GetValueOrDefault(), num2));
		}
		return num.GetValueOrDefault();
	}

	internal static double? GetTitleHeight(Graphics g, IEnumerable<StiGaugeIteration> iterations, RectangleD rect, StiGaugeElement element)
	{
		StiGaugeIteration stiGaugeIteration = iterations.FirstOrDefault((StiGaugeIteration c) => !string.IsNullOrEmpty(c.Series));
		if (stiGaugeIteration == null)
		{
			return null;
		}
		double height = StiTitleMeasureHelper.MeasureTitle(g, rect, element.Font, stiGaugeIteration.Series).Height;
		if (height > rect.Height / 2.0)
		{
			return null;
		}
		return height;
	}

	public StiGauge GetEmptyDataGauge(StiGaugeElement gaugeElement)
	{
		return gaugeElement.Type switch
		{
			StiGaugeType.FullCircular => GetFullCircularEmptyDataGauge(gaugeElement), 
			StiGaugeType.HalfCircular => GetHalfCircularEmptyDataGauge(gaugeElement), 
			StiGaugeType.Linear => GetLinearEmptyDataGauge(gaugeElement), 
			StiGaugeType.HorizontalLinear => GetHorizontalLinearEmptyDataGauge(gaugeElement), 
			StiGaugeType.Bullet => GetBulletEmptyDataGauge(gaugeElement), 
			_ => null, 
		};
	}

	private StiGauge GetBulletEmptyDataGauge(StiGaugeElement gaugeElement)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		Color dataEmptyColor = StiDashboardStyleHelper.GetDataEmptyColor(gaugeElement);
		Color dataEmptyForeColor = StiDashboardStyleHelper.GetDataEmptyForeColor(gaugeElement);
		StiGauge stiGauge = new StiGauge();
		StiLinearScale stiLinearScale = new StiLinearScale
		{
			AllowApplyStyle = false,
			RelativeHeight = 0.85f,
			Orientation = (Orientation)0,
			StartWidth = 0.1f,
			EndWidth = 0.1f,
			Maximum = 1f,
			MajorInterval = 1f,
			BorderBrush = new StiEmptyBrush(),
			Brush = new StiSolidBrush(dataEmptyColor)
		};
		StiLinearTickLabelMajor element = new StiLinearTickLabelMajor
		{
			AllowApplyStyle = false,
			TextBrush = new StiSolidBrush(dataEmptyForeColor)
		};
		stiLinearScale.Items.Add(element);
		StiLinearTickLabelCustom stiLinearTickLabelCustom = new StiLinearTickLabelCustom
		{
			Offset = 0.1f,
			Font = new Font("Arial", 14f),
			TextBrush = new StiSolidBrush(dataEmptyForeColor),
			Placement = StiPlacement.Inside,
			TextObj = StiLocalization.Get("Dashboard", "Blank")
		};
		stiLinearTickLabelCustom.Values.Add(new StiLinearTickLabelCustomValue(0.5f, StiLocalization.Get("Dashboard", "Blank")));
		stiLinearScale.Items.Add(stiLinearTickLabelCustom);
		stiGauge.Scales.Add(stiLinearScale);
		return stiGauge;
	}

	private StiGauge GetHorizontalLinearEmptyDataGauge(StiGaugeElement gaugeElement)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		Color dataEmptyColor = StiDashboardStyleHelper.GetDataEmptyColor(gaugeElement);
		Color dataEmptyForeColor = StiDashboardStyleHelper.GetDataEmptyForeColor(gaugeElement);
		StiGauge stiGauge = new StiGauge();
		StiLinearScale stiLinearScale = new StiLinearScale
		{
			AllowApplyStyle = false,
			RelativeHeight = 0.92f,
			Orientation = (Orientation)0,
			StartWidth = 0.1f,
			EndWidth = 0.1f,
			Maximum = 1f,
			MajorInterval = 1f,
			BorderBrush = new StiEmptyBrush(),
			Brush = new StiSolidBrush(dataEmptyColor)
		};
		StiLinearTickLabelMajor element = new StiLinearTickLabelMajor
		{
			AllowApplyStyle = false,
			TextBrush = new StiSolidBrush(dataEmptyForeColor)
		};
		stiLinearScale.Items.Add(element);
		StiLinearTickLabelCustom stiLinearTickLabelCustom = new StiLinearTickLabelCustom
		{
			Offset = 0.1f,
			Font = new Font("Arial", 14f),
			TextBrush = new StiSolidBrush(dataEmptyForeColor),
			Placement = StiPlacement.Inside,
			TextObj = StiLocalization.Get("Dashboard", "Blank")
		};
		stiLinearTickLabelCustom.Values.Add(new StiLinearTickLabelCustomValue(0.5f, StiLocalization.Get("Dashboard", "Blank")));
		stiLinearScale.Items.Add(stiLinearTickLabelCustom);
		stiGauge.Scales.Add(stiLinearScale);
		return stiGauge;
	}

	private StiGauge GetLinearEmptyDataGauge(StiGaugeElement gaugeElement)
	{
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		Color dataEmptyColor = StiDashboardStyleHelper.GetDataEmptyColor(gaugeElement);
		Color dataEmptyForeColor = StiDashboardStyleHelper.GetDataEmptyForeColor(gaugeElement);
		StiGauge stiGauge = new StiGauge();
		StiLinearScale stiLinearScale = new StiLinearScale
		{
			AllowApplyStyle = false,
			RelativeHeight = 0.84f,
			Orientation = (Orientation)1,
			StartWidth = 0.1f,
			EndWidth = 0.1f,
			Maximum = 1f,
			MajorInterval = 1f,
			BorderBrush = new StiEmptyBrush(),
			Brush = new StiSolidBrush(dataEmptyColor)
		};
		StiLinearTickLabelMajor element = new StiLinearTickLabelMajor
		{
			AllowApplyStyle = false,
			TextBrush = new StiSolidBrush(dataEmptyForeColor)
		};
		stiLinearScale.Items.Add(element);
		StiLinearTickLabelCustom stiLinearTickLabelCustom = new StiLinearTickLabelCustom
		{
			Font = new Font("Arial", 14f),
			TextBrush = new StiSolidBrush(dataEmptyForeColor),
			Placement = StiPlacement.Overlay,
			TextObj = StiLocalization.Get("Dashboard", "Blank")
		};
		stiLinearTickLabelCustom.Values.Add(new StiLinearTickLabelCustomValue(0.5f, StiLocalization.Get("Dashboard", "Blank")));
		stiLinearScale.Items.Add(stiLinearTickLabelCustom);
		stiGauge.Scales.Add(stiLinearScale);
		return stiGauge;
	}

	private StiGauge GetHalfCircularEmptyDataGauge(StiGaugeElement gaugeElement)
	{
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		Color dataEmptyColor = StiDashboardStyleHelper.GetDataEmptyColor(gaugeElement);
		Color dataEmptyForeColor = StiDashboardStyleHelper.GetDataEmptyForeColor(gaugeElement);
		StiGauge stiGauge = new StiGauge();
		StiRadialScale stiRadialScale = new StiRadialScale
		{
			AllowApplyStyle = false,
			StartAngle = 180f,
			SweepAngle = 180f,
			StartWidth = 0.13f,
			EndWidth = 0.13f,
			Minimum = 0f,
			Maximum = 1f,
			Radius = 0.92f,
			MajorInterval = 1f,
			Center = new PointF(0.5f, 0.7f),
			BorderBrush = new StiEmptyBrush(),
			Brush = new StiSolidBrush(dataEmptyColor)
		};
		StiRadialTickLabelMajor element = new StiRadialTickLabelMajor
		{
			AllowApplyStyle = false,
			TextBrush = new StiSolidBrush(dataEmptyForeColor)
		};
		stiRadialScale.Items.Add(element);
		StiRadialTickLabelCustom stiRadialTickLabelCustom = new StiRadialTickLabelCustom
		{
			Offset = 0.8f,
			Font = new Font("Arial", 14f),
			TextBrush = new StiSolidBrush(dataEmptyForeColor),
			Placement = StiPlacement.Overlay
		};
		stiRadialTickLabelCustom.Values.Add(new StiRadialTickLabelCustomValue(0.5f, StiLocalization.Get("Dashboard", "Blank")));
		stiRadialScale.Items.Add(stiRadialTickLabelCustom);
		stiGauge.Scales.Add(stiRadialScale);
		return stiGauge;
	}

	private StiGauge GetFullCircularEmptyDataGauge(StiGaugeElement gaugeElement)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		Color dataEmptyColor = StiDashboardStyleHelper.GetDataEmptyColor(gaugeElement);
		Color dataEmptyForeColor = StiDashboardStyleHelper.GetDataEmptyForeColor(gaugeElement);
		StiGauge stiGauge = new StiGauge();
		StiRadialScale stiRadialScale = new StiRadialScale
		{
			AllowApplyStyle = false,
			StartAngle = 120f,
			SweepAngle = 300f,
			StartWidth = 0.13f,
			EndWidth = 0.13f,
			Minimum = 0f,
			Maximum = 1f,
			MajorInterval = 1f,
			BorderBrush = new StiEmptyBrush(),
			Brush = new StiSolidBrush(dataEmptyColor)
		};
		StiRadialTickLabelMajor element = new StiRadialTickLabelMajor
		{
			AllowApplyStyle = false,
			TextBrush = new StiSolidBrush(dataEmptyForeColor)
		};
		stiRadialScale.Items.Add(element);
		stiGauge.Scales.Add(stiRadialScale);
		StiRadialTickLabelCustom stiRadialTickLabelCustom = new StiRadialTickLabelCustom
		{
			Offset = 0.8f,
			Font = new Font("Arial", 14f),
			TextBrush = new StiSolidBrush(dataEmptyForeColor),
			Placement = StiPlacement.Overlay
		};
		stiRadialTickLabelCustom.Values.Add(new StiRadialTickLabelCustomValue(0.5f, StiLocalization.Get("Dashboard", "Blank")));
		stiRadialScale.Items.Add(stiRadialTickLabelCustom);
		stiGauge.Scales.Add(stiRadialScale);
		return stiGauge;
	}
}
