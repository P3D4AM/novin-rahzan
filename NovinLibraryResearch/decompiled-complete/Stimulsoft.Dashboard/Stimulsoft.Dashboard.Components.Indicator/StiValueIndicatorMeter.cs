using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.Indicator;

public class StiValueIndicatorMeter : StiMeasureMeter, IStiValueMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.ValueIndicatorMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Value");

	public StiValueIndicatorMeter()
	{
	}

	public StiValueIndicatorMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiValueIndicatorMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
