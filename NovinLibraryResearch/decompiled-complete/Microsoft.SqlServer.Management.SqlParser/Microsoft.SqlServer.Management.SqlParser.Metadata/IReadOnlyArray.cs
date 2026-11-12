using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

internal interface IReadOnlyArray<T> : IEnumerable<T>, IEnumerable
{
	int Length { get; }

	T this[int index] { get; }
}
