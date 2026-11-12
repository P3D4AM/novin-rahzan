using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal interface ISerializationContext
{
	int RootObjectId { get; }

	int ObjectCount { get; }

	IEnumerable<TypeDataTable> DataTables { get; }

	ISequentialValueList<byte[]> ByteArrayTable { get; }

	ISequentialValueList<int[]> CollectionIdTable { get; }

	ISequentialValueList<CollationInfo> CollationInfoTable { get; }

	ISequentialValueList<ComputedColumnInfo> ComputedColumnInfoTable { get; }

	ISequentialValueList<DataTypeSpec> DataTypeSpecTable { get; }

	ISequentialValueList<double> DoubleTable { get; }

	ISequentialValueList<GridDensity> GridDensityTable { get; }

	ISequentialValueList<IdentityColumnInfo> IdentityColumnInfoTable { get; }

	ISequentialValueList<IndexType> IndexTypeTable { get; }

	ISequentialValueList<int> IntTable { get; }

	ISequentialValueList<IPassword> PasswordTable { get; }

	ISequentialValueList<string> StringTable { get; }

	ISequentialValueList<StatisticsType> StatisticsTypeTable { get; }
}
