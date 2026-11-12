using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrData;

public class AvailabilityGroupReplica
{
	public const string SidConditionFormat = "Server/Login[@SidHexString = \"{0}\"]";

	public const string DefaultEndpointName = "Hadr_endpoint";

	public const int DefaultEndpointPortNumber = 5022;

	public const int GetServerSleepTime = 1000;

	public const int DefaultBackupPriority = 50;

	public AvailabilityGroupReplicaData AvailabilityGroupReplicaData { get; set; }

	public int BackupPriority
	{
		get
		{
			return AvailabilityGroupReplicaData.BackupPriority;
		}
		set
		{
			AvailabilityGroupReplicaData.BackupPriority = value;
		}
	}

	public string InitialRoleString => new LocalizableEnumConverter(typeof(ReplicaRole)).ConvertToString(AvailabilityGroupReplicaData.InitialRole);

	public string EndpointName
	{
		get
		{
			return AvailabilityGroupReplicaData.EndpointName;
		}
		set
		{
			if (AvailabilityGroupReplicaData.EndpointCanBeConfigured && !AvailabilityGroupReplicaData.EndpointPresent && string.CompareOrdinal(AvailabilityGroupReplicaData.EndpointName, value) != 0)
			{
				AvailabilityGroupReplicaData.EndpointName = value;
				if (string.IsNullOrEmpty(AvailabilityGroupReplicaData.EndpointUrl))
				{
					SetEndpointUrl();
				}
			}
		}
	}

	public AvailabilityReplicaConnectionModeInSecondaryRole ReadableSecondaryRole
	{
		get
		{
			return AvailabilityGroupReplicaData.ReadableSecondaryRole;
		}
		set
		{
			if (!Enum.IsDefined(typeof(AvailabilityReplicaConnectionModeInSecondaryRole), value))
			{
				throw new InvalidEnumArgumentException("value", (int)value, typeof(AvailabilityReplicaConnectionModeInSecondaryRole));
			}
			if (AvailabilityGroupReplicaData.ReadableSecondaryRole != value)
			{
				AvailabilityGroupReplicaData.ReadableSecondaryRole = value;
			}
		}
	}

	public int EndpointPortNumber
	{
		get
		{
			return AvailabilityGroupReplicaData.EndpointPortNumber;
		}
		set
		{
			if (AvailabilityGroupReplicaData.EndpointCanBeConfigured && !AvailabilityGroupReplicaData.EndpointPresent && AvailabilityGroupReplicaData.EndpointPortNumber != value)
			{
				AvailabilityGroupReplicaData.EndpointPortNumber = value;
				SetEndpointUrl();
			}
		}
	}

	public string ConnectedAs
	{
		get
		{
			if (AvailabilityGroupReplicaData.Connection == null)
			{
				return "NotConnected";
			}
			return AvailabilityGroupReplicaData.Connection.TrueLogin;
		}
	}

	public EndpointEncryptionAlgorithm EndpointEncryptionAlgorithm
	{
		get
		{
			return AvailabilityGroupReplicaData.EndpointEncryptionAlgorithm;
		}
		set
		{
			AvailabilityGroupReplicaData.EndpointEncryptionAlgorithm = value;
		}
	}

	public bool IsEndpointEncrypted
	{
		get
		{
			return AvailabilityGroupReplicaData.EndpointEncryption != EndpointEncryption.Disabled;
		}
		set
		{
			if (AvailabilityGroupReplicaData.EndpointCanBeConfigured && !AvailabilityGroupReplicaData.EndpointPresent)
			{
				AvailabilityGroupReplicaData.EndpointEncryption = (value ? EndpointEncryption.Required : EndpointEncryption.Disabled);
			}
		}
	}

	public bool IsValidDomainUserForWinAuthentication
	{
		get
		{
			bool flag = false;
			if (!AvailabilityGroupReplicaData.EndpointPresent)
			{
				flag = true;
			}
			else
			{
				EndpointAuthenticationOrder endpointAuthenticationOrder = AvailabilityGroupReplicaData.Endpoint.Payload.DatabaseMirroring.EndpointAuthenticationOrder;
				if (endpointAuthenticationOrder == EndpointAuthenticationOrder.Kerberos || endpointAuthenticationOrder == EndpointAuthenticationOrder.Negotiate || endpointAuthenticationOrder == EndpointAuthenticationOrder.Ntlm)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return true;
			}
			if (string.IsNullOrEmpty(AvailabilityGroupReplicaData.EndpointServiceAccount))
			{
				return false;
			}
			string[] array = AvailabilityGroupReplicaData.EndpointServiceAccount.Split(new char[1] { '\\' });
			if (array.Length == 1)
			{
				return false;
			}
			if (array[0] == ".")
			{
				return false;
			}
			if (UserSecurity.IsDomainUserAccount(AvailabilityGroupReplicaData.EndpointServiceAccount, Constants.WellKnownSidTypes))
			{
				return true;
			}
			return false;
		}
	}

	public EndpointEncryption EndpointEncryption
	{
		get
		{
			return AvailabilityGroupReplicaData.EndpointEncryption;
		}
		set
		{
			AvailabilityGroupReplicaData.EndpointEncryption = value;
		}
	}

	public AvailabilityGroupReplica(AvailabilityGroupReplicaData availabilityGroupReplicaData)
	{
		if (availabilityGroupReplicaData == null)
		{
			throw new ArgumentNullException("availabilityGroupReplicaData");
		}
		AvailabilityGroupReplicaData = availabilityGroupReplicaData;
	}

	public Microsoft.SqlServer.Management.Smo.Server GetServer()
	{
		if (AvailabilityGroupReplicaData.Connection == null)
		{
			return null;
		}
		return new Microsoft.SqlServer.Management.Smo.Server(AvailabilityGroupReplicaData.Connection);
	}

	public void AddGrantServiceAccount(IEnumerable<string> loginNames)
	{
		if (loginNames == null)
		{
			throw new ArgumentNullException("loginNames");
		}
		foreach (string loginName in loginNames)
		{
			ObjectPermissionSet permission = new ObjectPermissionSet(ObjectPermission.Connect);
			AvailabilityGroupReplicaData.Endpoint.Grant(permission, loginName);
		}
	}

	public IEnumerable<KeyValuePair<string, bool>> GetLoginsForRemoteReplicas(IEnumerable<KeyValuePair<byte[], string>> sidServiceAccountPairCollection)
	{
		Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
		if (sidServiceAccountPairCollection != null && sidServiceAccountPairCollection.Any())
		{
			Microsoft.SqlServer.Management.Smo.Server server = GetServer();
			foreach (KeyValuePair<byte[], string> item in sidServiceAccountPairCollection)
			{
				byte[] key = item.Key;
				string value = item.Value;
				string loginName;
				bool flag = ExistsSID(server, key, out loginName);
				if (!flag)
				{
					loginName = value;
				}
				if (ValidDomainUserFormat(loginName) && !dictionary.ContainsKey(loginName))
				{
					dictionary.Add(loginName, flag);
				}
			}
		}
		return dictionary;
	}

	public void AdjustEndpointEncryption(AvailabilityGroupReplica primaryReplica)
	{
		if (AvailabilityGroupReplicaData.InitialRole != 0 && primaryReplica != null && !AvailabilityGroupReplicaData.EndpointPresent)
		{
			AvailabilityGroupReplicaData.EndpointEncryption = primaryReplica.EndpointEncryption;
			AvailabilityGroupReplicaData.EndpointEncryptionAlgorithm = primaryReplica.EndpointEncryptionAlgorithm;
		}
	}

	public bool IsValidEndpoint()
	{
		if (!AvailabilityGroupReplicaData.EndpointCanBeConfigured)
		{
			return true;
		}
		if (string.IsNullOrEmpty(AvailabilityGroupReplicaData.EndpointUrl))
		{
			return false;
		}
		if (string.IsNullOrEmpty(AvailabilityGroupReplicaData.EndpointName))
		{
			return false;
		}
		if (AvailabilityGroupReplicaData.EndpointPortNumber <= 0)
		{
			return false;
		}
		return true;
	}

	public void ConfigureEndpoint()
	{
		if (!AvailabilityGroupReplicaData.EndpointCanBeConfigured)
		{
			throw new InvalidOperationException();
		}
		if (AvailabilityGroupReplicaData.State != AvailabilityObjectState.Existing)
		{
			if (AvailabilityGroupReplicaData.EndpointPresent)
			{
				AlterEndpoint();
			}
			else
			{
				CreateEndpoint();
			}
		}
	}

	private void AlterEndpoint()
	{
		if (AvailabilityGroupReplicaData.AvailabilityMode != AvailabilityReplicaAvailabilityMode.ConfigurationOnly && AvailabilityGroupReplicaData.Endpoint.Payload.DatabaseMirroring.ServerMirroringRole == ServerMirroringRole.Witness)
		{
			AvailabilityGroupReplicaData.Endpoint.Payload.DatabaseMirroring.ServerMirroringRole = ServerMirroringRole.All;
			AvailabilityGroupReplicaData.Endpoint.Alter();
		}
		AvailabilityGroupReplicaData.Endpoint.Start();
	}

	public void SetEndpointUrl()
	{
		if (string.IsNullOrEmpty(AvailabilityGroupReplicaData.EndpointName))
		{
			AvailabilityGroupReplicaData.EndpointUrl = string.Empty;
			return;
		}
		string empty = string.Empty;
		try
		{
			empty = GetServer().Information.FullyQualifiedNetName;
		}
		catch
		{
			empty = AvailabilityGroupReplicaData.ReplicaName;
		}
		AvailabilityGroupReplicaData.EndpointUrl = "TCP://" + empty + ":" + AvailabilityGroupReplicaData.EndpointPortNumber;
	}

	private bool ValidDomainUserFormat(string serviceAccount)
	{
		if (string.IsNullOrEmpty(serviceAccount))
		{
			return false;
		}
		string[] array = serviceAccount.Split(new char[1] { '\\' });
		if (array.Length == 1)
		{
			return false;
		}
		if (array[0] == ".")
		{
			return false;
		}
		return true;
	}

	private bool ExistsSID(Microsoft.SqlServer.Management.Smo.Server server, byte[] sid, out string loginName)
	{
		loginName = string.Empty;
		bool result = false;
		Enumerator enumerator = new Enumerator();
		Request request = new Request(new Urn(string.Format(CultureInfo.InvariantCulture, "Server/Login[@SidHexString = \"{0}\"]", ByteArrayToString(sid))));
		request.Fields = new string[1] { "Name" };
		try
		{
			if (enumerator.Process(server.ConnectionContext, request).Data is DataTable dataTable)
			{
				DataRowCollection rows = dataTable.Rows;
				if (rows.Count > 0)
				{
					DataRow dataRow = rows[0];
					loginName = (string)dataRow["Name"];
					result = true;
				}
			}
		}
		catch
		{
		}
		return result;
	}

	private string ByteArrayToString(byte[] ba)
	{
		StringBuilder stringBuilder = new StringBuilder(ba.Length * 2);
		foreach (byte b in ba)
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0:x2}", b);
		}
		return stringBuilder.ToString();
	}

	public void CreateLogins(IEnumerable<string> loginNames)
	{
		if (loginNames == null || loginNames.Count() <= 0)
		{
			return;
		}
		Microsoft.SqlServer.Management.Smo.Server server = GetServer();
		foreach (string loginName in loginNames)
		{
			if (!server.Logins.Contains(loginName) && !server.Logins.Contains(loginName.ToUpperInvariant()))
			{
				Login login = new Login(server, loginName);
				login.LoginType = LoginType.WindowsUser;
				login.Create();
			}
		}
	}

	private byte[] GetServiceAccountSid(Microsoft.SqlServer.Management.Smo.Server server)
	{
		byte[] result = null;
		Enumerator enumerator = new Enumerator();
		Request request = new Request(new Urn("Server"));
		request.Fields = new string[2] { "ServiceAccount", "ServiceAccountSid" };
		try
		{
			if (enumerator.Process(server.ConnectionContext, request).Data is DataTable dataTable)
			{
				DataRowCollection rows = dataTable.Rows;
				if (rows.Count > 0)
				{
					result = (byte[])rows[0]["ServiceAccountSid"];
				}
			}
		}
		catch
		{
		}
		return result;
	}

	private void CreateEndpoint()
	{
		AvailabilityGroupReplicaData.Endpoint = new Endpoint(GetServer(), AvailabilityGroupReplicaData.EndpointName);
		AvailabilityGroupReplicaData.Endpoint.ProtocolType = ProtocolType.Tcp;
		AvailabilityGroupReplicaData.Endpoint.EndpointType = EndpointType.DatabaseMirroring;
		AvailabilityGroupReplicaData.Endpoint.Protocol.Tcp.ListenerPort = AvailabilityGroupReplicaData.EndpointPortNumber;
		AvailabilityGroupReplicaData.Endpoint.Payload.DatabaseMirroring.EndpointEncryption = AvailabilityGroupReplicaData.EndpointEncryption;
		if (AvailabilityGroupReplicaData.EndpointEncryption != 0)
		{
			AvailabilityGroupReplicaData.Endpoint.Payload.DatabaseMirroring.EndpointEncryptionAlgorithm = EndpointEncryptionAlgorithm;
		}
		AvailabilityGroupReplicaData.Endpoint.Payload.DatabaseMirroring.ServerMirroringRole = ((AvailabilityGroupReplicaData.AvailabilityMode == AvailabilityReplicaAvailabilityMode.ConfigurationOnly) ? ServerMirroringRole.Witness : ServerMirroringRole.All);
		AvailabilityGroupReplicaData.Endpoint.Create();
		try
		{
			AvailabilityGroupReplicaData.Endpoint.Start();
		}
		catch
		{
			AvailabilityGroupReplicaData.Endpoint.Drop();
			throw;
		}
	}
}
