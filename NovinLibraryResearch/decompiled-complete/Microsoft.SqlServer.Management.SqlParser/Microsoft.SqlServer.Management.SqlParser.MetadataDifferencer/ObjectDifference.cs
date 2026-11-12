using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

public class ObjectDifference
{
	private readonly IMetadataObject changedObjectSource;

	private readonly IMetadataObject changedObjectTarget;

	private readonly IDictionary<string, PropertyDifference> propertyDifferences;

	public IMetadataObject ChangedObjectSource => changedObjectSource;

	public IMetadataObject ChangedObjectTarget => changedObjectTarget;

	public IDictionary<string, PropertyDifference> PropertyDifferences => propertyDifferences;

	internal ObjectDifference(IMetadataObject changedObjectSource, IMetadataObject changedObjectTarget)
	{
		this.changedObjectSource = changedObjectSource;
		this.changedObjectTarget = changedObjectTarget;
		propertyDifferences = new Dictionary<string, PropertyDifference>();
	}

	internal void AddModifiedEntry(PropertyDifference propertyDifference)
	{
		propertyDifferences.Add(propertyDifference.Name, propertyDifference);
	}
}
