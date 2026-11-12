namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class XmlNamespace
{
	private readonly string prefix;

	private readonly string urn;

	public string Prefix => prefix;

	public string Urn => urn;

	public XmlNamespace(string prefix, string urn)
	{
		this.prefix = prefix;
		this.urn = urn;
	}
}
