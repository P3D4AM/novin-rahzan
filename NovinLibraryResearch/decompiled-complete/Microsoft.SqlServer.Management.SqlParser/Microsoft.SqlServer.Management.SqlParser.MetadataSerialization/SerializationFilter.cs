using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class SerializationFilter : IMetadataObjectVisitor<bool>, IDatabaseObjectVisitor<bool>, IServerOwnedObjectVisitor<bool>, IDatabaseOwnedObjectVisitor<bool>, ISchemaOwnedObjectVisitor<bool>, ISerializationFilter
{
	private Predicate<IDatabase> databaseFilter;

	private Predicate<ILogin> loginFilter;

	private Predicate<ISchema> schemaFilter;

	private Predicate<IUser> userFilter;

	public Predicate<IDatabase> DatabaseFilter
	{
		get
		{
			return databaseFilter;
		}
		set
		{
			databaseFilter = value;
		}
	}

	public Predicate<ILogin> LoginFilter
	{
		get
		{
			return loginFilter;
		}
		set
		{
			loginFilter = value;
		}
	}

	public Predicate<ISchema> SchemaFilter
	{
		get
		{
			return schemaFilter;
		}
		set
		{
			schemaFilter = value;
		}
	}

	public Predicate<IUser> UserFilter
	{
		get
		{
			return userFilter;
		}
		set
		{
			userFilter = value;
		}
	}

	public bool IsIncluded(IMetadataObject metadataObject)
	{
		if (metadataObject == null)
		{
			throw new ArgumentNullException("metadataObject");
		}
		return metadataObject.Accept(this);
	}

	bool IMetadataObjectVisitor<bool>.Visit(IBuiltInFunction builtInFunction)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(ICheckConstraint checkConstraint)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IClrDataType clrDataType)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(ICollation collation)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IColumn column)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(ICursorDataType cursorDataType)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(ICursorParameter cursorParameter)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(ICursorVariable cursorVariable)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IDatabasePermission databasePermission)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IDatePart datePart)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IDefaultConstraint defaultConstraint)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IDmlTrigger dmlTrigger)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IExecutionContext executionContext)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IForeignKeyColumn foreignKeyColumn)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IForeignKeyConstraint foreignKeyConstraint)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IPrimaryKeyConstraint primaryKeyConstraint)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IIndexedColumn indexedColumn)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IRelationalIndex relationalIndex)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IScalarExpression scalarExpression)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IScalarDataType scalarDataType)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IScalarParameter scalarParameter)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IScalarVariable scalarVariable)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(ISpatialIndex spatialIndex)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IStatistics statistics)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(ISystemClrDataType systemClrDataType)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(ISystemDataType systemDataType)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(ITableDataType tableDataType)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(ITableParameter tableParameter)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(ITableVariable tableVariable)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IUdtDataMember udtDataMember)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IUdtMethod udtMethod)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IUniqueConstraint uniqueConstraint)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IVirtualTable virtualTable)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IVoidDataType voidDataType)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IXmlDataTypeMethod xmlDataTypeMethod)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IXmlDataTypeTableMethod xmlDataTypeMethod)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IXmlDataType xmlDataType)
	{
		return true;
	}

	bool IMetadataObjectVisitor<bool>.Visit(IXmlIndex xmlIndex)
	{
		return true;
	}

	bool IDatabaseObjectVisitor<bool>.Visit(IServer server)
	{
		return true;
	}

	bool IServerOwnedObjectVisitor<bool>.Visit(ICredential credential)
	{
		return true;
	}

	bool IServerOwnedObjectVisitor<bool>.Visit(IDatabase database)
	{
		if (databaseFilter == null)
		{
			return true;
		}
		return databaseFilter(database);
	}

	bool IServerOwnedObjectVisitor<bool>.Visit(ILogin login)
	{
		if (loginFilter == null)
		{
			return true;
		}
		return loginFilter(login);
	}

	bool IServerOwnedObjectVisitor<bool>.Visit(IServerDdlTrigger serverDdlTrigger)
	{
		return true;
	}

	bool IDatabaseOwnedObjectVisitor<bool>.Visit(IApplicationRole applicationRole)
	{
		return true;
	}

	bool IDatabaseOwnedObjectVisitor<bool>.Visit(IAsymmetricKey assymetricKey)
	{
		return true;
	}

	bool IDatabaseOwnedObjectVisitor<bool>.Visit(ICertificate certificate)
	{
		return true;
	}

	bool IDatabaseOwnedObjectVisitor<bool>.Visit(IDatabaseDdlTrigger databaseDdlTrigger)
	{
		return true;
	}

	bool IDatabaseOwnedObjectVisitor<bool>.Visit(IDatabaseRole databaseRole)
	{
		return true;
	}

	bool IDatabaseOwnedObjectVisitor<bool>.Visit(IFileGroup fileGroup)
	{
		return true;
	}

	bool IDatabaseOwnedObjectVisitor<bool>.Visit(IPartitionScheme partitionScheme)
	{
		return true;
	}

	bool IDatabaseOwnedObjectVisitor<bool>.Visit(ISchema schema)
	{
		if (schemaFilter == null)
		{
			return true;
		}
		return schemaFilter(schema);
	}

	bool IDatabaseOwnedObjectVisitor<bool>.Visit(IUser user)
	{
		if (userFilter == null)
		{
			return true;
		}
		return userFilter(user);
	}

	bool ISchemaOwnedObjectVisitor<bool>.Visit(IExtendedStoredProcedure extendedStoredProcedure)
	{
		return true;
	}

	bool ISchemaOwnedObjectVisitor<bool>.Visit(IScalarValuedFunction scalarValuedFunction)
	{
		return true;
	}

	bool ISchemaOwnedObjectVisitor<bool>.Visit(IStoredProcedure storedProcedure)
	{
		return true;
	}

	bool ISchemaOwnedObjectVisitor<bool>.Visit(ISynonym synonym)
	{
		return true;
	}

	bool ISchemaOwnedObjectVisitor<bool>.Visit(ITable table)
	{
		return true;
	}

	bool ISchemaOwnedObjectVisitor<bool>.Visit(ITableValuedFunction tableValuedFunction)
	{
		return true;
	}

	bool ISchemaOwnedObjectVisitor<bool>.Visit(IUserDefinedAggregate userDefinedAggregate)
	{
		return true;
	}

	bool ISchemaOwnedObjectVisitor<bool>.Visit(IUserDefinedClrType userDefinedClrType)
	{
		return true;
	}

	bool ISchemaOwnedObjectVisitor<bool>.Visit(IUserDefinedDataType userDefinedDataType)
	{
		return true;
	}

	bool ISchemaOwnedObjectVisitor<bool>.Visit(IUserDefinedTableType userDefinedTableType)
	{
		return true;
	}

	bool ISchemaOwnedObjectVisitor<bool>.Visit(IView view)
	{
		return true;
	}
}
