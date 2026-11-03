using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Report.Gauge;

public class StiGaugeStyleCoreXF33 : StiGaugeStyleCoreXF30
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Style") + "33";

	public override StiBrush Brush => (StiBrush)new StiSolidBrush("#f2f5fc");

	public override Color ForeColor { get; } = StiDashboardStyleHelper.GetForeColor(StiElementStyleIdent.AliceBlue);


	public override Color TargetColor => ColorTranslator.FromHtml("#e47334");

	public override StiBrush TickMarkMajorBrush => (StiBrush)new StiSolidBrush("#3a3a3a");

	public override StiBrush TickMarkMajorBorder => (StiBrush)new StiEmptyBrush();

	public override StiBrush TickMarkMinorBrush => (StiBrush)new StiEmptyBrush();

	public override StiBrush TickMarkMinorBorder => (StiBrush)new StiEmptyBrush();

	public override StiBrush TickLabelMajorTextBrush => (StiBrush)new StiSolidBrush("#3a3a3a");

	public override Font TickLabelMajorFont => new Font("Arial", 8f);

	public override StiBrush TickLabelMinorTextBrush => (StiBrush)new StiSolidBrush("#3a3a3a");

	public override Font TickLabelMinorFont => new Font("Arial", 7f);

	public override StiBrush MarkerBrush => (StiBrush)new StiSolidBrush("#d41c2a");

	public override StiBrush LinearMarkerBorder => (StiBrush)new StiSolidBrush(Color.Transparent);

	public override StiBrush NeedleBrush => (StiBrush)new StiSolidBrush("#d41c2a");

	public override StiBrush NeedleBorderBrush => (StiBrush)new StiSolidBrush("#d41c2a");

	public override StiBrush NeedleCapBrush => (StiBrush)new StiSolidBrush("#d41c2a");

	public override StiBrush NeedleCapBorderBrush => (StiBrush)new StiSolidBrush("#d3d3d5");

	public override float NeedleCapBorderWidth => 0f;

	public override StiBrush LinearScaleBrush => (StiBrush)new StiSolidBrush("#ccd5f0");

	public override StiBrush LinearBarBrush => (StiBrush)new StiSolidBrush("#5d6b99");

	public override StiBrush RadialBarBrush => (StiBrush)new StiSolidBrush("#ccd5f0");

	public override StiBrush RadialBarEmptyBrush => (StiBrush)new StiSolidBrush("#5d6b99");
}
