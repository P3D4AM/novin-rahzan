namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IDatabaseOwnedObjectVisitor<T>
{
	T Visit(IApplicationRole applicationRole);

	T Visit(IAsymmetricKey assymetricKey);

	T Visit(ICertificate certificate);

	T Visit(IDatabaseDdlTrigger databaseDdlTrigger);

	T Visit(IDatabaseRole databaseRole);

	T Visit(IFileGroup fileGroup);

	T Visit(IPartitionScheme partitionScheme);

	T Visit(ISchema schema);

	T Visit(IUser user);
}
