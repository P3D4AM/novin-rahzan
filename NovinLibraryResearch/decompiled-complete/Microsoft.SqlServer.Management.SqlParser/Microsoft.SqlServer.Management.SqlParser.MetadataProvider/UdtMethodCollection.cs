using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class UdtMethodCollection : UdtMemberCollectionBase<IUdtMethod>
{
	internal static UdtMethodCollection Create(IUdtMethod[] methods)
	{
		if (methods == null)
		{
			throw new ArgumentNullException("methods");
		}
		UdtMethodCollection udtMethodCollection = new UdtMethodCollection(methods.Length);
		udtMethodCollection.AddRange(methods);
		return udtMethodCollection;
	}

	public UdtMethodCollection()
		: this(16)
	{
	}

	public UdtMethodCollection(int initialCapacity)
		: base(initialCapacity)
	{
	}
}
