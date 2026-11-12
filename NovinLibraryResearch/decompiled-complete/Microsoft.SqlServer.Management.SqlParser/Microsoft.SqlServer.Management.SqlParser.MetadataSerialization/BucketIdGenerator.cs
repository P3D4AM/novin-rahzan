namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class BucketIdGenerator
{
	private int currentId;

	public int CurrentId => currentId;

	public BucketIdGenerator()
	{
		currentId = 1;
	}

	public int GetNextId()
	{
		return currentId++;
	}
}
