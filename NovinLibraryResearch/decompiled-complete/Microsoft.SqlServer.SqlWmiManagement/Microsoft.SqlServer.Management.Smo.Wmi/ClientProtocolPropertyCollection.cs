namespace Microsoft.SqlServer.Management.Smo.Wmi;

public sealed class ClientProtocolPropertyCollection : ProtocolPropertyCollection
{
	internal ClientProtocolPropertyCollection()
	{
	}

	internal override void Add(ProtocolProperty property)
	{
		long num = (long)(((ulong)((ClientProtocolProperty)property).Index << 32) + property.PropertyType);
		sl.Add(num, property);
	}
}
