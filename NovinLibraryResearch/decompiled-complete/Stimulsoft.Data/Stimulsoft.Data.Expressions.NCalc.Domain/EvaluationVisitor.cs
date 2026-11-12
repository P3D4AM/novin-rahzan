using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Data.Extensions;

namespace Stimulsoft.Data.Expressions.NCalc.Domain;

public class EvaluationVisitor : LogicalExpressionVisitor
{
	private delegate T Func<T>();

	private readonly EvaluateOptions _options = EvaluateOptions.None;

	private static Type[] CommonTypes = new Type[5]
	{
		typeof(long),
		typeof(double),
		typeof(bool),
		typeof(string),
		typeof(decimal)
	};

	private bool IgnoreCase => (_options & EvaluateOptions.IgnoreCase) == EvaluateOptions.IgnoreCase;

	public object Result { get; private set; }

	public Dictionary<string, object> Parameters { get; set; }

	public event EvaluateFunctionHandler EvaluateFunction;

	public event EvaluateParameterHandler EvaluateParameter;

	public EvaluationVisitor(EvaluateOptions options)
	{
		_options = options;
	}

	private object Evaluate(LogicalExpression expression)
	{
		expression.Accept(this);
		return Result;
	}

	public override void Visit(LogicalExpression expression)
	{
		throw new Exception("The method or operation is not implemented.");
	}

	private static Type GetMostPreciseType(Type a, Type b)
	{
		Type[] commonTypes = CommonTypes;
		foreach (Type type in commonTypes)
		{
			if (a == type || b == type)
			{
				return type;
			}
		}
		return a;
	}

	public object CompareUsingMostPreciseType(object a, object b)
	{
		Type mostPreciseType = GetMostPreciseType(a?.GetType(), b?.GetType());
		if (ListExt.IsList(a) || ListExt.IsList(b))
		{
			return ListExt.Compare(a, b);
		}
		object a2 = ((a != null) ? Convert.ChangeType(a, mostPreciseType) : null);
		object b2 = ((b != null) ? Convert.ChangeType(b, mostPreciseType) : null);
		return Comparer.Default.Compare(a2, b2);
	}

	public override void Visit(TernaryExpression expression)
	{
		expression.LeftExpression.Accept(this);
		if (Convert.ToBoolean(Result))
		{
			expression.MiddleExpression.Accept(this);
		}
		else
		{
			expression.RightExpression.Accept(this);
		}
	}

	private static bool IsReal(object value)
	{
		if (value == null)
		{
			return false;
		}
		TypeCode typeCode = Type.GetTypeCode(value.GetType());
		if (typeCode != TypeCode.Decimal && typeCode != TypeCode.Double)
		{
			return typeCode == TypeCode.Single;
		}
		return true;
	}

	public override void Visit(BinaryExpression expression)
	{
		object leftValue = null;
		Func<object> func = delegate
		{
			if (leftValue == null)
			{
				expression.LeftExpression.Accept(this);
				leftValue = Result;
			}
			return leftValue;
		};
		object rightValue = null;
		Func<object> func2 = delegate
		{
			if (rightValue == null)
			{
				expression.RightExpression.Accept(this);
				rightValue = Result;
			}
			return rightValue;
		};
		switch (expression.Type)
		{
		case BinaryExpressionType.And:
			Result = Convert.ToBoolean(func()) && Convert.ToBoolean(func2());
			break;
		case BinaryExpressionType.Or:
			Result = Convert.ToBoolean(func()) || Convert.ToBoolean(func2());
			break;
		case BinaryExpressionType.Div:
			if (func() is IEnumerable<object> || func2() is IEnumerable<object>)
			{
				Result = Numbers.Divide(func(), func2());
			}
			else if (IsReal(func()) || IsReal(func2()))
			{
				Result = Numbers.Divide(func(), func2());
			}
			else
			{
				Result = Numbers.Divide(Convert.ToDouble(func()), func2());
			}
			break;
		case BinaryExpressionType.Equal:
			Result = CompareUsingMostPreciseType(func(), func2());
			if (Result is int)
			{
				Result = (int)Result == 0;
			}
			if (ListExt.IsList(Result))
			{
				Result = (from v in ListExt.ToList(Result).TryCastToInt()
					select v == 0).ToList();
			}
			break;
		case BinaryExpressionType.Greater:
			Result = CompareUsingMostPreciseType(func(), func2());
			if (Result is int)
			{
				Result = (int)Result > 0;
			}
			if (ListExt.IsList(Result))
			{
				Result = (from v in ListExt.ToList(Result).TryCastToInt()
					select v > 0).ToList();
			}
			break;
		case BinaryExpressionType.GreaterOrEqual:
			Result = CompareUsingMostPreciseType(func(), func2());
			if (Result is int)
			{
				Result = (int)Result >= 0;
			}
			if (ListExt.IsList(Result))
			{
				Result = (from v in ListExt.ToList(Result).TryCastToInt()
					select v >= 0).ToList();
			}
			break;
		case BinaryExpressionType.Lesser:
			Result = CompareUsingMostPreciseType(func(), func2());
			if (Result is int)
			{
				Result = (int)Result < 0;
			}
			if (ListExt.IsList(Result))
			{
				Result = (from v in ListExt.ToList(Result).TryCastToInt()
					select v < 0).ToList();
			}
			break;
		case BinaryExpressionType.LesserOrEqual:
			Result = CompareUsingMostPreciseType(func(), func2());
			if (Result is int)
			{
				Result = (int)Result <= 0;
			}
			if (ListExt.IsList(Result))
			{
				Result = (from v in ListExt.ToList(Result).TryCastToInt()
					select v <= 0).ToList();
			}
			break;
		case BinaryExpressionType.Minus:
			Result = Numbers.Soustract(func(), func2());
			break;
		case BinaryExpressionType.Modulo:
			Result = Numbers.Modulo(func(), func2());
			break;
		case BinaryExpressionType.NotEqual:
			Result = CompareUsingMostPreciseType(func(), func2());
			if (Result is int)
			{
				Result = (int)Result != 0;
			}
			if (ListExt.IsList(Result))
			{
				Result = (from v in ListExt.ToList(Result).TryCastToInt()
					select v != 0).ToList();
			}
			break;
		case BinaryExpressionType.Plus:
			Result = ((func() is string) ? string.Concat(func(), func2()) : Numbers.Add(func(), func2()));
			break;
		case BinaryExpressionType.Times:
			Result = Numbers.Multiply(func(), func2());
			break;
		case BinaryExpressionType.BitwiseAnd:
		{
			object obj5 = func();
			object obj6 = func2();
			if (ListExt.IsList(obj5) || ListExt.IsList(obj6))
			{
				Result = IEnumerableExt.BitwiseAnd(obj5, obj6);
			}
			else
			{
				Result = Convert.ToUInt16(obj5) & Convert.ToUInt16(obj6);
			}
			break;
		}
		case BinaryExpressionType.BitwiseOr:
		{
			object obj3 = func();
			object obj4 = func2();
			if (ListExt.IsList(obj3) || ListExt.IsList(obj4))
			{
				Result = IEnumerableExt.BitwiseOr(obj3, obj4);
			}
			else
			{
				Result = Convert.ToUInt16(obj3) | Convert.ToUInt16(obj4);
			}
			break;
		}
		case BinaryExpressionType.BitwiseXOr:
		{
			object obj = func();
			object obj2 = func2();
			if (ListExt.IsList(obj) || ListExt.IsList(obj2))
			{
				Result = IEnumerableExt.BitwiseXOr(obj, obj2);
			}
			else
			{
				Result = Convert.ToUInt16(obj) | Convert.ToUInt16(obj2);
			}
			break;
		}
		case BinaryExpressionType.LeftShift:
			Result = Convert.ToUInt16(func()) << (int)Convert.ToUInt16(func2());
			break;
		case BinaryExpressionType.RightShift:
			Result = Convert.ToUInt16(func()) >> (int)Convert.ToUInt16(func2());
			break;
		}
	}

	public override void Visit(UnaryExpression expression)
	{
		expression.Expression.Accept(this);
		switch (expression.Type)
		{
		case UnaryExpressionType.Not:
			if (ListExt.IsList(Result))
			{
				Result = IEnumerableExt.BitwiseXOr(Result, 1);
			}
			else
			{
				Result = !Convert.ToBoolean(Result);
			}
			break;
		case UnaryExpressionType.Negate:
			Result = Numbers.Soustract(0, Result);
			break;
		case UnaryExpressionType.BitwiseNot:
			Result = ~Convert.ToUInt16(Result);
			break;
		}
	}

	public override void Visit(ValueExpression expression)
	{
		Result = expression.Value;
	}

	public override void Visit(Function function)
	{
		FunctionArgs functionArgs = new FunctionArgs();
		functionArgs.Parameters = new Expression[function.Expressions.Length];
		FunctionArgs functionArgs2 = functionArgs;
		for (int i = 0; i < function.Expressions.Length; i++)
		{
			functionArgs2.Parameters[i] = new Expression(function.Expressions[i], _options);
			functionArgs2.Parameters[i].EvaluateFunction += this.EvaluateFunction;
			functionArgs2.Parameters[i].EvaluateParameter += this.EvaluateParameter;
			functionArgs2.Parameters[i].Parameters = Parameters;
		}
		OnEvaluateFunction(IgnoreCase ? function.Identifier.Name.ToLower() : function.Identifier.Name, functionArgs2);
		if (functionArgs2.HasResult)
		{
			Result = functionArgs2.Result;
			return;
		}
		switch (function.Identifier.Name.ToLower())
		{
		case "abs":
			CheckCase("Abs", function.Identifier.Name);
			if (function.Expressions.Length != 1)
			{
				throw new ArgumentException("Abs() takes exactly 1 argument");
			}
			Result = Math.Abs(Convert.ToDecimal(Evaluate(function.Expressions[0])));
			break;
		case "acos":
			CheckCase("Acos", function.Identifier.Name);
			if (function.Expressions.Length != 1)
			{
				throw new ArgumentException("Acos() takes exactly 1 argument");
			}
			Result = Math.Acos(Convert.ToDouble(Evaluate(function.Expressions[0])));
			break;
		case "asin":
			CheckCase("Asin", function.Identifier.Name);
			if (function.Expressions.Length != 1)
			{
				throw new ArgumentException("Asin() takes exactly 1 argument");
			}
			Result = Math.Asin(Convert.ToDouble(Evaluate(function.Expressions[0])));
			break;
		case "atan":
			CheckCase("Atan", function.Identifier.Name);
			if (function.Expressions.Length != 1)
			{
				throw new ArgumentException("Atan() takes exactly 1 argument");
			}
			Result = Math.Atan(Convert.ToDouble(Evaluate(function.Expressions[0])));
			break;
		case "ceiling":
			CheckCase("Ceiling", function.Identifier.Name);
			if (function.Expressions.Length != 1)
			{
				throw new ArgumentException("Ceiling() takes exactly 1 argument");
			}
			Result = Math.Ceiling(Convert.ToDouble(Evaluate(function.Expressions[0])));
			break;
		case "cos":
			CheckCase("Cos", function.Identifier.Name);
			if (function.Expressions.Length != 1)
			{
				throw new ArgumentException("Cos() takes exactly 1 argument");
			}
			Result = Math.Cos(Convert.ToDouble(Evaluate(function.Expressions[0])));
			break;
		case "exp":
			CheckCase("Exp", function.Identifier.Name);
			if (function.Expressions.Length != 1)
			{
				throw new ArgumentException("Exp() takes exactly 1 argument");
			}
			Result = Math.Exp(Convert.ToDouble(Evaluate(function.Expressions[0])));
			break;
		case "floor":
			CheckCase("Floor", function.Identifier.Name);
			if (function.Expressions.Length != 1)
			{
				throw new ArgumentException("Floor() takes exactly 1 argument");
			}
			Result = Math.Floor(Convert.ToDouble(Evaluate(function.Expressions[0])));
			break;
		case "ieeeremainder":
			CheckCase("IEEERemainder", function.Identifier.Name);
			if (function.Expressions.Length != 2)
			{
				throw new ArgumentException("IEEERemainder() takes exactly 2 arguments");
			}
			Result = Math.IEEERemainder(Convert.ToDouble(Evaluate(function.Expressions[0])), Convert.ToDouble(Evaluate(function.Expressions[1])));
			break;
		case "log":
			CheckCase("Log", function.Identifier.Name);
			if (function.Expressions.Length != 2)
			{
				throw new ArgumentException("Log() takes exactly 2 arguments");
			}
			Result = Math.Log(Convert.ToDouble(Evaluate(function.Expressions[0])), Convert.ToDouble(Evaluate(function.Expressions[1])));
			break;
		case "log10":
			CheckCase("Log10", function.Identifier.Name);
			if (function.Expressions.Length != 1)
			{
				throw new ArgumentException("Log10() takes exactly 1 argument");
			}
			Result = Math.Log10(Convert.ToDouble(Evaluate(function.Expressions[0])));
			break;
		case "pow":
			CheckCase("Pow", function.Identifier.Name);
			if (function.Expressions.Length != 2)
			{
				throw new ArgumentException("Pow() takes exactly 2 arguments");
			}
			Result = Math.Pow(Convert.ToDouble(Evaluate(function.Expressions[0])), Convert.ToDouble(Evaluate(function.Expressions[1])));
			break;
		case "round":
		{
			CheckCase("Round", function.Identifier.Name);
			if (function.Expressions.Length != 2)
			{
				throw new ArgumentException("Round() takes exactly 2 arguments");
			}
			MidpointRounding mode = (((_options & EvaluateOptions.RoundAwayFromZero) == EvaluateOptions.RoundAwayFromZero) ? MidpointRounding.AwayFromZero : MidpointRounding.ToEven);
			Result = Math.Round(Convert.ToDouble(Evaluate(function.Expressions[0])), Convert.ToInt16(Evaluate(function.Expressions[1])), mode);
			break;
		}
		case "sign":
			CheckCase("Sign", function.Identifier.Name);
			if (function.Expressions.Length != 1)
			{
				throw new ArgumentException("Sign() takes exactly 1 argument");
			}
			Result = Math.Sign(Convert.ToDouble(Evaluate(function.Expressions[0])));
			break;
		case "sin":
			CheckCase("Sin", function.Identifier.Name);
			if (function.Expressions.Length != 1)
			{
				throw new ArgumentException("Sin() takes exactly 1 argument");
			}
			Result = Math.Sin(Convert.ToDouble(Evaluate(function.Expressions[0])));
			break;
		case "sqrt":
			CheckCase("Sqrt", function.Identifier.Name);
			if (function.Expressions.Length != 1)
			{
				throw new ArgumentException("Sqrt() takes exactly 1 argument");
			}
			Result = Math.Sqrt(Convert.ToDouble(Evaluate(function.Expressions[0])));
			break;
		case "tan":
			CheckCase("Tan", function.Identifier.Name);
			if (function.Expressions.Length != 1)
			{
				throw new ArgumentException("Tan() takes exactly 1 argument");
			}
			Result = Math.Tan(Convert.ToDouble(Evaluate(function.Expressions[0])));
			break;
		case "truncate":
			CheckCase("Truncate", function.Identifier.Name);
			if (function.Expressions.Length != 1)
			{
				throw new ArgumentException("Truncate() takes exactly 1 argument");
			}
			Result = Math.Truncate(Convert.ToDouble(Evaluate(function.Expressions[0])));
			break;
		case "max":
		{
			CheckCase("Max", function.Identifier.Name);
			if (function.Expressions.Length != 2)
			{
				throw new ArgumentException("Max() takes exactly 2 arguments");
			}
			object a3 = Evaluate(function.Expressions[0]);
			object b3 = Evaluate(function.Expressions[1]);
			Result = Numbers.Max(a3, b3);
			break;
		}
		case "min":
		{
			CheckCase("Min", function.Identifier.Name);
			if (function.Expressions.Length != 2)
			{
				throw new ArgumentException("Min() takes exactly 2 arguments");
			}
			object a2 = Evaluate(function.Expressions[0]);
			object b2 = Evaluate(function.Expressions[1]);
			Result = Numbers.Min(a2, b2);
			break;
		}
		case "if":
		{
			CheckCase("if", function.Identifier.Name);
			if (function.Expressions.Length != 3)
			{
				throw new ArgumentException("if() takes exactly 3 arguments");
			}
			bool flag2 = Convert.ToBoolean(Evaluate(function.Expressions[0]));
			Result = (flag2 ? Evaluate(function.Expressions[1]) : Evaluate(function.Expressions[2]));
			break;
		}
		case "in":
		{
			CheckCase("in", function.Identifier.Name);
			if (function.Expressions.Length < 2)
			{
				throw new ArgumentException("in() takes at least 2 arguments");
			}
			object a = Evaluate(function.Expressions[0]);
			bool flag = false;
			for (int j = 1; j < function.Expressions.Length; j++)
			{
				object b = Evaluate(function.Expressions[j]);
				object obj = CompareUsingMostPreciseType(a, b);
				if (obj is int && (int)obj == 0)
				{
					flag = true;
					break;
				}
			}
			Result = flag;
			break;
		}
		default:
			throw new ArgumentException("Function not found", function.Identifier.Name);
		}
	}

	private void CheckCase(string function, string called)
	{
		if (IgnoreCase)
		{
			if (!(function.ToLower() == called.ToLower()))
			{
				throw new ArgumentException("Function not found", called);
			}
		}
		else if (function != called)
		{
			throw new ArgumentException($"Function not found {called}. Try {function} instead.");
		}
	}

	private void OnEvaluateFunction(string name, FunctionArgs args)
	{
		if (this.EvaluateFunction != null)
		{
			this.EvaluateFunction(name, args);
		}
	}

	public override void Visit(Identifier parameter)
	{
		if (Parameters.ContainsKey(parameter.Name))
		{
			if (Parameters[parameter.Name] is Expression)
			{
				Expression expression = (Expression)Parameters[parameter.Name];
				foreach (KeyValuePair<string, object> parameter2 in Parameters)
				{
					expression.Parameters[parameter2.Key] = parameter2.Value;
				}
				expression.EvaluateFunction += this.EvaluateFunction;
				expression.EvaluateParameter += this.EvaluateParameter;
				Result = ((Expression)Parameters[parameter.Name]).Evaluate();
			}
			else
			{
				Result = Parameters[parameter.Name];
			}
		}
		else
		{
			ParameterArgs parameterArgs = new ParameterArgs();
			OnEvaluateParameter(parameter.Name, parameterArgs);
			if (!parameterArgs.HasResult)
			{
				throw new ArgumentException("Parameter was not defined", parameter.Name);
			}
			Result = parameterArgs.Result;
		}
	}

	private void OnEvaluateParameter(string name, ParameterArgs args)
	{
		if (this.EvaluateParameter != null)
		{
			this.EvaluateParameter(name, args);
		}
	}
}
