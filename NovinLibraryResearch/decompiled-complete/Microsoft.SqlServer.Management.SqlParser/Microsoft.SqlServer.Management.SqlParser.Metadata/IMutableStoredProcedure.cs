namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableStoredProcedure : IStoredProcedure, ICallableModule, IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject, IMetadataObject, IFunctionModuleBase, IMutableCallableModule, IMutableUserDefinedFunctionModuleBase, IMutableSchemaOwnedObject, IMutableDatabaseObject, IMutableMetadataObject, IMutableFunctionModuleBase
{
	new string BodyText { get; set; }

	new bool ForReplication { get; set; }

	new bool IsRecompiled { get; set; }

	new bool IsSqlClr { get; set; }

	new bool Startup { get; set; }
}
