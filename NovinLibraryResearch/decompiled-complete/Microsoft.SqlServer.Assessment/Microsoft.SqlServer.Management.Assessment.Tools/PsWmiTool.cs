using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

internal class PsWmiTool : DependentTool<IPowerShellTool>, IWmiTool, ITool
{
	private const string CommandTemplate = "Get-WmiObject -computername 'localhost' -query {0}";

	private const string AddMemberTemplate = "Add-Member -MemberType ScriptProperty -Name '{0}' -Value {{$this.{1}()}} -PassThru";

	public PsWmiTool(RunSpace runSpace)
		: base(runSpace)
	{
	}

	protected override Task<bool> IsChannelAvailable(RunSpace runSpace)
	{
		return Task.FromResult(result: true);
	}

	public Task<IDataView> Execute(string query, IReadOnlyDictionary<string, string> methods, IReadOnlyDictionary<string, object> parameters)
	{
		return base.BaseTool.Execute(BuildCommand(query, methods), parameters);
	}

	private static string BuildCommand(string query, IReadOnlyDictionary<string, string> methods)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "Get-WmiObject -computername 'localhost' -query {0}", Escape(query));
		if (methods != null && methods.Count > 0)
		{
			foreach (KeyValuePair<string, string> method in methods)
			{
				stringBuilder.Append(" | ");
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "Add-Member -MemberType ScriptProperty -Name '{0}' -Value {{$this.{1}()}} -PassThru", method.Key, method.Value);
			}
		}
		return stringBuilder.ToString();
	}

	private static string Escape(string query)
	{
		return Utils.EscapePowerShellDoubleQuoteString(query);
	}

	public static IWmiTool Create(RunSpace runSpace)
	{
		return new PsWmiTool(runSpace);
	}
}
