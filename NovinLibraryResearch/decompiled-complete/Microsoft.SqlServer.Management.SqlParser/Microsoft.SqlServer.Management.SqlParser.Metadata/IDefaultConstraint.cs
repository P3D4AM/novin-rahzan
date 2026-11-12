namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[ReadOnly("Name")]
public interface IDefaultConstraint : IMetadataObject
{
	[ReadOnly]
	[Parent]
	IColumn Parent { get; }

	[ReadWrite]
	bool IsSystemNamed { get; }

	[ReadWrite]
	string Text { get; }
}
