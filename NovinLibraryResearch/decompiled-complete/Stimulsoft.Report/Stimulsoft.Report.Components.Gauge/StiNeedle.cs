using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public class StiNeedle : StiIndicatorBase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiNeedle;

	[StiSerializable]
	[DefaultValue("{0:F0}")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the format string for the ShowValue property.")]
	public string Format { get; set; } = "{0:F0}";


	[StiSerializable]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a value indicating whether to display a current value of the indicator.")]
	public bool ShowValue { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBrush TextBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.DimGray);


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public Font Font { get; set; } = new Font("Arial", 7f);


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a brush to fill a cap.")]
	[StiOrder(130)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBrush CapBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.FromArgb(158, 158, 158));


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the border of the cap.")]
	public StiBrush CapBorderBrush { get; set; } = (StiBrush)new StiEmptyBrush();


	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(200)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the border thickness of the cap.")]
	public float CapBorderWidth { get; set; }

	[DefaultValue(0f)]
	[StiOrder(418)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float OffsetNeedle { get; set; }

	[DefaultValue(0f)]
	[StiOrder(419)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float StartWidth { get; set; } = 0.1f;


	[DefaultValue(1f)]
	[StiOrder(420)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float EndWidth { get; set; } = 1f;


	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value indicating whether to calculate CenterPoint for the indicator automatically.")]
	public bool AutoCalculateCenterPoint { get; set; } = true;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiPointFConverter))]
	[Description("Gets or sets the center coordinate, around which it rotates. The minimum value - Point (0,0), the maximum - Point (1,1).")]
	public PointF CenterPoint { get; set; }

	[StiSerializable]
	[DefaultValue(0.1f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the height factor relative to the height of the basic component.")]
	public float RelativeHeight { get; set; } = 0.04f;


	[StiSerializable]
	[DefaultValue(0.6f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the width ratio relative to the height of the basic component.")]
	public float RelativeWidth { get; set; } = 0.4f;


	[Browsable(false)]
	[StiSerializable]
	[DefaultValue(StiNeedleSkin.SimpleNeedle)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets the skin of the component rendering.")]
	public StiNeedleSkin Skin { get; set; } = StiNeedleSkin.SimpleNeedle;


	[Browsable(false)]
	[StiSerializable]
	[DefaultValue(null)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiGaugeElementSkin CustomSkin { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	public override StiBrush Brush
	{
		get
		{
			return base.Brush;
		}
		set
		{
			base.Brush = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	public override StiBrush BorderBrush
	{
		get
		{
			return base.BorderBrush;
		}
		set
		{
			base.BorderBrush = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	public override float BorderWidth
	{
		get
		{
			return base.BorderWidth;
		}
		set
		{
			base.BorderWidth = value;
		}
	}

	public override StiGaugeElemenType ElementType => StiGaugeElemenType.RadialElement;

	public override string LocalizeName => "Needle";

	[Browsable(false)]
	public override StiPlacement Placement
	{
		get
		{
			return base.Placement;
		}
		set
		{
			base.Placement = value;
		}
	}

	public StiNeedle()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		Brush = (StiBrush)new StiSolidBrush(Color.FromArgb(158, 158, 158));
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Format", Format);
		JsonExtensions.AddPropertyBool(val, "ShowValue", ShowValue, false, false);
		JsonExtensions.AddPropertyBrush(val, "TextBrush", TextBrush);
		JsonExtensions.AddPropertyFontArial7(val, "Font", Font);
		JsonExtensions.AddPropertyBrush(val, "CapBrush", CapBrush);
		JsonExtensions.AddPropertyBrush(val, "CapBorderBrush", CapBorderBrush);
		JsonExtensions.AddPropertyFloat(val, "CapBorderWidth", CapBorderWidth, 0f);
		JsonExtensions.AddPropertyFloat(val, "OffsetNeedle", OffsetNeedle, 0f);
		JsonExtensions.AddPropertyFloat(val, "StartWidth", StartWidth, 0.1f);
		JsonExtensions.AddPropertyFloat(val, "EndWidth", EndWidth, 1f);
		JsonExtensions.AddPropertyBool(val, "AutoCalculateCenterPoint", AutoCalculateCenterPoint, true, false);
		JsonExtensions.AddPropertyPointF(val, "CenterPoint", CenterPoint);
		JsonExtensions.AddPropertyFloat(val, "RelativeHeight", RelativeHeight, 0.04f);
		JsonExtensions.AddPropertyFloat(val, "RelativeWidth", RelativeWidth, 0.4f);
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
			case "CapBrush":
				CapBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "CapBorderBrush":
				CapBorderBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "CapBorderWidth":
				CapBorderWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "OffsetNeedle":
				OffsetNeedle = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "StartWidth":
				StartWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "EndWidth":
				EndWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "AutoCalculateCenterPoint":
				AutoCalculateCenterPoint = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Center":
				CenterPoint = StiJsonReportObjectHelper.DeserializePointF(item);
				break;
			case "RelativeHeight":
				RelativeHeight = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "RelativeWidth":
				RelativeWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Skin":
				Skin = StiJsonReportObjectHelper.DeserializeEnum<StiNeedleSkin>(item);
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
		value = new StiPropertyObject[4]
		{
			propertiesHelper.Font(),
			propertiesHelper.Format(),
			propertiesHelper.ShowValue(),
			propertiesHelper.TextBrush()
		};
		stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, value);
		value = new StiPropertyObject[2]
		{
			propertiesHelper.AutoCalculateCenterPoint(),
			propertiesHelper.CenterPoint()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Position, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.CapBrush(),
			propertiesHelper.CapBorderBrush(),
			propertiesHelper.CapBorderWidth()
		};
		stiPropertyCollection.Add(StiPropertyCategories.CapNeedle, value);
		value = new StiPropertyObject[6]
		{
			propertiesHelper.Brush(),
			propertiesHelper.BorderBrush(),
			propertiesHelper.BorderWidth(),
			propertiesHelper.OffsetNeedle(),
			propertiesHelper.StartWidth(),
			propertiesHelper.EndWidth()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Needle, value);
		value = new StiPropertyObject[2]
		{
			propertiesHelper.RelativeHeight(),
			propertiesHelper.RelativeWidth()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Size, value);
		return stiPropertyCollection;
	}

	public override void ApplyStyle(IStiGaugeStyle style)
	{
		if (AllowApplyStyle)
		{
			Brush = style.Core.NeedleBrush;
			BorderBrush = style.Core.NeedleBorderBrush;
			CapBrush = style.Core.NeedleCapBrush;
			CapBorderBrush = style.Core.NeedleCapBorderBrush;
			BorderWidth = style.Core.NeedleBorderWidth;
			CapBorderWidth = style.Core.NeedleCapBorderWidth;
			StartWidth = style.Core.NeedleStartWidth;
			EndWidth = style.Core.NeedleEndWidth;
			RelativeHeight = style.Core.NeedleRelativeHeight;
			RelativeWidth = style.Core.NeedleRelativeWith;
		}
	}

	public override StiGaugeElement CreateNew()
	{
		return new StiNeedle();
	}

	protected internal override void DrawElement(StiGaugeContextPainter context)
	{
		if (!(base.Scale is StiRadialScale stiRadialScale))
		{
			return;
		}
		PointF center = base.Scale.barGeometry.Center;
		SizeF size = new SizeF(base.Scale.barGeometry.Diameter * RelativeWidth, base.Scale.barGeometry.Diameter * RelativeHeight);
		float num2;
		float num;
		if (size.Width > size.Height)
		{
			num2 = (num = size.Height / 2f);
		}
		else
		{
			num2 = size.Width / 2f;
			num = size.Height / 2f;
		}
		float x = center.X - num2;
		float y = center.Y - num;
		RectangleF rect = new RectangleF(new PointF(x, y), size);
		float? actualValue = GetActualValue();
		if (actualValue.HasValue)
		{
			float valueOrDefault = actualValue.GetValueOrDefault();
			float currentAngle = stiRadialScale.GetCurrentAngle(valueOrDefault);
			float value = ((!(currentAngle > stiRadialScale.SweepAngle + stiRadialScale.StartAngle)) ? (base.Scale.IsReversed ? (stiRadialScale.SweepAngle - currentAngle) : currentAngle) : (base.Scale.IsReversed ? stiRadialScale.GetCurrentAngle(stiRadialScale.Minimum) : stiRadialScale.GetCurrentAngle(stiRadialScale.Maximum)));
			float num3 = (0f - stiRadialScale.GetPosition(valueOrDefault)) * stiRadialScale.SweepAngle;
			if (base.Scale.IsReversed)
			{
				num3 = 0f - num3;
			}
			if (context.Gauge.IsAnimation)
			{
				StiRotationAnimation stiRotationAnimation = new StiRotationAnimation(num3, 0.0, center, StiGaugeHelper.GlobalDurationElement, TimeSpan.Zero);
				stiRotationAnimation.Id = "needle_" + stiRadialScale.Items.IndexOf(this);
				stiRotationAnimation.ApplyPreviousAnimation(context.Gauge.PreviousAnimations);
				base.Animation = stiRotationAnimation;
			}
			StiGaugeElementSkin actualSkin = GetActualSkin();
			actualSkin.Draw(context, this, rect, value, center);
		}
	}

	protected internal override void InteractiveClick(RectangleF rect, Point p)
	{
		throw new NotImplementedException();
	}

	private StiGaugeElementSkin GetActualSkin()
	{
		if (CustomSkin == null)
		{
			return StiGaugeSkinHelper.GetNeedleIndicatorSkin(Skin);
		}
		return CustomSkin;
	}
}
