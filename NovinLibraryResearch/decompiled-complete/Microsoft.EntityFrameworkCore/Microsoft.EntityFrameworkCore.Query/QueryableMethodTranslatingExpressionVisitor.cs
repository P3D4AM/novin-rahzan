using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace Microsoft.EntityFrameworkCore.Query;

public abstract class QueryableMethodTranslatingExpressionVisitor : ExpressionVisitor
{
	private class EntityShaperNullableMarkingExpressionVisitor : ExpressionVisitor
	{
		protected override Expression VisitExtension(Expression extensionExpression)
		{
			if (!(extensionExpression is EntityShaperExpression entityShaperExpression))
			{
				return base.VisitExtension(extensionExpression);
			}
			return entityShaperExpression.MarkAsNullable();
		}
	}

	private class MemberAccessShiftingExpressionVisitor : ExpressionVisitor
	{
		private readonly Expression _queryExpression;

		private readonly MemberInfo _memberShift;

		public MemberAccessShiftingExpressionVisitor(Expression queryExpression, MemberInfo memberShift)
		{
			_queryExpression = queryExpression;
			_memberShift = memberShift;
		}

		protected override Expression VisitExtension(Expression node)
		{
			if (node is ProjectionBindingExpression projectionBindingExpression)
			{
				return new ProjectionBindingExpression(_queryExpression, projectionBindingExpression.ProjectionMember.Prepend(_memberShift), projectionBindingExpression.Type);
			}
			return base.VisitExtension(node);
		}
	}

	private readonly bool _subquery;

	private readonly EntityShaperNullableMarkingExpressionVisitor _entityShaperNullableMarkingExpressionVisitor;

	protected virtual QueryableMethodTranslatingExpressionVisitorDependencies Dependencies { get; }

	protected QueryableMethodTranslatingExpressionVisitor(QueryableMethodTranslatingExpressionVisitorDependencies dependencies, bool subquery)
	{
		Dependencies = dependencies;
		_subquery = subquery;
		_entityShaperNullableMarkingExpressionVisitor = new EntityShaperNullableMarkingExpressionVisitor();
	}

	protected override Expression VisitConstant(ConstantExpression constantExpression)
	{
		if (!constantExpression.IsEntityQueryable())
		{
			return base.VisitConstant(constantExpression);
		}
		return CreateShapedQueryExpression(((IQueryable)constantExpression.Value).ElementType);
	}

	protected override Expression VisitExtension(Expression expression)
	{
		if (!(expression is ShapedQueryExpression))
		{
			return base.VisitExtension(expression);
		}
		return expression;
	}

	protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
	{
		MethodInfo method = methodCallExpression.Method;
		if (method.DeclaringType == typeof(Queryable) || method.DeclaringType == typeof(QueryableExtensions))
		{
			Expression expression = Visit(methodCallExpression.Arguments[0]);
			if (expression is ShapedQueryExpression shapedQueryExpression)
			{
				MethodInfo methodInfo = (method.IsGenericMethod ? method.GetGenericMethodDefinition() : null);
				switch (method.Name)
				{
				case "All":
					if (methodInfo == QueryableMethods.All)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateAll(shapedQueryExpression, GetLambdaExpressionFromArgument(1)));
					}
					break;
				case "Any":
					if (methodInfo == QueryableMethods.AnyWithoutPredicate)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateAny(shapedQueryExpression, null));
					}
					if (methodInfo == QueryableMethods.AnyWithPredicate)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateAny(shapedQueryExpression, GetLambdaExpressionFromArgument(1)));
					}
					break;
				case "AsQueryable":
					if (methodInfo == QueryableMethods.AsQueryable)
					{
						return expression;
					}
					break;
				case "Average":
					if (QueryableMethods.IsAverageWithoutSelector(method))
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateAverage(shapedQueryExpression, null, methodCallExpression.Type));
					}
					if (QueryableMethods.IsAverageWithSelector(method))
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateAverage(shapedQueryExpression, GetLambdaExpressionFromArgument(1), methodCallExpression.Type));
					}
					break;
				case "Cast":
					if (methodInfo == QueryableMethods.Cast)
					{
						return CheckTranslated(TranslateCast(shapedQueryExpression, method.GetGenericArguments()[0]));
					}
					break;
				case "Concat":
					if (methodInfo == QueryableMethods.Concat && Visit(methodCallExpression.Arguments[1]) is ShapedQueryExpression source3)
					{
						return TranslateConcat(shapedQueryExpression, source3);
					}
					break;
				case "Contains":
					if (methodInfo == QueryableMethods.Contains)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateContains(shapedQueryExpression, methodCallExpression.Arguments[1]));
					}
					break;
				case "Count":
					if (methodInfo == QueryableMethods.CountWithoutPredicate)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateCount(shapedQueryExpression, null));
					}
					if (methodInfo == QueryableMethods.CountWithPredicate)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateCount(shapedQueryExpression, GetLambdaExpressionFromArgument(1)));
					}
					break;
				case "DefaultIfEmpty":
					if (methodInfo == QueryableMethods.DefaultIfEmptyWithoutArgument)
					{
						return CheckTranslated(TranslateDefaultIfEmpty(shapedQueryExpression, null));
					}
					if (methodInfo == QueryableMethods.DefaultIfEmptyWithArgument)
					{
						return CheckTranslated(TranslateDefaultIfEmpty(shapedQueryExpression, methodCallExpression.Arguments[1]));
					}
					break;
				case "Distinct":
					if (methodInfo == QueryableMethods.Distinct)
					{
						return CheckTranslated(TranslateDistinct(shapedQueryExpression));
					}
					break;
				case "ElementAt":
					if (methodInfo == QueryableMethods.ElementAt)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateElementAtOrDefault(shapedQueryExpression, methodCallExpression.Arguments[1], returnDefault: false));
					}
					break;
				case "ElementAtOrDefault":
					if (methodInfo == QueryableMethods.ElementAtOrDefault)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.SingleOrDefault;
						return CheckTranslated(TranslateElementAtOrDefault(shapedQueryExpression, methodCallExpression.Arguments[1], returnDefault: true));
					}
					break;
				case "Except":
					if (methodInfo == QueryableMethods.Except && Visit(methodCallExpression.Arguments[1]) is ShapedQueryExpression source4)
					{
						return CheckTranslated(TranslateExcept(shapedQueryExpression, source4));
					}
					break;
				case "First":
					if (methodInfo == QueryableMethods.FirstWithoutPredicate)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateFirstOrDefault(shapedQueryExpression, null, methodCallExpression.Type, returnDefault: false));
					}
					if (methodInfo == QueryableMethods.FirstWithPredicate)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateFirstOrDefault(shapedQueryExpression, GetLambdaExpressionFromArgument(1), methodCallExpression.Type, returnDefault: false));
					}
					break;
				case "FirstOrDefault":
					if (methodInfo == QueryableMethods.FirstOrDefaultWithoutPredicate)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.SingleOrDefault;
						return CheckTranslated(TranslateFirstOrDefault(shapedQueryExpression, null, methodCallExpression.Type, returnDefault: true));
					}
					if (methodInfo == QueryableMethods.FirstOrDefaultWithPredicate)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.SingleOrDefault;
						return CheckTranslated(TranslateFirstOrDefault(shapedQueryExpression, GetLambdaExpressionFromArgument(1), methodCallExpression.Type, returnDefault: true));
					}
					break;
				case "GroupBy":
					if (methodInfo == QueryableMethods.GroupByWithKeySelector)
					{
						return CheckTranslated(TranslateGroupBy(shapedQueryExpression, GetLambdaExpressionFromArgument(1), null, null));
					}
					if (methodInfo == QueryableMethods.GroupByWithKeyElementSelector)
					{
						return CheckTranslated(TranslateGroupBy(shapedQueryExpression, GetLambdaExpressionFromArgument(1), GetLambdaExpressionFromArgument(2), null));
					}
					if (methodInfo == QueryableMethods.GroupByWithKeyElementResultSelector)
					{
						return CheckTranslated(TranslateGroupBy(shapedQueryExpression, GetLambdaExpressionFromArgument(1), GetLambdaExpressionFromArgument(2), GetLambdaExpressionFromArgument(3)));
					}
					if (methodInfo == QueryableMethods.GroupByWithKeyResultSelector)
					{
						return CheckTranslated(TranslateGroupBy(shapedQueryExpression, GetLambdaExpressionFromArgument(1), null, GetLambdaExpressionFromArgument(2)));
					}
					break;
				case "GroupJoin":
					if (methodInfo == QueryableMethods.GroupJoin && Visit(methodCallExpression.Arguments[1]) is ShapedQueryExpression inner)
					{
						return CheckTranslated(TranslateGroupJoin(shapedQueryExpression, inner, GetLambdaExpressionFromArgument(2), GetLambdaExpressionFromArgument(3), GetLambdaExpressionFromArgument(4)));
					}
					break;
				case "Intersect":
					if (methodInfo == QueryableMethods.Intersect && Visit(methodCallExpression.Arguments[1]) is ShapedQueryExpression source2)
					{
						return CheckTranslated(TranslateIntersect(shapedQueryExpression, source2));
					}
					break;
				case "Join":
					if (methodInfo == QueryableMethods.Join && Visit(methodCallExpression.Arguments[1]) is ShapedQueryExpression inner3)
					{
						return CheckTranslated(TranslateJoin(shapedQueryExpression, inner3, GetLambdaExpressionFromArgument(2), GetLambdaExpressionFromArgument(3), GetLambdaExpressionFromArgument(4)));
					}
					break;
				case "LeftJoin":
					if (methodInfo == QueryableExtensions.LeftJoinMethodInfo && Visit(methodCallExpression.Arguments[1]) is ShapedQueryExpression inner2)
					{
						return CheckTranslated(TranslateLeftJoin(shapedQueryExpression, inner2, GetLambdaExpressionFromArgument(2), GetLambdaExpressionFromArgument(3), GetLambdaExpressionFromArgument(4)));
					}
					break;
				case "Last":
					if (methodInfo == QueryableMethods.LastWithoutPredicate)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateLastOrDefault(shapedQueryExpression, null, methodCallExpression.Type, returnDefault: false));
					}
					if (methodInfo == QueryableMethods.LastWithPredicate)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateLastOrDefault(shapedQueryExpression, GetLambdaExpressionFromArgument(1), methodCallExpression.Type, returnDefault: false));
					}
					break;
				case "LastOrDefault":
					if (methodInfo == QueryableMethods.LastOrDefaultWithoutPredicate)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.SingleOrDefault;
						return CheckTranslated(TranslateLastOrDefault(shapedQueryExpression, null, methodCallExpression.Type, returnDefault: true));
					}
					if (methodInfo == QueryableMethods.LastOrDefaultWithPredicate)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.SingleOrDefault;
						return CheckTranslated(TranslateLastOrDefault(shapedQueryExpression, GetLambdaExpressionFromArgument(1), methodCallExpression.Type, returnDefault: true));
					}
					break;
				case "LongCount":
					if (methodInfo == QueryableMethods.LongCountWithoutPredicate)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateLongCount(shapedQueryExpression, null));
					}
					if (methodInfo == QueryableMethods.LongCountWithPredicate)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateLongCount(shapedQueryExpression, GetLambdaExpressionFromArgument(1)));
					}
					break;
				case "Max":
					if (methodInfo == QueryableMethods.MaxWithoutSelector)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateMax(shapedQueryExpression, null, methodCallExpression.Type));
					}
					if (methodInfo == QueryableMethods.MaxWithSelector)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateMax(shapedQueryExpression, GetLambdaExpressionFromArgument(1), methodCallExpression.Type));
					}
					break;
				case "Min":
					if (methodInfo == QueryableMethods.MinWithoutSelector)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateMin(shapedQueryExpression, null, methodCallExpression.Type));
					}
					if (methodInfo == QueryableMethods.MinWithSelector)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateMin(shapedQueryExpression, GetLambdaExpressionFromArgument(1), methodCallExpression.Type));
					}
					break;
				case "OfType":
					if (methodInfo == QueryableMethods.OfType)
					{
						return CheckTranslated(TranslateOfType(shapedQueryExpression, method.GetGenericArguments()[0]));
					}
					break;
				case "OrderBy":
					if (methodInfo == QueryableMethods.OrderBy)
					{
						return CheckTranslated(TranslateOrderBy(shapedQueryExpression, GetLambdaExpressionFromArgument(1), ascending: true));
					}
					break;
				case "OrderByDescending":
					if (methodInfo == QueryableMethods.OrderByDescending)
					{
						return CheckTranslated(TranslateOrderBy(shapedQueryExpression, GetLambdaExpressionFromArgument(1), ascending: false));
					}
					break;
				case "Reverse":
					if (methodInfo == QueryableMethods.Reverse)
					{
						return CheckTranslated(TranslateReverse(shapedQueryExpression));
					}
					break;
				case "Select":
					if (methodInfo == QueryableMethods.Select)
					{
						return CheckTranslated(TranslateSelect(shapedQueryExpression, GetLambdaExpressionFromArgument(1)));
					}
					break;
				case "SelectMany":
					if (methodInfo == QueryableMethods.SelectManyWithoutCollectionSelector)
					{
						return CheckTranslated(TranslateSelectMany(shapedQueryExpression, GetLambdaExpressionFromArgument(1)));
					}
					if (methodInfo == QueryableMethods.SelectManyWithCollectionSelector)
					{
						return CheckTranslated(TranslateSelectMany(shapedQueryExpression, GetLambdaExpressionFromArgument(1), GetLambdaExpressionFromArgument(2)));
					}
					break;
				case "Single":
					if (methodInfo == QueryableMethods.SingleWithoutPredicate)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateSingleOrDefault(shapedQueryExpression, null, methodCallExpression.Type, returnDefault: false));
					}
					if (methodInfo == QueryableMethods.SingleWithPredicate)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateSingleOrDefault(shapedQueryExpression, GetLambdaExpressionFromArgument(1), methodCallExpression.Type, returnDefault: false));
					}
					break;
				case "SingleOrDefault":
					if (methodInfo == QueryableMethods.SingleOrDefaultWithoutPredicate)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.SingleOrDefault;
						return CheckTranslated(TranslateSingleOrDefault(shapedQueryExpression, null, methodCallExpression.Type, returnDefault: true));
					}
					if (methodInfo == QueryableMethods.SingleOrDefaultWithPredicate)
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.SingleOrDefault;
						return CheckTranslated(TranslateSingleOrDefault(shapedQueryExpression, GetLambdaExpressionFromArgument(1), methodCallExpression.Type, returnDefault: true));
					}
					break;
				case "Skip":
					if (methodInfo == QueryableMethods.Skip)
					{
						return CheckTranslated(TranslateSkip(shapedQueryExpression, methodCallExpression.Arguments[1]));
					}
					break;
				case "SkipWhile":
					if (methodInfo == QueryableMethods.SkipWhile)
					{
						return CheckTranslated(TranslateSkipWhile(shapedQueryExpression, GetLambdaExpressionFromArgument(1)));
					}
					break;
				case "Sum":
					if (QueryableMethods.IsSumWithoutSelector(method))
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateSum(shapedQueryExpression, null, methodCallExpression.Type));
					}
					if (QueryableMethods.IsSumWithSelector(method))
					{
						shapedQueryExpression.ResultCardinality = ResultCardinality.Single;
						return CheckTranslated(TranslateSum(shapedQueryExpression, GetLambdaExpressionFromArgument(1), methodCallExpression.Type));
					}
					break;
				case "Take":
					if (methodInfo == QueryableMethods.Take)
					{
						return CheckTranslated(TranslateTake(shapedQueryExpression, methodCallExpression.Arguments[1]));
					}
					break;
				case "TakeWhile":
					if (methodInfo == QueryableMethods.TakeWhile)
					{
						return CheckTranslated(TranslateTakeWhile(shapedQueryExpression, GetLambdaExpressionFromArgument(1)));
					}
					break;
				case "ThenBy":
					if (methodInfo == QueryableMethods.ThenBy)
					{
						return CheckTranslated(TranslateThenBy(shapedQueryExpression, GetLambdaExpressionFromArgument(1), ascending: true));
					}
					break;
				case "ThenByDescending":
					if (methodInfo == QueryableMethods.ThenByDescending)
					{
						return CheckTranslated(TranslateThenBy(shapedQueryExpression, GetLambdaExpressionFromArgument(1), ascending: false));
					}
					break;
				case "Union":
					if (methodInfo == QueryableMethods.Union && Visit(methodCallExpression.Arguments[1]) is ShapedQueryExpression source)
					{
						return CheckTranslated(TranslateUnion(shapedQueryExpression, source));
					}
					break;
				case "Where":
					if (methodInfo == QueryableMethods.Where)
					{
						return CheckTranslated(TranslateWhere(shapedQueryExpression, GetLambdaExpressionFromArgument(1)));
					}
					break;
				}
			}
		}
		if (!_subquery)
		{
			throw new NotImplementedException("Unhandled method: " + method.Name);
		}
		return null;
		ShapedQueryExpression CheckTranslated(ShapedQueryExpression translated)
		{
			if (translated == null)
			{
				throw new InvalidOperationException(CoreStrings.TranslationFailed(methodCallExpression.Print()));
			}
			return translated;
		}
		LambdaExpression GetLambdaExpressionFromArgument(int argumentIndex)
		{
			return methodCallExpression.Arguments[argumentIndex].UnwrapLambdaFromQuote();
		}
	}

	protected virtual Expression MarkShaperNullable(Expression shaperExpression)
	{
		return _entityShaperNullableMarkingExpressionVisitor.Visit(shaperExpression);
	}

	protected virtual ShapedQueryExpression TranslateResultSelectorForJoin(ShapedQueryExpression outer, LambdaExpression resultSelector, Expression innerShaper, Type transparentIdentifierType)
	{
		outer.ShaperExpression = CombineShapers(outer.QueryExpression, outer.ShaperExpression, innerShaper, transparentIdentifierType);
		ParameterExpression parameterExpression = Expression.Parameter(transparentIdentifierType);
		Expression expression = resultSelector.Parameters[0];
		Expression expression2 = AccessOuterTransparentField(transparentIdentifierType, parameterExpression);
		Expression expression3 = resultSelector.Parameters[1];
		Expression expression4 = AccessInnerTransparentField(transparentIdentifierType, parameterExpression);
		LambdaExpression selector = Expression.Lambda(new ReplacingExpressionVisitor(new Expression[2] { expression, expression3 }, new Expression[2] { expression2, expression4 }).Visit(resultSelector.Body), parameterExpression);
		return TranslateSelect(outer, selector);
	}

	private Expression CombineShapers(Expression queryExpression, Expression outerShaper, Expression innerShaper, Type transparentIdentifierType)
	{
		FieldInfo declaredField = transparentIdentifierType.GetTypeInfo().GetDeclaredField("Outer");
		FieldInfo declaredField2 = transparentIdentifierType.GetTypeInfo().GetDeclaredField("Inner");
		outerShaper = new MemberAccessShiftingExpressionVisitor(queryExpression, declaredField).Visit(outerShaper);
		innerShaper = new MemberAccessShiftingExpressionVisitor(queryExpression, declaredField2).Visit(innerShaper);
		return Expression.New(transparentIdentifierType.GetTypeInfo().DeclaredConstructors.Single(), new Expression[2] { outerShaper, innerShaper }, declaredField, declaredField2);
	}

	private static Expression AccessOuterTransparentField(Type transparentIdentifierType, Expression targetExpression)
	{
		FieldInfo declaredField = transparentIdentifierType.GetTypeInfo().GetDeclaredField("Outer");
		return Expression.Field(targetExpression, declaredField);
	}

	private static Expression AccessInnerTransparentField(Type transparentIdentifierType, Expression targetExpression)
	{
		FieldInfo declaredField = transparentIdentifierType.GetTypeInfo().GetDeclaredField("Inner");
		return Expression.Field(targetExpression, declaredField);
	}

	public virtual ShapedQueryExpression TranslateSubquery(Expression expression)
	{
		return (ShapedQueryExpression)CreateSubqueryVisitor().Visit(expression);
	}

	protected abstract QueryableMethodTranslatingExpressionVisitor CreateSubqueryVisitor();

	protected abstract ShapedQueryExpression CreateShapedQueryExpression(Type elementType);

	protected abstract ShapedQueryExpression TranslateAll(ShapedQueryExpression source, LambdaExpression predicate);

	protected abstract ShapedQueryExpression TranslateAny(ShapedQueryExpression source, LambdaExpression predicate);

	protected abstract ShapedQueryExpression TranslateAverage(ShapedQueryExpression source, LambdaExpression selector, Type resultType);

	protected abstract ShapedQueryExpression TranslateCast(ShapedQueryExpression source, Type resultType);

	protected abstract ShapedQueryExpression TranslateConcat(ShapedQueryExpression source1, ShapedQueryExpression source2);

	protected abstract ShapedQueryExpression TranslateContains(ShapedQueryExpression source, Expression item);

	protected abstract ShapedQueryExpression TranslateCount(ShapedQueryExpression source, LambdaExpression predicate);

	protected abstract ShapedQueryExpression TranslateDefaultIfEmpty(ShapedQueryExpression source, Expression defaultValue);

	protected abstract ShapedQueryExpression TranslateDistinct(ShapedQueryExpression source);

	protected abstract ShapedQueryExpression TranslateElementAtOrDefault(ShapedQueryExpression source, Expression index, bool returnDefault);

	protected abstract ShapedQueryExpression TranslateExcept(ShapedQueryExpression source1, ShapedQueryExpression source2);

	protected abstract ShapedQueryExpression TranslateFirstOrDefault(ShapedQueryExpression source, LambdaExpression predicate, Type returnType, bool returnDefault);

	protected abstract ShapedQueryExpression TranslateGroupBy(ShapedQueryExpression source, LambdaExpression keySelector, LambdaExpression elementSelector, LambdaExpression resultSelector);

	protected abstract ShapedQueryExpression TranslateGroupJoin(ShapedQueryExpression outer, ShapedQueryExpression inner, LambdaExpression outerKeySelector, LambdaExpression innerKeySelector, LambdaExpression resultSelector);

	protected abstract ShapedQueryExpression TranslateIntersect(ShapedQueryExpression source1, ShapedQueryExpression source2);

	protected abstract ShapedQueryExpression TranslateJoin(ShapedQueryExpression outer, ShapedQueryExpression inner, LambdaExpression outerKeySelector, LambdaExpression innerKeySelector, LambdaExpression resultSelector);

	protected abstract ShapedQueryExpression TranslateLeftJoin(ShapedQueryExpression outer, ShapedQueryExpression inner, LambdaExpression outerKeySelector, LambdaExpression innerKeySelector, LambdaExpression resultSelector);

	protected abstract ShapedQueryExpression TranslateLastOrDefault(ShapedQueryExpression source, LambdaExpression predicate, Type returnType, bool returnDefault);

	protected abstract ShapedQueryExpression TranslateLongCount(ShapedQueryExpression source, LambdaExpression predicate);

	protected abstract ShapedQueryExpression TranslateMax(ShapedQueryExpression source, LambdaExpression selector, Type resultType);

	protected abstract ShapedQueryExpression TranslateMin(ShapedQueryExpression source, LambdaExpression selector, Type resultType);

	protected abstract ShapedQueryExpression TranslateOfType(ShapedQueryExpression source, Type resultType);

	protected abstract ShapedQueryExpression TranslateOrderBy(ShapedQueryExpression source, LambdaExpression keySelector, bool ascending);

	protected abstract ShapedQueryExpression TranslateReverse(ShapedQueryExpression source);

	protected abstract ShapedQueryExpression TranslateSelect(ShapedQueryExpression source, LambdaExpression selector);

	protected abstract ShapedQueryExpression TranslateSelectMany(ShapedQueryExpression source, LambdaExpression collectionSelector, LambdaExpression resultSelector);

	protected abstract ShapedQueryExpression TranslateSelectMany(ShapedQueryExpression source, LambdaExpression selector);

	protected abstract ShapedQueryExpression TranslateSingleOrDefault(ShapedQueryExpression source, LambdaExpression predicate, Type returnType, bool returnDefault);

	protected abstract ShapedQueryExpression TranslateSkip(ShapedQueryExpression source, Expression count);

	protected abstract ShapedQueryExpression TranslateSkipWhile(ShapedQueryExpression source, LambdaExpression predicate);

	protected abstract ShapedQueryExpression TranslateSum(ShapedQueryExpression source, LambdaExpression selector, Type resultType);

	protected abstract ShapedQueryExpression TranslateTake(ShapedQueryExpression source, Expression count);

	protected abstract ShapedQueryExpression TranslateTakeWhile(ShapedQueryExpression source, LambdaExpression predicate);

	protected abstract ShapedQueryExpression TranslateThenBy(ShapedQueryExpression source, LambdaExpression keySelector, bool ascending);

	protected abstract ShapedQueryExpression TranslateUnion(ShapedQueryExpression source1, ShapedQueryExpression source2);

	protected abstract ShapedQueryExpression TranslateWhere(ShapedQueryExpression source, LambdaExpression predicate);
}
