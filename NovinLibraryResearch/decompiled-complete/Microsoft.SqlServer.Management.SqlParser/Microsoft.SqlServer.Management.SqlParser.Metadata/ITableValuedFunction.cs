namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[Constant("TabularType", TabularType.TableValuedFunction)]
[Duplicate("CollationInfo", "Schema.Database.CollationInfo")]
public interface ITableValuedFunction : IDatabaseTable, ITabular, IMetadataObject, IUserDefinedFunction, IFunction, IFunctionModuleBase, IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject
{
	[ReadWrite]
	bool IsInline { get; }

	[ReadWrite]
	string TableVariableName { get; }
}
