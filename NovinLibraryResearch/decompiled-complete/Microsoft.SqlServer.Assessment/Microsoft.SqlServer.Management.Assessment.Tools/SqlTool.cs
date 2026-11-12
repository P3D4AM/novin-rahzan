using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Probes;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

internal class SqlTool : ISqlTool, ITool
{
	public DbConnection Connection { get; }

	IDbConnection ISqlTool.Connection => Connection;

	public SqlTool(DbConnection connection)
	{
		Connection = connection;
	}

	public ICharacteristicCollection GetDependencies(RunSpace runSpace)
	{
		return CharacteristicCollection.Empty;
	}

	public Task<bool> IsAvailable(RunSpace runSpace)
	{
		return Task.FromResult(Connection != null && Connection.State != ConnectionState.Broken);
	}

	public DbCommand CreateCommand()
	{
		return Connection.CreateCommand();
	}

	public DbCommand CreateCommand(string commandText)
	{
		DbCommand dbCommand = Connection.CreateCommand();
		dbCommand.CommandText = commandText;
		return dbCommand;
	}

	public void ChangeDatabase(string databaseName)
	{
		Connection.ChangeDatabase(databaseName);
	}

	public static ISqlTool Create(RunSpace runSpace)
	{
		return new SqlTool(runSpace.Connection);
	}
}
