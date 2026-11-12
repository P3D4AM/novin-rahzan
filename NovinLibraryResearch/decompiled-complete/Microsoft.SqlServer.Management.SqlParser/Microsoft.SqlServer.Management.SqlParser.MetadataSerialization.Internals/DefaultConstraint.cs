using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class DefaultConstraint : IDefaultConstraint, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly bool isSystemNamed;

	private readonly string name;

	private readonly int parent;

	private readonly string text;

	IColumn IDefaultConstraint.Parent => model.GetObjectById<IColumn>(parent);

	bool IDefaultConstraint.IsSystemNamed => isSystemNamed;

	string IDefaultConstraint.Text => text;

	string IMetadataObject.Name => name;

	public DefaultConstraint(DeserializationModel model, bool isSystemNamed, string name, int parent, string text)
	{
		this.model = model;
		this.isSystemNamed = isSystemNamed;
		this.name = name;
		this.parent = parent;
		this.text = text;
	}

	public DefaultConstraint(DeserializationModel model, object[] values)
		: this(model, (bool)values[0], (string)values[1], (int)values[2], (string)values[3])
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
