using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

internal interface IDropObjectStatement<T, N> where T : class, IMetadataObject where N : SqlCodeObject
{
	ISqlCodeCollection<N> DropObjects { get; }

	T FindMetadataObject(BatchBindingContext bindingContext, N name, LexicalScope lexicalScope);

	void ExecuteDrop(BatchBindingContext bindingContext, T dropObject, LexicalScope lexicalScope);
}
