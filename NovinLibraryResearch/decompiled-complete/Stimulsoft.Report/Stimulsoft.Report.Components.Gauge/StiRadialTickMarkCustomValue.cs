using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public sealed class StiRadialTickMarkCustomValue : StiCustomValueBase
{
	internal bool useBrush;

	internal bool useBorderBrush;

	internal bool useBorderWidth;

	private StiBrush brush = (StiBrush)new StiEmptyBrush();

	private StiBrush borderBrush = (StiBrush)new StiEmptyBrush();

	private float? borderWidth;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiRadialTickMarkCustomValue;

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
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets an additional rotation angle.")]
	public float? OffsetAngle { get; set; }

	[StiSerializable]
	[DefaultValue(null)]
	[Browsable(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the skin of the component rendering.")]
	public StiGaugeElementSkin Skin { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a brush to fill a component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBrush Brush
	{
		get
		{
			return brush;
		}
		set
		{
			brush = value;
			useBrush = true;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the border of the component.")]
	public StiBrush BorderBrush
	{
		get
		{
			return borderBrush;
		}
		set
		{
			borderBrush = value;
			useBorderBrush = true;
		}
	}

	[DefaultValue(null)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the border thickness of the component.")]
	public float? BorderWidth
	{
		get
		{
			return borderWidth;
		}
		set
		{
			borderWidth = value;
			useBorderWidth = true;
		}
	}

	public override string LocalizedName => "RadialTickMarkCustom";

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyFloatNullable(val, "RelativeHeight", RelativeHeight, (float?)null);
		JsonExtensions.AddPropertyFloatNullable(val, "RelativeWidth", RelativeWidth, (float?)null);
		JsonExtensions.AddPropertyFloatNullable(val, "OffsetAngle", OffsetAngle, (float?)null);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "RelativeHeight":
				RelativeHeight = StiJsonReportObjectHelper.DeserializeFloatNullable(item);
				break;
			case "RelativeWidth":
				RelativeWidth = StiJsonReportObjectHelper.DeserializeFloatNullable(item);
				break;
			case "OffsetAngle":
				OffsetAngle = StiJsonReportObjectHelper.DeserializeFloatNullable(item);
				break;
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
			propertiesHelper.NullableOffsetAngle(),
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
		value = new StiPropertyObject[3]
		{
			propertiesHelper.Brush(),
			propertiesHelper.BorderBrush(),
			propertiesHelper.NullableBorderWidth()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		return stiPropertyCollection;
	}

	public override object Clone()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		StiRadialTickMarkCustomValue stiRadialTickMarkCustomValue = (StiRadialTickMarkCustomValue)base.Clone();
		stiRadialTickMarkCustomValue.brush = (StiBrush)brush.Clone();
		stiRadialTickMarkCustomValue.borderBrush = (StiBrush)borderBrush.Clone();
		return stiRadialTickMarkCustomValue;
	}

	public override string ToString()
	{
		return $"Value={base.Value}";
	}

	public override StiCustomValueBase CreateNew()
	{
		return new StiRadialTickMarkCustomValue();
	}

	public StiRadialTickMarkCustomValue()
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_000b: Expected O, but got Unknown
	//IL_000c: Unknown result type (might be due to invalid IL or missing references)
	//IL_0016: Expected O, but got Unknown


	public StiRadialTickMarkCustomValue(float value)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		base.Value = value;
	}

	public StiRadialTickMarkCustomValue(float value, float? offset, float? relativeWidth, float? relativeHeight, float? offsetAngle, StiPlacement? placement, StiGaugeElementSkin skin)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		base.Value = value;
		base.Offset = offset;
		RelativeWidth = relativeWidth;
		RelativeHeight = relativeHeight;
		OffsetAngle = offsetAngle;
		base.Placement = placement;
		Skin = skin;
	}

	public StiRadialTickMarkCustomValue(float value, float? offset, float? relativeWidth, float? relativeHeight, float? offsetAngle, StiPlacement? placement, StiBrush brush, StiBrush borderBrush, float? borderWidth, StiGaugeElementSkin skin)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		base.Value = value;
		base.Offset = offset;
		RelativeWidth = relativeWidth;
		RelativeHeight = relativeHeight;
		OffsetAngle = offsetAngle;
		base.Placement = placement;
		Brush = brush;
		BorderBrush = borderBrush;
		BorderWidth = borderWidth;
		Skin = skin;
	}
}
