using System;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiOutsideLeftFunnelLabels : StiFunnelSeriesLabels, IStiOutsideLeftFunnelLabels, IStiCenterFunnelLabels, IStiFunnelSeriesLabels, IStiSeriesLabels, ICloneable, IStiJsonReportObject
{
	public override StiComponentId ComponentId => StiComponentId.StiOutsideLeftFunnelLabels;

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.SeriesOutsideLeftFunnelLabels() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override StiSeriesLabels CreateNew()
	{
		return new StiOutsideLeftFunnelLabels();
	}

	public StiOutsideLeftFunnelLabels()
	{
		base.Core = new StiOutsideLeftFunnelLabelsCoreXF(this);
	}
}
