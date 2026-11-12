using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface ISchemaFactory
{
	IMutableSchema Create(IDatabase database, string name);

	IMutableSchema Create(ISchema otherSchema);

	ISchema CreateEmptySchema(IDatabase database, string name, bool isSystemObject);
}
