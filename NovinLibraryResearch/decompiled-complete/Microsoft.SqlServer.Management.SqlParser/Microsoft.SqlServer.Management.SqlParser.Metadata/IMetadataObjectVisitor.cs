namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMetadataObjectVisitor<T> : IDatabaseObjectVisitor<T>, IServerOwnedObjectVisitor<T>, IDatabaseOwnedObjectVisitor<T>, ISchemaOwnedObjectVisitor<T>
{
	T Visit(IBuiltInFunction builtInFunction);

	T Visit(ICheckConstraint checkConstraint);

	T Visit(IClrDataType clrDataType);

	T Visit(ICollation collation);

	T Visit(IColumn column);

	T Visit(ICursorDataType cursorDataType);

	T Visit(ICursorParameter cursorParameter);

	T Visit(ICursorVariable cursorVariable);

	T Visit(IDatabasePermission databasePermission);

	T Visit(IDatePart datePart);

	T Visit(IDefaultConstraint defaultConstraint);

	T Visit(IDmlTrigger dmlTrigger);

	T Visit(IExecutionContext executionContext);

	T Visit(IForeignKeyColumn foreignKeyColumn);

	T Visit(IForeignKeyConstraint foreignKeyConstraint);

	T Visit(IIndexedColumn indexedColumn);

	T Visit(IPrimaryKeyConstraint primaryKeyConstraint);

	T Visit(IRelationalIndex relationalIndex);

	T Visit(IScalarExpression scalarExpression);

	T Visit(IScalarDataType scalarDataType);

	T Visit(IScalarParameter scalarParameter);

	T Visit(IScalarVariable scalarVariable);

	T Visit(ISpatialIndex spatialIndex);

	T Visit(IStatistics statistics);

	T Visit(ISystemDataType systemDataType);

	T Visit(ISystemClrDataType systemClrDataType);

	T Visit(ITableDataType tableDataType);

	T Visit(ITableParameter tableParameter);

	T Visit(ITableVariable tableVariable);

	T Visit(IUdtDataMember udtDataMember);

	T Visit(IUdtMethod udtMethod);

	T Visit(IUniqueConstraint uniqueConstraint);

	T Visit(IVirtualTable virtualTable);

	T Visit(IVoidDataType voidDataType);

	T Visit(IXmlDataType xmlDataType);

	T Visit(IXmlDataTypeMethod xmlDataTypeMethod);

	T Visit(IXmlDataTypeTableMethod xmlDataTypeMethod);

	T Visit(IXmlIndex xmlIndex);
}
