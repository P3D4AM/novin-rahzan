using Microsoft.SqlServer.Management.SqlParser.Binder;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlQueryExpression : SqlCodeObject
{
	internal abstract LocalBindingContext LocalBindingContext { get; }

	internal abstract bool HasUnionAll { get; }

	internal abstract void ShallowBind(CteBindingContext bindingContext);

	internal abstract void DeepBind(CteBindingContext bindingContext, SqlIdentifier cteName);

	internal abstract SqlSelectIntoClause GetIntoClause();

	internal abstract void Validate(QueryExpressionValidateOption option);
}
