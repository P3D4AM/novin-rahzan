using System.Data;
using System.Data.Common;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

internal interface ISqlTool : ITool
{
	IDbConnection Connection { get; }

	DbCommand CreateCommand();

	DbCommand CreateCommand(string commandText);

	void ChangeDatabase(string databaseName);
}
