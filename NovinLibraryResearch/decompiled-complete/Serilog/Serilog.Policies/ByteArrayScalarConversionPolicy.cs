using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Policies;

internal class ByteArrayScalarConversionPolicy : IScalarConversionPolicy
{
	private const int MaximumByteArrayLength = 1024;

	public bool TryConvertToScalar(object value, [NotNullWhen(true)] out ScalarValue? result)
	{
		if (!(value is byte[] array))
		{
			result = null;
			return false;
		}
		if (array.Length > 1024)
		{
			string value2 = string.Concat(string.Concat(from b in array.Take(16)
				select b.ToString("X2")), "... (", array.Length.ToString(), " bytes)");
			result = new ScalarValue(value2);
		}
		else
		{
			result = new ScalarValue(string.Concat(array.Select((byte b) => b.ToString("X2"))));
		}
		return true;
	}
}
