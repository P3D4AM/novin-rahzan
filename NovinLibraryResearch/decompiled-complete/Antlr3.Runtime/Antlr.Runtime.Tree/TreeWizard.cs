using System;
using System.Collections;
using System.Collections.Generic;

namespace Antlr.Runtime.Tree;

public class TreeWizard
{
	public interface IContextVisitor
	{
		void Visit(object t, object parent, int childIndex, IDictionary<string, object> labels);
	}

	public abstract class Visitor : IContextVisitor
	{
		public virtual void Visit(object t, object parent, int childIndex, IDictionary<string, object> labels)
		{
			Visit(t);
		}

		public abstract void Visit(object t);
	}

	private class ActionVisitor : Visitor
	{
		private Action<object> _action;

		public ActionVisitor(Action<object> action)
		{
			_action = action;
		}

		public override void Visit(object t)
		{
			_action(t);
		}
	}

	public class TreePattern : CommonTree
	{
		public string label;

		public bool hasTextArg;

		public TreePattern(IToken payload)
			: base(payload)
		{
		}

		public override string ToString()
		{
			if (label != null)
			{
				return "%" + label + ":";
			}
			return base.ToString();
		}
	}

	public class WildcardTreePattern : TreePattern
	{
		public WildcardTreePattern(IToken payload)
			: base(payload)
		{
		}
	}

	public class TreePatternTreeAdaptor : CommonTreeAdaptor
	{
		public override object Create(IToken payload)
		{
			return new TreePattern(payload);
		}
	}

	private class FindTreeWizardVisitor : Visitor
	{
		private IList _nodes;

		public FindTreeWizardVisitor(IList nodes)
		{
			_nodes = nodes;
		}

		public override void Visit(object t)
		{
			_nodes.Add(t);
		}
	}

	private class FindTreeWizardContextVisitor : IContextVisitor
	{
		private TreeWizard _outer;

		private TreePattern _tpattern;

		private IList _subtrees;

		public FindTreeWizardContextVisitor(TreeWizard outer, TreePattern tpattern, IList subtrees)
		{
			_outer = outer;
			_tpattern = tpattern;
			_subtrees = subtrees;
		}

		public void Visit(object t, object parent, int childIndex, IDictionary<string, object> labels)
		{
			if (_outer.ParseCore(t, _tpattern, null))
			{
				_subtrees.Add(t);
			}
		}
	}

	private class VisitTreeWizardContextVisitor : IContextVisitor
	{
		private TreeWizard _outer;

		private IContextVisitor _visitor;

		private IDictionary<string, object> _labels;

		private TreePattern _tpattern;

		public VisitTreeWizardContextVisitor(TreeWizard outer, IContextVisitor visitor, IDictionary<string, object> labels, TreePattern tpattern)
		{
			_outer = outer;
			_visitor = visitor;
			_labels = labels;
			_tpattern = tpattern;
		}

		public void Visit(object t, object parent, int childIndex, IDictionary<string, object> unusedlabels)
		{
			_labels.Clear();
			if (_outer.ParseCore(t, _tpattern, _labels))
			{
				_visitor.Visit(t, parent, childIndex, _labels);
			}
		}
	}

	protected ITreeAdaptor adaptor;

	protected IDictionary<string, int> tokenNameToTypeMap;

	public TreeWizard(ITreeAdaptor adaptor)
	{
		this.adaptor = adaptor;
	}

	public TreeWizard(ITreeAdaptor adaptor, IDictionary<string, int> tokenNameToTypeMap)
	{
		this.adaptor = adaptor;
		this.tokenNameToTypeMap = tokenNameToTypeMap;
	}

	public TreeWizard(ITreeAdaptor adaptor, string[] tokenNames)
	{
		this.adaptor = adaptor;
		tokenNameToTypeMap = ComputeTokenTypes(tokenNames);
	}

	public TreeWizard(string[] tokenNames)
		: this(new CommonTreeAdaptor(), tokenNames)
	{
	}

	public virtual IDictionary<string, int> ComputeTokenTypes(string[] tokenNames)
	{
		IDictionary<string, int> dictionary = new Dictionary<string, int>();
		if (tokenNames == null)
		{
			return dictionary;
		}
		for (int i = 4; i < tokenNames.Length; i++)
		{
			string key = tokenNames[i];
			dictionary[key] = i;
		}
		return dictionary;
	}

	public virtual int GetTokenType(string tokenName)
	{
		if (tokenNameToTypeMap == null)
		{
			return 0;
		}
		if (tokenNameToTypeMap.TryGetValue(tokenName, out var value))
		{
			return value;
		}
		return 0;
	}

	public IDictionary<int, IList> Index(object t)
	{
		IDictionary<int, IList> dictionary = new Dictionary<int, IList>();
		IndexCore(t, dictionary);
		return dictionary;
	}

	protected virtual void IndexCore(object t, IDictionary<int, IList> m)
	{
		if (t != null)
		{
			int type = adaptor.GetType(t);
			if (!m.TryGetValue(type, out var value) || value == null)
			{
				value = (m[type] = new List<object>());
			}
			value.Add(t);
			int childCount = adaptor.GetChildCount(t);
			for (int i = 0; i < childCount; i++)
			{
				object child = adaptor.GetChild(t, i);
				IndexCore(child, m);
			}
		}
	}

	public virtual IList Find(object t, int ttype)
	{
		IList list = new List<object>();
		Visit(t, ttype, new FindTreeWizardVisitor(list));
		return list;
	}

	public virtual IList Find(object t, string pattern)
	{
		IList list = new List<object>();
		TreePatternLexer tokenizer = new TreePatternLexer(pattern);
		TreePatternParser treePatternParser = new TreePatternParser(tokenizer, this, new TreePatternTreeAdaptor());
		TreePattern treePattern = (TreePattern)treePatternParser.Pattern();
		if (treePattern == null || treePattern.IsNil || (object)treePattern.GetType() == typeof(WildcardTreePattern))
		{
			return null;
		}
		int type = treePattern.Type;
		Visit(t, type, new FindTreeWizardContextVisitor(this, treePattern, list));
		return list;
	}

	public virtual object FindFirst(object t, int ttype)
	{
		return null;
	}

	public virtual object FindFirst(object t, string pattern)
	{
		return null;
	}

	public void Visit(object t, int ttype, IContextVisitor visitor)
	{
		VisitCore(t, null, 0, ttype, visitor);
	}

	public void Visit(object t, int ttype, Action<object> action)
	{
		Visit(t, ttype, new ActionVisitor(action));
	}

	protected virtual void VisitCore(object t, object parent, int childIndex, int ttype, IContextVisitor visitor)
	{
		if (t != null)
		{
			if (adaptor.GetType(t) == ttype)
			{
				visitor.Visit(t, parent, childIndex, null);
			}
			int childCount = adaptor.GetChildCount(t);
			for (int i = 0; i < childCount; i++)
			{
				object child = adaptor.GetChild(t, i);
				VisitCore(child, t, i, ttype, visitor);
			}
		}
	}

	public void Visit(object t, string pattern, IContextVisitor visitor)
	{
		TreePatternLexer tokenizer = new TreePatternLexer(pattern);
		TreePatternParser treePatternParser = new TreePatternParser(tokenizer, this, new TreePatternTreeAdaptor());
		TreePattern treePattern = (TreePattern)treePatternParser.Pattern();
		if (treePattern != null && !treePattern.IsNil && (object)treePattern.GetType() != typeof(WildcardTreePattern))
		{
			IDictionary<string, object> labels = new Dictionary<string, object>();
			int type = treePattern.Type;
			Visit(t, type, new VisitTreeWizardContextVisitor(this, visitor, labels, treePattern));
		}
	}

	public bool Parse(object t, string pattern, IDictionary<string, object> labels)
	{
		TreePatternLexer tokenizer = new TreePatternLexer(pattern);
		TreePatternParser treePatternParser = new TreePatternParser(tokenizer, this, new TreePatternTreeAdaptor());
		TreePattern tpattern = (TreePattern)treePatternParser.Pattern();
		return ParseCore(t, tpattern, labels);
	}

	public bool Parse(object t, string pattern)
	{
		return Parse(t, pattern, null);
	}

	protected virtual bool ParseCore(object t1, TreePattern tpattern, IDictionary<string, object> labels)
	{
		if (t1 == null || tpattern == null)
		{
			return false;
		}
		if ((object)tpattern.GetType() != typeof(WildcardTreePattern))
		{
			if (adaptor.GetType(t1) != tpattern.Type)
			{
				return false;
			}
			if (tpattern.hasTextArg && !adaptor.GetText(t1).Equals(tpattern.Text))
			{
				return false;
			}
		}
		if (tpattern.label != null && labels != null)
		{
			labels[tpattern.label] = t1;
		}
		int childCount = adaptor.GetChildCount(t1);
		int childCount2 = tpattern.ChildCount;
		if (childCount != childCount2)
		{
			return false;
		}
		for (int i = 0; i < childCount; i++)
		{
			object child = adaptor.GetChild(t1, i);
			TreePattern tpattern2 = (TreePattern)tpattern.GetChild(i);
			if (!ParseCore(child, tpattern2, labels))
			{
				return false;
			}
		}
		return true;
	}

	public virtual object Create(string pattern)
	{
		TreePatternLexer tokenizer = new TreePatternLexer(pattern);
		TreePatternParser treePatternParser = new TreePatternParser(tokenizer, this, adaptor);
		return treePatternParser.Pattern();
	}

	public static bool Equals(object t1, object t2, ITreeAdaptor adaptor)
	{
		return EqualsCore(t1, t2, adaptor);
	}

	public new bool Equals(object t1, object t2)
	{
		return EqualsCore(t1, t2, adaptor);
	}

	protected static bool EqualsCore(object t1, object t2, ITreeAdaptor adaptor)
	{
		if (t1 == null || t2 == null)
		{
			return false;
		}
		if (adaptor.GetType(t1) != adaptor.GetType(t2))
		{
			return false;
		}
		if (!adaptor.GetText(t1).Equals(adaptor.GetText(t2)))
		{
			return false;
		}
		int childCount = adaptor.GetChildCount(t1);
		int childCount2 = adaptor.GetChildCount(t2);
		if (childCount != childCount2)
		{
			return false;
		}
		for (int i = 0; i < childCount; i++)
		{
			object child = adaptor.GetChild(t1, i);
			object child2 = adaptor.GetChild(t2, i);
			if (!EqualsCore(child, child2, adaptor))
			{
				return false;
			}
		}
		return true;
	}
}
