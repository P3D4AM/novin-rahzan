using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal struct MultipartObjectName
{
	private readonly int identifierCount;

	private readonly SqlMultipartIdentifier multipartIdentifier;

	public string ServerName => GetNameAtPosition(4);

	public string DatabaseName => GetNameAtPosition(3);

	public string SchemaName => GetNameAtPosition(2);

	public string ObjectName => GetNameAtPosition(1);

	public MultipartObjectName(SqlMultipartIdentifier multipartIdentifier)
	{
		this.multipartIdentifier = multipartIdentifier;
		identifierCount = multipartIdentifier.Count;
	}

	public MultipartObjectName(SqlMultipartIdentifier multipartIdentifier, int count)
	{
		this.multipartIdentifier = multipartIdentifier;
		identifierCount = count;
	}

	private string GetNameAtPosition(int position)
	{
		string result = null;
		if (position <= identifierCount)
		{
			result = multipartIdentifier[identifierCount - position].Value;
		}
		return result;
	}
}
