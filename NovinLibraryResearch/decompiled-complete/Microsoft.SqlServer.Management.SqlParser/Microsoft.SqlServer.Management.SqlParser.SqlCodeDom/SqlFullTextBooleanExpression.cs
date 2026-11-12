using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlFullTextBooleanExpression : SqlBooleanExpression
{
	private SqlFullTextFunctionType functionName;

	private SqlFullTextColumnCollection columnList;

	private SqlScalarExpression searchString;

	private SqlScalarExpression langExpression;

	internal override bool IsVisitable => true;

	public SqlFullTextFunctionType FunctionName => functionName;

	public SqlFullTextColumnCollection ColumnList => columnList;

	public SqlScalarExpression SearchString => searchString;

	public SqlScalarExpression LangExpression => langExpression;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			foreach (SqlFullTextColumn column in columnList)
			{
				yield return column;
			}
			yield return searchString;
			if (langExpression != null)
			{
				yield return langExpression;
			}
		}
	}

	public override void Accept(ISqlCodeObjectVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	internal SqlFullTextBooleanExpression(SqlFullTextFunctionType functionName, List<SqlFullTextColumn> columnList, SqlScalarExpression searchString, SqlScalarExpression langExpression)
	{
		this.functionName = functionName;
		this.columnList = new SqlFullTextColumnCollection(columnList);
		this.searchString = searchString;
		if (langExpression == null)
		{
			return;
		}
		if (!langExpression.IsLiteral && !langExpression.IsVariableReference)
		{
			langExpression.ReportParseError(SR.IncorrectSyntaxNear(langExpression.ToString()));
			return;
		}
		if (langExpression.IsLiteral)
		{
			SqlLiteralExpression sqlLiteralExpression = langExpression as SqlLiteralExpression;
			if (sqlLiteralExpression.Type == LiteralValueType.Integer && !sqlLiteralExpression.AsInt32().HasValue)
			{
				langExpression.ReportParseError(SR.IncorrectSyntaxNear(langExpression.ToString()));
			}
		}
		this.langExpression = langExpression;
	}
}
