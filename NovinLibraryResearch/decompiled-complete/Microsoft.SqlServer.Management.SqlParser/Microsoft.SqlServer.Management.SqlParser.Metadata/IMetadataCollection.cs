using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
public interface IMetadataCollection<T> : IEnumerable<T>, IEnumerable where T : IMetadataObject
{
	int Count { get; }

	T this[string name] { get; }

	IMetadataCollection<IMetadataObject> AsMetadataObjectCollection { get; }

	bool Contains(string name);

	bool Contains(T item);

	IEnumerable<T> FindAll(Predicate<T> match);

	IEnumerable<T> FindAll(string name);
}
