namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class PropertyCounts
{
	public virtual int PropertyCount { get; }

	public virtual int NavigationCount { get; }

	public virtual int OriginalValueCount { get; }

	public virtual int ShadowCount { get; }

	public virtual int RelationshipCount { get; }

	public virtual int StoreGeneratedCount { get; }

	public PropertyCounts(int propertyCount, int navigationCount, int originalValueCount, int shadowCount, int relationshipCount, int storeGeneratedCount)
	{
		PropertyCount = propertyCount;
		NavigationCount = navigationCount;
		OriginalValueCount = originalValueCount;
		ShadowCount = shadowCount;
		RelationshipCount = relationshipCount;
		StoreGeneratedCount = storeGeneratedCount;
	}
}
