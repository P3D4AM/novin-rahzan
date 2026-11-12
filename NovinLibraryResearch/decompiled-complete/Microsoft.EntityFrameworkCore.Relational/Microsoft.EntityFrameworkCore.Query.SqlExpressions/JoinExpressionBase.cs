using System;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public abstract class JoinExpressionBase : TableExpressionBase
{
	public virtual TableExpressionBase Table { get; }

	protected JoinExpressionBase(TableExpressionBase table)
		: base(null)
	{
		Table = table;
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is JoinExpressionBase joinExpressionBase)
				{
					return Equals(joinExpressionBase);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(JoinExpressionBase joinExpressionBase)
	{
		if (base.Equals((object)joinExpressionBase))
		{
			return Table.Equals(joinExpressionBase.Table);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), Table);
	}
}
