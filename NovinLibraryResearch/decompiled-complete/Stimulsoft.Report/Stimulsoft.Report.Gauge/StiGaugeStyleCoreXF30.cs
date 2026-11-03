using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Report.Gauge;

public class StiGaugeStyleCoreXF30 : StiGaugeStyleCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Style") + "30";

	public override StiBrush Brush => (StiBrush)new StiSolidBrush("#595b65");

	public override Color BorderColor => Color.Transparent;

	public override Color ForeColor { get; } = StiDashboardStyleHelper.GetForeColor(StiElementStyleIdent.DarkGray);


	public override float BorderWidth => 1f;

	public override Color TargetColor => ColorTranslator.FromHtml("#d3d3d5");

	public override StiBrush TickMarkMajorBrush => (StiBrush)new StiSolidBrush("#ffffff");

	public override StiBrush TickMarkMajorBorder => (StiBrush)new StiEmptyBrush();

	public override StiBrush TickMarkMinorBrush => (StiBrush)new StiEmptyBrush();

	public override StiBrush TickMarkMinorBorder => (StiBrush)new StiEmptyBrush();

	public override StiBrush TickLabelMajorTextBrush => (StiBrush)new StiSolidBrush("#ffffff");

	public override Font TickLabelMajorFont => new Font("Arial", 10f);

	public override StiBrush TickLabelMinorTextBrush => (StiBrush)new StiSolidBrush("#ffffff");

	public override Font TickLabelMinorFont => new Font("Arial", 9f);

	public override StiBrush MarkerBrush => (StiBrush)new StiSolidBrush("#d3d3d5");

	public override StiBrush LinearMarkerBorder => (StiBrush)new StiSolidBrush("#d3d3d5");

	public override StiBrush LinearScaleBrush => (StiBrush)new StiSolidBrush("#db7b46");

	public override StiBrush LinearBarBrush => (StiBrush)new StiSolidBrush("#db7b46");

	public override StiBrush LinearBarBorderBrush => (StiBrush)new StiEmptyBrush();

	public override StiBrush LinearBarEmptyBrush => (StiBrush)new StiEmptyBrush();

	public override StiBrush LinearBarEmptyBorderBrush => (StiBrush)new StiEmptyBrush();

	public override float LinearBarStartWidth => 0.1f;

	public override float LinearBarEndWidth => 0.1f;

	public override StiBrush RadialBarBrush => (StiBrush)new StiSolidBrush("#db7b46");

	public override StiBrush RadialBarBorderBrush => (StiBrush)new StiEmptyBrush();

	public override StiBrush RadialBarEmptyBrush => (StiBrush)new StiSolidBrush("#6b6e75");

	public override StiBrush RadialBarEmptyBorderBrush => (StiBrush)new StiEmptyBrush();

	public override float RadialBarStartWidth => 0.1f;

	public override float RadialBarEndWidth => 0.1f;

	public override StiBrush NeedleBrush => (StiBrush)new StiSolidBrush("#d3d3d5");

	public override StiBrush NeedleBorderBrush => (StiBrush)new StiEmptyBrush();

	public override StiBrush NeedleCapBrush => (StiBrush)new StiSolidBrush("#ffffff");

	public override StiBrush NeedleCapBorderBrush => (StiBrush)new StiSolidBrush("#d3d3d5");

	public override float NeedleBorderWidth => 0f;

	public override float NeedleCapBorderWidth => 2f;

	public override float NeedleStartWidth => 0.1f;

	public override float NeedleEndWidth => 1f;

	public override float NeedleRelativeHeight => 0.06f;

	public override float NeedleRelativeWith => 0.45f;
}
