using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.Gauge;

[JsonObject]
public class StiMaxGaugeMeter : StiMeasureMeter, IStiMaxGaugeMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.MaxGaugeMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => "Max";

	public StiMaxGaugeMeter()
	{
	}

	public StiMaxGaugeMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiMaxGaugeMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
