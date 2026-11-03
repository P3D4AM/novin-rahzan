using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiAxisDateTimeStep : IStiAxisDateTimeStep, ICloneable, IStiJsonReportObject, IStiDefault
{
	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (Step == StiTimeDateStep.None && NumberOfValues == 1)
			{
				return !Interpolation;
			}
			return false;
		}
	}

	[DefaultValue(StiTimeDateStep.None)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a value that indicates with what the time step values will be shown.")]
	public StiTimeDateStep Step { get; set; }

	[DefaultValue(1)]
	[StiSerializable]
	[Description("Gets or sets number of values in step.")]
	public int NumberOfValues { get; set; } = 1;


	[DefaultValue(false)]
	[StiSerializable]
	[Description("Gets or sets A value indicates that the values \u200b\u200bwill be interpolated.")]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool Interpolation { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyEnum(val, "Step", (Enum)Step, (Enum)StiTimeDateStep.None);
		JsonExtensions.AddPropertyInt(val, "NumberOfValues", NumberOfValues, 1);
		JsonExtensions.AddPropertyBool(val, "Interpolation", Interpolation, false, false);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Step":
				Step = StiJsonReportObjectHelper.DeserializeEnum<StiTimeDateStep>(item);
				break;
			case "NumberOfValues":
				NumberOfValues = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Interpolation":
				Interpolation = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public StiAxisDateTimeStep()
	{
	}

	public StiAxisDateTimeStep(StiTimeDateStep step, int numberOfValues)
	{
		Step = step;
		NumberOfValues = numberOfValues;
	}

	[StiUniversalConstructor("DateTimeStep")]
	public StiAxisDateTimeStep(StiTimeDateStep step, int numberOfValues, bool interpolation)
	{
		Step = step;
		NumberOfValues = numberOfValues;
		Interpolation = interpolation;
	}
}
