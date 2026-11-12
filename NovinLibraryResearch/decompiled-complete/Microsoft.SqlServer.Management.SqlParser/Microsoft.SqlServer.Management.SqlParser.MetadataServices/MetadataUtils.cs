using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataServices;

public static class MetadataUtils
{
	private static readonly TypeInfoVisitor typeInfoVisitor = new TypeInfoVisitor();

	public static object GetPropertyValue(IMetadataObject obj, MetadataTypeInfo typeInfo, MetadataPropertyInfo propertyInfo)
	{
		object result = null;
		switch (typeInfo.Id)
		{
		case 15:
		{
			IApplicationRole applicationRole = (IApplicationRole)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = applicationRole.Database;
				break;
			case 1:
				result = applicationRole.DefaultSchema;
				break;
			case 2:
				result = applicationRole.IsSystemObject;
				break;
			case 3:
				result = applicationRole.MemberOfRoles;
				break;
			case 4:
				result = applicationRole.Name;
				break;
			case 5:
				result = applicationRole.Permissions;
				break;
			}
			break;
		}
		case 16:
		{
			IAsymmetricKey asymmetricKey = (IAsymmetricKey)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = asymmetricKey.Database;
				break;
			case 1:
				result = asymmetricKey.IsSystemObject;
				break;
			case 2:
				result = asymmetricKey.Name;
				break;
			}
			break;
		}
		case 18:
		{
			ICertificate certificate = (ICertificate)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = certificate.Database;
				break;
			case 1:
				result = certificate.IsSystemObject;
				break;
			case 2:
				result = certificate.Name;
				break;
			}
			break;
		}
		case 19:
		{
			ICheckConstraint checkConstraint = (ICheckConstraint)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = checkConstraint.IsChecked;
				break;
			case 1:
				result = checkConstraint.IsEnabled;
				break;
			case 2:
				result = checkConstraint.IsSystemNamed;
				break;
			case 3:
				result = checkConstraint.Name;
				break;
			case 4:
				result = checkConstraint.NotForReplication;
				break;
			case 5:
				result = checkConstraint.Parent;
				break;
			case 6:
				result = checkConstraint.Text;
				break;
			}
			break;
		}
		case 20:
		{
			IClrDataType clrDataType = (IClrDataType)obj;
			if (propertyInfo.Index == 0)
			{
				result = clrDataType.Name;
			}
			break;
		}
		case 21:
		{
			ICollation collation = (ICollation)obj;
			if (propertyInfo.Index == 0)
			{
				result = collation.Name;
			}
			break;
		}
		case 22:
		{
			IColumn column = (IColumn)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = column.Collation;
				break;
			case 1:
				result = column.ComputedColumnInfo;
				break;
			case 2:
				result = column.DataType;
				break;
			case 3:
				result = column.DefaultValue;
				break;
			case 4:
				result = column.IdentityColumnInfo;
				break;
			case 5:
				result = column.InPrimaryKey;
				break;
			case 6:
				result = column.IsColumnSet;
				break;
			case 7:
				result = column.IsGeneratedAlwaysAsRowEnd;
				break;
			case 8:
				result = column.IsGeneratedAlwaysAsRowStart;
				break;
			case 9:
				result = column.IsGeneratedAlwaysAsSequenceNumberEnd;
				break;
			case 10:
				result = column.IsGeneratedAlwaysAsSequenceNumberStart;
				break;
			case 11:
				result = column.IsGeneratedAlwaysAsTransactionIdEnd;
				break;
			case 12:
				result = column.IsGeneratedAlwaysAsTransactionIdStart;
				break;
			case 13:
				result = column.IsSparse;
				break;
			case 14:
				result = column.Name;
				break;
			case 15:
				result = column.Nullable;
				break;
			case 16:
				result = column.Parent;
				break;
			case 17:
				result = column.RowGuidCol;
				break;
			}
			break;
		}
		case 24:
		{
			ICredential credential = (ICredential)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = credential.IsSystemObject;
				break;
			case 1:
				result = credential.Name;
				break;
			case 2:
				result = credential.Server;
				break;
			}
			break;
		}
		case 26:
		{
			ICursorParameter cursorParameter = (ICursorParameter)obj;
			if (propertyInfo.Index == 0)
			{
				result = cursorParameter.Name;
			}
			break;
		}
		case 27:
		{
			ICursorVariable cursorVariable = (ICursorVariable)obj;
			if (propertyInfo.Index == 0)
			{
				result = cursorVariable.Name;
			}
			break;
		}
		case 29:
		{
			IDatabase database = (IDatabase)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = database.ApplicationRoles;
				break;
			case 1:
				result = database.AsymmetricKeys;
				break;
			case 2:
				result = database.Certificates;
				break;
			case 3:
				result = database.CollationInfo;
				break;
			case 4:
				result = database.CompatibilityLevel;
				break;
			case 5:
				result = database.DefaultSchemaName;
				break;
			case 6:
				result = database.IsSystemObject;
				break;
			case 7:
				result = database.Name;
				break;
			case 8:
				result = database.Owner;
				break;
			case 9:
				result = database.Roles;
				break;
			case 10:
				result = database.Schemas;
				break;
			case 11:
				result = database.Server;
				break;
			case 12:
				result = database.Triggers;
				break;
			case 13:
				result = database.Users;
				break;
			}
			break;
		}
		case 30:
		{
			IDatabaseDdlTrigger databaseDdlTrigger = (IDatabaseDdlTrigger)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = databaseDdlTrigger.BodyText;
				break;
			case 1:
				result = databaseDdlTrigger.Database;
				break;
			case 2:
				result = databaseDdlTrigger.DatabaseDdlEvents;
				break;
			case 3:
				result = databaseDdlTrigger.ExecutionContext;
				break;
			case 4:
				result = databaseDdlTrigger.IsEnabled;
				break;
			case 5:
				result = databaseDdlTrigger.IsEncrypted;
				break;
			case 6:
				result = databaseDdlTrigger.IsQuotedIdentifierOn;
				break;
			case 7:
				result = databaseDdlTrigger.IsSqlClr;
				break;
			case 8:
				result = databaseDdlTrigger.IsSystemObject;
				break;
			case 9:
				result = databaseDdlTrigger.Name;
				break;
			}
			break;
		}
		case 33:
		{
			IDatabasePermission databasePermission = (IDatabasePermission)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = databasePermission.DatabasePrincipal;
				break;
			case 1:
				result = databasePermission.Grantor;
				break;
			case 2:
				result = databasePermission.PermissionState;
				break;
			case 3:
				result = databasePermission.PermissionType;
				break;
			case 4:
				result = databasePermission.TargetObject;
				break;
			}
			break;
		}
		case 35:
		{
			IDatabaseRole databaseRole = (IDatabaseRole)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = databaseRole.Database;
				break;
			case 1:
				result = databaseRole.IsFixedRole;
				break;
			case 2:
				result = databaseRole.IsSystemObject;
				break;
			case 3:
				result = databaseRole.MemberOfRoles;
				break;
			case 4:
				result = databaseRole.Name;
				break;
			case 5:
				result = databaseRole.Owner;
				break;
			case 6:
				result = databaseRole.Permissions;
				break;
			}
			break;
		}
		case 37:
		{
			IDefaultConstraint defaultConstraint = (IDefaultConstraint)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = defaultConstraint.IsSystemNamed;
				break;
			case 1:
				result = defaultConstraint.Name;
				break;
			case 2:
				result = defaultConstraint.Parent;
				break;
			case 3:
				result = defaultConstraint.Text;
				break;
			}
			break;
		}
		case 38:
		{
			IDmlTrigger dmlTrigger = (IDmlTrigger)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = dmlTrigger.BodyText;
				break;
			case 1:
				result = dmlTrigger.Delete;
				break;
			case 2:
				result = dmlTrigger.DeleteActivationOrder;
				break;
			case 3:
				result = dmlTrigger.ExecutionContext;
				break;
			case 4:
				result = dmlTrigger.Insert;
				break;
			case 5:
				result = dmlTrigger.InsertActivationOrder;
				break;
			case 6:
				result = dmlTrigger.InsteadOf;
				break;
			case 7:
				result = dmlTrigger.IsEnabled;
				break;
			case 8:
				result = dmlTrigger.IsEncrypted;
				break;
			case 9:
				result = dmlTrigger.IsQuotedIdentifierOn;
				break;
			case 10:
				result = dmlTrigger.IsSqlClr;
				break;
			case 11:
				result = dmlTrigger.Name;
				break;
			case 12:
				result = dmlTrigger.NotForReplication;
				break;
			case 13:
				result = dmlTrigger.Parent;
				break;
			case 14:
				result = dmlTrigger.Update;
				break;
			case 15:
				result = dmlTrigger.UpdateActivationOrder;
				break;
			}
			break;
		}
		case 39:
		{
			IExecutionContext executionContext = (IExecutionContext)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = executionContext.ContextType;
				break;
			case 1:
				result = executionContext.Login;
				break;
			case 2:
				result = executionContext.User;
				break;
			}
			break;
		}
		case 40:
		{
			IExtendedStoredProcedure extendedStoredProcedure = (IExtendedStoredProcedure)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = extendedStoredProcedure.ExecutionContext;
				break;
			case 1:
				result = extendedStoredProcedure.IsEncrypted;
				break;
			case 2:
				result = extendedStoredProcedure.IsSystemObject;
				break;
			case 3:
				result = extendedStoredProcedure.Name;
				break;
			case 4:
				result = extendedStoredProcedure.Parameters;
				break;
			case 5:
				result = extendedStoredProcedure.ReturnType;
				break;
			case 6:
				result = extendedStoredProcedure.Schema;
				break;
			}
			break;
		}
		case 41:
		{
			IFileGroup fileGroup = (IFileGroup)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = fileGroup.Database;
				break;
			case 1:
				result = fileGroup.IsDefault;
				break;
			case 2:
				result = fileGroup.IsFileStream;
				break;
			case 3:
				result = fileGroup.IsReadOnly;
				break;
			case 4:
				result = fileGroup.IsSystemObject;
				break;
			case 5:
				result = fileGroup.Name;
				break;
			}
			break;
		}
		case 42:
		{
			IForeignKeyColumn foreignKeyColumn = (IForeignKeyColumn)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = foreignKeyColumn.ReferencedColumn;
				break;
			case 1:
				result = foreignKeyColumn.ReferencingColumn;
				break;
			}
			break;
		}
		case 43:
		{
			IForeignKeyConstraint foreignKeyConstraint = (IForeignKeyConstraint)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = foreignKeyConstraint.Columns;
				break;
			case 1:
				result = foreignKeyConstraint.DeleteAction;
				break;
			case 2:
				result = foreignKeyConstraint.IsChecked;
				break;
			case 3:
				result = foreignKeyConstraint.IsEnabled;
				break;
			case 4:
				result = foreignKeyConstraint.IsSystemNamed;
				break;
			case 5:
				result = foreignKeyConstraint.Name;
				break;
			case 6:
				result = foreignKeyConstraint.NotForReplication;
				break;
			case 7:
				result = foreignKeyConstraint.Parent;
				break;
			case 8:
				result = foreignKeyConstraint.ReferencedTable;
				break;
			case 9:
				result = foreignKeyConstraint.UpdateAction;
				break;
			}
			break;
		}
		case 47:
		{
			IIndexedColumn indexedColumn = (IIndexedColumn)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = indexedColumn.IsIncluded;
				break;
			case 1:
				result = indexedColumn.ReferencedColumn;
				break;
			case 2:
				result = indexedColumn.SortOrder;
				break;
			}
			break;
		}
		case 49:
		{
			ILogin login = (ILogin)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = login.AsymmetricKey;
				break;
			case 1:
				result = login.Certificate;
				break;
			case 2:
				result = login.Credential;
				break;
			case 3:
				result = login.DefaultDatabase;
				break;
			case 4:
				result = login.IsSystemObject;
				break;
			case 5:
				result = login.Language;
				break;
			case 6:
				result = login.LoginType;
				break;
			case 7:
				result = login.Name;
				break;
			case 8:
				result = login.Password;
				break;
			case 9:
				result = login.Server;
				break;
			case 10:
				result = login.Sid;
				break;
			}
			break;
		}
		case 88:
		{
			IPartitionScheme partitionScheme = (IPartitionScheme)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = partitionScheme.Database;
				break;
			case 1:
				result = partitionScheme.FileGroups;
				break;
			case 2:
				result = partitionScheme.IsSystemObject;
				break;
			case 3:
				result = partitionScheme.Name;
				break;
			}
			break;
		}
		case 90:
		{
			IPrimaryKeyConstraint primaryKeyConstraint = (IPrimaryKeyConstraint)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = primaryKeyConstraint.AssociatedIndex;
				break;
			case 1:
				result = primaryKeyConstraint.IsSystemNamed;
				break;
			case 2:
				result = primaryKeyConstraint.Name;
				break;
			case 3:
				result = primaryKeyConstraint.Parent;
				break;
			}
			break;
		}
		case 91:
		{
			IRelationalIndex relationalIndex = (IRelationalIndex)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = relationalIndex.CompactLargeObjects;
				break;
			case 1:
				result = relationalIndex.DisallowPageLocks;
				break;
			case 2:
				result = relationalIndex.DisallowRowLocks;
				break;
			case 3:
				result = relationalIndex.FileGroup;
				break;
			case 4:
				result = relationalIndex.FileStreamFileGroup;
				break;
			case 5:
				result = relationalIndex.FileStreamPartitionScheme;
				break;
			case 6:
				result = relationalIndex.FillFactor;
				break;
			case 7:
				result = relationalIndex.FilterDefinition;
				break;
			case 8:
				result = relationalIndex.IgnoreDuplicateKeys;
				break;
			case 9:
				result = relationalIndex.IndexKey;
				break;
			case 10:
				result = relationalIndex.IndexedColumns;
				break;
			case 11:
				result = relationalIndex.IsClustered;
				break;
			case 12:
				result = relationalIndex.IsDisabled;
				break;
			case 13:
				result = relationalIndex.IsSystemNamed;
				break;
			case 14:
				result = relationalIndex.IsUnique;
				break;
			case 15:
				result = relationalIndex.Name;
				break;
			case 16:
				result = relationalIndex.NoAutomaticRecomputation;
				break;
			case 17:
				result = relationalIndex.PadIndex;
				break;
			case 18:
				result = relationalIndex.Parent;
				break;
			case 19:
				result = relationalIndex.PartitionScheme;
				break;
			}
			break;
		}
		case 93:
		{
			IScalarDataType scalarDataType = (IScalarDataType)obj;
			if (propertyInfo.Index == 0)
			{
				result = scalarDataType.Name;
			}
			break;
		}
		case 95:
		{
			IScalarParameter scalarParameter = (IScalarParameter)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = scalarParameter.DataType;
				break;
			case 1:
				result = scalarParameter.DefaultValue;
				break;
			case 2:
				result = scalarParameter.IsOutput;
				break;
			case 3:
				result = scalarParameter.Name;
				break;
			case 4:
				result = scalarParameter.Nullable;
				break;
			}
			break;
		}
		case 96:
		{
			IScalarValuedFunction scalarValuedFunction = (IScalarValuedFunction)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = scalarValuedFunction.BodyText;
				break;
			case 1:
				result = scalarValuedFunction.DataType;
				break;
			case 2:
				result = scalarValuedFunction.ExecutionContext;
				break;
			case 3:
				result = scalarValuedFunction.IsEncrypted;
				break;
			case 4:
				result = scalarValuedFunction.IsQuotedIdentifierOn;
				break;
			case 5:
				result = scalarValuedFunction.IsSchemaBound;
				break;
			case 6:
				result = scalarValuedFunction.IsSqlClr;
				break;
			case 7:
				result = scalarValuedFunction.IsSystemObject;
				break;
			case 8:
				result = scalarValuedFunction.Name;
				break;
			case 9:
				result = scalarValuedFunction.Nullable;
				break;
			case 10:
				result = scalarValuedFunction.Parameters;
				break;
			case 11:
				result = scalarValuedFunction.ReturnsNullOnNullInput;
				break;
			case 12:
				result = scalarValuedFunction.Schema;
				break;
			}
			break;
		}
		case 97:
		{
			IScalarVariable scalarVariable = (IScalarVariable)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = scalarVariable.DataType;
				break;
			case 1:
				result = scalarVariable.Name;
				break;
			case 2:
				result = scalarVariable.Nullable;
				break;
			}
			break;
		}
		case 98:
		{
			ISchema schema = (ISchema)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = schema.Database;
				break;
			case 1:
				result = schema.ExtendedStoredProcedures;
				break;
			case 2:
				result = schema.IsSystemObject;
				break;
			case 3:
				result = schema.Name;
				break;
			case 4:
				result = schema.Owner;
				break;
			case 5:
				result = schema.ScalarValuedFunctions;
				break;
			case 6:
				result = schema.StoredProcedures;
				break;
			case 7:
				result = schema.Synonyms;
				break;
			case 8:
				result = schema.TableValuedFunctions;
				break;
			case 9:
				result = schema.Tables;
				break;
			case 10:
				result = schema.UserDefinedAggregates;
				break;
			case 11:
				result = schema.UserDefinedClrTypes;
				break;
			case 12:
				result = schema.UserDefinedDataTypes;
				break;
			case 13:
				result = schema.UserDefinedTableTypes;
				break;
			case 14:
				result = schema.Views;
				break;
			}
			break;
		}
		case 100:
		{
			IServer server = (IServer)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = server.CollationInfo;
				break;
			case 1:
				result = server.Credentials;
				break;
			case 2:
				result = server.Databases;
				break;
			case 3:
				result = server.IsSystemObject;
				break;
			case 4:
				result = server.Logins;
				break;
			case 5:
				result = server.Name;
				break;
			case 6:
				result = server.Triggers;
				break;
			}
			break;
		}
		case 101:
		{
			IServerDdlTrigger serverDdlTrigger = (IServerDdlTrigger)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = serverDdlTrigger.BodyText;
				break;
			case 1:
				result = serverDdlTrigger.ExecutionContext;
				break;
			case 2:
				result = serverDdlTrigger.IsEnabled;
				break;
			case 3:
				result = serverDdlTrigger.IsEncrypted;
				break;
			case 4:
				result = serverDdlTrigger.IsQuotedIdentifierOn;
				break;
			case 5:
				result = serverDdlTrigger.IsSqlClr;
				break;
			case 6:
				result = serverDdlTrigger.IsSystemObject;
				break;
			case 7:
				result = serverDdlTrigger.Name;
				break;
			case 8:
				result = serverDdlTrigger.Server;
				break;
			case 9:
				result = serverDdlTrigger.ServerDdlEvents;
				break;
			}
			break;
		}
		case 103:
		{
			ISpatialIndex spatialIndex = (ISpatialIndex)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = spatialIndex.BoundingBoxXMax;
				break;
			case 1:
				result = spatialIndex.BoundingBoxXMin;
				break;
			case 2:
				result = spatialIndex.BoundingBoxYMax;
				break;
			case 3:
				result = spatialIndex.BoundingBoxYMin;
				break;
			case 4:
				result = spatialIndex.CellsPerObject;
				break;
			case 5:
				result = spatialIndex.DisallowPageLocks;
				break;
			case 6:
				result = spatialIndex.DisallowRowLocks;
				break;
			case 7:
				result = spatialIndex.FillFactor;
				break;
			case 8:
				result = spatialIndex.IgnoreDuplicateKeys;
				break;
			case 9:
				result = spatialIndex.IndexedColumn;
				break;
			case 10:
				result = spatialIndex.IsDisabled;
				break;
			case 11:
				result = spatialIndex.Level1Density;
				break;
			case 12:
				result = spatialIndex.Level2Density;
				break;
			case 13:
				result = spatialIndex.Level3Density;
				break;
			case 14:
				result = spatialIndex.Level4Density;
				break;
			case 15:
				result = spatialIndex.Name;
				break;
			case 16:
				result = spatialIndex.NoAutomaticRecomputation;
				break;
			case 17:
				result = spatialIndex.PadIndex;
				break;
			case 18:
				result = spatialIndex.Parent;
				break;
			}
			break;
		}
		case 104:
		{
			IStatistics statistics = (IStatistics)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = statistics.Columns;
				break;
			case 1:
				result = statistics.FilterDefinition;
				break;
			case 2:
				result = statistics.Name;
				break;
			case 3:
				result = statistics.NoAutomaticRecomputation;
				break;
			case 4:
				result = statistics.Parent;
				break;
			case 5:
				result = statistics.Type;
				break;
			}
			break;
		}
		case 105:
		{
			IStoredProcedure storedProcedure = (IStoredProcedure)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = storedProcedure.BodyText;
				break;
			case 1:
				result = storedProcedure.ExecutionContext;
				break;
			case 2:
				result = storedProcedure.ForReplication;
				break;
			case 3:
				result = storedProcedure.IsEncrypted;
				break;
			case 4:
				result = storedProcedure.IsQuotedIdentifierOn;
				break;
			case 5:
				result = storedProcedure.IsRecompiled;
				break;
			case 6:
				result = storedProcedure.IsSqlClr;
				break;
			case 7:
				result = storedProcedure.IsSystemObject;
				break;
			case 8:
				result = storedProcedure.Name;
				break;
			case 9:
				result = storedProcedure.Parameters;
				break;
			case 10:
				result = storedProcedure.Schema;
				break;
			case 11:
				result = storedProcedure.Startup;
				break;
			}
			break;
		}
		case 106:
		{
			ISynonym synonym = (ISynonym)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = synonym.BaseObjectName;
				break;
			case 1:
				result = synonym.BaseType;
				break;
			case 2:
				result = synonym.IsSystemObject;
				break;
			case 3:
				result = synonym.Name;
				break;
			case 4:
				result = synonym.Schema;
				break;
			}
			break;
		}
		case 107:
		{
			ISystemClrDataType systemClrDataType = (ISystemClrDataType)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = systemClrDataType.Length;
				break;
			case 1:
				result = systemClrDataType.NumericPrecision;
				break;
			case 2:
				result = systemClrDataType.NumericScale;
				break;
			case 3:
				result = systemClrDataType.TypeSpec;
				break;
			}
			break;
		}
		case 108:
		{
			ISystemDataType systemDataType = (ISystemDataType)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = systemDataType.Length;
				break;
			case 1:
				result = systemDataType.NumericPrecision;
				break;
			case 2:
				result = systemDataType.NumericScale;
				break;
			case 3:
				result = systemDataType.TypeSpec;
				break;
			}
			break;
		}
		case 109:
		{
			ITable table = (ITable)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = table.Columns;
				break;
			case 1:
				result = table.Constraints;
				break;
			case 2:
				result = table.Indexes;
				break;
			case 3:
				result = table.IsQuotedIdentifierOn;
				break;
			case 4:
				result = table.IsSystemObject;
				break;
			case 5:
				result = table.Name;
				break;
			case 6:
				result = table.Schema;
				break;
			case 7:
				result = table.Statistics;
				break;
			case 8:
				result = table.Triggers;
				break;
			}
			break;
		}
		case 110:
		{
			ITableDataType tableDataType = (ITableDataType)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = tableDataType.Columns;
				break;
			case 1:
				result = tableDataType.Constraints;
				break;
			case 2:
				result = tableDataType.Indexes;
				break;
			case 3:
				result = tableDataType.Name;
				break;
			case 4:
				result = tableDataType.Statistics;
				break;
			}
			break;
		}
		case 111:
		{
			ITableParameter tableParameter = (ITableParameter)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = tableParameter.DataType;
				break;
			case 1:
				result = tableParameter.Name;
				break;
			}
			break;
		}
		case 112:
		{
			ITableValuedFunction tableValuedFunction = (ITableValuedFunction)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = tableValuedFunction.BodyText;
				break;
			case 1:
				result = tableValuedFunction.Columns;
				break;
			case 2:
				result = tableValuedFunction.Constraints;
				break;
			case 3:
				result = tableValuedFunction.ExecutionContext;
				break;
			case 4:
				result = tableValuedFunction.Indexes;
				break;
			case 5:
				result = tableValuedFunction.IsEncrypted;
				break;
			case 6:
				result = tableValuedFunction.IsInline;
				break;
			case 7:
				result = tableValuedFunction.IsQuotedIdentifierOn;
				break;
			case 8:
				result = tableValuedFunction.IsSchemaBound;
				break;
			case 9:
				result = tableValuedFunction.IsSqlClr;
				break;
			case 10:
				result = tableValuedFunction.IsSystemObject;
				break;
			case 11:
				result = tableValuedFunction.Name;
				break;
			case 12:
				result = tableValuedFunction.Parameters;
				break;
			case 13:
				result = tableValuedFunction.Schema;
				break;
			case 14:
				result = tableValuedFunction.Statistics;
				break;
			case 15:
				result = tableValuedFunction.TableVariableName;
				break;
			}
			break;
		}
		case 113:
		{
			ITableVariable tableVariable = (ITableVariable)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = tableVariable.DataType;
				break;
			case 1:
				result = tableVariable.Name;
				break;
			}
			break;
		}
		case 118:
		{
			IUdtDataMember udtDataMember = (IUdtDataMember)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = udtDataMember.DataType;
				break;
			case 1:
				result = udtDataMember.IsStatic;
				break;
			case 2:
				result = udtDataMember.Name;
				break;
			case 3:
				result = udtDataMember.Nullable;
				break;
			case 4:
				result = udtDataMember.UserDefinedType;
				break;
			}
			break;
		}
		case 120:
		{
			IUdtMethod udtMethod = (IUdtMethod)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = udtMethod.DataType;
				break;
			case 1:
				result = udtMethod.IsStatic;
				break;
			case 2:
				result = udtMethod.Name;
				break;
			case 3:
				result = udtMethod.Nullable;
				break;
			case 4:
				result = udtMethod.Parameters;
				break;
			case 5:
				result = udtMethod.UserDefinedType;
				break;
			}
			break;
		}
		case 121:
		{
			IUniqueConstraint uniqueConstraint = (IUniqueConstraint)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = uniqueConstraint.AssociatedIndex;
				break;
			case 1:
				result = uniqueConstraint.IsSystemNamed;
				break;
			case 2:
				result = uniqueConstraint.Name;
				break;
			case 3:
				result = uniqueConstraint.Parent;
				break;
			}
			break;
		}
		case 123:
		{
			IUser user = (IUser)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = user.AsymmetricKey;
				break;
			case 1:
				result = user.Certificate;
				break;
			case 2:
				result = user.Database;
				break;
			case 3:
				result = user.DefaultSchema;
				break;
			case 4:
				result = user.IsSystemObject;
				break;
			case 5:
				result = user.Login;
				break;
			case 6:
				result = user.MemberOfRoles;
				break;
			case 7:
				result = user.Name;
				break;
			case 8:
				result = user.Password;
				break;
			case 9:
				result = user.Permissions;
				break;
			case 10:
				result = user.UserType;
				break;
			}
			break;
		}
		case 124:
		{
			IUserDefinedAggregate userDefinedAggregate = (IUserDefinedAggregate)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = userDefinedAggregate.DataType;
				break;
			case 1:
				result = userDefinedAggregate.IsSystemObject;
				break;
			case 2:
				result = userDefinedAggregate.Name;
				break;
			case 3:
				result = userDefinedAggregate.Nullable;
				break;
			case 4:
				result = userDefinedAggregate.Parameters;
				break;
			case 5:
				result = userDefinedAggregate.Schema;
				break;
			}
			break;
		}
		case 125:
		{
			IUserDefinedClrType userDefinedClrType = (IUserDefinedClrType)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = userDefinedClrType.AssemblyName;
				break;
			case 1:
				result = userDefinedClrType.ClassName;
				break;
			case 2:
				result = userDefinedClrType.DataMembers;
				break;
			case 3:
				result = userDefinedClrType.IsBinaryOrdered;
				break;
			case 4:
				result = userDefinedClrType.IsComVisible;
				break;
			case 5:
				result = userDefinedClrType.IsNullable;
				break;
			case 6:
				result = userDefinedClrType.IsSystemObject;
				break;
			case 7:
				result = userDefinedClrType.Methods;
				break;
			case 8:
				result = userDefinedClrType.Name;
				break;
			case 9:
				result = userDefinedClrType.Schema;
				break;
			}
			break;
		}
		case 126:
		{
			IUserDefinedDataType userDefinedDataType = (IUserDefinedDataType)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = userDefinedDataType.BaseSystemDataType;
				break;
			case 1:
				result = userDefinedDataType.IsSystemObject;
				break;
			case 2:
				result = userDefinedDataType.Name;
				break;
			case 3:
				result = userDefinedDataType.Nullable;
				break;
			case 4:
				result = userDefinedDataType.Schema;
				break;
			}
			break;
		}
		case 129:
		{
			IUserDefinedTableType userDefinedTableType = (IUserDefinedTableType)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = userDefinedTableType.Columns;
				break;
			case 1:
				result = userDefinedTableType.Constraints;
				break;
			case 2:
				result = userDefinedTableType.Indexes;
				break;
			case 3:
				result = userDefinedTableType.IsSystemObject;
				break;
			case 4:
				result = userDefinedTableType.Name;
				break;
			case 5:
				result = userDefinedTableType.Schema;
				break;
			case 6:
				result = userDefinedTableType.Statistics;
				break;
			}
			break;
		}
		case 131:
		{
			IView view = (IView)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = view.Columns;
				break;
			case 1:
				result = view.Constraints;
				break;
			case 2:
				result = view.HasCheckOption;
				break;
			case 3:
				result = view.HasColumnSpecification;
				break;
			case 4:
				result = view.Indexes;
				break;
			case 5:
				result = view.IsEncrypted;
				break;
			case 6:
				result = view.IsQuotedIdentifierOn;
				break;
			case 7:
				result = view.IsSchemaBound;
				break;
			case 8:
				result = view.IsSystemObject;
				break;
			case 9:
				result = view.Name;
				break;
			case 10:
				result = view.QueryText;
				break;
			case 11:
				result = view.ReturnsViewMetadata;
				break;
			case 12:
				result = view.Schema;
				break;
			case 13:
				result = view.Statistics;
				break;
			case 14:
				result = view.Triggers;
				break;
			}
			break;
		}
		case 133:
		{
			IXmlDataType xmlDataType = (IXmlDataType)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = xmlDataType.IsXmlNode;
				break;
			case 1:
				result = xmlDataType.Length;
				break;
			case 2:
				result = xmlDataType.NumericPrecision;
				break;
			case 3:
				result = xmlDataType.NumericScale;
				break;
			case 4:
				result = xmlDataType.TypeSpec;
				break;
			}
			break;
		}
		case 136:
		{
			IXmlIndex xmlIndex = (IXmlIndex)obj;
			switch (propertyInfo.Index)
			{
			case 0:
				result = xmlIndex.DisallowPageLocks;
				break;
			case 1:
				result = xmlIndex.DisallowRowLocks;
				break;
			case 2:
				result = xmlIndex.FillFactor;
				break;
			case 3:
				result = xmlIndex.IgnoreDuplicateKeys;
				break;
			case 4:
				result = xmlIndex.IsDisabled;
				break;
			case 5:
				result = xmlIndex.Name;
				break;
			case 6:
				result = xmlIndex.PadIndex;
				break;
			case 7:
				result = xmlIndex.Parent;
				break;
			}
			break;
		}
		}
		return result;
	}

	public static MetadataTypeInfo TypeInfo(this IMetadataObject obj)
	{
		return obj?.Accept(typeInfoVisitor);
	}

	public static IMetadataObject GetParentObject(IMetadataObject obj)
	{
		MetadataTypeInfo metadataTypeInfo = obj.TypeInfo();
		MetadataPropertyInfo parentProperty = metadataTypeInfo.ParentProperty;
		return (IMetadataObject)GetPropertyValue(obj, metadataTypeInfo, parentProperty);
	}

	public static bool IsSystemObject(IMetadataObject obj)
	{
		IDatabaseObject databaseObject = obj as IDatabaseObject;
		bool num = databaseObject != null;
		bool flag = obj is ISchema && obj.Name.Equals("dbo", StringComparison.OrdinalIgnoreCase);
		bool flag2 = obj is IDatabaseRole && obj.Name.Equals("public", StringComparison.OrdinalIgnoreCase);
		bool flag3 = obj is IUser && obj.Name.Equals("guest", StringComparison.OrdinalIgnoreCase);
		if (num && databaseObject.IsSystemObject && !flag && !flag2)
		{
			return !flag3;
		}
		return false;
	}

	public static bool IsValueTypeObject(IMetadataObject obj)
	{
		if (obj.TypeInfo().MetadataType != MetadataType.Value)
		{
			return IsUnnamedObject(obj);
		}
		return true;
	}

	public static bool IsUnnamedObject(IMetadataObject obj)
	{
		if (obj is IConstraint constraint)
		{
			return IsUnnamedConstraint(constraint);
		}
		if (obj is IDefaultConstraint constraint2)
		{
			return IsUnnamedDefaultConstraint(constraint2);
		}
		if (obj is IIndex index)
		{
			return IsUnnamedIndex(index);
		}
		return obj is IDatabasePermission;
	}

	private static bool IsUnnamedIndex(IIndex index)
	{
		return IsUnnamedConstraintParent(index.Parent);
	}

	private static bool IsUnnamedConstraint(IConstraint constraint)
	{
		return IsUnnamedConstraintParent(constraint.Parent);
	}

	private static bool IsUnnamedDefaultConstraint(IDefaultConstraint constraint)
	{
		return IsUnnamedConstraintParent(constraint.Parent.Parent);
	}

	private static bool IsUnnamedConstraintParent(ITabular tabular)
	{
		switch (tabular.TabularType)
		{
		case TabularType.TableDataType:
		case TabularType.TableValuedFunction:
			return true;
		case TabularType.Table:
		case TabularType.View:
			return false;
		default:
			return false;
		}
	}
}
