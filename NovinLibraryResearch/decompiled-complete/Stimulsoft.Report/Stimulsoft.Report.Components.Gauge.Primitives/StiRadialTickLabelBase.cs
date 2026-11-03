using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.Gauge.Primitives;

public abstract class StiRadialTickLabelBase : StiTickLabelBase
{
	[StiSerializable]
	[DefaultValue(StiLabelRotationMode.None)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the rotation mode of labels.")]
	public StiLabelRotationMode LabelRotationMode { get; set; }

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
		JsonExtensions.AddPropertyEnum(val, "LabelRotationMode", (Enum)LabelRotationMode);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "OffsetAngle"))
			{
				if (name == "LabelRotationMode")
				{
					LabelRotationMode = StiJsonReportObjectHelper.DeserializeEnum<StiLabelRotationMode>(item);
				}
			}
			else
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
		double num = Math.Min((double)rectGeometry.Width * StiScale.System, (double)rectGeometry.Height * StiScale.System);
		if (num < 100.0)
		{
			return;
		}
		Dictionary<float, string> labels = StiTickLabelHelper.GetLabels(pointCollection);
		string textFormat = base.TextFormat;
		float actualMinimum = base.Scale.ScaleHelper.ActualMinimum;
		float actualMaximum = base.Scale.ScaleHelper.ActualMaximum;
		PointF center = stiRadialScale.barGeometry.Center;
		float radius = base.Scale.barGeometry.Radius;
		float diameter = base.Scale.barGeometry.Diameter;
		float sweepAngle = stiRadialScale.GetSweepAngle();
		float startAngle = stiRadialScale.StartAngle;
		float maxWidth = base.Scale.ScaleHelper.MaxWidth;
		float minWidth = base.Scale.ScaleHelper.MinWidth;
		maxWidth *= diameter;
		minWidth *= diameter;
		float num2 = maxWidth - minWidth;
		float num3 = ((base.Placement == StiPlacement.Outside) ? (radius * (1f + base.Offset)) : (radius * (1f - base.Offset)));
		float[] skipValuesObj = base.SkipValuesObj;
		float[] skipIndicesObj = base.SkipIndicesObj;
		int num4 = 1;
		Dictionary<float, string> dictionary = new Dictionary<float, string>();
		foreach (float key in pointCollection.Keys)
		{
			string text = ((base.FormatService == null) ? ((string.IsNullOrEmpty(textFormat) && base.Scale.Gauge.ShortValue) ? labels[key] : GetTextForRender(key, textFormat)) : base.FormatService.Format(key));
			dictionary.Add(key, text);
			if (text.Length > 0 && num4 < text.Length)
			{
				num4 = text.Length;
			}
		}
		if (num4 < 3)
		{
			num4 = 3;
		}
		int num5 = -1;
		foreach (float key2 in pointCollection.Keys)
		{
			num5++;
			if (key2 < actualMinimum || key2 > actualMaximum || CheckTickValue(skipValuesObj, skipIndicesObj, key2, num5) || (base.MinimumValue.HasValue && key2 < base.MinimumValue.Value) || (base.MaximumValue.HasValue && key2 > base.MaximumValue.Value))
			{
				continue;
			}
			string text2 = dictionary[key2];
			Font font = StiGaugeContextPainter.ChangeFontSize(base.Font, context.Zoom);
			string text3 = null;
			if (text2.Length < num4)
			{
				text3 = text2;
				int num6 = num4 - text2.Length;
				for (int i = 0; i < num6; i++)
				{
					text2 += "0";
				}
			}
			SizeF sizeF = context.MeasureString(text2, font);
			if (text3 != null)
			{
				text2 = text3;
			}
			float angle = (base.Scale.IsReversed ? (startAngle + sweepAngle - pointCollection[key2] * sweepAngle + OffsetAngle) : (startAngle + pointCollection[key2] * sweepAngle - OffsetAngle));
			float num7 = num3;
			PointF position;
			int num8 = ((base.Placement != 0) ? GetMatrixRotation(radius: (base.Placement != StiPlacement.Overlay) ? (base.Scale.IsUp ? (num7 - minWidth - num2 * pointCollection[key2] - sizeF.Width) : (num7 - maxWidth + num2 * pointCollection[key2] - sizeF.Width)) : (base.Scale.IsUp ? (num7 - (minWidth + num2 * pointCollection[key2]) / 2f - sizeF.Width / 2f) : (num7 - (maxWidth - num2 * pointCollection[key2]) / 2f - sizeF.Width / 2f)), context: context, centerPoint: center, textSize: sizeF, rotateMode: LabelRotationMode, angle: angle, position: out position) : GetMatrixRotation(context, center, sizeF, LabelRotationMode, num7, angle, out position));
			context.AddTextGaugeGeom(text2, font, base.TextBrush, new RectangleF(position, sizeF), null);
			int num9 = -1;
			while (++num9 < num8)
			{
				context.AddPopTranformGaugeGeom();
			}
		}
	}

	protected int GetMatrixRotation(StiGaugeContextPainter context, PointF centerPoint, SizeF textSize, StiLabelRotationMode rotateMode, float radius, float angle, out PointF position)
	{
		float num = 0f;
		switch (rotateMode)
		{
		case StiLabelRotationMode.Automatic:
			switch (GetRadialPosition(angle))
			{
			case StiRadialPosition.TopLeft:
			case StiRadialPosition.TopRight:
			case StiRadialPosition.TopCenter:
				num = 90f;
				break;
			case StiRadialPosition.BottonLeft:
			case StiRadialPosition.BottomRight:
			case StiRadialPosition.LeftCenter:
				num = -90f;
				break;
			case StiRadialPosition.BottomCenter:
				num = 0f - angle;
				break;
			case StiRadialPosition.RightCenter:
				num = 0f - angle - 90f;
				break;
			}
			break;
		case StiLabelRotationMode.None:
			num = 0f - angle;
			break;
		case StiLabelRotationMode.SurroundIn:
		{
			StiRadialPosition radialPosition = GetRadialPosition(angle);
			num = ((radialPosition != StiRadialPosition.TopCenter) ? (-90f) : (0f - angle - 180f));
			break;
		}
		default:
			num = 90f;
			break;
		}
		position = new PointF(centerPoint.X + radius, centerPoint.Y - textSize.Height / 2f);
		int num2 = 0;
		if (angle != 0f)
		{
			context.AddPushMatrixGaugeGeom(angle, centerPoint);
			num2++;
		}
		if (num != 0f)
		{
			context.AddPushMatrixGaugeGeom(num, new PointF(position.X + textSize.Width / 2f, position.Y + textSize.Height / 2f));
			num2++;
		}
		return num2;
	}

	private StiRadialPosition GetRadialPosition(float angle)
	{
		for (angle += 90f; angle > 360f; angle -= 360f)
		{
		}
		if (angle == 0f || angle == 360f)
		{
			return StiRadialPosition.TopCenter;
		}
		if (angle > 0f && angle < 90f)
		{
			return StiRadialPosition.TopRight;
		}
		if (angle == 90f)
		{
			return StiRadialPosition.RightCenter;
		}
		if (angle > 90f && angle < 180f)
		{
			return StiRadialPosition.BottomRight;
		}
		if (angle == 180f)
		{
			return StiRadialPosition.BottomCenter;
		}
		if (angle > 180f && angle < 270f)
		{
			return StiRadialPosition.BottonLeft;
		}
		if (angle == 270f)
		{
			return StiRadialPosition.LeftCenter;
		}
		return StiRadialPosition.TopLeft;
	}
}
