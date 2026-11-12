using System;
using System.Collections;

namespace Antlr.Runtime.Tree;

[Serializable]
public class RewriteRuleNodeStream : RewriteRuleElementStream
{
	public RewriteRuleNodeStream(ITreeAdaptor adaptor, string elementDescription)
		: base(adaptor, elementDescription)
	{
	}

	public RewriteRuleNodeStream(ITreeAdaptor adaptor, string elementDescription, object oneElement)
		: base(adaptor, elementDescription, oneElement)
	{
	}

	public RewriteRuleNodeStream(ITreeAdaptor adaptor, string elementDescription, IList elements)
		: base(adaptor, elementDescription, elements)
	{
	}

	public virtual object NextNode()
	{
		return NextCore();
	}

	protected override object ToTree(object el)
	{
		return adaptor.DupNode(el);
	}

	protected override object Dup(object el)
	{
		throw new NotSupportedException("dup can't be called for a node stream.");
	}
}
