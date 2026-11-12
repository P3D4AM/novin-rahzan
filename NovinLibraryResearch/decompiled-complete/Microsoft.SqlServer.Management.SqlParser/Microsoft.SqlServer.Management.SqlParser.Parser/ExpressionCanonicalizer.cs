using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

internal class ExpressionCanonicalizer : ISqlCodeObjectVisitor<string>, ISqlStatementVisitor<string>
{
	public enum ExpressionType
	{
		Check,
		ComputedColumn,
		Default,
		IndexFilter
	}

	public static ExpressionCanonicalizer Instance;

	private static Dictionary<SqlBooleanOperatorType, string> binaryBooleanOperators;

	private static Dictionary<SqlBinaryScalarOperatorType, string> binaryOperators;

	private static Dictionary<SqlComparisonBooleanExpressionType, string> comparisonOperators;

	private static Dictionary<SqlUnaryScalarOperatorType, string> unaryOperators;

	private ExpressionCanonicalizer()
	{
	}

	static ExpressionCanonicalizer()
	{
		Instance = new ExpressionCanonicalizer();
		binaryBooleanOperators = new Dictionary<SqlBooleanOperatorType, string>();
		binaryBooleanOperators.Add(SqlBooleanOperatorType.And, "AND");
		binaryBooleanOperators.Add(SqlBooleanOperatorType.Or, "OR");
		binaryOperators = new Dictionary<SqlBinaryScalarOperatorType, string>();
		binaryOperators.Add(SqlBinaryScalarOperatorType.Add, "+");
		binaryOperators.Add(SqlBinaryScalarOperatorType.BitwiseAnd, "&");
		binaryOperators.Add(SqlBinaryScalarOperatorType.BitwiseOr, "|");
		binaryOperators.Add(SqlBinaryScalarOperatorType.BitwiseXor, "^");
		binaryOperators.Add(SqlBinaryScalarOperatorType.Divide, "/");
		binaryOperators.Add(SqlBinaryScalarOperatorType.GreaterThan, ">");
		binaryOperators.Add(SqlBinaryScalarOperatorType.GreaterThanOrEqual, ">=");
		binaryOperators.Add(SqlBinaryScalarOperatorType.LessThan, "<");
		binaryOperators.Add(SqlBinaryScalarOperatorType.LessThanOrEqual, "<=");
		binaryOperators.Add(SqlBinaryScalarOperatorType.Modulus, "%");
		binaryOperators.Add(SqlBinaryScalarOperatorType.Multiply, "*");
		binaryOperators.Add(SqlBinaryScalarOperatorType.NotEqualTo, "<>");
		binaryOperators.Add(SqlBinaryScalarOperatorType.NotGreaterThan, "!>");
		binaryOperators.Add(SqlBinaryScalarOperatorType.NotLessThan, "!<");
		binaryOperators.Add(SqlBinaryScalarOperatorType.Subtract, "-");
		comparisonOperators = new Dictionary<SqlComparisonBooleanExpressionType, string>();
		comparisonOperators.Add(SqlComparisonBooleanExpressionType.Equals, "=");
		comparisonOperators.Add(SqlComparisonBooleanExpressionType.GreaterThan, ">");
		comparisonOperators.Add(SqlComparisonBooleanExpressionType.GreaterThanOrEqual, ">=");
		comparisonOperators.Add(SqlComparisonBooleanExpressionType.LessThan, "<=");
		comparisonOperators.Add(SqlComparisonBooleanExpressionType.LessThanOrEqual, "<=");
		comparisonOperators.Add(SqlComparisonBooleanExpressionType.NotEqual, "<>");
		comparisonOperators.Add(SqlComparisonBooleanExpressionType.NotGreaterThan, "!>");
		comparisonOperators.Add(SqlComparisonBooleanExpressionType.NotLessThan, "!<");
		comparisonOperators.Add(SqlComparisonBooleanExpressionType.LessOrGreaterThan, "<>");
		unaryOperators = new Dictionary<SqlUnaryScalarOperatorType, string>();
		unaryOperators.Add(SqlUnaryScalarOperatorType.BitwiseNot, "~");
		unaryOperators.Add(SqlUnaryScalarOperatorType.Positive, "-");
		unaryOperators.Add(SqlUnaryScalarOperatorType.Negative, "+");
	}

	public string Canonicalize(string expression, ExpressionType type)
	{
		return type switch
		{
			ExpressionType.Check => CanonicalizeCheckConstraintExpression(expression), 
			ExpressionType.ComputedColumn => CanonicalizeComputedColumnExpression(expression), 
			ExpressionType.Default => CanonicalizeDefaultConstraintExpression(expression), 
			ExpressionType.IndexFilter => CanonicalizeIndexFilterExpression(expression), 
			_ => expression, 
		};
	}

	private string CanonicalizeCheckConstraintExpression(string expression)
	{
		string sql = "CREATE TABLE T1(id int check (" + Environment.NewLine + expression + Environment.NewLine + "));";
		ParseResult parseResult = Parse(sql);
		if (parseResult.Errors.Count() != 0)
		{
			return expression;
		}
		return ((SqlCheckConstraint)((SqlCreateTableStatement)parseResult.Script.Batches[0].Statements[0]).Definition.ColumnDefinitions.First().Constraints[0]).Expression.Accept(this);
	}

	private string CanonicalizeComputedColumnExpression(string expression)
	{
		string sql = "CREATE TABLE T1(id as (" + Environment.NewLine + expression + Environment.NewLine + "));";
		return ((SqlComputedColumnDefinition)((SqlCreateTableStatement)Parse(sql).Script.Batches[0].Statements[0]).Definition.ColumnDefinitions.First()).Expression.Accept(this);
	}

	private string CanonicalizeDefaultConstraintExpression(string expression)
	{
		string sql = "CREATE TABLE T1(id int default " + Environment.NewLine + expression + Environment.NewLine + ");";
		return ((SqlDefaultConstraint)((SqlCreateTableStatement)Parse(sql).Script.Batches[0].Statements[0]).Definition.ColumnDefinitions.First().Constraints[0]).Expression.Accept(this);
	}

	private string CanonicalizeIndexFilterExpression(string expression)
	{
		string sql = "CREATE INDEX I1 ON T1(id) WHERE (" + Environment.NewLine + expression + Environment.NewLine + ");";
		return ((SqlCreateIndexStatement)Parse(sql).Script.Batches[0].Statements[0]).FilterClause.FilterExpression.Accept(this);
	}

	private ParseResult Parse(string sql)
	{
		ParseOptions parseOptions = new ParseOptions();
		parseOptions.TransactSqlVersion = TransactSqlVersion.Version105;
		return Parser.Parse(sql, parseOptions);
	}

	public string Visit(SqlLiteralExpression expression)
	{
		switch (expression.Type)
		{
		case LiteralValueType.Binary:
			return CanonicalizeBinaryExpression(expression);
		case LiteralValueType.Money:
			return CanonicalizeMoneyExpression(expression);
		case LiteralValueType.Real:
			return CanonicalizeRealExpression(expression);
		case LiteralValueType.Numeric:
			return CanonicalizeNumericExpression(expression);
		case LiteralValueType.Integer:
		case LiteralValueType.Null:
			return expression.Value;
		case LiteralValueType.String:
			return Append("'", expression.Value, "'");
		case LiteralValueType.UnicodeString:
			return Append("N'", expression.Value, "'");
		default:
			return expression.Sql;
		}
	}

	private string CanonicalizeBinaryExpression(SqlLiteralExpression expression)
	{
		string str = ((expression.Value.Length % 2 == 1) ? ("0x0" + expression.Value.Substring(2)) : expression.Value);
		return CultureInfo.InvariantCulture.TextInfo.ToUpper(str);
	}

	private string CanonicalizeNumericExpression(SqlLiteralExpression expression)
	{
		if (expression.Value.StartsWith(".", StringComparison.Ordinal))
		{
			return "0" + expression.Value;
		}
		if (!expression.Value.Contains("."))
		{
			return expression.Value + ".";
		}
		return expression.Value;
	}

	private string CanonicalizeRealExpression(SqlLiteralExpression expression)
	{
		double result;
		string str = (double.TryParse(expression.Value.EndsWith("e", StringComparison.OrdinalIgnoreCase) ? (expression.Value + "0") : expression.Value, out result) ? result.ToString(CultureInfo.InvariantCulture) : expression.Value);
		return CultureInfo.InvariantCulture.TextInfo.ToUpper(str);
	}

	private string CanonicalizeMoneyExpression(SqlLiteralExpression expression)
	{
		if (decimal.TryParse(expression.Value, NumberStyles.Integer | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowDecimalPoint | NumberStyles.AllowCurrencySymbol, NumberFormatInfo.InvariantInfo, out var result))
		{
			return result.ToString("C4", CultureInfo.InvariantCulture);
		}
		if (decimal.TryParse(expression.Value, NumberStyles.Currency, NumberFormatInfo.CurrentInfo, out result))
		{
			return result.ToString("C4", CultureInfo.InvariantCulture);
		}
		return expression.Sql;
	}

	public string Visit(SqlUserDefinedScalarFunctionCallExpression expression)
	{
		string s = string.Empty;
		if (expression.ObjectIdentifier.SchemaName != null)
		{
			s = Append(expression.ObjectIdentifier.SchemaName.Value, ".");
		}
		return Append(s, expression.ObjectIdentifier.Accept(this), Parenthesize(CanonicalizeArguments(expression)));
	}

	public string Visit(SqlBuiltinScalarFunctionCallExpression expression)
	{
		return Append(expression.FunctionName.Equals("user", StringComparison.OrdinalIgnoreCase) ? "user_name" : expression.FunctionName, Parenthesize(expression.IsStar ? "*" : CanonicalizeArguments(expression)));
	}

	private string CanonicalizeArguments(SqlScalarFunctionCallExpression expression)
	{
		string text = string.Empty;
		if (expression.Arguments != null)
		{
			int num = 0;
			foreach (SqlScalarExpression argument in expression.Arguments)
			{
				text = Append(text, argument.Accept(this));
				if (num < expression.Arguments.Count - 1)
				{
					text = Append(text, ",");
				}
			}
		}
		return text;
	}

	public string Visit(SqlCastExpression expression)
	{
		return CanonicalizeCastAndConvertFunctionCallExpressions(expression);
	}

	public string Visit(SqlConvertExpression expression)
	{
		return CanonicalizeCastAndConvertFunctionCallExpressions(expression);
	}

	private string CanonicalizeCastAndConvertFunctionCallExpressions(SqlCastExpression expression)
	{
		string text = string.Empty;
		if (expression.DataType.Argument1.HasValue)
		{
			text = GetBinaryExpression(expression.DataType.Argument1.Value.ToString(CultureInfo.InvariantCulture), ",", expression.DataType.Argument2.HasValue ? expression.DataType.Argument2.Value.ToString(CultureInfo.InvariantCulture) : string.Empty);
		}
		return Append("CONVERT", Parenthesize(Append(expression.DataType.DataType.ObjectIdentifier.ObjectName.Value, text, ",", expression.Arguments[0].Accept(this), ",", "0")));
	}

	public string Visit(SqlCollateScalarExpression collateExpression)
	{
		return GetBinaryExpression(collateExpression.Expression.Accept(this), "COLLATE", collateExpression.Collation.Name.Value);
	}

	public string Visit(SqlScalarRefExpression expression)
	{
		return expression.MultipartIdentifier.Accept(this);
	}

	public string Visit(SqlColumnRefExpression expression)
	{
		return expression.MultipartIdentifier.Accept(this);
	}

	public string Visit(SqlIdentifier identifier)
	{
		return EscapeSequence.BracketedEscapeSequence.Escape(identifier.Value);
	}

	public string Visit(SqlObjectIdentifier objectIdentifier)
	{
		string[] value = objectIdentifier.Select((SqlIdentifier identifier) => identifier.Accept(this)).ToArray();
		return string.Join(".", value);
	}

	public string Visit(SqlSearchedCaseExpression searchedCaseExpression)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("CASE");
		foreach (SqlSearchedWhenClause whenClause in searchedCaseExpression.WhenClauses)
		{
			stringBuilder.AppendLine(whenClause.Accept(this));
		}
		if (searchedCaseExpression.ElseExpression != null)
		{
			stringBuilder.AppendLine(searchedCaseExpression.ElseExpression.Accept(this));
		}
		return stringBuilder.ToString();
	}

	public string Visit(SqlSearchedWhenClause searchedWhenClause)
	{
		return Append("WHEN", searchedWhenClause.WhenExpression.Accept(this), "THEN", searchedWhenClause.ThenExpression.Accept(this));
	}

	public string Visit(SqlSimpleCaseExpression simpleCaseExpression)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("CASE ");
		stringBuilder.AppendLine(simpleCaseExpression.TestExpression.Accept(this));
		foreach (SqlSimpleWhenClause whenClause in simpleCaseExpression.WhenClauses)
		{
			stringBuilder.AppendLine(whenClause.Accept(this));
		}
		if (simpleCaseExpression.ElseExpression != null)
		{
			stringBuilder.AppendLine(simpleCaseExpression.ElseExpression.Accept(this));
		}
		return stringBuilder.ToString();
	}

	public string Visit(SqlSimpleWhenClause simpleWhenClause)
	{
		return Append("WHEN", simpleWhenClause.WhenExpression.Accept(this), "THEN", simpleWhenClause.ThenExpression.Accept(this));
	}

	public string Visit(SqlAtTimeZoneExpression expression)
	{
		return GetBinaryExpression(expression.DateValue.Accept(this), "AT TIME ZONE", expression.TimeZone.Accept(this));
	}

	public string Visit(SqlBinaryScalarExpression expression)
	{
		if (binaryOperators.TryGetValue(expression.Operator, out var value))
		{
			return GetBinaryExpression(expression.Left.Accept(this), value, expression.Right.Accept(this));
		}
		return expression.Sql;
	}

	public string Visit(SqlUnaryScalarExpression expression)
	{
		if (unaryOperators.TryGetValue(expression.Operator, out var value))
		{
			return GetUnaryExpression(value, expression.Expression.Accept(this));
		}
		return expression.Sql;
	}

	public string Visit(SqlBetweenBooleanExpression expression)
	{
		string left = expression.TestExpression.Accept(this);
		string binaryExpression = GetBinaryExpression(left, ">=", expression.BeginExpression.Accept(this));
		string binaryExpression2 = GetBinaryExpression(left, "<=", expression.EndExpression.Accept(this));
		string binaryExpression3 = GetBinaryExpression(binaryExpression, "AND", binaryExpression2);
		if (expression.HasNot)
		{
			return GetNotBooleanExpression(binaryExpression3);
		}
		return binaryExpression3;
	}

	public string Visit(SqlBinaryBooleanExpression expression)
	{
		if (!binaryBooleanOperators.TryGetValue(expression.Operator, out var value))
		{
			return expression.Sql;
		}
		return GetBinaryExpression(expression.Left.Accept(this), value, expression.Right.Accept(this));
	}

	public string Visit(SqlComparisonBooleanExpression expression)
	{
		if (!comparisonOperators.TryGetValue(expression.ComparisonOperator, out var value))
		{
			return expression.Sql;
		}
		return GetBinaryExpression(expression.Left.Accept(this), value, expression.Right.Accept(this));
	}

	public string Visit(SqlInBooleanExpression expression)
	{
		if (expression.ComparisonValue is SqlInBooleanExpressionCollectionValue sqlInBooleanExpressionCollectionValue)
		{
			string text = string.Empty;
			string left = expression.InExpression.Accept(this);
			if (sqlInBooleanExpressionCollectionValue.Values.Count > 0)
			{
				text = GetBinaryExpression(left, "=", sqlInBooleanExpressionCollectionValue.Values[sqlInBooleanExpressionCollectionValue.Values.Count - 1].Accept(this));
				for (int num = sqlInBooleanExpressionCollectionValue.Values.Count - 2; num >= 0; num--)
				{
					text = GetBinaryExpression(text, "OR", GetBinaryExpression(left, "=", sqlInBooleanExpressionCollectionValue.Values[num].Accept(this)));
				}
			}
			if (expression.HasNot)
			{
				text = GetNotBooleanExpression(text);
			}
			return text;
		}
		return expression.Sql;
	}

	public string Visit(SqlIsNullBooleanExpression expression)
	{
		string s = Append(expression.Expression.Accept(this), "IS");
		if (expression.HasNot)
		{
			s = Append(s, "NOT");
		}
		return Append(s, "NULL");
	}

	public string Visit(SqlLikeBooleanExpression expression)
	{
		string binaryExpression = GetBinaryExpression(expression.Expression.Accept(this), "LIKE", expression.LikePattern.Accept(this));
		if (expression.EscapeClause != null)
		{
			binaryExpression = GetBinaryExpression(binaryExpression, "ESCAPE", expression.EscapeClause.Accept(this));
		}
		if (expression.HasNot)
		{
			return GetNotBooleanExpression(binaryExpression);
		}
		return binaryExpression;
	}

	public string Visit(SqlNotBooleanExpression expression)
	{
		return GetNotBooleanExpression(expression.Expression.Accept(this));
	}

	private string GetNotBooleanExpression(string expression)
	{
		return GetUnaryExpression("NOT", expression);
	}

	public string Visit(SqlAllAnyComparisonBooleanExpression expression)
	{
		return expression.Sql;
	}

	public string Visit(SqlExistsBooleanExpression expression)
	{
		return expression.Sql;
	}

	public string Visit(SqlFullTextBooleanExpression expression)
	{
		return expression.Sql;
	}

	public string Visit(SqlUpdateBooleanExpression expression)
	{
		return expression.Sql;
	}

	public string Visit(SqlBinaryFilterExpression expression)
	{
		return GetBinaryExpression(expression.Left.Accept(this), "AND", expression.Right.Accept(this));
	}

	public string Visit(SqlBooleanFilterExpression expression)
	{
		return expression.Expression.Accept(this);
	}

	private static string GetBinaryExpression(string left, string operatorString, string right)
	{
		return Parenthesize(Append(left, operatorString, right));
	}

	private static string GetUnaryExpression(string operatorString, string operand)
	{
		return Parenthesize(Append(operatorString, operand));
	}

	private static string Append(params string[] strings)
	{
		string text = strings[0];
		for (int i = 1; i < strings.Length; i++)
		{
			text = Append(text, strings[i]);
		}
		return text;
	}

	private static string Append(string s1, string s2, string s3)
	{
		return Append(Append(s1, s2), s3);
	}

	private static string Append(string s1, string s2)
	{
		if (s1.Length == 0)
		{
			return s2;
		}
		if (s2.Length == 0)
		{
			return s1;
		}
		return s1 + " " + s2;
	}

	private static string Parenthesize(string expression)
	{
		return "(" + expression + ")";
	}

	public string Visit(SqlAggregateFunctionCallExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlAllowPageLocksIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlAllowRowLocksIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlAlterLoginStatement codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlAssignment codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlBatch codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlBinaryQueryExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlBooleanExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlChangeTrackingContext codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlCheckConstraint codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlClrAssemblySpecifier codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlClrClassSpecifier codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlClrFunctionBodyDefinition codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlClrMethodSpecifier codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlCollation codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlColumnAssignment codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlCreateUserOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlExternalStreamDefinition codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlCursorDeclareStatement codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlCursorOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlCursorVariableAssignment codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlCursorVariableRefExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlDefaultConstraint codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlInlineIndexConstraint codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlColumnDefinition codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlColumnIdentity codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlCommonTableExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlCompressionPartitionRange codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlComputedColumnDefinition codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlConditionClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlConstraint codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlCubeGroupByItem codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlDataCompressionIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlDataType codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlDataTypeSpecification codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlDdlTriggerDefinition codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlDefaultValuesInsertMergeActionSource codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlDefaultValuesInsertSource codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlDeleteMergeAction codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlDeleteSpecification codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlDerivedTableExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlDmlSpecificationTableSource codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlDmlTriggerDefinition codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlDropExistingIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlExecuteAsClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlFillFactorIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlFilterClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlForBrowseClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlForeignKeyConstraint codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlForXmlAutoClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlForXmlClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlForXmlDirective codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlForXmlExplicitClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlForXmlPathClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlForXmlRawClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlFromClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlFullTextColumn codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlFunctionDefinition codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlGlobalScalarVariableRefExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlGrandTotalGroupByItem codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlGrandTotalGroupingSet codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlGroupByClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlGroupBySets codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlGroupingSetItemsCollection codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlHavingClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlIdentityFunctionCallExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlIgnoreDupKeyIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlInBooleanExpressionCollectionValue codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlInBooleanExpressionQueryValue codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlIndexedColumn codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlIndexHint codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlInlineFunctionBodyDefinition codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlInlineTableRelationalFunctionDefinition codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlInlineTableVariableDeclaration codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlInsertMergeAction codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlInsertSpecification codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlIntoClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlLargeDataStorageInformation codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlLoginPassword codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlMaxDegreeOfParallelismIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlMaxDurationIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlMergeActionClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlMergeSpecification codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlInsertSource codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlExecuteArgument codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlModuleCalledOnNullInputOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlModuleEncryptionOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlModuleExecuteAsOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlModuleOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlModuleRecompileOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlModuleReturnsNullOnNullInputOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlModuleSchemaBindingOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlModuleNativeCompilationOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlModuleViewMetadataOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlMultistatementFunctionBodyDefinition codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlMultistatementTableRelationalFunctionDefinition codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlQueryExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlScalarExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlTableExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlObjectReference codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlOnlineIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlOptimizeForSequentialKeyIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlResumableIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlBucketCountIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlCompressionDelayIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlOffsetFetchClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlOrderByClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlOrderByItem codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlOutputClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlOutputIntoClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlPadIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlParameterDeclaration codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlPivotClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlPivotTableExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlPrimaryKeyConstraint codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlStorageSpecification codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlProcedureDefinition codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlQualifiedJoinTableExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlQuerySpecification codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlQueryWithClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlRollupGroupByItem codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlRowConstructorExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlScalarClrFunctionDefinition codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlScalarFunctionReturnType codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlScalarRelationalFunctionDefinition codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlScalarSubQueryExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlScalarVariableRefExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlScript codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlSelectClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlSelectIntoClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlSelectScalarExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlSelectSpecification codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlSelectSpecificationInsertSource codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlSelectStarExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlSelectVariableAssignmentExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlSetClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlSimpleGroupByItem codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlSimpleOrderByClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlSimpleOrderByItem codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlSortedDataIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlSortedDataReorgIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlSortInTempDbIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlStatisticsIncrementalIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlStatisticsNoRecomputeIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlStatisticsOnlyIndexOption codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlTableClrFunctionDefinition codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlTableConstructorExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlTableConstructorInsertSource codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlTableDefinition codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlTableFunctionReturnType codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlTableHint codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlTableRefExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlTableUdtInstanceMethodExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlTableValuedFunctionRefExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlTableVariableRefExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlTargetTableExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlTopSpecification codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlTriggerAction codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlTriggerDefinition codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlTriggerEvent codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlUdtInstanceDataMemberExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlUdtInstanceMethodExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlUdtStaticDataMemberExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlUdtStaticMethodExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlUniqueConstraint codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlUnpivotClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlUnpivotTableExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlUnqualifiedJoinTableExpression codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlUpdateMergeAction codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlUpdateSpecification codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlValuesInsertMergeActionSource codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlScalarVariableAssignment codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlVariableColumnAssignment codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlVariableDeclaration codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlViewDefinition codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlWhereClause codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlXmlNamespacesDeclaration codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlAlterFunctionStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlAlterProcedureStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlAlterTriggerStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlAlterViewStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlBackupCertificateStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlBackupDatabaseStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlBackupLogStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlBackupMasterKeyStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlBackupServiceMasterKeyStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlBackupTableStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlBreakStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCommentStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCompoundStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlContinueStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateFunctionStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateIndexStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateLoginFromAsymKeyStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateLoginFromCertificateStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateLoginFromWindowsStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateLoginWithPasswordStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateProcedureStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateRoleStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateSchemaStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateSynonymStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateTableStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateTriggerStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateUserDefinedDataTypeStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateUserDefinedTableTypeStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateUserDefinedTypeStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateUserFromAsymKeyStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateUserFromCertificateStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateUserWithImplicitAuthenticationStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateUserFromLoginStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateUserFromExternalProviderStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateUserStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateUserWithoutLoginStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlCreateViewStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlDBCCStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlDeleteStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlDenyStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlDropAggregateStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlDropDatabaseStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlDropDefaultStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlDropFunctionStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlDropLoginStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlDropProcedureStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlDropRuleStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlDropSchemaStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlDropSecurityPolicyStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlDropSequenceStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlDropSynonymStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlDropTableStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlDropTriggerStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlDropTypeStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlDropUserStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlDropViewStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlExecuteModuleStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlExecuteStringStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlGrantStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlIfElseStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlInlineTableVariableDeclareStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlInsertStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlMergeStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlRestoreDatabaseStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlRestoreInformationStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlRestoreLogStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlRestoreMasterKeyStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlRestoreServiceMasterKeyStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlRestoreTableStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlReturnStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlRevokeStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlSelectStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlSetAssignmentStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlSetStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlTemporalPeriodDefinition codeObject)
	{
		return codeObject.Sql;
	}

	public string Visit(SqlTryCatchStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlUpdateStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlUseStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlVariableDeclareStatement statement)
	{
		return statement.Sql;
	}

	public string Visit(SqlWhileStatement statement)
	{
		return statement.Sql;
	}
}
