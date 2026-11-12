using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Gauge;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Dashboard.Render;
using Stimulsoft.Dashboard.Visuals.Gauge;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Gauge;

namespace Stimulsoft.Dashboard.Export.Tools;

public class StiGaugeElementExportTool : StiElementExportTool
{
	public override void Render(IStiElement element, StiPanel destination, RectangleD rect, StiDashboardExportSettings settings)
	{
		if (RenderEmptyDataMessage(element, destination, rect, settings))
		{
			return;
		}
		StiGaugeElement gaugeElement = element as StiGaugeElement;
		StiDataTable dataTable = StiElementExportTool.GetDataTable(gaugeElement);
		IEnumerable<StiGaugeIteration> iterations = new StiGaugeElementBuilder().Render(gaugeElement, rect.Size, dataTable);
		if (iterations.Count() == 1)
		{
			RenderSingleGaugeFromIteration(gaugeElement, destination, iterations.FirstOrDefault(), rect);
		}
		else
		{
			if (iterations.Count() <= 0)
			{
				return;
			}
			if (!settings.IsDesignMode && settings.Format == StiDashboardExportFormat.Html)
			{
				int index = 0;
				List<RectangleD> rectangles = StiElementLocationHelper.GetLocationRectangles(rect, iterations.Count(), gaugeElement);
				RectangleD firstRect = rectangles[0];
				iterations = new StiGaugeElementBuilder().Render(gaugeElement, firstRect.Size, dataTable);
				iterations.ToList().ForEach(delegate(StiGaugeIteration iteration)
				{
					RectangleD rect2 = rectangles[index];
					double titleHeightValue = 0.0;
					if (!string.IsNullOrEmpty(iteration.Series))
					{
						RenderSingleGaugeTitleFromIteration(gaugeElement, destination, iteration, rect2, firstRect, iterations, index, out titleHeightValue);
						rect2.Height -= titleHeightValue;
						rect2.Y += titleHeightValue;
					}
					RenderSingleGaugeFromIteration(gaugeElement, destination, iteration, rect2, index);
					index++;
				});
			}
			else
			{
				RenderContent(element, destination, rect, settings);
			}
		}
	}

	private void RenderSingleGaugeFromIteration(StiGaugeElement gaugeElement, StiPanel destination, StiGaugeIteration iteration, RectangleD rect, int index = 0)
	{
		StiGauge gauge = iteration.Gauge;
		gauge.Name = $"{gaugeElement.Name}_Content{index}";
		gauge.ClientRectangle = rect;
		gauge.Style = StiDashboardStyleHelper.GetGaugeStyle(gaugeElement);
		gauge.CustomStyleName = gaugeElement.CustomStyleName;
		gauge.Brush = new StiSolidBrush(StiDashboardStyleHelper.GetBackColor(gaugeElement, null, allowOpacity: true));
		destination.Components.Add(gauge);
	}

	private void RenderSingleGaugeTitleFromIteration(StiGaugeElement gaugeElement, StiPanel destination, StiGaugeIteration iteration, RectangleD rect, RectangleD firstRect, IEnumerable<StiGaugeIteration> iterations, int index, out double titleHeightValue)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				string series = iteration.Series;
				double? titleHeight = StiGaugeVisualSvgHelper.GetTitleHeight(val2, iterations, rect, gaugeElement);
				double titleMinFontSize = StiGaugeVisualSvgHelper.GetTitleMinFontSize(val2, iterations, gaugeElement.Font, firstRect.Width, firstRect.Height);
				titleHeightValue = titleHeight.GetValueOrDefault(0.0);
				if (titleHeight.HasValue)
				{
					RectangleD rectangleD = new RectangleD(rect.X, rect.Y, rect.Width, titleHeightValue);
					Font val3 = StiFontUtils.ChangeFontSize(gaugeElement.Font, (float)titleMinFontSize);
					try
					{
						rectangleD.Height = StiTitleMeasureHelper.MeasureTitle(val2, rectangleD, gaugeElement.Font, series).Height;
						Color foreColor = StiDashboardExpressionHelper.GetForeColor(gaugeElement, gaugeElement.ForeColor);
						Color color = ((foreColor != Color.Transparent) ? foreColor : StiDashboardStyleHelper.GetForeColor(gaugeElement));
						destination.Components.Add(new StiText(rectangleD)
						{
							Name = $"{gaugeElement.Name}_Content{index}",
							Brush = new StiSolidBrush(Color.Transparent),
							Text = series,
							Font = val3,
							TextBrush = new StiSolidBrush(color),
							HorAlignment = StiTextHorAlignment.Center
						});
						return;
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
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
}
