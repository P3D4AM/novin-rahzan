using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public class StiRadialMarker : StiMarkerBase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiRadialMarker;

	public override StiGaugeElemenType ElementType => StiGaugeElemenType.RadialElement;

	public override string LocalizeName => "RadialMarker";

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.Value() };
		stiPropertyCollection.Add(StiPropertyCategories.Value, value);
		value = new StiPropertyObject[5]
		{
			propertiesHelper.Offset(),
			propertiesHelper.Placement(),
			propertiesHelper.RelativeHeight(),
			propertiesHelper.RelativeWidth(),
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

	public override void ApplyStyle(IStiGaugeStyle style)
	{
		if (AllowApplyStyle)
		{
			Brush = style.Core.MarkerBrush;
			BorderBrush = style.Core.MarkerBorderBrush;
			BorderWidth = style.Core.MarkerBorderWidth;
			base.Skin = style.Core.MarkerSkin;
		}
	}

	public override StiGaugeElement CreateNew()
	{
		return new StiRadialMarker();
	}

	protected internal override void DrawElement(StiGaugeContextPainter context)
	{
		if (base.Scale is StiRadialScale stiRadialScale)
		{
			float value = GetActualValue() ?? 0f;
			float position = stiRadialScale.GetPosition(value);
			PointF center = stiRadialScale.barGeometry.Center;
			float num = base.Scale.barGeometry.Size.Width * base.RelativeWidth;
			float num2 = base.Scale.barGeometry.Size.Height * base.RelativeHeight;
			float diameter = base.Scale.barGeometry.Diameter;
			float num3 = StiMathHelper.MaxMinusMin(base.Scale.StartWidth, base.Scale.EndWidth) * diameter * position;
			float num4 = base.Scale.barGeometry.Radius - base.Offset * diameter;
			if (Placement == StiPlacement.Overlay)
			{
				num4 = ((!base.Scale.IsReversed) ? (num4 - (base.Scale.IsUp ? ((base.Scale.StartWidth * diameter + num + num3) / 2f) : ((base.Scale.StartWidth * diameter + num - num3) / 2f))) : (num4 - (base.Scale.IsUp ? ((base.Scale.EndWidth * diameter + num + num3) / 2f) : ((base.Scale.EndWidth * diameter + num - num3) / 2f))));
			}
			else if (Placement == StiPlacement.Inside)
			{
				num4 = ((!base.Scale.IsReversed) ? (num4 - (base.Scale.IsUp ? (base.Scale.StartWidth * diameter + num - num3) : (base.Scale.StartWidth * diameter + num + num3))) : (num4 - (base.Scale.IsUp ? (base.Scale.EndWidth * diameter + num - num3) : (base.Scale.EndWidth * diameter + num + num3))));
			}
			RectangleF rect = new RectangleF(num4 + center.X, center.Y - num2 / 2f, num, num2);
			float num5 = stiRadialScale.StartAngle + stiRadialScale.GetSweepAngle() * position;
			float num6 = stiRadialScale.StartAngle - num5;
			if (base.Scale.Gauge.IsAnimation)
			{
				StiRotationAnimation stiRotationAnimation = new StiRotationAnimation(num6, 0.0, center, StiGaugeHelper.GlobalDurationElement, TimeSpan.Zero);
				stiRotationAnimation.Id = "radialMarker" + stiRadialScale.Items.IndexOf(this);
				stiRotationAnimation.ApplyPreviousAnimation(context.Gauge.PreviousAnimations);
				base.Animation = stiRotationAnimation;
			}
			GetActualSkin().Draw(context, this, rect, num5, center);
		}
	}

	protected internal override void InteractiveClick(RectangleF rect, Point p)
	{
		throw new NotImplementedException();
	}
}
