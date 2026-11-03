using System;

namespace Stimulsoft.Report.Chart;

public class StiRadarGridLinesCoreXF : IStiApplyStyle, ICloneable
{
	private IStiRadarGridLines gridLines;

	public IStiRadarGridLines GridLines
	{
		get
		{
			return gridLines;
		}
		set
		{
			gridLines = value;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public virtual void ApplyStyle(IStiChartStyle style)
	{
		if (GridLines.AllowApplyStyle)
		{
			if (GridLines is IStiRadarGridLinesVert)
			{
				GridLines.Color = style.Core.GridLinesVertColor;
			}
			else
			{
				GridLines.Color = style.Core.GridLinesHorColor;
			}
		}
	}

	public StiRadarGridLinesCoreXF(IStiRadarGridLines gridLines)
	{
		this.gridLines = gridLines;
	}
}
