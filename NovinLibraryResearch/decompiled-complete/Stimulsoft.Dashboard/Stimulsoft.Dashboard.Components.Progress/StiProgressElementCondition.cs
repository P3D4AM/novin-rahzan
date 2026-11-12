using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Progress;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiProgressElementCondition : IStiJsonReportObject, ICloneable, IStiProgressElementCondition
{
	[DefaultValue(Stimulsoft.Report.Components.StiFilterCondition.EqualTo)]
	[StiSerializable]
	public Stimulsoft.Report.Components.StiFilterCondition Condition { get; set; }

	[DefaultValue(StiIndicatorFieldCondition.Value)]
	[StiSerializable]
	public StiProgressFieldCondition Field { get; set; }

	[DefaultValue("")]
	[StiSerializable]
	public string Value { get; set; }

	[StiSerializable]
	public StiProgressConditionPermissions Permissions { get; set; } = StiProgressConditionPermissions.All;


	[StiSerializable]
	public Color TextColor { get; set; } = Color.Black;


	[StiSerializable]
	public Color Color { get; set; } = Color.Transparent;


	[StiSerializable]
	public Color TrackColor { get; set; } = Color.Transparent;


	[StiSerializable]
	public Font Font { get; set; } = new Font("Arial", 8f);


	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (Color == Color.FromArgb(68, 114, 196) && Condition == Stimulsoft.Report.Components.StiFilterCondition.EqualTo)
			{
				return Value == "";
			}
			return false;
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyIdent("Ident", GetType().Name);
		jObject.AddPropertyEnum("Condition", Condition, StiFilterCondition.EqualTo);
		jObject.AddPropertyEnum("Field", Field, StiProgressFieldCondition.Value);
		jObject.AddPropertyStringNullOrEmpty("Value", Value);
		jObject.AddPropertyColor("Color", Color, Color.Transparent);
		jObject.AddPropertyEnum("Permissions", Permissions, StiProgressConditionPermissions.All);
		jObject.AddPropertyFontArial8("Font", Font);
		jObject.AddPropertyColor("TextColor", TextColor, Color.Black);
		jObject.AddPropertyColor("TrackColor", TrackColor, Color.Transparent);
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
			case "Field":
				Field = item.DeserializeEnum<StiProgressFieldCondition>();
				break;
			case "Value":
				Value = item.DeserializeString();
				break;
			case "Color":
				Color = item.DeserializeColor();
				break;
			case "Permissions":
				Permissions = item.DeserializeEnum<StiProgressConditionPermissions>();
				break;
			case "TextColor":
				TextColor = item.DeserializeColor();
				break;
			case "TrackColor":
				TrackColor = item.DeserializeColor();
				break;
			case "Font":
				Font = item.DeserializeFont(Font);
				break;
			}
		}
	}

	public virtual object Clone()
	{
		return MemberwiseClone();
	}

	public static StiProgressElementCondition LoadFromJson(JObject json)
	{
		StiProgressElementCondition stiProgressElementCondition = new StiProgressElementCondition();
		stiProgressElementCondition.LoadFromJsonObject(json);
		return stiProgressElementCondition;
	}

	public StiProgressElementCondition()
	{
	}//IL_0037: Unknown result type (might be due to invalid IL or missing references)
	//IL_0041: Expected O, but got Unknown


	[StiUniversalConstructor("Condition")]
	public StiProgressElementCondition(StiProgressFieldCondition field, Stimulsoft.Report.Components.StiFilterCondition condition, string value, StiProgressConditionPermissions permissions, Font font, Color textColor, Color color, Color trackColor)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		Field = field;
		Condition = condition;
		Value = value;
		Permissions = permissions;
		Font = font;
		TextColor = textColor;
		Color = color;
		TrackColor = trackColor;
	}
}
