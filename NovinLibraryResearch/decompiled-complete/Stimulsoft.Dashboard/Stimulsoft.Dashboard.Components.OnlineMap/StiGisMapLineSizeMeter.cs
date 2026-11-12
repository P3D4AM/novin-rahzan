using System.ComponentModel;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Dashboard.Components.OnlineMap;

public class StiGisMapLineSizeMeter : StiDimensionMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.GisMapLineSizeMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => "Line Size";

	public StiGisMapLineSizeMeter()
	{
	}

	public StiGisMapLineSizeMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiGisMapLineSizeMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
