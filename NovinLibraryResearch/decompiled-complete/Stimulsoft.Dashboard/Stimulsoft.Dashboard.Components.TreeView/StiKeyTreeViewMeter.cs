using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.TreeView;

public class StiKeyTreeViewMeter : StiDimensionMeter, IStiValueMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.KeyTreeViewMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Key");

	public StiKeyTreeViewMeter()
	{
	}

	public StiKeyTreeViewMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiKeyTreeViewMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
