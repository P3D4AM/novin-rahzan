namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class PropertyIndexes
{
	public virtual int Index { get; }

	public virtual int OriginalValueIndex { get; }

	public virtual int ShadowIndex { get; }

	public virtual int RelationshipIndex { get; }

	public virtual int StoreGenerationIndex { get; }

	public PropertyIndexes(int index, int originalValueIndex, int shadowIndex, int relationshipIndex, int storeGenerationIndex)
	{
		Index = index;
		OriginalValueIndex = originalValueIndex;
		ShadowIndex = shadowIndex;
		RelationshipIndex = relationshipIndex;
		StoreGenerationIndex = storeGenerationIndex;
	}
}
