namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class CollectionMaterializationContext
{
	public virtual ResultContext ResultContext { get; }

	public virtual object Parent { get; }

	public virtual object Collection { get; }

	public virtual object[] ParentIdentifier { get; }

	public virtual object[] OuterIdentifier { get; }

	public virtual object[] SelfIdentifier { get; private set; }

	public CollectionMaterializationContext(object parent, object collection, object[] parentIdentifier, object[] outerIdentifier)
	{
		Parent = parent;
		Collection = collection;
		ParentIdentifier = parentIdentifier;
		OuterIdentifier = outerIdentifier;
		ResultContext = new ResultContext();
	}

	public virtual void UpdateSelfIdentifier(object[] selfIdentifier)
	{
		SelfIdentifier = selfIdentifier;
	}
}
