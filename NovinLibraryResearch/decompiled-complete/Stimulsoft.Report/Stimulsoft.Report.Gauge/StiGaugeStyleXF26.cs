using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Gauge;

public class StiGaugeStyleXF26 : StiGaugeStyleXF
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "Turquoise");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.Turquoise;

	public override StiGaugeStyleXF CreateNew()
	{
		return new StiGaugeStyleXF26();
	}

	public StiGaugeStyleXF26()
	{
		base.Core = new StiGaugeStyleCoreXF26();
	}
}
