using System;
using System.Collections;

namespace Antlr.Runtime.Tree;

[Serializable]
public class RewriteRuleTokenStream : RewriteRuleElementStream
{
	public RewriteRuleTokenStream(ITreeAdaptor adaptor, string elementDescription)
		: base(adaptor, elementDescription)
	{
	}

	public RewriteRuleTokenStream(ITreeAdaptor adaptor, string elementDescription, object oneElement)
		: base(adaptor, elementDescription, oneElement)
	{
	}

	public RewriteRuleTokenStream(ITreeAdaptor adaptor, string elementDescription, IList elements)
		: base(adaptor, elementDescription, elements)
	{
	}

	public virtual object NextNode()
	{
		IToken payload = (IToken)NextCore();
		return adaptor.Create(payload);
	}

	public virtual IToken NextToken()
	{
		return (IToken)NextCore();
	}

	protected override object ToTree(object el)
	{
		return el;
	}

	protected override object Dup(object el)
	{
		throw new NotSupportedException("dup can't be called for a token stream.");
	}
}
