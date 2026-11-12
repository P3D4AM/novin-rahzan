using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

internal interface IDropObjectBinderHelper<T, N> where T : class, IMetadataObject where N : SqlCodeObject
{
	ISqlCodeCollection<N> DropObjects { get; }

	SqlIdentifier GetObjectName(N dropObject);

	T FindDropMetadataObject(BatchBindingContext bindingContext, N name, LexicalScope lexicalScope);

	IMetadataObject FindAnyMetadataObject(BatchBindingContext bindingContext, N name, LexicalScope lexicalScope, out DdlTargetObjectType actualType);

	void ExecuteDrop(BatchBindingContext bindingContext, T dropObject, LexicalScope lexicalScope);
}
