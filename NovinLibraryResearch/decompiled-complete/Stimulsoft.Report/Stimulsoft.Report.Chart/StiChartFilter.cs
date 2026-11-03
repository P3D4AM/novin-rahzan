using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Chart.Design;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiChartFilterConverter))]
[StiSerializable]
public class StiChartFilter : IStiChartFilter, ICloneable, IStiJsonReportObject
{
	[Browsable(false)]
	public int Index
	{
		get
		{
			if (Filters == null)
			{
				return -1;
			}
			return Filters.IndexOf(this);
		}
	}

	[DefaultValue(StiFilterCondition.EqualTo)]
	public StiFilterCondition Condition { get; set; }

	[DefaultValue(StiFilterDataType.String)]
	public StiFilterDataType DataType { get; set; }

	[DefaultValue(StiFilterItem.Argument)]
	public StiFilterItem Item { get; set; }

	[DefaultValue("")]
	public string Value { get; set; }

	internal StiChartFiltersCollection Filters { get; set; }

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyEnum(val, "Condition", (Enum)Condition, (Enum)StiFilterCondition.EqualTo);
		JsonExtensions.AddPropertyEnum(val, "DataType", (Enum)DataType, (Enum)StiFilterDataType.String);
		JsonExtensions.AddPropertyEnum(val, "Item", (Enum)Item, (Enum)StiFilterItem.Argument);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Value", Value);
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Condition":
				Condition = StiJsonReportObjectHelper.DeserializeEnum<StiFilterCondition>(item);
				break;
			case "DataType":
				DataType = StiJsonReportObjectHelper.DeserializeEnum<StiFilterDataType>(item);
				break;
			case "Item":
				Item = StiJsonReportObjectHelper.DeserializeEnum<StiFilterItem>(item);
				break;
			case "Value":
				Value = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public override string ToString()
	{
		string text = "";
		switch (Condition)
		{
		case StiFilterCondition.EqualTo:
			text = "=";
			break;
		case StiFilterCondition.NotEqualTo:
			text = "<>";
			break;
		case StiFilterCondition.GreaterThan:
			text = ">";
			break;
		case StiFilterCondition.GreaterThanOrEqualTo:
			text = ">=";
			break;
		case StiFilterCondition.LessThan:
			text = "<";
			break;
		case StiFilterCondition.LessThanOrEqualTo:
			text = "=<";
			break;
		}
		string text2 = StiLocalization.Get("PropertyMain", "Argument");
		switch (Item)
		{
		case StiFilterItem.Expression:
			text2 = StiLocalization.Get("PropertyMain", "Expression");
			break;
		case StiFilterItem.ValueClose:
			text2 = StiLocalization.Get("PropertyMain", "ValueClose");
			break;
		case StiFilterItem.ValueHigh:
			text2 = StiLocalization.Get("PropertyMain", "ValueHigh");
			break;
		case StiFilterItem.ValueLow:
			text2 = StiLocalization.Get("PropertyMain", "ValueLow");
			break;
		case StiFilterItem.ValueOpen:
			text2 = StiLocalization.Get("PropertyMain", "ValueOpen");
			break;
		case StiFilterItem.Value:
			text2 = StiLocalization.Get("PropertyMain", "Value");
			break;
		case StiFilterItem.ValueEnd:
			text2 = StiLocalization.Get("PropertyMain", "Value");
			break;
		}
		return text2 + " " + text + " " + Value;
	}

	public StiChartFilter()
		: this(StiFilterItem.Argument, StiFilterDataType.String, StiFilterCondition.EqualTo, string.Empty)
	{
	}

	public StiChartFilter(StiFilterItem item, StiFilterDataType dataType, StiFilterCondition condition, string value)
	{
		Item = item;
		DataType = dataType;
		Condition = condition;
		Value = value;
	}
}
