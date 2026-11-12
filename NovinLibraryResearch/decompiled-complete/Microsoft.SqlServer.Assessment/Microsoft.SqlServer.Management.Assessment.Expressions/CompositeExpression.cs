using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal abstract class CompositeExpression : Expression, IEquatable<CompositeExpression>
{
	public List<Expression> Children { get; } = new List<Expression>();


	protected CompositeExpression(IEnumerable<Expression> source)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		Children.AddRange(source);
	}

	protected CompositeExpression()
	{
	}

	public bool Equals(CompositeExpression other)
	{
		if (other == null)
		{
			return false;
		}
		if (GetType() != other.GetType())
		{
			return false;
		}
		return Children.SequenceEqual(other.Children);
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
		return Equals((CompositeExpression)obj);
	}

	public override int GetHashCode()
	{
		int num = GetType().GetHashCode();
		foreach (Expression child in Children)
		{
			num = ((num * 397) ^ child?.GetHashCode()).GetValueOrDefault();
		}
		return num;
	}

	internal override IEnumerable<string> GetNames()
	{
		return Children.SelectMany((Expression i) => i.GetNames());
	}

	protected abstract CompositeExpression Copy();

	protected static object[] GetNArguments(IEnumerable<object> args, int n)
	{
		using IEnumerator<object> enumerator = args.GetEnumerator();
		object[] firstNArguments = GetFirstNArguments(enumerator, n);
		CheckUnexpectedArguments(n, enumerator);
		return firstNArguments;
	}

	private static void CheckUnexpectedArguments(int n, IEnumerator<object> enumerator)
	{
		int num = 0;
		while (enumerator.MoveNext())
		{
			num++;
		}
		if (num > 0)
		{
			throw new SqlAssessmentArityException(n, n + num);
		}
	}

	private static object[] GetFirstNArguments(IEnumerator<object> enumerator, int n)
	{
		object[] array = new object[n];
		for (int i = 0; i < n; i++)
		{
			if (!enumerator.MoveNext())
			{
				throw new SqlAssessmentArityException(n, i);
			}
			array[i] = enumerator.Current;
		}
		return array;
	}
}
