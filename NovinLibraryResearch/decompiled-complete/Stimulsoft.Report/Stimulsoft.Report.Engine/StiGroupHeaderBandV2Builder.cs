using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Engine;

public class StiGroupHeaderBandV2Builder : StiBandV2Builder
{
	public static StiDataBand GetMaster(StiGroupHeaderBand masterGroupHeaderBand)
	{
		for (int i = masterGroupHeaderBand.Parent.Components.IndexOf(masterGroupHeaderBand) + 1; i < masterGroupHeaderBand.Parent.Components.Count; i++)
		{
			if (masterGroupHeaderBand.Parent.Components[i] is StiDataBand)
			{
				return masterGroupHeaderBand.Parent.Components[i] as StiDataBand;
			}
		}
		return null;
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

	public override void SetReportVariables(StiComponent masterComp)
	{
		StiGroupHeaderBand stiGroupHeaderBand = masterComp as StiGroupHeaderBand;
		stiGroupHeaderBand.Report.GroupLine = stiGroupHeaderBand.Line;
	}

	public override void Prepare(StiComponent masterComp)
	{
		StiGroupHeaderBand stiGroupHeaderBand = masterComp as StiGroupHeaderBand;
		base.Prepare((StiComponent)stiGroupHeaderBand);
		stiGroupHeaderBand.Line = 1;
	}
}
