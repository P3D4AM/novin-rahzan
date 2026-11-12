namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class BuiltInFunctionLookup : BuiltInFunctionLookupBase
{
	private static class Singleton
	{
		public static BuiltInFunctionLookup Instance;

		static Singleton()
		{
			Instance = new BuiltInFunctionLookup();
		}
	}

	public static BuiltInFunctionLookup Instance => Singleton.Instance;

	private BuiltInFunctionLookup()
	{
	}
}
