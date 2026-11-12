using System;
using System.Collections.Generic;
using Stimulsoft.Report;

namespace Stimulsoft.Blockly.Model;

public class Context
{
	public IDictionary<string, object> Variables { get; set; }

	public IDictionary<string, object> Functions { get; set; }

	public EscapeMode EscapeMode { get; set; }

	public Context Parent { get; set; }

	public StiReport Report { get; set; }

	public object Sender { get; set; }

	public EventArgs EventArgs { get; set; }

	public Context()
	{
		Variables = new Dictionary<string, object>();
		Functions = new Dictionary<string, object>();
	}
}
