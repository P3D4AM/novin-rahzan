using System;
using System.Management;
using Microsoft.SqlServer.Management.Common;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

public abstract class ProtocolBase : WmiSmoObject, IAlterable
{
	internal ProtocolPropertyCollection m_protocolProperties;

	public ProtocolPropertyCollection ProtocolProperties
	{
		get
		{
			if (m_protocolProperties == null)
			{
				m_protocolProperties = GetProtocolPropertyCollection();
			}
			return m_protocolProperties;
		}
	}

	internal ProtocolBase(WmiCollectionBase parentColl, string name)
		: base(parentColl, name)
	{
	}

	protected abstract ManagementObject GetManagementObject();

	public void Alter()
	{
		try
		{
			AlterImplWorker();
			if (!SmoApplication.eventsSingleton.IsNullObjectAltered())
			{
				SmoApplication.eventsSingleton.CallObjectAltered(GetManagedComputer(), new ObjectAlteredEventArgs(Urn, this));
			}
		}
		catch (Exception ex)
		{
			SqlSmoObject.FilterException(ex);
			throw new FailedOperationException(ExceptionTemplatesImpl.Alter, this, ex);
		}
	}

	internal virtual void AlterImplWorker()
	{
		ManagementObject managementObject = GetManagementObject();
		Property property = base.Properties.Get("IsEnabled");
		if (property.Dirty)
		{
			if ((bool)property.Value)
			{
				InvokeMgmtMethod(managementObject, "SetEnable", null);
			}
			else
			{
				InvokeMgmtMethod(managementObject, "SetDisable", null);
			}
		}
		if (base.Properties.Contains("Order"))
		{
			Property property2 = base.Properties.Get("Order");
			if (property2.Dirty)
			{
				InvokeMgmtMethod(managementObject, "SetOrderValue", new object[1] { SmoApplication.ConvertInt32ToUInt32((int)property2.Value) });
			}
		}
		AlterProtocolProperties(ProtocolProperties);
	}

	public override void Refresh()
	{
		base.Refresh();
		m_protocolProperties = null;
	}
}
