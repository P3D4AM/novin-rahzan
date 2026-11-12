using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.Chart;

public class StiXChartMeter : StiDimensionMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.XChartMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => "X" + Loc.Get("PropertyMain", "Value");

	public StiXChartMeter()
	{
	}

	public StiXChartMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiXChartMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
