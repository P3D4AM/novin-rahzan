using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.Text;

public class StiTextMeter : StiDimensionMeter, IStiValueMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.TextMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Text");

	public StiTextMeter()
	{
	}

	public StiTextMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiTextMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
