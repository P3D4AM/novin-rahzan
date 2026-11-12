namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class CreateIndexStart
{
	public bool IsUnique { get; set; }

	public SqlClusterOption ClusterOption { get; set; }

	public CreateIndexStart(bool isUnique, SqlClusterOption clusterOption)
	{
		IsUnique = isUnique;
		ClusterOption = clusterOption;
	}
}
