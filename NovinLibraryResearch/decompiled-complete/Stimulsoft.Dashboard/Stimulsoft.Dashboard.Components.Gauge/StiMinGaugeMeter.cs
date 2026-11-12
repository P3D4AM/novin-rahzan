using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.Gauge;

[JsonObject]
public class StiMinGaugeMeter : StiMeasureMeter, IStiMinGaugeMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.MinGaugeMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => "Min";

	public StiMinGaugeMeter()
	{
	}

	public StiMinGaugeMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiMinGaugeMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
