namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public enum SqlIndexOptionType
{
	Invalid,
	AllowPageLocks,
	AllowRowLocks,
	DataCompression,
	DropExisting,
	FillFactor,
	IgnoreDupKey,
	MaxDegreeOfParallelism,
	Online,
	PadIndex,
	SortedData,
	SortedDataReorg,
	StatisticsIncremental,
	SortInTempDb,
	StatisticsNoRecompute,
	StatisticsOnly,
	BucketCount,
	CompressionDelay,
	Resumable,
	MaxDuration,
	OptimizeForSequentialKey
}
