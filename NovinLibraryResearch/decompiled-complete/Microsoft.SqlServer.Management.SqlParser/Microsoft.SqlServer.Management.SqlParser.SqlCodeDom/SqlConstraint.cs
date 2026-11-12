using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlConstraint : SqlCodeObject
{
	private sealed class TypeOnlyConstraint : SqlConstraint
	{
		internal override bool IsVisitable => true;

		public override IEnumerable<SqlCodeObject> Children
		{
			get
			{
				if (base.Name != null)
				{
					yield return base.Name;
				}
			}
		}

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

		internal override void Bind(BatchBindingContext bindingContext, ConstraintBindingHelper bindingHelper, IMutableDatabaseTable boundTable)
		{
		}

		internal override void Bind(BatchBindingContext bindingContext, ConstraintBindingHelper bindingHelper, IMutableDatabaseTable boundTable, IMutableColumn boundColumn)
		{
			switch (type)
			{
			case SqlConstraintType.Null:
				bindingHelper.NullableColumns[boundColumn] = true;
				base.BindingState = BindingState.BindSucceeded;
				break;
			case SqlConstraintType.NotNull:
				bindingHelper.NullableColumns[boundColumn] = false;
				base.BindingState = BindingState.BindSucceeded;
				break;
			case SqlConstraintType.RowGuidCol:
				if (!BindUtils.DataType.IsUniqueIdentifier(boundColumn.DataType))
				{
					ReportBindError(SR.RowguidcolDatatypeMismatch);
					base.BindingState |= BindingState.BindFailed;
				}
				else
				{
					boundColumn.RowGuidCol = true;
					base.BindingState = BindingState.BindSucceeded;
				}
				break;
			}
		}

		public TypeOnlyConstraint(SqlConstraintType type)
			: base(type)
		{
		}
	}

	internal struct ConstraintBindingHelper
	{
		public readonly Dictionary<IColumn, bool> NullableColumns;

		public ConstraintBindingHelper(Dictionary<IColumn, bool> nullableColumns)
		{
			NullableColumns = nullableColumns;
		}
	}

	private SqlIdentifier name;

	private readonly SqlConstraintType type;

	public SqlIdentifier Name
	{
		get
		{
			return name;
		}
		internal set
		{
			name = value;
		}
	}

	public SqlConstraintType Type => type;

	internal sealed override void Bind(IBindingContext bindingContext)
	{
	}

	internal abstract void Bind(BatchBindingContext bindingContext, ConstraintBindingHelper bindingHelper, IMutableDatabaseTable boundTable);

	internal abstract void Bind(BatchBindingContext bindingContext, ConstraintBindingHelper bindingHelper, IMutableDatabaseTable boundTable, IMutableColumn boundColumn);

	internal SqlConstraint(SqlConstraintType type)
	{
		this.type = type;
	}

	public override string ToString()
	{
		return type.ToString().ToUpperInvariant();
	}

	internal static SqlConstraint Null()
	{
		return new TypeOnlyConstraint(SqlConstraintType.Null);
	}

	internal static SqlConstraint NotNull()
	{
		return new TypeOnlyConstraint(SqlConstraintType.NotNull);
	}

	internal static SqlConstraint RowGuidCol()
	{
		return new TypeOnlyConstraint(SqlConstraintType.RowGuidCol);
	}
}
