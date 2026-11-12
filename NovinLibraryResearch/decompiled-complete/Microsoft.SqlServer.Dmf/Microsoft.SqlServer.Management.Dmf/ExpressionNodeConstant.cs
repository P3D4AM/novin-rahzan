using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo.Internal;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class ExpressionNodeConstant : ExpressionNode
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "ExpressionNodeConstant");

	private const string cXmlTagObjType = "ObjType";

	private const string cXmlTagValue = "Value";

	private object mObject;

	public object Value => mObject;

	internal ExpressionNodeConstant()
	{
		SetNodeType(ExpressionNodeType.Constant);
	}

	public ExpressionNodeConstant(object obj)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ExpressionNodeConstant", TraceEventType.Information);
		methodTraceContext.TraceParameters(obj);
		if (obj == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("obj"));
		}
		Type type = obj.GetType();
		if (!EvaluationFactory.IsTypeSupportedForConstant(type) && !(obj is SqlSecureString))
		{
			throw methodTraceContext.TraceThrow(new UnsupportedTypeException(GetType().Name, type.ToString()));
		}
		if (obj is SqlSecureString)
		{
			obj = obj.ToString();
		}
		SetNodeType(ExpressionNodeType.Constant);
		mObject = obj;
		base.LastEvaluationResult = obj;
		SetTypeClass(EvaluationFactory.ClassifyType(obj));
	}

	public override string ToString()
	{
		if (Value == null)
		{
			return "NULL";
		}
		if (Value.GetType() == typeof(string))
		{
			return $"'{Urn.EscapeString(Value.ToString())}'";
		}
		return Value.ToString();
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public override string ToStringForDisplay()
	{
		return ToString();
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	protected override bool EqualProperties(object obj)
	{
		object right = ((ExpressionNodeConstant)obj).mObject;
		try
		{
			return EvaluationFactory.Evaluate(mObject, right, OperatorType.EQ);
		}
		catch (ExpressionTypeMistmatchException)
		{
			traceContext.TraceError("Caught a general Exception of type ExpressionTypeMistmatchException");
			return false;
		}
	}

	internal override void ResetResult()
	{
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	internal override object DoEvaluate(FacetEvaluationContext context, bool checkSqlScriptAsProxy)
	{
		return mObject;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	protected override void SerializeProperties(XmlWriter xw, bool includeResult)
	{
		if (mObject != null)
		{
			xw.WriteElementString("ObjType", mObject.GetType().ToString());
			string value = Urn.EscapeString(ExpressionNode.ConvertToString(mObject));
			xw.WriteElementString("Value", value);
		}
		else
		{
			xw.WriteElementString("ObjType", "NULL");
			xw.WriteElementString("Value", "NULL");
		}
	}

	protected override void DeserializeProperties(XmlReader xr, bool includeResult)
	{
		traceContext.TraceMethodEnter("DeserializeProperties");
		traceContext.TraceParameters(xr, includeResult);
		object obj = null;
		List<string> list = ExpressionNode.ReadSimpleNodeWithCheck(xr, base.Type, "ObjType", "Value");
		string text = list[0];
		string escapedValue = list[1];
		if (text != "NULL")
		{
			base.LastEvaluationResult = (mObject = ExpressionNode.ConvertFromString(text, Urn.UnEscapeString(escapedValue)));
		}
		else
		{
			mObject = null;
			base.LastEvaluationResult = null;
		}
		traceContext.TraceMethodExit("DeserializeProperties");
	}

	protected override FilterNode DoConvertToFilterNode()
	{
		FilterNode filterNode = null;
		Type type = Value.GetType();
		if (ExpressionNodeOperator.numericTypes.Contains(type))
		{
			return new FilterNodeConstant(Value, FilterNodeConstant.ObjectType.Number);
		}
		if (ExpressionNodeOperator.stringTypes.Contains(type))
		{
			return new FilterNodeConstant(Value.ToString(), FilterNodeConstant.ObjectType.String);
		}
		if (type == typeof(bool))
		{
			return new FilterNodeFunction((!(bool)Value) ? FilterNodeFunction.Type.False : FilterNodeFunction.Type.True, ((bool)Value) ? "TRUE" : "FALSE");
		}
		if (type.IsSubclassOf(typeof(Enum)))
		{
			return new FilterNodeConstant(Convert.ToInt32(Value), FilterNodeConstant.ObjectType.Number);
		}
		throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.TypeNotSupported(type.ToString())));
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public override ExpressionNode DeepClone()
	{
		return new ExpressionNodeConstant(Value);
	}
}
