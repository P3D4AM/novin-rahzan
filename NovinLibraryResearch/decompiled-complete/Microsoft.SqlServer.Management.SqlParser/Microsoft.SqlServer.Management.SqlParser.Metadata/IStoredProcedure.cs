namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[Constant("ModuleType", CallableModuleType.StoredProcedure)]
[Constant("ReturnType", SpecialValue.IntDataType)]
public interface IStoredProcedure : ICallableModule, IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, IFunctionModuleBase
{
	[ReadWrite]
	string BodyText { get; }

	[ReadWrite]
	bool ForReplication { get; }

	[ReadWrite]
	bool IsRecompiled { get; }

	[ReadWrite]
	bool IsSqlClr { get; }

	[ReadWrite]
	bool Startup { get; }

	[ReadWrite]
	bool IsQuotedIdentifierOn { get; }
}
