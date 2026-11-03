using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public abstract class StiAreaCoreXF : ICloneable, IStiApplyStyle
{
	private IStiArea area;

	public IStiArea Area
	{
		get
		{
			return area;
		}
		set
		{
			area = value;
		}
	}

	public abstract string LocalizedName { get; }

	[Browsable(false)]
	public virtual StiChartSeriesOrientation SeriesOrientation => StiChartSeriesOrientation.Vertical;

	[Browsable(false)]
	public abstract int Position { get; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	public virtual void ApplyStyle(IStiChartStyle style)
	{
		if (Area.AllowApplyStyle)
		{
			Area.Brush = style.Core.ChartAreaBrush;
			Area.BorderColor = style.Core.ChartAreaBorderColor;
			Area.ShowShadow = style.Core.ChartAreaShowShadow;
		}
	}

	public abstract StiCellGeom Render(StiContext context, RectangleF rect);

	protected abstract void PrepareInfo(RectangleF rect);

	public bool CheckInLabelsTypes(Type typeForCheck)
	{
		Type[] seriesLabelsTypes = area.GetSeriesLabelsTypes();
		Type[] array = seriesLabelsTypes;
		foreach (Type type in array)
		{
			if (type.ToString() == typeForCheck.ToString())
			{
				return true;
			}
		}
		return false;
	}

	public List<IStiSeries> GetSeries()
	{
		Hashtable hashtable = new Hashtable();
		Type[] seriesTypes = Area.GetSeriesTypes();
		Type[] array = seriesTypes;
		foreach (Type type in array)
		{
			hashtable[type] = type;
		}
		List<IStiSeries> list = new List<IStiSeries>();
		foreach (IStiSeries item in Area.Chart.Series)
		{
			if (hashtable[item.GetType()] != null)
			{
				list.Add(item);
			}
		}
		return list;
	}

	public bool IsAcceptableSeries(Type seriesType)
	{
		Type[] seriesTypes = Area.GetSeriesTypes();
		Type[] array = seriesTypes;
		foreach (Type type in array)
		{
			if (type == seriesType)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsAcceptableSeriesLabels(Type seriesLabelsType)
	{
		Type[] seriesLabelsTypes = Area.GetSeriesLabelsTypes();
		Type[] array = seriesLabelsTypes;
		foreach (Type type in array)
		{
			if (type == seriesLabelsType)
			{
				return true;
			}
		}
		return false;
	}

	public StiAreaCoreXF(IStiArea area)
	{
		this.area = area;
	}
}
