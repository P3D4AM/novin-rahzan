using System;
using System.Collections;
using System.Collections.Generic;

namespace Antlr.Runtime.Tree;

[Serializable]
public abstract class RewriteRuleElementStream
{
	protected int cursor;

	protected object singleElement;

	protected IList elements;

	protected bool dirty;

	protected string elementDescription;

	protected ITreeAdaptor adaptor;

	public virtual bool HasNext
	{
		get
		{
			if (singleElement == null || cursor >= 1)
			{
				if (elements != null)
				{
					return cursor < elements.Count;
				}
				return false;
			}
			return true;
		}
	}

	public virtual int Count
	{
		get
		{
			int result = 0;
			if (singleElement != null)
			{
				result = 1;
			}
			if (elements != null)
			{
				return elements.Count;
			}
			return result;
		}
	}

	public virtual string Description => elementDescription;

	public RewriteRuleElementStream(ITreeAdaptor adaptor, string elementDescription)
	{
		this.elementDescription = elementDescription;
		this.adaptor = adaptor;
	}

	public RewriteRuleElementStream(ITreeAdaptor adaptor, string elementDescription, object oneElement)
		: this(adaptor, elementDescription)
	{
		Add(oneElement);
	}

	public RewriteRuleElementStream(ITreeAdaptor adaptor, string elementDescription, IList elements)
		: this(adaptor, elementDescription)
	{
		singleElement = null;
		this.elements = elements;
	}

	public virtual void Reset()
	{
		cursor = 0;
		dirty = true;
	}

	public virtual void Add(object el)
	{
		if (el != null)
		{
			if (elements != null)
			{
				elements.Add(el);
				return;
			}
			if (singleElement == null)
			{
				singleElement = el;
				return;
			}
			elements = new List<object>(5);
			elements.Add(singleElement);
			singleElement = null;
			elements.Add(el);
		}
	}

	public virtual object NextTree()
	{
		int count = Count;
		if (dirty || (cursor >= count && count == 1))
		{
			object el = NextCore();
			return Dup(el);
		}
		return NextCore();
	}

	protected virtual object NextCore()
	{
		int count = Count;
		if (count == 0)
		{
			throw new RewriteEmptyStreamException(elementDescription);
		}
		if (cursor >= count)
		{
			if (count == 1)
			{
				return ToTree(singleElement);
			}
			throw new RewriteCardinalityException(elementDescription);
		}
		if (singleElement != null)
		{
			cursor++;
			return ToTree(singleElement);
		}
		object result = ToTree(elements[cursor]);
		cursor++;
		return result;
	}

	protected abstract object Dup(object el);

	protected virtual object ToTree(object el)
	{
		return el;
	}
}
