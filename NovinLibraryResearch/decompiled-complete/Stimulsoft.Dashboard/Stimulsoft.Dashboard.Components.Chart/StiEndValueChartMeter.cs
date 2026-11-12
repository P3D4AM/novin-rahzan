using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.Chart;

public class StiEndValueChartMeter : StiMeasureMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.EndValueChartMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "ValueEnd");

	public StiEndValueChartMeter()
	{
	}

	public StiEndValueChartMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiEndValueChartMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
