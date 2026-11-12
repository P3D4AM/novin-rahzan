using System;
using System.Diagnostics.CodeAnalysis;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Policies;

internal class EnumScalarConversionPolicy : IScalarConversionPolicy
{
	public bool TryConvertToScalar(object value, [NotNullWhen(true)] out ScalarValue? result)
	{
		if (value is Enum)
		{
			result = new ScalarValue(value);
			return true;
		}
		result = null;
		return false;
	}
}
