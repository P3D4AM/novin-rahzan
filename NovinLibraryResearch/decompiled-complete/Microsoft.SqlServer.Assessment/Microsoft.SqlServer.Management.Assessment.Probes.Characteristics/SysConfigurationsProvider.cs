using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Tools;

namespace Microsoft.SqlServer.Management.Assessment.Probes.Characteristics;

internal class SysConfigurationsProvider : ICharacteristicProvider
{
	private const string ConfigurationName = "name";

	private static readonly Regex StaticPattern = new Regex("^sys.configuration(\\s+(?<name>.+))?$".ToString(CultureInfo.InvariantCulture), RegexOptions.ExplicitCapture);

	public Regex Pattern => StaticPattern;

	public int Priority => 0;

	public Task UpdateCharacteristics(RunSpace runSpace, string key, Dictionary<string, string> args, HashSet<string> specification)
	{
		if (!runSpace.KnownCharacteristics.TryGetValue(key, out var value))
		{
			value = new HashSet<string>();
			runSpace.KnownCharacteristics.Add(key, value);
		}
		if (args.TryGetValue("name", out var value2))
		{
			return UpdateNamed(runSpace, value2, value);
		}
		return UpdatePresence(runSpace, value);
	}

	private static async Task UpdatePresence(RunSpace runSpace, HashSet<string> confirmedValues)
	{
		ISqlTool tool = runSpace.GetTool<ISqlTool>();
		if (tool == null)
		{
			return;
		}
		confirmedValues.Clear();
		using DbCommand command = tool.CreateCommand();
		command.CommandText = "SELECT [name], [value_in_use] FROM sys.configurations";
		using DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(continueOnCapturedContext: false);
		while (true)
		{
			bool hasRows = reader.HasRows;
			bool flag = hasRows;
			if (flag)
			{
				flag = await reader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			if (flag)
			{
				string text = reader[0] as string;
				object valueInUse = reader[1];
				if (!string.IsNullOrEmpty(text) && IsPresent(valueInUse))
				{
					confirmedValues.Add(text);
				}
				continue;
			}
			break;
		}
	}

	private static bool IsPresent(object valueInUse)
	{
		if (valueInUse is int num)
		{
			return num != 0;
		}
		return valueInUse != null;
	}

	private static async Task UpdateNamed(RunSpace runSpace, string name, HashSet<string> confirmedValues)
	{
		ISqlTool tool = runSpace.GetTool<ISqlTool>();
		if (tool == null)
		{
			return;
		}
		confirmedValues.Clear();
		using DbCommand command = tool.CreateCommand();
		command.CommandText = "SELECT [value_in_use] FROM sys.configurations WHERE [name] = @name";
		DbParameter dbParameter = command.CreateParameter();
		dbParameter.ParameterName = "@name";
		dbParameter.Value = name;
		command.Parameters.Add(dbParameter);
		object obj = await command.ExecuteScalarAsync().ConfigureAwait(continueOnCapturedContext: false);
		if (obj != null)
		{
			confirmedValues.Add(obj.Normalize<string>());
		}
	}

	public CharacteristicCollection GetDependencies(RunSpace runSpace, Dictionary<string, string> arguments, HashSet<string> specification)
	{
		return CharacteristicCollection.Empty;
	}

	public string GetDisplayName(string key, Dictionary<string, string> arguments, HashSet<string> specification)
	{
		if (arguments.TryGetValue("name", out var value))
		{
			return value + " in sys.configurations set to " + specification.First();
		}
		return string.Join(", ", specification) + " in sys.configurations enabled";
	}
}
