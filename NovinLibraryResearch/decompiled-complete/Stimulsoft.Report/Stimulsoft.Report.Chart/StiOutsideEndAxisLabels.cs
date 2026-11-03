using System;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiOutsideEndAxisLabels : StiCenterAxisLabels, IStiOutsideEndAxisLabels, IStiCenterAxisLabels, IStiAxisSeriesLabels, IStiSeriesLabels, ICloneable, IStiJsonReportObject
{
	public override StiComponentId ComponentId => StiComponentId.StiOutsideEndAxisLabels;

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.SeriesOutsideEndAxisLabels() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override StiSeriesLabels CreateNew()
	{
		return new StiOutsideEndAxisLabels();
	}

	public StiOutsideEndAxisLabels()
	{
		base.Core = new StiOutsideEndAxisLabelsCoreXF(this);
	}
}
