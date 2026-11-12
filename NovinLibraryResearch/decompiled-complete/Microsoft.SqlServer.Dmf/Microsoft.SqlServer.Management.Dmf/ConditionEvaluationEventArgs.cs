using System;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.Dmf;

internal class ConditionEvaluationEventArgs : EventArgs
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "ConditionEvaluationEventArgs");

	private string targetUrn = string.Empty;

	private string targetUrnOnlyId = string.Empty;

	private string targetPsPath = string.Empty;

	private string serverName;

	private bool evaluationResult;

	private bool isConfigurable;

	private string configMessage = string.Empty;

	private object target;

	private Exception exception;

	internal string TargetUrn => targetUrn;

	internal string TargetUrnOnlyId => targetUrnOnlyId;

	internal string TargetPsPath => targetPsPath;

	internal string ServerName => serverName;

	internal bool EvaluationResult => evaluationResult;

	internal bool IsConfigurable => isConfigurable;

	internal string ConfigurationErrorMessage => configMessage;

	internal object Target => target;

	internal Exception EvaluationException => exception;

	internal ConditionEvaluationEventArgs(FacetEvaluationContext facetEvaluationContext, string facetType, object target, bool evaluationResult, bool isConfigurable, string configMessage, Exception exception)
	{
		traceContext.TraceMethodEnter("ConditionEvaluationEventArgs");
		traceContext.TraceParameters(facetEvaluationContext, facetType, target, evaluationResult, isConfigurable, configMessage, exception);
		serverName = string.Empty;
		if (target is IDmfObjectInfo)
		{
			targetPsPath = ((IDmfObjectInfo)target).ObjectPath;
			serverName = ((IDmfObjectInfo)target).RootPath;
		}
		else if (facetEvaluationContext != null && facetEvaluationContext.Target is IDmfObjectInfo)
		{
			targetPsPath = ((IDmfObjectInfo)facetEvaluationContext.Target).ObjectPath;
			serverName = ((IDmfObjectInfo)facetEvaluationContext.Target).RootPath;
		}
		else
		{
			targetUrn = SfcUtility.GetUrn(target);
			if (target is SqlSmoObject)
			{
				serverName = ((SqlSmoObject)target).GetServerObject().ConnectionContext.ServerInstance;
				if (!((SqlSmoObject)target).IsDesignMode)
				{
					targetUrnOnlyId = ((SqlSmoObject)target).UrnOnlyId.ToString();
				}
			}
			else if (target is SfcInstance)
			{
				if (((SfcInstance)target).GetDomain().GetConnection() != null)
				{
					serverName = ((SfcInstance)target).GetDomain().GetConnection().ServerInstance;
				}
				else
				{
					serverName = ((SfcInstance)target).GetDomain().DomainInstanceName;
				}
			}
			if (!string.IsNullOrEmpty(SfcRegistration.GetRegisteredDomainForType(target.GetType().FullName, throwOnUnregisteredDomain: false)))
			{
				targetPsPath = SfcSqlPathUtilities.ConvertUrnToPath(targetUrn);
			}
		}
		this.evaluationResult = evaluationResult;
		this.isConfigurable = isConfigurable;
		if (!string.IsNullOrEmpty(configMessage))
		{
			this.configMessage = configMessage;
		}
		this.target = target;
		this.exception = exception;
		traceContext.TraceMethodExit("ConditionEvaluationEventArgs");
	}
}
