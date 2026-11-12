using System;
using System.Diagnostics.CodeAnalysis;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Policies;

internal class DelegateDestructuringPolicy : IDestructuringPolicy
{
	public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, [NotNullWhen(true)] out LogEventPropertyValue? result)
	{
		if (value is Delegate @delegate)
		{
			result = new ScalarValue(@delegate.ToString());
			return true;
		}
		result = null;
		return false;
	}
}
