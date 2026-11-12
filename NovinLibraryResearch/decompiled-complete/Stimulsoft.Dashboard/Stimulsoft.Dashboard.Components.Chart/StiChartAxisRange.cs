using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Dashboard.Components.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiChartAxisRange : IStiJsonReportObject, ICloneable
{
	[DefaultValue(0.0)]
	[StiSerializable]
	[Description("Gets or sets minimum value of axis range.")]
	[StiOrder(100)]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	public double Minimum { get; set; }

	[DefaultValue(0.0)]
	[StiSerializable]
	[Description("Gets or sets maximum value of axis range.")]
	[StiOrder(110)]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	public double Maximum { get; set; }

	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that minimum and maximum values will be calculated automatically.")]
	[StiOrder(90)]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	public bool Auto { get; set; } = true;


	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (Minimum == 0.0 && Maximum == 0.0)
			{
				return Auto;
			}
			return false;
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyDouble("Minimum", Minimum, 0.0);
		jObject.AddPropertyDouble("Maximum", Maximum, 0.0);
		jObject.AddPropertyBool("Auto", Auto, defaultValue: true);
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Minimum":
				Minimum = item.DeserializeDouble();
				break;
			case "Maximum":
				Maximum = item.DeserializeDouble();
				break;
			case "Auto":
				Auto = item.DeserializeBool();
				break;
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public StiChartAxisRange()
	{
	}

	[StiUniversalConstructor("Range")]
	public StiChartAxisRange(bool auto, double minimum, double maximum)
	{
		Auto = auto;
		Minimum = minimum;
		Maximum = maximum;
	}
}
