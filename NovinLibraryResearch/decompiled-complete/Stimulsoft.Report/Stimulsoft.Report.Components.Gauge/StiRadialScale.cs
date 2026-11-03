using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public class StiRadialScale : StiScaleBase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiRadialScale;

	[StiSerializable]
	[DefaultValue(0.75f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets outer radius of the radial scale. Should be from 0 to 1.")]
	[StiOrder(190)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float Radius { get; set; } = 0.75f;


	[StiSerializable]
	[DefaultValue(StiRadiusMode.Auto)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(200)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the mode of calculating the radius RadialScale.")]
	public StiRadiusMode RadiusMode { get; set; }

	[StiSerializable]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets center of the radial scale.")]
	[TypeConverter(typeof(StiPointFConverter))]
	public PointF Center { get; set; } = new PointF(0.5f, 0.5f);


	[StiSerializable]
	[DefaultValue(45f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets start angle of the radial scale.")]
	[StiOrder(170)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float StartAngle { get; set; } = 45f;


	[StiSerializable]
	[DefaultValue(300f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets sweep angle of the radial scale.")]
	[StiOrder(180)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float SweepAngle { get; set; } = 300f;


	[StiSerializable]
	[DefaultValue(StiRadialScaleSkin.Default)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(210)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the skin of the component rendering.")]
	public StiRadialScaleSkin Skin { get; set; }

	public override StiGaugeElemenType ScaleType => StiGaugeElemenType.RadialElement;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyFloat(val, "Radius", Radius, 0f);
		JsonExtensions.AddPropertyEnum(val, "RadiusMode", (Enum)RadiusMode);
		JsonExtensions.AddPropertyPointF(val, "Center", Center);
		JsonExtensions.AddPropertyFloat(val, "StartAngle", StartAngle, 45f);
		JsonExtensions.AddPropertyFloat(val, "SweepAngle", SweepAngle, 300f);
		JsonExtensions.AddPropertyEnum(val, "Skin", (Enum)Skin);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Radius":
				Radius = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "RadiusMode":
				RadiusMode = StiJsonReportObjectHelper.DeserializeEnum<StiRadiusMode>(item);
				break;
			case "Center":
				Center = StiJsonReportObjectHelper.DeserializePointF(item);
				break;
			case "StartAngle":
				StartAngle = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "SweepAngle":
				SweepAngle = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Skin":
				Skin = StiJsonReportObjectHelper.DeserializeEnum<StiRadialScaleSkin>(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[12]
		{
			propertiesHelper.Minimum(),
			propertiesHelper.Maximum(),
			propertiesHelper.MajorInterval(),
			propertiesHelper.MinorInterval(),
			propertiesHelper.StartWidth(),
			propertiesHelper.EndWidth(),
			propertiesHelper.IsReversed(),
			propertiesHelper.StartAngle(),
			propertiesHelper.SweepAngle(),
			propertiesHelper.Radius(),
			propertiesHelper.RadiusMode(),
			propertiesHelper.Skin()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Scale, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.Center(),
			propertiesHelper.LeftF(),
			propertiesHelper.TopF()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Position, value);
		value = new StiPropertyObject[2]
		{
			propertiesHelper.Brush(),
			propertiesHelper.BorderBrush()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[1] { propertiesHelper.AllowApplyStyle() };
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		return stiPropertyCollection;
	}

	public override object Clone()
	{
		return base.Clone();
	}

	public override void ApplyStyle(IStiGaugeStyle style)
	{
		foreach (StiGaugeElement item in base.Items)
		{
			item.ApplyStyle(style);
		}
	}

	protected internal float GetRadius()
	{
		if (!(Radius < 0f))
		{
			return Radius;
		}
		return 0f;
	}

	protected internal float GetStartWidth()
	{
		float num = base.StartWidth;
		if (num < 0f)
		{
			num = 0f;
		}
		else if (num > 1f)
		{
			num = 1f;
		}
		return num;
	}

	protected internal float GetEndWidth()
	{
		float num = base.EndWidth;
		if (num < 0f)
		{
			num = 0f;
		}
		else if (num > 1f)
		{
			num = 1f;
		}
		return num;
	}

	protected internal float GetSweepAngle()
	{
		float num = SweepAngle;
		if (num < 0f)
		{
			num = 0f;
		}
		else if (num > 360f)
		{
			num = 360f;
		}
		return num;
	}

	internal float GetCurrentAngle(float angle)
	{
		return GetPosition(angle) * SweepAngle + StartAngle;
	}

	protected override void InteractiveClick(MouseEventArgs e)
	{
	}

	public override StiScaleBase CreateNew()
	{
		return new StiRadialScale();
	}

	public StiRadialScale()
	{
		barGeometry = new StiRadialBarGeometry(this);
	}
}
