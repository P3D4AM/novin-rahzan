using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Chart;

public class StiOpenValueChartMeter : StiValueChartMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.OpenValueChartMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "ValueOpen");

	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[DefaultValue(StiChartSeriesType.ClusteredColumn)]
	[TypeConverter(typeof(StiEnumConverter))]
	public override StiChartSeriesType SeriesType { get; set; } = StiChartSeriesType.Candlestick;


	public StiOpenValueChartMeter()
	{
	}

	public StiOpenValueChartMeter(string expression, string label, StiChartSeriesType seriesType)
		: base(expression, label, seriesType)
	{
	}

	public StiOpenValueChartMeter(string key, string expression, string label, StiChartSeriesType seriesType)
		: base(key, expression, label, seriesType)
	{
	}
}
