using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

public abstract class ColumnEncryptionKeyValueCollectionBase : SortedListCollectionBase
{
	internal ColumnEncryptionKeyValueCollectionBase(SqlSmoObject parent)
		: base(parent)
	{
	}

	protected override void InitInnerCollection()
	{
		base.InternalStorage = new SmoSortedList(new ColumnEncryptionKeyValueObjectComparer());
	}

	public bool Contains(int ColumnMasterKeyID)
	{
		return Contains(new ColumnEncryptionKeyValueObjectKey(ColumnMasterKeyID));
	}

	internal override ObjectKeyBase CreateKeyFromUrn(Urn urn)
	{
		return new ColumnEncryptionKeyValueObjectKey(int.Parse(urn.GetAttribute("ColumnMasterKeyID"), SmoApplication.DefaultCulture));
	}
}
