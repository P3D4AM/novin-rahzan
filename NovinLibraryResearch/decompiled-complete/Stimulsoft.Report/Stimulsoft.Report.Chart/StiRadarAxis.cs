using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiRadarAxis : IStiRadarAxis, ICloneable, IStiJsonReportObject
{
	internal bool jsonLoadFromJsonObjectArea;

	private bool allowApplyStyle = true;

	[Browsable(false)]
	public StiRadarAxisCoreXF Core { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that chart style will be used.2")]
	[DefaultValue(true)]
	[Browsable(false)]
	public virtual bool AllowApplyStyle
	{
		get
		{
			return allowApplyStyle;
		}
		set
		{
			if (allowApplyStyle != value)
			{
				allowApplyStyle = value;
				if (value && Area != null && Area.Chart != null)
				{
					Core.ApplyStyle(Area.Chart.Style);
				}
			}
		}
	}

	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of axis.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public virtual bool Visible { get; set; } = true;


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	[Description("Gets or sets axis range settings.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public virtual IStiAxisRange Range { get; set; } = new StiAxisRange();


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public IStiRadarArea Area { get; set; }

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", allowApplyStyle, true, false);
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, true, false);
		if (Area != null)
		{
			JsonExtensions.AddPropertyBool(val, "Area", true, false, false);
		}
		if (Range != null)
		{
			JsonExtensions.AddPropertyJObject(val, "Range", ((IStiJsonReportObject)Range).SaveToJsonObject(mode));
		}
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "AllowApplyStyle":
				allowApplyStyle = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Visible":
				Visible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Area":
				jsonLoadFromJsonObjectArea = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Range":
				((IStiJsonReportObject)Range).LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public virtual object Clone()
	{
		IStiRadarAxis stiRadarAxis = MemberwiseClone() as IStiRadarAxis;
		if (Core != null)
		{
			stiRadarAxis.Core = Core.Clone() as StiRadarAxisCoreXF;
			stiRadarAxis.Core.Axis = stiRadarAxis;
		}
		return stiRadarAxis;
	}

	public StiRadarAxis()
	{
	}

	public StiRadarAxis(bool visible, bool allowApplyStyle)
	{
		Visible = visible;
		this.allowApplyStyle = allowApplyStyle;
	}

	[StiUniversalConstructor("Axis")]
	public StiRadarAxis(IStiAxisRange range, bool visible, bool allowApplyStyle)
	{
		Range = range;
		Visible = visible;
		this.allowApplyStyle = allowApplyStyle;
	}
}
