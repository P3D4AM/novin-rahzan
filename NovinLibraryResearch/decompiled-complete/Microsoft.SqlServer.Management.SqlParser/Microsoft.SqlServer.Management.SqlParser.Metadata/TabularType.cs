namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public enum TabularType
{
	CommonTableExpression,
	DerivedTable,
	DmlInsertedDeleted,
	DmlTableSource,
	PivotTable,
	Table,
	TableAlias,
	TableDataType,
	TableValuedFunction,
	TableVariable,
	TemporaryTable,
	UnpivotTable,
	View
}
