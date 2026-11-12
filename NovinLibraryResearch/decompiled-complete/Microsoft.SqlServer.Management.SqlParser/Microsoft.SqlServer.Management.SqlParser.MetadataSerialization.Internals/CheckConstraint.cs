using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class CheckConstraint : ICheckConstraint, IConstraint, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly bool isChecked;

	private readonly bool isEnabled;

	private readonly bool isSystemNamed;

	private readonly string name;

	private readonly bool notForReplication;

	private readonly int parent;

	private readonly string text;

	bool ICheckConstraint.IsEnabled => isEnabled;

	bool ICheckConstraint.IsChecked => isChecked;

	bool ICheckConstraint.NotForReplication => notForReplication;

	string ICheckConstraint.Text => text;

	ITabular IConstraint.Parent => model.GetObjectById<ITabular>(parent);

	bool IConstraint.IsSystemNamed => isSystemNamed;

	ConstraintType IConstraint.Type => ConstraintType.Check;

	string IMetadataObject.Name => name;

	public CheckConstraint(DeserializationModel model, bool isChecked, bool isEnabled, bool isSystemNamed, string name, bool notForReplication, int parent, string text)
	{
		this.model = model;
		this.isChecked = isChecked;
		this.isEnabled = isEnabled;
		this.isSystemNamed = isSystemNamed;
		this.name = name;
		this.notForReplication = notForReplication;
		this.parent = parent;
		this.text = text;
	}

	public CheckConstraint(DeserializationModel model, object[] values)
		: this(model, (bool)values[0], (bool)values[1], (bool)values[2], (string)values[3], (bool)values[4], (int)values[5], (string)values[6])
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
