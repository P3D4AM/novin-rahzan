using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Gauge;

public abstract class StiGaugeStyleCoreXF
{
	public abstract string LocalizedName { get; }

	public abstract StiBrush Brush { get; }

	public abstract Color ForeColor { get; }

	public abstract Color BorderColor { get; }

	public abstract float BorderWidth { get; }

	public abstract Color TargetColor { get; }

	public abstract StiBrush TickMarkMajorBrush { get; }

	public abstract StiBrush TickMarkMajorBorder { get; }

	public virtual float TickMarkMajorBorderWidth => 0f;

	public abstract StiBrush TickMarkMinorBrush { get; }

	public abstract StiBrush TickMarkMinorBorder { get; }

	public virtual float TickMarkMinorBorderWidth => 0f;

	public abstract StiBrush TickLabelMajorTextBrush { get; }

	public abstract Font TickLabelMajorFont { get; }

	public abstract StiBrush TickLabelMinorTextBrush { get; }

	public abstract Font TickLabelMinorFont { get; }

	public abstract StiBrush LinearScaleBrush { get; }

	public abstract StiBrush LinearBarBrush { get; }

	public abstract StiBrush LinearBarBorderBrush { get; }

	public abstract StiBrush LinearBarEmptyBrush { get; }

	public abstract StiBrush LinearBarEmptyBorderBrush { get; }

	public abstract float LinearBarStartWidth { get; }

	public abstract float LinearBarEndWidth { get; }

	public abstract StiBrush RadialBarBrush { get; }

	public abstract StiBrush RadialBarBorderBrush { get; }

	public abstract StiBrush RadialBarEmptyBrush { get; }

	public abstract StiBrush RadialBarEmptyBorderBrush { get; }

	public abstract float RadialBarStartWidth { get; }

	public abstract float RadialBarEndWidth { get; }

	public abstract StiBrush NeedleBrush { get; }

	public abstract StiBrush NeedleBorderBrush { get; }

	public abstract StiBrush NeedleCapBrush { get; }

	public abstract StiBrush NeedleCapBorderBrush { get; }

	public abstract float NeedleBorderWidth { get; }

	public abstract float NeedleCapBorderWidth { get; }

	public abstract float NeedleStartWidth { get; }

	public abstract float NeedleEndWidth { get; }

	public abstract float NeedleRelativeHeight { get; }

	public abstract float NeedleRelativeWith { get; }

	public virtual StiMarkerSkin MarkerSkin => StiMarkerSkin.TriangleRight;

	public abstract StiBrush MarkerBrush { get; }

	public abstract StiBrush LinearMarkerBorder { get; }

	public virtual StiBrush MarkerBorderBrush => (StiBrush)new StiEmptyBrush();

	public virtual float MarkerBorderWidth => 0f;

	[StiNonSerialized]
	public virtual StiGaugeStyleId StyleId => StiGaugeStyleId.StiStyle26;

	[Browsable(false)]
	public StiGauge Gauge { get; set; }
}
