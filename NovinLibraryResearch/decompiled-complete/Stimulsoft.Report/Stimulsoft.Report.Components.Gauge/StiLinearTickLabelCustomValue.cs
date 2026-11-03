using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public sealed class StiLinearTickLabelCustomValue : StiCustomValueBase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiLinearTickLabelCustomValue;

	[StiSerializable]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets text corresponds to this tick label.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string Text { get; set; }

	public override string LocalizedName => "LinearTickLabelCustom";

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyString(val, "Text", Text);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "Text")
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
		return stiPropertyCollection;
	}

	public override string ToString()
	{
		return $"Value={base.Value}, Text={Text}";
	}

	public override StiCustomValueBase CreateNew()
	{
		return new StiLinearTickLabelCustomValue();
	}

	public StiLinearTickLabelCustomValue()
	{
	}

	public StiLinearTickLabelCustomValue(float value, string text)
	{
		base.Value = value;
		Text = text;
	}

	public StiLinearTickLabelCustomValue(float value, string text, float? offset, StiPlacement? placement)
	{
		base.Value = value;
		Text = text;
		base.Offset = offset;
		base.Placement = placement;
	}
}
