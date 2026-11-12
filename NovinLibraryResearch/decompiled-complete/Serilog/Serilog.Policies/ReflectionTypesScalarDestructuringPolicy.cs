using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Policies;

internal class ReflectionTypesScalarDestructuringPolicy : IDestructuringPolicy
{
	public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, [NotNullWhen(true)] out LogEventPropertyValue? result)
	{
		if ((value is Type || value is MemberInfo) ? true : false)
		{
			result = new ScalarValue(value);
			return true;
		}
		result = null;
		return false;
	}
}
