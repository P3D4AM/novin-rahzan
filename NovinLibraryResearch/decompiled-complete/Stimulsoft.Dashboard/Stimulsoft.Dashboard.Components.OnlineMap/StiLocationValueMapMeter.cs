using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.OnlineMap;

public class StiLocationValueMapMeter : StiDimensionMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.LocationValueMapMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Value");

	public StiLocationValueMapMeter()
	{
	}

	public StiLocationValueMapMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiLocationValueMapMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
