using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataServices;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

internal class Differencer
{
	public static ChangeResult Compare(IDatabaseObject source, IDatabaseObject target, IDifferencerFilter differenceFilter)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if (differenceFilter == null)
		{
			throw new ArgumentNullException("differenceFilter");
		}
		if (!AreTypesEqual(source, target))
		{
			throw new DifferencerException(SR.InvalidRootTypes);
		}
		DifferencerIdGenerator metadataIdGenerator = new DifferencerIdGenerator(source, target);
		DifferencerIdResolver sourceIdResolver = new DifferencerIdResolver(source);
		DifferencerIdResolver targetIdResolver = new DifferencerIdResolver(target);
		return new PropertyComparerFactory(metadataIdGenerator, differenceFilter, sourceIdResolver, targetIdResolver).Process(source, target);
	}

	private static bool AreTypesEqual(IDatabaseObject source, IDatabaseObject target)
	{
		return source.TypeInfo() == target.TypeInfo();
	}
}
