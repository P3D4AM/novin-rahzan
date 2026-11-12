namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

internal class TypeInfoVisitor : IMetadataObjectVisitor<MetadataTypeInfo>, IDatabaseObjectVisitor<MetadataTypeInfo>, IServerOwnedObjectVisitor<MetadataTypeInfo>, IDatabaseOwnedObjectVisitor<MetadataTypeInfo>, ISchemaOwnedObjectVisitor<MetadataTypeInfo>
{
	public MetadataTypeInfo Visit(IBuiltInFunction obj)
	{
		return null;
	}

	public MetadataTypeInfo Visit(ICheckConstraint obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(19);
	}

	public MetadataTypeInfo Visit(IClrDataType obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(20);
	}

	public MetadataTypeInfo Visit(ICollation obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(21);
	}

	public MetadataTypeInfo Visit(IColumn obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(22);
	}

	public MetadataTypeInfo Visit(ICursorDataType obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(25);
	}

	public MetadataTypeInfo Visit(ICursorParameter obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(26);
	}

	public MetadataTypeInfo Visit(ICursorVariable obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(27);
	}

	public MetadataTypeInfo Visit(IDatabasePermission obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(33);
	}

	public MetadataTypeInfo Visit(IDatePart obj)
	{
		return null;
	}

	public MetadataTypeInfo Visit(IDefaultConstraint obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(37);
	}

	public MetadataTypeInfo Visit(IDmlTrigger obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(38);
	}

	public MetadataTypeInfo Visit(IExecutionContext obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(39);
	}

	public MetadataTypeInfo Visit(IForeignKeyColumn obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(42);
	}

	public MetadataTypeInfo Visit(IForeignKeyConstraint obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(43);
	}

	public MetadataTypeInfo Visit(IIndexedColumn obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(47);
	}

	public MetadataTypeInfo Visit(IPrimaryKeyConstraint obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(90);
	}

	public MetadataTypeInfo Visit(IRelationalIndex obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(91);
	}

	public MetadataTypeInfo Visit(IScalarExpression obj)
	{
		return null;
	}

	public MetadataTypeInfo Visit(IScalarDataType obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(93);
	}

	public MetadataTypeInfo Visit(IScalarParameter obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(95);
	}

	public MetadataTypeInfo Visit(IScalarVariable obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(97);
	}

	public MetadataTypeInfo Visit(ISpatialIndex obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(103);
	}

	public MetadataTypeInfo Visit(IStatistics obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(104);
	}

	public MetadataTypeInfo Visit(ISystemDataType obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(108);
	}

	public MetadataTypeInfo Visit(ISystemClrDataType obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(107);
	}

	public MetadataTypeInfo Visit(ITableDataType obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(110);
	}

	public MetadataTypeInfo Visit(ITableParameter obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(111);
	}

	public MetadataTypeInfo Visit(ITableVariable obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(113);
	}

	public MetadataTypeInfo Visit(IUdtDataMember obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(118);
	}

	public MetadataTypeInfo Visit(IUdtMethod obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(120);
	}

	public MetadataTypeInfo Visit(IUniqueConstraint obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(121);
	}

	public MetadataTypeInfo Visit(IVirtualTable obj)
	{
		return null;
	}

	public MetadataTypeInfo Visit(IVoidDataType obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(132);
	}

	public MetadataTypeInfo Visit(IXmlDataType obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(133);
	}

	public MetadataTypeInfo Visit(IXmlDataTypeMethod obj)
	{
		return null;
	}

	public MetadataTypeInfo Visit(IXmlDataTypeTableMethod obj)
	{
		return null;
	}

	public MetadataTypeInfo Visit(IXmlIndex obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(136);
	}

	public MetadataTypeInfo Visit(IServer obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(100);
	}

	public MetadataTypeInfo Visit(ICredential obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(24);
	}

	public MetadataTypeInfo Visit(IDatabase obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(29);
	}

	public MetadataTypeInfo Visit(ILogin obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(49);
	}

	public MetadataTypeInfo Visit(IServerDdlTrigger obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(101);
	}

	public MetadataTypeInfo Visit(IApplicationRole obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(15);
	}

	public MetadataTypeInfo Visit(IAsymmetricKey obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(16);
	}

	public MetadataTypeInfo Visit(ICertificate obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(18);
	}

	public MetadataTypeInfo Visit(IDatabaseDdlTrigger obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(30);
	}

	public MetadataTypeInfo Visit(IDatabaseRole obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(35);
	}

	public MetadataTypeInfo Visit(IFileGroup obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(41);
	}

	public MetadataTypeInfo Visit(IPartitionScheme obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(88);
	}

	public MetadataTypeInfo Visit(ISchema obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(98);
	}

	public MetadataTypeInfo Visit(IUser obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(123);
	}

	public MetadataTypeInfo Visit(IExtendedStoredProcedure obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(40);
	}

	public MetadataTypeInfo Visit(IScalarValuedFunction obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(96);
	}

	public MetadataTypeInfo Visit(IStoredProcedure obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(105);
	}

	public MetadataTypeInfo Visit(ISynonym obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(106);
	}

	public MetadataTypeInfo Visit(ITable obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(109);
	}

	public MetadataTypeInfo Visit(ITableValuedFunction obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(112);
	}

	public MetadataTypeInfo Visit(IUserDefinedAggregate obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(124);
	}

	public MetadataTypeInfo Visit(IUserDefinedClrType obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(125);
	}

	public MetadataTypeInfo Visit(IUserDefinedDataType obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(126);
	}

	public MetadataTypeInfo Visit(IUserDefinedTableType obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(129);
	}

	public MetadataTypeInfo Visit(IView obj)
	{
		return (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(131);
	}
}
