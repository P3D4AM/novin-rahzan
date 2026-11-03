using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[RefreshProperties(RefreshProperties.All)]
[TypeConverter(typeof(StiConditionConverter))]
public class StiMultiCondition : StiCondition, IStiFilter
{
	[DefaultValue(StiFilterMode.And)]
	[StiSerializable]
	[Browsable(false)]
	public StiFilterMode FilterMode { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiFiltersCollection Filters { get; set; } = new StiFiltersCollection();


	[Browsable(false)]
	[StiNonSerialized]
	public StiFilterEventHandler FilterMethodHandler
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public bool FilterOn
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	public override StiFilterCondition Condition
	{
		get
		{
			throw new Exception("Please, use Filters collection!");
		}
		set
		{
			throw new Exception("Please, use Filters collection!");
		}
	}

	[StiNonSerialized]
	public override StiFilterDataType DataType
	{
		get
		{
			throw new Exception("Please, use Filters collection!");
		}
		set
		{
			throw new Exception("Please, use Filters collection!");
		}
	}

	[StiNonSerialized]
	public override string Column
	{
		get
		{
			throw new Exception("Please, use Filters collection!");
		}
		set
		{
			throw new Exception("Please, use Filters collection!");
		}
	}

	[StiNonSerialized]
	public override StiFilterItem Item
	{
		get
		{
			throw new Exception("Please, use Filters collection!");
		}
		set
		{
			throw new Exception("Please, use Filters collection!");
		}
	}

	[StiNonSerialized]
	public override string Value1
	{
		get
		{
			throw new Exception("Please, use Filters collection!");
		}
		set
		{
			throw new Exception("Please, use Filters collection!");
		}
	}

	[StiNonSerialized]
	public override string Value2
	{
		get
		{
			throw new Exception("Please, use Filters collection!");
		}
		set
		{
			throw new Exception("Please, use Filters collection!");
		}
	}

	[StiNonSerialized]
	public override StiExpression Expression
	{
		get
		{
			throw new Exception("Please, use Filters collection!");
		}
		set
		{
			throw new Exception("Please, use Filters collection!");
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyBool(val, "Enabled", base.Enabled, true, false);
		JsonExtensions.AddPropertyColor(val, "TextColor", base.TextColor, Color.Red);
		JsonExtensions.AddPropertyColor(val, "BackColor", base.BackColor, Color.Transparent);
		JsonExtensions.AddPropertyFontArial8(val, "Font", base.Font);
		JsonExtensions.AddPropertyBool(val, "CanAssignExpression", base.CanAssignExpression, false, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Style", base.Style);
		JsonExtensions.AddPropertyEnum(val, "BorderSides", (Enum)base.BorderSides, (Enum)StiConditionBorderSides.NotAssigned);
		JsonExtensions.AddPropertyEnum(val, "Permissions", (Enum)base.Permissions, (Enum)StiConditionPermissions.All);
		JsonExtensions.AddPropertyEnum(val, "FilterMode", (Enum)FilterMode, (Enum)StiFilterMode.And);
		JsonExtensions.AddPropertyJObject(val, "Filters", Filters.SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "FilterMode"))
			{
				if (name == "Filters")
				{
					Filters.LoadFromJsonObject((JObject)item.Value);
				}
			}
			else
			{
				FilterMode = StiJsonReportObjectHelper.DeserializeEnum<StiFilterMode>(item);
			}
		}
	}

	public override object Clone()
	{
		return (StiCondition)base.Clone();
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is StiMultiCondition stiMultiCondition))
		{
			return false;
		}
		if (base.BackColor.Equals((object?)stiMultiCondition.BackColor) && base.TextColor.Equals((object?)stiMultiCondition.TextColor) && base.Enabled.Equals(stiMultiCondition.Enabled) && ((object)base.Font).Equals((object?)stiMultiCondition.Font) && FilterMode.Equals(stiMultiCondition.FilterMode) && Filters.Equals(stiMultiCondition.Filters) && base.CanAssignExpression.Equals(stiMultiCondition.CanAssignExpression))
		{
			return base.AssignExpression.Equals(stiMultiCondition.AssignExpression);
		}
		return false;
	}

	public StiMultiCondition()
	{
	}

	public StiMultiCondition(Color textColor, Color backColor, Font font, bool enabled, StiFilterMode filterMode)
		: this(textColor, backColor, font, enabled, filterMode, canAssignExpression: false, string.Empty)
	{
	}

	public StiMultiCondition(Color textColor, Color backColor, Font font, bool enabled, StiFilterMode filterMode, bool canAssignExpression, string assignExpression)
		: this(textColor, backColor, font, enabled, filterMode, null, canAssignExpression, assignExpression)
	{
	}

	public StiMultiCondition(Color textColor, Color backColor, Font font, bool enabled, StiFilterMode filterMode, StiFilter[] filters)
		: this(textColor, backColor, font, enabled, filterMode, filters, canAssignExpression: false, string.Empty)
	{
	}

	public StiMultiCondition(Color textColor, Color backColor, Font font, bool enabled, StiFilterMode filterMode, StiFilter[] filters, string style, StiConditionBorderSides borderSides)
		: this(textColor, backColor, font, enabled, filterMode, filters, canAssignExpression: false, string.Empty)
	{
		base.Style = style;
		base.BorderSides = borderSides;
	}

	public StiMultiCondition(Color textColor, Color backColor, Font font, bool enabled, StiFilterMode filterMode, StiFilter[] filters, bool canAssignExpression, string assignExpression)
		: this(textColor, backColor, font, enabled, filterMode, filters, canAssignExpression, assignExpression, string.Empty, StiConditionBorderSides.NotAssigned)
	{
	}

	public StiMultiCondition(Color textColor, Color backColor, Font font, bool enabled, StiFilterMode filterMode, StiFilter[] filters, bool canAssignExpression, string assignExpression, string style, StiConditionBorderSides borderSides)
		: base(string.Empty, textColor, backColor, font, enabled, canAssignExpression, assignExpression)
	{
		base.Style = style;
		base.BorderSides = borderSides;
		FilterMode = filterMode;
		if (filters != null)
		{
			foreach (StiFilter filter in filters)
			{
				Filters.Add(filter);
			}
		}
	}
}
