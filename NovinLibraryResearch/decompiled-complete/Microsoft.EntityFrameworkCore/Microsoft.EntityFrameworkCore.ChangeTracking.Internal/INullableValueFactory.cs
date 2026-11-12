using System.Collections.Generic;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public interface INullableValueFactory<TKey> : IDependentKeyValueFactory<TKey>
{
	IEqualityComparer<TKey> EqualityComparer { get; }
}
