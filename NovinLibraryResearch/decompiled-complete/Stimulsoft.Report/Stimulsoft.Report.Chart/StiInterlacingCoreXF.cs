using System;

namespace Stimulsoft.Report.Chart;

public class StiInterlacingCoreXF : IStiApplyStyle, ICloneable
{
	private IStiInterlacing interlacing;

	public IStiInterlacing Interlacing
	{
		get
		{
			return interlacing;
		}
		set
		{
			interlacing = value;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public virtual void ApplyStyle(IStiChartStyle style)
	{
		if (Interlacing.AllowApplyStyle)
		{
			if (Interlacing is IStiInterlacingVert)
			{
				Interlacing.InterlacedBrush = style.Core.InterlacingVertBrush;
			}
			else
			{
				Interlacing.InterlacedBrush = style.Core.InterlacingHorBrush;
			}
		}
	}

	public StiInterlacingCoreXF(IStiInterlacing interlacing)
	{
		this.interlacing = interlacing;
	}
}
