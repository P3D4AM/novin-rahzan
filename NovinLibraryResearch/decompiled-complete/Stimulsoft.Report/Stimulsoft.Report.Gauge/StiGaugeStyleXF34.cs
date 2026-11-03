using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Gauge;

public class StiGaugeStyleXF34 : StiGaugeStyleXF
{
	public override bool AllowDashboard => true;

	public override string DashboardName => Loc.Get("PropertyColor", "DarkGreen");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.DarkGreen;

	public override StiGaugeStyleXF CreateNew()
	{
		return new StiGaugeStyleXF34();
	}

	public StiGaugeStyleXF34()
	{
		base.Core = new StiGaugeStyleCoreXF34();
	}
}
