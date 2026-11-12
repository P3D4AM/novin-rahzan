namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableColumn : IColumn, IScalar, IMetadataObject, IMutableScalar, IMutableMetadataObject
{
	new ICollation Collation { get; set; }

	new ComputedColumnInfo ComputedColumnInfo { get; set; }

	new IDefaultConstraint DefaultValue { get; set; }

	new IdentityColumnInfo IdentityColumnInfo { get; set; }

	new bool InPrimaryKey { get; set; }

	new bool RowGuidCol { get; set; }

	new bool IsSparse { get; set; }

	new bool IsColumnSet { get; set; }

	new bool IsGeneratedAlwaysAsRowStart { get; set; }

	new bool IsGeneratedAlwaysAsRowEnd { get; set; }

	new bool IsGeneratedAlwaysAsTransactionIdStart { get; set; }

	new bool IsGeneratedAlwaysAsTransactionIdEnd { get; set; }

	new bool IsGeneratedAlwaysAsSequenceNumberStart { get; set; }

	new bool IsGeneratedAlwaysAsSequenceNumberEnd { get; set; }
}
