using Stimulsoft.Base.Json;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components;

[JsonObject]
public abstract class StiDimensionMeter : StiMeter, IStiDimensionMeter, IStiMeter
{
	public StiDimensionMeter()
	{
	}

	public StiDimensionMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiDimensionMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
