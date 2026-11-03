using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Gauge;

public class StiGaugeStyleXF29 : StiGaugeStyleXF
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "Blue");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.Blue;

	public override StiGaugeStyleXF CreateNew()
	{
		return new StiGaugeStyleXF29();
	}

	public StiGaugeStyleXF29()
	{
		base.Core = new StiGaugeStyleCoreXF29();
	}
}
