using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

internal class StiEmptyBandsV1Helper
{
	private static StiContainer CreateEmptyBandContainer(StiReport report, StiContainer containerForRender, StiEmptyBand emptyBand)
	{
		report.Line++;
		report.LineThrough++;
		emptyBand.InvokeRendering();
		emptyBand.ParentBookmark = containerForRender.CurrentBookmark;
		StiComponent renderedComponent = null;
		emptyBand.RenderContainer(ref renderedComponent, containerForRender);
		containerForRender.Components.Remove(renderedComponent);
		StiOddEvenStylesHelper.ApplyOddEvenStyles(report, emptyBand, renderedComponent as StiContainer);
		return renderedComponent as StiContainer;
	}

	public static void Render(StiReport report, StiContainer containerForRender, StiEmptyBand emptyBand)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (emptyBand == null)
		{
			return;
		}
		emptyBand.InvokeBeginRender();
		int num = containerForRender.Components.Count;
		StiContainer stiContainer = null;
		RectangleD dockRegion = containerForRender.GetDockRegion(containerForRender);
		double num2 = ((RectangleD)(ref dockRegion)).Height;
		double top = ((RectangleD)(ref dockRegion)).Top;
		StiContainer stiContainer2;
		while (true)
		{
			stiContainer2 = CreateEmptyBandContainer(report, containerForRender, emptyBand);
			if (stiContainer2.Height == 0.0)
			{
				stiContainer2.Height = report.Unit.ConvertFromHInches(10.0);
			}
			if (!(stiContainer2.Height <= num2))
			{
				break;
			}
			stiContainer2.Top = top;
			num2 -= stiContainer2.Height;
			containerForRender.Components.Insert(num, stiContainer2);
			num++;
			stiContainer = stiContainer2;
			stiContainer2.DoBookmark();
		}
		if (emptyBand.SizeMode == StiEmptySizeMode.DecreaseLastRow)
		{
			stiContainer2.Height = num2;
			containerForRender.Components.Insert(num, stiContainer2);
			stiContainer2.DoBookmark();
		}
		else if (emptyBand.SizeMode == StiEmptySizeMode.IncreaseLastRow)
		{
			if (stiContainer == null)
			{
				if (num2 > 0.0)
				{
					stiContainer2 = CreateEmptyBandContainer(report, containerForRender, emptyBand);
					stiContainer2.Top = top;
					stiContainer2.Height = num2;
				}
			}
			else
			{
				stiContainer.Height += num2;
			}
		}
		else if (emptyBand.SizeMode == StiEmptySizeMode.AlignFooterToTop)
		{
			foreach (StiComponent component in containerForRender.Components)
			{
				if (component.Top > top)
				{
					component.Top -= num2;
				}
			}
		}
		emptyBand.InvokeEndRender();
	}
}
