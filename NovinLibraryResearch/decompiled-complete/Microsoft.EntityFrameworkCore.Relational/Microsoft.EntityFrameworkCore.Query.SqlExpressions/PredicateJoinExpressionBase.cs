using System;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public abstract class PredicateJoinExpressionBase : JoinExpressionBase
{
	public virtual SqlExpression JoinPredicate { get; }

	protected PredicateJoinExpressionBase(TableExpressionBase table, SqlExpression joinPredicate)
		: base(table)
	{
		JoinPredicate = joinPredicate;
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is PredicateJoinExpressionBase predicateJoinExpressionBase)
				{
					return Equals(predicateJoinExpressionBase);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(PredicateJoinExpressionBase predicateJoinExpressionBase)
	{
		if (base.Equals((object)predicateJoinExpressionBase))
		{
			return JoinPredicate.Equals(predicateJoinExpressionBase.JoinPredicate);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), JoinPredicate);
	}
}
