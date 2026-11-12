using System.Collections.Concurrent;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal class PropertyBagContents
{
	public readonly ConcurrentDictionary<string, string> StringProperties = new ConcurrentDictionary<string, string>();

	public readonly ConcurrentDictionary<string, int> IntProperties = new ConcurrentDictionary<string, int>();

	public readonly ConcurrentDictionary<string, long> Int64Properties = new ConcurrentDictionary<string, long>();

	public readonly ConcurrentDictionary<string, bool> BoolProperties = new ConcurrentDictionary<string, bool>();

	public EventType EventType { get; }

	public PropertyBagContents(EventType eventType)
	{
		EventType = eventType;
	}
}
