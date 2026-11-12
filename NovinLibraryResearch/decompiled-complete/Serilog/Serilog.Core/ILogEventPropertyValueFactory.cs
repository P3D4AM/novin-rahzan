using Serilog.Events;

namespace Serilog.Core;

public interface ILogEventPropertyValueFactory
{
	LogEventPropertyValue CreatePropertyValue(object? value, bool destructureObjects = false);
}
