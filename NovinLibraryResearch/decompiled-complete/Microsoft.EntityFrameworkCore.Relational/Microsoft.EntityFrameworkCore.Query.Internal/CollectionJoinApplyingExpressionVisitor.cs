using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class CollectionJoinApplyingExpressionVisitor : ExpressionVisitor
{
	private int _collectionId;

	protected override Expression VisitExtension(Expression extensionExpression)
	{
		if (extensionExpression is CollectionShaperExpression collectionShaperExpression)
		{
			int collectionId = _collectionId++;
			ProjectionBindingExpression projectionBindingExpression = (ProjectionBindingExpression)collectionShaperExpression.Projection;
			SelectExpression selectExpression = (SelectExpression)projectionBindingExpression.QueryExpression;
			if (selectExpression.IsDistinct || selectExpression.Limit != null || selectExpression.Offset != null || selectExpression.GroupBy.Count > 1)
			{
				selectExpression.PushdownIntoSubquery();
			}
			Expression innerShaper = Visit(collectionShaperExpression.InnerShaper);
			return selectExpression.ApplyCollectionJoin(projectionBindingExpression.Index.Value, collectionId, innerShaper, collectionShaperExpression.Navigation, collectionShaperExpression.ElementType);
		}
		if (extensionExpression is ShapedQueryExpression shapedQueryExpression)
		{
			shapedQueryExpression.ShaperExpression = Visit(shapedQueryExpression.ShaperExpression);
			return shapedQueryExpression;
		}
		return base.VisitExtension(extensionExpression);
	}
}
