using System.Collections.Generic;
using System.IO;

namespace Serilog.Sinks.Http;

public interface IBatchFormatter
{
	void Format(IEnumerable<string> logEvents, TextWriter output);
}
