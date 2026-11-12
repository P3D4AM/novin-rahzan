namespace Microsoft.SqlServer.Management.Smo.Wmi;

public sealed class ServerProtocolPropertyCollection : ProtocolPropertyCollection
{
	internal ServerProtocolPropertyCollection()
	{
	}

	internal override void Add(ProtocolProperty property)
	{
		sl.Add(property.Name, property);
	}
}
