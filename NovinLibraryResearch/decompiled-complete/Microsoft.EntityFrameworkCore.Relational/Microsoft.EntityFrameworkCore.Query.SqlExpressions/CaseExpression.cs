using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class CaseExpression : SqlExpression
{
	private readonly List<CaseWhenClause> _whenClauses = new List<CaseWhenClause>();

	public virtual SqlExpression Operand { get; }

	public virtual IReadOnlyList<CaseWhenClause> WhenClauses => _whenClauses;

	public virtual SqlExpression ElseResult { get; }

	public CaseExpression(SqlExpression operand, IReadOnlyList<CaseWhenClause> whenClauses)
		: this(operand, whenClauses, null)
	{
	}

	public CaseExpression(IReadOnlyList<CaseWhenClause> whenClauses, SqlExpression elseResult)
		: this(null, whenClauses, elseResult)
	{
	}

	public CaseExpression(SqlExpression operand, IReadOnlyList<CaseWhenClause> whenClauses, SqlExpression elseResult)
		: base(whenClauses[0].Result.Type, whenClauses[0].Result.TypeMapping)
	{
		Operand = operand;
		_whenClauses.AddRange(whenClauses);
		ElseResult = elseResult;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		SqlExpression sqlExpression = (SqlExpression)visitor.Visit(Operand);
		bool flag = sqlExpression != Operand;
		List<CaseWhenClause> list = new List<CaseWhenClause>();
		foreach (CaseWhenClause whenClause in WhenClauses)
		{
			SqlExpression sqlExpression2 = (SqlExpression)visitor.Visit(whenClause.Test);
			SqlExpression sqlExpression3 = (SqlExpression)visitor.Visit(whenClause.Result);
			if (sqlExpression2 != whenClause.Test || sqlExpression3 != whenClause.Result)
			{
				flag = flag || true;
				list.Add(new CaseWhenClause(sqlExpression2, sqlExpression3));
			}
			else
			{
				list.Add(whenClause);
			}
		}
		SqlExpression sqlExpression4 = (SqlExpression)visitor.Visit(ElseResult);
		if (!(flag | (sqlExpression4 != ElseResult)))
		{
			return this;
		}
		return new CaseExpression(sqlExpression, list, sqlExpression4);
	}

	public virtual CaseExpression Update(SqlExpression operand, IReadOnlyList<CaseWhenClause> whenClauses, SqlExpression elseResult)
	{
		if (operand == Operand && whenClauses.SequenceEqual(WhenClauses) && elseResult == ElseResult)
		{
			return this;
		}
		return new CaseExpression(operand, whenClauses, elseResult);
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.Append("CASE");
		if (Operand != null)
		{
			expressionPrinter.Append(" ");
			expressionPrinter.Visit(Operand);
		}
		using (expressionPrinter.Indent())
		{
			foreach (CaseWhenClause whenClause in WhenClauses)
			{
				expressionPrinter.AppendLine().Append("WHEN ");
				expressionPrinter.Visit(whenClause.Test);
				expressionPrinter.Append(" THEN ");
				expressionPrinter.Visit(whenClause.Result);
			}
			if (ElseResult != null)
			{
				expressionPrinter.AppendLine().Append("ELSE ");
				expressionPrinter.Visit(ElseResult);
			}
		}
		expressionPrinter.AppendLine().Append("END");
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is CaseExpression caseExpression)
				{
					return Equals(caseExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(CaseExpression caseExpression)
	{
		if (base.Equals((object)caseExpression) && ((Operand == null) ? (caseExpression.Operand == null) : Operand.Equals(caseExpression.Operand)) && WhenClauses.SequenceEqual(caseExpression.WhenClauses))
		{
			if (ElseResult != null)
			{
				return ElseResult.Equals(caseExpression.ElseResult);
			}
			return caseExpression.ElseResult == null;
		}
		return false;
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default(HashCode);
		hashCode.Add(base.GetHashCode());
		hashCode.Add(Operand);
		for (int i = 0; i < WhenClauses.Count; i++)
		{
			hashCode.Add(WhenClauses[i]);
		}
		hashCode.Add(ElseResult);
		return hashCode.ToHashCode();
	}
}
