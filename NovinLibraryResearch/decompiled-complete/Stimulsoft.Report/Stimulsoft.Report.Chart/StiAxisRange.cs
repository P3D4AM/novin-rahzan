using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiAxisRange : IStiAxisRange, ICloneable, IStiJsonReportObject, IStiDefault
{
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

	[DefaultValue(0.0)]
	[StiSerializable]
	[Description("Gets or sets minimum value of axis range.")]
	public double Minimum { get; set; }

	[DefaultValue(0.0)]
	[StiSerializable]
	[Description("Gets or sets maximum value of axis range.")]
	public double Maximum { get; set; }

	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that minimum and maximum values will be calculated automatically.")]
	public bool Auto { get; set; } = true;


	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyDouble(val, "Minimum", Minimum, 0.0);
		JsonExtensions.AddPropertyDouble(val, "Maximum", Maximum, 0.0);
		JsonExtensions.AddPropertyBool(val, "Auto", Auto, true, false);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Minimum":
				Minimum = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "Maximum":
				Maximum = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "Auto":
				Auto = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public StiAxisRange()
	{
	}

	[StiUniversalConstructor("Range")]
	public StiAxisRange(bool auto, double minimum, double maximum)
	{
		Auto = auto;
		Minimum = minimum;
		Maximum = maximum;
	}
}
