using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class Table : ITable, ITableViewBase, IDatabaseTable, ITabular, IMetadataObject, ISchemaOwnedObject, IDatabaseObject
{
	private readonly DeserializationModel model;

	private readonly int columns;

	private readonly int constraints;

	private readonly int indexes;

	private readonly bool isQuotedIdentifierOn;

	private readonly bool isSystemObject;

	private readonly string name;

	private readonly int schema;

	private readonly int statistics;

	private readonly int triggers;

	IMetadataCollection<IDmlTrigger> ITableViewBase.Triggers => model.GetCollectionById<IDmlTrigger>(triggers);

	bool ITableViewBase.IsQuotedIdentifierOn => isQuotedIdentifierOn;

	CollationInfo IDatabaseTable.CollationInfo => ((ISchemaOwnedObject)this).Schema.Database.CollationInfo;

	IMetadataCollection<IConstraint> IDatabaseTable.Constraints => model.GetCollectionById<IConstraint>(constraints);

	IMetadataCollection<IIndex> IDatabaseTable.Indexes => model.GetCollectionById<IIndex>(indexes);

	IMetadataCollection<IStatistics> IDatabaseTable.Statistics => model.GetCollectionById<IStatistics>(statistics);

	TabularType ITabular.TabularType => TabularType.Table;

	IMetadataOrderedCollection<IColumn> ITabular.Columns => model.GetOrderedCollectionById<IColumn>(columns);

	ITabular ITabular.Unaliased => this;

	string IMetadataObject.Name => name;

	ISchema ISchemaOwnedObject.Schema => model.GetObjectById<ISchema>(schema);

	IDatabaseObject IDatabaseObject.Parent => ((ISchemaOwnedObject)this).Schema;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	public Table(DeserializationModel model, int columns, int constraints, int indexes, bool isQuotedIdentifierOn, bool isSystemObject, string name, int schema, int statistics, int triggers)
	{
		this.model = model;
		this.columns = columns;
		this.constraints = constraints;
		this.indexes = indexes;
		this.isQuotedIdentifierOn = isQuotedIdentifierOn;
		this.isSystemObject = isSystemObject;
		this.name = name;
		this.schema = schema;
		this.statistics = statistics;
		this.triggers = triggers;
	}

	public Table(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (int)values[1], (int)values[2], (bool)values[3], (bool)values[4], (string)values[5], (int)values[6], (int)values[7], (int)values[8])
	{
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return ((ISchemaOwnedObject)this).Accept((ISchemaOwnedObjectVisitor<T>)visitor);
	}

	T ISchemaOwnedObject.Accept<T>(ISchemaOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	T IDatabaseObject.Accept<T>(IDatabaseObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return ((ISchemaOwnedObject)this).Accept((ISchemaOwnedObjectVisitor<T>)visitor);
	}
}
