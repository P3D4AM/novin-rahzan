using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.SqlServer.Diagnostics.STrace;

namespace Microsoft.SqlServer.Management.Dmf;

public abstract class ExpressionNodeChildren : ExpressionNode
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "ExpressionNodeChildren");

	private const string cXmlTagCount = "Count";

	private List<ExpressionNode> childrenList;

	protected List<ExpressionNode> ChildrenList
	{
		get
		{
			return childrenList;
		}
		set
		{
			traceContext.TraceVerbose("Setting ChildrenList to: {0}", value);
			childrenList = value;
		}
	}

	public IEnumerable<ExpressionNode> EnumerableChildrenList => ChildrenList;

	public int Count => ChildrenList.Count;

	[STraceConfiguration(SkipAutoTrace = true)]
	public override IEnumerable<ExpressionNode> EnumChildren()
	{
		return EnumerableChildrenList;
	}

	protected virtual void Add(ExpressionNode node)
	{
		ChildrenList.Add(node);
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	protected override bool EqualProperties(object obj)
	{
		bool result = false;
		if (Count == ((ExpressionNodeChildren)obj).Count)
		{
			for (int i = 0; i < Count; i++)
			{
				if (!ChildrenList[i].Equals(((ExpressionNodeChildren)obj).ChildrenList[i]))
				{
					return false;
				}
			}
			result = true;
		}
		return result;
	}

	internal override void ResetResult()
	{
		foreach (ExpressionNode enumerableChildren in EnumerableChildrenList)
		{
			enumerableChildren.ResetResult();
		}
	}

	internal override void AnalyzeForConfiguration(List<ConfigurationItem> configurationList)
	{
		foreach (ExpressionNode enumerableChildren in EnumerableChildrenList)
		{
			enumerableChildren.AnalyzeForConfiguration(configurationList);
		}
	}

	protected override void DoEnumAttributes(List<string> list)
	{
		foreach (ExpressionNode children in ChildrenList)
		{
			if (children is ExpressionNodeChildren)
			{
				((ExpressionNodeChildren)children).DoEnumAttributes(list);
			}
			else if (children.Type == ExpressionNodeType.Attribute)
			{
				list.Add(((ExpressionNodeAttribute)children).Name);
			}
		}
	}

	internal override void DoEnumAttributeOperatorPairs(List<AttributeOperatorPair> list)
	{
		foreach (ExpressionNode children in ChildrenList)
		{
			if (children is ExpressionNodeChildren)
			{
				((ExpressionNodeChildren)children).DoEnumAttributeOperatorPairs(list);
			}
			else if (children.Type == ExpressionNodeType.Attribute && base.Type == ExpressionNodeType.Operator)
			{
				list.Add(new AttributeOperatorPair(((ExpressionNodeAttribute)children).Name, ((ExpressionNodeOperator)this).OpType));
			}
		}
		base.DoEnumAttributeOperatorPairs(list);
	}

	public override string ToString()
	{
		bool flag = true;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("(");
		foreach (ExpressionNode enumerableChildren in EnumerableChildrenList)
		{
			if (flag)
			{
				flag = false;
			}
			else
			{
				stringBuilder.Append(", ");
			}
			stringBuilder.Append(enumerableChildren.ToString());
		}
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	protected override void SerializeProperties(XmlWriter xw, bool includeResult)
	{
		xw.WriteElementString("Count", Count.ToString());
		foreach (ExpressionNode enumerableChildren in EnumerableChildrenList)
		{
			if (includeResult)
			{
				enumerableChildren.SerializeWithResult(xw);
			}
			else
			{
				enumerableChildren.Serialize(xw);
			}
		}
	}

	protected override void DeserializeProperties(XmlReader xr, bool includeResult)
	{
		traceContext.TraceMethodEnter("DeserializeProperties");
		traceContext.TraceParameters(xr, includeResult);
		int num = ExpressionNode.ConvertToIntWithCheck(ExpressionNode.ReadNodeWithCheck(xr, "Count")[0]);
		for (int i = 0; i < num; i++)
		{
			ExpressionNode node = (includeResult ? ExpressionNode.DeserializeWithResult(xr) : ExpressionNode.Deserialize(xr));
			Add(node);
		}
		ReadEndElement(xr);
		traceContext.TraceMethodExit("DeserializeProperties");
	}
}
