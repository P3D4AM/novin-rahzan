using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal abstract class Constraint : MetadataObjectBase
{
	private readonly ITabular parent;

	public ITabular Parent => parent;

	public abstract ConstraintType Type { get; }

	protected Constraint(ITabular parent, string name)
		: base(name)
	{
		this.parent = parent;
	}
}
