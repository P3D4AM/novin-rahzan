using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Query;

public interface IPrintableExpression
{
	void Print([JetBrains.Annotations.NotNull] ExpressionPrinter expressionPrinter);
}
