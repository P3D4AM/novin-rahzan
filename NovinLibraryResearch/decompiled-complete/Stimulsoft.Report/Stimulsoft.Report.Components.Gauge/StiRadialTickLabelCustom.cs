using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
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

public class StiRadialTickLabelCustom : StiRadialTickLabelBase, IStiTickCustom
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiRadialTickLabelCustom;

	[Browsable(false)]
	[StiSerializable]
	[DefaultValue(0f)]
	public float ValueObj { get; set; }

	[Browsable(false)]
	[StiSerializable]
	[DefaultValue(null)]
	public string TextObj { get; set; }

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiCustomValuesCollection Values { get; set; } = new StiCustomValuesCollection();


	public override StiGaugeElemenType ElementType => StiGaugeElemenType.RadialElement;

	public override string LocalizeName => "RadialTickLabelCustom";

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the property Value.")]
	public StiGetValueEvent GetValueEvent { get; set; } = new StiGetValueEvent();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the property Text.")]
	public StiGetTextEvent GetTextEvent { get; set; } = new StiGetTextEvent();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets value corresponds to this tick label. Example: {Order.Value}")]
	public virtual StiValueExpression Value { get; set; } = new StiValueExpression();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets text corresponds to this tick label. Example: {Order.Value}")]
	public virtual StiTextExpression Text { get; set; } = new StiTextExpression();


	public event StiGetValueEventHandler GetValue;

	public event StiGetTextEventHandler GetText;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyJObject(val, "GetValueEvent", GetValueEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetTextEvent", GetTextEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Value", Value.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Text", Text.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Values", Values.SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "GetValueEvent":
			{
				StiGetValueEvent stiGetValueEvent = new StiGetValueEvent();
				stiGetValueEvent.LoadFromJsonObject((JObject)item.Value);
				GetValueEvent = stiGetValueEvent;
				break;
			}
			case "GetTextEvent":
			{
				StiGetTextEvent stiGetTextEvent = new StiGetTextEvent();
				stiGetTextEvent.LoadFromJsonObject((JObject)item.Value);
				GetTextEvent = stiGetTextEvent;
				break;
			}
			case "Value":
			{
				StiValueExpression stiValueExpression = new StiValueExpression();
				stiValueExpression.LoadFromJsonObject((JObject)item.Value);
				Value = stiValueExpression;
				break;
			}
			case "Text":
			{
				StiTextExpression stiTextExpression = new StiTextExpression();
				stiTextExpression.LoadFromJsonObject((JObject)item.Value);
				Text = stiTextExpression;
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
		StiPropertyObject[] value = new StiPropertyObject[7]
		{
			propertiesHelper.NullableMaximumValue(),
			propertiesHelper.NullableMinimumValue(),
			propertiesHelper.Offset(),
			propertiesHelper.SkipIndices(),
			propertiesHelper.SkipValues(),
			propertiesHelper.Text(),
			propertiesHelper.Value()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Value, value);
		value = new StiPropertyObject[1] { propertiesHelper.Placement() };
		stiPropertyCollection.Add(StiPropertyCategories.Tick, value);
		value = new StiPropertyObject[5]
		{
			propertiesHelper.Font(),
			propertiesHelper.LabelRotationMode(),
			propertiesHelper.OffsetAngle(),
			propertiesHelper.TextBrush(),
			propertiesHelper.TextFormatStr()
		};
		stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, value);
		value = new StiPropertyObject[1] { propertiesHelper.AllowApplyStyle() };
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		return stiPropertyCollection;
	}

	public override object Clone()
	{
		StiRadialTickLabelCustom stiRadialTickLabelCustom = (StiRadialTickLabelCustom)base.Clone();
		stiRadialTickLabelCustom.Value = ((Value != null) ? ((StiValueExpression)Value.Clone()) : null);
		stiRadialTickLabelCustom.Text = ((Text != null) ? ((StiTextExpression)Text.Clone()) : null);
		stiRadialTickLabelCustom.Values = new StiCustomValuesCollection();
		lock (((ICollection)Values).SyncRoot)
		{
			foreach (StiCustomValueBase value in Values)
			{
				stiRadialTickLabelCustom.Values.Add((StiCustomValueBase)value.Clone());
			}
			return stiRadialTickLabelCustom;
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
			if (this.GetValue != null)
			{
				this.GetValue(sender, e);
			}
		}
		catch (Exception ex)
		{
			string text = string.Format("Expression in GetValue property of '{0}' series from '{1}' chart can't be evaluated!", "Indicator", base.Scale.Gauge.Name);
			StiLogService.Write(GetType(), text);
			StiLogService.Write(GetType(), ex.Message);
			base.Scale.Gauge.Report.WriteToReportRenderingMessages(text);
		}
	}

	protected virtual void OnGetText(StiGetTextEventArgs e)
	{
	}

	public virtual void InvokeGetText(StiGaugeElement sender, StiGetTextEventArgs e)
	{
		try
		{
			OnGetText(e);
			this.GetText?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			string text = string.Format("Expression in GetText property of '{0}' series from '{1}' chart can't be evaluated!", "GaugeElement", base.Scale.Gauge.Name);
			StiLogService.Write(GetType(), text);
			StiLogService.Write(GetType(), ex.Message);
			base.Scale.Gauge.Report.WriteToReportRenderingMessages(text);
		}
	}

	public override StiGaugeElement CreateNew()
	{
		return new StiRadialTickLabelCustom();
	}

	protected internal override void PrepareGaugeElement()
	{
		base.PrepareGaugeElement();
		StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
		InvokeGetValue(this, stiGetValueEventArgs);
		ValueObj = StiGaugeHelper.GetFloatValueFromObject(stiGetValueEventArgs.Value, 0f);
		StiGetTextEventArgs stiGetTextEventArgs = new StiGetTextEventArgs();
		InvokeGetText(this, stiGetTextEventArgs);
		TextObj = stiGetTextEventArgs.Value;
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
		float radius = base.Scale.barGeometry.Radius;
		float sweepAngle = stiRadialScale.GetSweepAngle();
		float startAngle = stiRadialScale.StartAngle;
		float actualMinimum = base.Scale.ScaleHelper.ActualMinimum;
		float actualMaximum = base.Scale.ScaleHelper.ActualMaximum;
		float maxWidth = base.Scale.ScaleHelper.MaxWidth;
		float minWidth = base.Scale.ScaleHelper.MinWidth;
		maxWidth *= radius * 2f;
		minWidth *= radius * 2f;
		float num = maxWidth - minWidth;
		string textFormat = base.TextFormat;
		float[] skipValuesObj = base.SkipValuesObj;
		float[] skipIndicesObj = base.SkipIndicesObj;
		StiCustomValuesCollection stiCustomValuesCollection = Values;
		if (stiCustomValuesCollection == null || stiCustomValuesCollection.Count == 0)
		{
			stiCustomValuesCollection = new StiCustomValuesCollection
			{
				new StiRadialTickLabelCustomValue(ValueObj, TextObj, base.Offset, base.OffsetAngle, base.LabelRotationMode, base.Placement)
			};
		}
		Font font = StiGaugeContextPainter.ChangeFontSize(base.Font, context.Zoom);
		int num2 = -1;
		foreach (StiRadialTickLabelCustomValue item in stiCustomValuesCollection)
		{
			num2++;
			if (!(item.Value < actualMinimum) && !(item.Value > actualMaximum) && !CheckTickValue(skipValuesObj, skipIndicesObj, item.Value, num2) && (!base.MinimumValue.HasValue || !(item.Value < base.MinimumValue.Value)) && (!base.MaximumValue.HasValue || !(item.Value > base.MaximumValue.Value)))
			{
				float position = base.Scale.GetPosition(item.Value);
				string value = (string.IsNullOrEmpty(item.Text) ? string.Empty : GetTextForRender(item.Text, textFormat));
				float num3 = ((GetPlacement(item.Placement) == StiPlacement.Outside) ? (radius * (1f + GetOffset(item.Offset))) : (radius * (1f - GetOffset(item.Offset))));
				string textForRender = GetTextForRender(value, textFormat);
				SizeF sizeF = context.MeasureString(textForRender, font);
				float angle = (base.Scale.IsReversed ? (startAngle + sweepAngle - position * sweepAngle + GetOffsetAngle(item.OffsetAngle)) : (startAngle + position * sweepAngle - GetOffsetAngle(item.OffsetAngle)));
				int num4 = 0;
				PointF position2 = default(PointF);
				if (GetPlacement(item.Placement) == StiPlacement.Outside)
				{
					num4 = GetMatrixRotation(context, center, sizeF, GetLabelRotationMode(item.LabelRotationMode), num3, angle, out position2);
				}
				else if (GetPlacement(item.Placement) == StiPlacement.Overlay)
				{
					num4 = GetMatrixRotation(radius: (!base.Scale.IsUp) ? (num3 - (maxWidth - num * position) / 2f - sizeF.Width / 2f) : (num3 - (minWidth + num * position) / 2f - sizeF.Width / 2f), context: context, centerPoint: center, textSize: sizeF, rotateMode: GetLabelRotationMode(item.LabelRotationMode), angle: angle, position: out position2);
				}
				else
				{
					float num5 = 0f;
					num4 = GetMatrixRotation(radius: (!base.Scale.IsUp) ? (num3 - maxWidth + num * position - sizeF.Width) : (num3 - minWidth - num * position - sizeF.Width), context: context, centerPoint: center, textSize: sizeF, rotateMode: GetLabelRotationMode(item.LabelRotationMode), angle: angle, position: out position2);
				}
				context.AddTextGaugeGeom(textForRender, font, base.TextBrush, new RectangleF(position2, sizeF), null);
				for (int i = 0; i < num4; i++)
				{
					context.AddPopTranformGaugeGeom();
				}
			}
		}
	}

	private float GetOffsetAngle(float? value)
	{
		if (value.HasValue)
		{
			return value.Value;
		}
		return base.OffsetAngle;
	}

	private StiLabelRotationMode GetLabelRotationMode(StiLabelRotationMode? value)
	{
		if (value.HasValue)
		{
			return value.Value;
		}
		return base.LabelRotationMode;
	}
}
