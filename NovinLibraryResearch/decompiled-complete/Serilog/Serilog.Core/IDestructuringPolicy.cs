using System.Diagnostics.CodeAnalysis;
using Serilog.Events;

namespace Serilog.Core;

public interface IDestructuringPolicy
{
	bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, [NotNullWhen(true)] out LogEventPropertyValue? result);
}
