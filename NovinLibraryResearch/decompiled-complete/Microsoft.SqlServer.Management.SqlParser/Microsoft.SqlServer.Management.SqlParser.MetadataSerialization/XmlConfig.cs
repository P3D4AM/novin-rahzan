using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal static class XmlConfig
{
	public static XmlNamespace MM = new XmlNamespace("MM", "http://schemas.microsoft.com/sqlserver/ManagementModel/Serialization/2011/03");

	public static XmlQualifiedNameEx RootElement = new XmlQualifiedNameEx("Instances", MM.Urn);

	public static XmlQualifiedNameEx ReferenceElement = new XmlQualifiedNameEx("Reference", MM.Urn);

	public static XmlQualifiedNameEx KeyAttribute = new XmlQualifiedNameEx("Key", MM.Urn);

	public static XmlQualifiedNameEx RefAttribute = new XmlQualifiedNameEx("ReferenceKey", MM.Urn);

	public static XmlNamespace RE = new XmlNamespace("RE", "http://schemas.microsoft.com/sqlserver/RelationalEngine/Serialization/2011/03");

	public static XmlQualifiedNameEx ParentProperty = new XmlQualifiedNameEx("Parent", RE.Urn);

	public static XmlQualifiedNameEx TextProperty = new XmlQualifiedNameEx("Text", RE.Urn);

	public static XmlQualifiedNameEx IsPersistedProperty = new XmlQualifiedNameEx("IsPersisted", RE.Urn);

	public static XmlQualifiedNameEx SeedProperty = new XmlQualifiedNameEx("Seed", RE.Urn);

	public static XmlQualifiedNameEx IncrementProperty = new XmlQualifiedNameEx("Increment", RE.Urn);

	public static XmlQualifiedNameEx NotForReplicationProperty = new XmlQualifiedNameEx("NotForReplication", RE.Urn);

	public static XmlQualifiedNameEx IApplicationRole = new XmlQualifiedNameEx("ApplicationRole", RE.Urn);

	public static XmlQualifiedNameEx IApplicationRole_Database = new XmlQualifiedNameEx("Database", RE.Urn);

	public static XmlQualifiedNameEx IApplicationRole_DefaultSchema = new XmlQualifiedNameEx("DefaultSchema", RE.Urn);

	public static XmlQualifiedNameEx IApplicationRole_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx IApplicationRole_MemberOfRoles = new XmlQualifiedNameEx("MemberOfRoles", RE.Urn);

	public static XmlQualifiedNameEx IApplicationRole_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IApplicationRole_Permissions = new XmlQualifiedNameEx("Permissions", RE.Urn);

	public static XmlQualifiedNameEx IAsymmetricKey = new XmlQualifiedNameEx("AsymmetricKey", RE.Urn);

	public static XmlQualifiedNameEx IAsymmetricKey_Database = new XmlQualifiedNameEx("Database", RE.Urn);

	public static XmlQualifiedNameEx IAsymmetricKey_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx IAsymmetricKey_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx ICertificate = new XmlQualifiedNameEx("Certificate", RE.Urn);

	public static XmlQualifiedNameEx ICertificate_Database = new XmlQualifiedNameEx("Database", RE.Urn);

	public static XmlQualifiedNameEx ICertificate_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx ICertificate_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx ICheckConstraint = new XmlQualifiedNameEx("CheckConstraint", RE.Urn);

	public static XmlQualifiedNameEx ICheckConstraint_IsChecked = new XmlQualifiedNameEx("IsChecked", RE.Urn);

	public static XmlQualifiedNameEx ICheckConstraint_IsEnabled = new XmlQualifiedNameEx("IsEnabled", RE.Urn);

	public static XmlQualifiedNameEx ICheckConstraint_IsSystemNamed = new XmlQualifiedNameEx("IsSystemNamed", RE.Urn);

	public static XmlQualifiedNameEx ICheckConstraint_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx ICheckConstraint_NotForReplication = new XmlQualifiedNameEx("NotForReplication", RE.Urn);

	public static XmlQualifiedNameEx ICheckConstraint_Parent = new XmlQualifiedNameEx("Parent", RE.Urn);

	public static XmlQualifiedNameEx ICheckConstraint_Text = new XmlQualifiedNameEx("Text", RE.Urn);

	public static XmlQualifiedNameEx IClrDataType = new XmlQualifiedNameEx("ClrDataType", RE.Urn);

	public static XmlQualifiedNameEx IClrDataType_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx ICollation = new XmlQualifiedNameEx("Collation", RE.Urn);

	public static XmlQualifiedNameEx ICollation_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IColumn = new XmlQualifiedNameEx("Column", RE.Urn);

	public static XmlQualifiedNameEx IColumn_Collation = new XmlQualifiedNameEx("Collation", RE.Urn);

	public static XmlQualifiedNameEx IColumn_ComputedColumnInfo = new XmlQualifiedNameEx("ComputedColumnInfo", RE.Urn);

	public static XmlQualifiedNameEx IColumn_DataType = new XmlQualifiedNameEx("DataType", RE.Urn);

	public static XmlQualifiedNameEx IColumn_DefaultValue = new XmlQualifiedNameEx("DefaultValue", RE.Urn);

	public static XmlQualifiedNameEx IColumn_IdentityColumnInfo = new XmlQualifiedNameEx("IdentityColumnInfo", RE.Urn);

	public static XmlQualifiedNameEx IColumn_InPrimaryKey = new XmlQualifiedNameEx("InPrimaryKey", RE.Urn);

	public static XmlQualifiedNameEx IColumn_IsColumnSet = new XmlQualifiedNameEx("IsColumnSet", RE.Urn);

	public static XmlQualifiedNameEx IColumn_IsGeneratedAlwaysAsRowEnd = new XmlQualifiedNameEx("IsGeneratedAlwaysAsRowEnd", RE.Urn);

	public static XmlQualifiedNameEx IColumn_IsGeneratedAlwaysAsRowStart = new XmlQualifiedNameEx("IsGeneratedAlwaysAsRowStart", RE.Urn);

	public static XmlQualifiedNameEx IColumn_IsGeneratedAlwaysAsSequenceNumberEnd = new XmlQualifiedNameEx("IsGeneratedAlwaysAsSequenceNumberEnd", RE.Urn);

	public static XmlQualifiedNameEx IColumn_IsGeneratedAlwaysAsSequenceNumberStart = new XmlQualifiedNameEx("IsGeneratedAlwaysAsSequenceNumberStart", RE.Urn);

	public static XmlQualifiedNameEx IColumn_IsGeneratedAlwaysAsTransactionIdEnd = new XmlQualifiedNameEx("IsGeneratedAlwaysAsTransactionIdEnd", RE.Urn);

	public static XmlQualifiedNameEx IColumn_IsGeneratedAlwaysAsTransactionIdStart = new XmlQualifiedNameEx("IsGeneratedAlwaysAsTransactionIdStart", RE.Urn);

	public static XmlQualifiedNameEx IColumn_IsSparse = new XmlQualifiedNameEx("IsSparse", RE.Urn);

	public static XmlQualifiedNameEx IColumn_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IColumn_Nullable = new XmlQualifiedNameEx("Nullable", RE.Urn);

	public static XmlQualifiedNameEx IColumn_Parent = new XmlQualifiedNameEx("Parent", RE.Urn);

	public static XmlQualifiedNameEx IColumn_RowGuidCol = new XmlQualifiedNameEx("RowGuidCol", RE.Urn);

	public static XmlQualifiedNameEx ICredential = new XmlQualifiedNameEx("Credential", RE.Urn);

	public static XmlQualifiedNameEx ICredential_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx ICredential_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx ICredential_Server = new XmlQualifiedNameEx("Server", RE.Urn);

	public static XmlQualifiedNameEx ICursorDataType = new XmlQualifiedNameEx("CursorDataType", RE.Urn);

	public static XmlQualifiedNameEx ICursorParameter = new XmlQualifiedNameEx("CursorParameter", RE.Urn);

	public static XmlQualifiedNameEx ICursorParameter_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx ICursorVariable = new XmlQualifiedNameEx("CursorVariable", RE.Urn);

	public static XmlQualifiedNameEx ICursorVariable_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IDatabase = new XmlQualifiedNameEx("Database", RE.Urn);

	public static XmlQualifiedNameEx IDatabase_ApplicationRoles = new XmlQualifiedNameEx("ApplicationRoles", RE.Urn);

	public static XmlQualifiedNameEx IDatabase_AsymmetricKeys = new XmlQualifiedNameEx("AsymmetricKeys", RE.Urn);

	public static XmlQualifiedNameEx IDatabase_Certificates = new XmlQualifiedNameEx("Certificates", RE.Urn);

	public static XmlQualifiedNameEx IDatabase_CollationInfo = new XmlQualifiedNameEx("Collation", RE.Urn);

	public static XmlQualifiedNameEx IDatabase_CompatibilityLevel = new XmlQualifiedNameEx("CompatibilityLevel", RE.Urn);

	public static XmlQualifiedNameEx IDatabase_DefaultSchemaName = new XmlQualifiedNameEx("DefaultSchemaName", RE.Urn);

	public static XmlQualifiedNameEx IDatabase_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx IDatabase_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IDatabase_Owner = new XmlQualifiedNameEx("Owner", RE.Urn);

	public static XmlQualifiedNameEx IDatabase_Roles = new XmlQualifiedNameEx("Roles", RE.Urn);

	public static XmlQualifiedNameEx IDatabase_Schemas = new XmlQualifiedNameEx("Schemas", RE.Urn);

	public static XmlQualifiedNameEx IDatabase_Server = new XmlQualifiedNameEx("Server", RE.Urn);

	public static XmlQualifiedNameEx IDatabase_Triggers = new XmlQualifiedNameEx("Triggers", RE.Urn);

	public static XmlQualifiedNameEx IDatabase_Users = new XmlQualifiedNameEx("Users", RE.Urn);

	public static XmlQualifiedNameEx IDatabaseDdlTrigger = new XmlQualifiedNameEx("DatabaseDdlTrigger", RE.Urn);

	public static XmlQualifiedNameEx IDatabaseDdlTrigger_BodyText = new XmlQualifiedNameEx("BodyText", RE.Urn);

	public static XmlQualifiedNameEx IDatabaseDdlTrigger_Database = new XmlQualifiedNameEx("Database", RE.Urn);

	public static XmlQualifiedNameEx IDatabaseDdlTrigger_DatabaseDdlEvents = new XmlQualifiedNameEx("DatabaseDdlEvents", RE.Urn);

	public static XmlQualifiedNameEx IDatabaseDdlTrigger_ExecutionContext = new XmlQualifiedNameEx("ExecutionContext", RE.Urn);

	public static XmlQualifiedNameEx IDatabaseDdlTrigger_IsEnabled = new XmlQualifiedNameEx("IsEnabled", RE.Urn);

	public static XmlQualifiedNameEx IDatabaseDdlTrigger_IsEncrypted = new XmlQualifiedNameEx("IsEncrypted", RE.Urn);

	public static XmlQualifiedNameEx IDatabaseDdlTrigger_IsQuotedIdentifierOn = new XmlQualifiedNameEx("IsQuotedIdentifierOn", RE.Urn);

	public static XmlQualifiedNameEx IDatabaseDdlTrigger_IsSqlClr = new XmlQualifiedNameEx("IsSqlClr", RE.Urn);

	public static XmlQualifiedNameEx IDatabaseDdlTrigger_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx IDatabaseDdlTrigger_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IDatabasePermission = new XmlQualifiedNameEx("DatabasePermission", RE.Urn);

	public static XmlQualifiedNameEx IDatabasePermission_DatabasePrincipal = new XmlQualifiedNameEx("DatabasePrincipal", RE.Urn);

	public static XmlQualifiedNameEx IDatabasePermission_Grantor = new XmlQualifiedNameEx("Grantor", RE.Urn);

	public static XmlQualifiedNameEx IDatabasePermission_PermissionState = new XmlQualifiedNameEx("PermissionState", RE.Urn);

	public static XmlQualifiedNameEx IDatabasePermission_PermissionType = new XmlQualifiedNameEx("PermissionType", RE.Urn);

	public static XmlQualifiedNameEx IDatabasePermission_TargetObject = new XmlQualifiedNameEx("TargetObject", RE.Urn);

	public static XmlQualifiedNameEx IDatabaseRole = new XmlQualifiedNameEx("DatabaseRole", RE.Urn);

	public static XmlQualifiedNameEx IDatabaseRole_Database = new XmlQualifiedNameEx("Database", RE.Urn);

	public static XmlQualifiedNameEx IDatabaseRole_IsFixedRole = new XmlQualifiedNameEx("IsFixedRole", RE.Urn);

	public static XmlQualifiedNameEx IDatabaseRole_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx IDatabaseRole_MemberOfRoles = new XmlQualifiedNameEx("MemberOfRoles", RE.Urn);

	public static XmlQualifiedNameEx IDatabaseRole_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IDatabaseRole_Owner = new XmlQualifiedNameEx("Owner", RE.Urn);

	public static XmlQualifiedNameEx IDatabaseRole_Permissions = new XmlQualifiedNameEx("Permissions", RE.Urn);

	public static XmlQualifiedNameEx IDefaultConstraint = new XmlQualifiedNameEx("DefaultConstraint", RE.Urn);

	public static XmlQualifiedNameEx IDefaultConstraint_IsSystemNamed = new XmlQualifiedNameEx("IsSystemNamed", RE.Urn);

	public static XmlQualifiedNameEx IDefaultConstraint_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IDefaultConstraint_Parent = new XmlQualifiedNameEx("Parent", RE.Urn);

	public static XmlQualifiedNameEx IDefaultConstraint_Text = new XmlQualifiedNameEx("Text", RE.Urn);

	public static XmlQualifiedNameEx IDmlTrigger = new XmlQualifiedNameEx("DmlTrigger", RE.Urn);

	public static XmlQualifiedNameEx IDmlTrigger_BodyText = new XmlQualifiedNameEx("BodyText", RE.Urn);

	public static XmlQualifiedNameEx IDmlTrigger_Delete = new XmlQualifiedNameEx("Delete", RE.Urn);

	public static XmlQualifiedNameEx IDmlTrigger_DeleteActivationOrder = new XmlQualifiedNameEx("DeleteActivationOrder", RE.Urn);

	public static XmlQualifiedNameEx IDmlTrigger_ExecutionContext = new XmlQualifiedNameEx("ExecutionContext", RE.Urn);

	public static XmlQualifiedNameEx IDmlTrigger_Insert = new XmlQualifiedNameEx("Insert", RE.Urn);

	public static XmlQualifiedNameEx IDmlTrigger_InsertActivationOrder = new XmlQualifiedNameEx("InsertActivationOrder", RE.Urn);

	public static XmlQualifiedNameEx IDmlTrigger_InsteadOf = new XmlQualifiedNameEx("InsteadOf", RE.Urn);

	public static XmlQualifiedNameEx IDmlTrigger_IsEnabled = new XmlQualifiedNameEx("IsEnabled", RE.Urn);

	public static XmlQualifiedNameEx IDmlTrigger_IsEncrypted = new XmlQualifiedNameEx("IsEncrypted", RE.Urn);

	public static XmlQualifiedNameEx IDmlTrigger_IsQuotedIdentifierOn = new XmlQualifiedNameEx("IsQuotedIdentifierOn", RE.Urn);

	public static XmlQualifiedNameEx IDmlTrigger_IsSqlClr = new XmlQualifiedNameEx("IsSqlClr", RE.Urn);

	public static XmlQualifiedNameEx IDmlTrigger_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IDmlTrigger_NotForReplication = new XmlQualifiedNameEx("NotForReplication", RE.Urn);

	public static XmlQualifiedNameEx IDmlTrigger_Parent = new XmlQualifiedNameEx("Parent", RE.Urn);

	public static XmlQualifiedNameEx IDmlTrigger_Update = new XmlQualifiedNameEx("Update", RE.Urn);

	public static XmlQualifiedNameEx IDmlTrigger_UpdateActivationOrder = new XmlQualifiedNameEx("UpdateActivationOrder", RE.Urn);

	public static XmlQualifiedNameEx IExecutionContext = new XmlQualifiedNameEx("ExecutionContext", RE.Urn);

	public static XmlQualifiedNameEx IExecutionContext_ContextType = new XmlQualifiedNameEx("ContextType", RE.Urn);

	public static XmlQualifiedNameEx IExecutionContext_Login = new XmlQualifiedNameEx("Login", RE.Urn);

	public static XmlQualifiedNameEx IExecutionContext_User = new XmlQualifiedNameEx("User", RE.Urn);

	public static XmlQualifiedNameEx IExtendedStoredProcedure = new XmlQualifiedNameEx("ExtendedStoredProcedure", RE.Urn);

	public static XmlQualifiedNameEx IExtendedStoredProcedure_ExecutionContext = new XmlQualifiedNameEx("ExecutionContext", RE.Urn);

	public static XmlQualifiedNameEx IExtendedStoredProcedure_IsEncrypted = new XmlQualifiedNameEx("IsEncrypted", RE.Urn);

	public static XmlQualifiedNameEx IExtendedStoredProcedure_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx IExtendedStoredProcedure_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IExtendedStoredProcedure_Parameters = new XmlQualifiedNameEx("Parameters", RE.Urn);

	public static XmlQualifiedNameEx IExtendedStoredProcedure_ReturnType = new XmlQualifiedNameEx("ReturnType", RE.Urn);

	public static XmlQualifiedNameEx IExtendedStoredProcedure_Schema = new XmlQualifiedNameEx("Schema", RE.Urn);

	public static XmlQualifiedNameEx IFileGroup = new XmlQualifiedNameEx("FileGroup", RE.Urn);

	public static XmlQualifiedNameEx IFileGroup_Database = new XmlQualifiedNameEx("Database", RE.Urn);

	public static XmlQualifiedNameEx IFileGroup_IsDefault = new XmlQualifiedNameEx("IsDefault", RE.Urn);

	public static XmlQualifiedNameEx IFileGroup_IsFileStream = new XmlQualifiedNameEx("IsFileStream", RE.Urn);

	public static XmlQualifiedNameEx IFileGroup_IsReadOnly = new XmlQualifiedNameEx("IsReadOnly", RE.Urn);

	public static XmlQualifiedNameEx IFileGroup_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx IFileGroup_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IForeignKeyColumn = new XmlQualifiedNameEx("ForeignKeyColumn", RE.Urn);

	public static XmlQualifiedNameEx IForeignKeyColumn_ReferencedColumn = new XmlQualifiedNameEx("ReferencedColumn", RE.Urn);

	public static XmlQualifiedNameEx IForeignKeyColumn_ReferencingColumn = new XmlQualifiedNameEx("ReferencingColumn", RE.Urn);

	public static XmlQualifiedNameEx IForeignKeyConstraint = new XmlQualifiedNameEx("ForeignKeyConstraint", RE.Urn);

	public static XmlQualifiedNameEx IForeignKeyConstraint_Columns = new XmlQualifiedNameEx("Columns", RE.Urn);

	public static XmlQualifiedNameEx IForeignKeyConstraint_DeleteAction = new XmlQualifiedNameEx("DeleteAction", RE.Urn);

	public static XmlQualifiedNameEx IForeignKeyConstraint_IsChecked = new XmlQualifiedNameEx("IsChecked", RE.Urn);

	public static XmlQualifiedNameEx IForeignKeyConstraint_IsEnabled = new XmlQualifiedNameEx("IsEnabled", RE.Urn);

	public static XmlQualifiedNameEx IForeignKeyConstraint_IsSystemNamed = new XmlQualifiedNameEx("IsSystemNamed", RE.Urn);

	public static XmlQualifiedNameEx IForeignKeyConstraint_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IForeignKeyConstraint_NotForReplication = new XmlQualifiedNameEx("NotForReplication", RE.Urn);

	public static XmlQualifiedNameEx IForeignKeyConstraint_Parent = new XmlQualifiedNameEx("Parent", RE.Urn);

	public static XmlQualifiedNameEx IForeignKeyConstraint_ReferencedTable = new XmlQualifiedNameEx("ReferencedTable", RE.Urn);

	public static XmlQualifiedNameEx IForeignKeyConstraint_UpdateAction = new XmlQualifiedNameEx("UpdateAction", RE.Urn);

	public static XmlQualifiedNameEx IIndexedColumn = new XmlQualifiedNameEx("IndexedColumn", RE.Urn);

	public static XmlQualifiedNameEx IIndexedColumn_IsIncluded = new XmlQualifiedNameEx("IsIncluded", RE.Urn);

	public static XmlQualifiedNameEx IIndexedColumn_ReferencedColumn = new XmlQualifiedNameEx("ReferencedColumn", RE.Urn);

	public static XmlQualifiedNameEx IIndexedColumn_SortOrder = new XmlQualifiedNameEx("SortOrder", RE.Urn);

	public static XmlQualifiedNameEx ILogin = new XmlQualifiedNameEx("Login", RE.Urn);

	public static XmlQualifiedNameEx ILogin_AsymmetricKey = new XmlQualifiedNameEx("AsymmetricKey", RE.Urn);

	public static XmlQualifiedNameEx ILogin_Certificate = new XmlQualifiedNameEx("Certificate", RE.Urn);

	public static XmlQualifiedNameEx ILogin_Credential = new XmlQualifiedNameEx("Credential", RE.Urn);

	public static XmlQualifiedNameEx ILogin_DefaultDatabase = new XmlQualifiedNameEx("DefaultDatabase", RE.Urn);

	public static XmlQualifiedNameEx ILogin_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx ILogin_Language = new XmlQualifiedNameEx("Language", RE.Urn);

	public static XmlQualifiedNameEx ILogin_LoginType = new XmlQualifiedNameEx("LoginType", RE.Urn);

	public static XmlQualifiedNameEx ILogin_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx ILogin_Password = new XmlQualifiedNameEx("Password", RE.Urn);

	public static XmlQualifiedNameEx ILogin_Server = new XmlQualifiedNameEx("Server", RE.Urn);

	public static XmlQualifiedNameEx ILogin_Sid = new XmlQualifiedNameEx("Sid", RE.Urn);

	public static XmlQualifiedNameEx IPartitionScheme = new XmlQualifiedNameEx("PartitionScheme", RE.Urn);

	public static XmlQualifiedNameEx IPartitionScheme_Database = new XmlQualifiedNameEx("Database", RE.Urn);

	public static XmlQualifiedNameEx IPartitionScheme_FileGroups = new XmlQualifiedNameEx("FileGroups", RE.Urn);

	public static XmlQualifiedNameEx IPartitionScheme_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx IPartitionScheme_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IPrimaryKeyConstraint = new XmlQualifiedNameEx("PrimaryKeyConstraint", RE.Urn);

	public static XmlQualifiedNameEx IPrimaryKeyConstraint_AssociatedIndex = new XmlQualifiedNameEx("AssociatedIndex", RE.Urn);

	public static XmlQualifiedNameEx IPrimaryKeyConstraint_IsSystemNamed = new XmlQualifiedNameEx("IsSystemNamed", RE.Urn);

	public static XmlQualifiedNameEx IPrimaryKeyConstraint_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IPrimaryKeyConstraint_Parent = new XmlQualifiedNameEx("Parent", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex = new XmlQualifiedNameEx("RelationalIndex", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_CompactLargeObjects = new XmlQualifiedNameEx("CompactLargeObjects", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_DisallowPageLocks = new XmlQualifiedNameEx("DisallowPageLocks", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_DisallowRowLocks = new XmlQualifiedNameEx("DisallowRowLocks", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_FileGroup = new XmlQualifiedNameEx("FileGroup", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_FileStreamFileGroup = new XmlQualifiedNameEx("FileStreamFileGroup", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_FileStreamPartitionScheme = new XmlQualifiedNameEx("FileStreamPartitionScheme", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_FillFactor = new XmlQualifiedNameEx("FillFactor", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_FilterDefinition = new XmlQualifiedNameEx("FilterDefinition", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_IgnoreDuplicateKeys = new XmlQualifiedNameEx("IgnoreDuplicateKeys", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_IndexKey = new XmlQualifiedNameEx("IndexKey", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_IndexedColumns = new XmlQualifiedNameEx("IndexedColumns", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_IsClustered = new XmlQualifiedNameEx("IsClustered", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_IsDisabled = new XmlQualifiedNameEx("IsDisabled", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_IsSystemNamed = new XmlQualifiedNameEx("IsSystemNamed", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_IsUnique = new XmlQualifiedNameEx("IsUnique", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_NoAutomaticRecomputation = new XmlQualifiedNameEx("NoAutomaticRecomputation", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_PadIndex = new XmlQualifiedNameEx("PadIndex", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_Parent = new XmlQualifiedNameEx("Parent", RE.Urn);

	public static XmlQualifiedNameEx IRelationalIndex_PartitionScheme = new XmlQualifiedNameEx("PartitionScheme", RE.Urn);

	public static XmlQualifiedNameEx IScalarDataType = new XmlQualifiedNameEx("ScalarDataType", RE.Urn);

	public static XmlQualifiedNameEx IScalarDataType_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IScalarParameter = new XmlQualifiedNameEx("ScalarParameter", RE.Urn);

	public static XmlQualifiedNameEx IScalarParameter_DataType = new XmlQualifiedNameEx("DataType", RE.Urn);

	public static XmlQualifiedNameEx IScalarParameter_DefaultValue = new XmlQualifiedNameEx("DefaultValue", RE.Urn);

	public static XmlQualifiedNameEx IScalarParameter_IsOutput = new XmlQualifiedNameEx("IsOutput", RE.Urn);

	public static XmlQualifiedNameEx IScalarParameter_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IScalarParameter_Nullable = new XmlQualifiedNameEx("Nullable", RE.Urn);

	public static XmlQualifiedNameEx IScalarValuedFunction = new XmlQualifiedNameEx("ScalarValuedFunction", RE.Urn);

	public static XmlQualifiedNameEx IScalarValuedFunction_BodyText = new XmlQualifiedNameEx("BodyText", RE.Urn);

	public static XmlQualifiedNameEx IScalarValuedFunction_DataType = new XmlQualifiedNameEx("DataType", RE.Urn);

	public static XmlQualifiedNameEx IScalarValuedFunction_ExecutionContext = new XmlQualifiedNameEx("ExecutionContext", RE.Urn);

	public static XmlQualifiedNameEx IScalarValuedFunction_IsEncrypted = new XmlQualifiedNameEx("IsEncrypted", RE.Urn);

	public static XmlQualifiedNameEx IScalarValuedFunction_IsQuotedIdentifierOn = new XmlQualifiedNameEx("IsQuotedIdentifierOn", RE.Urn);

	public static XmlQualifiedNameEx IScalarValuedFunction_IsSchemaBound = new XmlQualifiedNameEx("IsSchemaBound", RE.Urn);

	public static XmlQualifiedNameEx IScalarValuedFunction_IsSqlClr = new XmlQualifiedNameEx("IsSqlClr", RE.Urn);

	public static XmlQualifiedNameEx IScalarValuedFunction_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx IScalarValuedFunction_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IScalarValuedFunction_Nullable = new XmlQualifiedNameEx("Nullable", RE.Urn);

	public static XmlQualifiedNameEx IScalarValuedFunction_Parameters = new XmlQualifiedNameEx("Parameters", RE.Urn);

	public static XmlQualifiedNameEx IScalarValuedFunction_ReturnsNullOnNullInput = new XmlQualifiedNameEx("ReturnsNullOnNullInput", RE.Urn);

	public static XmlQualifiedNameEx IScalarValuedFunction_Schema = new XmlQualifiedNameEx("Schema", RE.Urn);

	public static XmlQualifiedNameEx IScalarVariable = new XmlQualifiedNameEx("ScalarVariable", RE.Urn);

	public static XmlQualifiedNameEx IScalarVariable_DataType = new XmlQualifiedNameEx("DataType", RE.Urn);

	public static XmlQualifiedNameEx IScalarVariable_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IScalarVariable_Nullable = new XmlQualifiedNameEx("Nullable", RE.Urn);

	public static XmlQualifiedNameEx ISchema = new XmlQualifiedNameEx("Schema", RE.Urn);

	public static XmlQualifiedNameEx ISchema_Database = new XmlQualifiedNameEx("Database", RE.Urn);

	public static XmlQualifiedNameEx ISchema_ExtendedStoredProcedures = new XmlQualifiedNameEx("ExtendedStoredProcedures", RE.Urn);

	public static XmlQualifiedNameEx ISchema_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx ISchema_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx ISchema_Owner = new XmlQualifiedNameEx("Owner", RE.Urn);

	public static XmlQualifiedNameEx ISchema_ScalarValuedFunctions = new XmlQualifiedNameEx("ScalarValuedFunctions", RE.Urn);

	public static XmlQualifiedNameEx ISchema_StoredProcedures = new XmlQualifiedNameEx("StoredProcedures", RE.Urn);

	public static XmlQualifiedNameEx ISchema_Synonyms = new XmlQualifiedNameEx("Synonyms", RE.Urn);

	public static XmlQualifiedNameEx ISchema_TableValuedFunctions = new XmlQualifiedNameEx("TableValuedFunctions", RE.Urn);

	public static XmlQualifiedNameEx ISchema_Tables = new XmlQualifiedNameEx("Tables", RE.Urn);

	public static XmlQualifiedNameEx ISchema_UserDefinedAggregates = new XmlQualifiedNameEx("UserDefinedAggregates", RE.Urn);

	public static XmlQualifiedNameEx ISchema_UserDefinedClrTypes = new XmlQualifiedNameEx("UserDefinedClrTypes", RE.Urn);

	public static XmlQualifiedNameEx ISchema_UserDefinedDataTypes = new XmlQualifiedNameEx("UserDefinedDataTypes", RE.Urn);

	public static XmlQualifiedNameEx ISchema_UserDefinedTableTypes = new XmlQualifiedNameEx("UserDefinedTableTypes", RE.Urn);

	public static XmlQualifiedNameEx ISchema_Views = new XmlQualifiedNameEx("Views", RE.Urn);

	public static XmlQualifiedNameEx IServer = new XmlQualifiedNameEx("Server", RE.Urn);

	public static XmlQualifiedNameEx IServer_CollationInfo = new XmlQualifiedNameEx("CollationInfo", RE.Urn);

	public static XmlQualifiedNameEx IServer_Credentials = new XmlQualifiedNameEx("Credentials", RE.Urn);

	public static XmlQualifiedNameEx IServer_Databases = new XmlQualifiedNameEx("Databases", RE.Urn);

	public static XmlQualifiedNameEx IServer_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx IServer_Logins = new XmlQualifiedNameEx("Logins", RE.Urn);

	public static XmlQualifiedNameEx IServer_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IServer_Triggers = new XmlQualifiedNameEx("Triggers", RE.Urn);

	public static XmlQualifiedNameEx IServerDdlTrigger = new XmlQualifiedNameEx("ServerDdlTrigger", RE.Urn);

	public static XmlQualifiedNameEx IServerDdlTrigger_BodyText = new XmlQualifiedNameEx("BodyText", RE.Urn);

	public static XmlQualifiedNameEx IServerDdlTrigger_ExecutionContext = new XmlQualifiedNameEx("ExecutionContext", RE.Urn);

	public static XmlQualifiedNameEx IServerDdlTrigger_IsEnabled = new XmlQualifiedNameEx("IsEnabled", RE.Urn);

	public static XmlQualifiedNameEx IServerDdlTrigger_IsEncrypted = new XmlQualifiedNameEx("IsEncrypted", RE.Urn);

	public static XmlQualifiedNameEx IServerDdlTrigger_IsQuotedIdentifierOn = new XmlQualifiedNameEx("IsQuotedIdentifierOn", RE.Urn);

	public static XmlQualifiedNameEx IServerDdlTrigger_IsSqlClr = new XmlQualifiedNameEx("IsSqlClr", RE.Urn);

	public static XmlQualifiedNameEx IServerDdlTrigger_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx IServerDdlTrigger_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IServerDdlTrigger_Server = new XmlQualifiedNameEx("Server", RE.Urn);

	public static XmlQualifiedNameEx IServerDdlTrigger_ServerDdlEvents = new XmlQualifiedNameEx("ServerDdlEvents", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex = new XmlQualifiedNameEx("SpatialIndex", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex_BoundingBoxXMax = new XmlQualifiedNameEx("BoundingBoxXMax", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex_BoundingBoxXMin = new XmlQualifiedNameEx("BoundingBoxXMin", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex_BoundingBoxYMax = new XmlQualifiedNameEx("BoundingBoxYMax", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex_BoundingBoxYMin = new XmlQualifiedNameEx("BoundingBoxYMin", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex_CellsPerObject = new XmlQualifiedNameEx("CellsPerObject", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex_DisallowPageLocks = new XmlQualifiedNameEx("DisallowPageLocks", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex_DisallowRowLocks = new XmlQualifiedNameEx("DisallowRowLocks", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex_FillFactor = new XmlQualifiedNameEx("FillFactor", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex_IgnoreDuplicateKeys = new XmlQualifiedNameEx("IgnoreDuplicateKeys", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex_IndexedColumn = new XmlQualifiedNameEx("IndexedColumn", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex_IsDisabled = new XmlQualifiedNameEx("IsDisabled", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex_Level1Density = new XmlQualifiedNameEx("Level1Density", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex_Level2Density = new XmlQualifiedNameEx("Level2Density", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex_Level3Density = new XmlQualifiedNameEx("Level3Density", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex_Level4Density = new XmlQualifiedNameEx("Level4Density", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex_NoAutomaticRecomputation = new XmlQualifiedNameEx("NoAutomaticRecomputation", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex_PadIndex = new XmlQualifiedNameEx("PadIndex", RE.Urn);

	public static XmlQualifiedNameEx ISpatialIndex_Parent = new XmlQualifiedNameEx("Parent", RE.Urn);

	public static XmlQualifiedNameEx IStatistics = new XmlQualifiedNameEx("Statistics", RE.Urn);

	public static XmlQualifiedNameEx IStatistics_Columns = new XmlQualifiedNameEx("Columns", RE.Urn);

	public static XmlQualifiedNameEx IStatistics_FilterDefinition = new XmlQualifiedNameEx("FilterDefinition", RE.Urn);

	public static XmlQualifiedNameEx IStatistics_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IStatistics_NoAutomaticRecomputation = new XmlQualifiedNameEx("NoAutomaticRecomputation", RE.Urn);

	public static XmlQualifiedNameEx IStatistics_Parent = new XmlQualifiedNameEx("Parent", RE.Urn);

	public static XmlQualifiedNameEx IStatistics_Type = new XmlQualifiedNameEx("Type", RE.Urn);

	public static XmlQualifiedNameEx IStoredProcedure = new XmlQualifiedNameEx("StoredProcedure", RE.Urn);

	public static XmlQualifiedNameEx IStoredProcedure_BodyText = new XmlQualifiedNameEx("BodyText", RE.Urn);

	public static XmlQualifiedNameEx IStoredProcedure_ExecutionContext = new XmlQualifiedNameEx("ExecutionContext", RE.Urn);

	public static XmlQualifiedNameEx IStoredProcedure_ForReplication = new XmlQualifiedNameEx("ForReplication", RE.Urn);

	public static XmlQualifiedNameEx IStoredProcedure_IsEncrypted = new XmlQualifiedNameEx("IsEncrypted", RE.Urn);

	public static XmlQualifiedNameEx IStoredProcedure_IsQuotedIdentifierOn = new XmlQualifiedNameEx("IsQuotedIdentifierOn", RE.Urn);

	public static XmlQualifiedNameEx IStoredProcedure_IsRecompiled = new XmlQualifiedNameEx("IsRecompiled", RE.Urn);

	public static XmlQualifiedNameEx IStoredProcedure_IsSqlClr = new XmlQualifiedNameEx("IsSqlClr", RE.Urn);

	public static XmlQualifiedNameEx IStoredProcedure_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx IStoredProcedure_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IStoredProcedure_Parameters = new XmlQualifiedNameEx("Parameters", RE.Urn);

	public static XmlQualifiedNameEx IStoredProcedure_Schema = new XmlQualifiedNameEx("Schema", RE.Urn);

	public static XmlQualifiedNameEx IStoredProcedure_Startup = new XmlQualifiedNameEx("Startup", RE.Urn);

	public static XmlQualifiedNameEx ISynonym = new XmlQualifiedNameEx("Synonym", RE.Urn);

	public static XmlQualifiedNameEx ISynonym_BaseObjectName = new XmlQualifiedNameEx("BaseObjectName", RE.Urn);

	public static XmlQualifiedNameEx ISynonym_BaseType = new XmlQualifiedNameEx("BaseType", RE.Urn);

	public static XmlQualifiedNameEx ISynonym_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx ISynonym_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx ISynonym_Schema = new XmlQualifiedNameEx("Schema", RE.Urn);

	public static XmlQualifiedNameEx ISystemClrDataType = new XmlQualifiedNameEx("SystemClrDataType", RE.Urn);

	public static XmlQualifiedNameEx ISystemClrDataType_Length = new XmlQualifiedNameEx("Length", RE.Urn);

	public static XmlQualifiedNameEx ISystemClrDataType_NumericPrecision = new XmlQualifiedNameEx("NumericPrecision", RE.Urn);

	public static XmlQualifiedNameEx ISystemClrDataType_NumericScale = new XmlQualifiedNameEx("NumericScale", RE.Urn);

	public static XmlQualifiedNameEx ISystemClrDataType_TypeSpec = new XmlQualifiedNameEx("TypeSpec", RE.Urn);

	public static XmlQualifiedNameEx ISystemDataType = new XmlQualifiedNameEx("SystemDataType", RE.Urn);

	public static XmlQualifiedNameEx ISystemDataType_Length = new XmlQualifiedNameEx("Length", RE.Urn);

	public static XmlQualifiedNameEx ISystemDataType_NumericPrecision = new XmlQualifiedNameEx("NumericPrecision", RE.Urn);

	public static XmlQualifiedNameEx ISystemDataType_NumericScale = new XmlQualifiedNameEx("NumericScale", RE.Urn);

	public static XmlQualifiedNameEx ISystemDataType_TypeSpec = new XmlQualifiedNameEx("TypeSpec", RE.Urn);

	public static XmlQualifiedNameEx ITable = new XmlQualifiedNameEx("Table", RE.Urn);

	public static XmlQualifiedNameEx ITable_Columns = new XmlQualifiedNameEx("Columns", RE.Urn);

	public static XmlQualifiedNameEx ITable_Constraints = new XmlQualifiedNameEx("Constraints", RE.Urn);

	public static XmlQualifiedNameEx ITable_Indexes = new XmlQualifiedNameEx("Indexes", RE.Urn);

	public static XmlQualifiedNameEx ITable_IsQuotedIdentifierOn = new XmlQualifiedNameEx("IsQuotedIdentifierOn", RE.Urn);

	public static XmlQualifiedNameEx ITable_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx ITable_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx ITable_Schema = new XmlQualifiedNameEx("Schema", RE.Urn);

	public static XmlQualifiedNameEx ITable_Statistics = new XmlQualifiedNameEx("Statistics", RE.Urn);

	public static XmlQualifiedNameEx ITable_Triggers = new XmlQualifiedNameEx("Triggers", RE.Urn);

	public static XmlQualifiedNameEx ITableDataType = new XmlQualifiedNameEx("TableDataType", RE.Urn);

	public static XmlQualifiedNameEx ITableDataType_Columns = new XmlQualifiedNameEx("Columns", RE.Urn);

	public static XmlQualifiedNameEx ITableDataType_Constraints = new XmlQualifiedNameEx("Constraints", RE.Urn);

	public static XmlQualifiedNameEx ITableDataType_Indexes = new XmlQualifiedNameEx("Indexes", RE.Urn);

	public static XmlQualifiedNameEx ITableDataType_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx ITableDataType_Statistics = new XmlQualifiedNameEx("Statistics", RE.Urn);

	public static XmlQualifiedNameEx ITableParameter = new XmlQualifiedNameEx("TableParameter", RE.Urn);

	public static XmlQualifiedNameEx ITableParameter_DataType = new XmlQualifiedNameEx("DataType", RE.Urn);

	public static XmlQualifiedNameEx ITableParameter_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx ITableValuedFunction = new XmlQualifiedNameEx("TableValuedFunction", RE.Urn);

	public static XmlQualifiedNameEx ITableValuedFunction_BodyText = new XmlQualifiedNameEx("BodyText", RE.Urn);

	public static XmlQualifiedNameEx ITableValuedFunction_Columns = new XmlQualifiedNameEx("Columns", RE.Urn);

	public static XmlQualifiedNameEx ITableValuedFunction_Constraints = new XmlQualifiedNameEx("Constraints", RE.Urn);

	public static XmlQualifiedNameEx ITableValuedFunction_ExecutionContext = new XmlQualifiedNameEx("ExecutionContext", RE.Urn);

	public static XmlQualifiedNameEx ITableValuedFunction_Indexes = new XmlQualifiedNameEx("Indexes", RE.Urn);

	public static XmlQualifiedNameEx ITableValuedFunction_IsEncrypted = new XmlQualifiedNameEx("IsEncrypted", RE.Urn);

	public static XmlQualifiedNameEx ITableValuedFunction_IsInline = new XmlQualifiedNameEx("IsInline", RE.Urn);

	public static XmlQualifiedNameEx ITableValuedFunction_IsQuotedIdentifierOn = new XmlQualifiedNameEx("IsQuotedIdentifierOn", RE.Urn);

	public static XmlQualifiedNameEx ITableValuedFunction_IsSchemaBound = new XmlQualifiedNameEx("IsSchemaBound", RE.Urn);

	public static XmlQualifiedNameEx ITableValuedFunction_IsSqlClr = new XmlQualifiedNameEx("IsSqlClr", RE.Urn);

	public static XmlQualifiedNameEx ITableValuedFunction_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx ITableValuedFunction_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx ITableValuedFunction_Parameters = new XmlQualifiedNameEx("Parameters", RE.Urn);

	public static XmlQualifiedNameEx ITableValuedFunction_Schema = new XmlQualifiedNameEx("Schema", RE.Urn);

	public static XmlQualifiedNameEx ITableValuedFunction_Statistics = new XmlQualifiedNameEx("Statistics", RE.Urn);

	public static XmlQualifiedNameEx ITableValuedFunction_TableVariableName = new XmlQualifiedNameEx("TableVariableName", RE.Urn);

	public static XmlQualifiedNameEx ITableVariable = new XmlQualifiedNameEx("TableVariable", RE.Urn);

	public static XmlQualifiedNameEx ITableVariable_DataType = new XmlQualifiedNameEx("DataType", RE.Urn);

	public static XmlQualifiedNameEx ITableVariable_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IUdtDataMember = new XmlQualifiedNameEx("UdtDataMember", RE.Urn);

	public static XmlQualifiedNameEx IUdtDataMember_DataType = new XmlQualifiedNameEx("DataType", RE.Urn);

	public static XmlQualifiedNameEx IUdtDataMember_IsStatic = new XmlQualifiedNameEx("IsStatic", RE.Urn);

	public static XmlQualifiedNameEx IUdtDataMember_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IUdtDataMember_Nullable = new XmlQualifiedNameEx("Nullable", RE.Urn);

	public static XmlQualifiedNameEx IUdtDataMember_UserDefinedType = new XmlQualifiedNameEx("UserDefinedType", RE.Urn);

	public static XmlQualifiedNameEx IUdtMethod = new XmlQualifiedNameEx("UdtMethod", RE.Urn);

	public static XmlQualifiedNameEx IUdtMethod_DataType = new XmlQualifiedNameEx("DataType", RE.Urn);

	public static XmlQualifiedNameEx IUdtMethod_IsStatic = new XmlQualifiedNameEx("IsStatic", RE.Urn);

	public static XmlQualifiedNameEx IUdtMethod_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IUdtMethod_Nullable = new XmlQualifiedNameEx("Nullable", RE.Urn);

	public static XmlQualifiedNameEx IUdtMethod_Parameters = new XmlQualifiedNameEx("Parameters", RE.Urn);

	public static XmlQualifiedNameEx IUdtMethod_UserDefinedType = new XmlQualifiedNameEx("UserDefinedType", RE.Urn);

	public static XmlQualifiedNameEx IUniqueConstraint = new XmlQualifiedNameEx("UniqueConstraint", RE.Urn);

	public static XmlQualifiedNameEx IUniqueConstraint_AssociatedIndex = new XmlQualifiedNameEx("AssociatedIndex", RE.Urn);

	public static XmlQualifiedNameEx IUniqueConstraint_IsSystemNamed = new XmlQualifiedNameEx("IsSystemNamed", RE.Urn);

	public static XmlQualifiedNameEx IUniqueConstraint_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IUniqueConstraint_Parent = new XmlQualifiedNameEx("Parent", RE.Urn);

	public static XmlQualifiedNameEx IUser = new XmlQualifiedNameEx("User", RE.Urn);

	public static XmlQualifiedNameEx IUser_AsymmetricKey = new XmlQualifiedNameEx("AsymmetricKey", RE.Urn);

	public static XmlQualifiedNameEx IUser_Certificate = new XmlQualifiedNameEx("Certificate", RE.Urn);

	public static XmlQualifiedNameEx IUser_Database = new XmlQualifiedNameEx("Database", RE.Urn);

	public static XmlQualifiedNameEx IUser_DefaultSchema = new XmlQualifiedNameEx("DefaultSchema", RE.Urn);

	public static XmlQualifiedNameEx IUser_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx IUser_Login = new XmlQualifiedNameEx("Login", RE.Urn);

	public static XmlQualifiedNameEx IUser_MemberOfRoles = new XmlQualifiedNameEx("MemberOfRoles", RE.Urn);

	public static XmlQualifiedNameEx IUser_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IUser_Password = new XmlQualifiedNameEx("Password", RE.Urn);

	public static XmlQualifiedNameEx IUser_Permissions = new XmlQualifiedNameEx("Permissions", RE.Urn);

	public static XmlQualifiedNameEx IUser_UserType = new XmlQualifiedNameEx("UserType", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedAggregate = new XmlQualifiedNameEx("UserDefinedAggregate", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedAggregate_DataType = new XmlQualifiedNameEx("DataType", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedAggregate_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedAggregate_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedAggregate_Nullable = new XmlQualifiedNameEx("Nullable", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedAggregate_Parameters = new XmlQualifiedNameEx("Parameters", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedAggregate_Schema = new XmlQualifiedNameEx("Schema", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedClrType = new XmlQualifiedNameEx("UserDefinedClrType", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedClrType_AssemblyName = new XmlQualifiedNameEx("AssemblyName", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedClrType_ClassName = new XmlQualifiedNameEx("ClassName", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedClrType_DataMembers = new XmlQualifiedNameEx("DataMembers", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedClrType_IsBinaryOrdered = new XmlQualifiedNameEx("IsBinaryOrdered", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedClrType_IsComVisible = new XmlQualifiedNameEx("IsComVisible", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedClrType_IsNullable = new XmlQualifiedNameEx("IsNullable", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedClrType_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedClrType_Methods = new XmlQualifiedNameEx("Methods", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedClrType_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedClrType_Schema = new XmlQualifiedNameEx("Schema", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedDataType = new XmlQualifiedNameEx("UserDefinedDataType", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedDataType_BaseSystemDataType = new XmlQualifiedNameEx("BaseSystemDataType", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedDataType_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedDataType_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedDataType_Nullable = new XmlQualifiedNameEx("Nullable", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedDataType_Schema = new XmlQualifiedNameEx("Schema", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedTableType = new XmlQualifiedNameEx("UserDefinedTableType", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedTableType_Columns = new XmlQualifiedNameEx("Columns", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedTableType_Constraints = new XmlQualifiedNameEx("Constraints", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedTableType_Indexes = new XmlQualifiedNameEx("Indexes", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedTableType_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedTableType_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedTableType_Schema = new XmlQualifiedNameEx("Schema", RE.Urn);

	public static XmlQualifiedNameEx IUserDefinedTableType_Statistics = new XmlQualifiedNameEx("Statistics", RE.Urn);

	public static XmlQualifiedNameEx IView = new XmlQualifiedNameEx("View", RE.Urn);

	public static XmlQualifiedNameEx IView_Columns = new XmlQualifiedNameEx("Columns", RE.Urn);

	public static XmlQualifiedNameEx IView_Constraints = new XmlQualifiedNameEx("Constraints", RE.Urn);

	public static XmlQualifiedNameEx IView_HasCheckOption = new XmlQualifiedNameEx("HasCheckOption", RE.Urn);

	public static XmlQualifiedNameEx IView_HasColumnSpecification = new XmlQualifiedNameEx("HasColumnSpecification", RE.Urn);

	public static XmlQualifiedNameEx IView_Indexes = new XmlQualifiedNameEx("Indexes", RE.Urn);

	public static XmlQualifiedNameEx IView_IsEncrypted = new XmlQualifiedNameEx("IsEncrypted", RE.Urn);

	public static XmlQualifiedNameEx IView_IsQuotedIdentifierOn = new XmlQualifiedNameEx("IsQuotedIdentifierOn", RE.Urn);

	public static XmlQualifiedNameEx IView_IsSchemaBound = new XmlQualifiedNameEx("IsSchemaBound", RE.Urn);

	public static XmlQualifiedNameEx IView_IsSystemObject = new XmlQualifiedNameEx("IsSystemObject", RE.Urn);

	public static XmlQualifiedNameEx IView_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IView_QueryText = new XmlQualifiedNameEx("QueryText", RE.Urn);

	public static XmlQualifiedNameEx IView_ReturnsViewMetadata = new XmlQualifiedNameEx("ReturnsViewMetadata", RE.Urn);

	public static XmlQualifiedNameEx IView_Schema = new XmlQualifiedNameEx("Schema", RE.Urn);

	public static XmlQualifiedNameEx IView_Statistics = new XmlQualifiedNameEx("Statistics", RE.Urn);

	public static XmlQualifiedNameEx IView_Triggers = new XmlQualifiedNameEx("Triggers", RE.Urn);

	public static XmlQualifiedNameEx IVoidDataType = new XmlQualifiedNameEx("VoidDataType", RE.Urn);

	public static XmlQualifiedNameEx IXmlDataType = new XmlQualifiedNameEx("XmlDataType", RE.Urn);

	public static XmlQualifiedNameEx IXmlDataType_IsXmlNode = new XmlQualifiedNameEx("IsXmlNode", RE.Urn);

	public static XmlQualifiedNameEx IXmlDataType_Length = new XmlQualifiedNameEx("Length", RE.Urn);

	public static XmlQualifiedNameEx IXmlDataType_NumericPrecision = new XmlQualifiedNameEx("NumericPrecision", RE.Urn);

	public static XmlQualifiedNameEx IXmlDataType_NumericScale = new XmlQualifiedNameEx("NumericScale", RE.Urn);

	public static XmlQualifiedNameEx IXmlDataType_TypeSpec = new XmlQualifiedNameEx("TypeSpec", RE.Urn);

	public static XmlQualifiedNameEx IXmlIndex = new XmlQualifiedNameEx("XmlIndex", RE.Urn);

	public static XmlQualifiedNameEx IXmlIndex_DisallowPageLocks = new XmlQualifiedNameEx("DisallowPageLocks", RE.Urn);

	public static XmlQualifiedNameEx IXmlIndex_DisallowRowLocks = new XmlQualifiedNameEx("DisallowRowLocks", RE.Urn);

	public static XmlQualifiedNameEx IXmlIndex_FillFactor = new XmlQualifiedNameEx("FillFactor", RE.Urn);

	public static XmlQualifiedNameEx IXmlIndex_IgnoreDuplicateKeys = new XmlQualifiedNameEx("IgnoreDuplicateKeys", RE.Urn);

	public static XmlQualifiedNameEx IXmlIndex_IsDisabled = new XmlQualifiedNameEx("IsDisabled", RE.Urn);

	public static XmlQualifiedNameEx IXmlIndex_Name = new XmlQualifiedNameEx("Name", RE.Urn);

	public static XmlQualifiedNameEx IXmlIndex_PadIndex = new XmlQualifiedNameEx("PadIndex", RE.Urn);

	public static XmlQualifiedNameEx IXmlIndex_Parent = new XmlQualifiedNameEx("Parent", RE.Urn);

	public const string Xmlns = "xmlns";

	public const string ServerStringId = "/";

	private static MetadataPropertyInfo Login_Sid = ((MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoByType(typeof(ILogin))).GetPropertyByName("Sid");

	private static MetadataPropertyInfo Database_Owner = ((MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoByType(typeof(IDatabase))).GetPropertyByName("Owner");

	public static XmlQualifiedNameEx GetInstanceElementName(TypeInfo typeInfo)
	{
		return typeInfo.Id switch
		{
			15 => IApplicationRole, 
			16 => IAsymmetricKey, 
			18 => ICertificate, 
			19 => ICheckConstraint, 
			20 => IClrDataType, 
			21 => ICollation, 
			22 => IColumn, 
			24 => ICredential, 
			25 => ICursorDataType, 
			26 => ICursorParameter, 
			27 => ICursorVariable, 
			29 => IDatabase, 
			30 => IDatabaseDdlTrigger, 
			33 => IDatabasePermission, 
			35 => IDatabaseRole, 
			37 => IDefaultConstraint, 
			38 => IDmlTrigger, 
			39 => IExecutionContext, 
			40 => IExtendedStoredProcedure, 
			41 => IFileGroup, 
			42 => IForeignKeyColumn, 
			43 => IForeignKeyConstraint, 
			47 => IIndexedColumn, 
			49 => ILogin, 
			88 => IPartitionScheme, 
			90 => IPrimaryKeyConstraint, 
			91 => IRelationalIndex, 
			93 => IScalarDataType, 
			95 => IScalarParameter, 
			96 => IScalarValuedFunction, 
			97 => IScalarVariable, 
			98 => ISchema, 
			100 => IServer, 
			101 => IServerDdlTrigger, 
			103 => ISpatialIndex, 
			104 => IStatistics, 
			105 => IStoredProcedure, 
			106 => ISynonym, 
			107 => ISystemClrDataType, 
			108 => ISystemDataType, 
			109 => ITable, 
			110 => ITableDataType, 
			111 => ITableParameter, 
			112 => ITableValuedFunction, 
			113 => ITableVariable, 
			118 => IUdtDataMember, 
			120 => IUdtMethod, 
			121 => IUniqueConstraint, 
			123 => IUser, 
			124 => IUserDefinedAggregate, 
			125 => IUserDefinedClrType, 
			126 => IUserDefinedDataType, 
			129 => IUserDefinedTableType, 
			131 => IView, 
			132 => IVoidDataType, 
			133 => IXmlDataType, 
			136 => IXmlIndex, 
			_ => null, 
		};
	}

	public static XmlQualifiedNameEx GetPropertyElementName(TypeInfo typeInfo, MetadataPropertyInfo propertyInfo)
	{
		switch (typeInfo.Id)
		{
		case 15:
			return propertyInfo.Index switch
			{
				0 => IApplicationRole_Database, 
				1 => IApplicationRole_DefaultSchema, 
				2 => IApplicationRole_IsSystemObject, 
				3 => IApplicationRole_MemberOfRoles, 
				4 => IApplicationRole_Name, 
				5 => IApplicationRole_Permissions, 
				_ => null, 
			};
		case 16:
			return propertyInfo.Index switch
			{
				0 => IAsymmetricKey_Database, 
				1 => IAsymmetricKey_IsSystemObject, 
				2 => IAsymmetricKey_Name, 
				_ => null, 
			};
		case 18:
			return propertyInfo.Index switch
			{
				0 => ICertificate_Database, 
				1 => ICertificate_IsSystemObject, 
				2 => ICertificate_Name, 
				_ => null, 
			};
		case 19:
			return propertyInfo.Index switch
			{
				0 => ICheckConstraint_IsChecked, 
				1 => ICheckConstraint_IsEnabled, 
				2 => ICheckConstraint_IsSystemNamed, 
				3 => ICheckConstraint_Name, 
				4 => ICheckConstraint_NotForReplication, 
				5 => ICheckConstraint_Parent, 
				6 => ICheckConstraint_Text, 
				_ => null, 
			};
		case 20:
			if (propertyInfo.Index == 0)
			{
				return IClrDataType_Name;
			}
			return null;
		case 21:
			if (propertyInfo.Index == 0)
			{
				return ICollation_Name;
			}
			return null;
		case 22:
			return propertyInfo.Index switch
			{
				0 => IColumn_Collation, 
				1 => IColumn_ComputedColumnInfo, 
				2 => IColumn_DataType, 
				3 => IColumn_DefaultValue, 
				4 => IColumn_IdentityColumnInfo, 
				5 => IColumn_InPrimaryKey, 
				6 => IColumn_IsColumnSet, 
				7 => IColumn_IsGeneratedAlwaysAsRowEnd, 
				8 => IColumn_IsGeneratedAlwaysAsRowStart, 
				9 => IColumn_IsGeneratedAlwaysAsSequenceNumberEnd, 
				10 => IColumn_IsGeneratedAlwaysAsSequenceNumberStart, 
				11 => IColumn_IsGeneratedAlwaysAsTransactionIdEnd, 
				12 => IColumn_IsGeneratedAlwaysAsTransactionIdStart, 
				13 => IColumn_IsSparse, 
				14 => IColumn_Name, 
				15 => IColumn_Nullable, 
				16 => IColumn_Parent, 
				17 => IColumn_RowGuidCol, 
				_ => null, 
			};
		case 24:
			return propertyInfo.Index switch
			{
				0 => ICredential_IsSystemObject, 
				1 => ICredential_Name, 
				2 => ICredential_Server, 
				_ => null, 
			};
		case 25:
			_ = propertyInfo.Index;
			return null;
		case 26:
			if (propertyInfo.Index == 0)
			{
				return ICursorParameter_Name;
			}
			return null;
		case 27:
			if (propertyInfo.Index == 0)
			{
				return ICursorVariable_Name;
			}
			return null;
		case 29:
			return propertyInfo.Index switch
			{
				0 => IDatabase_ApplicationRoles, 
				1 => IDatabase_AsymmetricKeys, 
				2 => IDatabase_Certificates, 
				3 => IDatabase_CollationInfo, 
				4 => IDatabase_CompatibilityLevel, 
				5 => IDatabase_DefaultSchemaName, 
				6 => IDatabase_IsSystemObject, 
				7 => IDatabase_Name, 
				8 => IDatabase_Owner, 
				9 => IDatabase_Roles, 
				10 => IDatabase_Schemas, 
				11 => IDatabase_Server, 
				12 => IDatabase_Triggers, 
				13 => IDatabase_Users, 
				_ => null, 
			};
		case 30:
			return propertyInfo.Index switch
			{
				0 => IDatabaseDdlTrigger_BodyText, 
				1 => IDatabaseDdlTrigger_Database, 
				2 => IDatabaseDdlTrigger_DatabaseDdlEvents, 
				3 => IDatabaseDdlTrigger_ExecutionContext, 
				4 => IDatabaseDdlTrigger_IsEnabled, 
				5 => IDatabaseDdlTrigger_IsEncrypted, 
				6 => IDatabaseDdlTrigger_IsQuotedIdentifierOn, 
				7 => IDatabaseDdlTrigger_IsSqlClr, 
				8 => IDatabaseDdlTrigger_IsSystemObject, 
				9 => IDatabaseDdlTrigger_Name, 
				_ => null, 
			};
		case 33:
			return propertyInfo.Index switch
			{
				0 => IDatabasePermission_DatabasePrincipal, 
				1 => IDatabasePermission_Grantor, 
				2 => IDatabasePermission_PermissionState, 
				3 => IDatabasePermission_PermissionType, 
				4 => IDatabasePermission_TargetObject, 
				_ => null, 
			};
		case 35:
			return propertyInfo.Index switch
			{
				0 => IDatabaseRole_Database, 
				1 => IDatabaseRole_IsFixedRole, 
				2 => IDatabaseRole_IsSystemObject, 
				3 => IDatabaseRole_MemberOfRoles, 
				4 => IDatabaseRole_Name, 
				5 => IDatabaseRole_Owner, 
				6 => IDatabaseRole_Permissions, 
				_ => null, 
			};
		case 37:
			return propertyInfo.Index switch
			{
				0 => IDefaultConstraint_IsSystemNamed, 
				1 => IDefaultConstraint_Name, 
				2 => IDefaultConstraint_Parent, 
				3 => IDefaultConstraint_Text, 
				_ => null, 
			};
		case 38:
			return propertyInfo.Index switch
			{
				0 => IDmlTrigger_BodyText, 
				1 => IDmlTrigger_Delete, 
				2 => IDmlTrigger_DeleteActivationOrder, 
				3 => IDmlTrigger_ExecutionContext, 
				4 => IDmlTrigger_Insert, 
				5 => IDmlTrigger_InsertActivationOrder, 
				6 => IDmlTrigger_InsteadOf, 
				7 => IDmlTrigger_IsEnabled, 
				8 => IDmlTrigger_IsEncrypted, 
				9 => IDmlTrigger_IsQuotedIdentifierOn, 
				10 => IDmlTrigger_IsSqlClr, 
				11 => IDmlTrigger_Name, 
				12 => IDmlTrigger_NotForReplication, 
				13 => IDmlTrigger_Parent, 
				14 => IDmlTrigger_Update, 
				15 => IDmlTrigger_UpdateActivationOrder, 
				_ => null, 
			};
		case 39:
			return propertyInfo.Index switch
			{
				0 => IExecutionContext_ContextType, 
				1 => IExecutionContext_Login, 
				2 => IExecutionContext_User, 
				_ => null, 
			};
		case 40:
			return propertyInfo.Index switch
			{
				0 => IExtendedStoredProcedure_ExecutionContext, 
				1 => IExtendedStoredProcedure_IsEncrypted, 
				2 => IExtendedStoredProcedure_IsSystemObject, 
				3 => IExtendedStoredProcedure_Name, 
				4 => IExtendedStoredProcedure_Parameters, 
				5 => IExtendedStoredProcedure_ReturnType, 
				6 => IExtendedStoredProcedure_Schema, 
				_ => null, 
			};
		case 41:
			return propertyInfo.Index switch
			{
				0 => IFileGroup_Database, 
				1 => IFileGroup_IsDefault, 
				2 => IFileGroup_IsFileStream, 
				3 => IFileGroup_IsReadOnly, 
				4 => IFileGroup_IsSystemObject, 
				5 => IFileGroup_Name, 
				_ => null, 
			};
		case 42:
			return propertyInfo.Index switch
			{
				0 => IForeignKeyColumn_ReferencedColumn, 
				1 => IForeignKeyColumn_ReferencingColumn, 
				_ => null, 
			};
		case 43:
			return propertyInfo.Index switch
			{
				0 => IForeignKeyConstraint_Columns, 
				1 => IForeignKeyConstraint_DeleteAction, 
				2 => IForeignKeyConstraint_IsChecked, 
				3 => IForeignKeyConstraint_IsEnabled, 
				4 => IForeignKeyConstraint_IsSystemNamed, 
				5 => IForeignKeyConstraint_Name, 
				6 => IForeignKeyConstraint_NotForReplication, 
				7 => IForeignKeyConstraint_Parent, 
				8 => IForeignKeyConstraint_ReferencedTable, 
				9 => IForeignKeyConstraint_UpdateAction, 
				_ => null, 
			};
		case 47:
			return propertyInfo.Index switch
			{
				0 => IIndexedColumn_IsIncluded, 
				1 => IIndexedColumn_ReferencedColumn, 
				2 => IIndexedColumn_SortOrder, 
				_ => null, 
			};
		case 49:
			return propertyInfo.Index switch
			{
				0 => ILogin_AsymmetricKey, 
				1 => ILogin_Certificate, 
				2 => ILogin_Credential, 
				3 => ILogin_DefaultDatabase, 
				4 => ILogin_IsSystemObject, 
				5 => ILogin_Language, 
				6 => ILogin_LoginType, 
				7 => ILogin_Name, 
				8 => ILogin_Password, 
				9 => ILogin_Server, 
				10 => ILogin_Sid, 
				_ => null, 
			};
		case 88:
			return propertyInfo.Index switch
			{
				0 => IPartitionScheme_Database, 
				1 => IPartitionScheme_FileGroups, 
				2 => IPartitionScheme_IsSystemObject, 
				3 => IPartitionScheme_Name, 
				_ => null, 
			};
		case 90:
			return propertyInfo.Index switch
			{
				0 => IPrimaryKeyConstraint_AssociatedIndex, 
				1 => IPrimaryKeyConstraint_IsSystemNamed, 
				2 => IPrimaryKeyConstraint_Name, 
				3 => IPrimaryKeyConstraint_Parent, 
				_ => null, 
			};
		case 91:
			return propertyInfo.Index switch
			{
				0 => IRelationalIndex_CompactLargeObjects, 
				1 => IRelationalIndex_DisallowPageLocks, 
				2 => IRelationalIndex_DisallowRowLocks, 
				3 => IRelationalIndex_FileGroup, 
				4 => IRelationalIndex_FileStreamFileGroup, 
				5 => IRelationalIndex_FileStreamPartitionScheme, 
				6 => IRelationalIndex_FillFactor, 
				7 => IRelationalIndex_FilterDefinition, 
				8 => IRelationalIndex_IgnoreDuplicateKeys, 
				9 => IRelationalIndex_IndexKey, 
				10 => IRelationalIndex_IndexedColumns, 
				11 => IRelationalIndex_IsClustered, 
				12 => IRelationalIndex_IsDisabled, 
				13 => IRelationalIndex_IsSystemNamed, 
				14 => IRelationalIndex_IsUnique, 
				15 => IRelationalIndex_Name, 
				16 => IRelationalIndex_NoAutomaticRecomputation, 
				17 => IRelationalIndex_PadIndex, 
				18 => IRelationalIndex_Parent, 
				19 => IRelationalIndex_PartitionScheme, 
				_ => null, 
			};
		case 93:
			if (propertyInfo.Index == 0)
			{
				return IScalarDataType_Name;
			}
			return null;
		case 95:
			return propertyInfo.Index switch
			{
				0 => IScalarParameter_DataType, 
				1 => IScalarParameter_DefaultValue, 
				2 => IScalarParameter_IsOutput, 
				3 => IScalarParameter_Name, 
				4 => IScalarParameter_Nullable, 
				_ => null, 
			};
		case 96:
			return propertyInfo.Index switch
			{
				0 => IScalarValuedFunction_BodyText, 
				1 => IScalarValuedFunction_DataType, 
				2 => IScalarValuedFunction_ExecutionContext, 
				3 => IScalarValuedFunction_IsEncrypted, 
				4 => IScalarValuedFunction_IsQuotedIdentifierOn, 
				5 => IScalarValuedFunction_IsSchemaBound, 
				6 => IScalarValuedFunction_IsSqlClr, 
				7 => IScalarValuedFunction_IsSystemObject, 
				8 => IScalarValuedFunction_Name, 
				9 => IScalarValuedFunction_Nullable, 
				10 => IScalarValuedFunction_Parameters, 
				11 => IScalarValuedFunction_ReturnsNullOnNullInput, 
				12 => IScalarValuedFunction_Schema, 
				_ => null, 
			};
		case 97:
			return propertyInfo.Index switch
			{
				0 => IScalarVariable_DataType, 
				1 => IScalarVariable_Name, 
				2 => IScalarVariable_Nullable, 
				_ => null, 
			};
		case 98:
			return propertyInfo.Index switch
			{
				0 => ISchema_Database, 
				1 => ISchema_ExtendedStoredProcedures, 
				2 => ISchema_IsSystemObject, 
				3 => ISchema_Name, 
				4 => ISchema_Owner, 
				5 => ISchema_ScalarValuedFunctions, 
				6 => ISchema_StoredProcedures, 
				7 => ISchema_Synonyms, 
				8 => ISchema_TableValuedFunctions, 
				9 => ISchema_Tables, 
				10 => ISchema_UserDefinedAggregates, 
				11 => ISchema_UserDefinedClrTypes, 
				12 => ISchema_UserDefinedDataTypes, 
				13 => ISchema_UserDefinedTableTypes, 
				14 => ISchema_Views, 
				_ => null, 
			};
		case 100:
			return propertyInfo.Index switch
			{
				0 => IServer_CollationInfo, 
				1 => IServer_Credentials, 
				2 => IServer_Databases, 
				3 => IServer_IsSystemObject, 
				4 => IServer_Logins, 
				5 => IServer_Name, 
				6 => IServer_Triggers, 
				_ => null, 
			};
		case 101:
			return propertyInfo.Index switch
			{
				0 => IServerDdlTrigger_BodyText, 
				1 => IServerDdlTrigger_ExecutionContext, 
				2 => IServerDdlTrigger_IsEnabled, 
				3 => IServerDdlTrigger_IsEncrypted, 
				4 => IServerDdlTrigger_IsQuotedIdentifierOn, 
				5 => IServerDdlTrigger_IsSqlClr, 
				6 => IServerDdlTrigger_IsSystemObject, 
				7 => IServerDdlTrigger_Name, 
				8 => IServerDdlTrigger_Server, 
				9 => IServerDdlTrigger_ServerDdlEvents, 
				_ => null, 
			};
		case 103:
			return propertyInfo.Index switch
			{
				0 => ISpatialIndex_BoundingBoxXMax, 
				1 => ISpatialIndex_BoundingBoxXMin, 
				2 => ISpatialIndex_BoundingBoxYMax, 
				3 => ISpatialIndex_BoundingBoxYMin, 
				4 => ISpatialIndex_CellsPerObject, 
				5 => ISpatialIndex_DisallowPageLocks, 
				6 => ISpatialIndex_DisallowRowLocks, 
				7 => ISpatialIndex_FillFactor, 
				8 => ISpatialIndex_IgnoreDuplicateKeys, 
				9 => ISpatialIndex_IndexedColumn, 
				10 => ISpatialIndex_IsDisabled, 
				11 => ISpatialIndex_Level1Density, 
				12 => ISpatialIndex_Level2Density, 
				13 => ISpatialIndex_Level3Density, 
				14 => ISpatialIndex_Level4Density, 
				15 => ISpatialIndex_Name, 
				16 => ISpatialIndex_NoAutomaticRecomputation, 
				17 => ISpatialIndex_PadIndex, 
				18 => ISpatialIndex_Parent, 
				_ => null, 
			};
		case 104:
			return propertyInfo.Index switch
			{
				0 => IStatistics_Columns, 
				1 => IStatistics_FilterDefinition, 
				2 => IStatistics_Name, 
				3 => IStatistics_NoAutomaticRecomputation, 
				4 => IStatistics_Parent, 
				5 => IStatistics_Type, 
				_ => null, 
			};
		case 105:
			return propertyInfo.Index switch
			{
				0 => IStoredProcedure_BodyText, 
				1 => IStoredProcedure_ExecutionContext, 
				2 => IStoredProcedure_ForReplication, 
				3 => IStoredProcedure_IsEncrypted, 
				4 => IStoredProcedure_IsQuotedIdentifierOn, 
				5 => IStoredProcedure_IsRecompiled, 
				6 => IStoredProcedure_IsSqlClr, 
				7 => IStoredProcedure_IsSystemObject, 
				8 => IStoredProcedure_Name, 
				9 => IStoredProcedure_Parameters, 
				10 => IStoredProcedure_Schema, 
				11 => IStoredProcedure_Startup, 
				_ => null, 
			};
		case 106:
			return propertyInfo.Index switch
			{
				0 => ISynonym_BaseObjectName, 
				1 => ISynonym_BaseType, 
				2 => ISynonym_IsSystemObject, 
				3 => ISynonym_Name, 
				4 => ISynonym_Schema, 
				_ => null, 
			};
		case 107:
			return propertyInfo.Index switch
			{
				0 => ISystemClrDataType_Length, 
				1 => ISystemClrDataType_NumericPrecision, 
				2 => ISystemClrDataType_NumericScale, 
				3 => ISystemClrDataType_TypeSpec, 
				_ => null, 
			};
		case 108:
			return propertyInfo.Index switch
			{
				0 => ISystemDataType_Length, 
				1 => ISystemDataType_NumericPrecision, 
				2 => ISystemDataType_NumericScale, 
				3 => ISystemDataType_TypeSpec, 
				_ => null, 
			};
		case 109:
			return propertyInfo.Index switch
			{
				0 => ITable_Columns, 
				1 => ITable_Constraints, 
				2 => ITable_Indexes, 
				3 => ITable_IsQuotedIdentifierOn, 
				4 => ITable_IsSystemObject, 
				5 => ITable_Name, 
				6 => ITable_Schema, 
				7 => ITable_Statistics, 
				8 => ITable_Triggers, 
				_ => null, 
			};
		case 110:
			return propertyInfo.Index switch
			{
				0 => ITableDataType_Columns, 
				1 => ITableDataType_Constraints, 
				2 => ITableDataType_Indexes, 
				3 => ITableDataType_Name, 
				4 => ITableDataType_Statistics, 
				_ => null, 
			};
		case 111:
			return propertyInfo.Index switch
			{
				0 => ITableParameter_DataType, 
				1 => ITableParameter_Name, 
				_ => null, 
			};
		case 112:
			return propertyInfo.Index switch
			{
				0 => ITableValuedFunction_BodyText, 
				1 => ITableValuedFunction_Columns, 
				2 => ITableValuedFunction_Constraints, 
				3 => ITableValuedFunction_ExecutionContext, 
				4 => ITableValuedFunction_Indexes, 
				5 => ITableValuedFunction_IsEncrypted, 
				6 => ITableValuedFunction_IsInline, 
				7 => ITableValuedFunction_IsQuotedIdentifierOn, 
				8 => ITableValuedFunction_IsSchemaBound, 
				9 => ITableValuedFunction_IsSqlClr, 
				10 => ITableValuedFunction_IsSystemObject, 
				11 => ITableValuedFunction_Name, 
				12 => ITableValuedFunction_Parameters, 
				13 => ITableValuedFunction_Schema, 
				14 => ITableValuedFunction_Statistics, 
				15 => ITableValuedFunction_TableVariableName, 
				_ => null, 
			};
		case 113:
			return propertyInfo.Index switch
			{
				0 => ITableVariable_DataType, 
				1 => ITableVariable_Name, 
				_ => null, 
			};
		case 118:
			return propertyInfo.Index switch
			{
				0 => IUdtDataMember_DataType, 
				1 => IUdtDataMember_IsStatic, 
				2 => IUdtDataMember_Name, 
				3 => IUdtDataMember_Nullable, 
				4 => IUdtDataMember_UserDefinedType, 
				_ => null, 
			};
		case 120:
			return propertyInfo.Index switch
			{
				0 => IUdtMethod_DataType, 
				1 => IUdtMethod_IsStatic, 
				2 => IUdtMethod_Name, 
				3 => IUdtMethod_Nullable, 
				4 => IUdtMethod_Parameters, 
				5 => IUdtMethod_UserDefinedType, 
				_ => null, 
			};
		case 121:
			return propertyInfo.Index switch
			{
				0 => IUniqueConstraint_AssociatedIndex, 
				1 => IUniqueConstraint_IsSystemNamed, 
				2 => IUniqueConstraint_Name, 
				3 => IUniqueConstraint_Parent, 
				_ => null, 
			};
		case 123:
			return propertyInfo.Index switch
			{
				0 => IUser_AsymmetricKey, 
				1 => IUser_Certificate, 
				2 => IUser_Database, 
				3 => IUser_DefaultSchema, 
				4 => IUser_IsSystemObject, 
				5 => IUser_Login, 
				6 => IUser_MemberOfRoles, 
				7 => IUser_Name, 
				8 => IUser_Password, 
				9 => IUser_Permissions, 
				10 => IUser_UserType, 
				_ => null, 
			};
		case 124:
			return propertyInfo.Index switch
			{
				0 => IUserDefinedAggregate_DataType, 
				1 => IUserDefinedAggregate_IsSystemObject, 
				2 => IUserDefinedAggregate_Name, 
				3 => IUserDefinedAggregate_Nullable, 
				4 => IUserDefinedAggregate_Parameters, 
				5 => IUserDefinedAggregate_Schema, 
				_ => null, 
			};
		case 125:
			return propertyInfo.Index switch
			{
				0 => IUserDefinedClrType_AssemblyName, 
				1 => IUserDefinedClrType_ClassName, 
				2 => IUserDefinedClrType_DataMembers, 
				3 => IUserDefinedClrType_IsBinaryOrdered, 
				4 => IUserDefinedClrType_IsComVisible, 
				5 => IUserDefinedClrType_IsNullable, 
				6 => IUserDefinedClrType_IsSystemObject, 
				7 => IUserDefinedClrType_Methods, 
				8 => IUserDefinedClrType_Name, 
				9 => IUserDefinedClrType_Schema, 
				_ => null, 
			};
		case 126:
			return propertyInfo.Index switch
			{
				0 => IUserDefinedDataType_BaseSystemDataType, 
				1 => IUserDefinedDataType_IsSystemObject, 
				2 => IUserDefinedDataType_Name, 
				3 => IUserDefinedDataType_Nullable, 
				4 => IUserDefinedDataType_Schema, 
				_ => null, 
			};
		case 129:
			return propertyInfo.Index switch
			{
				0 => IUserDefinedTableType_Columns, 
				1 => IUserDefinedTableType_Constraints, 
				2 => IUserDefinedTableType_Indexes, 
				3 => IUserDefinedTableType_IsSystemObject, 
				4 => IUserDefinedTableType_Name, 
				5 => IUserDefinedTableType_Schema, 
				6 => IUserDefinedTableType_Statistics, 
				_ => null, 
			};
		case 131:
			return propertyInfo.Index switch
			{
				0 => IView_Columns, 
				1 => IView_Constraints, 
				2 => IView_HasCheckOption, 
				3 => IView_HasColumnSpecification, 
				4 => IView_Indexes, 
				5 => IView_IsEncrypted, 
				6 => IView_IsQuotedIdentifierOn, 
				7 => IView_IsSchemaBound, 
				8 => IView_IsSystemObject, 
				9 => IView_Name, 
				10 => IView_QueryText, 
				11 => IView_ReturnsViewMetadata, 
				12 => IView_Schema, 
				13 => IView_Statistics, 
				14 => IView_Triggers, 
				_ => null, 
			};
		case 132:
			_ = propertyInfo.Index;
			return null;
		case 133:
			return propertyInfo.Index switch
			{
				0 => IXmlDataType_IsXmlNode, 
				1 => IXmlDataType_Length, 
				2 => IXmlDataType_NumericPrecision, 
				3 => IXmlDataType_NumericScale, 
				4 => IXmlDataType_TypeSpec, 
				_ => null, 
			};
		case 136:
			return propertyInfo.Index switch
			{
				0 => IXmlIndex_DisallowPageLocks, 
				1 => IXmlIndex_DisallowRowLocks, 
				2 => IXmlIndex_FillFactor, 
				3 => IXmlIndex_IgnoreDuplicateKeys, 
				4 => IXmlIndex_IsDisabled, 
				5 => IXmlIndex_Name, 
				6 => IXmlIndex_PadIndex, 
				7 => IXmlIndex_Parent, 
				_ => null, 
			};
		default:
			return null;
		}
	}

	public static MetadataTypeInfo GetTypeInfoByName(XmlToken elementName)
	{
		return elementName switch
		{
			XmlToken.ApplicationRole => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(15), 
			XmlToken.AsymmetricKey => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(16), 
			XmlToken.Certificate => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(18), 
			XmlToken.CheckConstraint => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(19), 
			XmlToken.ClrDataType => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(20), 
			XmlToken.Collation => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(21), 
			XmlToken.Column => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(22), 
			XmlToken.Credential => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(24), 
			XmlToken.CursorDataType => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(25), 
			XmlToken.CursorParameter => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(26), 
			XmlToken.CursorVariable => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(27), 
			XmlToken.Database => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(29), 
			XmlToken.DatabaseDdlTrigger => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(30), 
			XmlToken.DatabasePermission => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(33), 
			XmlToken.DatabaseRole => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(35), 
			XmlToken.DefaultConstraint => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(37), 
			XmlToken.DmlTrigger => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(38), 
			XmlToken.ExecutionContext => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(39), 
			XmlToken.ExtendedStoredProcedure => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(40), 
			XmlToken.FileGroup => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(41), 
			XmlToken.ForeignKeyColumn => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(42), 
			XmlToken.ForeignKeyConstraint => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(43), 
			XmlToken.IndexedColumn => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(47), 
			XmlToken.Login => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(49), 
			XmlToken.PartitionScheme => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(88), 
			XmlToken.PrimaryKeyConstraint => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(90), 
			XmlToken.RelationalIndex => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(91), 
			XmlToken.ScalarDataType => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(93), 
			XmlToken.ScalarParameter => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(95), 
			XmlToken.ScalarValuedFunction => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(96), 
			XmlToken.ScalarVariable => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(97), 
			XmlToken.Schema => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(98), 
			XmlToken.Server => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(100), 
			XmlToken.ServerDdlTrigger => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(101), 
			XmlToken.SpatialIndex => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(103), 
			XmlToken.Statistics => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(104), 
			XmlToken.StoredProcedure => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(105), 
			XmlToken.Synonym => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(106), 
			XmlToken.SystemClrDataType => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(107), 
			XmlToken.SystemDataType => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(108), 
			XmlToken.Table => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(109), 
			XmlToken.TableDataType => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(110), 
			XmlToken.TableParameter => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(111), 
			XmlToken.TableValuedFunction => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(112), 
			XmlToken.TableVariable => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(113), 
			XmlToken.UdtDataMember => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(118), 
			XmlToken.UdtMethod => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(120), 
			XmlToken.UniqueConstraint => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(121), 
			XmlToken.User => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(123), 
			XmlToken.UserDefinedAggregate => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(124), 
			XmlToken.UserDefinedClrType => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(125), 
			XmlToken.UserDefinedDataType => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(126), 
			XmlToken.UserDefinedTableType => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(129), 
			XmlToken.View => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(131), 
			XmlToken.VoidDataType => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(132), 
			XmlToken.XmlDataType => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(133), 
			XmlToken.XmlIndex => (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(136), 
			_ => null, 
		};
	}

	public static MetadataPropertyInfo GetPropertyInfoByName(MetadataTypeInfo typeInfo, XmlToken propertyName)
	{
		switch (typeInfo.Id)
		{
		case 15:
			return propertyName switch
			{
				XmlToken.Database => typeInfo.InstanceProperties[0], 
				XmlToken.DefaultSchema => typeInfo.InstanceProperties[1], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[2], 
				XmlToken.MemberOfRoles => typeInfo.InstanceProperties[3], 
				XmlToken.Name => typeInfo.InstanceProperties[4], 
				XmlToken.Permissions => typeInfo.InstanceProperties[5], 
				_ => null, 
			};
		case 16:
			return propertyName switch
			{
				XmlToken.Database => typeInfo.InstanceProperties[0], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[1], 
				XmlToken.Name => typeInfo.InstanceProperties[2], 
				_ => null, 
			};
		case 18:
			return propertyName switch
			{
				XmlToken.Database => typeInfo.InstanceProperties[0], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[1], 
				XmlToken.Name => typeInfo.InstanceProperties[2], 
				_ => null, 
			};
		case 19:
			return propertyName switch
			{
				XmlToken.IsChecked => typeInfo.InstanceProperties[0], 
				XmlToken.IsEnabled => typeInfo.InstanceProperties[1], 
				XmlToken.IsSystemNamed => typeInfo.InstanceProperties[2], 
				XmlToken.Name => typeInfo.InstanceProperties[3], 
				XmlToken.NotForReplication => typeInfo.InstanceProperties[4], 
				XmlToken.Parent => typeInfo.InstanceProperties[5], 
				XmlToken.Text => typeInfo.InstanceProperties[6], 
				_ => null, 
			};
		case 20:
			if (propertyName == XmlToken.Name)
			{
				return typeInfo.InstanceProperties[0];
			}
			return null;
		case 21:
			if (propertyName == XmlToken.Name)
			{
				return typeInfo.InstanceProperties[0];
			}
			return null;
		case 22:
			return propertyName switch
			{
				XmlToken.Collation => typeInfo.InstanceProperties[0], 
				XmlToken.ComputedColumnInfo => typeInfo.InstanceProperties[1], 
				XmlToken.DataType => typeInfo.InstanceProperties[2], 
				XmlToken.DefaultValue => typeInfo.InstanceProperties[3], 
				XmlToken.IdentityColumnInfo => typeInfo.InstanceProperties[4], 
				XmlToken.InPrimaryKey => typeInfo.InstanceProperties[5], 
				XmlToken.IsColumnSet => typeInfo.InstanceProperties[6], 
				XmlToken.IsGeneratedAlwaysAsRowEnd => typeInfo.InstanceProperties[7], 
				XmlToken.IsGeneratedAlwaysAsRowStart => typeInfo.InstanceProperties[8], 
				XmlToken.IsGeneratedAlwaysAsSequenceNumberEnd => typeInfo.InstanceProperties[9], 
				XmlToken.IsGeneratedAlwaysAsSequenceNumberStart => typeInfo.InstanceProperties[10], 
				XmlToken.IsGeneratedAlwaysAsTransactionIdEnd => typeInfo.InstanceProperties[11], 
				XmlToken.IsGeneratedAlwaysAsTransactionIdStart => typeInfo.InstanceProperties[12], 
				XmlToken.IsSparse => typeInfo.InstanceProperties[13], 
				XmlToken.Name => typeInfo.InstanceProperties[14], 
				XmlToken.Nullable => typeInfo.InstanceProperties[15], 
				XmlToken.Parent => typeInfo.InstanceProperties[16], 
				XmlToken.RowGuidCol => typeInfo.InstanceProperties[17], 
				_ => null, 
			};
		case 24:
			return propertyName switch
			{
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[0], 
				XmlToken.Name => typeInfo.InstanceProperties[1], 
				XmlToken.Server => typeInfo.InstanceProperties[2], 
				_ => null, 
			};
		case 25:
			return null;
		case 26:
			if (propertyName == XmlToken.Name)
			{
				return typeInfo.InstanceProperties[0];
			}
			return null;
		case 27:
			if (propertyName == XmlToken.Name)
			{
				return typeInfo.InstanceProperties[0];
			}
			return null;
		case 29:
			return propertyName switch
			{
				XmlToken.ApplicationRoles => typeInfo.InstanceProperties[0], 
				XmlToken.AsymmetricKeys => typeInfo.InstanceProperties[1], 
				XmlToken.Certificates => typeInfo.InstanceProperties[2], 
				XmlToken.Collation => typeInfo.InstanceProperties[3], 
				XmlToken.CompatibilityLevel => typeInfo.InstanceProperties[4], 
				XmlToken.DefaultSchemaName => typeInfo.InstanceProperties[5], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[6], 
				XmlToken.Name => typeInfo.InstanceProperties[7], 
				XmlToken.Owner => typeInfo.InstanceProperties[8], 
				XmlToken.Roles => typeInfo.InstanceProperties[9], 
				XmlToken.Schemas => typeInfo.InstanceProperties[10], 
				XmlToken.Server => typeInfo.InstanceProperties[11], 
				XmlToken.Triggers => typeInfo.InstanceProperties[12], 
				XmlToken.Users => typeInfo.InstanceProperties[13], 
				_ => null, 
			};
		case 30:
			return propertyName switch
			{
				XmlToken.BodyText => typeInfo.InstanceProperties[0], 
				XmlToken.Database => typeInfo.InstanceProperties[1], 
				XmlToken.DatabaseDdlEvents => typeInfo.InstanceProperties[2], 
				XmlToken.ExecutionContext => typeInfo.InstanceProperties[3], 
				XmlToken.IsEnabled => typeInfo.InstanceProperties[4], 
				XmlToken.IsEncrypted => typeInfo.InstanceProperties[5], 
				XmlToken.IsQuotedIdentifierOn => typeInfo.InstanceProperties[6], 
				XmlToken.IsSqlClr => typeInfo.InstanceProperties[7], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[8], 
				XmlToken.Name => typeInfo.InstanceProperties[9], 
				_ => null, 
			};
		case 33:
			return propertyName switch
			{
				XmlToken.DatabasePrincipal => typeInfo.InstanceProperties[0], 
				XmlToken.Grantor => typeInfo.InstanceProperties[1], 
				XmlToken.PermissionState => typeInfo.InstanceProperties[2], 
				XmlToken.PermissionType => typeInfo.InstanceProperties[3], 
				XmlToken.TargetObject => typeInfo.InstanceProperties[4], 
				_ => null, 
			};
		case 35:
			return propertyName switch
			{
				XmlToken.Database => typeInfo.InstanceProperties[0], 
				XmlToken.IsFixedRole => typeInfo.InstanceProperties[1], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[2], 
				XmlToken.MemberOfRoles => typeInfo.InstanceProperties[3], 
				XmlToken.Name => typeInfo.InstanceProperties[4], 
				XmlToken.Owner => typeInfo.InstanceProperties[5], 
				XmlToken.Permissions => typeInfo.InstanceProperties[6], 
				_ => null, 
			};
		case 37:
			return propertyName switch
			{
				XmlToken.IsSystemNamed => typeInfo.InstanceProperties[0], 
				XmlToken.Name => typeInfo.InstanceProperties[1], 
				XmlToken.Parent => typeInfo.InstanceProperties[2], 
				XmlToken.Text => typeInfo.InstanceProperties[3], 
				_ => null, 
			};
		case 38:
			return propertyName switch
			{
				XmlToken.BodyText => typeInfo.InstanceProperties[0], 
				XmlToken.Delete => typeInfo.InstanceProperties[1], 
				XmlToken.DeleteActivationOrder => typeInfo.InstanceProperties[2], 
				XmlToken.ExecutionContext => typeInfo.InstanceProperties[3], 
				XmlToken.Insert => typeInfo.InstanceProperties[4], 
				XmlToken.InsertActivationOrder => typeInfo.InstanceProperties[5], 
				XmlToken.InsteadOf => typeInfo.InstanceProperties[6], 
				XmlToken.IsEnabled => typeInfo.InstanceProperties[7], 
				XmlToken.IsEncrypted => typeInfo.InstanceProperties[8], 
				XmlToken.IsQuotedIdentifierOn => typeInfo.InstanceProperties[9], 
				XmlToken.IsSqlClr => typeInfo.InstanceProperties[10], 
				XmlToken.Name => typeInfo.InstanceProperties[11], 
				XmlToken.NotForReplication => typeInfo.InstanceProperties[12], 
				XmlToken.Parent => typeInfo.InstanceProperties[13], 
				XmlToken.Update => typeInfo.InstanceProperties[14], 
				XmlToken.UpdateActivationOrder => typeInfo.InstanceProperties[15], 
				_ => null, 
			};
		case 39:
			return propertyName switch
			{
				XmlToken.ContextType => typeInfo.InstanceProperties[0], 
				XmlToken.Login => typeInfo.InstanceProperties[1], 
				XmlToken.User => typeInfo.InstanceProperties[2], 
				_ => null, 
			};
		case 40:
			return propertyName switch
			{
				XmlToken.ExecutionContext => typeInfo.InstanceProperties[0], 
				XmlToken.IsEncrypted => typeInfo.InstanceProperties[1], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[2], 
				XmlToken.Name => typeInfo.InstanceProperties[3], 
				XmlToken.Parameters => typeInfo.InstanceProperties[4], 
				XmlToken.ReturnType => typeInfo.InstanceProperties[5], 
				XmlToken.Schema => typeInfo.InstanceProperties[6], 
				_ => null, 
			};
		case 41:
			return propertyName switch
			{
				XmlToken.Database => typeInfo.InstanceProperties[0], 
				XmlToken.IsDefault => typeInfo.InstanceProperties[1], 
				XmlToken.IsFileStream => typeInfo.InstanceProperties[2], 
				XmlToken.IsReadOnly => typeInfo.InstanceProperties[3], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[4], 
				XmlToken.Name => typeInfo.InstanceProperties[5], 
				_ => null, 
			};
		case 42:
			return propertyName switch
			{
				XmlToken.ReferencedColumn => typeInfo.InstanceProperties[0], 
				XmlToken.ReferencingColumn => typeInfo.InstanceProperties[1], 
				_ => null, 
			};
		case 43:
			return propertyName switch
			{
				XmlToken.Columns => typeInfo.InstanceProperties[0], 
				XmlToken.DeleteAction => typeInfo.InstanceProperties[1], 
				XmlToken.IsChecked => typeInfo.InstanceProperties[2], 
				XmlToken.IsEnabled => typeInfo.InstanceProperties[3], 
				XmlToken.IsSystemNamed => typeInfo.InstanceProperties[4], 
				XmlToken.Name => typeInfo.InstanceProperties[5], 
				XmlToken.NotForReplication => typeInfo.InstanceProperties[6], 
				XmlToken.Parent => typeInfo.InstanceProperties[7], 
				XmlToken.ReferencedTable => typeInfo.InstanceProperties[8], 
				XmlToken.UpdateAction => typeInfo.InstanceProperties[9], 
				_ => null, 
			};
		case 47:
			return propertyName switch
			{
				XmlToken.IsIncluded => typeInfo.InstanceProperties[0], 
				XmlToken.ReferencedColumn => typeInfo.InstanceProperties[1], 
				XmlToken.SortOrder => typeInfo.InstanceProperties[2], 
				_ => null, 
			};
		case 49:
			return propertyName switch
			{
				XmlToken.AsymmetricKey => typeInfo.InstanceProperties[0], 
				XmlToken.Certificate => typeInfo.InstanceProperties[1], 
				XmlToken.Credential => typeInfo.InstanceProperties[2], 
				XmlToken.DefaultDatabase => typeInfo.InstanceProperties[3], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[4], 
				XmlToken.Language => typeInfo.InstanceProperties[5], 
				XmlToken.LoginType => typeInfo.InstanceProperties[6], 
				XmlToken.Name => typeInfo.InstanceProperties[7], 
				XmlToken.Password => typeInfo.InstanceProperties[8], 
				XmlToken.Server => typeInfo.InstanceProperties[9], 
				XmlToken.Sid => typeInfo.InstanceProperties[10], 
				_ => null, 
			};
		case 88:
			return propertyName switch
			{
				XmlToken.Database => typeInfo.InstanceProperties[0], 
				XmlToken.FileGroups => typeInfo.InstanceProperties[1], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[2], 
				XmlToken.Name => typeInfo.InstanceProperties[3], 
				_ => null, 
			};
		case 90:
			return propertyName switch
			{
				XmlToken.AssociatedIndex => typeInfo.InstanceProperties[0], 
				XmlToken.IsSystemNamed => typeInfo.InstanceProperties[1], 
				XmlToken.Name => typeInfo.InstanceProperties[2], 
				XmlToken.Parent => typeInfo.InstanceProperties[3], 
				_ => null, 
			};
		case 91:
			return propertyName switch
			{
				XmlToken.CompactLargeObjects => typeInfo.InstanceProperties[0], 
				XmlToken.DisallowPageLocks => typeInfo.InstanceProperties[1], 
				XmlToken.DisallowRowLocks => typeInfo.InstanceProperties[2], 
				XmlToken.FileGroup => typeInfo.InstanceProperties[3], 
				XmlToken.FileStreamFileGroup => typeInfo.InstanceProperties[4], 
				XmlToken.FileStreamPartitionScheme => typeInfo.InstanceProperties[5], 
				XmlToken.FillFactor => typeInfo.InstanceProperties[6], 
				XmlToken.FilterDefinition => typeInfo.InstanceProperties[7], 
				XmlToken.IgnoreDuplicateKeys => typeInfo.InstanceProperties[8], 
				XmlToken.IndexKey => typeInfo.InstanceProperties[9], 
				XmlToken.IndexedColumns => typeInfo.InstanceProperties[10], 
				XmlToken.IsClustered => typeInfo.InstanceProperties[11], 
				XmlToken.IsDisabled => typeInfo.InstanceProperties[12], 
				XmlToken.IsSystemNamed => typeInfo.InstanceProperties[13], 
				XmlToken.IsUnique => typeInfo.InstanceProperties[14], 
				XmlToken.Name => typeInfo.InstanceProperties[15], 
				XmlToken.NoAutomaticRecomputation => typeInfo.InstanceProperties[16], 
				XmlToken.PadIndex => typeInfo.InstanceProperties[17], 
				XmlToken.Parent => typeInfo.InstanceProperties[18], 
				XmlToken.PartitionScheme => typeInfo.InstanceProperties[19], 
				_ => null, 
			};
		case 93:
			if (propertyName == XmlToken.Name)
			{
				return typeInfo.InstanceProperties[0];
			}
			return null;
		case 95:
			return propertyName switch
			{
				XmlToken.DataType => typeInfo.InstanceProperties[0], 
				XmlToken.DefaultValue => typeInfo.InstanceProperties[1], 
				XmlToken.IsOutput => typeInfo.InstanceProperties[2], 
				XmlToken.Name => typeInfo.InstanceProperties[3], 
				XmlToken.Nullable => typeInfo.InstanceProperties[4], 
				_ => null, 
			};
		case 96:
			return propertyName switch
			{
				XmlToken.BodyText => typeInfo.InstanceProperties[0], 
				XmlToken.DataType => typeInfo.InstanceProperties[1], 
				XmlToken.ExecutionContext => typeInfo.InstanceProperties[2], 
				XmlToken.IsEncrypted => typeInfo.InstanceProperties[3], 
				XmlToken.IsQuotedIdentifierOn => typeInfo.InstanceProperties[4], 
				XmlToken.IsSchemaBound => typeInfo.InstanceProperties[5], 
				XmlToken.IsSqlClr => typeInfo.InstanceProperties[6], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[7], 
				XmlToken.Name => typeInfo.InstanceProperties[8], 
				XmlToken.Nullable => typeInfo.InstanceProperties[9], 
				XmlToken.Parameters => typeInfo.InstanceProperties[10], 
				XmlToken.ReturnsNullOnNullInput => typeInfo.InstanceProperties[11], 
				XmlToken.Schema => typeInfo.InstanceProperties[12], 
				_ => null, 
			};
		case 97:
			return propertyName switch
			{
				XmlToken.DataType => typeInfo.InstanceProperties[0], 
				XmlToken.Name => typeInfo.InstanceProperties[1], 
				XmlToken.Nullable => typeInfo.InstanceProperties[2], 
				_ => null, 
			};
		case 98:
			return propertyName switch
			{
				XmlToken.Database => typeInfo.InstanceProperties[0], 
				XmlToken.ExtendedStoredProcedures => typeInfo.InstanceProperties[1], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[2], 
				XmlToken.Name => typeInfo.InstanceProperties[3], 
				XmlToken.Owner => typeInfo.InstanceProperties[4], 
				XmlToken.ScalarValuedFunctions => typeInfo.InstanceProperties[5], 
				XmlToken.StoredProcedures => typeInfo.InstanceProperties[6], 
				XmlToken.Synonyms => typeInfo.InstanceProperties[7], 
				XmlToken.TableValuedFunctions => typeInfo.InstanceProperties[8], 
				XmlToken.Tables => typeInfo.InstanceProperties[9], 
				XmlToken.UserDefinedAggregates => typeInfo.InstanceProperties[10], 
				XmlToken.UserDefinedClrTypes => typeInfo.InstanceProperties[11], 
				XmlToken.UserDefinedDataTypes => typeInfo.InstanceProperties[12], 
				XmlToken.UserDefinedTableTypes => typeInfo.InstanceProperties[13], 
				XmlToken.Views => typeInfo.InstanceProperties[14], 
				_ => null, 
			};
		case 100:
			return propertyName switch
			{
				XmlToken.CollationInfo => typeInfo.InstanceProperties[0], 
				XmlToken.Credentials => typeInfo.InstanceProperties[1], 
				XmlToken.Databases => typeInfo.InstanceProperties[2], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[3], 
				XmlToken.Logins => typeInfo.InstanceProperties[4], 
				XmlToken.Name => typeInfo.InstanceProperties[5], 
				XmlToken.Triggers => typeInfo.InstanceProperties[6], 
				_ => null, 
			};
		case 101:
			return propertyName switch
			{
				XmlToken.BodyText => typeInfo.InstanceProperties[0], 
				XmlToken.ExecutionContext => typeInfo.InstanceProperties[1], 
				XmlToken.IsEnabled => typeInfo.InstanceProperties[2], 
				XmlToken.IsEncrypted => typeInfo.InstanceProperties[3], 
				XmlToken.IsQuotedIdentifierOn => typeInfo.InstanceProperties[4], 
				XmlToken.IsSqlClr => typeInfo.InstanceProperties[5], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[6], 
				XmlToken.Name => typeInfo.InstanceProperties[7], 
				XmlToken.Server => typeInfo.InstanceProperties[8], 
				XmlToken.ServerDdlEvents => typeInfo.InstanceProperties[9], 
				_ => null, 
			};
		case 103:
			return propertyName switch
			{
				XmlToken.BoundingBoxXMax => typeInfo.InstanceProperties[0], 
				XmlToken.BoundingBoxXMin => typeInfo.InstanceProperties[1], 
				XmlToken.BoundingBoxYMax => typeInfo.InstanceProperties[2], 
				XmlToken.BoundingBoxYMin => typeInfo.InstanceProperties[3], 
				XmlToken.CellsPerObject => typeInfo.InstanceProperties[4], 
				XmlToken.DisallowPageLocks => typeInfo.InstanceProperties[5], 
				XmlToken.DisallowRowLocks => typeInfo.InstanceProperties[6], 
				XmlToken.FillFactor => typeInfo.InstanceProperties[7], 
				XmlToken.IgnoreDuplicateKeys => typeInfo.InstanceProperties[8], 
				XmlToken.IndexedColumn => typeInfo.InstanceProperties[9], 
				XmlToken.IsDisabled => typeInfo.InstanceProperties[10], 
				XmlToken.Level1Density => typeInfo.InstanceProperties[11], 
				XmlToken.Level2Density => typeInfo.InstanceProperties[12], 
				XmlToken.Level3Density => typeInfo.InstanceProperties[13], 
				XmlToken.Level4Density => typeInfo.InstanceProperties[14], 
				XmlToken.Name => typeInfo.InstanceProperties[15], 
				XmlToken.NoAutomaticRecomputation => typeInfo.InstanceProperties[16], 
				XmlToken.PadIndex => typeInfo.InstanceProperties[17], 
				XmlToken.Parent => typeInfo.InstanceProperties[18], 
				_ => null, 
			};
		case 104:
			return propertyName switch
			{
				XmlToken.Columns => typeInfo.InstanceProperties[0], 
				XmlToken.FilterDefinition => typeInfo.InstanceProperties[1], 
				XmlToken.Name => typeInfo.InstanceProperties[2], 
				XmlToken.NoAutomaticRecomputation => typeInfo.InstanceProperties[3], 
				XmlToken.Parent => typeInfo.InstanceProperties[4], 
				XmlToken.Type => typeInfo.InstanceProperties[5], 
				_ => null, 
			};
		case 105:
			return propertyName switch
			{
				XmlToken.BodyText => typeInfo.InstanceProperties[0], 
				XmlToken.ExecutionContext => typeInfo.InstanceProperties[1], 
				XmlToken.ForReplication => typeInfo.InstanceProperties[2], 
				XmlToken.IsEncrypted => typeInfo.InstanceProperties[3], 
				XmlToken.IsQuotedIdentifierOn => typeInfo.InstanceProperties[4], 
				XmlToken.IsRecompiled => typeInfo.InstanceProperties[5], 
				XmlToken.IsSqlClr => typeInfo.InstanceProperties[6], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[7], 
				XmlToken.Name => typeInfo.InstanceProperties[8], 
				XmlToken.Parameters => typeInfo.InstanceProperties[9], 
				XmlToken.Schema => typeInfo.InstanceProperties[10], 
				XmlToken.Startup => typeInfo.InstanceProperties[11], 
				_ => null, 
			};
		case 106:
			return propertyName switch
			{
				XmlToken.BaseObjectName => typeInfo.InstanceProperties[0], 
				XmlToken.BaseType => typeInfo.InstanceProperties[1], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[2], 
				XmlToken.Name => typeInfo.InstanceProperties[3], 
				XmlToken.Schema => typeInfo.InstanceProperties[4], 
				_ => null, 
			};
		case 107:
			return propertyName switch
			{
				XmlToken.Length => typeInfo.InstanceProperties[0], 
				XmlToken.NumericPrecision => typeInfo.InstanceProperties[1], 
				XmlToken.NumericScale => typeInfo.InstanceProperties[2], 
				XmlToken.TypeSpec => typeInfo.InstanceProperties[3], 
				_ => null, 
			};
		case 108:
			return propertyName switch
			{
				XmlToken.Length => typeInfo.InstanceProperties[0], 
				XmlToken.NumericPrecision => typeInfo.InstanceProperties[1], 
				XmlToken.NumericScale => typeInfo.InstanceProperties[2], 
				XmlToken.TypeSpec => typeInfo.InstanceProperties[3], 
				_ => null, 
			};
		case 109:
			return propertyName switch
			{
				XmlToken.Columns => typeInfo.InstanceProperties[0], 
				XmlToken.Constraints => typeInfo.InstanceProperties[1], 
				XmlToken.Indexes => typeInfo.InstanceProperties[2], 
				XmlToken.IsQuotedIdentifierOn => typeInfo.InstanceProperties[3], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[4], 
				XmlToken.Name => typeInfo.InstanceProperties[5], 
				XmlToken.Schema => typeInfo.InstanceProperties[6], 
				XmlToken.Statistics => typeInfo.InstanceProperties[7], 
				XmlToken.Triggers => typeInfo.InstanceProperties[8], 
				_ => null, 
			};
		case 110:
			return propertyName switch
			{
				XmlToken.Columns => typeInfo.InstanceProperties[0], 
				XmlToken.Constraints => typeInfo.InstanceProperties[1], 
				XmlToken.Indexes => typeInfo.InstanceProperties[2], 
				XmlToken.Name => typeInfo.InstanceProperties[3], 
				XmlToken.Statistics => typeInfo.InstanceProperties[4], 
				_ => null, 
			};
		case 111:
			return propertyName switch
			{
				XmlToken.DataType => typeInfo.InstanceProperties[0], 
				XmlToken.Name => typeInfo.InstanceProperties[1], 
				_ => null, 
			};
		case 112:
			return propertyName switch
			{
				XmlToken.BodyText => typeInfo.InstanceProperties[0], 
				XmlToken.Columns => typeInfo.InstanceProperties[1], 
				XmlToken.Constraints => typeInfo.InstanceProperties[2], 
				XmlToken.ExecutionContext => typeInfo.InstanceProperties[3], 
				XmlToken.Indexes => typeInfo.InstanceProperties[4], 
				XmlToken.IsEncrypted => typeInfo.InstanceProperties[5], 
				XmlToken.IsInline => typeInfo.InstanceProperties[6], 
				XmlToken.IsQuotedIdentifierOn => typeInfo.InstanceProperties[7], 
				XmlToken.IsSchemaBound => typeInfo.InstanceProperties[8], 
				XmlToken.IsSqlClr => typeInfo.InstanceProperties[9], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[10], 
				XmlToken.Name => typeInfo.InstanceProperties[11], 
				XmlToken.Parameters => typeInfo.InstanceProperties[12], 
				XmlToken.Schema => typeInfo.InstanceProperties[13], 
				XmlToken.Statistics => typeInfo.InstanceProperties[14], 
				XmlToken.TableVariableName => typeInfo.InstanceProperties[15], 
				_ => null, 
			};
		case 113:
			return propertyName switch
			{
				XmlToken.DataType => typeInfo.InstanceProperties[0], 
				XmlToken.Name => typeInfo.InstanceProperties[1], 
				_ => null, 
			};
		case 118:
			return propertyName switch
			{
				XmlToken.DataType => typeInfo.InstanceProperties[0], 
				XmlToken.IsStatic => typeInfo.InstanceProperties[1], 
				XmlToken.Name => typeInfo.InstanceProperties[2], 
				XmlToken.Nullable => typeInfo.InstanceProperties[3], 
				XmlToken.UserDefinedType => typeInfo.InstanceProperties[4], 
				_ => null, 
			};
		case 120:
			return propertyName switch
			{
				XmlToken.DataType => typeInfo.InstanceProperties[0], 
				XmlToken.IsStatic => typeInfo.InstanceProperties[1], 
				XmlToken.Name => typeInfo.InstanceProperties[2], 
				XmlToken.Nullable => typeInfo.InstanceProperties[3], 
				XmlToken.Parameters => typeInfo.InstanceProperties[4], 
				XmlToken.UserDefinedType => typeInfo.InstanceProperties[5], 
				_ => null, 
			};
		case 121:
			return propertyName switch
			{
				XmlToken.AssociatedIndex => typeInfo.InstanceProperties[0], 
				XmlToken.IsSystemNamed => typeInfo.InstanceProperties[1], 
				XmlToken.Name => typeInfo.InstanceProperties[2], 
				XmlToken.Parent => typeInfo.InstanceProperties[3], 
				_ => null, 
			};
		case 123:
			return propertyName switch
			{
				XmlToken.AsymmetricKey => typeInfo.InstanceProperties[0], 
				XmlToken.Certificate => typeInfo.InstanceProperties[1], 
				XmlToken.Database => typeInfo.InstanceProperties[2], 
				XmlToken.DefaultSchema => typeInfo.InstanceProperties[3], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[4], 
				XmlToken.Login => typeInfo.InstanceProperties[5], 
				XmlToken.MemberOfRoles => typeInfo.InstanceProperties[6], 
				XmlToken.Name => typeInfo.InstanceProperties[7], 
				XmlToken.Password => typeInfo.InstanceProperties[8], 
				XmlToken.Permissions => typeInfo.InstanceProperties[9], 
				XmlToken.UserType => typeInfo.InstanceProperties[10], 
				_ => null, 
			};
		case 124:
			return propertyName switch
			{
				XmlToken.DataType => typeInfo.InstanceProperties[0], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[1], 
				XmlToken.Name => typeInfo.InstanceProperties[2], 
				XmlToken.Nullable => typeInfo.InstanceProperties[3], 
				XmlToken.Parameters => typeInfo.InstanceProperties[4], 
				XmlToken.Schema => typeInfo.InstanceProperties[5], 
				_ => null, 
			};
		case 125:
			return propertyName switch
			{
				XmlToken.AssemblyName => typeInfo.InstanceProperties[0], 
				XmlToken.ClassName => typeInfo.InstanceProperties[1], 
				XmlToken.DataMembers => typeInfo.InstanceProperties[2], 
				XmlToken.IsBinaryOrdered => typeInfo.InstanceProperties[3], 
				XmlToken.IsComVisible => typeInfo.InstanceProperties[4], 
				XmlToken.IsNullable => typeInfo.InstanceProperties[5], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[6], 
				XmlToken.Methods => typeInfo.InstanceProperties[7], 
				XmlToken.Name => typeInfo.InstanceProperties[8], 
				XmlToken.Schema => typeInfo.InstanceProperties[9], 
				_ => null, 
			};
		case 126:
			return propertyName switch
			{
				XmlToken.BaseSystemDataType => typeInfo.InstanceProperties[0], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[1], 
				XmlToken.Name => typeInfo.InstanceProperties[2], 
				XmlToken.Nullable => typeInfo.InstanceProperties[3], 
				XmlToken.Schema => typeInfo.InstanceProperties[4], 
				_ => null, 
			};
		case 129:
			return propertyName switch
			{
				XmlToken.Columns => typeInfo.InstanceProperties[0], 
				XmlToken.Constraints => typeInfo.InstanceProperties[1], 
				XmlToken.Indexes => typeInfo.InstanceProperties[2], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[3], 
				XmlToken.Name => typeInfo.InstanceProperties[4], 
				XmlToken.Schema => typeInfo.InstanceProperties[5], 
				XmlToken.Statistics => typeInfo.InstanceProperties[6], 
				_ => null, 
			};
		case 131:
			return propertyName switch
			{
				XmlToken.Columns => typeInfo.InstanceProperties[0], 
				XmlToken.Constraints => typeInfo.InstanceProperties[1], 
				XmlToken.HasCheckOption => typeInfo.InstanceProperties[2], 
				XmlToken.HasColumnSpecification => typeInfo.InstanceProperties[3], 
				XmlToken.Indexes => typeInfo.InstanceProperties[4], 
				XmlToken.IsEncrypted => typeInfo.InstanceProperties[5], 
				XmlToken.IsQuotedIdentifierOn => typeInfo.InstanceProperties[6], 
				XmlToken.IsSchemaBound => typeInfo.InstanceProperties[7], 
				XmlToken.IsSystemObject => typeInfo.InstanceProperties[8], 
				XmlToken.Name => typeInfo.InstanceProperties[9], 
				XmlToken.QueryText => typeInfo.InstanceProperties[10], 
				XmlToken.ReturnsViewMetadata => typeInfo.InstanceProperties[11], 
				XmlToken.Schema => typeInfo.InstanceProperties[12], 
				XmlToken.Statistics => typeInfo.InstanceProperties[13], 
				XmlToken.Triggers => typeInfo.InstanceProperties[14], 
				_ => null, 
			};
		case 132:
			return null;
		case 133:
			return propertyName switch
			{
				XmlToken.IsXmlNode => typeInfo.InstanceProperties[0], 
				XmlToken.Length => typeInfo.InstanceProperties[1], 
				XmlToken.NumericPrecision => typeInfo.InstanceProperties[2], 
				XmlToken.NumericScale => typeInfo.InstanceProperties[3], 
				XmlToken.TypeSpec => typeInfo.InstanceProperties[4], 
				_ => null, 
			};
		case 136:
			return propertyName switch
			{
				XmlToken.DisallowPageLocks => typeInfo.InstanceProperties[0], 
				XmlToken.DisallowRowLocks => typeInfo.InstanceProperties[1], 
				XmlToken.FillFactor => typeInfo.InstanceProperties[2], 
				XmlToken.IgnoreDuplicateKeys => typeInfo.InstanceProperties[3], 
				XmlToken.IsDisabled => typeInfo.InstanceProperties[4], 
				XmlToken.Name => typeInfo.InstanceProperties[5], 
				XmlToken.PadIndex => typeInfo.InstanceProperties[6], 
				XmlToken.Parent => typeInfo.InstanceProperties[7], 
				_ => null, 
			};
		default:
			return null;
		}
	}

	public static MetadataPropertyInfo GetCollectionProperty(MetadataTypeInfo typeInfo, TypeInfo elemType)
	{
		switch (typeInfo.Id)
		{
		case 15:
			if (elemType.Id == 33)
			{
				return typeInfo.InstanceProperties[5];
			}
			return null;
		case 16:
			_ = elemType.Id;
			return null;
		case 18:
			_ = elemType.Id;
			return null;
		case 19:
			_ = elemType.Id;
			return null;
		case 20:
			_ = elemType.Id;
			return null;
		case 21:
			_ = elemType.Id;
			return null;
		case 22:
			_ = elemType.Id;
			return null;
		case 24:
			_ = elemType.Id;
			return null;
		case 25:
			_ = elemType.Id;
			return null;
		case 26:
			_ = elemType.Id;
			return null;
		case 27:
			_ = elemType.Id;
			return null;
		case 29:
			return elemType.Id switch
			{
				15 => typeInfo.InstanceProperties[0], 
				16 => typeInfo.InstanceProperties[1], 
				18 => typeInfo.InstanceProperties[2], 
				30 => typeInfo.InstanceProperties[12], 
				35 => typeInfo.InstanceProperties[9], 
				98 => typeInfo.InstanceProperties[10], 
				123 => typeInfo.InstanceProperties[13], 
				_ => null, 
			};
		case 30:
			_ = elemType.Id;
			return null;
		case 33:
			_ = elemType.Id;
			return null;
		case 35:
			if (elemType.Id == 33)
			{
				return typeInfo.InstanceProperties[6];
			}
			return null;
		case 37:
			_ = elemType.Id;
			return null;
		case 38:
			_ = elemType.Id;
			return null;
		case 39:
			_ = elemType.Id;
			return null;
		case 40:
			_ = elemType.Id;
			return null;
		case 41:
			_ = elemType.Id;
			return null;
		case 42:
			_ = elemType.Id;
			return null;
		case 43:
			_ = elemType.Id;
			return null;
		case 47:
			_ = elemType.Id;
			return null;
		case 49:
			_ = elemType.Id;
			return null;
		case 88:
			if (elemType.Id == 41)
			{
				return typeInfo.InstanceProperties[1];
			}
			return null;
		case 90:
			_ = elemType.Id;
			return null;
		case 91:
			_ = elemType.Id;
			return null;
		case 93:
			_ = elemType.Id;
			return null;
		case 95:
			_ = elemType.Id;
			return null;
		case 96:
			_ = elemType.Id;
			return null;
		case 97:
			_ = elemType.Id;
			return null;
		case 98:
			return elemType.Id switch
			{
				40 => typeInfo.InstanceProperties[1], 
				96 => typeInfo.InstanceProperties[5], 
				105 => typeInfo.InstanceProperties[6], 
				106 => typeInfo.InstanceProperties[7], 
				109 => typeInfo.InstanceProperties[9], 
				112 => typeInfo.InstanceProperties[8], 
				124 => typeInfo.InstanceProperties[10], 
				125 => typeInfo.InstanceProperties[11], 
				126 => typeInfo.InstanceProperties[12], 
				129 => typeInfo.InstanceProperties[13], 
				131 => typeInfo.InstanceProperties[14], 
				_ => null, 
			};
		case 100:
			return elemType.Id switch
			{
				24 => typeInfo.InstanceProperties[1], 
				29 => typeInfo.InstanceProperties[2], 
				49 => typeInfo.InstanceProperties[4], 
				101 => typeInfo.InstanceProperties[6], 
				_ => null, 
			};
		case 101:
			_ = elemType.Id;
			return null;
		case 103:
			_ = elemType.Id;
			return null;
		case 104:
			_ = elemType.Id;
			return null;
		case 105:
			_ = elemType.Id;
			return null;
		case 106:
			_ = elemType.Id;
			return null;
		case 107:
			_ = elemType.Id;
			return null;
		case 108:
			_ = elemType.Id;
			return null;
		case 109:
			return elemType.Id switch
			{
				19 => typeInfo.InstanceProperties[1], 
				22 => typeInfo.InstanceProperties[0], 
				38 => typeInfo.InstanceProperties[8], 
				43 => typeInfo.InstanceProperties[1], 
				90 => typeInfo.InstanceProperties[1], 
				91 => typeInfo.InstanceProperties[2], 
				103 => typeInfo.InstanceProperties[2], 
				104 => typeInfo.InstanceProperties[7], 
				121 => typeInfo.InstanceProperties[1], 
				136 => typeInfo.InstanceProperties[2], 
				_ => null, 
			};
		case 110:
			return elemType.Id switch
			{
				19 => typeInfo.InstanceProperties[1], 
				22 => typeInfo.InstanceProperties[0], 
				43 => typeInfo.InstanceProperties[1], 
				90 => typeInfo.InstanceProperties[1], 
				91 => typeInfo.InstanceProperties[2], 
				103 => typeInfo.InstanceProperties[2], 
				104 => typeInfo.InstanceProperties[4], 
				121 => typeInfo.InstanceProperties[1], 
				136 => typeInfo.InstanceProperties[2], 
				_ => null, 
			};
		case 111:
			_ = elemType.Id;
			return null;
		case 112:
			return elemType.Id switch
			{
				19 => typeInfo.InstanceProperties[2], 
				22 => typeInfo.InstanceProperties[1], 
				43 => typeInfo.InstanceProperties[2], 
				90 => typeInfo.InstanceProperties[2], 
				91 => typeInfo.InstanceProperties[4], 
				103 => typeInfo.InstanceProperties[4], 
				104 => typeInfo.InstanceProperties[14], 
				121 => typeInfo.InstanceProperties[2], 
				136 => typeInfo.InstanceProperties[4], 
				_ => null, 
			};
		case 113:
			_ = elemType.Id;
			return null;
		case 118:
			_ = elemType.Id;
			return null;
		case 120:
			_ = elemType.Id;
			return null;
		case 121:
			_ = elemType.Id;
			return null;
		case 123:
			if (elemType.Id == 33)
			{
				return typeInfo.InstanceProperties[9];
			}
			return null;
		case 124:
			_ = elemType.Id;
			return null;
		case 125:
			_ = elemType.Id;
			return null;
		case 126:
			_ = elemType.Id;
			return null;
		case 129:
			return elemType.Id switch
			{
				19 => typeInfo.InstanceProperties[1], 
				22 => typeInfo.InstanceProperties[0], 
				43 => typeInfo.InstanceProperties[1], 
				90 => typeInfo.InstanceProperties[1], 
				91 => typeInfo.InstanceProperties[2], 
				103 => typeInfo.InstanceProperties[2], 
				104 => typeInfo.InstanceProperties[6], 
				121 => typeInfo.InstanceProperties[1], 
				136 => typeInfo.InstanceProperties[2], 
				_ => null, 
			};
		case 131:
			return elemType.Id switch
			{
				19 => typeInfo.InstanceProperties[1], 
				22 => typeInfo.InstanceProperties[0], 
				38 => typeInfo.InstanceProperties[14], 
				43 => typeInfo.InstanceProperties[1], 
				90 => typeInfo.InstanceProperties[1], 
				91 => typeInfo.InstanceProperties[4], 
				103 => typeInfo.InstanceProperties[4], 
				104 => typeInfo.InstanceProperties[13], 
				121 => typeInfo.InstanceProperties[1], 
				136 => typeInfo.InstanceProperties[4], 
				_ => null, 
			};
		case 132:
			_ = elemType.Id;
			return null;
		case 133:
			_ = elemType.Id;
			return null;
		case 136:
			_ = elemType.Id;
			return null;
		default:
			return null;
		}
	}

	public static bool IncludeParentProperty(MetadataTypeInfo typeInfo)
	{
		return typeInfo.Id switch
		{
			15 => true, 
			16 => true, 
			18 => true, 
			19 => true, 
			20 => false, 
			21 => false, 
			22 => false, 
			24 => true, 
			25 => false, 
			26 => false, 
			27 => false, 
			29 => true, 
			30 => true, 
			33 => true, 
			35 => true, 
			37 => false, 
			38 => true, 
			39 => false, 
			40 => true, 
			41 => true, 
			42 => false, 
			43 => true, 
			47 => false, 
			49 => true, 
			88 => false, 
			90 => true, 
			91 => true, 
			93 => false, 
			95 => false, 
			96 => true, 
			97 => false, 
			98 => true, 
			100 => false, 
			101 => true, 
			103 => true, 
			104 => true, 
			105 => true, 
			106 => true, 
			107 => false, 
			108 => false, 
			109 => true, 
			110 => false, 
			111 => false, 
			112 => true, 
			113 => false, 
			118 => false, 
			120 => false, 
			121 => true, 
			123 => true, 
			124 => true, 
			125 => true, 
			126 => true, 
			129 => true, 
			131 => true, 
			132 => false, 
			133 => false, 
			136 => true, 
			_ => false, 
		};
	}

	public static IMetadataObject GetChildElement(MetadataTypeInfo typeInfo, TypeInfo elemType, IMetadataObject obj, string name)
	{
		switch (typeInfo.Id)
		{
		case 15:
			if (elemType.Id == 33)
			{
				return ((IApplicationRole)obj).Permissions[name];
			}
			return null;
		case 16:
			_ = elemType.Id;
			return null;
		case 18:
			_ = elemType.Id;
			return null;
		case 19:
			_ = elemType.Id;
			return null;
		case 20:
			_ = elemType.Id;
			return null;
		case 21:
			_ = elemType.Id;
			return null;
		case 22:
			_ = elemType.Id;
			return null;
		case 24:
			_ = elemType.Id;
			return null;
		case 25:
			_ = elemType.Id;
			return null;
		case 26:
			_ = elemType.Id;
			return null;
		case 27:
			_ = elemType.Id;
			return null;
		case 29:
			return elemType.Id switch
			{
				15 => ((IDatabase)obj).ApplicationRoles[name], 
				16 => ((IDatabase)obj).AsymmetricKeys[name], 
				18 => ((IDatabase)obj).Certificates[name], 
				30 => ((IDatabase)obj).Triggers[name], 
				35 => ((IDatabase)obj).Roles[name], 
				98 => ((IDatabase)obj).Schemas[name], 
				123 => ((IDatabase)obj).Users[name], 
				_ => null, 
			};
		case 30:
			_ = elemType.Id;
			return null;
		case 33:
			_ = elemType.Id;
			return null;
		case 35:
			if (elemType.Id == 33)
			{
				return ((IDatabaseRole)obj).Permissions[name];
			}
			return null;
		case 37:
			_ = elemType.Id;
			return null;
		case 38:
			_ = elemType.Id;
			return null;
		case 39:
			_ = elemType.Id;
			return null;
		case 40:
			_ = elemType.Id;
			return null;
		case 41:
			_ = elemType.Id;
			return null;
		case 42:
			_ = elemType.Id;
			return null;
		case 43:
			_ = elemType.Id;
			return null;
		case 47:
			_ = elemType.Id;
			return null;
		case 49:
			_ = elemType.Id;
			return null;
		case 88:
			if (elemType.Id == 41)
			{
				return ((IPartitionScheme)obj).FileGroups[name];
			}
			return null;
		case 90:
			_ = elemType.Id;
			return null;
		case 91:
			_ = elemType.Id;
			return null;
		case 93:
			_ = elemType.Id;
			return null;
		case 95:
			_ = elemType.Id;
			return null;
		case 96:
			_ = elemType.Id;
			return null;
		case 97:
			_ = elemType.Id;
			return null;
		case 98:
			return elemType.Id switch
			{
				40 => ((ISchema)obj).ExtendedStoredProcedures[name], 
				96 => ((ISchema)obj).ScalarValuedFunctions[name], 
				105 => ((ISchema)obj).StoredProcedures[name], 
				106 => ((ISchema)obj).Synonyms[name], 
				109 => ((ISchema)obj).Tables[name], 
				112 => ((ISchema)obj).TableValuedFunctions[name], 
				124 => ((ISchema)obj).UserDefinedAggregates[name], 
				125 => ((ISchema)obj).UserDefinedClrTypes[name], 
				126 => ((ISchema)obj).UserDefinedDataTypes[name], 
				129 => ((ISchema)obj).UserDefinedTableTypes[name], 
				131 => ((ISchema)obj).Views[name], 
				_ => null, 
			};
		case 100:
			return elemType.Id switch
			{
				24 => ((IServer)obj).Credentials[name], 
				29 => ((IServer)obj).Databases[name], 
				49 => ((IServer)obj).Logins[name], 
				101 => ((IServer)obj).Triggers[name], 
				_ => null, 
			};
		case 101:
			_ = elemType.Id;
			return null;
		case 103:
			_ = elemType.Id;
			return null;
		case 104:
			_ = elemType.Id;
			return null;
		case 105:
			_ = elemType.Id;
			return null;
		case 106:
			_ = elemType.Id;
			return null;
		case 107:
			_ = elemType.Id;
			return null;
		case 108:
			_ = elemType.Id;
			return null;
		case 109:
			return elemType.Id switch
			{
				19 => ((ITable)obj).Constraints[name], 
				22 => ((ITable)obj).Columns[name], 
				38 => ((ITable)obj).Triggers[name], 
				43 => ((ITable)obj).Constraints[name], 
				90 => ((ITable)obj).Constraints[name], 
				91 => ((ITable)obj).Indexes[name], 
				103 => ((ITable)obj).Indexes[name], 
				104 => ((ITable)obj).Statistics[name], 
				121 => ((ITable)obj).Constraints[name], 
				136 => ((ITable)obj).Indexes[name], 
				_ => null, 
			};
		case 110:
			return elemType.Id switch
			{
				19 => ((ITableDataType)obj).Constraints[name], 
				22 => ((ITableDataType)obj).Columns[name], 
				43 => ((ITableDataType)obj).Constraints[name], 
				90 => ((ITableDataType)obj).Constraints[name], 
				91 => ((ITableDataType)obj).Indexes[name], 
				103 => ((ITableDataType)obj).Indexes[name], 
				104 => ((ITableDataType)obj).Statistics[name], 
				121 => ((ITableDataType)obj).Constraints[name], 
				136 => ((ITableDataType)obj).Indexes[name], 
				_ => null, 
			};
		case 111:
			_ = elemType.Id;
			return null;
		case 112:
			return elemType.Id switch
			{
				19 => ((ITableValuedFunction)obj).Constraints[name], 
				22 => ((ITableValuedFunction)obj).Columns[name], 
				43 => ((ITableValuedFunction)obj).Constraints[name], 
				90 => ((ITableValuedFunction)obj).Constraints[name], 
				91 => ((ITableValuedFunction)obj).Indexes[name], 
				103 => ((ITableValuedFunction)obj).Indexes[name], 
				104 => ((ITableValuedFunction)obj).Statistics[name], 
				121 => ((ITableValuedFunction)obj).Constraints[name], 
				136 => ((ITableValuedFunction)obj).Indexes[name], 
				_ => null, 
			};
		case 113:
			_ = elemType.Id;
			return null;
		case 118:
			_ = elemType.Id;
			return null;
		case 120:
			_ = elemType.Id;
			return null;
		case 121:
			_ = elemType.Id;
			return null;
		case 123:
			if (elemType.Id == 33)
			{
				return ((IUser)obj).Permissions[name];
			}
			return null;
		case 124:
			_ = elemType.Id;
			return null;
		case 125:
			_ = elemType.Id;
			return null;
		case 126:
			_ = elemType.Id;
			return null;
		case 129:
			return elemType.Id switch
			{
				19 => ((IUserDefinedTableType)obj).Constraints[name], 
				22 => ((IUserDefinedTableType)obj).Columns[name], 
				43 => ((IUserDefinedTableType)obj).Constraints[name], 
				90 => ((IUserDefinedTableType)obj).Constraints[name], 
				91 => ((IUserDefinedTableType)obj).Indexes[name], 
				103 => ((IUserDefinedTableType)obj).Indexes[name], 
				104 => ((IUserDefinedTableType)obj).Statistics[name], 
				121 => ((IUserDefinedTableType)obj).Constraints[name], 
				136 => ((IUserDefinedTableType)obj).Indexes[name], 
				_ => null, 
			};
		case 131:
			return elemType.Id switch
			{
				19 => ((IView)obj).Constraints[name], 
				22 => ((IView)obj).Columns[name], 
				38 => ((IView)obj).Triggers[name], 
				43 => ((IView)obj).Constraints[name], 
				90 => ((IView)obj).Constraints[name], 
				91 => ((IView)obj).Indexes[name], 
				103 => ((IView)obj).Indexes[name], 
				104 => ((IView)obj).Statistics[name], 
				121 => ((IView)obj).Constraints[name], 
				136 => ((IView)obj).Indexes[name], 
				_ => null, 
			};
		case 132:
			_ = elemType.Id;
			return null;
		case 133:
			_ = elemType.Id;
			return null;
		case 136:
			_ = elemType.Id;
			return null;
		default:
			return null;
		}
	}

	public static bool IncludeProperty(MetadataTypeInfo typeInfo, MetadataPropertyInfo propertyInfo, IXmlSerializationSettings serializationSettings)
	{
		TypeInfo returnType = propertyInfo.ReturnType;
		if (returnType.IsCollection)
		{
			MetadataTypeInfo typeInfo2 = (MetadataTypeInfo)((CollectionTypeInfo)returnType).ElementType;
			XmlNestingType nestingType = serializationSettings.GetNestingType(typeInfo2);
			if (!((CollectionTypeInfo)returnType).IsOrdered && propertyInfo.PropertyType == PropertyType.Child)
			{
				return IsInlineNesting(nestingType);
			}
			return true;
		}
		if (propertyInfo == Login_Sid || propertyInfo == Database_Owner)
		{
			return false;
		}
		if (returnType.Id == 89)
		{
			return false;
		}
		return true;
	}

	public static bool IsInlineNesting(XmlNestingType nestingType)
	{
		switch (nestingType)
		{
		case XmlNestingType.Standalone:
			return false;
		case XmlNestingType.InlineDynamicType:
		case XmlNestingType.InlineStaticType:
			return true;
		default:
			return false;
		}
	}

	public static bool IsStaticTypeNesting(XmlNestingType nestingType)
	{
		switch (nestingType)
		{
		case XmlNestingType.Standalone:
		case XmlNestingType.InlineDynamicType:
			return false;
		case XmlNestingType.InlineStaticType:
			return true;
		default:
			return false;
		}
	}
}
