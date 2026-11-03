using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Gauge;

public class StiGaugeStyleXF25 : StiGaugeStyleXF
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "Green");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.Green;

	public override StiGaugeStyleXF CreateNew()
	{
		return new StiGaugeStyleXF25();
	}

	public StiGaugeStyleXF25()
	{
		base.Core = new StiGaugeStyleCoreXF25();
	}
}
