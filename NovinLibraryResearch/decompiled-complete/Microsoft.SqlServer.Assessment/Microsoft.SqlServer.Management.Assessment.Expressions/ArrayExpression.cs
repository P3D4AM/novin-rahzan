using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class ArrayExpression : Expression, IEquatable<ArrayExpression>
{
	public IEnumerable<Expression> Items { get; }

	public ArrayExpression(IEnumerable<Expression> source)
	{
		Items = source ?? throw new ArgumentNullException("source");
	}

	internal override object EvaluateCore(IEvaluationContext data)
	{
		return Items.Select((Expression i) => i.Evaluate(data)).ToArray();
	}

	internal override IEnumerable<string> GetNames()
	{
		return Items.SelectMany((Expression i) => i.GetNames());
	}

	public bool Equals(ArrayExpression other)
	{
		if (other == null)
		{
			return false;
		}
		if (GetType() != other.GetType())
		{
			return false;
		}
		return Items.SequenceEqual(other.Items);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((ArrayExpression)obj);
	}

	public override int GetHashCode()
	{
		int num = GetType().GetHashCode();
		foreach (Expression item in Items)
		{
			num = ((num * 397) ^ item?.GetHashCode()).GetValueOrDefault();
		}
		return num;
	}
}
