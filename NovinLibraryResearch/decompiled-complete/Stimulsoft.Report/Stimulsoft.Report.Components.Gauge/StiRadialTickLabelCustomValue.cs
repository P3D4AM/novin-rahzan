using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public sealed class StiRadialTickLabelCustomValue : StiCustomValueBase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiRadialTickLabelCustomValue;

	[DefaultValue(null)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets text corresponds to this tick label.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string Text { get; set; }

	[DefaultValue(null)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets an additional rotation angle.")]
	public float? OffsetAngle { get; set; }

	[DefaultValue(null)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the rotation mode of labels.")]
	public StiLabelRotationMode? LabelRotationMode { get; set; }

	public override string LocalizedName => "RadialTickLabelCustom";

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyString(val, "Text", Text);
		JsonExtensions.AddPropertyFloatNullable(val, "OffsetAngle", OffsetAngle, (float?)null);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "Text"))
			{
				if (name == "OffsetAngle")
				{
					OffsetAngle = StiJsonReportObjectHelper.DeserializeFloatNullable(item);
				}
			}
			else
			{
				Text = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[3]
		{
			propertiesHelper.NullableOffset(),
			propertiesHelper.TextStr(),
			propertiesHelper.ValueF()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Value, value);
		value = new StiPropertyObject[1] { propertiesHelper.NullablePlacement() };
		stiPropertyCollection.Add(StiPropertyCategories.Tick, value);
		value = new StiPropertyObject[2]
		{
			propertiesHelper.NullableLabelRotationMode(),
			propertiesHelper.NullableOffsetAngle()
		};
		stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, value);
		return stiPropertyCollection;
	}

	public override string ToString()
	{
		return $"Value={base.Value}, Text={Text}";
	}

	public override StiCustomValueBase CreateNew()
	{
		return new StiRadialTickLabelCustomValue();
	}

	public StiRadialTickLabelCustomValue()
	{
	}

	public StiRadialTickLabelCustomValue(float value, string text)
	{
		base.Value = value;
		Text = text;
	}

	public StiRadialTickLabelCustomValue(float value, string text, float offset)
	{
		base.Value = value;
		Text = text;
		base.Offset = offset;
	}

	public StiRadialTickLabelCustomValue(float value, string text, float? offset, float? offsetAngle, StiLabelRotationMode? labelRotationMode, StiPlacement? placement)
	{
		base.Value = value;
		Text = text;
		base.Offset = offset;
		OffsetAngle = offsetAngle;
		LabelRotationMode = labelRotationMode;
		base.Placement = placement;
	}
}
