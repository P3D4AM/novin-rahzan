using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

public class ChangeResult
{
	private readonly List<IMetadataObject> createdObjects;

	private readonly List<IMetadataObject> deletedObjects;

	private readonly IDictionary<IMetadataObject, ObjectDifference> sourceModifiedObjects;

	private readonly IDictionary<IMetadataObject, ObjectDifference> targetModifiedObjects;

	public IEnumerable<IMetadataObject> CreatedObjects => createdObjects;

	public IEnumerable<IMetadataObject> DeletedObjects => deletedObjects;

	public IDictionary<IMetadataObject, ObjectDifference> SourceModifiedObjects => sourceModifiedObjects;

	public IDictionary<IMetadataObject, ObjectDifference> TargetModifiedObjects => targetModifiedObjects;

	internal ChangeResult(IEnumerable<IMetadataObject> createdObjects, IEnumerable<IMetadataObject> deletedObjects, IDictionary<IMetadataObject, ObjectDifference> sourceModifiedObjects, IDictionary<IMetadataObject, ObjectDifference> targetModifiedObjects)
	{
		this.createdObjects = new List<IMetadataObject>(createdObjects);
		this.deletedObjects = new List<IMetadataObject>(deletedObjects);
		this.sourceModifiedObjects = new Dictionary<IMetadataObject, ObjectDifference>();
		foreach (KeyValuePair<IMetadataObject, ObjectDifference> sourceModifiedObject in sourceModifiedObjects)
		{
			this.sourceModifiedObjects.Add(sourceModifiedObject);
		}
		this.targetModifiedObjects = new Dictionary<IMetadataObject, ObjectDifference>();
		foreach (KeyValuePair<IMetadataObject, ObjectDifference> targetModifiedObject in targetModifiedObjects)
		{
			this.targetModifiedObjects.Add(targetModifiedObject);
		}
	}

	public bool IsEmpty()
	{
		if (createdObjects.Count == 0 && deletedObjects.Count == 0 && sourceModifiedObjects.Count == 0)
		{
			return targetModifiedObjects.Count == 0;
		}
		return false;
	}
}
