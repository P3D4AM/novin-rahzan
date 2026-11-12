using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.TreeViewBox;

public class StiKeyTreeViewBoxMeter : StiDimensionMeter, IStiValueMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.KeyTreeViewBoxMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Key");

	public StiKeyTreeViewBoxMeter()
	{
	}

	public StiKeyTreeViewBoxMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiKeyTreeViewBoxMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
