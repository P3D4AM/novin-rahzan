using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableDatabase : IDatabase, IServerOwnedObject, IDatabaseObject, IMetadataObject, IMutableServerOwnedObject, IMutableDatabaseObject, IMutableMetadataObject
{
	new IMutableMetadataCollection<IApplicationRole> ApplicationRoles { get; }

	new IMutableMetadataCollection<IAsymmetricKey> AsymmetricKeys { get; }

	new IMutableMetadataCollection<ICertificate> Certificates { get; }

	new DatabaseCompatibilityLevel CompatibilityLevel { get; set; }

	new string DefaultSchemaName { get; set; }

	new IUser Owner { get; set; }

	new IMutableMetadataCollection<IDatabaseRole> Roles { get; }

	new IMutableMetadataCollection<ISchema> Schemas { get; }

	new IMutableMetadataCollection<IDatabaseDdlTrigger> Triggers { get; }

	new IMutableMetadataCollection<IUser> Users { get; }
}
