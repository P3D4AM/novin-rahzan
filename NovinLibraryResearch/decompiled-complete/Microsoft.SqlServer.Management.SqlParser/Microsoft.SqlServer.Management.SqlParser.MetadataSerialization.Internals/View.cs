using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class View : IView, ITableViewBase, IDatabaseTable, ITabular, IMetadataObject, ISchemaOwnedObject, IDatabaseObject
{
	private readonly DeserializationModel model;

	private readonly int columns;

	private readonly int constraints;

	private readonly bool hasCheckOption;

	private readonly bool hasColumnSpecification;

	private readonly int indexes;

	private readonly bool isEncrypted;

	private readonly bool isQuotedIdentifierOn;

	private readonly bool isSchemaBound;

	private readonly bool isSystemObject;

	private readonly string name;

	private readonly string queryText;

	private readonly bool returnsViewMetadata;

	private readonly int schema;

	private readonly int statistics;

	private readonly int triggers;

	bool IView.IsEncrypted => isEncrypted;

	bool IView.IsSchemaBound => isSchemaBound;

	string IView.QueryText => queryText;

	bool IView.ReturnsViewMetadata => returnsViewMetadata;

	bool IView.HasCheckOption => hasCheckOption;

	bool IView.HasColumnSpecification => hasColumnSpecification;

	IMetadataCollection<IDmlTrigger> ITableViewBase.Triggers => model.GetCollectionById<IDmlTrigger>(triggers);

	bool ITableViewBase.IsQuotedIdentifierOn => isQuotedIdentifierOn;

	CollationInfo IDatabaseTable.CollationInfo => ((ISchemaOwnedObject)this).Schema.Database.CollationInfo;

	IMetadataCollection<IConstraint> IDatabaseTable.Constraints => model.GetCollectionById<IConstraint>(constraints);

	IMetadataCollection<IIndex> IDatabaseTable.Indexes => model.GetCollectionById<IIndex>(indexes);

	IMetadataCollection<IStatistics> IDatabaseTable.Statistics => model.GetCollectionById<IStatistics>(statistics);

	TabularType ITabular.TabularType => TabularType.View;

	IMetadataOrderedCollection<IColumn> ITabular.Columns => model.GetOrderedCollectionById<IColumn>(columns);

	ITabular ITabular.Unaliased => this;

	string IMetadataObject.Name => name;

	ISchema ISchemaOwnedObject.Schema => model.GetObjectById<ISchema>(schema);

	IDatabaseObject IDatabaseObject.Parent => ((ISchemaOwnedObject)this).Schema;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	public View(DeserializationModel model, int columns, int constraints, bool hasCheckOption, bool hasColumnSpecification, int indexes, bool isEncrypted, bool isQuotedIdentifierOn, bool isSchemaBound, bool isSystemObject, string name, string queryText, bool returnsViewMetadata, int schema, int statistics, int triggers)
	{
		this.model = model;
		this.columns = columns;
		this.constraints = constraints;
		this.hasCheckOption = hasCheckOption;
		this.hasColumnSpecification = hasColumnSpecification;
		this.indexes = indexes;
		this.isEncrypted = isEncrypted;
		this.isQuotedIdentifierOn = isQuotedIdentifierOn;
		this.isSchemaBound = isSchemaBound;
		this.isSystemObject = isSystemObject;
		this.name = name;
		this.queryText = queryText;
		this.returnsViewMetadata = returnsViewMetadata;
		this.schema = schema;
		this.statistics = statistics;
		this.triggers = triggers;
	}

	public View(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (int)values[1], (bool)values[2], (bool)values[3], (int)values[4], (bool)values[5], (bool)values[6], (bool)values[7], (bool)values[8], (string)values[9], (string)values[10], (bool)values[11], (int)values[12], (int)values[13], (int)values[14])
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
