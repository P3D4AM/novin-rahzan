using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Chart.Design;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Chart;

[TypeConverter(typeof(StiYChartAxisConverter))]
public class StiYChartAxis : StiChartAxis, IStiYChartAxis
{
	[StiSerializable(StiSerializationVisibility.Class)]
	[TypeConverter(typeof(StiUniversalConverter))]
	[StiOrder(5)]
	public StiYChartAxisTitle Title { get; set; } = new StiYChartAxisTitle();


	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that all arguments will be shows from zero.")]
	[StiOrder(4)]
	public virtual bool StartFromZero { get; set; } = true;


	[Browsable(false)]
	public override bool IsDefault
	{
		get
		{
			if (base.IsDefault && !ShouldSerializeTitle())
			{
				return !ShouldSerializeStartFromZero();
			}
			return false;
		}
	}

	public override object Clone()
	{
		StiYChartAxis stiYChartAxis = base.Clone() as StiYChartAxis;
		stiYChartAxis.Title = Title?.Clone() as StiYChartAxisTitle;
		stiYChartAxis.Range = Range?.Clone() as StiChartAxisRange;
		return stiYChartAxis;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		if (Title != null)
		{
			jObject.AddPropertyJObject("Title", Title.SaveToJsonObject(mode));
		}
		jObject.AddPropertyBool("StartFromZero", StartFromZero, defaultValue: true);
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "Title"))
			{
				if (name == "StartFromZero")
				{
					StartFromZero = item.DeserializeBool();
				}
			}
			else
			{
				Title.LoadFromJsonObject((JObject)item.Value);
			}
		}
	}

	protected bool ShouldSerializeTitle()
	{
		if (Title != null)
		{
			return !Title.IsDefault;
		}
		return true;
	}

	protected virtual bool ShouldSerializeStartFromZero()
	{
		return !StartFromZero;
	}

	public StiYChartAxis()
	{
	}

	public StiYChartAxis(StiChartAxisLabels labels, StiYChartAxisTitle title)
		: base(labels)
	{
		Title = title;
	}

	public StiYChartAxis(StiChartAxisLabels labels, StiYChartAxisTitle title, bool visible)
		: base(labels, visible)
	{
		Title = title;
	}

	public StiYChartAxis(StiChartAxisLabels labels, StiChartAxisRange range, StiYChartAxisTitle title, bool visible, bool startFromZero)
		: base(labels, range, visible)
	{
		Title = title;
		StartFromZero = startFromZero;
	}
}
