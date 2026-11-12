namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableDefaultConstraint : IDefaultConstraint, IMetadataObject, IMutableMetadataObject
{
	new bool IsSystemNamed { get; set; }

	new string Text { get; set; }
}
