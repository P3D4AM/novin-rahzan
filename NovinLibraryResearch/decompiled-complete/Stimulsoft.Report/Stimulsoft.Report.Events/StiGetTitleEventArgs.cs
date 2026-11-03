using System;
using Stimulsoft.Report.Chart;

namespace Stimulsoft.Report.Events;

public class StiGetTitleEventArgs : EventArgs
{
	private string valueObject;

	private int index;

	private IStiSeries series;

	public virtual string Value
	{
		get
		{
			return valueObject;
		}
		set
		{
			valueObject = value;
		}
	}

	public virtual int Index
	{
		get
		{
			return index;
		}
		set
		{
			index = value;
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
}
