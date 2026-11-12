using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.Chart;

public class StiCloseValueChartMeter : StiMeasureMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.CloseValueChartMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "ValueClose");

	public StiCloseValueChartMeter()
	{
	}

	public StiCloseValueChartMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiCloseValueChartMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
