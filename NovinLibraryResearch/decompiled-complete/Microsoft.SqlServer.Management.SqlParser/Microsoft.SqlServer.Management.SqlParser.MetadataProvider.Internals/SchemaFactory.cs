using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class SchemaFactory : ObjectFactoryBase<SchemaFactory>, ISchemaFactory
{
	public IMutableSchema Create(IDatabase database, string name)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new Schema(database, name);
	}

	public IMutableSchema Create(ISchema otherSchema)
	{
		if (otherSchema == null)
		{
			throw new ArgumentNullException("otherSchema");
		}
		return new Schema(otherSchema);
	}

	public ISchema CreateEmptySchema(IDatabase database, string name, bool isSystemObject)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new EmptySchema(database, name, isSystemObject);
	}
}
