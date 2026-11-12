using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.Progress;

public class StiValueProgressMeter : StiMeasureMeter, IStiValueMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.ValueProgressMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Value");

	public StiValueProgressMeter()
	{
	}

	public StiValueProgressMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiValueProgressMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
