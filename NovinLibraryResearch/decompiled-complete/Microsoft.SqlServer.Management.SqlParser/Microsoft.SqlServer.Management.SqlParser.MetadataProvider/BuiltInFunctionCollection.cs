using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class BuiltInFunctionCollection : DictionaryCollection<IBuiltInFunction>
{
	private static class PopulatedCollection
	{
		public static readonly BuiltInFunctionCollection Instance;

		static PopulatedCollection()
		{
			Instance = new BuiltInFunctionCollection();
			BuiltInFunction.PopulateCollection(Instance);
		}
	}

	internal static IMetadataCollection<IBuiltInFunction> PopulatedInstance => PopulatedCollection.Instance;

	public BuiltInFunctionCollection()
		: this(512)
	{
	}

	public BuiltInFunctionCollection(int initialCapacity)
		: base(initialCapacity, CollationInfo.OrdinalIgnoreCase)
	{
	}
}
