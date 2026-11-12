using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal interface IDeserializationDataProvider
{
	int ObjectCount { get; }

	int CollectionCount { get; }

	DataRow<object> GetObjectData(int id);

	IMetadataObject GetExternalObjectById(int id);

	List<int> GetCollectionData(int id);
}
