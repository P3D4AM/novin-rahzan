using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Design;

namespace Stimulsoft.Report.Components;

[RefreshProperties(RefreshProperties.All)]
[TypeConverter(typeof(StiConditionConverter))]
public class StiCondition : StiBaseCondition, IStiGetFonts, IStiFont
{
	private StiConditionBorderSides borderSides = StiConditionBorderSides.NotAssigned;

	public ContentAlignment IconAlignment = (ContentAlignment)64;

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(170)]
	[StiSerializable]
	[DefaultValue(true)]
	public bool Enabled { get; set; } = true;


	[StiSerializable]
	[Description("Gets or sets a color to draw text.")]
	public Color TextColor { get; set; } = Color.Red;


	[StiSerializable]
	[Description("Gets or sets a color to draw background of text.")]
	public Color BackColor { get; set; } = Color.Transparent;


	[StiSerializable]
	[Description("Gets or sets font of text.")]
	public Font Font { get; set; } = new Font("Arial", 8f);


	[StiSerializable]
	[DefaultValue(false)]
	public bool CanAssignExpression { get; set; }

	[StiSerializable]
	[DefaultValue(false)]
	public bool BreakIfTrue { get; set; }

	[StiSerializable]
	[DefaultValue("")]
	public string AssignExpression { get; set; } = string.Empty;


	[StiSerializable]
	[DefaultValue("")]
	public string Style { get; set; } = string.Empty;


	[StiSerializable]
	[DefaultValue(StiConditionBorderSides.NotAssigned)]
	public StiConditionBorderSides BorderSides
	{
		get
		{
			return borderSides;
		}
		set
		{
			borderSides = value;
			if (value == StiConditionBorderSides.NotAssigned)
			{
				borderSides = StiConditionBorderSides.None;
				if ((Permissions & StiConditionPermissions.Borders) > StiConditionPermissions.None)
				{
					Permissions -= 256;
				}
			}
		}
	}

	[StiSerializable]
	[DefaultValue(StiConditionPermissions.All)]
	public StiConditionPermissions Permissions { get; set; } = StiConditionPermissions.All;


	public byte[] Icon { get; set; }

	public Size? IconSize { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "Enabled", Enabled, true, false);
		JsonExtensions.AddPropertyColor(val, "TextColor", TextColor, Color.Red);
		JsonExtensions.AddPropertyColor(val, "BackColor", BackColor, Color.Transparent);
		JsonExtensions.AddPropertyFontArial8(val, "Font", Font);
		JsonExtensions.AddPropertyBool(val, "CanAssignExpression", CanAssignExpression, false, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "AssignExpression", AssignExpression);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Style", Style);
		JsonExtensions.AddPropertyEnum(val, "BorderSides", (Enum)BorderSides, (Enum)StiConditionBorderSides.NotAssigned);
		JsonExtensions.AddPropertyEnum(val, "Permissions", (Enum)Permissions, (Enum)StiConditionPermissions.All);
		JsonExtensions.AddPropertyBool(val, "BreakIfTrue", BreakIfTrue, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Enabled":
				Enabled = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "TextColor":
				TextColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BackColor":
				BackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Font":
				Font = StiJsonReportObjectHelper.DeserializeFont(item, Font);
				break;
			case "CanAssignExpression":
				CanAssignExpression = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AssignExpression":
				AssignExpression = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Style":
				Style = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "BorderSides":
				borderSides = StiJsonReportObjectHelper.DeserializeEnum<StiConditionBorderSides>(item);
				break;
			case "Permissions":
				Permissions = StiJsonReportObjectHelper.DeserializeEnum<StiConditionPermissions>(item);
				break;
			case "BreakIfTrue":
				BreakIfTrue = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public override object Clone()
	{
		return (StiCondition)base.Clone();
	}

	public List<StiFont> GetFonts()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return new List<StiFont>
		{
			new StiFont(Font)
		};
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is StiCondition stiCondition))
		{
			return false;
		}
		if (BackColor.Equals((object?)stiCondition.BackColor) && TextColor.Equals((object?)stiCondition.TextColor) && Column.Equals(stiCondition.Column) && Condition.Equals(stiCondition.Condition) && DataType.Equals(stiCondition.DataType) && Enabled.Equals(stiCondition.Enabled) && ((object)Font).Equals((object?)stiCondition.Font) && Item.Equals(stiCondition.Item) && Value1.Equals(stiCondition.Value1) && Value2.Equals(stiCondition.Value2) && Expression.Value.Trim().Equals(stiCondition.Expression.Value.Trim()) && CanAssignExpression.Equals(stiCondition.CanAssignExpression))
		{
			return AssignExpression.Equals(stiCondition.AssignExpression);
		}
		return false;
	}

	public StiCondition()
	{
	}//IL_0028: Unknown result type (might be due to invalid IL or missing references)
	//IL_0032: Expected O, but got Unknown
	//IL_005e: Unknown result type (might be due to invalid IL or missing references)


	public StiCondition(string expression, Color textColor, Color backColor, Font font, bool enabled)
		: base(expression)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		TextColor = textColor;
		BackColor = backColor;
		Font = font;
		Enabled = enabled;
	}

	public StiCondition(string expression, Color textColor, Color backColor, Font font, bool enabled, bool canAssignExpression, string assignExpression)
		: this(expression, textColor, backColor, font, enabled, canAssignExpression, assignExpression, string.Empty, StiConditionBorderSides.NotAssigned)
	{
		if ((Permissions & StiConditionPermissions.Borders) > StiConditionPermissions.None)
		{
			Permissions -= 256;
		}
	}

	public StiCondition(string expression, Color textColor, Color backColor, Font font, bool enabled, bool canAssignExpression, string assignExpression, string style, StiConditionBorderSides borderSides)
		: this(expression, textColor, backColor, font, enabled)
	{
		CanAssignExpression = canAssignExpression;
		AssignExpression = assignExpression;
		Style = style;
		this.borderSides = borderSides;
	}

	public StiCondition(string expression, Color textColor, Color backColor, Font font, bool enabled, bool canAssignExpression, string assignExpression, string style, StiConditionBorderSides borderSides, StiConditionPermissions permissions)
		: this(expression, textColor, backColor, font, enabled)
	{
		CanAssignExpression = canAssignExpression;
		AssignExpression = assignExpression;
		Style = style;
		this.borderSides = borderSides;
		Permissions = permissions;
	}

	public StiCondition(string expression, Color textColor, Color backColor, Font font, bool enabled, bool canAssignExpression, string assignExpression, string style, StiConditionBorderSides borderSides, StiConditionPermissions permissions, bool breakIfTrue)
		: this(expression, textColor, backColor, font, enabled)
	{
		CanAssignExpression = canAssignExpression;
		AssignExpression = assignExpression;
		Style = style;
		this.borderSides = borderSides;
		Permissions = permissions;
		BreakIfTrue = breakIfTrue;
	}

	public StiCondition(string column, StiFilterCondition condition, DateTime date1, Color textColor, Color backColor, Font font, bool enabled)
		: base(column, condition, date1)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		TextColor = textColor;
		BackColor = backColor;
		Font = font;
		Enabled = enabled;
	}

	public StiCondition(string column, StiFilterCondition condition, DateTime date1, DateTime date2, Color textColor, Color backColor, Font font, bool enabled)
		: base(column, condition, date1, date2)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		TextColor = textColor;
		BackColor = backColor;
		Font = font;
		Enabled = enabled;
	}

	public StiCondition(string column, StiFilterCondition condition, string value, StiFilterDataType dataType, Color textColor, Color backColor, Font font, bool enabled)
		: base(column, condition, value, string.Empty, dataType)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		TextColor = textColor;
		BackColor = backColor;
		Font = font;
		Enabled = enabled;
	}

	public StiCondition(string column, StiFilterCondition condition, string value1, string value2, StiFilterDataType dataType, Color textColor, Color backColor, Font font, bool enabled)
		: this(column, condition, value1, value2, dataType, textColor, backColor, font, enabled, canAssignExpression: false, string.Empty)
	{
	}

	public StiCondition(string column, StiFilterCondition condition, string value1, string value2, StiFilterDataType dataType, Color textColor, Color backColor, Font font, bool enabled, bool canAssignExpression, string assignExpression)
		: this(column, condition, value1, value2, dataType, textColor, backColor, font, enabled, canAssignExpression, assignExpression, string.Empty, StiConditionBorderSides.NotAssigned)
	{
	}

	public StiCondition(string column, StiFilterCondition condition, string value1, string value2, StiFilterDataType dataType, Color textColor, Color backColor, Font font, bool enabled, bool canAssignExpression, string assignExpression, string style, StiConditionBorderSides borderSides)
		: base(column, condition, value1, value2, dataType)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		TextColor = textColor;
		BackColor = backColor;
		Font = font;
		Enabled = enabled;
		CanAssignExpression = canAssignExpression;
		AssignExpression = assignExpression;
		Style = style;
		this.borderSides = borderSides;
	}

	public StiCondition(string column, StiFilterCondition condition, string value1, string value2, StiFilterDataType dataType, Color textColor, Color backColor, Font font, bool enabled, bool canAssignExpression, string assignExpression, string style, StiConditionBorderSides borderSides, StiConditionPermissions permissions)
		: base(column, condition, value1, value2, dataType)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		TextColor = textColor;
		BackColor = backColor;
		Font = font;
		Enabled = enabled;
		CanAssignExpression = canAssignExpression;
		AssignExpression = assignExpression;
		Style = style;
		this.borderSides = borderSides;
		Permissions = permissions;
	}

	public StiCondition(string column, StiFilterCondition condition, string value1, string value2, StiFilterDataType dataType, Color textColor, Color backColor, Font font, bool enabled, bool canAssignExpression, string assignExpression, string style, StiConditionBorderSides borderSides, StiConditionPermissions permissions, bool breakIfTrue)
		: base(column, condition, value1, value2, dataType)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		TextColor = textColor;
		BackColor = backColor;
		Font = font;
		Enabled = enabled;
		CanAssignExpression = canAssignExpression;
		AssignExpression = assignExpression;
		Style = style;
		this.borderSides = borderSides;
		Permissions = permissions;
		BreakIfTrue = breakIfTrue;
	}

	public StiCondition(StiFilterItem item, string column, StiFilterCondition condition, string value1, string value2, StiFilterDataType dataType, string expression, Color textColor, Color backColor, Font font, bool enabled)
		: this(item, column, condition, value1, value2, dataType, expression, textColor, backColor, font, enabled, canAssignExpression: false, string.Empty)
	{
	}

	public StiCondition(StiFilterItem item, string column, StiFilterCondition condition, string value1, string value2, StiFilterDataType dataType, string expression, Color textColor, Color backColor, Font font, bool enabled, bool canAssignExpression, string assignExpression)
		: this(item, column, condition, value1, value2, dataType, expression, textColor, backColor, font, enabled, canAssignExpression, assignExpression, string.Empty, StiConditionBorderSides.NotAssigned)
	{
	}

	public StiCondition(StiFilterItem item, string column, StiFilterCondition condition, string value1, string value2, StiFilterDataType dataType, string expression, Color textColor, Color backColor, Font font, bool enabled, bool canAssignExpression, string assignExpression, string style, StiConditionBorderSides borderSides)
		: this(item, column, condition, value1, value2, dataType, expression, textColor, backColor, font, enabled, canAssignExpression, assignExpression, style, borderSides, StiConditionPermissions.All)
	{
	}

	public StiCondition(StiFilterItem item, string column, StiFilterCondition condition, string value1, string value2, StiFilterDataType dataType, string expression, Color textColor, Color backColor, Font font, bool enabled, bool canAssignExpression, string assignExpression, string style, StiConditionBorderSides borderSides, StiConditionPermissions permissions)
		: base(item, column, condition, value1, value2, dataType, expression)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		TextColor = textColor;
		BackColor = backColor;
		Font = font;
		Enabled = enabled;
		CanAssignExpression = canAssignExpression;
		AssignExpression = assignExpression;
		Style = style;
		this.borderSides = borderSides;
		Permissions = permissions;
	}

	public StiCondition(StiFilterItem item, string column, StiFilterCondition condition, string value1, string value2, StiFilterDataType dataType, string expression, Color textColor, Color backColor, Font font, bool enabled, bool canAssignExpression, string assignExpression, string style, StiConditionBorderSides borderSides, StiConditionPermissions permissions, bool breakIfTrue)
		: base(item, column, condition, value1, value2, dataType, expression)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		TextColor = textColor;
		BackColor = backColor;
		Font = font;
		Enabled = enabled;
		CanAssignExpression = canAssignExpression;
		AssignExpression = assignExpression;
		Style = style;
		this.borderSides = borderSides;
		Permissions = permissions;
		BreakIfTrue = breakIfTrue;
	}
}
