using System.Diagnostics.CodeAnalysis;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Policies;

internal class PrimitiveScalarConversionPolicy : IScalarConversionPolicy
{
	public bool TryConvertToScalar(object value, [NotNullWhen(true)] out ScalarValue? result)
	{
		if (value.GetType().IsPrimitive)
		{
			result = new ScalarValue(value);
			return true;
		}
		result = null;
		return false;
	}
}
