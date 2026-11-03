using System;

namespace Stimulsoft.Report.Chart;

public class StiGridLinesCoreXF : IStiApplyStyle, ICloneable
{
	private IStiGridLines gridLines;

	public IStiGridLines GridLines
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
			if (GridLines is IStiGridLinesVert)
			{
				GridLines.Color = style.Core.GridLinesVertColor;
				GridLines.MinorColor = style.Core.GridLinesVertColor;
			}
			else
			{
				GridLines.Color = style.Core.GridLinesHorColor;
				GridLines.MinorColor = style.Core.GridLinesHorColor;
			}
		}
	}

	public StiGridLinesCoreXF(IStiGridLines gridLines)
	{
		this.gridLines = gridLines;
	}
}
