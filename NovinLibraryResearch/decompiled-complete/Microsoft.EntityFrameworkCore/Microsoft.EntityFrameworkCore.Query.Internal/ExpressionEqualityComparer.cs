using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Internal;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class ExpressionEqualityComparer : IEqualityComparer<Expression>
{
	private struct ExpressionComparer
	{
		private class ScopedDictionary<TKey, TValue>
		{
			private readonly ScopedDictionary<TKey, TValue> _previous;

			private readonly Dictionary<TKey, TValue> _map;

			public ScopedDictionary(ScopedDictionary<TKey, TValue> previous)
			{
				_previous = previous;
				_map = new Dictionary<TKey, TValue>();
			}

			public void Add(TKey key, TValue value)
			{
				_map.Add(key, value);
			}

			public bool TryGetValue(TKey key, out TValue value)
			{
				for (ScopedDictionary<TKey, TValue> scopedDictionary = this; scopedDictionary != null; scopedDictionary = scopedDictionary._previous)
				{
					if (scopedDictionary._map.TryGetValue(key, out value))
					{
						return true;
					}
				}
				value = default(TValue);
				return false;
			}
		}

		private ScopedDictionary<ParameterExpression, ParameterExpression> _parameterScope;

		public bool Compare(Expression a, Expression b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			if (a.NodeType != b.NodeType)
			{
				return false;
			}
			if (a.Type != b.Type)
			{
				return false;
			}
			switch (a.NodeType)
			{
			case ExpressionType.ArrayLength:
			case ExpressionType.Convert:
			case ExpressionType.ConvertChecked:
			case ExpressionType.Negate:
			case ExpressionType.UnaryPlus:
			case ExpressionType.NegateChecked:
			case ExpressionType.Not:
			case ExpressionType.Quote:
			case ExpressionType.TypeAs:
				return CompareUnary((UnaryExpression)a, (UnaryExpression)b);
			case ExpressionType.Add:
			case ExpressionType.AddChecked:
			case ExpressionType.And:
			case ExpressionType.AndAlso:
			case ExpressionType.ArrayIndex:
			case ExpressionType.Coalesce:
			case ExpressionType.Divide:
			case ExpressionType.Equal:
			case ExpressionType.ExclusiveOr:
			case ExpressionType.GreaterThan:
			case ExpressionType.GreaterThanOrEqual:
			case ExpressionType.LeftShift:
			case ExpressionType.LessThan:
			case ExpressionType.LessThanOrEqual:
			case ExpressionType.Modulo:
			case ExpressionType.Multiply:
			case ExpressionType.MultiplyChecked:
			case ExpressionType.NotEqual:
			case ExpressionType.Or:
			case ExpressionType.OrElse:
			case ExpressionType.Power:
			case ExpressionType.RightShift:
			case ExpressionType.Subtract:
			case ExpressionType.SubtractChecked:
				return CompareBinary((BinaryExpression)a, (BinaryExpression)b);
			case ExpressionType.TypeIs:
				return CompareTypeIs((TypeBinaryExpression)a, (TypeBinaryExpression)b);
			case ExpressionType.Conditional:
				return CompareConditional((ConditionalExpression)a, (ConditionalExpression)b);
			case ExpressionType.Constant:
				return CompareConstant((ConstantExpression)a, (ConstantExpression)b);
			case ExpressionType.Parameter:
				return CompareParameter((ParameterExpression)a, (ParameterExpression)b);
			case ExpressionType.MemberAccess:
				return CompareMemberAccess((MemberExpression)a, (MemberExpression)b);
			case ExpressionType.Call:
				return CompareMethodCall((MethodCallExpression)a, (MethodCallExpression)b);
			case ExpressionType.Lambda:
				return CompareLambda((LambdaExpression)a, (LambdaExpression)b);
			case ExpressionType.New:
				return CompareNew((NewExpression)a, (NewExpression)b);
			case ExpressionType.NewArrayInit:
			case ExpressionType.NewArrayBounds:
				return CompareNewArray((NewArrayExpression)a, (NewArrayExpression)b);
			case ExpressionType.Invoke:
				return CompareInvocation((InvocationExpression)a, (InvocationExpression)b);
			case ExpressionType.MemberInit:
				return CompareMemberInit((MemberInitExpression)a, (MemberInitExpression)b);
			case ExpressionType.ListInit:
				return CompareListInit((ListInitExpression)a, (ListInitExpression)b);
			case ExpressionType.Extension:
				return CompareExtension(a, b);
			case ExpressionType.Default:
				return CompareDefault((DefaultExpression)a, (DefaultExpression)b);
			case ExpressionType.Index:
				return CompareIndex((IndexExpression)a, (IndexExpression)b);
			default:
				throw new NotImplementedException($"Unhandled expression node type: {a.NodeType}");
			}
		}

		private bool CompareDefault(DefaultExpression a, DefaultExpression b)
		{
			return a.Type == b.Type;
		}

		private bool CompareUnary(UnaryExpression a, UnaryExpression b)
		{
			if (object.Equals(a.Method, b.Method) && a.IsLifted == b.IsLifted && a.IsLiftedToNull == b.IsLiftedToNull)
			{
				return Compare(a.Operand, b.Operand);
			}
			return false;
		}

		private bool CompareBinary(BinaryExpression a, BinaryExpression b)
		{
			if (object.Equals(a.Method, b.Method) && a.IsLifted == b.IsLifted && a.IsLiftedToNull == b.IsLiftedToNull && Compare(a.Left, b.Left))
			{
				return Compare(a.Right, b.Right);
			}
			return false;
		}

		private bool CompareTypeIs(TypeBinaryExpression a, TypeBinaryExpression b)
		{
			if (a.TypeOperand == b.TypeOperand)
			{
				return Compare(a.Expression, b.Expression);
			}
			return false;
		}

		private bool CompareConditional(ConditionalExpression a, ConditionalExpression b)
		{
			if (Compare(a.Test, b.Test) && Compare(a.IfTrue, b.IfTrue))
			{
				return Compare(a.IfFalse, b.IfFalse);
			}
			return false;
		}

		private static bool CompareConstant(ConstantExpression a, ConstantExpression b)
		{
			if (a.Value == b.Value)
			{
				return true;
			}
			if (a.Value == null || b.Value == null)
			{
				return false;
			}
			if (!a.IsEntityQueryable() || !b.IsEntityQueryable() || !(a.Value.GetType() == b.Value.GetType()))
			{
				return object.Equals(a.Value, b.Value);
			}
			return true;
		}

		private bool CompareParameter(ParameterExpression a, ParameterExpression b)
		{
			if (_parameterScope != null && _parameterScope.TryGetValue(a, out var value))
			{
				if (value.Name == b.Name)
				{
					return value.Type == b.Type;
				}
				return false;
			}
			if (a.Name == b.Name)
			{
				return a.Type == b.Type;
			}
			return false;
		}

		private bool CompareMemberAccess(MemberExpression a, MemberExpression b)
		{
			if (object.Equals(a.Member, b.Member))
			{
				return Compare(a.Expression, b.Expression);
			}
			return false;
		}

		private bool CompareMethodCall(MethodCallExpression a, MethodCallExpression b)
		{
			if (object.Equals(a.Method, b.Method) && Compare(a.Object, b.Object))
			{
				return CompareExpressionList(a.Arguments, b.Arguments);
			}
			return false;
		}

		private bool CompareLambda(LambdaExpression a, LambdaExpression b)
		{
			int count = a.Parameters.Count;
			if (b.Parameters.Count != count)
			{
				return false;
			}
			for (int i = 0; i < count; i++)
			{
				if (a.Parameters[i].Type != b.Parameters[i].Type)
				{
					return false;
				}
			}
			ScopedDictionary<ParameterExpression, ParameterExpression> parameterScope = _parameterScope;
			_parameterScope = new ScopedDictionary<ParameterExpression, ParameterExpression>(_parameterScope);
			try
			{
				for (int j = 0; j < count; j++)
				{
					_parameterScope.Add(a.Parameters[j], b.Parameters[j]);
				}
				return Compare(a.Body, b.Body);
			}
			finally
			{
				_parameterScope = parameterScope;
			}
		}

		private bool CompareNew(NewExpression a, NewExpression b)
		{
			if (object.Equals(a.Constructor, b.Constructor) && CompareExpressionList(a.Arguments, b.Arguments))
			{
				return CompareMemberList(a.Members, b.Members);
			}
			return false;
		}

		private bool CompareExpressionList(IReadOnlyList<Expression> a, IReadOnlyList<Expression> b)
		{
			if (object.Equals(a, b))
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			if (a.Count != b.Count)
			{
				return false;
			}
			int i = 0;
			for (int count = a.Count; i < count; i++)
			{
				if (!Compare(a[i], b[i]))
				{
					return false;
				}
			}
			return true;
		}

		private static bool CompareMemberList(IReadOnlyList<MemberInfo> a, IReadOnlyList<MemberInfo> b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			if (a.Count != b.Count)
			{
				return false;
			}
			int i = 0;
			for (int count = a.Count; i < count; i++)
			{
				if (!object.Equals(a[i], b[i]))
				{
					return false;
				}
			}
			return true;
		}

		private bool CompareNewArray(NewArrayExpression a, NewArrayExpression b)
		{
			return CompareExpressionList(a.Expressions, b.Expressions);
		}

		private bool CompareExtension(Expression a, Expression b)
		{
			return a.Equals(b);
		}

		private bool CompareInvocation(InvocationExpression a, InvocationExpression b)
		{
			if (Compare(a.Expression, b.Expression))
			{
				return CompareExpressionList(a.Arguments, b.Arguments);
			}
			return false;
		}

		private bool CompareMemberInit(MemberInitExpression a, MemberInitExpression b)
		{
			if (Compare(a.NewExpression, b.NewExpression))
			{
				return CompareBindingList(a.Bindings, b.Bindings);
			}
			return false;
		}

		private bool CompareBindingList(IReadOnlyList<MemberBinding> a, IReadOnlyList<MemberBinding> b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			if (a.Count != b.Count)
			{
				return false;
			}
			int i = 0;
			for (int count = a.Count; i < count; i++)
			{
				if (!CompareBinding(a[i], b[i]))
				{
					return false;
				}
			}
			return true;
		}

		private bool CompareBinding(MemberBinding a, MemberBinding b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			if (a.BindingType != b.BindingType)
			{
				return false;
			}
			if (!object.Equals(a.Member, b.Member))
			{
				return false;
			}
			return a.BindingType switch
			{
				MemberBindingType.Assignment => CompareMemberAssignment((MemberAssignment)a, (MemberAssignment)b), 
				MemberBindingType.ListBinding => CompareMemberListBinding((MemberListBinding)a, (MemberListBinding)b), 
				MemberBindingType.MemberBinding => CompareMemberMemberBinding((MemberMemberBinding)a, (MemberMemberBinding)b), 
				_ => throw new InvalidOperationException("Unhandled member binding type: " + a.BindingType), 
			};
		}

		private bool CompareMemberAssignment(MemberAssignment a, MemberAssignment b)
		{
			if (object.Equals(a.Member, b.Member))
			{
				return Compare(a.Expression, b.Expression);
			}
			return false;
		}

		private bool CompareMemberListBinding(MemberListBinding a, MemberListBinding b)
		{
			if (object.Equals(a.Member, b.Member))
			{
				return CompareElementInitList(a.Initializers, b.Initializers);
			}
			return false;
		}

		private bool CompareMemberMemberBinding(MemberMemberBinding a, MemberMemberBinding b)
		{
			if (object.Equals(a.Member, b.Member))
			{
				return CompareBindingList(a.Bindings, b.Bindings);
			}
			return false;
		}

		private bool CompareListInit(ListInitExpression a, ListInitExpression b)
		{
			if (Compare(a.NewExpression, b.NewExpression))
			{
				return CompareElementInitList(a.Initializers, b.Initializers);
			}
			return false;
		}

		private bool CompareElementInitList(IReadOnlyList<ElementInit> a, IReadOnlyList<ElementInit> b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			if (a.Count != b.Count)
			{
				return false;
			}
			int i = 0;
			for (int count = a.Count; i < count; i++)
			{
				if (!CompareElementInit(a[i], b[i]))
				{
					return false;
				}
			}
			return true;
		}

		private bool CompareElementInit(ElementInit a, ElementInit b)
		{
			if (object.Equals(a.AddMethod, b.AddMethod))
			{
				return CompareExpressionList(a.Arguments, b.Arguments);
			}
			return false;
		}

		private bool CompareIndex(IndexExpression a, IndexExpression b)
		{
			if (object.Equals(a.Indexer, b.Indexer) && Compare(a.Object, b.Object))
			{
				return CompareExpressionList(a.Arguments, b.Arguments);
			}
			return false;
		}
	}

	public static ExpressionEqualityComparer Instance { get; } = new ExpressionEqualityComparer();


	private ExpressionEqualityComparer()
	{
	}

	public virtual int GetHashCode(Expression obj)
	{
		if (obj == null)
		{
			return 0;
		}
		HashCode hash = default(HashCode);
		hash.Add(obj.NodeType);
		hash.Add(obj.Type);
		switch (obj.NodeType)
		{
		case ExpressionType.ArrayLength:
		case ExpressionType.Convert:
		case ExpressionType.ConvertChecked:
		case ExpressionType.Negate:
		case ExpressionType.UnaryPlus:
		case ExpressionType.NegateChecked:
		case ExpressionType.Not:
		case ExpressionType.Quote:
		case ExpressionType.TypeAs:
		{
			UnaryExpression unaryExpression = (UnaryExpression)obj;
			if (unaryExpression.Method != null)
			{
				hash.Add(unaryExpression.Method);
			}
			hash.Add(unaryExpression.Operand, this);
			break;
		}
		case ExpressionType.Add:
		case ExpressionType.AddChecked:
		case ExpressionType.And:
		case ExpressionType.AndAlso:
		case ExpressionType.ArrayIndex:
		case ExpressionType.Coalesce:
		case ExpressionType.Divide:
		case ExpressionType.Equal:
		case ExpressionType.ExclusiveOr:
		case ExpressionType.GreaterThan:
		case ExpressionType.GreaterThanOrEqual:
		case ExpressionType.LeftShift:
		case ExpressionType.LessThan:
		case ExpressionType.LessThanOrEqual:
		case ExpressionType.Modulo:
		case ExpressionType.Multiply:
		case ExpressionType.MultiplyChecked:
		case ExpressionType.NotEqual:
		case ExpressionType.Or:
		case ExpressionType.OrElse:
		case ExpressionType.Power:
		case ExpressionType.RightShift:
		case ExpressionType.Subtract:
		case ExpressionType.SubtractChecked:
		{
			BinaryExpression binaryExpression = (BinaryExpression)obj;
			hash.Add(binaryExpression.Left, this);
			hash.Add(binaryExpression.Right, this);
			break;
		}
		case ExpressionType.TypeIs:
		{
			TypeBinaryExpression typeBinaryExpression = (TypeBinaryExpression)obj;
			hash.Add(typeBinaryExpression.Expression, this);
			hash.Add(typeBinaryExpression.TypeOperand);
			break;
		}
		case ExpressionType.Constant:
		{
			ConstantExpression constantExpression = (ConstantExpression)obj;
			if (constantExpression.Value != null && !(constantExpression.Value is IQueryable))
			{
				hash.Add(constantExpression.Value);
			}
			break;
		}
		case ExpressionType.Parameter:
		{
			ParameterExpression parameterExpression = (ParameterExpression)obj;
			if (parameterExpression.Name != null)
			{
				hash.Add(parameterExpression.Name);
			}
			break;
		}
		case ExpressionType.MemberAccess:
		{
			MemberExpression memberExpression = (MemberExpression)obj;
			hash.Add(memberExpression.Member);
			hash.Add<Expression>(memberExpression.Expression, this);
			break;
		}
		case ExpressionType.Call:
		{
			MethodCallExpression methodCallExpression = (MethodCallExpression)obj;
			hash.Add(methodCallExpression.Method);
			hash.Add<Expression>(methodCallExpression.Object, this);
			AddListToHash(ref hash, methodCallExpression.Arguments);
			break;
		}
		case ExpressionType.Lambda:
		{
			LambdaExpression lambdaExpression = (LambdaExpression)obj;
			hash.Add(lambdaExpression.ReturnType);
			hash.Add(lambdaExpression.Body, this);
			AddListToHash(ref hash, lambdaExpression.Parameters);
			break;
		}
		case ExpressionType.New:
		{
			NewExpression newExpression = (NewExpression)obj;
			hash.Add(newExpression.Constructor);
			if (newExpression.Members != null)
			{
				for (int l = 0; l < newExpression.Members.Count; l++)
				{
					hash.Add(newExpression.Members[l]);
				}
			}
			AddListToHash(ref hash, newExpression.Arguments);
			break;
		}
		case ExpressionType.NewArrayInit:
		case ExpressionType.NewArrayBounds:
		{
			NewArrayExpression newArrayExpression = (NewArrayExpression)obj;
			AddListToHash(ref hash, newArrayExpression.Expressions);
			break;
		}
		case ExpressionType.Invoke:
		{
			InvocationExpression invocationExpression = (InvocationExpression)obj;
			hash.Add(invocationExpression.Expression, this);
			AddListToHash(ref hash, invocationExpression.Arguments);
			break;
		}
		case ExpressionType.MemberInit:
		{
			MemberInitExpression memberInitExpression = (MemberInitExpression)obj;
			hash.Add(memberInitExpression.NewExpression, this);
			for (int j = 0; j < memberInitExpression.Bindings.Count; j++)
			{
				MemberBinding memberBinding = memberInitExpression.Bindings[j];
				hash.Add(memberBinding.Member);
				hash.Add(memberBinding.BindingType);
				switch (memberBinding.BindingType)
				{
				case MemberBindingType.Assignment:
				{
					MemberAssignment memberAssignment = (MemberAssignment)memberBinding;
					hash.Add(memberAssignment.Expression, this);
					break;
				}
				case MemberBindingType.ListBinding:
				{
					MemberListBinding memberListBinding = (MemberListBinding)memberBinding;
					for (int k = 0; k < memberListBinding.Initializers.Count; k++)
					{
						AddListToHash(ref hash, memberListBinding.Initializers[k].Arguments);
					}
					break;
				}
				default:
					throw new NotImplementedException($"Unhandled binding type: {memberBinding}");
				}
			}
			break;
		}
		case ExpressionType.ListInit:
		{
			ListInitExpression listInitExpression = (ListInitExpression)obj;
			hash.Add(listInitExpression.NewExpression, this);
			for (int i = 0; i < listInitExpression.Initializers.Count; i++)
			{
				AddListToHash(ref hash, listInitExpression.Initializers[i].Arguments);
			}
			break;
		}
		case ExpressionType.Conditional:
		{
			ConditionalExpression conditionalExpression = (ConditionalExpression)obj;
			hash.Add(conditionalExpression.Test, this);
			hash.Add(conditionalExpression.IfTrue, this);
			hash.Add(conditionalExpression.IfFalse, this);
			break;
		}
		case ExpressionType.Default:
			hash.Add(obj.Type);
			break;
		case ExpressionType.Extension:
			hash.Add(obj);
			break;
		case ExpressionType.Index:
		{
			IndexExpression indexExpression = (IndexExpression)obj;
			hash.Add(indexExpression.Indexer);
			hash.Add<Expression>(indexExpression.Object, this);
			AddListToHash(ref hash, indexExpression.Arguments);
			break;
		}
		default:
			throw new NotImplementedException($"Unhandled expression node type: {obj.NodeType}");
		}
		return hash.ToHashCode();
	}

	private void AddListToHash<T>(ref HashCode hash, IReadOnlyList<T> expressions) where T : Expression
	{
		for (int i = 0; i < expressions.Count; i++)
		{
			hash.Add(expressions[i], this);
		}
	}

	public virtual bool Equals(Expression x, Expression y)
	{
		return default(ExpressionComparer).Compare(x, y);
	}

	[Obsolete("Use Equals instead")]
	public virtual bool SequenceEquals(IEnumerable<Expression> x, IEnumerable<Expression> y)
	{
		if (x == null || y == null)
		{
			return false;
		}
		if (x.Count() != y.Count())
		{
			return false;
		}
		return x.Zip(y, (Expression l, Expression r) => default(ExpressionComparer).Compare(l, r)).All((bool r) => r);
	}
}
