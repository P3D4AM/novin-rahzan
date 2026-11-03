using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public sealed class StiLinearTickMarkCustomValue : StiCustomValueBase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiLinearTickMarkCustomValue;

	[StiSerializable]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the width ratio relative to the height of the basic component.")]
	public float? RelativeWidth { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the height factor relative to the height of the basic component.")]
	public float? RelativeHeight { get; set; }

	[StiSerializable]
	[Browsable(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(null)]
	[Description("Gets or sets the skin of the component rendering.")]
	public StiGaugeElementSkin Skin { get; set; }

	public override string LocalizedName => "LinearTickMarkCustom";

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyFloatNullable(val, "RelativeHeight", RelativeHeight, (float?)null);
		JsonExtensions.AddPropertyFloatNullable(val, "RelativeWidth", RelativeWidth, (float?)null);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "RelativeHeight"))
			{
				if (name == "RelativeWidth")
				{
					RelativeWidth = StiJsonReportObjectHelper.DeserializeFloatNullable(item);
				}
			}
			else
			{
				RelativeHeight = StiJsonReportObjectHelper.DeserializeFloatNullable(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[2]
		{
			propertiesHelper.NullableOffset(),
			propertiesHelper.ValueF()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Value, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.NullablePlacement(),
			propertiesHelper.NullableRelativeHeight(),
			propertiesHelper.NullableRelativeWidth()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Tick, value);
		return stiPropertyCollection;
	}

	public override string ToString()
	{
		return $"Value={base.Value}";
	}

	public override StiCustomValueBase CreateNew()
	{
		return new StiLinearTickMarkCustomValue();
	}

	public StiLinearTickMarkCustomValue()
	{
	}

	public StiLinearTickMarkCustomValue(float value)
	{
		base.Value = value;
	}

	public StiLinearTickMarkCustomValue(float value, float? offset, float? relativeWidth, float? relativeHeight, StiPlacement? placement, StiGaugeElementSkin skin)
	{
		base.Value = value;
		base.Offset = offset;
		RelativeWidth = relativeWidth;
		RelativeHeight = relativeHeight;
		base.Placement = placement;
		Skin = skin;
	}
}
