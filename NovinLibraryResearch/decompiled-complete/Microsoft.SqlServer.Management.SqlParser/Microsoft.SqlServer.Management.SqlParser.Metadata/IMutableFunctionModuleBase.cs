namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableFunctionModuleBase : IFunctionModuleBase, IMetadataObject, IMutableMetadataObject
{
	new IMutableMetadataOrderedCollection<IParameter> Parameters { get; }
}
