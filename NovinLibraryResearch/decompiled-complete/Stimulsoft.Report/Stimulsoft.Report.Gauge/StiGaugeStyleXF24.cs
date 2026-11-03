using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Gauge;

public class StiGaugeStyleXF24 : StiGaugeStyleXF
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "Orange");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.Orange;

	public override StiGaugeStyleXF CreateNew()
	{
		return new StiGaugeStyleXF24();
	}

	public StiGaugeStyleXF24()
	{
		base.Core = new StiGaugeStyleCoreXF24();
	}
}
