using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal interface IScopedMetadataCollection<T, MT> where T : class, IDatabaseObject where MT : class, T
{
	T this[string name, LexicalScope lexicalScope] { get; }

	IMetadataCollection<T> GlobalMetadata { get; }

	bool Contains(string name, LexicalScope lexicalScope);

	IMetadataCollection<IMetadataObject> GetMetadataCollection(LexicalScope lexicalScope);

	void Create(MT createdObject, IBindFinalizer finalizer, StatementBindMode bindMode);

	void Drop(T droppedObject, LexicalScope lexicalScope);

	void Alter(MT alteredObject, LexicalScope lexicalScope);

	MT AsMutableObject(T immutableObject);

	void ReplaceWithMutableObject(T immutableObject, MT mutableObject);
}
