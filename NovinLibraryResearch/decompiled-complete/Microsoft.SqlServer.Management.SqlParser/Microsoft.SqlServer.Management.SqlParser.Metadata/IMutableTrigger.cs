namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableTrigger : ITrigger, IMetadataObject, IMutableMetadataObject
{
	new string BodyText { get; set; }

	new bool IsEncrypted { get; set; }

	[ReadWrite]
	new bool IsEnabled { get; set; }

	[ReadWrite]
	new bool IsSqlClr { get; set; }

	new IExecutionContext ExecutionContext { get; set; }
}
