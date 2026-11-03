using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Events;
using Stimulsoft.Report.Gauge.Helpers;

namespace Stimulsoft.Report.Components.Gauge.Primitives;

public abstract class StiTickBase : StiGaugeElement
{
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the property SkipValues.")]
	public StiGetSkipValuesEvent GetSkipValuesEvent { get; set; } = new StiGetSkipValuesEvent();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the property SkipIndices.")]
	public StiGetSkipIndicesEvent GetSkipIndicesEvent { get; set; } = new StiGetSkipIndicesEvent();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the values that should not be rendered. Example: 1;2;3")]
	public virtual StiSkipValuesExpression SkipValues { get; set; } = new StiSkipValuesExpression();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the indexes of values that cannot be displayed. Example: 1;2;3")]
	public virtual StiSkipIndicesExpression SkipIndices { get; set; } = new StiSkipIndicesExpression();


	[DefaultValue(StiPlacement.Outside)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets the placement of the tick relative to the scale.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiPlacement Placement { get; set; }

	[Browsable(false)]
	[DefaultValue(null)]
	[StiSerializable]
	public float[] SkipValuesObj { get; set; }

	[Browsable(false)]
	[DefaultValue(null)]
	[StiSerializable]
	public float[] SkipIndicesObj { get; set; }

	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the offset from the scale based on the StiPlacement.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float Offset { get; set; }

	[DefaultValue(null)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the minimum value, with which displaying should begin.")]
	public float? MinimumValue { get; set; }

	[DefaultValue(null)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the maximum value to which displaying should be.")]
	public float? MaximumValue { get; set; }

	[Browsable(false)]
	protected virtual bool IsSkipMajorValues => false;

	public event StiGetValueEventHandler GetSkipValues;

	public event StiGetValueEventHandler GetSkipIndices;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyJObject(val, "GetSkipValuesEvent", GetSkipValuesEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetSkipIndicesEvent", GetSkipIndicesEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "SkipValues", SkipValues.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "SkipIndices", SkipIndices.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyEnum(val, "Placement", (Enum)Placement);
		JsonExtensions.AddPropertyFloat(val, "Offset", Offset, 0f);
		JsonExtensions.AddPropertyFloatNullable(val, "MinimumValue", MinimumValue, (float?)null);
		JsonExtensions.AddPropertyFloatNullable(val, "MaximumValue", MaximumValue, (float?)null);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "GetSkipValuesEvent":
			{
				StiGetSkipValuesEvent stiGetSkipValuesEvent = new StiGetSkipValuesEvent();
				stiGetSkipValuesEvent.LoadFromJsonObject((JObject)item.Value);
				GetSkipValuesEvent = stiGetSkipValuesEvent;
				break;
			}
			case "GetSkipIndicesEvent":
			{
				StiGetSkipIndicesEvent stiGetSkipIndicesEvent = new StiGetSkipIndicesEvent();
				stiGetSkipIndicesEvent.LoadFromJsonObject((JObject)item.Value);
				GetSkipIndicesEvent = stiGetSkipIndicesEvent;
				break;
			}
			case "SkipValues":
			{
				StiSkipValuesExpression stiSkipValuesExpression = new StiSkipValuesExpression();
				stiSkipValuesExpression.LoadFromJsonObject((JObject)item.Value);
				SkipValues = stiSkipValuesExpression;
				break;
			}
			case "SkipIndices":
			{
				StiSkipIndicesExpression stiSkipIndicesExpression = new StiSkipIndicesExpression();
				stiSkipIndicesExpression.LoadFromJsonObject((JObject)item.Value);
				SkipIndices = stiSkipIndicesExpression;
				break;
			}
			case "Placement":
				Placement = StiJsonReportObjectHelper.DeserializeEnum<StiPlacement>(item);
				break;
			case "Offset":
				Offset = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "MinimumValue":
				MinimumValue = StiJsonReportObjectHelper.DeserializeFloatNullable(item);
				break;
			case "MaximumValue":
				MaximumValue = StiJsonReportObjectHelper.DeserializeFloatNullable(item);
				break;
			}
		}
	}

	public override object Clone()
	{
		StiTickBase stiTickBase = (StiTickBase)base.Clone();
		if (SkipValues != null)
		{
			stiTickBase.SkipValues = (StiSkipValuesExpression)SkipValues.Clone();
		}
		else
		{
			stiTickBase.SkipValues = null;
		}
		if (SkipIndices != null)
		{
			stiTickBase.SkipIndices = (StiSkipIndicesExpression)SkipIndices.Clone();
		}
		else
		{
			stiTickBase.SkipIndices = null;
		}
		return stiTickBase;
	}

	protected virtual void OnGetSkipValues(StiGetValueEventArgs e)
	{
	}

	public virtual void InvokeGetSkipValues(StiGaugeElement sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetSkipValues(e);
			this.GetSkipValues?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			string text = string.Format("Expression in GetSkipValues property of '{0}' series from '{1}' chart can't be evaluated!", "StiTickBase", base.Scale.Gauge.Name);
			StiLogService.Write(GetType(), text);
			StiLogService.Write(GetType(), ex.Message);
			base.Scale.Gauge.Report.WriteToReportRenderingMessages(text);
		}
	}

	protected virtual void OnGetSkipIndices(StiGetValueEventArgs e)
	{
	}

	public virtual void InvokeGetSkipIndices(StiGaugeElement sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetSkipIndices(e);
			this.GetSkipIndices?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			string text = string.Format("Expression in GetSkipIndices property of '{0}' series from '{1}' chart can't be evaluated!", "StiTickBase", base.Scale.Gauge.Name);
			StiLogService.Write(GetType(), text);
			StiLogService.Write(GetType(), ex.Message);
			base.Scale.Gauge.Report.WriteToReportRenderingMessages(text);
		}
	}

	protected virtual Dictionary<float, float> GetPointCollection()
	{
		return null;
	}

	protected Dictionary<float, float> GetMinorCollections()
	{
		Dictionary<float, float> dictionary = new Dictionary<float, float>();
		Dictionary<float, float> dictionary2 = ((!IsSkipMajorValues) ? null : GetMajorCollections());
		float num = Math.Abs(base.Scale.MinorInterval);
		if (num == 0f)
		{
			num = 1f;
		}
		float actualMinimum = base.Scale.ScaleHelper.ActualMinimum;
		float actualMaximum = base.Scale.ScaleHelper.ActualMaximum;
		float totalLength = base.Scale.ScaleHelper.TotalLength;
		float num2 = actualMinimum;
		float num3 = num;
		float num4 = 0f;
		dictionary.Add(num2, 0f);
		float num5 = totalLength / 100f;
		for (num2 += num; num2 <= actualMaximum; num2 += num)
		{
			num4 += num3;
			if (!dictionary.ContainsKey(num2))
			{
				dictionary.Add(num2, num4 / num5 * 0.01f);
			}
		}
		if (dictionary2 != null)
		{
			foreach (float key in dictionary2.Keys)
			{
				if (dictionary.ContainsKey(key))
				{
					dictionary.Remove(key);
				}
			}
		}
		return dictionary;
	}

	protected Dictionary<float, float> GetMajorCollections()
	{
		Dictionary<float, float> dictionary = new Dictionary<float, float>();
		float num = Math.Abs(base.Scale.MajorInterval);
		if (num == 0f)
		{
			num = 1f;
		}
		float actualMinimum = base.Scale.ScaleHelper.ActualMinimum;
		float actualMaximum = base.Scale.ScaleHelper.ActualMaximum;
		float totalLength = base.Scale.ScaleHelper.TotalLength;
		float num2 = num;
		float num3 = actualMinimum;
		float num4 = 0f;
		dictionary.Add(num3, 0f);
		float num5 = totalLength / 100f;
		for (num3 += num; num3 <= actualMaximum; num3 += num)
		{
			num4 += num2;
			if (!dictionary.ContainsKey(num3))
			{
				dictionary.Add(num3, num4 / num5 * 0.01f);
			}
		}
		return dictionary;
	}

	protected bool CheckTickValue(float[] skipValues, float[] skipIndices, double key, int value)
	{
		if (skipIndices != null)
		{
			for (int i = 0; i < skipIndices.Length; i++)
			{
				if (skipIndices[i] == (float)value)
				{
					return true;
				}
			}
		}
		if (skipValues != null)
		{
			for (int j = 0; j < skipValues.Length; j++)
			{
				if ((double)skipValues[j] == key)
				{
					return true;
				}
			}
		}
		return false;
	}

	protected internal override void PrepareGaugeElement()
	{
		StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
		InvokeGetSkipValues(this, stiGetValueEventArgs);
		SkipValuesObj = StiGaugeHelper.GetFloatArrayValueFromString(stiGetValueEventArgs.Value);
		StiGetValueEventArgs stiGetValueEventArgs2 = new StiGetValueEventArgs();
		InvokeGetSkipIndices(this, stiGetValueEventArgs2);
		SkipIndicesObj = StiGaugeHelper.GetFloatArrayValueFromString(stiGetValueEventArgs2.Value);
	}

	protected float GetOffset(float? value)
	{
		if (value.HasValue)
		{
			return value.Value;
		}
		return Offset;
	}

	protected StiPlacement GetPlacement(StiPlacement? value)
	{
		if (value.HasValue)
		{
			return value.Value;
		}
		return Placement;
	}
}
