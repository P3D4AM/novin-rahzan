using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

internal interface IParserTestSink
{
	void StatementEvent(SqlStatement sqlStatement, StatementEventType eventType);
}
