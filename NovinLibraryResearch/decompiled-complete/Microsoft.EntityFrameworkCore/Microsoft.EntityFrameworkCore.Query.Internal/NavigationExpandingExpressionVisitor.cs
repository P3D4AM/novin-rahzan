using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class NavigationExpandingExpressionVisitor : ExpressionVisitor
{
	private class Parameters : IParameterValues
	{
		private readonly IDictionary<string, object> _parameterValues = new Dictionary<string, object>();

		public IReadOnlyDictionary<string, object> ParameterValues => (IReadOnlyDictionary<string, object>)_parameterValues;

		public virtual void AddParameter(string name, object value)
		{
			_parameterValues.Add(name, value);
		}
	}

	protected class EntityReference : Expression, IPrintableExpression
	{
		public virtual IEntityType EntityType { get; }

		public virtual IDictionary<INavigation, Expression> NavigationMap { get; } = new Dictionary<INavigation, Expression>();


		public virtual bool IsOptional { get; private set; }

		public virtual IncludeTreeNode IncludePaths { get; private set; }

		public virtual IncludeTreeNode LastIncludeTreeNode { get; private set; }

		public override ExpressionType NodeType => ExpressionType.Extension;

		public override Type Type => EntityType.ClrType;

		public EntityReference(IEntityType entityType)
		{
			EntityType = entityType;
			IncludePaths = new IncludeTreeNode(entityType, this);
		}

		protected override Expression VisitChildren(ExpressionVisitor visitor)
		{
			return this;
		}

		public virtual void SetIncludePaths(IncludeTreeNode includePaths)
		{
			IncludePaths = includePaths;
			includePaths.SetEntityReference(this);
		}

		public virtual EntityReference Clone()
		{
			EntityReference entityReference = new EntityReference(EntityType)
			{
				IsOptional = IsOptional
			};
			entityReference.IncludePaths = IncludePaths.Clone(entityReference);
			return entityReference;
		}

		public virtual void SetLastInclude(IncludeTreeNode lastIncludeTree)
		{
			LastIncludeTreeNode = lastIncludeTree;
		}

		public virtual void MarkAsOptional()
		{
			IsOptional = true;
		}

		public virtual void Print(ExpressionPrinter expressionPrinter)
		{
			expressionPrinter.Append("EntityReference: " + EntityType.DisplayName());
			if (IsOptional)
			{
				expressionPrinter.Append("[Optional]");
			}
			if (IncludePaths.Count > 0)
			{
				expressionPrinter.Append(" | IncludePaths: " + string.Join(" ", IncludePaths.Select((KeyValuePair<INavigation, IncludeTreeNode> ip) => (ip.Value.Count() <= 0) ? ip.Key.Name : (ip.Key.Name + "->..."))));
			}
		}
	}

	protected class IncludeTreeNode : Dictionary<INavigation, IncludeTreeNode>
	{
		private EntityReference _entityReference;

		public virtual IEntityType EntityType { get; private set; }

		public IncludeTreeNode(IEntityType entityType, EntityReference entityReference)
		{
			EntityType = entityType;
			_entityReference = entityReference;
		}

		public virtual IncludeTreeNode AddNavigation(INavigation navigation)
		{
			if (TryGetValue(navigation, out var value))
			{
				return value;
			}
			if (_entityReference != null && _entityReference.NavigationMap.TryGetValue(navigation, out var value2))
			{
				EntityReference entityReference;
				if (!(value2 is NavigationTreeExpression navigationTreeExpression))
				{
					if (!(value2 is OwnedNavigationReference ownedNavigationReference))
					{
						throw new InvalidOperationException("Invalid expression type stored in NavigationMap.");
					}
					entityReference = ownedNavigationReference.EntityReference;
				}
				else
				{
					entityReference = (EntityReference)navigationTreeExpression.Value;
				}
				EntityReference entityReference2 = entityReference;
				base[navigation] = entityReference2.IncludePaths;
			}
			else
			{
				base[navigation] = new IncludeTreeNode(navigation.GetTargetType(), null);
			}
			return base[navigation];
		}

		public virtual void SetEntityReference(EntityReference entityReference)
		{
			_entityReference = entityReference;
			EntityType = entityReference.EntityType;
		}

		public virtual IncludeTreeNode Clone(EntityReference entityReference)
		{
			IncludeTreeNode includeTreeNode = new IncludeTreeNode(EntityType, entityReference);
			using Enumerator enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<INavigation, IncludeTreeNode> current = enumerator.Current;
				includeTreeNode[current.Key] = current.Value.Clone(current.Value._entityReference);
			}
			return includeTreeNode;
		}

		public override bool Equals(object obj)
		{
			if (obj != null)
			{
				if (this != obj)
				{
					if (obj is IncludeTreeNode includeTreeNode)
					{
						return Equals(includeTreeNode);
					}
					return false;
				}
				return true;
			}
			return false;
		}

		private bool Equals(IncludeTreeNode includeTreeNode)
		{
			if (base.Count != includeTreeNode.Count)
			{
				return false;
			}
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<INavigation, IncludeTreeNode> current = enumerator.Current;
					if (!includeTreeNode.TryGetValue(current.Key, out var value) || !current.Value.Equals(value))
					{
						return false;
					}
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(base.GetHashCode(), EntityType);
		}
	}

	protected class NavigationExpansionExpression : Expression, IPrintableExpression
	{
		private readonly List<(MethodInfo OrderingMethod, Expression KeySelector)> _pendingOrderings = new List<(MethodInfo, Expression)>();

		private readonly string _parameterName;

		private NavigationTreeNode _currentTree;

		public virtual Expression Source { get; private set; }

		public virtual ParameterExpression CurrentParameter => CurrentTree.CurrentParameter;

		public virtual NavigationTreeNode CurrentTree
		{
			get
			{
				return _currentTree;
			}
			private set
			{
				_currentTree = value;
				_currentTree.SetParameter(_parameterName);
			}
		}

		public virtual Expression PendingSelector { get; private set; }

		public virtual MethodInfo CardinalityReducingGenericMethodInfo { get; private set; }

		public virtual Type SourceElementType => CurrentParameter.Type;

		public virtual IReadOnlyList<(MethodInfo OrderingMethod, Expression KeySelector)> PendingOrderings => _pendingOrderings;

		public override ExpressionType NodeType => ExpressionType.Extension;

		public override Type Type
		{
			get
			{
				if (!(CardinalityReducingGenericMethodInfo == null))
				{
					return PendingSelector.Type;
				}
				return typeof(IQueryable<>).MakeGenericType(PendingSelector.Type);
			}
		}

		public NavigationExpansionExpression(Expression source, NavigationTreeNode currentTree, Expression pendingSelector, string parameterName)
		{
			Source = source;
			_parameterName = parameterName;
			CurrentTree = currentTree;
			PendingSelector = pendingSelector;
		}

		public virtual void UpdateSource(Expression source)
		{
			Source = source;
		}

		public virtual void UpdateCurrentTree(NavigationTreeNode currentTree)
		{
			CurrentTree = currentTree;
		}

		public virtual void ApplySelector(Expression selector)
		{
			PendingSelector = selector;
		}

		public virtual void AddPendingOrdering(MethodInfo orderingMethod, Expression keySelector)
		{
			_pendingOrderings.Clear();
			_pendingOrderings.Add((orderingMethod, keySelector));
		}

		public virtual void AppendPendingOrdering(MethodInfo orderingMethod, Expression keySelector)
		{
			_pendingOrderings.Add((orderingMethod, keySelector));
		}

		public virtual void ClearPendingOrderings()
		{
			_pendingOrderings.Clear();
		}

		public virtual void ConvertToSingleResult(MethodInfo genericMethod)
		{
			CardinalityReducingGenericMethodInfo = genericMethod;
		}

		protected override Expression VisitChildren(ExpressionVisitor visitor)
		{
			return this;
		}

		public virtual void Print(ExpressionPrinter expressionPrinter)
		{
			expressionPrinter.AppendLine("NavigationExpansionExpression");
			using (expressionPrinter.Indent())
			{
				expressionPrinter.Append("Source: ");
				expressionPrinter.Visit(Source);
				expressionPrinter.AppendLine();
				expressionPrinter.Append("PendingSelector: ");
				expressionPrinter.Visit(Expression.Lambda(PendingSelector, CurrentParameter));
				expressionPrinter.AppendLine();
				if (CardinalityReducingGenericMethodInfo != null)
				{
					expressionPrinter.AppendLine("CardinalityReducingMethod: " + CardinalityReducingGenericMethodInfo.Name);
				}
			}
		}
	}

	protected class NavigationTreeExpression : NavigationTreeNode, IPrintableExpression
	{
		public virtual Expression Value { get; private set; }

		public override Type Type => Value.Type;

		public NavigationTreeExpression(Expression value)
			: base(null, null)
		{
			Value = value;
		}

		protected override Expression VisitChildren(ExpressionVisitor visitor)
		{
			Value = visitor.Visit(Value);
			return this;
		}

		public virtual void Print(ExpressionPrinter expressionPrinter)
		{
			expressionPrinter.AppendLine("NavigationTreeExpression");
			using (expressionPrinter.Indent())
			{
				expressionPrinter.Append("Value: ");
				expressionPrinter.Visit(Value);
				expressionPrinter.AppendLine();
				expressionPrinter.Append("Expression: ");
				expressionPrinter.Visit(GetExpression());
			}
		}
	}

	protected class NavigationTreeNode : Expression
	{
		private NavigationTreeNode _parent;

		public virtual NavigationTreeNode Parent
		{
			get
			{
				return _parent;
			}
			private set
			{
				_parent = value;
				CurrentParameter = null;
			}
		}

		public virtual NavigationTreeNode Left { get; }

		public virtual NavigationTreeNode Right { get; }

		public virtual ParameterExpression CurrentParameter { get; private set; }

		public override ExpressionType NodeType => ExpressionType.Extension;

		public override Type Type => TransparentIdentifierFactory.Create(Left.Type, Right.Type);

		public NavigationTreeNode(NavigationTreeNode left, NavigationTreeNode right)
		{
			Left = left;
			Right = right;
			if (left != null)
			{
				Left.Parent = this;
				Right.Parent = this;
			}
		}

		protected override Expression VisitChildren(ExpressionVisitor visitor)
		{
			throw new InvalidOperationException(CoreStrings.QueryFailed(this.Print(), GetType().Name));
		}

		public virtual void SetParameter(string parameterName)
		{
			CurrentParameter = Expression.Parameter(Type, parameterName);
		}

		public virtual Expression GetExpression()
		{
			if (Parent == null)
			{
				return CurrentParameter;
			}
			Expression expression = Parent.GetExpression();
			if (Parent.Left != this)
			{
				return Expression.MakeMemberAccess(expression, expression.Type.GetTypeInfo().GetMember("Inner")[0]);
			}
			return Expression.MakeMemberAccess(expression, expression.Type.GetTypeInfo().GetMember("Outer")[0]);
		}
	}

	protected class OwnedNavigationReference : Expression
	{
		public virtual Expression Parent { get; private set; }

		public virtual INavigation Navigation { get; }

		public virtual EntityReference EntityReference { get; }

		public override Type Type => Navigation.ClrType;

		public override ExpressionType NodeType => ExpressionType.Extension;

		public OwnedNavigationReference(Expression parent, INavigation navigation, EntityReference entityReference)
		{
			Parent = parent;
			Navigation = navigation;
			EntityReference = entityReference;
		}

		protected override Expression VisitChildren(ExpressionVisitor visitor)
		{
			Parent = visitor.Visit(Parent);
			return this;
		}
	}

	private class ExpandingExpressionVisitor : ExpressionVisitor
	{
		private readonly NavigationExpandingExpressionVisitor _navigationExpandingExpressionVisitor;

		private readonly NavigationExpansionExpression _source;

		public ExpandingExpressionVisitor(NavigationExpandingExpressionVisitor navigationExpandingExpressionVisitor, NavigationExpansionExpression source)
		{
			_navigationExpandingExpressionVisitor = navigationExpandingExpressionVisitor;
			_source = source;
		}

		public Expression Expand(Expression expression, bool applyIncludes = false)
		{
			expression = Visit(expression);
			if (applyIncludes)
			{
				expression = new IncludeExpandingExpressionVisitor(_navigationExpandingExpressionVisitor, _source).Visit(expression);
			}
			return expression;
		}

		protected override Expression VisitExtension(Expression expression)
		{
			if (expression is NavigationExpansionExpression || expression is NavigationTreeExpression)
			{
				return expression;
			}
			return base.VisitExtension(expression);
		}

		protected override Expression VisitMember(MemberExpression memberExpression)
		{
			Expression expression = Visit(memberExpression.Expression);
			return TryExpandNavigation(expression, MemberIdentity.Create(memberExpression.Member)) ?? memberExpression.Update(expression);
		}

		protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
		{
			if (methodCallExpression.TryGetEFPropertyArguments(out var entityExpression, out var propertyName))
			{
				entityExpression = Visit(entityExpression);
				return TryExpandNavigation(entityExpression, MemberIdentity.Create(propertyName)) ?? methodCallExpression.Update(null, new Expression[2]
				{
					entityExpression,
					methodCallExpression.Arguments[1]
				});
			}
			return base.VisitMethodCall(methodCallExpression);
		}

		protected EntityReference UnwrapEntityReference(Expression expression)
		{
			if (!(expression is EntityReference result))
			{
				if (!(expression is NavigationTreeExpression navigationTreeExpression))
				{
					if (!(expression is NavigationExpansionExpression navigationExpansionExpression))
					{
						if (expression is OwnedNavigationReference ownedNavigationReference)
						{
							return ownedNavigationReference.EntityReference;
						}
					}
					else if (navigationExpansionExpression.CardinalityReducingGenericMethodInfo != null)
					{
						return UnwrapEntityReference(navigationExpansionExpression.PendingSelector);
					}
					return null;
				}
				return UnwrapEntityReference(navigationTreeExpression.Value);
			}
			return result;
		}

		private Expression TryExpandNavigation(Expression root, MemberIdentity memberIdentity)
		{
			Type convertedType;
			Expression expression = root.UnwrapTypeConversion(out convertedType);
			EntityReference entityReference = UnwrapEntityReference(expression);
			if (entityReference != null)
			{
				IEntityType entityType = entityReference.EntityType;
				if (convertedType != null)
				{
					entityType = entityType.GetTypesInHierarchy().FirstOrDefault((IEntityType et) => et.ClrType == convertedType);
					if (entityType == null)
					{
						return null;
					}
				}
				INavigation navigation = ((memberIdentity.MemberInfo != null) ? entityType.FindNavigation(memberIdentity.MemberInfo) : entityType.FindNavigation(memberIdentity.Name));
				if (navigation != null)
				{
					return ExpandNavigation(root, entityReference, navigation, convertedType != null);
				}
			}
			return null;
		}

		protected Expression ExpandNavigation(Expression root, EntityReference entityReference, INavigation navigation, bool derivedTypeConversion)
		{
			if (entityReference.NavigationMap.TryGetValue(navigation, out var value))
			{
				return value;
			}
			IEntityType targetType = navigation.GetTargetType();
			if (targetType.HasDefiningNavigation() || targetType.IsOwned())
			{
				EntityReference entityReference2 = new EntityReference(targetType);
				entityReference2.MarkAsOptional();
				if (entityReference.IncludePaths.ContainsKey(navigation))
				{
					entityReference2.SetIncludePaths(entityReference.IncludePaths[navigation]);
				}
				OwnedNavigationReference ownedNavigationReference = new OwnedNavigationReference(root, navigation, entityReference2);
				if (navigation.IsCollection())
				{
					Type type = ownedNavigationReference.Type.TryGetSequenceType();
					return new MaterializeCollectionNavigationExpression(Expression.Call(QueryableMethods.AsQueryable.MakeGenericMethod(type), ownedNavigationReference), navigation);
				}
				entityReference.NavigationMap[navigation] = ownedNavigationReference;
				return ownedNavigationReference;
			}
			Type clrType = targetType.ClrType;
			ConstantExpression node = NullAsyncQueryProvider.Instance.CreateEntityQueryableExpression(clrType);
			NavigationExpansionExpression navigationExpansionExpression = (NavigationExpansionExpression)_navigationExpandingExpressionVisitor.Visit(node);
			if (entityReference.IncludePaths.ContainsKey(navigation))
			{
				IncludeTreeNode includePaths = entityReference.IncludePaths[navigation];
				((EntityReference)((NavigationTreeExpression)navigationExpansionExpression.PendingSelector).Value).SetIncludePaths(includePaths);
			}
			Type sequenceType = navigationExpansionExpression.Type.GetSequenceType();
			ParameterExpression parameterExpression = Expression.Parameter(sequenceType, "i");
			Expression expression;
			if (root is NavigationExpansionExpression navigationExpansionExpression2 && navigationExpansionExpression2.CardinalityReducingGenericMethodInfo != null)
			{
				ParameterExpression parameterExpression2 = Expression.Parameter(root.Type);
				Expression selector = ReplacingExpressionVisitor.Replace(tree: parameterExpression2.CreateKeyAccessExpression(navigation.IsDependentToPrincipal() ? navigation.ForeignKey.Properties : navigation.ForeignKey.PrincipalKey.Properties, makeNullable: true), original: parameterExpression2, replacement: navigationExpansionExpression2.PendingSelector);
				navigationExpansionExpression2.ApplySelector(selector);
				expression = navigationExpansionExpression2;
			}
			else
			{
				expression = root.CreateKeyAccessExpression(navigation.IsDependentToPrincipal() ? navigation.ForeignKey.Properties : navigation.ForeignKey.PrincipalKey.Properties, makeNullable: true);
			}
			Expression expression2 = parameterExpression.CreateKeyAccessExpression(navigation.IsDependentToPrincipal() ? navigation.ForeignKey.PrincipalKey.Properties : navigation.ForeignKey.Properties, makeNullable: true);
			if (expression.Type != expression2.Type)
			{
				if (!expression.Type.IsNullableType())
				{
					expression = Expression.Convert(expression, expression.Type.MakeNullable());
				}
				if (!expression2.Type.IsNullableType())
				{
					expression2 = Expression.Convert(expression2, expression2.Type.MakeNullable());
				}
			}
			if (navigation.IsCollection())
			{
				Expression expression3 = ((expression is NewExpression newExpression) ? ((UnaryExpression)((NewArrayExpression)newExpression.Arguments[0]).Expressions[0]).Operand : expression);
				BinaryExpression body = (expression3.Type.IsNullableType() ? Expression.AndAlso(Expression.NotEqual(expression3, Expression.Constant(null, expression3.Type)), Expression.Equal(expression, expression2)) : Expression.Equal(expression, expression2));
				return new MaterializeCollectionNavigationExpression(Expression.Call(QueryableMethods.Where.MakeGenericMethod(sequenceType), navigationExpansionExpression, Expression.Quote(Expression.Lambda(body, parameterExpression))), navigation);
			}
			LambdaExpression lambdaExpression = _navigationExpandingExpressionVisitor.GenerateLambda(expression, _source.CurrentParameter);
			LambdaExpression arg = _navigationExpandingExpressionVisitor.GenerateLambda(_navigationExpandingExpressionVisitor.ExpandNavigationsInLambdaExpression(navigationExpansionExpression, Expression.Lambda(expression2, parameterExpression)), navigationExpansionExpression.CurrentParameter);
			ParameterExpression parameterExpression3 = Expression.Parameter(_source.SourceElementType, "o");
			ParameterExpression parameterExpression4 = Expression.Parameter(navigationExpansionExpression.SourceElementType, "i");
			Type type2 = TransparentIdentifierFactory.Create(_source.SourceElementType, navigationExpansionExpression.SourceElementType);
			FieldInfo declaredField = type2.GetTypeInfo().GetDeclaredField("Outer");
			FieldInfo declaredField2 = type2.GetTypeInfo().GetDeclaredField("Inner");
			LambdaExpression lambdaExpression2 = Expression.Lambda(Expression.New(type2.GetTypeInfo().GetConstructors().Single(), new ParameterExpression[2] { parameterExpression3, parameterExpression4 }, declaredField, declaredField2), parameterExpression3, parameterExpression4);
			bool flag = !entityReference.IsOptional && !derivedTypeConversion && navigation.IsDependentToPrincipal() && navigation.ForeignKey.IsRequired;
			if (!flag)
			{
				((EntityReference)((NavigationTreeExpression)navigationExpansionExpression.PendingSelector).Value).MarkAsOptional();
			}
			_source.UpdateSource(Expression.Call((flag ? QueryableMethods.Join : QueryableExtensions.LeftJoinMethodInfo).MakeGenericMethod(_source.SourceElementType, navigationExpansionExpression.SourceElementType, lambdaExpression.ReturnType, lambdaExpression2.ReturnType), _source.Source, navigationExpansionExpression.Source, lambdaExpression, arg, lambdaExpression2));
			entityReference.NavigationMap[navigation] = navigationExpansionExpression.PendingSelector;
			_source.UpdateCurrentTree(new NavigationTreeNode(_source.CurrentTree, navigationExpansionExpression.CurrentTree));
			return navigationExpansionExpression.PendingSelector;
		}
	}

	private class IncludeExpandingExpressionVisitor : ExpandingExpressionVisitor
	{
		private readonly bool _isTracking;

		public IncludeExpandingExpressionVisitor(NavigationExpandingExpressionVisitor navigationExpandingExpressionVisitor, NavigationExpansionExpression source)
			: base(navigationExpandingExpressionVisitor, source)
		{
			_isTracking = navigationExpandingExpressionVisitor._queryCompilationContext.IsTracking;
		}

		protected override Expression VisitExtension(Expression extensionExpression)
		{
			if (!(extensionExpression is NavigationTreeExpression navigationTreeExpression))
			{
				if (extensionExpression is OwnedNavigationReference ownedNavigationReference)
				{
					return ExpandInclude(ownedNavigationReference, ownedNavigationReference.EntityReference);
				}
				if (extensionExpression is MaterializeCollectionNavigationExpression)
				{
					return extensionExpression;
				}
			}
			else
			{
				if (navigationTreeExpression.Value is EntityReference entityReference)
				{
					return ExpandInclude(navigationTreeExpression, entityReference);
				}
				if (navigationTreeExpression.Value is NewExpression newExpression && ReconstructAnonymousType(navigationTreeExpression, newExpression, out var replacement))
				{
					return replacement;
				}
			}
			return base.VisitExtension(extensionExpression);
		}

		protected override Expression VisitMember(MemberExpression memberExpression)
		{
			EntityReference entityReference = UnwrapEntityReference(memberExpression.Expression);
			if (entityReference != null && entityReference.EntityType.FindProperty(memberExpression.Member) != null)
			{
				return memberExpression;
			}
			return base.VisitMember(memberExpression);
		}

		protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
		{
			if (methodCallExpression.TryGetEFPropertyArguments(out var _, out var _))
			{
				return methodCallExpression;
			}
			return base.VisitMethodCall(methodCallExpression);
		}

		protected override Expression VisitNew(NewExpression newExpression)
		{
			if (newExpression.Constructor == null)
			{
				return newExpression;
			}
			Expression[] array = new Expression[newExpression.Arguments.Count];
			for (int i = 0; i < newExpression.Arguments.Count; i++)
			{
				Expression expression = newExpression.Arguments[i];
				array[i] = ((expression is EntityReference entityReference) ? ExpandInclude(expression, entityReference) : Visit(expression));
			}
			return newExpression.Update(array);
		}

		private bool ReconstructAnonymousType(Expression currentRoot, NewExpression newExpression, out Expression replacement)
		{
			replacement = null;
			bool flag = false;
			if (newExpression.Constructor == null)
			{
				return false;
			}
			if (newExpression.Arguments.Count > 0 && newExpression.Members == null)
			{
				return flag;
			}
			Expression[] array = new Expression[newExpression.Arguments.Count];
			for (int i = 0; i < newExpression.Arguments.Count; i++)
			{
				Expression expression = newExpression.Arguments[i];
				MemberExpression memberExpression = Expression.MakeMemberAccess(currentRoot, newExpression.Members[i]);
				if (expression is EntityReference entityReference)
				{
					flag = true;
					array[i] = ExpandInclude(memberExpression, entityReference);
				}
				else if (expression is NewExpression newExpression2)
				{
					if (ReconstructAnonymousType(memberExpression, newExpression2, out var replacement2))
					{
						flag = true;
						array[i] = replacement2;
					}
					else
					{
						array[i] = memberExpression;
					}
				}
				else
				{
					array[i] = memberExpression;
				}
			}
			if (flag)
			{
				replacement = newExpression.Update(array);
			}
			return flag;
		}

		private Expression ExpandInclude(Expression root, EntityReference entityReference)
		{
			if (!_isTracking)
			{
				VerifyNoCycles(entityReference.IncludePaths);
			}
			return ExpandIncludesHelper(root, entityReference);
		}

		private void VerifyNoCycles(IncludeTreeNode includeTreeNode)
		{
			foreach (KeyValuePair<INavigation, IncludeTreeNode> item in includeTreeNode)
			{
				INavigation key = item.Key;
				IncludeTreeNode value = item.Value;
				INavigation navigation = key.FindInverse();
				if (navigation != null && value.ContainsKey(navigation))
				{
					throw new InvalidOperationException("The Include path '" + key.Name + "->" + navigation.Name + "' results in a cycle. Cycles are not allowed in no-tracking queries. Either use a tracking query or remove the cycle.");
				}
				VerifyNoCycles(value);
			}
		}

		private Expression ExpandIncludesHelper(Expression root, EntityReference entityReference)
		{
			Expression expression = root;
			Expression root2 = root;
			foreach (KeyValuePair<INavigation, IncludeTreeNode> includePath in entityReference.IncludePaths)
			{
				INavigation key = includePath.Key;
				bool derivedTypeConversion = false;
				if (entityReference.EntityType != key.DeclaringEntityType && entityReference.EntityType.IsAssignableFrom(key.DeclaringEntityType))
				{
					derivedTypeConversion = true;
					root2 = Expression.Convert(root, key.DeclaringEntityType.ClrType);
				}
				Expression expression2 = ExpandNavigation(root2, entityReference, key, derivedTypeConversion);
				if (!key.IsCollection())
				{
					EntityReference entityReference2 = ((key.GetTargetType().HasDefiningNavigation() || key.GetTargetType().IsOwned()) ? ((OwnedNavigationReference)expression2).EntityReference : ((EntityReference)((NavigationTreeExpression)expression2).Value));
					expression2 = ExpandIncludesHelper(expression2, entityReference2);
				}
				expression = new IncludeExpression(expression, expression2, key);
			}
			return expression;
		}
	}

	private class PendingSelectorExpandingExpressionVisitor : ExpressionVisitor
	{
		private readonly NavigationExpandingExpressionVisitor _visitor;

		private readonly bool _applyIncludes;

		public PendingSelectorExpandingExpressionVisitor(NavigationExpandingExpressionVisitor visitor, bool applyIncludes = false)
		{
			_visitor = visitor;
			_applyIncludes = applyIncludes;
		}

		public override Expression Visit(Expression expression)
		{
			if (expression is NavigationExpansionExpression navigationExpansionExpression)
			{
				_visitor.ApplyPendingOrderings(navigationExpansionExpression);
				Expression node = new ExpandingExpressionVisitor(_visitor, navigationExpansionExpression).Expand(navigationExpansionExpression.PendingSelector, _applyIncludes);
				node = _visitor._subqueryMemberPushdownExpressionVisitor.Visit(node);
				node = _visitor.Visit(node);
				node = Visit(node);
				navigationExpansionExpression.ApplySelector(node);
				return navigationExpansionExpression;
			}
			return base.Visit(expression);
		}
	}

	private class ReducingExpressionVisitor : ExpressionVisitor
	{
		public override Expression Visit(Expression expression)
		{
			if (!(expression is NavigationTreeExpression navigationTreeExpression))
			{
				if (!(expression is NavigationExpansionExpression navigationExpansionExpression))
				{
					if (!(expression is OwnedNavigationReference ownedNavigationReference))
					{
						if (expression is IncludeExpression includeExpression)
						{
							Expression expression2 = Visit(includeExpression.EntityExpression);
							Expression node = ReplacingExpressionVisitor.Replace(includeExpression.EntityExpression, expression2, includeExpression.NavigationExpression);
							node = Visit(node);
							return includeExpression.Update(expression2, node);
						}
						return base.Visit(expression);
					}
					return Visit(ownedNavigationReference.Parent).CreateEFPropertyExpression(ownedNavigationReference.Navigation);
				}
				Expression expression3 = Visit(navigationExpansionExpression.PendingSelector);
				Expression expression4 = Visit(navigationExpansionExpression.Source);
				Expression expression5;
				if (expression3 == navigationExpansionExpression.CurrentParameter)
				{
					expression5 = expression4;
				}
				else
				{
					LambdaExpression lambdaExpression = Expression.Lambda(expression3, navigationExpansionExpression.CurrentParameter);
					expression5 = Expression.Call(QueryableMethods.Select.MakeGenericMethod(navigationExpansionExpression.SourceElementType, lambdaExpression.ReturnType), expression4, Expression.Quote(lambdaExpression));
				}
				if (navigationExpansionExpression.CardinalityReducingGenericMethodInfo != null)
				{
					expression5 = Expression.Call(navigationExpansionExpression.CardinalityReducingGenericMethodInfo.MakeGenericMethod(expression5.Type.TryGetSequenceType()), expression5);
				}
				return expression5;
			}
			return navigationTreeExpression.GetExpression();
		}
	}

	private class EntityReferenceOptionalMarkingExpressionVisitor : ExpressionVisitor
	{
		public override Expression Visit(Expression expression)
		{
			if (expression is EntityReference entityReference)
			{
				entityReference.MarkAsOptional();
				return entityReference;
			}
			return base.Visit(expression);
		}
	}

	private class SelfReferenceEntityQueryableRewritingExpressionVisitor : ExpressionVisitor
	{
		private readonly NavigationExpandingExpressionVisitor _navigationExpandingExpressionVisitor;

		private readonly IEntityType _entityType;

		public SelfReferenceEntityQueryableRewritingExpressionVisitor(NavigationExpandingExpressionVisitor navigationExpandingExpressionVisitor, IEntityType entityType)
		{
			_navigationExpandingExpressionVisitor = navigationExpandingExpressionVisitor;
			_entityType = entityType;
		}

		protected override Expression VisitConstant(ConstantExpression constantExpression)
		{
			if (constantExpression.IsEntityQueryable())
			{
				IEntityType entityType = _navigationExpandingExpressionVisitor._queryCompilationContext.Model.FindEntityType(((IQueryable)constantExpression.Value).ElementType);
				if (entityType == _entityType)
				{
					return _navigationExpandingExpressionVisitor.CreateNavigationExpansionExpression(constantExpression, entityType);
				}
			}
			return base.VisitConstant(constantExpression);
		}
	}

	private static readonly PropertyInfo _queryContextContextPropertyInfo = typeof(QueryContext).GetTypeInfo().GetDeclaredProperty("Context");

	private static readonly IDictionary<MethodInfo, MethodInfo> _predicateLessMethodInfo = new Dictionary<MethodInfo, MethodInfo>
	{
		{
			QueryableMethods.FirstWithPredicate,
			QueryableMethods.FirstWithoutPredicate
		},
		{
			QueryableMethods.FirstOrDefaultWithPredicate,
			QueryableMethods.FirstOrDefaultWithoutPredicate
		},
		{
			QueryableMethods.SingleWithPredicate,
			QueryableMethods.SingleWithoutPredicate
		},
		{
			QueryableMethods.SingleOrDefaultWithPredicate,
			QueryableMethods.SingleOrDefaultWithoutPredicate
		},
		{
			QueryableMethods.LastWithPredicate,
			QueryableMethods.LastWithoutPredicate
		},
		{
			QueryableMethods.LastOrDefaultWithPredicate,
			QueryableMethods.LastOrDefaultWithoutPredicate
		}
	};

	private readonly QueryCompilationContext _queryCompilationContext;

	private readonly PendingSelectorExpandingExpressionVisitor _pendingSelectorExpandingExpressionVisitor;

	private readonly SubqueryMemberPushdownExpressionVisitor _subqueryMemberPushdownExpressionVisitor;

	private readonly ReducingExpressionVisitor _reducingExpressionVisitor;

	private readonly EntityReferenceOptionalMarkingExpressionVisitor _entityReferenceOptionalMarkingExpressionVisitor;

	private readonly ISet<string> _parameterNames = new HashSet<string>();

	private readonly EnumerableToQueryableMethodConvertingExpressionVisitor _enumerableToQueryableMethodConvertingExpressionVisitor;

	private readonly EntityEqualityRewritingExpressionVisitor _entityEqualityRewritingExpressionVisitor;

	private readonly ParameterExtractingExpressionVisitor _parameterExtractingExpressionVisitor;

	private readonly Dictionary<IEntityType, LambdaExpression> _parameterizedQueryFilterPredicateCache = new Dictionary<IEntityType, LambdaExpression>();

	private readonly Parameters _parameters = new Parameters();

	public NavigationExpandingExpressionVisitor(QueryCompilationContext queryCompilationContext, IEvaluatableExpressionFilter evaluatableExpressionFilter)
	{
		_queryCompilationContext = queryCompilationContext;
		_pendingSelectorExpandingExpressionVisitor = new PendingSelectorExpandingExpressionVisitor(this);
		_subqueryMemberPushdownExpressionVisitor = new SubqueryMemberPushdownExpressionVisitor();
		_reducingExpressionVisitor = new ReducingExpressionVisitor();
		_entityReferenceOptionalMarkingExpressionVisitor = new EntityReferenceOptionalMarkingExpressionVisitor();
		_enumerableToQueryableMethodConvertingExpressionVisitor = new EnumerableToQueryableMethodConvertingExpressionVisitor();
		_entityEqualityRewritingExpressionVisitor = new EntityEqualityRewritingExpressionVisitor(_queryCompilationContext);
		_parameterExtractingExpressionVisitor = new ParameterExtractingExpressionVisitor(evaluatableExpressionFilter, _parameters, _queryCompilationContext.ContextType, _queryCompilationContext.Model, _queryCompilationContext.Logger, parameterize: false, generateContextAccessors: true);
	}

	public virtual Expression Expand(Expression query)
	{
		Expression node = Visit(query);
		node = new PendingSelectorExpandingExpressionVisitor(this, applyIncludes: true).Visit(node);
		node = Reduce(node);
		UnaryExpression replacement = Expression.Convert(Expression.Property(QueryCompilationContext.QueryContextParameter, _queryContextContextPropertyInfo), _queryCompilationContext.ContextType);
		foreach (KeyValuePair<string, object> parameterValue in _parameters.ParameterValues)
		{
			LambdaExpression lambdaExpression = (LambdaExpression)parameterValue.Value;
			Expression expression = ReplacingExpressionVisitor.Replace(lambdaExpression.Parameters[0], replacement, lambdaExpression.Body);
			_queryCompilationContext.RegisterRuntimeParameter(parameterValue.Key, Expression.Lambda(expression.Type.IsValueType ? Expression.Convert(expression, typeof(object)) : expression, QueryCompilationContext.QueryContextParameter));
		}
		return node;
	}

	protected override Expression VisitConstant(ConstantExpression constantExpression)
	{
		if (constantExpression.IsEntityQueryable())
		{
			IEntityType entityType = _queryCompilationContext.Model.FindEntityType(((IQueryable)constantExpression.Value).ElementType);
			LambdaExpression definingQuery = entityType.GetDefiningQuery();
			NavigationExpansionExpression navigationExpansionExpression;
			if (definingQuery != null)
			{
				Expression node = _parameterExtractingExpressionVisitor.ExtractParameters(definingQuery.Body);
				node = _enumerableToQueryableMethodConvertingExpressionVisitor.Visit(node);
				node = new SelfReferenceEntityQueryableRewritingExpressionVisitor(this, entityType).Visit(node);
				node = Visit(node);
				node = _pendingSelectorExpandingExpressionVisitor.Visit(node);
				node = Reduce(node);
				navigationExpansionExpression = CreateNavigationExpansionExpression(node, entityType);
			}
			else
			{
				navigationExpansionExpression = CreateNavigationExpansionExpression(constantExpression, entityType);
			}
			return ApplyQueryFilter(navigationExpansionExpression);
		}
		return base.VisitConstant(constantExpression);
	}

	protected override Expression VisitExtension(Expression extensionExpression)
	{
		if (!(extensionExpression is NavigationExpansionExpression) && !(extensionExpression is OwnedNavigationReference))
		{
			return base.VisitExtension(extensionExpression);
		}
		return extensionExpression;
	}

	protected override Expression VisitMember(MemberExpression memberExpression)
	{
		Expression expression = Visit(memberExpression.Expression);
		if (expression is MaterializeCollectionNavigationExpression materializeCollectionNavigationExpression && memberExpression.Member.Name == "Count")
		{
			Expression expression2 = materializeCollectionNavigationExpression.Subquery;
			Type type = expression2.Type.TryGetSequenceType();
			if (expression2 is OwnedNavigationReference ownedNavigationReference && ownedNavigationReference.Navigation.IsCollection())
			{
				expression2 = Expression.Call(QueryableMethods.AsQueryable.MakeGenericMethod(type), expression2);
			}
			return Visit(Expression.Call(QueryableMethods.CountWithoutPredicate.MakeGenericMethod(type), expression2));
		}
		Expression expression3 = memberExpression.Update(expression);
		if (expression is NavigationExpansionExpression navigationExpansionExpression && navigationExpansionExpression.CardinalityReducingGenericMethodInfo != null)
		{
			Expression expression4 = new ExpandingExpressionVisitor(this, navigationExpansionExpression).Visit(expression3);
			if (expression4 != expression3)
			{
				expression3 = Visit(expression4);
			}
		}
		return expression3;
	}

	protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
	{
		MethodInfo method = methodCallExpression.Method;
		if (method.DeclaringType == typeof(Queryable) || method.DeclaringType == typeof(QueryableExtensions) || method.DeclaringType == typeof(EntityFrameworkQueryableExtensions))
		{
			MethodInfo methodInfo = (method.IsGenericMethod ? method.GetGenericMethodDefinition() : null);
			Expression expression = Visit(methodCallExpression.Arguments[0]);
			if (expression is NavigationExpansionExpression navigationExpansionExpression)
			{
				if (navigationExpansionExpression.PendingOrderings.Any() && methodInfo != QueryableMethods.ThenBy && methodInfo != QueryableMethods.ThenByDescending)
				{
					ApplyPendingOrderings(navigationExpansionExpression);
				}
				switch (method.Name)
				{
				case "AsQueryable":
					if (methodInfo == QueryableMethods.AsQueryable)
					{
						return navigationExpansionExpression;
					}
					goto default;
				case "Any":
					if (methodInfo == QueryableMethods.AnyWithoutPredicate)
					{
						goto IL_06ed;
					}
					if (methodInfo == QueryableMethods.AnyWithPredicate)
					{
						goto IL_073e;
					}
					goto default;
				case "Count":
					if (methodInfo == QueryableMethods.CountWithoutPredicate)
					{
						goto IL_06ed;
					}
					if (methodInfo == QueryableMethods.CountWithPredicate)
					{
						goto IL_073e;
					}
					goto default;
				case "LongCount":
					if (methodInfo == QueryableMethods.LongCountWithoutPredicate)
					{
						goto IL_06ed;
					}
					if (methodInfo == QueryableMethods.LongCountWithPredicate)
					{
						goto IL_073e;
					}
					goto default;
				case "All":
					if (methodInfo == QueryableMethods.All)
					{
						goto IL_073e;
					}
					goto default;
				case "Average":
					if (QueryableMethods.IsAverageWithoutSelector(method))
					{
						goto IL_0789;
					}
					if (QueryableMethods.IsAverageWithSelector(method))
					{
						goto IL_07d5;
					}
					goto default;
				case "Sum":
					if (QueryableMethods.IsSumWithoutSelector(method))
					{
						goto IL_0789;
					}
					if (QueryableMethods.IsSumWithSelector(method))
					{
						goto IL_07d5;
					}
					goto default;
				case "Max":
					if (methodInfo == QueryableMethods.MaxWithoutSelector)
					{
						goto IL_0789;
					}
					if (methodInfo == QueryableMethods.MaxWithSelector)
					{
						goto IL_07d5;
					}
					goto default;
				case "Min":
					if (methodInfo == QueryableMethods.MinWithoutSelector)
					{
						goto IL_0789;
					}
					if (methodInfo == QueryableMethods.MinWithSelector)
					{
						goto IL_07d5;
					}
					goto default;
				case "Distinct":
					if (methodInfo == QueryableMethods.Distinct)
					{
						return ProcessDistinctSkipTake(navigationExpansionExpression, methodInfo, null);
					}
					goto default;
				case "Skip":
					if (methodInfo == QueryableMethods.Skip)
					{
						goto IL_0832;
					}
					goto default;
				case "Take":
					if (methodInfo == QueryableMethods.Take)
					{
						goto IL_0832;
					}
					goto default;
				case "Contains":
					if (methodInfo == QueryableMethods.Contains)
					{
						return ProcessContains(navigationExpansionExpression, methodCallExpression.Arguments[1]);
					}
					goto default;
				case "First":
					if (methodInfo == QueryableMethods.FirstWithoutPredicate)
					{
						goto IL_08c5;
					}
					if (methodInfo == QueryableMethods.FirstWithPredicate)
					{
						goto IL_0940;
					}
					goto default;
				case "FirstOrDefault":
					if (methodInfo == QueryableMethods.FirstOrDefaultWithoutPredicate)
					{
						goto IL_08c5;
					}
					if (methodInfo == QueryableMethods.FirstOrDefaultWithPredicate)
					{
						goto IL_0940;
					}
					goto default;
				case "Single":
					if (methodInfo == QueryableMethods.SingleWithoutPredicate)
					{
						goto IL_08c5;
					}
					if (methodInfo == QueryableMethods.SingleWithPredicate)
					{
						goto IL_0940;
					}
					goto default;
				case "SingleOrDefault":
					if (methodInfo == QueryableMethods.SingleOrDefaultWithoutPredicate)
					{
						goto IL_08c5;
					}
					if (methodInfo == QueryableMethods.SingleOrDefaultWithPredicate)
					{
						goto IL_0940;
					}
					goto default;
				case "Last":
					if (methodInfo == QueryableMethods.LastWithoutPredicate)
					{
						goto IL_08c5;
					}
					if (methodInfo == QueryableMethods.LastWithPredicate)
					{
						goto IL_0940;
					}
					goto default;
				case "LastOrDefault":
					if (methodInfo == QueryableMethods.LastOrDefaultWithoutPredicate)
					{
						goto IL_08c5;
					}
					if (methodInfo == QueryableMethods.LastOrDefaultWithPredicate)
					{
						goto IL_0940;
					}
					goto default;
				case "Join":
					if (methodInfo == QueryableMethods.Join)
					{
						if (Visit(methodCallExpression.Arguments[1]) is NavigationExpansionExpression innerSource3)
						{
							return ProcessJoin(navigationExpansionExpression, innerSource3, methodCallExpression.Arguments[2].UnwrapLambdaFromQuote(), methodCallExpression.Arguments[3].UnwrapLambdaFromQuote(), methodCallExpression.Arguments[4].UnwrapLambdaFromQuote());
						}
						break;
					}
					goto default;
				case "LeftJoin":
					if (methodInfo == QueryableExtensions.LeftJoinMethodInfo)
					{
						if (Visit(methodCallExpression.Arguments[1]) is NavigationExpansionExpression innerSource2)
						{
							return ProcessLeftJoin(navigationExpansionExpression, innerSource2, methodCallExpression.Arguments[2].UnwrapLambdaFromQuote(), methodCallExpression.Arguments[3].UnwrapLambdaFromQuote(), methodCallExpression.Arguments[4].UnwrapLambdaFromQuote());
						}
						break;
					}
					goto default;
				case "SelectMany":
					if (methodInfo == QueryableMethods.SelectManyWithoutCollectionSelector)
					{
						return ProcessSelectMany(navigationExpansionExpression, methodCallExpression.Arguments[1].UnwrapLambdaFromQuote(), null);
					}
					if (methodInfo == QueryableMethods.SelectManyWithCollectionSelector)
					{
						return ProcessSelectMany(navigationExpansionExpression, methodCallExpression.Arguments[1].UnwrapLambdaFromQuote(), methodCallExpression.Arguments[2].UnwrapLambdaFromQuote());
					}
					goto default;
				case "Concat":
					if (methodInfo == QueryableMethods.Concat)
					{
						goto IL_0ae6;
					}
					goto default;
				case "Except":
					if (methodInfo == QueryableMethods.Except)
					{
						goto IL_0ae6;
					}
					goto default;
				case "Intersect":
					if (methodInfo == QueryableMethods.Intersect)
					{
						goto IL_0ae6;
					}
					goto default;
				case "Union":
					if (methodInfo == QueryableMethods.Union)
					{
						goto IL_0ae6;
					}
					goto default;
				case "Cast":
					if (methodInfo == QueryableMethods.Cast)
					{
						goto IL_0b34;
					}
					goto default;
				case "OfType":
					if (methodInfo == QueryableMethods.OfType)
					{
						goto IL_0b34;
					}
					goto default;
				case "Include":
				case "ThenInclude":
					return ProcessInclude(navigationExpansionExpression, methodCallExpression.Arguments[1], string.Equals(method.Name, "ThenInclude"));
				case "GroupBy":
					if (methodInfo == QueryableMethods.GroupByWithKeySelector)
					{
						return ProcessGroupBy(navigationExpansionExpression, methodCallExpression.Arguments[1].UnwrapLambdaFromQuote(), null, null);
					}
					if (methodInfo == QueryableMethods.GroupByWithKeyElementSelector)
					{
						return ProcessGroupBy(navigationExpansionExpression, methodCallExpression.Arguments[1].UnwrapLambdaFromQuote(), methodCallExpression.Arguments[2].UnwrapLambdaFromQuote(), null);
					}
					if (methodInfo == QueryableMethods.GroupByWithKeyElementResultSelector)
					{
						return ProcessGroupBy(navigationExpansionExpression, methodCallExpression.Arguments[1].UnwrapLambdaFromQuote(), methodCallExpression.Arguments[2].UnwrapLambdaFromQuote(), methodCallExpression.Arguments[3].UnwrapLambdaFromQuote());
					}
					if (methodInfo == QueryableMethods.GroupByWithKeyResultSelector)
					{
						return ProcessGroupBy(navigationExpansionExpression, methodCallExpression.Arguments[1].UnwrapLambdaFromQuote(), null, methodCallExpression.Arguments[2].UnwrapLambdaFromQuote());
					}
					goto default;
				case "OrderBy":
					if (methodInfo == QueryableMethods.OrderBy)
					{
						goto IL_0c77;
					}
					goto default;
				case "OrderByDescending":
					if (methodInfo == QueryableMethods.OrderByDescending)
					{
						goto IL_0c77;
					}
					goto default;
				case "ThenBy":
					if (methodInfo == QueryableMethods.ThenBy)
					{
						goto IL_0cb5;
					}
					goto default;
				case "ThenByDescending":
					if (methodInfo == QueryableMethods.ThenByDescending)
					{
						goto IL_0cb5;
					}
					goto default;
				case "Select":
					if (methodInfo == QueryableMethods.Select)
					{
						return ProcessSelect(navigationExpansionExpression, methodCallExpression.Arguments[1].UnwrapLambdaFromQuote());
					}
					goto default;
				case "Where":
					if (methodInfo == QueryableMethods.Where)
					{
						return ProcessWhere(navigationExpansionExpression, methodCallExpression.Arguments[1].UnwrapLambdaFromQuote());
					}
					goto default;
				case "DefaultIfEmpty":
					if (methodInfo == QueryableMethods.DefaultIfEmptyWithoutArgument)
					{
						return ProcessDefaultIfEmpty(navigationExpansionExpression);
					}
					goto default;
				default:
					{
						throw new InvalidOperationException(CoreStrings.QueryFailed(methodCallExpression.Print(), GetType().Name));
					}
					IL_0789:
					return ProcessAverageMaxMinSum(navigationExpansionExpression, methodInfo ?? method, null);
					IL_0cb5:
					return ProcessOrderByThenBy(navigationExpansionExpression, methodInfo, methodCallExpression.Arguments[1].UnwrapLambdaFromQuote(), thenBy: true);
					IL_0c77:
					return ProcessOrderByThenBy(navigationExpansionExpression, methodInfo, methodCallExpression.Arguments[1].UnwrapLambdaFromQuote(), thenBy: false);
					IL_0940:
					return ProcessFirstSingleLastOrDefault(navigationExpansionExpression, methodInfo, methodCallExpression.Arguments[1].UnwrapLambdaFromQuote(), methodCallExpression.Type);
					IL_08c5:
					return ProcessFirstSingleLastOrDefault(navigationExpansionExpression, methodInfo, null, methodCallExpression.Type);
					IL_073e:
					return ProcessAllAnyCountLongCount(navigationExpansionExpression, methodInfo, methodCallExpression.Arguments[1].UnwrapLambdaFromQuote());
					IL_0832:
					return ProcessDistinctSkipTake(navigationExpansionExpression, methodInfo, methodCallExpression.Arguments[1]);
					IL_0b34:
					return ProcessCastOfType(navigationExpansionExpression, methodInfo, methodCallExpression.Type.TryGetSequenceType());
					IL_0ae6:
					if (Visit(methodCallExpression.Arguments[1]) is NavigationExpansionExpression innerSource)
					{
						return ProcessSetOperation(navigationExpansionExpression, methodInfo, innerSource);
					}
					break;
					IL_06ed:
					return ProcessAllAnyCountLongCount(navigationExpansionExpression, methodInfo, null);
					IL_07d5:
					return ProcessAverageMaxMinSum(navigationExpansionExpression, methodInfo ?? method, methodCallExpression.Arguments[1].UnwrapLambdaFromQuote());
				}
			}
			if (methodInfo == QueryableMethods.AsQueryable)
			{
				if (expression is MaterializeCollectionNavigationExpression { Subquery: var subquery })
				{
					if (!(subquery is OwnedNavigationReference ownedNavigationReference) || !ownedNavigationReference.Navigation.IsCollection())
					{
						return subquery;
					}
					return Visit(Expression.Call(QueryableMethods.AsQueryable.MakeGenericMethod(subquery.Type.TryGetSequenceType()), subquery));
				}
				if (expression is OwnedNavigationReference ownedNavigationReference2 && ownedNavigationReference2.Navigation.IsCollection())
				{
					string parameterName = GetParameterName("o");
					NavigationTreeExpression navigationTreeExpression = new NavigationTreeExpression(ownedNavigationReference2.EntityReference);
					return new NavigationExpansionExpression(methodCallExpression, navigationTreeExpression, navigationTreeExpression, parameterName);
				}
				return expression;
			}
			if (expression.Type.TryGetElementType(typeof(IQueryable<>)) == null)
			{
				List<Expression> arguments = new Expression[1] { expression }.Concat<Expression>(methodCallExpression.Arguments.Skip(1).Select(Visit)).ToList();
				return ConvertToEnumerable(method, arguments);
			}
			throw new InvalidOperationException(CoreStrings.QueryFailed(methodCallExpression.Print(), GetType().Name));
		}
		if (method.IsGenericMethod && method.GetGenericMethodDefinition() == EnumerableMethods.ToList)
		{
			Expression expression2 = Visit(methodCallExpression.Arguments[0]);
			if (expression2 is MaterializeCollectionNavigationExpression materializeCollectionNavigationExpression2)
			{
				expression2 = materializeCollectionNavigationExpression2.Subquery;
			}
			return methodCallExpression.Update(null, new Expression[1] { expression2 });
		}
		if (method.IsGenericMethod && method.Name == "FromSqlOnQueryable" && methodCallExpression.Arguments.Count == 3 && methodCallExpression.Arguments[0] is ConstantExpression constantExpression && methodCallExpression.Arguments[1] is ConstantExpression && (methodCallExpression.Arguments[2] is ParameterExpression || methodCallExpression.Arguments[2] is ConstantExpression) && constantExpression.IsEntityQueryable())
		{
			IEntityType entityType = _queryCompilationContext.Model.FindEntityType(((IQueryable)constantExpression.Value).ElementType);
			NavigationExpansionExpression navigationExpansionExpression2 = CreateNavigationExpansionExpression(constantExpression, entityType);
			navigationExpansionExpression2.UpdateSource(methodCallExpression.Update(null, new Expression[3]
			{
				navigationExpansionExpression2.Source,
				methodCallExpression.Arguments[1],
				methodCallExpression.Arguments[2]
			}));
			return ApplyQueryFilter(navigationExpansionExpression2);
		}
		return ProcessUnknownMethod(methodCallExpression);
	}

	private Expression ProcessAllAnyCountLongCount(NavigationExpansionExpression source, MethodInfo genericMethod, LambdaExpression predicate)
	{
		if (predicate != null)
		{
			Expression body = ExpandNavigationsInLambdaExpression(source, predicate);
			return Expression.Call(genericMethod.GetGenericMethodDefinition().MakeGenericMethod(source.SourceElementType), source.Source, Expression.Quote(GenerateLambda(body, source.CurrentParameter)));
		}
		return Expression.Call(genericMethod.MakeGenericMethod(source.SourceElementType), source.Source);
	}

	private Expression ProcessAverageMaxMinSum(NavigationExpansionExpression source, MethodInfo method, LambdaExpression selector)
	{
		if (selector != null)
		{
			source = (NavigationExpansionExpression)ProcessSelect(source, selector);
			source = (NavigationExpansionExpression)_pendingSelectorExpandingExpressionVisitor.Visit(source);
			LambdaExpression lambdaExpression = GenerateLambda(source.PendingSelector, source.CurrentParameter);
			method = ((method.GetGenericArguments().Length != 2) ? method.MakeGenericMethod(source.SourceElementType) : method.MakeGenericMethod(source.SourceElementType, lambdaExpression.ReturnType));
			return Expression.Call(method, source.Source, lambdaExpression);
		}
		source = (NavigationExpansionExpression)_pendingSelectorExpandingExpressionVisitor.Visit(source);
		Expression expression = Reduce(source);
		if (method.GetGenericArguments().Length == 1)
		{
			method = method.MakeGenericMethod(expression.Type.TryGetSequenceType());
		}
		return Expression.Call(method, expression);
	}

	private Expression ProcessCastOfType(NavigationExpansionExpression source, MethodInfo genericMethod, Type castType)
	{
		if ((castType.IsInterface && castType.IsAssignableFrom(source.PendingSelector.Type)) || castType == typeof(object))
		{
			return source;
		}
		source = (NavigationExpansionExpression)_pendingSelectorExpandingExpressionVisitor.Visit(source);
		Expression expression = SnapshotExpression(source.PendingSelector);
		Expression arg = Reduce(source);
		MethodCallExpression source2 = Expression.Call(genericMethod.MakeGenericMethod(castType), arg);
		if (expression is EntityReference entityReference)
		{
			IEntityType entityType = entityReference.EntityType.GetTypesInHierarchy().FirstOrDefault((IEntityType et) => et.ClrType == castType);
			EntityType castEntityType = entityType as EntityType;
			if (castEntityType != null)
			{
				EntityReference entityReference2 = new EntityReference(castEntityType);
				if (entityReference.IsOptional)
				{
					entityReference2.MarkAsOptional();
				}
				entityReference2.SetIncludePaths(entityReference.IncludePaths);
				foreach (INavigation item in entityReference2.IncludePaths.Keys.Where((INavigation n) => !castEntityType.IsAssignableFrom(n.DeclaringEntityType) && !n.DeclaringEntityType.IsAssignableFrom(castEntityType)).ToList())
				{
					entityReference2.IncludePaths.Remove(item);
				}
				expression = entityReference2;
				goto IL_016d;
			}
		}
		expression = Expression.Default(castType);
		goto IL_016d;
		IL_016d:
		NavigationTreeExpression navigationTreeExpression = new NavigationTreeExpression(expression);
		string parameterName = GetParameterName("e");
		return new NavigationExpansionExpression(source2, navigationTreeExpression, navigationTreeExpression, parameterName);
	}

	private Expression ProcessContains(NavigationExpansionExpression source, Expression item)
	{
		source = (NavigationExpansionExpression)_pendingSelectorExpandingExpressionVisitor.Visit(source);
		Expression expression = Reduce(source);
		return Expression.Call(QueryableMethods.Contains.MakeGenericMethod(expression.Type.TryGetSequenceType()), expression, item);
	}

	private Expression ProcessDefaultIfEmpty(NavigationExpansionExpression source)
	{
		source.UpdateSource(Expression.Call(QueryableMethods.DefaultIfEmptyWithoutArgument.MakeGenericMethod(source.SourceElementType), source.Source));
		_entityReferenceOptionalMarkingExpressionVisitor.Visit(source.PendingSelector);
		return source;
	}

	private Expression ProcessDistinctSkipTake(NavigationExpansionExpression source, MethodInfo genericMethod, Expression count)
	{
		source = (NavigationExpansionExpression)_pendingSelectorExpandingExpressionVisitor.Visit(source);
		Expression value = SnapshotExpression(source.PendingSelector);
		Expression expression = Reduce(source);
		MethodCallExpression source2 = ((count == null) ? Expression.Call(genericMethod.MakeGenericMethod(expression.Type.TryGetSequenceType()), expression) : Expression.Call(genericMethod.MakeGenericMethod(expression.Type.TryGetSequenceType()), expression, count));
		NavigationTreeExpression navigationTreeExpression = new NavigationTreeExpression(value);
		string parameterName = GetParameterName("e");
		return new NavigationExpansionExpression(source2, navigationTreeExpression, navigationTreeExpression, parameterName);
	}

	private Expression ProcessFirstSingleLastOrDefault(NavigationExpansionExpression source, MethodInfo genericMethod, LambdaExpression predicate, Type returnType)
	{
		if (predicate != null)
		{
			Expression body = ExpandNavigationsInLambdaExpression(source, predicate);
			source.UpdateSource(Expression.Call(QueryableMethods.Where.MakeGenericMethod(source.SourceElementType), source.Source, Expression.Quote(GenerateLambda(body, source.CurrentParameter))));
			genericMethod = _predicateLessMethodInfo[genericMethod];
		}
		if (source.PendingSelector.Type != returnType)
		{
			source.ApplySelector(Expression.Convert(source.PendingSelector, returnType));
		}
		source.ConvertToSingleResult(genericMethod);
		return source;
	}

	private Expression ProcessGroupBy(NavigationExpansionExpression source, LambdaExpression keySelector, LambdaExpression elementSelector, LambdaExpression resultSelector)
	{
		Expression body = ExpandNavigationsInLambdaExpression(source, keySelector);
		Expression expression;
		if (elementSelector == null)
		{
			source = (NavigationExpansionExpression)_pendingSelectorExpandingExpressionVisitor.Visit(source);
			keySelector = GenerateLambda(body, source.CurrentParameter);
			elementSelector = GenerateLambda(source.PendingSelector, source.CurrentParameter);
			expression = ((resultSelector == null) ? Expression.Call(QueryableMethods.GroupByWithKeyElementSelector.MakeGenericMethod(source.CurrentParameter.Type, keySelector.ReturnType, elementSelector.ReturnType), source.Source, Expression.Quote(keySelector), Expression.Quote(elementSelector)) : Expression.Call(QueryableMethods.GroupByWithKeyElementResultSelector.MakeGenericMethod(source.CurrentParameter.Type, keySelector.ReturnType, elementSelector.ReturnType, resultSelector.ReturnType), source.Source, Expression.Quote(keySelector), Expression.Quote(elementSelector), Expression.Quote(resultSelector)));
		}
		else
		{
			source = (NavigationExpansionExpression)ProcessSelect(source, elementSelector);
			source = (NavigationExpansionExpression)new PendingSelectorExpandingExpressionVisitor(this, applyIncludes: true).Visit(source);
			keySelector = GenerateLambda(body, source.CurrentParameter);
			elementSelector = GenerateLambda(source.PendingSelector, source.CurrentParameter);
			expression = ((resultSelector == null) ? Expression.Call(QueryableMethods.GroupByWithKeyElementSelector.MakeGenericMethod(source.CurrentParameter.Type, keySelector.ReturnType, elementSelector.ReturnType), source.Source, Expression.Quote(keySelector), Expression.Quote(elementSelector)) : Expression.Call(QueryableMethods.GroupByWithKeyElementResultSelector.MakeGenericMethod(source.CurrentParameter.Type, keySelector.ReturnType, elementSelector.ReturnType, resultSelector.ReturnType), source.Source, Expression.Quote(keySelector), Expression.Quote(elementSelector), Expression.Quote(resultSelector)));
		}
		NavigationTreeExpression navigationTreeExpression = new NavigationTreeExpression(Expression.Default(expression.Type.TryGetSequenceType()));
		string parameterName = GetParameterName("e");
		return new NavigationExpansionExpression(expression, navigationTreeExpression, navigationTreeExpression, parameterName);
	}

	private Expression ProcessInclude(NavigationExpansionExpression source, Expression expression, bool thenInclude)
	{
		if (source.PendingSelector is NavigationTreeExpression { Value: EntityReference value })
		{
			if (value.EntityType.GetDefiningQuery() != null)
			{
				throw new InvalidOperationException(CoreStrings.IncludeOnEntityWithDefiningQueryNotSupported(value.EntityType.DisplayName()));
			}
			if (expression is ConstantExpression { Value: string value2 })
			{
				string[] array = value2.Split(new string[1] { "." }, StringSplitOptions.None);
				Queue<IncludeTreeNode> queue = new Queue<IncludeTreeNode>();
				queue.Enqueue(value.IncludePaths);
				string[] array2 = array;
				foreach (string text in array2)
				{
					int count = queue.Count;
					while (count-- > 0)
					{
						IncludeTreeNode includeTreeNode = queue.Dequeue();
						foreach (INavigation item in FindNavigations(includeTreeNode.EntityType, text))
						{
							IncludeTreeNode includeTreeNode2 = includeTreeNode.AddNavigation(item);
							PopulateEagerLoadedNavigations(includeTreeNode2);
							queue.Enqueue(includeTreeNode2);
						}
					}
					if (queue.Count == 0)
					{
						throw new InvalidOperationException("Invalid include path: '" + value2 + "' - couldn't find navigation for: '" + text + "'");
					}
				}
			}
			else
			{
				IncludeTreeNode includeTreeNode3 = (thenInclude ? value.LastIncludeTreeNode : value.IncludePaths);
				LambdaExpression lambdaExpression = expression.UnwrapLambdaFromQuote();
				IncludeTreeNode includeTreeNode4 = PopulateIncludeTree(includeTreeNode3, lambdaExpression.Body);
				if (includeTreeNode4 == null)
				{
					throw new InvalidOperationException("Lambda expression used inside Include is not valid.");
				}
				value.SetLastInclude(includeTreeNode4);
			}
			return source;
		}
		throw new InvalidOperationException("Include has been used on non entity queryable.");
	}

	private Expression ProcessJoin(NavigationExpansionExpression outerSource, NavigationExpansionExpression innerSource, LambdaExpression outerKeySelector, LambdaExpression innerKeySelector, LambdaExpression resultSelector)
	{
		if (innerSource.PendingOrderings.Any())
		{
			ApplyPendingOrderings(innerSource);
		}
		Expression body = ExpandNavigationsInLambdaExpression(outerSource, outerKeySelector);
		Expression body2 = ExpandNavigationsInLambdaExpression(innerSource, innerKeySelector);
		outerKeySelector = GenerateLambda(body, outerSource.CurrentParameter);
		innerKeySelector = GenerateLambda(body2, innerSource.CurrentParameter);
		Type type = TransparentIdentifierFactory.Create(outerSource.SourceElementType, innerSource.SourceElementType);
		FieldInfo declaredField = type.GetTypeInfo().GetDeclaredField("Outer");
		FieldInfo declaredField2 = type.GetTypeInfo().GetDeclaredField("Inner");
		LambdaExpression lambdaExpression = Expression.Lambda(Expression.New(type.GetTypeInfo().GetConstructors().Single(), new ParameterExpression[2] { outerSource.CurrentParameter, innerSource.CurrentParameter }, declaredField, declaredField2), outerSource.CurrentParameter, innerSource.CurrentParameter);
		MethodCallExpression source = Expression.Call(QueryableMethods.Join.MakeGenericMethod(outerSource.SourceElementType, innerSource.SourceElementType, outerKeySelector.ReturnType, lambdaExpression.ReturnType), outerSource.Source, innerSource.Source, Expression.Quote(outerKeySelector), Expression.Quote(innerKeySelector), Expression.Quote(lambdaExpression));
		NavigationTreeNode currentTree = new NavigationTreeNode(outerSource.CurrentTree, innerSource.CurrentTree);
		Expression pendingSelector = new ReplacingExpressionVisitor(new Expression[2]
		{
			resultSelector.Parameters[0],
			resultSelector.Parameters[1]
		}, new Expression[2] { outerSource.PendingSelector, innerSource.PendingSelector }).Visit(resultSelector.Body);
		string parameterName = GetParameterName("ti");
		return new NavigationExpansionExpression(source, currentTree, pendingSelector, parameterName);
	}

	private Expression ProcessLeftJoin(NavigationExpansionExpression outerSource, NavigationExpansionExpression innerSource, LambdaExpression outerKeySelector, LambdaExpression innerKeySelector, LambdaExpression resultSelector)
	{
		if (innerSource.PendingOrderings.Any())
		{
			ApplyPendingOrderings(innerSource);
		}
		Expression body = ExpandNavigationsInLambdaExpression(outerSource, outerKeySelector);
		Expression body2 = ExpandNavigationsInLambdaExpression(innerSource, innerKeySelector);
		outerKeySelector = GenerateLambda(body, outerSource.CurrentParameter);
		innerKeySelector = GenerateLambda(body2, innerSource.CurrentParameter);
		Type type = TransparentIdentifierFactory.Create(outerSource.SourceElementType, innerSource.SourceElementType);
		FieldInfo declaredField = type.GetTypeInfo().GetDeclaredField("Outer");
		FieldInfo declaredField2 = type.GetTypeInfo().GetDeclaredField("Inner");
		LambdaExpression lambdaExpression = Expression.Lambda(Expression.New(type.GetTypeInfo().GetConstructors().Single(), new ParameterExpression[2] { outerSource.CurrentParameter, innerSource.CurrentParameter }, declaredField, declaredField2), outerSource.CurrentParameter, innerSource.CurrentParameter);
		MethodCallExpression source = Expression.Call(QueryableExtensions.LeftJoinMethodInfo.MakeGenericMethod(outerSource.SourceElementType, innerSource.SourceElementType, outerKeySelector.ReturnType, lambdaExpression.ReturnType), outerSource.Source, innerSource.Source, Expression.Quote(outerKeySelector), Expression.Quote(innerKeySelector), Expression.Quote(lambdaExpression));
		Expression pendingSelector = innerSource.PendingSelector;
		pendingSelector = _entityReferenceOptionalMarkingExpressionVisitor.Visit(pendingSelector);
		NavigationTreeNode currentTree = new NavigationTreeNode(outerSource.CurrentTree, innerSource.CurrentTree);
		Expression pendingSelector2 = new ReplacingExpressionVisitor(new Expression[2]
		{
			resultSelector.Parameters[0],
			resultSelector.Parameters[1]
		}, new Expression[2] { outerSource.PendingSelector, pendingSelector }).Visit(resultSelector.Body);
		string parameterName = GetParameterName("ti");
		return new NavigationExpansionExpression(source, currentTree, pendingSelector2, parameterName);
	}

	private Expression ProcessOrderByThenBy(NavigationExpansionExpression source, MethodInfo genericMethod, LambdaExpression keySelector, bool thenBy)
	{
		Expression keySelector2;
		if (AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue18904", out var isEnabled) && isEnabled)
		{
			keySelector2 = ExpandNavigationsInLambdaExpression(source, keySelector);
		}
		else
		{
			keySelector2 = ReplacingExpressionVisitor.Replace(keySelector.Parameters[0], source.PendingSelector, keySelector.Body);
			keySelector2 = new ExpandingExpressionVisitor(this, source).Visit(keySelector2);
			keySelector2 = _subqueryMemberPushdownExpressionVisitor.Visit(keySelector2);
		}
		if (thenBy)
		{
			source.AppendPendingOrdering(genericMethod, keySelector2);
		}
		else
		{
			source.AddPendingOrdering(genericMethod, keySelector2);
		}
		return source;
	}

	private Expression ProcessSelect(NavigationExpansionExpression source, LambdaExpression selector)
	{
		if (source.SourceElementType.IsGenericType && source.SourceElementType.GetGenericTypeDefinition() == typeof(IGrouping<, >) && (!selector.ReturnType.IsGenericType || !(selector.ReturnType.GetGenericTypeDefinition() == typeof(IGrouping<, >))))
		{
			LambdaExpression lambdaExpression = GenerateLambda(ExpandNavigationsInLambdaExpression(source, selector), source.CurrentParameter);
			MethodCallExpression source2 = Expression.Call(QueryableMethods.Select.MakeGenericMethod(source.SourceElementType, lambdaExpression.ReturnType), source.Source, Expression.Quote(lambdaExpression));
			NavigationTreeExpression navigationTreeExpression = new NavigationTreeExpression(Expression.Default(lambdaExpression.ReturnType));
			string parameterName = GetParameterName("e");
			return new NavigationExpansionExpression(source2, navigationTreeExpression, navigationTreeExpression, parameterName);
		}
		Expression selector2 = ReplacingExpressionVisitor.Replace(selector.Parameters[0], source.PendingSelector, selector.Body);
		source.ApplySelector(selector2);
		return source;
	}

	private Expression ProcessSelectMany(NavigationExpansionExpression source, LambdaExpression collectionSelector, LambdaExpression resultSelector)
	{
		Expression expression = ExpandNavigationsInLambdaExpression(source, collectionSelector);
		if (expression is MaterializeCollectionNavigationExpression materializeCollectionNavigationExpression)
		{
			expression = materializeCollectionNavigationExpression.Subquery;
		}
		if (expression is NavigationExpansionExpression node)
		{
			NavigationExpansionExpression navigationExpansionExpression = (NavigationExpansionExpression)_pendingSelectorExpandingExpressionVisitor.Visit(node);
			NavigationTreeExpression navigationTreeExpression = new NavigationTreeExpression(SnapshotExpression(navigationExpansionExpression.PendingSelector));
			collectionSelector = GenerateLambda(navigationExpansionExpression, source.CurrentParameter);
			Type type = collectionSelector.ReturnType.TryGetSequenceType();
			collectionSelector = Expression.Lambda(typeof(Func<, >).MakeGenericType(source.SourceElementType, typeof(IEnumerable<>).MakeGenericType(type)), collectionSelector.Body, collectionSelector.Parameters[0]);
			Type type2 = TransparentIdentifierFactory.Create(source.SourceElementType, type);
			FieldInfo declaredField = type2.GetTypeInfo().GetDeclaredField("Outer");
			FieldInfo declaredField2 = type2.GetTypeInfo().GetDeclaredField("Inner");
			ParameterExpression parameterExpression = Expression.Parameter(type, "c");
			LambdaExpression lambdaExpression = Expression.Lambda(Expression.New(type2.GetTypeInfo().GetConstructors().Single(), new ParameterExpression[2] { source.CurrentParameter, parameterExpression }, declaredField, declaredField2), source.CurrentParameter, parameterExpression);
			MethodCallExpression source2 = Expression.Call(QueryableMethods.SelectManyWithCollectionSelector.MakeGenericMethod(source.SourceElementType, type, lambdaExpression.ReturnType), source.Source, Expression.Quote(collectionSelector), Expression.Quote(lambdaExpression));
			NavigationTreeNode currentTree = new NavigationTreeNode(source.CurrentTree, navigationTreeExpression);
			Expression pendingSelector = ((resultSelector == null) ? navigationTreeExpression : new ReplacingExpressionVisitor(new Expression[2]
			{
				resultSelector.Parameters[0],
				resultSelector.Parameters[1]
			}, new Expression[2] { source.PendingSelector, navigationTreeExpression }).Visit(resultSelector.Body));
			string parameterName = GetParameterName("ti");
			return new NavigationExpansionExpression(source2, currentTree, pendingSelector, parameterName);
		}
		throw new InvalidOperationException(CoreStrings.QueryFailed(collectionSelector.Print(), GetType().Name));
	}

	private Expression ProcessSetOperation(NavigationExpansionExpression outerSource, MethodInfo genericMethod, NavigationExpansionExpression innerSource)
	{
		outerSource = (NavigationExpansionExpression)_pendingSelectorExpandingExpressionVisitor.Visit(outerSource);
		Expression expression = SnapshotExpression(outerSource.PendingSelector);
		innerSource = (NavigationExpansionExpression)_pendingSelectorExpandingExpressionVisitor.Visit(innerSource);
		Expression expression2 = SnapshotExpression(innerSource.PendingSelector);
		if (!CompareIncludes(expression, expression2))
		{
			throw new InvalidOperationException(CoreStrings.SetOperationWithDifferentIncludesInOperands);
		}
		Expression expression3 = Reduce(outerSource);
		Expression expression4 = Reduce(innerSource);
		Type type = expression3.Type.TryGetSequenceType();
		Type type2 = expression4.Type.TryGetSequenceType();
		MethodCallExpression source = Expression.Call(genericMethod.MakeGenericMethod(type.IsAssignableFrom(type2) ? type : type2), expression3, expression4);
		NavigationTreeExpression navigationTreeExpression = new NavigationTreeExpression(type.IsAssignableFrom(type2) ? expression : expression2);
		string parameterName = GetParameterName("e");
		return new NavigationExpansionExpression(source, navigationTreeExpression, navigationTreeExpression, parameterName);
	}

	private Expression ProcessUnknownMethod(MethodCallExpression methodCallExpression)
	{
		Type type = methodCallExpression.Type.TryGetElementType(typeof(IQueryable<>));
		if (type != null && methodCallExpression.Object == null && methodCallExpression.Arguments.All((Expression a) => a.GetLambdaOrNull() == null) && methodCallExpression.Method.IsGenericMethod && methodCallExpression.Method.GetGenericArguments().Length == 1 && methodCallExpression.Method.GetGenericArguments()[0] == type && methodCallExpression.Arguments.Count > 0 && methodCallExpression.Arguments.Skip(1).All((Expression e) => e.Type.TryGetElementType(typeof(IQueryable<>)) == null) && Visit(methodCallExpression.Arguments[0]) is NavigationExpansionExpression navigationExpansionExpression && navigationExpansionExpression.Type == methodCallExpression.Type)
		{
			NavigationExpansionExpression navigationExpansionExpression2 = (NavigationExpansionExpression)_pendingSelectorExpandingExpressionVisitor.Visit(navigationExpansionExpression);
			Expression value = SnapshotExpression(navigationExpansionExpression2.PendingSelector);
			Expression expression = Reduce(navigationExpansionExpression2);
			MethodCallExpression source = Expression.Call(methodCallExpression.Method.GetGenericMethodDefinition().MakeGenericMethod(type), new Expression[1] { expression }.Concat<Expression>(from e in methodCallExpression.Arguments.Skip(1)
				select Visit(e)));
			NavigationTreeExpression navigationTreeExpression = new NavigationTreeExpression(value);
			string parameterName = GetParameterName("e");
			return new NavigationExpansionExpression(source, navigationTreeExpression, navigationTreeExpression, parameterName);
		}
		return base.VisitMethodCall(methodCallExpression);
	}

	private Expression ProcessWhere(NavigationExpansionExpression source, LambdaExpression predicate)
	{
		Expression body = ExpandNavigationsInLambdaExpression(source, predicate);
		source.UpdateSource(Expression.Call(QueryableMethods.Where.MakeGenericMethod(source.SourceElementType), source.Source, Expression.Quote(GenerateLambda(body, source.CurrentParameter))));
		return source;
	}

	private void ApplyPendingOrderings(NavigationExpansionExpression source)
	{
		if (!source.PendingOrderings.Any())
		{
			return;
		}
		foreach (var pendingOrdering in source.PendingOrderings)
		{
			MethodInfo item = pendingOrdering.OrderingMethod;
			Expression item2 = pendingOrdering.KeySelector;
			bool isEnabled;
			Expression body = ((!(AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue18904", out isEnabled) && isEnabled)) ? _pendingSelectorExpandingExpressionVisitor.Visit(Visit(item2)) : item2);
			LambdaExpression lambdaExpression = GenerateLambda(body, source.CurrentParameter);
			source.UpdateSource(Expression.Call(item.MakeGenericMethod(source.SourceElementType, lambdaExpression.ReturnType), source.Source, lambdaExpression));
		}
		source.ClearPendingOrderings();
	}

	private Expression ApplyQueryFilter(NavigationExpansionExpression navigationExpansionExpression)
	{
		if (!_queryCompilationContext.IgnoreQueryFilters)
		{
			Type sequenceType = navigationExpansionExpression.Type.GetSequenceType();
			IEntityType entityType = _queryCompilationContext.Model.FindEntityType(sequenceType);
			IEntityType rootType = entityType.GetRootType();
			LambdaExpression queryFilter = rootType.GetQueryFilter();
			if (queryFilter != null)
			{
				if (!_parameterizedQueryFilterPredicateCache.TryGetValue(rootType, out var value))
				{
					value = queryFilter;
					value = (LambdaExpression)_parameterExtractingExpressionVisitor.ExtractParameters(value);
					value = (LambdaExpression)_enumerableToQueryableMethodConvertingExpressionVisitor.Visit(value);
					MethodCallExpression expression = Expression.Call(QueryableMethods.Where.MakeGenericMethod(rootType.ClrType), NullAsyncQueryProvider.Instance.CreateEntityQueryableExpression(rootType.ClrType), value);
					value = ((MethodCallExpression)_entityEqualityRewritingExpressionVisitor.Rewrite(expression)).Arguments[1].UnwrapLambdaFromQuote();
					_parameterizedQueryFilterPredicateCache[rootType] = value;
				}
				value = (LambdaExpression)new SelfReferenceEntityQueryableRewritingExpressionVisitor(this, entityType).Visit(value);
				ParameterExpression parameterExpression = value.Parameters[0];
				if (parameterExpression.Type != sequenceType)
				{
					ParameterExpression parameterExpression2 = Expression.Parameter(sequenceType, parameterExpression.Name);
					value = Expression.Lambda(ReplacingExpressionVisitor.Replace(parameterExpression, parameterExpression2, value.Body), parameterExpression2);
				}
				MethodCallExpression node = Expression.Call(QueryableMethods.Where.MakeGenericMethod(sequenceType), navigationExpansionExpression, value);
				return Visit(node);
			}
		}
		return navigationExpansionExpression;
	}

	private bool CompareIncludes(Expression outer, Expression inner)
	{
		if (outer is EntityReference entityReference && inner is EntityReference entityReference2)
		{
			return entityReference.IncludePaths.Equals(entityReference2.IncludePaths);
		}
		if (outer is NewExpression newExpression && inner is NewExpression newExpression2)
		{
			if (newExpression.Arguments.Count != newExpression2.Arguments.Count)
			{
				return false;
			}
			for (int i = 0; i < newExpression.Arguments.Count; i++)
			{
				if (!CompareIncludes(newExpression.Arguments[i], newExpression2.Arguments[i]))
				{
					return false;
				}
			}
			return true;
		}
		if (outer is DefaultExpression defaultExpression && inner is DefaultExpression defaultExpression2)
		{
			return defaultExpression.Type == defaultExpression2.Type;
		}
		return false;
	}

	private MethodCallExpression ConvertToEnumerable(MethodInfo queryableMethod, List<Expression> arguments)
	{
		Type[] array = (queryableMethod.IsGenericMethod ? queryableMethod.GetGenericArguments() : null);
		List<Expression> list = arguments.Select((Expression arg) => (!(arg is UnaryExpression { NodeType: ExpressionType.Quote } unaryExpression) || !(unaryExpression.Operand is LambdaExpression)) ? arg : unaryExpression.Operand).ToList();
		if (queryableMethod.Name == "Min")
		{
			if (array.Length == 1)
			{
				Type type2 = array[0];
				MethodInfo methodInfo = EnumerableMethods.GetMinWithoutSelector(type2);
				if (!IsNumericType(type2))
				{
					methodInfo = methodInfo.MakeGenericMethod(type2);
				}
				return Expression.Call(methodInfo, list);
			}
			if (array.Length == 2)
			{
				Type type3 = array[1];
				MethodInfo minWithSelector = EnumerableMethods.GetMinWithSelector(type3);
				minWithSelector = (IsNumericType(type3) ? minWithSelector.MakeGenericMethod(type3) : minWithSelector.MakeGenericMethod(array));
				return Expression.Call(minWithSelector, list);
			}
		}
		if (queryableMethod.Name == "Max")
		{
			if (array.Length == 1)
			{
				Type type4 = array[0];
				MethodInfo methodInfo2 = EnumerableMethods.GetMaxWithoutSelector(type4);
				if (!IsNumericType(type4))
				{
					methodInfo2 = methodInfo2.MakeGenericMethod(type4);
				}
				return Expression.Call(methodInfo2, list);
			}
			if (array.Length == 2)
			{
				Type type5 = array[1];
				MethodInfo maxWithSelector = EnumerableMethods.GetMaxWithSelector(type5);
				maxWithSelector = (IsNumericType(type5) ? maxWithSelector.MakeGenericMethod(type5) : maxWithSelector.MakeGenericMethod(array));
				return Expression.Call(maxWithSelector, list);
			}
		}
		foreach (MethodInfo declaredMethod in typeof(Enumerable).GetTypeInfo().GetDeclaredMethods(queryableMethod.Name))
		{
			MethodInfo methodInfo3 = declaredMethod;
			if (methodInfo3.IsGenericMethod)
			{
				if (array == null || methodInfo3.GetGenericArguments().Length != array.Length)
				{
					continue;
				}
				methodInfo3 = methodInfo3.MakeGenericMethod(array);
			}
			ParameterInfo[] parameters = methodInfo3.GetParameters();
			if (parameters.Length != list.Count)
			{
				continue;
			}
			bool flag = true;
			for (int i = 0; i < parameters.Length; i++)
			{
				if (!parameters[i].ParameterType.IsAssignableFrom(list[i].Type))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return Expression.Call(methodInfo3, list);
			}
		}
		throw new InvalidOperationException("Unable to convert queryable method to enumerable method.");
		static bool IsNumericType(Type type)
		{
			type = type.UnwrapNullableType();
			if (!(type == typeof(int)) && !(type == typeof(long)) && !(type == typeof(float)) && !(type == typeof(double)))
			{
				return type == typeof(decimal);
			}
			return true;
		}
	}

	private NavigationExpansionExpression CreateNavigationExpansionExpression(Expression sourceExpression, IEntityType entityType)
	{
		EntityReference entityReference = new EntityReference(entityType);
		PopulateEagerLoadedNavigations(entityReference.IncludePaths);
		NavigationTreeExpression navigationTreeExpression = new NavigationTreeExpression(entityReference);
		string parameterName = GetParameterName(entityType.ShortName()[0].ToString().ToLower());
		return new NavigationExpansionExpression(sourceExpression, navigationTreeExpression, navigationTreeExpression, parameterName);
	}

	private Expression ExpandNavigationsInLambdaExpression(NavigationExpansionExpression source, LambdaExpression lambdaExpression)
	{
		Expression node = ReplacingExpressionVisitor.Replace(lambdaExpression.Parameters[0], source.PendingSelector, lambdaExpression.Body);
		node = new ExpandingExpressionVisitor(this, source).Visit(node);
		node = _subqueryMemberPushdownExpressionVisitor.Visit(node);
		node = Visit(node);
		return _pendingSelectorExpandingExpressionVisitor.Visit(node);
	}

	private static IEnumerable<INavigation> FindNavigations(IEntityType entityType, string navigationName)
	{
		INavigation navigation = entityType.FindNavigation(navigationName);
		if (navigation != null)
		{
			yield return navigation;
			yield break;
		}
		foreach (INavigation item in from et in entityType.GetDerivedTypes()
			select et.FindDeclaredNavigation(navigationName) into n
			where n != null
			select n)
		{
			yield return item;
		}
	}

	private LambdaExpression GenerateLambda(Expression body, ParameterExpression currentParameter)
	{
		return Expression.Lambda(Reduce(body), currentParameter);
	}

	private string GetParameterName(string prefix)
	{
		string text = prefix;
		int num = 0;
		while (_parameterNames.Contains(text))
		{
			text = $"{prefix}{num++}";
		}
		_parameterNames.Add(text);
		return text;
	}

	private static void PopulateEagerLoadedNavigations(IncludeTreeNode includeTreeNode)
	{
		IEntityType entityType = includeTreeNode.EntityType;
		foreach (INavigation item in from n in entityType.GetNavigations().Concat(entityType.GetDerivedNavigations())
			where n.IsEagerLoaded()
			select n)
		{
			PopulateEagerLoadedNavigations(includeTreeNode.AddNavigation(item));
		}
	}

	private IncludeTreeNode PopulateIncludeTree(IncludeTreeNode includeTreeNode, Expression expression)
	{
		if (!(expression is ParameterExpression))
		{
			if (expression is MemberExpression memberExpression)
			{
				Expression expression2 = memberExpression.Expression;
				Type convertedType = null;
				if (expression2 is UnaryExpression unaryExpression && (unaryExpression.NodeType == ExpressionType.Convert || unaryExpression.NodeType == ExpressionType.ConvertChecked || unaryExpression.NodeType == ExpressionType.TypeAs))
				{
					convertedType = unaryExpression.Type;
					expression2 = unaryExpression.Operand;
				}
				IncludeTreeNode includeTreeNode2 = PopulateIncludeTree(includeTreeNode, expression2);
				IEntityType entityType = includeTreeNode2.EntityType;
				if (convertedType != null)
				{
					entityType = entityType.GetTypesInHierarchy().FirstOrDefault((IEntityType et) => et.ClrType == convertedType);
					if (entityType == null)
					{
						throw new InvalidOperationException("Invalid include.");
					}
				}
				INavigation navigation = entityType.FindNavigation(memberExpression.Member);
				if (navigation != null)
				{
					IncludeTreeNode includeTreeNode3 = includeTreeNode2.AddNavigation(navigation);
					PopulateEagerLoadedNavigations(includeTreeNode3);
					return includeTreeNode3;
				}
			}
			return null;
		}
		return includeTreeNode;
	}

	private Expression Reduce(Expression source)
	{
		return _reducingExpressionVisitor.Visit(source);
	}

	private Expression SnapshotExpression(Expression selector)
	{
		if (!(selector is EntityReference entityReference))
		{
			if (!(selector is NavigationTreeExpression navigationTreeExpression))
			{
				if (!(selector is NewExpression newExpression))
				{
					if (selector is OwnedNavigationReference ownedNavigationReference)
					{
						return ownedNavigationReference.EntityReference.Clone();
					}
					return Expression.Default(selector.Type);
				}
				if (newExpression.Constructor == null)
				{
					return Expression.Default(newExpression.Type);
				}
				Expression[] array = new Expression[newExpression.Arguments.Count];
				for (int i = 0; i < newExpression.Arguments.Count; i++)
				{
					array[i] = SnapshotExpression(newExpression.Arguments[i]);
				}
				return newExpression.Update(array);
			}
			return SnapshotExpression(navigationTreeExpression.Value);
		}
		return entityReference.Clone();
	}
}
