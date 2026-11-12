using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.Indicator;

public class StiTargetIndicatorMeter : StiMeasureMeter, IStiTargetMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.TargetIndicatorMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Target");

	public StiTargetIndicatorMeter()
	{
	}

	public StiTargetIndicatorMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiTargetIndicatorMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
