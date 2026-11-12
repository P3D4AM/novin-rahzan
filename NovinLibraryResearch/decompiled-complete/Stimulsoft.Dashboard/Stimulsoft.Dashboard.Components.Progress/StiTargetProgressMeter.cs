using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.Progress;

public class StiTargetProgressMeter : StiMeasureMeter, IStiTargetMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.TargetProgressMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Target");

	public StiTargetProgressMeter()
	{
	}

	public StiTargetProgressMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiTargetProgressMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
