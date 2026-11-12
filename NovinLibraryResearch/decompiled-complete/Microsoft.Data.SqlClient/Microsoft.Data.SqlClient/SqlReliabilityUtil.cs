using System;
using System.Collections.Generic;

namespace Microsoft.Data.SqlClient;

internal static class SqlReliabilityUtil
{
	internal static AggregateException ConfigurableRetryFail(IList<Exception> exceptions, SqlRetryLogicBase retryLogic, bool canceled)
	{
		if (!canceled)
		{
			return new AggregateException(StringsHelper.GetString(Strings.SqlRetryLogic_RetryExceeded, retryLogic.NumberOfTries), exceptions);
		}
		return new AggregateException(StringsHelper.GetString(Strings.SqlRetryLogic_RetryCanceled, retryLogic.Current), exceptions);
	}

	internal static ArgumentOutOfRangeException ArgumentOutOfRange(string paramName, int value, int minValue, int MaxValue)
	{
		return new ArgumentOutOfRangeException(paramName, StringsHelper.GetString(Strings.SqlRetryLogic_InvalidRange, value, minValue, MaxValue));
	}

	internal static ArgumentOutOfRangeException ArgumentOutOfRange(string paramName, TimeSpan value, TimeSpan minValue, TimeSpan MaxValue)
	{
		return new ArgumentOutOfRangeException(paramName, StringsHelper.GetString(Strings.SqlRetryLogic_InvalidRange, value, minValue, MaxValue));
	}

	internal static ArgumentNullException ArgumentNull(string paramName)
	{
		return new ArgumentNullException(paramName);
	}

	internal static ArgumentOutOfRangeException InvalidMinAndMaxPair(string minParamName, TimeSpan minValue, string maxParamName, TimeSpan maxValue)
	{
		return new ArgumentOutOfRangeException(minParamName, StringsHelper.GetString(Strings.SqlRetryLogic_InvalidMinMaxPair, minValue, maxValue, minParamName, maxParamName));
	}
}
