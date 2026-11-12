using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Policies;

internal class SimpleScalarConversionPolicy : IScalarConversionPolicy
{
	private readonly HashSet<Type> _scalarTypes;

	public SimpleScalarConversionPolicy(IEnumerable<Type> scalarTypes)
	{
		_scalarTypes = new HashSet<Type>(scalarTypes);
	}

	public bool TryConvertToScalar(object value, [NotNullWhen(true)] out ScalarValue? result)
	{
		if (_scalarTypes.Contains(value.GetType()))
		{
			result = new ScalarValue(value);
			return true;
		}
		result = null;
		return false;
	}
}
