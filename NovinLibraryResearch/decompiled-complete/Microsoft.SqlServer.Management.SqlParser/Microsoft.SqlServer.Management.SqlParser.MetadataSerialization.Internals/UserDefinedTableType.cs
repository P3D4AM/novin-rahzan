using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class UserDefinedTableType : IUserDefinedTableType, IUserDefinedType, IDataType, IMetadataObject, ISchemaOwnedObject, IDatabaseObject, ITableDataType, IDatabaseTable, ITabular
{
	private readonly DeserializationModel model;

	private readonly int columns;

	private readonly int constraints;

	private readonly int indexes;

	private readonly bool isSystemObject;

	private readonly string name;

	private readonly int schema;

	private readonly int statistics;

	bool IDataType.IsScalar => this is IScalarDataType;

	bool IDataType.IsTable => this != null;

	bool IDataType.IsCursor => this is ICursorDataType;

	bool IDataType.IsUnknown => false;

	string IMetadataObject.Name => name;

	ISchema ISchemaOwnedObject.Schema => model.GetObjectById<ISchema>(schema);

	IDatabaseObject IDatabaseObject.Parent => ((ISchemaOwnedObject)this).Schema;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	CollationInfo IDatabaseTable.CollationInfo => ((ISchemaOwnedObject)this).Schema.Database.CollationInfo;

	IMetadataCollection<IConstraint> IDatabaseTable.Constraints => model.GetCollectionById<IConstraint>(constraints);

	IMetadataCollection<IIndex> IDatabaseTable.Indexes => model.GetCollectionById<IIndex>(indexes);

	IMetadataCollection<IStatistics> IDatabaseTable.Statistics => model.GetCollectionById<IStatistics>(statistics);

	TabularType ITabular.TabularType => TabularType.TableDataType;

	IMetadataOrderedCollection<IColumn> ITabular.Columns => model.GetOrderedCollectionById<IColumn>(columns);

	ITabular ITabular.Unaliased => this;

	public UserDefinedTableType(DeserializationModel model, int columns, int constraints, int indexes, bool isSystemObject, string name, int schema, int statistics)
	{
		this.model = model;
		this.columns = columns;
		this.constraints = constraints;
		this.indexes = indexes;
		this.isSystemObject = isSystemObject;
		this.name = name;
		this.schema = schema;
		this.statistics = statistics;
	}

	public UserDefinedTableType(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (int)values[1], (int)values[2], (bool)values[3], (string)values[4], (int)values[5], (int)values[6])
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
