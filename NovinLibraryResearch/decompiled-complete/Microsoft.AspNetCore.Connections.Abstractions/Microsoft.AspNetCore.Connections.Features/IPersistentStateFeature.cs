using System.Collections.Generic;

namespace Microsoft.AspNetCore.Connections.Features;

public interface IPersistentStateFeature
{
	IDictionary<object, object?> State { get; }
}
