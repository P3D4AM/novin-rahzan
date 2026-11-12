namespace Microsoft.SqlServer.Management.Smo.Wmi;

public sealed class ServerInstance : WmiSmoObject
{
	private class PropertyMetadataProvider : Microsoft.SqlServer.Management.Smo.PropertyMetadataProvider
	{
		private static StaticMetadata[] staticMetadata = new StaticMetadata[0];

		public override int Count => 0;

		public override int PropertyNameToIDLookup(string propertyName)
		{
			return -1;
		}

		public override StaticMetadata GetStaticMetadata(int id)
		{
			return staticMetadata[id];
		}
	}

	private ServerProtocolCollection m_ServerProtocols;

	public ManagedComputer Parent
	{
		get
		{
			CheckObjectState();
			return base.ParentColl.ParentInstance as ManagedComputer;
		}
	}

	internal static string UrnSuffix => "ServerInstance";

	public ServerProtocolCollection ServerProtocols
	{
		get
		{
			if (m_ServerProtocols == null)
			{
				m_ServerProtocols = new ServerProtocolCollection(this);
			}
			return m_ServerProtocols;
		}
	}

	internal override Microsoft.SqlServer.Management.Smo.PropertyMetadataProvider GetPropertyMetadataProvider()
	{
		return new PropertyMetadataProvider();
	}

	internal ServerInstance(WmiCollectionBase parentColl, string name)
		: base(parentColl, name)
	{
	}
}
