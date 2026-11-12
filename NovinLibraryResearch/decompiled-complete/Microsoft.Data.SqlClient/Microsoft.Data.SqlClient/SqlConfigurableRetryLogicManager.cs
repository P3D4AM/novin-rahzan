using System;
using System.Diagnostics;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlConfigurableRetryLogicManager
{
	private const string TypeName = "SqlConfigurableRetryLogicManager";

	private static readonly Lazy<SqlConfigurableRetryLogicLoader> s_loader = new Lazy<SqlConfigurableRetryLogicLoader>(delegate
	{
		ISqlConfigurableRetryConnectionSection sqlConfigurableRetryConnectionSection = null;
		ISqlConfigurableRetryCommandSection sqlConfigurableRetryCommandSection = null;
		sqlConfigurableRetryConnectionSection = AppConfigManager.FetchConfigurationSection<SqlConfigurableRetryConnectionSection>("SqlConfigurableRetryLogicConnection");
		sqlConfigurableRetryCommandSection = AppConfigManager.FetchConfigurationSection<SqlConfigurableRetryCommandSection>("SqlConfigurableRetryLogicCommand");
		return new SqlConfigurableRetryLogicLoader(sqlConfigurableRetryConnectionSection, sqlConfigurableRetryCommandSection);
	});

	internal static SqlRetryLogicBaseProvider ConnectionProvider
	{
		get
		{
			try
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> Requested the {1} value.", "SqlConfigurableRetryLogicManager", "ConnectionProvider");
				return s_loader.Value.ConnectionProvider;
			}
			catch (Exception args)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> An exception occurred in access to the instance of the class, and the default non-retriable provider will apply: {2}", "SqlConfigurableRetryLogicManager", "ConnectionProvider", args);
				if (SqlClientEventSource.Log.IsAdvancedTraceOn())
				{
					StackTrace args2 = new StackTrace();
					SqlClientEventSource.Log.AdvancedTraceEvent("<sc.{0}.{1}|ADV|INFO> An exception occurred in access to the instance of the class, and the default non-retriable provider will apply: {2}", "SqlConfigurableRetryLogicManager", "ConnectionProvider", args2);
				}
				return SqlConfigurableRetryFactory.CreateNoneRetryProvider();
			}
		}
	}

	internal static SqlRetryLogicBaseProvider CommandProvider
	{
		get
		{
			try
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> Requested the {1} value.", "SqlConfigurableRetryLogicManager", "CommandProvider");
				return s_loader.Value.CommandProvider;
			}
			catch (Exception args)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|INFO> An exception occurred in access to the instance of the class, and the default non-retriable provider will apply: {2}", "SqlConfigurableRetryLogicManager", "CommandProvider", args);
				if (SqlClientEventSource.Log.IsAdvancedTraceOn())
				{
					StackTrace args2 = new StackTrace();
					SqlClientEventSource.Log.AdvancedTraceEvent("<sc.{0}.{1}|ADV|INFO> An exception occurred in access to the instance of the class, and the default non-retriable provider will apply: {2}", "SqlConfigurableRetryLogicManager", "CommandProvider", args2);
				}
				return SqlConfigurableRetryFactory.CreateNoneRetryProvider();
			}
		}
	}

	private SqlConfigurableRetryLogicManager()
	{
	}
}
