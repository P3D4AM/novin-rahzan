using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Probes;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

internal class XpCmdShellTool : DependentTool<ISqlTool>, ICmdShellTool, ITool
{
	private static readonly CharacteristicCollection StaticRequirements = new CharacteristicCollection
	{
		{
			"object permission on xp_cmdshell",
			new HashSet<string> { "EXECUTE" }
		},
		{
			"sys.configuration",
			new HashSet<string> { "xp_cmdshell" }
		}
	};

	private const string XpCmdShellScript = "EXEC master..xp_cmdshell @cmd";

	public XpCmdShellTool(RunSpace runSpace)
		: base(runSpace)
	{
	}

	public async Task<List<string>> ExecuteLines(string script)
	{
		if (string.IsNullOrWhiteSpace(script))
		{
			return new List<string>();
		}
		List<string> lines = new List<string>();
		using (DbCommand command = base.BaseTool.CreateCommand("EXEC master..xp_cmdshell @cmd"))
		{
			DbParameter dbParameter = command.CreateParameter();
			dbParameter.ParameterName = "@cmd";
			dbParameter.Value = script;
			command.Parameters.Add(dbParameter);
			using DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(continueOnCapturedContext: false);
			while (await reader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false))
			{
				if (!reader.IsDBNull(0))
				{
					lines.Add(reader[0].ToString());
				}
			}
		}
		return lines;
	}

	public async Task<LinesReader> ExecuteTextReader(string script)
	{
		return new LinesReader(await ExecuteLines(script).ConfigureAwait(continueOnCapturedContext: false));
	}

	public override ICharacteristicCollection GetDependencies(RunSpace runSpace)
	{
		return CharacteristicCollection.Union(base.GetDependencies(runSpace), StaticRequirements);
	}

	public static ICmdShellTool Create(RunSpace runSpace)
	{
		return new XpCmdShellTool(runSpace);
	}

	protected override Task<bool> IsChannelAvailable(RunSpace runSpace)
	{
		return Task.FromResult(runSpace.Meets(StaticRequirements));
	}
}
