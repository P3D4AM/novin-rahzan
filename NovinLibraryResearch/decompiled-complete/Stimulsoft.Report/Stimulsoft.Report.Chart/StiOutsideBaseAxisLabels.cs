using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiOutsideBaseAxisLabels : StiCenterAxisLabels, IStiOutsideBaseAxisLabels, IStiCenterAxisLabels, IStiAxisSeriesLabels, IStiSeriesLabels, ICloneable, IStiJsonReportObject
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiOutsideBaseAxisLabels;

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.SeriesOutsideBaseAxisLabels() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override StiSeriesLabels CreateNew()
	{
		return new StiOutsideBaseAxisLabels();
	}

	public StiOutsideBaseAxisLabels()
	{
		base.Core = new StiOutsideBaseAxisLabelsCoreXF(this);
	}
}
