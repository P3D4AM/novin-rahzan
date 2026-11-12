using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.OnlineMap;

public class StiLongitudeMapMeter : StiDimensionMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.LongitudeMapMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Longitude");

	public StiLongitudeMapMeter()
	{
	}

	public StiLongitudeMapMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiLongitudeMapMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
