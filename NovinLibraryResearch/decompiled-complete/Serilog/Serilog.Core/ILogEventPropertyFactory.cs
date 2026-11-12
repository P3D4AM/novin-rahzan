using Serilog.Events;

namespace Serilog.Core;

public interface ILogEventPropertyFactory
{
	LogEventProperty CreateProperty(string name, object? value, bool destructureObjects = false);
}
