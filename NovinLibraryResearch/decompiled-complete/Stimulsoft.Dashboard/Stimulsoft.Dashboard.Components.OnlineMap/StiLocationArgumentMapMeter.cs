using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.OnlineMap;

public class StiLocationArgumentMapMeter : StiDimensionMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.LocationArgumentMapMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Argument");

	public StiLocationArgumentMapMeter()
	{
	}

	public StiLocationArgumentMapMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiLocationArgumentMapMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
