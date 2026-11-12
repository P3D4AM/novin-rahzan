using System;
using System.Collections;

namespace Antlr.Runtime.Tree;

[Serializable]
public class RewriteRuleSubtreeStream : RewriteRuleElementStream
{
	public RewriteRuleSubtreeStream(ITreeAdaptor adaptor, string elementDescription)
		: base(adaptor, elementDescription)
	{
	}

	public RewriteRuleSubtreeStream(ITreeAdaptor adaptor, string elementDescription, object oneElement)
		: base(adaptor, elementDescription, oneElement)
	{
	}

	public RewriteRuleSubtreeStream(ITreeAdaptor adaptor, string elementDescription, IList elements)
		: base(adaptor, elementDescription, elements)
	{
	}

	public virtual object NextNode()
	{
		int count = Count;
		if (dirty || (cursor >= count && count == 1))
		{
			object treeNode = NextCore();
			return adaptor.DupNode(treeNode);
		}
		object obj = NextCore();
		while (adaptor.IsNil(obj) && adaptor.GetChildCount(obj) == 1)
		{
			obj = adaptor.GetChild(obj, 0);
		}
		return adaptor.DupNode(obj);
	}

	protected override object Dup(object el)
	{
		return adaptor.DupTree(el);
	}
}
