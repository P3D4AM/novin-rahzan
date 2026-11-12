using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class DatePartCollection : DictionaryCollection<IDatePart>
{
	private static class PopulatedCollection
	{
		public static readonly DatePartCollection Instance;

		static PopulatedCollection()
		{
			Instance = new DatePartCollection();
			DatePart.PopulateCollection(Instance);
		}
	}

	internal static IMetadataCollection<IDatePart> PopulatedInstance => PopulatedCollection.Instance;

	public DatePartCollection()
		: this(64)
	{
	}

	public DatePartCollection(int initialCapacity)
		: base(initialCapacity, CollationInfo.OrdinalIgnoreCase)
	{
	}
}
