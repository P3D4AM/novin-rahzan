using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Chart.Design;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiChartConditionConverter))]
public class StiChartCondition : StiChartFilter, IStiChartCondition, IStiChartFilter, ICloneable
{
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets a series color.")]
	public Color Color { get; set; } = Color.White;


	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets rotation angle of the marker.")]
	public virtual float MarkerAngle { get; set; }

	[DefaultValue(StiMarkerType.Circle)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets type of the marker.")]
	public StiMarkerType MarkerType { get; set; } = StiMarkerType.Circle;


	[Browsable(false)]
	internal StiChartConditionsCollection Conditions { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyColor(val, "Color", Color, Color.White);
		JsonExtensions.AddPropertyEnum(val, "MarkerType", (Enum)MarkerType, (Enum)StiMarkerType.Circle);
		JsonExtensions.AddPropertyFloat(val, "MarkerAngle", MarkerAngle, 0f);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Color":
				Color = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "MarkerType":
				MarkerType = StiJsonReportObjectHelper.DeserializeEnum<StiMarkerType>(item);
				break;
			case "MarkerAngle":
				MarkerAngle = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			}
		}
	}

	public StiChartCondition()
	{
	}

	public StiChartCondition(Color color, StiFilterItem item, StiFilterDataType dataType, StiFilterCondition condition, string value)
		: base(item, dataType, condition, value)
	{
		Color = color;
	}

	public StiChartCondition(Color color, StiFilterItem item, StiFilterDataType dataType, StiFilterCondition condition, string value, StiMarkerType markerType, float markerAngle)
		: base(item, dataType, condition, value)
	{
		Color = color;
		MarkerType = markerType;
		MarkerAngle = markerAngle;
	}
}
