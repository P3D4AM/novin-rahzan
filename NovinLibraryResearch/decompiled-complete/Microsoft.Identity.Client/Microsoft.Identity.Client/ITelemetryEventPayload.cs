using System.Collections.Generic;

namespace Microsoft.Identity.Client;

public interface ITelemetryEventPayload
{
	string Name { get; }

	IReadOnlyDictionary<string, bool> BoolValues { get; }

	IReadOnlyDictionary<string, long> Int64Values { get; }

	IReadOnlyDictionary<string, int> IntValues { get; }

	IReadOnlyDictionary<string, string> StringValues { get; }

	string ToJsonString();
}
