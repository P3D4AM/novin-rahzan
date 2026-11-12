namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableSchema : ISchema, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject, IMutableDatabaseOwnedObject, IMutableDatabaseObject, IMutableMetadataObject
{
	new IDatabasePrincipal Owner { get; set; }

	new IMutableMetadataCollection<ITable> Tables { get; }

	new IMutableMetadataCollection<IView> Views { get; }

	new IMutableMetadataCollection<IUserDefinedAggregate> UserDefinedAggregates { get; }

	new IMutableMetadataCollection<ITableValuedFunction> TableValuedFunctions { get; }

	new IMutableMetadataCollection<IScalarValuedFunction> ScalarValuedFunctions { get; }

	new IMutableMetadataCollection<IStoredProcedure> StoredProcedures { get; }

	new IMutableMetadataCollection<IExtendedStoredProcedure> ExtendedStoredProcedures { get; }

	new IMutableMetadataCollection<IUserDefinedDataType> UserDefinedDataTypes { get; }

	new IMutableMetadataCollection<IUserDefinedTableType> UserDefinedTableTypes { get; }

	new IMutableMetadataCollection<IUserDefinedClrType> UserDefinedClrTypes { get; }

	new IMutableMetadataCollection<ISynonym> Synonyms { get; }
}
