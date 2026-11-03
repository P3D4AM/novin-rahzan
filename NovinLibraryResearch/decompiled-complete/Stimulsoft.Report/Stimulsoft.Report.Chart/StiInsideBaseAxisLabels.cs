using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiInsideBaseAxisLabels : StiCenterAxisLabels, IStiInsideBaseAxisLabels, IStiCenterAxisLabels, IStiAxisSeriesLabels, IStiSeriesLabels, ICloneable, IStiJsonReportObject
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiInsideBaseAxisLabels;

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.SeriesInsideBaseAxisLabels() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override StiSeriesLabels CreateNew()
	{
		return new StiInsideBaseAxisLabels();
	}

	public StiInsideBaseAxisLabels()
	{
		base.Core = new StiInsideBaseAxisLabelsCoreXF(this);
	}
}
