using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.ListBox;

public class StiKeyListBoxMeter : StiDimensionMeter, IStiValueMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.KeyListBoxMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Key");

	public StiKeyListBoxMeter()
	{
	}

	public StiKeyListBoxMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiKeyListBoxMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
