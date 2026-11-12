using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal interface IAlterableScopedMetadataCollection<T, MT> : IScopedMetadataCollection<T, MT> where T : class, IDatabaseObject where MT : class, T
{
	MT GetAlterableObject(T immutableObject, LexicalScope lexicalScope, StatementBindMode bindMode);
}
