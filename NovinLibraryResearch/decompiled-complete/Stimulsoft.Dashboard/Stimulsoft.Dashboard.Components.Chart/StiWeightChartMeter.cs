using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.Chart;

public class StiWeightChartMeter : StiMeasureMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.WeightChartMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Weight");

	public StiWeightChartMeter()
	{
	}

	public StiWeightChartMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiWeightChartMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
