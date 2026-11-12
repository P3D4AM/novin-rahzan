using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.OnlineMap;

public class StiLocationMapMeter : StiDimensionMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.LocationMapMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Location");

	public StiLocationMapMeter()
	{
	}

	public StiLocationMapMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiLocationMapMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
