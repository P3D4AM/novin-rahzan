using System;
using System.Collections.Specialized;
using System.Data;
using System.Management;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

public sealed class Service : WmiSmoObject, IAlterable
{
	private class PropertyMetadataProvider : Microsoft.SqlServer.Management.Smo.PropertyMetadataProvider
	{
		private static StaticMetadata[] staticMetadata = new StaticMetadata[13]
		{
			new StaticMetadata("AcceptsPause", expensive: false, readOnly: true, typeof(bool)),
			new StaticMetadata("AcceptsStop", expensive: false, readOnly: true, typeof(bool)),
			new StaticMetadata("Dependencies", expensive: false, readOnly: true, typeof(string)),
			new StaticMetadata("Description", expensive: false, readOnly: true, typeof(string)),
			new StaticMetadata("DisplayName", expensive: false, readOnly: true, typeof(string)),
			new StaticMetadata("ErrorControl", expensive: false, readOnly: true, typeof(ServiceErrorControl)),
			new StaticMetadata("ExitCode", expensive: false, readOnly: true, typeof(int)),
			new StaticMetadata("PathName", expensive: false, readOnly: true, typeof(string)),
			new StaticMetadata("ProcessId", expensive: false, readOnly: true, typeof(int)),
			new StaticMetadata("ServiceAccount", expensive: false, readOnly: true, typeof(string)),
			new StaticMetadata("ServiceState", expensive: false, readOnly: true, typeof(ServiceState)),
			new StaticMetadata("StartMode", expensive: false, readOnly: false, typeof(ServiceStartMode)),
			new StaticMetadata("Type", expensive: false, readOnly: true, typeof(ManagedServiceType))
		};

		public override int Count => 13;

		public override int PropertyNameToIDLookup(string propertyName)
		{
			return propertyName switch
			{
				"AcceptsPause" => 0, 
				"AcceptsStop" => 1, 
				"Dependencies" => 2, 
				"Description" => 3, 
				"DisplayName" => 4, 
				"ErrorControl" => 5, 
				"ExitCode" => 6, 
				"PathName" => 7, 
				"ProcessId" => 8, 
				"ServiceAccount" => 9, 
				"ServiceState" => 10, 
				"StartMode" => 11, 
				"Type" => 12, 
				_ => -1, 
			};
		}

		public override StaticMetadata GetStaticMetadata(int id)
		{
			return staticMetadata[id];
		}
	}

	private class PropertyMetadataProviderAdvanced : Microsoft.SqlServer.Management.Smo.PropertyMetadataProvider
	{
		private static StaticMetadata[] staticMetadata = new StaticMetadata[8]
		{
			new StaticMetadata("Idx", expensive: false, readOnly: true, typeof(uint)),
			new StaticMetadata("IsReadOnly", expensive: false, readOnly: true, typeof(bool)),
			new StaticMetadata("Name", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("NumericValue", expensive: false, readOnly: false, typeof(long)),
			new StaticMetadata("ServiceName", expensive: false, readOnly: true, typeof(string)),
			new StaticMetadata("ServiceType", expensive: false, readOnly: true, typeof(ManagedServiceType)),
			new StaticMetadata("StringValue", expensive: false, readOnly: false, typeof(string)),
			new StaticMetadata("ValueType", expensive: false, readOnly: true, typeof(PropertyType))
		};

		public override int Count => 8;

		public override int PropertyNameToIDLookup(string propertyName)
		{
			return propertyName switch
			{
				"Idx" => 0, 
				"IsReadOnly" => 1, 
				"Name" => 2, 
				"NumericValue" => 3, 
				"ServiceName" => 4, 
				"ServiceType" => 5, 
				"StringValue" => 6, 
				"ValueType" => 7, 
				_ => -1, 
			};
		}

		public override StaticMetadata GetStaticMetadata(int id)
		{
			return staticMetadata[id];
		}
	}

	[CompilerGenerated]
	private CompletedEventHandler m_ManagementStateChange;

	internal StringCollection dependencies;

	internal PropertyCollection advancedProperties;

	public ManagedComputer Parent
	{
		get
		{
			CheckObjectState();
			return base.ParentColl.ParentInstance as ManagedComputer;
		}
	}

	public bool AcceptsPause => (bool)base.Properties.GetValueWithNullReplacement("AcceptsPause");

	public bool AcceptsStop => (bool)base.Properties.GetValueWithNullReplacement("AcceptsStop");

	public string Description => (string)base.Properties.GetValueWithNullReplacement("Description");

	public string DisplayName => (string)base.Properties.GetValueWithNullReplacement("DisplayName");

	public ServiceErrorControl ErrorControl => (ServiceErrorControl)base.Properties.GetValueWithNullReplacement("ErrorControl");

	public int ExitCode => (int)base.Properties.GetValueWithNullReplacement("ExitCode");

	public string PathName => (string)base.Properties.GetValueWithNullReplacement("PathName");

	public int ProcessId => (int)base.Properties.GetValueWithNullReplacement("ProcessId");

	public string ServiceAccount => (string)base.Properties.GetValueWithNullReplacement("ServiceAccount");

	public ServiceState ServiceState => (ServiceState)base.Properties.GetValueWithNullReplacement("ServiceState");

	public ServiceStartMode StartMode
	{
		get
		{
			return (ServiceStartMode)base.Properties.GetValueWithNullReplacement("StartMode");
		}
		set
		{
			base.Properties.SetValueWithConsistencyCheck("StartMode", value);
		}
	}

	public ManagedServiceType Type => (ManagedServiceType)base.Properties.GetValueWithNullReplacement("Type");

	internal static string UrnSuffix => "Service";

	public bool IsHadrEnabled
	{
		get
		{
			ManagementObject hADRManagementObject = GetHADRManagementObject();
			if (hADRManagementObject != null)
			{
				object propertyValue = ((ManagementBaseObject)hADRManagementObject).GetPropertyValue("HADRServiceEnabled");
				if (propertyValue != null)
				{
					return (bool)propertyValue;
				}
			}
			throw new NotSupportedException("IsHadrEnabled");
		}
	}

	public string StartupParameters
	{
		get
		{
			Property property = AdvancedProperties["STARTUPPARAMETERS"];
			if (null != property)
			{
				return property.Value as string;
			}
			return null;
		}
		set
		{
			Property property = AdvancedProperties["STARTUPPARAMETERS"];
			if (null != property)
			{
				property.Value = value;
				return;
			}
			throw new UnknownPropertyException("StartupParameters");
		}
	}

	public StringCollection Dependencies
	{
		get
		{
			if (dependencies == null)
			{
				try
				{
					dependencies = GetDependencies();
				}
				catch (Exception ex)
				{
					SqlSmoObject.FilterException(ex);
					throw new FailedOperationException(ExceptionTemplatesImpl.FailedOperationExceptionText(ExceptionTemplatesImpl.AdvancedProperties, GetType().Name, base.Name), ex);
				}
			}
			return dependencies;
		}
	}

	public PropertyCollection AdvancedProperties
	{
		get
		{
			if (advancedProperties == null)
			{
				try
				{
					advancedProperties = GetAdvancedProperties();
				}
				catch (Exception ex)
				{
					SqlSmoObject.FilterException(ex);
					throw new FailedOperationException(ExceptionTemplatesImpl.FailedOperationExceptionText(ExceptionTemplatesImpl.AdvancedProperties, GetType().Name, base.Name), ex);
				}
			}
			return advancedProperties;
		}
	}

	public event CompletedEventHandler ManagementStateChange
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			CompletedEventHandler val = this.m_ManagementStateChange;
			CompletedEventHandler val2;
			do
			{
				val2 = val;
				CompletedEventHandler value2 = (CompletedEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_ManagementStateChange, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			CompletedEventHandler val = this.m_ManagementStateChange;
			CompletedEventHandler val2;
			do
			{
				val2 = val;
				CompletedEventHandler value2 = (CompletedEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_ManagementStateChange, value2, val2);
			}
			while (val != val2);
		}
	}

	internal override Microsoft.SqlServer.Management.Smo.PropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return new PropertyMetadataProvider();
	}

	internal Microsoft.SqlServer.Management.Smo.PropertyMetadataProvider GetPropertyMetadataProviderAdvanced()
	{
		return new PropertyMetadataProviderAdvanced();
	}

	internal Service(WmiCollectionBase parentColl, string name)
		: base(parentColl, name)
	{
	}

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

	private void AlterImplWorker()
	{
		ManagementObject managementObject = GetManagementObject();
		Property property = base.Properties.Get("StartMode");
		if (property.Dirty)
		{
			ServiceStartMode serviceStartMode = (ServiceStartMode)property.Value;
			InvokeMgmtMethod(managementObject, "SetStartMode", new object[1] { Convert.ToUInt32(serviceStartMode, SmoApplication.DefaultCulture) });
		}
		if (advancedProperties == null)
		{
			return;
		}
		foreach (Property advancedProperty in advancedProperties)
		{
			if (advancedProperty.Writable && advancedProperty.Dirty)
			{
				ManagementObject propertyManagementObject = GetPropertyManagementObject(advancedProperty.Name);
				if (propertyManagementObject == null)
				{
					throw new InternalSmoErrorException(ExceptionTemplatesImpl.CouldNotFindManagementObject("AdvancedProperty", advancedProperty.Name));
				}
				if (advancedProperty.Type.Equals(typeof(bool)))
				{
					((ManagementBaseObject)propertyManagementObject).SetPropertyValue("PropertyNumValue", (object)(bool)advancedProperty.Value);
					InvokeMgmtMethod(propertyManagementObject, "SetNumericalValue", new object[1] { (bool)advancedProperty.Value });
				}
				else if (advancedProperty.Type.Equals(typeof(long)))
				{
					((ManagementBaseObject)propertyManagementObject).SetPropertyValue("PropertyNumValue", (object)(long)advancedProperty.Value);
					InvokeMgmtMethod(propertyManagementObject, "SetNumericalValue", new object[1] { (long)advancedProperty.Value });
				}
				else
				{
					((ManagementBaseObject)propertyManagementObject).SetPropertyValue("PropertyStrValue", (object)advancedProperty.Value.ToString());
					InvokeMgmtMethod(propertyManagementObject, "SetStringValue", new object[1] { advancedProperty.Value.ToString() });
				}
			}
		}
	}

	private ManagementObject GetManagementObject()
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		try
		{
			ManagementScope managementScope = GetManagedComputer().ManagementScope;
			StringBuilder stringBuilder = new StringBuilder(Globals.INIT_BUFFER_SIZE);
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "SqlService.ServiceName=\"{0}\"", base.Name);
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, ",SQLServiceType={0}", (int)base.Properties["Type"].Value);
			ManagementPath val = new ManagementPath(stringBuilder.ToString());
			return new ManagementObject(managementScope, val, new ObjectGetOptions());
		}
		catch (Exception innerException)
		{
			throw new ServiceRequestException(ExceptionTemplatesImpl.InnerWmiException, innerException);
		}
	}

	private void InvokeServiceMethod(string methodName)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		try
		{
			ManagementOperationObserver val = new ManagementOperationObserver();
			val.Completed += new CompletedEventHandler(OnCompletedMessage);
			InvokeMgmtMethod(GetManagementObject(), val, methodName, null);
		}
		catch (Exception ex)
		{
			SqlSmoObject.FilterException(ex);
			throw new FailedOperationException(ExceptionTemplatesImpl.FailedOperationExceptionText(methodName, ExceptionTemplatesImpl.Service, base.Name), ex);
		}
	}

	public void Start()
	{
		InvokeServiceMethod("StartService");
	}

	public void Stop()
	{
		InvokeServiceMethod("StopService");
	}

	public void Pause()
	{
		InvokeServiceMethod("PauseService");
	}

	public void Resume()
	{
		InvokeServiceMethod("ResumeService");
	}

	public void ChangeHadrServiceSetting(bool enable)
	{
		ManagementObject hADRManagementObject = GetHADRManagementObject();
		if (hADRManagementObject == null)
		{
			throw new NotSupportedException("ChangeHadrServiceSetting");
		}
		InvokeMgmtMethod(hADRManagementObject, "ChangeHADRService", new object[1] { enable ? 1u : 0u });
	}

	private ManagementObject GetHADRManagementObject()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		if ((ManagedServiceType)base.Properties["Type"].Value != ManagedServiceType.SqlServer)
		{
			return null;
		}
		string format = "select * from HADRServiceSettings where InstanceName = '{0}'";
		format = ((!base.Name.StartsWith("MSSQL$", StringComparison.OrdinalIgnoreCase)) ? string.Format(SmoApplication.DefaultCulture, format, base.Name) : string.Format(SmoApplication.DefaultCulture, format, base.Name.Substring("MSSQL$".Length)));
		ManagementObjectEnumerator enumerator = new ManagementObjectSearcher(format)
		{
			Scope = GetManagedComputer().ManagementScope
		}.Get().GetEnumerator();
		try
		{
			if (enumerator.MoveNext())
			{
				return (ManagementObject)enumerator.Current;
			}
		}
		finally
		{
			((IDisposable)enumerator)?.Dispose();
		}
		return null;
	}

	public void SetServiceAccount(string userName, string password)
	{
		try
		{
			if (userName == null)
			{
				throw new ArgumentNullException("userName");
			}
			InvokeMgmtMethod(GetManagementObject(), "SetServiceAccount", new object[2] { userName, password });
			base.Properties.Get("ServiceAccount").SetValue(userName);
		}
		catch (Exception ex)
		{
			SqlSmoObject.FilterException(ex);
			throw new FailedOperationException(ExceptionTemplatesImpl.SetServiceAccount, this, ex);
		}
	}

	public void ChangePassword(string oldPassword, string newPassword)
	{
		try
		{
			InvokeMgmtMethod(GetManagementObject(), "SetServiceAccountPassword", new object[2] { oldPassword, newPassword });
		}
		catch (Exception ex)
		{
			SqlSmoObject.FilterException(ex);
			throw new FailedOperationException(ExceptionTemplatesImpl.ChangeServicePassword, this, ex);
		}
	}

	internal void OnCompletedMessage(object sender, CompletedEventArgs e)
	{
		if (this.ManagementStateChange != null)
		{
			this.ManagementStateChange.Invoke((object)this, e);
		}
	}

	private ManagementObject GetPropertyManagementObject(string propertyName)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		_ = GetManagedComputer().ManagementScope;
		ManagementObjectEnumerator enumerator = new ManagementObjectSearcher("select * from SqlServiceAdvancedProperty")
		{
			Scope = GetManagedComputer().ManagementScope
		}.Get().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ManagementObject val = (ManagementObject)enumerator.Current;
				if (string.Compare((string)((ManagementBaseObject)val).GetPropertyValue("ServiceName"), base.Name, StringComparison.OrdinalIgnoreCase) == 0 && (uint)((ManagementBaseObject)val).GetPropertyValue("SqlServiceType") == (uint)(ManagedServiceType)base.Properties["Type"].Value && (string)((ManagementBaseObject)val).GetPropertyValue("PropertyName") == propertyName)
				{
					return val;
				}
			}
		}
		finally
		{
			((IDisposable)enumerator)?.Dispose();
		}
		return null;
	}

	private StringCollection GetDependencies()
	{
		string text = base.Properties["Dependencies"].Value as string;
		string[] value = text.Split(new char[1] { ';' });
		StringCollection stringCollection = new StringCollection();
		if (text.Length > 0)
		{
			stringCollection.AddRange(value);
		}
		return stringCollection;
	}

	private PropertyCollection GetAdvancedProperties()
	{
		DynamicPropertyMetadataProvider dynamicPropertyMetadataProvider = new DynamicPropertyMetadataProvider();
		Urn urn = base.ParentColl.ParentInstance.Urn;
		urn = string.Concat(base.ParentColl.ParentInstance.Urn, string.Format(SmoApplication.DefaultCulture, "/ServiceAdvancedProperty[@ServiceName='{0}' and @ServiceType={1}]", Urn.EscapeString(base.Name), Enum.Format(typeof(ManagedServiceType), base.Properties["Type"].Value, "d")));
		DataSet dataSet = base.Proxy.ProcessRequest(new Request(urn));
		foreach (DataRow row in dataSet.Tables[0].Rows)
		{
			string typeName;
			switch ((uint)row["ValueType"])
			{
			default:
				typeName = "System.String";
				break;
			case 1u:
				typeName = "System.Boolean";
				break;
			case 2u:
			case 3u:
				typeName = "System.Int64";
				break;
			}
			bool readOnly = true;
			if ((string)row["Name"] != "Name")
			{
				readOnly = (bool)row["IsReadOnly"];
			}
			dynamicPropertyMetadataProvider.AddMetadata((string)row["Name"], readOnly, System.Type.GetType(typeName));
		}
		PropertyCollection propertyCollection = new PropertyCollection(this, dynamicPropertyMetadataProvider);
		foreach (DataRow row2 in dataSet.Tables[0].Rows)
		{
			object obj = null;
			string text;
			switch ((uint)row2["ValueType"])
			{
			default:
				text = "System.String";
				obj = (string)row2["StringValue"];
				break;
			case 1u:
				text = "System.Boolean";
				obj = Convert.ToBoolean(row2["NumericValue"], SmoApplication.DefaultCulture);
				break;
			case 2u:
			case 3u:
				text = "System.Int64";
				obj = Convert.ToInt64(row2["NumericValue"], SmoApplication.DefaultCulture);
				break;
			}
			Property property = propertyCollection.Get((string)row2["Name"]);
			if (obj is DBNull)
			{
				if (text == "System.Boolean")
				{
					property.SetValue(false);
				}
				else if (text == "System.Int64")
				{
					property.SetValue(0);
				}
				else
				{
					property.SetValue(string.Empty);
				}
			}
			else
			{
				property.SetValue(obj);
			}
			property.SetRetrieved(retrieved: true);
		}
		return propertyCollection;
	}

	public override void Refresh()
	{
		base.Refresh();
		advancedProperties = GetAdvancedProperties();
	}
}
