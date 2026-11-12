using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class Schema : DatabaseOwnedObject, IMutableSchema, ISchema, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject, IMutableDatabaseOwnedObject, IMutableDatabaseObject, IMutableMetadataObject
{
	private readonly ExtendedStoredProcedureCollection extendedStoredProcedures;

	private readonly ScalarValuedFunctionCollection scalarValuedFunctions;

	private readonly StoredProcedureCollection storedProcedures;

	private readonly SynonymCollection synonyms;

	private readonly TableCollection tables;

	private readonly TableValuedFunctionCollection tableValuedFunctions;

	private readonly UserDefinedAggregateCollection userDefinedAggregates;

	private readonly UserDefinedDataTypeCollection userDefinedDataTypes;

	private readonly UserDefinedTableTypeCollection userDefinedTableTypes;

	private readonly UserDefinedClrTypeCollection userDefinedClrTypes;

	private readonly ViewCollection views;

	private IDatabasePrincipal owner;

	public IDatabasePrincipal Owner
	{
		get
		{
			return owner;
		}
		set
		{
			owner = value;
		}
	}

	public IMutableMetadataCollection<ITable> Tables => tables;

	public IMutableMetadataCollection<IView> Views => views;

	public IMutableMetadataCollection<IUserDefinedAggregate> UserDefinedAggregates => userDefinedAggregates;

	public IMutableMetadataCollection<ITableValuedFunction> TableValuedFunctions => tableValuedFunctions;

	public IMutableMetadataCollection<IScalarValuedFunction> ScalarValuedFunctions => scalarValuedFunctions;

	public IMutableMetadataCollection<IStoredProcedure> StoredProcedures => storedProcedures;

	public IMutableMetadataCollection<ISynonym> Synonyms => synonyms;

	public IMutableMetadataCollection<IExtendedStoredProcedure> ExtendedStoredProcedures => extendedStoredProcedures;

	public IMutableMetadataCollection<IUserDefinedDataType> UserDefinedDataTypes => userDefinedDataTypes;

	public IMutableMetadataCollection<IUserDefinedTableType> UserDefinedTableTypes => userDefinedTableTypes;

	public IMutableMetadataCollection<IUserDefinedClrType> UserDefinedClrTypes => userDefinedClrTypes;

	IMetadataCollection<ITable> ISchema.Tables => tables;

	IMetadataCollection<IView> ISchema.Views => views;

	IMetadataCollection<IUserDefinedAggregate> ISchema.UserDefinedAggregates => userDefinedAggregates;

	IMetadataCollection<ITableValuedFunction> ISchema.TableValuedFunctions => tableValuedFunctions;

	IMetadataCollection<IScalarValuedFunction> ISchema.ScalarValuedFunctions => scalarValuedFunctions;

	IMetadataCollection<IStoredProcedure> ISchema.StoredProcedures => storedProcedures;

	IMetadataCollection<ISynonym> ISchema.Synonyms => synonyms;

	IMetadataCollection<IExtendedStoredProcedure> ISchema.ExtendedStoredProcedures => extendedStoredProcedures;

	IMetadataCollection<IUserDefinedDataType> ISchema.UserDefinedDataTypes => userDefinedDataTypes;

	IMetadataCollection<IUserDefinedTableType> ISchema.UserDefinedTableTypes => userDefinedTableTypes;

	IMetadataCollection<IUserDefinedClrType> ISchema.UserDefinedClrTypes => userDefinedClrTypes;

	public Schema(IDatabase database, string name)
		: base(database, name)
	{
		CollationInfo collationInfo = database.CollationInfo;
		extendedStoredProcedures = new ExtendedStoredProcedureCollection(collationInfo);
		scalarValuedFunctions = new ScalarValuedFunctionCollection(collationInfo);
		storedProcedures = new StoredProcedureCollection(collationInfo);
		synonyms = new SynonymCollection(collationInfo);
		tables = new TableCollection(collationInfo);
		tableValuedFunctions = new TableValuedFunctionCollection(collationInfo);
		userDefinedAggregates = new UserDefinedAggregateCollection(collationInfo);
		userDefinedClrTypes = new UserDefinedClrTypeCollection(collationInfo);
		userDefinedDataTypes = new UserDefinedDataTypeCollection(collationInfo);
		userDefinedTableTypes = new UserDefinedTableTypeCollection(collationInfo);
		views = new ViewCollection(collationInfo);
	}

	public Schema(ISchema other)
		: base(other)
	{
		CollationInfo collationInfo = other.Database.CollationInfo;
		extendedStoredProcedures = new ExtendedStoredProcedureCollection(other.ExtendedStoredProcedures.Count, collationInfo);
		extendedStoredProcedures.AddRange(other.ExtendedStoredProcedures);
		scalarValuedFunctions = new ScalarValuedFunctionCollection(other.ScalarValuedFunctions.Count, collationInfo);
		scalarValuedFunctions.AddRange(other.ScalarValuedFunctions);
		storedProcedures = new StoredProcedureCollection(other.StoredProcedures.Count, collationInfo);
		storedProcedures.AddRange(other.StoredProcedures);
		synonyms = new SynonymCollection(other.Synonyms.Count, collationInfo);
		synonyms.AddRange(other.Synonyms);
		tables = new TableCollection(other.Tables.Count, collationInfo);
		tables.AddRange(other.Tables);
		tableValuedFunctions = new TableValuedFunctionCollection(other.TableValuedFunctions.Count, collationInfo);
		tableValuedFunctions.AddRange(other.TableValuedFunctions);
		userDefinedAggregates = new UserDefinedAggregateCollection(other.UserDefinedAggregates.Count, collationInfo);
		userDefinedAggregates.AddRange(other.UserDefinedAggregates);
		userDefinedClrTypes = new UserDefinedClrTypeCollection(other.UserDefinedClrTypes.Count, collationInfo);
		userDefinedClrTypes.AddRange(other.UserDefinedClrTypes);
		userDefinedDataTypes = new UserDefinedDataTypeCollection(other.UserDefinedDataTypes.Count, collationInfo);
		userDefinedDataTypes.AddRange(other.UserDefinedDataTypes);
		userDefinedTableTypes = new UserDefinedTableTypeCollection(other.UserDefinedTableTypes.Count, collationInfo);
		userDefinedTableTypes.AddRange(other.UserDefinedTableTypes);
		views = new ViewCollection(other.Views.Count, collationInfo);
		views.AddRange(other.Views);
		owner = other.Owner;
	}

	public override T Accept<T>(IDatabaseOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
