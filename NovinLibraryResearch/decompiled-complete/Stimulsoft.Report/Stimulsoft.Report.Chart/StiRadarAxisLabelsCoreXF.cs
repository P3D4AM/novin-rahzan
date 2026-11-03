using System;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiRadarAxisLabelsCoreXF : IStiApplyStyle, ICloneable
{
	private IStiRadarAxisLabels labels;

	public IStiRadarAxisLabels Labels
	{
		get
		{
			return labels;
		}
		set
		{
			labels = value;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public virtual void ApplyStyle(IStiChartStyle style)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		if (Labels.AllowApplyStyle)
		{
			Labels.Color = style.Core.AxisLabelsColor;
			Labels.Brush = (StiBrush)new StiEmptyBrush();
		}
	}

	public StiRadarAxisLabelsCoreXF(IStiRadarAxisLabels labels)
	{
		this.labels = labels;
	}
}
