using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiSteppedLineSeries : StiBaseLineSeries, IStiSteppedLineSeries, IStiBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyColorNegative, IStiShowNullsSeries, IStiShowZerosSeries
{
	private bool pointAtCenter;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiSteppedLineSeries;

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates where to place center of series segment.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool PointAtCenter
	{
		get
		{
			return pointAtCenter;
		}
		set
		{
			pointAtCenter = value;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "PointAtCenter", pointAtCenter, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "PointAtCenter")
			{
				pointAtCenter = StiJsonReportObjectHelper.DeserializeBool(item);
			}
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
		value = new StiPropertyObject[7]
		{
			propertiesHelper.LabelsOffset(),
			propertiesHelper.Lighting(),
			propertiesHelper.LineColor(),
			propertiesHelper.LineColorNegative(),
			propertiesHelper.LineStyle(),
			propertiesHelper.LineWidth(),
			propertiesHelper.ShowShadow()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[8]
		{
			propertiesHelper.AllowApplyStyle(),
			propertiesHelper.AllowApplyColorNegative(),
			propertiesHelper.PointAtCenter(),
			propertiesHelper.ShowInLegend(),
			propertiesHelper.ShowNulls(),
			propertiesHelper.ShowSeriesLabels(),
			propertiesHelper.Title(),
			propertiesHelper.YAxis()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		return stiPropertyCollection;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiClusteredColumnArea);
	}

	public override StiSeries CreateNew()
	{
		return new StiSteppedLineSeries();
	}

	public StiSteppedLineSeries()
	{
		base.Core = new StiSteppedLineSeriesCoreXF(this);
	}
}
