using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Collections;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public class StiStateIndicator : StiIndicatorBase, IStiGaugeMarker
{
	private StiStateIndicatorFilter lastFilter;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiStateIndicator;

	[StiSerializable]
	[DefaultValue("{0:F0}")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the format string for the ShowValue property.")]
	public string Format { get; set; } = "{0:F0}";


	[StiSerializable]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a value indicating whether to display a current value of the indicator.")]
	public bool ShowValue { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("The brush, which is used to display text.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBrush TextBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.DimGray);


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets font of component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public Font Font { get; set; } = new Font("Atial", 7f);


	public override StiGaugeElemenType ElementType => StiGaugeElemenType.All;

	public override string LocalizeName => "StateIndicator";

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiFilterCollection Filters { get; set; } = new StiFilterCollection();


	[StiSerializable]
	[DefaultValue(0f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets X coordinate of the indicator relative to the scale dimension.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float Left { get; set; }

	[StiSerializable]
	[DefaultValue(0f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets Y coordinate of the indicator relative to the scale dimension.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float Top { get; set; }

	[StiSerializable]
	[DefaultValue(0.05f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets relative width of the indicator.  It is given as part of the scale dimension.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float RelativeWidth { get; set; } = 0.05f;


	[StiSerializable]
	[DefaultValue(0.05f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets relative height of the indicator.  It is given as part of the scale dimension.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float RelativeHeight { get; set; } = 0.05f;


	[StiSerializable]
	[DefaultValue(StiStateSkin.Ellipse)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the skin of the component rendering.")]
	public StiStateSkin Skin { get; set; }

	[Browsable(false)]
	[StiSerializable]
	[DefaultValue(null)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiGaugeElementSkin CustomSkin { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Format", Format);
		JsonExtensions.AddPropertyBool(val, "ShowValue", ShowValue, false, false);
		JsonExtensions.AddPropertyBrush(val, "TextBrush", TextBrush);
		JsonExtensions.AddPropertyFontArial7(val, "Font", Font);
		JsonExtensions.AddPropertyFloat(val, "Left", Left, 0f);
		JsonExtensions.AddPropertyFloat(val, "Top", Top, 0f);
		JsonExtensions.AddPropertyFloat(val, "RelativeHeight", RelativeHeight, 0.05f);
		JsonExtensions.AddPropertyFloat(val, "RelativeWidth", RelativeWidth, 0.05f);
		JsonExtensions.AddPropertyEnum(val, "Skin", (Enum)Skin);
		if ((int)mode == 0)
		{
			JsonExtensions.AddPropertyJObject(val, "Filters", Filters.SaveToJsonObject(mode));
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Format":
				Format = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ShowValue":
				ShowValue = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "TextBrush":
				TextBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Font":
				Font = StiJsonReportObjectHelper.DeserializeFont(item, Font);
				break;
			case "Left":
				Left = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Top":
				Top = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "RelativeHeight":
				RelativeHeight = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "RelativeWidth":
				RelativeWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Skin":
				Skin = StiJsonReportObjectHelper.DeserializeEnum<StiStateSkin>(item);
				break;
			case "Filters":
				Filters.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.Value() };
		stiPropertyCollection.Add(StiPropertyCategories.Value, value);
		value = new StiPropertyObject[2]
		{
			propertiesHelper.Placement(),
			propertiesHelper.Skin()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Indicator, value);
		value = new StiPropertyObject[4]
		{
			propertiesHelper.Font(),
			propertiesHelper.Format(),
			propertiesHelper.ShowValue(),
			propertiesHelper.TextBrush()
		};
		stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, value);
		value = new StiPropertyObject[4]
		{
			propertiesHelper.LeftF(),
			propertiesHelper.RelativeHeight(),
			propertiesHelper.RelativeWidth(),
			propertiesHelper.TopF()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Position, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.Brush(),
			propertiesHelper.BorderBrush(),
			propertiesHelper.BorderWidth()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[1] { propertiesHelper.AllowApplyStyle() };
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		return stiPropertyCollection;
	}

	public override StiGaugeElement CreateNew()
	{
		return new StiStateIndicator();
	}

	protected override void OnValueChanged()
	{
		if (Filters == null)
		{
			return;
		}
		StiStateIndicatorFilter stiStateIndicatorFilter = null;
		float valueOrDefault = GetActualValue().GetValueOrDefault();
		for (int i = 0; i < Filters.Count; i++)
		{
			if (valueOrDefault >= Filters[i].StartValue && valueOrDefault <= Filters[i].EndValue)
			{
				stiStateIndicatorFilter = Filters[i];
				break;
			}
		}
		if (lastFilter != stiStateIndicatorFilter)
		{
			lastFilter = stiStateIndicatorFilter;
			if (stiStateIndicatorFilter != null)
			{
				Brush = stiStateIndicatorFilter.Brush;
				BorderBrush = stiStateIndicatorFilter.BorderBrush;
			}
		}
	}

	protected internal override void InteractiveClick(RectangleF rect, Point p)
	{
	}

	protected internal override void DrawElement(StiGaugeContextPainter context)
	{
		SizeF size = base.Scale.barGeometry.Size;
		RectangleF rect = context.Rect;
		RectangleF rect2 = new RectangleF(rect.X + size.Width * Left, rect.Y + size.Height * Top, size.Width * RelativeWidth, size.Height * RelativeHeight);
		GetActualSkin().Draw(context, this, rect2, null, null);
	}

	protected StiGaugeElementSkin GetActualSkin()
	{
		if (CustomSkin == null)
		{
			return StiGaugeSkinHelper.GetStateIndicatorSkin(Skin);
		}
		return CustomSkin;
	}
}
