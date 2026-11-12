using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IStoredProcedureFactory
{
	IMutableStoredProcedure Create(ISchema schema, string name);
}
