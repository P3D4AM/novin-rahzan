using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class Schema : ISchema, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly int database;

	private readonly int extendedStoredProcedures;

	private readonly bool isSystemObject;

	private readonly string name;

	private readonly int owner;

	private readonly int scalarValuedFunctions;

	private readonly int storedProcedures;

	private readonly int synonyms;

	private readonly int tableValuedFunctions;

	private readonly int tables;

	private readonly int userDefinedAggregates;

	private readonly int userDefinedClrTypes;

	private readonly int userDefinedDataTypes;

	private readonly int userDefinedTableTypes;

	private readonly int views;

	IDatabasePrincipal ISchema.Owner => model.GetObjectById<IDatabasePrincipal>(owner);

	IMetadataCollection<ITable> ISchema.Tables => model.GetCollectionById<ITable>(tables);

	IMetadataCollection<IView> ISchema.Views => model.GetCollectionById<IView>(views);

	IMetadataCollection<IUserDefinedAggregate> ISchema.UserDefinedAggregates => model.GetCollectionById<IUserDefinedAggregate>(userDefinedAggregates);

	IMetadataCollection<ITableValuedFunction> ISchema.TableValuedFunctions => model.GetCollectionById<ITableValuedFunction>(tableValuedFunctions);

	IMetadataCollection<IScalarValuedFunction> ISchema.ScalarValuedFunctions => model.GetCollectionById<IScalarValuedFunction>(scalarValuedFunctions);

	IMetadataCollection<IStoredProcedure> ISchema.StoredProcedures => model.GetCollectionById<IStoredProcedure>(storedProcedures);

	IMetadataCollection<IExtendedStoredProcedure> ISchema.ExtendedStoredProcedures => model.GetCollectionById<IExtendedStoredProcedure>(extendedStoredProcedures);

	IMetadataCollection<IUserDefinedDataType> ISchema.UserDefinedDataTypes => model.GetCollectionById<IUserDefinedDataType>(userDefinedDataTypes);

	IMetadataCollection<IUserDefinedTableType> ISchema.UserDefinedTableTypes => model.GetCollectionById<IUserDefinedTableType>(userDefinedTableTypes);

	IMetadataCollection<IUserDefinedClrType> ISchema.UserDefinedClrTypes => model.GetCollectionById<IUserDefinedClrType>(userDefinedClrTypes);

	IMetadataCollection<ISynonym> ISchema.Synonyms => model.GetCollectionById<ISynonym>(synonyms);

	IDatabase IDatabaseOwnedObject.Database => model.GetObjectById<IDatabase>(database);

	IDatabaseObject IDatabaseObject.Parent => ((IDatabaseOwnedObject)this).Database;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	string IMetadataObject.Name => name;

	public Schema(DeserializationModel model, int database, int extendedStoredProcedures, bool isSystemObject, string name, int owner, int scalarValuedFunctions, int storedProcedures, int synonyms, int tableValuedFunctions, int tables, int userDefinedAggregates, int userDefinedClrTypes, int userDefinedDataTypes, int userDefinedTableTypes, int views)
	{
		this.model = model;
		this.database = database;
		this.extendedStoredProcedures = extendedStoredProcedures;
		this.isSystemObject = isSystemObject;
		this.name = name;
		this.owner = owner;
		this.scalarValuedFunctions = scalarValuedFunctions;
		this.storedProcedures = storedProcedures;
		this.synonyms = synonyms;
		this.tableValuedFunctions = tableValuedFunctions;
		this.tables = tables;
		this.userDefinedAggregates = userDefinedAggregates;
		this.userDefinedClrTypes = userDefinedClrTypes;
		this.userDefinedDataTypes = userDefinedDataTypes;
		this.userDefinedTableTypes = userDefinedTableTypes;
		this.views = views;
	}

	public Schema(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (int)values[1], (bool)values[2], (string)values[3], (int)values[4], (int)values[5], (int)values[6], (int)values[7], (int)values[8], (int)values[9], (int)values[10], (int)values[11], (int)values[12], (int)values[13], (int)values[14])
	{
	}

	T IDatabaseOwnedObject.Accept<T>(IDatabaseOwnedObjectVisitor<T> visitor)
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
		return ((IDatabaseOwnedObject)this).Accept((IDatabaseOwnedObjectVisitor<T>)visitor);
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return ((IDatabaseOwnedObject)this).Accept((IDatabaseOwnedObjectVisitor<T>)visitor);
	}
}
