namespace Microsoft.SqlServer.Management.Smo.Wmi;

public class ProtocolProperty : Property
{
	private uint propertyType;

	internal uint PropertyType
	{
		get
		{
			return propertyType;
		}
		set
		{
			propertyType = value;
		}
	}

	internal ProtocolProperty(Property p)
		: base(p)
	{
	}
}
