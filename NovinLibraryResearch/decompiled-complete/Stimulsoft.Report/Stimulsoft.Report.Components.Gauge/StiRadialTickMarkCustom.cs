using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Collections;
using Stimulsoft.Report.Gauge.Events;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Gauge.Primitives;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public class StiRadialTickMarkCustom : StiRadialTickMarkBase, IStiTickCustom
{
	private StiValueExpression value = new StiValueExpression();

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiRadialTickMarkCustom;

	[Browsable(false)]
	[DefaultValue(0f)]
	[StiSerializable]
	public float ValueObj { get; set; }

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiCustomValuesCollection Values { get; set; } = new StiCustomValuesCollection();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the property Value.")]
	public StiGetValueEvent GetValueEvent { get; set; } = new StiGetValueEvent();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets value corresponds to this tick mark. Example: {Order.Value}")]
	public virtual StiValueExpression Value
	{
		get
		{
			return value;
		}
		set
		{
			this.value = value;
		}
	}

	public override StiGaugeElemenType ElementType => StiGaugeElemenType.RadialElement;

	public override string LocalizeName => "RadialTickMarkCustom";

	public event StiGetValueEventHandler GetValue;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyJObject(val, "GetValueEvent", GetValueEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Value", value.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Values", Values.SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "GetSkipValuesEvent":
			{
				StiGetValueEvent stiGetValueEvent = new StiGetValueEvent();
				stiGetValueEvent.LoadFromJsonObject((JObject)item.Value);
				GetValueEvent = stiGetValueEvent;
				break;
			}
			case "GetSkipIndicesEvent":
			{
				StiValueExpression stiValueExpression = new StiValueExpression();
				stiValueExpression.LoadFromJsonObject((JObject)item.Value);
				value = stiValueExpression;
				break;
			}
			case "Values":
				Values.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] array = new StiPropertyObject[6]
		{
			propertiesHelper.NullableMaximumValue(),
			propertiesHelper.NullableMinimumValue(),
			propertiesHelper.Offset(),
			propertiesHelper.SkipIndices(),
			propertiesHelper.SkipValues(),
			propertiesHelper.Value()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Value, array);
		array = new StiPropertyObject[5]
		{
			propertiesHelper.OffsetAngle(),
			propertiesHelper.Placement(),
			propertiesHelper.RelativeHeight(),
			propertiesHelper.RelativeWidth(),
			propertiesHelper.Skin()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Tick, array);
		array = new StiPropertyObject[3]
		{
			propertiesHelper.Brush(),
			propertiesHelper.BorderBrush(),
			propertiesHelper.BorderWidth()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, array);
		array = new StiPropertyObject[1] { propertiesHelper.AllowApplyStyle() };
		stiPropertyCollection.Add(StiPropertyCategories.Misc, array);
		return stiPropertyCollection;
	}

	public override object Clone()
	{
		StiRadialTickMarkCustom stiRadialTickMarkCustom = (StiRadialTickMarkCustom)base.Clone();
		if (value != null)
		{
			stiRadialTickMarkCustom.value = (StiValueExpression)value.Clone();
		}
		else
		{
			stiRadialTickMarkCustom.value = null;
		}
		stiRadialTickMarkCustom.Values = new StiCustomValuesCollection();
		lock (((ICollection)Values).SyncRoot)
		{
			foreach (StiCustomValueBase value in Values)
			{
				stiRadialTickMarkCustom.Values.Add((StiCustomValueBase)value.Clone());
			}
			return stiRadialTickMarkCustom;
		}
	}

	protected virtual void OnGetValue(StiGetValueEventArgs e)
	{
	}

	public virtual void InvokeGetValue(StiGaugeElement sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetValue(e);
			if (sender.Scale.Gauge.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				object obj = StiParser.ParseTextValue(Value.Value, sender.Scale.Gauge);
				e.Value = sender.Scale.Gauge.Report.ToString(obj);
			}
			this.GetValue?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			string text = string.Format("Expression in GetValue property of '{0}' series from '{1}' chart can't be evaluated!", "Indicator", base.Scale.Gauge.Name);
			StiLogService.Write(GetType(), text);
			StiLogService.Write(GetType(), ex.Message);
			base.Scale.Gauge.Report.WriteToReportRenderingMessages(text);
		}
	}

	public override StiGaugeElement CreateNew()
	{
		return new StiRadialTickMarkCustom();
	}

	protected internal override void PrepareGaugeElement()
	{
		base.PrepareGaugeElement();
		StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
		InvokeGetValue(this, stiGetValueEventArgs);
		ValueObj = StiGaugeHelper.GetFloatValueFromObject(stiGetValueEventArgs.Value, 0f);
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
		PointF center = stiRadialScale.barGeometry.Center;
		float sweepAngle = stiRadialScale.GetSweepAngle();
		float startAngle = stiRadialScale.StartAngle;
		float diameter = base.Scale.barGeometry.Diameter;
		float radius = base.Scale.barGeometry.Radius;
		float actualMinimum = base.Scale.ScaleHelper.ActualMinimum;
		float actualMaximum = base.Scale.ScaleHelper.ActualMaximum;
		float minWidth = base.Scale.ScaleHelper.MinWidth;
		float maxWidth = base.Scale.ScaleHelper.MaxWidth;
		maxWidth *= base.Scale.barGeometry.RectGeometry.Width;
		minWidth *= base.Scale.barGeometry.RectGeometry.Width;
		float num = maxWidth - minWidth;
		StiGaugeElementSkin actualSkin = GetActualSkin();
		float[] skipValuesObj = base.SkipValuesObj;
		float[] skipIndicesObj = base.SkipIndicesObj;
		StiCustomValuesCollection stiCustomValuesCollection = Values;
		if (stiCustomValuesCollection == null || stiCustomValuesCollection.Count == 0)
		{
			stiCustomValuesCollection = new StiCustomValuesCollection
			{
				new StiRadialTickMarkCustomValue(ValueObj, base.Offset, base.RelativeWidth, base.RelativeHeight, base.OffsetAngle, base.Placement, actualSkin)
			};
		}
		int num2 = -1;
		foreach (StiRadialTickMarkCustomValue item in stiCustomValuesCollection)
		{
			num2++;
			if (!(item.Value < actualMinimum) && !(item.Value > actualMaximum) && !CheckTickValue(skipValuesObj, skipIndicesObj, item.Value, num2) && (!base.MinimumValue.HasValue || !(item.Value < base.MinimumValue.Value)) && (!base.MaximumValue.HasValue || !(item.Value > base.MaximumValue.Value)))
			{
				float position = base.Scale.GetPosition(item.Value);
				float num3 = diameter * GetRelativeWidth(item.RelativeWidth);
				float height = diameter * GetRelativeHeight(item.RelativeHeight);
				float num4 = ((GetPlacement(item.Placement) == StiPlacement.Outside) ? (radius * (1f + GetOffset(item.Offset))) : (radius * (1f - GetOffset(item.Offset))));
				float num5 = sweepAngle * GetRelativeHeight(item.RelativeHeight) / 4f;
				float angle = (base.Scale.IsReversed ? (startAngle + sweepAngle - position * sweepAngle - GetOffsetAngle(item.OffsetAngle) + num5) : (startAngle + position * sweepAngle + GetOffsetAngle(item.OffsetAngle) - num5));
				float num6 = ((GetPlacement(item.Placement) == StiPlacement.Outside) ? num4 : ((GetPlacement(item.Placement) == StiPlacement.Overlay) ? ((!base.Scale.IsUp) ? (num4 - (maxWidth - num * position + num3) / 2f) : (num4 - (minWidth + num * position + num3) / 2f)) : ((!base.Scale.IsUp) ? (num4 - maxWidth + num * position - num3) : (num4 - minWidth - num * position - num3))));
				StiBrush val = null;
				float borderWidth = 0f;
				if (item.useBrush)
				{
					val = base.Brush;
					base.Brush = item.Brush;
				}
				if (item.useBorderBrush)
				{
					StiBrush borderBrush = base.BorderBrush;
					base.BorderBrush = item.BorderBrush;
				}
				if (item.useBorderWidth)
				{
					borderWidth = base.BorderWidth;
					base.BorderWidth = ((!item.BorderWidth.HasValue) ? 0f : item.BorderWidth.Value);
				}
				RectangleF rect = new RectangleF(center.X + num6, center.Y, num3, height);
				context.AddPushMatrixGaugeGeom(angle, center);
				StiGaugeElementSkin stiGaugeElementSkin = ((item.Skin == null) ? actualSkin : item.Skin);
				stiGaugeElementSkin.Draw(context, this, rect);
				context.AddPopTranformGaugeGeom();
				if (item.useBrush)
				{
					base.Brush = val;
				}
				if (item.useBorderBrush)
				{
					base.BorderBrush = val;
				}
				if (item.useBorderWidth)
				{
					base.BorderWidth = borderWidth;
				}
			}
		}
	}

	private float GetOffsetAngle(float? value)
	{
		if (!value.HasValue)
		{
			if (!base.Scale.IsReversed)
			{
				return base.OffsetAngle;
			}
			return 0f - base.OffsetAngle;
		}
		return value.Value;
	}
}
