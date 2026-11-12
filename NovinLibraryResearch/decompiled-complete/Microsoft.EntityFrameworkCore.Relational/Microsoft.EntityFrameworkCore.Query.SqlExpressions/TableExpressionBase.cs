using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public abstract class TableExpressionBase : Expression, IPrintableExpression
{
	public virtual string Alias { get; internal set; }

	public override Type Type => typeof(object);

	public sealed override ExpressionType NodeType => ExpressionType.Extension;

	protected TableExpressionBase([JetBrains.Annotations.CanBeNull] string alias)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(alias, "alias");
		Alias = alias;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		return this;
	}

	public abstract void Print(ExpressionPrinter expressionPrinter);

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is TableExpressionBase tableExpressionBase)
				{
					return Equals(tableExpressionBase);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(TableExpressionBase tableExpressionBase)
	{
		return string.Equals(Alias, tableExpressionBase.Alias);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Alias);
	}
}
