using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace Microsoft.EntityFrameworkCore.Query;

public class ExpressionPrinter : ExpressionVisitor
{
	private readonly IndentedStringBuilder _stringBuilder;

	private readonly Dictionary<ParameterExpression, string> _parametersInScope;

	private readonly List<ParameterExpression> _namelessParameters;

	private readonly List<ParameterExpression> _encounteredParameters;

	private readonly Dictionary<ExpressionType, string> _binaryOperandMap = new Dictionary<ExpressionType, string>
	{
		{
			ExpressionType.Assign,
			" = "
		},
		{
			ExpressionType.Equal,
			" == "
		},
		{
			ExpressionType.NotEqual,
			" != "
		},
		{
			ExpressionType.GreaterThan,
			" > "
		},
		{
			ExpressionType.GreaterThanOrEqual,
			" >= "
		},
		{
			ExpressionType.LessThan,
			" < "
		},
		{
			ExpressionType.LessThanOrEqual,
			" <= "
		},
		{
			ExpressionType.OrElse,
			" || "
		},
		{
			ExpressionType.AndAlso,
			" && "
		},
		{
			ExpressionType.Coalesce,
			" ?? "
		},
		{
			ExpressionType.Add,
			" + "
		},
		{
			ExpressionType.Subtract,
			" - "
		},
		{
			ExpressionType.Multiply,
			" * "
		},
		{
			ExpressionType.Divide,
			" / "
		},
		{
			ExpressionType.Modulo,
			" % "
		},
		{
			ExpressionType.And,
			" & "
		},
		{
			ExpressionType.Or,
			" | "
		},
		{
			ExpressionType.ExclusiveOr,
			" ^ "
		}
	};

	private static readonly List<string> _simpleMethods = new List<string> { "get_Item", "TryReadValue", "ReferenceEquals" };

	private int? CharacterLimit { get; set; }

	private bool GenerateUniqueParameterIds { get; set; }

	public ExpressionPrinter()
	{
		_stringBuilder = new IndentedStringBuilder();
		_parametersInScope = new Dictionary<ParameterExpression, string>();
		_namelessParameters = new List<ParameterExpression>();
		_encounteredParameters = new List<ParameterExpression>();
	}

	public virtual void VisitList<T>(IReadOnlyList<T> items, Action<ExpressionPrinter> joinAction = null) where T : Expression
	{
		if (joinAction == null)
		{
			joinAction = delegate(ExpressionPrinter p)
			{
				p.Append(", ");
			};
		}
		for (int i = 0; i < items.Count; i++)
		{
			if (i > 0)
			{
				joinAction(this);
			}
			Visit(items[i]);
		}
	}

	public virtual ExpressionPrinter Append([JetBrains.Annotations.NotNull] object o)
	{
		_stringBuilder.Append(o);
		return this;
	}

	public virtual ExpressionPrinter AppendLine()
	{
		_stringBuilder.AppendLine();
		return this;
	}

	public virtual ExpressionVisitor AppendLine([JetBrains.Annotations.NotNull] object o)
	{
		_stringBuilder.AppendLine(o);
		return this;
	}

	public virtual ExpressionPrinter AppendLines([JetBrains.Annotations.NotNull] object o, bool skipFinalNewline = false)
	{
		_stringBuilder.AppendLines(o, skipFinalNewline);
		return this;
	}

	public virtual IDisposable Indent()
	{
		return _stringBuilder.Indent();
	}

	private void Append([JetBrains.Annotations.NotNull] string message)
	{
		_stringBuilder.Append(message);
	}

	private void AppendLine([JetBrains.Annotations.NotNull] string message)
	{
		_stringBuilder.AppendLine(message);
	}

	public virtual string Print(Expression expression, int? characterLimit = null)
	{
		return PrintCore(expression, characterLimit, generateUniqueParameterIds: false);
	}

	public virtual string PrintDebug(Expression expression, int? characterLimit = null, bool generateUniqueParameterIds = true)
	{
		return PrintCore(expression, characterLimit, generateUniqueParameterIds);
	}

	protected virtual string PrintCore(Expression expression, int? characterLimit, bool generateUniqueParameterIds)
	{
		_stringBuilder.Clear();
		_parametersInScope.Clear();
		_namelessParameters.Clear();
		_encounteredParameters.Clear();
		CharacterLimit = characterLimit;
		GenerateUniqueParameterIds = generateUniqueParameterIds;
		Visit(expression);
		string text = PostProcess(_stringBuilder.ToString());
		if (characterLimit.HasValue && characterLimit.Value > 0)
		{
			text = ((text.Length > characterLimit) ? (text.Substring(0, characterLimit.Value) + "...") : text);
		}
		return text;
	}

	public virtual string GenerateBinaryOperator(ExpressionType expressionType)
	{
		return _binaryOperandMap[expressionType];
	}

	public override Expression Visit(Expression expression)
	{
		if (expression == null)
		{
			return null;
		}
		if (CharacterLimit.HasValue && _stringBuilder.Length > CharacterLimit.Value)
		{
			return expression;
		}
		switch (expression.NodeType)
		{
		case ExpressionType.Add:
		case ExpressionType.And:
		case ExpressionType.AndAlso:
		case ExpressionType.ArrayIndex:
		case ExpressionType.Coalesce:
		case ExpressionType.Divide:
		case ExpressionType.Equal:
		case ExpressionType.ExclusiveOr:
		case ExpressionType.GreaterThan:
		case ExpressionType.GreaterThanOrEqual:
		case ExpressionType.LessThan:
		case ExpressionType.LessThanOrEqual:
		case ExpressionType.Modulo:
		case ExpressionType.Multiply:
		case ExpressionType.NotEqual:
		case ExpressionType.Or:
		case ExpressionType.OrElse:
		case ExpressionType.Subtract:
		case ExpressionType.Assign:
			VisitBinary((BinaryExpression)expression);
			break;
		case ExpressionType.Block:
			VisitBlock((BlockExpression)expression);
			break;
		case ExpressionType.Conditional:
			VisitConditional((ConditionalExpression)expression);
			break;
		case ExpressionType.Constant:
			VisitConstant((ConstantExpression)expression);
			break;
		case ExpressionType.Lambda:
			base.Visit(expression);
			break;
		case ExpressionType.Goto:
			VisitGoto((GotoExpression)expression);
			break;
		case ExpressionType.Label:
			VisitLabel((LabelExpression)expression);
			break;
		case ExpressionType.MemberAccess:
			VisitMember((MemberExpression)expression);
			break;
		case ExpressionType.MemberInit:
			VisitMemberInit((MemberInitExpression)expression);
			break;
		case ExpressionType.Call:
			VisitMethodCall((MethodCallExpression)expression);
			break;
		case ExpressionType.New:
			VisitNew((NewExpression)expression);
			break;
		case ExpressionType.NewArrayInit:
			VisitNewArray((NewArrayExpression)expression);
			break;
		case ExpressionType.Parameter:
			VisitParameter((ParameterExpression)expression);
			break;
		case ExpressionType.Convert:
		case ExpressionType.Not:
		case ExpressionType.Quote:
		case ExpressionType.TypeAs:
		case ExpressionType.Throw:
			VisitUnary((UnaryExpression)expression);
			break;
		case ExpressionType.Default:
			VisitDefault((DefaultExpression)expression);
			break;
		case ExpressionType.Try:
			VisitTry((TryExpression)expression);
			break;
		case ExpressionType.Index:
			VisitIndex((IndexExpression)expression);
			break;
		case ExpressionType.TypeIs:
			VisitTypeBinary((TypeBinaryExpression)expression);
			break;
		case ExpressionType.Extension:
			VisitExtension(expression);
			break;
		default:
			UnhandledExpressionType(expression);
			break;
		}
		return expression;
	}

	protected override Expression VisitBinary(BinaryExpression binaryExpression)
	{
		Visit(binaryExpression.Left);
		if (binaryExpression.NodeType == ExpressionType.ArrayIndex)
		{
			_stringBuilder.Append("[");
			Visit(binaryExpression.Right);
			_stringBuilder.Append("]");
		}
		else
		{
			if (!_binaryOperandMap.TryGetValue(binaryExpression.NodeType, out var value))
			{
				UnhandledExpressionType(binaryExpression);
			}
			else
			{
				_stringBuilder.Append(value);
			}
			Visit(binaryExpression.Right);
		}
		return binaryExpression;
	}

	protected override Expression VisitBlock(BlockExpression blockExpression)
	{
		AppendLine();
		AppendLine("{");
		using (_stringBuilder.Indent())
		{
			foreach (ParameterExpression variable in blockExpression.Variables)
			{
				if (!_parametersInScope.ContainsKey(variable))
				{
					_parametersInScope.Add(variable, variable.Name);
					Append(variable.Type.ShortDisplayName());
					Append(" ");
					VisitParameter(variable);
					AppendLine(";");
				}
			}
			IEnumerable<Expression> enumerable;
			if (blockExpression.Result == null)
			{
				IEnumerable<Expression> expressions = blockExpression.Expressions;
				enumerable = expressions;
			}
			else
			{
				enumerable = blockExpression.Expressions.Except(new Expression[1] { blockExpression.Result });
			}
			foreach (Expression item in enumerable)
			{
				Visit(item);
				AppendLine(";");
			}
			if (blockExpression.Result != null)
			{
				Append("return ");
				Visit(blockExpression.Result);
				AppendLine(";");
			}
		}
		Append("}");
		return blockExpression;
	}

	protected override Expression VisitConditional(ConditionalExpression conditionalExpression)
	{
		Visit(conditionalExpression.Test);
		_stringBuilder.Append(" ? ");
		Visit(conditionalExpression.IfTrue);
		_stringBuilder.Append(" : ");
		Visit(conditionalExpression.IfFalse);
		return conditionalExpression;
	}

	protected override Expression VisitConstant(ConstantExpression constantExpression)
	{
		if (constantExpression.Value is IPrintableExpression printableExpression)
		{
			printableExpression.Print(this);
		}
		else if (constantExpression.IsEntityQueryable())
		{
			_stringBuilder.Append("DbSet<" + constantExpression.Type.GetTypeInfo().GenericTypeArguments.First().ShortDisplayName() + ">");
		}
		else
		{
			Print(constantExpression.Value);
		}
		return constantExpression;
	}

	private void Print(object value)
	{
		if (value is IEnumerable enumerable && !(value is string))
		{
			_stringBuilder.Append(value.GetType().ShortDisplayName() + " { ");
			foreach (object item in enumerable)
			{
				Print(item);
				_stringBuilder.Append(", ");
			}
			_stringBuilder.Append("}");
		}
		else
		{
			string text = ((value == null) ? "null" : ((value.ToString() != value.GetType().ToString()) ? value.ToString() : value.GetType().ShortDisplayName()));
			if (value != null && value is string)
			{
				text = "\"" + text + "\"";
			}
			_stringBuilder.Append(text);
		}
	}

	protected override Expression VisitGoto(GotoExpression gotoExpression)
	{
		AppendLine("return (" + gotoExpression.Target.Type.ShortDisplayName() + ")" + gotoExpression.Target?.ToString() + " {");
		using (_stringBuilder.Indent())
		{
			Visit(gotoExpression.Value);
		}
		_stringBuilder.Append("}");
		return gotoExpression;
	}

	protected override Expression VisitLabel(LabelExpression labelExpression)
	{
		_stringBuilder.Append(labelExpression.Target.ToString());
		return labelExpression;
	}

	protected override Expression VisitLambda<T>(Expression<T> lambdaExpression)
	{
		if (lambdaExpression.Parameters.Count != 1)
		{
			_stringBuilder.Append("(");
		}
		foreach (ParameterExpression parameter in lambdaExpression.Parameters)
		{
			string name = parameter.Name;
			if (!_parametersInScope.ContainsKey(parameter))
			{
				_parametersInScope.Add(parameter, name);
			}
			Visit(parameter);
			if (parameter != lambdaExpression.Parameters.Last())
			{
				_stringBuilder.Append(", ");
			}
		}
		if (lambdaExpression.Parameters.Count != 1)
		{
			_stringBuilder.Append(")");
		}
		_stringBuilder.Append(" => ");
		Visit(lambdaExpression.Body);
		foreach (ParameterExpression parameter2 in lambdaExpression.Parameters)
		{
			_parametersInScope.Remove(parameter2);
		}
		return lambdaExpression;
	}

	protected override Expression VisitMember(MemberExpression memberExpression)
	{
		if (memberExpression.Expression != null)
		{
			if (memberExpression.Expression.NodeType == ExpressionType.Convert || memberExpression.Expression is BinaryExpression)
			{
				_stringBuilder.Append("(");
				Visit(memberExpression.Expression);
				_stringBuilder.Append(")");
			}
			else
			{
				Visit(memberExpression.Expression);
			}
		}
		else
		{
			_stringBuilder.Append(memberExpression.Member.DeclaringType.Name);
		}
		_stringBuilder.Append("." + memberExpression.Member.Name);
		return memberExpression;
	}

	protected override Expression VisitMemberInit(MemberInitExpression memberInitExpression)
	{
		_stringBuilder.Append("new " + memberInitExpression.Type.ShortDisplayName());
		Action<string> action = ((memberInitExpression.Bindings.Count > 1) ? new Action<string>(AppendLine) : new Action<string>(Append));
		action("{ ");
		using (_stringBuilder.Indent())
		{
			for (int i = 0; i < memberInitExpression.Bindings.Count; i++)
			{
				if (memberInitExpression.Bindings[i] is MemberAssignment memberAssignment)
				{
					_stringBuilder.Append(memberAssignment.Member.Name + " = ");
					Visit(memberAssignment.Expression);
					action((i == memberInitExpression.Bindings.Count - 1) ? " " : ", ");
				}
				else
				{
					AppendLine(CoreStrings.InvalidMemberInitBinding);
				}
			}
		}
		AppendLine("}");
		return memberInitExpression;
	}

	protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
	{
		if (methodCallExpression.Object != null)
		{
			if (methodCallExpression.Object is BinaryExpression)
			{
				_stringBuilder.Append("(");
				Visit(methodCallExpression.Object);
				_stringBuilder.Append(")");
			}
			else
			{
				Visit(methodCallExpression.Object);
			}
			_stringBuilder.Append(".");
		}
		List<Expression> list = methodCallExpression.Arguments.ToList();
		MethodInfo method = methodCallExpression.Method;
		bool flag = !GenerateUniqueParameterIds && methodCallExpression.Arguments.Count > 0 && method.IsDefined(typeof(ExtensionAttribute), inherit: false);
		if (flag)
		{
			Visit(list[0]);
			_stringBuilder.IncrementIndent();
			_stringBuilder.AppendLine();
			_stringBuilder.Append("." + method.Name);
			list = list.Skip(1).ToList();
		}
		else
		{
			if (method.IsStatic)
			{
				_stringBuilder.Append(method.DeclaringType.ShortDisplayName()).Append(".");
			}
			_stringBuilder.Append(method.Name);
			if (method.IsGenericMethod)
			{
				_stringBuilder.Append("<");
				bool flag2 = true;
				Type[] genericArguments = method.GetGenericArguments();
				foreach (Type type in genericArguments)
				{
					if (!flag2)
					{
						_stringBuilder.Append(", ");
					}
					_stringBuilder.Append(type.ShortDisplayName());
					flag2 = false;
				}
				_stringBuilder.Append(">");
			}
		}
		_stringBuilder.Append("(");
		bool flag3 = _simpleMethods.Contains(method.Name) || list.Count < 2 || method.IsEFPropertyMethod();
		Action<string> action = (flag3 ? new Action<string>(Append) : new Action<string>(AppendLine));
		if (list.Count > 0)
		{
			action("");
			List<string> list2 = ((!flag3) ? (from p in method.GetParameters()
				select p.Name).ToList() : new List<string>());
			IDisposable disposable = null;
			if (!flag3)
			{
				disposable = _stringBuilder.Indent();
			}
			for (int j = 0; j < list.Count; j++)
			{
				Expression node = list[j];
				if (!flag3)
				{
					_stringBuilder.Append(list2[j] + ": ");
				}
				Visit(node);
				if (j < list.Count - 1)
				{
					action(", ");
				}
			}
			if (!flag3)
			{
				disposable?.Dispose();
			}
		}
		Append(")");
		if (flag)
		{
			_stringBuilder.DecrementIndent();
		}
		return methodCallExpression;
	}

	protected override Expression VisitNew(NewExpression newExpression)
	{
		_stringBuilder.Append("new ");
		bool flag = newExpression.Arguments.Count > 1;
		Action<string> action = (flag ? new Action<string>(AppendLine) : new Action<string>(Append));
		bool flag2 = newExpression.Type.IsAnonymousType();
		if (!flag2)
		{
			_stringBuilder.Append(newExpression.Type.ShortDisplayName());
			action("(");
		}
		else
		{
			action("{ ");
		}
		IDisposable disposable = null;
		if (flag)
		{
			disposable = _stringBuilder.Indent();
		}
		for (int i = 0; i < newExpression.Arguments.Count; i++)
		{
			if (newExpression.Members != null)
			{
				Append(newExpression.Members[i].Name + " = ");
			}
			Visit(newExpression.Arguments[i]);
			action((i == newExpression.Arguments.Count - 1) ? "" : ", ");
		}
		if (flag)
		{
			disposable?.Dispose();
		}
		if (!flag2)
		{
			_stringBuilder.Append(")");
		}
		else
		{
			_stringBuilder.Append(" }");
		}
		return newExpression;
	}

	protected override Expression VisitNewArray(NewArrayExpression newArrayExpression)
	{
		bool num = newArrayExpression.Expressions.Count > 1;
		Action<string> action = (num ? new Action<string>(AppendLine) : new Action<string>(Append));
		action("new " + newArrayExpression.Type.GetElementType().ShortDisplayName() + "[]");
		action("{ ");
		IDisposable disposable = null;
		if (num)
		{
			disposable = _stringBuilder.Indent();
		}
		VisitArguments(newArrayExpression.Expressions, action, " ");
		if (num)
		{
			disposable?.Dispose();
		}
		Append("}");
		return newArrayExpression;
	}

	protected override Expression VisitParameter(ParameterExpression parameterExpression)
	{
		if (_parametersInScope.ContainsKey(parameterExpression))
		{
			string text = _parametersInScope[parameterExpression];
			if (text == null)
			{
				if (!_namelessParameters.Contains(parameterExpression))
				{
					_namelessParameters.Add(parameterExpression);
				}
				Append("namelessParameter{");
				Append(_namelessParameters.IndexOf(parameterExpression));
				Append("}");
			}
			else if (text.Contains("."))
			{
				Append("[");
				Append(text);
				Append("]");
			}
			else
			{
				Append(text);
			}
		}
		else if (GenerateUniqueParameterIds)
		{
			Append("(Unhandled parameter: ");
			Append(parameterExpression.Name);
			Append(")");
		}
		else
		{
			Append(parameterExpression.Name);
		}
		if (GenerateUniqueParameterIds)
		{
			int num = _encounteredParameters.Count;
			if (_encounteredParameters.Contains(parameterExpression))
			{
				num = _encounteredParameters.IndexOf(parameterExpression);
			}
			else
			{
				_encounteredParameters.Add(parameterExpression);
			}
			_stringBuilder.Append("{" + num + "}");
		}
		return parameterExpression;
	}

	protected override Expression VisitUnary(UnaryExpression unaryExpression)
	{
		switch (unaryExpression.NodeType)
		{
		case ExpressionType.Convert:
			_stringBuilder.Append("(" + unaryExpression.Type.ShortDisplayName() + ")");
			if (unaryExpression.Operand is BinaryExpression)
			{
				_stringBuilder.Append("(");
				Visit(unaryExpression.Operand);
				_stringBuilder.Append(")");
			}
			else
			{
				Visit(unaryExpression.Operand);
			}
			break;
		case ExpressionType.Throw:
			_stringBuilder.Append("throw ");
			Visit(unaryExpression.Operand);
			break;
		case ExpressionType.Not:
			_stringBuilder.Append("!(");
			Visit(unaryExpression.Operand);
			_stringBuilder.Append(")");
			break;
		case ExpressionType.TypeAs:
			_stringBuilder.Append("(");
			Visit(unaryExpression.Operand);
			_stringBuilder.Append(" as " + unaryExpression.Type.ShortDisplayName() + ")");
			break;
		case ExpressionType.Quote:
			Visit(unaryExpression.Operand);
			break;
		default:
			UnhandledExpressionType(unaryExpression);
			break;
		}
		return unaryExpression;
	}

	protected override Expression VisitDefault(DefaultExpression defaultExpression)
	{
		_stringBuilder.Append("default(" + defaultExpression.Type.ShortDisplayName() + ")");
		return defaultExpression;
	}

	protected override Expression VisitTry(TryExpression tryExpression)
	{
		_stringBuilder.Append("try { ");
		Visit(tryExpression.Body);
		_stringBuilder.Append(" } ");
		foreach (CatchBlock handler in tryExpression.Handlers)
		{
			_stringBuilder.Append("catch (" + handler.Test.Name + ") { ... } ");
		}
		return tryExpression;
	}

	protected override Expression VisitIndex(IndexExpression indexExpression)
	{
		Visit(indexExpression.Object);
		_stringBuilder.Append("[");
		VisitArguments(indexExpression.Arguments, delegate(string s)
		{
			_stringBuilder.Append(s);
		});
		_stringBuilder.Append("]");
		return indexExpression;
	}

	protected override Expression VisitTypeBinary(TypeBinaryExpression typeBinaryExpression)
	{
		_stringBuilder.Append("(");
		Visit(typeBinaryExpression.Expression);
		_stringBuilder.Append(" is " + typeBinaryExpression.TypeOperand.ShortDisplayName() + ")");
		return typeBinaryExpression;
	}

	protected override Expression VisitExtension(Expression extensionExpression)
	{
		if (extensionExpression is IPrintableExpression printableExpression)
		{
			_stringBuilder.Append("(");
			printableExpression.Print(this);
			_stringBuilder.Append(")");
		}
		else
		{
			UnhandledExpressionType(extensionExpression);
		}
		return extensionExpression;
	}

	private void VisitArguments(IReadOnlyList<Expression> arguments, Action<string> appendAction, string lastSeparator = "", bool areConnected = false)
	{
		for (int i = 0; i < arguments.Count; i++)
		{
			if (areConnected && i == arguments.Count - 1)
			{
				Append("");
			}
			Visit(arguments[i]);
			appendAction((i == arguments.Count - 1) ? lastSeparator : ", ");
		}
	}

	protected virtual string PostProcess([JetBrains.Annotations.NotNull] string printedExpression)
	{
		return printedExpression.Replace("Microsoft.EntityFrameworkCore.Query.", "").Replace("Microsoft.EntityFrameworkCore.", "").Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
	}

	private void UnhandledExpressionType(Expression expression)
	{
		AppendLine(expression.ToString());
	}
}
