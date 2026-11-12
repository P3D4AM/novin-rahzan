using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo.Agent;

[TypeConverter(typeof(AgentSubSystemTypeConverter))]
public enum AgentSubSystem
{
	[LocDisplayName("TransactSql")]
	[TsqlSyntaxString("TSQL")]
	TransactSql = 1,
	[LocDisplayName("ActiveScripting")]
	[TsqlSyntaxString("ActiveScripting")]
	ActiveScripting,
	[LocDisplayName("CmdExec")]
	[TsqlSyntaxString("CmdExec")]
	CmdExec,
	[LocDisplayName("ReplSnapshot")]
	[TsqlSyntaxString("Snapshot")]
	Snapshot,
	[LocDisplayName("ReplLogReader")]
	[TsqlSyntaxString("LogReader")]
	LogReader,
	[LocDisplayName("ReplDistribution")]
	[TsqlSyntaxString("Distribution")]
	Distribution,
	[LocDisplayName("ReplMerge")]
	[TsqlSyntaxString("Merge")]
	Merge,
	[LocDisplayName("ReplQueueReader")]
	[TsqlSyntaxString("QueueReader")]
	QueueReader,
	[LocDisplayName("AnalysisQuery")]
	[TsqlSyntaxString("ANALYSISQUERY")]
	AnalysisQuery,
	[LocDisplayName("AnalysisCommand")]
	[TsqlSyntaxString("ANALYSISCOMMAND")]
	AnalysisCommand,
	[LocDisplayName("SSIS")]
	[TsqlSyntaxString("SSIS")]
	Ssis,
	[LocDisplayName("PowerShell")]
	[TsqlSyntaxString("PowerShell")]
	PowerShell
}
