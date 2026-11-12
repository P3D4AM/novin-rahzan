using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrData;

public static class FailoverUtilities
{
	private static readonly string[] ServerFields = new string[1] { "ClusterQuorumState" };

	private static readonly string[] AvailabilityGroupFields = new string[2] { "Name", "PrimaryReplicaServerName" };

	private static readonly string[] AvailabilityReplicaFields = new string[8] { "UniqueId", "Name", "AvailabilityMode", "FailoverMode", "OperationalState", "RollupSynchronizationState", "Role", "QuorumVoteCount" };

	private static readonly string[] DatabaseReplicaStateFields = new string[4] { "AvailabilityReplicaId", "IsFailoverReady", "IsJoined", "SynchronizationState" };

	public static Microsoft.SqlServer.Management.Smo.Server GetServerWithInitFieldsSetting(ServerConnection connection)
	{
		Microsoft.SqlServer.Management.Smo.Server server = new Microsoft.SqlServer.Management.Smo.Server(connection);
		server.SetDefaultInitFields(typeof(Microsoft.SqlServer.Management.Smo.Server), ServerFields);
		server.SetDefaultInitFields(typeof(AvailabilityGroup), AvailabilityGroupFields);
		server.SetDefaultInitFields(typeof(AvailabilityReplica), AvailabilityReplicaFields);
		server.SetDefaultInitFields(typeof(DatabaseReplicaState), DatabaseReplicaStateFields);
		return server;
	}

	public static bool TryDecrypt(Database database, string pwd)
	{
		if (database == null)
		{
			throw new ArgumentNullException("database");
		}
		if (pwd == null)
		{
			throw new ArgumentNullException("pwd");
		}
		try
		{
			database.MasterKey.Open(pwd);
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static void ProvisionCredential(Microsoft.SqlServer.Management.Smo.Server server, string dbName, string pwd)
	{
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		if (pwd == null)
		{
			throw new ArgumentNullException("pwd");
		}
		if (string.IsNullOrWhiteSpace(dbName))
		{
			throw new ArgumentNullException("dbName");
		}
		bool isOpen = server.ConnectionContext.IsOpen;
		try
		{
			if (!isOpen)
			{
				server.ConnectionContext.SqlConnectionObject.Open();
			}
			using SqlCommand sqlCommand = new SqlCommand("DECLARE @pwd nvarchar(4000) = REPLACE(@password, N'''', N'''''');EXEC sp_control_dbmasterkey_password @db_name = @dbName, @password = @pwd, @action = N'add'", server.ConnectionContext.SqlConnectionObject);
			SqlParameter[] values = new SqlParameter[2]
			{
				new SqlParameter("@dbName", dbName),
				new SqlParameter("@password", pwd)
			};
			sqlCommand.Parameters.AddRange(values);
			sqlCommand.ExecuteNonQuery();
		}
		catch (SqlException ex)
		{
			if (ex.Number != 15594)
			{
				throw;
			}
		}
		finally
		{
			if (!isOpen)
			{
				server.ConnectionContext.SqlConnectionObject.Close();
			}
		}
	}

	public static bool HasDbNeedToBeDecrypted(Microsoft.SqlServer.Management.Smo.Server server, List<PrimaryDatabaseData> databases)
	{
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		if (databases == null)
		{
			throw new ArgumentNullException("databases");
		}
		foreach (PrimaryDatabaseData database in databases)
		{
			if (server.Databases[database.Name].MasterKey != null)
			{
				return true;
			}
		}
		return false;
	}
}
