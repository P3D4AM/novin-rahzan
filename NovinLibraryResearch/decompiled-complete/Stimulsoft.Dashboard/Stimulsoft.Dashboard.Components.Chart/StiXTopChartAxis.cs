using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Chart.Design;

namespace Stimulsoft.Dashboard.Components.Chart;

[TypeConverter(typeof(StiXTopChartAxisConverter))]
public class StiXTopChartAxis : StiXChartAxis
{
	[DefaultValue(false)]
	public override bool Visible { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	public override StiChartAxisRange Range { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	public override StiAutoBool StartFromZero { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.RemoveProperty("Range");
		jObject.RemoveProperty("Visible");
		jObject.AddPropertyBool("Visible", Visible);
		return jObject;
	}

	protected override bool ShouldSerializeVisible()
	{
		return Visible;
	}

	protected override bool ShouldSerializeRange()
	{
		return Range != null;
	}

	protected override bool ShouldSerializeStartFromZero()
	{
		return false;
	}

	public StiXTopChartAxis()
	{
		Range = null;
	}

	public StiXTopChartAxis(StiChartAxisLabels labels, StiXChartAxisTitle title, bool visible)
		: base(labels, title, visible)
	{
		Range = null;
	}

	public StiXTopChartAxis(StiChartAxisLabels labels, StiXChartAxisTitle title, bool visible, StiAutoBool showEdgeValues)
		: base(labels, null, title, visible, StiAutoBool.Auto, showEdgeValues)
	{
	}
}
