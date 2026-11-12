using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(ExternalDataSourceTypeConverter))]
public enum ExternalDataSourceType
{
	[TsqlSyntaxString("HADOOP")]
	Hadoop = 0,
	[TsqlSyntaxString("RDBMS")]
	Rdbms = 1,
	[TsqlSyntaxString("SHARD_MAP_MANAGER")]
	ShardMapManager = 2,
	[TsqlSyntaxString("BLOB_STORAGE")]
	BlobStorage = 5,
	ExternalGenerics = 6
}
