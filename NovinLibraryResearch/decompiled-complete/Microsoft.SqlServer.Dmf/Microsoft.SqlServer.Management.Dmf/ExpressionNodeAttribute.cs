using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Xml;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo.Internal;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class ExpressionNodeAttribute : ExpressionNode
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "ExpressionNodeAttribute");

	private const string cXmlTagName = "Name";

	private string mName;

	public string Name => mName;

	internal ExpressionNodeAttribute()
	{
		SetNodeType(ExpressionNodeType.Attribute);
	}

	public ExpressionNodeAttribute(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException(name);
		}
		SetNodeType(ExpressionNodeType.Attribute);
		mName = name;
	}

	public ExpressionNodeAttribute(string name, Type facet)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ExpressionNodeAttribute", TraceEventType.Information);
		methodTraceContext.TraceParameters(name, facet);
		if (string.IsNullOrEmpty(name))
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("name"));
		}
		if (null == facet)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("facet"));
		}
		PropertyInfo[] facetProperties = FacetRepository.GetFacetProperties(facet);
		foreach (PropertyInfo propertyInfo in facetProperties)
		{
			if (propertyInfo.Name == name)
			{
				SetNodeType(ExpressionNodeType.Attribute);
				mName = name;
				SetTypeClass(EvaluationFactory.ClassifyType(propertyInfo.PropertyType));
				return;
			}
		}
		throw methodTraceContext.TraceThrow(new ObjectValidationException(ExceptionTemplatesSR.Property, name, new MissingPropertyException(name)));
	}

	public override string ToString()
	{
		return "@" + Name;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public override string ToStringForDisplay()
	{
		return ToString();
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	protected override bool EqualProperties(object obj)
	{
		if (Name == ((ExpressionNodeAttribute)obj).Name)
		{
			return base.TypeClass == ((ExpressionNodeAttribute)obj).TypeClass;
		}
		return false;
	}

	internal override object DoEvaluate(FacetEvaluationContext context, bool checkSqlScriptAsProxy)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("DoEvaluate");
		methodTraceContext.TraceParameters(context, checkSqlScriptAsProxy);
		if (context == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("context"));
		}
		base.LastEvaluationResult = context.GetPropertyValue(mName);
		if (base.LastEvaluationResult is SqlSecureString)
		{
			base.LastEvaluationResult = base.LastEvaluationResult.ToString();
		}
		methodTraceContext.TraceParameterOut("returnVal", base.LastEvaluationResult);
		return base.LastEvaluationResult;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	protected override void SerializeProperties(XmlWriter xw, bool includeResult)
	{
		xw.WriteElementString("Name", mName);
		if (includeResult)
		{
			SerializeResult(xw);
		}
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	protected override void DeserializeProperties(XmlReader xr, bool includeResult)
	{
		List<string> list;
		if (includeResult)
		{
			list = ExpressionNode.ReadNodeWithCheck(xr, "Name");
			DeserializeResult(xr);
			ReadEndElement(xr);
		}
		else
		{
			list = ExpressionNode.ReadSimpleNodeWithCheck(xr, base.Type, "Name");
		}
		mName = list[0];
	}

	protected override FilterNode DoConvertToFilterNode()
	{
		return new FilterNodeAttribute(Name);
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public override ExpressionNode DeepClone()
	{
		ExpressionNodeAttribute expressionNodeAttribute = new ExpressionNodeAttribute(Name);
		expressionNodeAttribute.LastEvaluationResult = base.LastEvaluationResult;
		expressionNodeAttribute.SetTypeClass(base.TypeClass);
		return expressionNodeAttribute;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	protected override void DoEnumAttributes(List<string> list)
	{
		list.Add(Name);
	}
}
