namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IVirtualTable : ITabular, IMetadataObject
{
	ITabular TargetTable { get; }
}
