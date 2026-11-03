using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiPictorialArea : StiArea, IStiPictorialArea, IStiArea, ICloneable, IStiJsonReportObject, IStiRoundValuesArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiPictorialArea;

	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public bool RoundValues { get; set; } = true;


	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public bool Actual { get; set; }

	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	public override bool ColorEach { get; set; } = true;


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "RoundValues", RoundValues, true, false);
		JsonExtensions.AddPropertyBool(val, "Actual", Actual, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "Actual"))
			{
				if (name == "RoundValues")
				{
					RoundValues = StiJsonReportObjectHelper.DeserializeBool(item);
				}
			}
			else
			{
				Actual = StiJsonReportObjectHelper.DeserializeBool(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.Main, new StiPropertyObject[1] { propertyGrid.PropertiesHelper.PictorialArea() });
		return stiPropertyCollection;
	}

	public override Type GetDefaultSeriesLabelsType()
	{
		return typeof(StiNoneLabels);
	}

	public override Type[] GetSeriesLabelsTypes()
	{
		return new Type[1] { typeof(StiNoneLabels) };
	}

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiPictorialSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiPictorialSeries) };
	}

	public override StiArea CreateNew()
	{
		return new StiPictorialArea();
	}

	[StiUniversalConstructor("Area")]
	public StiPictorialArea()
	{
		base.Core = new StiPictorialAreaCoreXF(this);
	}
}
