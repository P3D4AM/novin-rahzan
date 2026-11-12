using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlComputedColumnDefinition : SqlColumnDefinition
{
	private const SqlConstraintType ValidAlways = SqlConstraintType.Unique;

	private const SqlConstraintType ValidIfPersisted = SqlConstraintType.ForeignKey;

	private const SqlConstraintType ValidTypes = SqlConstraintType.ForeignKey;

	private LocalBindingContext localBindingContext;

	private readonly SqlScalarExpression expression;

	private readonly bool isPersisted;

	internal override bool IsVisitable => true;

	internal override IBindingContext BindingContext => localBindingContext;

	public SqlScalarExpression Expression => expression;

	public bool IsPersisted => isPersisted;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.Name;
			yield return expression;
			if (base.Constraints == null)
			{
				yield break;
			}
			foreach (SqlConstraint constraint in base.Constraints)
			{
				yield return constraint;
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

	internal override ComputedColumnInfo CreateComputedColumnInfo()
	{
		return new ComputedColumnInfo(expression.Sql, isPersisted);
	}

	internal override void BindPhase2(BatchBindingContext bindingContext, SqlConstraint.ConstraintBindingHelper bindingHelper, IMutableDatabaseTable boundTable)
	{
		localBindingContext = bindingContext.CreateLocalBindingContext();
		localBindingContext.AddLocalTableExpression(boundTable);
		expression.Bind(localBindingContext);
		base.BindingState |= expression.BindingState;
		IScalar boundScalar = expression.BoundScalar;
		if (boundScalar != null)
		{
			boundColumn.DataType = boundScalar.DataType;
		}
		if (base.Constraints == null)
		{
			return;
		}
		foreach (SqlConstraint constraint in base.Constraints)
		{
			SqlConstraintType sqlConstraintType = (isPersisted ? SqlConstraintType.ForeignKey : SqlConstraintType.Unique);
			if ((constraint.Type | sqlConstraintType) != sqlConstraintType)
			{
				constraint.ReportBindError(SR.ComputedColumnsConstraintCheckError);
			}
		}
		base.BindPhase2(bindingContext, bindingHelper, boundTable);
	}

	internal override void Unbind()
	{
		base.Unbind();
		localBindingContext = null;
	}

	internal SqlComputedColumnDefinition(SqlIdentifier name, SqlScalarExpression expression, bool isPersisted)
		: base(name, (SqlDataTypeSpecification)null, (SqlCollation)null)
	{
		this.expression = expression;
		this.isPersisted = isPersisted;
	}
}
