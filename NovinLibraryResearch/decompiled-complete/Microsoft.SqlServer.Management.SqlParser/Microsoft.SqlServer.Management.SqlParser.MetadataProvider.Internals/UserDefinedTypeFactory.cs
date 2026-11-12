using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class UserDefinedTypeFactory : ObjectFactoryBase<UserDefinedTypeFactory>, IUserDefinedTypeFactory
{
	public IMutableUserDefinedClrType CreateClrType(ISchema schema, string name)
	{
		if (schema == null)
		{
			throw new ArgumentNullException("schema");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new UserDefinedClrType(schema, name);
	}

	public IMutableUserDefinedDataType CreateDataType(ISchema schema, string name)
	{
		if (schema == null)
		{
			throw new ArgumentNullException("schema");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new UserDefinedDataType(schema, name);
	}

	public IMutableUserDefinedTableType CreateTableType(ISchema schema, string name)
	{
		if (schema == null)
		{
			throw new ArgumentNullException("schema");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new UserDefinedTableType(schema, name);
	}
}
