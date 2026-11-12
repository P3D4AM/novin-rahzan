using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IUserDefinedFunctionFactory
{
	IMutableScalarValuedFunction CreateScalarValuedFunction(ISchema schema, string name);

	IMutableTableValuedFunction CreateTableValuedFunction(ISchema schema, string name);
}
