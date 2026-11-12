using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class UserDefinedFunctionFactory : ObjectFactoryBase<UserDefinedFunctionFactory>, IUserDefinedFunctionFactory
{
	public IMutableScalarValuedFunction CreateScalarValuedFunction(ISchema schema, string name)
	{
		if (schema == null)
		{
			throw new ArgumentNullException("schema");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new ScalarValuedFunction(schema, name);
	}

	public IMutableTableValuedFunction CreateTableValuedFunction(ISchema schema, string name)
	{
		if (schema == null)
		{
			throw new ArgumentNullException("schema");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new TableValuedFunction(schema, name);
	}
}
