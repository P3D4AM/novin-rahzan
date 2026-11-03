using System;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiChartV2Builder : StiComponentV2Builder
{
	public static void RenderAtEnd(StiChart masterChart)
	{
		if (masterChart.ChartInfoV2.StoredForProcessAtEndChart != null)
		{
			RenderChart(masterChart);
		}
		masterChart.ChartInfoV2.StoredForProcessAtEndChart = null;
	}

	public static StiComponent RenderChart(StiChart masterChart)
	{
		StiChart stiChart;
		if (masterChart.ChartInfoV2.StoredForProcessAtEndChart != null)
		{
			stiChart = masterChart.ChartInfoV2.StoredForProcessAtEndChart;
		}
		else
		{
			stiChart = (StiChart)((StiService)masterChart).Clone();
			if (masterChart.ProcessAtEnd)
			{
				return stiChart;
			}
		}
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
			imageFromChart.ImageRotation = stiChart.Rotation;
			StiImageRotation rotation = stiChart.Rotation;
			if ((uint)(rotation - 1) <= 1u)
			{
				double height = imageFromChart.Height;
				imageFromChart.Height = imageFromChart.Width;
				imageFromChart.Width = height;
			}
			return imageFromChart;
		}
		return stiChart;
	}

	public override void Prepare(StiComponent masterComp)
	{
		base.Prepare(masterComp);
		StiFilterHelper.SetFilter(masterComp);
		if (masterComp.Height > masterComp.Page.Height || masterComp.Height > masterComp.Parent.Height)
		{
			masterComp.Height = Math.Min(masterComp.Page.Height, masterComp.Parent.Height);
		}
	}

	public override StiComponent InternalRender(StiComponent masterComp)
	{
		StiChart stiChart = masterComp as StiChart;
		StiComponent stiComponent = RenderChart(stiChart);
		StiChart childChart = stiComponent as StiChart;
		if (stiChart.ProcessAtEnd)
		{
			stiChart.ChartInfoV2.StoredForProcessAtEndChart = stiComponent as StiChart;
		}
		StiChartInteractionHelper.ProcessChart(stiChart, childChart);
		return stiComponent;
	}
}
