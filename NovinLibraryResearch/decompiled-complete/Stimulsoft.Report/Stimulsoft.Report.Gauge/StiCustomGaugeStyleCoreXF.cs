using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Gauge;

public class StiCustomGaugeStyleCoreXF : StiGaugeStyleCoreXF25
{
	public override string LocalizedName => "CustomStyle";

	public StiGaugeStyle ReportGaugeStyle { get; private set; }

	public string ReportStyleName
	{
		get
		{
			if (ReportGaugeStyle != null)
			{
				return ReportGaugeStyle.Name;
			}
			return null;
		}
	}

	public override StiBrush LinearScaleBrush
	{
		get
		{
			if (ReportGaugeStyle == null)
			{
				return base.LinearScaleBrush;
			}
			return ReportGaugeStyle.LinearScaleBrush;
		}
	}

	public override StiBrush Brush
	{
		get
		{
			if (ReportGaugeStyle == null)
			{
				return base.Brush;
			}
			return ReportGaugeStyle.Brush;
		}
	}

	public override Color BorderColor
	{
		get
		{
			if (ReportGaugeStyle == null)
			{
				return base.BorderColor;
			}
			return ReportGaugeStyle.BorderColor;
		}
	}

	public override Color ForeColor
	{
		get
		{
			if (ReportGaugeStyle == null)
			{
				return base.ForeColor;
			}
			return ReportGaugeStyle.ForeColor;
		}
	}

	public override Color TargetColor
	{
		get
		{
			if (ReportGaugeStyle == null)
			{
				return base.TargetColor;
			}
			return ReportGaugeStyle.TargetColor;
		}
	}

	public override float BorderWidth
	{
		get
		{
			if (ReportGaugeStyle == null)
			{
				return base.BorderWidth;
			}
			return ReportGaugeStyle.BorderWidth;
		}
	}

	public override StiBrush TickMarkMajorBrush
	{
		get
		{
			if (ReportGaugeStyle == null)
			{
				return base.TickMarkMajorBrush;
			}
			return ReportGaugeStyle.TickMarkMajorBrush;
		}
	}

	public override StiBrush TickMarkMajorBorder
	{
		get
		{
			if (ReportGaugeStyle == null)
			{
				return base.TickMarkMajorBorder;
			}
			return ReportGaugeStyle.TickMarkMajorBorder;
		}
	}

	public override float TickMarkMajorBorderWidth
	{
		get
		{
			if (ReportGaugeStyle == null)
			{
				return base.TickMarkMajorBorderWidth;
			}
			return ReportGaugeStyle.TickMarkMajorBorderWidth;
		}
	}

	public override StiBrush TickMarkMinorBrush
	{
		get
		{
			if (ReportGaugeStyle == null)
			{
				return base.TickMarkMinorBrush;
			}
			return ReportGaugeStyle.TickMarkMinorBrush;
		}
	}

	public override StiBrush TickMarkMinorBorder
	{
		get
		{
			if (ReportGaugeStyle == null)
			{
				return base.TickMarkMinorBorder;
			}
			return ReportGaugeStyle.TickMarkMinorBorder;
		}
	}

	public override float TickMarkMinorBorderWidth
	{
		get
		{
			if (ReportGaugeStyle == null)
			{
				return base.TickMarkMinorBorderWidth;
			}
			return ReportGaugeStyle.TickMarkMinorBorderWidth;
		}
	}

	public override StiBrush TickLabelMajorTextBrush
	{
		get
		{
			if (ReportGaugeStyle == null)
			{
				return base.TickLabelMajorTextBrush;
			}
			return ReportGaugeStyle.TickLabelMajorTextBrush;
		}
	}

	public override Font TickLabelMajorFont
	{
		get
		{
			if (ReportGaugeStyle == null)
			{
				return base.TickLabelMajorFont;
			}
			return ReportGaugeStyle.TickLabelMajorFont;
		}
	}

	public override StiBrush TickLabelMinorTextBrush
	{
		get
		{
			if (ReportGaugeStyle == null)
			{
				return base.TickLabelMinorTextBrush;
			}
			return ReportGaugeStyle.TickLabelMinorTextBrush;
		}
	}

	public override Font TickLabelMinorFont
	{
		get
		{
			if (ReportGaugeStyle == null)
			{
				return base.TickLabelMinorFont;
			}
			return ReportGaugeStyle.TickLabelMinorFont;
		}
	}

	public override StiBrush MarkerBrush
	{
		get
		{
			if (ReportGaugeStyle == null)
			{
				return base.MarkerBrush;
			}
			return ReportGaugeStyle.MarkerBrush;
		}
	}

	public override StiBrush LinearBarBrush
	{
		get
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (ReportGaugeStyle == null)
			{
				return (StiBrush)new StiSolidBrush("#4472c4");
			}
			return ReportGaugeStyle.LinearBarBrush;
		}
	}

	public override StiBrush LinearBarBorderBrush
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			if (ReportGaugeStyle == null)
			{
				return (StiBrush)new StiEmptyBrush();
			}
			return ReportGaugeStyle.LinearBarBorderBrush;
		}
	}

	public override StiBrush LinearBarEmptyBrush
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			if (ReportGaugeStyle == null)
			{
				return (StiBrush)new StiEmptyBrush();
			}
			return ReportGaugeStyle.LinearBarEmptyBrush;
		}
	}

	public override StiBrush LinearBarEmptyBorderBrush
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			if (ReportGaugeStyle == null)
			{
				return (StiBrush)new StiEmptyBrush();
			}
			return ReportGaugeStyle.LinearBarEmptyBorderBrush;
		}
	}

	public override float LinearBarStartWidth => 0.1f;

	public override float LinearBarEndWidth => 0.1f;

	public override StiBrush RadialBarBrush
	{
		get
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (ReportGaugeStyle == null)
			{
				return (StiBrush)new StiSolidBrush("#ffc000");
			}
			return ReportGaugeStyle.RadialBarBrush;
		}
	}

	public override StiBrush RadialBarBorderBrush
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			if (ReportGaugeStyle == null)
			{
				return (StiBrush)new StiEmptyBrush();
			}
			return ReportGaugeStyle.RadialBarBorderBrush;
		}
	}

	public override StiBrush RadialBarEmptyBrush
	{
		get
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (ReportGaugeStyle == null)
			{
				return (StiBrush)new StiSolidBrush("#43682b");
			}
			return ReportGaugeStyle.RadialBarEmptyBrush;
		}
	}

	public override StiBrush RadialBarEmptyBorderBrush
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			if (ReportGaugeStyle == null)
			{
				return (StiBrush)new StiEmptyBrush();
			}
			return ReportGaugeStyle.RadialBarEmptyBorderBrush;
		}
	}

	public override float RadialBarStartWidth => 0.1f;

	public override float RadialBarEndWidth => 0.1f;

	public override StiBrush NeedleBrush
	{
		get
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (ReportGaugeStyle == null)
			{
				return (StiBrush)new StiSolidBrush("#ffc000");
			}
			return ReportGaugeStyle.NeedleBrush;
		}
	}

	public override StiBrush NeedleBorderBrush
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			if (ReportGaugeStyle == null)
			{
				return (StiBrush)new StiEmptyBrush();
			}
			return ReportGaugeStyle.NeedleBorderBrush;
		}
	}

	public override StiBrush NeedleCapBrush
	{
		get
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (ReportGaugeStyle == null)
			{
				return (StiBrush)new StiSolidBrush("#ffc000");
			}
			return ReportGaugeStyle.NeedleCapBrush;
		}
	}

	public override StiBrush NeedleCapBorderBrush
	{
		get
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (ReportGaugeStyle == null)
			{
				return (StiBrush)new StiSolidBrush("#ffc000");
			}
			return ReportGaugeStyle.NeedleCapBorderBrush;
		}
	}

	public override float NeedleBorderWidth
	{
		get
		{
			if (ReportGaugeStyle == null)
			{
				return base.NeedleBorderWidth;
			}
			return ReportGaugeStyle.NeedleBorderWidth;
		}
	}

	public override float NeedleCapBorderWidth
	{
		get
		{
			if (ReportGaugeStyle == null)
			{
				return base.NeedleBorderWidth;
			}
			return ReportGaugeStyle.NeedleBorderWidth;
		}
	}

	public override float NeedleStartWidth => 0.1f;

	public override float NeedleEndWidth => 1f;

	public override float NeedleRelativeHeight => 0.08f;

	public override float NeedleRelativeWith => 0.55f;

	public StiCustomGaugeStyleCoreXF(StiGaugeStyle style)
	{
		ReportGaugeStyle = style;
	}
}
