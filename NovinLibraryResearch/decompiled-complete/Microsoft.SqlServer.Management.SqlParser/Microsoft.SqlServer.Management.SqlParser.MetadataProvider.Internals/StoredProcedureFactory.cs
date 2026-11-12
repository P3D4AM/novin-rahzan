using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal sealed class StoredProcedureFactory : ObjectFactoryBase<StoredProcedureFactory>, IStoredProcedureFactory
{
	public IMutableStoredProcedure Create(ISchema schema, string name)
	{
		if (schema == null)
		{
			throw new ArgumentNullException("schema");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new StoredProcedure(schema, name);
	}
}
