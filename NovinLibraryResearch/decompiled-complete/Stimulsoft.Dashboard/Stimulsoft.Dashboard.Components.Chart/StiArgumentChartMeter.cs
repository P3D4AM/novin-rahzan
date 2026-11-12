using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.Chart;

public class StiArgumentChartMeter : StiDimensionMeter, IStiArgumentMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.ArgumentChartMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Argument");

	public StiArgumentChartMeter()
	{
	}

	public StiArgumentChartMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiArgumentChartMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
