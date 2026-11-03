using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Gauge;

public class StiGaugeStyleXF27 : StiGaugeStyleXF
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "SlateGray");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.SlateGray;

	public override StiGaugeStyleXF CreateNew()
	{
		return new StiGaugeStyleXF27();
	}

	public StiGaugeStyleXF27()
	{
		base.Core = new StiGaugeStyleCoreXF27();
	}
}
