namespace Microsoft.SqlServer.Management.Smo.Wmi;

internal abstract class ProtocolPropertiesBase : WmiChildObjectBase
{
	public ProtocolPropertiesBase(string className)
		: base(className)
	{
		WmiProperty wmiProperty = null;
		AddProperty(new WmiProperty("Name", "System.String")
		{
			PhysicalName = "PropertyName"
		});
		AddProperty(new WmiProperty("Type", "System.UInt32")
		{
			PhysicalName = "PropertyType"
		});
		AddProperty(new WmiProperty("ValType", "Microsoft.SqlServer.Management.Smo.Wmi.PropertyType")
		{
			PhysicalName = "PropertyValType",
			BaseType = "System.UInt32"
		});
		AddProperty(new WmiProperty("StrValue", "System.String")
		{
			PhysicalName = "PropertyStrVal"
		});
		AddProperty(new WmiProperty("NumValue", "System.Int32")
		{
			PhysicalName = "PropertyNumVal"
		});
		wmiProperty = new WmiProperty("ProtocolName", "System.String");
		AddProperty(wmiProperty);
	}
}
