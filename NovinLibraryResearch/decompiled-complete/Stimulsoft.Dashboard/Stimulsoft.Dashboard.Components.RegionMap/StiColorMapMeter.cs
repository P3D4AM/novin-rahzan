using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.RegionMap;

public class StiColorMapMeter : StiDimensionMeter, IStiColorMapMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.ColorMapMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Color");

	public StiColorMapMeter()
	{
	}

	public StiColorMapMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiColorMapMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
