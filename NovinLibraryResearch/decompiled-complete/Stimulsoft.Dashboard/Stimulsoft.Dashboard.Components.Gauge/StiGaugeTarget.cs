using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Gauge;

namespace Stimulsoft.Dashboard.Components.Gauge;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiGaugeTarget : IStiJsonReportObject, IStiGaugeTarget
{
	public enum Order
	{
		ShowLabel = 1,
		Placement
	}

	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of labels.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(1)]
	public bool ShowLabel { get; set; } = true;


	[StiSerializable]
	[DefaultValue(StiPlacement.Inside)]
	[Description("Gets or sets the placement of the tick relative to the scale.")]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(2)]
	public StiPlacement Placement { get; set; } = StiPlacement.Inside;


	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			if (ShowLabel)
			{
				return Placement == StiPlacement.Inside;
			}
			return false;
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyBool("ShowLabel", ShowLabel, defaultValue: true);
		jObject.AddPropertyEnum("Placement", Placement, StiPlacement.Inside);
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "ShowLabel"))
			{
				if (name == "Placement")
				{
					Placement = item.DeserializeEnum<StiPlacement>();
				}
			}
			else
			{
				ShowLabel = item.DeserializeBool();
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public StiGaugeTarget()
	{
	}

	[StiUniversalConstructor("Target")]
	public StiGaugeTarget(bool showLabel, StiPlacement placement)
	{
		ShowLabel = showLabel;
		Placement = placement;
	}
}
