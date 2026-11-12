namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[Constant("Type", ConstraintType.Check)]
public interface ICheckConstraint : IConstraint, IMetadataObject
{
	[ReadWrite]
	bool IsEnabled { get; }

	[ReadWrite]
	bool IsChecked { get; }

	[ReadWrite]
	bool NotForReplication { get; }

	[ReadWrite]
	string Text { get; }
}
