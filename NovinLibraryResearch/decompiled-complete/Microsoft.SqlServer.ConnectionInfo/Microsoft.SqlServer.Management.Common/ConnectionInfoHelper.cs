using System;
using Microsoft.Data.SqlClient;

namespace Microsoft.SqlServer.Management.Common;

public static class ConnectionInfoHelper
{
	public static void SetTokenOnConnection(SqlConnection conn, string accessToken)
	{
		conn.AccessToken = accessToken;
	}

	public static string GetTokenFromSqlConnection(SqlConnection conn)
	{
		CheckForNull(conn);
		return conn.AccessToken;
	}

	public static SqlConnection CreateSqlConnection(SqlConnectionInfo connectionInfo)
	{
		SqlConnection sqlConnection = new SqlConnection(connectionInfo.ConnectionString);
		if (connectionInfo.AccessToken != null)
		{
			SetTokenOnConnection(sqlConnection, connectionInfo.AccessToken.GetAccessToken());
		}
		return sqlConnection;
	}

	private static void CheckForNull(SqlConnection conn)
	{
		if (conn == null)
		{
			throw new ArgumentNullException("conn");
		}
	}
}
