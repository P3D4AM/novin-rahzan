using System;

namespace Stimulsoft.Report.Chart;

public class StiAxisTitleCoreXF : IStiApplyStyle, ICloneable
{
	private IStiAxisTitle title;

	public IStiAxisTitle Title
	{
		get
		{
			return title;
		}
		set
		{
			title = value;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public virtual void ApplyStyle(IStiChartStyle style)
	{
		if (Title.AllowApplyStyle)
		{
			Title.Color = style.Core.AxisTitleColor;
		}
	}

	public StiAxisTitleCoreXF(IStiAxisTitle title)
	{
		this.title = title;
	}
}
