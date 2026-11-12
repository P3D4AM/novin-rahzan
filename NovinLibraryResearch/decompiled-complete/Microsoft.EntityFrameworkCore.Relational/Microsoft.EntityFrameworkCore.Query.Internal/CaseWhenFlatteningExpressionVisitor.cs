using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class CaseWhenFlatteningExpressionVisitor : ExpressionVisitor
{
	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	public CaseWhenFlatteningExpressionVisitor(ISqlExpressionFactory sqlExpressionFactory)
	{
		_sqlExpressionFactory = sqlExpressionFactory;
	}

	protected override Expression VisitExtension(Expression node)
	{
		if (node is CaseExpression { Operand: null, ElseResult: CaseExpression { Operand: null } elseResult } caseExpression)
		{
			return VisitExtension(_sqlExpressionFactory.Case(caseExpression.WhenClauses.Union(elseResult.WhenClauses).ToList(), elseResult.ElseResult));
		}
		return base.VisitExtension(node);
	}
}
