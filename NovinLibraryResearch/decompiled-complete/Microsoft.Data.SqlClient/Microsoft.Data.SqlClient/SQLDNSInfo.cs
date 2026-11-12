namespace Microsoft.Data.SqlClient;

internal sealed class SQLDNSInfo
{
	public string FQDN { get; set; }

	public string AddrIPv4 { get; set; }

	public string AddrIPv6 { get; set; }

	public string Port { get; set; }

	internal SQLDNSInfo(string FQDN, string ipv4, string ipv6, string port)
	{
		this.FQDN = FQDN;
		AddrIPv4 = ipv4;
		AddrIPv6 = ipv6;
		Port = port;
	}
}
