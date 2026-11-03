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

public class StiLinearTickMarkCustom : StiLinearTickMarkBase, IStiTickCustom
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiLinearTickMarkCustom;

	[Browsable(false)]
	[StiSerializable]
	[DefaultValue(0f)]
	public float ValueObj { get; set; }

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiCustomValuesCollection Values { get; set; } = new StiCustomValuesCollection();


	public override string LocalizeName => "LinearTickMarkCustom";

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the property Value.")]
	public StiGetValueEvent GetValueEvent { get; set; } = new StiGetValueEvent();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets value corresponds to this tick mark. Example: {Order.Value}")]
	public virtual StiValueExpression Value { get; set; } = new StiValueExpression();


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
		JsonExtensions.AddPropertyJObject(val, "Value", Value.SaveToJsonObject(mode));
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
				Value = stiValueExpression;
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
		StiPropertyObject[] value = new StiPropertyObject[6]
		{
			propertiesHelper.NullableMaximumValue(),
			propertiesHelper.NullableMinimumValue(),
			propertiesHelper.Offset(),
			propertiesHelper.SkipIndices(),
			propertiesHelper.SkipValues(),
			propertiesHelper.Value()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Value, value);
		value = new StiPropertyObject[4]
		{
			propertiesHelper.Placement(),
			propertiesHelper.RelativeHeight(),
			propertiesHelper.RelativeWidth(),
			propertiesHelper.Skin()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Tick, value);
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

	public override object Clone()
	{
		StiLinearTickMarkCustom stiLinearTickMarkCustom = (StiLinearTickMarkCustom)base.Clone();
		stiLinearTickMarkCustom.Value = ((Value != null) ? ((StiValueExpression)Value.Clone()) : null);
		stiLinearTickMarkCustom.Values = new StiCustomValuesCollection();
		lock (((ICollection)Values).SyncRoot)
		{
			foreach (StiCustomValueBase value in Values)
			{
				stiLinearTickMarkCustom.Values.Add((StiCustomValueBase)value.Clone());
			}
			return stiLinearTickMarkCustom;
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
		return new StiLinearTickMarkCustom();
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
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		if (!(base.Scale is StiLinearScale stiLinearScale))
		{
			return;
		}
		SizeF size = stiLinearScale.barGeometry.Size;
		RectangleF rectGeometry = stiLinearScale.barGeometry.RectGeometry;
		float restToLenght = base.Scale.barGeometry.GetRestToLenght();
		float num = (((int)stiLinearScale.Orientation == 0) ? size.Height : size.Width);
		num *= base.Offset;
		float actualMinimum = base.Scale.ScaleHelper.ActualMinimum;
		float actualMaximum = base.Scale.ScaleHelper.ActualMaximum;
		StiGaugeElementSkin actualSkin = GetActualSkin();
		float[] skipValuesObj = base.SkipValuesObj;
		float[] skipIndicesObj = base.SkipIndicesObj;
		StiCustomValuesCollection stiCustomValuesCollection = Values;
		if (stiCustomValuesCollection == null || stiCustomValuesCollection.Count == 0)
		{
			stiCustomValuesCollection = new StiCustomValuesCollection
			{
				new StiLinearTickMarkCustomValue(ValueObj, base.Offset, base.RelativeWidth, base.RelativeHeight, base.Placement, actualSkin)
			};
		}
		int num2 = -1;
		foreach (StiLinearTickMarkCustomValue item in stiCustomValuesCollection)
		{
			num2++;
			if (item.Value < actualMinimum || item.Value > actualMaximum || CheckTickValue(skipValuesObj, skipIndicesObj, item.Value, num2) || (base.MinimumValue.HasValue && item.Value < base.MinimumValue.Value) || (base.MaximumValue.HasValue && item.Value > base.MaximumValue.Value))
			{
				continue;
			}
			float position = base.Scale.GetPosition(item.Value);
			float num3 = size.Width * GetRelativeWidth(item.RelativeWidth);
			float num4 = size.Height * GetRelativeHeight(item.RelativeHeight);
			float x;
			float y;
			if ((int)stiLinearScale.Orientation == 0)
			{
				float num5 = (stiLinearScale.IsReversed ? (rectGeometry.Width - rectGeometry.Width * position) : (rectGeometry.Width * position));
				if (GetPlacement(item.Placement) == StiPlacement.Overlay)
				{
					x = rectGeometry.Left + num5 - num3 / 2f;
					y = StiRectangleHelper.CenterY(rectGeometry) - num4 / 2f - num;
				}
				else
				{
					float num6 = ((stiLinearScale.StartWidth < stiLinearScale.EndWidth) ? ((1f - position) * restToLenght) : (restToLenght * position));
					if (GetPlacement(item.Placement) == StiPlacement.Outside)
					{
						x = rectGeometry.Left + num5 - num3 / 2f;
						y = rectGeometry.Top - num4 - num + num6;
					}
					else
					{
						x = rectGeometry.Left + num5 - num3 / 2f;
						y = rectGeometry.Bottom + num - num6;
					}
				}
			}
			else
			{
				float num5 = (stiLinearScale.IsReversed ? (rectGeometry.Height * position) : (rectGeometry.Height - rectGeometry.Height * position));
				if (GetPlacement(item.Placement) == StiPlacement.Overlay)
				{
					x = StiRectangleHelper.CenterX(rectGeometry) - num3 / 2f;
					y = rectGeometry.Top + num5 - num4 / 2f;
				}
				else
				{
					float num7 = ((stiLinearScale.StartWidth < stiLinearScale.EndWidth) ? ((1f - position) * restToLenght) : (restToLenght * position));
					if (GetPlacement(item.Placement) == StiPlacement.Outside)
					{
						x = rectGeometry.Left - num3 - 1f - num + num7;
						y = rectGeometry.Top + num5 - num4 / 2f;
					}
					else
					{
						x = rectGeometry.Right + 1f + num - num7;
						y = rectGeometry.Top + num5 - num4 / 2f;
					}
				}
			}
			StiGaugeElementSkin stiGaugeElementSkin = ((item.Skin == null) ? actualSkin : item.Skin);
			stiGaugeElementSkin.Draw(context, this, new RectangleF(x, y, num3, num4));
		}
	}
}
