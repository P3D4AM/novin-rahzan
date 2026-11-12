using System;
using System.Management;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

public sealed class WmiConnectionInfo
{
	private ProviderArchitecture m_ProviderArchitecture;

	private ManagementScope managementScope;

	private ManagedComputer parent;

	internal ConnectionOptions Options => managementScope.Options;

	public TimeSpan Timeout
	{
		get
		{
			return ((ManagementOptions)managementScope.Options).Timeout;
		}
		set
		{
			((ManagementOptions)managementScope.Options).Timeout = value;
		}
	}

	public string MachineName
	{
		get
		{
			return parent.Name;
		}
		set
		{
			if (value == null)
			{
				throw new SmoException(ExceptionTemplatesImpl.InnerException, new ArgumentNullException("MachineName"));
			}
			if (managementScope.IsConnected)
			{
				throw new SmoException(ExceptionTemplatesImpl.PropertyCannotBeChangedAfterConnection("MachineName"));
			}
			if (value.Length == 0 || "." == value || "(local)" == value)
			{
				value = Environment.MachineName;
			}
			managementScope.Path = ManagedComputer.GetPossibleManagementPaths(value)[0];
			parent.SetName(value);
		}
	}

	public string Username
	{
		get
		{
			return managementScope.Options.Username;
		}
		set
		{
			if (value == null)
			{
				throw new SmoException(ExceptionTemplatesImpl.InnerException, new ArgumentNullException("Username"));
			}
			if (managementScope.IsConnected)
			{
				throw new SmoException(ExceptionTemplatesImpl.PropertyCannotBeChangedAfterConnection("Username"));
			}
			managementScope.Options.Username = value;
		}
	}

	public ProviderArchitecture ProviderArchitecture
	{
		get
		{
			return m_ProviderArchitecture;
		}
		set
		{
			m_ProviderArchitecture = value;
			if (m_ProviderArchitecture != 0)
			{
				((ManagementOptions)managementScope.Options).Context.Add("__RequiredArchitecture", (object)true);
				((ManagementOptions)managementScope.Options).Context.Add("__ProviderArchitecture", (ProviderArchitecture.Use32bit == m_ProviderArchitecture) ? ((object)32) : ((object)64));
			}
		}
	}

	internal WmiConnectionInfo(ManagementScope scope, ManagedComputer parent)
	{
		managementScope = scope;
		this.parent = parent;
		((ManagementOptions)managementScope.Options).Timeout = new TimeSpan(0, 0, 10);
	}

	public void SetPassword(string password)
	{
		if (password == null)
		{
			throw new SmoException(ExceptionTemplatesImpl.InnerException, new ArgumentNullException("Password"));
		}
		if (managementScope.IsConnected)
		{
			throw new SmoException(ExceptionTemplatesImpl.PropertyCannotBeChangedAfterConnection("Password"));
		}
		managementScope.Options.Password = password;
	}
}
