using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.ComboBox;

public class StiNameComboBoxMeter : StiDimensionMeter, IStiArgumentMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.NameComboBoxMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Name");

	public StiNameComboBoxMeter()
	{
	}

	public StiNameComboBoxMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiNameComboBoxMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
