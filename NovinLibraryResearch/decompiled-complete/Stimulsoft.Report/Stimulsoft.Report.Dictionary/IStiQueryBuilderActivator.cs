using System;
using System.Data;

namespace Stimulsoft.Report.Dictionary;

public interface IStiQueryBuilderActivator
{
	string RunQueryBuilder(IDbConnection connection, string sql, EventHandler handler, object owner, object syntaxProvider, object metadataProvider);
}
