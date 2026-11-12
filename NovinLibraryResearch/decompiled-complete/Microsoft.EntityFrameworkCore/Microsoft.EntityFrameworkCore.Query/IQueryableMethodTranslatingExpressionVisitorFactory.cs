using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Query;

public interface IQueryableMethodTranslatingExpressionVisitorFactory
{
	QueryableMethodTranslatingExpressionVisitor Create(IModel model);
}
