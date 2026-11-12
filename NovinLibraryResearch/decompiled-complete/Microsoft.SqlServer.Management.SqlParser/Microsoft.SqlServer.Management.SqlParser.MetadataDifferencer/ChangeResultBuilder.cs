using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

internal class ChangeResultBuilder
{
	private readonly List<IMetadataObject> createdObjects;

	private readonly List<IMetadataObject> deletedObjects;

	private readonly IDictionary<IMetadataObject, ObjectDifference> sourceModifiedObjects;

	private readonly IDictionary<IMetadataObject, ObjectDifference> targetModifiedObjects;

	public ChangeResultBuilder()
	{
		createdObjects = new List<IMetadataObject>();
		deletedObjects = new List<IMetadataObject>();
		sourceModifiedObjects = new Dictionary<IMetadataObject, ObjectDifference>();
		targetModifiedObjects = new Dictionary<IMetadataObject, ObjectDifference>();
	}

	public ChangeResult GetChangeResult()
	{
		return new ChangeResult(createdObjects, deletedObjects, sourceModifiedObjects, targetModifiedObjects);
	}

	public void AddCreatedEntry(IMetadataObject createdObject)
	{
		createdObjects.Add(createdObject);
	}

	public void AddDeletedEntry(IMetadataObject deletedObject)
	{
		deletedObjects.Add(deletedObject);
	}

	public void AddModifiedEntry(IMetadataObject changedObjectSource, IMetadataObject changedObjectTarget, PropertyDifference propertyDifference)
	{
		if (!sourceModifiedObjects.TryGetValue(changedObjectSource, out var value))
		{
			value = CreateObjectDifference(changedObjectSource, changedObjectTarget);
		}
		value.AddModifiedEntry(propertyDifference);
	}

	public void AddOrderedScalarDifferenceEntry(IMetadataObject changedObjectSource, IMetadataObject changedObjectTarget, OrderedScalarDifference orderedDifference)
	{
		if (!sourceModifiedObjects.TryGetValue(changedObjectSource, out var value))
		{
			value = CreateObjectDifference(changedObjectSource, changedObjectTarget);
		}
		if (!value.PropertyDifferences.TryGetValue(orderedDifference.Name, out var value2))
		{
			value2 = CreateCollectionDifference(orderedDifference.Name, value);
		}
		((OrderedCollectionDifference)value2).AddOrderedScalarDifferenceEntry(orderedDifference);
	}

	private ObjectDifference CreateObjectDifference(IMetadataObject changedObjectSource, IMetadataObject changedObjectTarget)
	{
		ObjectDifference objectDifference = new ObjectDifference(changedObjectSource, changedObjectTarget);
		sourceModifiedObjects.Add(changedObjectSource, objectDifference);
		targetModifiedObjects.Add(changedObjectTarget, objectDifference);
		return objectDifference;
	}

	private OrderedCollectionDifference CreateCollectionDifference(string propertyName, ObjectDifference objectDifference)
	{
		OrderedCollectionDifference orderedCollectionDifference = new OrderedCollectionDifference(propertyName);
		objectDifference.AddModifiedEntry(orderedCollectionDifference);
		return orderedCollectionDifference;
	}
}
