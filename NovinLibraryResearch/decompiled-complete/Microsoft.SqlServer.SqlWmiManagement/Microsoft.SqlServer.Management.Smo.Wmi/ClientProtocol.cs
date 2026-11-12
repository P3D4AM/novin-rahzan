using System;
using System.Data;
using System.Management;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

public sealed class ClientProtocol : ProtocolBase
{
	private class PropertyMetadataProvider : Microsoft.SqlServer.Management.Smo.PropertyMetadataProvider
	{
		private static StaticMetadata[] staticMetadata = new StaticMetadata[4]
		{
			new StaticMetadata("DisplayName", expensive: false, readOnly: true, typeof(string)),
			new StaticMetadata("IsEnabled", expensive: false, readOnly: false, typeof(bool)),
			new StaticMetadata("NetworkLibrary", expensive: false, readOnly: true, typeof(string)),
			new StaticMetadata("Order", expensive: false, readOnly: false, typeof(int))
		};

		public override int Count => 4;

		public override int PropertyNameToIDLookup(string propertyName)
		{
			return propertyName switch
			{
				"DisplayName" => 0, 
				"IsEnabled" => 1, 
				"NetworkLibrary" => 2, 
				"Order" => 3, 
				_ => -1, 
			};
		}

		public override StaticMetadata GetStaticMetadata(int id)
		{
			return staticMetadata[id];
		}
	}

	private NetLibInfo netLibInfo;

	public ManagedComputer Parent
	{
		get
		{
			CheckObjectState();
			return base.ParentColl.ParentInstance as ManagedComputer;
		}
	}

	public string DisplayName => (string)base.Properties.GetValueWithNullReplacement("DisplayName");

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

	public string NetworkLibrary => (string)base.Properties.GetValueWithNullReplacement("NetworkLibrary");

	public int Order
	{
		get
		{
			return (int)base.Properties.GetValueWithNullReplacement("Order");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("Order", value);
		}
	}

	internal static string UrnSuffix => "ClientProtocol";

	public NetLibInfo NetLibInfo
	{
		get
		{
			if (netLibInfo == null)
			{
				RefreshNetLibInfo();
			}
			return netLibInfo;
		}
	}

	internal override Microsoft.SqlServer.Management.Smo.PropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return new PropertyMetadataProvider();
	}

	internal ClientProtocol(WmiCollectionBase parentColl, string name)
		: base(parentColl, name)
	{
	}

	protected override ManagementObject GetManagementObject()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			ManagementScope managementScope = GetManagedComputer().ManagementScope;
			ManagementPath val = new ManagementPath(string.Format(SmoApplication.DefaultCulture, "ClientNetworkProtocol.ProtocolName=\"{0}\"", base.Name));
			return new ManagementObject(managementScope, val, new ObjectGetOptions());
		}
		catch (Exception innerException)
		{
			throw new ServiceRequestException(ExceptionTemplatesImpl.InnerWmiException, innerException);
		}
	}

	protected override ManagementObject GetPropertyManagementObject(ProtocolProperty pp)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			ManagementScope managementScope = GetManagedComputer().ManagementScope;
			ManagementPath val = new ManagementPath(string.Format(SmoApplication.DefaultCulture, "ClientNetworkProtocolProperty.PropertyIdx={0},PropertyType={1},ProtocolName=\"{2}\"", ((ClientProtocolProperty)pp).Index, pp.PropertyType, base.Name));
			return new ManagementObject(managementScope, val, new ObjectGetOptions());
		}
		catch (Exception innerException)
		{
			throw new ServiceRequestException(ExceptionTemplatesImpl.InnerWmiException, innerException);
		}
	}

	protected override ProtocolPropertyCollection CreateProtocolPropertyCollection()
	{
		return new ClientProtocolPropertyCollection();
	}

	protected override ProtocolProperty GetPropertyObject(PropertyCollection properties, DataRow dr, object propValue)
	{
		ClientProtocolProperty clientProtocolProperty = new ClientProtocolProperty(properties.Get((string)dr["Name"]));
		clientProtocolProperty.PropertyType = (uint)dr["Type"];
		clientProtocolProperty.Index = (uint)dr["Idx"];
		clientProtocolProperty.SetValue(propValue);
		clientProtocolProperty.SetRetrieved(retrieved: true);
		return clientProtocolProperty;
	}

	internal bool RefreshNetLibInfo()
	{
		Urn urn = string.Empty;
		urn = string.Format(SmoApplication.DefaultCulture, "{0}/ClientNetLibInfo[@ProtocolName='{1}']", GetManagedComputer().Urn, Urn.EscapeString(base.Name));
		DataSet dataSet = base.Proxy.ProcessRequest(new Request(urn));
		if (dataSet.Tables[0].Rows.Count == 0)
		{
			netLibInfo = null;
			return false;
		}
		netLibInfo = new NetLibInfo();
		DataRow dataRow = dataSet.Tables[0].Rows[0];
		netLibInfo.fileName = (string)dataRow["FileName"];
		netLibInfo.version = (string)dataRow["Version"];
		netLibInfo.date = new DateTime(Convert.ToInt64((uint)dataRow["Date"], SmoApplication.DefaultCulture) << 32);
		TimeSpan timeSpan = new DateTime(1601, 1, 1, 12, 0, 0) - new DateTime(1, 1, 1, 12, 0, 0);
		netLibInfo.date += timeSpan;
		netLibInfo.size = Convert.ToInt32(dataRow["Size"], SmoApplication.DefaultCulture);
		return true;
	}

	public override void Refresh()
	{
		base.Refresh();
		netLibInfo = null;
	}
}
