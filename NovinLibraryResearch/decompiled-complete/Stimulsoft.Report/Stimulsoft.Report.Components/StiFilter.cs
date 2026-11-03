using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Design;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiFilterConverter))]
[StiSerializable]
public class StiFilter : ICloneable, IStiJsonReportObject
{
	[DefaultValue(StiFilterCondition.EqualTo)]
	[StiSerializable]
	public virtual StiFilterCondition Condition { get; set; }

	[DefaultValue(StiFilterDataType.String)]
	[StiSerializable]
	public virtual StiFilterDataType DataType { get; set; }

	[DefaultValue("")]
	[StiSerializable]
	public virtual string Column { get; set; } = string.Empty;


	[DefaultValue(StiFilterItem.Value)]
	[StiSerializable]
	public virtual StiFilterItem Item { get; set; } = StiFilterItem.Value;


	[DefaultValue("")]
	[StiSerializable]
	public virtual string Value1 { get; set; } = string.Empty;


	[DefaultValue("")]
	[StiSerializable]
	public virtual string Value2 { get; set; } = string.Empty;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets the filter expression.")]
	public virtual StiExpression Expression { get; set; } = new StiFilterExpression();


	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyEnum(val, "Condition", (Enum)Condition, (Enum)StiFilterCondition.EqualTo);
		JsonExtensions.AddPropertyEnum(val, "DataType", (Enum)DataType, (Enum)StiFilterDataType.String);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Column", Column);
		JsonExtensions.AddPropertyEnum(val, "Item", (Enum)Item, (Enum)StiFilterItem.Value);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Value1", Value1);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Value2", Value2);
		JsonExtensions.AddPropertyJObject(val, "Expression", Expression.SaveToJsonObject(mode));
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Expected O, but got Unknown
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
			case "Column":
				Column = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Item":
				Item = StiJsonReportObjectHelper.DeserializeEnum<StiFilterItem>(item);
				break;
			case "Value1":
				Value1 = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Value2":
				Value2 = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Expression":
				Expression.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public virtual object Clone()
	{
		return MemberwiseClone();
	}

	private static string ConvertData(DateTime date)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			return date.ToShortDateString();
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public StiFilter()
	{
	}

	public StiFilter(string expression)
	{
		if (!(this is StiMultiCondition))
		{
			Expression.Value = expression;
			Item = StiFilterItem.Expression;
		}
	}

	public StiFilter(string column, StiFilterCondition condition, DateTime date1)
		: this(column, condition, ConvertData(date1), string.Empty, StiFilterDataType.DateTime)
	{
	}

	public StiFilter(string column, StiFilterCondition condition, DateTime date1, DateTime date2)
		: this(column, condition, ConvertData(date1), ConvertData(date2), StiFilterDataType.DateTime)
	{
	}

	public StiFilter(string column, StiFilterCondition condition, string value, StiFilterDataType dataType)
		: this(column, condition, value, string.Empty, dataType)
	{
	}

	public StiFilter(string column, StiFilterCondition condition, string value1, string value2, StiFilterDataType dataType)
		: this(StiFilterItem.Value, column, condition, value1, value2, dataType, string.Empty)
	{
	}

	public StiFilter(StiFilterItem item, string column, StiFilterCondition condition, string value1, string value2, StiFilterDataType dataType, string expression)
	{
		Item = item;
		Column = column;
		Condition = condition;
		Value1 = value1;
		Value2 = value2;
		DataType = dataType;
		Expression.Value = expression;
	}
}
