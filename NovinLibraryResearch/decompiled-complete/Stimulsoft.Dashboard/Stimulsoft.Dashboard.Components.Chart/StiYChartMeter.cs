using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.Chart;

public class StiYChartMeter : StiDimensionMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.YChartMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => "Y" + Loc.Get("PropertyMain", "Value");

	public StiYChartMeter()
	{
	}

	public StiYChartMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiYChartMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
