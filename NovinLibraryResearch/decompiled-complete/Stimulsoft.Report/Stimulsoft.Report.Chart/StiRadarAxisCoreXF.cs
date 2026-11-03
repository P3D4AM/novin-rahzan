using System;

namespace Stimulsoft.Report.Chart;

public abstract class StiRadarAxisCoreXF : ICloneable, IStiApplyStyle
{
	private IStiRadarAxis axis;

	public IStiRadarAxis Axis
	{
		get
		{
			return axis;
		}
		set
		{
			axis = value;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public abstract void ApplyStyle(IStiChartStyle style);

	public StiRadarAxisCoreXF(IStiRadarAxis axis)
	{
		this.axis = axis;
	}
}
