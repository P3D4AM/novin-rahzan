using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public static class MetadataInfoProvider
{
	public static class MetadataTypes
	{
		public const int IApplicationRole = 15;

		public const int IAsymmetricKey = 16;

		public const int ICallableModule = 17;

		public const int ICertificate = 18;

		public const int ICheckConstraint = 19;

		public const int IClrDataType = 20;

		public const int ICollation = 21;

		public const int IColumn = 22;

		public const int IConstraint = 23;

		public const int ICredential = 24;

		public const int ICursorDataType = 25;

		public const int ICursorParameter = 26;

		public const int ICursorVariable = 27;

		public const int IDataType = 28;

		public const int IDatabase = 29;

		public const int IDatabaseDdlTrigger = 30;

		public const int IDatabaseObject = 31;

		public const int IDatabaseOwnedObject = 32;

		public const int IDatabasePermission = 33;

		public const int IDatabasePrincipal = 34;

		public const int IDatabaseRole = 35;

		public const int IDatabaseTable = 36;

		public const int IDefaultConstraint = 37;

		public const int IDmlTrigger = 38;

		public const int IExecutionContext = 39;

		public const int IExtendedStoredProcedure = 40;

		public const int IFileGroup = 41;

		public const int IForeignKeyColumn = 42;

		public const int IForeignKeyConstraint = 43;

		public const int IFunction = 44;

		public const int IFunctionModuleBase = 45;

		public const int IIndex = 46;

		public const int IIndexedColumn = 47;

		public const int ILocalVariable = 48;

		public const int ILogin = 49;

		public const int IMetadataObject = 82;

		public const int IParameter = 87;

		public const int IPartitionScheme = 88;

		public const int IPrimaryKeyConstraint = 90;

		public const int IRelationalIndex = 91;

		public const int IScalar = 92;

		public const int IScalarDataType = 93;

		public const int IScalarFunction = 94;

		public const int IScalarParameter = 95;

		public const int IScalarValuedFunction = 96;

		public const int IScalarVariable = 97;

		public const int ISchema = 98;

		public const int ISchemaOwnedObject = 99;

		public const int IServer = 100;

		public const int IServerDdlTrigger = 101;

		public const int IServerOwnedObject = 102;

		public const int ISpatialIndex = 103;

		public const int IStatistics = 104;

		public const int IStoredProcedure = 105;

		public const int ISynonym = 106;

		public const int ISystemClrDataType = 107;

		public const int ISystemDataType = 108;

		public const int ITable = 109;

		public const int ITableDataType = 110;

		public const int ITableParameter = 111;

		public const int ITableValuedFunction = 112;

		public const int ITableVariable = 113;

		public const int ITableViewBase = 114;

		public const int ITabular = 115;

		public const int ITrigger = 116;

		public const int IUdtDataMember = 118;

		public const int IUdtMember = 119;

		public const int IUdtMethod = 120;

		public const int IUniqueConstraint = 121;

		public const int IUniqueConstraintBase = 122;

		public const int IUser = 123;

		public const int IUserDefinedAggregate = 124;

		public const int IUserDefinedClrType = 125;

		public const int IUserDefinedDataType = 126;

		public const int IUserDefinedFunction = 127;

		public const int IUserDefinedFunctionModuleBase = 128;

		public const int IUserDefinedTableType = 129;

		public const int IUserDefinedType = 130;

		public const int IView = 131;

		public const int IVoidDataType = 132;

		public const int IXmlDataType = 133;

		public const int IXmlIndex = 136;
	}

	public static class CollectionTypes
	{
		public const int IMetadataCollection_IStoredProcedure = 50;

		public const int IMetadataCollection_IDmlTrigger = 51;

		public const int IMetadataCollection_ICredential = 52;

		public const int IMetadataCollection_IConstraint = 53;

		public const int IMetadataCollection_IIndex = 54;

		public const int IMetadataCollection_IStatistics = 55;

		public const int IMetadataCollection_IFileGroup = 56;

		public const int IMetadataCollection_IServerDdlTrigger = 57;

		public const int IMetadataCollection_IDatabaseRole = 58;

		public const int IMetadataCollection_IDatabasePermission = 59;

		public const int IMetadataCollection_IXmlDataTypeMethod = 60;

		public const int IMetadataCollection_IXmlDataTypeTableMethod = 61;

		public const int IMetadataCollection_ILogin = 62;

		public const int IMetadataCollection_ITable = 63;

		public const int IMetadataCollection_IDatabase = 64;

		public const int IMetadataCollection_IUserDefinedClrType = 65;

		public const int IMetadataCollection_IScalarValuedFunction = 66;

		public const int IMetadataCollection_IExtendedStoredProcedure = 67;

		public const int IMetadataCollection_IApplicationRole = 68;

		public const int IMetadataCollection_IAsymmetricKey = 69;

		public const int IMetadataCollection_ICertificate = 70;

		public const int IMetadataCollection_IUdtDataMember = 71;

		public const int IMetadataCollection_ISynonym = 72;

		public const int IMetadataCollection_IUdtMethod = 73;

		public const int IMetadataCollection_ISchema = 74;

		public const int IMetadataCollection_IDatabaseDdlTrigger = 75;

		public const int IMetadataCollection_IUser = 76;

		public const int IMetadataCollection_IUserDefinedAggregate = 77;

		public const int IMetadataCollection_IUserDefinedTableType = 78;

		public const int IMetadataCollection_IView = 79;

		public const int IMetadataCollection_ITableValuedFunction = 80;

		public const int IMetadataCollection_IUserDefinedDataType = 81;

		public const int IMetadataOrderedCollection_IIndexedColumn = 83;

		public const int IMetadataOrderedCollection_IColumn = 84;

		public const int IMetadataOrderedCollection_IForeignKeyColumn = 85;

		public const int IMetadataOrderedCollection_IParameter = 86;
	}

	public static class PrimitiveTypes
	{
		public const int ActivationOrder = 0;

		public const int Boolean = 1;

		public const int Byte = 2;

		public const int ByteArray = 3;

		public const int CallableModuleType = 4;

		public const int CollationInfo = 5;

		public const int ComputedColumnInfo = 6;

		public const int ConstraintType = 7;

		public const int DataTypeSpec = 8;

		public const int DatabaseCompatibilityLevel = 9;

		public const int DatabasePermissionType = 10;

		public const int Double = 11;

		public const int ExecutionContextType = 12;

		public const int ForeignKeyAction = 13;

		public const int GridDensity = 14;

		public const int IPassword = 89;

		public const int ITriggerEventTypeSet = 117;

		public const int IXmlDataTypeMethod = 134;

		public const int IXmlDataTypeTableMethod = 135;

		public const int IdentityColumnInfo = 137;

		public const int IndexType = 138;

		public const int Int32 = 139;

		public const int Int64 = 140;

		public const int LoginType = 141;

		public const int PermissionState = 142;

		public const int ScalarType = 143;

		public const int SortOrder = 144;

		public const int StatisticsType = 145;

		public const int String = 146;

		public const int SynonymBaseType = 147;

		public const int TabularType = 148;

		public const int UserType = 149;
	}

	public static class MetadataPropertyId
	{
		public const int Database = 0;

		public const int DefaultSchema = 1;

		public const int IsSystemObject = 2;

		public const int MemberOfRoles = 3;

		public const int Name = 4;

		public const int Permissions = 5;

		public const int Parent = 6;

		public const int ExecutionContext = 7;

		public const int IsEncrypted = 8;

		public const int ModuleType = 9;

		public const int Parameters = 10;

		public const int ReturnType = 11;

		public const int Schema = 12;

		public const int IsChecked = 13;

		public const int IsEnabled = 14;

		public const int IsSystemNamed = 15;

		public const int NotForReplication = 16;

		public const int Text = 17;

		public const int Type = 18;

		public const int AssemblyName = 19;

		public const int BaseSystemDataType = 20;

		public const int ClassName = 21;

		public const int DataMembers = 22;

		public const int IsBinaryOrdered = 23;

		public const int IsClr = 24;

		public const int IsComVisible = 25;

		public const int IsCursor = 26;

		public const int IsNullable = 27;

		public const int IsScalar = 28;

		public const int IsSystem = 29;

		public const int IsTable = 30;

		public const int IsUnknown = 31;

		public const int IsVoid = 32;

		public const int IsXml = 33;

		public const int Methods = 34;

		public const int Collation = 35;

		public const int ComputedColumnInfo = 36;

		public const int DataType = 37;

		public const int DefaultValue = 38;

		public const int IdentityColumnInfo = 39;

		public const int InPrimaryKey = 40;

		public const int IsColumnSet = 41;

		public const int IsGeneratedAlwaysAsRowEnd = 42;

		public const int IsGeneratedAlwaysAsRowStart = 43;

		public const int IsGeneratedAlwaysAsSequenceNumberEnd = 44;

		public const int IsGeneratedAlwaysAsSequenceNumberStart = 45;

		public const int IsGeneratedAlwaysAsTransactionIdEnd = 46;

		public const int IsGeneratedAlwaysAsTransactionIdStart = 47;

		public const int IsSparse = 48;

		public const int Nullable = 49;

		public const int RowGuidCol = 50;

		public const int ScalarType = 51;

		public const int Server = 52;

		public const int IsCursorVariable = 53;

		public const int IsOutput = 54;

		public const int IsParameter = 55;

		public const int IsReadOnly = 56;

		public const int IsScalarVariable = 57;

		public const int IsTableVariable = 58;

		public const int ApplicationRoles = 59;

		public const int AsymmetricKeys = 60;

		public const int Certificates = 61;

		public const int CollationInfo = 62;

		public const int CompatibilityLevel = 63;

		public const int DefaultSchemaName = 64;

		public const int Owner = 65;

		public const int Roles = 66;

		public const int Schemas = 67;

		public const int Triggers = 68;

		public const int Users = 69;

		public const int BodyText = 70;

		public const int DatabaseDdlEvents = 71;

		public const int IsQuotedIdentifierOn = 72;

		public const int IsSqlClr = 73;

		public const int DatabasePrincipal = 74;

		public const int Grantor = 75;

		public const int PermissionState = 76;

		public const int PermissionType = 77;

		public const int TargetObject = 78;

		public const int IsFixedRole = 79;

		public const int Columns = 80;

		public const int Constraints = 81;

		public const int Indexes = 82;

		public const int Statistics = 83;

		public const int TabularType = 84;

		public const int Unaliased = 85;

		public const int Delete = 86;

		public const int DeleteActivationOrder = 87;

		public const int Insert = 88;

		public const int InsertActivationOrder = 89;

		public const int InsteadOf = 90;

		public const int Update = 91;

		public const int UpdateActivationOrder = 92;

		public const int ContextType = 93;

		public const int Login = 94;

		public const int User = 95;

		public const int IsDefault = 96;

		public const int IsFileStream = 97;

		public const int ReferencedColumn = 98;

		public const int ReferencingColumn = 99;

		public const int DeleteAction = 100;

		public const int ReferencedTable = 101;

		public const int UpdateAction = 102;

		public const int DisallowPageLocks = 103;

		public const int DisallowRowLocks = 104;

		public const int FillFactor = 105;

		public const int IgnoreDuplicateKeys = 106;

		public const int IsDisabled = 107;

		public const int PadIndex = 108;

		public const int IsIncluded = 109;

		public const int SortOrder = 110;

		public const int AsymmetricKey = 111;

		public const int Certificate = 112;

		public const int Credential = 113;

		public const int DefaultDatabase = 114;

		public const int Language = 115;

		public const int LoginType = 116;

		public const int Password = 117;

		public const int Sid = 118;

		public const int FileGroups = 119;

		public const int AssociatedIndex = 120;

		public const int CompactLargeObjects = 121;

		public const int FileGroup = 122;

		public const int FileStreamFileGroup = 123;

		public const int FileStreamPartitionScheme = 124;

		public const int FilterDefinition = 125;

		public const int IndexKey = 126;

		public const int IndexedColumns = 127;

		public const int IsClustered = 128;

		public const int IsUnique = 129;

		public const int NoAutomaticRecomputation = 130;

		public const int PartitionScheme = 131;

		public const int IsAggregateFunction = 132;

		public const int IsSchemaBound = 133;

		public const int ReturnsNullOnNullInput = 134;

		public const int ExtendedStoredProcedures = 135;

		public const int ScalarValuedFunctions = 136;

		public const int StoredProcedures = 137;

		public const int Synonyms = 138;

		public const int TableValuedFunctions = 139;

		public const int Tables = 140;

		public const int UserDefinedAggregates = 141;

		public const int UserDefinedClrTypes = 142;

		public const int UserDefinedDataTypes = 143;

		public const int UserDefinedTableTypes = 144;

		public const int Views = 145;

		public const int Credentials = 146;

		public const int Databases = 147;

		public const int Logins = 148;

		public const int ServerDdlEvents = 149;

		public const int BoundingBoxXMax = 150;

		public const int BoundingBoxXMin = 151;

		public const int BoundingBoxYMax = 152;

		public const int BoundingBoxYMin = 153;

		public const int CellsPerObject = 154;

		public const int IndexedColumn = 155;

		public const int Level1Density = 156;

		public const int Level2Density = 157;

		public const int Level3Density = 158;

		public const int Level4Density = 159;

		public const int ForReplication = 160;

		public const int IsRecompiled = 161;

		public const int Startup = 162;

		public const int BaseObjectName = 163;

		public const int BaseType = 164;

		public const int Length = 165;

		public const int NumericPrecision = 166;

		public const int NumericScale = 167;

		public const int TypeSpec = 168;

		public const int IsInline = 169;

		public const int TableVariableName = 170;

		public const int IsStatic = 171;

		public const int UserDefinedType = 172;

		public const int UserType = 173;

		public const int HasCheckOption = 174;

		public const int HasColumnSpecification = 175;

		public const int QueryText = 176;

		public const int ReturnsViewMetadata = 177;

		public const int IsXmlNode = 178;

		public const int TableMethods = 179;
	}

	private static readonly MetadataInfo metadataInfo;

	public static MetadataInfo MetadataInfo => metadataInfo;

	static MetadataInfoProvider()
	{
		TypeInfo[] array = new TypeInfo[150]
		{
			new PrimitiveTypeInfo(typeof(ActivationOrder), 0),
			new PrimitiveTypeInfo(typeof(bool), 1),
			new PrimitiveTypeInfo(typeof(byte), 2),
			new PrimitiveTypeInfo(typeof(byte[]), 3),
			new PrimitiveTypeInfo(typeof(CallableModuleType), 4),
			new PrimitiveTypeInfo(typeof(CollationInfo), 5),
			new PrimitiveTypeInfo(typeof(ComputedColumnInfo), 6),
			new PrimitiveTypeInfo(typeof(ConstraintType), 7),
			new PrimitiveTypeInfo(typeof(DataTypeSpec), 8),
			new PrimitiveTypeInfo(typeof(DatabaseCompatibilityLevel), 9),
			new PrimitiveTypeInfo(typeof(DatabasePermissionType), 10),
			new PrimitiveTypeInfo(typeof(double), 11),
			new PrimitiveTypeInfo(typeof(ExecutionContextType), 12),
			new PrimitiveTypeInfo(typeof(ForeignKeyAction), 13),
			new PrimitiveTypeInfo(typeof(GridDensity), 14),
			new MetadataTypeInfo(typeof(IApplicationRole), 15, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IAsymmetricKey), 16, MetadataType.Reference),
			new MetadataTypeInfo(typeof(ICallableModule), 17, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(ICertificate), 18, MetadataType.Reference),
			new MetadataTypeInfo(typeof(ICheckConstraint), 19, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IClrDataType), 20, MetadataType.Value),
			new MetadataTypeInfo(typeof(ICollation), 21, MetadataType.Value),
			new MetadataTypeInfo(typeof(IColumn), 22, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IConstraint), 23, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(ICredential), 24, MetadataType.Reference),
			new MetadataTypeInfo(typeof(ICursorDataType), 25, MetadataType.Value),
			new MetadataTypeInfo(typeof(ICursorParameter), 26, MetadataType.Value),
			new MetadataTypeInfo(typeof(ICursorVariable), 27, MetadataType.Value),
			new MetadataTypeInfo(typeof(IDataType), 28, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(IDatabase), 29, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IDatabaseDdlTrigger), 30, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IDatabaseObject), 31, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(IDatabaseOwnedObject), 32, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(IDatabasePermission), 33, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IDatabasePrincipal), 34, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(IDatabaseRole), 35, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IDatabaseTable), 36, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(IDefaultConstraint), 37, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IDmlTrigger), 38, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IExecutionContext), 39, MetadataType.Value),
			new MetadataTypeInfo(typeof(IExtendedStoredProcedure), 40, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IFileGroup), 41, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IForeignKeyColumn), 42, MetadataType.Value),
			new MetadataTypeInfo(typeof(IForeignKeyConstraint), 43, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IFunction), 44, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(IFunctionModuleBase), 45, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(IIndex), 46, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(IIndexedColumn), 47, MetadataType.Value),
			new MetadataTypeInfo(typeof(ILocalVariable), 48, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(ILogin), 49, MetadataType.Reference),
			new CollectionTypeInfo(typeof(IMetadataCollection<IStoredProcedure>), 50, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IDmlTrigger>), 51, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<ICredential>), 52, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IConstraint>), 53, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IIndex>), 54, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IStatistics>), 55, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IFileGroup>), 56, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IServerDdlTrigger>), 57, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IDatabaseRole>), 58, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IDatabasePermission>), 59, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IXmlDataTypeMethod>), 60, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IXmlDataTypeTableMethod>), 61, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<ILogin>), 62, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<ITable>), 63, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IDatabase>), 64, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IUserDefinedClrType>), 65, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IScalarValuedFunction>), 66, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IExtendedStoredProcedure>), 67, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IApplicationRole>), 68, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IAsymmetricKey>), 69, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<ICertificate>), 70, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IUdtDataMember>), 71, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<ISynonym>), 72, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IUdtMethod>), 73, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<ISchema>), 74, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IDatabaseDdlTrigger>), 75, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IUser>), 76, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IUserDefinedAggregate>), 77, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IUserDefinedTableType>), 78, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IView>), 79, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<ITableValuedFunction>), 80, isOrdered: false),
			new CollectionTypeInfo(typeof(IMetadataCollection<IUserDefinedDataType>), 81, isOrdered: false),
			new MetadataTypeInfo(typeof(IMetadataObject), 82, MetadataType.Abstract),
			new CollectionTypeInfo(typeof(IMetadataOrderedCollection<IIndexedColumn>), 83, isOrdered: true),
			new CollectionTypeInfo(typeof(IMetadataOrderedCollection<IColumn>), 84, isOrdered: true),
			new CollectionTypeInfo(typeof(IMetadataOrderedCollection<IForeignKeyColumn>), 85, isOrdered: true),
			new CollectionTypeInfo(typeof(IMetadataOrderedCollection<IParameter>), 86, isOrdered: true),
			new MetadataTypeInfo(typeof(IParameter), 87, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(IPartitionScheme), 88, MetadataType.Reference),
			new PrimitiveTypeInfo(typeof(IPassword), 89),
			new MetadataTypeInfo(typeof(IPrimaryKeyConstraint), 90, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IRelationalIndex), 91, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IScalar), 92, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(IScalarDataType), 93, MetadataType.Value),
			new MetadataTypeInfo(typeof(IScalarFunction), 94, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(IScalarParameter), 95, MetadataType.Value),
			new MetadataTypeInfo(typeof(IScalarValuedFunction), 96, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IScalarVariable), 97, MetadataType.Value),
			new MetadataTypeInfo(typeof(ISchema), 98, MetadataType.Reference),
			new MetadataTypeInfo(typeof(ISchemaOwnedObject), 99, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(IServer), 100, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IServerDdlTrigger), 101, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IServerOwnedObject), 102, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(ISpatialIndex), 103, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IStatistics), 104, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IStoredProcedure), 105, MetadataType.Reference),
			new MetadataTypeInfo(typeof(ISynonym), 106, MetadataType.Reference),
			new MetadataTypeInfo(typeof(ISystemClrDataType), 107, MetadataType.Value),
			new MetadataTypeInfo(typeof(ISystemDataType), 108, MetadataType.Value),
			new MetadataTypeInfo(typeof(ITable), 109, MetadataType.Reference),
			new MetadataTypeInfo(typeof(ITableDataType), 110, MetadataType.Value),
			new MetadataTypeInfo(typeof(ITableParameter), 111, MetadataType.Value),
			new MetadataTypeInfo(typeof(ITableValuedFunction), 112, MetadataType.Reference),
			new MetadataTypeInfo(typeof(ITableVariable), 113, MetadataType.Value),
			new MetadataTypeInfo(typeof(ITableViewBase), 114, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(ITabular), 115, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(ITrigger), 116, MetadataType.Abstract),
			new PrimitiveTypeInfo(typeof(ITriggerEventTypeSet), 117),
			new MetadataTypeInfo(typeof(IUdtDataMember), 118, MetadataType.Value),
			new MetadataTypeInfo(typeof(IUdtMember), 119, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(IUdtMethod), 120, MetadataType.Value),
			new MetadataTypeInfo(typeof(IUniqueConstraint), 121, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IUniqueConstraintBase), 122, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(IUser), 123, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IUserDefinedAggregate), 124, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IUserDefinedClrType), 125, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IUserDefinedDataType), 126, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IUserDefinedFunction), 127, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(IUserDefinedFunctionModuleBase), 128, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(IUserDefinedTableType), 129, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IUserDefinedType), 130, MetadataType.Abstract),
			new MetadataTypeInfo(typeof(IView), 131, MetadataType.Reference),
			new MetadataTypeInfo(typeof(IVoidDataType), 132, MetadataType.Value),
			new MetadataTypeInfo(typeof(IXmlDataType), 133, MetadataType.Value),
			new PrimitiveTypeInfo(typeof(IXmlDataTypeMethod), 134),
			new PrimitiveTypeInfo(typeof(IXmlDataTypeTableMethod), 135),
			new MetadataTypeInfo(typeof(IXmlIndex), 136, MetadataType.Reference),
			new PrimitiveTypeInfo(typeof(IdentityColumnInfo), 137),
			new PrimitiveTypeInfo(typeof(IndexType), 138),
			new PrimitiveTypeInfo(typeof(int), 139),
			new PrimitiveTypeInfo(typeof(long), 140),
			new PrimitiveTypeInfo(typeof(LoginType), 141),
			new PrimitiveTypeInfo(typeof(PermissionState), 142),
			new PrimitiveTypeInfo(typeof(ScalarType), 143),
			new PrimitiveTypeInfo(typeof(SortOrder), 144),
			new PrimitiveTypeInfo(typeof(StatisticsType), 145),
			new PrimitiveTypeInfo(typeof(string), 146),
			new PrimitiveTypeInfo(typeof(SynonymBaseType), 147),
			new PrimitiveTypeInfo(typeof(TabularType), 148),
			new PrimitiveTypeInfo(typeof(UserType), 149)
		};
		((MetadataTypeInfo)array[15]).SetProperties(new MetadataPropertyInfo[6]
		{
			new MetadataPropertyInfo(0, 0, "Database", array[29], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(1, 1, "DefaultSchema", array[98], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 2, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(3, 3, "MemberOfRoles", array[58], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 4, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(5, 5, "Permissions", array[59], PropertyType.Child, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Database"))
		});
		((MetadataTypeInfo)array[16]).SetProperties(new MetadataPropertyInfo[3]
		{
			new MetadataPropertyInfo(0, 0, "Database", array[29], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(2, 1, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 2, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Database"))
		});
		((MetadataTypeInfo)array[17]).SetProperties(new MetadataPropertyInfo[8]
		{
			new MetadataPropertyInfo(7, 0, "ExecutionContext", array[39], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(8, 1, "IsEncrypted", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 2, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(9, 3, "ModuleType", array[4], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(4, 4, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(10, 5, "Parameters", array[86], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(11, 6, "ReturnType", array[93], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(12, 7, "Schema", array[98], PropertyType.Regular, isParent: true, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema"))
		});
		((MetadataTypeInfo)array[18]).SetProperties(new MetadataPropertyInfo[3]
		{
			new MetadataPropertyInfo(0, 0, "Database", array[29], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(2, 1, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 2, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Database"))
		});
		((MetadataTypeInfo)array[19]).SetProperties(new MetadataPropertyInfo[7]
		{
			new MetadataPropertyInfo(13, 0, "IsChecked", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(14, 1, "IsEnabled", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(15, 2, "IsSystemNamed", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 3, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(16, 4, "NotForReplication", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(6, 5, "Parent", array[115], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(17, 6, "Text", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(18, 0, "Type", array[7], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(ConstraintType.Check))
		});
		((MetadataTypeInfo)array[20]).SetProperties(new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(4, 0, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[16]
		{
			new MetadataPropertyInfo(19, 0, "AssemblyName", array[146], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(null)),
			new MetadataPropertyInfo(20, 1, "BaseSystemDataType", array[108], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(null)),
			new MetadataPropertyInfo(21, 2, "ClassName", array[146], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(null)),
			new MetadataPropertyInfo(22, 3, "DataMembers", array[71], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(null)),
			new MetadataPropertyInfo(23, 4, "IsBinaryOrdered", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(false)),
			new MetadataPropertyInfo(24, 5, "IsClr", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IClrDataType))),
			new MetadataPropertyInfo(25, 6, "IsComVisible", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(false)),
			new MetadataPropertyInfo(26, 7, "IsCursor", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorDataType))),
			new MetadataPropertyInfo(27, 8, "IsNullable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(false)),
			new MetadataPropertyInfo(28, 9, "IsScalar", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarDataType))),
			new MetadataPropertyInfo(29, 10, "IsSystem", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ISystemDataType))),
			new MetadataPropertyInfo(30, 11, "IsTable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableDataType))),
			new MetadataPropertyInfo(31, 12, "IsUnknown", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(true)),
			new MetadataPropertyInfo(32, 13, "IsVoid", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IVoidDataType))),
			new MetadataPropertyInfo(33, 14, "IsXml", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IXmlDataType))),
			new MetadataPropertyInfo(34, 15, "Methods", array[73], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(null))
		});
		((MetadataTypeInfo)array[21]).SetProperties(new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(4, 0, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[0]);
		((MetadataTypeInfo)array[22]).SetProperties(new MetadataPropertyInfo[18]
		{
			new MetadataPropertyInfo(35, 0, "Collation", array[21], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(36, 1, "ComputedColumnInfo", array[6], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(37, 2, "DataType", array[93], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(38, 3, "DefaultValue", array[37], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(39, 4, "IdentityColumnInfo", array[137], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(40, 5, "InPrimaryKey", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(41, 6, "IsColumnSet", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(42, 7, "IsGeneratedAlwaysAsRowEnd", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(43, 8, "IsGeneratedAlwaysAsRowStart", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(44, 9, "IsGeneratedAlwaysAsSequenceNumberEnd", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(45, 10, "IsGeneratedAlwaysAsSequenceNumberStart", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(46, 11, "IsGeneratedAlwaysAsTransactionIdEnd", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(47, 12, "IsGeneratedAlwaysAsTransactionIdStart", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(48, 13, "IsSparse", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 14, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(49, 15, "Nullable", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(6, 16, "Parent", array[115], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(50, 17, "RowGuidCol", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(51, 0, "ScalarType", array[143], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(ScalarType.Column))
		});
		((MetadataTypeInfo)array[23]).SetProperties(new MetadataPropertyInfo[4]
		{
			new MetadataPropertyInfo(15, 0, "IsSystemNamed", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 1, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(6, 2, "Parent", array[115], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(18, 3, "Type", array[7], PropertyType.Abstract, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[0]);
		((MetadataTypeInfo)array[24]).SetProperties(new MetadataPropertyInfo[3]
		{
			new MetadataPropertyInfo(2, 0, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 1, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(52, 2, "Server", array[100], PropertyType.Regular, isParent: true, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Server"))
		});
		((MetadataTypeInfo)array[25]).SetProperties(new MetadataPropertyInfo[0], new MetadataPropertyInfo[5]
		{
			new MetadataPropertyInfo(26, 0, "IsCursor", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorDataType))),
			new MetadataPropertyInfo(28, 1, "IsScalar", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarDataType))),
			new MetadataPropertyInfo(30, 2, "IsTable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableDataType))),
			new MetadataPropertyInfo(31, 3, "IsUnknown", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(false)),
			new MetadataPropertyInfo(4, 4, "Name", array[146], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr("cursor"))
		});
		((MetadataTypeInfo)array[26]).SetProperties(new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(4, 0, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[8]
		{
			new MetadataPropertyInfo(37, 0, "DataType", array[25], PropertyType.Inferred, isParent: false, isReadWrite: false, new PropertyValueExpr(PropertValueExprType.CursorDataType)),
			new MetadataPropertyInfo(38, 1, "DefaultValue", array[146], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(null)),
			new MetadataPropertyInfo(53, 2, "IsCursorVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorVariable))),
			new MetadataPropertyInfo(54, 3, "IsOutput", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(true)),
			new MetadataPropertyInfo(55, 4, "IsParameter", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IParameter))),
			new MetadataPropertyInfo(56, 5, "IsReadOnly", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(false)),
			new MetadataPropertyInfo(57, 6, "IsScalarVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarVariable))),
			new MetadataPropertyInfo(58, 7, "IsTableVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableVariable)))
		});
		((MetadataTypeInfo)array[27]).SetProperties(new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(4, 0, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[5]
		{
			new MetadataPropertyInfo(37, 0, "DataType", array[25], PropertyType.Inferred, isParent: false, isReadWrite: false, new PropertyValueExpr(PropertValueExprType.CursorDataType)),
			new MetadataPropertyInfo(53, 1, "IsCursorVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorVariable))),
			new MetadataPropertyInfo(55, 2, "IsParameter", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IParameter))),
			new MetadataPropertyInfo(57, 3, "IsScalarVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarVariable))),
			new MetadataPropertyInfo(58, 4, "IsTableVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableVariable)))
		});
		((MetadataTypeInfo)array[28]).SetProperties(new MetadataPropertyInfo[2]
		{
			new MetadataPropertyInfo(31, 0, "IsUnknown", array[1], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(4, 1, "Name", array[146], PropertyType.Abstract, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[3]
		{
			new MetadataPropertyInfo(26, 0, "IsCursor", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorDataType))),
			new MetadataPropertyInfo(28, 1, "IsScalar", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarDataType))),
			new MetadataPropertyInfo(30, 2, "IsTable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableDataType)))
		});
		((MetadataTypeInfo)array[29]).SetProperties(new MetadataPropertyInfo[14]
		{
			new MetadataPropertyInfo(59, 0, "ApplicationRoles", array[68], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(60, 1, "AsymmetricKeys", array[69], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(61, 2, "Certificates", array[70], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(62, 3, "CollationInfo", array[5], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(63, 4, "CompatibilityLevel", array[9], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(64, 5, "DefaultSchemaName", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 6, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 7, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(65, 8, "Owner", array[123], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(66, 9, "Roles", array[58], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(67, 10, "Schemas", array[74], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(52, 11, "Server", array[100], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(68, 12, "Triggers", array[75], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(69, 13, "Users", array[76], PropertyType.Child, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Server"))
		});
		((MetadataTypeInfo)array[30]).SetProperties(new MetadataPropertyInfo[10]
		{
			new MetadataPropertyInfo(70, 0, "BodyText", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(0, 1, "Database", array[29], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(71, 2, "DatabaseDdlEvents", array[117], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(7, 3, "ExecutionContext", array[39], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(14, 4, "IsEnabled", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(8, 5, "IsEncrypted", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(72, 6, "IsQuotedIdentifierOn", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(73, 7, "IsSqlClr", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 8, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 9, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Database"))
		});
		((MetadataTypeInfo)array[31]).SetProperties(new MetadataPropertyInfo[3]
		{
			new MetadataPropertyInfo(2, 0, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 1, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(6, 2, "Parent", array[31], PropertyType.Abstract, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[0]);
		((MetadataTypeInfo)array[32]).SetProperties(new MetadataPropertyInfo[3]
		{
			new MetadataPropertyInfo(0, 0, "Database", array[29], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(2, 1, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 2, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Database"))
		});
		((MetadataTypeInfo)array[33]).SetProperties(new MetadataPropertyInfo[5]
		{
			new MetadataPropertyInfo(74, 0, "DatabasePrincipal", array[34], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(75, 1, "Grantor", array[34], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(76, 2, "PermissionState", array[142], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(77, 3, "PermissionType", array[10], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(78, 4, "TargetObject", array[82], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(4, 0, "Name", array[146], PropertyType.Inferred, isParent: false, isReadWrite: false, new PropertyValueExpr(PropertValueExprType.RandomGuid))
		});
		((MetadataTypeInfo)array[34]).SetProperties(new MetadataPropertyInfo[5]
		{
			new MetadataPropertyInfo(0, 0, "Database", array[29], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(2, 1, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(3, 2, "MemberOfRoles", array[58], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 3, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(5, 4, "Permissions", array[59], PropertyType.Child, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Database"))
		});
		((MetadataTypeInfo)array[35]).SetProperties(new MetadataPropertyInfo[7]
		{
			new MetadataPropertyInfo(0, 0, "Database", array[29], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(79, 1, "IsFixedRole", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 2, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(3, 3, "MemberOfRoles", array[58], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 4, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(65, 5, "Owner", array[34], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(5, 6, "Permissions", array[59], PropertyType.Child, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Database"))
		});
		((MetadataTypeInfo)array[36]).SetProperties(new MetadataPropertyInfo[7]
		{
			new MetadataPropertyInfo(62, 0, "CollationInfo", array[5], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(80, 1, "Columns", array[84], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(81, 2, "Constraints", array[53], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(82, 3, "Indexes", array[54], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 4, "Name", array[146], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(83, 5, "Statistics", array[55], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(84, 6, "TabularType", array[148], PropertyType.Abstract, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(85, 0, "Unaliased", array[115], PropertyType.Inferred, isParent: false, isReadWrite: false, new PropertyValueExpr(PropertValueExprType.This))
		});
		((MetadataTypeInfo)array[37]).SetProperties(new MetadataPropertyInfo[4]
		{
			new MetadataPropertyInfo(15, 0, "IsSystemNamed", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 1, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(6, 2, "Parent", array[22], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(17, 3, "Text", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[0]);
		((MetadataTypeInfo)array[38]).SetProperties(new MetadataPropertyInfo[16]
		{
			new MetadataPropertyInfo(70, 0, "BodyText", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(86, 1, "Delete", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(87, 2, "DeleteActivationOrder", array[0], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(7, 3, "ExecutionContext", array[39], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(88, 4, "Insert", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(89, 5, "InsertActivationOrder", array[0], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(90, 6, "InsteadOf", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(14, 7, "IsEnabled", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(8, 8, "IsEncrypted", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(72, 9, "IsQuotedIdentifierOn", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(73, 10, "IsSqlClr", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 11, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(16, 12, "NotForReplication", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(6, 13, "Parent", array[114], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(91, 14, "Update", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(92, 15, "UpdateActivationOrder", array[0], PropertyType.Regular, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[0]);
		((MetadataTypeInfo)array[39]).SetProperties(new MetadataPropertyInfo[3]
		{
			new MetadataPropertyInfo(93, 0, "ContextType", array[12], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(94, 1, "Login", array[49], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(95, 2, "User", array[123], PropertyType.Regular, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(4, 0, "Name", array[146], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(null))
		});
		((MetadataTypeInfo)array[40]).SetProperties(new MetadataPropertyInfo[7]
		{
			new MetadataPropertyInfo(7, 0, "ExecutionContext", array[39], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(8, 1, "IsEncrypted", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 2, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 3, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(10, 4, "Parameters", array[86], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(11, 5, "ReturnType", array[93], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(12, 6, "Schema", array[98], PropertyType.Regular, isParent: true, isReadWrite: false, null)
		}, new MetadataPropertyInfo[2]
		{
			new MetadataPropertyInfo(9, 0, "ModuleType", array[4], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(CallableModuleType.ExtendedStoredProcedure)),
			new MetadataPropertyInfo(6, 1, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema"))
		});
		((MetadataTypeInfo)array[41]).SetProperties(new MetadataPropertyInfo[6]
		{
			new MetadataPropertyInfo(0, 0, "Database", array[29], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(96, 1, "IsDefault", array[1], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(97, 2, "IsFileStream", array[1], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(56, 3, "IsReadOnly", array[1], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(2, 4, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 5, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Database"))
		});
		((MetadataTypeInfo)array[42]).SetProperties(new MetadataPropertyInfo[2]
		{
			new MetadataPropertyInfo(98, 0, "ReferencedColumn", array[22], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(99, 1, "ReferencingColumn", array[22], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(4, 0, "Name", array[146], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("ReferencingColumn.Name"))
		});
		((MetadataTypeInfo)array[43]).SetProperties(new MetadataPropertyInfo[10]
		{
			new MetadataPropertyInfo(80, 0, "Columns", array[85], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(100, 1, "DeleteAction", array[13], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(13, 2, "IsChecked", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(14, 3, "IsEnabled", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(15, 4, "IsSystemNamed", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 5, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(16, 6, "NotForReplication", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(6, 7, "Parent", array[115], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(101, 8, "ReferencedTable", array[109], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(102, 9, "UpdateAction", array[13], PropertyType.Regular, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(18, 0, "Type", array[7], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(ConstraintType.ForeignKey))
		});
		((MetadataTypeInfo)array[44]).SetProperties(new MetadataPropertyInfo[2]
		{
			new MetadataPropertyInfo(4, 0, "Name", array[146], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(10, 1, "Parameters", array[86], PropertyType.Regular, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[0]);
		((MetadataTypeInfo)array[45]).SetProperties(new MetadataPropertyInfo[2]
		{
			new MetadataPropertyInfo(4, 0, "Name", array[146], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(10, 1, "Parameters", array[86], PropertyType.Regular, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[0]);
		((MetadataTypeInfo)array[46]).SetProperties(new MetadataPropertyInfo[9]
		{
			new MetadataPropertyInfo(103, 0, "DisallowPageLocks", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(104, 1, "DisallowRowLocks", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(105, 2, "FillFactor", array[2], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(106, 3, "IgnoreDuplicateKeys", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(107, 4, "IsDisabled", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 5, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(108, 6, "PadIndex", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(6, 7, "Parent", array[115], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(18, 8, "Type", array[138], PropertyType.Abstract, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[0]);
		((MetadataTypeInfo)array[47]).SetProperties(new MetadataPropertyInfo[3]
		{
			new MetadataPropertyInfo(109, 0, "IsIncluded", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(98, 1, "ReferencedColumn", array[22], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(110, 2, "SortOrder", array[144], PropertyType.Regular, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(4, 0, "Name", array[146], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("ReferencedColumn.Name"))
		});
		((MetadataTypeInfo)array[48]).SetProperties(new MetadataPropertyInfo[2]
		{
			new MetadataPropertyInfo(37, 0, "DataType", array[28], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(4, 1, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[4]
		{
			new MetadataPropertyInfo(53, 0, "IsCursorVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorVariable))),
			new MetadataPropertyInfo(55, 1, "IsParameter", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IParameter))),
			new MetadataPropertyInfo(57, 2, "IsScalarVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarVariable))),
			new MetadataPropertyInfo(58, 3, "IsTableVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableVariable)))
		});
		((MetadataTypeInfo)array[49]).SetProperties(new MetadataPropertyInfo[11]
		{
			new MetadataPropertyInfo(111, 0, "AsymmetricKey", array[16], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(112, 1, "Certificate", array[18], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(113, 2, "Credential", array[24], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(114, 3, "DefaultDatabase", array[29], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 4, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(115, 5, "Language", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(116, 6, "LoginType", array[141], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(4, 7, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(117, 8, "Password", array[89], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(52, 9, "Server", array[100], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(118, 10, "Sid", array[3], PropertyType.Regular, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Server"))
		});
		((CollectionTypeInfo)array[50]).SetElementType(array[105]);
		((CollectionTypeInfo)array[51]).SetElementType(array[38]);
		((CollectionTypeInfo)array[52]).SetElementType(array[24]);
		((CollectionTypeInfo)array[53]).SetElementType(array[23]);
		((CollectionTypeInfo)array[54]).SetElementType(array[46]);
		((CollectionTypeInfo)array[55]).SetElementType(array[104]);
		((CollectionTypeInfo)array[56]).SetElementType(array[41]);
		((CollectionTypeInfo)array[57]).SetElementType(array[101]);
		((CollectionTypeInfo)array[58]).SetElementType(array[35]);
		((CollectionTypeInfo)array[59]).SetElementType(array[33]);
		((CollectionTypeInfo)array[60]).SetElementType(array[134]);
		((CollectionTypeInfo)array[61]).SetElementType(array[135]);
		((CollectionTypeInfo)array[62]).SetElementType(array[49]);
		((CollectionTypeInfo)array[63]).SetElementType(array[109]);
		((CollectionTypeInfo)array[64]).SetElementType(array[29]);
		((CollectionTypeInfo)array[65]).SetElementType(array[125]);
		((CollectionTypeInfo)array[66]).SetElementType(array[96]);
		((CollectionTypeInfo)array[67]).SetElementType(array[40]);
		((CollectionTypeInfo)array[68]).SetElementType(array[15]);
		((CollectionTypeInfo)array[69]).SetElementType(array[16]);
		((CollectionTypeInfo)array[70]).SetElementType(array[18]);
		((CollectionTypeInfo)array[71]).SetElementType(array[118]);
		((CollectionTypeInfo)array[72]).SetElementType(array[106]);
		((CollectionTypeInfo)array[73]).SetElementType(array[120]);
		((CollectionTypeInfo)array[74]).SetElementType(array[98]);
		((CollectionTypeInfo)array[75]).SetElementType(array[30]);
		((CollectionTypeInfo)array[76]).SetElementType(array[123]);
		((CollectionTypeInfo)array[77]).SetElementType(array[124]);
		((CollectionTypeInfo)array[78]).SetElementType(array[129]);
		((CollectionTypeInfo)array[79]).SetElementType(array[131]);
		((CollectionTypeInfo)array[80]).SetElementType(array[112]);
		((CollectionTypeInfo)array[81]).SetElementType(array[126]);
		((MetadataTypeInfo)array[82]).SetProperties(new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(4, 0, "Name", array[146], PropertyType.Abstract, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[0]);
		((CollectionTypeInfo)array[83]).SetElementType(array[47]);
		((CollectionTypeInfo)array[84]).SetElementType(array[22]);
		((CollectionTypeInfo)array[85]).SetElementType(array[42]);
		((CollectionTypeInfo)array[86]).SetElementType(array[87]);
		((MetadataTypeInfo)array[87]).SetProperties(new MetadataPropertyInfo[5]
		{
			new MetadataPropertyInfo(37, 0, "DataType", array[28], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(38, 1, "DefaultValue", array[146], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(54, 2, "IsOutput", array[1], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(56, 3, "IsReadOnly", array[1], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(4, 4, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[4]
		{
			new MetadataPropertyInfo(53, 0, "IsCursorVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorVariable))),
			new MetadataPropertyInfo(55, 1, "IsParameter", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IParameter))),
			new MetadataPropertyInfo(57, 2, "IsScalarVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarVariable))),
			new MetadataPropertyInfo(58, 3, "IsTableVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableVariable)))
		});
		((MetadataTypeInfo)array[88]).SetProperties(new MetadataPropertyInfo[4]
		{
			new MetadataPropertyInfo(0, 0, "Database", array[29], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(119, 1, "FileGroups", array[56], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 2, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 3, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Database"))
		});
		((MetadataTypeInfo)array[90]).SetProperties(new MetadataPropertyInfo[4]
		{
			new MetadataPropertyInfo(120, 0, "AssociatedIndex", array[91], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(15, 1, "IsSystemNamed", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 2, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(6, 3, "Parent", array[115], PropertyType.Regular, isParent: true, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(18, 0, "Type", array[7], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(ConstraintType.PrimaryKey))
		});
		((MetadataTypeInfo)array[91]).SetProperties(new MetadataPropertyInfo[20]
		{
			new MetadataPropertyInfo(121, 0, "CompactLargeObjects", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(103, 1, "DisallowPageLocks", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(104, 2, "DisallowRowLocks", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(122, 3, "FileGroup", array[41], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(123, 4, "FileStreamFileGroup", array[41], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(124, 5, "FileStreamPartitionScheme", array[88], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(105, 6, "FillFactor", array[2], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(125, 7, "FilterDefinition", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(106, 8, "IgnoreDuplicateKeys", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(126, 9, "IndexKey", array[122], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(127, 10, "IndexedColumns", array[83], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(128, 11, "IsClustered", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(107, 12, "IsDisabled", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(15, 13, "IsSystemNamed", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(129, 14, "IsUnique", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 15, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(130, 16, "NoAutomaticRecomputation", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(108, 17, "PadIndex", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(6, 18, "Parent", array[115], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(131, 19, "PartitionScheme", array[88], PropertyType.Child, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(18, 0, "Type", array[138], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(IndexType.Relational))
		});
		((MetadataTypeInfo)array[92]).SetProperties(new MetadataPropertyInfo[4]
		{
			new MetadataPropertyInfo(37, 0, "DataType", array[93], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 1, "Name", array[146], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(49, 2, "Nullable", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(51, 3, "ScalarType", array[143], PropertyType.Abstract, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[0]);
		((MetadataTypeInfo)array[93]).SetProperties(new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(4, 0, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[9]
		{
			new MetadataPropertyInfo(20, 0, "BaseSystemDataType", array[108], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(null)),
			new MetadataPropertyInfo(24, 1, "IsClr", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IClrDataType))),
			new MetadataPropertyInfo(26, 2, "IsCursor", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorDataType))),
			new MetadataPropertyInfo(28, 3, "IsScalar", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarDataType))),
			new MetadataPropertyInfo(29, 4, "IsSystem", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ISystemDataType))),
			new MetadataPropertyInfo(30, 5, "IsTable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableDataType))),
			new MetadataPropertyInfo(31, 6, "IsUnknown", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(true)),
			new MetadataPropertyInfo(32, 7, "IsVoid", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IVoidDataType))),
			new MetadataPropertyInfo(33, 8, "IsXml", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IXmlDataType)))
		});
		((MetadataTypeInfo)array[94]).SetProperties(new MetadataPropertyInfo[6]
		{
			new MetadataPropertyInfo(37, 0, "DataType", array[93], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(132, 1, "IsAggregateFunction", array[1], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(4, 2, "Name", array[146], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(49, 3, "Nullable", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(10, 4, "Parameters", array[86], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(51, 5, "ScalarType", array[143], PropertyType.Abstract, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[0]);
		((MetadataTypeInfo)array[95]).SetProperties(new MetadataPropertyInfo[5]
		{
			new MetadataPropertyInfo(37, 0, "DataType", array[93], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(38, 1, "DefaultValue", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(54, 2, "IsOutput", array[1], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(4, 3, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(49, 4, "Nullable", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[6]
		{
			new MetadataPropertyInfo(53, 0, "IsCursorVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorVariable))),
			new MetadataPropertyInfo(55, 1, "IsParameter", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IParameter))),
			new MetadataPropertyInfo(56, 2, "IsReadOnly", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(false)),
			new MetadataPropertyInfo(57, 3, "IsScalarVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarVariable))),
			new MetadataPropertyInfo(58, 4, "IsTableVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableVariable))),
			new MetadataPropertyInfo(51, 5, "ScalarType", array[143], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(ScalarType.ScalarVariable))
		});
		((MetadataTypeInfo)array[96]).SetProperties(new MetadataPropertyInfo[13]
		{
			new MetadataPropertyInfo(70, 0, "BodyText", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(37, 1, "DataType", array[93], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(7, 2, "ExecutionContext", array[39], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(8, 3, "IsEncrypted", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(72, 4, "IsQuotedIdentifierOn", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(133, 5, "IsSchemaBound", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(73, 6, "IsSqlClr", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 7, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 8, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(49, 9, "Nullable", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(10, 10, "Parameters", array[86], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(134, 11, "ReturnsNullOnNullInput", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(12, 12, "Schema", array[98], PropertyType.Regular, isParent: true, isReadWrite: false, null)
		}, new MetadataPropertyInfo[5]
		{
			new MetadataPropertyInfo(132, 0, "IsAggregateFunction", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(false)),
			new MetadataPropertyInfo(9, 1, "ModuleType", array[4], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(CallableModuleType.ScalarFunction)),
			new MetadataPropertyInfo(6, 2, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema")),
			new MetadataPropertyInfo(11, 3, "ReturnType", array[93], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("DataType")),
			new MetadataPropertyInfo(51, 4, "ScalarType", array[143], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(ScalarType.ScalarFunction))
		});
		((MetadataTypeInfo)array[97]).SetProperties(new MetadataPropertyInfo[3]
		{
			new MetadataPropertyInfo(37, 0, "DataType", array[93], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(4, 1, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(49, 2, "Nullable", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[5]
		{
			new MetadataPropertyInfo(53, 0, "IsCursorVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorVariable))),
			new MetadataPropertyInfo(55, 1, "IsParameter", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IParameter))),
			new MetadataPropertyInfo(57, 2, "IsScalarVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarVariable))),
			new MetadataPropertyInfo(58, 3, "IsTableVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableVariable))),
			new MetadataPropertyInfo(51, 4, "ScalarType", array[143], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(ScalarType.ScalarVariable))
		});
		((MetadataTypeInfo)array[98]).SetProperties(new MetadataPropertyInfo[15]
		{
			new MetadataPropertyInfo(0, 0, "Database", array[29], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(135, 1, "ExtendedStoredProcedures", array[67], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 2, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 3, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(65, 4, "Owner", array[34], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(136, 5, "ScalarValuedFunctions", array[66], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(137, 6, "StoredProcedures", array[50], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(138, 7, "Synonyms", array[72], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(139, 8, "TableValuedFunctions", array[80], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(140, 9, "Tables", array[63], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(141, 10, "UserDefinedAggregates", array[77], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(142, 11, "UserDefinedClrTypes", array[65], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(143, 12, "UserDefinedDataTypes", array[81], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(144, 13, "UserDefinedTableTypes", array[78], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(145, 14, "Views", array[79], PropertyType.Child, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Database"))
		});
		((MetadataTypeInfo)array[99]).SetProperties(new MetadataPropertyInfo[3]
		{
			new MetadataPropertyInfo(2, 0, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 1, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(12, 2, "Schema", array[98], PropertyType.Regular, isParent: true, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema"))
		});
		((MetadataTypeInfo)array[100]).SetProperties(new MetadataPropertyInfo[7]
		{
			new MetadataPropertyInfo(62, 0, "CollationInfo", array[5], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(146, 1, "Credentials", array[52], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(147, 2, "Databases", array[64], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 3, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(148, 4, "Logins", array[62], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 5, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(68, 6, "Triggers", array[57], PropertyType.Child, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(null))
		});
		((MetadataTypeInfo)array[101]).SetProperties(new MetadataPropertyInfo[10]
		{
			new MetadataPropertyInfo(70, 0, "BodyText", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(7, 1, "ExecutionContext", array[39], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(14, 2, "IsEnabled", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(8, 3, "IsEncrypted", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(72, 4, "IsQuotedIdentifierOn", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(73, 5, "IsSqlClr", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 6, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 7, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(52, 8, "Server", array[100], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(149, 9, "ServerDdlEvents", array[117], PropertyType.Regular, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Server"))
		});
		((MetadataTypeInfo)array[102]).SetProperties(new MetadataPropertyInfo[3]
		{
			new MetadataPropertyInfo(2, 0, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 1, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(52, 2, "Server", array[100], PropertyType.Regular, isParent: true, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Server"))
		});
		((MetadataTypeInfo)array[103]).SetProperties(new MetadataPropertyInfo[19]
		{
			new MetadataPropertyInfo(150, 0, "BoundingBoxXMax", array[11], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(151, 1, "BoundingBoxXMin", array[11], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(152, 2, "BoundingBoxYMax", array[11], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(153, 3, "BoundingBoxYMin", array[11], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(154, 4, "CellsPerObject", array[139], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(103, 5, "DisallowPageLocks", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(104, 6, "DisallowRowLocks", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(105, 7, "FillFactor", array[2], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(106, 8, "IgnoreDuplicateKeys", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(155, 9, "IndexedColumn", array[22], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(107, 10, "IsDisabled", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(156, 11, "Level1Density", array[14], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(157, 12, "Level2Density", array[14], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(158, 13, "Level3Density", array[14], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(159, 14, "Level4Density", array[14], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 15, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(130, 16, "NoAutomaticRecomputation", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(108, 17, "PadIndex", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(6, 18, "Parent", array[115], PropertyType.Regular, isParent: true, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(18, 0, "Type", array[138], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(IndexType.Spatial))
		});
		((MetadataTypeInfo)array[104]).SetProperties(new MetadataPropertyInfo[6]
		{
			new MetadataPropertyInfo(80, 0, "Columns", array[84], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(125, 1, "FilterDefinition", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 2, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(130, 3, "NoAutomaticRecomputation", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(6, 4, "Parent", array[115], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(18, 5, "Type", array[145], PropertyType.Regular, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[0]);
		((MetadataTypeInfo)array[105]).SetProperties(new MetadataPropertyInfo[12]
		{
			new MetadataPropertyInfo(70, 0, "BodyText", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(7, 1, "ExecutionContext", array[39], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(160, 2, "ForReplication", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(8, 3, "IsEncrypted", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(72, 4, "IsQuotedIdentifierOn", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(161, 5, "IsRecompiled", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(73, 6, "IsSqlClr", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 7, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 8, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(10, 9, "Parameters", array[86], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(12, 10, "Schema", array[98], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(162, 11, "Startup", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[3]
		{
			new MetadataPropertyInfo(9, 0, "ModuleType", array[4], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(CallableModuleType.StoredProcedure)),
			new MetadataPropertyInfo(6, 1, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema")),
			new MetadataPropertyInfo(11, 2, "ReturnType", array[93], PropertyType.Inferred, isParent: false, isReadWrite: false, new PropertyValueExpr(PropertValueExprType.IntDataType))
		});
		((MetadataTypeInfo)array[106]).SetProperties(new MetadataPropertyInfo[5]
		{
			new MetadataPropertyInfo(163, 0, "BaseObjectName", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(164, 1, "BaseType", array[147], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 2, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 3, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(12, 4, "Schema", array[98], PropertyType.Regular, isParent: true, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema"))
		});
		((MetadataTypeInfo)array[107]).SetProperties(new MetadataPropertyInfo[4]
		{
			new MetadataPropertyInfo(165, 0, "Length", array[139], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(166, 1, "NumericPrecision", array[139], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(167, 2, "NumericScale", array[139], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(168, 3, "TypeSpec", array[8], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[17]
		{
			new MetadataPropertyInfo(19, 0, "AssemblyName", array[146], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(null)),
			new MetadataPropertyInfo(20, 1, "BaseSystemDataType", array[108], PropertyType.Inferred, isParent: false, isReadWrite: false, new PropertyValueExpr(PropertValueExprType.This)),
			new MetadataPropertyInfo(21, 2, "ClassName", array[146], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(null)),
			new MetadataPropertyInfo(22, 3, "DataMembers", array[71], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(null)),
			new MetadataPropertyInfo(23, 4, "IsBinaryOrdered", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(false)),
			new MetadataPropertyInfo(24, 5, "IsClr", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IClrDataType))),
			new MetadataPropertyInfo(25, 6, "IsComVisible", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(false)),
			new MetadataPropertyInfo(26, 7, "IsCursor", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorDataType))),
			new MetadataPropertyInfo(27, 8, "IsNullable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(false)),
			new MetadataPropertyInfo(28, 9, "IsScalar", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarDataType))),
			new MetadataPropertyInfo(29, 10, "IsSystem", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ISystemDataType))),
			new MetadataPropertyInfo(30, 11, "IsTable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableDataType))),
			new MetadataPropertyInfo(31, 12, "IsUnknown", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(false)),
			new MetadataPropertyInfo(32, 13, "IsVoid", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IVoidDataType))),
			new MetadataPropertyInfo(33, 14, "IsXml", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IXmlDataType))),
			new MetadataPropertyInfo(34, 15, "Methods", array[73], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(null)),
			new MetadataPropertyInfo(4, 16, "Name", array[146], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("TypeSpec.Name"))
		});
		((MetadataTypeInfo)array[108]).SetProperties(new MetadataPropertyInfo[4]
		{
			new MetadataPropertyInfo(165, 0, "Length", array[139], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(166, 1, "NumericPrecision", array[139], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(167, 2, "NumericScale", array[139], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(168, 3, "TypeSpec", array[8], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[10]
		{
			new MetadataPropertyInfo(20, 0, "BaseSystemDataType", array[108], PropertyType.Inferred, isParent: false, isReadWrite: false, new PropertyValueExpr(PropertValueExprType.This)),
			new MetadataPropertyInfo(24, 1, "IsClr", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IClrDataType))),
			new MetadataPropertyInfo(26, 2, "IsCursor", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorDataType))),
			new MetadataPropertyInfo(28, 3, "IsScalar", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarDataType))),
			new MetadataPropertyInfo(29, 4, "IsSystem", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ISystemDataType))),
			new MetadataPropertyInfo(30, 5, "IsTable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableDataType))),
			new MetadataPropertyInfo(31, 6, "IsUnknown", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(false)),
			new MetadataPropertyInfo(32, 7, "IsVoid", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IVoidDataType))),
			new MetadataPropertyInfo(33, 8, "IsXml", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IXmlDataType))),
			new MetadataPropertyInfo(4, 9, "Name", array[146], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("TypeSpec.Name"))
		});
		((MetadataTypeInfo)array[109]).SetProperties(new MetadataPropertyInfo[9]
		{
			new MetadataPropertyInfo(80, 0, "Columns", array[84], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(81, 1, "Constraints", array[53], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(82, 2, "Indexes", array[54], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(72, 3, "IsQuotedIdentifierOn", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 4, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 5, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(12, 6, "Schema", array[98], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(83, 7, "Statistics", array[55], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(68, 8, "Triggers", array[51], PropertyType.Child, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[4]
		{
			new MetadataPropertyInfo(62, 0, "CollationInfo", array[5], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema.Database.CollationInfo")),
			new MetadataPropertyInfo(6, 1, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema")),
			new MetadataPropertyInfo(84, 2, "TabularType", array[148], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(TabularType.Table)),
			new MetadataPropertyInfo(85, 3, "Unaliased", array[115], PropertyType.Inferred, isParent: false, isReadWrite: false, new PropertyValueExpr(PropertValueExprType.This))
		});
		((MetadataTypeInfo)array[110]).SetProperties(new MetadataPropertyInfo[5]
		{
			new MetadataPropertyInfo(80, 0, "Columns", array[84], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(81, 1, "Constraints", array[53], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(82, 2, "Indexes", array[54], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 3, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(83, 4, "Statistics", array[55], PropertyType.Child, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[7]
		{
			new MetadataPropertyInfo(62, 0, "CollationInfo", array[5], PropertyType.Inferred, isParent: false, isReadWrite: false, new PropertyValueExpr(PropertValueExprType.DefaultCollationInfo)),
			new MetadataPropertyInfo(26, 1, "IsCursor", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorDataType))),
			new MetadataPropertyInfo(28, 2, "IsScalar", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarDataType))),
			new MetadataPropertyInfo(30, 3, "IsTable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableDataType))),
			new MetadataPropertyInfo(31, 4, "IsUnknown", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(true)),
			new MetadataPropertyInfo(84, 5, "TabularType", array[148], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(TabularType.TableDataType)),
			new MetadataPropertyInfo(85, 6, "Unaliased", array[115], PropertyType.Inferred, isParent: false, isReadWrite: false, new PropertyValueExpr(PropertValueExprType.This))
		});
		((MetadataTypeInfo)array[111]).SetProperties(new MetadataPropertyInfo[2]
		{
			new MetadataPropertyInfo(37, 0, "DataType", array[110], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(4, 1, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[10]
		{
			new MetadataPropertyInfo(80, 0, "Columns", array[84], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("DataType.Columns")),
			new MetadataPropertyInfo(38, 1, "DefaultValue", array[146], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(null)),
			new MetadataPropertyInfo(53, 2, "IsCursorVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorVariable))),
			new MetadataPropertyInfo(54, 3, "IsOutput", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(false)),
			new MetadataPropertyInfo(55, 4, "IsParameter", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IParameter))),
			new MetadataPropertyInfo(56, 5, "IsReadOnly", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(true)),
			new MetadataPropertyInfo(57, 6, "IsScalarVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarVariable))),
			new MetadataPropertyInfo(58, 7, "IsTableVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableVariable))),
			new MetadataPropertyInfo(84, 8, "TabularType", array[148], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(TabularType.TableVariable)),
			new MetadataPropertyInfo(85, 9, "Unaliased", array[115], PropertyType.Inferred, isParent: false, isReadWrite: false, new PropertyValueExpr(PropertValueExprType.This))
		});
		((MetadataTypeInfo)array[112]).SetProperties(new MetadataPropertyInfo[16]
		{
			new MetadataPropertyInfo(70, 0, "BodyText", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(80, 1, "Columns", array[84], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(81, 2, "Constraints", array[53], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(7, 3, "ExecutionContext", array[39], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(82, 4, "Indexes", array[54], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(8, 5, "IsEncrypted", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(169, 6, "IsInline", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(72, 7, "IsQuotedIdentifierOn", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(133, 8, "IsSchemaBound", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(73, 9, "IsSqlClr", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 10, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 11, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(10, 12, "Parameters", array[86], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(12, 13, "Schema", array[98], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(83, 14, "Statistics", array[55], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(170, 15, "TableVariableName", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[4]
		{
			new MetadataPropertyInfo(62, 0, "CollationInfo", array[5], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema.Database.CollationInfo")),
			new MetadataPropertyInfo(6, 1, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema")),
			new MetadataPropertyInfo(84, 2, "TabularType", array[148], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(TabularType.TableValuedFunction)),
			new MetadataPropertyInfo(85, 3, "Unaliased", array[115], PropertyType.Inferred, isParent: false, isReadWrite: false, new PropertyValueExpr(PropertValueExprType.This))
		});
		((MetadataTypeInfo)array[113]).SetProperties(new MetadataPropertyInfo[2]
		{
			new MetadataPropertyInfo(37, 0, "DataType", array[110], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(4, 1, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[7]
		{
			new MetadataPropertyInfo(80, 0, "Columns", array[84], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("DataType.Columns")),
			new MetadataPropertyInfo(53, 1, "IsCursorVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorVariable))),
			new MetadataPropertyInfo(55, 2, "IsParameter", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IParameter))),
			new MetadataPropertyInfo(57, 3, "IsScalarVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarVariable))),
			new MetadataPropertyInfo(58, 4, "IsTableVariable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableVariable))),
			new MetadataPropertyInfo(84, 5, "TabularType", array[148], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(TabularType.TableVariable)),
			new MetadataPropertyInfo(85, 6, "Unaliased", array[115], PropertyType.Inferred, isParent: false, isReadWrite: false, new PropertyValueExpr(PropertValueExprType.This))
		});
		((MetadataTypeInfo)array[114]).SetProperties(new MetadataPropertyInfo[10]
		{
			new MetadataPropertyInfo(80, 0, "Columns", array[84], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(81, 1, "Constraints", array[53], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(82, 2, "Indexes", array[54], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(72, 3, "IsQuotedIdentifierOn", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 4, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 5, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(12, 6, "Schema", array[98], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(83, 7, "Statistics", array[55], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(84, 8, "TabularType", array[148], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(68, 9, "Triggers", array[51], PropertyType.Child, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[3]
		{
			new MetadataPropertyInfo(62, 0, "CollationInfo", array[5], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema.Database.CollationInfo")),
			new MetadataPropertyInfo(6, 1, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema")),
			new MetadataPropertyInfo(85, 2, "Unaliased", array[115], PropertyType.Inferred, isParent: false, isReadWrite: false, new PropertyValueExpr(PropertValueExprType.This))
		});
		((MetadataTypeInfo)array[115]).SetProperties(new MetadataPropertyInfo[4]
		{
			new MetadataPropertyInfo(80, 0, "Columns", array[84], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(4, 1, "Name", array[146], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(84, 2, "TabularType", array[148], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(85, 3, "Unaliased", array[115], PropertyType.Abstract, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[0]);
		((MetadataTypeInfo)array[116]).SetProperties(new MetadataPropertyInfo[6]
		{
			new MetadataPropertyInfo(70, 0, "BodyText", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(7, 1, "ExecutionContext", array[39], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(14, 2, "IsEnabled", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(8, 3, "IsEncrypted", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(73, 4, "IsSqlClr", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 5, "Name", array[146], PropertyType.Abstract, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[0]);
		((MetadataTypeInfo)array[118]).SetProperties(new MetadataPropertyInfo[5]
		{
			new MetadataPropertyInfo(37, 0, "DataType", array[93], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(171, 1, "IsStatic", array[1], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(4, 2, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(49, 3, "Nullable", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(172, 4, "UserDefinedType", array[20], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(51, 0, "ScalarType", array[143], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(ScalarType.UdtMember))
		});
		((MetadataTypeInfo)array[119]).SetProperties(new MetadataPropertyInfo[5]
		{
			new MetadataPropertyInfo(37, 0, "DataType", array[93], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(171, 1, "IsStatic", array[1], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(4, 2, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(49, 3, "Nullable", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(172, 4, "UserDefinedType", array[20], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(51, 0, "ScalarType", array[143], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(ScalarType.UdtMember))
		});
		((MetadataTypeInfo)array[120]).SetProperties(new MetadataPropertyInfo[6]
		{
			new MetadataPropertyInfo(37, 0, "DataType", array[93], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(171, 1, "IsStatic", array[1], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(4, 2, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(49, 3, "Nullable", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(10, 4, "Parameters", array[86], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(172, 5, "UserDefinedType", array[20], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[2]
		{
			new MetadataPropertyInfo(132, 0, "IsAggregateFunction", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(false)),
			new MetadataPropertyInfo(51, 1, "ScalarType", array[143], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(ScalarType.UdtMember))
		});
		((MetadataTypeInfo)array[121]).SetProperties(new MetadataPropertyInfo[4]
		{
			new MetadataPropertyInfo(120, 0, "AssociatedIndex", array[91], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(15, 1, "IsSystemNamed", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 2, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(6, 3, "Parent", array[115], PropertyType.Regular, isParent: true, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(18, 0, "Type", array[7], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(ConstraintType.Unique))
		});
		((MetadataTypeInfo)array[122]).SetProperties(new MetadataPropertyInfo[5]
		{
			new MetadataPropertyInfo(120, 0, "AssociatedIndex", array[91], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(15, 1, "IsSystemNamed", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 2, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(6, 3, "Parent", array[115], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(18, 4, "Type", array[7], PropertyType.Abstract, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[0]);
		((MetadataTypeInfo)array[123]).SetProperties(new MetadataPropertyInfo[11]
		{
			new MetadataPropertyInfo(111, 0, "AsymmetricKey", array[16], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(112, 1, "Certificate", array[18], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(0, 2, "Database", array[29], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(1, 3, "DefaultSchema", array[98], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 4, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(94, 5, "Login", array[49], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(3, 6, "MemberOfRoles", array[58], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 7, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(117, 8, "Password", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(5, 9, "Permissions", array[59], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(173, 10, "UserType", array[149], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Database"))
		});
		((MetadataTypeInfo)array[124]).SetProperties(new MetadataPropertyInfo[6]
		{
			new MetadataPropertyInfo(37, 0, "DataType", array[93], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 1, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 2, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(49, 3, "Nullable", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(10, 4, "Parameters", array[86], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(12, 5, "Schema", array[98], PropertyType.Regular, isParent: true, isReadWrite: false, null)
		}, new MetadataPropertyInfo[3]
		{
			new MetadataPropertyInfo(132, 0, "IsAggregateFunction", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(true)),
			new MetadataPropertyInfo(6, 1, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema")),
			new MetadataPropertyInfo(51, 2, "ScalarType", array[143], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(ScalarType.ScalarFunction))
		});
		((MetadataTypeInfo)array[125]).SetProperties(new MetadataPropertyInfo[10]
		{
			new MetadataPropertyInfo(19, 0, "AssemblyName", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(21, 1, "ClassName", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(22, 2, "DataMembers", array[71], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(23, 3, "IsBinaryOrdered", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(25, 4, "IsComVisible", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(27, 5, "IsNullable", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 6, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(34, 7, "Methods", array[73], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 8, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(12, 9, "Schema", array[98], PropertyType.Regular, isParent: true, isReadWrite: false, null)
		}, new MetadataPropertyInfo[10]
		{
			new MetadataPropertyInfo(20, 0, "BaseSystemDataType", array[108], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(null)),
			new MetadataPropertyInfo(24, 1, "IsClr", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IClrDataType))),
			new MetadataPropertyInfo(26, 2, "IsCursor", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorDataType))),
			new MetadataPropertyInfo(28, 3, "IsScalar", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarDataType))),
			new MetadataPropertyInfo(29, 4, "IsSystem", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ISystemDataType))),
			new MetadataPropertyInfo(30, 5, "IsTable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableDataType))),
			new MetadataPropertyInfo(31, 6, "IsUnknown", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(false)),
			new MetadataPropertyInfo(32, 7, "IsVoid", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IVoidDataType))),
			new MetadataPropertyInfo(33, 8, "IsXml", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IXmlDataType))),
			new MetadataPropertyInfo(6, 9, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema"))
		});
		((MetadataTypeInfo)array[126]).SetProperties(new MetadataPropertyInfo[5]
		{
			new MetadataPropertyInfo(20, 0, "BaseSystemDataType", array[108], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 1, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 2, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(49, 3, "Nullable", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(12, 4, "Schema", array[98], PropertyType.Regular, isParent: true, isReadWrite: false, null)
		}, new MetadataPropertyInfo[9]
		{
			new MetadataPropertyInfo(24, 0, "IsClr", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IClrDataType))),
			new MetadataPropertyInfo(26, 1, "IsCursor", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorDataType))),
			new MetadataPropertyInfo(28, 2, "IsScalar", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarDataType))),
			new MetadataPropertyInfo(29, 3, "IsSystem", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ISystemDataType))),
			new MetadataPropertyInfo(30, 4, "IsTable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableDataType))),
			new MetadataPropertyInfo(31, 5, "IsUnknown", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(false)),
			new MetadataPropertyInfo(32, 6, "IsVoid", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IVoidDataType))),
			new MetadataPropertyInfo(33, 7, "IsXml", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IXmlDataType))),
			new MetadataPropertyInfo(6, 8, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema"))
		});
		((MetadataTypeInfo)array[127]).SetProperties(new MetadataPropertyInfo[10]
		{
			new MetadataPropertyInfo(70, 0, "BodyText", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(7, 1, "ExecutionContext", array[39], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(8, 2, "IsEncrypted", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(72, 3, "IsQuotedIdentifierOn", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(133, 4, "IsSchemaBound", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(73, 5, "IsSqlClr", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 6, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 7, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(10, 8, "Parameters", array[86], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(12, 9, "Schema", array[98], PropertyType.Regular, isParent: true, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema"))
		});
		((MetadataTypeInfo)array[128]).SetProperties(new MetadataPropertyInfo[5]
		{
			new MetadataPropertyInfo(7, 0, "ExecutionContext", array[39], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(8, 1, "IsEncrypted", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 2, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 3, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(12, 4, "Schema", array[98], PropertyType.Regular, isParent: true, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(6, 0, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema"))
		});
		((MetadataTypeInfo)array[129]).SetProperties(new MetadataPropertyInfo[7]
		{
			new MetadataPropertyInfo(80, 0, "Columns", array[84], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(81, 1, "Constraints", array[53], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(82, 2, "Indexes", array[54], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 3, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 4, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(12, 5, "Schema", array[98], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(83, 6, "Statistics", array[55], PropertyType.Child, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[8]
		{
			new MetadataPropertyInfo(62, 0, "CollationInfo", array[5], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema.Database.CollationInfo")),
			new MetadataPropertyInfo(26, 1, "IsCursor", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorDataType))),
			new MetadataPropertyInfo(28, 2, "IsScalar", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarDataType))),
			new MetadataPropertyInfo(30, 3, "IsTable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableDataType))),
			new MetadataPropertyInfo(31, 4, "IsUnknown", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(false)),
			new MetadataPropertyInfo(6, 5, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema")),
			new MetadataPropertyInfo(84, 6, "TabularType", array[148], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(TabularType.TableDataType)),
			new MetadataPropertyInfo(85, 7, "Unaliased", array[115], PropertyType.Inferred, isParent: false, isReadWrite: false, new PropertyValueExpr(PropertValueExprType.This))
		});
		((MetadataTypeInfo)array[130]).SetProperties(new MetadataPropertyInfo[4]
		{
			new MetadataPropertyInfo(2, 0, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(31, 1, "IsUnknown", array[1], PropertyType.Abstract, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(4, 2, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(12, 3, "Schema", array[98], PropertyType.Regular, isParent: true, isReadWrite: false, null)
		}, new MetadataPropertyInfo[4]
		{
			new MetadataPropertyInfo(26, 0, "IsCursor", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorDataType))),
			new MetadataPropertyInfo(28, 1, "IsScalar", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarDataType))),
			new MetadataPropertyInfo(30, 2, "IsTable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableDataType))),
			new MetadataPropertyInfo(6, 3, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema"))
		});
		((MetadataTypeInfo)array[131]).SetProperties(new MetadataPropertyInfo[15]
		{
			new MetadataPropertyInfo(80, 0, "Columns", array[84], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(81, 1, "Constraints", array[53], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(174, 2, "HasCheckOption", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(175, 3, "HasColumnSpecification", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(82, 4, "Indexes", array[54], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(8, 5, "IsEncrypted", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(72, 6, "IsQuotedIdentifierOn", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(133, 7, "IsSchemaBound", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(2, 8, "IsSystemObject", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 9, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(176, 10, "QueryText", array[146], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(177, 11, "ReturnsViewMetadata", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(12, 12, "Schema", array[98], PropertyType.Regular, isParent: true, isReadWrite: false, null),
			new MetadataPropertyInfo(83, 13, "Statistics", array[55], PropertyType.Child, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(68, 14, "Triggers", array[51], PropertyType.Child, isParent: false, isReadWrite: true, null)
		}, new MetadataPropertyInfo[4]
		{
			new MetadataPropertyInfo(62, 0, "CollationInfo", array[5], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema.Database.CollationInfo")),
			new MetadataPropertyInfo(6, 1, "Parent", array[31], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("Schema")),
			new MetadataPropertyInfo(84, 2, "TabularType", array[148], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(TabularType.View)),
			new MetadataPropertyInfo(85, 3, "Unaliased", array[115], PropertyType.Inferred, isParent: false, isReadWrite: false, new PropertyValueExpr(PropertValueExprType.This))
		});
		((MetadataTypeInfo)array[132]).SetProperties(new MetadataPropertyInfo[0], new MetadataPropertyInfo[10]
		{
			new MetadataPropertyInfo(20, 0, "BaseSystemDataType", array[108], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(null)),
			new MetadataPropertyInfo(24, 1, "IsClr", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IClrDataType))),
			new MetadataPropertyInfo(26, 2, "IsCursor", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorDataType))),
			new MetadataPropertyInfo(28, 3, "IsScalar", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarDataType))),
			new MetadataPropertyInfo(29, 4, "IsSystem", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ISystemDataType))),
			new MetadataPropertyInfo(30, 5, "IsTable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableDataType))),
			new MetadataPropertyInfo(31, 6, "IsUnknown", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(false)),
			new MetadataPropertyInfo(32, 7, "IsVoid", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IVoidDataType))),
			new MetadataPropertyInfo(33, 8, "IsXml", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IXmlDataType))),
			new MetadataPropertyInfo(4, 9, "Name", array[146], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr("void"))
		});
		((MetadataTypeInfo)array[133]).SetProperties(new MetadataPropertyInfo[5]
		{
			new MetadataPropertyInfo(178, 0, "IsXmlNode", array[1], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(165, 1, "Length", array[139], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(166, 2, "NumericPrecision", array[139], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(167, 3, "NumericScale", array[139], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(168, 4, "TypeSpec", array[8], PropertyType.Regular, isParent: false, isReadWrite: false, null)
		}, new MetadataPropertyInfo[12]
		{
			new MetadataPropertyInfo(20, 0, "BaseSystemDataType", array[108], PropertyType.Inferred, isParent: false, isReadWrite: false, new PropertyValueExpr(PropertValueExprType.This)),
			new MetadataPropertyInfo(24, 1, "IsClr", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IClrDataType))),
			new MetadataPropertyInfo(26, 2, "IsCursor", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ICursorDataType))),
			new MetadataPropertyInfo(28, 3, "IsScalar", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IScalarDataType))),
			new MetadataPropertyInfo(29, 4, "IsSystem", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ISystemDataType))),
			new MetadataPropertyInfo(30, 5, "IsTable", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(ITableDataType))),
			new MetadataPropertyInfo(31, 6, "IsUnknown", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(false)),
			new MetadataPropertyInfo(32, 7, "IsVoid", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IVoidDataType))),
			new MetadataPropertyInfo(33, 8, "IsXml", array[1], PropertyType.Inferred, isParent: false, isReadWrite: false, new IsTypePropertyValueExpr(typeof(IXmlDataType))),
			new MetadataPropertyInfo(34, 9, "Methods", array[60], PropertyType.Inferred, isParent: false, isReadWrite: false, new PropertyValueExpr(PropertValueExprType.XmlMethods)),
			new MetadataPropertyInfo(4, 10, "Name", array[146], PropertyType.Inferred, isParent: false, isReadWrite: false, new DuplicatePropertyValueExpr("TypeSpec.Name")),
			new MetadataPropertyInfo(179, 11, "TableMethods", array[61], PropertyType.Inferred, isParent: false, isReadWrite: false, new PropertyValueExpr(PropertValueExprType.XmlTableMethods))
		});
		((MetadataTypeInfo)array[136]).SetProperties(new MetadataPropertyInfo[8]
		{
			new MetadataPropertyInfo(103, 0, "DisallowPageLocks", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(104, 1, "DisallowRowLocks", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(105, 2, "FillFactor", array[2], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(106, 3, "IgnoreDuplicateKeys", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(107, 4, "IsDisabled", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(4, 5, "Name", array[146], PropertyType.Regular, isParent: false, isReadWrite: false, null),
			new MetadataPropertyInfo(108, 6, "PadIndex", array[1], PropertyType.Regular, isParent: false, isReadWrite: true, null),
			new MetadataPropertyInfo(6, 7, "Parent", array[115], PropertyType.Regular, isParent: true, isReadWrite: false, null)
		}, new MetadataPropertyInfo[1]
		{
			new MetadataPropertyInfo(18, 0, "Type", array[138], PropertyType.Inferred, isParent: false, isReadWrite: false, new ConstantPropertyValueExpr(IndexType.Xml))
		});
		metadataInfo = new MetadataInfo(array);
	}
}
