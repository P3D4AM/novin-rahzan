using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal interface IBindingContext
{
	BatchBindingContext Batch { get; }

	BatchBindingContext AsBatchBindingContext { get; }

	LocalBindingContext AsLocalBindingContext { get; }

	CollationInfo DatabaseCollation { get; }

	CollationInfo ServerCollation { get; }

	IMetadataFactory MetadataFactory { get; }

	IMetadataCollection<ITabular> LocalTableExpressions { get; }

	IMetadataOrderedCollection<IColumn> LocalColumnExpressions { get; }

	LocalBindingContext CreateLocalBindingContext();

	ITableVariable FindLocalTableVariable(string name, LexicalScope lexicalScope);

	IScalarVariable FindLocalScalarVariable(string name, LexicalScope lexicalScope);

	ICursorVariable FindLocalCursorVariable(string name, LexicalScope lexicalScope);

	ILocalVariable FindLocalVariable(string name, LexicalScope lexicalScope);

	void OnBindingToSelfCte();

	ITabular FindCommonTableExpression(string name, out CteBindMode bindMode, out CteQueryFlags queryFlags);

	ITabular FindDmlTable(string name);

	IColumn AddLocalColumnExpression(IColumn expression);

	ITabular AddLocalTableExpression(ITabular expression);

	IScalar FindLocalScalarExpression(string name);

	ITabular FindLocalTableExpression(string databaseName, string schemaName, string name);

	IColumn FindColumn(string name, out bool isAmbiguous);

	IEnumerable<IColumn> FindAllColumns(string name);

	IMetadataCollection<IMetadataObject> FindAll(CompletionContext context, LexicalScope lexicalScope);

	IMetadataCollection<IMetadataObject> FindAll(List<string> ids, CompletionContext context, LexicalScope lexicalScope);

	void EnterTempBindMode(TemporaryBindMode mode);

	void LeaveTempBindMode(TemporaryBindMode mode);
}
