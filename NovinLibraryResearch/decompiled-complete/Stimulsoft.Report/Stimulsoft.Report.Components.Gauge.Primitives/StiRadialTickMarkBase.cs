using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.Gauge.Primitives;

public abstract class StiRadialTickMarkBase : StiTickMarkBase
{
	[StiSerializable]
	[DefaultValue(0f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets an additional rotation angle.")]
	public float OffsetAngle { get; set; }

	public override StiGaugeElemenType ElementType => StiGaugeElemenType.RadialElement;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyFloat(val, "OffsetAngle", OffsetAngle, 0f);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "OffsetAngle")
			{
				OffsetAngle = StiJsonReportObjectHelper.DeserializeFloat(item);
			}
		}
	}

	protected internal override void DrawElement(StiGaugeContextPainter context)
	{
		if (!(base.Scale is StiRadialScale stiRadialScale))
		{
			return;
		}
		RectangleF rectGeometry = base.Scale.barGeometry.RectGeometry;
		if (rectGeometry.Width <= 0f || rectGeometry.Height <= 0f)
		{
			return;
		}
		Dictionary<float, float> pointCollection = GetPointCollection();
		if (pointCollection.Count == 0)
		{
			return;
		}
		float[] skipValuesObj = base.SkipValuesObj;
		float[] skipIndicesObj = base.SkipIndicesObj;
		StiGaugeElementSkin actualSkin = GetActualSkin();
		PointF center = stiRadialScale.barGeometry.Center;
		float sweepAngle = stiRadialScale.GetSweepAngle();
		float startAngle = stiRadialScale.StartAngle;
		float actualMinimum = base.Scale.ScaleHelper.ActualMinimum;
		float actualMaximum = base.Scale.ScaleHelper.ActualMaximum;
		float diameter = base.Scale.barGeometry.Diameter;
		float radius = base.Scale.barGeometry.Radius;
		float minWidth = base.Scale.ScaleHelper.MinWidth;
		float maxWidth = base.Scale.ScaleHelper.MaxWidth;
		maxWidth *= base.Scale.barGeometry.RectGeometry.Width;
		minWidth *= base.Scale.barGeometry.RectGeometry.Width;
		float num = maxWidth - minWidth;
		radius = ((base.Placement == StiPlacement.Outside) ? (radius * (1f + base.Offset)) : (radius * (1f - base.Offset)));
		float num2 = diameter * base.RelativeWidth;
		float height = diameter * base.RelativeHeight;
		int num3 = -1;
		foreach (float key in pointCollection.Keys)
		{
			num3++;
			if (!(key < actualMinimum) && !(key > actualMaximum) && !CheckTickValue(skipValuesObj, skipIndicesObj, key, num3) && (!base.MinimumValue.HasValue || !(key < base.MinimumValue.Value)) && (!base.MaximumValue.HasValue || !(key > base.MaximumValue.Value)))
			{
				float num4 = sweepAngle * GetRelativeHeight(base.RelativeHeight) / 4f;
				float angle = (base.Scale.IsReversed ? (startAngle + sweepAngle - pointCollection[key] * sweepAngle + OffsetAngle + num4) : (startAngle + pointCollection[key] * sweepAngle + OffsetAngle - num4));
				float num5 = ((base.Placement == StiPlacement.Outside) ? radius : ((base.Placement == StiPlacement.Overlay) ? ((!base.Scale.IsUp) ? (radius - (maxWidth - num * pointCollection[key] + num2) / 2f) : (radius - (minWidth + num * pointCollection[key] + num2) / 2f)) : ((!base.Scale.IsUp) ? (radius - maxWidth + num * pointCollection[key] - num2) : (radius - minWidth - num * pointCollection[key] - num2))));
				RectangleF rect = new RectangleF(center.X + num5, center.Y, num2, height);
				context.AddPushMatrixGaugeGeom(angle, center);
				actualSkin.Draw(context, this, rect);
				context.AddPopTranformGaugeGeom();
			}
		}
	}
}
