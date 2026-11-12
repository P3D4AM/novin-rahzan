using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Data.SqlClient;

public sealed class SqlConfigurableRetryFactory
{
	private static readonly HashSet<int> s_defaultTransientErrors = new HashSet<int>
	{
		1204, 1205, 1222, 49918, 49919, 49920, 4060, 4221, 40143, 40613,
		40501, 40540, 40197, 10929, 10928, 10060, 10054, 10053, 997, 233
	};

	public static SqlRetryLogicBaseProvider CreateExponentialRetryProvider(SqlRetryLogicOption retryLogicOption)
	{
		return InternalCreateRetryProvider(retryLogicOption, (retryLogicOption != null) ? new SqlExponentialIntervalEnumerator(retryLogicOption.DeltaTime, retryLogicOption.MaxTimeInterval, retryLogicOption.MinTimeInterval) : null);
	}

	public static SqlRetryLogicBaseProvider CreateIncrementalRetryProvider(SqlRetryLogicOption retryLogicOption)
	{
		return InternalCreateRetryProvider(retryLogicOption, (retryLogicOption != null) ? new SqlIncrementalIntervalEnumerator(retryLogicOption.DeltaTime, retryLogicOption.MaxTimeInterval, retryLogicOption.MinTimeInterval) : null);
	}

	public static SqlRetryLogicBaseProvider CreateFixedRetryProvider(SqlRetryLogicOption retryLogicOption)
	{
		return InternalCreateRetryProvider(retryLogicOption, (retryLogicOption != null) ? new SqlFixedIntervalEnumerator(retryLogicOption.DeltaTime, retryLogicOption.MaxTimeInterval, retryLogicOption.MinTimeInterval) : null);
	}

	private static SqlRetryLogicBaseProvider InternalCreateRetryProvider(SqlRetryLogicOption retryLogicOption, SqlRetryIntervalBaseEnumerator enumerator)
	{
		if (retryLogicOption == null)
		{
			throw new ArgumentNullException("retryLogicOption");
		}
		SqlRetryLogic retryLogic = new SqlRetryLogic(retryLogicOption.NumberOfTries, enumerator, (Exception e) => TransientErrorsCondition(e, retryLogicOption.TransientErrors ?? s_defaultTransientErrors), retryLogicOption.AuthorizedSqlCondition);
		return new SqlRetryLogicProvider(retryLogic);
	}

	public static SqlRetryLogicBaseProvider CreateNoneRetryProvider()
	{
		SqlRetryLogic retryLogic = new SqlRetryLogic(new SqlNoneIntervalEnumerator(), (Exception _) => false);
		return new SqlRetryLogicProvider(retryLogic);
	}

	private static bool TransientErrorsCondition(Exception e, IEnumerable<int> retriableConditions)
	{
		bool result = false;
		if (retriableConditions != null && e is SqlException { _doNotReconnect: false } ex)
		{
			foreach (SqlError error in ex.Errors)
			{
				if (retriableConditions.Contains(error.Number))
				{
					SqlClientEventSource.Log.TryTraceEvent("<sc.{0}.{1}|ERR|CATCH> Found a transient error: number = <{2}>, message = <{3}>", "SqlConfigurableRetryFactory", MethodBase.GetCurrentMethod().Name, error.Number, error.Message);
					result = true;
					break;
				}
			}
		}
		return result;
	}
}
