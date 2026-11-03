using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiScatterLineSeries : StiScatterSeries, IStiScatterLineSeries, IStiScatterSeries, IStiBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyColorNegative, IStiShowNullsSeries, IStiShowZerosSeries
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiScatterLineSeries;

	[Browsable(true)]
	public override Color LineColor
	{
		get
		{
			return base.LineColor;
		}
		set
		{
			base.LineColor = value;
		}
	}

	[Browsable(true)]
	public override StiPenStyle LineStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.LineStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.LineStyle = value;
		}
	}

	[Browsable(true)]
	public override bool Lighting
	{
		get
		{
			return base.Lighting;
		}
		set
		{
			base.Lighting = value;
		}
	}

	[Browsable(true)]
	public override float LineWidth
	{
		get
		{
			return base.LineWidth;
		}
		set
		{
			base.LineWidth = value;
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[3]
		{
			propertiesHelper.ValueDataColumn(),
			propertiesHelper.Value(),
			propertiesHelper.ListOfValues()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Value, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.ArgumentDataColumn(),
			propertiesHelper.Argument(),
			propertiesHelper.ListOfArguments()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Argument, value);
		value = new StiPropertyObject[6]
		{
			propertiesHelper.Format(),
			propertiesHelper.SortBy(),
			propertiesHelper.SortDirection(),
			propertiesHelper.AutoSeriesKeyDataColumn(),
			propertiesHelper.AutoSeriesColorDataColumn(),
			propertiesHelper.AutoSeriesTitleDataColumn()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		value = new StiPropertyObject[6]
		{
			propertiesHelper.LabelsOffset(),
			propertiesHelper.Lighting(),
			propertiesHelper.LineColor(),
			propertiesHelper.LineStyle(),
			propertiesHelper.LineWidth(),
			propertiesHelper.ShowShadow()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[6]
		{
			propertiesHelper.AllowApplyStyle(),
			propertiesHelper.ShowInLegend(),
			propertiesHelper.ShowNulls(),
			propertiesHelper.ShowSeriesLabels(),
			propertiesHelper.Title(),
			propertiesHelper.YAxis()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		return stiPropertyCollection;
	}

	public override object Clone()
	{
		return base.Clone() as IStiScatterLineSeries;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiScatterArea);
	}

	public override StiSeries CreateNew()
	{
		return new StiScatterLineSeries();
	}

	public StiScatterLineSeries()
	{
		base.Core = new StiScatterLineSeriesCoreXF(this);
	}
}
