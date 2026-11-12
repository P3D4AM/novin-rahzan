using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Indicator.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Components.Indicator;

[JsonObject]
[TypeConverter(typeof(StiIndicatorIconRangeConverter))]
public class StiIndicatorIconRange : IStiJsonReportObject, IStiIndicatorIconRange, ICloneable
{
	[StiSerializable]
	[DefaultValue(StiFontIcons.Rating4)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiFontIcons Icon { get; set; } = StiFontIcons.Rating4;


	[DefaultValue("0")]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	public string StartExpression { get; set; } = "0";


	[DefaultValue("100")]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	public string EndExpression { get; set; } = "100";


	public object Clone()
	{
		return MemberwiseClone();
	}

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyStringNullOrEmpty("StartExpression", StartExpression);
		jObject.AddPropertyStringNullOrEmpty("EndExpression", EndExpression);
		jObject.AddPropertyEnum("Icon", Icon, StiFontIcons.Rating4);
		return jObject;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "StartExpression":
				StartExpression = item.DeserializeString();
				break;
			case "EndExpression":
				EndExpression = item.DeserializeString();
				break;
			case "Icon":
				Icon = item.DeserializeEnum<StiFontIcons>();
				break;
			}
		}
	}

	public static StiIndicatorIconRange LoadFromJson(string json)
	{
		StiIndicatorIconRange stiIndicatorIconRange = new StiIndicatorIconRange();
		stiIndicatorIconRange.LoadFromJsonObject(JObject.Parse(json));
		return stiIndicatorIconRange;
	}

	public static StiIndicatorIconRange LoadFromJson(JObject json)
	{
		StiIndicatorIconRange stiIndicatorIconRange = new StiIndicatorIconRange();
		stiIndicatorIconRange.LoadFromJsonObject(json);
		return stiIndicatorIconRange;
	}

	public virtual string SaveToString()
	{
		return JsonConvert.SerializeObject(this, Formatting.Indented, StiJsonHelper.DefaultSerializerSettings);
	}

	public string GetStringRepresentation()
	{
		return StartExpression + " - " + EndExpression;
	}

	public StiIndicatorIconRange()
	{
	}

	public StiIndicatorIconRange(StiFontIcons icon, string start, string end)
	{
		Icon = icon;
		StartExpression = start;
		EndExpression = end;
	}
}
