using System;
using System.Data;
using System.Management;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

public sealed class ServerProtocol : ProtocolBase
{
	private class PropertyMetadataProvider : Microsoft.SqlServer.Management.Smo.PropertyMetadataProvider
	{
		private static StaticMetadata[] staticMetadata = new StaticMetadata[3]
		{
			new StaticMetadata("DisplayName", expensive: false, readOnly: true, typeof(string)),
			new StaticMetadata("HasMultiIPAddresses", expensive: false, readOnly: true, typeof(bool)),
			new StaticMetadata("IsEnabled", expensive: false, readOnly: false, typeof(bool))
		};

		public override int Count => 3;

		public override int PropertyNameToIDLookup(string propertyName)
		{
			return propertyName switch
			{
				"DisplayName" => 0, 
				"HasMultiIPAddresses" => 1, 
				"IsEnabled" => 2, 
				_ => -1, 
			};
		}

		public override StaticMetadata GetStaticMetadata(int id)
		{
			return staticMetadata[id];
		}
	}

	private ServerIPAddressCollection ipAddressCollection;

	public ServerInstance Parent
	{
		get
		{
			CheckObjectState();
			return base.ParentColl.ParentInstance as ServerInstance;
		}
	}

	public string DisplayName => (string)base.Properties.GetValueWithNullReplacement("DisplayName");

	public bool HasMultiIPAddresses => (bool)base.Properties.GetValueWithNullReplacement("HasMultiIPAddresses");

	public bool IsEnabled
	{
		get
		{
			return (bool)base.Properties.GetValueWithNullReplacement("IsEnabled");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("IsEnabled", value);
		}
	}

	internal static string UrnSuffix => "ServerProtocol";

	public ServerIPAddressCollection IPAddresses
	{
		get
		{
			if (ipAddressCollection == null)
			{
				ipAddressCollection = new ServerIPAddressCollection(this);
			}
			return ipAddressCollection;
		}
	}

	internal override Microsoft.SqlServer.Management.Smo.PropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return new PropertyMetadataProvider();
	}

	internal ServerProtocol(WmiCollectionBase parentColl, string name)
		: base(parentColl, name)
	{
	}

	protected override ManagementObject GetManagementObject()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			ManagementScope managementScope = GetManagedComputer().ManagementScope;
			ManagementPath val = new ManagementPath(string.Format(SmoApplication.DefaultCulture, "ServerNetworkProtocol.InstanceName=\"{0}\",ProtocolName=\"{1}\"", Parent.Name, base.Name));
			return new ManagementObject(managementScope, val, new ObjectGetOptions());
		}
		catch (Exception innerException)
		{
			throw new ServiceRequestException(ExceptionTemplatesImpl.InnerWmiException, innerException);
		}
	}

	protected override ManagementObject GetPropertyManagementObject(ProtocolProperty pp)
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		try
		{
			ManagementScope managementScope = GetManagedComputer().ManagementScope;
			ServerInstance serverInstance = (ServerInstance)base.ParentColl.ParentInstance;
			ManagementPath val = new ManagementPath(string.Format(SmoApplication.DefaultCulture, "ServerNetworkProtocolProperty.InstanceName=\"{0}\",IPAddressName=\"\",PropertyName=\"{1}\",PropertyType={2},ProtocolName=\"{3}\"", Urn.EscapeString(serverInstance.Name), Urn.EscapeString(pp.Name), pp.PropertyType, Urn.EscapeString(base.Name)));
			return new ManagementObject(managementScope, val, new ObjectGetOptions());
		}
		catch (Exception innerException)
		{
			throw new ServiceRequestException(ExceptionTemplatesImpl.InnerWmiException, innerException);
		}
	}

	protected override ProtocolPropertyCollection CreateProtocolPropertyCollection()
	{
		return new ServerProtocolPropertyCollection();
	}

	protected override ProtocolProperty GetPropertyObject(PropertyCollection properties, DataRow dr, object propValue)
	{
		ServerProtocolProperty serverProtocolProperty = new ServerProtocolProperty(properties.Get((string)dr["Name"]));
		serverProtocolProperty.PropertyType = (uint)dr["Type"];
		serverProtocolProperty.SetValue(propValue);
		serverProtocolProperty.SetRetrieved(retrieved: true);
		return serverProtocolProperty;
	}

	internal override void AlterImplWorker()
	{
		foreach (ServerIPAddress iPAddress in IPAddresses)
		{
			iPAddress.AlterImplWorker();
		}
		base.AlterImplWorker();
	}
}
