namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
public interface ITrigger : IMetadataObject
{
	[ReadWrite]
	string BodyText { get; }

	[ReadWrite]
	bool IsEncrypted { get; }

	[ReadWrite]
	bool IsEnabled { get; }

	[ReadWrite]
	bool IsSqlClr { get; }

	[ReadWrite]
	IExecutionContext ExecutionContext { get; }
}
