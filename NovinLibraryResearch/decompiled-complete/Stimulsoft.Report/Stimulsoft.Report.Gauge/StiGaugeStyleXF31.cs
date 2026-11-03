using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Gauge;

public class StiGaugeStyleXF31 : StiGaugeStyleXF
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "DarkTurquoise");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.DarkTurquoise;

	public override StiGaugeStyleXF CreateNew()
	{
		return new StiGaugeStyleXF31();
	}

	public StiGaugeStyleXF31()
	{
		base.Core = new StiGaugeStyleCoreXF31();
	}
}
