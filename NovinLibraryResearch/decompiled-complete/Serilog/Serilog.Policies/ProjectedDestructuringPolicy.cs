using System;
using System.Diagnostics.CodeAnalysis;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Policies;

internal class ProjectedDestructuringPolicy : IDestructuringPolicy
{
	private readonly Func<Type, bool> _canApply;

	private readonly Func<object, object> _projection;

	public ProjectedDestructuringPolicy(Func<Type, bool> canApply, Func<object, object> projection)
	{
		_canApply = Guard.AgainstNull(canApply, "canApply");
		_projection = Guard.AgainstNull(projection, "projection");
	}

	public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, [NotNullWhen(true)] out LogEventPropertyValue? result)
	{
		Guard.AgainstNull(value, "value");
		if (!_canApply(value.GetType()))
		{
			result = null;
			return false;
		}
		object value2 = _projection(value);
		result = propertyValueFactory.CreatePropertyValue(value2, destructureObjects: true);
		return true;
	}
}
