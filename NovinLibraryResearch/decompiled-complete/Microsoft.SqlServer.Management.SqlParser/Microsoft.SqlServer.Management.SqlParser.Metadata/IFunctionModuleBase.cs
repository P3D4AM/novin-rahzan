namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
public interface IFunctionModuleBase : IMetadataObject
{
	[ReadWrite]
	IMetadataOrderedCollection<IParameter> Parameters { get; }
}
