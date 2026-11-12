namespace Microsoft.SqlServer.Management.Smo.Wmi;

public sealed class IPAddressPropertyCollection : ProtocolPropertyCollection
{
	internal IPAddressPropertyCollection()
	{
	}

	internal override void Add(ProtocolProperty property)
	{
		sl.Add(property.Name, property);
	}
}
