using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Engine;

public class StiGroupHeaderBandV1Builder : StiBandV1Builder
{
	internal static void SecondPassBreak(StiGroupHeaderBand masterGroupHeaderBand, StiComponent renderedComponent, StiComponent masterComponent, ref double freeSpace)
	{
		masterGroupHeaderBand.GroupHeaderBandInfoV1.IsFirstPassOfBreak = true;
		if (masterGroupHeaderBand.GroupHeaderBandInfoV1.BreakableComps != null)
		{
			masterGroupHeaderBand.GroupHeaderBandInfoV1.IsFirstPassOfBreak = false;
			StiComponentDivider.ProcessPreviousBreakedContainer(renderedComponent as StiContainer, masterGroupHeaderBand.GroupHeaderBandInfoV1.BreakableComps, ref freeSpace);
			masterGroupHeaderBand.GroupHeaderBandInfoV1.BreakableComps = null;
		}
	}

	internal static void FirstPassBreak(StiGroupHeaderBand masterGroupHeaderBand, StiComponent renderedComponent, StiComponent masterComponent, ref double freeSpace)
	{
		renderedComponent.Height += freeSpace;
		masterGroupHeaderBand.GroupHeaderBandInfoV1.BreakableComps = StiComponentDivider.BreakContainer(renderedComponent as StiContainer);
	}

	public static object GetCurrentConditionValue(StiGroupHeaderBand masterGroupHeaderBand)
	{
		StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
		masterGroupHeaderBand.InvokeGetValue(stiValueEventArgs);
		return stiValueEventArgs.Value;
	}

	public static object GetCurrentSummaryExpressionValue(StiGroupHeaderBand masterGroupHeaderBand)
	{
		StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
		masterGroupHeaderBand.InvokeGetSummaryExpression(stiValueEventArgs);
		return stiValueEventArgs.Value;
	}

	public override void Prepare(StiComponent masterComp)
	{
		base.Prepare(masterComp);
		StiGroupHeaderBand stiGroupHeaderBand = masterComp as StiGroupHeaderBand;
		if (stiGroupHeaderBand.CanBreak)
		{
			stiGroupHeaderBand.GroupHeaderBandInfoV1.ForceCanBreak = true;
			return;
		}
		foreach (StiComponent component in stiGroupHeaderBand.Components)
		{
			if (component is IStiCrossTab)
			{
				stiGroupHeaderBand.GroupHeaderBandInfoV1.ForceCanBreak = true;
				break;
			}
		}
		stiGroupHeaderBand.GroupHeaderBandInfoV1.LastPositionRendering = -1;
		stiGroupHeaderBand.GroupHeaderBandInfoV1.LastPositionLineRendering = -1;
	}

	public override bool Render(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		StiGroupHeaderBand stiGroupHeaderBand = masterComp as StiGroupHeaderBand;
		int line = stiGroupHeaderBand.Report.Line;
		stiGroupHeaderBand.Report.Line = stiGroupHeaderBand.Line;
		stiGroupHeaderBand.RenderedCount++;
		bool result = InternalRender(stiGroupHeaderBand, ref renderedComponent, outContainer);
		stiGroupHeaderBand.Report.Line = line;
		return result;
	}
}
