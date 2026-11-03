using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiClusteredColumnArea : StiAxisArea, IStiClusteredColumnArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiClusteredColumnArea;

	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[DefaultValue(false)]
	public bool RoundValues { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "RoundValues", RoundValues, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "RoundValues")
			{
				RoundValues = StiJsonReportObjectHelper.DeserializeBool(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.Main, new StiPropertyObject[1] { propertyGrid.PropertiesHelper.ClusteredColumnArea() });
		return stiPropertyCollection;
	}

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiClusteredColumnSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[7]
		{
			typeof(StiClusteredColumnSeries),
			typeof(StiLineSeries),
			typeof(StiSteppedLineSeries),
			typeof(StiSplineSeries),
			typeof(StiAreaSeries),
			typeof(StiSteppedAreaSeries),
			typeof(StiSplineAreaSeries)
		};
	}

	public override StiArea CreateNew()
	{
		return new StiClusteredColumnArea();
	}

	[StiUniversalConstructor("Area")]
	public StiClusteredColumnArea()
	{
		base.Core = new StiClusteredColumnAreaCoreXF(this);
	}
}
