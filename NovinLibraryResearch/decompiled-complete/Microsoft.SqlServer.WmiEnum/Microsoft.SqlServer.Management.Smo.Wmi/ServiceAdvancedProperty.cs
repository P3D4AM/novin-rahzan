namespace Microsoft.SqlServer.Management.Smo.Wmi;

internal class ServiceAdvancedProperty : WmiBase
{
	public ServiceAdvancedProperty()
		: base("SqlServiceAdvancedProperty")
	{
		WmiProperty wmiProperty = null;
		AddProperty(new WmiProperty("Name", "System.String")
		{
			PhysicalName = "PropertyName"
		});
		AddProperty(new WmiProperty("ServiceType", "Microsoft.SqlServer.Management.Smo.Wmi.ManagedServiceType")
		{
			BaseType = "System.Int32",
			PhysicalName = "SqlServiceType",
			ReadOnly = true
		});
		AddProperty(new WmiProperty("ServiceName", "System.String")
		{
			PhysicalName = "ServiceName",
			ReadOnly = true
		});
		AddProperty(new WmiProperty("StringValue", "System.String")
		{
			PhysicalName = "PropertyStrValue"
		});
		AddProperty(new WmiProperty("NumericValue", "System.Int64")
		{
			PhysicalName = "PropertyNumValue"
		});
		AddProperty(new WmiProperty("ValueType", "Microsoft.SqlServer.Management.Smo.Wmi.PropertyType")
		{
			PhysicalName = "PropertyValueType",
			BaseType = "System.UInt32",
			ReadOnly = true
		});
		AddProperty(new WmiProperty("Idx", "System.UInt32")
		{
			PhysicalName = "PropertyIndex",
			ReadOnly = true
		});
		AddProperty(new WmiProperty("IsReadOnly", "System.Boolean")
		{
			PhysicalName = "IsReadOnly",
			ReadOnly = true
		});
	}
}
