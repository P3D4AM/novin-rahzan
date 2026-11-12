using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Antlr.Runtime.Tree;

public class BufferedTreeNodeStream : ITreeNodeStream, IIntStream, ITokenStreamInformation
{
	protected sealed class StreamIterator : IEnumerator<object>, IDisposable, IEnumerator
	{
		private BufferedTreeNodeStream _outer;

		private int _index;

		public object Current
		{
			get
			{
				if (_index < _outer.nodes.Count)
				{
					return _outer.nodes[_index];
				}
				return _outer.eof;
			}
		}

		public StreamIterator(BufferedTreeNodeStream outer)
		{
			_outer = outer;
			_index = -1;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			if (_index < _outer.nodes.Count)
			{
				_index++;
			}
			return _index < _outer.nodes.Count;
		}

		public void Reset()
		{
			_index = -1;
		}
	}

	public const int DEFAULT_INITIAL_BUFFER_SIZE = 100;

	public const int INITIAL_CALL_STACK_SIZE = 10;

	protected object down;

	protected object up;

	protected object eof;

	protected IList nodes;

	protected object root;

	protected ITokenStream tokens;

	private ITreeAdaptor adaptor;

	private bool uniqueNavigationNodes;

	protected int p = -1;

	protected int lastMarker;

	protected Stack<int> calls;

	public virtual int Count
	{
		get
		{
			if (p == -1)
			{
				throw new InvalidOperationException("Cannot determine the Count before the buffer is filled.");
			}
			return nodes.Count;
		}
	}

	public virtual object TreeSource => root;

	public virtual string SourceName => TokenStream.SourceName;

	public virtual ITokenStream TokenStream
	{
		get
		{
			return tokens;
		}
		set
		{
			tokens = value;
		}
	}

	public virtual ITreeAdaptor TreeAdaptor
	{
		get
		{
			return adaptor;
		}
		set
		{
			adaptor = value;
		}
	}

	public virtual bool UniqueNavigationNodes
	{
		get
		{
			return uniqueNavigationNodes;
		}
		set
		{
			uniqueNavigationNodes = value;
		}
	}

	public virtual IToken LastToken => TreeAdaptor.GetToken(LB(1));

	public virtual IToken LastRealToken
	{
		get
		{
			int num = 0;
			IToken token;
			do
			{
				num++;
				token = TreeAdaptor.GetToken(LB(num));
			}
			while (token != null && token.Line <= 0);
			return token;
		}
	}

	public virtual int MaxLookBehind => int.MaxValue;

	public virtual object this[int i]
	{
		get
		{
			if (p == -1)
			{
				throw new InvalidOperationException("Cannot get the node at index i before the buffer is filled.");
			}
			return nodes[i];
		}
	}

	public virtual int Index => p;

	public BufferedTreeNodeStream(object tree)
		: this(new CommonTreeAdaptor(), tree)
	{
	}

	public BufferedTreeNodeStream(ITreeAdaptor adaptor, object tree)
		: this(adaptor, tree, 100)
	{
	}

	public BufferedTreeNodeStream(ITreeAdaptor adaptor, object tree, int initialBufferSize)
	{
		root = tree;
		this.adaptor = adaptor;
		nodes = new List<object>(initialBufferSize);
		down = adaptor.Create(2, "DOWN");
		up = adaptor.Create(3, "UP");
		eof = adaptor.Create(-1, "EOF");
	}

	protected virtual void FillBuffer()
	{
		FillBuffer(root);
		p = 0;
	}

	public virtual void FillBuffer(object t)
	{
		bool flag = adaptor.IsNil(t);
		if (!flag)
		{
			nodes.Add(t);
		}
		int childCount = adaptor.GetChildCount(t);
		if (!flag && childCount > 0)
		{
			AddNavigationNode(2);
		}
		for (int i = 0; i < childCount; i++)
		{
			object child = adaptor.GetChild(t, i);
			FillBuffer(child);
		}
		if (!flag && childCount > 0)
		{
			AddNavigationNode(3);
		}
	}

	protected virtual int GetNodeIndex(object node)
	{
		if (p == -1)
		{
			FillBuffer();
		}
		for (int i = 0; i < nodes.Count; i++)
		{
			object obj = nodes[i];
			if (obj == node)
			{
				return i;
			}
		}
		return -1;
	}

	protected virtual void AddNavigationNode(int ttype)
	{
		object obj = null;
		obj = ((ttype == 2) ? ((!UniqueNavigationNodes) ? down : adaptor.Create(2, "DOWN")) : ((!UniqueNavigationNodes) ? up : adaptor.Create(3, "UP")));
		nodes.Add(obj);
	}

	public virtual object LT(int k)
	{
		if (p == -1)
		{
			FillBuffer();
		}
		if (k == 0)
		{
			return null;
		}
		if (k < 0)
		{
			return LB(-k);
		}
		if (p + k - 1 >= nodes.Count)
		{
			return eof;
		}
		return nodes[p + k - 1];
	}

	public virtual object GetCurrentSymbol()
	{
		return LT(1);
	}

	protected virtual object LB(int k)
	{
		if (k == 0)
		{
			return null;
		}
		if (p - k < 0)
		{
			return null;
		}
		return nodes[p - k];
	}

	public virtual void Consume()
	{
		if (p == -1)
		{
			FillBuffer();
		}
		p++;
	}

	public virtual int LA(int i)
	{
		return adaptor.GetType(LT(i));
	}

	public virtual int Mark()
	{
		if (p == -1)
		{
			FillBuffer();
		}
		lastMarker = Index;
		return lastMarker;
	}

	public virtual void Release(int marker)
	{
	}

	public virtual void Rewind(int marker)
	{
		Seek(marker);
	}

	public virtual void Rewind()
	{
		Seek(lastMarker);
	}

	public virtual void Seek(int index)
	{
		if (p == -1)
		{
			FillBuffer();
		}
		p = index;
	}

	public virtual void Push(int index)
	{
		if (calls == null)
		{
			calls = new Stack<int>();
		}
		calls.Push(p);
		Seek(index);
	}

	public virtual int Pop()
	{
		int num = calls.Pop();
		Seek(num);
		return num;
	}

	public virtual void Reset()
	{
		p = 0;
		lastMarker = 0;
		if (calls != null)
		{
			calls.Clear();
		}
	}

	public virtual IEnumerator<object> Iterator()
	{
		if (p == -1)
		{
			FillBuffer();
		}
		return new StreamIterator(this);
	}

	public virtual void ReplaceChildren(object parent, int startChildIndex, int stopChildIndex, object t)
	{
		if (parent != null)
		{
			adaptor.ReplaceChildren(parent, startChildIndex, stopChildIndex, t);
		}
	}

	public virtual string ToTokenTypeString()
	{
		if (p == -1)
		{
			FillBuffer();
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < nodes.Count; i++)
		{
			object t = nodes[i];
			stringBuilder.Append(" ");
			stringBuilder.Append(adaptor.GetType(t));
		}
		return stringBuilder.ToString();
	}

	public virtual string ToTokenString(int start, int stop)
	{
		if (p == -1)
		{
			FillBuffer();
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = start; i < nodes.Count && i <= stop; i++)
		{
			object t = nodes[i];
			stringBuilder.Append(" ");
			stringBuilder.Append(adaptor.GetToken(t));
		}
		return stringBuilder.ToString();
	}

	public virtual string ToString(object start, object stop)
	{
		Console.Out.WriteLine("toString");
		if (start == null || stop == null)
		{
			return null;
		}
		if (p == -1)
		{
			throw new InvalidOperationException("Buffer is not yet filled.");
		}
		if (start is CommonTree)
		{
			Console.Out.Write(string.Concat("toString: ", ((CommonTree)start).Token, ", "));
		}
		else
		{
			Console.Out.WriteLine(start);
		}
		if (stop is CommonTree)
		{
			Console.Out.WriteLine(((CommonTree)stop).Token);
		}
		else
		{
			Console.Out.WriteLine(stop);
		}
		if (tokens != null)
		{
			int tokenStartIndex = adaptor.GetTokenStartIndex(start);
			int num = adaptor.GetTokenStopIndex(stop);
			if (adaptor.GetType(stop) == 3)
			{
				num = adaptor.GetTokenStopIndex(start);
			}
			else if (adaptor.GetType(stop) == -1)
			{
				num = Count - 2;
			}
			return tokens.ToString(tokenStartIndex, num);
		}
		object obj = null;
		int i;
		for (i = 0; i < nodes.Count; i++)
		{
			obj = nodes[i];
			if (obj == start)
			{
				break;
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (obj = nodes[i]; obj != stop; obj = nodes[i])
		{
			string text = adaptor.GetText(obj);
			if (text == null)
			{
				text = " " + adaptor.GetType(obj);
			}
			stringBuilder.Append(text);
			i++;
		}
		string text2 = adaptor.GetText(stop);
		if (text2 == null)
		{
			text2 = " " + adaptor.GetType(stop);
		}
		stringBuilder.Append(text2);
		return stringBuilder.ToString();
	}
}
