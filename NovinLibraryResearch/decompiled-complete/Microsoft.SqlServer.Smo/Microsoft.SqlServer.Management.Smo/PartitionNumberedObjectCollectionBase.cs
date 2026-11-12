using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

public abstract class PartitionNumberedObjectCollectionBase : SortedListCollectionBase
{
	internal PartitionNumberedObjectCollectionBase(SqlSmoObject parent)
		: base(parent)
	{
	}

	protected override void InitInnerCollection()
	{
		base.InternalStorage = new SmoSortedList(new PartitionNumberedObjectComparer());
	}

	public bool Contains(int number)
	{
		return Contains(new PartitionNumberedObjectKey(number));
	}

	internal override ObjectKeyBase CreateKeyFromUrn(Urn urn)
	{
		return new PartitionNumberedObjectKey(short.Parse(urn.GetAttribute("PartitionNumber"), SmoApplication.DefaultCulture));
	}
}
