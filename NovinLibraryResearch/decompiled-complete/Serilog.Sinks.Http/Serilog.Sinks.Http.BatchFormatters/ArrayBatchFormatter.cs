using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Serilog.Sinks.Http.BatchFormatters;

public class ArrayBatchFormatter : IBatchFormatter
{
	public void Format(IEnumerable<string> logEvents, TextWriter output)
	{
		if (logEvents == null)
		{
			throw new ArgumentNullException("logEvents");
		}
		if (output == null)
		{
			throw new ArgumentNullException("output");
		}
		if (!logEvents.Any())
		{
			return;
		}
		output.Write("[");
		string value = string.Empty;
		foreach (string logEvent in logEvents)
		{
			if (!string.IsNullOrWhiteSpace(logEvent))
			{
				output.Write(value);
				output.Write(logEvent);
				value = ",";
			}
		}
		output.Write("]");
	}
}
