using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Chart.Design;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Chart;

[TypeConverter(typeof(StiXChartAxisConverter))]
public class StiXChartAxis : StiChartAxis, IStiXChartAxis
{
	[DefaultValue(StiAutoBool.Auto)]
	[StiSerializable]
	[TypeConverter(typeof(StiAutoBoolConverter))]
	[Description("Gets or sets value which indicates that first and last arguments on axis will be shown anyway.")]
	[StiOrder(3)]
	public StiAutoBool ShowEdgeValues { get; set; }

	[StiSerializable]
	[DefaultValue(StiAutoBool.Auto)]
	[TypeConverter(typeof(StiAutoBoolConverter))]
	[Description("Gets or sets value which indicates that all arguments will be shows from zero.")]
	[StiOrder(4)]
	public virtual StiAutoBool StartFromZero { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[StiOrder(5)]
	public StiXChartAxisTitle Title { get; set; } = new StiXChartAxisTitle();


	[Browsable(false)]
	public override bool IsDefault
	{
		get
		{
			if (base.IsDefault && !ShouldSerializeTitle() && !ShouldSerializeShowEdgeValues())
			{
				return !ShouldSerializeStartFromZero();
			}
			return false;
		}
	}

	public override object Clone()
	{
		StiXChartAxis stiXChartAxis = base.Clone() as StiXChartAxis;
		stiXChartAxis.Title = Title?.Clone() as StiXChartAxisTitle;
		return stiXChartAxis;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		if (Title != null)
		{
			jObject.AddPropertyJObject("Title", Title.SaveToJsonObject(mode));
		}
		jObject.AddPropertyEnum("ShowEdgeValues", ShowEdgeValues, StiAutoBool.Auto);
		jObject.AddPropertyEnum("StartFromZero", StartFromZero, StiAutoBool.Auto);
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Title":
				Title.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ShowEdgeValues":
				ShowEdgeValues = item.DeserializeEnum<StiAutoBool>();
				break;
			case "StartFromZero":
				StartFromZero = item.DeserializeEnum<StiAutoBool>();
				break;
			}
		}
	}

	protected virtual bool ShouldSerializeShowEdgeValues()
	{
		return ShowEdgeValues != StiAutoBool.Auto;
	}

	protected virtual bool ShouldSerializeStartFromZero()
	{
		return StartFromZero != StiAutoBool.Auto;
	}

	protected bool ShouldSerializeTitle()
	{
		if (Title != null)
		{
			return !Title.IsDefault;
		}
		return true;
	}

	public StiXChartAxis()
	{
	}

	public StiXChartAxis(StiChartAxisLabels labels, StiXChartAxisTitle title)
		: base(labels)
	{
		Title = title;
	}

	public StiXChartAxis(StiChartAxisLabels labels, StiXChartAxisTitle title, bool visible)
		: base(labels, visible)
	{
		Title = title;
	}

	public StiXChartAxis(StiChartAxisLabels labels, StiChartAxisRange range, StiXChartAxisTitle title, bool visible, StiAutoBool startFromZero, StiAutoBool showEdgeValues)
		: base(labels, range, visible)
	{
		Title = title;
		StartFromZero = startFromZero;
		ShowEdgeValues = showEdgeValues;
	}
}
