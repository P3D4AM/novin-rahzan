using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Chart.Design;

namespace Stimulsoft.Dashboard.Components.Chart;

[TypeConverter(typeof(StiYRightChartAxisConverter))]
public class StiYRightChartAxis : StiYChartAxis
{
	[DefaultValue(false)]
	public override bool Visible { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	public override StiChartAxisRange Range { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[StiOrder(1)]
	public override StiChartAxisLabels Labels { get; set; } = new StiChartYRightAxisLabels();


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

	protected override bool ShouldSerializeLabels()
	{
		if (Labels != null)
		{
			return !Labels.IsDefault;
		}
		return true;
	}

	public StiYRightChartAxis()
	{
	}

	public StiYRightChartAxis(StiChartAxisLabels labels, StiYChartAxisTitle title, bool visible)
		: base(labels, title, visible)
	{
		Range = null;
	}

	public StiYRightChartAxis(StiChartAxisLabels labels, StiYChartAxisTitle title, bool visible, bool startFromZero)
		: base(labels, null, title, visible, startFromZero)
	{
	}
}
