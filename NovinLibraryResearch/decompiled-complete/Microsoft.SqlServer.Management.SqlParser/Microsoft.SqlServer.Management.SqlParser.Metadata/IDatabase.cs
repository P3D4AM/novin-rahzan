using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
public interface IDatabase : IServerOwnedObject, IDatabaseObject, IMetadataObject
{
	[ReadWrite]
	[Child]
	IMetadataCollection<IApplicationRole> ApplicationRoles { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<IAsymmetricKey> AsymmetricKeys { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<ICertificate> Certificates { get; }

	[ReadOnly]
	CollationInfo CollationInfo { get; }

	[ReadWrite]
	DatabaseCompatibilityLevel CompatibilityLevel { get; }

	[ReadWrite]
	string DefaultSchemaName { get; }

	[ReadWrite]
	[Reference]
	IUser Owner { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<IDatabaseRole> Roles { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<ISchema> Schemas { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<IDatabaseDdlTrigger> Triggers { get; }

	[ReadWrite]
	[Child]
	IMetadataCollection<IUser> Users { get; }
}
