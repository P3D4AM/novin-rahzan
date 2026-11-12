using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public abstract class SetOperationBase : TableExpressionBase
{
	public virtual bool IsDistinct { get; }

	public virtual SelectExpression Source1 { get; }

	public virtual SelectExpression Source2 { get; }

	protected SetOperationBase([JetBrains.Annotations.NotNull] string alias, SelectExpression source1, SelectExpression source2, bool distinct)
		: base(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(alias, "alias"))
	{
		IsDistinct = distinct;
		Source1 = source1;
		Source2 = source2;
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is SetOperationBase setOperationBase)
				{
					return Equals(setOperationBase);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(SetOperationBase setOperationBase)
	{
		if (IsDistinct == setOperationBase.IsDistinct && Source1.Equals(setOperationBase.Source1))
		{
			return Source2.Equals(setOperationBase.Source2);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), IsDistinct, Source1, Source2);
	}
}
