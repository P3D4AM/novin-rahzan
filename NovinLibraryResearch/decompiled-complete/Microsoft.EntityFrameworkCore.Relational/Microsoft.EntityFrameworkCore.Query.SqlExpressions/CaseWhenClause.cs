using System;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class CaseWhenClause
{
	public virtual SqlExpression Test { get; }

	public virtual SqlExpression Result { get; }

	public CaseWhenClause(SqlExpression test, SqlExpression result)
	{
		Test = test;
		Result = result;
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is CaseWhenClause caseWhenClause)
				{
					return Equals(caseWhenClause);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(CaseWhenClause caseWhenClause)
	{
		if (Test.Equals(caseWhenClause.Test))
		{
			return Result.Equals(caseWhenClause.Result);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Test, Result);
	}
}
