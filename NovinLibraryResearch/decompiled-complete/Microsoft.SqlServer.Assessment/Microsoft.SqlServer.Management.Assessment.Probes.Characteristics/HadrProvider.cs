using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Tools;

namespace Microsoft.SqlServer.Management.Assessment.Probes.Characteristics;

internal class HadrProvider : IFeatureProvider
{
	public const string KeyValue = "HADR";

	public string Key => "HADR";

	public async Task<bool> IsPresent(RunSpace runSpace)
	{
		ISqlTool tool = runSpace.GetTool<ISqlTool>();
		using DbCommand command = tool.CreateCommand();
		command.CommandText = "SELECT SERVERPROPERTY ('IsHadrEnabled')";
		return (await command.ExecuteScalarAsync().ConfigureAwait(continueOnCapturedContext: false)).Equals(1);
	}

	public CharacteristicCollection GetDependencies()
	{
		return CharacteristicCollection.Empty;
	}
}
