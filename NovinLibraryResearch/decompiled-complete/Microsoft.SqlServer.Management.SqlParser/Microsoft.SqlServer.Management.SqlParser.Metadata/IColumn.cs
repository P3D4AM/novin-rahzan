namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[ReadOnly("Name")]
[Constant("ScalarType", ScalarType.Column)]
public interface IColumn : IScalar, IMetadataObject
{
	[ReadOnly]
	[Parent]
	ITabular Parent { get; }

	[ReadWrite]
	[Reference]
	ICollation Collation { get; }

	[ReadWrite]
	ComputedColumnInfo ComputedColumnInfo { get; }

	[ReadWrite]
	[Child]
	IDefaultConstraint DefaultValue { get; }

	[ReadWrite]
	IdentityColumnInfo IdentityColumnInfo { get; }

	[ReadWrite]
	bool InPrimaryKey { get; }

	[ReadWrite]
	bool RowGuidCol { get; }

	[ReadWrite]
	bool IsSparse { get; }

	[ReadWrite]
	bool IsColumnSet { get; }

	[ReadWrite]
	bool IsGeneratedAlwaysAsRowStart { get; }

	[ReadWrite]
	bool IsGeneratedAlwaysAsRowEnd { get; }

	[ReadWrite]
	bool IsGeneratedAlwaysAsTransactionIdStart { get; }

	[ReadWrite]
	bool IsGeneratedAlwaysAsTransactionIdEnd { get; }

	[ReadWrite]
	bool IsGeneratedAlwaysAsSequenceNumberStart { get; }

	[ReadWrite]
	bool IsGeneratedAlwaysAsSequenceNumberEnd { get; }
}
