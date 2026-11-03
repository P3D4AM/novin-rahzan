using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Chart;

public class StiPie3dOptions : Sti3dOptions
{
	private float height = 25f;

	private float distance = 4f;

	[Browsable(false)]
	public override bool IsDefault
	{
		get
		{
			if (base.IsDefault && Lighting == StiPie3dLightingStyle.Gradient && Height == 25f)
			{
				return Distance == 4f;
			}
			return false;
		}
	}

	[StiSerializable]
	[DefaultValue(StiPie3dLightingStyle.Gradient)]
	public StiPie3dLightingStyle Lighting { get; set; } = StiPie3dLightingStyle.Gradient;


	[StiSerializable]
	[DefaultValue(25f)]
	public float Height
	{
		get
		{
			return height;
		}
		set
		{
			if (value >= 0f)
			{
				height = value;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(4f)]
	[Description("Gets or sets distance between the center of series and the center of each segment.")]
	public virtual float Distance
	{
		get
		{
			return distance;
		}
		set
		{
			if (value >= 0f)
			{
				distance = value;
			}
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyFloat(val, "Height", Height, 25f);
		JsonExtensions.AddPropertyFloat(val, "Distance", Distance, 4f);
		JsonExtensions.AddPropertyEnum(val, "Lighting", (Enum)Lighting);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Height":
				Height = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Distance":
				Distance = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Lighting":
				Lighting = StiJsonReportObjectHelper.DeserializeEnum<StiPie3dLightingStyle>(item);
				break;
			}
		}
	}
}
