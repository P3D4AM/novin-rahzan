using System.Collections.Generic;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class ResultCoordinator
{
	public virtual ResultContext ResultContext { get; }

	public virtual bool ResultReady { get; set; }

	public virtual bool? HasNext { get; set; }

	public virtual IList<CollectionMaterializationContext> Collections { get; } = new List<CollectionMaterializationContext>();


	public ResultCoordinator()
	{
		ResultContext = new ResultContext();
	}

	public virtual void SetCollectionMaterializationContext(int collectionId, CollectionMaterializationContext collectionMaterializationContext)
	{
		while (Collections.Count <= collectionId)
		{
			Collections.Add(null);
		}
		Collections[collectionId] = collectionMaterializationContext;
	}
}
