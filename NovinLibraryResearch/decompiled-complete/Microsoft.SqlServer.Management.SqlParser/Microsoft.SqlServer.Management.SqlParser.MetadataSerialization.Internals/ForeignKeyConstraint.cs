using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class ForeignKeyConstraint : IForeignKeyConstraint, IConstraint, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly int columns;

	private readonly ForeignKeyAction deleteAction;

	private readonly bool isChecked;

	private readonly bool isEnabled;

	private readonly bool isSystemNamed;

	private readonly string name;

	private readonly bool notForReplication;

	private readonly int parent;

	private readonly int referencedTable;

	private readonly ForeignKeyAction updateAction;

	IMetadataOrderedCollection<IForeignKeyColumn> IForeignKeyConstraint.Columns => model.GetOrderedCollectionById<IForeignKeyColumn>(columns);

	ForeignKeyAction IForeignKeyConstraint.DeleteAction => deleteAction;

	bool IForeignKeyConstraint.IsEnabled => isEnabled;

	bool IForeignKeyConstraint.IsChecked => isChecked;

	bool IForeignKeyConstraint.NotForReplication => notForReplication;

	ITable IForeignKeyConstraint.ReferencedTable => model.GetObjectById<ITable>(referencedTable);

	ForeignKeyAction IForeignKeyConstraint.UpdateAction => updateAction;

	ITabular IConstraint.Parent => model.GetObjectById<ITabular>(parent);

	bool IConstraint.IsSystemNamed => isSystemNamed;

	ConstraintType IConstraint.Type => ConstraintType.ForeignKey;

	string IMetadataObject.Name => name;

	public ForeignKeyConstraint(DeserializationModel model, int columns, ForeignKeyAction deleteAction, bool isChecked, bool isEnabled, bool isSystemNamed, string name, bool notForReplication, int parent, int referencedTable, ForeignKeyAction updateAction)
	{
		this.model = model;
		this.columns = columns;
		this.deleteAction = deleteAction;
		this.isChecked = isChecked;
		this.isEnabled = isEnabled;
		this.isSystemNamed = isSystemNamed;
		this.name = name;
		this.notForReplication = notForReplication;
		this.parent = parent;
		this.referencedTable = referencedTable;
		this.updateAction = updateAction;
	}

	public ForeignKeyConstraint(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (ForeignKeyAction)values[1], (bool)values[2], (bool)values[3], (bool)values[4], (string)values[5], (bool)values[6], (int)values[7], (int)values[8], (ForeignKeyAction)values[9])
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
