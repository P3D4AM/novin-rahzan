using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class ExpressionNodeGroup : ExpressionNodeChildren
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "ExpressionNodeGroup");

	public ExpressionNode Group
	{
		get
		{
			return base.ChildrenList[0];
		}
		set
		{
			traceContext.TraceVerbose("Setting Group to: {0}", value);
			base.ChildrenList[0] = value;
		}
	}

	internal ExpressionNodeGroup()
	{
		SetNodeType(ExpressionNodeType.Group);
		base.ChildrenList = new List<ExpressionNode>(1);
	}

	public ExpressionNodeGroup(ExpressionNode node)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ExpressionNodeGroup", TraceEventType.Information);
		methodTraceContext.TraceParameters(node);
		if (node == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException());
		}
		SetNodeType(ExpressionNodeType.Group);
		SetTypeClass(node.TypeClass);
		base.ChildrenList = new List<ExpressionNode>(1);
		Add(node);
		SetProperties();
	}

	protected override void SetProperties()
	{
		SetHasScript(Group.HasScript);
		SetNameConditionType(Group.NameConditionType);
		SetFilterNodeCompatible(Group.FilterNodeCompatible);
	}

	internal override void ResetResult()
	{
		base.LastEvaluationResult = null;
		base.ResetResult();
	}

	internal override object DoEvaluate(FacetEvaluationContext context, bool checkSqlScriptAsProxy)
	{
		base.LastEvaluationResult = Group.DoEvaluate(context, checkSqlScriptAsProxy);
		return base.LastEvaluationResult;
	}

	protected override void DeserializeProperties(XmlReader xr, bool includeResult)
	{
		base.DeserializeProperties(xr, includeResult);
		SetProperties();
	}

	public override ExpressionNode DeepClone()
	{
		ExpressionNodeGroup expressionNodeGroup = new ExpressionNodeGroup();
		expressionNodeGroup.Add(Group.DeepClone());
		expressionNodeGroup.SetTypeClass(base.TypeClass);
		return expressionNodeGroup;
	}

	protected override FilterNode DoConvertToFilterNode()
	{
		return new FilterNodeGroup(Group.ConvertToFilterNode());
	}
}
