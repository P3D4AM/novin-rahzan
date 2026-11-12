namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlForXmlUnknownDirective : SqlForXmlDirective
{
	private string name;

	public string Name => name;

	public SqlForXmlUnknownDirective(string name, string argument)
		: base(argument)
	{
		this.name = name;
	}
}
