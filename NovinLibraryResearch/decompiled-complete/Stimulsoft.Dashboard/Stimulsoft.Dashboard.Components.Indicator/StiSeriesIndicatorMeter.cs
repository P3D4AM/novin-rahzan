using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.Indicator;

public class StiSeriesIndicatorMeter : StiDimensionMeter, IStiSeriesMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.SeriesIndicatorMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Series");

	public StiSeriesIndicatorMeter()
	{
	}

	public StiSeriesIndicatorMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiSeriesIndicatorMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
