using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal class NameCollationVisitor : IDatabaseObjectVisitor<CollationInfo>, IServerOwnedObjectVisitor<CollationInfo>, IDatabaseOwnedObjectVisitor<CollationInfo>, ISchemaOwnedObjectVisitor<CollationInfo>
{
	private static CollationInfo VisitSchemaOwnedObject(ISchemaOwnedObject scOwnedObject)
	{
		return scOwnedObject.Schema.Database.CollationInfo;
	}

	public static CollationInfo VisitDatabaseOwnedObject(IDatabaseOwnedObject dbOwnedObject)
	{
		return dbOwnedObject.Database.CollationInfo;
	}

	public static CollationInfo VisitServerOwnedObject(IServerOwnedObject svrOwnedObject)
	{
		return svrOwnedObject.Server.CollationInfo;
	}

	public CollationInfo Visit(IServer server)
	{
		return server.CollationInfo;
	}

	public CollationInfo Visit(ICredential credential)
	{
		return VisitServerOwnedObject(credential);
	}

	public CollationInfo Visit(IDatabase database)
	{
		return VisitServerOwnedObject(database);
	}

	public CollationInfo Visit(ILogin login)
	{
		return VisitServerOwnedObject(login);
	}

	public CollationInfo Visit(IServerDdlTrigger serverDdlTrigger)
	{
		return VisitServerOwnedObject(serverDdlTrigger);
	}

	public CollationInfo Visit(IApplicationRole applicationRole)
	{
		return VisitDatabaseOwnedObject(applicationRole);
	}

	public CollationInfo Visit(IAsymmetricKey assymetricKey)
	{
		return VisitDatabaseOwnedObject(assymetricKey);
	}

	public CollationInfo Visit(ICertificate certificate)
	{
		return VisitDatabaseOwnedObject(certificate);
	}

	public CollationInfo Visit(IDatabaseDdlTrigger databaseDdlTrigger)
	{
		return VisitDatabaseOwnedObject(databaseDdlTrigger);
	}

	public CollationInfo Visit(IDatabaseRole databaseRole)
	{
		return VisitDatabaseOwnedObject(databaseRole);
	}

	public CollationInfo Visit(IFileGroup fileGroup)
	{
		return VisitDatabaseOwnedObject(fileGroup);
	}

	public CollationInfo Visit(IPartitionScheme partitionScheme)
	{
		return VisitDatabaseOwnedObject(partitionScheme);
	}

	public CollationInfo Visit(ISchema schema)
	{
		return VisitDatabaseOwnedObject(schema);
	}

	public CollationInfo Visit(IUser user)
	{
		return VisitDatabaseOwnedObject(user);
	}

	public CollationInfo Visit(IExtendedStoredProcedure extendedStoredProcedure)
	{
		return VisitSchemaOwnedObject(extendedStoredProcedure);
	}

	public CollationInfo Visit(IScalarValuedFunction scalarValuedFunction)
	{
		return VisitSchemaOwnedObject(scalarValuedFunction);
	}

	public CollationInfo Visit(IStoredProcedure storedProcedure)
	{
		return VisitSchemaOwnedObject(storedProcedure);
	}

	public CollationInfo Visit(ISynonym synonym)
	{
		return VisitSchemaOwnedObject(synonym);
	}

	public CollationInfo Visit(ITable table)
	{
		return VisitSchemaOwnedObject(table);
	}

	public CollationInfo Visit(ITableValuedFunction tableValuedFunction)
	{
		return VisitSchemaOwnedObject(tableValuedFunction);
	}

	public CollationInfo Visit(IUserDefinedAggregate userDefinedAggregate)
	{
		return VisitSchemaOwnedObject(userDefinedAggregate);
	}

	public CollationInfo Visit(IUserDefinedClrType userDefinedClrType)
	{
		return VisitSchemaOwnedObject(userDefinedClrType);
	}

	public CollationInfo Visit(IUserDefinedDataType userDefinedDataType)
	{
		return VisitSchemaOwnedObject(userDefinedDataType);
	}

	public CollationInfo Visit(IUserDefinedTableType userDefinedTableType)
	{
		return VisitSchemaOwnedObject(userDefinedTableType);
	}

	public CollationInfo Visit(IView view)
	{
		return VisitSchemaOwnedObject(view);
	}
}
