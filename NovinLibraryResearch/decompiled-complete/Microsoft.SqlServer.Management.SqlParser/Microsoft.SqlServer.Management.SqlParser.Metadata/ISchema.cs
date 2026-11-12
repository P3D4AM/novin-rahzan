namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
public interface ISchema : IDatabaseOwnedObject, IDatabaseObject, IMetadataObject
{
	[ReadWrite]
	[Reference]
	IDatabasePrincipal Owner { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<ITable> Tables { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<IView> Views { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<IUserDefinedAggregate> UserDefinedAggregates { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<ITableValuedFunction> TableValuedFunctions { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<IScalarValuedFunction> ScalarValuedFunctions { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<IStoredProcedure> StoredProcedures { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<IExtendedStoredProcedure> ExtendedStoredProcedures { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<IUserDefinedDataType> UserDefinedDataTypes { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<IUserDefinedTableType> UserDefinedTableTypes { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<IUserDefinedClrType> UserDefinedClrTypes { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<ISynonym> Synonyms { get; }
}
