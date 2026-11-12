namespace Microsoft.SqlServer.Management.Smo.Wmi;

public sealed class ClientProtocolProperty : ProtocolProperty
{
	private uint index;

	internal uint Index
	{
		get
		{
			return index;
		}
		set
		{
			index = value;
		}
	}

	internal ClientProtocolProperty(Property p)
		: base(p)
	{
	}
}
