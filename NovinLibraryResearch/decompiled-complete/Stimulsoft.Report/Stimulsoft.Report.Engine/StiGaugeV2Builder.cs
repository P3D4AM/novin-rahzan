using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;

namespace Stimulsoft.Report.Engine;

public class StiGaugeV2Builder : StiComponentV2Builder
{
	public override StiComponent InternalRender(StiComponent masterComp)
	{
		StiGauge stiGauge = (StiGauge)((StiService)masterComp).Clone();
		if (StiGaugeV2InitHelper.IsGaugeV2(stiGauge))
		{
			StiGaugeV2InitHelper.Prepare(stiGauge);
		}
		else
		{
			foreach (StiScaleBase scale in stiGauge.Scales)
			{
				scale.Prepare();
			}
		}
		return stiGauge;
	}
}
