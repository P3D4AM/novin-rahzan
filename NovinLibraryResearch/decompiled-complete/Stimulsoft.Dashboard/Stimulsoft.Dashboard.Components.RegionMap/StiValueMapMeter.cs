using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.RegionMap;

public class StiValueMapMeter : StiMeasureMeter, IStiValueMapMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.ValueMapMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Value");

	public StiValueMapMeter()
	{
	}

	public StiValueMapMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiValueMapMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
