using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dashboard.Design;

namespace Stimulsoft.Report.Dashboard;

[TypeConverter(typeof(StiUserViewStateConverter))]
public class StiUserViewState : ICloneable, IStiJsonReportObject
{
	[StiSerializable]
	[DefaultValue(null)]
	public string Key { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	public string Name { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	public string State { get; set; }

	[StiSerializable]
	[DefaultValue(StiChartSeriesType.ClusteredColumn)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiChartSeriesType SeriesType { get; set; } = StiChartSeriesType.ClusteredColumn;


	public object Clone()
	{
		return MemberwiseClone();
	}

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Key", Key);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Name", Name);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "State", State);
		JsonExtensions.AddPropertyEnum(val, "SeriesType", (Enum)SeriesType, (Enum)StiChartSeriesType.ClusteredColumn);
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Key":
				Key = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Name":
				Name = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "State":
				State = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "SeriesType":
				SeriesType = StiJsonReportObjectHelper.DeserializeEnum<StiChartSeriesType>(item);
				break;
			}
		}
	}

	public virtual string SaveToString()
	{
		return JsonConvert.SerializeObject((object)this, (Formatting)1, StiJsonHelper.DefaultSerializerSettings);
	}

	public List<StiUserViewState> ToList()
	{
		return new List<StiUserViewState> { this };
	}

	public static StiUserViewState LoadFromJson(string json)
	{
		StiUserViewState stiUserViewState = new StiUserViewState();
		stiUserViewState.LoadFromJsonObject(JObject.Parse(json));
		return stiUserViewState;
	}

	public static StiUserViewState LoadFromJson(JObject json)
	{
		StiUserViewState stiUserViewState = new StiUserViewState();
		stiUserViewState.LoadFromJsonObject(json);
		return stiUserViewState;
	}

	public StiUserViewState()
		: this(StiKeyHelper.GenerateKey(), "Name", null, StiChartSeriesType.ClusteredColumn)
	{
	}

	public StiUserViewState(string key, string name, string state, StiChartSeriesType seriesType)
	{
		Key = key;
		Name = name;
		State = state;
		SeriesType = seriesType;
	}
}
