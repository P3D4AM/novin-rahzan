using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.OnlineMap;

public class StiLatitudeMapMeter : StiDimensionMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.LatitudeMapMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Latitude");

	public StiLatitudeMapMeter()
	{
	}

	public StiLatitudeMapMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiLatitudeMapMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
