using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(IndexTypeConverter))]
public enum IndexType
{
	[TsqlSyntaxString("CLUSTERED INDEX")]
	[LocDisplayName("Clustered")]
	ClusteredIndex,
	[TsqlSyntaxString("INDEX")]
	[LocDisplayName("NonClustered")]
	NonClusteredIndex,
	[TsqlSyntaxString("PRIMARY XML INDEX")]
	[LocDisplayName("PrimaryXml")]
	PrimaryXmlIndex,
	[TsqlSyntaxString("XML INDEX")]
	[LocDisplayName("SecondaryXml")]
	SecondaryXmlIndex,
	[TsqlSyntaxString("SPATIAL INDEX")]
	[LocDisplayName("Spatial")]
	SpatialIndex,
	[TsqlSyntaxString("NONCLUSTERED COLUMNSTORE INDEX")]
	[LocDisplayName("NonClusteredColumnStore")]
	NonClusteredColumnStoreIndex,
	[TsqlSyntaxString("NONCLUSTERED HASH INDEX")]
	[LocDisplayName("NonClusteredHash")]
	NonClusteredHashIndex,
	[TsqlSyntaxString("SELECTIVE XML INDEX")]
	[LocDisplayName("SelectiveXml")]
	SelectiveXmlIndex,
	[TsqlSyntaxString("")]
	[LocDisplayName("SecondarySelectiveXml")]
	SecondarySelectiveXmlIndex,
	[TsqlSyntaxString("CLUSTERED COLUMNSTORE INDEX")]
	[LocDisplayName("ClusteredColumnStore")]
	ClusteredColumnStoreIndex,
	[TsqlSyntaxString("HEAP")]
	[LocDisplayName("Heap")]
	HeapIndex
}
