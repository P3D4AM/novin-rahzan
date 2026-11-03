using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Gauge;

public class StiGaugeStyleXF33 : StiGaugeStyleXF
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "AliceBlue");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.AliceBlue;

	public override StiGaugeStyleXF CreateNew()
	{
		return new StiGaugeStyleXF33();
	}

	public StiGaugeStyleXF33()
	{
		base.Core = new StiGaugeStyleCoreXF33();
	}
}
