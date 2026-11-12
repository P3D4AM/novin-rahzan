using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Facets;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class EvaluationFactory
{
	private static TraceContext traceContext;

	private static readonly Dictionary<Type, SupportedTypeAttributes> supportedTypes;

	private static readonly Dictionary<TypeClass, List<OperatorType>> supportedTypeClass;

	private static readonly List<OperatorType> operatorsDefault;

	private EvaluationFactory()
	{
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public static bool IsTypeSupported(Type type)
	{
		if (!(type == typeof(DBNull)) && !(null == type) && !supportedTypes.ContainsKey(type))
		{
			return type.IsEnum;
		}
		return true;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public static bool IsTypeSupportedForConstant(Type type)
	{
		if (type == null)
		{
			return false;
		}
		if (supportedTypes.ContainsKey(type))
		{
			TypeClass @class = supportedTypes[type].Class;
			if (@class != TypeClass.Numeric)
			{
				return @class == TypeClass.String;
			}
			return true;
		}
		return type.IsEnum;
	}

	static EvaluationFactory()
	{
		traceContext = TraceContext.GetTraceContext("DMF", "EvaluationFactory");
		supportedTypes = new Dictionary<Type, SupportedTypeAttributes>(21);
		supportedTypeClass = new Dictionary<TypeClass, List<OperatorType>>();
		operatorsDefault = new List<OperatorType>(2);
		List<OperatorType> list = new List<OperatorType>(8)
		{
			OperatorType.EQ,
			OperatorType.GE,
			OperatorType.GT,
			OperatorType.LE,
			OperatorType.LT,
			OperatorType.NE,
			OperatorType.IN,
			OperatorType.NOT_IN
		};
		List<OperatorType> list2 = new List<OperatorType>(6)
		{
			OperatorType.EQ,
			OperatorType.NE,
			OperatorType.LIKE,
			OperatorType.IN,
			OperatorType.NOT_LIKE,
			OperatorType.NOT_IN
		};
		List<OperatorType> list3 = new List<OperatorType>(2)
		{
			OperatorType.EQ,
			OperatorType.NE
		};
		List<OperatorType> list4 = new List<OperatorType>(6)
		{
			OperatorType.EQ,
			OperatorType.GE,
			OperatorType.GT,
			OperatorType.LE,
			OperatorType.LT,
			OperatorType.NE
		};
		List<OperatorType> list5 = new List<OperatorType>(4)
		{
			OperatorType.EQ,
			OperatorType.NE,
			OperatorType.IN,
			OperatorType.NOT_IN
		};
		List<OperatorType> value = new List<OperatorType>(2)
		{
			OperatorType.BEQ,
			OperatorType.BNE
		};
		operatorsDefault.Add(OperatorType.EQ);
		operatorsDefault.Add(OperatorType.NE);
		supportedTypes.Add(typeof(int), new SupportedTypeAttributes(TypeClass.Numeric, list));
		supportedTypes.Add(typeof(byte), new SupportedTypeAttributes(TypeClass.Numeric, list));
		supportedTypes.Add(typeof(long), new SupportedTypeAttributes(TypeClass.Numeric, list));
		supportedTypes.Add(typeof(short), new SupportedTypeAttributes(TypeClass.Numeric, list));
		supportedTypes.Add(typeof(double), new SupportedTypeAttributes(TypeClass.Numeric, list));
		supportedTypes.Add(typeof(float), new SupportedTypeAttributes(TypeClass.Numeric, list));
		supportedTypes.Add(typeof(decimal), new SupportedTypeAttributes(TypeClass.Numeric, list));
		supportedTypes.Add(typeof(string), new SupportedTypeAttributes(TypeClass.String, list2));
		supportedTypes.Add(typeof(char), new SupportedTypeAttributes(TypeClass.String, list2));
		supportedTypes.Add(typeof(bool), new SupportedTypeAttributes(TypeClass.Bool, list3));
		supportedTypes.Add(typeof(DateTime), new SupportedTypeAttributes(TypeClass.DateTime, list4));
		supportedTypes.Add(typeof(Guid), new SupportedTypeAttributes(TypeClass.Guid, operatorsDefault));
		supportedTypes.Add(typeof(byte[]), new SupportedTypeAttributes(TypeClass.Array, list5));
		supportedTypes.Add(typeof(int[]), new SupportedTypeAttributes(TypeClass.Array, list5));
		supportedTypes.Add(typeof(long[]), new SupportedTypeAttributes(TypeClass.Array, list5));
		supportedTypes.Add(typeof(short[]), new SupportedTypeAttributes(TypeClass.Array, list5));
		supportedTypes.Add(typeof(double[]), new SupportedTypeAttributes(TypeClass.Array, list5));
		supportedTypes.Add(typeof(float[]), new SupportedTypeAttributes(TypeClass.Array, list5));
		supportedTypes.Add(typeof(decimal[]), new SupportedTypeAttributes(TypeClass.Array, list5));
		supportedTypes.Add(typeof(string[]), new SupportedTypeAttributes(TypeClass.Array, list5));
		supportedTypes.Add(typeof(char[]), new SupportedTypeAttributes(TypeClass.Array, list5));
		supportedTypeClass.Add(TypeClass.Numeric, list);
		supportedTypeClass.Add(TypeClass.String, list2);
		supportedTypeClass.Add(TypeClass.Bool, list3);
		supportedTypeClass.Add(TypeClass.DateTime, list4);
		supportedTypeClass.Add(TypeClass.Guid, operatorsDefault);
		supportedTypeClass.Add(TypeClass.Array, list5);
		supportedTypeClass.Add(TypeClass.BitmappedEnum, value);
		supportedTypeClass.Add(TypeClass.Unsupported, operatorsDefault);
	}

	public static List<OperatorType> SupportedOperators(Type type)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("SupportedOperators", TraceEventType.Information);
		methodTraceContext.TraceParameters(type);
		if (!IsTypeSupported(type))
		{
			return new List<OperatorType>();
		}
		if (null == type || type.IsEnum)
		{
			methodTraceContext.TraceParameterOut("returnVal", operatorsDefault);
			return operatorsDefault;
		}
		return supportedTypes[type].SupportedOperators;
	}

	public static List<OperatorType> SupportedOperators(TypeClass typeClass)
	{
		traceContext.DebugAssert(supportedTypeClass.ContainsKey(typeClass), "Unexpected TypeClass requested for supported operators");
		return supportedTypeClass[typeClass];
	}

	public static string LikeToRegex(string likePattern)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("LikeToRegex", TraceEventType.Information);
		methodTraceContext.TraceParameters(likePattern);
		Regex regex = new Regex("\\[.\\]");
		likePattern = likePattern.Replace("''", "'");
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("\\A");
		for (int i = 0; i < likePattern.Length; i++)
		{
			switch (likePattern[i])
			{
			case '\\':
				if (flag2)
				{
					stringBuilder.Append("\\\\");
				}
				else
				{
					flag = true;
				}
				break;
			case '%':
				if (flag2)
				{
					stringBuilder.Append(likePattern[i]);
				}
				else
				{
					stringBuilder.Append(".*");
				}
				break;
			case '_':
				if (flag2)
				{
					stringBuilder.Append(likePattern[i]);
				}
				else
				{
					stringBuilder.Append('.');
				}
				break;
			case '*':
				stringBuilder.Append("\\*");
				break;
			case '.':
				stringBuilder.Append("\\.");
				break;
			case '[':
			{
				if (flag2)
				{
					stringBuilder.Append("\\[");
					break;
				}
				string text = string.Empty;
				char c = '\0';
				if (likePattern.Length >= i + 3)
				{
					text = likePattern.Substring(i, 3);
					c = text[1];
				}
				else if (likePattern.Length >= i + 2)
				{
					c = likePattern[i + 1];
				}
				if (regex.IsMatch(text))
				{
					switch (text[1])
					{
					case '*':
						stringBuilder.Append("\\*");
						break;
					case '[':
						stringBuilder.Append("\\[");
						break;
					case ']':
						stringBuilder.Append("\\]");
						break;
					case '.':
						stringBuilder.Append("\\.");
						break;
					case '\\':
						stringBuilder.Append("\\\\");
						break;
					case '^':
						stringBuilder.Append("\\^");
						break;
					case '$':
						stringBuilder.Append("\\$");
						break;
					case '+':
						stringBuilder.Append("\\+");
						break;
					case '?':
						stringBuilder.Append("\\?");
						break;
					default:
						stringBuilder.Append(text[1]);
						break;
					}
					i += 2;
					continue;
				}
				stringBuilder.Append(likePattern[i]);
				if (c == '^')
				{
					stringBuilder.Append(c);
					i++;
				}
				flag3 = true;
				break;
			}
			case ']':
				if (flag2)
				{
					stringBuilder.Append("\\]");
					break;
				}
				stringBuilder.Append(likePattern[i]);
				flag3 = false;
				break;
			case '(':
				stringBuilder.Append("\\(");
				break;
			case ')':
				stringBuilder.Append("\\)");
				break;
			case '^':
				stringBuilder.Append("\\^");
				break;
			case '$':
				stringBuilder.Append("\\$");
				break;
			case '+':
				stringBuilder.Append("\\+");
				break;
			case '?':
				stringBuilder.Append("\\?");
				break;
			default:
				stringBuilder.Append(likePattern[i]);
				break;
			}
			if (flag)
			{
				flag2 = true;
				flag = false;
			}
			else
			{
				flag2 = false;
			}
		}
		if (flag3)
		{
			throw methodTraceContext.TraceThrow(new ArgumentException(ExceptionTemplatesSR.ParsingArgumentException(likePattern, ExceptionTemplatesSR.ParsingUnclosedBracketMsg)));
		}
		stringBuilder.Append("\\Z");
		return stringBuilder.ToString();
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	internal static TypeClass ClassifyType(object obj)
	{
		return ClassifyType(obj.GetType());
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	internal static TypeClass ClassifyType(Type type)
	{
		if (supportedTypes.ContainsKey(type))
		{
			return supportedTypes[type].Class;
		}
		if (type.IsEnum)
		{
			return supportedTypes[typeof(int)].Class;
		}
		if (type.IsArray)
		{
			return TypeClass.Array;
		}
		return TypeClass.Unsupported;
	}

	internal static Evaluator ConstructEvaluator(object left, object right, OperatorType opType, FacetEvaluationContext context)
	{
		if (left == null || right == null)
		{
			return new EvaluatorNull();
		}
		if (left is DBNull || right is DBNull)
		{
			return new EvaluatorDBNull();
		}
		if (!(left is Array) && !IsTypeSupported(left.GetType()))
		{
			throw traceContext.TraceThrow(new UnsupportedObjectTypeException(left.GetType().Name, typeof(EvaluationFactory).Name));
		}
		if (!(right is Array) && !IsTypeSupported(right.GetType()))
		{
			throw traceContext.TraceThrow(new UnsupportedObjectTypeException(right.GetType().Name, typeof(EvaluationFactory).Name));
		}
		TypeClass typeClass = ClassifyType(left);
		TypeClass typeClass2 = ClassifyType(right);
		switch (opType)
		{
		case OperatorType.IN:
		case OperatorType.NOT_IN:
			if (!SupportedOperators(left.GetType()).Contains(opType))
			{
				throw traceContext.TraceThrow(new OperatorNotApplicableException(ExpressionNodeOperator.OperatorTypeToString(opType), typeClass.ToString()));
			}
			switch (typeClass)
			{
			case TypeClass.Array:
				throw traceContext.TraceThrow(new InvalidInOperatorException(ExpressionNodeOperator.OperatorTypeToString(opType)));
			case TypeClass.Numeric:
				if (typeClass2 == TypeClass.Array)
				{
					return new EvaluatorArrayNumeric();
				}
				break;
			}
			if (typeClass == TypeClass.String && typeClass2 == TypeClass.Array)
			{
				return new EvaluatorArrayString(context);
			}
			throw traceContext.TraceThrow(new ExpressionTypeMistmatchException(typeClass.ToString(), typeClass2.ToString()));
		case OperatorType.BEQ:
		case OperatorType.BNE:
			return new EvaluatorBitmappedEnum();
		case OperatorType.NONE:
			traceContext.DebugAssert(test: false, "Unsupported Operator");
			return null;
		default:
			if (typeClass != typeClass2)
			{
				throw traceContext.TraceThrow(new ExpressionTypeMistmatchException(typeClass.ToString(), typeClass2.ToString()));
			}
			switch (typeClass)
			{
			case TypeClass.Numeric:
				return new EvaluatorNumeric();
			case TypeClass.String:
				return new EvaluatorString(context);
			case TypeClass.Bool:
				return new EvaluatorBool();
			case TypeClass.DateTime:
				return new EvaluatorDateTime();
			case TypeClass.Guid:
				return new EvaluatorGuid();
			case TypeClass.Array:
				return EvaluatorArray.ConstructEvaluator(left);
			default:
				traceContext.DebugAssert(test: false, "Unsupported TypeClass");
				return null;
			}
		}
	}

	internal static bool Evaluate(object left, object right, OperatorType opType, FacetEvaluationContext context)
	{
		Evaluator evaluator = null;
		evaluator = ConstructEvaluator(left, right, opType, context);
		if (evaluator == null)
		{
			traceContext.DebugAssert(test: false, $"Failed to construct evaluator for types '{left.GetType().Name}','{right.GetType().Name}'");
		}
		evaluator.SetEvaluatorObjects(left, right);
		return evaluator.Evaluate(opType);
	}

	internal static bool Evaluate(object left, object right, OperatorType opType)
	{
		return Evaluate(left, right, opType, null);
	}
}
