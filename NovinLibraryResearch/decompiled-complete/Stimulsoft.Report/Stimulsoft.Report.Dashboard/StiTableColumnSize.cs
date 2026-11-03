using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Dashboard;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiTableColumnSize : ICloneable, IStiJsonReportObject
{
	private int width;

	private int minWidth = 30;

	private int maxWidth = 300;

	[StiSerializable]
	[DefaultValue(0)]
	[Description("Gets or sets the specified width for the table column. The width will be calculated automatically by the table element if a value equal to zero.")]
	[RefreshProperties(RefreshProperties.All)]
	public int Width
	{
		get
		{
			return width;
		}
		set
		{
			if (width != value)
			{
				if (value < 0)
				{
					value = 0;
				}
				width = value;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(30)]
	[Description("Gets or sets a minimum width of the table column.")]
	[RefreshProperties(RefreshProperties.All)]
	public int MinWidth
	{
		get
		{
			return minWidth;
		}
		set
		{
			if (minWidth != value)
			{
				if (value < 0)
				{
					value = 0;
				}
				minWidth = value;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(300)]
	[Description("Gets or sets a maximum width of the table column.")]
	[RefreshProperties(RefreshProperties.All)]
	public int MaxWidth
	{
		get
		{
			return maxWidth;
		}
		set
		{
			if (maxWidth != value)
			{
				if (value <= 0)
				{
					value = 300;
				}
				maxWidth = value;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets word wrap.")]
	public bool WordWrap { get; set; }

	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			if (Width == 0 && MinWidth == 30 && MaxWidth == 300)
			{
				return !WordWrap;
			}
			return false;
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyInt(val, "Width", Width, 0);
		JsonExtensions.AddPropertyInt(val, "MinWidth", MinWidth, 30);
		JsonExtensions.AddPropertyInt(val, "MaxWidth", MaxWidth, 300);
		JsonExtensions.AddPropertyBool(val, "WordWrap", WordWrap, false, false);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Width":
				Width = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "MinWidth":
				MinWidth = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "MaxWidth":
				MaxWidth = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "WordWrap":
				WordWrap = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public void CheckRules()
	{
		if (MaxWidth < MinWidth)
		{
			MinWidth = MaxWidth;
		}
		if (Width < MinWidth && Width != 0)
		{
			Width = MinWidth;
		}
		if (Width > MaxWidth)
		{
			Width = MaxWidth;
		}
	}

	public int GetUniqueCode()
	{
		int num = Width;
		num = (num * 297) ^ MinWidth;
		num = (num * 297) ^ MaxWidth;
		return (num * 297) ^ WordWrap.GetHashCode();
	}

	public StiTableColumnSize()
	{
	}

	[StiUniversalConstructor("Size")]
	public StiTableColumnSize(int width, int minWidth, int maxWidth, bool wordWrap)
	{
		Width = width;
		MinWidth = minWidth;
		MaxWidth = maxWidth;
		WordWrap = wordWrap;
	}
}
