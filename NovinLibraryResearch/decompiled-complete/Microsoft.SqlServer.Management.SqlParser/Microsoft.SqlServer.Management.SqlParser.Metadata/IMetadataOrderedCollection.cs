using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
public interface IMetadataOrderedCollection<T> : IMetadataCollection<T>, IEnumerable<T>, IEnumerable where T : IMetadataObject
{
	T this[int index] { get; }
}
