namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Value)]
[Constant("Name", null)]
public interface IExecutionContext : IMetadataObject
{
	[ReadWrite]
	ExecutionContextType ContextType { get; }

	[ReadWrite]
	[Reference]
	ILogin Login { get; }

	[ReadWrite]
	[Reference]
	IUser User { get; }
}
