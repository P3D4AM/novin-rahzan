using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.Dmf;

internal class LogPolicyEvents
{
	private static TraceContext traceContext;

	private static readonly SfcTsqlProcFormatter scriptLogEvaluationStart;

	private static readonly SfcTsqlProcFormatter scriptLogEvaluationEnd;

	private static readonly SfcTsqlProcFormatter scriptLogEvaluationDetail;

	private long historyId;

	private ServerConnection serverConnection;

	internal long HistoryId => historyId;

	static LogPolicyEvents()
	{
		traceContext = TraceContext.GetTraceContext("DMF", "LogPolicyEvents");
		scriptLogEvaluationStart = new SfcTsqlProcFormatter();
		scriptLogEvaluationEnd = new SfcTsqlProcFormatter();
		scriptLogEvaluationDetail = new SfcTsqlProcFormatter();
		scriptLogEvaluationStart.Procedure = "msdb.dbo.sp_syspolicy_log_policy_execution_start";
		scriptLogEvaluationStart.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("history_id", required: true, output: true));
		scriptLogEvaluationStart.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("policy_id", required: true));
		scriptLogEvaluationStart.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("is_full_run", required: true));
		scriptLogEvaluationEnd.Procedure = "msdb.dbo.sp_syspolicy_log_policy_execution_end";
		scriptLogEvaluationEnd.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("history_id", required: true));
		scriptLogEvaluationEnd.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("result", required: true));
		scriptLogEvaluationEnd.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("exception_message", required: true));
		scriptLogEvaluationEnd.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("exception", required: true));
		scriptLogEvaluationDetail.Procedure = "msdb.dbo.sp_syspolicy_log_policy_execution_detail";
		scriptLogEvaluationDetail.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("history_id", required: true));
		scriptLogEvaluationDetail.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("target_query_expression", required: true));
		scriptLogEvaluationDetail.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("target_query_expression_with_id", required: true));
		scriptLogEvaluationDetail.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("result", required: true));
		scriptLogEvaluationDetail.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("result_detail", required: true));
		scriptLogEvaluationDetail.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("exception_message", required: true));
		scriptLogEvaluationDetail.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("exception", required: true));
	}

	internal LogPolicyEvents(ServerConnection serverConnection)
	{
		this.serverConnection = serverConnection;
	}

	internal void LogPolicyEvaluationStart(int policyId, bool isFullRun)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("LogPolicyEvaluationStart");
		methodTraceContext.TraceParameters(policyId, isFullRun);
		if (serverConnection != null)
		{
			historyId = 0L;
			List<SfcTsqlProcFormatter.RuntimeArg> list = new List<SfcTsqlProcFormatter.RuntimeArg>();
			list.Add(new SfcTsqlProcFormatter.RuntimeArg(historyId.GetType(), historyId));
			list.Add(new SfcTsqlProcFormatter.RuntimeArg(policyId.GetType(), policyId));
			list.Add(new SfcTsqlProcFormatter.RuntimeArg(isFullRun.GetType(), isFullRun));
			string text = scriptLogEvaluationStart.GenerateScript(null, list);
			methodTraceContext.TraceVerbose("Script generated: " + text);
			historyId = (long)serverConnection.ExecuteScalar(text);
		}
	}

	internal void LogPolicyEvaluationDetail(Condition policyCondition, ConditionEvaluationEventArgs eventArgs)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("LogPolicyEvaluationDetail");
		methodTraceContext.TraceParameters(policyCondition, eventArgs);
		if (serverConnection != null && (eventArgs.Target is SqlSmoObject || eventArgs.Target is SfcInstance))
		{
			string text = string.Empty;
			if (policyCondition != null)
			{
				text = ExpressionNode.SerializeNodeWithResult(policyCondition.ExpressionNode);
			}
			List<SfcTsqlProcFormatter.RuntimeArg> list = new List<SfcTsqlProcFormatter.RuntimeArg>();
			list.Add(new SfcTsqlProcFormatter.RuntimeArg(historyId.GetType(), historyId));
			list.Add(new SfcTsqlProcFormatter.RuntimeArg(eventArgs.TargetPsPath.GetType(), eventArgs.TargetPsPath));
			list.Add(new SfcTsqlProcFormatter.RuntimeArg(eventArgs.TargetUrnOnlyId.GetType(), eventArgs.TargetUrnOnlyId));
			list.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(int), eventArgs.EvaluationResult ? 1 : 0));
			list.Add(new SfcTsqlProcFormatter.RuntimeArg(text.GetType(), text));
			list.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(string), (eventArgs.EvaluationException != null) ? eventArgs.EvaluationException.Message : string.Empty));
			list.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(string), (eventArgs.EvaluationException != null) ? eventArgs.EvaluationException.ToString() : string.Empty));
			string text2 = scriptLogEvaluationDetail.GenerateScript(null, list);
			methodTraceContext.TraceVerbose("Script generated: " + text2);
			serverConnection.ExecuteNonQuery(text2, ExecutionTypes.NoCommands);
		}
	}

	internal void LogPolicyEvaluationEnd(bool result, Exception exception)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("LogPolicyEvaluationEnd");
		methodTraceContext.TraceParameters(result, exception);
		if (serverConnection != null)
		{
			List<SfcTsqlProcFormatter.RuntimeArg> list = new List<SfcTsqlProcFormatter.RuntimeArg>();
			list.Add(new SfcTsqlProcFormatter.RuntimeArg(historyId.GetType(), historyId));
			list.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(int), result ? 1 : 0));
			list.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(string), (exception != null) ? exception.Message : string.Empty));
			list.Add(new SfcTsqlProcFormatter.RuntimeArg(typeof(string), (exception != null) ? exception.ToString() : string.Empty));
			string text = scriptLogEvaluationEnd.GenerateScript(null, list);
			methodTraceContext.TraceVerbose("Script generated: " + text);
			serverConnection.ExecuteNonQuery(text, ExecutionTypes.NoCommands);
		}
	}
}
