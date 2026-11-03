using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Events;
using Stimulsoft.Report.Gauge.Helpers;

namespace Stimulsoft.Report.Components.Gauge.Primitives;

public abstract class StiIndicatorBase : StiGaugeElement
{
	private float valueObj;

	[Browsable(false)]
	[DefaultValue(0f)]
	[StiSerializable]
	public float ValueObj
	{
		get
		{
			return valueObj;
		}
		set
		{
			valueObj = value;
			OnValueChanged();
		}
	}

	[DefaultValue(StiPlacement.Overlay)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the placement of the component.")]
	public virtual StiPlacement Placement { get; set; } = StiPlacement.Overlay;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a brush to fill a component.")]
	[StiOrder(130)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.White);


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the border of the component.")]
	public virtual StiBrush BorderBrush { get; set; } = (StiBrush)new StiEmptyBrush();


	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(200)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the border thickness of the component.")]
	public virtual float BorderWidth { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the property Value.")]
	public StiGetValueEvent GetValueEvent { get; set; } = new StiGetValueEvent();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets the current position of an indicator. Example: {Order.Value}")]
	public virtual StiValueExpression Value { get; set; } = new StiValueExpression();


	public event StiGetValueEventHandler GetValue;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "Placement", (Enum)Placement);
		JsonExtensions.AddPropertyBrush(val, "BorderBrush", BorderBrush);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyFloat(val, "BorderWidth", BorderWidth, 0f);
		JsonExtensions.AddPropertyJObject(val, "GetValueEvent", GetValueEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Value", Value.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyFloat(val, "ValueObj", ValueObj, 0f);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Placement":
				Placement = StiJsonReportObjectHelper.DeserializeEnum<StiPlacement>(item);
				break;
			case "ValueObj":
				valueObj = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "BorderBrush":
				BorderBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "GetValueEvent":
			{
				StiGetValueEvent stiGetValueEvent = new StiGetValueEvent();
				stiGetValueEvent.LoadFromJsonObject((JObject)item.Value);
				GetValueEvent = stiGetValueEvent;
				break;
			}
			case "Value":
			{
				StiValueExpression stiValueExpression = new StiValueExpression();
				stiValueExpression.LoadFromJsonObject((JObject)item.Value);
				Value = stiValueExpression;
				break;
			}
			}
		}
	}

	public override object Clone()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		StiIndicatorBase stiIndicatorBase = (StiIndicatorBase)base.Clone();
		stiIndicatorBase.Brush = (StiBrush)Brush.Clone();
		stiIndicatorBase.BorderBrush = (StiBrush)BorderBrush.Clone();
		object value;
		if (Value == null)
		{
			StiValueExpression stiValueExpression2 = (stiIndicatorBase.Value = null);
			value = stiValueExpression2;
		}
		else
		{
			value = (StiValueExpression)Value.Clone();
		}
		stiIndicatorBase.Value = (StiValueExpression)value;
		return stiIndicatorBase;
	}

	protected virtual void OnGetValue(StiGetValueEventArgs e)
	{
	}

	public virtual void InvokeGetValue(StiGaugeElement sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetValue(e);
			if (sender.Scale.Gauge.Report != null && sender.Scale.Gauge.Report.CalculationMode == StiCalculationMode.Interpretation)
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

	protected internal override void PrepareGaugeElement()
	{
		StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
		InvokeGetValue(this, stiGetValueEventArgs);
		ValueObj = StiGaugeHelper.GetFloatValueFromObject(stiGetValueEventArgs.Value, base.Scale);
	}

	protected internal abstract void InteractiveClick(RectangleF rect, Point p);

	protected virtual void OnValueChanged()
	{
	}

	public float? GetActualValue()
	{
		if (base.Scale == null || base.Scale.Gauge == null)
		{
			return null;
		}
		try
		{
			if (base.Scale.Gauge.IsDesigning && ValueObj == 0f)
			{
				object obj = (Value.Value.StartsWith("{") ? StiParser.ParseTextValue(Value.Value, base.Scale.Gauge) : StiParser.ParseTextValue("{" + Value.Value + "}", base.Scale.Gauge));
				if (obj == null)
				{
					return 0f;
				}
				if (obj is string)
				{
					return float.Parse((obj as string).Trim().Replace(",", "."), new CultureInfo("en-us"));
				}
				return Convert.ToSingle(obj);
			}
			return ValueObj;
		}
		catch
		{
			return 0f;
		}
	}
}
