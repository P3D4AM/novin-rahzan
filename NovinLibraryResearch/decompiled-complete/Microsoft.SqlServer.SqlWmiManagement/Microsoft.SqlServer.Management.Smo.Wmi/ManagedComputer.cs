using System;
using System.Collections.Generic;
using System.Management;
using System.Reflection;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

public sealed class ManagedComputer : WmiSmoObject
{
	private class PropertyMetadataProvider : Microsoft.SqlServer.Management.Smo.PropertyMetadataProvider
	{
		public override int Count => 0;

		public override int PropertyNameToIDLookup(string propertyName)
		{
			return -1;
		}

		public override StaticMetadata GetStaticMetadata(int id)
		{
			return StaticMetadata.Empty;
		}
	}

	private IList<ManagementPath> managementPaths;

	private WmiConnectionInfo m_WmiConnectionInfo;

	private ManagementScope m_ManagementScope;

	private ServiceCollection m_Services;

	private ClientProtocolCollection m_ClientProtocols;

	private ServerInstanceCollection serverInstances;

	private ServerAliasCollection m_ServerAliases;

	public WmiConnectionInfo ConnectionSettings => m_WmiConnectionInfo;

	internal static string UrnSuffix => "ManagedComputer";

	internal ManagementScope ManagementScope
	{
		get
		{
			if (!m_ManagementScope.IsConnected)
			{
				TryConnect();
			}
			return m_ManagementScope;
		}
	}

	public ServiceCollection Services
	{
		get
		{
			if (m_Services == null)
			{
				m_Services = new ServiceCollection(this);
			}
			return m_Services;
		}
	}

	public ClientProtocolCollection ClientProtocols
	{
		get
		{
			if (m_ClientProtocols == null)
			{
				m_ClientProtocols = new ClientProtocolCollection(this);
			}
			return m_ClientProtocols;
		}
	}

	public ServerInstanceCollection ServerInstances
	{
		get
		{
			if (serverInstances == null)
			{
				serverInstances = new ServerInstanceCollection(this);
			}
			return serverInstances;
		}
	}

	public ServerAliasCollection ServerAliases
	{
		get
		{
			if (m_ServerAliases == null)
			{
				m_ServerAliases = new ServerAliasCollection(this);
			}
			return m_ServerAliases;
		}
	}

	internal static IList<ManagementPath> GetPossibleManagementPaths(string machineName)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		List<ManagementPath> list = new List<ManagementPath>();
		for (int num = 16; num >= 10; num--)
		{
			list.Add(new ManagementPath(string.Format(SmoApplication.DefaultCulture, "\\\\{0}\\ROOT\\Microsoft\\SqlServer\\ComputerManagement{1}", machineName, num)));
		}
		list.Add(new ManagementPath(string.Format(SmoApplication.DefaultCulture, "\\\\{0}\\ROOT\\Microsoft\\SqlServer\\ComputerManagement", machineName)));
		return list;
	}

	public ManagedComputer()
		: base(string.Empty)
	{
		Init(string.Empty);
	}

	public ManagedComputer(string machineName)
		: base(machineName)
	{
		if (machineName == null)
		{
			throw new SmoException(ExceptionTemplatesImpl.InnerException, new ArgumentNullException("machineName"));
		}
		Init(machineName);
	}

	public ManagedComputer(string machineName, string userName, string password)
		: base(machineName)
	{
		if (machineName == null)
		{
			throw new SmoException(ExceptionTemplatesImpl.InnerException, new ArgumentNullException("machineName"));
		}
		if (userName == null)
		{
			throw new SmoException(ExceptionTemplatesImpl.InnerException, new ArgumentNullException("userName"));
		}
		if (password == null)
		{
			throw new SmoException(ExceptionTemplatesImpl.InnerException, new ArgumentNullException("password"));
		}
		Init(machineName);
		m_ManagementScope.Options.Username = userName;
		m_ManagementScope.Options.Password = password;
	}

	public ManagedComputer(string machineName, string userName, string password, ProviderArchitecture providerArchitecture)
		: base(machineName)
	{
		if (machineName == null)
		{
			throw new SmoException(ExceptionTemplatesImpl.InnerException, new ArgumentNullException("machineName"));
		}
		if (userName == null)
		{
			throw new SmoException(ExceptionTemplatesImpl.InnerException, new ArgumentNullException("userName"));
		}
		if (password == null)
		{
			throw new SmoException(ExceptionTemplatesImpl.InnerException, new ArgumentNullException("password"));
		}
		Init(machineName);
		m_WmiConnectionInfo.ProviderArchitecture = providerArchitecture;
		m_ManagementScope.Options.Username = userName;
		m_ManagementScope.Options.Password = password;
	}

	internal override Microsoft.SqlServer.Management.Smo.PropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return new PropertyMetadataProvider();
	}

	private void Init(string machineName)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		m_ManagementScope = new ManagementScope();
		if (machineName.Length == 0 || "." == machineName || "(local)" == machineName)
		{
			machineName = Environment.MachineName;
			SetName(machineName);
		}
		managementPaths = GetPossibleManagementPaths(machineName);
		m_ManagementScope.Path = managementPaths[0];
		m_WmiConnectionInfo = new WmiConnectionInfo(m_ManagementScope, this);
		SetState(SqlSmoState.Existing);
	}

	public WmiSmoObject GetSmoObject(Urn urn)
	{
		try
		{
			if (null == urn)
			{
				throw new ArgumentNullException("urn");
			}
			return GetSmoObjectRec(urn);
		}
		catch (Exception ex)
		{
			SqlSmoObject.FilterException(ex);
			throw new FailedOperationException(ExceptionTemplatesImpl.FailedOperationExceptionText(ExceptionTemplatesImpl.GetSmoObject, ExceptionTemplatesImpl.ManagedComputer, base.Name), ex);
		}
	}

	private WmiSmoObject GetSmoObjectRec(Urn urn)
	{
		if (null == urn.Parent)
		{
			if (urn.Type == "ManagedComputer")
			{
				if (urn.GetAttribute("Name") == base.Name)
				{
					return this;
				}
				throw new SmoException(ExceptionTemplatesImpl.InvalidServerUrn(urn.GetAttribute("Name")));
			}
			throw new SmoException(ExceptionTemplatesImpl.InvalidUrn(urn.Type));
		}
		WmiSmoObject smoObjectRec = GetSmoObjectRec(urn.Parent);
		string type = urn.Type;
		string nameForType = urn.GetNameForType(type);
		string empty = string.Empty;
		empty = ((type == "ServerAlias") ? "ServerAliases" : ((!(type == "IPAddress")) ? (type + "s") : "IPAddresses"));
		object obj = smoObjectRec.GetType().InvokeMember(empty, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, null, smoObjectRec, new object[0], SmoApplication.DefaultCulture);
		if (obj == null)
		{
			throw new MissingObjectException(ExceptionTemplatesImpl.MissingObjectExceptionText(empty, smoObjectRec.GetType().ToString(), null)).SetHelpContext("MissingObjectExceptionText");
		}
		WmiSmoObject wmiSmoObject = (WmiSmoObject)obj.GetType().InvokeMember("GetObjectByName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, obj, new object[1] { nameForType }, SmoApplication.DefaultCulture);
		if (wmiSmoObject == null)
		{
			throw new MissingObjectException(ExceptionTemplatesImpl.ObjectDoesNotExist(SqlSmoObject.GetTypeName(type), nameForType));
		}
		return wmiSmoObject;
	}

	private Exception TryConnectUsingPath(ManagementPath path, out bool unrecoverableException)
	{
		//IL_0038: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0034: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Invalid comparison between Unknown and I4
		unrecoverableException = false;
		try
		{
			m_ManagementScope.Path = path;
			m_ManagementScope.Connect();
			new ManagementObject(m_ManagementScope, new ManagementPath("Win32Provider.Name=\"MSSQL_ManagementProvider\""), new ObjectGetOptions());
		}
		catch (ManagementException val)
		{
			ManagementException val2 = val;
			string message;
			if ((int)val2.ErrorCode == -2147217394 || (int)val2.ErrorCode == -2147217389)
			{
				message = ExceptionTemplatesImpl.WMIProviderNotInstalled(base.Name);
			}
			else
			{
				unrecoverableException = true;
				message = ExceptionTemplatesImpl.InnerWmiException;
			}
			return new SmoException(message, (Exception)(object)val2);
		}
		return null;
	}

	private void TryConnect()
	{
		Exception ex = null;
		foreach (ManagementPath managementPath in managementPaths)
		{
			ex = TryConnectUsingPath(managementPath, out var unrecoverableException);
			if (ex == null)
			{
				m_WmiConnectionInfo = new WmiConnectionInfo(m_ManagementScope, this);
				return;
			}
			if (unrecoverableException)
			{
				break;
			}
		}
		throw ex;
	}
}
