using System.Collections.Generic;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Data.Design;

namespace Stimulsoft.Data.Engine;

[TypeConverter(typeof(StiDataSortRuleConverter))]
public class StiDataSortRule : StiDataRule, IStiJsonReportObject
{
	[StiSerializable]
	[DefaultValue(null)]
	public string Key { get; set; }

	[StiSerializable]
	[DefaultValue(StiDataSortDirection.Ascending)]
	public StiDataSortDirection Direction { get; set; }

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyStringNullOrEmpty("Key", Key);
		jObject.AddPropertyEnum("Direction", Direction, StiDataSortDirection.Ascending);
		return jObject;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "Key"))
			{
				if (name == "Direction")
				{
					Direction = item.DeserializeEnum<StiDataSortDirection>();
				}
			}
			else
			{
				Key = item.DeserializeString();
			}
		}
	}

	public static StiDataSortRule LoadFromJson(string json)
	{
		StiDataSortRule stiDataSortRule = new StiDataSortRule();
		stiDataSortRule.LoadFromJsonObject(JObject.Parse(json));
		return stiDataSortRule;
	}

	public static StiDataSortRule LoadFromJson(JObject json)
	{
		StiDataSortRule stiDataSortRule = new StiDataSortRule();
		stiDataSortRule.LoadFromJsonObject(json);
		return stiDataSortRule;
	}

	public virtual string SaveToString()
	{
		return JsonConvert.SerializeObject(this, Formatting.Indented, StiJsonHelper.DefaultSerializerSettings);
	}

	public override string ToString()
	{
		return $"{Direction} {Key}";
	}

	public override int GetUniqueCode()
	{
		return (((Key != null) ? Key.GetHashCode() : 0) * 397) ^ (int)Direction;
	}

	public string GetStringRepresentation()
	{
		string text = Key;
		if (string.IsNullOrWhiteSpace(text))
		{
			text = "[" + Loc.GetMain("Field") + "]";
		}
		return Direction switch
		{
			StiDataSortDirection.Ascending => text + " " + Loc.GetEnum("StiSortDirectionAsc"), 
			StiDataSortDirection.Descending => text + " " + Loc.GetEnum("StiSortDirectionDesc"), 
			_ => "", 
		};
	}

	public List<StiDataSortRule> ToList()
	{
		return new List<StiDataSortRule> { this };
	}

	public StiDataSortRule()
		: this(StiKeyHelper.GenerateKey(), StiDataSortDirection.Ascending)
	{
	}

	public StiDataSortRule(string key, StiDataSortDirection direction)
	{
		Key = key;
		Direction = direction;
	}
}
