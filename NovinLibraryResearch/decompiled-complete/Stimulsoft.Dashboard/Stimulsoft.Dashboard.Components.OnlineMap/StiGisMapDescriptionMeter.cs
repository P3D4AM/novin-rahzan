using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.OnlineMap;

public class StiGisMapDescriptionMeter : StiDimensionMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.GisMapDescriptionMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Description");

	public StiGisMapDescriptionMeter()
	{
	}

	public StiGisMapDescriptionMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiGisMapDescriptionMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
