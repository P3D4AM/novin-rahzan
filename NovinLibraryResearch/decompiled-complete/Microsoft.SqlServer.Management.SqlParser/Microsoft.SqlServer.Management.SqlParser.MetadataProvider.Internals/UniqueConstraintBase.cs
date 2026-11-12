using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal abstract class UniqueConstraintBase : Constraint, IUniqueConstraintBase, IConstraint, IMetadataObject
{
	private readonly IRelationalIndex m_index;

	public bool IsSystemNamed => m_index.IsSystemNamed;

	public IRelationalIndex AssociatedIndex => m_index;

	protected UniqueConstraintBase(ITabular parent, IRelationalIndex index)
		: base(parent, index.Name)
	{
		m_index = index;
	}
}
