using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using System.Xml;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Server;

namespace Microsoft.SqlServer.Management.Dmf;

internal sealed class PolicyEvaluationHelper
{
	private static TraceContext traceContext;

	private static StringBuilder raiserrorBuilder;

	private static SqlCommand exceptionLoggingCommand;

	private const int RaiserrorBufferSize = 1024;

	public static int EvaluateAutomatedPolicy(string policy, SqlXml eventData, ref long historyId)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("EvaluateAutomatedPolicy");
		methodTraceContext.TraceParameters(policy, eventData, historyId);
		string command = string.Empty;
		if (!SqlContext.IsAvailable)
		{
			throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.OnlyInSqlClr));
		}
		bool flag = false;
		using (SqlConnection sqlConnection = new SqlConnection("context connection=true"))
		{
			if (eventData == null)
			{
				LogExceptionMessage(ExceptionTemplatesSR.NullArgInPolicyEvaluation, sqlConnection);
				methodTraceContext.TraceParameterOut("returnVal", 1);
				return 1;
			}
			if (policy == null)
			{
				LogExceptionMessage(ExceptionTemplatesSR.NullArgInPolicyEvaluation, sqlConnection);
				methodTraceContext.TraceParameterOut("returnVal", 1);
				return 1;
			}
			try
			{
				sqlConnection.Open();
				Version ver = new Version(sqlConnection.ServerVersion);
				string text = new SqlCommand("SELECT DB_NAME()", sqlConnection).ExecuteScalar() as string;
				traceContext.DebugAssert(text != null);
				if (text != "msdb")
				{
					throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.OnlyInMsdb));
				}
				if ((int)new SqlCommand("DECLARE @ret int; EXECUTE @ret = [msdb].[dbo].[sp_syspolicy_check_membership] 'PolicyAdministratorRole'; SELECT @ret", sqlConnection).ExecuteScalar() != 0)
				{
					throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.OnlyRoleMember("PolicyAdministratorRole")));
				}
				PolicyStore policyStore = new PolicyStore(new SqlStoreConnection(sqlConnection));
				Policy policy2 = policyStore.Policies[policy];
				if (policy2 == null)
				{
					throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.MissingPolicy(policy)));
				}
				if (policy2.HasScript)
				{
					SqlContext.Pipe.Send(ExceptionTemplatesSR.NoScriptInSqlClr(policy));
					methodTraceContext.TraceParameterOut("returnVal", 0);
					return 0;
				}
				StringBuilder stringBuilder = new StringBuilder();
				SfcQueryExpression sfcQueryExpression = null;
				if (!eventData.IsNull)
				{
					sfcQueryExpression = GetTargetQueryExpression(eventData, policy2, stringBuilder, out command);
					SfcQueryExpression targetQueryExpression = ObjectSet.AdjustForSystem(ver, policy2, sfcQueryExpression);
					flag = policy2.Evaluate(AdHocPolicyEvaluationMode.Check, targetQueryExpression, ref historyId, policyStore.SqlStoreConnection);
				}
				else
				{
					flag = policy2.Evaluate(AdHocPolicyEvaluationMode.Check, ref historyId, policyStore.SqlStoreConnection);
				}
				if (policy2.AutomatedPolicyEvaluationMode == AutomatedPolicyEvaluationMode.Enforce && !flag && sfcQueryExpression != null)
				{
					if (command.Length > 150)
					{
						command = command.TrimStart(new char[1] { ' ' }).Substring(0, 147) + "...";
					}
					Condition condition = policyStore.Conditions[policy2.Condition];
					SqlContext.Pipe.Send(ExceptionTemplatesSR.RollBack(policy, stringBuilder.ToString(), condition.ExpressionNode.ToString(), policy2.Description, policy2.HelpText, policy2.HelpLink, command));
				}
			}
			catch (Exception ex)
			{
				methodTraceContext.TraceCatch(ex);
				LogExceptionMessage(ex.ToString(), sqlConnection);
			}
		}
		return Convert.ToInt32(!flag);
	}

	static PolicyEvaluationHelper()
	{
		traceContext = TraceContext.GetTraceContext("DMF", "PolicyEvaluationHelper");
		raiserrorBuilder = new StringBuilder(1024);
		exceptionLoggingCommand = new SqlCommand();
	}

	internal static void LogExceptionMessage(string message, SqlConnection connection)
	{
		traceContext.TraceMethodEnter("LogExceptionMessage");
		traceContext.TraceParameters(message, connection);
		raiserrorBuilder.Length = 0;
		raiserrorBuilder.AppendFormat("RAISERROR({0}, 1,1) WITH LOG", SfcTsqlProcFormatter.MakeSqlString(message));
		exceptionLoggingCommand.CommandText = raiserrorBuilder.ToString();
		exceptionLoggingCommand.Connection = connection;
		exceptionLoggingCommand.ExecuteNonQuery();
		traceContext.TraceMethodExit("LogExceptionMessage");
	}

	private static SfcQueryExpression GetTargetQueryExpression(SqlXml eventData, Policy p, StringBuilder targetPsPathBuilder, out string command)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetTargetQueryExpression");
		methodTraceContext.TraceParameters(eventData, p, targetPsPathBuilder);
		command = string.Empty;
		XmlNode xmlNode = new XmlDocument().ReadNode(eventData.CreateReader());
		traceContext.DebugAssert(xmlNode.Name == "EVENT_INSTANCE");
		SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
		foreach (XmlNode childNode in xmlNode.ChildNodes)
		{
			XmlNode firstChild = childNode.FirstChild;
			if (firstChild != null && firstChild.Value != null)
			{
				sortedDictionary.Add(childNode.Name, childNode.FirstChild.Value);
			}
			else if (firstChild != null && firstChild.NextSibling != null && firstChild.NextSibling.FirstChild != null && firstChild.NextSibling.FirstChild.Value != null)
			{
				sortedDictionary.Add(firstChild.NextSibling.Name, firstChild.NextSibling.FirstChild.Value);
			}
			else
			{
				sortedDictionary.Add(childNode.Name, string.Empty);
			}
		}
		if (sortedDictionary.ContainsKey("CommandText"))
		{
			command = sortedDictionary["CommandText"];
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("Server[@Name='{0}']", Urn.EscapeString(p.Parent.SqlStoreConnection.ServerConnection.TrueName));
		traceContext.DebugAssert(sortedDictionary.ContainsKey("EventType"));
		switch (sortedDictionary["EventType"])
		{
		case "CREATE_DATABASE":
		case "ALTER_DATABASE":
			if (sortedDictionary.ContainsKey("DatabaseName"))
			{
				stringBuilder.AppendFormat("/Database[@Name='{0}']", Urn.EscapeString(sortedDictionary["DatabaseName"]));
			}
			break;
		case "CREATE_RESOURCE_POOL":
		case "ALTER_RESOURCE_POOL":
			stringBuilder.AppendFormat("/ResourceGovernor");
			GetExpressionGeneric(sortedDictionary, stringBuilder, eventData, p);
			break;
		case "CREATE_WORKLOAD_GROUP":
		case "ALTER_WORKLOAD_GROUP":
		{
			stringBuilder.Length = 0;
			WorkloadGroup workloadGroup = WorkloadGroup.GetWorkloadGroup(new Microsoft.SqlServer.Management.Smo.Server(p.Parent.SqlStoreConnection.ServerConnection), sortedDictionary["ObjectName"]);
			stringBuilder.Append(workloadGroup.Urn);
			break;
		}
		default:
			GetExpressionGeneric(sortedDictionary, stringBuilder, eventData, p);
			break;
		}
		targetPsPathBuilder.Length = 0;
		targetPsPathBuilder.Append(SfcSqlPathUtilities.ConvertUrnToPath(stringBuilder.ToString()));
		return new SfcQueryExpression(stringBuilder.ToString());
	}

	private static void GetExpressionGeneric(SortedDictionary<string, string> eventValues, StringBuilder targetUrn, SqlXml eventData, Policy p)
	{
		traceContext.TraceMethodEnter("GetExpressionGeneric");
		traceContext.TraceParameters(eventValues, targetUrn, eventData, p);
		if (eventValues.ContainsKey("DatabaseName"))
		{
			targetUrn.AppendFormat("/Database[@Name='{0}']", Urn.EscapeString(eventValues["DatabaseName"]));
		}
		_ = string.Empty;
		string eventValue = GetEventValue(eventValues, eventData, "EventType");
		string text = string.Empty;
		if (eventValue == "RENAME")
		{
			text = GetEventValue(eventValues, eventData, "NewObjectName");
		}
		string text2 = eventValues["ObjectType"];
		if (text2 != null && !char.IsDigit(text2[0]))
		{
			string text3 = string.Empty;
			if (text2 != "SCHEMA" && eventValues.ContainsKey("SchemaName"))
			{
				text3 = eventValues["SchemaName"];
			}
			if (eventValues.ContainsKey("TargetObjectType") && eventValues.ContainsKey("TargetObjectName"))
			{
				string typeCorrectCase = GetTypeCorrectCase(eventValues["TargetObjectType"]);
				string text4 = eventValues["TargetObjectName"];
				if (typeCorrectCase.Length > 0 && text4.Length > 0 && text3.Length > 0)
				{
					targetUrn.AppendFormat("/{0}[@Name='{1}' and @Schema='{2}']", typeCorrectCase, Urn.EscapeString(text4), Urn.EscapeString(text3));
					text3 = string.Empty;
				}
				else if (typeCorrectCase.Length > 0 && text4.Length > 0)
				{
					targetUrn.AppendFormat("/{0}[@Name='{1}']", typeCorrectCase, Urn.EscapeString(text4));
				}
			}
			string typeCorrectCase2 = GetTypeCorrectCase(text2.ToString());
			string text5 = GetEventValue(eventValues, eventData, "ObjectName");
			if (text5.Length > 0 && text.Length > 0)
			{
				text5 = text;
			}
			if (text3 != string.Empty)
			{
				targetUrn.AppendFormat("/{0}[@Name='{1}' and @Schema='{2}']", typeCorrectCase2, Urn.EscapeString(text5), Urn.EscapeString(text3));
			}
			else
			{
				targetUrn.AppendFormat("/{0}[@Name='{1}']", typeCorrectCase2, Urn.EscapeString(text5));
			}
		}
		else
		{
			targetUrn.Length = 0;
			targetUrn.AppendFormat("Server[@Name='{0}']", Urn.EscapeString(p.Parent.SqlStoreConnection.ServerInstance));
		}
		traceContext.TraceMethodExit("GetExpressionGeneric");
	}

	private static string GetEventValue(SortedDictionary<string, string> eventValues, SqlXml eventData, string elementName)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetEventValue");
		methodTraceContext.TraceParameters(eventValues, eventData, elementName);
		string empty = string.Empty;
		if (!eventValues.ContainsKey(elementName) || (empty = eventValues[elementName]) == null)
		{
			throw traceContext.TraceThrow(new BadEventDataException(ExceptionTemplatesSR.BadEventData(elementName, eventData.ToString())));
		}
		methodTraceContext.TraceParameterOut("returnVal", empty);
		return empty;
	}

	private static string GetTypeCorrectCase(string type)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetTypeCorrectCase");
		methodTraceContext.TraceParameters(type);
		if (type.Length == 0)
		{
			methodTraceContext.TraceParameterOut("returnVal", type);
			return type;
		}
		string text = type.ToLowerInvariant();
		switch (text)
		{
		case "procedure":
			return "StoredProcedure";
		case "function":
			return "UserDefinedFunction";
		case "type":
			return "UserDefinedType";
		case "sql user":
		case "certificate user":
		case "asymmetric key user":
		case "windows user":
		case "group user":
			return "User";
		case "server role":
			return "Role";
		default:
		{
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = text.Split(new char[1] { ' ' });
			foreach (string text2 in array)
			{
				stringBuilder.Append(text2[0].ToString().ToUpperInvariant()[0]);
				stringBuilder.Append(text2.Substring(1));
			}
			return stringBuilder.ToString();
		}
		}
	}
}
