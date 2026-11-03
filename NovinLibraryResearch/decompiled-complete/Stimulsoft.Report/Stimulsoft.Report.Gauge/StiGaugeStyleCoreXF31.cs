using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Report.Gauge;

public class StiGaugeStyleCoreXF31 : StiGaugeStyleCoreXF30
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Style") + "31";

	public override StiBrush Brush => (StiBrush)new StiSolidBrush("#235e6d");

	public override Color ForeColor { get; } = StiDashboardStyleHelper.GetForeColor(StiElementStyleIdent.DarkTurquoise);


	public override Color TargetColor => ColorTranslator.FromHtml("#fefef9");

	public override StiBrush MarkerBrush => (StiBrush)new StiSolidBrush("#f2ffff");

	public override StiBrush LinearMarkerBorder => (StiBrush)new StiSolidBrush("#f2ffff");

	public override StiBrush LinearScaleBrush => (StiBrush)new StiSolidBrush("#f0621e");

	public override StiBrush LinearBarBrush => (StiBrush)new StiSolidBrush("#f0621e");

	public override StiBrush RadialBarBrush => (StiBrush)new StiSolidBrush("#f0621e");

	public override StiBrush RadialBarEmptyBrush => (StiBrush)new StiSolidBrush("#1c445b");
}
