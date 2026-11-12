using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Data.Design;

namespace Stimulsoft.Data.Engine;

[TypeConverter(typeof(StiDataFilterRuleConverter))]
public class StiDataFilterRule : StiDataRule, IStiJsonReportObject
{
	[StiSerializable]
	[DefaultValue(null)]
	public string Key { get; set; }

	[Browsable(false)]
	[StiNonSerialized]
	public string ElementKey { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	public string Path { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	public string Path2 { get; set; }

	[StiSerializable]
	[DefaultValue(StiDataFilterCondition.EqualTo)]
	public StiDataFilterCondition Condition { get; set; }

	[StiSerializable]
	[DefaultValue(StiDataFilterOperation.AND)]
	public StiDataFilterOperation Operation { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	public string Value { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	public string Value2 { get; set; }

	[StiSerializable]
	[DefaultValue(true)]
	public bool IsEnabled { get; set; }

	[StiSerializable]
	[DefaultValue(false)]
	public bool IsExpression { get; set; }

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyStringNullOrEmpty("Key", Key);
		jObject.AddPropertyStringNullOrEmpty("Path", Path);
		jObject.AddPropertyStringNullOrEmpty("Path2", Path2);
		jObject.AddPropertyEnum("Condition", Condition, StiDataFilterCondition.EqualTo);
		jObject.AddPropertyEnum("Operation", Operation, StiDataFilterOperation.AND);
		jObject.AddPropertyStringNullOrEmpty("Value", Value);
		jObject.AddPropertyStringNullOrEmpty("Value2", Value2);
		jObject.AddPropertyBool("IsEnabled", IsEnabled, defaultValue: true);
		jObject.AddPropertyBool("IsExpression", IsExpression);
		return jObject;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Key":
				Key = item.DeserializeString();
				break;
			case "Path":
				Path = item.DeserializeString();
				break;
			case "Path2":
				Path2 = item.DeserializeString();
				break;
			case "Condition":
				Condition = item.DeserializeEnum<StiDataFilterCondition>();
				break;
			case "Operation":
				Operation = item.DeserializeEnum<StiDataFilterOperation>();
				break;
			case "Value":
				Value = item.DeserializeString();
				break;
			case "Value2":
				Value2 = item.DeserializeString();
				break;
			case "IsEnabled":
				IsEnabled = item.DeserializeBool();
				break;
			case "IsExpression":
				IsExpression = item.DeserializeBool();
				break;
			}
		}
	}

	public static StiDataFilterRule LoadFromJson(string json)
	{
		StiDataFilterRule stiDataFilterRule = new StiDataFilterRule();
		stiDataFilterRule.LoadFromJsonObject(JObject.Parse(json));
		return stiDataFilterRule;
	}

	public static StiDataFilterRule LoadFromJson(JObject json)
	{
		StiDataFilterRule stiDataFilterRule = new StiDataFilterRule();
		stiDataFilterRule.LoadFromJsonObject(json);
		return stiDataFilterRule;
	}

	public virtual string SaveToString()
	{
		return JsonConvert.SerializeObject(this, Formatting.Indented, StiJsonHelper.DefaultSerializerSettings);
	}

	public override string ToString()
	{
		StiDataFilterCondition condition = Condition;
		if ((uint)(condition - 6) <= 1u || condition == StiDataFilterCondition.PairEqualTo)
		{
			return $"{Path} {Condition} {Value} - {Value2}";
		}
		return $"{Path} {Condition} {Value}";
	}

	public override int GetUniqueCode()
	{
		int num = ((Key != null) ? Key.GetHashCode() : 0);
		num = (num * 397) ^ ((Path != null) ? Path.GetHashCode() : 0);
		num = (num * 397) ^ ((Path2 != null) ? Path2.GetHashCode() : 0);
		num = (num * 397) ^ Condition.GetHashCode();
		num = (num * 397) ^ Operation.GetHashCode();
		num = (num * 397) ^ ((Value != null) ? Value.GetHashCode() : 0);
		num = (num * 397) ^ ((Value2 != null) ? Value2.GetHashCode() : 0);
		num = (num * 397) ^ IsEnabled.GetHashCode();
		return (num * 397) ^ IsExpression.GetHashCode();
	}

	public List<StiDataFilterRule> ToList()
	{
		return new List<StiDataFilterRule> { this };
	}

	public string GetStringRepresentation()
	{
		string text = Path;
		if (string.IsNullOrWhiteSpace(text))
		{
			text = "[" + Loc.GetMain("Field") + "]";
		}
		string text2 = Path2;
		if (string.IsNullOrWhiteSpace(text2))
		{
			text2 = "[" + Loc.GetMain("Field") + "]";
		}
		string text3 = GetValue(Value);
		if (string.IsNullOrWhiteSpace(text3))
		{
			text3 = "[" + Loc.GetMain("Value") + "]";
		}
		string text4 = GetValue(Value2);
		if (string.IsNullOrWhiteSpace(text4))
		{
			text4 = "[" + Loc.GetMain("Value") + "]";
		}
		return Condition switch
		{
			StiDataFilterCondition.IsBlank => text + " " + Loc.GetEnum("StiFilterConditionIsBlank"), 
			StiDataFilterCondition.IsNotBlank => text + " " + Loc.GetEnum("StiFilterConditionIsNotBlank"), 
			StiDataFilterCondition.IsNull => text + " " + Loc.GetEnum("StiFilterConditionIsNull"), 
			StiDataFilterCondition.IsNotNull => text + " " + Loc.GetEnum("StiFilterConditionIsNotNull"), 
			StiDataFilterCondition.Containing => text + " " + Loc.GetEnum("StiFilterConditionContaining") + " " + text3, 
			StiDataFilterCondition.NotContaining => text + " " + Loc.GetEnum("StiFilterConditionNotContaining") + " " + text3, 
			StiDataFilterCondition.PairEqualTo => text + " = " + text3 + " AND " + text2 + " = " + text4, 
			StiDataFilterCondition.EqualTo => text + " = " + text3, 
			StiDataFilterCondition.NotEqualTo => text + " <> " + text3, 
			StiDataFilterCondition.GreaterThan => text + " > " + text3, 
			StiDataFilterCondition.GreaterThanOrEqualTo => text + " >= " + text3, 
			StiDataFilterCondition.LessThan => text + " < " + text3, 
			StiDataFilterCondition.LessThanOrEqualTo => text + " <= " + text3, 
			StiDataFilterCondition.BeginningWith => text + " " + Loc.GetEnum("StiFilterConditionBeginningWith") + " " + text3, 
			StiDataFilterCondition.EndingWith => text + " " + Loc.GetEnum("StiFilterConditionEndingWith") + " " + text3, 
			StiDataFilterCondition.Between => text + " " + Loc.GetEnum("StiFilterConditionBetween") + " " + text3 + " " + Loc.GetEnum("StiFilterModeAnd").ToLowerInvariant() + " " + text4, 
			StiDataFilterCondition.NotBetween => text + " " + Loc.GetEnum("StiFilterConditionNotBetween") + " " + text3 + " " + Loc.GetEnum("StiFilterModeAnd").ToLowerInvariant() + " " + text4, 
			_ => "", 
		};
	}

	private string GetValue(string value)
	{
		value = value ?? "";
		if (DateTime.TryParseExact(value, "MM'/'dd'/'yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
		{
			return "'" + result.ToShortDateString() + "'";
		}
		if (decimal.TryParse(value, out var result2))
		{
			return $"{result2}";
		}
		if (value != null && (value.ToLowerInvariant() == "true" || value.ToLowerInvariant() == "false"))
		{
			return value;
		}
		return "'" + value + "'";
	}

	public StiDataFilterRule()
		: this(null, null, StiDataFilterCondition.EqualTo, null, null, isEnabled: true, isExpression: false)
	{
	}

	public StiDataFilterRule(string path, StiDataFilterCondition condition)
		: this(null, path, condition, null, null, isEnabled: true, isExpression: false)
	{
	}

	public StiDataFilterRule(string path, StiDataFilterCondition condition, string value)
		: this(null, path, condition, value, null, isEnabled: true, isExpression: false)
	{
	}

	public StiDataFilterRule(string key, string path, StiDataFilterCondition condition)
		: this(key, path, condition, null, null, isEnabled: true, isExpression: false)
	{
	}

	public StiDataFilterRule(string key, string path, StiDataFilterCondition condition, string value)
		: this(key, path, condition, value, null, isEnabled: true, isExpression: false)
	{
	}

	public StiDataFilterRule(string path, StiDataFilterCondition condition, string value1, string value2)
		: this(null, path, condition, value1, value2, isEnabled: true, isExpression: false)
	{
	}

	public StiDataFilterRule(string key, string path, StiDataFilterCondition condition, string value, string value2, bool isEnabled, bool isExpression)
		: this(key, path, null, condition, value, value2, isEnabled, isExpression)
	{
	}

	public StiDataFilterRule(string key, string path, string path2, StiDataFilterCondition condition, string value, string value2, bool isEnabled, bool isExpression)
		: this(key, path, path2, condition, StiDataFilterOperation.AND, value, value2, isEnabled, isExpression)
	{
	}

	public StiDataFilterRule(string key, string path, string path2, StiDataFilterCondition condition, StiDataFilterOperation operation, string value, string value2, bool isEnabled, bool isExpression)
	{
		Key = key;
		Path = path;
		Path2 = path2;
		Condition = condition;
		Operation = operation;
		Value = value;
		Value2 = value2;
		IsEnabled = isEnabled;
		IsExpression = isExpression;
	}
}
