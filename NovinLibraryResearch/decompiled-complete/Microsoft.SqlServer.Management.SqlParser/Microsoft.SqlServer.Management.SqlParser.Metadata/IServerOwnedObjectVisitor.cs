namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IServerOwnedObjectVisitor<T>
{
	T Visit(ICredential credential);

	T Visit(IDatabase database);

	T Visit(ILogin login);

	T Visit(IServerDdlTrigger serverDdlTrigger);
}
