namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

internal class SqlColumnAdditionalInfo
{
	private readonly SqlSparseOption sparseOption;

	private readonly SqlGeneratedAlwaysType generatedAlwaysType;

	public SqlSparseOption SparseOption => sparseOption;

	public SqlGeneratedAlwaysType GeneratedAlwaysType => generatedAlwaysType;

	internal SqlColumnAdditionalInfo(SqlGeneratedAlwaysType generatedAlwaysType)
	{
		sparseOption = SqlSparseOption.None;
		this.generatedAlwaysType = generatedAlwaysType;
	}

	internal SqlColumnAdditionalInfo(SqlSparseOption sparseOption)
	{
		this.sparseOption = sparseOption;
		generatedAlwaysType = SqlGeneratedAlwaysType.None;
	}
}
