using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class LikeExpression : SqlExpression
{
	public virtual SqlExpression Match { get; }

	public virtual SqlExpression Pattern { get; }

	public virtual SqlExpression EscapeChar { get; }

	public LikeExpression(SqlExpression match, SqlExpression pattern, SqlExpression escapeChar, RelationalTypeMapping typeMapping)
		: base(typeof(bool), typeMapping)
	{
		Match = match;
		Pattern = pattern;
		EscapeChar = escapeChar;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		SqlExpression match = (SqlExpression)visitor.Visit(Match);
		SqlExpression pattern = (SqlExpression)visitor.Visit(Pattern);
		SqlExpression escapeChar = (SqlExpression)visitor.Visit(EscapeChar);
		return Update(match, pattern, escapeChar);
	}

	public virtual LikeExpression Update(SqlExpression match, SqlExpression pattern, SqlExpression escapeChar)
	{
		if (match == Match && pattern == Pattern && escapeChar == EscapeChar)
		{
			return this;
		}
		return new LikeExpression(match, pattern, escapeChar, TypeMapping);
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.Visit(Match);
		expressionPrinter.Append(" LIKE ");
		expressionPrinter.Visit(Pattern);
		if (EscapeChar != null)
		{
			expressionPrinter.Append(" ESCAPE ");
			expressionPrinter.Visit(EscapeChar);
		}
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is LikeExpression likeExpression)
				{
					return Equals(likeExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(LikeExpression likeExpression)
	{
		if (base.Equals((object)likeExpression) && Match.Equals(likeExpression.Match) && Pattern.Equals(likeExpression.Pattern))
		{
			return EscapeChar.Equals(likeExpression.EscapeChar);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), Match, Pattern, EscapeChar);
	}
}
