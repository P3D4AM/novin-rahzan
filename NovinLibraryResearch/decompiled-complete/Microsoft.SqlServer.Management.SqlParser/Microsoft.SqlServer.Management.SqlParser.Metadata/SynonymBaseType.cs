namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public enum SynonymBaseType
{
	None,
	ClrAggregateFunction,
	ClrScalarFunction,
	ClrStoredProcedure,
	ClrTableValuedFunction,
	ExtendedStoredProcedure,
	ReplicationFilterProcedure,
	SqlInlineTableValuedFunction,
	SqlScalarFunction,
	SqlStoredProcedure,
	SqlTableValuedFunction,
	Table,
	View
}
