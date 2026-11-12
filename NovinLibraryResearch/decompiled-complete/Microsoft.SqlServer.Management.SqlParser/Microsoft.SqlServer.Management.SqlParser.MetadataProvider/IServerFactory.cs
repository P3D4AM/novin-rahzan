using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IServerFactory
{
	IMutableServer Create(string name, CollationInfo collationInfo);

	IMutableServer Create(IServer otherServer);
}
