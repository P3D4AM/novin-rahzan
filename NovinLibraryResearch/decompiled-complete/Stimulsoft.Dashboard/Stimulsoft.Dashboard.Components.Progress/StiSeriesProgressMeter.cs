using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.Progress;

public class StiSeriesProgressMeter : StiDimensionMeter, IStiSeriesMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.SeriesProgressMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Series");

	public StiSeriesProgressMeter()
	{
	}

	public StiSeriesProgressMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiSeriesProgressMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
