using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Gauge;

namespace Stimulsoft.Dashboard.Components.Gauge;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiGaugeLabels : IStiJsonReportObject, IStiGaugeLabels
{
	public enum Order
	{
		Visible = 1,
		Placement,
		RotationMode
	}

	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of labels.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(1)]
	public virtual bool Visible { get; set; } = true;


	[StiSerializable]
	[DefaultValue(StiLabelPlacement.Inside)]
	[Description("Gets or sets the placement of the tick relative to the scale.")]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(2)]
	public virtual StiLabelPlacement Placement { get; set; } = StiLabelPlacement.Inside;


	[StiSerializable]
	[Browsable(false)]
	[DefaultValue(StiLabelRotationMode.Automatic)]
	[Description("Gets or sets the rotation mode of labels.")]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(3)]
	public virtual StiLabelRotationMode RotationMode { get; set; } = StiLabelRotationMode.Automatic;


	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			if (Visible && Placement == StiLabelPlacement.Inside)
			{
				return RotationMode == StiLabelRotationMode.Automatic;
			}
			return false;
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyBool("Visible", Visible, defaultValue: true);
		jObject.AddPropertyEnum("Placement", Placement, StiLabelPlacement.Inside);
		jObject.AddPropertyEnum("RotationMode", RotationMode, StiLabelRotationMode.Automatic);
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Visible":
				Visible = item.DeserializeBool();
				break;
			case "Placement":
				Placement = item.DeserializeEnum<StiLabelPlacement>();
				break;
			case "RotationMode":
				RotationMode = item.DeserializeEnum<StiLabelRotationMode>();
				break;
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public StiGaugeLabels()
	{
	}

	[StiUniversalConstructor("Labels")]
	public StiGaugeLabels(bool visible, StiLabelPlacement placement, StiLabelRotationMode rotationMode)
	{
		Visible = visible;
		Placement = placement;
		RotationMode = rotationMode;
	}
}
