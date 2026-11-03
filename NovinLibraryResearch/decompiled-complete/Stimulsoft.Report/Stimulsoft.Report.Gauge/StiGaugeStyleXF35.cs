using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Gauge;

public class StiGaugeStyleXF35 : StiGaugeStyleXF
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "Sienna");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.Sienna;

	public override StiGaugeStyleXF CreateNew()
	{
		return new StiGaugeStyleXF35();
	}

	public StiGaugeStyleXF35()
	{
		base.Core = new StiGaugeStyleCoreXF35();
	}
}
