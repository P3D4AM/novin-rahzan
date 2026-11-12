using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class UniqueConstraint : UniqueConstraintBase, IUniqueConstraint, IUniqueConstraintBase, IConstraint, IMetadataObject
{
	public override ConstraintType Type => ConstraintType.Unique;

	public UniqueConstraint(ITabular parent, IRelationalIndex index)
		: base(parent, index)
	{
	}

	public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
