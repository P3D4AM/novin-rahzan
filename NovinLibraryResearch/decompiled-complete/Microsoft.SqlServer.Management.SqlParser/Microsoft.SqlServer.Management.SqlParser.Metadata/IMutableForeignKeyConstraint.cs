namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableForeignKeyConstraint : IForeignKeyConstraint, IConstraint, IMetadataObject, IMutableConstraint, IMutableMetadataObject
{
	new IMutableMetadataOrderedCollection<IForeignKeyColumn> Columns { get; }

	new ForeignKeyAction DeleteAction { get; set; }

	new bool IsEnabled { get; set; }

	new bool IsChecked { get; set; }

	new bool NotForReplication { get; set; }

	new ITable ReferencedTable { get; set; }

	new ForeignKeyAction UpdateAction { get; set; }
}
