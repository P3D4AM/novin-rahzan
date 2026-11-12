using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Query;

public interface IEvaluatableExpressionFilter
{
	bool IsEvaluatableExpression(Expression expression, IModel model);
}
