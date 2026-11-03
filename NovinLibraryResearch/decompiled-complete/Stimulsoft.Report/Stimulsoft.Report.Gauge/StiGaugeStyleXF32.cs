using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Gauge;

public class StiGaugeStyleXF32 : StiGaugeStyleXF
{
	public override bool AllowDashboard => true;

	public override string DashboardName => StiLocalization.Get("PropertyColor", "Silver");

	public override StiElementStyleIdent StyleIdent => StiElementStyleIdent.Silver;

	public override StiGaugeStyleXF CreateNew()
	{
		return new StiGaugeStyleXF32();
	}

	public StiGaugeStyleXF32()
	{
		base.Core = new StiGaugeStyleCoreXF32();
	}
}
