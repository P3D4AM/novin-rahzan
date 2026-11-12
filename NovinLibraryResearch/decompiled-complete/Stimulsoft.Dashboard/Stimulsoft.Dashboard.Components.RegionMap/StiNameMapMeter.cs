using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.RegionMap;

public class StiNameMapMeter : StiDimensionMeter, IStiNameMapMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.NameMapMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Name");

	public StiNameMapMeter()
	{
	}

	public StiNameMapMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiNameMapMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
