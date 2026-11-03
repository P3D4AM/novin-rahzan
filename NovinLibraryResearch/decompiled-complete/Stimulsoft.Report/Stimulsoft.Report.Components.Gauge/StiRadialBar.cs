using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public class StiRadialBar : StiBarBase
{
	private class RadialColorModeHelper
	{
		public bool standardBackground;

		public int defaultIndex = -99;

		public void Reset()
		{
			standardBackground = false;
			defaultIndex = -99;
		}
	}

	private StiBrush actualBush = (StiBrush)new StiEmptyBrush();

	private RadialColorModeHelper colorModeHelper = new RadialColorModeHelper();

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiRadialBar;

	public override StiGaugeElemenType ElementType => StiGaugeElemenType.RadialElement;

	protected override StiBarRangeListType BarType => StiBarRangeListType.RadialBar;

	public override string LocalizeName => "RadialBar";

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
			propertiesHelper.EndWidth(),
			propertiesHelper.Offset(),
			propertiesHelper.Placement(),
			propertiesHelper.StartWidth(),
			propertiesHelper.UseRangeColor()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Indicator, value);
		value = new StiPropertyObject[6]
		{
			propertiesHelper.Brush(),
			propertiesHelper.BorderBrush(),
			propertiesHelper.BorderWidth(),
			propertiesHelper.EmptyBrush(),
			propertiesHelper.EmptyBorderBrush(),
			propertiesHelper.EmptyBorderWidth()
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
			Brush = style.Core.RadialBarBrush;
			BorderBrush = style.Core.RadialBarBorderBrush;
			BorderWidth = style.Core.BorderWidth;
			base.EmptyBrush = style.Core.RadialBarEmptyBrush;
			base.EmptyBorderBrush = style.Core.RadialBarEmptyBorderBrush;
			base.EmptyBorderWidth = style.Core.BorderWidth;
			base.StartWidth = style.Core.RadialBarStartWidth;
			base.EndWidth = style.Core.RadialBarEndWidth;
			CheckActualBrushForTopGeometry();
		}
	}

	protected override void CheckActualBrushForTopGeometry()
	{
		if (base.UseRangeColor)
		{
			for (int num = base.RangeList.Count - 1; num >= 0; num--)
			{
				StiRadialIndicatorRangeInfo stiRadialIndicatorRangeInfo = base.RangeList[num] as StiRadialIndicatorRangeInfo;
				if (base.ValueObj >= stiRadialIndicatorRangeInfo.Value)
				{
					if (colorModeHelper.defaultIndex != num)
					{
						colorModeHelper.defaultIndex = num;
						actualBush = stiRadialIndicatorRangeInfo.Brush;
					}
					break;
				}
			}
		}
		else
		{
			colorModeHelper.standardBackground = true;
			actualBush = Brush;
		}
	}

	public override StiGaugeElement CreateNew()
	{
		return new StiRadialBar();
	}

	protected internal override void DrawElement(StiGaugeContextPainter context)
	{
		StiRadialScale stiRadialScale = base.Scale as StiRadialScale;
		float num = GetActualValue() ?? base.Scale.Minimum;
		if (context.Gauge.IsDesigning)
		{
			CheckActualBrushForTopGeometry();
		}
		if (stiRadialScale == null)
		{
			return;
		}
		float startValue;
		float num2;
		if (base.Scale.IsReversed)
		{
			startValue = base.Scale.Maximum - num;
			num2 = base.Scale.Maximum;
		}
		else
		{
			startValue = base.Scale.Minimum;
			num2 = num;
			if (num2 > base.Scale.Maximum)
			{
				num2 = base.Scale.Maximum;
			}
		}
		GetRangeGeometry(context, stiRadialScale, base.EmptyBrush, base.EmptyBorderBrush, base.EmptyBorderWidth, base.Scale.Minimum, base.Scale.Maximum);
		GetRangeGeometry(context, stiRadialScale, actualBush, BorderBrush, BorderWidth, startValue, num2);
	}

	protected override void OnRangeColorChanged()
	{
		colorModeHelper.Reset();
	}

	protected internal override void InteractiveClick(RectangleF rect, Point p)
	{
	}

	private void GetRangeGeometry(StiGaugeContextPainter context, StiRadialScale scale, StiBrush bg, StiBrush bb, float bw, float startValue, float endValue)
	{
		PointF center = scale.barGeometry.Center;
		float radius = scale.barGeometry.Radius;
		float diameter = scale.barGeometry.Diameter;
		float num = ((!(base.StartWidth >= 0f)) ? 0f : (scale.IsReversed ? (base.EndWidth * diameter) : (base.StartWidth * diameter)));
		float num2 = ((!(base.EndWidth >= 0f)) ? 0f : (scale.IsReversed ? (base.StartWidth * diameter) : (base.EndWidth * diameter)));
		float startAngle = scale.StartAngle + scale.GetPosition(startValue) * scale.SweepAngle;
		float sweepAngle = (scale.GetPosition(endValue) - scale.GetPosition(startValue)) * scale.SweepAngle;
		float num3 = (scale.IsReversed ? (scale.EndWidth * diameter) : (scale.StartWidth * diameter));
		float value = (scale.IsReversed ? (scale.StartWidth * diameter) : (scale.EndWidth * diameter));
		float position = scale.GetPosition(startValue);
		float position2 = scale.GetPosition(endValue);
		float num4 = StiMathHelper.MaxMinusMin(num3, value);
		float num5;
		float num6;
		float num7;
		float num8;
		if (Placement == StiPlacement.Outside)
		{
			num5 = radius;
			num6 = radius;
			num7 = radius + num;
			num8 = radius + num2;
		}
		else if (Placement == StiPlacement.Overlay)
		{
			if (scale.IsUp)
			{
				num5 = radius - (num3 + num4 * position - num) / 2f;
				num6 = radius - (num3 + num4 * position2 - num2) / 2f;
				num7 = radius - (num3 + num4 * position + num) / 2f;
				num8 = radius - (num3 + num4 * position2 + num2) / 2f;
			}
			else
			{
				num5 = radius - (num3 - num4 * position - num) / 2f;
				num6 = radius - (num3 - num4 * position2 - num2) / 2f;
				num7 = radius - (num3 - num4 * position + num) / 2f;
				num8 = radius - (num3 - num4 * position2 + num2) / 2f;
			}
		}
		else if (scale.IsUp)
		{
			num5 = radius - (num3 + num4 * position);
			num6 = radius - (num3 + num4 * position2);
			num7 = radius - (num3 + num4 * position + num);
			num8 = radius - (num3 + num4 * position2 + num2);
		}
		else
		{
			num5 = radius - (num3 - num4 * position);
			num6 = radius - (num3 - num4 * position2);
			num7 = radius - (num3 - num4 * position + num);
			num8 = radius - (num3 - num4 * position2 + num2);
		}
		if (!float.IsNaN(num5) && !float.IsNaN(num6) && !float.IsNaN(num7) && !float.IsNaN(num8))
		{
			float max = StiMathHelper.GetMax(num5, num6, num7, num8);
			RectangleF rect = new RectangleF(center.X - max, center.Y - max, max * 2f, max * 2f);
			context.AddRadialRangeGaugeGeom(rect, bg, bb, bw, center, startAngle, sweepAngle, num5, num6, num7, num8);
		}
	}
}
