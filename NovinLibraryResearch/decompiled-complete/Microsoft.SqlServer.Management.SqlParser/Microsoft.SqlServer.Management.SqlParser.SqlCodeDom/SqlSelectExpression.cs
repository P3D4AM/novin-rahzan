namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlSelectExpression : SqlCodeObject
{
	internal abstract SelectExpressionType ExpressionType { get; }

	internal bool IsScalar => IsOfType(SelectExpressionType.Scalar);

	internal bool IsStar => IsOfType(SelectExpressionType.Star);

	internal bool IsVariableAssignment => IsOfType(SelectExpressionType.VariableAssignment);

	internal bool IsError => IsOfType(SelectExpressionType.Error);

	internal virtual SqlSelectScalarExpression AsSelectScalarExpression => null;

	private bool IsOfType(SelectExpressionType type)
	{
		return (ExpressionType & type) == type;
	}
}
