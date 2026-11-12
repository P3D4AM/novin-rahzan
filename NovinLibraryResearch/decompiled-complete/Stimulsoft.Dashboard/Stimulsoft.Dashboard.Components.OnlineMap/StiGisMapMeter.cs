using System.ComponentModel;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Dashboard.Components.OnlineMap;

public class StiGisMapMeter : StiDimensionMeter
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.GisMapMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => "GIS";

	public StiGisMapMeter()
	{
	}

	public StiGisMapMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiGisMapMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
