using System;
using Stimulsoft.Report.Chart;

namespace Stimulsoft.Report.Events;

public class StiNewAutoSeriesEventArgs : EventArgs
{
	private int seriesIndex;

	private object color;

	private IStiSeries series;

	public virtual int SeriesIndex
	{
		get
		{
			return seriesIndex;
		}
		set
		{
			seriesIndex = value;
		}
	}

	public virtual object Color
	{
		get
		{
			return color;
		}
		set
		{
			color = value;
		}
	}

	public virtual IStiSeries Series
	{
		get
		{
			return series;
		}
		set
		{
			series = value;
		}
	}

	public StiNewAutoSeriesEventArgs(int seriesIndex, IStiSeries series, object color)
	{
		this.seriesIndex = seriesIndex;
		this.series = series;
		this.color = color;
	}
}
