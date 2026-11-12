using System;
using System.Management;
using Microsoft.SqlServer.Management.Common;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

public sealed class ServerAlias : WmiSmoObject, ICreatable, IDroppable
{
	private class PropertyMetadataProvider : Microsoft.SqlServer.Management.Smo.PropertyMetadataProvider
	{
		private static StaticMetadata[] staticMetadata = new StaticMetadata[3]
		{
			new StaticMetadata("ConnectionString", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("ProtocolName", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("ServerName", expensive: false, readOnly: false, typeof(string))
		};

		public override int Count => 3;

		public override int PropertyNameToIDLookup(string propertyName)
		{
			return propertyName switch
			{
				"ConnectionString" => 0, 
				"ProtocolName" => 1, 
				"ServerName" => 2, 
				_ => -1, 
			};
		}

		public override StaticMetadata GetStaticMetadata(int id)
		{
			return staticMetadata[id];
		}
	}

	public ManagedComputer Parent
	{
		get
		{
			CheckObjectState();
			return base.ParentColl.ParentInstance as ManagedComputer;
		}
		set
		{
			SetParentImpl(value);
		}
	}

	public string ConnectionString
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("ConnectionString");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("ConnectionString", value);
		}
	}

	public string ProtocolName
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("ProtocolName");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("ProtocolName", value);
		}
	}

	public string ServerName
	{
		get
		{
			return (string)base.Properties.GetValueWithNullReplacement("ServerName");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("ServerName", value);
		}
	}

	internal static string UrnSuffix => "ServerAlias";

	internal override Microsoft.SqlServer.Management.Smo.PropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return new PropertyMetadataProvider();
	}

	internal ServerAlias(WmiCollectionBase parentColl, string name)
		: base(parentColl, name)
	{
	}

	public ServerAlias(ManagedComputer managedComputer, string name)
	{
		Parent = managedComputer;
		SetName(name);
	}

	public ServerAlias()
	{
	}

	public void Create()
	{
		try
		{
			CreateImplWorker();
			if (!SmoApplication.eventsSingleton.IsNullObjectCreated())
			{
				SmoApplication.eventsSingleton.CallObjectCreated(GetManagedComputer(), new ObjectCreatedEventArgs(Urn, this));
			}
		}
		catch (Exception ex)
		{
			SqlSmoObject.FilterException(ex);
			throw new FailedOperationException(ExceptionTemplatesImpl.Create, this, ex);
		}
	}

	private void CreateImplWorker()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0044: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		CheckObjectState();
		if (base.State != SqlSmoState.Creating)
		{
			throw new SmoException(ExceptionTemplatesImpl.ObjectAlreadyExists(UrnSuffix, base.Name));
		}
		ManagementObject val = new ManagementClass(GetManagedComputer().ManagementScope, new ManagementPath("SqlServerAlias"), new ObjectGetOptions()).CreateInstance();
		((ManagementBaseObject)val)["AliasName"] = base.Name;
		SetMOProp(val, "ServerName", "ServerName");
		SetMOProp(val, "ProtocolName", "ProtocolName");
		SetMOProp(val, "ConnectionString", "ConnectionString");
		val.Put();
		SetState(SqlSmoState.Existing);
		base.ParentColl.AddInternal(this);
	}

	private void SetMOProp(ManagementObject mo, string moPropName, string propName)
	{
		Property property = base.Properties.Get(propName);
		if (property.Value != null)
		{
			((ManagementBaseObject)mo)[moPropName] = property.Value;
			return;
		}
		throw new PropertyNotSetException(propName);
	}

	public void Drop()
	{
		try
		{
			DropImplWorker();
			if (!SmoApplication.eventsSingleton.IsNullObjectDropped())
			{
				SmoApplication.eventsSingleton.CallObjectDropped(GetManagedComputer(), new ObjectDroppedEventArgs(Urn));
			}
		}
		catch (Exception ex)
		{
			SqlSmoObject.FilterException(ex);
			throw new FailedOperationException(ExceptionTemplatesImpl.Drop, this, ex);
		}
	}

	private void DropImplWorker()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		CheckObjectState();
		ManagementScope managementScope = GetManagedComputer().ManagementScope;
		ManagementPath val = new ManagementPath(string.Format(SmoApplication.DefaultCulture, "SqlServerAlias.AliasName=\"{0}\"", base.Name));
		new ManagementObject(managementScope, val, new ObjectGetOptions()).Delete();
		SetState(SqlSmoState.Dropped);
		base.ParentColl.RemoveInternal(base.Name);
	}
}
