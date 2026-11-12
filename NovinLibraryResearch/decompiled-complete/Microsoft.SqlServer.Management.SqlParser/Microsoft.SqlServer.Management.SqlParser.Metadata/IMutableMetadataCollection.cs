using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableMetadataCollection<T> : IMetadataCollection<T>, IEnumerable<T>, IEnumerable where T : IMetadataObject
{
	new T this[string name] { get; set; }

	void Add(T item);

	void AddRange(IEnumerable<T> collection);

	bool Remove(string name);

	bool Remove(T item);

	void Clear();

	IMutableMetadataCollection<T> Clone();

	IMutableMetadataCollection<T> Clone(bool copyData);
}
