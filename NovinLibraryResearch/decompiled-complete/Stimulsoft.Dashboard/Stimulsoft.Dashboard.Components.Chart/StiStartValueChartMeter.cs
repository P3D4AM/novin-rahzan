using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Chart;

public class StiStartValueChartMeter : StiValueChartMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.StartValueChartMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "StartValue");

	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[DefaultValue(StiChartSeriesType.Gantt)]
	[TypeConverter(typeof(StiEnumConverter))]
	public override StiChartSeriesType SeriesType { get; set; } = StiChartSeriesType.Gantt;


	public StiStartValueChartMeter()
	{
	}

	public StiStartValueChartMeter(string expression, string label, StiChartSeriesType seriesType)
		: base(expression, label, seriesType)
	{
	}

	public StiStartValueChartMeter(string key, string expression, string label, StiChartSeriesType seriesType)
		: base(key, expression, label, seriesType)
	{
	}
}
