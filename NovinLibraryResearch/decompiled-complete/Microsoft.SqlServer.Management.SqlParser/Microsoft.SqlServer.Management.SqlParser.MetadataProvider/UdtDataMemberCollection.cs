using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

internal class UdtDataMemberCollection : UdtMemberCollectionBase<IUdtDataMember>
{
	public static UdtDataMemberCollection Create(IUdtDataMember[] dataMembers)
	{
		if (dataMembers == null)
		{
			throw new ArgumentNullException("dataMembers");
		}
		UdtDataMemberCollection udtDataMemberCollection = new UdtDataMemberCollection(dataMembers.Length);
		udtDataMemberCollection.AddRange(dataMembers);
		return udtDataMemberCollection;
	}

	public UdtDataMemberCollection()
		: this(16)
	{
	}

	public UdtDataMemberCollection(int initialCapacity)
		: base(initialCapacity)
	{
	}
}
