using System.Collections.Generic;

namespace Stimulsoft.Blockly.Model;

public class ProcedureContext : Context
{
	public IDictionary<string, object> Parameters { get; set; }

	public ProcedureContext()
	{
		Parameters = new Dictionary<string, object>();
	}
}
