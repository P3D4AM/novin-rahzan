using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiChartV1Builder : StiComponentV1Builder
{
	public static void RenderAtEnd(StiChart masterChart)
	{
		if (masterChart.ChartInfoV1.Cloner != null && masterChart.ChartInfoV1.Cloner.Parent != null)
		{
			StiComponent renderedComponent = null;
			RenderChart(masterChart, ref renderedComponent, masterChart.ChartInfoV1.OutContainer);
			masterChart.ChartInfoV1.OutContainer = null;
			masterChart.ChartInfoV1.Cloner.Parent.Components.Add(renderedComponent);
			masterChart.ChartInfoV1.Cloner.Parent.Components.Remove(masterChart.ChartInfoV1.Cloner);
		}
	}

	public static void RenderChart(StiChart masterChart, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		StiChart stiChart = (StiChart)((StiService)masterChart).Clone();
		if (!StiOptions.Engine.DontSaveDataSourceBeforeChartRendering)
		{
			masterChart.SaveState("ChartRender");
		}
		if (masterChart.MasterComponent == null)
		{
			StiDataHelper.SetData(masterChart, reinit: true, masterChart.Parent);
		}
		StiChartHelper.CreateChart(masterChart, stiChart);
		if (!StiOptions.Engine.DontSaveDataSourceBeforeChartRendering)
		{
			masterChart.RestoreState("ChartRender");
		}
		if (StiOptions.Engine.RenderChartAsBitmap)
		{
			StiImage imageFromChart = StiChartHelper.GetImageFromChart(stiChart);
			outContainer.Components.Add(imageFromChart);
			renderedComponent = imageFromChart;
		}
		else
		{
			outContainer.Components.Add(stiChart);
			renderedComponent = stiChart;
		}
		StiChartInteractionHelper.ProcessChart(masterChart, stiChart);
	}

	public override void Prepare(StiComponent masterComp)
	{
		StiFilterHelper.SetFilter(masterComp);
	}

	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		StiChart stiChart = masterComp as StiChart;
		if (!stiChart.ProcessAtEnd)
		{
			RenderChart(stiChart, ref renderedComponent, outContainer);
		}
		else if (stiChart.ChartInfoV1.OutContainer != null)
		{
			RenderAtEnd(stiChart);
		}
		else
		{
			stiChart.ChartInfoV1.OutContainer = outContainer;
			stiChart.ChartInfoV1.Cloner = new StiContainer();
			stiChart.ChartInfoV1.Cloner.Left = stiChart.Left;
			stiChart.ChartInfoV1.Cloner.Top = stiChart.Top;
			stiChart.ChartInfoV1.Cloner.Width = stiChart.Width;
			stiChart.ChartInfoV1.Cloner.Height = stiChart.Height;
			stiChart.ChartInfoV1.Cloner.Border.Side = (StiBorderSides)15;
			stiChart.ChartInfoV1.OutContainer.Components.Add(stiChart.ChartInfoV1.Cloner);
		}
		return true;
	}
}
