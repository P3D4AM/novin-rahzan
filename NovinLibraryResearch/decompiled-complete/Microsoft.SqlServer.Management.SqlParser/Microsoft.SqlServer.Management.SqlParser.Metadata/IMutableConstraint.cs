namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableConstraint : IConstraint, IMetadataObject, IMutableMetadataObject
{
	new bool IsSystemNamed { get; set; }
}
