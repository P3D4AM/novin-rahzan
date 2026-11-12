using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.RegionMap;

public class StiKeyMapMeter : StiDimensionMeter, IStiKeyMapMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.KeyMapMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Key");

	public StiKeyMapMeter()
	{
	}

	public StiKeyMapMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiKeyMapMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
