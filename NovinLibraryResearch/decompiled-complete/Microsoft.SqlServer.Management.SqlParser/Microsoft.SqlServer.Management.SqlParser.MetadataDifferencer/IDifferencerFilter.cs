using System.Collections;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

internal interface IDifferencerFilter
{
	IEqualityComparer GetEqualityComparer(MetadataTypeInfo typeInfo, MetadataPropertyInfo propertyInfo);
}
