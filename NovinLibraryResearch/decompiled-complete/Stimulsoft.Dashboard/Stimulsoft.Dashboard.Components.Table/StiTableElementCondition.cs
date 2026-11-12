using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Table;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiTableElementCondition : IStiJsonReportObject, ICloneable, IStiTableElementCondition
{
	[StiNonSerialized]
	[Browsable(false)]
	[StiSerializable(StiSerializationVisibility.List)]
	[Obsolete("'KeyValueMeters' property of the table element condition is obsolete! Please use 'KeyDataFieldMeters' and 'KeyDestinationMeters' properties instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string[] KeyValueMeters
	{
		get
		{
			return KeyDataFieldMeters;
		}
		set
		{
			KeyDataFieldMeters = value;
			KeyDestinationMeters = value;
		}
	}

	[StiSerializable(StiSerializationVisibility.List)]
	public string[] KeyDataFieldMeters { get; set; } = new string[0];


	[StiSerializable(StiSerializationVisibility.List)]
	public string[] KeyDestinationMeters { get; set; } = new string[0];


	[DefaultValue(Stimulsoft.Report.Components.StiFilterDataType.Numeric)]
	[StiSerializable]
	public Stimulsoft.Report.Components.StiFilterDataType DataType { get; set; } = Stimulsoft.Report.Components.StiFilterDataType.Numeric;


	[DefaultValue(Stimulsoft.Report.Components.StiFilterCondition.EqualTo)]
	[StiSerializable]
	public Stimulsoft.Report.Components.StiFilterCondition Condition { get; set; }

	[DefaultValue("")]
	[StiSerializable]
	public string Value { get; set; }

	[DefaultValue(false)]
	[StiSerializable]
	public bool IsExpression { get; set; }

	[StiSerializable]
	public StiTableConditionPermissions Permissions { get; set; } = StiTableConditionPermissions.All;


	[StiSerializable]
	public Font Font { get; set; } = new Font("Arial", 8f);


	[StiSerializable]
	public Color ForeColor { get; set; } = Color.Black;


	[StiSerializable]
	public Color BackColor { get; set; } = Color.Transparent;


	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (ForeColor == Color.Black && BackColor == Color.Transparent && Condition == Stimulsoft.Report.Components.StiFilterCondition.EqualTo && DataType == Stimulsoft.Report.Components.StiFilterDataType.Numeric && Value == "" && !IsExpression && KeyDataFieldMeters == null)
			{
				return KeyDestinationMeters == null;
			}
			return false;
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyIdent("Ident", GetType().Name);
		jObject.AddPropertyStringArray("KeyDataFieldMeters", KeyDataFieldMeters);
		jObject.AddPropertyStringArray("KeyDestinationMeters", KeyDestinationMeters);
		jObject.AddPropertyEnum("DataType", DataType, StiFilterDataType.Numeric);
		jObject.AddPropertyEnum("Condition", Condition, StiFilterCondition.EqualTo);
		jObject.AddPropertyStringNullOrEmpty("Value", Value);
		jObject.AddPropertyBool("IsExpression", IsExpression);
		jObject.AddPropertyEnum("Permissions", Permissions, StiTableConditionPermissions.All);
		jObject.AddPropertyFontArial8("Font", Font);
		jObject.AddPropertyColor("ForeColor", ForeColor, Color.Black);
		jObject.AddPropertyColor("BackColor", BackColor, Color.Transparent);
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "KeyValueMeters":
				KeyValueMeters = item.DeserializeStringArray();
				break;
			case "KeyDataFieldMeters":
				KeyDataFieldMeters = item.DeserializeStringArray();
				break;
			case "KeyDestinationMeters":
				KeyDestinationMeters = item.DeserializeStringArray();
				break;
			case "DataType":
				DataType = item.DeserializeEnum<Stimulsoft.Report.Components.StiFilterDataType>();
				break;
			case "Condition":
				Condition = item.DeserializeEnum<Stimulsoft.Report.Components.StiFilterCondition>();
				break;
			case "Value":
				Value = item.DeserializeString();
				break;
			case "Permissions":
				Permissions = item.DeserializeEnum<StiTableConditionPermissions>();
				break;
			case "IsExpression":
				IsExpression = item.DeserializeBool();
				break;
			case "Font":
				Font = item.DeserializeFont(Font);
				break;
			case "ForeColor":
				ForeColor = item.DeserializeColor();
				break;
			case "BackColor":
				BackColor = item.DeserializeColor();
				break;
			}
		}
	}

	public virtual object Clone()
	{
		return MemberwiseClone();
	}

	public static StiTableElementCondition LoadFromJson(JObject json)
	{
		StiTableElementCondition stiTableElementCondition = new StiTableElementCondition();
		stiTableElementCondition.LoadFromJsonObject(json);
		return stiTableElementCondition;
	}

	public StiTableElementCondition()
	{
	}//IL_0035: Unknown result type (might be due to invalid IL or missing references)
	//IL_003f: Expected O, but got Unknown


	public StiTableElementCondition(string[] keyValueMeters, Stimulsoft.Report.Components.StiFilterDataType dataType, Stimulsoft.Report.Components.StiFilterCondition condition, string value, StiTableConditionPermissions permissions, Font font, Color foreColor, Color backColor)
		: this(keyValueMeters, keyValueMeters, dataType, condition, value, permissions, font, foreColor, backColor, isExpression: false)
	{
	}

	[StiUniversalConstructor("Condition")]
	public StiTableElementCondition(string[] keyDataFieldMeters, string[] keyDestinationMeters, Stimulsoft.Report.Components.StiFilterDataType dataType, Stimulsoft.Report.Components.StiFilterCondition condition, string value, StiTableConditionPermissions permissions, Font font, Color foreColor, Color backColor, bool isExpression)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		KeyDataFieldMeters = keyDataFieldMeters;
		KeyDestinationMeters = keyDestinationMeters;
		DataType = dataType;
		Condition = condition;
		Value = value;
		Permissions = permissions;
		Font = font;
		ForeColor = foreColor;
		BackColor = backColor;
		IsExpression = isExpression;
	}
}
