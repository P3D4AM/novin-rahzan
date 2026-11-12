#define TRACE
using System;
using System.Data;
using System.Diagnostics;
using System.Management;
using System.Reflection;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

public abstract class WmiSmoObject : SmoObjectBase
{
	private WmiCollectionBase parentColl;

	private ManagedComputer m_ManagedComputer;

	private string name;

	internal PropertyCollection m_properties;

	private WMIProxy proxy;

	internal WmiCollectionBase ParentColl => parentColl;

	public virtual Urn Urn
	{
		get
		{
			string text = GetType().InvokeMember("UrnSuffix", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetProperty, null, null, new object[0], SmoApplication.DefaultCulture) as string;
			if (text.Length == 0)
			{
				return new Urn(string.Empty);
			}
			Urn urn = string.Empty;
			if (ParentColl != null)
			{
				urn = ParentColl.ParentInstance.Urn;
			}
			if (urn.ToString().Length != 0)
			{
				return new Urn(string.Format(SmoApplication.DefaultCulture, "{0}/{1}[@Name='{2}']", urn, text, Urn.EscapeString(Name)));
			}
			return new Urn(string.Format(SmoApplication.DefaultCulture, "{0}[@Name='{1}']", text, Urn.EscapeString(Name)));
		}
	}

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			if (value == null)
			{
				throw new SmoException(ExceptionTemplatesImpl.InnerException, new ArgumentNullException("Name"));
			}
			if (base.State != 0)
			{
				throw new InvalidSmoOperationException(ExceptionTemplatesImpl.OperationOnlyInPendingState);
			}
			name = value;
			UpdateObjectState();
		}
	}

	public PropertyCollection Properties
	{
		get
		{
			if (m_properties == null)
			{
				m_properties = new PropertyCollection(this, GetPropertyMetadataProvider());
			}
			return m_properties;
		}
	}

	internal WMIProxy Proxy
	{
		get
		{
			if (proxy == null)
			{
				if (this is ManagedComputer)
				{
					proxy = new WMIProxy(GetManagedComputer().ManagementScope);
				}
				else
				{
					proxy = GetManagedComputer().Proxy;
				}
			}
			return proxy;
		}
	}

	protected internal WmiSmoObject(WmiCollectionBase parentColl, string name)
	{
		SetName(name);
		this.parentColl = parentColl;
		UpdateObjectState();
		Init();
	}

	protected internal WmiSmoObject(string name)
	{
		SetName(name);
		Init();
	}

	protected internal WmiSmoObject()
	{
		Init();
	}

	internal virtual PropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return null;
	}

	private void Init()
	{
		propertyBagState = PropertyBagState.Empty;
		m_properties = null;
	}

	internal ManagedComputer GetManagedComputer()
	{
		if (m_ManagedComputer == null)
		{
			WmiSmoObject wmiSmoObject = this;
			while (wmiSmoObject.ParentColl != null && !typeof(ManagedComputer).Equals(wmiSmoObject.GetType()))
			{
				wmiSmoObject = wmiSmoObject.ParentColl.ParentInstance;
			}
			if (wmiSmoObject.ParentColl == null && !typeof(ManagedComputer).Equals(wmiSmoObject.GetType()))
			{
				throw new InternalSmoErrorException(ExceptionTemplatesImpl.ObjectNotUnderServer(GetType().ToString()));
			}
			m_ManagedComputer = wmiSmoObject as ManagedComputer;
		}
		return m_ManagedComputer;
	}

	protected internal void SetParentImpl(WmiSmoObject newParent)
	{
		if (newParent == null)
		{
			throw new SmoException(ExceptionTemplatesImpl.InnerException, new ArgumentNullException("newParent"));
		}
		if (base.State != 0)
		{
			throw new InvalidSmoOperationException(ExceptionTemplatesImpl.OperationOnlyInPendingState);
		}
		if (newParent.State == SqlSmoState.Pending)
		{
			throw new InvalidSmoOperationException();
		}
		if (!(newParent is ManagedComputer))
		{
			throw new FailedOperationException(ExceptionTemplatesImpl.InvalidType(newParent.GetType().ToString())).SetHelpContext("InvalidType");
		}
		parentColl = null;
		if (this is ServerAlias)
		{
			parentColl = ((ManagedComputer)newParent).ServerAliases;
		}
		UpdateObjectState();
	}

	protected void UpdateObjectState()
	{
		if (base.State == SqlSmoState.Pending && name != null && parentColl != null)
		{
			SetState(SqlSmoState.Creating);
		}
	}

	protected internal void SetName(string name)
	{
		this.name = name;
		UpdateObjectState();
	}

	internal override object OnPropertyMissing(string propname, bool useDefaultValue)
	{
		switch (propertyBagState)
		{
		case PropertyBagState.Empty:
			Initialize();
			break;
		case PropertyBagState.Lazy:
			Initialize();
			break;
		case PropertyBagState.Full:
			throw new InternalSmoErrorException(ExceptionTemplatesImpl.FullPropertyBag(propname));
		}
		return Properties.Get(propname).Value;
	}

	public bool Initialize()
	{
		if (propertyBagState == PropertyBagState.Full)
		{
			return false;
		}
		string[] fields = null;
		OrderBy[] orderby = null;
		bool num = ImplInitialize(fields, orderby);
		if (num)
		{
			propertyBagState = PropertyBagState.Full;
		}
		return num;
	}

	protected virtual bool ImplInitialize(string[] fields, OrderBy[] orderby)
	{
		Urn urn = Urn;
		Request request = new Request();
		request.Urn = urn;
		request.Fields = fields;
		request.OrderByList = orderby;
		DataTable dataTable = Proxy.ProcessRequest(request).Tables[0];
		if (dataTable.Rows.Count < 1)
		{
			Trace("Failed to Initialize urn " + urn);
			return false;
		}
		AddObjectProps(dataTable.Columns, dataTable.Rows[0]);
		return true;
	}

	internal void AddObjectProps(DataColumnCollection columns, DataRow dr)
	{
		int num = 0;
		for (int i = 0; i < columns.Count; i++)
		{
			DataColumn dataColumn = columns[i];
			if (1 > num && string.Compare(dataColumn.ColumnName, "Name", StringComparison.Ordinal) == 0)
			{
				num++;
				continue;
			}
			Property property = Properties.Get(dataColumn.ColumnName);
			if (property == null)
			{
				throw new InternalSmoErrorException(ExceptionTemplatesImpl.UnknownProperty(dataColumn.ColumnName, GetType().ToString()));
			}
			if (property.Enumeration)
			{
				property.SetValue(Enum.ToObject(property.Type, Convert.ToInt32(dr[i], SmoApplication.DefaultCulture)));
			}
			else if (DBNull.Value.Equals(dr[i]))
			{
				if (property.Type.Equals(typeof(string)))
				{
					property.SetValue(string.Empty);
				}
				else
				{
					property.SetValue(null);
				}
			}
			else
			{
				property.SetValue(dr[i]);
			}
			property.SetRetrieved(retrieved: true);
		}
	}

	protected internal bool IsObjectInitialized()
	{
		if ((base.State != SqlSmoState.Existing && base.State != SqlSmoState.ToBeDropped) || propertyBagState == PropertyBagState.Full)
		{
			return true;
		}
		return false;
	}

	public virtual void Refresh()
	{
		m_properties = null;
		propertyBagState = PropertyBagState.Empty;
		Initialize();
	}

	protected void CheckObjectState()
	{
		if (base.State == SqlSmoState.Dropped)
		{
			throw new SmoException(ExceptionTemplatesImpl.ObjectDroppedExceptionText(GetType().ToString(), Name));
		}
		if (base.State == SqlSmoState.Pending)
		{
			throw new InvalidSmoOperationException("", SqlSmoState.Pending);
		}
	}

	internal void EnumChildren(string childTypeName, WmiCollectionBase coll)
	{
		Type type = Type.GetType(childTypeName, throwOnError: true);
		Request request = new Request();
		request.Urn = string.Concat(Urn, "/", type.InvokeMember("UrnSuffix", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetProperty, null, null, new object[0], SmoApplication.DefaultCulture)?.ToString());
		request.Fields = null;
		request.OrderByList = new OrderBy[1]
		{
			new OrderBy("Name", OrderBy.Direction.Asc)
		};
		DataTable dataTable = Proxy.ProcessRequest(request).Tables[0];
		foreach (DataRow row in dataTable.Rows)
		{
			object[] args = new object[2]
			{
				coll,
				(string)row["Name"]
			};
			object obj = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, args, null);
			if (obj == null)
			{
				throw new InternalSmoErrorException(ExceptionTemplatesImpl.CantCreateType(childTypeName));
			}
			if (dataTable.Columns.Count > 1)
			{
				((WmiSmoObject)obj).AddObjectProps(dataTable.Columns, row);
			}
			((WmiSmoObject)obj).SetState(PropertyBagState.Empty);
			((WmiSmoObject)obj).SetState(SqlSmoState.Existing);
			coll.AddInternal((WmiSmoObject)obj);
		}
	}

	internal static void Trace(string traceText)
	{
		System.Diagnostics.Trace.TraceInformation(traceText);
	}

	protected void InvokeMgmtMethod(ManagementObject mo, string methodName, object[] parameters)
	{
		InvokeMgmtMethod(mo, null, methodName, parameters);
	}

	protected void InvokeMgmtMethod(ManagementObject mo, ManagementOperationObserver observer, string methodName, object[] parameters)
	{
		try
		{
			if (observer == null)
			{
				uint num = Convert.ToUInt32(mo.InvokeMethod(methodName, parameters), SmoApplication.DefaultCulture);
				if (num != 0)
				{
					if (this is Service && 0 < num && num <= 24)
					{
						throw new ServiceRequestException(num);
					}
					if (1 == (num & 0x80000000u))
					{
						throw new ServiceRequestException(num);
					}
				}
			}
			else
			{
				mo.InvokeMethod(observer, methodName, parameters);
			}
		}
		catch (Exception ex)
		{
			SqlSmoObject.FilterException(ex);
			throw new FailedOperationException(ExceptionTemplatesImpl.FailedOperationExceptionText(methodName, GetType().Name, Name), ex);
		}
	}

	protected virtual ProtocolPropertyCollection CreateProtocolPropertyCollection()
	{
		return null;
	}

	protected virtual ProtocolProperty GetPropertyObject(PropertyCollection properties, DataRow dr, object propValue)
	{
		return null;
	}

	protected ProtocolPropertyCollection GetProtocolPropertyCollection()
	{
		ProtocolPropertyCollection protocolPropertyCollection = CreateProtocolPropertyCollection();
		DynamicPropertyMetadataProvider dynamicPropertyMetadataProvider = new DynamicPropertyMetadataProvider();
		Urn urn = string.Concat(Urn, "/Property");
		DataSet dataSet = Proxy.ProcessRequest(new Request(urn));
		foreach (DataRow row in dataSet.Tables[0].Rows)
		{
			string typeName;
			if ((uint)row["Type"] == 0)
			{
				typeName = "System.Boolean";
			}
			else
			{
				switch ((uint)row["ValType"])
				{
				case 1u:
				case 2u:
				case 7u:
					typeName = "System.String";
					break;
				case 4u:
				case 5u:
					typeName = "System.Int32";
					break;
				case 11u:
					typeName = "System.Int64";
					break;
				default:
					typeName = "System.String";
					break;
				}
			}
			bool readOnly = false;
			if ((string)row["Name"] == "Name")
			{
				readOnly = true;
			}
			dynamicPropertyMetadataProvider.AddMetadata((string)row["Name"], readOnly, Type.GetType(typeName));
		}
		PropertyCollection properties = new PropertyCollection(this, dynamicPropertyMetadataProvider);
		foreach (DataRow row2 in dataSet.Tables[0].Rows)
		{
			object obj = null;
			if ((uint)row2["Type"] == 0)
			{
				try
				{
					obj = Convert.ToBoolean(row2["NumValue"], SmoApplication.DefaultCulture);
				}
				catch (Exception innerException)
				{
					throw new SmoException(ExceptionTemplatesImpl.InnerException, innerException);
				}
			}
			else
			{
				switch ((uint)row2["ValType"])
				{
				case 1u:
				case 2u:
				case 7u:
					obj = row2["StrValue"];
					break;
				case 4u:
				case 5u:
					obj = row2["NumValue"];
					break;
				case 11u:
					obj = row2["NumValue"];
					break;
				default:
					obj = row2["StrValue"];
					break;
				}
			}
			ProtocolProperty propertyObject = GetPropertyObject(properties, row2, obj);
			protocolPropertyCollection.Add(propertyObject);
		}
		return protocolPropertyCollection;
	}

	protected virtual ManagementObject GetPropertyManagementObject(ProtocolProperty pp)
	{
		return null;
	}

	protected void AlterProtocolProperties(ProtocolPropertyCollection protocolProperties)
	{
		foreach (ProtocolProperty protocolProperty in protocolProperties)
		{
			if (protocolProperty.Dirty)
			{
				ManagementObject propertyManagementObject = GetPropertyManagementObject(protocolProperty);
				if (protocolProperty.Type.Equals(typeof(bool)))
				{
					InvokeMgmtMethod(propertyManagementObject, "SetFlag", new object[1] { (bool)protocolProperty.Value });
				}
				else if (protocolProperty.Type.Equals(typeof(string)))
				{
					InvokeMgmtMethod(propertyManagementObject, "SetStringValue", new object[1] { (string)protocolProperty.Value });
				}
				else
				{
					InvokeMgmtMethod(propertyManagementObject, "SetNumericalValue", new object[1] { SmoApplication.ConvertInt32ToUInt32((int)protocolProperty.Value) });
				}
			}
		}
	}
}
