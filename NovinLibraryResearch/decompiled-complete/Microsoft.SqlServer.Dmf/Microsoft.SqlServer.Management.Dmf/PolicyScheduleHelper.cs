using System;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;

namespace Microsoft.SqlServer.Management.Dmf;

internal sealed class PolicyScheduleHelper
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "PolicyScheduleHelper");

	public static void FixPolicySchedule(Policy policy)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("FixPolicySchedule");
		methodTraceContext.TraceParameters(policy);
		traceContext.DebugAssert(policy.AutomatedPolicyEvaluationMode == AutomatedPolicyEvaluationMode.CheckOnSchedule);
		if (string.IsNullOrEmpty(policy.Schedule))
		{
			policy.AutomatedPolicyEvaluationMode = AutomatedPolicyEvaluationMode.None;
			policy.Enabled = false;
			return;
		}
		Microsoft.SqlServer.Management.Smo.Server server = new Microsoft.SqlServer.Management.Smo.Server(policy.Parent.SqlStoreConnection.ServerConnection);
		JobSchedule jobSchedule = server.JobServer.SharedSchedules[policy.ScheduleUid];
		bool flag = jobSchedule != null;
		if (flag && JobScheduleMatchesPolicy(jobSchedule, policy))
		{
			return;
		}
		foreach (JobSchedule sharedSchedule in server.JobServer.SharedSchedules)
		{
			if (JobScheduleMatchesPolicy(sharedSchedule, policy))
			{
				policy.ScheduleUid = sharedSchedule.ScheduleUid;
				return;
			}
		}
		JobSchedule jobSchedule3 = new JobSchedule(server.JobServer, policy.Schedule);
		if (!flag && policy.ScheduleUid != Guid.Empty)
		{
			jobSchedule3.ScheduleUid = policy.ScheduleUid;
		}
		if (!policy.Properties["ActiveEndDate"].IsNull)
		{
			jobSchedule3.ActiveEndDate = policy.ActiveEndDate;
		}
		if (!policy.Properties["ActiveEndTimeOfDay"].IsNull)
		{
			jobSchedule3.ActiveEndTimeOfDay = new TimeSpan(policy.ActiveEndTimeOfDay);
		}
		if (!policy.Properties["ActiveStartDate"].IsNull)
		{
			jobSchedule3.ActiveStartDate = policy.ActiveStartDate;
		}
		if (!policy.Properties["ActiveStartTimeOfDay"].IsNull)
		{
			jobSchedule3.ActiveStartTimeOfDay = new TimeSpan(policy.ActiveStartTimeOfDay);
		}
		if (!policy.Properties["FrequencyInterval"].IsNull)
		{
			jobSchedule3.FrequencyInterval = policy.FrequencyInterval;
		}
		if (!policy.Properties["FrequencyRecurrenceFactor"].IsNull)
		{
			jobSchedule3.FrequencyRecurrenceFactor = policy.FrequencyRecurrenceFactor;
		}
		if (!policy.Properties["FrequencyRelativeIntervals"].IsNull)
		{
			jobSchedule3.FrequencyRelativeIntervals = policy.FrequencyRelativeIntervals;
		}
		if (!policy.Properties["FrequencySubDayInterval"].IsNull)
		{
			jobSchedule3.FrequencySubDayInterval = policy.FrequencySubDayInterval;
		}
		if (!policy.Properties["FrequencySubDayTypes"].IsNull)
		{
			jobSchedule3.FrequencySubDayTypes = policy.FrequencySubDayTypes;
		}
		if (!policy.Properties["FrequencyTypes"].IsNull)
		{
			jobSchedule3.FrequencyTypes = policy.FrequencyTypes;
		}
		jobSchedule3.Create();
		jobSchedule3.Refresh();
		policy.ScheduleUid = jobSchedule3.ScheduleUid;
	}

	private static bool JobScheduleMatchesPolicy(JobSchedule js, Policy policy)
	{
		if (policy.Schedule == js.Name && policy.ActiveEndDate == js.ActiveEndDate && policy.ActiveEndTimeOfDay == js.ActiveEndTimeOfDay.Ticks && policy.ActiveStartDate == js.ActiveStartDate && policy.ActiveStartTimeOfDay == js.ActiveStartTimeOfDay.Ticks && policy.FrequencyInterval == js.FrequencyInterval && policy.FrequencyRecurrenceFactor == js.FrequencyRecurrenceFactor && policy.FrequencyRelativeIntervals == js.FrequencyRelativeIntervals && policy.FrequencySubDayInterval == js.FrequencySubDayInterval && policy.FrequencySubDayTypes == js.FrequencySubDayTypes)
		{
			return policy.FrequencyTypes == js.FrequencyTypes;
		}
		return false;
	}
}
