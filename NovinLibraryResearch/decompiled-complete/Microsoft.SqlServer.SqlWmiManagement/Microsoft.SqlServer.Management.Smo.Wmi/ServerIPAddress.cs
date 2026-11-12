using System;
using System.Data;
using System.Management;
using System.Net;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

public sealed class ServerIPAddress : WmiSmoObject
{
	private class PropertyMetadataProvider : Microsoft.SqlServer.Management.Smo.PropertyMetadataProvider
	{
		private static StaticMetadata[] staticMetadata = new StaticMetadata[0];

		public override int Count => 0;

		public override int PropertyNameToIDLookup(string propertyName)
		{
			return -1;
		}

		public override StaticMetadata GetStaticMetadata(int id)
		{
			return staticMetadata[id];
		}
	}

	internal IPAddressPropertyCollection m_protocolProperties;

	public ServerProtocol Parent
	{
		get
		{
			CheckObjectState();
			return base.ParentColl.ParentInstance as ServerProtocol;
		}
	}

	internal static string UrnSuffix => "IPAddress";

	public IPAddressPropertyCollection IPAddressProperties
	{
		get
		{
			if (m_protocolProperties == null)
			{
				m_protocolProperties = (IPAddressPropertyCollection)GetProtocolPropertyCollection();
			}
			return m_protocolProperties;
		}
	}

	public IPAddress IPAddress
	{
		get
		{
			if (IPAddressProperties.Contains("IpAddress"))
			{
				return IPAddress.Parse((string)((IPAddressProperty)IPAddressProperties["IpAddress"]).Value);
			}
			return IPAddress.Any;
		}
	}

	internal override Microsoft.SqlServer.Management.Smo.PropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return new PropertyMetadataProvider();
	}

	internal ServerIPAddress(WmiCollectionBase parentColl, string name)
		: base(parentColl, name)
	{
	}

	protected override ProtocolPropertyCollection CreateProtocolPropertyCollection()
	{
		return new IPAddressPropertyCollection();
	}

	protected override ProtocolProperty GetPropertyObject(PropertyCollection properties, DataRow dr, object propValue)
	{
		IPAddressProperty iPAddressProperty = new IPAddressProperty(properties.Get((string)dr["Name"]));
		iPAddressProperty.PropertyType = (uint)dr["Type"];
		iPAddressProperty.SetValue(propValue);
		iPAddressProperty.SetRetrieved(retrieved: true);
		return iPAddressProperty;
	}

	protected override ManagementObject GetPropertyManagementObject(ProtocolProperty pp)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		try
		{
			ManagementScope managementScope = GetManagedComputer().ManagementScope;
			ServerProtocol serverProtocol = (ServerProtocol)base.ParentColl.ParentInstance;
			ManagementPath val = new ManagementPath(string.Format(SmoApplication.DefaultCulture, "ServerNetworkProtocolProperty.InstanceName=\"{0}\",IPAddressName=\"{1}\",PropertyName=\"{2}\",PropertyType={3},ProtocolName=\"{4}\"", Urn.EscapeString(serverProtocol.Parent.Name), Urn.EscapeString(base.Name), Urn.EscapeString(pp.Name), pp.PropertyType, Urn.EscapeString(serverProtocol.Name)));
			return new ManagementObject(managementScope, val, new ObjectGetOptions());
		}
		catch (Exception innerException)
		{
			throw new ServiceRequestException(ExceptionTemplatesImpl.InnerWmiException, innerException);
		}
	}

	internal void AlterImplWorker()
	{
		try
		{
			AlterProtocolProperties(IPAddressProperties);
		}
		catch (Exception ex)
		{
			SqlSmoObject.FilterException(ex);
			throw new FailedOperationException(ExceptionTemplatesImpl.Alter, this, ex);
		}
	}
}
