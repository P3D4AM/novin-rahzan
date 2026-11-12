namespace Microsoft.SqlServer.Management.Sdk.Sfc;

internal class SfcSimpleNodeAdapter : SimpleNodeAdapter
{
	public override bool IsSupported(object node)
	{
		if (node is SfcInstance)
		{
			return true;
		}
		return false;
	}

	public override Urn GetUrn(object reference)
	{
		return (reference as SfcInstance).Urn;
	}

	public override object GetProperty(object instance, string propertyName)
	{
		return ((SfcInstance)instance).Properties[propertyName].Value;
	}
}
