using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.ComboBox;

public class StiKeyComboBoxMeter : StiDimensionMeter, IStiValueMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.KeyComboBoxMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Key");

	public StiKeyComboBoxMeter()
	{
	}

	public StiKeyComboBoxMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiKeyComboBoxMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
