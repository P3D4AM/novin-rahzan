using System.Diagnostics.CodeAnalysis;
using Serilog.Events;

namespace Serilog.Core;

internal interface IScalarConversionPolicy
{
	bool TryConvertToScalar(object value, [NotNullWhen(true)] out ScalarValue? result);
}
