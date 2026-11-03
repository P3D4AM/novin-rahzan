using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiAxisTicks : IStiAxisTicks, ICloneable, IStiJsonReportObject, IStiDefault
{
	private float lengthUnderLabels = 5f;

	private float length = 5f;

	private float minorLength = 2f;

	private int minorCount = 4;

	private int step;

	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (LengthUnderLabels == 5f && Length == 5f && MinorLength == 2f && MinorCount == 4 && Step == 0 && !MinorVisible)
			{
				return Visible;
			}
			return false;
		}
	}

	[DefaultValue(5f)]
	[StiSerializable]
	[Description("Gets or sets length of one major tick under labels.")]
	public float LengthUnderLabels
	{
		get
		{
			return lengthUnderLabels;
		}
		set
		{
			if (value > 0f)
			{
				lengthUnderLabels = value;
			}
		}
	}

	[DefaultValue(5f)]
	[StiSerializable]
	[Description("Gets or sets length of one major tick.")]
	public float Length
	{
		get
		{
			return length;
		}
		set
		{
			if (value > 0f)
			{
				length = value;
			}
		}
	}

	[DefaultValue(2f)]
	[StiSerializable]
	[Description("Gets or sets length of one minor tick.")]
	public float MinorLength
	{
		get
		{
			return minorLength;
		}
		set
		{
			if (value > 0f)
			{
				minorLength = value;
			}
		}
	}

	[DefaultValue(4)]
	[StiSerializable]
	[Description("Gets or sets count of minor ticks between two major ticks.")]
	public int MinorCount
	{
		get
		{
			return minorCount;
		}
		set
		{
			if (value >= 0)
			{
				minorCount = value;
			}
		}
	}

	[DefaultValue(0)]
	[StiSerializable]
	[Description("Gets or sets value which indicates on which steps major ticks will be displayed.")]
	public int Step
	{
		get
		{
			return step;
		}
		set
		{
			if (value >= 0)
			{
				step = value;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of minor ticks.")]
	public virtual bool MinorVisible { get; set; }

	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visility of major ticks.")]
	public virtual bool Visible { get; set; } = true;


	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyFloat(val, "LengthUnderLabels", lengthUnderLabels, 5f);
		JsonExtensions.AddPropertyFloat(val, "Length", length, 5f);
		JsonExtensions.AddPropertyFloat(val, "MinorLength", minorLength, 2f);
		JsonExtensions.AddPropertyInt(val, "MinorCount", minorCount, 4);
		JsonExtensions.AddPropertyInt(val, "Step", step, 0);
		JsonExtensions.AddPropertyBool(val, "MinorVisible", MinorVisible, false, false);
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, true, false);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "LengthUnderLabels":
				lengthUnderLabels = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Length":
				length = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "MinorLength":
				minorLength = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "MinorCount":
				minorCount = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Step":
				step = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "MinorVisible":
				MinorVisible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Visible":
				Visible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public StiAxisTicks()
	{
	}

	public StiAxisTicks(bool visible, float length, bool minorVisible, float minorLength, int minorCount, int step)
		: this(visible, length, minorVisible, minorLength, minorCount, step, length)
	{
	}

	[StiUniversalConstructor("Ticks")]
	public StiAxisTicks(bool visible, float length, bool minorVisible, float minorLength, int minorCount, int step, float lengthUnderLabels)
	{
		Visible = visible;
		this.length = length;
		MinorVisible = minorVisible;
		this.minorLength = minorLength;
		this.minorCount = minorCount;
		this.step = step;
		this.lengthUnderLabels = lengthUnderLabels;
	}
}
