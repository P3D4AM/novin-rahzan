using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.Gauge;

[JsonObject]
public class StiSeriesGaugeMeter : StiDimensionMeter, IStiSeriesMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.SeriesGaugeMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Series");

	public StiSeriesGaugeMeter()
	{
	}

	public StiSeriesGaugeMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiSeriesGaugeMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
