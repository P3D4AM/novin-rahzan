using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class EntityEqualityRewritingExpressionVisitor : ExpressionVisitor
{
	protected struct EntityOrDtoType
	{
		public IEntityType EntityType;

		public Dictionary<string, EntityOrDtoType> DtoType;

		public bool IsEntityType => EntityType != null;

		public bool IsDto => DtoType != null;

		public static EntityOrDtoType FromEntityReferenceExpression(EntityReferenceExpression ere)
		{
			EntityOrDtoType result = default(EntityOrDtoType);
			result.EntityType = (ere.IsEntityType ? ere.EntityType : null);
			result.DtoType = (ere.IsDtoType ? ere.DtoType : null);
			return result;
		}

		public static EntityOrDtoType FromDtoType(Dictionary<string, EntityOrDtoType> dtoType)
		{
			EntityOrDtoType result = default(EntityOrDtoType);
			result.DtoType = dtoType;
			return result;
		}
	}

	protected class EntityReferenceExpression : Expression
	{
		[JetBrains.Annotations.CanBeNull]
		private readonly INavigation _lastNavigation;

		public sealed override ExpressionType NodeType => ExpressionType.Extension;

		[JetBrains.Annotations.NotNull]
		public Expression Underlying { get; }

		public override Type Type => Underlying.Type;

		[JetBrains.Annotations.CanBeNull]
		public IEntityType EntityType { get; }

		[JetBrains.Annotations.CanBeNull]
		public INavigation LastNavigation
		{
			get
			{
				if (EntityType != null)
				{
					return _lastNavigation;
				}
				return null;
			}
		}

		[JetBrains.Annotations.CanBeNull]
		public Dictionary<string, EntityOrDtoType> DtoType { get; }

		public bool SubqueryTraversed { get; }

		public bool IsDtoType => DtoType != null;

		public bool IsEntityType => EntityType != null;

		public EntityReferenceExpression(Expression underlying, Dictionary<string, EntityOrDtoType> dtoType)
		{
			Underlying = underlying;
			DtoType = dtoType;
		}

		public EntityReferenceExpression(Expression underlying, IEntityType entityType)
			: this(underlying, entityType, subqueryTraversed: false)
		{
		}

		private EntityReferenceExpression(Expression underlying, IEntityType entityType, bool subqueryTraversed)
		{
			Underlying = underlying;
			EntityType = entityType;
			SubqueryTraversed = subqueryTraversed;
		}

		public EntityReferenceExpression(Expression underlying, IEntityType entityType, INavigation lastNavigation, Dictionary<string, EntityOrDtoType> dtoType, bool subqueryTraversed)
		{
			Underlying = underlying;
			EntityType = entityType;
			_lastNavigation = lastNavigation;
			DtoType = dtoType;
			SubqueryTraversed = subqueryTraversed;
		}

		public virtual Expression TraverseProperty(string propertyName, Expression destinationExpression)
		{
			if (IsEntityType)
			{
				INavigation navigation = EntityType.FindNavigation(propertyName);
				if (navigation == null)
				{
					return destinationExpression;
				}
				return new EntityReferenceExpression(destinationExpression, navigation.GetTargetType(), navigation, null, SubqueryTraversed);
			}
			if (IsDtoType)
			{
				if (DtoType.TryGetValue(propertyName, out var value))
				{
					if (!value.IsEntityType)
					{
						return new EntityReferenceExpression(destinationExpression, value.DtoType);
					}
					return new EntityReferenceExpression(destinationExpression, value.EntityType);
				}
				return destinationExpression;
			}
			throw new InvalidOperationException("Unknown type info");
		}

		public EntityReferenceExpression Update(Expression newUnderlying)
		{
			return new EntityReferenceExpression(newUnderlying, EntityType, null, DtoType, SubqueryTraversed);
		}

		protected override Expression VisitChildren(ExpressionVisitor visitor)
		{
			return Update(visitor.Visit(Underlying));
		}

		public virtual void Print(ExpressionPrinter expressionPrinter)
		{
			expressionPrinter.Visit(Underlying);
			if (IsEntityType)
			{
				expressionPrinter.Append($".EntityType({EntityType})");
			}
			else if (IsDtoType)
			{
				expressionPrinter.Append(".DTO");
			}
			if (SubqueryTraversed)
			{
				expressionPrinter.Append(".SubqueryTraversed");
			}
		}

		public override string ToString()
		{
			return string.Format("{0}[{1}{2}]", Underlying, IsEntityType ? EntityType.ShortName() : "DTO", SubqueryTraversed ? ", Subquery" : "");
		}
	}

	private const string RuntimeParameterPrefix = "__entity_equality_";

	private readonly QueryCompilationContext _queryCompilationContext;

	private readonly IDiagnosticsLogger<DbLoggerCategory.Query> _logger;

	private static readonly MethodInfo _objectEqualsMethodInfo = typeof(object).GetRuntimeMethod("Equals", new Type[2]
	{
		typeof(object),
		typeof(object)
	});

	private static readonly MethodInfo _enumerableContainsMethodInfo = typeof(Enumerable).GetTypeInfo().GetDeclaredMethods("Contains").Single((MethodInfo mi) => mi.GetParameters().Length == 2);

	private static readonly MethodInfo _parameterValueExtractor = typeof(EntityEqualityRewritingExpressionVisitor).GetTypeInfo().GetDeclaredMethod("ParameterValueExtractor");

	private static readonly MethodInfo _parameterListValueExtractor = typeof(EntityEqualityRewritingExpressionVisitor).GetTypeInfo().GetDeclaredMethod("ParameterListValueExtractor");

	public EntityEqualityRewritingExpressionVisitor(QueryCompilationContext queryCompilationContext)
	{
		_queryCompilationContext = queryCompilationContext;
		_logger = queryCompilationContext.Logger;
	}

	public virtual Expression Rewrite(Expression expression)
	{
		return Unwrap(Visit(expression));
	}

	protected override Expression VisitConstant(ConstantExpression constantExpression)
	{
		if (!constantExpression.IsEntityQueryable())
		{
			return constantExpression;
		}
		return new EntityReferenceExpression(constantExpression, _queryCompilationContext.Model.FindEntityType(((IQueryable)constantExpression.Value).ElementType));
	}

	protected override Expression VisitNew(NewExpression newExpression)
	{
		if (newExpression.Constructor == null)
		{
			return newExpression;
		}
		ReadOnlyCollection<Expression> source = Visit(newExpression.Arguments);
		NewExpression visitedExpression = newExpression.Update(source.Select(Unwrap));
		if (newExpression.Members == null)
		{
			return visitedExpression;
		}
		Dictionary<string, EntityOrDtoType> dictionary = (from ai in source.Select((Expression a, int i) => (Arg: a, Index: i))
			where ai.Arg is EntityReferenceExpression
			select ai).ToDictionary(((Expression Arg, int Index) ai) => visitedExpression.Members[ai.Index].Name, ((Expression Arg, int Index) ai) => EntityOrDtoType.FromEntityReferenceExpression((EntityReferenceExpression)ai.Arg));
		if (dictionary.Count != 0)
		{
			return new EntityReferenceExpression(visitedExpression, dictionary);
		}
		return visitedExpression;
	}

	protected override Expression VisitMemberInit(MemberInitExpression memberInitExpression)
	{
		Expression expression = Visit(memberInitExpression.NewExpression);
		(IEnumerable<MemberBinding>, Dictionary<string, EntityOrDtoType>) tuple = VisitMemberBindings(memberInitExpression.Bindings);
		IEnumerable<MemberBinding> item = tuple.Item1;
		Dictionary<string, EntityOrDtoType> item2 = tuple.Item2;
		MemberInitExpression memberInitExpression2 = memberInitExpression.Update((NewExpression)Unwrap(expression), item);
		if (item2 != null)
		{
			return new EntityReferenceExpression(memberInitExpression2, item2);
		}
		return memberInitExpression2;
		(IEnumerable<MemberBinding>, Dictionary<string, EntityOrDtoType>) VisitMemberBindings(ReadOnlyCollection<MemberBinding> bindings)
		{
			MemberBinding[] array = new MemberBinding[bindings.Count];
			Dictionary<string, EntityOrDtoType> dictionary = null;
			for (int i = 0; i < bindings.Count; i++)
			{
				if (bindings[i] is MemberAssignment memberAssignment)
				{
					MemberAssignment memberAssignment2 = VisitMemberAssignment(memberAssignment);
					if (memberAssignment2.Expression is EntityReferenceExpression ere)
					{
						if (dictionary == null)
						{
							dictionary = new Dictionary<string, EntityOrDtoType>();
						}
						dictionary[memberAssignment.Member.Name] = EntityOrDtoType.FromEntityReferenceExpression(ere);
					}
					array[i] = memberAssignment.Update(Unwrap(memberAssignment2.Expression));
				}
				else
				{
					array[i] = VisitMemberBinding(bindings[i]);
				}
			}
			return (array, dictionary);
		}
	}

	protected override Expression VisitNewArray(NewArrayExpression newArrayExpression)
	{
		return newArrayExpression.Update(Visit(newArrayExpression.Expressions).Select(Unwrap));
	}

	protected override Expression VisitListInit(ListInitExpression listInitExpression)
	{
		return listInitExpression.Update((NewExpression)Unwrap(listInitExpression.NewExpression), listInitExpression.Initializers.Select(VisitElementInit));
	}

	protected override ElementInit VisitElementInit(ElementInit elementInit)
	{
		return Expression.ElementInit(elementInit.AddMethod, Visit(elementInit.Arguments).Select(Unwrap));
	}

	protected override Expression VisitMember(MemberExpression memberExpression)
	{
		Expression expression = base.Visit(memberExpression.Expression);
		MemberExpression memberExpression2 = memberExpression.Update(Unwrap(expression));
		if (!(expression is EntityReferenceExpression entityReferenceExpression))
		{
			return memberExpression2;
		}
		return entityReferenceExpression.TraverseProperty(memberExpression.Member.Name, memberExpression2);
	}

	protected override Expression VisitBinary(BinaryExpression binaryExpression)
	{
		Expression expression = Visit(binaryExpression.Left);
		Expression? expression2 = Visit(binaryExpression.Right);
		Expression expression3 = expression;
		Expression expression4 = expression2;
		if (binaryExpression.NodeType == ExpressionType.Equal || binaryExpression.NodeType == ExpressionType.NotEqual)
		{
			Expression expression5 = RewriteEquality(binaryExpression.NodeType == ExpressionType.Equal, expression3, expression4);
			if (expression5 != null)
			{
				return expression5;
			}
		}
		return binaryExpression.Update(Unwrap(expression3), binaryExpression.Conversion, Unwrap(expression4));
	}

	protected override Expression VisitUnary(UnaryExpression unaryExpression)
	{
		Expression expression = Visit(unaryExpression.Operand);
		UnaryExpression unaryExpression2 = unaryExpression.Update(Unwrap(expression));
		if (unaryExpression.NodeType == ExpressionType.Convert)
		{
			if (!(expression is EntityReferenceExpression { EntityType: not null } entityReferenceExpression))
			{
				return unaryExpression2;
			}
			Type castType = unaryExpression.Type;
			IEntityType entityType = entityReferenceExpression.EntityType.GetTypesInHierarchy().FirstOrDefault((IEntityType et) => et.ClrType == castType);
			if (entityType == null)
			{
				return unaryExpression2;
			}
			return new EntityReferenceExpression(unaryExpression2, entityType);
		}
		if (!(expression is EntityReferenceExpression entityReferenceExpression2))
		{
			return unaryExpression2;
		}
		return entityReferenceExpression2.Update(unaryExpression2);
	}

	protected override Expression VisitTypeBinary(TypeBinaryExpression typeBinaryExpression)
	{
		Expression expression = Visit(typeBinaryExpression.Expression);
		TypeBinaryExpression typeBinaryExpression2 = typeBinaryExpression.Update(Unwrap(expression));
		if (!(expression is EntityReferenceExpression entityReferenceExpression))
		{
			return typeBinaryExpression2;
		}
		return entityReferenceExpression.Update(typeBinaryExpression2);
	}

	protected override Expression VisitConditional(ConditionalExpression conditionalExpression)
	{
		Expression expression = Visit(conditionalExpression.Test);
		Expression expression2 = Visit(conditionalExpression.IfTrue);
		Expression expression3 = Visit(conditionalExpression.IfFalse);
		ConditionalExpression conditionalExpression2 = conditionalExpression.Update(Unwrap(expression), Unwrap(expression2), Unwrap(expression3));
		EntityReferenceExpression entityReferenceExpression = (expression2 as EntityReferenceExpression) ?? (expression3 as EntityReferenceExpression);
		if (entityReferenceExpression != null)
		{
			return entityReferenceExpression.Update(conditionalExpression2);
		}
		return conditionalExpression2;
	}

	protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
	{
		MethodInfo method = methodCallExpression.Method;
		MethodInfo methodInfo = (method.IsGenericMethod ? method.GetGenericMethodDefinition() : null);
		ReadOnlyCollection<Expression> arguments = methodCallExpression.Arguments;
		Expression expression;
		if (method.Name == "Equals" && methodCallExpression.Object != null && methodCallExpression.Arguments.Count == 1)
		{
			expression = Visit(methodCallExpression.Object);
			Expression? expression2 = Visit(arguments[0]);
			Expression expression3 = expression;
			Expression expression4 = expression2;
			return RewriteEquality(equality: true, expression3, expression4) ?? methodCallExpression.Update(Unwrap(expression3), new Expression[1] { Unwrap(expression4) });
		}
		if (method.Equals(_objectEqualsMethodInfo))
		{
			expression = Visit(arguments[0]);
			Expression? expression5 = Visit(arguments[1]);
			Expression expression6 = expression;
			Expression expression7 = expression5;
			return RewriteEquality(equality: true, expression6, expression7) ?? methodCallExpression.Update(null, new Expression[2]
			{
				Unwrap(expression6),
				Unwrap(expression7)
			});
		}
		Expression expression8;
		if (methodCallExpression.TryGetEFPropertyArguments(out expression, out var propertyName) || methodCallExpression.TryGetEFIndexerArguments(out expression, out propertyName))
		{
			expression8 = Visit(arguments[0]);
			MethodCallExpression methodCallExpression2 = methodCallExpression.Update(null, new Expression[2]
			{
				Unwrap(expression8),
				arguments[1]
			});
			if (!(expression8 is EntityReferenceExpression entityReferenceExpression))
			{
				return methodCallExpression2;
			}
			return entityReferenceExpression.TraverseProperty(propertyName, methodCallExpression2);
		}
		switch (method.Name)
		{
		case "Contains":
			if (methodInfo == QueryableMethods.Contains)
			{
				return VisitContainsMethodCall(methodCallExpression);
			}
			break;
		case "OrderBy":
			if (!(methodInfo == QueryableMethods.OrderBy))
			{
				break;
			}
			goto IL_0393;
		case "OrderByDescending":
			if (!(methodInfo == QueryableMethods.OrderByDescending))
			{
				break;
			}
			goto IL_0393;
		case "ThenBy":
			if (!(methodInfo == QueryableMethods.ThenBy))
			{
				break;
			}
			goto IL_0393;
		case "ThenByDescending":
			if (!(methodInfo == QueryableMethods.ThenByDescending))
			{
				break;
			}
			goto IL_0393;
		case "Select":
			if (!(methodInfo == QueryableMethods.Select))
			{
				break;
			}
			goto IL_03ca;
		case "SelectMany":
			if (!(methodInfo == QueryableMethods.SelectManyWithoutCollectionSelector) && !(methodInfo == QueryableMethods.SelectManyWithCollectionSelector))
			{
				break;
			}
			goto IL_03ca;
		case "GroupJoin":
			if (!(methodInfo == QueryableMethods.GroupJoin))
			{
				break;
			}
			goto IL_03fd;
		case "Join":
			if (!(methodInfo == QueryableMethods.Join))
			{
				break;
			}
			goto IL_03fd;
		case "LeftJoin":
			if (!(methodInfo == QueryableExtensions.LeftJoinMethodInfo))
			{
				break;
			}
			goto IL_03fd;
		case "OfType":
			if (methodInfo == QueryableMethods.OfType)
			{
				return VisitOfType(methodCallExpression);
			}
			break;
		case "GroupBy":
			{
				if (!(methodInfo == QueryableMethods.GroupByWithKeySelector) && !(methodInfo == QueryableMethods.GroupByWithKeyElementSelector) && !(methodInfo == QueryableMethods.GroupByWithKeyResultSelector))
				{
					_ = methodInfo == QueryableMethods.GroupByWithKeyElementResultSelector;
				}
				break;
			}
			IL_0393:
			return VisitOrderingMethodCall(methodCallExpression);
			IL_03fd:
			return VisitJoinMethodCall(methodCallExpression);
			IL_03ca:
			return VisitSelectMethodCall(methodCallExpression);
		}
		if (methodInfo == _enumerableContainsMethodInfo || (method.DeclaringType.GetInterfaces().Contains(typeof(IList)) && string.Equals(method.Name, "Contains")))
		{
			return VisitContainsMethodCall(methodCallExpression);
		}
		if (arguments.Count == 0)
		{
			return methodCallExpression.Update(Unwrap(Visit(methodCallExpression.Object)), Array.Empty<Expression>());
		}
		Expression[] array = new Expression[arguments.Count];
		LambdaExpression[] array2 = (from a in arguments
			select a.GetLambdaOrNull() into l
			where l != null
			select l).ToArray();
		expression8 = Visit(arguments[0]);
		array[0] = Unwrap(expression8);
		if (methodCallExpression.Object == null && expression8 is EntityReferenceExpression entityReferenceExpression2 && (array2.Length == 0 || (array2.Length == 1 && array2[0].Parameters.Count == 1)))
		{
			for (int i = 1; i < arguments.Count; i++)
			{
				int num = i;
				LambdaExpression lambdaOrNull = arguments[i].GetLambdaOrNull();
				array[num] = ((lambdaOrNull != null) ? Unwrap(RewriteAndVisitLambda(lambdaOrNull, entityReferenceExpression2)) : Unwrap(Visit(arguments[i])));
			}
			Type type = methodCallExpression.Method.GetParameters()[0].ParameterType.TryGetSequenceType();
			if (type != null)
			{
				Type type2 = methodCallExpression.Method.ReturnType.TryGetSequenceType();
				if ((object)type2 != null && type2 == type)
				{
					return entityReferenceExpression2.Update(methodCallExpression.Update(null, array));
				}
				if (methodCallExpression.Method.ReturnType == type)
				{
					return new EntityReferenceExpression(methodCallExpression.Update(null, array), entityReferenceExpression2.EntityType, null, entityReferenceExpression2.DtoType, subqueryTraversed: true);
				}
			}
			return methodCallExpression.Update(null, array);
		}
		for (int j = 1; j < arguments.Count; j++)
		{
			array[j] = Unwrap(Visit(arguments[j]));
		}
		return methodCallExpression.Update(Unwrap(Visit(methodCallExpression.Object)), array);
	}

	private Expression VisitContainsMethodCall(MethodCallExpression methodCallExpression)
	{
		MethodCallExpression methodCallExpression2 = methodCallExpression;
		Expression newSource;
		Expression newItem;
		Expression @object;
		Expression expression;
		if (methodCallExpression2.Arguments.Count != 2)
		{
			@object = methodCallExpression2.Object;
			expression = methodCallExpression2.Arguments[0];
			newSource = @object;
			newItem = expression;
		}
		else
		{
			expression = methodCallExpression2.Arguments[0];
			@object = methodCallExpression2.Arguments[1];
			newSource = expression;
			newItem = @object;
		}
		@object = Visit(newSource);
		expression = Visit(newItem);
		newSource = @object;
		newItem = expression;
		IEntityType entityType = (newSource as EntityReferenceExpression)?.EntityType;
		IEntityType entityType2 = (newItem as EntityReferenceExpression)?.EntityType;
		if (entityType == null && entityType2 == null)
		{
			return NoTranslation();
		}
		if (entityType != null && entityType2 != null && entityType.GetRootType() != entityType2.GetRootType())
		{
			return Expression.Constant(false);
		}
		IEntityType entityType3 = entityType ?? entityType2;
		IReadOnlyList<IProperty> readOnlyList = entityType3.FindPrimaryKey()?.Properties;
		IProperty property;
		Expression arg;
		if (readOnlyList != null)
		{
			if (readOnlyList.Count != 1)
			{
				throw new InvalidOperationException(CoreStrings.EntityEqualityContainsWithCompositeKeyNotSupported(entityType3.DisplayName()));
			}
			property = readOnlyList[0];
			bool isEnabled;
			if (newSource is ConstantExpression constantExpression)
			{
				IEnumerable enumerable = (IEnumerable)constantExpression.Value;
				Type type = typeof(List<>).MakeGenericType(property.ClrType.MakeNullable());
				IList list = (IList)Activator.CreateInstance(type);
				IClrPropertyGetter getter = property.GetGetter();
				foreach (object item in enumerable)
				{
					list.Add(getter.GetClrValue(item));
				}
				arg = Expression.Constant(list, type);
			}
			else if (AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue20485", out isEnabled) && isEnabled && newSource is ParameterExpression parameterExpression && parameterExpression.Name.StartsWith("__", StringComparison.Ordinal))
			{
				LambdaExpression valueExtractor = Expression.Lambda(Expression.Call(_parameterListValueExtractor.MakeGenericMethod(entityType3.ClrType, property.ClrType.MakeNullable()), QueryCompilationContext.QueryContextParameter, Expression.Constant(parameterExpression.Name, typeof(string)), Expression.Constant(property, typeof(IProperty))), QueryCompilationContext.QueryContextParameter);
				string name = "__entity_equality_" + parameterExpression.Name.Substring("__".Length) + "_" + property.Name;
				arg = _queryCompilationContext.RegisterRuntimeParameter(name, valueExtractor);
			}
			else
			{
				if (newSource is ParameterExpression parameterExpression2)
				{
					string? name2 = parameterExpression2.Name;
					if (name2 != null && name2.StartsWith("__", StringComparison.Ordinal))
					{
						LambdaExpression valueExtractor2 = Expression.Lambda(Expression.Call(_parameterListValueExtractor.MakeGenericMethod(entityType3.ClrType, property.ClrType.MakeNullable()), QueryCompilationContext.QueryContextParameter, Expression.Constant(parameterExpression2.Name, typeof(string)), Expression.Constant(property, typeof(IProperty))), QueryCompilationContext.QueryContextParameter);
						string name3 = "__entity_equality_" + parameterExpression2.Name.Substring("__".Length) + "_" + property.Name;
						arg = _queryCompilationContext.RegisterRuntimeParameter(name3, valueExtractor2);
						goto IL_0463;
					}
				}
				ParameterExpression parameterExpression3 = Expression.Parameter(entityType3.ClrType, "v");
				LambdaExpression expression2 = Expression.Lambda(CreatePropertyAccessExpression(parameterExpression3, property), parameterExpression3);
				arg = Expression.Call(QueryableMethods.Select.MakeGenericMethod(entityType3.ClrType, property.ClrType.MakeNullable()), Unwrap(newSource), Expression.Quote(expression2));
			}
			goto IL_0463;
		}
		throw new InvalidOperationException(CoreStrings.EntityEqualityOnKeylessEntityNotSupported(entityType3.DisplayName()));
		IL_0463:
		Expression arg2 = (newItem.IsNullConstantExpression() ? Expression.Constant(null, entityType3.ClrType) : CreatePropertyAccessExpression(Unwrap(newItem), property));
		return Expression.Call((Unwrap(newSource).Type.IsQueryableType() ? QueryableMethods.Contains : _enumerableContainsMethodInfo).MakeGenericMethod(property.ClrType.MakeNullable()), arg, arg2);
		Expression NoTranslation()
		{
			if (methodCallExpression2.Arguments.Count != 2)
			{
				return methodCallExpression2.Update(Unwrap(newSource), new Expression[1] { Unwrap(newItem) });
			}
			return methodCallExpression2.Update(null, new Expression[2]
			{
				Unwrap(newSource),
				Unwrap(newItem)
			});
		}
	}

	private Expression VisitOrderingMethodCall(MethodCallExpression methodCallExpression)
	{
		ReadOnlyCollection<Expression> arguments = methodCallExpression.Arguments;
		Expression expression = Visit(arguments[0]);
		if (!(expression is EntityReferenceExpression entityReferenceExpression))
		{
			return methodCallExpression.Update(null, new Expression[2]
			{
				expression,
				Unwrap(Visit(arguments[1]))
			});
		}
		LambdaExpression lambdaExpression = RewriteAndVisitLambda(arguments[1].UnwrapLambdaFromQuote(), entityReferenceExpression);
		if (!(lambdaExpression.Body is EntityReferenceExpression { EntityType: { } entityType }))
		{
			return entityReferenceExpression.Update(methodCallExpression.Update(null, new Expression[2]
			{
				Unwrap(expression),
				Unwrap(lambdaExpression)
			}));
		}
		MethodInfo genericMethodDefinition = methodCallExpression.Method.GetGenericMethodDefinition();
		bool firstOrdering2 = genericMethodDefinition == QueryableMethods.OrderBy || genericMethodDefinition == QueryableMethods.OrderByDescending;
		bool ascending2 = genericMethodDefinition == QueryableMethods.OrderBy || genericMethodDefinition == QueryableMethods.ThenBy;
		IReadOnlyList<IProperty> obj = entityType.FindPrimaryKey()?.Properties ?? throw new InvalidOperationException(CoreStrings.EntityEqualityOnKeylessEntityNotSupported(entityType.DisplayName()));
		Expression expression2 = Unwrap(expression);
		Expression target = Unwrap(lambdaExpression.Body);
		ParameterExpression parameterExpression = lambdaExpression.Parameters.Single();
		foreach (IProperty item in obj)
		{
			ParameterExpression parameterExpression2 = Expression.Parameter(parameterExpression.Type, parameterExpression.Name);
			LambdaExpression expression3 = Expression.Lambda(ReplacingExpressionVisitor.Replace(parameterExpression, parameterExpression2, CreatePropertyAccessExpression(target, item)), parameterExpression2);
			expression2 = Expression.Call(GetOrderingMethodInfo(firstOrdering2, ascending2).MakeGenericMethod(parameterExpression.Type, item.ClrType.MakeNullable()), expression2, Expression.Quote(expression3));
			firstOrdering2 = false;
		}
		return entityReferenceExpression.Update(expression2);
		static MethodInfo GetOrderingMethodInfo(bool firstOrdering, bool ascending)
		{
			if (firstOrdering)
			{
				if (!ascending)
				{
					return QueryableMethods.OrderByDescending;
				}
				return QueryableMethods.OrderBy;
			}
			if (!ascending)
			{
				return QueryableMethods.ThenByDescending;
			}
			return QueryableMethods.ThenBy;
		}
	}

	private Expression VisitSelectMethodCall(MethodCallExpression methodCallExpression)
	{
		ReadOnlyCollection<Expression> arguments = methodCallExpression.Arguments;
		Expression expression = Visit(arguments[0]);
		if (!(expression is EntityReferenceExpression entityReferenceExpression))
		{
			if (arguments.Count != 2)
			{
				if (arguments.Count != 3)
				{
					throw new InvalidOperationException(CoreStrings.QueryFailed(methodCallExpression.Print(), GetType().Name));
				}
				return methodCallExpression.Update(null, new Expression[3]
				{
					expression,
					Unwrap(Visit(arguments[1])),
					Unwrap(Visit(arguments[2]))
				});
			}
			return methodCallExpression.Update(null, new Expression[2]
			{
				expression,
				Unwrap(Visit(arguments[1]))
			});
		}
		if (arguments.Count == 2)
		{
			LambdaExpression lambda = arguments[1].UnwrapLambdaFromQuote();
			LambdaExpression lambdaExpression = RewriteAndVisitLambda(lambda, entityReferenceExpression);
			MethodCallExpression methodCallExpression2 = methodCallExpression.Update(null, new Expression[2]
			{
				Unwrap(expression),
				Unwrap(lambdaExpression)
			});
			if (!(lambdaExpression.Body is EntityReferenceExpression entityReferenceExpression2))
			{
				return methodCallExpression2;
			}
			return entityReferenceExpression2.Update(methodCallExpression2);
		}
		if (arguments.Count == 3)
		{
			LambdaExpression lambda2 = arguments[1].UnwrapLambdaFromQuote();
			LambdaExpression lambdaExpression2 = RewriteAndVisitLambda(lambda2, entityReferenceExpression);
			LambdaExpression lambdaExpression3 = arguments[2].UnwrapLambdaFromQuote();
			LambdaExpression lambdaExpression4 = ((lambdaExpression2.Body is EntityReferenceExpression source) ? RewriteAndVisitLambda(lambdaExpression3, entityReferenceExpression, source) : ((LambdaExpression)Visit(lambdaExpression3)));
			MethodCallExpression methodCallExpression2 = methodCallExpression.Update(null, new Expression[3]
			{
				Unwrap(expression),
				Unwrap(lambdaExpression2),
				Unwrap(lambdaExpression4)
			});
			if (!(lambdaExpression4.Body is EntityReferenceExpression entityReferenceExpression3))
			{
				return methodCallExpression2;
			}
			return entityReferenceExpression3.Update(methodCallExpression2);
		}
		throw new InvalidOperationException(CoreStrings.QueryFailed(methodCallExpression.Print(), GetType().Name));
	}

	private Expression VisitJoinMethodCall(MethodCallExpression methodCallExpression)
	{
		ReadOnlyCollection<Expression> arguments = methodCallExpression.Arguments;
		if (arguments.Count != 5)
		{
			return base.VisitMethodCall(methodCallExpression);
		}
		Expression expression = Visit(arguments[0]);
		Expression expression2 = Visit(arguments[1]);
		LambdaExpression lambdaExpression = arguments[2].UnwrapLambdaFromQuote();
		LambdaExpression lambdaExpression2 = arguments[3].UnwrapLambdaFromQuote();
		LambdaExpression lambdaExpression3 = arguments[4].UnwrapLambdaFromQuote();
		if (!(expression is EntityReferenceExpression entityReferenceExpression) || !(expression2 is EntityReferenceExpression entityReferenceExpression2))
		{
			return methodCallExpression.Update(null, new Expression[5]
			{
				Unwrap(expression),
				Unwrap(expression2),
				Unwrap(Visit(lambdaExpression)),
				Unwrap(Visit(lambdaExpression2)),
				Unwrap(Visit(lambdaExpression3))
			});
		}
		LambdaExpression lambdaExpression4 = RewriteAndVisitLambda(lambdaExpression, entityReferenceExpression);
		LambdaExpression lambdaExpression5 = RewriteAndVisitLambda(lambdaExpression2, entityReferenceExpression2);
		LambdaExpression lambdaExpression6 = RewriteAndVisitLambda(lambdaExpression3, entityReferenceExpression, entityReferenceExpression2);
		MethodCallExpression methodCallExpression2;
		if (lambdaExpression4.Body is EntityReferenceExpression entityReferenceExpression3 && lambdaExpression5.Body is EntityReferenceExpression entityReferenceExpression4 && entityReferenceExpression3.IsEntityType && entityReferenceExpression4.IsEntityType && entityReferenceExpression3.EntityType.GetRootType() == entityReferenceExpression4.EntityType.GetRootType())
		{
			IEntityType entityType = entityReferenceExpression3.EntityType;
			IReadOnlyList<IProperty> readOnlyList = entityType.FindPrimaryKey()?.Properties;
			if (readOnlyList == null)
			{
				throw new InvalidOperationException(CoreStrings.EntityEqualityOnKeylessEntityNotSupported(entityType.DisplayName()));
			}
			if (readOnlyList.Count > 1 && (entityReferenceExpression3.SubqueryTraversed || entityReferenceExpression4.SubqueryTraversed))
			{
				throw new InvalidOperationException(CoreStrings.EntityEqualitySubqueryWithCompositeKeyNotSupported(entityType.DisplayName()));
			}
			Type[] genericArguments = methodCallExpression.Method.GetGenericArguments();
			Expression expression3 = CreateKeyAccessExpression(Unwrap(entityReferenceExpression3), readOnlyList);
			lambdaExpression4 = Expression.Lambda(typeof(Func<, >).MakeGenericType(genericArguments[0], expression3.Type), expression3, lambdaExpression4.TailCall, lambdaExpression4.Parameters);
			Expression expression4 = CreateKeyAccessExpression(Unwrap(entityReferenceExpression4), readOnlyList);
			lambdaExpression5 = Expression.Lambda(typeof(Func<, >).MakeGenericType(genericArguments[1], expression4.Type), expression4, lambdaExpression5.TailCall, lambdaExpression5.Parameters);
			methodCallExpression2 = Expression.Call(methodCallExpression.Method.GetGenericMethodDefinition().MakeGenericMethod(genericArguments[0], genericArguments[1], expression3.Type, genericArguments[3]), Unwrap(expression), Unwrap(expression2), Expression.Quote(lambdaExpression4), Expression.Quote(lambdaExpression5), Expression.Quote(Unwrap(lambdaExpression6)));
		}
		else
		{
			methodCallExpression2 = methodCallExpression.Update(null, new Expression[5]
			{
				Unwrap(expression),
				Unwrap(expression2),
				Expression.Quote(Unwrap(lambdaExpression4)),
				Expression.Quote(Unwrap(lambdaExpression5)),
				Expression.Quote(Unwrap(lambdaExpression6))
			});
		}
		if (!(lambdaExpression6.Body is EntityReferenceExpression entityReferenceExpression5))
		{
			return methodCallExpression2;
		}
		return entityReferenceExpression5.Update(methodCallExpression2);
	}

	private Expression VisitOfType(MethodCallExpression methodCallExpression)
	{
		Expression expression = Visit(methodCallExpression.Arguments[0]);
		MethodCallExpression methodCallExpression2 = methodCallExpression.Update(null, new Expression[1] { Unwrap(expression) });
		if (!(expression is EntityReferenceExpression { EntityType: not null } entityReferenceExpression))
		{
			return methodCallExpression2;
		}
		Type castType = methodCallExpression.Type.TryGetSequenceType();
		IEntityType entityType = entityReferenceExpression.EntityType.GetTypesInHierarchy().FirstOrDefault((IEntityType et) => et.ClrType == castType);
		if (entityType == null)
		{
			return methodCallExpression2;
		}
		return new EntityReferenceExpression(methodCallExpression2, entityType);
	}

	private LambdaExpression RewriteAndVisitLambda(LambdaExpression lambda, EntityReferenceExpression source)
	{
		return Expression.Lambda(lambda.Type, Visit(ReplacingExpressionVisitor.Replace(lambda.Parameters.Single(), source.Update(lambda.Parameters.Single()), lambda.Body)), lambda.TailCall, lambda.Parameters);
	}

	private LambdaExpression RewriteAndVisitLambda(LambdaExpression lambda, EntityReferenceExpression source1, EntityReferenceExpression source2)
	{
		Expression expression = lambda.Parameters[0];
		Expression expression2 = source1.Update(lambda.Parameters[0]);
		Expression expression3 = lambda.Parameters[1];
		Expression expression4 = source2.Update(lambda.Parameters[1]);
		return Expression.Lambda(lambda.Type, Visit(new ReplacingExpressionVisitor(new Expression[2] { expression, expression3 }, new Expression[2] { expression2, expression4 }).Visit(lambda.Body)), lambda.TailCall, lambda.Parameters);
	}

	private Expression RewriteEquality(bool equality, Expression left, Expression right)
	{
		EntityReferenceExpression entityReferenceExpression = left as EntityReferenceExpression;
		EntityReferenceExpression entityReferenceExpression2 = right as EntityReferenceExpression;
		if ((entityReferenceExpression == null && entityReferenceExpression2 == null) || (entityReferenceExpression != null && entityReferenceExpression.IsDtoType) || (entityReferenceExpression2 != null && entityReferenceExpression2.IsDtoType))
		{
			return null;
		}
		if (left.IsNullConstantExpression())
		{
			if (right.IsNullConstantExpression())
			{
				if (!equality)
				{
					return Expression.Constant(false);
				}
				return Expression.Constant(true);
			}
			if (entityReferenceExpression2 == null || !entityReferenceExpression2.IsEntityType)
			{
				return null;
			}
			return RewriteNullEquality(equality, entityReferenceExpression2.EntityType, entityReferenceExpression2.Underlying, entityReferenceExpression2.LastNavigation);
		}
		if (right.IsNullConstantExpression())
		{
			if (entityReferenceExpression == null || !entityReferenceExpression.IsEntityType)
			{
				return null;
			}
			return RewriteNullEquality(equality, entityReferenceExpression.EntityType, entityReferenceExpression.Underlying, entityReferenceExpression.LastNavigation);
		}
		if (entityReferenceExpression != null && entityReferenceExpression2 != null && entityReferenceExpression.EntityType.GetRootType() != entityReferenceExpression2.EntityType.GetRootType())
		{
			return Expression.Constant(!equality);
		}
		IEntityType entityType = (entityReferenceExpression ?? entityReferenceExpression2).EntityType;
		return RewriteEntityEquality(equality, entityType, Unwrap(left), entityReferenceExpression?.LastNavigation, Unwrap(right), entityReferenceExpression2?.LastNavigation, (entityReferenceExpression != null && entityReferenceExpression.SubqueryTraversed) || (entityReferenceExpression2?.SubqueryTraversed ?? false));
	}

	private Expression RewriteNullEquality(bool equality, [JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] Expression nonNullExpression, [JetBrains.Annotations.CanBeNull] INavigation lastNavigation)
	{
		if (lastNavigation != null && lastNavigation.IsCollection())
		{
			_logger.PossibleUnintendedCollectionNavigationNullComparisonWarning(lastNavigation);
			return RewriteNullEquality(equality, lastNavigation.DeclaringEntityType, UnwrapLastNavigation(nonNullExpression), null);
		}
		IReadOnlyList<IProperty> readOnlyList = entityType.FindPrimaryKey()?.Properties;
		if (readOnlyList == null)
		{
			throw new InvalidOperationException(CoreStrings.EntityEqualityOnKeylessEntityNotSupported(entityType.DisplayName()));
		}
		return Expression.MakeBinary(equality ? ExpressionType.Equal : ExpressionType.NotEqual, CreatePropertyAccessExpression(nonNullExpression, readOnlyList[0]), Expression.Constant(null));
	}

	private Expression RewriteEntityEquality(bool equality, [JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] Expression left, [JetBrains.Annotations.CanBeNull] INavigation leftNavigation, [JetBrains.Annotations.NotNull] Expression right, [JetBrains.Annotations.CanBeNull] INavigation rightNavigation, bool subqueryTraversed)
	{
		if ((leftNavigation != null && leftNavigation.IsCollection()) || (rightNavigation != null && rightNavigation.IsCollection()))
		{
			if (leftNavigation != null && leftNavigation.Equals(rightNavigation))
			{
				_logger.PossibleUnintendedReferenceComparisonWarning(left, right);
				return RewriteEntityEquality(equality, leftNavigation.DeclaringEntityType, UnwrapLastNavigation(left), null, UnwrapLastNavigation(right), null, subqueryTraversed);
			}
			return Expression.Constant(!equality);
		}
		IReadOnlyList<IProperty> readOnlyList = entityType.FindPrimaryKey()?.Properties;
		if (readOnlyList == null)
		{
			throw new InvalidOperationException(CoreStrings.EntityEqualityOnKeylessEntityNotSupported(entityType.DisplayName()));
		}
		if (subqueryTraversed && readOnlyList.Count > 1)
		{
			throw new InvalidOperationException(CoreStrings.EntityEqualitySubqueryWithCompositeKeyNotSupported(entityType.DisplayName()));
		}
		return Expression.MakeBinary(equality ? ExpressionType.Equal : ExpressionType.NotEqual, CreateKeyAccessExpression(Unwrap(left), readOnlyList), CreateKeyAccessExpression(Unwrap(right), readOnlyList));
	}

	protected override Expression VisitExtension(Expression extensionExpression)
	{
		if (!(extensionExpression is EntityReferenceExpression))
		{
			return base.VisitExtension(extensionExpression);
		}
		return extensionExpression;
	}

	private Expression CreateKeyAccessExpression(Expression target, IReadOnlyList<IProperty> properties)
	{
		if (properties.Count != 1)
		{
			return Expression.New(AnonymousObject.AnonymousObjectCtor, Expression.NewArrayInit(typeof(object), properties.Select((IProperty p) => Expression.Convert(CreatePropertyAccessExpression(target, p), typeof(object))).Cast<Expression>().ToArray()));
		}
		return CreatePropertyAccessExpression(target, properties[0]);
	}

	private Expression CreatePropertyAccessExpression(Expression target, IProperty property)
	{
		if (target is ConstantExpression constantExpression)
		{
			return Expression.Constant(property.GetGetter().GetClrValue(constantExpression.Value), property.ClrType.MakeNullable());
		}
		if (CanEvaluate(target))
		{
			object entity = Expression.Lambda<Func<object>>(Expression.Convert(target, typeof(object)), Array.Empty<ParameterExpression>()).Compile()();
			return Expression.Constant(property.GetGetter().GetClrValue(entity), property.ClrType.MakeNullable());
		}
		if (AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue20485", out var isEnabled) && isEnabled)
		{
			if (target is ParameterExpression parameterExpression && parameterExpression.Name.StartsWith("__", StringComparison.Ordinal))
			{
				LambdaExpression valueExtractor = Expression.Lambda(Expression.Call(_parameterValueExtractor.MakeGenericMethod(property.ClrType.MakeNullable()), QueryCompilationContext.QueryContextParameter, Expression.Constant(parameterExpression.Name, typeof(string)), Expression.Constant(property, typeof(IProperty))), QueryCompilationContext.QueryContextParameter);
				string name = "__entity_equality_" + parameterExpression.Name.Substring("__".Length) + "_" + property.Name;
				return _queryCompilationContext.RegisterRuntimeParameter(name, valueExtractor);
			}
		}
		else if (target is ParameterExpression parameterExpression2)
		{
			string? name2 = parameterExpression2.Name;
			if (name2 != null && name2.StartsWith("__", StringComparison.Ordinal))
			{
				LambdaExpression valueExtractor2 = Expression.Lambda(Expression.Call(_parameterValueExtractor.MakeGenericMethod(property.ClrType.MakeNullable()), QueryCompilationContext.QueryContextParameter, Expression.Constant(parameterExpression2.Name, typeof(string)), Expression.Constant(property, typeof(IProperty))), QueryCompilationContext.QueryContextParameter);
				string name3 = "__entity_equality_" + parameterExpression2.Name.Substring("__".Length) + "_" + property.Name;
				return _queryCompilationContext.RegisterRuntimeParameter(name3, valueExtractor2);
			}
		}
		return target.CreateEFPropertyExpression(property);
	}

	private static bool CanEvaluate(Expression expression)
	{
		if (!(expression is ConstantExpression))
		{
			if (!(expression is NewExpression newExpression))
			{
				if (expression is MemberInitExpression memberInitExpression)
				{
					if (CanEvaluate(memberInitExpression.NewExpression))
					{
						return memberInitExpression.Bindings.All((MemberBinding mb) => mb is MemberAssignment memberAssignment && CanEvaluate(memberAssignment.Expression));
					}
					return false;
				}
				return false;
			}
			return newExpression.Arguments.All((Expression e) => CanEvaluate(e));
		}
		return true;
	}

	private static T ParameterValueExtractor<T>(QueryContext context, string baseParameterName, IProperty property)
	{
		object obj = context.ParameterValues[baseParameterName];
		if (obj != null)
		{
			return (T)property.GetGetter().GetClrValue(obj);
		}
		return (T)(object)null;
	}

	private static List<TProperty> ParameterListValueExtractor<TEntity, TProperty>(QueryContext context, string baseParameterName, IProperty property)
	{
		if (!(context.ParameterValues[baseParameterName] is IEnumerable<TEntity> source))
		{
			return null;
		}
		IClrPropertyGetter getter = property.GetGetter();
		return source.Select((TEntity e) => (TProperty)getter.GetClrValue(e)).ToList();
	}

	protected static Expression UnwrapLastNavigation(Expression expression)
	{
		object obj = (expression as MemberExpression)?.Expression;
		if (obj == null)
		{
			if (!(expression is MethodCallExpression methodCallExpression) || !methodCallExpression.Method.IsEFPropertyMethod())
			{
				return null;
			}
			obj = methodCallExpression.Arguments[0];
		}
		return (Expression)obj;
	}

	protected static Expression Unwrap(Expression expression)
	{
		if (!(expression is EntityReferenceExpression { Underlying: var underlying }))
		{
			if (expression is LambdaExpression { Body: EntityReferenceExpression body } lambdaExpression)
			{
				return Expression.Lambda(lambdaExpression.Type, body.Underlying, lambdaExpression.TailCall, lambdaExpression.Parameters);
			}
			return expression;
		}
		return underlying;
	}
}
