using System.Globalization;
using System.Xml;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class XmlQualifiedNameEx : XmlQualifiedName
{
	private readonly XmlToken nameToken;

	public XmlToken NameToken => nameToken;

	public XmlQualifiedNameEx(string name, string ns)
		: base(name, ns)
	{
		nameToken = XmlTokenLookup.Find(base.Name);
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}:{1}", base.Namespace, base.Name);
	}
}
