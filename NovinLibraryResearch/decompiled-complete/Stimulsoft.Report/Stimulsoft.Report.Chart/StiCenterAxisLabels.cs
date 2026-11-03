using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiCenterAxisLabels : StiAxisSeriesLabels, IStiCenterAxisLabels, IStiAxisSeriesLabels, IStiSeriesLabels, ICloneable, IStiJsonReportObject
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiCenterAxisLabels;

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.SeriesCenterAxisLabels() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override StiSeriesLabels CreateNew()
	{
		return new StiCenterAxisLabels();
	}

	public StiCenterAxisLabels()
	{
		base.Core = new StiCenterAxisLabelsCoreXF(this);
	}
}
