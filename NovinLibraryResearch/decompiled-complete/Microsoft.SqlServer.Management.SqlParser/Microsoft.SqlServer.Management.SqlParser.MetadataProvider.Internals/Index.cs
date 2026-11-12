namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal abstract class Index : MetadataObjectBase
{
	protected Index(string name)
		: base(name)
	{
	}
}
