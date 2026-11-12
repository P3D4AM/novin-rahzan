using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

public interface IBinder
{
	IServer Bind(IEnumerable<ParseResult> parseResults, string contextDatabaseName, BindMode bindMode);
}
