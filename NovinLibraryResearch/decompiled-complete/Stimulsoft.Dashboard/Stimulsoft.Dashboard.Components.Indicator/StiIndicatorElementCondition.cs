using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Image.Design;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Components.Indicator;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiIndicatorElementCondition : IStiJsonReportObject, ICloneable, IStiIndicatorElementCondition
{
	[StiSerializable]
	public Color IconColor { get; set; } = Color.FromArgb(68, 114, 196);


	[DefaultValue(StiFontIcons.Rating4)]
	[StiSerializable]
	public StiFontIcons Icon { get; set; } = StiFontIcons.Rating4;


	[StiSerializable]
	public Color TargetIconColor { get; set; } = Color.FromArgb(68, 114, 196);


	[DefaultValue(StiFontIcons.Rating4)]
	[StiSerializable]
	public StiFontIcons TargetIcon { get; set; } = StiFontIcons.Rating4;


	[DefaultValue(Stimulsoft.Report.Components.StiFilterCondition.EqualTo)]
	[StiSerializable]
	public Stimulsoft.Report.Components.StiFilterCondition Condition { get; set; }

	[DefaultValue(StiIndicatorFieldCondition.Value)]
	[StiSerializable]
	public StiIndicatorFieldCondition Field { get; set; }

	[DefaultValue("")]
	[StiSerializable]
	public string Value { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	[TypeConverter(typeof(StiImageBytesConverter))]
	public byte[] CustomIcon { get; set; }

	[StiSerializable]
	public StiIndicatorConditionPermissions Permissions { get; set; } = StiIndicatorConditionPermissions.All;


	[StiSerializable]
	public Color TextColor { get; set; } = Color.Black;


	[StiSerializable]
	public Color BackColor { get; set; } = Color.Transparent;


	[StiSerializable]
	public Font Font { get; set; } = new Font("Arial", 8f);


	[StiSerializable]
	[DefaultValue(StiIconAlignment.Right)]
	public StiIconAlignment IconAlignment { get; set; } = StiIconAlignment.Right;


	[StiSerializable]
	[DefaultValue(StiIconAlignment.Right)]
	public StiIconAlignment TargetIconAlignment { get; set; } = StiIconAlignment.Right;


	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (IconColor == Color.FromArgb(68, 114, 196) && Icon == StiFontIcons.Rating4 && TargetIconColor == Color.FromArgb(68, 114, 196) && TargetIcon == StiFontIcons.Rating4 && IconAlignment == StiIconAlignment.Right && TargetIconAlignment == StiIconAlignment.Right && Condition == Stimulsoft.Report.Components.StiFilterCondition.EqualTo && Field == StiIndicatorFieldCondition.Value)
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
		jObject.AddPropertyEnum("Field", Field, StiIndicatorFieldCondition.Value);
		jObject.AddPropertyStringNullOrEmpty("Value", Value);
		jObject.AddPropertyColor("IconColor", IconColor, Color.FromArgb(68, 114, 196));
		jObject.AddPropertyEnum("Icon", Icon, StiFontIcons.Rating4);
		jObject.AddPropertyColor("TargetIconColor", TargetIconColor, Color.FromArgb(68, 114, 196));
		jObject.AddPropertyEnum("TargetIcon", TargetIcon, StiFontIcons.Rating4);
		if (CustomIcon != null)
		{
			jObject.AddPropertyByteArray("CustomIcon", CustomIcon);
		}
		jObject.AddPropertyEnum("Permissions", Permissions, StiIndicatorConditionPermissions.All);
		jObject.AddPropertyFontArial8("Font", Font);
		jObject.AddPropertyColor("TextColor", TextColor, Color.Black);
		jObject.AddPropertyColor("BackColor", BackColor, Color.Transparent);
		jObject.AddPropertyEnum("IconAlignment", IconAlignment, StiIconAlignment.Right);
		jObject.AddPropertyEnum("TargetIconAlignment", TargetIconAlignment, StiIconAlignment.Right);
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
				Field = item.DeserializeEnum<StiIndicatorFieldCondition>();
				break;
			case "Value":
				Value = item.DeserializeString();
				break;
			case "Icon":
				Icon = item.DeserializeEnum<StiFontIcons>();
				break;
			case "IconColor":
				IconColor = item.DeserializeColor();
				break;
			case "TargetIcon":
				TargetIcon = item.DeserializeEnum<StiFontIcons>();
				break;
			case "TargetIconColor":
				TargetIconColor = item.DeserializeColor();
				break;
			case "CustomIcon":
				CustomIcon = item.DeserializeByteArray();
				break;
			case "IconAlignment":
				IconAlignment = item.DeserializeEnum<StiIconAlignment>();
				break;
			case "TargetIconAlignment":
				TargetIconAlignment = item.DeserializeEnum<StiIconAlignment>();
				break;
			case "Permissions":
				Permissions = item.DeserializeEnum<StiIndicatorConditionPermissions>();
				break;
			case "TextColor":
				TextColor = item.DeserializeColor();
				break;
			case "BackColor":
				BackColor = item.DeserializeColor();
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

	public static StiIndicatorElementCondition LoadFromJson(JObject json)
	{
		StiIndicatorElementCondition stiIndicatorElementCondition = new StiIndicatorElementCondition();
		stiIndicatorElementCondition.LoadFromJsonObject(json);
		return stiIndicatorElementCondition;
	}

	public StiIndicatorElementCondition()
	{
	}//IL_0064: Unknown result type (might be due to invalid IL or missing references)
	//IL_006e: Expected O, but got Unknown


	[StiUniversalConstructor("Condition")]
	public StiIndicatorElementCondition(StiIndicatorFieldCondition field, Stimulsoft.Report.Components.StiFilterCondition condition, string value, StiFontIcons icon, Color iconColor, StiFontIcons targetIcon, Color targetIconColor, byte[] customIcon, StiIconAlignment iconAlignment, StiIconAlignment targetIconAlignment, StiIndicatorConditionPermissions permissions, Font font, Color textColor, Color backColor)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		Field = field;
		Condition = condition;
		Value = value;
		TargetIcon = targetIcon;
		TargetIconColor = targetIconColor;
		Icon = icon;
		IconColor = iconColor;
		CustomIcon = customIcon;
		IconAlignment = iconAlignment;
		TargetIconAlignment = targetIconAlignment;
		Permissions = permissions;
		Font = font;
		TextColor = textColor;
		BackColor = backColor;
	}
}
