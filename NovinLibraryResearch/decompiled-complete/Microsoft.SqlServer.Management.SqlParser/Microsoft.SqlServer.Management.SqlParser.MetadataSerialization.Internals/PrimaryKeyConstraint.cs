using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class PrimaryKeyConstraint : IPrimaryKeyConstraint, IUniqueConstraintBase, IConstraint, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly int associatedIndex;

	private readonly bool isSystemNamed;

	private readonly string name;

	private readonly int parent;

	IRelationalIndex IUniqueConstraintBase.AssociatedIndex => model.GetObjectById<IRelationalIndex>(associatedIndex);

	ITabular IConstraint.Parent => model.GetObjectById<ITabular>(parent);

	bool IConstraint.IsSystemNamed => isSystemNamed;

	ConstraintType IConstraint.Type => ConstraintType.PrimaryKey;

	string IMetadataObject.Name => name;

	public PrimaryKeyConstraint(DeserializationModel model, int associatedIndex, bool isSystemNamed, string name, int parent)
	{
		this.model = model;
		this.associatedIndex = associatedIndex;
		this.isSystemNamed = isSystemNamed;
		this.name = name;
		this.parent = parent;
	}

	public PrimaryKeyConstraint(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (bool)values[1], (string)values[2], (int)values[3])
	{
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
