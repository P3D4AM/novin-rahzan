using System;
using System.Collections.Generic;
using System.Text;

namespace Antlr.Runtime;

[Serializable]
public class BufferedTokenStream : ITokenStream, IIntStream, ITokenStreamInformation
{
	private ITokenSource _tokenSource;

	[CLSCompliant(false)]
	protected List<IToken> _tokens = new List<IToken>(100);

	private int _lastMarker;

	[CLSCompliant(false)]
	protected int _p = -1;

	public virtual ITokenSource TokenSource
	{
		get
		{
			return _tokenSource;
		}
		set
		{
			_tokenSource = value;
			_tokens.Clear();
			_p = -1;
		}
	}

	public virtual int Index => _p;

	public virtual int Range { get; protected set; }

	public virtual int Count => _tokens.Count;

	public virtual string SourceName => _tokenSource.SourceName;

	public virtual IToken LastToken => LB(1);

	public virtual IToken LastRealToken
	{
		get
		{
			int num = 0;
			IToken token;
			do
			{
				num++;
				token = LB(num);
			}
			while (token != null && token.Line <= 0);
			return token;
		}
	}

	public virtual int MaxLookBehind => int.MaxValue;

	public BufferedTokenStream()
	{
	}

	public BufferedTokenStream(ITokenSource tokenSource)
	{
		_tokenSource = tokenSource;
	}

	public virtual int Mark()
	{
		if (_p == -1)
		{
			Setup();
		}
		_lastMarker = Index;
		return _lastMarker;
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
		Seek(_lastMarker);
	}

	public virtual void Reset()
	{
		_p = 0;
		_lastMarker = 0;
	}

	public virtual void Seek(int index)
	{
		_p = index;
	}

	public virtual void Consume()
	{
		if (_p == -1)
		{
			Setup();
		}
		_p++;
		Sync(_p);
	}

	protected virtual void Sync(int i)
	{
		int num = i - _tokens.Count + 1;
		if (num > 0)
		{
			Fetch(num);
		}
	}

	protected virtual void Fetch(int n)
	{
		for (int i = 0; i < n; i++)
		{
			IToken token = TokenSource.NextToken();
			token.TokenIndex = _tokens.Count;
			_tokens.Add(token);
			if (token.Type == -1)
			{
				break;
			}
		}
	}

	public virtual IToken Get(int i)
	{
		if (i < 0 || i >= _tokens.Count)
		{
			throw new IndexOutOfRangeException("token index " + i + " out of range 0.." + (_tokens.Count - 1));
		}
		return _tokens[i];
	}

	public virtual int LA(int i)
	{
		return LT(i).Type;
	}

	protected virtual IToken LB(int k)
	{
		if (_p - k < 0)
		{
			return null;
		}
		return _tokens[_p - k];
	}

	public virtual IToken LT(int k)
	{
		if (_p == -1)
		{
			Setup();
		}
		if (k == 0)
		{
			return null;
		}
		if (k < 0)
		{
			return LB(-k);
		}
		int num = _p + k - 1;
		Sync(num);
		if (num >= _tokens.Count)
		{
			return _tokens[_tokens.Count - 1];
		}
		if (num > Range)
		{
			Range = num;
		}
		return _tokens[_p + k - 1];
	}

	protected virtual void Setup()
	{
		Sync(0);
		_p = 0;
	}

	public virtual List<IToken> GetTokens()
	{
		return _tokens;
	}

	public virtual List<IToken> GetTokens(int start, int stop)
	{
		return GetTokens(start, stop, (BitSet)null);
	}

	public virtual List<IToken> GetTokens(int start, int stop, BitSet types)
	{
		if (_p == -1)
		{
			Setup();
		}
		if (stop >= _tokens.Count)
		{
			stop = _tokens.Count - 1;
		}
		if (start < 0)
		{
			start = 0;
		}
		if (start > stop)
		{
			return null;
		}
		List<IToken> list = new List<IToken>();
		for (int i = start; i <= stop; i++)
		{
			IToken token = _tokens[i];
			if (types == null || types.Member(token.Type))
			{
				list.Add(token);
			}
		}
		if (list.Count == 0)
		{
			list = null;
		}
		return list;
	}

	public virtual List<IToken> GetTokens(int start, int stop, IEnumerable<int> types)
	{
		return GetTokens(start, stop, new BitSet(types));
	}

	public virtual List<IToken> GetTokens(int start, int stop, int ttype)
	{
		return GetTokens(start, stop, BitSet.Of(ttype));
	}

	public override string ToString()
	{
		if (_p == -1)
		{
			Setup();
		}
		Fill();
		return ToString(0, _tokens.Count - 1);
	}

	public virtual string ToString(int start, int stop)
	{
		if (start < 0 || stop < 0)
		{
			return null;
		}
		if (_p == -1)
		{
			Setup();
		}
		if (stop >= _tokens.Count)
		{
			stop = _tokens.Count - 1;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = start; i <= stop; i++)
		{
			IToken token = _tokens[i];
			if (token.Type == -1)
			{
				break;
			}
			stringBuilder.Append(token.Text);
		}
		return stringBuilder.ToString();
	}

	public virtual string ToString(IToken start, IToken stop)
	{
		if (start != null && stop != null)
		{
			return ToString(start.TokenIndex, stop.TokenIndex);
		}
		return null;
	}

	public virtual void Fill()
	{
		if (_p == -1)
		{
			Setup();
		}
		if (_tokens[_p].Type != -1)
		{
			int num = _p + 1;
			Sync(num);
			while (_tokens[num].Type != -1)
			{
				num++;
				Sync(num);
			}
		}
	}
}
