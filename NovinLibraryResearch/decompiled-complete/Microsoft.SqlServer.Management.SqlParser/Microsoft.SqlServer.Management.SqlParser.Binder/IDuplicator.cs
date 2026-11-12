using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal interface IDuplicator<T, MT> where T : class, IMetadataObject where MT : class, T
{
	MT Duplicate(T obj);
}
