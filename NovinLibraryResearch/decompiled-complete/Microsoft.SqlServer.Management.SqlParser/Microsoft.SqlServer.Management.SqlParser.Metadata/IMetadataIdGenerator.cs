namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

internal interface IMetadataIdGenerator<TId>
{
	TId GenerateId(IMetadataObject obj);
}
