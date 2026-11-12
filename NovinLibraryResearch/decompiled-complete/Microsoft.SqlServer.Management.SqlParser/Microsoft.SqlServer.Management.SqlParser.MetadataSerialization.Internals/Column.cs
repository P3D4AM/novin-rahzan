using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class Column : IColumn, IScalar, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly int collation;

	private readonly ComputedColumnInfo computedColumnInfo;

	private readonly int dataType;

	private readonly int defaultValue;

	private readonly IdentityColumnInfo identityColumnInfo;

	private readonly bool inPrimaryKey;

	private readonly bool isColumnSet;

	private readonly bool isGeneratedAlwaysAsRowEnd;

	private readonly bool isGeneratedAlwaysAsRowStart;

	private readonly bool isGeneratedAlwaysAsSequenceNumberEnd;

	private readonly bool isGeneratedAlwaysAsSequenceNumberStart;

	private readonly bool isGeneratedAlwaysAsTransactionIdEnd;

	private readonly bool isGeneratedAlwaysAsTransactionIdStart;

	private readonly bool isSparse;

	private readonly string name;

	private readonly bool nullable;

	private readonly int parent;

	private readonly bool rowGuidCol;

	ITabular IColumn.Parent => model.GetObjectById<ITabular>(parent);

	ICollation IColumn.Collation => model.GetObjectById<ICollation>(collation);

	ComputedColumnInfo IColumn.ComputedColumnInfo => computedColumnInfo;

	IDefaultConstraint IColumn.DefaultValue => model.GetObjectById<IDefaultConstraint>(defaultValue);

	IdentityColumnInfo IColumn.IdentityColumnInfo => identityColumnInfo;

	bool IColumn.InPrimaryKey => inPrimaryKey;

	bool IColumn.RowGuidCol => rowGuidCol;

	bool IColumn.IsSparse => isSparse;

	bool IColumn.IsColumnSet => isColumnSet;

	bool IColumn.IsGeneratedAlwaysAsRowStart => isGeneratedAlwaysAsRowStart;

	bool IColumn.IsGeneratedAlwaysAsRowEnd => isGeneratedAlwaysAsRowEnd;

	bool IColumn.IsGeneratedAlwaysAsTransactionIdStart => isGeneratedAlwaysAsTransactionIdStart;

	bool IColumn.IsGeneratedAlwaysAsTransactionIdEnd => isGeneratedAlwaysAsTransactionIdEnd;

	bool IColumn.IsGeneratedAlwaysAsSequenceNumberStart => isGeneratedAlwaysAsSequenceNumberStart;

	bool IColumn.IsGeneratedAlwaysAsSequenceNumberEnd => isGeneratedAlwaysAsSequenceNumberEnd;

	ScalarType IScalar.ScalarType => ScalarType.Column;

	IScalarDataType IScalar.DataType => model.GetObjectById<IScalarDataType>(dataType);

	bool IScalar.Nullable => nullable;

	string IMetadataObject.Name => name;

	public Column(DeserializationModel model, int collation, ComputedColumnInfo computedColumnInfo, int dataType, int defaultValue, IdentityColumnInfo identityColumnInfo, bool inPrimaryKey, bool isColumnSet, bool isGeneratedAlwaysAsRowEnd, bool isGeneratedAlwaysAsRowStart, bool isGeneratedAlwaysAsSequenceNumberEnd, bool isGeneratedAlwaysAsSequenceNumberStart, bool isGeneratedAlwaysAsTransactionIdEnd, bool isGeneratedAlwaysAsTransactionIdStart, bool isSparse, string name, bool nullable, int parent, bool rowGuidCol)
	{
		this.model = model;
		this.collation = collation;
		this.computedColumnInfo = computedColumnInfo;
		this.dataType = dataType;
		this.defaultValue = defaultValue;
		this.identityColumnInfo = identityColumnInfo;
		this.inPrimaryKey = inPrimaryKey;
		this.isColumnSet = isColumnSet;
		this.isGeneratedAlwaysAsRowEnd = isGeneratedAlwaysAsRowEnd;
		this.isGeneratedAlwaysAsRowStart = isGeneratedAlwaysAsRowStart;
		this.isGeneratedAlwaysAsSequenceNumberEnd = isGeneratedAlwaysAsSequenceNumberEnd;
		this.isGeneratedAlwaysAsSequenceNumberStart = isGeneratedAlwaysAsSequenceNumberStart;
		this.isGeneratedAlwaysAsTransactionIdEnd = isGeneratedAlwaysAsTransactionIdEnd;
		this.isGeneratedAlwaysAsTransactionIdStart = isGeneratedAlwaysAsTransactionIdStart;
		this.isSparse = isSparse;
		this.name = name;
		this.nullable = nullable;
		this.parent = parent;
		this.rowGuidCol = rowGuidCol;
	}

	public Column(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (ComputedColumnInfo)values[1], (int)values[2], (int)values[3], (IdentityColumnInfo)values[4], (bool)values[5], (bool)values[6], (bool)values[7], (bool)values[8], (bool)values[9], (bool)values[10], (bool)values[11], (bool)values[12], (bool)values[13], (string)values[14], (bool)values[15], (int)values[16], (bool)values[17])
	{
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
