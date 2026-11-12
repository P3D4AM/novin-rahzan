using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlScalarExpression : SqlCodeObject
{
	internal abstract bool IsValidFilterExpressionConstant { get; }

	public abstract IScalar BoundScalar { get; }

	public override IMetadataObject BoundObject => BoundScalar;

	internal abstract ScalarExpressionType ExpressionType { get; }

	internal abstract bool IsConstantExpression { get; }

	internal bool IsBinary => IsOfType(ScalarExpressionType.Binary);

	internal bool IsCase => IsOfType(ScalarExpressionType.Case);

	internal bool IsCollate => IsOfType(ScalarExpressionType.Collate);

	internal bool IsColumnReference => IsOfType(ScalarExpressionType.ColumnReference);

	internal bool IsLiteral => IsOfType(ScalarExpressionType.Literal);

	internal bool IsFunction => IsOfType(ScalarExpressionType.Function);

	internal bool IsGlobalVariableReference => IsOfType(ScalarExpressionType.GlobalVariableReference);

	internal bool IsSubQuery => IsOfType(ScalarExpressionType.SubQuery);

	internal bool IsUnary => IsOfType(ScalarExpressionType.Unary);

	internal bool IsUdt => IsOfType(ScalarExpressionType.Udt);

	internal bool IsVariableReference => IsOfType(ScalarExpressionType.VariableReference);

	internal bool IsNull => IsOfType(ScalarExpressionType.Null);

	internal bool IsError => IsOfType(ScalarExpressionType.Error);

	internal bool IsSimpleExpression => (ExpressionType & (ScalarExpressionType)1104) != 0;

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		if (!IsSimpleExpression)
		{
			return base.GetCompletionList(caretLocation, completionContext);
		}
		return RetrieveCompletionListItems(completionContext);
	}

	internal static SqlScalarExpression Create(SqlScalarExpression expression, SqlCollation collation)
	{
		if (collation == null)
		{
			return expression;
		}
		return new SqlCollateScalarExpression(expression, collation);
	}

	private bool IsOfType(ScalarExpressionType type)
	{
		return (ExpressionType & type) == type;
	}
}
