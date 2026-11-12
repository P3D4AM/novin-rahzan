using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Data.SqlClient;

public class SqlAuthenticationParameters
{
	internal class Builder
	{
		private readonly SqlAuthenticationMethod _authenticationMethod;

		private readonly string _serverName;

		private readonly string _databaseName;

		private readonly string _resource;

		private readonly string _authority;

		private string _userId;

		private string _password;

		private Guid _connectionId = Guid.NewGuid();

		private int _connectionTimeout = 15;

		public static implicit operator SqlAuthenticationParameters(Builder builder)
		{
			return new SqlAuthenticationParameters(builder._authenticationMethod, builder._serverName, builder._databaseName, builder._resource, builder._authority, builder._userId, builder._password, builder._connectionId, builder._connectionTimeout);
		}

		public Builder WithUserId(string userId)
		{
			_userId = userId;
			return this;
		}

		public Builder WithPassword(string password)
		{
			_password = password;
			return this;
		}

		public Builder WithPassword(SecureString password)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = Marshal.SecureStringToGlobalAllocUnicode(password);
				_password = Marshal.PtrToStringUni(intPtr);
				return this;
			}
			finally
			{
				Marshal.ZeroFreeGlobalAllocUnicode(intPtr);
			}
		}

		public Builder WithConnectionId(Guid connectionId)
		{
			_connectionId = connectionId;
			return this;
		}

		public Builder WithConnectionTimeout(int timeout)
		{
			_connectionTimeout = timeout;
			return this;
		}

		internal Builder(SqlAuthenticationMethod authenticationMethod, string resource, string authority, string serverName, string databaseName)
		{
			_authenticationMethod = authenticationMethod;
			_serverName = serverName;
			_databaseName = databaseName;
			_resource = resource;
			_authority = authority;
		}
	}

	public int ConnectionTimeout = 15;

	public SqlAuthenticationMethod AuthenticationMethod { get; }

	public string Resource { get; }

	public string Authority { get; }

	public string UserId { get; }

	public string Password { get; }

	public Guid ConnectionId { get; }

	public string ServerName { get; }

	public string DatabaseName { get; }

	protected SqlAuthenticationParameters(SqlAuthenticationMethod authenticationMethod, string serverName, string databaseName, string resource, string authority, string userId, string password, Guid connectionId, int connectionTimeout)
	{
		AuthenticationMethod = authenticationMethod;
		ServerName = serverName;
		DatabaseName = databaseName;
		Resource = resource;
		Authority = authority;
		UserId = userId;
		Password = password;
		ConnectionId = connectionId;
		ConnectionTimeout = connectionTimeout;
	}
}
