using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class ExpressionNodeOperator : ExpressionNodeChildren
{
	private static TraceContext traceContext;

	private const string cXmlTagOpType = "OpType";

	private OperatorType mOpType;

	internal static readonly List<Type> numericTypes;

	internal static readonly List<Type> stringTypes;

	internal static readonly List<OperatorType> operatorsFilterNode;

	public OperatorType OpType => mOpType;

	public ExpressionNode Left
	{
		get
		{
			return base.ChildrenList[0];
		}
		set
		{
			traceContext.TraceVerbose("Setting Left to: {0}", value);
			base.ChildrenList[0] = value;
		}
	}

	public ExpressionNode Right
	{
		get
		{
			return base.ChildrenList[1];
		}
		set
		{
			traceContext.TraceVerbose("Setting Right to: {0}", value);
			base.ChildrenList[1] = value;
		}
	}

	internal ExpressionNodeOperator()
	{
		SetNodeType(ExpressionNodeType.Operator);
		base.ChildrenList = new List<ExpressionNode>(2);
	}

	public override ExpressionNode DeepClone()
	{
		ExpressionNodeOperator expressionNodeOperator = new ExpressionNodeOperator();
		expressionNodeOperator.mOpType = mOpType;
		expressionNodeOperator.LastEvaluationResult = base.LastEvaluationResult;
		expressionNodeOperator.Tag = base.Tag;
		expressionNodeOperator.SetTypeClass(base.TypeClass);
		expressionNodeOperator.Add(Left.DeepClone());
		expressionNodeOperator.Add(Right.DeepClone());
		return expressionNodeOperator;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	private bool ValidOperandNode(ExpressionNode node)
	{
		if (node.Type != ExpressionNodeType.Constant && node.Type != ExpressionNodeType.Function)
		{
			return node.Type == ExpressionNodeType.Attribute;
		}
		return true;
	}

	protected override void SetProperties()
	{
		SetHasScript(Left.HasScript | Right.HasScript);
		SetFilterNodeCompatible(operatorsFilterNode.Contains(OpType));
		if (base.FilterNodeCompatible)
		{
			if (Left.Type == ExpressionNodeType.Group || Left.Type == ExpressionNodeType.Operator || Left.Type == ExpressionNodeType.Function)
			{
				SetFilterNodeCompatible(Left.FilterNodeCompatible);
			}
			if (base.FilterNodeCompatible && (Right.Type == ExpressionNodeType.Group || Right.Type == ExpressionNodeType.Operator || Right.Type == ExpressionNodeType.Function))
			{
				SetFilterNodeCompatible(Right.FilterNodeCompatible);
			}
		}
		NameConditionType nameConditionType = NameConditionType.None;
		switch (OpType)
		{
		default:
			return;
		case OperatorType.EQ:
			nameConditionType = NameConditionType.Equal;
			break;
		case OperatorType.LIKE:
			nameConditionType = NameConditionType.Like;
			break;
		case OperatorType.NE:
			nameConditionType = NameConditionType.NotEqual;
			break;
		case OperatorType.NOT_LIKE:
			nameConditionType = NameConditionType.NotLike;
			break;
		}
		if (Left.Type == ExpressionNodeType.Attribute && ((ExpressionNodeAttribute)Left).Name == "Name" && Right.Type == ExpressionNodeType.Constant && ((ExpressionNodeConstant)Right).Value.GetType() == typeof(string))
		{
			SetObjectName((string)((ExpressionNodeConstant)Right).Value);
			SetNameConditionType(nameConditionType);
		}
	}

	public ExpressionNodeOperator(OperatorType type, ExpressionNode left, ExpressionNode right)
	{
		traceContext.TraceMethodEnter("ExpressionNodeOperator");
		traceContext.TraceParameters(type);
		if (type == OperatorType.NONE)
		{
			throw new ArgumentException(ExceptionTemplatesSR.InvalidArgument(type.ToString()));
		}
		if (left == null || right == null)
		{
			throw new ArgumentNullException();
		}
		switch (type)
		{
		case OperatorType.AND:
		case OperatorType.OR:
			if ((left.Type != ExpressionNodeType.Operator && left.Type != ExpressionNodeType.Group) || (right.Type != ExpressionNodeType.Operator && right.Type != ExpressionNodeType.Group))
			{
				throw traceContext.TraceThrow(new OperatorNotApplicableException(type.ToString(), left.GetType().ToString()));
			}
			break;
		case OperatorType.IN:
		case OperatorType.NOT_IN:
		{
			if ((right.Type != ExpressionNodeType.Attribute || right.TypeClass != TypeClass.Array) && right.Type != ExpressionNodeType.Function)
			{
				throw traceContext.TraceThrow(new InvalidInOperatorException(OperatorTypeToString(type)));
			}
			if (!ValidOperandNode(left))
			{
				throw traceContext.TraceThrow(new InvalidOperandException(left.Type.ToString(), ExceptionTemplatesSR.LeftOperand));
			}
			if (right.Type != ExpressionNodeType.Function)
			{
				break;
			}
			ExpressionNodeFunction obj = (ExpressionNodeFunction)right;
			if (obj.FunctionType != ExpressionNodeFunction.Function.Array)
			{
				throw traceContext.TraceThrow(new InvalidInOperatorException(OperatorTypeToString(type)));
			}
			using (IEnumerator<ExpressionNode> enumerator = obj.EnumChildren().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					ExpressionNode current = enumerator.Current;
					if (current.TypeClass != 0 && left.TypeClass != 0 && current.TypeClass != left.TypeClass)
					{
						throw traceContext.TraceThrow(new ExpressionTypeMistmatchException(left.TypeClass.ToString(), current.TypeClass.ToString()));
					}
				}
			}
			break;
		}
		default:
			if (!ValidOperandNode(left))
			{
				throw traceContext.TraceThrow(new InvalidOperandException(left.GetType().Name, ExceptionTemplatesSR.LeftOperand));
			}
			if (!ValidOperandNode(right))
			{
				throw traceContext.TraceThrow(new InvalidOperandException(right.GetType().Name, ExceptionTemplatesSR.RightOperand));
			}
			if (left.TypeClass != 0)
			{
				if (right.TypeClass != 0 && right.TypeClass != TypeClass.Variant && left.TypeClass != TypeClass.Variant && right.TypeClass != left.TypeClass)
				{
					throw traceContext.TraceThrow(new ExpressionTypeMistmatchException(left.TypeClass.ToString(), right.TypeClass.ToString()));
				}
				if (!EvaluationFactory.SupportedOperators(left.TypeClass).Contains(type))
				{
					throw traceContext.TraceThrow(new OperatorNotApplicableException(type.ToString(), left.TypeClass.ToString()));
				}
			}
			else if (right.TypeClass != 0 && !EvaluationFactory.SupportedOperators(right.TypeClass).Contains(type))
			{
				throw traceContext.TraceThrow(new OperatorNotApplicableException(type.ToString(), right.TypeClass.ToString()));
			}
			break;
		}
		SetNodeType(ExpressionNodeType.Operator);
		mOpType = type;
		SetTypeClass(TypeClass.Bool);
		base.ChildrenList = new List<ExpressionNode>(2);
		base.ChildrenList.Add(left);
		base.ChildrenList.Add(right);
		SetProperties();
		traceContext.TraceMethodExit("ExpressionNodeOperator");
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public static string OperatorTypeToString(OperatorType type)
	{
		return type switch
		{
			OperatorType.AND => "AND", 
			OperatorType.EQ => "=", 
			OperatorType.GE => ">=", 
			OperatorType.GT => ">", 
			OperatorType.IN => "IN", 
			OperatorType.LE => "<=", 
			OperatorType.LIKE => "LIKE", 
			OperatorType.LT => "<", 
			OperatorType.NE => "!=", 
			OperatorType.OR => "OR", 
			OperatorType.NOT_IN => "NOT IN", 
			OperatorType.NOT_LIKE => "NOT LIKE", 
			_ => string.Empty, 
		};
	}

	public static OperatorType OperatorTypeFromString(string opType)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("OperatorTypeFromString", TraceEventType.Information);
		methodTraceContext.TraceParameters(opType);
		return opType switch
		{
			"AND" => OperatorType.AND, 
			"=" => OperatorType.EQ, 
			">=" => OperatorType.GE, 
			">" => OperatorType.GT, 
			"IN" => OperatorType.IN, 
			"<=" => OperatorType.LE, 
			"LIKE" => OperatorType.LIKE, 
			"<" => OperatorType.LT, 
			"!=" => OperatorType.NE, 
			"OR" => OperatorType.OR, 
			"NOT IN" => OperatorType.NOT_IN, 
			"NOT LIKE" => OperatorType.NOT_LIKE, 
			_ => throw methodTraceContext.TraceThrow(new ArgumentException("opType")), 
		};
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Left.ToString());
		stringBuilder.Append(" ");
		stringBuilder.Append(OperatorTypeToString(OpType));
		stringBuilder.Append(" ");
		stringBuilder.Append(Right.ToString());
		return stringBuilder.ToString();
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	protected override bool EqualProperties(object obj)
	{
		if (OpType == ((ExpressionNodeOperator)obj).OpType && Left.Equals(((ExpressionNodeOperator)obj).Left))
		{
			return Right.Equals(((ExpressionNodeOperator)obj).Right);
		}
		return false;
	}

	internal override void ResetResult()
	{
		base.LastEvaluationResult = null;
		base.ResetResult();
	}

	internal override object DoEvaluate(FacetEvaluationContext context, bool checkSqlScriptAsProxy)
	{
		if (mOpType == OperatorType.NONE)
		{
			throw traceContext.TraceThrow(new BadExpressionTreeException(ExceptionTemplatesSR.EvaluatingOperatorNone));
		}
		bool flag = false;
		object obj = Left.DoEvaluate(context, checkSqlScriptAsProxy);
		object obj2 = Right.DoEvaluate(context, checkSqlScriptAsProxy);
		if (obj.GetType().IsBitmappedEnum() || obj2.GetType().IsBitmappedEnum())
		{
			bool flag2 = false;
			bool flag3 = false;
			if (Left.Type == ExpressionNodeType.Constant || (Left.Type == ExpressionNodeType.Function && ((ExpressionNodeFunction)Left).FunctionType == ExpressionNodeFunction.Function.Enum))
			{
				flag2 = true;
			}
			if (Right.Type == ExpressionNodeType.Constant || (Right.Type == ExpressionNodeType.Function && ((ExpressionNodeFunction)Right).FunctionType == ExpressionNodeFunction.Function.Enum))
			{
				flag3 = true;
			}
			if (flag2 ^ flag3)
			{
				switch (mOpType)
				{
				case OperatorType.EQ:
					mOpType = OperatorType.BEQ;
					break;
				case OperatorType.NE:
					mOpType = OperatorType.BNE;
					break;
				}
				if (flag2)
				{
					ExpressionNode right = Right;
					Right = Left;
					Left = right;
				}
			}
		}
		flag = EvaluationFactory.Evaluate(obj, obj2, mOpType, context);
		base.LastEvaluationResult = flag;
		return flag;
	}

	internal override void AnalyzeForConfiguration(List<ConfigurationItem> configurationList)
	{
		if (GetResult() == null || (bool)GetResult())
		{
			return;
		}
		if (mOpType == OperatorType.OR)
		{
			throw traceContext.TraceThrow(new ExpressionNodeNotConfigurableException(OperatorTypeToString(mOpType)));
		}
		if (Left.Type == ExpressionNodeType.Attribute)
		{
			if (mOpType != OperatorType.EQ)
			{
				throw traceContext.TraceThrow(new ExpressionNodeNotConfigurableOperatorException(((ExpressionNodeAttribute)Left).Name, ToString()));
			}
			traceContext.DebugAssert(Right.Type == ExpressionNodeType.Constant || Right.Type == ExpressionNodeType.Function || Right.Type == ExpressionNodeType.Attribute, "Unsupported node structure");
			configurationList.Add(new ConfigurationItem(((ExpressionNodeAttribute)Left).Name, Right.GetResult()));
			return;
		}
		if (Left.Type == ExpressionNodeType.Operator || Left.Type == ExpressionNodeType.Group)
		{
			Left.AnalyzeForConfiguration(configurationList);
			traceContext.DebugAssert(Right.Type == ExpressionNodeType.Operator || Right.Type == ExpressionNodeType.Group, "Unsupported node structure");
			Right.AnalyzeForConfiguration(configurationList);
			return;
		}
		throw traceContext.TraceThrow(new ExpressionNodeNotConfigurableException());
	}

	protected override void SerializeProperties(XmlWriter xw, bool includeResult)
	{
		xw.WriteElementString("OpType", mOpType.ToString());
		if (includeResult)
		{
			SerializeResult(xw);
		}
		base.SerializeProperties(xw, includeResult);
	}

	protected override void DeserializeProperties(XmlReader xr, bool includeResult)
	{
		OperatorType operatorType = ExpressionNode.MatchType<OperatorType>(ExpressionNode.ReadNodeWithCheck(xr, "OpType")[0]);
		mOpType = operatorType;
		if (includeResult)
		{
			DeserializeResult(xr);
		}
		base.DeserializeProperties(xr, includeResult);
		SetProperties();
	}

	static ExpressionNodeOperator()
	{
		traceContext = TraceContext.GetTraceContext("DMF", "ExpressionNodeOperator");
		numericTypes = new List<Type>(7);
		stringTypes = new List<Type>(4);
		operatorsFilterNode = new List<OperatorType>(8);
		numericTypes.Add(typeof(int));
		numericTypes.Add(typeof(double));
		numericTypes.Add(typeof(byte));
		numericTypes.Add(typeof(long));
		numericTypes.Add(typeof(short));
		numericTypes.Add(typeof(float));
		numericTypes.Add(typeof(decimal));
		stringTypes.Add(typeof(string));
		stringTypes.Add(typeof(char));
		stringTypes.Add(typeof(DateTime));
		stringTypes.Add(typeof(Guid));
		operatorsFilterNode.Add(OperatorType.EQ);
		operatorsFilterNode.Add(OperatorType.GE);
		operatorsFilterNode.Add(OperatorType.GT);
		operatorsFilterNode.Add(OperatorType.LE);
		operatorsFilterNode.Add(OperatorType.LT);
		operatorsFilterNode.Add(OperatorType.NE);
		operatorsFilterNode.Add(OperatorType.AND);
		operatorsFilterNode.Add(OperatorType.OR);
		operatorsFilterNode.Add(OperatorType.LIKE);
		operatorsFilterNode.Add(OperatorType.NOT_LIKE);
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public static List<OperatorType> SupportedFilterOperators(Type type, AutomatedPolicyEvaluationMode mode)
	{
		return new List<OperatorType>(1) { OperatorType.EQ };
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	internal static OperatorType ConvertType(FilterNodeOperator.Type ftype)
	{
		switch (ftype)
		{
		case FilterNodeOperator.Type.And:
			return OperatorType.AND;
		case FilterNodeOperator.Type.EQ:
			return OperatorType.EQ;
		case FilterNodeOperator.Type.GE:
			return OperatorType.GE;
		case FilterNodeOperator.Type.GT:
			return OperatorType.GT;
		case FilterNodeOperator.Type.LE:
			return OperatorType.LE;
		case FilterNodeOperator.Type.LT:
			return OperatorType.LT;
		case FilterNodeOperator.Type.NE:
			return OperatorType.NE;
		case FilterNodeOperator.Type.OR:
			return OperatorType.OR;
		default:
			traceContext.DebugAssert(test: false, $"Converting FilterNode to ExpressionNode - Unrecognized OperatorType '{ftype.ToString()}'");
			return OperatorType.NONE;
		}
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	internal static FilterNodeOperator.Type ConvertType(OperatorType type)
	{
		return type switch
		{
			OperatorType.AND => FilterNodeOperator.Type.And, 
			OperatorType.EQ => FilterNodeOperator.Type.EQ, 
			OperatorType.GE => FilterNodeOperator.Type.GE, 
			OperatorType.GT => FilterNodeOperator.Type.GT, 
			OperatorType.LE => FilterNodeOperator.Type.LE, 
			OperatorType.LT => FilterNodeOperator.Type.LT, 
			OperatorType.NE => FilterNodeOperator.Type.NE, 
			OperatorType.OR => FilterNodeOperator.Type.OR, 
			_ => throw traceContext.TraceThrow(new ConversionNotSupportedException(ExceptionTemplatesSR.Operator, type.ToString())), 
		};
	}

	protected override FilterNode DoConvertToFilterNode()
	{
		if (!operatorsFilterNode.Contains(OpType))
		{
			throw traceContext.TraceThrow(new ConversionNotSupportedException(ExceptionTemplatesSR.Operator, OpType.ToString()));
		}
		switch (OpType)
		{
		case OperatorType.LIKE:
			return new FilterNodeFunction(FilterNodeFunction.Type.Like, "like", Left.ConvertToFilterNode(), Right.ConvertToFilterNode());
		case OperatorType.NOT_LIKE:
			return new FilterNodeFunction(FilterNodeFunction.Type.Not, "not", new FilterNodeFunction(FilterNodeFunction.Type.Like, "like", Left.ConvertToFilterNode(), Right.ConvertToFilterNode()));
		default:
		{
			FilterNodeOperator.Type opType = ConvertType(OpType);
			FilterNode left = Left.ConvertToFilterNode();
			FilterNode right = Right.ConvertToFilterNode();
			return new FilterNodeOperator(opType, left, right);
		}
		}
	}
}
