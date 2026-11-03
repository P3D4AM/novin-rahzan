using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public class StiRadialRange : StiRangeBase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiRadialRange;

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool UseValuesFromTheSpecifiedRange { get; set; } = true;


	public override string LocalizeName => "RadialRange";

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyBool(val, "UseValuesFromTheSpecifiedRange", UseValuesFromTheSpecifiedRange, true, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "UseValuesFromTheSpecifiedRange")
			{
				UseValuesFromTheSpecifiedRange = StiJsonReportObjectHelper.DeserializeBool(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[2]
		{
			propertiesHelper.EndValue(),
			propertiesHelper.StartValue()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Value, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.Brush(),
			propertiesHelper.BorderBrush(),
			propertiesHelper.BorderWidth()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[5]
		{
			propertiesHelper.EndWidth(),
			propertiesHelper.Offset(),
			propertiesHelper.Placement(),
			propertiesHelper.StartWidth(),
			propertiesHelper.UseValuesFromTheSpecifiedRange()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		return stiPropertyCollection;
	}

	protected internal override void DrawRange(StiGaugeContextPainter context, StiScaleBase scale)
	{
		if (!(scale is StiRadialScale stiRadialScale) || base.StartWidth == 0f || base.EndWidth == 0f)
		{
			return;
		}
		float num = base.StartValue;
		if (UseValuesFromTheSpecifiedRange && num < scale.Minimum)
		{
			num = scale.Minimum;
		}
		else if (num > scale.Maximum)
		{
			return;
		}
		float num2 = base.EndValue;
		if (UseValuesFromTheSpecifiedRange && num2 > scale.Maximum)
		{
			num2 = scale.Maximum;
		}
		else if (num2 < scale.Minimum)
		{
			return;
		}
		float diameter = scale.barGeometry.Diameter;
		float num3;
		float value;
		if (scale.IsReversed)
		{
			num = scale.Maximum - num;
			num2 = scale.Maximum - num2;
			num3 = scale.EndWidth * diameter;
			value = scale.StartWidth * diameter;
		}
		else
		{
			num3 = scale.StartWidth * diameter;
			value = scale.EndWidth * diameter;
		}
		RectangleF rectGeometry = scale.barGeometry.RectGeometry;
		PointF center = scale.barGeometry.Center;
		float radius = scale.barGeometry.Radius;
		float num4 = ((base.StartWidth >= 0f) ? (base.StartWidth * diameter) : 0f);
		float num5 = ((base.EndWidth >= 0f) ? (base.EndWidth * diameter) : 0f);
		float startAngle = stiRadialScale.StartAngle + scale.GetPosition(num) * stiRadialScale.SweepAngle;
		float sweepAngle = (scale.GetPosition(num2) - scale.GetPosition(num)) * stiRadialScale.SweepAngle;
		float position = scale.GetPosition(num);
		float position2 = scale.GetPosition(num2);
		float num6 = StiMathHelper.MaxMinusMin(num3, value);
		float radius2;
		float radius3;
		float radius4;
		float radius5;
		if (base.Placement == StiPlacement.Outside)
		{
			radius2 = radius;
			radius3 = radius;
			radius4 = radius + num4;
			radius5 = radius + num5;
		}
		else if (base.Placement == StiPlacement.Overlay)
		{
			if (scale.IsUp)
			{
				radius2 = radius - (num3 + num6 * position - num4) / 2f;
				radius3 = radius - (num3 + num6 * position2 - num5) / 2f;
				radius4 = radius - (num3 + num6 * position + num4) / 2f;
				radius5 = radius - (num3 + num6 * position2 + num5) / 2f;
			}
			else
			{
				radius2 = radius - (num3 - num6 * position - num4) / 2f;
				radius3 = radius - (num3 - num6 * position2 - num5) / 2f;
				radius4 = radius - (num3 - num6 * position + num4) / 2f;
				radius5 = radius - (num3 - num6 * position2 + num5) / 2f;
			}
		}
		else if (scale.IsUp)
		{
			radius2 = radius - (num3 + num6 * position);
			radius3 = radius - (num3 + num6 * position2);
			radius4 = radius - (num3 + num6 * position + num4);
			radius5 = radius - (num3 + num6 * position2 + num5);
		}
		else
		{
			radius2 = radius - (num3 - num6 * position);
			radius3 = radius - (num3 - num6 * position2);
			radius4 = radius - (num3 - num6 * position + num4);
			radius5 = radius - (num3 - num6 * position2 + num5);
		}
		context.AddRadialRangeGaugeGeom(rectGeometry, base.Brush, base.BorderBrush, base.BorderWidth, center, startAngle, sweepAngle, radius2, radius3, radius4, radius5);
	}

	public override StiRangeBase CreateNew()
	{
		return new StiRadialRange();
	}
}
