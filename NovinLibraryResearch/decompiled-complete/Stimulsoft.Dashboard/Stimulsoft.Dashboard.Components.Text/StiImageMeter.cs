using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components.Text;

public class StiImageMeter : StiDimensionMeter, IStiValueMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.ImageMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.GetMain("Image");

	public StiImageMeter()
	{
	}

	public StiImageMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiImageMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
