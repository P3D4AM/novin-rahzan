using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiWaterfallArea : StiAxisArea, IStiWaterfallArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiWaterfallArea;

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
		stiPropertyCollection.Add(StiPropertyCategories.Main, new StiPropertyObject[1] { propertyGrid.PropertiesHelper.WaterfallArea() });
		return stiPropertyCollection;
	}

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiWaterfallSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiWaterfallSeries) };
	}

	public override Type[] GetSeriesLabelsTypes()
	{
		return new Type[8]
		{
			typeof(StiNoneLabels),
			typeof(StiInsideBaseAxisLabels),
			typeof(StiInsideEndAxisLabels),
			typeof(StiCenterAxisLabels),
			typeof(StiOutsideBaseAxisLabels),
			typeof(StiOutsideEndAxisLabels),
			typeof(StiOutsideAxisLabels),
			typeof(StiValueAxisLabels)
		};
	}

	public override StiArea CreateNew()
	{
		return new StiWaterfallArea();
	}

	[StiUniversalConstructor("Area")]
	public StiWaterfallArea()
	{
		base.Core = new StiWaterfallAreaCoreXF(this);
	}
}
