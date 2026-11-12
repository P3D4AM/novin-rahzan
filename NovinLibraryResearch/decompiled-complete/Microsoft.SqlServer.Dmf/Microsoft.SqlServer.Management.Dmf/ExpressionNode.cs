using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Server;

namespace Microsoft.SqlServer.Management.Dmf;

public abstract class ExpressionNode
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "ExpressionNode");

	private const string cXmlTagResultObjType = "ResultObjType";

	private const string cXmlTagResultValue = "ResultValue";

	private const string cXmlTagCount = "Count";

	private const string cXmlTagTypeClass = "TypeClass";

	private ExpressionNodeType nodeType;

	private string tag = string.Empty;

	private TypeClass typeClass;

	private object lastEvaluationResult;

	private NameConditionType nameConditionType;

	private string objectName;

	private bool hasScript;

	private bool filterNodeCompatible;

	protected object LastEvaluationResult
	{
		get
		{
			return lastEvaluationResult;
		}
		set
		{
			traceContext.TraceVerbose("Setting LastEvaluationResult to: {0}", value);
			lastEvaluationResult = value;
		}
	}

	public ExpressionNodeType Type => nodeType;

	public string Tag
	{
		get
		{
			return tag;
		}
		set
		{
			traceContext.TraceVerbose("Setting Tag to: {0}", value);
			tag = value;
		}
	}

	public TypeClass TypeClass => typeClass;

	internal NameConditionType NameConditionType => nameConditionType;

	internal string ObjectName => objectName;

	internal bool HasScript => hasScript;

	internal bool FilterNodeCompatible => filterNodeCompatible;

	protected void SetNodeType(ExpressionNodeType value)
	{
		traceContext.TraceMethodEnter("SetNodeType");
		traceContext.TraceParameters(value);
		nodeType = value;
		traceContext.TraceMethodExit("SetNodeType");
	}

	protected void SetTypeClass(TypeClass value)
	{
		traceContext.TraceMethodEnter("SetTypeClass");
		traceContext.TraceParameters(value);
		typeClass = value;
		traceContext.TraceMethodExit("SetTypeClass");
	}

	protected void SetNameConditionType(NameConditionType value)
	{
		traceContext.TraceMethodEnter("SetNameConditionType");
		traceContext.TraceParameters(value);
		nameConditionType = value;
		traceContext.TraceMethodExit("SetNameConditionType");
	}

	protected void SetObjectName(string value)
	{
		traceContext.TraceMethodEnter("SetObjectName");
		traceContext.TraceParameters(value);
		objectName = value;
		traceContext.TraceMethodExit("SetObjectName");
	}

	protected void SetHasScript(bool value)
	{
		traceContext.TraceMethodEnter("SetHasScript");
		traceContext.TraceParameters(value);
		hasScript = value;
		traceContext.TraceMethodExit("SetHasScript");
	}

	protected void SetFilterNodeCompatible(bool value)
	{
		traceContext.TraceMethodEnter("SetFilterNodeCompatible");
		traceContext.TraceParameters(value);
		filterNodeCompatible = value;
		traceContext.TraceMethodExit("SetFilterNodeCompatible");
	}

	protected virtual void SetProperties()
	{
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public virtual ExpressionNode DeepClone()
	{
		return null;
	}

	internal virtual void ResetResult()
	{
		LastEvaluationResult = null;
	}

	public object Evaluate(FacetEvaluationContext context)
	{
		return Evaluate(context, checkSqlScriptAsProxy: false);
	}

	public object Evaluate(FacetEvaluationContext context, bool checkSqlScriptAsProxy)
	{
		ResetResult();
		return DoEvaluate(context, checkSqlScriptAsProxy);
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	internal virtual object DoEvaluate(FacetEvaluationContext context, bool checkSqlScriptAsProxy)
	{
		return null;
	}

	internal virtual void AnalyzeForConfiguration(List<ConfigurationItem> configurationList)
	{
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public object GetResult()
	{
		return LastEvaluationResult;
	}

	public string GetResultString()
	{
		if (LastEvaluationResult == null)
		{
			return string.Empty;
		}
		if (LastEvaluationResult is Array)
		{
			StringBuilder stringBuilder = new StringBuilder("{");
			foreach (object item in LastEvaluationResult as Array)
			{
				stringBuilder.Append(item.ToString() + ",");
			}
			if (((Array)LastEvaluationResult).Length > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}
		if (LastEvaluationResult.GetType() == typeof(string))
		{
			return "'" + LastEvaluationResult.ToString() + "'";
		}
		return LastEvaluationResult.ToString();
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public virtual IEnumerable<ExpressionNode> EnumChildren()
	{
		return new List<ExpressionNode>(0);
	}

	internal IEnumerable<string> EnumAttributes()
	{
		List<string> list = new List<string>();
		DoEnumAttributes(list);
		return list;
	}

	protected virtual void DoEnumAttributes(List<string> list)
	{
	}

	internal List<AttributeOperatorPair> EnumAttributeOperatorPairs()
	{
		List<AttributeOperatorPair> list = new List<AttributeOperatorPair>();
		DoEnumAttributeOperatorPairs(list);
		return list;
	}

	internal virtual void DoEnumAttributeOperatorPairs(List<AttributeOperatorPair> list)
	{
	}

	public override string ToString()
	{
		return string.Empty;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public virtual string ToStringForDisplay()
	{
		return null;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public override bool Equals(object obj)
	{
		bool result = false;
		if (obj == null)
		{
			return false;
		}
		if (obj is ExpressionNode && Type == ((ExpressionNode)obj).Type)
		{
			result = EqualProperties(obj);
		}
		return result;
	}

	protected abstract bool EqualProperties(object obj);

	[STraceConfiguration(SkipAutoTrace = true)]
	public void Serialize(XmlWriter xmlWriter)
	{
		if (xmlWriter == null)
		{
			throw new ArgumentNullException("xmlWriter");
		}
		xmlWriter.WriteStartElement(nodeType.ToString());
		xmlWriter.WriteElementString("TypeClass", typeClass.ToString());
		SerializeProperties(xmlWriter, includeResult: false);
		xmlWriter.WriteEndElement();
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	internal void SerializeWithResult(XmlWriter xw)
	{
		xw.WriteStartElement(nodeType.ToString());
		xw.WriteElementString("TypeClass", typeClass.ToString());
		SerializeProperties(xw, includeResult: true);
		xw.WriteEndElement();
	}

	protected virtual void SerializeProperties(XmlWriter xw, bool includeResult)
	{
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	protected void SerializeResult(XmlWriter xw)
	{
		if (LastEvaluationResult == null)
		{
			xw.WriteElementString("ResultObjType", "NULL");
			xw.WriteElementString("ResultValue", "NULL");
			return;
		}
		xw.WriteElementString("ResultObjType", LastEvaluationResult.GetType().ToString());
		if (LastEvaluationResult is Array)
		{
			xw.WriteStartElement("ResultValue");
			Array array = (Array)LastEvaluationResult;
			xw.WriteElementString("Count", array.Length.ToString());
			foreach (object item in array)
			{
				xw.WriteElementString("ResultObjType", item.GetType().ToString());
				xw.WriteElementString("ResultValue", ConvertToString(item));
			}
			xw.WriteEndElement();
		}
		else
		{
			xw.WriteElementString("ResultValue", ConvertToString(LastEvaluationResult));
		}
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	protected void DeserializeResult(XmlReader xr)
	{
		object obj = null;
		string text = ReadElementWithCheck(xr, "ResultObjType");
		if (text.EndsWith("[]", StringComparison.Ordinal))
		{
			ReadWithCheck(xr, XmlNodeType.Element, "ResultValue");
			string value = ReadElementWithCheck(xr, "Count");
			int num = Convert.ToInt32(value);
			object[] array = new object[num];
			for (int i = 0; i < num; i++)
			{
				List<string> list = ReadNodeWithCheck(xr, "ResultObjType", "ResultValue");
				text = list[0];
				value = list[1];
				array[i] = ConvertFromString(text, value);
			}
			ReadWithCheck(xr, XmlNodeType.EndElement, "ResultValue");
			LastEvaluationResult = array;
		}
		else
		{
			string value = ReadElementWithCheck(xr, "ResultValue");
			obj = ConvertFromString(text, value);
			LastEvaluationResult = obj;
		}
	}

	protected virtual void DeserializeProperties(XmlReader xr, bool includeResult)
	{
	}

	protected void ReadEndElement(XmlReader xr)
	{
		ReadWithCheck(xr, XmlNodeType.EndElement, Type.ToString());
	}

	protected static void ReadWithCheck(XmlReader xr, XmlNodeType nodeType, string name)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ReadWithCheck");
		methodTraceContext.TraceParameters(nodeType, name);
		xr.Read();
		if (xr.NodeType != nodeType || (!string.IsNullOrEmpty(name) && !(xr.Name == name)))
		{
			throw methodTraceContext.TraceThrow(new ExpressionSerializationException(xr.NodeType.ToString(), xr.Name, nodeType.ToString(), name));
		}
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	protected static void MoveToElementWithCheck(XmlReader xr, string name)
	{
		while (xr.Read())
		{
			if (xr.NodeType == XmlNodeType.Element)
			{
				if (string.IsNullOrEmpty(name) || xr.Name == name)
				{
					return;
				}
				continue;
			}
			throw traceContext.TraceThrow(new ExpressionSerializationException(xr.Name, name));
		}
		throw traceContext.TraceThrow(new ExpressionSerializationException(string.Empty, name));
	}

	protected static string ReadElementWithCheck(XmlReader xr, string name)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ReadElementWithCheck");
		methodTraceContext.TraceParameters(xr, name);
		MoveToElementWithCheck(xr, name);
		string value;
		if (!xr.IsEmptyElement)
		{
			ReadWithCheck(xr, XmlNodeType.Text, null);
			value = xr.Value;
			ReadWithCheck(xr, XmlNodeType.EndElement, name);
		}
		else
		{
			value = xr.Value;
		}
		methodTraceContext.TraceParameterOut("returnVal", value);
		return value;
	}

	protected static List<string> ReadNodeWithCheck(XmlReader xr, params string[] elements)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ReadNodeWithCheck");
		methodTraceContext.TraceParameters(xr, elements);
		List<string> list = new List<string>();
		foreach (string name in elements)
		{
			string item = ReadElementWithCheck(xr, name);
			list.Add(item);
		}
		methodTraceContext.TraceParameterOut("returnVal", list);
		return list;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	protected static List<string> ReadSimpleNodeWithCheck(XmlReader xr, ExpressionNodeType type, params string[] elements)
	{
		List<string> list = new List<string>();
		foreach (string name in elements)
		{
			string item = ReadElementWithCheck(xr, name);
			list.Add(item);
		}
		ReadWithCheck(xr, XmlNodeType.EndElement, type.ToString());
		return list;
	}

	public static ExpressionNode Deserialize(string value)
	{
		XmlReader xmlReader = XmlReader.Create(new StringReader(value), new XmlReaderSettings
		{
			IgnoreWhitespace = true
		});
		ExpressionNode result = Deserialize(xmlReader);
		xmlReader.Close();
		return result;
	}

	public static ExpressionNode DeserializeWithResult(string value)
	{
		XmlReader xmlReader = XmlReader.Create(new StringReader(value), new XmlReaderSettings
		{
			IgnoreWhitespace = true
		});
		ExpressionNode result = DeserializeWithResult(xmlReader);
		xmlReader.Close();
		return result;
	}

	public static ExpressionNode Deserialize(XmlReader xr)
	{
		return DoDeserialize(xr, includeResult: false);
	}

	internal static ExpressionNode DeserializeWithResult(XmlReader xr)
	{
		return DoDeserialize(xr, includeResult: true);
	}

	private static ExpressionNode DoDeserialize(XmlReader xr, bool includeResult)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("DoDeserialize");
		methodTraceContext.TraceParameters(xr, includeResult);
		ExpressionNode expressionNode = null;
		MoveToElementWithCheck(xr, null);
		switch (MatchType<ExpressionNodeType>(xr.Name))
		{
		case ExpressionNodeType.Attribute:
			expressionNode = new ExpressionNodeAttribute();
			break;
		case ExpressionNodeType.Constant:
			expressionNode = new ExpressionNodeConstant();
			break;
		case ExpressionNodeType.Operator:
			expressionNode = new ExpressionNodeOperator();
			break;
		case ExpressionNodeType.Group:
			expressionNode = new ExpressionNodeGroup();
			break;
		case ExpressionNodeType.Function:
			expressionNode = new ExpressionNodeFunction();
			break;
		default:
			traceContext.DebugAssert(test: false, "ExpressionTree Deserialize - Unknown Node Type");
			break;
		}
		if (expressionNode != null)
		{
			string value = ReadElementWithCheck(xr, "TypeClass");
			expressionNode.SetTypeClass(MatchType<TypeClass>(value));
			expressionNode.DeserializeProperties(xr, includeResult);
			methodTraceContext.TraceParameterOut("returnVal", expressionNode);
			return expressionNode;
		}
		methodTraceContext.TraceParameterOut("returnVal", null);
		return null;
	}

	public static string SerializeNode(ExpressionNode node)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("SerializeNode", TraceEventType.Information);
		methodTraceContext.TraceParameters(node);
		if (node == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("node"));
		}
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings
		{
			Indent = true,
			OmitXmlDeclaration = true,
			NewLineOnAttributes = true
		});
		node.Serialize(xmlWriter);
		xmlWriter.Close();
		return stringBuilder.ToString();
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public static string SerializeNodeWithResult(ExpressionNode node)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings
		{
			Indent = true,
			OmitXmlDeclaration = true,
			NewLineOnAttributes = true
		});
		node.SerializeWithResult(xmlWriter);
		xmlWriter.Close();
		return stringBuilder.ToString();
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public static T MatchType<T>(string value)
	{
		return (T)typeof(T).GetField(value).GetValue(null);
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	protected static string ConvertToString(object value)
	{
		if (value == null)
		{
			return "NULL";
		}
		if (value.GetType() == typeof(DateTime))
		{
			return SfcSecureString.XmlEscape(((DateTime)value).ToBinary().ToString());
		}
		return SfcSecureString.XmlEscape(value.ToString());
	}

	protected static object ResolveEnum(string stringObjType, string stringValue)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ResolveEnum");
		methodTraceContext.TraceParameters(stringObjType, stringValue);
		object obj = null;
		Type type = System.Type.GetType(stringObjType, throwOnError: false);
		if (null == type)
		{
			type = typeof(Microsoft.SqlServer.Management.Smo.Server).Assembly.GetType(stringObjType, throwOnError: false);
		}
		if (null == type)
		{
			type = typeof(LoginType).Assembly.GetType(stringObjType, throwOnError: false);
		}
		if (null == type)
		{
			type = Assembly.Load(typeof(LoginType).Assembly.FullName.Replace("SqlEnum", "ServiceBrokerEnum")).GetType(stringObjType, throwOnError: false);
		}
		if (null == type)
		{
			type = typeof(DatabaseEngineType).Assembly.GetType(stringObjType, throwOnError: false);
		}
		if (null == type && !SqlContext.IsAvailable)
		{
			string[] array = Assembly.GetExecutingAssembly().FullName.Split(new char[1] { ',' }, 2);
			type = Assembly.Load("Microsoft.SqlServer.Dmf.Adapters, " + array[1]).GetType(stringObjType, throwOnError: false);
		}
		if (null != type && type.IsEnum)
		{
			obj = Enum.Parse(type, stringValue);
		}
		methodTraceContext.TraceParameterOut("returnVal", obj);
		return obj;
	}

	protected static object ConvertFromString(string stringObjType, string stringValue)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ConvertFromString");
		methodTraceContext.TraceParameters(stringObjType, stringValue);
		object obj = null;
		string text = SfcSecureString.XmlUnEscape(stringValue);
		try
		{
			switch (stringObjType)
			{
			case "System.String":
				obj = text;
				break;
			case "System.Int32":
				obj = Convert.ToInt32(text);
				break;
			case "System.Boolean":
				obj = Convert.ToBoolean(text);
				break;
			case "System.Double":
				obj = Convert.ToDouble(text);
				break;
			case "System.Int64":
				obj = Convert.ToInt64(text);
				break;
			case "System.Int16":
				obj = Convert.ToInt16(text);
				break;
			case "System.Byte":
				obj = Convert.ToByte(text);
				break;
			case "System.DateTime":
				obj = DateTime.FromBinary(Convert.ToInt64(text));
				break;
			case "System.Guid":
				obj = new Guid(text);
				break;
			case "System.Decimal":
				obj = Convert.ToDecimal(text);
				break;
			case "System.Char":
				obj = Convert.ToChar(text);
				break;
			case "System.Single":
				obj = Convert.ToSingle(text);
				break;
			default:
				obj = ResolveEnum(stringObjType, text);
				if (obj == null)
				{
					throw methodTraceContext.TraceThrow(new UnsupportedTypeException(typeof(ExpressionNodeConstant).Name, stringObjType));
				}
				break;
			case "NULL":
				break;
			}
		}
		catch (FormatException ex)
		{
			methodTraceContext.TraceCatch(ex);
			throw methodTraceContext.TraceThrow(new TypeConversionException(text, stringObjType, ex));
		}
		methodTraceContext.TraceParameterOut("returnVal", obj);
		return obj;
	}

	protected static int ConvertToIntWithCheck(string value)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ConvertToIntWithCheck");
		methodTraceContext.TraceParameters(value);
		int num;
		try
		{
			num = Convert.ToInt32(value);
		}
		catch (FormatException ex)
		{
			methodTraceContext.TraceCatch(ex);
			throw methodTraceContext.TraceThrow(new TypeConversionException(value, typeof(int).ToString(), ex));
		}
		methodTraceContext.TraceParameterOut("returnVal", num);
		return num;
	}

	public static ExpressionNode Parse(string input)
	{
		return Parse(input, null);
	}

	public static ExpressionNode Parse(string input, Type facet)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("Parse", TraceEventType.Information);
		methodTraceContext.TraceParameters(input, facet);
		if (input == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("input"));
		}
		if (string.IsNullOrEmpty(input.Trim()))
		{
			throw methodTraceContext.TraceThrow(new ArgumentException(ExceptionTemplatesSR.ArgumentNullOrEmpty("input")));
		}
		Urn urn = new Urn($"FakeLevel[{input}]");
		try
		{
			_ = urn.XPathExpression;
		}
		catch (XPathException ex)
		{
			methodTraceContext.TraceCatch(ex);
			CheckForDateFunctions(input);
			throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.ParsingError(input), ex));
		}
		catch (InvalidQueryExpressionEnumeratorException ex2)
		{
			methodTraceContext.TraceCatch(ex2);
			CheckForDateFunctions(input);
			throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.ParsingError(input), ex2));
		}
		ExpressionNode expressionNode = ConvertFromFilterNode(urn.XPathExpression[0].Filter, facet);
		methodTraceContext.TraceParameterOut("returnVal", expressionNode);
		return expressionNode;
	}

	private static void CheckForDateFunctions(string input)
	{
		if (new Regex("(DATEADD|DATEPART)\\(\\s*(YEAR|Y|YY|YYYY|MONTH|MM|M|DAYOFYEAR|DY|DAY|DD|D|WEEKDAY|DW|HOUR|HH|MINUTE|MI|N|SECOND|SS|S|MILLISECOND|MS)\\s*\\,", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant).IsMatch(input))
		{
			throw new DmfException(ExceptionTemplatesSR.ParsingUnquotedDatePart(input));
		}
	}

	public static ExpressionNode ConvertFromFilterNode(FilterNode filterNode)
	{
		return ConvertFromFilterNode(filterNode, null);
	}

	public static ExpressionNode ConvertFromFilterNode(FilterNode filterNode, Type facet)
	{
		ExpressionNode result = null;
		if (filterNode == null)
		{
			return null;
		}
		switch (filterNode.NodeType)
		{
		case FilterNode.Type.Attribute:
		{
			FilterNodeAttribute filterNodeAttribute = (FilterNodeAttribute)filterNode;
			result = ((!(null == facet)) ? new ExpressionNodeAttribute(filterNodeAttribute.Name, facet) : new ExpressionNodeAttribute(filterNodeAttribute.Name));
			break;
		}
		case FilterNode.Type.Constant:
			result = new ExpressionNodeConstant(((FilterNodeConstant)filterNode).Value);
			break;
		case FilterNode.Type.Function:
		{
			FilterNodeFunction filterNodeFunction = (FilterNodeFunction)filterNode;
			switch (filterNodeFunction.FunctionType)
			{
			case FilterNodeFunction.Type.True:
				result = new ExpressionNodeFunction(ExpressionNodeFunction.Function.True);
				break;
			case FilterNodeFunction.Type.False:
				result = new ExpressionNodeFunction(ExpressionNodeFunction.Function.False);
				break;
			case FilterNodeFunction.Type.Like:
			{
				ExpressionNode left = ConvertFromFilterNode(filterNodeFunction.GetParameter(0), facet);
				ExpressionNode right = ConvertFromFilterNode(filterNodeFunction.GetParameter(1), facet);
				result = new ExpressionNodeOperator(OperatorType.LIKE, left, right);
				break;
			}
			case FilterNodeFunction.Type.String:
			{
				ExpressionNode expressionNode4 = ConvertFromFilterNode(filterNodeFunction.GetParameter(0), facet);
				result = new ExpressionNodeFunction(ExpressionNodeFunction.Function.String, expressionNode4);
				break;
			}
			case FilterNodeFunction.Type.UserDefined:
			{
				string name = filterNodeFunction.Name;
				int parameterCount = filterNodeFunction.ParameterCount;
				ExpressionNode[] array = new ExpressionNode[parameterCount];
				ExpressionNodeFunction.Function functionType = (ExpressionNodeFunction.Function)Enum.Parse(typeof(ExpressionNodeFunction.Function), name, ignoreCase: true);
				for (int i = 0; i < parameterCount; i++)
				{
					array[i] = ConvertFromFilterNode(filterNodeFunction.GetParameter(i), facet);
				}
				result = new ExpressionNodeFunction(functionType, array);
				break;
			}
			default:
				throw traceContext.TraceThrow(new ConversionNotSupportedException(ExceptionTemplatesSR.Function, filterNodeFunction.FunctionType.ToString()));
			}
			break;
		}
		case FilterNode.Type.Operator:
		{
			FilterNodeOperator filterNodeOperator = (FilterNodeOperator)filterNode;
			if (filterNodeOperator.OpType == FilterNodeOperator.Type.NEG)
			{
				ExpressionNode expressionNode = ConvertFromFilterNode(filterNodeOperator.Left, facet);
				if (expressionNode.Type == ExpressionNodeType.Constant && expressionNode.TypeClass == TypeClass.Numeric)
				{
					result = new ExpressionNodeConstant(-1.0 * (double)expressionNode.LastEvaluationResult);
					break;
				}
				throw traceContext.TraceThrow(new ConversionNotSupportedException(ExceptionTemplatesSR.Operator, filterNodeOperator.OpType.ToString()));
			}
			ExpressionNode expressionNode2 = ConvertFromFilterNode(filterNodeOperator.Left, facet);
			ExpressionNode expressionNode3 = ConvertFromFilterNode(filterNodeOperator.Right, facet);
			OperatorType operatorType = ExpressionNodeOperator.ConvertType(filterNodeOperator.OpType);
			if (expressionNode2 != null && expressionNode3 != null && operatorType != 0)
			{
				result = new ExpressionNodeOperator(operatorType, expressionNode2, expressionNode3);
			}
			break;
		}
		case FilterNode.Type.Group:
			result = new ExpressionNodeGroup(ConvertFromFilterNode(((FilterNodeGroup)filterNode).Node, facet));
			break;
		default:
			traceContext.DebugAssert(test: false, $"Converting FilterNode to ExpressionNode - Unsupported FilterNode type '{filterNode.NodeType.ToString()}'");
			break;
		}
		return result;
	}

	public FilterNode ConvertToFilterNode()
	{
		return DoConvertToFilterNode();
	}

	protected virtual FilterNode DoConvertToFilterNode()
	{
		traceContext.DebugAssert(test: false, $"Converting ExpressionNode to FilterNode - conversion of {Type.ToString()} is not supported");
		return null;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public static ExpressionNode ConstructNode(object obj)
	{
		if (obj == null)
		{
			throw traceContext.TraceThrow(new ArgumentNullException("obj"));
		}
		if (!EvaluationFactory.IsTypeSupported(obj.GetType()))
		{
			throw traceContext.TraceThrow(new UnsupportedObjectTypeException(obj.GetType().ToString(), typeof(ExpressionNode).ToString()));
		}
		switch (EvaluationFactory.ClassifyType(obj))
		{
		case TypeClass.Numeric:
			if (obj.GetType().IsEnum)
			{
				return new ExpressionNodeFunction(ExpressionNodeFunction.Function.Enum, new ExpressionNodeConstant(obj.GetType().ToString()), new ExpressionNodeConstant(obj.ToString()));
			}
			return new ExpressionNodeConstant(obj);
		case TypeClass.String:
			return new ExpressionNodeConstant(obj);
		case TypeClass.DateTime:
			return new ExpressionNodeFunction(ExpressionNodeFunction.Function.DateTime, new ExpressionNodeConstant(((DateTime)obj).ToString("o")));
		case TypeClass.Bool:
			if (!Evaluator.ConvertToBool(obj))
			{
				return new ExpressionNodeFunction(ExpressionNodeFunction.Function.False);
			}
			return new ExpressionNodeFunction(ExpressionNodeFunction.Function.True);
		case TypeClass.Guid:
			return new ExpressionNodeFunction(ExpressionNodeFunction.Function.Guid, new ExpressionNodeConstant(obj.ToString()));
		case TypeClass.Array:
		{
			Array array = (Array)obj;
			ExpressionNode[] array2 = new ExpressionNode[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = new ExpressionNodeConstant(array.GetValue(i));
			}
			return new ExpressionNodeFunction(ExpressionNodeFunction.Function.Array, array2);
		}
		case TypeClass.BitmappedEnum:
			return new ExpressionNodeFunction(ExpressionNodeFunction.Function.Enum, new ExpressionNodeConstant(obj.GetType().ToString()), new ExpressionNodeConstant(obj.ToString()));
		default:
			traceContext.DebugAssert(test: false, "Unable construct node!");
			return null;
		}
	}

	public string ToStringForUrn()
	{
		return DoConvertToFilterNode().ToString();
	}
}
