namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface ISchemaOwnedObjectVisitor<T>
{
	T Visit(IExtendedStoredProcedure extendedStoredProcedure);

	T Visit(IScalarValuedFunction scalarValuedFunction);

	T Visit(IStoredProcedure storedProcedure);

	T Visit(ISynonym synonym);

	T Visit(ITable table);

	T Visit(ITableValuedFunction tableValuedFunction);

	T Visit(IUserDefinedAggregate userDefinedAggregate);

	T Visit(IUserDefinedClrType userDefinedClrType);

	T Visit(IUserDefinedDataType userDefinedDataType);

	T Visit(IUserDefinedTableType userDefinedTableType);

	T Visit(IView view);
}
