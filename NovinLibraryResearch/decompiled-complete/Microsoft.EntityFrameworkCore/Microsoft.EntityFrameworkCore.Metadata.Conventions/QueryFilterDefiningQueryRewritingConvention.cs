using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class QueryFilterDefiningQueryRewritingConvention : IModelFinalizedConvention, IConvention
{
	protected class DbSetAccessRewritingExpressionVisitor : ExpressionVisitor
	{
		private readonly Type _contextType;

		public DbSetAccessRewritingExpressionVisitor(Type contextType)
		{
			_contextType = contextType;
		}

		protected override Expression VisitMember(MemberExpression memberExpression)
		{
			if (memberExpression.Expression != null && (memberExpression.Expression.Type.IsAssignableFrom(_contextType) || _contextType.IsAssignableFrom(memberExpression.Expression.Type)) && memberExpression.Type.IsGenericType && (memberExpression.Type.GetGenericTypeDefinition() == typeof(DbSet<>) || memberExpression.Type.GetGenericTypeDefinition() == typeof(DbQuery<>)))
			{
				return NullAsyncQueryProvider.Instance.CreateEntityQueryableExpression(memberExpression.Type.GetGenericArguments()[0]);
			}
			return base.VisitMember(memberExpression);
		}

		protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
		{
			if (methodCallExpression.Method.Name == "Set" && methodCallExpression.Object != null && typeof(DbContext).IsAssignableFrom(methodCallExpression.Object.Type) && methodCallExpression.Type.IsGenericType && methodCallExpression.Type.GetGenericTypeDefinition() == typeof(DbSet<>))
			{
				return NullAsyncQueryProvider.Instance.CreateEntityQueryableExpression(methodCallExpression.Type.GetGenericArguments()[0]);
			}
			return base.VisitMethodCall(methodCallExpression);
		}
	}

	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	protected virtual DbSetAccessRewritingExpressionVisitor DbSetAccessRewriter
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public QueryFilterDefiningQueryRewritingConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
		DbSetAccessRewriter = new DbSetAccessRewritingExpressionVisitor(dependencies.ContextType);
	}

	public virtual void ProcessModelFinalized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		foreach (IConventionEntityType entityType in modelBuilder.Metadata.GetEntityTypes())
		{
			LambdaExpression queryFilter = entityType.GetQueryFilter();
			if (queryFilter != null)
			{
				entityType.SetQueryFilter((LambdaExpression)DbSetAccessRewriter.Visit(queryFilter));
			}
			LambdaExpression definingQuery = entityType.GetDefiningQuery();
			if (definingQuery != null)
			{
				entityType.SetDefiningQuery((LambdaExpression)DbSetAccessRewriter.Visit(definingQuery));
			}
		}
	}
}
