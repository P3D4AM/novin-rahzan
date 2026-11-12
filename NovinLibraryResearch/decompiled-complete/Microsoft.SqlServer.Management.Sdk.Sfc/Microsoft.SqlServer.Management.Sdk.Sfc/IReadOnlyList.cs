using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Sdk.Sfc;

public interface IReadOnlyList<T> : IReadOnlyCollection<T>, IReadOnlyCollection, IEnumerable, IEnumerable<T>
{
	T this[int index] { get; }
}
