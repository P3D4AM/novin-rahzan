using System.Collections.Generic;

namespace Serilog.Exceptions.Core;

public interface IExceptionPropertiesBag
{
	IReadOnlyDictionary<string, object?> GetResultDictionary();

	void AddProperty(string key, object? value);

	bool ContainsProperty(string key);
}
