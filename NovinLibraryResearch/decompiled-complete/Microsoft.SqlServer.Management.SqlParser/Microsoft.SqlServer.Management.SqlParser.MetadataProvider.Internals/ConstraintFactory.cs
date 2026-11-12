using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class ConstraintFactory : ObjectFactoryBase<ConstraintFactory>, IConstraintFactory
{
	public IForeignKeyColumn CreateForeignKeyColumn(IColumn referencingColumn, IColumn referencedColumn)
	{
		if (referencingColumn == null)
		{
			throw new ArgumentNullException("referencingColumn");
		}
		if (referencedColumn == null)
		{
			throw new ArgumentNullException("referencedColumn");
		}
		return new ForeignKeyColumn(referencingColumn, referencedColumn);
	}

	public IMutableCheckConstraint CreateCheckConstraint(ITabular parent, string name)
	{
		if (parent == null)
		{
			throw new ArgumentNullException("parent");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new CheckConstraint(parent, name);
	}

	public IMutableDefaultConstraint CreateDefaultConstraint(IColumn parent, string name)
	{
		if (parent == null)
		{
			throw new ArgumentNullException("parent");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new DefaultConstraint(parent, name);
	}

	public IMutableForeignKeyConstraint CreateForeignKeyConstraint(ITable parent, string name)
	{
		if (parent == null)
		{
			throw new ArgumentNullException("parent");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new ForeignKeyConstraint(parent, name);
	}

	public IPrimaryKeyConstraint CreatePrimaryKeyConstraint(ITabular parent, IRelationalIndex index)
	{
		if (parent == null)
		{
			throw new ArgumentNullException("parent");
		}
		if (index == null)
		{
			throw new ArgumentNullException("index");
		}
		return new PrimaryKeyConstraint(parent, index);
	}

	public IUniqueConstraint CreateUniqueConstraint(ITabular parent, IRelationalIndex index)
	{
		if (parent == null)
		{
			throw new ArgumentNullException("parent");
		}
		if (index == null)
		{
			throw new ArgumentNullException("index");
		}
		return new UniqueConstraint(parent, index);
	}
}
