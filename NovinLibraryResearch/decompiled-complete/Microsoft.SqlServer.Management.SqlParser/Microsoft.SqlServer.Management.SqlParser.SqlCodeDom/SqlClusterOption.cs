namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public enum SqlClusterOption
{
	Default,
	Clustered,
	NonClustered,
	NonClusteredHash,
	Hash,
	ClusteredColumnstore,
	NonClusteredColumnstore
}
