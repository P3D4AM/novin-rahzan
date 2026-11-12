using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Dashboard.Components.Chart;

public abstract class StiChartAxis : IStiDefault, IStiJsonReportObject, ICloneable
{
	public enum Order
	{
		Labels = 1,
		Range,
		ShowEdgeValues,
		StartFromZero,
		Title,
		Visible
	}

	[StiSerializable(StiSerializationVisibility.Class)]
	[TypeConverter(typeof(StiUniversalConverter))]
	[StiOrder(2)]
	public virtual StiChartAxisRange Range { get; set; } = new StiChartAxisRange();


	[StiSerializable(StiSerializationVisibility.Class)]
	[StiOrder(1)]
	public virtual StiChartAxisLabels Labels { get; set; } = new StiChartAxisLabels();


	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of axis.")]
	[StiOrder(6)]
	public virtual bool Visible { get; set; } = true;


	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			if (!ShouldSerializeLabels() && !ShouldSerializeRange())
			{
				return !ShouldSerializeVisible();
			}
			return false;
		}
	}

	public virtual object Clone()
	{
		StiChartAxis stiChartAxis = MemberwiseClone() as StiChartAxis;
		stiChartAxis.Labels = Labels?.Clone() as StiChartAxisLabels;
		stiChartAxis.Range = Range?.Clone() as StiChartAxisRange;
		return stiChartAxis;
	}

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		if (Labels != null)
		{
			jObject.AddPropertyJObject("Labels", Labels.SaveToJsonObject(mode));
		}
		if (Range != null)
		{
			jObject.AddPropertyJObject("Range", Range.SaveToJsonObject(mode));
		}
		jObject.AddPropertyBool("Visible", Visible, defaultValue: true);
		return jObject;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Labels":
				Labels.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Range":
				Range.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Visible":
				Visible = item.DeserializeBool();
				break;
			}
		}
	}

	protected virtual bool ShouldSerializeRange()
	{
		if (Range != null)
		{
			return !Range.IsDefault;
		}
		return true;
	}

	protected virtual bool ShouldSerializeLabels()
	{
		if (Labels != null)
		{
			return !Labels.IsDefault;
		}
		return true;
	}

	protected virtual bool ShouldSerializeVisible()
	{
		return !Visible;
	}

	public StiChartAxis()
	{
	}

	public StiChartAxis(StiChartAxisLabels labels)
	{
		Labels = labels;
	}

	public StiChartAxis(StiChartAxisLabels labels, bool visible)
	{
		Labels = labels;
		Visible = visible;
	}

	public StiChartAxis(StiChartAxisLabels labels, StiChartAxisRange range, bool visible)
	{
		Labels = labels;
		Range = range;
		Visible = visible;
	}
}
