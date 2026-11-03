using System;
using System.Collections.Generic;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Engine;

public class StiSubReportV1Builder : StiContainerV1Builder
{
	public override void Prepare(StiComponent masterComp)
	{
		base.Prepare(masterComp);
		StiSubReport stiSubReport = masterComp as StiSubReport;
		StiPage subReportPage = stiSubReport.SubReportPage;
		if (subReportPage != null)
		{
			subReportPage.ProcessPageBeforeRender();
			subReportPage.Prepare();
		}
	}

	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		StiSubReport stiSubReport = masterComp as StiSubReport;
		double num = 0.0;
		foreach (StiComponent component in outContainer.Components)
		{
			if ((component.Left > stiSubReport.Left && component.Left < stiSubReport.Right) || (component.Right > stiSubReport.Left && component.Right < stiSubReport.Right) || (stiSubReport.Left > component.Left && stiSubReport.Left < component.Right) || (stiSubReport.Right > component.Left && stiSubReport.Right < component.Right))
			{
				num = Math.Max(component.Bottom, num);
			}
		}
		StiReport externalSubReport = stiSubReport.GetExternalSubReport();
		StiFillParametersEventArgs stiFillParametersEventArgs = new StiFillParametersEventArgs();
		stiSubReport.InvokeFillParameters(stiSubReport, stiFillParametersEventArgs);
		if (stiFillParametersEventArgs.Value != null && stiFillParametersEventArgs.Value.Count > 0)
		{
			StiReport stiReport = externalSubReport ?? stiSubReport.Report;
			foreach (KeyValuePair<string, object> item in stiFillParametersEventArgs.Value)
			{
				stiReport[item.Key] = item.Value;
			}
		}
		StiDataBand stiDataBand = null;
		double num2 = 0.0;
		if (externalSubReport == null)
		{
			base.InternalRender((StiComponent)stiSubReport, ref renderedComponent, outContainer);
			if (stiSubReport.Top < num)
			{
				renderedComponent.Top = num;
			}
			num2 = outContainer.Height;
			stiDataBand = stiSubReport.Parent as StiDataBand;
			if (stiDataBand != null)
			{
				renderedComponent.Height = stiDataBand.DataBandInfoV1.FreeSpace - renderedComponent.Top;
				if (renderedComponent.Height > stiDataBand.DataBandInfoV1.FreeSpace - renderedComponent.Top)
				{
					renderedComponent.Height -= 0.01;
				}
			}
		}
		StiComponent renderedComponent2 = null;
		bool flag = true;
		if ((stiSubReport.SubReportPage != null && renderedComponent != null) || externalSubReport != null)
		{
			if (externalSubReport != null)
			{
				if (!externalSubReport.IsDocument)
				{
					foreach (StiPage page in externalSubReport.Pages)
					{
						page.UnlimitedHeight = true;
					}
					externalSubReport.Render(showProgress: false);
				}
				double left = stiSubReport.Left;
				double num3 = stiSubReport.Top;
				foreach (StiPage renderedPage in externalSubReport.RenderedPages)
				{
					outContainer.Components.AddRange(renderedPage.Components);
					foreach (StiComponent component2 in renderedPage.Components)
					{
						component2.Left += left;
						component2.Top += num3;
					}
					renderedPage.Components.Clear();
					num3 += renderedPage.Height;
				}
				outContainer.Components.SetParent(renderedComponent as StiContainer);
			}
			else if (stiSubReport.SubReportPage != null)
			{
				stiSubReport.SubReportPage.ParentBookmark = stiSubReport.CurrentBookmark;
				flag = stiSubReport.SubReportPage.Render(ref renderedComponent2, renderedComponent as StiContainer);
				if (stiSubReport.SubReportPage.Columns > 1)
				{
					StiComponentsCollection components = ((StiContainer)renderedComponent).Components;
					foreach (StiComponent item2 in components)
					{
						if (!item2.Name.StartsWith("#Column#", StringComparison.InvariantCulture))
						{
							continue;
						}
						item2.DockStyle = StiDockStyle.None;
						double num4 = 0.0;
						StiComponentsCollection components2 = ((StiContainer)item2).Components;
						foreach (StiComponent item3 in components2)
						{
							num = Math.Max(num4, item3.Bottom);
						}
						item2.Height = num4;
					}
				}
			}
			if (stiDataBand != null)
			{
				renderedComponent.CanShrink = true;
				SizeD actualSize = renderedComponent.GetActualSize();
				if (((SizeD)(ref actualSize)).Height != 0.0)
				{
					renderedComponent.Height = ((SizeD)(ref actualSize)).Height;
					outContainer.Height = Math.Max(renderedComponent.Bottom, outContainer.Height);
				}
				else
				{
					StiComponent obj = renderedComponent;
					double height = (outContainer.Height = num2);
					obj.Height = height;
				}
			}
			stiSubReport.SubReportInfoV1.SubReportRenderResult = flag;
		}
		return flag;
	}
}
