using System.Collections.Generic;

namespace Serilog.Sinks.Http.Private.Durable;

public class Batch
{
	public List<string> LogEvents { get; } = new List<string>();


	public bool HasReachedLimit { get; set; }
}
