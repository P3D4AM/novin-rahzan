using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Data.Design;

namespace Stimulsoft.Data.Engine;

[TypeConverter(typeof(StiDataActionRuleConverter))]
public class StiDataActionRule : StiDataRule, IStiJsonReportObject
{
	[StiSerializable]
	public StiDataActionType Type { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	public string Key { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	public string Path { get; set; }

	[StiSerializable]
	[DefaultValue(0)]
	public int StartIndex { get; set; }

	[StiSerializable]
	[DefaultValue(-1)]
	public int RowsCount { get; set; } = -1;


	[StiSerializable]
	[DefaultValue(null)]
	public string InitialValue { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	public string ValueFrom { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	public string ValueTo { get; set; }

	[StiSerializable]
	[DefaultValue(false)]
	public bool MatchCase { get; set; }

	[StiSerializable]
	[DefaultValue(false)]
	public bool MatchWholeWord { get; set; }

	[StiSerializable]
	[DefaultValue(StiDataActionPriority.AfterGroupingData)]
	public StiDataActionPriority Priority { get; set; } = StiDataActionPriority.AfterGroupingData;


	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyEnum("Type", Type);
		jObject.AddPropertyStringNullOrEmpty("Key", Key);
		jObject.AddPropertyStringNullOrEmpty("Path", Path);
		jObject.AddPropertyInt("StartIndex", StartIndex);
		jObject.AddPropertyInt("RowsCount", RowsCount, -1);
		jObject.AddPropertyStringNullOrEmpty("InitialValue", InitialValue);
		jObject.AddPropertyStringNullOrEmpty("ValueFrom", ValueFrom);
		jObject.AddPropertyStringNullOrEmpty("ValueTo", ValueTo);
		jObject.AddPropertyBool("MatchCase", MatchCase);
		jObject.AddPropertyBool("MatchWholeWord", MatchWholeWord);
		jObject.AddPropertyEnum("Priority", Priority);
		return jObject;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Type":
				Type = item.DeserializeEnum<StiDataActionType>();
				break;
			case "Key":
				Key = item.DeserializeString();
				break;
			case "Path":
				Path = item.DeserializeString();
				break;
			case "StartIndex":
				StartIndex = item.DeserializeInt();
				break;
			case "RowsCount":
				RowsCount = item.DeserializeInt();
				break;
			case "InitialValue":
				InitialValue = item.DeserializeString();
				break;
			case "ValueFrom":
				ValueFrom = item.DeserializeString();
				break;
			case "ValueTo":
				ValueTo = item.DeserializeString();
				break;
			case "MatchCase":
				MatchCase = item.DeserializeBool();
				break;
			case "MatchWholeWord":
				MatchWholeWord = item.DeserializeBool();
				break;
			case "AfterGroupingData":
				if (item.DeserializeBool())
				{
					Priority = StiDataActionPriority.AfterGroupingData;
				}
				break;
			case "Priority":
				Priority = item.DeserializeEnum<StiDataActionPriority>();
				break;
			}
		}
	}

	public static StiDataActionRule LoadFromJson(JObject json)
	{
		StiDataActionRule stiDataActionRule = new StiDataActionRule();
		stiDataActionRule.LoadFromJsonObject(json);
		return stiDataActionRule;
	}

	public override int GetUniqueCode()
	{
		int type = (int)Type;
		type = (type * 397) ^ ((Key != null) ? Key.GetHashCode() : 0);
		type = (type * 397) ^ ((Path != null) ? Path.GetHashCode() : 0);
		type = (type * 397) ^ StartIndex;
		type = (type * 397) ^ RowsCount;
		type = (type * 397) ^ ((InitialValue != null) ? InitialValue.GetHashCode() : 0);
		type = (type * 397) ^ ((ValueFrom != null) ? ValueFrom.GetHashCode() : 0);
		type = (type * 397) ^ ((ValueTo != null) ? ValueTo.GetHashCode() : 0);
		type = (type * 397) ^ MatchCase.GetHashCode();
		type = (type * 397) ^ MatchWholeWord.GetHashCode();
		return (type * 397) ^ (int)Priority;
	}

	public StiDataActionRule()
	{
	}

	public StiDataActionRule(string key, string path)
		: this(key, path, StiDataActionType.Percentage, 0, -1, StiDataActionPriority.AfterGroupingData, null, null, matchCase: false, matchWholeWord: false, null)
	{
	}

	public StiDataActionRule(string key, string path, int startIndex, int rowsCount, StiDataActionPriority priority)
		: this(key, path, StiDataActionType.Limit, startIndex, rowsCount, priority, null, null, matchCase: false, matchWholeWord: false, null)
	{
	}

	public StiDataActionRule(string key, string path, string valueFrom, string valueTo, bool matchCase, bool matchWholeWord)
		: this(key, path, StiDataActionType.Replace, 0, -1, StiDataActionPriority.BeforeTransformation, valueFrom, valueTo, matchCase, matchWholeWord, null)
	{
	}

	public StiDataActionRule(string key, string path, string initialValue)
		: this(key, path, StiDataActionType.RunningTotal, 0, -1, StiDataActionPriority.AfterGroupingData, null, null, matchCase: false, matchWholeWord: false, initialValue)
	{
	}

	public StiDataActionRule(string key, string path, StiDataActionType type, int startIndex, int rowsCount, StiDataActionPriority priority, string valueFrom, string valueTo, bool matchCase, bool matchWholeWord, string initialValue)
	{
		Key = key;
		Path = path;
		Type = type;
		StartIndex = startIndex;
		RowsCount = rowsCount;
		ValueFrom = valueFrom;
		ValueTo = valueTo;
		MatchCase = matchCase;
		MatchWholeWord = matchWholeWord;
		InitialValue = initialValue;
		Priority = priority;
	}
}
