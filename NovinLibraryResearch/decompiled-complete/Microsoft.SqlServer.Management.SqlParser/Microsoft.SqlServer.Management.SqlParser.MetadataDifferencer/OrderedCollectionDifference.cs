using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

public class OrderedCollectionDifference : PropertyDifference
{
	private readonly List<OrderedScalarDifference> orderDifferences;

	public IEnumerable<OrderedScalarDifference> OrderDifferences => orderDifferences;

	internal OrderedCollectionDifference(string propertyName)
		: base(propertyName)
	{
		orderDifferences = new List<OrderedScalarDifference>();
	}

	internal void AddOrderedScalarDifferenceEntry(OrderedScalarDifference orderedDifference)
	{
		orderDifferences.Add(orderedDifference);
	}
}
