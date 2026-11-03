using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Report.Gauge;

public class StiGaugeStyleCoreXF29 : StiGaugeStyleCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Style") + "29";

	public override StiBrush Brush => (StiBrush)new StiSolidBrush("#ffffff");

	public override Color ForeColor { get; } = StiDashboardStyleHelper.GetForeColor(StiElementStyleIdent.Blue);


	public override Color BorderColor => Color.Transparent;

	public override float BorderWidth => 1f;

	public override Color TargetColor => ColorTranslator.FromHtml("#ef717a");

	public override StiBrush TickMarkMajorBrush => (StiBrush)new StiSolidBrush("#8c8c8c");

	public override StiBrush TickMarkMajorBorder => (StiBrush)new StiEmptyBrush();

	public override StiBrush TickMarkMinorBrush => (StiBrush)new StiSolidBrush("#8c8c8c");

	public override StiBrush TickMarkMinorBorder => (StiBrush)new StiEmptyBrush();

	public override StiBrush TickLabelMajorTextBrush => (StiBrush)new StiSolidBrush("#8c8c8c");

	public override Font TickLabelMajorFont => new Font("Arial", 10f);

	public override StiBrush TickLabelMinorTextBrush => (StiBrush)new StiSolidBrush("#8c8c8c");

	public override Font TickLabelMinorFont => new Font("Arial", 9f);

	public override StiBrush MarkerBrush => (StiBrush)new StiSolidBrush("#888");

	public override StiBrush LinearMarkerBorder => (StiBrush)new StiSolidBrush("#888");

	public override StiBrush LinearScaleBrush => (StiBrush)new StiSolidBrush("#3498db");

	public override StiBrush LinearBarBrush => (StiBrush)new StiSolidBrush("#3498db");

	public override StiBrush LinearBarBorderBrush => (StiBrush)new StiEmptyBrush();

	public override StiBrush LinearBarEmptyBrush => (StiBrush)new StiEmptyBrush();

	public override StiBrush LinearBarEmptyBorderBrush => (StiBrush)new StiEmptyBrush();

	public override float LinearBarStartWidth => 0.1f;

	public override float LinearBarEndWidth => 0.1f;

	public override StiBrush RadialBarBrush => (StiBrush)new StiSolidBrush("#3498db");

	public override StiBrush RadialBarBorderBrush => (StiBrush)new StiEmptyBrush();

	public override StiBrush RadialBarEmptyBrush => (StiBrush)new StiSolidBrush("#e7ebec");

	public override StiBrush RadialBarEmptyBorderBrush => (StiBrush)new StiEmptyBrush();

	public override float RadialBarStartWidth => 0.1f;

	public override float RadialBarEndWidth => 0.1f;

	public override StiBrush NeedleBrush => (StiBrush)new StiSolidBrush("#3498db");

	public override StiBrush NeedleBorderBrush => (StiBrush)new StiEmptyBrush();

	public override StiBrush NeedleCapBrush => (StiBrush)new StiSolidBrush("#3498db");

	public override StiBrush NeedleCapBorderBrush => (StiBrush)new StiSolidBrush("#3498db");

	public override float NeedleBorderWidth => 0f;

	public override float NeedleCapBorderWidth => 2f;

	public override float NeedleStartWidth => 0.1f;

	public override float NeedleEndWidth => 1f;

	public override float NeedleRelativeHeight => 0.06f;

	public override float NeedleRelativeWith => 0.45f;
}
