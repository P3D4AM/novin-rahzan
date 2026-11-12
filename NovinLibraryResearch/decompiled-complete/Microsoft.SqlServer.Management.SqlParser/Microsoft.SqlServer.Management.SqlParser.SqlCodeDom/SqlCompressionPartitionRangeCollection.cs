using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCompressionPartitionRangeCollection : SqlCollection<SqlCompressionPartitionRange>
{
	internal SqlCompressionPartitionRangeCollection(IEnumerable<SqlCompressionPartitionRange> partitions)
		: base(partitions)
	{
	}
}
