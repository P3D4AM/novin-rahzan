using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Data.SqlClient;

internal static class LocalAppContextSwitches
{
	private const string TypeName = "LocalAppContextSwitches";

	internal const string MakeReadAsyncBlockingString = "Switch.Microsoft.Data.SqlClient.MakeReadAsyncBlocking";

	internal const string LegacyRowVersionNullString = "Switch.Microsoft.Data.SqlClient.LegacyRowVersionNullBehavior";

	internal const string EnableRetryLogicSwitch = "Switch.Microsoft.Data.SqlClient.EnableRetryLogic";

	private static bool _makeReadAsyncBlocking;

	private static bool? s_LegacyRowVersionNullBehavior;

	private static bool? s_isRetryEnabled;

	internal const string UseMinimumLoginTimeoutString = "Switch.Microsoft.Data.SqlClient.UseOneSecFloorInTimeoutCalculationDuringLogin";

	private static bool _useMinimumLoginTimeout;

	internal const string DisableTNIRByDefaultString = "Switch.Microsoft.Data.SqlClient.DisableTNIRByDefaultInConnectionString";

	private static bool _disableTNIRByDefault;

	internal static bool IsRetryEnabled
	{
		get
		{
			bool? flag = s_isRetryEnabled;
			if (!flag.HasValue)
			{
				bool isEnabled = AppContext.TryGetSwitch("Switch.Microsoft.Data.SqlClient.EnableRetryLogic", out isEnabled) && isEnabled;
				s_isRetryEnabled = isEnabled;
			}
			return s_isRetryEnabled.Value;
		}
	}

	public static bool MakeReadAsyncBlocking
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!AppContext.TryGetSwitch("Switch.Microsoft.Data.SqlClient.MakeReadAsyncBlocking", out _makeReadAsyncBlocking))
			{
				return false;
			}
			return _makeReadAsyncBlocking;
		}
	}

	public static bool LegacyRowVersionNullBehavior
	{
		get
		{
			bool? flag = s_LegacyRowVersionNullBehavior;
			if (!flag.HasValue)
			{
				bool value = false;
				if (AppContext.TryGetSwitch("Switch.Microsoft.Data.SqlClient.LegacyRowVersionNullBehavior", out var isEnabled))
				{
					value = isEnabled;
				}
				s_LegacyRowVersionNullBehavior = value;
			}
			return s_LegacyRowVersionNullBehavior.Value;
		}
	}

	public static bool UseMinimumLoginTimeout
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!AppContext.TryGetSwitch("Switch.Microsoft.Data.SqlClient.UseOneSecFloorInTimeoutCalculationDuringLogin", out _useMinimumLoginTimeout))
			{
				return false;
			}
			return _useMinimumLoginTimeout;
		}
	}

	public static bool DisableTNIRByDefault
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!AppContext.TryGetSwitch("Switch.Microsoft.Data.SqlClient.DisableTNIRByDefaultInConnectionString", out _disableTNIRByDefault))
			{
				return false;
			}
			return _disableTNIRByDefault;
		}
	}
}
