using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IConstraintFactory
{
	IForeignKeyColumn CreateForeignKeyColumn(IColumn referencingColumn, IColumn referencedColumn);

	IMutableCheckConstraint CreateCheckConstraint(ITabular parent, string name);

	IMutableDefaultConstraint CreateDefaultConstraint(IColumn parent, string name);

	IMutableForeignKeyConstraint CreateForeignKeyConstraint(ITable parent, string name);

	IPrimaryKeyConstraint CreatePrimaryKeyConstraint(ITabular parent, IRelationalIndex index);

	IUniqueConstraint CreateUniqueConstraint(ITabular parent, IRelationalIndex index);
}
