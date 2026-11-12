using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.Chart;

public class StiLowValueChartMeter : StiMeasureMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.LowValueChartMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "ValueLow");

	public StiLowValueChartMeter()
	{
	}

	public StiLowValueChartMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiLowValueChartMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
