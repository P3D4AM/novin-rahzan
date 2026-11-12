using System.Collections.Generic;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Tools;

namespace Microsoft.SqlServer.Management.Assessment.Probes.Characteristics;

internal class RoleCharacteristicProvider : ICharacteristicProvider
{
	public const string Key = "role";

	private static readonly Regex StaticPattern = new Regex("^role$", RegexOptions.Compiled);

	public Regex Pattern { get; } = StaticPattern;


	public int Priority => 0;

	public async Task UpdateCharacteristics(RunSpace runSpace, string key, Dictionary<string, string> args, HashSet<string> specification)
	{
		if (!runSpace.KnownCharacteristics.TryGetValue(key, out var confirmedRoles))
		{
			confirmedRoles = new HashSet<string>();
			runSpace.KnownCharacteristics.Add(key, confirmedRoles);
		}
		ISqlTool tool = runSpace.GetTool<ISqlTool>();
		if (tool == null)
		{
			return;
		}
		using DbCommand command = tool.CreateCommand();
		command.CommandText = "SELECT ISNULL(IS_SRVROLEMEMBER('@roleName'), 0)";
		DbParameter parameter = command.CreateParameter();
		parameter.ParameterName = "@roleName";
		command.Parameters.Add(parameter);
		using HashSet<string>.Enumerator enumerator = specification.GetEnumerator();
		while (enumerator.MoveNext())
		{
			string role = (string)(parameter.Value = enumerator.Current);
			if ((bool)(await command.ExecuteScalarAsync().ConfigureAwait(continueOnCapturedContext: false)))
			{
				confirmedRoles.Add(role);
			}
		}
	}

	public CharacteristicCollection GetDependencies(RunSpace runSpace, Dictionary<string, string> arguments, HashSet<string> specification)
	{
		return CharacteristicCollection.Empty;
	}

	public string GetDisplayName(string key, Dictionary<string, string> arguments, HashSet<string> specification)
	{
		return string.Join(", ", specification) + " role" + ((specification.Count == 1) ? string.Empty : "s");
	}
}
