using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class TableAliasUniquifyingExpressionVisitor : ExpressionVisitor
{
	private readonly ISet<string> _usedAliases = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

	private readonly ISet<TableExpressionBase> _visitedTableExpressionBases = new HashSet<TableExpressionBase>(Microsoft.EntityFrameworkCore.Internal.ReferenceEqualityComparer.Instance);

	protected override Expression VisitExtension(Expression extensionExpression)
	{
		Expression expression = base.VisitExtension(extensionExpression);
		if (expression is TableExpressionBase tableExpressionBase && !_visitedTableExpressionBases.Contains(tableExpressionBase) && tableExpressionBase.Alias != null)
		{
			tableExpressionBase.Alias = GenerateUniqueAlias(tableExpressionBase.Alias);
			_visitedTableExpressionBases.Add(tableExpressionBase);
		}
		return expression;
	}

	private string GenerateUniqueAlias(string currentAlias)
	{
		if (!_usedAliases.Contains(currentAlias))
		{
			_usedAliases.Add(currentAlias);
			return currentAlias;
		}
		int num = 0;
		string text = currentAlias;
		while (_usedAliases.Contains(text))
		{
			text = currentAlias + num++;
		}
		_usedAliases.Add(text);
		return text;
	}
}
