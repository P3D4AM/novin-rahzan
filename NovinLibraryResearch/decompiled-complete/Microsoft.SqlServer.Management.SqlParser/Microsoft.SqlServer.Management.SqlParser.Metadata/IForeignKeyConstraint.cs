namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[Constant("Type", ConstraintType.ForeignKey)]
public interface IForeignKeyConstraint : IConstraint, IMetadataObject
{
	[ReadWrite]
	IMetadataOrderedCollection<IForeignKeyColumn> Columns { get; }

	[ReadWrite]
	ForeignKeyAction DeleteAction { get; }

	[ReadWrite]
	bool IsEnabled { get; }

	[ReadWrite]
	bool IsChecked { get; }

	[ReadWrite]
	bool NotForReplication { get; }

	[ReadWrite]
	[Reference]
	ITable ReferencedTable { get; }

	[ReadWrite]
	ForeignKeyAction UpdateAction { get; }
}
