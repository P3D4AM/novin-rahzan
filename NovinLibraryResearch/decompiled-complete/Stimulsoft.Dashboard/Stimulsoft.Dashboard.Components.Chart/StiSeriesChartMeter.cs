using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.Chart;

public class StiSeriesChartMeter : StiDimensionMeter, IStiSeriesMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.SeriesChartMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Series");

	public StiSeriesChartMeter()
	{
	}

	public StiSeriesChartMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiSeriesChartMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
