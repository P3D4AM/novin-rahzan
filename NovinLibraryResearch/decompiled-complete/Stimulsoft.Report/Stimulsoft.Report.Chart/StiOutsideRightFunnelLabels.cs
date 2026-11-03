using System;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiOutsideRightFunnelLabels : StiFunnelSeriesLabels, IStiOutsideRightFunnelLabels, IStiCenterFunnelLabels, IStiFunnelSeriesLabels, IStiSeriesLabels, ICloneable, IStiJsonReportObject
{
	public override StiComponentId ComponentId => StiComponentId.StiOutsideRightFunnelLabels;

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.SeriesOutsideRightFunnelLabels() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override StiSeriesLabels CreateNew()
	{
		return new StiOutsideRightFunnelLabels();
	}

	public StiOutsideRightFunnelLabels()
	{
		base.Core = new StiOutsideRightFunnelLabelsCoreXF(this);
	}
}
