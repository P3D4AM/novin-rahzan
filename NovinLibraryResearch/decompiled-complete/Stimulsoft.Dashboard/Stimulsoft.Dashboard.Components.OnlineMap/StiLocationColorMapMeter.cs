using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.OnlineMap;

public class StiLocationColorMapMeter : StiDimensionMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.LocationColorMapMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Group");

	public StiLocationColorMapMeter()
	{
	}

	public StiLocationColorMapMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiLocationColorMapMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
