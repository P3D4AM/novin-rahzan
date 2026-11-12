using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiChartElementCondition : IStiJsonReportObject, ICloneable, IStiChartElementCondition
{
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color Color { get; set; } = Color.White;


	[DefaultValue(0f)]
	[StiSerializable]
	public virtual float MarkerAngle { get; set; }

	[DefaultValue(StiMarkerType.Circle)]
	[StiSerializable]
	public StiMarkerType MarkerType { get; set; } = StiMarkerType.Circle;


	[DefaultValue(Stimulsoft.Report.Components.StiFilterCondition.EqualTo)]
	[StiSerializable]
	public Stimulsoft.Report.Components.StiFilterCondition Condition { get; set; }

	[DefaultValue(Stimulsoft.Report.Components.StiFilterDataType.Numeric)]
	[StiSerializable]
	public Stimulsoft.Report.Components.StiFilterDataType DataType { get; set; } = Stimulsoft.Report.Components.StiFilterDataType.Numeric;


	[DefaultValue(StiChartConditionalField.Value)]
	[StiSerializable]
	public StiChartConditionalField Field { get; set; }

	[DefaultValue("")]
	[StiSerializable]
	public string Value { get; set; }

	[DefaultValue(false)]
	[StiSerializable]
	public bool IsExpression { get; set; }

	[DefaultValue("")]
	[StiSerializable]
	public string KeyValueMeter { get; set; }

	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (Color == Color.White && MarkerType == StiMarkerType.Circle && Condition == Stimulsoft.Report.Components.StiFilterCondition.EqualTo && DataType == Stimulsoft.Report.Components.StiFilterDataType.Numeric && Field == StiChartConditionalField.Value && Value == "" && !IsExpression)
			{
				return KeyValueMeter == "";
			}
			return false;
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyIdent("Ident", GetType().Name);
		jObject.AddPropertyStringNullOrEmpty("KeyValueMeter", KeyValueMeter);
		jObject.AddPropertyEnum("Condition", Condition, Stimulsoft.Report.Components.StiFilterCondition.EqualTo);
		jObject.AddPropertyEnum("DataType", DataType, Stimulsoft.Report.Components.StiFilterDataType.Numeric);
		jObject.AddPropertyEnum("Field", Field, StiChartConditionalField.Value);
		jObject.AddPropertyStringNullOrEmpty("Value", Value);
		jObject.AddPropertyBool("IsExpression", IsExpression);
		jObject.AddPropertyColor("Color", Color, Color.White);
		jObject.AddPropertyEnum("MarkerType", MarkerType, StiMarkerType.Circle);
		jObject.AddPropertyFloat("MarkerAngle", MarkerAngle);
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Condition":
				Condition = item.DeserializeEnum<Stimulsoft.Report.Components.StiFilterCondition>();
				break;
			case "DataType":
				DataType = item.DeserializeEnum<Stimulsoft.Report.Components.StiFilterDataType>();
				break;
			case "Field":
				Field = item.DeserializeEnum<StiChartConditionalField>();
				break;
			case "KeyValueMeter":
				KeyValueMeter = item.DeserializeString();
				break;
			case "Value":
				Value = item.DeserializeString();
				break;
			case "IsExpression":
				IsExpression = item.DeserializeBool();
				break;
			case "Color":
				Color = item.DeserializeColor();
				break;
			case "MarkerType":
				MarkerType = item.DeserializeEnum<StiMarkerType>();
				break;
			case "MarkerAngle":
				MarkerAngle = item.DeserializeFloat();
				break;
			}
		}
	}

	public virtual object Clone()
	{
		return MemberwiseClone();
	}

	public static StiChartElementCondition LoadFromJson(JObject json)
	{
		StiChartElementCondition stiChartElementCondition = new StiChartElementCondition();
		stiChartElementCondition.LoadFromJsonObject(json);
		return stiChartElementCondition;
	}

	public StiChartElementCondition()
	{
	}

	public StiChartElementCondition(string keyValueMeter, Color color, Stimulsoft.Report.Components.StiFilterDataType dataType, Stimulsoft.Report.Components.StiFilterCondition condition, string value, StiMarkerType markerType, float markerAngle)
		: this(keyValueMeter, color, dataType, condition, value, markerType, markerAngle, StiChartConditionalField.Value)
	{
	}

	public StiChartElementCondition(string keyValueMeter, Color color, Stimulsoft.Report.Components.StiFilterDataType dataType, Stimulsoft.Report.Components.StiFilterCondition condition, string value, StiMarkerType markerType, float markerAngle, StiChartConditionalField field)
		: this(keyValueMeter, color, dataType, condition, value, markerType, markerAngle, field, isExpression: false)
	{
	}

	[StiUniversalConstructor("Condition")]
	public StiChartElementCondition(string keyValueMeter, Color color, Stimulsoft.Report.Components.StiFilterDataType dataType, Stimulsoft.Report.Components.StiFilterCondition condition, string value, StiMarkerType markerType, float markerAngle, StiChartConditionalField field, bool isExpression)
	{
		KeyValueMeter = keyValueMeter;
		DataType = dataType;
		Condition = condition;
		Value = value;
		Color = color;
		MarkerType = markerType;
		MarkerAngle = markerAngle;
		Field = field;
		IsExpression = isExpression;
	}
}
