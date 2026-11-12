using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.OnlineMap;

public class StiGisMapColorMeter : StiDimensionMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.GisMapColorMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Color");

	public StiGisMapColorMeter()
	{
	}

	public StiGisMapColorMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiGisMapColorMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
