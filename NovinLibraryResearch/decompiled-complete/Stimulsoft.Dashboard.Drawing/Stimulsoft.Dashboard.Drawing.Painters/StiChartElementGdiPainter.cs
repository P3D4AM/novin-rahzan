using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Dashboard.Render;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public class StiChartElementGdiPainter : StiAtomElementGdiPainter
{
	public override void PaintCheckBoxTitle(Graphics g, RectangleD rect, IStiElement element)
	{
		base.PaintCheckBoxTitle(g, rect, element);
		StiViewStatesElementGdiPainter.PaintViewStatesButtons(g, rect, element);
		StiSortElementGdiPainter.PaintSortButton(g, rect, element);
	}

	public override void PaintEmptyData(Graphics g, RectangleD rect, IStiElement element, double? zoom = null)
	{
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		StiChartElement stiChartElement = ((StiChartElement)element).Clone() as StiChartElement;
		StiDataTable stiDataTable = null;
		if (element.IsQuerable)
		{
			stiDataTable = StiElementDataCache.GetOrCreateWithProgress(element);
			if (stiDataTable == null)
			{
				return;
			}
		}
		if (stiChartElement.Values.Count >= 1)
		{
			if (stiChartElement.Values != null && stiChartElement.Values.Count > 1)
			{
				stiChartElement.Values.RemoveRange(1, stiChartElement.Values.Count - 1);
			}
			stiChartElement.Values[0].Expression = "";
			StiChart stiChart = StiChartElementBuilder.Create(stiChartElement).Render(stiChartElement, stiDataTable);
			stiChart.Title.Text = StiLocalization.Get("Dashboard", "Blank");
			stiChart.Title.Visible = true;
			stiChart.Title.Font = new Font("Aria", 13f, (FontStyle)0);
			stiChart.Brush = new StiEmptyBrush();
			stiChart.Area.Brush = new StiEmptyBrush();
			StiChartGdiPainter stiChartGdiPainter = new StiChartGdiPainter();
			stiChartGdiPainter.DrawChart(stiChart, g, rect.ToRectangleF(), useMargins: true, useBackground: false, zoom);
		}
	}
}
