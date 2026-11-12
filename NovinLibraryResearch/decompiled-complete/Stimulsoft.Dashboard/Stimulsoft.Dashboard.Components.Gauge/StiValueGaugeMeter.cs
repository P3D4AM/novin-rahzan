using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.Gauge;

[JsonObject]
public class StiValueGaugeMeter : StiMeasureMeter, IStiValueMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.ValueGaugeMeter;


	public override string LocalizedName => Loc.Get("PropertyMain", "Value");

	public StiValueGaugeMeter()
	{
	}

	public StiValueGaugeMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiValueGaugeMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
