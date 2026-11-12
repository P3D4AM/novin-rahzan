using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.DatePicker;

public class StiValueDatePickerMeter : StiDimensionMeter, IStiValueMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.ValueDatePickerMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Value");

	public StiValueDatePickerMeter()
	{
	}

	public StiValueDatePickerMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiValueDatePickerMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
