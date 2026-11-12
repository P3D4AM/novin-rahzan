using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.Gauge;

public class StiTargetGaugeMeter : StiMeasureMeter, IStiTargetMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.TargetGaugeMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Target");

	public StiTargetGaugeMeter()
	{
	}

	public StiTargetGaugeMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiTargetGaugeMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
