using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlColumnDefinition : SqlCodeObject, ISqlTableElement
{
	[Flags]
	private enum ColumnConstraintType
	{
		None = 0,
		Check = 1,
		Default = 2,
		ForeignKey = 4,
		Identity = 8,
		NotNull = 0x10,
		Null = 0x20,
		PrimaryKey = 0x40,
		RowGuidCol = 0x80,
		Unique = 0x100,
		NullOrNotNull = 0x30,
		PrimaryKeyOrUnique = 0x140,
		IdentityOrDefault = 0xA,
		IdentityOrNull = 0x28
	}

	private struct ConstraintNode
	{
		private sealed class Comparer : IComparer<ConstraintNode>
		{
			public int Compare(ConstraintNode x, ConstraintNode y)
			{
				int priority = x.priority;
				return priority.CompareTo(y.priority);
			}
		}

		private static IComparer<ConstraintNode> ComparerInstance = new Comparer();

		private readonly SqlConstraint constraint;

		private readonly int priority;

		public SqlConstraint Constraint => constraint;

		public ConstraintNode(SqlConstraint constraint, int priority)
		{
			this.constraint = constraint;
			this.priority = priority;
		}

		public static List<ConstraintNode> Add(List<ConstraintNode> constraintList, SqlConstraint constraint, int priority)
		{
			ConstraintNode item = new ConstraintNode(constraint, priority);
			int num;
			if (constraintList == null)
			{
				constraintList = new List<ConstraintNode>();
				num = 0;
			}
			else
			{
				num = constraintList.BinarySearch(item, ComparerInstance);
				if (num < 0)
				{
					num = ~num;
				}
			}
			constraintList.Insert(num, item);
			return constraintList;
		}
	}

	internal IMutableColumn boundColumn;

	private readonly bool isImplicitTimestamp;

	private readonly SqlIdentifier name;

	private readonly SqlDataTypeSpecification dataType;

	private readonly SqlCollation collation;

	private SqlConstraintCollection constraints;

	private readonly SqlSparseOption sparseOption;

	private readonly SqlGeneratedAlwaysType generatedAlwaysType;

	internal override bool IsVisitable => true;

	public IColumn BoundColumn => boundColumn;

	public override IMetadataObject BoundObject => boundColumn;

	public SqlIdentifier Name => name;

	public SqlDataTypeSpecification DataType => dataType;

	public SqlCollation Collation => collation;

	public SqlConstraintCollection Constraints => constraints;

	public SqlSparseOption SparseOption => sparseOption;

	public SqlGeneratedAlwaysType GeneratedAlwaysType => generatedAlwaysType;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return name;
			if (dataType != null)
			{
				yield return dataType;
			}
			if (collation != null)
			{
				yield return collation;
			}
			if (constraints == null)
			{
				yield break;
			}
			foreach (SqlConstraint constraint in constraints)
			{
				yield return constraint;
			}
		}
	}

	bool ISqlTableElement.IsColumnDefinition => true;

	SqlColumnDefinition ISqlTableElement.AsColumnDefinition => this;

	bool ISqlTableElement.IsConstraint => false;

	SqlConstraint ISqlTableElement.AsConstraint => null;

	SqlTemporalPeriodDefinition ISqlTableElement.AsTemporalPeriodDefinition => null;

	bool ISqlTableElement.IsTemporalPeriodDefinition => false;

	public override void Accept(ISqlCodeObjectVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	internal virtual ComputedColumnInfo CreateComputedColumnInfo()
	{
		return null;
	}

	internal sealed override void Bind(IBindingContext bindingContext)
	{
	}

	internal void BindPhase1(BatchBindingContext bindingContext, IMutableDatabaseTable boundTable)
	{
		IMetadataFactory metadataFactory = bindingContext.MetadataFactory;
		IScalarDataType scalarDataType;
		if (dataType == null)
		{
			scalarDataType = ((!isImplicitTimestamp) ? null : bindingContext.FindSystemDataType(DataTypeSpec.Timestamp));
		}
		else
		{
			dataType.Bind(bindingContext);
			base.BindingState |= dataType.BindingState;
			if (dataType.BoundType != null)
			{
				if (dataType.BoundType.IsTable)
				{
					ReportBindError(SR.ColumnHasUserDefinedTableType(name.Value));
					scalarDataType = null;
				}
				else
				{
					scalarDataType = BindUtils.DataType.AsScalarDataType(dataType.BoundType);
				}
			}
			else
			{
				dataType.ReportInvalidTypeBindError(Name.Value, isColumn: true);
				scalarDataType = null;
			}
		}
		string value = name.Value;
		boundColumn = metadataFactory.Column.Create(boundTable, value);
		boundColumn.DataType = scalarDataType ?? metadataFactory.DataType.UnknownScalar;
		boundColumn.ComputedColumnInfo = CreateComputedColumnInfo();
		switch (sparseOption)
		{
		case SqlSparseOption.ColumnSet:
			boundColumn.IsColumnSet = true;
			break;
		case SqlSparseOption.Sparse:
			boundColumn.IsSparse = true;
			break;
		}
		switch (GeneratedAlwaysType)
		{
		case SqlGeneratedAlwaysType.AsRowStart:
			boundColumn.IsGeneratedAlwaysAsRowStart = true;
			break;
		case SqlGeneratedAlwaysType.AsRowEnd:
			boundColumn.IsGeneratedAlwaysAsRowEnd = true;
			break;
		case SqlGeneratedAlwaysType.AsTransactionIdStart:
			boundColumn.IsGeneratedAlwaysAsTransactionIdStart = true;
			break;
		case SqlGeneratedAlwaysType.AsTransactionIdEnd:
			boundColumn.IsGeneratedAlwaysAsTransactionIdEnd = true;
			break;
		case SqlGeneratedAlwaysType.AsSequenceNumberStart:
			boundColumn.IsGeneratedAlwaysAsSequenceNumberStart = true;
			break;
		case SqlGeneratedAlwaysType.AsSequenceNumberEnd:
			boundColumn.IsGeneratedAlwaysAsSequenceNumberEnd = true;
			break;
		}
		if (collation != null)
		{
			collation.Bind(bindingContext);
			base.BindingState |= collation.BindingState;
			if (collation.BoundCollation != null)
			{
				IScalarDataType scalarDataType2 = boundColumn.DataType;
				if (BindUtils.DataType.IsValidForCollate(scalarDataType2))
				{
					boundColumn.Collation = collation.BoundCollation;
				}
				else
				{
					if (BindUtils.DataType.isSystemDataType(scalarDataType2))
					{
						collation.ReportBindError(SR.ExpressionTypeInvalidForCollate(scalarDataType2.BaseSystemDataType.TypeSpec.Name));
					}
					else if (BindUtils.DataType.IsUserDefinedType(scalarDataType2))
					{
						collation.ReportBindError(SR.CollateCannotBeUsedOnUddt);
					}
					base.BindingState |= BindingState.BindFailed;
				}
			}
		}
		if (name.IsEmpty)
		{
			base.BindingState |= BindingState.BindFailed;
		}
		else if (boundTable.Columns.Contains(value))
		{
			name.ReportBindError(SR.ColumnNameNotUnique(value, boundTable.Name));
			base.BindingState |= BindingState.BindFailed;
		}
		else
		{
			boundTable.Columns.Add(boundColumn);
			base.BindingState |= BindingState.BindSucceeded;
		}
	}

	internal virtual void BindPhase2(BatchBindingContext bindingContext, SqlConstraint.ConstraintBindingHelper bindingHelper, IMutableDatabaseTable boundTable)
	{
		if (constraints == null)
		{
			return;
		}
		string value = name.Value;
		List<ConstraintNode> list = null;
		ColumnConstraintType columnConstraintType = ColumnConstraintType.None;
		foreach (SqlConstraint constraint in constraints)
		{
			ColumnConstraintType columnConstraintType2 = GetColumnConstraintType(constraint.Type);
			if ((columnConstraintType & columnConstraintType2) == columnConstraintType2 || ((columnConstraintType | columnConstraintType2) & ColumnConstraintType.NullOrNotNull) == ColumnConstraintType.NullOrNotNull)
			{
				string constraintName = ((columnConstraintType2 == ColumnConstraintType.NotNull) ? "NULL" : constraint.ToString());
				constraint.ReportBindError(SR.ColumnConstraintNotUnique(constraintName, value, boundTable.Name));
				base.BindingState |= BindingState.BindFailed;
				continue;
			}
			if (((columnConstraintType | columnConstraintType2) & ColumnConstraintType.PrimaryKeyOrUnique) == ColumnConstraintType.PrimaryKeyOrUnique)
			{
				constraint.ReportBindError(SR.PrimaryKeyNotUnique(value, boundTable.Name));
				base.BindingState |= BindingState.BindFailed;
				continue;
			}
			if (((columnConstraintType | columnConstraintType2) & ColumnConstraintType.IdentityOrDefault) == ColumnConstraintType.IdentityOrDefault)
			{
				constraint.ReportBindError(SR.CannotHaveDefaultsOnIdentity(value, boundTable.Name));
				base.BindingState |= BindingState.BindFailed;
				continue;
			}
			if (((columnConstraintType | columnConstraintType2) & ColumnConstraintType.IdentityOrNull) == ColumnConstraintType.IdentityOrNull)
			{
				constraint.ReportBindError(SR.CannotCreateIdentityOnNullable(value, boundTable.Name));
				base.BindingState |= BindingState.BindFailed;
				continue;
			}
			switch (constraint.Type)
			{
			case SqlConstraintType.Null:
			case SqlConstraintType.NotNull:
			case SqlConstraintType.RowGuidCol:
				list = ConstraintNode.Add(list, constraint, 1);
				break;
			case SqlConstraintType.PrimaryKey:
			case SqlConstraintType.Unique:
			case SqlConstraintType.Identity:
			case SqlConstraintType.Default:
			case SqlConstraintType.Check:
				list = ConstraintNode.Add(list, constraint, 2);
				break;
			}
			columnConstraintType |= columnConstraintType2;
		}
		if (list == null)
		{
			return;
		}
		foreach (ConstraintNode item in list)
		{
			item.Constraint.Bind(bindingContext, bindingHelper, boundTable, boundColumn);
			base.BindingState |= item.Constraint.BindingState;
		}
	}

	internal void BindPhase3(BatchBindingContext bindingContext, SqlConstraint.ConstraintBindingHelper bindingHelper, IMutableDatabaseTable boundTable)
	{
		if (constraints == null)
		{
			return;
		}
		_ = name.Value;
		foreach (SqlConstraint constraint in constraints)
		{
			if (constraint.Type == SqlConstraintType.ForeignKey)
			{
				constraint.Bind(bindingContext, bindingHelper, boundTable, boundColumn);
				base.BindingState |= constraint.BindingState;
				break;
			}
		}
	}

	internal void BindPhase4(BatchBindingContext bindingContext, SqlConstraint.ConstraintBindingHelper bindingHelper, IMutableDatabaseTable boundTable)
	{
		if (!bindingHelper.NullableColumns.TryGetValue(boundColumn, out var value))
		{
			if (boundColumn.IsGeneratedAlwaysAsRowStart || boundColumn.IsGeneratedAlwaysAsRowEnd)
			{
				value = false;
			}
			else if (boundColumn.IsGeneratedAlwaysAsTransactionIdStart || boundColumn.IsGeneratedAlwaysAsSequenceNumberStart)
			{
				value = false;
			}
			else if (boundColumn.IsGeneratedAlwaysAsTransactionIdEnd || boundColumn.IsGeneratedAlwaysAsSequenceNumberEnd)
			{
				value = true;
			}
			else if (boundColumn.DataType.BaseSystemDataType != null)
			{
				Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType sqlDataType = boundColumn.DataType.BaseSystemDataType.TypeSpec.SqlDataType;
				value = ((sqlDataType != Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.SysName && sqlDataType != Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Timestamp) ? true : false);
			}
			else
			{
				value = true;
			}
		}
		boundColumn.Nullable = value;
		if (!ValidateSparseOption(boundTable, out var errorMessage))
		{
			ReportBindError(errorMessage);
			base.BindingState |= BindingState.BindFailed;
		}
		if (!ValidateGeneratedAlways(boundTable, out errorMessage))
		{
			ReportBindError(errorMessage);
			base.BindingState |= BindingState.BindFailed;
		}
	}

	private bool ValidateGeneratedAlways(IMutableDatabaseTable boundTable, out string errorMessage)
	{
		errorMessage = null;
		if (boundColumn.IsGeneratedAlwaysAsRowStart || boundColumn.IsGeneratedAlwaysAsRowEnd)
		{
			if (dataType.DataType.TypeSpec != null && dataType.DataType.TypeSpec.SqlDataType != Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.DateTime2)
			{
				errorMessage = SR.CannotCreateGeneratedAlwaysColumnType(Name.Value);
			}
			else if (boundColumn.Nullable)
			{
				errorMessage = SR.CannotCreateGeneratedAlwaysColumnNullable(Name.Value);
			}
		}
		return errorMessage == null;
	}

	private bool ValidateSparseOption(IMutableDatabaseTable boundTable, out string errorMessage)
	{
		errorMessage = null;
		if (boundColumn.IsSparse)
		{
			bool flag = !(dataType.BoundType is IUserDefinedClrType);
			if (flag && dataType.DataType.TypeSpec != null)
			{
				switch (dataType.DataType.TypeSpec.SqlDataType)
				{
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Geography:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Geometry:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Image:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.NText:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Text:
				case Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Timestamp:
					flag = false;
					break;
				}
			}
			if (!flag || !boundColumn.Nullable)
			{
				errorMessage = SR.CannotCreateSparseColumn(Name.Value, boundTable.Name);
			}
			if (errorMessage == null && constraints != null)
			{
				foreach (SqlConstraint constraint in constraints)
				{
					ColumnConstraintType columnConstraintType = GetColumnConstraintType(constraint.Type);
					if ((columnConstraintType & ColumnConstraintType.Identity) == ColumnConstraintType.Identity || (columnConstraintType & ColumnConstraintType.RowGuidCol) == ColumnConstraintType.RowGuidCol)
					{
						errorMessage = SR.CannotCreateSparseColumn(Name.Value, boundTable.Name);
						break;
					}
					if ((columnConstraintType & ColumnConstraintType.Default) == ColumnConstraintType.Default)
					{
						errorMessage = SR.CannotCreateDefaultConstraintOnSparseColumn(Name.Value, boundTable.Name);
						break;
					}
					if ((columnConstraintType & ColumnConstraintType.PrimaryKey) == ColumnConstraintType.PrimaryKey || (columnConstraintType & ColumnConstraintType.Unique) == ColumnConstraintType.Unique)
					{
						errorMessage = SR.ColumnIsInvalidForUseAsKeyColumnInIndex(Name.Value, boundTable.Name);
						break;
					}
				}
			}
		}
		else if (boundColumn.IsColumnSet && !dataType.DataType.IsXml)
		{
			errorMessage = SR.CannotCreateSparseColumnSetOnTable(Name.Value, boundTable.Name);
		}
		return errorMessage == null;
	}

	internal override void Unbind()
	{
		boundColumn = null;
		base.Unbind();
	}

	private static ColumnConstraintType GetColumnConstraintType(SqlConstraintType constraintType)
	{
		return constraintType switch
		{
			SqlConstraintType.Check => ColumnConstraintType.Check, 
			SqlConstraintType.Default => ColumnConstraintType.Default, 
			SqlConstraintType.ForeignKey => ColumnConstraintType.ForeignKey, 
			SqlConstraintType.Identity => ColumnConstraintType.Identity, 
			SqlConstraintType.NotNull => ColumnConstraintType.NotNull, 
			SqlConstraintType.Null => ColumnConstraintType.Null, 
			SqlConstraintType.PrimaryKey => ColumnConstraintType.PrimaryKey, 
			SqlConstraintType.RowGuidCol => ColumnConstraintType.RowGuidCol, 
			SqlConstraintType.Unique => ColumnConstraintType.Unique, 
			_ => ColumnConstraintType.None, 
		};
	}

	internal SqlColumnDefinition(SqlIdentifier name)
		: this(name, null, null, SqlSparseOption.None)
	{
		if (StringTokenLookup.Find(name.Value) == StringToken.Timestamp)
		{
			isImplicitTimestamp = true;
		}
		else
		{
			name.ReportParseError(SR.DataTypeMissing(name.Value));
		}
	}

	internal SqlColumnDefinition(SqlIdentifier name, SqlDataTypeSpecification dataType, SqlCollation collation)
		: this(name, dataType, collation, SqlSparseOption.None)
	{
	}

	internal SqlColumnDefinition(SqlIdentifier name, SqlDataTypeSpecification dataType, SqlSparseOption sparseOption)
		: this(name, dataType, null, sparseOption)
	{
	}

	internal SqlColumnDefinition(SqlIdentifier name, SqlDataTypeSpecification dataType, SqlCollation collation, SqlSparseOption sparseOption)
	{
		this.name = name;
		this.dataType = dataType;
		this.collation = collation;
		this.sparseOption = sparseOption;
		generatedAlwaysType = SqlGeneratedAlwaysType.None;
	}

	internal SqlColumnDefinition(SqlIdentifier name, SqlDataTypeSpecification dataType, SqlColumnAdditionalInfo colAdditionalInfo)
		: this(name, dataType, null, colAdditionalInfo?.SparseOption ?? SqlSparseOption.None)
	{
		generatedAlwaysType = colAdditionalInfo?.GeneratedAlwaysType ?? SqlGeneratedAlwaysType.None;
	}

	internal SqlColumnDefinition(SqlIdentifier name, SqlDataTypeSpecification dataType, SqlCollation collation, SqlColumnAdditionalInfo colAdditionalInfo)
	{
		this.name = name;
		this.dataType = dataType;
		this.collation = collation;
		sparseOption = colAdditionalInfo?.SparseOption ?? SqlSparseOption.None;
		generatedAlwaysType = colAdditionalInfo?.GeneratedAlwaysType ?? SqlGeneratedAlwaysType.None;
	}

	internal void AddConstraints(List<SqlConstraint> constraints)
	{
		if (constraints != null)
		{
			this.constraints = new SqlConstraintCollection(constraints);
		}
	}
}
