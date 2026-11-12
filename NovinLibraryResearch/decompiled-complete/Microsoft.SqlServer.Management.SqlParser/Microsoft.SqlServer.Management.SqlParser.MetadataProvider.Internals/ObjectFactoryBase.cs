namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal abstract class ObjectFactoryBase<T> where T : new()
{
	private static class StaticHelper
	{
		public static readonly T Instance;

		static StaticHelper()
		{
			Instance = new T();
		}
	}

	public static T Instance => StaticHelper.Instance;
}
