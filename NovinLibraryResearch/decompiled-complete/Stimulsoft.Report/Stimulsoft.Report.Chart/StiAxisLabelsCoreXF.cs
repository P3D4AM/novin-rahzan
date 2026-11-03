using System;

namespace Stimulsoft.Report.Chart;

public class StiAxisLabelsCoreXF : IStiApplyStyle, ICloneable
{
	public IStiAxisLabels Labels { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	public virtual void ApplyStyle(IStiChartStyle style)
	{
		if (Labels.AllowApplyStyle)
		{
			Labels.Color = style.Core.AxisLabelsColor;
		}
	}

	public StiAxisLabelsCoreXF(IStiAxisLabels labels)
	{
		Labels = labels;
	}
}
