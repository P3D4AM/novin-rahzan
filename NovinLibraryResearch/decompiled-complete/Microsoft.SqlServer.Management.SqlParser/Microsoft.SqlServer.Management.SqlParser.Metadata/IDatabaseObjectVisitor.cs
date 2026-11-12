namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IDatabaseObjectVisitor<T> : IServerOwnedObjectVisitor<T>, IDatabaseOwnedObjectVisitor<T>, ISchemaOwnedObjectVisitor<T>
{
	T Visit(IServer server);
}
