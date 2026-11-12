using Stimulsoft.Base.Json;
using Stimulsoft.Base.Meters;

namespace Stimulsoft.Dashboard.Components;

[JsonObject]
public abstract class StiMeasureMeter : StiMeter, IStiMeasureMeter, IStiMeter
{
	public StiMeasureMeter()
	{
	}

	public StiMeasureMeter(string expression, string label)
		: base(expression, label)
	{
	}

	public StiMeasureMeter(string key, string expression, string label)
		: base(key, expression, label)
	{
	}
}
