namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

internal interface IMetadataTypeInfoVisitor<T>
{
	T Visit(IApplicationRole obj);

	T Visit(IAsymmetricKey obj);

	T Visit(ICertificate obj);

	T Visit(ICheckConstraint obj);

	T Visit(IClrDataType obj);

	T Visit(ICollation obj);

	T Visit(IColumn obj);

	T Visit(ICredential obj);

	T Visit(ICursorDataType obj);

	T Visit(ICursorParameter obj);

	T Visit(ICursorVariable obj);

	T Visit(IDatabase obj);

	T Visit(IDatabaseDdlTrigger obj);

	T Visit(IDatabasePermission obj);

	T Visit(IDatabaseRole obj);

	T Visit(IDefaultConstraint obj);

	T Visit(IDmlTrigger obj);

	T Visit(IExecutionContext obj);

	T Visit(IExtendedStoredProcedure obj);

	T Visit(IFileGroup obj);

	T Visit(IForeignKeyColumn obj);

	T Visit(IForeignKeyConstraint obj);

	T Visit(IIndexedColumn obj);

	T Visit(ILogin obj);

	T Visit(IPartitionScheme obj);

	T Visit(IPrimaryKeyConstraint obj);

	T Visit(IRelationalIndex obj);

	T Visit(IScalarDataType obj);

	T Visit(IScalarParameter obj);

	T Visit(IScalarValuedFunction obj);

	T Visit(IScalarVariable obj);

	T Visit(ISchema obj);

	T Visit(IServer obj);

	T Visit(IServerDdlTrigger obj);

	T Visit(ISpatialIndex obj);

	T Visit(IStatistics obj);

	T Visit(IStoredProcedure obj);

	T Visit(ISynonym obj);

	T Visit(ISystemClrDataType obj);

	T Visit(ISystemDataType obj);

	T Visit(ITable obj);

	T Visit(ITableDataType obj);

	T Visit(ITableParameter obj);

	T Visit(ITableValuedFunction obj);

	T Visit(ITableVariable obj);

	T Visit(IUdtDataMember obj);

	T Visit(IUdtMethod obj);

	T Visit(IUniqueConstraint obj);

	T Visit(IUser obj);

	T Visit(IUserDefinedAggregate obj);

	T Visit(IUserDefinedClrType obj);

	T Visit(IUserDefinedDataType obj);

	T Visit(IUserDefinedTableType obj);

	T Visit(IView obj);

	T Visit(IVoidDataType obj);

	T Visit(IXmlDataType obj);

	T Visit(IXmlIndex obj);
}
