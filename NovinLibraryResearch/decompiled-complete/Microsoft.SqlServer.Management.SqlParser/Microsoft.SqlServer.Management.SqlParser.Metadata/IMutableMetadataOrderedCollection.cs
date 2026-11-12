using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableMetadataOrderedCollection<T> : IMetadataOrderedCollection<T>, IMetadataCollection<T>, IEnumerable<T>, IEnumerable, IMutableMetadataCollection<T> where T : class, IMetadataObject
{
}
