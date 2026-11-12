namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
public interface IUserDefinedFunction : IFunction, IMetadataObject, IFunctionModuleBase, IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject
{
	[ReadWrite]
	string BodyText { get; }

	[ReadWrite]
	bool IsSchemaBound { get; }

	[ReadWrite]
	bool IsSqlClr { get; }

	[ReadWrite]
	bool IsQuotedIdentifierOn { get; }
}
