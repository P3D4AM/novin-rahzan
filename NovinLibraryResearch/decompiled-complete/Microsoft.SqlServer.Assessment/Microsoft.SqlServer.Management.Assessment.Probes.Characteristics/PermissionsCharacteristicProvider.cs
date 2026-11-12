using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Tools;

namespace Microsoft.SqlServer.Management.Assessment.Probes.Characteristics;

internal class PermissionsCharacteristicProvider : ICharacteristicProvider
{
	private const string SecurableClassArgumentName = "class";

	private const string SecurableNameArgumentName = "name";

	private static readonly Regex StaticPattern = new Regex("^(?<class>.+) permission( on (?<name>.+))?$".ToString(CultureInfo.InvariantCulture), RegexOptions.ExplicitCapture | RegexOptions.Compiled);

	public Regex Pattern { get; } = StaticPattern;


	public int Priority => 0;

	public async Task UpdateCharacteristics(RunSpace runSpace, string key, Dictionary<string, string> args, HashSet<string> specification)
	{
		if (!runSpace.KnownCharacteristics.TryGetValue(key, out var confirmedPermissions))
		{
			confirmedPermissions = new HashSet<string>();
			runSpace.KnownCharacteristics.Add(key, confirmedPermissions);
		}
		ISqlTool tool = runSpace.GetTool<ISqlTool>();
		if (tool == null)
		{
			return;
		}
		string text = args["class"].ToUpper(CultureInfo.InvariantCulture);
		using DbCommand command = tool.CreateCommand();
		command.CommandText = "SELECT [permission_name] FROM [sys].[fn_my_permissions](@securableName, @securableClass)";
		DbParameter dbParameter = command.CreateParameter();
		dbParameter.ParameterName = "@securableClass";
		dbParameter.Value = text;
		DbParameter dbParameter2 = command.CreateParameter();
		dbParameter2.ParameterName = "@securableName";
		if (args["name"] != null)
		{
			dbParameter2.Value = args["name"];
		}
		else if (string.CompareOrdinal("DATABASE", text) == 0 && runSpace.Target.Type == SqlObjectType.Database)
		{
			dbParameter2.Value = runSpace.Target.Name;
		}
		else
		{
			dbParameter2.Value = DBNull.Value;
		}
		command.Parameters.Add(dbParameter);
		command.Parameters.Add(dbParameter2);
		using DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(continueOnCapturedContext: false);
		while (await reader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false))
		{
			confirmedPermissions.Add(reader[0].ToString());
		}
	}

	public CharacteristicCollection GetDependencies(RunSpace runSpace, Dictionary<string, string> arguments, HashSet<string> specification)
	{
		return CharacteristicCollection.Empty;
	}

	public string GetDisplayName(string key, Dictionary<string, string> arguments, HashSet<string> specification)
	{
		StringBuilder stringBuilder = new StringBuilder(string.Join(", ", specification));
		if (arguments.TryGetValue("class", out var value) && string.Compare(value, "OBJECT", StringComparison.OrdinalIgnoreCase) != 0)
		{
			stringBuilder.Append(' ').Append(value);
		}
		stringBuilder.Append(" permission");
		if (arguments.TryGetValue("name", out var value2))
		{
			stringBuilder.Append(" on ").Append(value2);
		}
		return stringBuilder.ToString();
	}
}
