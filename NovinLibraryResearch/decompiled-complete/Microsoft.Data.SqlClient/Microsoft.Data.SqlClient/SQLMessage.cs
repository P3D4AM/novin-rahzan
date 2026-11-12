using System;

namespace Microsoft.Data.SqlClient;

internal sealed class SQLMessage
{
	private SQLMessage()
	{
	}

	internal static string CultureIdError()
	{
		return StringsHelper.GetString(Strings.SQL_CultureIdError);
	}

	internal static string EncryptionNotSupportedByClient()
	{
		return StringsHelper.GetString(Strings.SQL_EncryptionNotSupportedByClient);
	}

	internal static string EncryptionNotSupportedByServer()
	{
		return StringsHelper.GetString(Strings.SQL_EncryptionNotSupportedByServer);
	}

	internal static string CTAIPNotSupportedByServer()
	{
		return StringsHelper.GetString(Strings.SQL_CTAIPNotSupportedByServer);
	}

	internal static string OperationCancelled()
	{
		return StringsHelper.GetString(Strings.SQL_OperationCancelled);
	}

	internal static string SevereError()
	{
		return StringsHelper.GetString(Strings.SQL_SevereError);
	}

	internal static string SSPIInitializeError()
	{
		return StringsHelper.GetString(Strings.SQL_SSPIInitializeError);
	}

	internal static string SSPIGenerateError()
	{
		return StringsHelper.GetString(Strings.SQL_SSPIGenerateError);
	}

	internal static string Timeout()
	{
		return StringsHelper.GetString(Strings.SQL_Timeout_Execution);
	}

	internal static string Timeout_PreLogin_Begin()
	{
		return StringsHelper.GetString(Strings.SQL_Timeout_PreLogin_Begin);
	}

	internal static string Timeout_PreLogin_InitializeConnection()
	{
		return StringsHelper.GetString(Strings.SQL_Timeout_PreLogin_InitializeConnection);
	}

	internal static string Timeout_PreLogin_SendHandshake()
	{
		return StringsHelper.GetString(Strings.SQL_Timeout_PreLogin_SendHandshake);
	}

	internal static string Timeout_PreLogin_ConsumeHandshake()
	{
		return StringsHelper.GetString(Strings.SQL_Timeout_PreLogin_ConsumeHandshake);
	}

	internal static string Timeout_Login_Begin()
	{
		return StringsHelper.GetString(Strings.SQL_Timeout_Login_Begin);
	}

	internal static string Timeout_Login_ProcessConnectionAuth()
	{
		return StringsHelper.GetString(Strings.SQL_Timeout_Login_ProcessConnectionAuth);
	}

	internal static string Timeout_PostLogin()
	{
		return StringsHelper.GetString(Strings.SQL_Timeout_PostLogin);
	}

	internal static string Timeout_FailoverInfo()
	{
		return StringsHelper.GetString(Strings.SQL_Timeout_FailoverInfo);
	}

	internal static string Timeout_RoutingDestination()
	{
		return StringsHelper.GetString(Strings.SQL_Timeout_RoutingDestinationInfo);
	}

	internal static string Duration_PreLogin_Begin(long PreLoginBeginDuration)
	{
		return StringsHelper.GetString(Strings.SQL_Duration_PreLogin_Begin, PreLoginBeginDuration);
	}

	internal static string Duration_PreLoginHandshake(long PreLoginBeginDuration, long PreLoginHandshakeDuration)
	{
		return StringsHelper.GetString(Strings.SQL_Duration_PreLoginHandshake, PreLoginBeginDuration, PreLoginHandshakeDuration);
	}

	internal static string Duration_Login_Begin(long PreLoginBeginDuration, long PreLoginHandshakeDuration, long LoginBeginDuration)
	{
		return StringsHelper.GetString(Strings.SQL_Duration_Login_Begin, PreLoginBeginDuration, PreLoginHandshakeDuration, LoginBeginDuration);
	}

	internal static string Duration_Login_ProcessConnectionAuth(long PreLoginBeginDuration, long PreLoginHandshakeDuration, long LoginBeginDuration, long LoginAuthDuration)
	{
		return StringsHelper.GetString(Strings.SQL_Duration_Login_ProcessConnectionAuth, PreLoginBeginDuration, PreLoginHandshakeDuration, LoginBeginDuration, LoginAuthDuration);
	}

	internal static string Duration_PostLogin(long PreLoginBeginDuration, long PreLoginHandshakeDuration, long LoginBeginDuration, long LoginAuthDuration, long PostLoginDuration)
	{
		return StringsHelper.GetString(Strings.SQL_Duration_PostLogin, PreLoginBeginDuration, PreLoginHandshakeDuration, LoginBeginDuration, LoginAuthDuration, PostLoginDuration);
	}

	internal static string UserInstanceFailure()
	{
		return StringsHelper.GetString(Strings.SQL_UserInstanceFailure);
	}

	internal static string PreloginError()
	{
		return StringsHelper.GetString(Strings.Snix_PreLogin);
	}

	internal static string ExClientConnectionId()
	{
		return StringsHelper.GetString(Strings.SQL_ExClientConnectionId);
	}

	internal static string ExErrorNumberStateClass()
	{
		return StringsHelper.GetString(Strings.SQL_ExErrorNumberStateClass);
	}

	internal static string ExOriginalClientConnectionId()
	{
		return StringsHelper.GetString(Strings.SQL_ExOriginalClientConnectionId);
	}

	internal static string ExRoutingDestination()
	{
		return StringsHelper.GetString(Strings.SQL_ExRoutingDestination);
	}
}
