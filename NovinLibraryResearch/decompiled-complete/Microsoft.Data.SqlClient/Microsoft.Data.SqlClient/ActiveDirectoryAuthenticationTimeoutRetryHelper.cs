using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.Data.SqlClient;

internal class ActiveDirectoryAuthenticationTimeoutRetryHelper
{
	private ActiveDirectoryAuthenticationTimeoutRetryState _state;

	private SqlFedAuthToken _token;

	private readonly string _typeName;

	private readonly SqlClientLogger _sqlAuthLogger = new SqlClientLogger();

	public ActiveDirectoryAuthenticationTimeoutRetryState State
	{
		get
		{
			return _state;
		}
		set
		{
			switch (_state)
			{
			case ActiveDirectoryAuthenticationTimeoutRetryState.NotStarted:
				if (value != ActiveDirectoryAuthenticationTimeoutRetryState.Retrying && value != ActiveDirectoryAuthenticationTimeoutRetryState.HasLoggedIn)
				{
					throw new InvalidOperationException($"Cannot transit from {_state} to {value}.");
				}
				break;
			case ActiveDirectoryAuthenticationTimeoutRetryState.Retrying:
				if (value != ActiveDirectoryAuthenticationTimeoutRetryState.HasLoggedIn)
				{
					throw new InvalidOperationException($"Cannot transit from {_state} to {value}.");
				}
				break;
			case ActiveDirectoryAuthenticationTimeoutRetryState.HasLoggedIn:
				throw new InvalidOperationException($"Cannot transit from {_state} to {value}.");
			default:
				throw new InvalidOperationException($"Unsupported state: {value}.");
			}
			if (_sqlAuthLogger.IsLoggingEnabled)
			{
				_sqlAuthLogger.LogInfo(_typeName, "SetState", $"State changed from {_state} to {value}.");
			}
			_state = value;
		}
	}

	public SqlFedAuthToken CachedToken
	{
		get
		{
			if (_sqlAuthLogger.IsLoggingEnabled)
			{
				_sqlAuthLogger.LogInfo(_typeName, "GetCachedToken", "Retrieved cached token " + GetTokenHash(_token) + ".");
			}
			return _token;
		}
		set
		{
			if (_sqlAuthLogger.IsLoggingEnabled)
			{
				_sqlAuthLogger.LogInfo(_typeName, "SetCachedToken", "CachedToken changed from " + GetTokenHash(_token) + " to " + GetTokenHash(value) + ".");
			}
			_token = value;
		}
	}

	public ActiveDirectoryAuthenticationTimeoutRetryHelper()
	{
		_typeName = GetType().Name;
	}

	public bool CanRetryWithSqlException(SqlException sqlex)
	{
		string method = "CheckCanRetry";
		if (_sqlAuthLogger.LogAssert(_state == ActiveDirectoryAuthenticationTimeoutRetryState.NotStarted, _typeName, method, $"Cannot retry due to state == {_state}.") && _sqlAuthLogger.LogAssert(CachedToken != null, _typeName, method, "Cannot retry when cached token is null.") && _sqlAuthLogger.LogAssert(IsConnectTimeoutError(sqlex), _typeName, method, "Cannot retry when exception is not timeout."))
		{
			_sqlAuthLogger.LogInfo(_typeName, method, "All checks passed.");
			return true;
		}
		return false;
	}

	private static bool IsConnectTimeoutError(SqlException sqlex)
	{
		if (!(sqlex.InnerException is Win32Exception ex))
		{
			return false;
		}
		if (ex.NativeErrorCode != 10054)
		{
			return ex.NativeErrorCode == 258;
		}
		return true;
	}

	private static string GetTokenHash(SqlFedAuthToken token)
	{
		if (token == null)
		{
			return "null";
		}
		string s = SqlAuthenticationToken.AccessTokenStringFromBytes(token.accessToken);
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		using SHA256 sHA = SHA256.Create();
		byte[] inArray = sHA.ComputeHash(bytes);
		return Convert.ToBase64String(inArray);
	}
}
