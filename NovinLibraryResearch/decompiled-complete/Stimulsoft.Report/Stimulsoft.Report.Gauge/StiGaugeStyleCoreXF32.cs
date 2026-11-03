using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Report.Gauge;

public class StiGaugeStyleCoreXF32 : StiGaugeStyleCoreXF30
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Style") + "32";

	public override StiBrush Brush => (StiBrush)new StiSolidBrush("#6d7e8b");

	public override Color ForeColor { get; } = StiDashboardStyleHelper.GetForeColor(StiElementStyleIdent.Silver);


	public override Color TargetColor => ColorTranslator.FromHtml("#90a1ab");

	public override StiBrush MarkerBrush => (StiBrush)new StiSolidBrush("#d41c2a");

	public override StiBrush LinearMarkerBorder => (StiBrush)new StiSolidBrush("#d41c2a");

	public override StiBrush NeedleBrush => (StiBrush)new StiSolidBrush("#d41c2a");

	public override StiBrush NeedleBorderBrush => (StiBrush)new StiSolidBrush("#d41c2a");

	public override StiBrush NeedleCapBrush => (StiBrush)new StiSolidBrush("#d41c2a");

	public override StiBrush NeedleCapBorderBrush => (StiBrush)new StiSolidBrush("#d3d3d5");

	public override float NeedleCapBorderWidth => 0f;

	public override StiBrush LinearScaleBrush => (StiBrush)new StiSolidBrush("#e9f4fc");

	public override StiBrush LinearBarBrush => (StiBrush)new StiSolidBrush("#3a5263");

	public override StiBrush RadialBarBrush => (StiBrush)new StiSolidBrush("#e9f4fc");

	public override StiBrush RadialBarEmptyBrush => (StiBrush)new StiSolidBrush("#3a5263");
}
