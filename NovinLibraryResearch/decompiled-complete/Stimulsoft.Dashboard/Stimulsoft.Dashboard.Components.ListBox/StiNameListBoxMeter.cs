using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.ListBox;

public class StiNameListBoxMeter : StiDimensionMeter, IStiArgumentMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.NameListBoxMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Name");

	public StiNameListBoxMeter()
	{
	}

	public StiNameListBoxMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiNameListBoxMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
