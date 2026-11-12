using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

public class SqlServerSqlTranslatingExpressionVisitor : RelationalSqlTranslatingExpressionVisitor
{
	private static readonly HashSet<string> _dateTimeDataTypes = new HashSet<string> { "time", "date", "datetime", "datetime2", "datetimeoffset" };

	private static readonly HashSet<ExpressionType> _arithmeticOperatorTypes = new HashSet<ExpressionType>
	{
		ExpressionType.Add,
		ExpressionType.Subtract,
		ExpressionType.Multiply,
		ExpressionType.Divide,
		ExpressionType.Modulo
	};

	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	public SqlServerSqlTranslatingExpressionVisitor(RelationalSqlTranslatingExpressionVisitorDependencies dependencies, IModel model, QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor)
		: base(dependencies, model, queryableMethodTranslatingExpressionVisitor)
	{
		_sqlExpressionFactory = dependencies.SqlExpressionFactory;
	}

	protected override Expression VisitBinary(BinaryExpression binaryExpression)
	{
		SqlExpression sqlExpression = (SqlExpression)base.VisitBinary(binaryExpression);
		if (sqlExpression == null)
		{
			return null;
		}
		if (!(sqlExpression is SqlBinaryExpression sqlBinaryExpression) || !_arithmeticOperatorTypes.Contains(sqlBinaryExpression.OperatorType) || (!_dateTimeDataTypes.Contains(GetProviderType(sqlBinaryExpression.Left)) && !_dateTimeDataTypes.Contains(GetProviderType(sqlBinaryExpression.Right))))
		{
			return sqlExpression;
		}
		return null;
	}

	public override SqlExpression TranslateLongCount(Expression expression = null)
	{
		if (expression != null)
		{
			return null;
		}
		return _sqlExpressionFactory.ApplyDefaultTypeMapping(_sqlExpressionFactory.Function("COUNT_BIG", new SqlFragmentExpression[1] { _sqlExpressionFactory.Fragment("*") }, typeof(long)));
	}

	private static string GetProviderType(SqlExpression expression)
	{
		return expression.TypeMapping?.StoreType;
	}
}
