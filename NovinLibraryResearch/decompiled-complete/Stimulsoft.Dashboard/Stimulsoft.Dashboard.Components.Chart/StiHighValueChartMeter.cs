using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.Chart;

public class StiHighValueChartMeter : StiMeasureMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.HighValueChartMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "ValueHigh");

	public StiHighValueChartMeter()
	{
	}

	public StiHighValueChartMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiHighValueChartMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
