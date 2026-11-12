using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal interface ISequentialValueList<T> : IEnumerable<T>, IEnumerable
{
	int Count { get; }
}
