using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class ParameterExtractingExpressionVisitor : ExpressionVisitor
{
	private class ContextParameterReplacingExpressionVisitor : ExpressionVisitor
	{
		private readonly Type _contextType;

		public ParameterExpression ContextParameterExpression { get; }

		public ContextParameterReplacingExpressionVisitor(Type contextType)
		{
			ContextParameterExpression = Expression.Parameter(contextType, "context");
			_contextType = contextType;
		}

		public override Expression Visit(Expression expression)
		{
			if (!(expression?.Type != typeof(object)) || expression == null || !expression.Type.GetTypeInfo().IsAssignableFrom(_contextType))
			{
				return base.Visit(expression);
			}
			return ContextParameterExpression;
		}
	}

	private class EvaluatableExpressionFindingExpressionVisitor : ExpressionVisitor
	{
		private readonly IEvaluatableExpressionFilter _evaluatableExpressionFilter;

		private readonly ISet<ParameterExpression> _allowedParameters = new HashSet<ParameterExpression>();

		private readonly IModel _model;

		private bool _evaluatable;

		private bool _containsClosure;

		private bool _inLambda;

		private IDictionary<Expression, bool> _evaluatableExpressions;

		public EvaluatableExpressionFindingExpressionVisitor(IEvaluatableExpressionFilter evaluatableExpressionFilter, IModel model)
		{
			_evaluatableExpressionFilter = evaluatableExpressionFilter;
			_model = model;
		}

		public IDictionary<Expression, bool> Find(Expression expression)
		{
			_evaluatable = true;
			_containsClosure = false;
			_inLambda = false;
			_evaluatableExpressions = new Dictionary<Expression, bool>();
			_allowedParameters.Clear();
			Visit(expression);
			return _evaluatableExpressions;
		}

		public override Expression Visit(Expression expression)
		{
			if (expression == null)
			{
				return base.Visit(expression);
			}
			bool evaluatable = _evaluatable;
			bool containsClosure = _containsClosure;
			_evaluatable = IsEvaluatableNodeType(expression) && _evaluatableExpressionFilter.IsEvaluatableExpression(expression, _model);
			_containsClosure = false;
			base.Visit(expression);
			if (_evaluatable)
			{
				_evaluatableExpressions[expression] = _containsClosure;
			}
			_evaluatable = evaluatable && _evaluatable;
			_containsClosure = containsClosure || _containsClosure;
			return expression;
		}

		protected override Expression VisitLambda<T>(Expression<T> lambdaExpression)
		{
			bool inLambda = _inLambda;
			_inLambda = true;
			base.VisitLambda(lambdaExpression);
			_inLambda = inLambda;
			return lambdaExpression;
		}

		protected override Expression VisitMemberInit(MemberInitExpression memberInitExpression)
		{
			ExpressionVisitor.Visit(memberInitExpression.Bindings, VisitMemberBinding);
			if (_evaluatable)
			{
				Visit(memberInitExpression.NewExpression);
			}
			return memberInitExpression;
		}

		protected override Expression VisitListInit(ListInitExpression listInitExpression)
		{
			ExpressionVisitor.Visit(listInitExpression.Initializers, VisitElementInit);
			if (_evaluatable)
			{
				Visit(listInitExpression.NewExpression);
			}
			return listInitExpression;
		}

		protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
		{
			Visit(methodCallExpression.Object);
			ParameterInfo[] parameters = methodCallExpression.Method.GetParameters();
			for (int i = 0; i < methodCallExpression.Arguments.Count; i++)
			{
				if (i == 1 && _evaluatableExpressions.ContainsKey(methodCallExpression.Arguments[0]) && methodCallExpression.Method.DeclaringType == typeof(Enumerable) && methodCallExpression.Method.Name == "Select" && methodCallExpression.Arguments[1] is LambdaExpression lambdaExpression)
				{
					foreach (ParameterExpression parameter in lambdaExpression.Parameters)
					{
						_allowedParameters.Add(parameter);
					}
				}
				Visit(methodCallExpression.Arguments[i]);
				if (_evaluatableExpressions.ContainsKey(methodCallExpression.Arguments[i]))
				{
					if (parameters[i].GetCustomAttribute<NotParameterizedAttribute>() != null || methodCallExpression.Method.IsEFIndexer())
					{
						_evaluatableExpressions[methodCallExpression.Arguments[i]] = false;
					}
					else if (!_inLambda)
					{
						_evaluatableExpressions[methodCallExpression.Arguments[i]] = true;
					}
				}
			}
			return methodCallExpression;
		}

		protected override Expression VisitMember(MemberExpression memberExpression)
		{
			_containsClosure = memberExpression.Expression != null || !(memberExpression.Member is FieldInfo fieldInfo) || !fieldInfo.IsInitOnly;
			return base.VisitMember(memberExpression);
		}

		protected override Expression VisitParameter(ParameterExpression parameterExpression)
		{
			_evaluatable = _allowedParameters.Contains(parameterExpression);
			return base.VisitParameter(parameterExpression);
		}

		protected override Expression VisitConstant(ConstantExpression constantExpression)
		{
			_evaluatable = !(constantExpression.Value is IDetachableContext) && !(constantExpression.Value is IQueryable);
			_containsClosure = (constantExpression.Type.Attributes.HasFlag(TypeAttributes.NestedPrivate) && Attribute.IsDefined(constantExpression.Type, typeof(CompilerGeneratedAttribute), inherit: true)) || constantExpression.Type == typeof(ValueBuffer);
			return base.VisitConstant(constantExpression);
		}

		private static bool IsEvaluatableNodeType(Expression expression)
		{
			if (expression.NodeType == ExpressionType.Extension)
			{
				if (!expression.CanReduce)
				{
					return false;
				}
				return IsEvaluatableNodeType(expression.ReduceAndCheck());
			}
			return true;
		}
	}

	private const string QueryFilterPrefix = "ef_filter";

	private readonly IParameterValues _parameterValues;

	private readonly IDiagnosticsLogger<DbLoggerCategory.Query> _logger;

	private readonly bool _parameterize;

	private readonly bool _generateContextAccessors;

	private readonly EvaluatableExpressionFindingExpressionVisitor _evaluatableExpressionFindingExpressionVisitor;

	private readonly ContextParameterReplacingExpressionVisitor _contextParameterReplacingExpressionVisitor;

	private readonly Dictionary<Expression, Expression> _evaluatedValues = new Dictionary<Expression, Expression>(ExpressionEqualityComparer.Instance);

	private IDictionary<Expression, bool> _evaluatableExpressions;

	private IQueryProvider _currentQueryProvider;

	public ParameterExtractingExpressionVisitor(IEvaluatableExpressionFilter evaluatableExpressionFilter, IParameterValues parameterValues, Type contextType, IModel model, IDiagnosticsLogger<DbLoggerCategory.Query> logger, bool parameterize, bool generateContextAccessors)
	{
		_evaluatableExpressionFindingExpressionVisitor = new EvaluatableExpressionFindingExpressionVisitor(evaluatableExpressionFilter, model);
		_parameterValues = parameterValues;
		_logger = logger;
		_parameterize = parameterize;
		_generateContextAccessors = generateContextAccessors;
		if (_generateContextAccessors)
		{
			_contextParameterReplacingExpressionVisitor = new ContextParameterReplacingExpressionVisitor(contextType);
		}
	}

	public virtual Expression ExtractParameters(Expression expression)
	{
		IDictionary<Expression, bool> evaluatableExpressions = _evaluatableExpressions;
		_evaluatableExpressions = _evaluatableExpressionFindingExpressionVisitor.Find(expression);
		try
		{
			return Visit(expression);
		}
		finally
		{
			_evaluatableExpressions = evaluatableExpressions;
			_evaluatedValues.Clear();
		}
	}

	public override Expression Visit(Expression expression)
	{
		if (expression == null)
		{
			return null;
		}
		if (_evaluatableExpressions.TryGetValue(expression, out var value) && !PreserveInitializationConstant(expression, value) && !PreserveConvertNode(expression))
		{
			return Evaluate(expression, _parameterize && value);
		}
		return base.Visit(expression);
	}

	private bool PreserveInitializationConstant(Expression expression, bool generateParameter)
	{
		if (!generateParameter)
		{
			if (!(expression is NewExpression))
			{
				return expression is MemberInitExpression;
			}
			return true;
		}
		return false;
	}

	private bool PreserveConvertNode(Expression expression)
	{
		if (expression is UnaryExpression unaryExpression && (unaryExpression.NodeType == ExpressionType.Convert || unaryExpression.NodeType == ExpressionType.ConvertChecked))
		{
			if (unaryExpression.Type == typeof(object) || unaryExpression.Type == typeof(Enum) || unaryExpression.Operand.Type.UnwrapNullableType().IsEnum)
			{
				return true;
			}
			Type type = unaryExpression.Operand.Type.UnwrapNullableType();
			if (unaryExpression.Type.UnwrapNullableType() == typeof(int) && (type == typeof(byte) || type == typeof(sbyte) || type == typeof(char) || type == typeof(short) || type == typeof(ushort)))
			{
				return true;
			}
			return PreserveConvertNode(unaryExpression.Operand);
		}
		return false;
	}

	protected override Expression VisitConditional(ConditionalExpression conditionalExpression)
	{
		Expression expression = TryGetConstantValue(conditionalExpression.Test) ?? Visit(conditionalExpression.Test);
		if (expression is ConstantExpression constantExpression)
		{
			object value = constantExpression.Value;
			if (value is bool)
			{
				if (!(bool)value)
				{
					return Visit(conditionalExpression.IfFalse);
				}
				return Visit(conditionalExpression.IfTrue);
			}
		}
		return conditionalExpression.Update(expression, Visit(conditionalExpression.IfTrue), Visit(conditionalExpression.IfFalse));
	}

	protected override Expression VisitBinary(BinaryExpression binaryExpression)
	{
		switch (binaryExpression.NodeType)
		{
		case ExpressionType.Coalesce:
		{
			Expression expression3 = TryGetConstantValue(binaryExpression.Left) ?? Visit(binaryExpression.Left);
			if (expression3 is ConstantExpression constantExpression)
			{
				if (constantExpression.Value != null)
				{
					return expression3;
				}
				return Visit(binaryExpression.Right);
			}
			return binaryExpression.Update(expression3, binaryExpression.Conversion, Visit(binaryExpression.Right));
		}
		case ExpressionType.AndAlso:
		case ExpressionType.OrElse:
		{
			Expression expression = TryGetConstantValue(binaryExpression.Left) ?? Visit(binaryExpression.Left);
			if (ShortCircuitLogicalExpression(expression, binaryExpression.NodeType))
			{
				return expression;
			}
			Expression expression2 = TryGetConstantValue(binaryExpression.Right) ?? Visit(binaryExpression.Right);
			if (ShortCircuitLogicalExpression(expression2, binaryExpression.NodeType))
			{
				return expression2;
			}
			return binaryExpression.Update(expression, binaryExpression.Conversion, expression2);
		}
		default:
			return base.VisitBinary(binaryExpression);
		}
	}

	private Expression TryGetConstantValue(Expression expression)
	{
		if (_evaluatableExpressions.ContainsKey(expression))
		{
			string parameterName;
			object value = GetValue(expression, out parameterName);
			if (value is bool)
			{
				return Expression.Constant(value, typeof(bool));
			}
		}
		return null;
	}

	private static bool ShortCircuitLogicalExpression(Expression expression, ExpressionType nodeType)
	{
		if (expression is ConstantExpression constantExpression && constantExpression.Value is bool flag)
		{
			if (!flag || nodeType != ExpressionType.OrElse)
			{
				if (!flag)
				{
					return nodeType == ExpressionType.AndAlso;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	protected override Expression VisitConstant(ConstantExpression constantExpression)
	{
		if (constantExpression.Value is IDetachableContext detachableContext)
		{
			IQueryProvider provider = ((IQueryable)constantExpression.Value).Provider;
			if (_currentQueryProvider == null)
			{
				_currentQueryProvider = provider;
			}
			else if (provider != _currentQueryProvider && provider.GetType() == _currentQueryProvider.GetType())
			{
				throw new InvalidOperationException(CoreStrings.ErrorInvalidQueryable);
			}
			return Expression.Constant(detachableContext.DetachContext());
		}
		return base.VisitConstant(constantExpression);
	}

	private static Expression GenerateConstantExpression(object value, Type returnType)
	{
		ConstantExpression constantExpression = Expression.Constant(value, value?.GetType() ?? returnType);
		if (!(constantExpression.Type != returnType))
		{
			return constantExpression;
		}
		return Expression.Convert(constantExpression, returnType);
	}

	private Expression Evaluate(Expression expression, bool generateParameter)
	{
		if (_evaluatedValues.TryGetValue(expression, out var value))
		{
			return value;
		}
		string parameterName;
		object value2 = GetValue(expression, out parameterName);
		if (value2 is IQueryable queryable)
		{
			return ExtractParameters(queryable.Expression);
		}
		if (parameterName == null || !parameterName.StartsWith("ef_filter", StringComparison.Ordinal))
		{
			if (value2 is Expression expression2)
			{
				return ExtractParameters(expression2);
			}
			if (!generateParameter)
			{
				Expression expression3 = GenerateConstantExpression(value2, expression.Type);
				_evaluatedValues.Add(expression, expression3);
				return expression3;
			}
		}
		if (parameterName == null)
		{
			parameterName = "p";
		}
		if (string.Equals("ef_filter", parameterName, StringComparison.Ordinal))
		{
			parameterName = "ef_filter__p";
		}
		int num = parameterName.LastIndexOf(">", StringComparison.Ordinal);
		if (num != -1)
		{
			parameterName = parameterName.Substring(num + 1);
		}
		parameterName = "__" + parameterName + "_" + _parameterValues.ParameterValues.Count;
		_parameterValues.AddParameter(parameterName, value2);
		ParameterExpression parameterExpression = Expression.Parameter(expression.Type, parameterName);
		_evaluatedValues.Add(expression, parameterExpression);
		return parameterExpression;
	}

	private static Expression RemoveConvert(Expression expression)
	{
		if (expression is UnaryExpression unaryExpression && (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked))
		{
			return RemoveConvert(unaryExpression.Operand);
		}
		return expression;
	}

	private object GetValue(Expression expression, out string parameterName)
	{
		parameterName = null;
		if (expression == null)
		{
			return null;
		}
		if (_generateContextAccessors)
		{
			Expression expression2 = _contextParameterReplacingExpressionVisitor.Visit(expression);
			if (expression2 != expression)
			{
				if (expression2.Type is IQueryable)
				{
					return expression2;
				}
				parameterName = "ef_filter" + ((RemoveConvert(expression) is MemberExpression memberExpression) ? ("__" + memberExpression.Member.Name) : "");
				return Expression.Lambda(expression2, _contextParameterReplacingExpressionVisitor.ContextParameterExpression);
			}
		}
		if (!(expression is MemberExpression memberExpression2))
		{
			if (expression is ConstantExpression constantExpression)
			{
				return constantExpression.Value;
			}
			if (!(expression is MethodCallExpression methodCallExpression))
			{
				if (expression is UnaryExpression unaryExpression && (unaryExpression.NodeType == ExpressionType.Convert || unaryExpression.NodeType == ExpressionType.ConvertChecked) && unaryExpression.Type.UnwrapNullableType() == unaryExpression.Operand.Type)
				{
					return GetValue(unaryExpression.Operand, out parameterName);
				}
			}
			else
			{
				parameterName = methodCallExpression.Method.Name;
			}
		}
		else
		{
			object value = GetValue(memberExpression2.Expression, out parameterName);
			try
			{
				MemberInfo member = memberExpression2.Member;
				if (member is FieldInfo fieldInfo)
				{
					parameterName = ((parameterName != null) ? (parameterName + "_") : "") + fieldInfo.Name;
					return fieldInfo.GetValue(value);
				}
				if (member is PropertyInfo propertyInfo)
				{
					parameterName = ((parameterName != null) ? (parameterName + "_") : "") + propertyInfo.Name;
					return propertyInfo.GetValue(value);
				}
			}
			catch
			{
			}
		}
		try
		{
			return Expression.Lambda<Func<object>>(Expression.Convert(expression, typeof(object)), Array.Empty<ParameterExpression>()).Compile()();
		}
		catch (Exception innerException)
		{
			throw new InvalidOperationException(_logger.ShouldLogSensitiveData() ? CoreStrings.ExpressionParameterizationExceptionSensitive(expression) : CoreStrings.ExpressionParameterizationException, innerException);
		}
	}
}
