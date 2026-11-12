namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class CollationLookup : CollationLookupBase
{
	private static class Singleton
	{
		public static CollationLookup Instance;

		static Singleton()
		{
			Instance = new CollationLookup();
		}
	}

	public static CollationLookup Instance => Singleton.Instance;

	private CollationLookup()
	{
	}
}
