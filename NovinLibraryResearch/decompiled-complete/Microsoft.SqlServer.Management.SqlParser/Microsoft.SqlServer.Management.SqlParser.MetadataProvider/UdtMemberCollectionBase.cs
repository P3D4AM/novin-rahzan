using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public abstract class UdtMemberCollectionBase<T> : DualTypeSortedListCollection<T, IUdtMember> where T : class, IUdtMember
{
	protected UdtMemberCollectionBase(int initialCapacity)
		: base(initialCapacity, CollationInfo.Ordinal)
	{
	}
}
