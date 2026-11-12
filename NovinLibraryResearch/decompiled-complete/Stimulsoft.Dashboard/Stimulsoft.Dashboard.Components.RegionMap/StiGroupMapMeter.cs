using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.RegionMap;

public class StiGroupMapMeter : StiDimensionMeter, IStiGroupMapMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.GroupMapMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Group");

	public StiGroupMapMeter()
	{
	}

	public StiGroupMapMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiGroupMapMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
